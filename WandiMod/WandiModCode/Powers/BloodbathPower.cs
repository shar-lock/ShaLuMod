using MegaCrit.Sts2.Core.Entities.Powers;

namespace WandiMod.WandiModCode.Powers;

/// <summary>
/// 浴血奋战 Power：荡平万邦只攻击到一个敌人时伤害倍率。
/// 纯标记 Power——ConquerAllLands.OnPlay 检查本 Power 是否存在 + 敌人数量==1 时乘以 Amount/100。
/// Amount 存整数百分比（150=1.5×, 200=2×）。
/// </summary>
public class BloodbathPower : WandiModPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
}
