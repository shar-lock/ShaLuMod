using MegaCrit.Sts2.Core.Commands;                 // PowerCmd（失血时给自己叠层）
using MegaCrit.Sts2.Core.Entities.Cards;            // CardModel / CardPlay
using MegaCrit.Sts2.Core.Entities.Creatures;        // Creature
using MegaCrit.Sts2.Core.Entities.Powers;           // PowerType / PowerStackType
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext / DamageResult
using MegaCrit.Sts2.Core.Models;                    // PowerModel（M2.3 钩子参数用）
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using WandiMod.WandiModCode.Cards;                  // ConquerAllLands（8 层触发生成荡平万邦）
using WandiMod.WandiModCode.Relics;                 // UndyingRoyalBlood（觉醒判定）

namespace WandiMod.WandiModCode.Powers;

/// <summary>
/// 血仇（Vengeance）—— 万敌的核心计数型 Power（Buff + Counter）。
/// 机制：
///   1. 失血叠层：每当万敌失去生命（自伤或受击）→ +1 层（钩子 AfterDamageReceived，参考原生 RupturePower）。
///   2. 伤害放大：每层血仇使万敌的攻击牌伤害 +2%（钩子 ModifyDamageMultiplicative，参考原生 WeakPower）。
/// 由起手遗物「弑亲血脉」在战斗开始时赋予（BeforeCombatStart）。
///   3. 满 8 触发：血仇 ≥ 8 时消耗至保底 1 层、扣 5% 当前血、生成「荡平万邦」到手牌（钩子 AfterPowerAmountChanged）。
///      保底 1 层的原因：层数归零会被游戏自动移除能力（PowerModel.ShouldRemoveDueToAmount），
///      能力移除后「掉血叠层」钩子随之失效，血仇将永远无法再叠加；保底 1 层绕开该机制，稳态仍是每 7 次失血触发一轮。
/// </summary>
public class VengeancePower : WandiModPower
{
    /// <summary>触发阈值：血仇达到 8 层时触发荡平万邦。</summary>
    private const int TriggerThreshold = 8;
    /// <summary>触发后保底保留层数（不可为 0，见类注释）。</summary>
    private const int FloorAfterTrigger = 1;

    // 增益型、计数叠加型 Power
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

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

        // 每层 +2%，例如 5 层 → ×1.10
        return 1m + 0.02m * Amount;
    }

    /// <summary>
    /// 满 8 触发荡平万邦：血仇 ≥ 8 时消耗至保底 1 层、扣当前血 5%（至少 1）、生成「荡平万邦」到手牌。
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
        // 未达 8 层不触发
        if (Amount < TriggerThreshold)
            return;

        // ① 消耗至保底 1 层（PowerCmd.Apply 负值；Counter 型累减）。
        //    绝不能消耗到 0：层数 ≤0 时游戏自动移除本能力（ShouldRemoveDueToAmount），
        //    能力移除后 AfterDamageReceived 掉血叠层钩子随之失效，血仇将永远无法再叠加。
        int consume = Amount - FloorAfterTrigger;
        await PowerCmd.Apply<VengeancePower>(choiceContext, Owner, -consume, Owner, null);

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
            MainFile.Logger.Error("[血仇] ≥8 触发时 CombatState/Owner.Player 为空，荡平万邦未生成（层数已消耗）");
            return;
        }
        CardModel card = CombatState.CreateCard<ConquerAllLands>(Owner.Player);
        // 觉醒遗物「不灭王血」在场 → 生成升级版（原版 Jackpot/ManifestAuthority 写法：CardCmd.Upgrade 就地升级可变实例）
        bool awakened = Owner.Player.GetRelic<UndyingRoyalBlood>() != null;
        if (awakened)
            CardCmd.Upgrade(card);
        await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner.Player);

        MainFile.Logger.Info($"[血仇] ≥8 触发荡平万邦：消耗 {consume} 层（保底 {FloorAfterTrigger} 层），扣血 {loss}，生成荡平万邦到手牌（觉醒={awakened}）");
    }
}
