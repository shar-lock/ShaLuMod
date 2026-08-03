using MegaCrit.Sts2.Core.Commands;                  // CreatureCmd / PowerCmd
using MegaCrit.Sts2.Core.Commands.Builders;         // AttackCommand / AttackContext（多段 + 活力）
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // DamageVar / RepeatVar / IntVar
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using WandiMod.WandiModCode.Character;              // WandiModCard
using WandiMod.WandiModCode.Extensions;             // WithUpgradeTo
using WandiMod.WandiModCode.Powers;                 // VengeancePower

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 弑王枪·连突 / Kingslayer Chain（罕见 · 攻击）
/// 造成 11 伤害 ×2；消耗 1 层【血仇】。升级：15 ×2。
/// —— 纯血仇消耗件（无增伤加成）：打出需消耗 1 层血仇，由 WandiModCard 基类的 BloodCost 门控
///   （血仇≤1 不可打出、充足时金边高亮）。多段走 AttackContext 保证活力覆盖每一段。
/// </summary>
public class KingslayerChain : WandiModCard
{
    public KingslayerChain() : base(
        cost: 2,
        type: CardType.Attack,
        rarity: CardRarity.Uncommon,
        target: TargetType.AnyEnemy)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(10, ValueProp.Move).WithUpgradeTo(14),  // 每段基础伤害 10→14
        new RepeatVar(2),                                     // 固定 2 段（升级不变）
        new IntVar("BloodCost", 1),                           // 消耗 1 血仇（BloodCost：基类门控 IsPlayable/金边）
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [WandiModKeywords.Vengeance];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var creature = Owner.Creature;
        if (creature == null || cardPlay.Target == null || CombatState == null)
        {
            MainFile.Logger.Error("[弑王枪·连突] OnPlay 时 Owner.Creature / 目标 / CombatState 为空，效果未触发");
            return;
        }

        // ① 消耗 BloodCost 层血仇（纯费用，无增伤；门控保证打出时血仇充足，消耗后保底≥1）
        int bloodCost = DynamicVars["BloodCost"].IntValue;
        await PowerCmd.Apply<VengeancePower>(choiceContext, creature, -bloodCost, creature, null);

        // ② 11/15 ×2 多段（AttackContext：一次 BeforeAttack/AfterAttack，中间多段吃满活力）
        int hits = DynamicVars.Repeat.IntValue;
        decimal baseDmg = DynamicVars.Damage.BaseValue;
        await using var ctx = await AttackCommand.CreateContextAsync(CombatState, choiceContext, cardPlay);
        for (int i = 0; i < hits; i++)
        {
            var results = await CreatureCmd.Damage(
                choiceContext, cardPlay.Target, baseDmg, ValueProp.Move, this, cardPlay);
            ctx.AddHit(results);
        }

        MainFile.Logger.Info($"[弑王枪·连突] 消耗 1 血仇，打出 {hits} 段 × {baseDmg} 伤");
    }
}
