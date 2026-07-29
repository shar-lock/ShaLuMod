using BaseLib.Extensions;                           // WithUpgrade / WithValueProp
using MegaCrit.Sts2.Core.Commands;                  // DamageCmd（手动伤害，便于条件加成）
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // DamageVar / IntVar
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using WandiMod.WandiModCode.Character;              // WandiModCard

using WandiMod.WandiModCode.Extensions;  // WithUpgradeTo（升级目标值语义）

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 绝杀 / Execute（罕见 · 攻击）
/// 造成 14 点伤害；目标 HP &lt;= 30% 最大生命时，额外 +8 伤害。升级：20 伤害，+12。
/// —— 斩杀件：DesperateThrust 的镜像版（读「目标」残血而非「自身」残血）。
///    手动算总伤，用 DamageCmd 单次结算。
/// </summary>
public class Execute : WandiModCard
{
    public Execute() : base(
        cost: 2,
        type: CardType.Attack,
        rarity: CardRarity.Uncommon,
        target: TargetType.AnyEnemy)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(14, ValueProp.Move).WithUpgradeTo(20),
        new IntVar("Bonus", 8).WithUpgradeTo(12),     // 目标 HP<=30% 时的额外伤害
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target == null)
        {
            MainFile.Logger.Error("[绝杀] OnPlay 时目标为空，效果未触发");
            return;
        }

        decimal dmg = DynamicVars.Damage.BaseValue;
        // 目标 HP <= 30% 最大生命 → 触发斩杀奖励
        bool lowHp = cardPlay.Target.CurrentHp <= cardPlay.Target.MaxHp * 0.3m;
        if (lowHp)
            dmg += DynamicVars["Bonus"].IntValue;

        await DamageCmd.Attack(dmg)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithValueProp(ValueProp.Move)
            .Execute(choiceContext);

        if (lowHp)
            MainFile.Logger.Info($"[绝杀] 目标 HP<=30%，触发斩杀奖励，总伤 {dmg}");
    }
}
