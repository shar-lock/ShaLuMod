using BaseLib.Extensions;                           // WithUpgrade
using BaseLib.Utils;                                // CommonActions
using MegaCrit.Sts2.Core.Commands;                  // CreatureCmd（回血）
using MegaCrit.Sts2.Core.GameActions.Multiplayer;   // PlayerChoiceContext
using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay / CardType / CardRarity / TargetType
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // DamageVar / IntVar
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using WandiMod.WandiModCode.Character;              // WandiModCard
using WandiMod.WandiModCode.Extensions;             // WithUpgradeTo

namespace WandiMod.WandiModCode.Cards;

/// <summary>
/// 饮血反击 / Blood-Drink Counter（罕见 · 攻击）
/// 造成 8 点伤害，回复造成伤害的 30%。升级：11 伤害，40% 吸血。
/// —— 吸血件：攻击后直接读 AttackCommand.Results 的实际 UnblockedDamage 汇总回血。
///    不再走 Power 中转，一次结算、代码更简。
/// </summary>
public class BloodDrinkCounter : WandiModCard
{
    public BloodDrinkCounter() : base(
        cost: 1,
        type: CardType.Attack,
        rarity: CardRarity.Uncommon,
        target: TargetType.AnyEnemy)
    {
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(8, ValueProp.Move).WithUpgradeTo(11),
        new IntVar("LifestealPct", 30).WithUpgradeTo(40),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var creature = Owner.Creature;
        if (creature == null)
        {
            MainFile.Logger.Error("[饮血反击] OnPlay 时 Owner.Creature 为空，吸血未触发");
            return;
        }

        // ① 攻击，取实际伤害结果（参考 DoomVerdict 击杀判定 / SuckPower 汇总范式）
        var executed = await CommonActions.CardAttack(this, cardPlay).Execute(choiceContext);

        // ② 汇总实际造成的 UnblockedDamage（减去格挡后的真扣血量）
        decimal totalDamage = 0;
        foreach (var results in executed.Results)
            foreach (var r in results)
                totalDamage += r.UnblockedDamage;

        // ③ 按比例回血
        if (totalDamage > 0)
        {
            int pct = DynamicVars["LifestealPct"].IntValue;
            decimal heal = totalDamage * pct / 100m;
            await CreatureCmd.Heal(creature, heal);
            MainFile.Logger.Info($"[饮血反击] 造成 {totalDamage} 伤害 → 吸血 {heal}（{pct}%）");
        }
    }
}
