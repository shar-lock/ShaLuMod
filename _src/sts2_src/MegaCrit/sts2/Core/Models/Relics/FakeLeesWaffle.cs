// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.FakeLeesWaffle
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class FakeLeesWaffle : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Event;

  public override bool HasUponPickupEffect => true;

  public override int MerchantCost => 50;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new HealVar(10M));
    }
  }

  public override async Task AfterObtained()
  {
    await CreatureCmd.Heal(this.Owner.Creature, (Decimal) this.Owner.Creature.MaxHp * (this.DynamicVars.Heal.BaseValue / 100M));
  }
}
