// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.Cauldron
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class Cauldron : RelicModel
{
  private const string _potionsKey = "Potions";
  private static readonly PotionModel[] _testPotions = new PotionModel[5]
  {
    (PotionModel) ModelDb.Potion<FlexPotion>(),
    (PotionModel) ModelDb.Potion<WeakPotion>(),
    (PotionModel) ModelDb.Potion<VulnerablePotion>(),
    (PotionModel) ModelDb.Potion<StrengthPotion>(),
    (PotionModel) ModelDb.Potion<DexterityPotion>()
  };

  public override RelicRarity Rarity => RelicRarity.Shop;

  public override bool HasUponPickupEffect => true;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Potions", 5M));
    }
  }

  public override async Task AfterObtained()
  {
    await RewardsCmd.OfferCustom(this.Owner, this.GenerateRewards());
  }

  private List<Reward> GenerateRewards()
  {
    int intValue = this.DynamicVars["Potions"].IntValue;
    List<Reward> rewards = new List<Reward>();
    if (TestMode.IsOn)
    {
      for (int index = 0; index < intValue; ++index)
        rewards.Add((Reward) new PotionReward(Cauldron._testPotions[index % intValue].ToMutable(), this.Owner));
    }
    else
    {
      for (int index = 0; index < intValue; ++index)
        rewards.Add((Reward) new PotionReward(this.Owner));
    }
    return rewards;
  }
}
