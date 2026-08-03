using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using WandiMod.WandiModCode.Character;
using WandiMod.WandiModCode.Powers;

namespace WandiMod.WandiModCode.Cards;

/// <summary>力敌万邦 / Rival All Lands ⭐（稀有 · 能力，原「纷争形态」）。打出获得 4 血仇；回合开始 +4 血仇。升级降费。</summary>
public class RivalAllLands : WandiModCard
{
    public RivalAllLands() : base(3, CardType.Power, CardRarity.Rare, TargetType.Self) { }
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("Vengeance", 4)];
    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // ① 打出时立即获得 4 血仇
        await VengeancePower.Grant(choiceContext, Owner.Creature, DynamicVars["Vengeance"].IntValue, this);
        // ② 授予 Power（Amount=4=每回合产血仇数）：回合开始 +4 血仇
        await PowerCmd.Apply<RivalAllLandsPower>(choiceContext, Owner.Creature, DynamicVars["Vengeance"].IntValue, Owner.Creature, this);
    }
}
