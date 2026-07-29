using BaseLib.Extensions;                          // WithUpgrade（增量语义，本类对其包装）
using MegaCrit.Sts2.Core.Localization.DynamicVars;  // DynamicVar

namespace WandiMod.WandiModCode.Extensions;

/// <summary>
/// DynamicVar 升级扩展（全 mod 统一使用本类，勿直接用 BaseLib 的 WithUpgrade）。
/// ⚠️ 重点：BaseLib 的 WithUpgrade(x) 是【增量】语义——升级时 BaseValue += x
/// （BaseLib DynamicVarExtensions.WithUpgrade → 游戏 DynamicVar.UpgradeValueBy 是 +=）。
/// 直接写目标值会双重叠加（如 DamageVar(6).WithUpgrade(9) → 升级后 15，不是 9！）。
/// 本类 WithUpgradeTo(x) 是【目标值】语义：升级后 BaseValue == x（内部换算成增量 x − 基础值），
/// 与设计文档「基础值 / 升级值」写法一致。
/// </summary>
public static class WandiDynamicVarExtensions
{
    /// <summary>
    /// 设定升级后的目标值：升级后该变量的 BaseValue 变为 <paramref name="targetValue"/>。
    /// 支持降值（如血仇消耗 2→1，内部自动换算为负增量）。
    /// </summary>
    /// <param name="dynamicVar">目标 DynamicVar（DamageVar / IntVar / BlockVar / CalculationBaseVar / ExtraDamageVar 等）。</param>
    /// <param name="targetValue">升级后的目标值（<b>不是增量</b>）。</param>
    public static TDynamicVar WithUpgradeTo<TDynamicVar>(this TDynamicVar dynamicVar, decimal targetValue)
        where TDynamicVar : DynamicVar
    {
        return dynamicVar.WithUpgrade(targetValue - dynamicVar.BaseValue);
    }
}
