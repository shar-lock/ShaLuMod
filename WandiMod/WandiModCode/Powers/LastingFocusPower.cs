using MegaCrit.Sts2.Core.Entities.Cards;           // CardPlay
using MegaCrit.Sts2.Core.Entities.Creatures;       // Creature
using MegaCrit.Sts2.Core.Entities.Powers;          // PowerType / PowerStackType
using MegaCrit.Sts2.Core.Models;                   // CardModel
using MegaCrit.Sts2.Core.ValueProps;               // ValueProp

namespace WandiMod.WandiModCode.Powers;

/// <summary>
/// 战意持久 / Lasting Focus（万敌 · 能力 Power）。
/// 机制：生命值减少至生命上限的 50%，受到的伤害降低 20%（升级 30%）。
///   - 生命值设定：由卡牌 OnPlay 在授予本 Power 前用 CreatureCmd.SetCurrentHp 完成（卡牌持有 choiceContext）。
///   - 钩子 ModifyDamageMultiplicative：当万敌是伤害目标时返回 1 - Amount/100（Amount=20 → 0.80，即减伤 20%）。
/// 参考 VengeancePower.ModifyDamageMultiplicative（返回倍率语义）、原生 TankPower / PaperKrane（target==Owner 收伤过滤）。
/// 注意：ModifyDamageMultiplicative 默认返回 1（不变）；返回 0.7 = 伤害变 70%。
///   减伤对所有「以万敌为目标」的伤害生效（含自伤——自伤时 target 也是 Owner，会被一并打折；
///   设计未单独排除，保持统一语义；如需排除自伤可加 dealer != Owner 条件）。
/// </summary>
public class LastingFocusPower : WandiModPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;  // 形态类，不叠加

    /// <summary>
    /// 减伤：万敌作为伤害目标时，伤害 ×（1 - Amount/100）。其余返回 1（不影响打出的伤害）。
    /// </summary>
    public override decimal ModifyDamageMultiplicative(
        Creature? target,
        decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource,
        CardPlay? cardPlay)
    {
        // 仅减免打到万敌身上的伤害（target == Owner）
        if (target != Owner)
            return 1m;

        // Amount=20 → 0.80；Amount=30 → 0.70
        return 1m - Amount / 100m;
    }
}
