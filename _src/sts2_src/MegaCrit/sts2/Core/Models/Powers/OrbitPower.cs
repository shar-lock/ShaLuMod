// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.OrbitPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class OrbitPower : PowerModel
{
  private const int _energyIncrement = 4;

  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override int DisplayAmount => 4 - this.GetInternalData<OrbitPower.Data>().energySpent % 4;

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.ForEnergy((PowerModel) this));
    }
  }

  protected override object InitInternalData() => (object) new OrbitPower.Data();

  public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new EnergyVar(4));
    }
  }

  public override async Task AfterEnergySpent(CardModel card, int amount)
  {
    OrbitPower.Data data;
    if (card.Owner.Creature != this.Owner)
      data = (OrbitPower.Data) null;
    else if (amount <= 0)
    {
      data = (OrbitPower.Data) null;
    }
    else
    {
      data = this.GetInternalData<OrbitPower.Data>();
      data.energySpent += amount;
      int triggers = data.energySpent / 4 - data.triggerCount;
      if (triggers > 0)
      {
        this.Flash();
        await PlayerCmd.GainEnergy((Decimal) (this.Amount * triggers), this.Owner.Player);
        data.triggerCount += triggers;
      }
      this.InvokeDisplayAmountChanged();
      data = (OrbitPower.Data) null;
    }
  }

  private class Data
  {
    public int energySpent;
    public int triggerCount;
  }
}
