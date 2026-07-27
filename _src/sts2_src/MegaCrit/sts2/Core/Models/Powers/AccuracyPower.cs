// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.AccuracyPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class AccuracyPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override Decimal ModifyDamageAdditive(
    Creature? target,
    Decimal amount,
    ValueProp props,
    Creature? dealer,
    CardModel? card,
    CardPlay? cardPlay)
  {
    return this.Owner != dealer || !props.IsPoweredAttack() || card == null || !card.Tags.Contains<CardTag>(CardTag.Shiv) ? 0M : (Decimal) this.Amount;
  }
}
