using MegaCrit.Sts2.Core.Combat;                   // CombatSide / ICombatState
using MegaCrit.Sts2.Core.Entities.Creatures;       // Creature
using MegaCrit.Sts2.Core.Entities.Powers;          // PowerType / PowerStackType
using MegaCrit.Sts2.Core.GameActions.Multiplayer;  // PlayerChoiceContext / DamageResult
using MegaCrit.Sts2.Core.Models;                   // CardModel
using MegaCrit.Sts2.Core.ValueProps;               // ValueProp

namespace WandiMod.WandiModCode.Powers;

/// <summary>
/// 嗜血形态 / Bloodthirst Form（万敌 · 能力 Power）。
/// 机制：HP ≤ 50% 时，攻击造成伤害获得 1 血仇（每回合 ≤ 3 次，升级 ≤ 4 次）。
///   - 钩子 AfterDamageReceived：万敌本人打出受力量加成的攻击且实际造成伤害（dealer==Owner），
///     且当前生命 ≤ 上限 50% 时，+1 血仇，本回合计数 +1，达上限则停止。
///   - 钩子 AfterSideTurnStart：每回合开始重置计数。
///   - Amount = 每回合上限（3 或 4，由卡牌按升级态传入）。
/// 与 VengeancePower 的「失血叠层」独立——这里是「打伤害产血仇」的新通道，故显式调用 VengeancePower.Grant。
/// 参考 VengeancePower.AfterDamageReceived（dealer==Owner 判定「自己打出的伤害」）、DemonFormPower.AfterSideTurnStart。
/// 注意：AfterDamageReceived 钩子分发给战斗中所有模型的监听者（不限于受击者），故 dealer==Owner 过滤有效。
/// </summary>
public class BloodthirstFormPower : WandiModPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;  // 形态类，不叠加

    /// <summary>本回合已通过本 Power 产出的血仇次数。AfterSideTurnStart 重置。</summary>
    private int _grantedThisTurn;

    public override async Task AfterDamageReceived(
        PlayerChoiceContext choiceContext,
        Creature target,
        DamageResult result,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        // dealer==Owner：万敌本人打出的伤害；IsPoweredAttack：受力量加成的攻击伤害；实际造成了伤害
        if (dealer != Owner || !props.IsPoweredAttack() || result.UnblockedDamage <= 0)
            return;

        // HP ≤ 50% 才触发
        if (Owner.CurrentHp > Owner.MaxHp * 0.5m)
            return;

        // 每回合上限（Amount = 3 或 4）
        if (_grantedThisTurn >= Amount)
            return;

        _grantedThisTurn++;
        Flash();
        // 显式产 1 层血仇（独立于失血叠层通道）
        await VengeancePower.Grant(choiceContext, Owner, 1, cardSource);

        MainFile.Logger.Info($"[嗜血形态] HP≤50% 打出攻击产 1 血仇（本回合 {_grantedThisTurn}/{Amount}）");
    }

    /// <summary>每回合开始重置计数（仅轮到万敌这一侧时）。参考 DemonFormPower.AfterSideTurnStart。</summary>
    public override Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (participants.Contains(Owner))
            _grantedThisTurn = 0;
        return Task.CompletedTask;
    }
}
