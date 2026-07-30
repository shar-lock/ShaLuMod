using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace WandiMod.WandiModCode.Powers;

/// <summary>血色共鸣 Power：每次失血获得 Amount 纷争。</summary>
public class BloodResonancePower : WandiModPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterDamageReceived(PlayerChoiceContext ctx, Creature target,
        MegaCrit.Sts2.Core.GameActions.Multiplayer.DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target != Owner || result.UnblockedDamage <= 0) return;
        await StrifePower.Grant(ctx, Owner, Amount, Owner, null);
        MainFile.Logger.Info($"[血色共鸣] 失血→{Amount} 纷争");
    }
}
