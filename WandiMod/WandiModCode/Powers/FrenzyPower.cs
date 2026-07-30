using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace WandiMod.WandiModCode.Powers;

/// <summary>狂化 Power：获得血仇时抽1牌（每回合≤Amount）。Amount=抽牌上限。</summary>
public class FrenzyPower : WandiModPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    private int _drawnThisTurn = 0;

    // TODO: 需重写 AfterSideTurnStart 重置 _drawnThisTurn=0
    public override async Task AfterPowerAmountChanged(PlayerChoiceContext ctx, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
    {
        if (power is not VengeancePower || amount <= 0 || _drawnThisTurn >= Amount) return;
        await CardPileCmd.Draw(ctx, 1, Owner.Player);
        _drawnThisTurn++;
    }
}
