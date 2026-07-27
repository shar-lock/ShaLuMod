// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.ArtifactPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using System;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class ArtifactPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override bool ShouldScaleInMultiplayer => true;

  public override bool TryModifyPowerAmountReceived(
    PowerModel canonicalPower,
    Creature target,
    Decimal amount,
    Creature? _,
    out Decimal modifiedAmount)
  {
    if (target != this.Owner)
    {
      modifiedAmount = amount;
      return false;
    }
    if (canonicalPower.GetTypeForAmount(amount) != PowerType.Debuff)
    {
      modifiedAmount = amount;
      return false;
    }
    if (!canonicalPower.IsVisible)
    {
      modifiedAmount = amount;
      return false;
    }
    modifiedAmount = 0M;
    return true;
  }

  public override async Task AfterModifyingPowerAmountReceived(PowerModel power)
  {
    await PowerCmd.Decrement((PowerModel) this);
  }

  public override Decimal GetScaledAmountForMultiplayer(
    ICombatState combatState,
    Creature? applier,
    Decimal amount,
    Creature target,
    CardModel? cardSource)
  {
    return amount + (Decimal) combatState.Players.Count - 1M;
  }
}
