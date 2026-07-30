using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Combat;

namespace WandiMod.WandiModCode.Powers;

/// <summary>
/// 弑神登神 Power——万敌终极形态。致敬星铁「弑神状态」。
///   1. 回合开始+2 血仇（AfterSideTurnStart → VengeancePower.Grant）。
///   2. 攻击牌额外造成等同血仇层数的固定伤害（ModifyDamageAdditive，参考原生 StrengthPower）。
///   3. 攻击造成伤害时回复 2% 最大生命（AfterDamageReceived：dealer==Owner → Heal）。
/// </summary>
public class GodslayerAscensionPower : WandiModPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    /// <summary>回合开始+2血仇。</summary>
    public override async Task AfterSideTurnStart(CombatSide side, System.Collections.Generic.IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (!participants.Contains(Owner)) return;
        await VengeancePower.Grant(new ThrowingPlayerChoiceContext(), Owner, 2, null);
    }

    /// <summary>攻击牌额外造成等同血仇层数的固定伤害（不消耗血仇）。</summary>
    public override decimal ModifyDamageAdditive(
        Creature? target, decimal amount, ValueProp props,
        Creature? dealer, CardModel? cardSource, CardPlay? cardPlay)
    {
        if (dealer != Owner || !props.IsPoweredAttack()) return 0m;
        int blood = Owner.GetPower<VengeancePower>()?.Amount ?? 0;
        return blood; // 额外固定伤害 = 血仇层数
    }

    /// <summary>攻击造成伤害时回复 2% 最大生命（简化：每次攻击命中回 2% MaxHp）。</summary>
    public override async Task AfterDamageReceived(PlayerChoiceContext ctx, Creature target,
        DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        // dealer==Owner = 万敌打出去的伤害；target 是被攻击者
        if (dealer != Owner || target == Owner) return;
        decimal heal = Owner.MaxHp * 0.02m;
        await CreatureCmd.Heal(Owner, heal);
        MainFile.Logger.Info($"[弑神登神] 攻击回血 {heal}（MaxHp×2%）");
    }
}
