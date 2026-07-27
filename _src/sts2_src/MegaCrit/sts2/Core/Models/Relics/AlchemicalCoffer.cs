// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.AlchemicalCoffer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class AlchemicalCoffer : RelicModel
{
  private const string _potionSlotsKey = "PotionSlots";

  public override RelicRarity Rarity => RelicRarity.Ancient;

  public override bool HasUponPickupEffect => true;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("PotionSlots", 4M));
    }
  }

  public override async Task AfterObtained()
  {
    int originalSlotCount = this.Owner.MaxPotionCount;
    await PlayerCmd.GainMaxPotionCount(this.DynamicVars["PotionSlots"].IntValue, this.Owner);
    IEnumerable<PotionModel> potionsOutOfCombat = PotionFactory.CreateRandomPotionsOutOfCombat(this.Owner, this.DynamicVars["PotionSlots"].IntValue, this.Owner.RunState.Rng.CombatPotionGeneration);
    int i = 0;
    foreach (PotionModel potionModel in potionsOutOfCombat)
    {
      PotionProcureResult procure = await PotionCmd.TryToProcure(potionModel.ToMutable(), this.Owner, originalSlotCount + i);
      ++i;
    }
  }
}
