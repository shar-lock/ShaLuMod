// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.OneForAllPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class OneForAllPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override Decimal ModifyDamageAdditive(
    Creature? target,
    Decimal amount,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource,
    CardPlay? cardPlay)
  {
    return !props.IsPoweredAttack() || cardSource == null || cardSource.Owner.Creature != this.Owner || (cardPlay != null ? (cardPlay.Card.EnergyCost.CostsX ? 1 : 0) : (cardSource.EnergyCost.CostsX ? 1 : 0)) != 0 || cardPlay != null && cardPlay.Resources.EnergySpent != 0 || cardPlay == null && cardSource.EnergyCost.GetWithModifiers(CostModifiers.All) != 0 ? 0M : (Decimal) this.Amount;
  }
}
