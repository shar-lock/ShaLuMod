using MegaCrit.Sts2.Core.Entities.Cards;            // CardPlay
using MegaCrit.Sts2.Core.Entities.Creatures;        // Creature
using MegaCrit.Sts2.Core.Entities.Powers;           // PowerType / PowerStackType
using MegaCrit.Sts2.Core.Models;                    // CardModel
using MegaCrit.Sts2.Core.ValueProps;                // ValueProp
using WandiMod.WandiModCode.Cards;                  // ConquerAllLands（倍率只作用于荡平万邦）

namespace WandiMod.WandiModCode.Powers;

/// <summary>
/// 浴血奋战 Power：荡平万邦只攻击到一个敌人时，总伤害提高 Amount%（50=×1.5，100=×2）。
/// 实现走 ModifyDamageMultiplicative（与 SpiritOfKingPower 同钩、可连乘叠加），
/// 直接放大结算总伤——旧版在 ConquerAllLands.OnPlay 里只放大「已损生命加成」分量，
/// 基础伤和血仇/王之意志的乘区都吃不到，与设计「总伤×1.5/×2」不符，已废弃。
/// 单体判定用 target.CombatState.HittableEnemies.Count <= 1（荡平万邦打出时的快照）。
/// </summary>
public class BloodbathPower : WandiModPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override decimal ModifyDamageMultiplicative(
        Creature? target,
        decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource,
        CardPlay? cardPlay)
    {
        if (dealer != Owner || cardSource is not ConquerAllLands)
            return 1m;

        int enemies = target?.CombatState?.HittableEnemies.Count ?? 0;
        if (enemies > 1)
            return 1m;

        // Amount = 提升百分比（50 → ×1.5，100 → ×2）
        return 1m + Amount / 100m;
    }
}
