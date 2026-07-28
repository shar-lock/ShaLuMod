using MegaCrit.Sts2.Core.Commands;                 // PowerCmd（失血时给自己叠层）
using MegaCrit.Sts2.Core.Entities.Cards;            // CardModel / CardPlay
using MegaCrit.Sts2.Core.Entities.Creatures;        // Creature
using MegaCrit.Sts2.Core.Entities.Powers;           // PowerType / PowerStackType
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext / DamageResult
using MegaCrit.Sts2.Core.Models;                    // PowerModel（M2.3 钩子参数用）
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp

namespace WandiMod.WandiModCode.Powers;

/// <summary>
/// 血仇（Vengeance）—— 万敌的核心计数型 Power（Buff + Counter）。
/// 机制：
///   1. 失血叠层：每当万敌失去生命（自伤或受击）→ +1 层（钩子 AfterDamageReceived，参考原生 RupturePower）。
///   2. 伤害放大：每层血仇使万敌的攻击牌伤害 +2%（钩子 ModifyDamageMultiplicative，参考原生 WeakPower）。
/// 由起手遗物「弑亲血脉」在战斗开始时赋予（BeforeCombatStart）。
/// （M2.3 将在此 Power 上加「血仇 ≥ 7 → 消耗 7 层、扣 5% 当前血、生成荡平万邦」的触发。）
/// </summary>
public class VengeancePower : WandiModPower
{
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
}
