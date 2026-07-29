using MegaCrit.Sts2.Core.Entities.Creatures;       // Creature
using MegaCrit.Sts2.Core.Entities.Powers;          // PowerType / PowerStackType
using MegaCrit.Sts2.Core.Entities.Cards;           // CardPlay
using MegaCrit.Sts2.Core.Models;                   // CardModel
using MegaCrit.Sts2.Core.ValueProps;               // ValueProp
using WandiMod.WandiModCode.Cards;                 // ConquerAllLands（类型判定）

namespace WandiMod.WandiModCode.Powers;

/// <summary>
/// 王之意志 / Spirit of King（万敌 · 能力 Power，固有）。
/// 机制：[荡平万邦]的伤害额外提高 20%（升级 25%）。
///   - 钩子 ModifyDamageMultiplicative：当万敌本人打出「荡平万邦」时，对该牌的伤害返回 1 + Amount/100。
///   - Amount 存放百分比（20 或 25），由卡牌 OnPlay 按升级态传入。
/// 与 VengeancePower 的 +2%/层伤害放大叠加（乘法链上各自返回倍率，引擎连乘）。
/// 参考 VengeancePower.ModifyDamageMultiplicative（dealer==Owner 过滤）、原生 PenNib / PaperPhrog（cardSource 类型过滤）。
/// </summary>
public class SpiritOfKingPower : WandiModPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;  // 形态类，不叠加

    /// <summary>
    /// 放大荡平万邦的伤害。仅当「万敌本人打出荡平万邦」时生效。
    /// 返回 1 + Amount/100（例如 Amount=20 → 1.20）；其余情况返回 1（不变）。
    /// </summary>
    public override decimal ModifyDamageMultiplicative(
        Creature? target,
        decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource,
        CardPlay? cardPlay)
    {
        // 仅万敌本人打出的「荡平万邦」放大（cardSource 类型 + dealer 双重过滤）
        if (dealer != Owner || cardSource is not ConquerAllLands)
            return 1m;

        return 1m + Amount / 100m;
    }
}
