using MegaCrit.Sts2.Core.Commands;                 // PowerCmd（移除自身）
using MegaCrit.Sts2.Core.Combat;                   // CombatSide / ICombatState
using MegaCrit.Sts2.Core.Entities.Cards;           // CardPlay
using MegaCrit.Sts2.Core.Entities.Creatures;       // Creature
using MegaCrit.Sts2.Core.Entities.Powers;          // PowerType / PowerStackType
using MegaCrit.Sts2.Core.GameActions.Multiplayer;  // PlayerChoiceContext
using MegaCrit.Sts2.Core.Models;                   // CardModel
using MegaCrit.Sts2.Core.ValueProps;               // ValueProp

namespace WandiMod.WandiModCode.Powers;

/// <summary>
/// 庇护盟约 / Guardian Pact（万敌 · 联机临时增益 Power）。
/// 机制：本回合——队友受到的伤害减半（固定 50%），你多承受 Amount% 的伤害（Amount=50 或 45）。
///   - 钩子 ModifyDamageMultiplicative（已确认：全局分发，对所有生物伤害事件都咨询本 Power）：
///       · target == Owner（自己挨打）→ 返回 1 + Amount/100（Amount=50→1.5，Amount=45→1.45）
///       · target 是同侧玩家队友（!= 自己）→ 返回 0.5（固定减半）
///       · 其余 → 1
///   - 钩子 AfterSideTurnEnd：敌方回合结束后移除自身（参考原生 FlameBarrierPower）。
///   - Amount = 自身增伤百分比（50 或 45）。
/// </summary>
public class GuardianPactPower : WandiModPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;  // 一次性临时增益，不叠加

    /// <summary>队友挨打的固定减伤倍率（减半）。</summary>
    private const decimal AllyReduceMul = 0.5m;

    public override decimal ModifyDamageMultiplicative(
        Creature? target,
        decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource,
        CardPlay? cardPlay)
    {
        if (target == null || Owner == null)
            return 1m;

        // 自己挨打：多承受 Amount%（Amount=50→1.5，Amount=45→1.45）
        if (target == Owner)
            return 1m + Amount / 100m;

        // 同侧玩家队友挨打：固定减半
        if (target.Side == Owner.Side && target.IsPlayer && target != Owner)
            return AllyReduceMul;

        return 1m;
    }

    /// <summary>
    /// 敌方回合结束后移除自身（保护覆盖了敌方出手阶段）。
    /// 参考 FlameBarrierPower.AfterSideTurnEnd：`if (Owner.Side == side) return; await PowerCmd.Remove(this);`
    /// </summary>
    public override async Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (side == Owner.Side)
            return;
        await PowerCmd.Remove(this);
        MainFile.Logger.Info("[庇护盟约] 敌方回合结束，移除庇护效果");
    }
}
