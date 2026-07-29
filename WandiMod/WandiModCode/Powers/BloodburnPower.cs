using MegaCrit.Sts2.Core.Commands;                 // CreatureCmd（回血）/ PowerCmd
using MegaCrit.Sts2.Core.Combat;                   // CombatSide / ICombatState
using MegaCrit.Sts2.Core.Entities.Creatures;       // Creature
using MegaCrit.Sts2.Core.Entities.Powers;          // PowerType / PowerStackType
using MegaCrit.Sts2.Core.GameActions.Multiplayer;  // PlayerChoiceContext / DamageResult
using MegaCrit.Sts2.Core.Models;                   // CardModel
using MegaCrit.Sts2.Core.ValueProps;               // ValueProp

namespace WandiMod.WandiModCode.Powers;

/// <summary>
/// 焚血 / Bloodburn（万敌 · 能力 Power）。
/// 机制：因自身卡牌失血时，回复相同的血量，每回合一次。
///   - 钩子 AfterDamageReceived：当万敌本人因「自己的卡牌」失血（自伤类卡牌如血祭/嗜血），
///     回复等量生命，并标记本回合已用。
///   - 钩子 AfterSideTurnStart：每回合开始重置「已用」标志。
/// 设计：与血仇引擎联动——自伤会触发 VengeancePower.AfterDamageReceived 自动 +1 血仇，
///   焚血把「失的血」回补，相当于「白嫖」一次血仇层数（每回合一次）。
/// 注意：「每回合一次」用实例字段追踪；Power 实例在整个战斗中存活（每战斗新建），
///   字段无需 InitInternalData 持久化（参考原生 HardenedShellPower 用 Data 类，这里更简单）。
/// </summary>
public class BloodburnPower : WandiModPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;  // 形态类，不叠加

    /// <summary>本回合是否已触发过回血（每回合一次）。在 AfterSideTurnStart 重置。</summary>
    private bool _usedThisTurn;

    /// <summary>
    /// 失血回补：万敌本人因自身卡牌失血时，回等量血，每回合一次。
    /// 参考 VengeancePower.AfterDamageReceived、RupturePower.AfterDamageReceived。
    /// 过滤条件：
    ///   1. target == Owner（掉血的是万敌本人）
    ///   2. result.UnblockedDamage > 0（确实扣了生命，被格挡/纷争吸收的不算）
    ///   3. cardSource != null 且 cardSource 属于万敌本人（「自身卡牌失血」——自伤类卡牌；
    ///      敌人打来的伤害 cardSource 为 null 或属于敌人，不算）
    ///   4. 本回合尚未触发
    /// </summary>
    public override async Task AfterDamageReceived(
        PlayerChoiceContext choiceContext,
        Creature target,
        DamageResult result,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource)
    {
        if (target != Owner || result.UnblockedDamage <= 0)
            return;
        if (_usedThisTurn)
            return;
        // 「自身卡牌」：来源卡牌存在且属于万敌本人（卡牌 Owner 是 Player，取其 Creature）
        if (cardSource == null || cardSource.Owner?.Creature != Owner)
            return;

        _usedThisTurn = true;
        Flash();
        // 回复等量生命（Heal 内部按上限封顶）。不触发失血（回血≠失血），无反馈循环。
        await CreatureCmd.Heal(Owner, result.UnblockedDamage);

        MainFile.Logger.Info($"[焚血] 自身卡牌失血 {result.UnblockedDamage}，等量回血（本回合已用）");
    }

    /// <summary>
    /// 每回合开始重置「已用」标志。参考 DemonFormPower.AfterSideTurnStart、HardenedShellPower 重置点。
    /// 仅在轮到万敌这一侧的回合时重置（participants 含万敌）。
    /// </summary>
    public override Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (participants.Contains(Owner))
            _usedThisTurn = false;
        return Task.CompletedTask;
    }
}
