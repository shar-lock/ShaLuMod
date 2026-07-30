using BaseLib.Extensions;                           // WithUpgrade 扩展方法
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // IntVar
using WandiMod.WandiModCode.Powers;                 // StrifePower

using WandiMod.WandiModCode.Extensions;  // WithUpgradeTo（升级目标值语义）

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 巨灵之躯 / Titan Body（罕见 · 技能 · 消耗）
/// 获得 15 点【纷争】。升级：18 纷争。
/// —— 2 费大额纷争件（临时上限大幅扩张）。消耗防与「涅槃/血祭」类卡牌循环刷临时上限。
/// </summary>
public class TitanBody : WandiModCard
{
    public TitanBody() : base(
        cost: 2,
        type: CardType.Skill,
        rarity: CardRarity.Uncommon,
        target: TargetType.Self)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("Strife", 15).WithUpgradeTo(18),
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Strife, CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await StrifePower.Grant(choiceContext, Owner.Creature, DynamicVars["Strife"].IntValue, Owner.Creature, this);
    }
}
