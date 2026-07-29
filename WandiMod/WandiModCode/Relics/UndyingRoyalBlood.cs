using MegaCrit.Sts2.Core.Combat;                  // CombatSide / ICombatState
using MegaCrit.Sts2.Core.Commands;                // PowerCmd
using MegaCrit.Sts2.Core.Entities.Creatures;      // Creature
using MegaCrit.Sts2.Core.Entities.Relics;         // RelicRarity
using MegaCrit.Sts2.Core.GameActions.Multiplayer; // ThrowingPlayerChoiceContext
using MegaCrit.Sts2.Core.Models;                  // RelicModel
using WandiMod.WandiModCode.Powers;               // VengeancePower

namespace WandiMod.WandiModCode.Relics;

/// <summary>
/// 不灭王血 / Undying Royal Blood —— 弑亲血脉的先古（觉醒）版本，经先古之民欧洛巴斯的「欧洛巴斯之触」替换而得。
/// 继承弑亲血脉的全部效果（战斗开始赋予血仇、4 次免死回 30%、血仇≥8 生成荡平万邦），并强化：
///   ① 每回合开始额外 +1 血仇（下方 AfterSideTurnStart）；
///   ② 血仇≥8 触发生成的「荡平万邦」为升级版——由 VengeancePower 的触发处查本遗物是否在场，
///      在场则对新建可变实例调 CardCmd.Upgrade（原版 Jackpot/ManifestAuthority 写法，已落地）。
/// 不进普通遗物池（只能经欧洛巴斯替换获得）。
/// </summary>
public class UndyingRoyalBlood : BloodOfTheKinslayer
{
    // 先古稀有度（仅经欧洛巴斯之触获得）
    public override RelicRarity Rarity => RelicRarity.Ancient;

    // 已是最高阶，不再有升级替换（避免循环）
    public override RelicModel? GetUpgradeReplacement() => null;

    /// <summary>
    /// 每回合开始（轮到万敌时）额外 +1 血仇。参考原生 Akabeko.AfterSideTurnStart。
    /// 与 Akabeko 不同：这里每个自己的回合都触发（Akabeko 仅首回合），持续强化血仇引擎。
    /// </summary>
    public override async Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        // participants 含万敌本人 = 轮到万敌这一侧的回合
        if (!participants.Contains(Owner.Creature))
            return;

        Flash();
        await PowerCmd.Apply<VengeancePower>(
            new ThrowingPlayerChoiceContext(),
            Owner.Creature,
            1,
            Owner.Creature,
            null);

        MainFile.Logger.Info("[不灭王血] 万敌回合开始：额外 +1 血仇");
    }
}
