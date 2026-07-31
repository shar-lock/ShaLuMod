using MegaCrit.Sts2.Core.Commands;                  // CreatureCmd（回血）/ CardPileCmd（抽牌）
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // IntVar
using WandiMod.WandiModCode.Character;              // WandiModCard
using WandiMod.WandiModCode.Extensions;             // WithUpgradeTo

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 备战 / Preparations（普通 · 技能）
/// 回复 3 点生命值，抽 1 张牌。升级：5 生命值，抽 2 张牌。
/// —— 回血+过牌，维持手牌数量同时续航。
/// </summary>
public class Preparations : WandiModCard
{
    public Preparations() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self) { }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("Heal", 3).WithUpgradeTo(5),
        new IntVar("Draw", 1).WithUpgradeTo(2),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.Heal(Owner.Creature, DynamicVars["Heal"].IntValue);
        await CardPileCmd.Draw(choiceContext, DynamicVars["Draw"].IntValue, Owner);
    }
}
