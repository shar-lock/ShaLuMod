using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace WandiMod.WandiModCode.Powers;

/// <summary>
/// 死亡拒绝 Power —— 本回合免死一次。参考 LizardTail（ShouldDieLate + AfterPreventingDeath）。
/// 免死触发后移除自身；我方回合结束时若未触发也移除（一次性，防永久免死）。
/// </summary>
public class DeathDenialPower : WandiModPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    /// <summary>拦截死亡：false = 不让该生物死。Owner 即万敌的 Creature。</summary>
    public override bool ShouldDieLate(Creature creature)
        => creature != Owner; // 只保护万敌本人

    /// <summary>免死后回血 + 移除自身（一次性）。</summary>
    public override async Task AfterPreventingDeath(Creature creature)
    {
        Flash();
        decimal heal = creature.MaxHp * Amount / 100m;
        await CreatureCmd.Heal(creature, Math.Max(1m, heal));
        MainFile.Logger.Info($"[死亡拒绝] 免死触发，回血 {heal}（{Amount}%）");
        await PowerCmd.Remove(this);  // 免死已消耗
    }

    /// <summary>
    /// 下一次我方回合开始时过期移除——打出时机必在本回合开始之后，
    /// 故首次触发即下一回合，保护窗口 = 本回合剩余 + 敌方阶段（符合「本回合免死」）。
    /// </summary>
    public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
    {
        if (!participants.Contains(Owner)) return;
        await PowerCmd.Remove(this);
    }
}
