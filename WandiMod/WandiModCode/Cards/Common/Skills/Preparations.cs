using MegaCrit.Sts2.Core.Commands;                  // CardPileCmd（抽牌）
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // IntVar
using WandiMod.WandiModCode.Character;              // WandiModCard
using WandiMod.WandiModCode.Extensions;             // WithUpgradeTo
using WandiMod.WandiModCode.Powers;                 // StrifePower

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 备战 / Preparations（普通 · 技能）
/// 获得 4 点【纷争】，抽 1 张牌。升级：6 纷争，抽 2 张牌。
/// —— 纷争+过牌（纷争版 Backflip），维持手牌数量同时建防御。
/// </summary>
public class Preparations : WandiModCard
{
    public Preparations() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self) { }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("Strife", 4).WithUpgradeTo(6),
        new IntVar("Draw", 1).WithUpgradeTo(2),
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Strife];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await StrifePower.Grant(choiceContext, Owner.Creature, DynamicVars["Strife"].IntValue, Owner.Creature, this);
        await CardPileCmd.Draw(choiceContext, DynamicVars["Draw"].IntValue, Owner);
    }
}
