using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace WandiMod.WandiModCode.Powers;

/// <summary>
/// 死亡拒绝 Power —— 本回合免死一次。参考 LizardTail（ShouldDieLate + AfterPreventingDeath）。
/// 回合结束自动移除（一次性）。
/// </summary>
public class DeathDenialPower : WandiModPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    /// <summary>拦截死亡：false = 不让该生物死。</summary>
    public override bool ShouldDieLate(Creature creature)
        => creature != Owner.Creature; // 只保护万敌本人

    /// <summary>免死后回血 + 移除自身（一次性）。</summary>
    public override async Task AfterPreventingDeath(Creature creature)
    {
        Flash();
        decimal heal = creature.MaxHp * Amount / 100m;
        await CreatureCmd.Heal(creature, Math.Max(1m, heal));
        MainFile.Logger.Info($"[死亡拒绝] 免死触发，回血 {heal}（{Amount}%）");
    }
}
