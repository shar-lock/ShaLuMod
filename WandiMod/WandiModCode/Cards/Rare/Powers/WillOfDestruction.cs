using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using WandiMod.WandiModCode.Character;
using WandiMod.WandiModCode.Powers;

namespace WandiMod.WandiModCode.Cards;

/// <summary>毁灭意志 / Will of Destruction（稀有 · 能力）。每次失血获得+1力量。升级降费。</summary>
public class WillOfDestruction : WandiModCard
{
    public WillOfDestruction() : base(2, CardType.Power, CardRarity.Rare, TargetType.Self) { }
    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        => await PowerCmd.Apply<WillOfDestructionPower>(choiceContext, Owner.Creature, 0, Owner.Creature, this);
}
