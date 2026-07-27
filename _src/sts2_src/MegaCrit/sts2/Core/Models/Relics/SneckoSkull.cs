// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.SneckoSkull
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class SneckoSkull : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Common;

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<PoisonPower>());
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new PowerVar<PoisonPower>(1M));
    }
  }

  public override Decimal ModifyPowerAmountGivenAdditive(
    PowerModel power,
    Creature giver,
    Decimal amount,
    Creature? target,
    CardModel? cardSource)
  {
    return !(power is PoisonPower) || giver != this.Owner.Creature ? 0M : this.DynamicVars.Poison.BaseValue;
  }

  public override Task AfterModifyingPowerAmountGiven(PowerModel power)
  {
    this.Flash();
    return Task.CompletedTask;
  }
}
