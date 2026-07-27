// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Map.MapPointTypeCounts
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Map;

public class MapPointTypeCounts
{
  public HashSet<MapPointType> PointTypesThatIgnoreRules { get; init; } = new HashSet<MapPointType>();

  public int NumOfElites { get; init; } = (int) Math.Round(5.0 * (AscensionHelper.HasAscension(AscensionLevel.SwarmingElites) ? 1.6000000238418579 : 1.0));

  public int NumOfShops { get; } = 3;

  public int NumOfUnknowns { get; }

  public int NumOfRests { get; }

  public bool ShouldIgnoreMapPointRulesForMapPointType(MapPointType pointType)
  {
    return this.PointTypesThatIgnoreRules.Contains(pointType);
  }

  public static int StandardRandomUnknownCount(Rng rng) => rng.NextGaussianInt(12, 1, 10, 14);

  public MapPointTypeCounts(int unknownCount, int restCount)
  {
    this.NumOfUnknowns = unknownCount;
    this.NumOfRests = restCount;
  }

  public MapPointTypeCounts(ActMap existingMap)
  {
    MapPoint[] array = existingMap.GetAllMapPoints().ToArray<MapPoint>();
    this.NumOfUnknowns = ((IEnumerable<MapPoint>) array).Count<MapPoint>((Func<MapPoint, bool>) (p => p.PointType == MapPointType.Unknown));
    this.NumOfRests = ((IEnumerable<MapPoint>) array).Count<MapPoint>((Func<MapPoint, bool>) (p => p.PointType == MapPointType.RestSite));
  }
}
