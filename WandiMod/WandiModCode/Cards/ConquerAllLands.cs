using BaseLib.Abstracts;                        // CustomCardModel
using BaseLib.Extensions;                       // RemovePrefix（资源路径用）
using MegaCrit.Sts2.Core.Commands;              // DamageCmd
using MegaCrit.Sts2.Core.Entities.Cards;        // CardType / CardRarity / TargetType / CardKeyword / CardPlay
using MegaCrit.Sts2.Core.GameActions.Multiplayer;// PlayerChoiceContext
using MegaCrit.Sts2.Core.Localization.DynamicVars;// DamageVar / IntVar
using MegaCrit.Sts2.Core.ValueProps;            // ValueProp
using WandiMod.WandiModCode.Extensions;          // CardImagePath / BigCardImagePath（资源路径）

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 荡平万邦 / Conquer all lands!（机制卡 · 攻击 · 0 费）
/// 由起手遗物「弑亲血脉」在血仇 ≥ 7 时生成到手牌（保留 + 消耗）。
/// 效果：对全体敌方造成 14 伤 + 「已损失生命」20% + 「敌方最大生命」5%（升级：20 伤 + 30%）。
///
/// 实现说明：
///   - 不入卡池（仅由遗物生成）——直接继承 CustomCardModel 不标 [Pool]，稀有度用 Token
///     （参考原生 Soul/灵魂、Apparition：生成卡均无 Pool、用 Token/Ancient 稀有度，不掉落）。
///     卡牌不像遗物那样强制要求 PoolAttribute（原生 Token 卡即为先例），无需担心启动崩溃。
///   - 三段加法伤害（14 + MissingHp×20% + 敌MaxHp×5%）不适合 CalculatedDamageVar 的 base×倍率模型，
///     故 OnPlay 里逐敌用 DamageCmd 手动结算（ValueProp.Move 自动吃力量加成）。
/// </summary>
public class ConquerAllLands : CustomCardModel
{
    public ConquerAllLands() : base(
        cost: 0,
        type: CardType.Attack,
        rarity: CardRarity.Token,       // Token = 生成卡稀有度，不入掉落池（参考原生 Soul/灵魂）
        target: TargetType.AllEnemies)
    {
    }

    // 资源路径（类名小写，缺图回退 card.png 占位）
    public override string CustomPortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigCardImagePath();
    public override string PortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();

    // 数值（真相源）：基础伤害 14→20；已损失生命百分比 20→30（IntVar 存整数，运算时 /100）
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(14, ValueProp.Move).WithUpgrade(20),
        new IntVar("MissingHpPct", 20).WithUpgrade(30),
    ];

    // 词条：保留（留手）+ 消耗（打出后消失，防囤积）
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Retain, CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // Owner 是 Player，生物实体取 Owner.Creature；战斗外 / 异常时序下可能为 null
        var creature = Owner.Creature;
        if (creature == null)
        {
            MainFile.Logger.Error("[荡平万邦] OnPlay 时 Owner.Creature 为空（不在战斗中？），效果未触发");
            return;
        }
        if (CombatState == null)
        {
            MainFile.Logger.Error("[荡平万邦] CombatState 为空，无法取敌方列表，效果未触发");
            return;
        }

        decimal baseDmg = DynamicVars.Damage.BaseValue;                   // 14 / 20
        decimal missingPct = DynamicVars["MissingHpPct"].IntValue / 100m; // 0.20 / 0.30
        const decimal enemyMaxPct = 0.05m;                                 // 敌方最大生命 5%（固定，不随升级变）
        decimal missingHp = creature.MaxHp - creature.CurrentHp;          // 已损失生命
        // 对所有敌人相同的基础 + MissingHp 部分
        decimal fixedPart = baseDmg + missingHp * missingPct;

        var enemies = CombatState.HittableEnemies.ToList();
        if (enemies.Count == 0)
        {
            MainFile.Logger.Warn("[荡平万邦] 没有可命中的敌人，伤害落空");
            return;
        }

        // 逐敌结算：每个敌人额外加「其最大生命 5%」（因敌而异，故逐个打）
        foreach (var enemy in enemies)
        {
            decimal total = fixedPart + enemy.MaxHp * enemyMaxPct;
            // ValueProp.Move → 受力量加成（走 Hook.ModifyDamage）；FromCard 关联本牌用于战斗记录/VFX
            await DamageCmd.Attack(total)
                .FromCard(this)
                .Targeting(enemy)
                .WithValueProp(ValueProp.Move)
                .Execute(choiceContext);
        }

        MainFile.Logger.Info($"[荡平万邦] 打出：基础 {baseDmg} + 已损失生命 {missingHp}×{missingPct} = {fixedPart}（每敌再 + 其MaxHp×{enemyMaxPct}），命中 {enemies.Count} 个敌人");
    }
}
