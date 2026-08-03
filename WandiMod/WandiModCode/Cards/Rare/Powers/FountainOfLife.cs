using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using WandiMod.WandiModCode.Character;
using WandiMod.WandiModCode.Extensions;
using WandiMod.WandiModCode.Powers;

namespace WandiMod.WandiModCode.Cards;

/// <summary>生命之泉 / Fountain of Life（稀有 · 能力）。每回合开始获得「入场/基础最大生命」5%/8% 的纷争（不含纷争膨胀）。</summary>
public class FountainOfLife : WandiModCard
{
    public FountainOfLife() : base(2, CardType.Power, CardRarity.Rare, TargetType.Self) { }
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("MaxHpPct", 5).WithUpgradeTo(8)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Strife];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        => await PowerCmd.Apply<FountainOfLifePower>(choiceContext, Owner.Creature, DynamicVars["MaxHpPct"].IntValue, Owner.Creature, this);
}
