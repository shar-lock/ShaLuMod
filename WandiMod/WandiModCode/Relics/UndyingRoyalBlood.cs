using MegaCrit.Sts2.Core.Entities.Relics;   // RelicRarity
using MegaCrit.Sts2.Core.Models;             // RelicModel

namespace WandiMod.WandiModCode.Relics;

/// <summary>
/// 不灭王血 / Undying Royal Blood —— 弑亲血脉的先古（觉醒）版本，经先古之民欧洛巴斯的「欧洛巴斯之触」替换而得。
/// 继承弑亲血脉的全部效果（战斗开始赋予血仇、4 次免死回 30%、血仇≥7 生成荡平万邦），并强化：
///   ① 每回合开始额外 +1 血仇（M2.4：AfterSideTurnStart）；
///   ② 血仇≥7 触发生成的「荡平万邦」为升级版（M2.4：触发处查本遗物是否在场）。
/// 不进普通遗物池（只能经欧洛巴斯替换获得）。
/// </summary>
public class UndyingRoyalBlood : BloodOfTheKinslayer
{
    // 先古稀有度（仅经欧洛巴斯之触获得）
    public override RelicRarity Rarity => RelicRarity.Ancient;

    // 已是最高阶，不再有升级替换（避免循环）
    public override RelicModel? GetUpgradeReplacement() => null;

    // TODO(M2.4): 重写 AfterSideTurnStart —— 每回合开始 +1 血仇（PowerCmd.Apply<VengeancePower>）
    // TODO(M2.4): 与 VengeancePower 的 7 层触发配合——本遗物在场时，生成的荡平万邦为升级版
}
