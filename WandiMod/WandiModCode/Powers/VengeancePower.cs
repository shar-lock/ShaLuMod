using HarmonyLib;                                   // Traverse / HarmonyPatch（显示补丁用）
using MegaCrit.Sts2.Core.Commands;                 // PowerCmd（失血时给自己叠层）
using MegaCrit.Sts2.Core.Entities.Cards;            // CardModel / CardPlay
using MegaCrit.Sts2.Core.Entities.Creatures;        // Creature
using MegaCrit.Sts2.Core.Entities.Powers;           // PowerType / PowerStackType
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext / DamageResult
using MegaCrit.Sts2.Core.HoverTips;                 // IHoverTip / HoverTipFactory（悬停预览荡平万邦）
using MegaCrit.Sts2.Core.Models;                    // PowerModel（M2.3 钩子参数用）
using MegaCrit.Sts2.Core.Nodes.Combat;              // NPower（显示补丁目标）
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using MegaCrit.Sts2.addons.mega_text;               // MegaLabel（清空数字标签）
using WandiMod.WandiModCode.Cards;                  // ConquerAllLands（8 层触发生成荡平万邦）
using WandiMod.WandiModCode.Relics;                 // UndyingRoyalBlood（觉醒判定）

namespace WandiMod.WandiModCode.Powers;

/// <summary>
/// 血仇（Vengeance）—— 万敌的核心计数型 Power（Buff + Counter）。
/// 机制：
///   1. 失血叠层：每当万敌失去生命（自伤或受击）→ +1 层（钩子 AfterDamageReceived，参考原生 RupturePower）。
///   2. 伤害放大：每层【有效】血仇使万敌的攻击牌伤害 +2%（钩子 ModifyDamageMultiplicative，参考原生 WeakPower）。
///      「有效层数 = 真实层数 - 1」——保底那 1 层是结构性储备（维持 Power 存活），不提供任何增伤。
///      由起手遗物「弑亲血脉」在战斗开始时赋予（BeforeCombatStart，初始 1 层 = 0 有效层）。
///   3. 成长型触发：血仇累积到「上次基准 + 7」时消耗 4 层、扣 5% 当前血、生成「荡平万邦」到手牌（钩子 AfterPowerAmountChanged）。
///      消耗 4 &lt; 累积 7 → 净留 +3，触发基准逐次抬高（触发点 8→11→14...，消耗后 4→7→10...），血仇**逐轮成长**而非重置——
///      给「血仇叠层流」提供真正的局内数值成长（战斗越久、血仇越高、增伤越强）。
///      不会归零：触发留 ≥4 层；消耗卡（复仇心/涅槃）由 IsPlayable 门控保底 ≥1 层。故 Amount 始终 ≥1，
///      不会触发 PowerModel.ShouldRemoveDueToAmount 自动移除（能力移除会让掉血叠层钩子失效，血仇永远无法再叠加）。
///
/// 玩家可见模型（与真实 Amount 的换算）：
///   - 状态栏显示数字 = 真实层数 - 1（override DisplayAmount）。
///   - 1 层（保底）→ 显示为空（不展示数字），且 0% 增伤——「1 层血仇不触发任何效果」。
///   - 8 层触发荡平万邦时，玩家看到的是「7 层」触发（显示值=真实值-1）。
///   - 显示为空需 Harmony 补丁（VengeancePowerDisplayPatch）：原生 Counter 型 Power 在 DisplayAmount=0 时会显示 "0"。
/// </summary>
public class VengeancePower : WandiModPower
{
    /// <summary>每次触发需新累积的层数（基准线 +7 才触发）。</summary>
    private const int GainPerTrigger = 7;
    /// <summary>每次触发固定消耗的层数（不清空；消耗 4 &lt; 累积 7 → 净留 +3，血仇逐轮成长）。</summary>
    private const int ConsumeOnTrigger = 4;

    /// <summary>
    /// 成长型触发基准线：上次触发「消耗后」的血仇值。下次触发条件 = 基准 + GainPerTrigger。
    /// 初始 1（= 开局血仇）→ 首次触发在 8 层；每次触发后基准抬高 → 触发点 8→11→14...，血仇净成长 +3/轮。
    /// 战斗实例字段（每战 Power 重建即重置为 1）；AfterPowerAmountChanged 只在层数变化时触发、存档恢复不触发，重载不会误触发。
    /// </summary>
    private int _lastTriggerBase = 1;

    // 增益型、计数叠加型 Power
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    /// <summary>
    /// 状态栏显示数字 = 真实层数 - 1。保底 1 层（真实）对应显示值 0（= 不展示数字，见 VengeancePowerDisplayPatch）。
    /// 例：真实 1→显示 0、真实 2→显示 1、真实 8→显示 7（触发荡平万邦时玩家看到的「7 层」）。
    /// </summary>
    public override int DisplayAmount => Math.Max(0, Amount - 1);

    /// <summary>
    /// 悬停提示：荡平万邦卡牌预览——血仇描述里提到触发产物，玩家悬停血仇图标即可实时查看荡平万邦效果。
    /// </summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromCard<ConquerAllLands>(),
    ];

    /// <summary>
    /// 授予血仇（卡牌调用的统一入口）：叠 Counter 层数。
    /// 注意：「失血自动 +1」走 AfterDamageReceived 钩子（下方），不走这里——
    /// 这里只服务卡面写的「获得 X 血仇」效果，两条通道并存叠加。
    /// </summary>
    /// <param name="choiceContext">选择上下文（卡牌 OnPlay 透传）。</param>
    /// <param name="target">获得血仇的生物（一般 = 万敌本人；为 null 记错误日志并忽略）。</param>
    /// <param name="amount">层数（≤0 记警告并忽略，正常不会发生）。</param>
    /// <param name="cardSource">来源牌（传 this，用于战斗记录）。</param>
    public static async Task Grant(PlayerChoiceContext choiceContext, Creature? target, decimal amount, CardModel? cardSource = null)
    {
        if (target == null)
        {
            MainFile.Logger.Error($"[血仇] Grant 收到空 target（amount={amount}, cardSource={cardSource?.Id.Entry ?? "null"}），已忽略");
            return;
        }
        if (amount <= 0)
        {
            MainFile.Logger.Warn($"[血仇] Grant 收到非正层数 {amount}（cardSource={cardSource?.Id.Entry ?? "null"}），已忽略");
            return;
        }
        await PowerCmd.Apply<VengeancePower>(choiceContext, target, amount, target, cardSource);
    }

    /// <summary>
    /// 失血叠层：万敌每次实际掉血（未格挡伤害 > 0）时，给自己 +1 血仇。
    /// 参考 RupturePower.AfterDamageReceived。注意：每次掉血事件 +1，不是每点生命 +1。
    /// </summary>
    public override async Task AfterDamageReceived(
        PlayerChoiceContext choiceContext,
        Creature target,
        DamageResult result,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        // 仅当受击者是万敌本人，且确实掉了血（未格挡伤害 > 0）才叠层
        if (target != Owner || result.UnblockedDamage <= 0)
            return;

        // 给自己叠 1 层血仇（Counter 型会累加；施放者=自己）
        await PowerCmd.Apply<VengeancePower>(choiceContext, Owner, 1, Owner, null);
    }

    /// <summary>
    /// 伤害放大器：每层血仇使万敌打出的攻击牌伤害 +2%（乘法）。
    /// 参考 WeakPower.ModifyDamageMultiplicative（虚弱返回 0.75 缩放；这里返回 1 + 2%×屽数 放大）。
    /// 仅对「万敌本人打出的、受力量加成的攻击伤害」生效。
    /// </summary>
    public override decimal ModifyDamageMultiplicative(
        Creature? target,
        decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource,
        CardPlay? cardPlay)
    {
        // 非万敌自己打出的、或非攻击伤害，不放大（返回 1 = 不变）
        if (dealer != Owner || !props.IsPoweredAttack())
            return 1m;

        // 有效层数（真实-1）× 2%：真实 2 层（有效1）→×1.02、真实 8 层（有效7）→×1.14；保底 1 层=0 有效=无增伤
        int effective = Math.Max(0, Amount - 1);
        return 1m + 0.02m * effective;
    }

    /// <summary>
    /// 成长型触发荡平万邦：血仇累积到「上次基准 + 7」时消耗 4 层（不清空）、扣当前血 5%（至少 1）、生成「荡平万邦」到手牌。
    /// 触发基准随每次触发抬高（8→11→14...），血仇净成长 +3/轮——给血仇叠层流提供局内数值成长。
    /// 钩子 AfterPowerAmountChanged 在任意 Power 层数变化时分发到所有监听者，故需过滤「本 Power + 正向变化」。
    /// 参考 OutbreakPower.AfterPowerAmountChanged。
    /// </summary>
    public override async Task AfterPowerAmountChanged(
        PlayerChoiceContext choiceContext,
        PowerModel power,
        decimal amount,
        Creature? applier,
        CardModel? cardSource)
    {
        // 只关心本血仇 Power 的正向变化：消耗层（amount < 0）会被过滤，避免「消耗→触发→消耗」递归
        if (power != this || amount <= 0)
            return;
        // 成长型触发：累积到「上次基准 + 7」才触发。基准随每次触发抬高 → 触发点 8→11→14...，血仇逐轮成长。
        if (Amount < _lastTriggerBase + GainPerTrigger)
            return;

        // ① 固定消耗 4 层（PowerCmd.Apply 负值；Counter 型累减）。不清空——保留剩余层让血仇累积成长。
        //    触发时 Amount ≥ 基准+7，消耗 4 后仍 ≥ 基准+3，远离 0；不会触发 ShouldRemoveDueToAmount 自动移除。
        int consume = ConsumeOnTrigger;
        await PowerCmd.Apply<VengeancePower>(choiceContext, Owner, -consume, Owner, null);
        // 更新触发基准为「消耗后的当前值」：下次需再累积 7 层（8→4→基准4→下次11；11→7→基准7→下次14...）。
        _lastTriggerBase = Amount;

        // ② 扣当前血 5%（至少 1）。Unblockable|Unpowered → 全额计入失血、不吃力量；
        //    会触发上面的 AfterDamageReceived 再 +1 层（设计内的副反馈，不会无限循环：消耗后远低于阈值）
        decimal loss = Math.Max(1m, Owner.CurrentHp * 0.05m);
        await CreatureCmd.Damage(choiceContext, Owner, loss,
            ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, null, null);

        // ③ 生成荡平万邦到手牌：必须用 CombatState.CreateCard 创建战斗内可变实例——
        //    ModelDb.Card<T>() 是规范（不可变）实例，直接进牌堆会在 AddGeneratedCardToCombat
        //    访问 card.Owner 时触发 AssertMutable → CanonicalModelException（参考原版 Turbo 造 Void 写法）。
        if (CombatState == null || Owner.Player == null)
        {
            MainFile.Logger.Error("[血仇] 成长触发时 CombatState/Owner.Player 为空，荡平万邦未生成（层数已消耗）");
            return;
        }
        CardModel card = CombatState.CreateCard<ConquerAllLands>(Owner.Player);
        // 觉醒遗物「不灭王血」在场 → 生成升级版（原版 Jackpot/ManifestAuthority 写法：CardCmd.Upgrade 就地升级可变实例）
        bool awakened = Owner.Player.GetRelic<UndyingRoyalBlood>() != null;
        if (awakened)
            CardCmd.Upgrade(card);
        await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner.Player);

        MainFile.Logger.Info($"[血仇] 成长触发荡平万邦：消耗 {consume} 层，当前 {Amount} 层，下次触发基准 {_lastTriggerBase}（需累积到 {_lastTriggerBase + GainPerTrigger}），扣血 {loss}，生成荡平万邦（觉醒={awakened}）");
    }
}

/// <summary>
/// Harmony 补丁：血仇 Power 显示值 ≤ 0（即真实层数 = 1 保底）时，隐藏状态栏左下角的数字。
/// 原生 NPower.RefreshAmount 对 Counter 型 Power 写死显示 DisplayAmount.ToString()——
/// DisplayAmount = 0 时会显示 "0"。此处 Postfix 在 RefreshAmount 之后把血仇的标签清空，
/// 实现「1 层不展示任何数字」。参考 BloodburnEmblemHealPatch（同款 Harmony Postfix 范式）。
/// </summary>
[HarmonyPatch(typeof(NPower), "RefreshAmount")]
static class VengeancePowerDisplayPatch
{
    [HarmonyPostfix]
    static void Postfix(NPower __instance)
    {
        var model = __instance.Model;
        // 仅作用于血仇：显示值（真实层数 - 1）> 0 时照常显示数字，≤ 0 时清空
        if (model is not VengeancePower || model.DisplayAmount > 0)
            return;

        // _amountLabel 是 NPower 的私有字段（MegaLabel），用 Traverse 取出后清空文字
        var label = Traverse.Create(__instance).Field("_amountLabel").GetValue<MegaLabel>();
        label?.SetTextAutoSize("");
    }
}
