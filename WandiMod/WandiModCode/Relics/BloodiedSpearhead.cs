using MegaCrit.Sts2.Core.Commands;                 // CreatureCmd
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using WandiMod.WandiModCode.Powers;

namespace WandiMod.WandiModCode.Relics;

/// <summary>
/// 沾血枪尖 / Bloodied Spearhead（普通遗物）
/// 当你获得【血仇】时，对随机一名敌人造成 3 点伤害。
/// —— 每次血仇正向增加（无论+1还是+3）只触发一次：对随机单体敌人打 3 伤。
///   钩子 AfterPowerAmountChanged：监听 VengeancePower 正向变化。
/// </summary>
public class BloodiedSpearhead : WandiModRelic
{
    public override RelicRarity Rarity => RelicRarity.Common;

    public override async Task AfterPowerAmountChanged(
        PlayerChoiceContext choiceContext,
        PowerModel power,
        decimal amount,
        Creature? applier,
        CardModel? cardSource)
    {
        // 仅血仇正向变化（获得血仇）——一次 PowerCmd.Apply 无论 +1 还是 +3 都只触发一次
        if (power is not VengeancePower || amount <= 0) return;
        if (Owner?.Creature == null) return;

        var combatState = Owner.Creature.CombatState;
        if (combatState == null) return;

        // 随机选一名可命中敌人
        var enemies = combatState.HittableEnemies.Where(e => e != null && e.IsAlive).ToList();
        if (enemies.Count == 0) return;

        Flash();
        // 必须用局内 CombatTargets RNG（对齐 Tingsha / ParryingShield），禁止 Random.Shared（联机不同步）
        var target = Owner.RunState.Rng.CombatTargets.NextItem(enemies);
        if (target == null) return;
        await CreatureCmd.Damage(choiceContext, target, 3m, ValueProp.Move, null, null);
        MainFile.Logger.Info($"[沾血枪尖] 获得血仇 → 随机敌人 {target} 受 3 伤害");
    }
}
