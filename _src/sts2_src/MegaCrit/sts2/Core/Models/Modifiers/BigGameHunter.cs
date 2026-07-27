// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Modifiers.BigGameHunter
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Modifiers;

public class BigGameHunter : ModifierModel
{
  public override ActMap ModifyGeneratedMap(IRunState runState, ActMap map, int actIndex)
  {
    Rng mapRng = new Rng(runState.Rng.Seed, $"act_{runState.CurrentActIndex + 1}_map");
    MapPointTypeCounts mapPointTypeCountsOverride = new MapPointTypeCounts(map)
    {
      NumOfElites = (int) Math.Round((double) map.GetAllMapPoints().Count<MapPoint>((Func<MapPoint, bool>) (p => p.PointType == MapPointType.Elite)) * 2.5),
      PointTypesThatIgnoreRules = new HashSet<MapPointType>()
      {
        MapPointType.Elite
      }
    };
    switch (map)
    {
      case StandardActMap _:
        bool shouldReplaceTreasureWithElites = map is StandardActMap standardActMap && standardActMap.ShouldReplaceTreasureWithElites;
        return (ActMap) new StandardActMap(mapRng, runState.Act, runState.Players.Count > 1, shouldReplaceTreasureWithElites, runState.Act.HasSecondBoss, mapPointTypeCountsOverride);
      case SpoilsActMap _:
        return (ActMap) new SpoilsActMap(runState, mapPointTypeCountsOverride);
      default:
        return map;
    }
  }

  public override CardCreationOptions ModifyCardRewardCreationOptions(
    Player player,
    CardCreationOptions options)
  {
    if (options.Source != CardCreationSource.Encounter || options.RarityOdds != CardRarityOddsType.EliteEncounter || options.Flags.HasFlag((Enum) CardCreationFlags.NoCardPoolModifications) || options.Flags.HasFlag((Enum) CardCreationFlags.NoRarityModification))
      return options;
    options = options.WithRarityOdds(CardRarityOddsType.Uniform).WithFilter((Func<CardModel, bool>) (c => c.Rarity == CardRarity.Rare));
    if (options.GetPossibleCards(player).ToList<CardModel>().Count <= 0)
    {
      // ISSUE: object of a compiler-generated type is created
      options = options.WithCardPools((IEnumerable<CardPoolModel>) new \u003C\u003Ez__ReadOnlySingleElementList<CardPoolModel>(player.Character.CardPool));
    }
    return options;
  }
}
