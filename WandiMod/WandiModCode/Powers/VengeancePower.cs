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
