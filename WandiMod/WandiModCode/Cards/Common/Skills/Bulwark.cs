using BaseLib.Extensions;                           // WithUpgrade 扩展方法
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // IntVar
using WandiMod.WandiModCode.Powers;                 // StrifePower / VengeancePower

using WandiMod.WandiModCode.Extensions;  // WithUpgradeTo（升级目标值语义）

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 坚壁 / Bulwark（普通 · 技能）
/// 获得 10 点【纷争】和 1 层【血仇】。升级：12 纷争，2 血仇。
/// —— 2 费大额纷争，是纷争流的中坚防御（等效大格挡，但走的是临时上限通道）。
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
        new IntVar("Strife", 10).WithUpgradeTo(12),
        new IntVar("Vengeance", 1).WithUpgradeTo(2),
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Strife, WandiModKeywords.Vengeance];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await StrifePower.Grant(choiceContext, Owner.Creature, DynamicVars["Strife"].IntValue, Owner.Creature, this);
        await VengeancePower.Grant(choiceContext, Owner.Creature, DynamicVars["Vengeance"].IntValue, this);
    }
}
