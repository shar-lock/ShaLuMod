// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.OldCoin
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Runs;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class OldCoin : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Rare;

  public override bool IsAllowed(IRunState runState)
  {
    return RelicModel.IsBeforeAct3TreasureChest(runState);
  }

  public override bool IsAllowedInShops => false;

  public override bool HasUponPickupEffect => true;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new GoldVar(300));
    }
  }

  public override async Task AfterObtained()
  {
    await PlayerCmd.GainGold(this.DynamicVars.Gold.BaseValue, this.Owner);
  }
}
