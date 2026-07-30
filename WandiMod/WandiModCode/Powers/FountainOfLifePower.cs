using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Combat;

namespace WandiMod.WandiModCode.Powers;

/// <summary>生命之泉 Power：每回合开始获得「当前最大生命」Amount% 的纷争。</summary>
public class FountainOfLifePower : WandiModPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterSideTurnStart(CombatSide side, System.Collections.Generic.IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (!participants.Contains(Owner)) return;
        int strife = (int)(Owner.MaxHp * (decimal)Amount / 100m);
        await StrifePower.Grant(new ThrowingPlayerChoiceContext(), Owner, strife, Owner, null);
        MainFile.Logger.Info($"[生命之泉] 回合开始：{strife} 纷争（MaxHp {Owner.MaxHp}×{Amount}%）");
    }
}
