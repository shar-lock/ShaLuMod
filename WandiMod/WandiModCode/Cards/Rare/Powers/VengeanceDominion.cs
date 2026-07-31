using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using WandiMod.WandiModCode.Character;
using WandiMod.WandiModCode.Powers;

namespace WandiMod.WandiModCode.Cards;

/// <summary>血仇主宰 / Vengeance Dominion（稀有 · 能力 · 虚无）。血仇层数不会被消耗。</summary>
public class VengeanceDominion : WandiModCard
{
    public VengeanceDominion() : base(3, CardType.Power, CardRarity.Rare, TargetType.Self) { }
    // 设计「虚无 / 升级失去虚无」：基础虚无，升级后不再虚无
    public override IEnumerable<CardKeyword> CanonicalKeywords => IsUpgraded
        ? [WandiModKeywords.Vengeance]
        : [CardKeyword.Ethereal, WandiModKeywords.Vengeance];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        // amount=1：PowerCmd.Apply 在 amount==0 时不附着（PowerCmd.cs:84），标记型 Power 传 1（对齐原生 Barricade）
        => await PowerCmd.Apply<VengeanceDominionPower>(choiceContext, Owner.Creature, 1, Owner.Creature, this);
}
