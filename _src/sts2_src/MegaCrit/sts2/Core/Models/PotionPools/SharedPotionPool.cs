// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.PotionPools.SharedPotionPool
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Timeline.Epochs;
using MegaCrit.Sts2.Core.Unlocks;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.PotionPools;

public sealed class SharedPotionPool : PotionPoolModel
{
  public override string EnergyColorName => "colorless";

  protected override IEnumerable<PotionModel> GenerateAllPotions()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerable<PotionModel>) new \u003C\u003Ez__ReadOnlyArray<PotionModel>(new PotionModel[45]
    {
      (PotionModel) ModelDb.Potion<AttackPotion>(),
      (PotionModel) ModelDb.Potion<BeetleJuice>(),
      (PotionModel) ModelDb.Potion<BlessingOfTheForge>(),
      (PotionModel) ModelDb.Potion<BlockPotion>(),
      (PotionModel) ModelDb.Potion<BottledPotential>(),
      (PotionModel) ModelDb.Potion<Clarity>(),
      (PotionModel) ModelDb.Potion<ColorlessPotion>(),
      (PotionModel) ModelDb.Potion<CureAll>(),
      (PotionModel) ModelDb.Potion<DexterityPotion>(),
      (PotionModel) ModelDb.Potion<DistilledChaos>(),
      (PotionModel) ModelDb.Potion<DropletOfPrecognition>(),
      (PotionModel) ModelDb.Potion<Duplicator>(),
      (PotionModel) ModelDb.Potion<EnergyPotion>(),
      (PotionModel) ModelDb.Potion<EntropicBrew>(),
      (PotionModel) ModelDb.Potion<ExplosiveAmpoule>(),
      (PotionModel) ModelDb.Potion<FairyInABottle>(),
      (PotionModel) ModelDb.Potion<FirePotion>(),
      (PotionModel) ModelDb.Potion<FlexPotion>(),
      (PotionModel) ModelDb.Potion<Fortifier>(),
      (PotionModel) ModelDb.Potion<FruitJuice>(),
      (PotionModel) ModelDb.Potion<FyshOil>(),
      (PotionModel) ModelDb.Potion<GamblersBrew>(),
      (PotionModel) ModelDb.Potion<GigantificationPotion>(),
      (PotionModel) ModelDb.Potion<HeartOfIron>(),
      (PotionModel) ModelDb.Potion<LiquidBronze>(),
      (PotionModel) ModelDb.Potion<LiquidMemories>(),
      (PotionModel) ModelDb.Potion<LuckyTonic>(),
      (PotionModel) ModelDb.Potion<MazalethsGift>(),
      (PotionModel) ModelDb.Potion<OrobicAcid>(),
      (PotionModel) ModelDb.Potion<PotionOfBinding>(),
      (PotionModel) ModelDb.Potion<PowderedDemise>(),
      (PotionModel) ModelDb.Potion<PowerPotion>(),
      (PotionModel) ModelDb.Potion<RadiantTincture>(),
      (PotionModel) ModelDb.Potion<RegenPotion>(),
      (PotionModel) ModelDb.Potion<ShacklingPotion>(),
      (PotionModel) ModelDb.Potion<ShipInABottle>(),
      (PotionModel) ModelDb.Potion<SkillPotion>(),
      (PotionModel) ModelDb.Potion<SneckoOil>(),
      (PotionModel) ModelDb.Potion<SpeedPotion>(),
      (PotionModel) ModelDb.Potion<StableSerum>(),
      (PotionModel) ModelDb.Potion<StrengthPotion>(),
      (PotionModel) ModelDb.Potion<SwiftPotion>(),
      (PotionModel) ModelDb.Potion<TouchOfInsanity>(),
      (PotionModel) ModelDb.Potion<VulnerablePotion>(),
      (PotionModel) ModelDb.Potion<WeakPotion>()
    });
  }

  public override IEnumerable<PotionModel> GetUnlockedPotions(UnlockState unlockState)
  {
    List<PotionModel> list = this.AllPotions.ToList<PotionModel>();
    if (!unlockState.IsEpochRevealed<Potion1Epoch>())
    {
      foreach (PotionModel potion in Potion1Epoch.Potions)
        list.Remove(potion);
    }
    if (!unlockState.IsEpochRevealed<Potion2Epoch>())
    {
      foreach (PotionModel potion in Potion2Epoch.Potions)
        list.Remove(potion);
    }
    return (IEnumerable<PotionModel>) list;
  }
}
