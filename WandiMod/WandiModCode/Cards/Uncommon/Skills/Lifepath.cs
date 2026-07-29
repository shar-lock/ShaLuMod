using BaseLib.Extensions;                           // WithUpgrade 扩展方法
using MegaCrit.Sts2.Core.Commands;                 // CardPileCmd（抽牌）
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // IntVar
using WandiMod.WandiModCode.Powers;                 // VengeancePower

using WandiMod.WandiModCode.Extensions;  // WithUpgradeTo（升级目标值语义）

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 命途指引 / Lifepath（罕见 · 技能）
/// 抽 2 张牌；若 HP ＜ 50%，额外获得 1 层【血仇】。升级：抽 3 张。
/// —— 灵活过牌件：健康时是纯过牌，受伤时顺带喂养血仇引擎（血仇是失血计数器，低血量触发更贴主题）。
/// </summary>
public class Lifepath : WandiModCard
{
    public Lifepath() : base(
        cost: 1,
        type: CardType.Skill,
        rarity: CardRarity.Uncommon,
        target: TargetType.Self)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("Draw", 2).WithUpgradeTo(3),
        new IntVar("Vengeance", 1),
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Vengeance];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var creature = Owner.Creature;

        // 抽牌
        await CardPileCmd.Draw(choiceContext, DynamicVars["Draw"].IntValue, Owner);

        // HP < 50% 时额外获得血仇
        if (creature != null && creature.CurrentHp < creature.MaxHp * 0.5m)
        {
            await VengeancePower.Grant(choiceContext, creature, DynamicVars["Vengeance"].IntValue, this);
            MainFile.Logger.Info($"[命途指引] HP<50%（{creature.CurrentHp}/{creature.MaxHp}），抽 {DynamicVars["Draw"].IntValue} 牌 + 获得 {DynamicVars["Vengeance"].IntValue} 血仇");
        }
        else
        {
            var hp = creature != null ? $"{creature.CurrentHp}/{creature.MaxHp}" : "?";
            MainFile.Logger.Info($"[命途指引] HP≥50%（{hp}），仅抽 {DynamicVars["Draw"].IntValue} 牌");
        }
    }
}
