using BaseLib.Extensions;                           // WithUpgrade / WithValueProp
using MegaCrit.Sts2.Core.Commands;                  // DamageCmd
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // DamageVar / IntVar
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using WandiMod.WandiModCode.Character;              // WandiModCard

using WandiMod.WandiModCode.Extensions;  // WithUpgradeTo（升级目标值语义）

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 绝命突 / Desperate Thrust（普通 · 攻击）
/// 造成 8 伤害；HP &lt; 50% 时额外 +4 伤害。升级：11 伤害，+6。
/// —— 残血流基础件：HP 阈值触发（原版完全空白的轴）。手动算总伤（基础 + 阈值奖励）。
/// </summary>
public class DesperateThrust : WandiModCard
{
    public DesperateThrust() : base(
        cost: 1,
        type: CardType.Attack,
        rarity: CardRarity.Common,
        target: TargetType.AnyEnemy)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(8, ValueProp.Move).WithUpgradeTo(11),
        new IntVar("Bonus", 4).WithUpgradeTo(6),   // HP<50% 时的额外伤害
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target == null)
        {
            MainFile.Logger.Error("[绝命突] OnPlay 时目标为空，效果未触发");
            return;
        }
        var creature = Owner.Creature;
        if (creature == null)
        {
            MainFile.Logger.Error("[绝命突] OnPlay 时 Owner.Creature 为空，无法判 HP 阈值");
            return;
        }

        decimal dmg = DynamicVars.Damage.BaseValue;
        // HP < 50% 最大生命 → 加额外伤害
        bool lowHp = creature.CurrentHp < creature.MaxHp * 0.5m;
        if (lowHp)
            dmg += DynamicVars["Bonus"].IntValue;

        await DamageCmd.Attack(dmg)
            .FromCard(this, cardPlay)
            .Targeting(cardPlay.Target)
            .WithValueProp(ValueProp.Move)
            .Execute(choiceContext);

        if (lowHp)
            MainFile.Logger.Info($"[绝命突] HP<50% 触发额外伤害，总伤 {dmg}");
    }
}
