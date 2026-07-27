// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.HardenedShellPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class HardenedShellPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override bool ShouldScaleInMultiplayer => true;

  public override int DisplayAmount
  {
    get
    {
      return (int) Math.Max(0M, (Decimal) this.Amount - this.GetInternalData<HardenedShellPower.Data>().damageReceivedThisTurn);
    }
  }

  protected override object InitInternalData() => (object) new HardenedShellPower.Data();

  public override Decimal ModifyHpLostBeforeOstyLate(
    Creature target,
    Decimal amount,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource)
  {
    return target != this.Owner || amount == 0M ? amount : Math.Min(amount, (Decimal) this.Amount - this.GetInternalData<HardenedShellPower.Data>().damageReceivedThisTurn);
  }

  public override Task AfterModifyingHpLostBeforeOsty()
  {
    this.Flash();
    return Task.CompletedTask;
  }

  public override Task AfterDamageReceived(
    PlayerChoiceContext choiceContext,
    Creature target,
    DamageResult result,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource)
  {
    if (target != this.Owner || result.WasFullyBlocked)
      return Task.CompletedTask;
    HardenedShellPower.Data internalData = this.GetInternalData<HardenedShellPower.Data>();
    internalData.damageReceivedThisTurn += (Decimal) result.UnblockedDamage;
    this.InvokeDisplayAmountChanged();
    if (internalData.damageReceivedThisTurn >= (Decimal) this.Amount)
      this.Owner.HpDisplay = HpDisplay.InfiniteWithNumbers;
    return Task.CompletedTask;
  }

  public override Task BeforeSideTurnStart(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IReadOnlyList<Creature> participants,
    ICombatState combatState)
  {
    this.GetInternalData<HardenedShellPower.Data>().damageReceivedThisTurn = 0M;
    this.InvokeDisplayAmountChanged();
    this.Owner.HpDisplay = HpDisplay.Normal;
    return Task.CompletedTask;
  }

  private class Data
  {
    public Decimal damageReceivedThisTurn;
  }
}
