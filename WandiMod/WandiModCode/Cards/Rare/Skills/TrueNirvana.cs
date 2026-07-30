using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using WandiMod.WandiModCode.Character;
using WandiMod.WandiModCode.Extensions;

namespace WandiMod.WandiModCode.Cards;

/// <summary>涅槃重生 / True Nirvana（稀有 · 技能 · 消耗）。全局增加 +5 最大生命 / +7。</summary>
public class TrueNirvana : WandiModCard
{
    public TrueNirvana() : base(2, CardType.Skill, CardRarity.Rare, TargetType.Self) { }
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("MaxHpGain", 5).WithUpgradeTo(7)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainMaxHp(Owner.Creature, DynamicVars["MaxHpGain"].IntValue);
        MainFile.Logger.Info($"[涅槃重生] 最大生命 +{DynamicVars["MaxHpGain"].IntValue}");
    }
}
