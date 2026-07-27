// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.TheBoot
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class TheBoot : RelicModel
{
  private const int _damageMinimum = 5;
  private const string _damageMinimumKey = "DamageMinimum";
  private const string _damageThresholdKey = "DamageThreshold";

  public override RelicRarity Rarity => RelicRarity.Event;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        new DynamicVar("DamageMinimum", 5M),
        new DynamicVar("DamageThreshold", 4M)
      });
    }
  }

  public override Decimal ModifyHpLostAfterOstyLate(
    Creature target,
    Decimal amount,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource)
  {
    return dealer != this.Owner.Creature && dealer != this.Owner.Osty || target == this.Owner.Creature || !props.IsPoweredAttack() || amount < 1M || amount >= this.DynamicVars["DamageMinimum"].BaseValue ? amount : this.DynamicVars["DamageMinimum"].BaseValue;
  }

  public override Task AfterModifyingHpLostAfterOsty()
  {
    this.Flash();
    return Task.CompletedTask;
  }
}
