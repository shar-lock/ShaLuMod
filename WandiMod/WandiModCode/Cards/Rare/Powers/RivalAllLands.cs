using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using WandiMod.WandiModCode.Character;
using WandiMod.WandiModCode.Extensions;
using WandiMod.WandiModCode.Powers;

namespace WandiMod.WandiModCode.Cards;

/// <summary>力敌万邦 / Rival All Lands ⭐（稀有 · 能力，原「纷争形态」）。回合开始+2血仇；获得血仇时抽1（每回合≤2）。升级降费。</summary>
public class RivalAllLands : WandiModCard
{
    public RivalAllLands() : base(3, CardType.Power, CardRarity.Rare, TargetType.Self) { }
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("Vengeance", 2), new IntVar("DrawCap", 2)];
    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        => await PowerCmd.Apply<RivalAllLandsPower>(choiceContext, Owner.Creature, 0, Owner.Creature, this);
}
