using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // IntVar
using WandiMod.WandiModCode.Character;              // WandiModCard
using WandiMod.WandiModCode.Extensions;             // WithUpgradeTo
using WandiMod.WandiModCode.Powers;                 // StrifePower

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 凝血 / Coagulate（普通 · 技能）
/// 获得 6 点【纷争】。升级：9 纷争。
/// —— 纯纷争件（御敌是 Basic 4/6，这是 Common 6/9，更大的防御量）。
/// </summary>
public class Coagulate : WandiModCard
{
    public Coagulate() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self) { }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("Strife", 6).WithUpgradeTo(9),
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Strife];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await StrifePower.Grant(choiceContext, Owner.Creature, DynamicVars["Strife"].IntValue, Owner.Creature, this);
    }
}
