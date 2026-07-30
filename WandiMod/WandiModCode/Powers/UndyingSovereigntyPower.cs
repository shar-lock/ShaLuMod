using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace WandiMod.WandiModCode.Powers;

/// <summary>不死王权 Power：受到攻击（掉血）时回复最大生命 Amount% 的血量。</summary>
public class UndyingSovereigntyPower : WandiModPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterDamageReceived(PlayerChoiceContext ctx, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target != Owner || result.UnblockedDamage <= 0) return;
        decimal heal = Owner.MaxHp * (decimal)Amount / 100m;
        await CreatureCmd.Heal(Owner, Math.Max(1m, heal));
        MainFile.Logger.Info($"[不死王权] 受击回血 {heal}（MaxHp {Owner.MaxHp}×{Amount}%）");
    }
}
