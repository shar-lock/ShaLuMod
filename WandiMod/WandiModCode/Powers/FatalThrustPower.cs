using MegaCrit.Sts2.Core.Models;           // AbstractModel / ModelDb
using MegaCrit.Sts2.Core.Models.Powers;    // TemporaryStrengthPower（原生临时力量基类）
using WandiMod.WandiModCode.Cards;          // FatalThrust（来源卡牌）

namespace WandiMod.WandiModCode.Powers;

/// <summary>
/// 致命突刺的临时降力量 Power —— 继承原生 TemporaryStrengthPower。
/// IsPositive=false → Sign=-1 → 施加时减力量；目标回合结束自动恢复（AfterSideTurnEnd 反转）。
/// 参考原生 PiercingWailPower（尖啸）。
/// </summary>
public class FatalThrustPower : TemporaryStrengthPower
{
    /// <summary>来源卡牌（Power 名称/tooltip 显示用）。</summary>
    public override AbstractModel OriginModel => ModelDb.Card<FatalThrust>();

    /// <summary>负面效果（减力量）。</summary>
    protected override bool IsPositive => false;
}
