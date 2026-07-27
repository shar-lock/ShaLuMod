// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Map.StandardActMap
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

#nullable enable
namespace MegaCrit.Sts2.Core.Map;

public sealed class StandardActMap : ActMap
{
  public const int maxElites = 15;
  private const int _iterations = 7;
  private const int _mapWidth = 7;
  private readonly MapPointTypeCounts _pointTypeCounts;
  private readonly int _mapLength;
  private readonly Rng _rng;
  private static readonly HashSet<MapPointType> _lowerMapPointRestrictions = new HashSet<MapPointType>()
  {
    MapPointType.RestSite,
    MapPointType.Elite
  };
  private static readonly HashSet<MapPointType> _upperMapPointRestrictions = new HashSet<MapPointType>()
  {
    MapPointType.RestSite
  };
  private static readonly HashSet<MapPointType> _parentMapPointRestrictions = new HashSet<MapPointType>()
  {
    MapPointType.Elite,
    MapPointType.RestSite,
    MapPointType.Treasure,
    MapPointType.Shop
  };
  private static readonly HashSet<MapPointType> _childMapPointRestrictions = new HashSet<MapPointType>()
  {
    MapPointType.Elite,
    MapPointType.RestSite,
    MapPointType.Treasure,
    MapPointType.Shop
  };
  private static readonly HashSet<MapPointType> _siblingPointTypeRestrictions = new HashSet<MapPointType>()
  {
    MapPointType.RestSite,
    MapPointType.Monster,
    MapPointType.Unknown,
    MapPointType.Elite,
    MapPointType.Shop
  };

  public override MapPoint BossMapPoint { get; }

  public override MapPoint StartingMapPoint { get; }

  public override MapPoint? SecondBossMapPoint { get; }

  public bool ShouldReplaceTreasureWithElites { get; }

  protected override MapPoint?[,] Grid { get; }

  public StandardActMap(
    Rng mapRng,
    ActModel actModel,
    bool isMultiplayer,
    bool shouldReplaceTreasureWithElites,
    bool hasSecondBoss = false,
    MapPointTypeCounts? mapPointTypeCountsOverride = null,
    bool enablePruning = true)
  {
    this._mapLength = actModel.GetNumberOfRooms(isMultiplayer) + 1;
    this.ShouldReplaceTreasureWithElites = shouldReplaceTreasureWithElites;
    this.Grid = new MapPoint[7, this._mapLength];
    this._rng = mapRng;
    this._pointTypeCounts = mapPointTypeCountsOverride ?? actModel.GetMapPointTypes(mapRng);
    this.BossMapPoint = new MapPoint(this.GetColumnCount() / 2, this.GetRowCount());
    this.StartingMapPoint = new MapPoint(this.GetColumnCount() / 2, 0);
    if (hasSecondBoss)
      this.SecondBossMapPoint = new MapPoint(this.GetColumnCount() / 2, this.GetRowCount() + 1);
    this.GenerateMap();
    this.AssignPointTypes();
    if (enablePruning)
      MapPathPruning.PruneAndRepair(this.Grid, this.startMapPoints, (ActMap) this, this._pointTypeCounts, this._rng, new Func<MapPointType, MapPoint, bool>(this.IsValidPointType));
    this.Grid = MapPostProcessing.CenterGrid(this.Grid);
    this.Grid = MapPostProcessing.SpreadAdjacentMapPoints(this.Grid);
    this.Grid = MapPostProcessing.StraightenPaths(this.Grid);
  }

  public static StandardActMap CreateFor(RunState runState, bool replaceTreasureWithElites)
  {
    return new StandardActMap(new Rng(runState.Rng.Seed, $"act_{runState.CurrentActIndex + 1}_map"), runState.Act, runState.Players.Count > 1, replaceTreasureWithElites, runState.Act.HasSecondBoss);
  }

  private MapPoint GetOrCreateMapPoint(MapCoord coord)
  {
    return this.GetOrCreatePoint(coord.col, coord.row);
  }

  private MapPoint GetOrCreatePoint(int col, int row)
  {
    MapPoint point1 = this.GetPoint(col, row);
    if (point1 != null)
      return point1;
    MapPoint point2 = new MapPoint(col, row);
    this.Grid[col, row] = point2;
    return point2;
  }

  private void PathGenerate(MapPoint startingPoint)
  {
    MapPoint mapPoint;
    for (MapPoint current = startingPoint; current.coord.row < this._mapLength - 1; current = mapPoint)
    {
      mapPoint = this.GetOrCreateMapPoint(this.GenerateNextCoord(current));
      current.AddChildPoint(mapPoint);
    }
  }

  private MapCoord GenerateNextCoord(MapPoint current)
  {
    int col = current.coord.col;
    int num1 = Mathf.Max(0, col - 1);
    int num2 = Mathf.Min(col + 1, 6);
    int capacity = 3;
    List<int> intList = new List<int>(capacity);
    CollectionsMarshal.SetCount<int>(intList, capacity);
    Span<int> span = CollectionsMarshal.AsSpan<int>(intList);
    int num3 = 0;
    span[num3] = -1;
    int num4 = num3 + 1;
    span[num4] = 0;
    int num5 = num4 + 1;
    span[num5] = 1;
    List<int> list = intList;
    list.StableShuffle<int>(this._rng);
    foreach (int num6 in list)
    {
      int num7 = current.coord.row + 1;
      int num8;
      switch (num6)
      {
        case -1:
          num8 = num1;
          break;
        case 0:
          num8 = col;
          break;
        case 1:
          num8 = num2;
          break;
        default:
          throw new InvalidOperationException("This isn't possible");
      }
      int targetX = num8;
      if (!this.HasInvalidCrossover(current, targetX))
        return new MapCoord() { col = targetX, row = num7 };
    }
    throw new InvalidOperationException("Cannot find next node");
  }

  private bool HasInvalidCrossover(MapPoint current, int targetX)
  {
    int num = targetX - current.coord.col;
    bool flag;
    switch (num)
    {
      case 0:
      case 7:
        flag = true;
        break;
      default:
        flag = false;
        break;
    }
    if (flag)
      return false;
    MapPoint mapPoint = this.Grid[targetX, current.coord.row];
    if (mapPoint == null)
      return false;
    foreach (MapPoint child in mapPoint.Children)
    {
      if (child.coord.col - mapPoint.coord.col == -num)
        return true;
    }
    return false;
  }

  private void GenerateMap()
  {
    for (int index = 0; index < 7; ++index)
    {
      MapPoint point = this.GetOrCreatePoint(this._rng.NextInt(0, 7), 1);
      if (index == 1)
      {
        while (this.startMapPoints.Contains(point))
          point = this.GetOrCreatePoint(this._rng.NextInt(0, 7), 1);
      }
      this.startMapPoints.Add(point);
      this.PathGenerate(point);
    }
    StandardActMap.ForEachInRow(this.Grid, this.GetRowCount() - 1, (Action<MapPoint>) (x => x.AddChildPoint(this.BossMapPoint)));
    if (this.SecondBossMapPoint != null)
      this.BossMapPoint.AddChildPoint(this.SecondBossMapPoint);
    StandardActMap.ForEachInRow(this.Grid, 1, (Action<MapPoint>) (x => this.StartingMapPoint.AddChildPoint(x)));
  }

  private static void ForEachInRow(
    MapPoint?[,] grid,
    int rowIndex,
    Action<MapPoint> processor,
    bool canBeModified = false)
  {
    for (int index = 0; index < grid.GetLength(0); ++index)
    {
      MapPoint mapPoint = grid[index, rowIndex];
      if (mapPoint != null)
        processor(mapPoint);
    }
  }

  private void AssignPointTypes()
  {
    StandardActMap.ForEachInRow(this.Grid, this.GetRowCount() - 1, (Action<MapPoint>) (p =>
    {
      p.PointType = MapPointType.RestSite;
      p.CanBeModified = false;
    }));
    if (this.ShouldReplaceTreasureWithElites)
      StandardActMap.ForEachInRow(this.Grid, this.GetRowCount() - 7, (Action<MapPoint>) (p =>
      {
        p.PointType = MapPointType.Elite;
        p.CanBeModified = false;
      }));
    else
      StandardActMap.ForEachInRow(this.Grid, this.GetRowCount() - 7, (Action<MapPoint>) (p =>
      {
        p.PointType = MapPointType.Treasure;
        p.CanBeModified = false;
      }));
    StandardActMap.ForEachInRow(this.Grid, 1, (Action<MapPoint>) (p =>
    {
      p.PointType = MapPointType.Monster;
      p.CanBeModified = false;
    }));
    List<MapPointType> collection = new List<MapPointType>();
    for (int index = 0; index < this._pointTypeCounts.NumOfRests; ++index)
      collection.Add(MapPointType.RestSite);
    for (int index = 0; index < this._pointTypeCounts.NumOfShops; ++index)
      collection.Add(MapPointType.Shop);
    for (int index = 0; index < this._pointTypeCounts.NumOfElites; ++index)
      collection.Add(MapPointType.Elite);
    for (int index = 0; index < this._pointTypeCounts.NumOfUnknowns; ++index)
      collection.Add(MapPointType.Unknown);
    this.AssignRemainingTypesToRandomPoints(new Queue<MapPointType>((IEnumerable<MapPointType>) collection));
    foreach (MapPoint mapPoint in this.GetAllMapPoints().Where<MapPoint>((Func<MapPoint, bool>) (x => x.PointType == MapPointType.Unassigned)))
      mapPoint.PointType = MapPointType.Monster;
    this.BossMapPoint.PointType = MapPointType.Boss;
    this.StartingMapPoint.PointType = MapPointType.Ancient;
    if (this.SecondBossMapPoint == null)
      return;
    this.SecondBossMapPoint.PointType = MapPointType.Boss;
  }

  private void EnsureRowsContainsPointType(MapPointType pointType, List<List<MapPoint>> rows)
  {
    if (StandardActMap.RowsContainPointType(pointType, (IEnumerable<List<MapPoint>>) rows))
      return;
    Queue<MapPointType> pointTypesToBeAssigned = new Queue<MapPointType>();
    pointTypesToBeAssigned.Enqueue(pointType);
    this.AssignPointTypesToRandomRows(pointTypesToBeAssigned, rows);
  }

  private static bool RowsContainPointType(MapPointType pointType, IEnumerable<List<MapPoint>> rows)
  {
    return rows.SelectMany<List<MapPoint>, MapPoint>((Func<List<MapPoint>, IEnumerable<MapPoint>>) (row => row.Where<MapPoint>((Func<MapPoint, bool>) (p => p.PointType == pointType)))).Any<MapPoint>();
  }

  private List<List<MapPoint>> GetRows(int firstRow, int lastRow)
  {
    List<List<MapPoint>> rows = new List<List<MapPoint>>();
    for (int index1 = firstRow; index1 <= lastRow; ++index1)
    {
      List<MapPoint> mapPointList = new List<MapPoint>();
      for (int index2 = 0; index2 < 7; ++index2)
      {
        MapPoint mapPoint = this.Grid[index2, index1];
        if (mapPoint != null)
          mapPointList.Add(mapPoint);
      }
      rows.Add(mapPointList);
    }
    return rows;
  }

  private void AssignPointTypesToRandomRows(
    Queue<MapPointType> pointTypesToBeAssigned,
    List<List<MapPoint>> rows)
  {
    rows.UnstableShuffle<List<MapPoint>>(this._rng);
    foreach (List<MapPoint> row in rows)
    {
      row.StableShuffle<MapPoint>(this._rng);
      using (IEnumerator<MapPoint> enumerator = row.Where<MapPoint>((Func<MapPoint, bool>) (r => r.PointType == MapPointType.Unassigned)).GetEnumerator())
      {
        if (enumerator.MoveNext())
        {
          MapPoint current = enumerator.Current;
          MapPointType nextValidPointType = this.GetNextValidPointType(pointTypesToBeAssigned, current);
          current.PointType = nextValidPointType;
        }
      }
    }
  }

  private void AssignRemainingTypesToRandomPoints(Queue<MapPointType> pointTypesToBeAssigned)
  {
    for (int index = 0; index < 3 && pointTypesToBeAssigned.Count > 0; ++index)
    {
      List<MapPoint> list = this.GetAllMapPoints().Where<MapPoint>((Func<MapPoint, bool>) (p => p.PointType == MapPointType.Unassigned)).ToList<MapPoint>();
      list.StableShuffle<MapPoint>(this._rng);
      foreach (MapPoint mapPoint in list)
      {
        if (pointTypesToBeAssigned.Count != 0)
          mapPoint.PointType = this.GetNextValidPointType(pointTypesToBeAssigned, mapPoint);
        else
          break;
      }
    }
  }

  private MapPointType GetNextValidPointType(Queue<MapPointType> pointTypesQueue, MapPoint mapPoint)
  {
    for (int index = 0; index < pointTypesQueue.Count; ++index)
    {
      MapPointType pointType = pointTypesQueue.Dequeue();
      if (this._pointTypeCounts.ShouldIgnoreMapPointRulesForMapPointType(pointType) || this.IsValidPointType(pointType, mapPoint))
        return pointType;
      pointTypesQueue.Enqueue(pointType);
    }
    return MapPointType.Unassigned;
  }

  public bool IsValidPointType(MapPointType pointType, MapPoint mapPoint)
  {
    return this.IsValidForUpper(pointType, mapPoint) && StandardActMap.IsValidForLower(pointType, mapPoint) && StandardActMap.IsValidWithParents(pointType, mapPoint) && StandardActMap.IsValidWithChildren(pointType, mapPoint) && StandardActMap.IsValidWithSiblings(pointType, mapPoint);
  }

  private static bool IsValidForLower(MapPointType pointType, MapPoint mapPoint)
  {
    return mapPoint.coord.row >= 6 || !StandardActMap._lowerMapPointRestrictions.Contains(pointType);
  }

  private bool IsValidForUpper(MapPointType pointType, MapPoint mapPoint)
  {
    return mapPoint.coord.row < this._mapLength - 3 || !StandardActMap._upperMapPointRestrictions.Contains(pointType);
  }

  private static bool IsValidWithParents(MapPointType pointType, MapPoint mapPoint)
  {
    return !StandardActMap._parentMapPointRestrictions.Contains(pointType) || !mapPoint.parents.Concat<MapPoint>((IEnumerable<MapPoint>) mapPoint.Children).Any<MapPoint>((Func<MapPoint, bool>) (p => pointType == p.PointType));
  }

  private static bool IsValidWithChildren(MapPointType pointType, MapPoint mapPoint)
  {
    return !StandardActMap._childMapPointRestrictions.Contains(pointType) || !mapPoint.Children.Any<MapPoint>((Func<MapPoint, bool>) (p => pointType == p.PointType));
  }

  private static bool IsValidWithSiblings(MapPointType pointType, MapPoint mapPoint)
  {
    return !StandardActMap._siblingPointTypeRestrictions.Contains(pointType) || !StandardActMap.GetSiblings(mapPoint).Any<MapPoint>((Func<MapPoint, bool>) (p => pointType == p.PointType));
  }

  private static IEnumerable<MapPoint> GetSiblings(MapPoint mapPoint)
  {
    return mapPoint.parents.SelectMany<MapPoint, MapPoint>((Func<MapPoint, IEnumerable<MapPoint>>) (x => (IEnumerable<MapPoint>) x.Children)).Where<MapPoint>((Func<MapPoint, bool>) (x => !object.Equals((object) x, (object) mapPoint)));
  }
}
