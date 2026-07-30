using BaseLib.Extensions;                           // PlayerExtensions（GetRelic）
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using WandiMod.WandiModCode.Powers;

namespace WandiMod.WandiModCode.Relics;

/// <summary>
/// 焚血勋章 / Bloodburn Emblem（罕见遗物）
/// 每当你回复血量，获得 1 点【血仇】。
/// —— 回血双通道产血仇：纷争获得（抬上限=回血）/ 直接回血卡 / 吸血 都触发。
/// 让纷争流拿到它等于白捡一座血仇引擎。
///
/// 实现说明：StS2 无原生 AfterHeal 钩子（AbstractModel 上无此 virtual 方法），
/// 故用 Harmony Postfix 拦截 CreatureCmd.Heal——所有回血必经此方法。
/// 参考 BaseLib ModifyHealAmountPatches（同样 patch CreatureCmd.Heal）。
/// </summary>
public class BloodburnEmblem : WandiModRelic
{
    public override RelicRarity Rarity => RelicRarity.Uncommon;
}

/// <summary>
/// Harmony 补丁：CreatureCmd.Heal 后分发焚血勋章效果。
/// 所有回血（纷争/卡牌/Power/圣坛/遗物）都走 CreatureCmd.Heal，一处拦截全覆盖。
/// </summary>
[HarmonyPatch(typeof(CreatureCmd), nameof(CreatureCmd.Heal))]
static class BloodburnEmblemHealPatch
{
    [HarmonyPostfix]
    static void Postfix(Creature creature, decimal amount)
    {
        if (amount <= 0 || creature?.Player == null) return;

        // 查该玩家是否有焚血勋章
        var relic = creature.Player.GetRelic<BloodburnEmblem>();
        if (relic == null) return;

        relic.Flash();
        // 异步授予血仇（Harmony Postfix 不能 await，用 fire-and-forget）
        _ = VengeancePower.Grant(new ThrowingPlayerChoiceContext(), creature, 1, null);
        MainFile.Logger.Info("[焚血勋章] 回血触发 → +1 血仇");
    }
}
