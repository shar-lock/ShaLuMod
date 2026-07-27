// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.PossessStrengthPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class PossessStrengthPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Single;

  protected override object InitInternalData() => (object) new PossessStrengthPower.Data();

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>());
    }
  }

  private Dictionary<Creature, Decimal> StolenStrength
  {
    get => this.GetInternalData<PossessStrengthPower.Data>().stolenStrength;
  }

  public override Task AfterPowerAmountChanged(
    PlayerChoiceContext choiceContext,
    PowerModel power,
    Decimal amount,
    Creature? applier,
    CardModel? cardSource)
  {
    if (applier != this.Owner || !power.Owner.IsPlayer || !(power is StrengthPower) || amount >= 0M)
      return Task.CompletedTask;
    if (!this.StolenStrength.ContainsKey(power.Owner))
      this.StolenStrength.Add(power.Owner, 0M);
    this.StolenStrength[power.Owner] += amount;
    return Task.CompletedTask;
  }

  public override async Task AfterDeath(
    PlayerChoiceContext choiceContext,
    Creature creature,
    bool wasRemovalPrevented,
    float deathAnimLength)
  {
    if (wasRemovalPrevented || creature != this.Owner)
      return;
    foreach (KeyValuePair<Creature, Decimal> keyValuePair in this.StolenStrength)
    {
      StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>(choiceContext, keyValuePair.Key, -keyValuePair.Value, (Creature) null, (CardModel) null);
    }
  }

  private class Data
  {
    public Dictionary<Creature, Decimal> stolenStrength = new Dictionary<Creature, Decimal>();
  }
}
