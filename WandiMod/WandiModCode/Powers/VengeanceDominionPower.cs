using MegaCrit.Sts2.Core.Entities.Powers;

namespace WandiMod.WandiModCode.Powers;

/// <summary>
/// 血仇主宰 Power：血仇层数不会被消耗。
/// 纯标记 Power——不做任何 hook。各消耗血仇的卡牌在 OnPlay 中检查 GetPower&lt;VengeanceDominionPower&gt;() 是否存在，
/// 存在则跳过 PowerCmd.Apply&lt;VengeancePower&gt;(负值) 消耗。
/// // TODO: 各消耗血仇的卡牌（连刺/弑王枪/守誓/涅槃/复仇心 等）需加此���查。
/// </summary>
public class VengeanceDominionPower : WandiModPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
}
