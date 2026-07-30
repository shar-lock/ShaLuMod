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
/// 每当你因**打出的卡牌**失去生命，获得 1 点【血仇】。
/// —— 被动收益件：自伤卡额外触发血仇，让卖血卡越打血仇涨得越快。
/// 触发口径：限定 cardSource != null（因卡牌失血），区别于起手遗物的「任意来源失血」。
/// 参考原生 DemonTongue.AfterDamageReceived（遗物监听失血的标准范式）。
/// </summary>
public class BloodiedSpearhead : WandiModRelic
{
    public override RelicRarity Rarity => RelicRarity.Common;

    /// <summary>
    /// 失血回调：仅当因卡牌效果失血（cardSource != null）时，+1 血仇。
    /// 参考原生 DemonTongue / BeatingRemnant。
    /// </summary>
    public override async Task AfterDamageReceived(
        PlayerChoiceContext choiceContext,
        Creature target,
        DamageResult result,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        // 仅万敌本人 + 实际掉血 + 因卡牌效果（非受击）
        if (target != Owner.Creature || result.UnblockedDamage <= 0 || cardSource == null)
            return;

        Flash();
        await VengeancePower.Grant(choiceContext, Owner.Creature, 1, null);
        MainFile.Logger.Info("[沾血枪尖] 因卡牌失血 → +1 血仇");
    }
}
