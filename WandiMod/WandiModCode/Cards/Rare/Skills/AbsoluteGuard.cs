using WandiMod.WandiModCode.Character;
using WandiMod.WandiModCode.Extensions;
using WandiMod.WandiModCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace WandiMod.WandiModCode.Cards;

/// <summary>绝对防御 / Absolute Guard（稀有 · 技能）。获得「当前最大生命」8%/10% 的纷争。</summary>
public class AbsoluteGuard : WandiModCard
{
    public AbsoluteGuard() : base(2, CardType.Skill, CardRarity.Rare, TargetType.Self) { }
    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("MaxHpPct", 8).WithUpgradeTo(10)];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Strife];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var c = Owner.Creature;
        int strife = (int)(c.MaxHp * DynamicVars["MaxHpPct"].IntValue / 100m);
        await StrifePower.Grant(choiceContext, c, strife, c, this);
        MainFile.Logger.Info($"[绝对防御] MaxHp={c.MaxHp} → {strife} 纷争");
    }
}
