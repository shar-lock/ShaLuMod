using BaseLib.Abstracts;                        // CustomCardModel
using BaseLib.Extensions;                       // RemovePrefix（资源路径用）
using BaseLib.Utils;                            // [Pool]
using MegaCrit.Sts2.Core.Commands;              // DamageCmd
using MegaCrit.Sts2.Core.Entities.Cards;        // CardType / CardRarity / TargetType / CardKeyword / CardPlay
using MegaCrit.Sts2.Core.GameActions.Multiplayer;// PlayerChoiceContext
using MegaCrit.Sts2.Core.Localization.DynamicVars;// DamageVar / IntVar
using MegaCrit.Sts2.Core.Models.CardPools;       // TokenCardPool（原版机制卡池）
using MegaCrit.Sts2.Core.ValueProps;            // ValueProp
using WandiMod.WandiModCode.Extensions;          // CardImagePath / BigCardImagePath（资源路径）

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 荡平万邦 / Conquer all lands!（机制卡 · 攻击 · 0 费）
/// 由起手遗物「弑亲血脉」在血仇 ≥ 8 时生成到手牌（保留 + 消耗）。
/// 效果：对全体敌方造成 14 伤 + 「已损失生命」25%（升级：20 伤 + 35%）。
///
/// 实现说明：
///   - 机制卡归属**原版 TokenCardPool**（[Pool(typeof(TokenCardPool))]，与原版 Soul/Shiv 同池）：
///       ① BaseLib CustomCardModel 构造时（autoAdd 默认 true）强制要求 PoolAttribute，缺失启动即崩；
///       ② 游戏 CardModel.Pool 找不到所属池会抛 InvalidProgramException（手牌渲染必触达）；
///       ③ Token 池不是奖励/商店/战斗内生成的取数源，Token 稀有度也不会被稀有度掷骰选中 → 不入掉落。
///     （曾误判「卡牌不像遗物那样强制 PoolAttribute」——原生 Token 卡不经过 BaseLib 注册，由
///       TokenCardPool.GenerateAllCards 显式列出，自定义卡必须通过 [Pool] 注入，见 log/TODO。）
///   - 伤害用 **CalculatedDamageVar**（原版 BodySlam/PerfectedStrike 写法）：
///       公式 = CalculationBase(14→20) + ExtraDamage(25→35) × multiplier（已损失生命/100），
///       卡面伤害数字/描述 {CalculatedDamage:diff()} 实时显示含动态加成的总伤（指向敌人时刷新），
///       且与 OnPlay 结算同源，不会出现「显示与实打不一致」。
///   - 全体一致的伤害 → 一次性 AoE（TargetingAllOpponents，原版 Sow/CrashLanding 写法，
///     一次攻击动画、全体同时结算）。
/// </summary>
[Pool(typeof(TokenCardPool))]
public class ConquerAllLands : CustomCardModel
{
    public ConquerAllLands() : base(
        0,
        CardType.Attack,
        CardRarity.Token,       // Token = 生成卡稀有度，不入掉落池（参考原生 Soul/灵魂）
        TargetType.AllEnemies)
    {
    }

    // 资源路径（类名小写，缺图回退 card.png 占位）
    public override string CustomPortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigCardImagePath();
    public override string PortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();

    // 数值（真相源）：基础 14→20（CalculationBase）+ 已损失生命百分比 25→35（ExtraDamage，单位 %）。
    // CalculatedDamageVar 公式 = CalculationBase + ExtraDamage × multiplier（已损失生命/100）——
    // 卡面预览（UpdateCardPreview）与实际结算（AttackCommand.Execute 内 Calculate(null)）共用此公式。
    // multiplier 必须是静态 lambda（WithMultiplier 运行时强制），经 card 参数读当前 Creature 状态；
    // 战斗外 Owner.Creature 为 null → 返回 0（卡面只显示基础值）。
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CalculationBaseVar(14m).WithUpgradeTo(20),
        new ExtraDamageVar(25m).WithUpgradeTo(35),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier(static (card, _) =>
            card.Owner.Creature == null ? 0m : (card.Owner.Creature.MaxHp - card.Owner.Creature.CurrentHp) / 100m),
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
        if (!CombatState.HittableEnemies.Any())
        {
            MainFile.Logger.Warn("[荡平万邦] 没有可命中的敌人，伤害落空");
            return;
        }

        // 一次性全体攻击：DamageCmd.Attack(CalculatedDamageVar) → 执行时以 Calculate(null) 取值，
        // 与卡面预览显示的总伤同源。TargetingAllOpponents = 原版 AoE（一次攻击动画、全体同时结算）。
        // Props = Move（构造 CalculatedDamageVar 时声明）→ 受力量/血仇等伤害加成。
        await DamageCmd.Attack(DynamicVars.CalculatedDamage)
            .FromCard(this, cardPlay)
            .TargetingAllOpponents(CombatState)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);

        MainFile.Logger.Info($"[荡平万邦] 打出：对全体造成 {DynamicVars.CalculatedDamage.Calculate(null)} 伤" +
            $"（基础 {DynamicVars.CalculationBase.BaseValue} + 已损失生命 {creature.MaxHp - creature.CurrentHp}×{DynamicVars.ExtraDamage.BaseValue}%）");
    }
}
