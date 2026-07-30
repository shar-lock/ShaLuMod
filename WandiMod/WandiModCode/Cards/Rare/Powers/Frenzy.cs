using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using WandiMod.WandiModCode.Character;
using WandiMod.WandiModCode.Extensions;
using WandiMod.WandiModCode.Powers;

namespace WandiMod.WandiModCode.Cards;

/// <summary>狂化 / Frenzy（稀有 · 能力）。每当你获得血仇，抽1牌（每回合≤2/≤3）。</summary>
public class Frenzy : WandiModCard
{
    public Frenzy() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self) { }
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("DrawCap", 2).WithUpgradeTo(3)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        => await PowerCmd.Apply<FrenzyPower>(choiceContext, Owner.Creature, DynamicVars["DrawCap"].IntValue, Owner.Creature, this);
}
