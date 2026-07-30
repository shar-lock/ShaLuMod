using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using WandiMod.WandiModCode.Character;
using WandiMod.WandiModCode.Powers;

namespace WandiMod.WandiModCode.Cards;

/// <summary>血仇主宰 / Vengeance Dominion（稀有 · 能力 · 虚无）。血仇层数不会被消耗。</summary>
public class VengeanceDominion : WandiModCard
{
    public VengeanceDominion() : base(2, CardType.Power, CardRarity.Rare, TargetType.Self) { }
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Ethereal, WandiModKeywords.Vengeance];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        => await PowerCmd.Apply<VengeanceDominionPower>(choiceContext, Owner.Creature, 0, Owner.Creature, this);
}
