// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.DragonFruit
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Runs;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class DragonFruit : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Shop;

  public override bool IsAllowed(IRunState runState)
  {
    return RelicModel.IsBeforeAct3TreasureChest(runState);
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new MaxHpVar(1M));
    }
  }

  public override async Task AfterGoldGained(Player player)
  {
    if (player != this.Owner)
      return;
    this.Flash();
    await CreatureCmd.GainMaxHp(this.Owner.Creature, this.DynamicVars.MaxHp.BaseValue);
  }
}
