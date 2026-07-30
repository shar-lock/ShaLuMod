using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using WandiMod.WandiModCode.Character;
using WandiMod.WandiModCode.Extensions;

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 焚天 / Heavenburn（稀有 · 攻击 · 全体）。失8血，造成「力量×3+12」全体伤 / ×4+18。
/// 伤害用 CalculatedDamageVar（荡平万邦同款写法）：
///   公式 = CalculationBase(12→18) + ExtraDamage(3→4) × multiplier（当前力量），
///   卡面伤害数字/描述 {CalculatedDamage:diff()} 实时显示含力量的总伤，且与 OnPlay 结算同源——
///   修复旧版「IntVar 手算 + 描述硬编码」导致卡面不显示伤害的问题。
/// multiplier 必须是静态 lambda（WithMultiplier 运行时强制），经 card 参数读当前 Creature 状态；
/// 战斗外 Owner.Creature 为 null → 返回 0（卡面只显示基础值）。
/// </summary>
public class Heavenburn : WandiModCard
{
    public Heavenburn() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies) { }
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new HpLossVar(8),
        new CalculationBaseVar(12m).WithUpgradeTo(18),
        new ExtraDamageVar(3m).WithUpgradeTo(4),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier(static (card, _) =>
            card.Owner.Creature?.GetPower<StrengthPower>()?.Amount ?? 0m),
    ];
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var c = Owner.Creature;
        if (c == null || CombatState == null) return;
        // 自伤
        await CreatureCmd.Damage(choiceContext, c, DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this, cardPlay);
        // 力量缩放 AoE：DamageCmd.Attack(CalculatedDamageVar) → 执行时以 Calculate(null) 取值，与卡面预览同源
        await DamageCmd.Attack(DynamicVars.CalculatedDamage).FromCard(this, cardPlay).TargetingAllOpponents(CombatState).Execute(choiceContext);
        MainFile.Logger.Info($"[焚天] 失血+全体 {DynamicVars.CalculatedDamage.Calculate(null)}（基础 {DynamicVars.CalculationBase.BaseValue} + 力量 {c.GetPower<StrengthPower>()?.Amount ?? 0}×{DynamicVars.ExtraDamage.BaseValue}）");
    }
}
