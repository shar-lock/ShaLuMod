using MegaCrit.Sts2.Core.Commands;                 // CreatureCmd（格挡 + 失血）
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardMultiplayerConstraint
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // BlockVar / IntVar
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using WandiMod.WandiModCode.Extensions;             // WithUpgradeTo（升级目标值语义）

namespace WandiMod.WandiModCode.Cards;

// 联机

/// <summary>
/// 王者之佑 / King's Blessing（罕见 · 技能 · 联机）
/// 所有队友获得 16 点格挡；仅万敌自己失去 1 点生命值。升级：22 格挡。
/// —— 联机专属群体护盾：全队（含自己）发格挡；自伤只打万敌，触发血仇叠层。
/// </summary>
public class KingsBlessing : WandiModCard
{
    public KingsBlessing() : base(
        cost: 2,
        type: CardType.Skill,
        rarity: CardRarity.Uncommon,
        target: TargetType.Self)
    {
    }

    // 联机专属
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new BlockVar(16, ValueProp.Move).WithUpgradeTo(22),  // 给每个队友的格挡
        new IntVar("HpLoss", 1),                            // 仅万敌自己失去的生命值
    ];

    // 万敌自伤会触发血仇引擎，挂血仇关键词便于 tooltip
    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Vengeance];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var creature = Owner.Creature;
        if (creature == null || CombatState == null)
        {
            MainFile.Logger.Error("[王者之佑] OnPlay 时 Owner.Creature 或 CombatState 为空，效果未触发");
            return;
        }

        // 枚举所有队友（同侧、存活、玩家身份；含自己）——参考原生 Rally
        var teammates = CombatState.GetTeammatesOf(creature)
            .Where(c => c != null && c.IsAlive && c.IsPlayer)
            .ToList();

        foreach (var ally in teammates)
            await CreatureCmd.GainBlock(ally, DynamicVars.Block, cardPlay);

        // 仅万敌自己失血（Unblockable 绕过刚发的格挡；触发 VengeancePower +1）
        decimal hpLoss = DynamicVars["HpLoss"].IntValue;
        await CreatureCmd.Damage(choiceContext, creature, hpLoss,
            ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this, cardPlay);

        MainFile.Logger.Info($"[王者之佑] 给 {teammates.Count} 名队友（含自己）各发 {DynamicVars.Block.BaseValue} 格挡；仅万敌失血 {hpLoss}");
    }
}
