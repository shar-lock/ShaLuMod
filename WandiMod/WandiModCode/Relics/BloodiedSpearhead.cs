using MegaCrit.Sts2.Core.Commands;                 // CreatureCmd / DamageCmd
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
/// 当你获得【血仇】时，对所有敌人造成 1 点伤害。
/// —— 血仇伤害被动：每次血仇正向增加 → 全体敌人各受 1 点伤害（与血仇引擎联动：挨打→+1血仇→全体1伤）。
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
        // 仅血仇正向变化（获得血仇）
        if (power is not VengeancePower || amount <= 0) return;
        if (Owner?.Creature == null) return;

        Flash();
        // 对所有可命中敌人造成 1 点伤害
        var combatState = Owner.Creature.CombatState;
        if (combatState == null) return;
        foreach (var enemy in combatState.HittableEnemies)
        {
            await CreatureCmd.Damage(choiceContext, enemy, 1m, ValueProp.Move, null, null);
        }
        MainFile.Logger.Info("[沾血枪尖] 获得血仇 → 对全体敌人造成 1 伤害");
    }
}
