// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.BowlerHat
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class BowlerHat : RelicModel
{
  private const string _goldIncreaseKey = "GoldIncrease";

  public override RelicRarity Rarity => RelicRarity.Uncommon;

  public override bool IsAllowedInShops => false;

  public override bool IsAllowed(IRunState runState)
  {
    return RelicModel.IsBeforeAct3TreasureChest(runState);
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("GoldIncrease", 1.25M));
    }
  }

  public override Decimal ModifyGoldGained(Player player, Decimal amount)
  {
    return player != this.Owner ? amount : amount * this.DynamicVars["GoldIncrease"].BaseValue;
  }

  public override Task AfterModifyingGoldGained(Player player, Decimal amount)
  {
    this.Flash();
    return Task.CompletedTask;
  }
}
