using MegaCrit.Sts2.Core.Commands;                  // CreatureCmd（回血）
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardTag
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // IntVar
using WandiMod.WandiModCode.Extensions;             // WithUpgradeTo（升级目标值语义）

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 御敌 / Hold the Line（起手 · 技能，原「防御」位）
/// 回复 4 点生命值。升级：5 点。
/// —— CanonicalTags 含 CardTag.Defend，供潘多拉魔盒等 IsBasicStrikeOrDefend 识别替换（原版 Defend 同款）。
/// </summary>
public class HoldTheLine : WandiModCard
{
    public HoldTheLine() : base(
        cost: 1,
        type: CardType.Skill,
        rarity: CardRarity.Basic,
        target: TargetType.Self)
    {
    }

    // 原版各角色 Defend 均挂 Defend 标签；潘多拉魔盒过滤 IsBasicStrikeOrDefend 依赖此标签
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Defend];

    // 回血数值（真相源）。IntVar 名称 "Heal" 对应本地化占位符 {Heal:diff()}。
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("Heal", 4).WithUpgradeTo(5),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (Owner.Creature == null)
        {
            MainFile.Logger.Error("[御敌] OnPlay 时 Owner.Creature 为空（不在战斗中？），回血未生效");
            return;
        }
        await CreatureCmd.Heal(Owner.Creature, DynamicVars["Heal"].IntValue);
    }
}
