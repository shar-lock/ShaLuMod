using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

namespace WandiMod.WandiModCode.Powers;

/// <summary>
/// 金色壁垒·下回合力量 Power：下回合开始时获得 Amount 力量，然后自毁。
/// 参考 DrawCardsNextTurnPower（触发后自毁）+ DemonFormPower（回合给力量）。
/// </summary>
public class GoldenBastionNextTurnPower : WandiModPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (!participants.Contains(Owner)) return;
        Flash();
        await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), Owner, Amount, Owner, null);
        await PowerCmd.Remove(this);  // 触发后自毁（一次性，参考 DrawCardsNextTurnPower）
        MainFile.Logger.Info($"[金色壁垒] 下回合触发：+{Amount} 力量");
    }
}
