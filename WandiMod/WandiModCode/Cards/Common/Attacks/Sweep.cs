using BaseLib.Extensions;                           // WithUpgrade 扩展方法
using MegaCrit.Sts2.Core.Commands;                 // DamageCmd（AoE 建造器）
using MegaCrit.Sts2.Core.Commands.Builders;         // AttackCommand 建造器扩展
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // DamageVar
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp

using WandiMod.WandiModCode.Extensions;  // WithUpgradeTo（升级目标值语义）

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 横扫 / Sweep（普通 · 攻击 · AoE）
/// 对所有敌人造成 8 点伤害。升级：12 伤害。
/// —— AoE 写法参考原版 Stomp：DamageCmd.Attack(...).FromCard(...).TargetingAllOpponents(CombatState)。
/// </summary>
public class Sweep : WandiModCard
{
    public Sweep() : base(
        cost: 1,
        type: CardType.Attack,
        rarity: CardRarity.Common,
        target: TargetType.AllEnemies)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(8, ValueProp.Move).WithUpgradeTo(12),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        // 全体攻击（原版 Stomp 的 DamageCmd 建造器写法；TargetType.AllEnemies 决定选牌 UI 无需点目标）
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this, cardPlay)
            .TargetingAllOpponents(CombatState)
            .Execute(choiceContext);
    }
}
