using BaseLib.Extensions;                           // WithUpgrade 扩展方法
using MegaCrit.Sts2.Core.Commands;                 // CardPileCmd（抽牌）
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // IntVar
using WandiMod.WandiModCode.Powers;                 // VengeancePower

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 战意 / Focus（普通 · 技能）
/// 抽 2 张牌，获得 1 层【血仇】。升级：抽 3 张。
/// —— 万敌的基础过牌件，顺带喂养血仇引擎。
/// </summary>
public class Focus : WandiModCard
{
    public Focus() : base(
        cost: 1,
        type: CardType.Skill,
        rarity: CardRarity.Common,
        target: TargetType.Self)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("Draw", 2).WithUpgrade(3),
        new IntVar("Vengeance", 1),
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Vengeance];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 抽牌（CardPileCmd.Draw：count 用 decimal 重载；Owner 是 Player 不是 Creature）
        await CardPileCmd.Draw(choiceContext, DynamicVars["Draw"].IntValue, Owner);
        await VengeancePower.Grant(choiceContext, Owner.Creature, DynamicVars["Vengeance"].IntValue, this);
    }
}
