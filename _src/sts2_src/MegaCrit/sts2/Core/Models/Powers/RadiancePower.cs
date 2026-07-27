// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.RadiancePower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class RadiancePower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.ForEnergy((PowerModel) this));
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new EnergyVar(1));
    }
  }

  public override async Task AfterEnergyReset(Player player)
  {
    if (player != this.Owner.Player)
      return;
    await PlayerCmd.GainEnergy((Decimal) this.DynamicVars.Energy.IntValue, player);
    await PowerCmd.Decrement((PowerModel) this);
  }
}
