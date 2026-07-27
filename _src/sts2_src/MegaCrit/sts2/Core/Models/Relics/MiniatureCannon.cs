// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.MiniatureCannon
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class MiniatureCannon : RelicModel
{
  private const string _extraDamageKey = "ExtraDamage";

  public override RelicRarity Rarity => RelicRarity.Uncommon;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("ExtraDamage", 3M));
    }
  }

  public override Decimal ModifyDamageAdditive(
    Creature? target,
    Decimal amount,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource,
    CardPlay? cardPlay)
  {
    return !props.IsPoweredAttack() || cardSource == null || !cardSource.IsUpgraded || dealer != this.Owner.Creature && cardSource.Owner != this.Owner ? 0M : this.DynamicVars["ExtraDamage"].BaseValue;
  }
}
