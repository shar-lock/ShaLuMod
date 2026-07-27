// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.BufferPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class BufferPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override Decimal ModifyHpLostAfterOstyLate(
    Creature target,
    Decimal amount,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource)
  {
    return target != this.Owner ? amount : 0M;
  }

  public override async Task AfterModifyingHpLostAfterOsty()
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
    return (Decimal) ((combatState.Players.Count - 1) * 2 + 1) * amount;
  }
}
