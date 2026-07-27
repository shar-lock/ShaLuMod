// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Factories.RelicFactory
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.TestSupport;
using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Factories;

public static class RelicFactory
{
  public static RelicModel FallbackRelic => (RelicModel) ModelDb.Relic<Circlet>();

  public static RelicModel PullNextRelicFromFront(Player player, Rng rng)
  {
    return RelicFactory.PullNextRelicFromFront(player, RelicFactory.RollRarity(rng), (Func<RelicModel, bool>) (_ => true));
  }

  public static RelicModel PullNextRelicFromFront(Player player)
  {
    return RelicFactory.PullNextRelicFromFront(player, RelicFactory.RollRarity(player), (Func<RelicModel, bool>) (_ => true));
  }

  public static RelicModel PullNextRelicFromFront(Player player, RelicRarity rarity)
  {
    return RelicFactory.PullNextRelicFromFront(player, rarity, (Func<RelicModel, bool>) (_ => true));
  }

  public static RelicModel PullNextRelicFromFront(
    Player player,
    RelicRarity rarity,
    Func<RelicModel, bool> filter)
  {
    RelicModel relic = TestRngInjector.ConsumeRelicOverride() ?? player.RelicGrabBag.PullFromFront(rarity, filter, player.RunState) ?? RelicFactory.FallbackRelic;
    player.RunState.SharedRelicGrabBag.Remove(relic);
    return relic;
  }

  public static RelicModel PullNextRelicFromBack(Player player)
  {
    return RelicFactory.PullNextRelicFromBack(player, RelicFactory.RollRarity(player), (Func<RelicModel, bool>) (_ => true));
  }

  public static RelicModel PullNextRelicFromBack(
    Player player,
    RelicRarity rarity,
    Func<RelicModel, bool> filter)
  {
    RelicModel relic = TestRngInjector.ConsumeRelicOverride() ?? player.RelicGrabBag.PullFromBack(rarity, filter, player.RunState) ?? RelicFactory.FallbackRelic;
    player.RunState.SharedRelicGrabBag.Remove(relic);
    return relic;
  }

  public static RelicRarity RollRarity(Player player)
  {
    return RelicFactory.RollRarity(player.PlayerRng.Rewards);
  }

  public static RelicRarity RollRarity(Rng rng)
  {
    RelicRarity? relicRarityOverride = TestRngInjector.GetRelicRarityOverride();
    RelicRarity relicRarity;
    if (relicRarityOverride.HasValue)
    {
      relicRarity = relicRarityOverride.GetValueOrDefault();
    }
    else
    {
      float num = rng.NextFloat();
      relicRarity = (double) num < 0.5 ? RelicRarity.Common : ((double) num < 0.82999998331069946 ? RelicRarity.Uncommon : RelicRarity.Rare);
    }
    return relicRarity;
  }
}
