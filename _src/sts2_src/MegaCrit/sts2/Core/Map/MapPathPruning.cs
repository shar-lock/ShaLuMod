// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Map.MapPathPruning
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

#nullable enable
namespace MegaCrit.Sts2.Core.Map;

public static class MapPathPruning
{
  public static void PruneAndRepair(
    MapPoint?[,] grid,
    HashSet<MapPoint> startMapPoints,
    ActMap map,
    MapPointTypeCounts pointTypeCounts,
    Rng rng,
    Func<MapPointType, MapPoint, bool> isValidPointType)
  {
    for (int index = 0; index < 3; ++index)
    {
      MapPathPruning.PruneDuplicateSegments(grid, startMapPoints, map.StartingMapPoint, rng);
      if (!MapPathPruning.RepairPrunedPointTypes(map, pointTypeCounts, rng, isValidPointType))
        break;
    }
  }

  public static bool RepairPrunedPointTypes(
    ActMap map,
    MapPointTypeCounts pointTypeCounts,
    Rng rng,
    Func<MapPointType, MapPoint, bool> isValidPointType)
  {
    return false | MapPathPruning.RepairPointType(map, MapPointType.Shop, pointTypeCounts.NumOfShops, rng, isValidPointType) | MapPathPruning.RepairPointType(map, MapPointType.Elite, pointTypeCounts.NumOfElites, rng, isValidPointType) | MapPathPruning.RepairPointType(map, MapPointType.RestSite, pointTypeCounts.NumOfRests, rng, isValidPointType) | MapPathPruning.RepairPointType(map, MapPointType.Unknown, pointTypeCounts.NumOfUnknowns, rng, isValidPointType);
  }

  private static bool RepairPointType(
    ActMap map,
    MapPointType type,
    int targetCount,
    Rng rng,
    Func<MapPointType, MapPoint, bool> isValidPointType)
  {
    int num1 = map.GetAllMapPoints().Count<MapPoint>((Func<MapPoint, bool>) (p => p.PointType == type));
    int num2 = targetCount - num1;
    if (num2 <= 0)
      return false;
    bool flag = false;
    List<MapPoint> list = map.GetAllMapPoints().Where<MapPoint>((Func<MapPoint, bool>) (p => p.PointType == MapPointType.Monster && p.CanBeModified)).ToList<MapPoint>();
    list.StableShuffle<MapPoint>(rng);
    foreach (MapPoint mapPoint in list)
    {
      if (num2 != 0)
      {
        if (isValidPointType(type, mapPoint))
        {
          mapPoint.PointType = type;
          --num2;
          flag = true;
        }
      }
      else
        break;
    }
    return flag;
  }

  public static void PruneDuplicateSegments(
    MapPoint?[,] grid,
    HashSet<MapPoint> startMapPoints,
    MapPoint startingMapPoint,
    Rng rng)
  {
    int num = 0;
    for (List<List<MapPoint[]>> matchingSegments = MapPathPruning.FindMatchingSegments(startingMapPoint); MapPathPruning.PrunePaths(grid, startMapPoints, (IEnumerable<List<MapPoint[]>>) matchingSegments, rng); matchingSegments = MapPathPruning.FindMatchingSegments(startingMapPoint))
    {
      ++num;
      if (num > 50)
        throw new InvalidOperationException($"Unable to prune matching segments in {num} iterations");
    }
  }

  public static List<List<MapPoint[]>> FindMatchingSegments(MapPoint startingMapPoint)
  {
    List<List<MapPoint>> allPaths = MapPathPruning.FindAllPaths(startingMapPoint);
    SortedDictionary<string, List<MapPoint[]>> segments = new SortedDictionary<string, List<MapPoint[]>>((IComparer<string>) StringComparer.Ordinal);
    foreach (IReadOnlyList<MapPoint> path in allPaths)
      MapPathPruning.AddSegmentsToDictionary(path, (IDictionary<string, List<MapPoint[]>>) segments);
    return MapPathPruning.GetDuplicateSegments((IDictionary<string, List<MapPoint[]>>) segments);
  }

  public static List<List<MapPoint>> FindAllPaths(MapPoint currentMapPoint)
  {
    List<List<MapPoint>> allPaths = new List<List<MapPoint>>();
    if (currentMapPoint.PointType == MapPointType.Boss)
    {
      List<List<MapPoint>> mapPointListList = allPaths;
      int capacity = 1;
      List<MapPoint> mapPointList = new List<MapPoint>(capacity);
      CollectionsMarshal.SetCount<MapPoint>(mapPointList, capacity);
      CollectionsMarshal.AsSpan<MapPoint>(mapPointList)[0] = currentMapPoint;
      mapPointListList.Add(mapPointList);
      return allPaths;
    }
    foreach (MapPoint child in currentMapPoint.Children)
    {
      foreach (List<MapPoint> allPath in MapPathPruning.FindAllPaths(child))
      {
        int capacity = 1;
        List<MapPoint> mapPointList1 = new List<MapPoint>(capacity);
        CollectionsMarshal.SetCount<MapPoint>(mapPointList1, capacity);
        CollectionsMarshal.AsSpan<MapPoint>(mapPointList1)[0] = currentMapPoint;
        List<MapPoint> mapPointList2 = mapPointList1;
        mapPointList2.AddRange((IEnumerable<MapPoint>) allPath);
        allPaths.Add(mapPointList2);
      }
    }
    return allPaths;
  }

  private static void AddSegmentsToDictionary(
    IReadOnlyList<MapPoint> path,
    IDictionary<string, List<MapPoint[]>> segments)
  {
    for (int index1 = 0; index1 < path.Count - 1; ++index1)
    {
      if (MapPathPruning.IsValidSegmentStartMapPoint(path[index1]))
      {
        for (int index2 = 2; index2 < path.Count - index1; ++index2)
        {
          if (MapPathPruning.IsValidSegmentEndMapPoint(path[index1 + index2]))
          {
            MapPoint[] array = path.Skip<MapPoint>(index1).Take<MapPoint>(index2 + 1).ToArray<MapPoint>();
            string segmentKey = MapPathPruning.GenerateSegmentKey((IReadOnlyList<MapPoint>) array);
            if (!segments.ContainsKey(segmentKey))
            {
              IDictionary<string, List<MapPoint[]>> dictionary = segments;
              string key = segmentKey;
              int capacity = 1;
              List<MapPoint[]> mapPointArrayList = new List<MapPoint[]>(capacity);
              CollectionsMarshal.SetCount<MapPoint[]>(mapPointArrayList, capacity);
              CollectionsMarshal.AsSpan<MapPoint[]>(mapPointArrayList)[0] = array;
              dictionary[key] = mapPointArrayList;
            }
            else if (!MapPathPruning.AnyOverlappingSegments((IEnumerable<MapPoint[]>) segments[segmentKey], (IReadOnlyList<MapPoint>) array))
              segments[segmentKey].Add(array);
          }
        }
      }
    }
  }

  private static bool IsValidSegmentStartMapPoint(MapPoint startMapPoint)
  {
    return startMapPoint.Children.Count > 1 || startMapPoint.coord.row == 0;
  }

  private static bool IsValidSegmentEndMapPoint(MapPoint endMapPoint)
  {
    return endMapPoint.parents.Count >= 2;
  }

  private static string GenerateSegmentKey(IReadOnlyList<MapPoint> segment)
  {
    StringBuilder stringBuilder1 = new StringBuilder();
    MapPoint mapPoint1 = segment[0];
    IReadOnlyList<MapPoint> mapPointList = segment;
    MapPoint mapPoint2 = mapPointList[mapPointList.Count - 1];
    if (mapPoint1.coord.row == 0)
    {
      StringBuilder stringBuilder2 = stringBuilder1;
      StringBuilder stringBuilder3 = stringBuilder2;
      StringBuilder.AppendInterpolatedStringHandler interpolatedStringHandler;
      // ISSUE: explicit constructor call
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(3, 3, stringBuilder2);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<int>(mapPoint1.coord.row);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("-");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<int>(mapPoint2.coord.col);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(",");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<int>(mapPoint2.coord.row);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("-");
      ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
      stringBuilder3.Append(ref local);
    }
    else
    {
      StringBuilder stringBuilder4 = stringBuilder1;
      StringBuilder stringBuilder5 = stringBuilder4;
      StringBuilder.AppendInterpolatedStringHandler interpolatedStringHandler;
      // ISSUE: explicit constructor call
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(4, 4, stringBuilder4);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<int>(mapPoint1.coord.col);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(",");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<int>(mapPoint1.coord.row);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("-");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<int>(mapPoint2.coord.col);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(",");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<int>(mapPoint2.coord.row);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("-");
      ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
      stringBuilder5.Append(ref local);
    }
    stringBuilder1.Append(string.Join<int>(",", segment.Select<MapPoint, int>((Func<MapPoint, int>) (point => (int) point.PointType))));
    return stringBuilder1.ToString();
  }

  private static bool AnyOverlappingSegments(
    IEnumerable<MapPoint[]> existingSegments,
    IReadOnlyList<MapPoint> segment)
  {
    return existingSegments.Any<MapPoint[]>((Func<MapPoint[], bool>) (existingSegment => MapPathPruning.OverlappingSegment((IReadOnlyList<MapPoint>) existingSegment, segment)));
  }

  private static bool OverlappingSegment(IReadOnlyList<MapPoint> a, IReadOnlyList<MapPoint> b)
  {
    if (a.Count < 3 || b.Count < 3)
      return false;
    for (int index = 1; index <= a.Count - 2; ++index)
    {
      if (object.Equals((object) a[index], (object) b[index]))
        return true;
    }
    return false;
  }

  private static List<List<MapPoint[]>> GetDuplicateSegments(
    IDictionary<string, List<MapPoint[]>> segments)
  {
    return segments.Values.Where<List<MapPoint[]>>((Func<List<MapPoint[]>, bool>) (segmentList => segmentList.Count > 1)).ToList<List<MapPoint[]>>();
  }

  private static bool PrunePaths(
    MapPoint?[,] grid,
    HashSet<MapPoint> startMapPoints,
    IEnumerable<List<MapPoint[]>> matchingSegments,
    Rng rng)
  {
    foreach (List<MapPoint[]> matchingSegment in matchingSegments)
    {
      matchingSegment.UnstableShuffle<MapPoint[]>(rng);
      if (MapPathPruning.PruneAllButLast(grid, startMapPoints, (IReadOnlyList<MapPoint[]>) matchingSegment) != 0 || MapPathPruning.BreakAParentChildRelationshipInAnySegment(matchingSegment))
        return true;
    }
    return false;
  }

  private static int PruneAllButLast(
    MapPoint?[,] grid,
    HashSet<MapPoint> startMapPoints,
    IReadOnlyList<MapPoint[]> matches)
  {
    int num = 0;
    foreach (MapPoint[] match in (IEnumerable<MapPoint[]>) matches)
    {
      if (num == matches.Count - 1)
        return num;
      if (MapPathPruning.PruneSegment(grid, startMapPoints, match))
        ++num;
    }
    return num;
  }

  private static bool PruneSegment(
    MapPoint?[,] grid,
    HashSet<MapPoint> startMapPoints,
    MapPoint[] segment)
  {
    bool flag = false;
    for (int count = 0; count < segment.Length - 1; ++count)
    {
      MapPoint mapPoint = segment[count];
      if (!MapPathPruning.IsInMap(grid, mapPoint))
        return true;
      if (mapPoint.Children.Count <= 1 && mapPoint.parents.Count <= 1 && !mapPoint.parents.Any<MapPoint>((Func<MapPoint, bool>) (n => n.Children.Count == 1 && !MapPathPruning.IsRemoved(grid, n))) && !((IEnumerable<MapPoint>) ((IEnumerable<MapPoint>) segment).Skip<MapPoint>(count).ToArray<MapPoint>()).Any<MapPoint>((Func<MapPoint, bool>) (n => n.Children.Count > 1 && n.parents.Count == 1)))
      {
        MapPoint[] mapPointArray = segment;
        if (mapPointArray[mapPointArray.Length - 1].parents.Count == 1)
          return false;
        if (!mapPoint.Children.Where<MapPoint>((Func<MapPoint, bool>) (c => !((IEnumerable<MapPoint>) segment).Contains<MapPoint>(c))).Any<MapPoint>((Func<MapPoint, bool>) (c => c.parents.Count == 1)))
        {
          MapPathPruning.RemovePoint(grid, startMapPoints, mapPoint);
          flag = true;
        }
      }
    }
    return flag;
  }

  private static void RemovePoint(
    MapPoint?[,] grid,
    HashSet<MapPoint> startMapPoints,
    MapPoint mapPoint)
  {
    grid[mapPoint.coord.col, mapPoint.coord.row] = (MapPoint) null;
    startMapPoints.Remove(mapPoint);
    foreach (MapPoint child in mapPoint.Children.ToList<MapPoint>())
      mapPoint.RemoveChildPoint(child);
    foreach (MapPoint mapPoint1 in mapPoint.parents.ToList<MapPoint>())
      mapPoint1.RemoveChildPoint(mapPoint);
  }

  private static bool IsInMap(MapPoint?[,] grid, MapPoint mapPoint)
  {
    return grid[mapPoint.coord.col, mapPoint.coord.row] != null || mapPoint.PointType == MapPointType.Ancient || mapPoint.PointType == MapPointType.Boss;
  }

  private static bool IsRemoved(MapPoint?[,] grid, MapPoint mapPoint)
  {
    return grid[mapPoint.coord.col, mapPoint.coord.row] == null;
  }

  private static bool BreakAParentChildRelationshipInAnySegment(List<MapPoint[]> matches)
  {
    foreach (MapPoint[] match in matches)
    {
      if (MapPathPruning.BreakAParentChildRelationshipInSegment(match))
        return true;
    }
    return false;
  }

  private static bool BreakAParentChildRelationshipInSegment(MapPoint[] segment)
  {
    bool flag = false;
    for (int index = 0; index < segment.Length - 1; ++index)
    {
      MapPoint mapPoint = segment[index];
      if (mapPoint.Children.Count >= 2)
      {
        MapPoint child = segment[index + 1];
        if (child.parents.Count != 1)
        {
          mapPoint.RemoveChildPoint(child);
          flag = true;
        }
      }
    }
    return flag;
  }
}
