using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using WandiMod.WandiModCode.Cards;

namespace WandiMod.WandiModCode.Powers;

/// <summary>力敌万邦 Power：回合开始+2血仇；获得血仇时抽1（每回合≤2）。</summary>
public class RivalAllLandsPower : WandiModPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    private int _drawnThisTurn = 0;
    private const int DrawCap = 2;

    public override async Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (!participants.Contains(Owner)) return;
        _drawnThisTurn = 0;
        await VengeancePower.Grant(new ThrowingPlayerChoiceContext(), Owner, 2, null);
    }

    public override async Task AfterPowerAmountChanged(PlayerChoiceContext ctx, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (power is not VengeancePower || amount <= 0 || _drawnThisTurn >= DrawCap) return;
        await CardPileCmd.Draw(ctx, 1, Owner.Player);
        _drawnThisTurn++;
    }
}
