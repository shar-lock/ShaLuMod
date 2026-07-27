// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Factories.PotionFactory
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.PotionPools;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Factories;

public static class PotionFactory
{
  private const float _rareChance = 0.1f;
  private const float _rareThreshold = 0.1f;
  private const float _uncommonChance = 0.25f;
  private const float _uncommonThreshold = 0.35f;

  public static PotionModel CreateRandomPotionOutOfCombat(
    Player player,
    Rng rng,
    IEnumerable<PotionModel>? blacklist = null)
  {
    return PotionFactory.CreateRandomPotionsOutOfCombat(player, 1, rng, blacklist).First<PotionModel>();
  }

  public static IEnumerable<PotionModel> CreateRandomPotionsOutOfCombat(
    Player player,
    int count,
    Rng rng,
    IEnumerable<PotionModel>? blacklist = null)
  {
    IEnumerable<PotionModel> potionModels = PotionFactory.GetPotionOptions(player);
    if (blacklist != null)
      potionModels = potionModels.Except<PotionModel>(blacklist);
    return PotionFactory.CreateRandomPotions(potionModels, count, rng);
  }

  public static PotionModel CreateRandomPotionInCombat(
    Player player,
    Rng rng,
    IEnumerable<PotionModel>? blacklist = null)
  {
    IEnumerable<PotionModel> potionModels = PotionFactory.GetPotionOptions(player).Where<PotionModel>((Func<PotionModel, bool>) (p => p.CanBeGeneratedInCombat));
    if (blacklist != null)
      potionModels = potionModels.Except<PotionModel>(blacklist);
    return PotionFactory.CreateRandomPotions(potionModels, 1, rng).First<PotionModel>();
  }

  private static IEnumerable<PotionModel> CreateRandomPotions(
    IEnumerable<PotionModel> options,
    int count,
    Rng rng)
  {
    List<PotionModel> list = options.ToList<PotionModel>();
    List<PotionModel> randomPotions = new List<PotionModel>();
    for (int index = 0; index < count; ++index)
    {
      float num = rng.NextFloat();
      PotionRarity rarity = (double) num <= 0.10000000149011612 ? PotionRarity.Rare : ((double) num <= 0.34999999403953552 ? PotionRarity.Uncommon : PotionRarity.Common);
      PotionModel potionModel = rng.NextItem<PotionModel>(list.Where<PotionModel>((Func<PotionModel, bool>) (potion => potion.Rarity == rarity)));
      randomPotions.Add(potionModel);
      list.Remove(potionModel);
    }
    return (IEnumerable<PotionModel>) randomPotions;
  }

  public static IEnumerable<PotionModel> GetPotionOptions(Player player)
  {
    return player.Character.PotionPool.GetUnlockedPotions(player.UnlockState).Concat<PotionModel>(ModelDb.PotionPool<SharedPotionPool>().GetUnlockedPotions(player.UnlockState));
  }
}
