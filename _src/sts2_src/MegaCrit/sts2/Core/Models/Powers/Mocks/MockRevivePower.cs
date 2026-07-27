// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.Mocks.MockRevivePower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers.Mocks;

public sealed class MockRevivePower : PowerModel
{
  public override bool IsMock => true;

  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Single;

  protected override object InitInternalData() => (object) new MockRevivePower.Data();

  private bool IsReviving => this.GetInternalData<MockRevivePower.Data>().isReviving;

  public override Decimal ModifyDamageMultiplicative(
    Creature? target,
    Decimal amount,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource,
    CardPlay? cardPlay)
  {
    return target != this.Owner || !this.IsReviving ? 1M : 0M;
  }

  public override bool TryModifyPowerAmountReceived(
    PowerModel canonicalPower,
    Creature target,
    Decimal amount,
    Creature? applier,
    out Decimal modifiedAmount)
  {
    modifiedAmount = amount;
    return target == this.Owner && this.IsReviving;
  }

  public override bool ShouldAllowHitting(Creature creature) => !this.IsReviving;

  public override bool ShouldDie(Creature creature) => creature != this.Owner;

  public override bool ShouldStopCombatFromEnding() => true;

  public override async Task AfterPreventingDeath(Creature creature)
  {
    if (creature != this.Owner)
      return;
    this.GetInternalData<MockRevivePower.Data>().isReviving = true;
    await CreatureCmd.Heal(this.Owner, 1M, false);
  }

  private class Data
  {
    public bool isReviving;
  }
}
