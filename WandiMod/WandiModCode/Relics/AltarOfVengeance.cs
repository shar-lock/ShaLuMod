using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace WandiMod.WandiModCode.Relics;

/// <summary>
/// 血仇圣坛 / Altar of Vengeance（稀有遗物）
/// 敌方单位死亡后恢复 5% 最大生命值的血量。
/// —— 卖血流的续航：击杀按最大生命比例回血（含纷争上限），让「敢卖血」可持续。
/// 钩子 AfterDeath（AbstractModel.cs:313），每次任意生物死亡时分发到所有模型。
/// 参考原生 BookRepairKnife.AfterDiedToDoom（监听死亡→回血）。
/// </summary>
public class AltarOfVengeance : WandiModRelic
{
    public override RelicRarity Rarity => RelicRarity.Rare;

    /// <summary>
    /// 任意生物死亡后触发。仅当死者是敌方（非自己同侧）时回血。
    /// 参考原生 BookRepairKnife.AfterDiedToDoom 的「c != Owner.Creature」过滤。
    /// </summary>
    public override async Task AfterDeath(
        PlayerChoiceContext choiceContext,
        Creature creature,
        bool wasRemovalPrevented,
        float deathAnimLength)
    {
        // 死者是敌方单位（非万敌同侧）
        if (creature == Owner.Creature || creature.Side == Owner.Creature.Side)
            return;

        // 按「当前最大生命（含纷争）」5% 回血
        decimal heal = Owner.Creature.MaxHp * 0.05m;
        Flash();
        await CreatureCmd.Heal(Owner.Creature, heal);
        MainFile.Logger.Info($"[血仇圣坛] 敌方死亡 → 回血 {heal}（MaxHp {Owner.Creature.MaxHp}×2%）");
    }
}
