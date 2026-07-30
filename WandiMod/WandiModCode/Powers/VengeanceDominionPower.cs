using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace WandiMod.WandiModCode.Powers;

/// <summary>
/// 血仇主宰 Power：血仇层数不会被消耗。
/// 全局拦截——override TryModifyPowerAmountReceived（参考 ArtifactPower 的减益拦截范式）。
/// 拦截所有对 VengeancePower 的负向 Apply（消耗），一处实现覆盖全部消耗卡 + 血仇自身满 8 触发。
/// </summary>
public class VengeanceDominionPower : WandiModPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    /// <summary>
    /// 全局拦截：当目标是万敌、被施加的是血仇、且 amount < 0（消耗）时，将消耗归零。
    /// 参考 ArtifactPower.TryModifyPowerAmountReceived（拦截减益）。
    /// </summary>
    public override bool TryModifyPowerAmountReceived(
        PowerModel canonicalPower, Creature target, decimal amount,
        Creature? applier, out decimal modifiedAmount)
    {
        modifiedAmount = amount;
        if (target != Owner) return false;          // 只保护万敌
        if (canonicalPower is not VengeancePower) return false; // 只拦截血仇
        if (amount >= 0) return false;              // 只拦截消耗（负值），不拦截获得

        modifiedAmount = 0m;  // 消耗归零，血仇保留
        Flash();
        MainFile.Logger.Info($"[血仇主宰] 拦截血仇消耗 {amount} → 0（血仇保留）");
        return true;
    }
}
