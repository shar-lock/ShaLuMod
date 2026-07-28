using MegaCrit.Sts2.Core.Commands;                 // CreatureCmd / PowerCmd
using MegaCrit.Sts2.Core.Entities.Creatures;        // Creature
using MegaCrit.Sts2.Core.Entities.Powers;           // PowerType / PowerStackType
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Models;                    // CardModel
using MegaCrit.Sts2.Core.Rooms;                     // CombatRoom

namespace WandiMod.WandiModCode.Powers;

/// <summary>
/// 纷争（Strife）—— 万敌的临时生命上限（Buff + Counter），层数 = 当前临时生命上限总量。
/// 机制：
///   1. 获得 X 层纷争 = 最大生命 +X 并回复 X（抬上限 + 等量回血，语义同 GainMaxHp）。
///   2. 积累上限：纷争层数 ≤ 基础最大生命（= 当前上限 − 已有纷争），即有效上限最高翻倍。
///      基础值动态计算，战斗中发生的永久提升（如涅槃重生）会自动并入并抬高上限。
///   3. 战斗结束（AfterCombatEnd）：最大生命仅扣减纷争累计值（永久提升保留），当前血超出则截断。
/// 注意：
///   - 抬/扣上限走 SetMaxHp 而非 GainMaxHp —— GainMaxHp 会把数值计入地图历史 MaxHpGained
///     永久统计，设计上临时与永久必须严格分通道。
///   - 获得纷争的唯一入口是静态方法 Grant（由卡牌调用）；本 Power 只负责「层数展示 + 结束还原」。
///   - 回血经 CreatureCmd.Heal，会触发 AfterCurrentHpChanged 钩子（回血联动件的正常通道）。
/// </summary>
public class StrifePower : WandiModPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    /// <summary>
    /// 授予纷争（唯一入口）：抬临时上限 + 等量回血 + 叠层，带 100% 积累上限 clamp。
    /// 执行顺序有讲究：必须先 SetMaxHp 抬上限、再 Heal 回血——Heal 内部按「当前上限 − 当前血」封顶，
    /// 先抬上限后恰好能回满 gain，顺序反了会被旧上限截断。
    /// </summary>
    /// <param name="choiceContext">选择上下文（卡牌 OnPlay 透传）。</param>
    /// <param name="target">获得纷争的生物（一般 = 万敌本人）。</param>
    /// <param name="amount">请求的纷争点数。</param>
    /// <param name="applier">施加者（默认 = target 自己）。</param>
    /// <param name="cardSource">来源牌（卡牌调用时传 this，用于战斗记录）。</param>
    public static async Task Grant(
        PlayerChoiceContext choiceContext,
        Creature target,
        decimal amount,
        Creature? applier = null,
        CardModel? cardSource = null)
    {
        // 防御性检查：target 为空属于调用方 bug，记错误日志并直接返回（不抛异常，避免打断战斗流程）
        if (target == null)
        {
            MainFile.Logger.Error($"[纷争] Grant 收到空 target（amount={amount}, cardSource={cardSource?.Id.Entry ?? "null"}），已忽略");
            return;
        }

        // 当前已积累的纷争层数（无 Power 时为 0）
        int current = target.GetPower<StrifePower>()?.Amount ?? 0;

        // 积累上限 clamp：基础最大生命 = 当前上限 − 已有纷争；距「翻倍」的余量 = 基础值 − 已有纷争。
        // 用动态算法而非记录入场值——战斗中若发生永久提升（上限+N），基础值自动 +N，余量随之放宽。
        int headroom = Math.Max(0, target.MaxHp - current - current);
        int gain = (int)Math.Min(amount, headroom);

        if (gain <= 0)
        {
            // 打满上限是正常情况（玩家连打防御牌），Warn 级记录便于平衡调试
            MainFile.Logger.Warn($"[纷争] 已达积累上限（当前 {current}/基础 {target.MaxHp - current}），请求 {amount} 点被完全拦截");
            return;
        }
        if (gain < amount)
        {
            // 部分被 cap 拦截也值得记录——说明数值接近上限，平衡调参时关注
            MainFile.Logger.Info($"[纷争] 积累上限部分生效：请求 {amount} 点，实际授予 {gain} 点（当前 {current} → {current + gain}）");
        }

        // ① 抬临时上限：SetMaxHp 返回实际上限变化量；不动当前血、不计入 MaxHpGained 永久统计
        await CreatureCmd.SetMaxHp(target, target.MaxHp + gain);
        // ② 等量回血：Heal 内部按新上限封顶，恰好回满 gain；会触发 AfterCurrentHpChanged 钩子
        await CreatureCmd.Heal(target, gain);
        // ③ 叠层展示：Counter 型重复 Apply 走 ModifyAmount 累加（不会重复触发前两步——它们只在这里做）
        await PowerCmd.Apply<StrifePower>(choiceContext, target, gain, applier ?? target, cardSource);
    }

    /// <summary>
    /// 战斗结束：清空纷争 —— 最大生命只扣纷争累计值（战斗中若有永久提升自然保留），
    /// 当前生命超出还原后上限则截断。Power 实例随战斗状态销毁，无需手动移除。
    /// </summary>
    public override async Task AfterCombatEnd(CombatRoom room)
    {
        // Owner 为 null 理论上不会发生（Power 必有宿主），一旦发生说明生命周期异常，记错误日志
        if (Owner == null)
        {
            MainFile.Logger.Error("[纷争] AfterCombatEnd 时 Owner 为空，无法还原最大生命");
            return;
        }

        int oldMax = Owner.MaxHp;
        // 还原上限：只扣本 Power 累计的临时量。Amount 即临时上限总量（1 层 = 1 点）。
        await CreatureCmd.SetMaxHp(Owner, Owner.MaxHp - Amount);
        MainFile.Logger.Info($"[纷争] 战斗结束清空纷争：层数 {Amount}，最大生命 {oldMax} → {Owner.MaxHp}");

        // 截断当前血：抬上限时回的血可能让当前血超过还原后的上限
        if (Owner.CurrentHp > Owner.MaxHp)
        {
            MainFile.Logger.Info($"[纷争] 当前生命 {Owner.CurrentHp} 超出还原后上限 {Owner.MaxHp}，截断");
            await CreatureCmd.SetCurrentHp(Owner, Owner.MaxHp);
        }
    }
}
