// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.PlowPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class PlowPower : PowerModel
{
  public override PowerType Type => PowerType.Debuff;

  public override PowerStackType StackType => PowerStackType.Counter;

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlyArray<IHoverTip>(new IHoverTip[2]
      {
        HoverTipFactory.Static(StaticHoverTip.Stun),
        HoverTipFactory.FromPower<StrengthPower>()
      });
    }
  }

  public override bool ShouldScaleInMultiplayer => true;

  public override async Task AfterDamageReceived(
    PlayerChoiceContext choiceContext,
    Creature target,
    DamageResult result,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource)
  {
    if (target != this.Owner)
      ceremonialBeast = (CeremonialBeast) null;
    else if (result.UnblockedDamage <= 0)
      ceremonialBeast = (CeremonialBeast) null;
    else if (target.CurrentHp > this.Amount)
    {
      ceremonialBeast = (CeremonialBeast) null;
    }
    else
    {
      this.Flash();
      foreach (PowerModel power in this.Owner.GetPowerInstances<TemporaryStrengthPower>().ToList<TemporaryStrengthPower>())
        await PowerCmd.Remove(power);
      await PowerCmd.Remove<StrengthPower>(this.Owner);
      if (this.Owner.Monster is CeremonialBeast ceremonialBeast)
      {
        await ceremonialBeast.SetStunned();
        await CreatureCmd.Stun(this.Owner, new Func<IReadOnlyList<Creature>, Task>(ceremonialBeast.StunnedMove), ceremonialBeast.BeastCryState.StateId);
      }
      else
        await CreatureCmd.Stun(this.Owner);
      await PowerCmd.Remove((PowerModel) this);
      ceremonialBeast = (CeremonialBeast) null;
    }
  }
}
