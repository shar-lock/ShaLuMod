using MegaCrit.Sts2.Core.Commands;                  // CreatureCmd（回血）
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // IntVar
using WandiMod.WandiModCode.Powers;                 // VengeancePower

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 残影 / Afterimage（起手 · 技能，初始机制牌）
/// 获得 1 层【血仇】，回复 3 点生命。升级：费用 1 → 0。
/// —— 升级走降费（EnergyCost.UpgradeBy(-1)，参考原版 Alchemize），数值本身不变。
/// </summary>
public class Afterimage : WandiModCard
{
    public Afterimage() : base(
        cost: 1,
        type: CardType.Skill,
        rarity: CardRarity.Basic,
        target: TargetType.Self)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("Vengeance", 1),
        new IntVar("Heal", 3),
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Vengeance];

    // 升级降费：1 费 → 0 费（原版惯例写法，见 Alchemize.OnUpgrade）
    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await VengeancePower.Grant(choiceContext, Owner.Creature, DynamicVars["Vengeance"].IntValue, this);
        await CreatureCmd.Heal(Owner.Creature, DynamicVars["Heal"].IntValue);
    }
}
