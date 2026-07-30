using MegaCrit.Sts2.Core.Commands;                 // CardCmd / CardPileCmd
using MegaCrit.Sts2.Core.Entities.Cards;           // CardModel / PileType
using MegaCrit.Sts2.Core.Entities.Relics;          // RelicRarity
using MegaCrit.Sts2.Core.HoverTips;               // IHoverTip / HoverTipFactory（悬停预览荡平万邦）
using MegaCrit.Sts2.Core.Models;                  // CardModel
using WandiMod.WandiModCode.Cards;                // ConquerAllLands / WandiModKeywords

namespace WandiMod.WandiModCode.Relics;

/// <summary>
/// 深红契印 / Crimson Sigil —— 万敌商店遗物（Shop 稀有度：只在商店出售，不进掉落池）。
/// 战斗开始时，获得 1 张【荡平万邦】到手牌；若持有觉醒起手遗物「不灭王血」则获得升级版。
/// —— 机制件（强度对标罕见遗物）：直接发一张血仇引擎的终结技，让「荡平万邦流」开局就有
///    终结手段/触发器——与血祭之枪（产层→触发）路线互补，也可当泛用的 0 费 AoE 终结件。
/// 实现：BeforeCombatStart（弑亲血脉同钩）；CombatState.CreateCard 造战斗内可变实例
/// （规范实例直接进手牌会抛 CanonicalModelException——血仇 Power 触发同范式）；
/// 不灭王血在场 → CardCmd.Upgrade 就地升级（血仇 Power 触发同款觉醒判定）。
/// </summary>
public class CrimsonSigil : WandiModRelic
{
    // Shop 稀有度：只会在商店出售，不进普通奖励掉落池
    public override RelicRarity Rarity => RelicRarity.Shop;

    /// <summary>悬停提示：预览【荡平万邦】卡牌（与弑亲血脉同款 FromCard 写法）。</summary>
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromCard<ConquerAllLands>(),
    ];

    /// <summary>
    /// 战斗开始：造 1 张荡平万邦入手牌（不灭王血在场 → 升级版）。
    /// </summary>
    public override async Task BeforeCombatStart()
    {
        // 遗物的 Owner 是 Player；CombatState 挂在 Creature 上（PowerModel.CombatState 同款链路）
        var state = Owner.Creature?.CombatState;
        if (state == null)
        {
            MainFile.Logger.Error("[深红契印] BeforeCombatStart 时 CombatState 为空，荡平万邦未生成");
            return;
        }

        Flash();
        CardModel card = state.CreateCard<ConquerAllLands>(Owner);
        // 觉醒遗物「不灭王血」在场 → 生成升级版（与血仇 Power 触发判定一致）
        bool awakened = Owner.GetRelic<UndyingRoyalBlood>() != null;
        if (awakened)
            CardCmd.Upgrade(card);
        await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner);

        MainFile.Logger.Info($"[深红契印] 战斗开始：获得荡平万邦（觉醒={awakened}）");
    }
}
