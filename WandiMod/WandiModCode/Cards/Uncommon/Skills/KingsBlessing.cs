using BaseLib.Extensions;                           // WithUpgrade
using MegaCrit.Sts2.Core.Commands;                 // CreatureCmd（格挡 + 失血）
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardMultiplayerConstraint
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // BlockVar / IntVar
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using WandiMod.WandiModCode.Powers;                 // VengeancePower（关键词）

using WandiMod.WandiModCode.Extensions;  // WithUpgradeTo（升级目标值语义）

namespace WandiMod.WandiModCode.Cards;

// 联机

/// <summary>
/// 王者之佑 / King's Blessing（罕见 · 技能 · 联机）
/// 所有队友获得 14 点格挡，失去 5 点生命值。升级：20 格挡。
/// —— 联机专属群体护盾：给全队（含自己）发格挡，代价是每人掉 5 血。
///   万敌自身掉血会触发 VengeancePower 失血叠层（+1 血仇），与血仇引擎联动。
///
/// 实现说明：
///   - 队友枚举走 CombatState.GetTeammatesOf（含自己），参考原生 Rally。
///   - 格挡走 BlockVar + CreatureCmd.GainBlock（BlockVar=ValueProp.Move，吃各自敏捷，参考原生 Armaments/Backflip）。
///   - 失血走 CreatureCmd.Damage + Unblockable（绕过刚发的格挡，确保实打实掉血）。
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
        new BlockVar(14, ValueProp.Move).WithUpgradeTo(20),  // 给每个队友的格挡
        new IntVar("HpLoss", 5),                            // 每个队友失去的生命值（固定 5）
    ];

    // 失血会触发血仇引擎（万敌自身），挂血仇关键词便于 tooltip
    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Vengeance];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var creature = Owner.Creature;
        if (creature == null || CombatState == null)
        {
            MainFile.Logger.Error("[王者之佑] OnPlay 时 Owner.Creature 或 CombatState 为空，效果未触发");
            return;
        }

        decimal hpLoss = DynamicVars["HpLoss"].IntValue;
        // 枚举所有队友（同侧、存活、玩家身份；含自己）——参考原生 Rally
        var teammates = CombatState.GetTeammatesOf(creature)
            .Where(c => c != null && c.IsAlive && c.IsPlayer)
            .ToList();

        foreach (var ally in teammates)
        {
            // ① 格挡（BlockVar 吃各自敏捷；GainBlock 的 BlockVar 重载，参考原生 Rally）
            await CreatureCmd.GainBlock(ally, DynamicVars.Block, cardPlay);
            // ② 失血（Unblockable 绕过格挡；万敌掉血会触发 VengeancePower +1 血仇）
            await CreatureCmd.Damage(choiceContext, ally, hpLoss,
                ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this, cardPlay);
        }

        MainFile.Logger.Info($"[王者之佑] 给 {teammates.Count} 名队友（含自己）各发 {DynamicVars.Block.BaseValue} 格挡、各掉 {hpLoss} 血");
    }
}
