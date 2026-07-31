using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using WandiMod.WandiModCode.Character;
using WandiMod.WandiModCode.Extensions;
using WandiMod.WandiModCode.Powers;

namespace WandiMod.WandiModCode.Cards;

/// <summary>血色共鸣 / Blood Resonance（稀有 · 能力）。每次失血获得 1/2 纷争（上限服从全局 100%）。</summary>
public class BloodResonance : WandiModCard
{
    public BloodResonance() : base(2, CardType.Power, CardRarity.Rare, TargetType.Self) { }
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("Strife", 1).WithUpgradeTo(2)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Strife, WandiModKeywords.Vengeance];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        => await PowerCmd.Apply<BloodResonancePower>(choiceContext, Owner.Creature, DynamicVars["Strife"].IntValue, Owner.Creature, this);
}
