using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using WandiMod.WandiModCode.Character;
using WandiMod.WandiModCode.Extensions;
using WandiMod.WandiModCode.Powers;

namespace WandiMod.WandiModCode.Cards;

/// <summary>不死王权 / Undying Sovereignty ⭐（稀有 · 能力）。每次受到攻击时回复最大生命1%/2%的血量。</summary>
public class UndyingSovereignty : WandiModCard
{
    public UndyingSovereignty() : base(3, CardType.Power, CardRarity.Rare, TargetType.Self) { }
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("HealPct", 1).WithUpgradeTo(2)];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        => await PowerCmd.Apply<UndyingSovereigntyPower>(choiceContext, Owner.Creature, DynamicVars["HealPct"].IntValue, Owner.Creature, this);
}
