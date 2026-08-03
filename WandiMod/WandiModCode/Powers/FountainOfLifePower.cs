using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace WandiMod.WandiModCode.Powers;

/// <summary>
/// 生命之泉 Power：每回合开始获得「基础最大生命」Amount% 的纷争。
/// 基础最大生命 = 当前上限 − 已有纷争层（= 入场基数，含永久提升、不含纷争膨胀），避免按含纷争上限自我增殖。
/// </summary>
public class FountainOfLifePower : WandiModPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterSideTurnStart(CombatSide side, System.Collections.Generic.IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (!participants.Contains(Owner)) return;

        // 基础上限 = MaxHp − 纷争；与 StrifePower 积累上限同口径，不含临时膨胀
        int strifeStacks = Owner.GetPower<StrifePower>()?.Amount ?? 0;
        int baseMaxHp = Math.Max(1, Owner.MaxHp - strifeStacks);
        int strife = (int)(baseMaxHp * (decimal)Amount / 100m);
        if (strife <= 0) return;

        await StrifePower.Grant(new ThrowingPlayerChoiceContext(), Owner, strife, Owner, null);
        MainFile.Logger.Info($"[生命之泉] 回合开始：{strife} 纷争（基础 MaxHp {baseMaxHp}×{Amount}%，当前上限 {Owner.MaxHp}）");
    }
}
