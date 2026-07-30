using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace WandiMod.WandiModCode.Powers;

/// <summary>
/// 死亡拒绝 Power —— 下一张攻击牌造成伤害后，等量回血（UnblockedDamage 总和），然后自毁。
/// 参考原生 SuckPower.AfterAttack（汇总多目标 UnblockedDamage 的精确范式）。
/// 一次性 Power：AfterAttack 触发后立即 Remove 自身。
/// </summary>
public class DeathDenialPower : WandiModPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    /// <summary>
    /// 攻击结算后：汇总所有目标的 UnblockedDamage，等量回血，然后移除自身。
    /// 参考 SuckPower.AfterAttack。
    /// </summary>
    public override async Task AfterAttack(PlayerChoiceContext choiceContext, AttackCommand command)
    {
        if (command.Attacker != Owner) return;

        decimal totalDamage = 0;
        foreach (var results in command.Results)
            foreach (var r in results)
                totalDamage += r.UnblockedDamage;

        if (totalDamage > 0)
        {
            Flash();
            await CreatureCmd.Heal(Owner, totalDamage);
            MainFile.Logger.Info($"[死亡拒绝] 下一张攻击牌吸血触发：造成 {totalDamage} → 回血 {totalDamage}");
        }

        // 一次性：触发后自毁
        await PowerCmd.Remove(this);
    }
}
