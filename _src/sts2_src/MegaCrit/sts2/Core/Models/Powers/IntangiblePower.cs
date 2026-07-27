// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.IntangiblePower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class IntangiblePower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override Decimal ModifyHpLostAfterOsty(
    Creature target,
    Decimal amount,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource)
  {
    return !CombatManager.Instance.IsInProgress || target != this.Owner || amount < 1M ? amount : 1M;
  }

  public override Task AfterModifyingHpLostAfterOsty()
  {
    this.Flash();
    return Task.CompletedTask;
  }

  public override Decimal ModifyDamageCap(
    Creature? target,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource,
    CardPlay? cardPlay)
  {
    return target != this.Owner ? Decimal.MaxValue : 1M;
  }

  public override Task AfterModifyingDamageAmount(CardModel? cardSource)
  {
    this.Flash();
    return Task.CompletedTask;
  }

  public override async Task AfterSideTurnEnd(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IEnumerable<Creature> participants)
  {
    if (side != CombatSide.Enemy)
      return;
    await PowerCmd.Decrement((PowerModel) this);
  }
}
