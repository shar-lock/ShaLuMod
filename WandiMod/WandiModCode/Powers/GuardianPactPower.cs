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
/// 机制（简化版）：本回合——队友受到的伤害减半（升级 75%），你多承受 50% 的伤害。
///   - 钩子 ModifyDamageMultiplicative（分发到战斗中所有模型，含本 Power；故单一 Power 即可改两队伤害）：
///       · target == Owner（自己挨打）→ 返回 1.5（多承受 50%）
///       · target 是同侧玩家队友（!= 自己）→ 返回 1 − Amount/100（Amount=50→0.5，Amount=75→0.25）
///       · 其余 → 1
///   - 钩子 AfterSideTurnEnd：敌方回合结束后移除自身（参考原生 FlameBarrierPower 的 opposing-side-end 移除）。
///   - Amount = 队友减伤百分比（50 或 75，由卡牌按升级态传入）；自身惩罚固定 50%（不随升级变）。
///
/// // TODO: 运行时确认——
///   1) ModifyDamageMultiplicative 是否对所有生物的伤害事件都咨询本 Power（理论上是；若否需改走 DieForYouPower 的 ModifyUnblockedDamageTarget 全额转移）；
///   2) 「本回合」语义当前实现为「敌方回合结束移除」，若设计要求「玩家自己回合结束移除」请调整 side 判定；
///   3) 分数比例伤害转移（如「替队友承担 50%」精确语义）需自定义 ModifyUnblockedDamageTarget，当前为乘法减伤近似。
/// </summary>
public class GuardianPactPower : WandiModPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;  // 一次性临时增益，不叠加

    /// <summary>自身挨打的额外惩罚倍率（多承受 50%）。</summary>
    private const decimal SelfPenaltyMul = 1.5m;

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

        // 自己挨打：多承受 50%
        if (target == Owner)
            return SelfPenaltyMul;

        // 同侧玩家队友挨打：减伤（1 − Amount/100）。Side/IsPlayer 见 Agent 多人报告
        if (target.Side == Owner.Side && target.IsPlayer && target != Owner)
            return 1m - Amount / 100m;

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
