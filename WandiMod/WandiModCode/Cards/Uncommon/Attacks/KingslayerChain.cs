using MegaCrit.Sts2.Core.Commands;                  // DamageCmd / PowerCmd / CreatureCmd
using MegaCrit.Sts2.Core.Commands.Builders;         // AttackCommand / AttackContext（多段可变伤 + 活力）
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType / CardKeyword
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // DamageVar / RepeatVar / IntVar
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using WandiMod.WandiModCode.Character;              // WandiModCard
using WandiMod.WandiModCode.Powers;                 // VengeancePower
using WandiMod.WandiModCode.Extensions;             // WithUpgradeTo（升级目标值语义）

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 弑王枪·连突 / Kingslayer Chain（罕见 · 攻击）
/// 造成 5 伤害 ×2；每段消耗 1 层【血仇】额外 +5 伤害（无血仇则不加）。升级：6 伤害 ×2，每段 +6。
/// —— 连刺（RepeatedThrust）的罕见加强版：更高单段基数 + 更高血仇增伤，2 费标杆多段。
/// 每段伤害可变（是否耗血仇决定），不能用扁平 WithHitCount——改走 AttackContext（回响斩/全切同款）：
/// CreateContextAsync 触发一次 BeforeAttack（活力绑定整次攻击），逐段 CreatureCmd.Damage + AddHit，
/// DisposeAsync 触发一次 AfterAttack（活力此时才消耗）→ 活力覆盖每一段。
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
        new DamageVar(5, ValueProp.Move).WithUpgradeTo(6),  // 每段基础伤害 5→6
        new RepeatVar(2),                                  // 固定 2 段（升级不变）
        new IntVar("BloodBonus", 5).WithUpgradeTo(6),        // 每段消耗 1 血仇的额外伤害 5→6
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

        int hits = DynamicVars.Repeat.IntValue;
        decimal baseDmg = DynamicVars.Damage.BaseValue;
        int bonus = DynamicVars["BloodBonus"].IntValue;
        int consumed = 0;

        // AttackContext：一次 BeforeAttack / AfterAttack，中间可变伤多段仍吃满活力
        await using var ctx = await AttackCommand.CreateContextAsync(CombatState, choiceContext, cardPlay);
        for (int i = 0; i < hits; i++)
        {
            decimal dmg = baseDmg;
            var vengeance = creature.GetPower<VengeancePower>();
            // 保底1层：Amount>=2 才允许消耗（Amount==1 是地板层，耗掉会把血仇 Power 移除）
            if (vengeance != null && vengeance.Amount >= 2)
            {
                await PowerCmd.Apply<VengeancePower>(choiceContext, creature, -1, creature, null);
                dmg += bonus;
                consumed++;
            }
            var results = await CreatureCmd.Damage(
                choiceContext, cardPlay.Target, dmg, ValueProp.Move, this, cardPlay);
            ctx.AddHit(results);
        }

        if (consumed > 0)
            MainFile.Logger.Info($"[弑王枪·连突] 打出 {hits} 段，消耗 {consumed} 血仇增伤");
    }
}
