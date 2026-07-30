using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace WandiMod.WandiModCode.Powers;

/// <summary>
/// 饮血反击·精确吸血 Power：攻击结算后汇总所有目标的 UnblockedDamage，按 Amount% 回血。
/// 参考 SuckPower.AfterAttack（汇总多目标伤害的精确范式）。
/// Amount = 吸血百分比（30 / 40）。
/// </summary>
public class BloodDrinkCounterPower : WandiModPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    /// <summary>
    /// 攻击结算后：汇总所有目标的 UnblockedDamage，按 Amount% 回血。
    /// 参考 SuckPower.AfterAttack。
    /// </summary>
    public override async Task AfterAttack(PlayerChoiceContext choiceContext, AttackCommand command)
    {
        if (command.Attacker != Owner) return;

        decimal totalDamage = 0;
        foreach (var results in command.Results)
            foreach (var r in results)
                totalDamage += r.UnblockedDamage;

        if (totalDamage <= 0) return;

        decimal heal = totalDamage * Amount / 100m;
        Flash();
        await CreatureCmd.Heal(Owner, heal);
        MainFile.Logger.Info($"[饮血反击] 精确吸血：造成 {totalDamage} → 回血 {heal}（{Amount}%）");
    }
}
