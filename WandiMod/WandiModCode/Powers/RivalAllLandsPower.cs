using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace WandiMod.WandiModCode.Powers;

/// <summary>力敌万邦 Power：回合开始 +Amount 血仇（Amount 由卡牌传入 = 4）。</summary>
public class RivalAllLandsPower : WandiModPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public override async Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (!participants.Contains(Owner)) return;
        await VengeancePower.Grant(new ThrowingPlayerChoiceContext(), Owner, Amount, null);
        MainFile.Logger.Info($"[力敌万邦] 回合开始 +{Amount} 血仇");
    }
}
