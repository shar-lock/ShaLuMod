using MegaCrit.Sts2.Core.Commands;                  // CreatureCmd（回血）
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // IntVar
using WandiMod.WandiModCode.Extensions;             // WithUpgradeTo
using WandiMod.WandiModCode.Powers;                 // VengeancePower

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 坚壁 / Bulwark（普通 · 技能）
/// 回复 8 点生命值，获得 1 层【血仇】。升级：12 生命值，2 血仇。
/// —— 2 费回血 + 血仇引擎件。
/// </summary>
public class Bulwark : WandiModCard
{
    public Bulwark() : base(
        cost: 2,
        type: CardType.Skill,
        rarity: CardRarity.Common,
        target: TargetType.Self)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("Heal", 8).WithUpgradeTo(10),
        new IntVar("Vengeance", 1).WithUpgradeTo(2),
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Vengeance];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.Heal(Owner.Creature, DynamicVars["Heal"].IntValue);
        await VengeancePower.Grant(choiceContext, Owner.Creature, DynamicVars["Vengeance"].IntValue, this);
    }
}
