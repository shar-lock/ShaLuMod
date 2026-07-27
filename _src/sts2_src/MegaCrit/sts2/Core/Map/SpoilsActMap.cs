// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Map.SpoilsActMap
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Map;

public sealed class SpoilsActMap : ActMap
{
  private const int _mapWidth = 7;
  private const int _pathCount = 7;
  private readonly int _mapLength;
  private readonly Rng _rng;
  private readonly int _treasureRow;
  private readonly MapPointTypeCounts _pointTypeCounts;
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

  protected override MapPoint?[,] Grid { get; }

  public SpoilsActMap(IRunState runState, MapPointTypeCounts? mapPointTypeCountsOverride = null)
  {
    ActModel act = runState.Act;
    bool isMultiplayer = runState.Players.Count > 1;
    this._mapLength = act.GetNumberOfRooms(isMultiplayer) + 1;
    this._rng = new Rng(runState.Rng.Seed, "spoils_map");
    this._pointTypeCounts = mapPointTypeCountsOverride ?? act.GetMapPointTypes(this._rng);
    this.Grid = new MapPoint[7, this._mapLength];
    this._treasureRow = this.GetRowCount() - 7;
    this.BossMapPoint = new MapPoint(this.GetColumnCount() / 2, this.GetRowCount());
    this.StartingMapPoint = new MapPoint(this.GetColumnCount() / 2, 0);
    this.GenerateHourglassMap();
    this.AssignPointTypes();
    MapPathPruning.PruneAndRepair(this.Grid, this.startMapPoints, (ActMap) this, this._pointTypeCounts, this._rng, new Func<MapPointType, MapPoint, bool>(this.IsValidPointType));
  }

  private MapPoint GetOrCreatePoint(int col, int row)
  {
    if (col >= 0 && col < this.GetColumnCount() && row >= 0 && row < this.GetRowCount())
    {
      MapPoint point = this.Grid[col, row];
      if (point != null)
        return point;
    }
    MapPoint point1 = new MapPoint(col, row);
    this.Grid[col, row] = point1;
    return point1;
  }

  private void GenerateHourglassMap()
  {
    int col = this.GetColumnCount() / 2;
    if (this._treasureRow <= 0 || this._treasureRow >= this.GetRowCount())
      throw new InvalidOperationException("Treasure row is out of bounds for SpoilsActMap");
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
    MapPoint point1 = this.GetOrCreatePoint(col, this._treasureRow);
    point1.PointType = MapPointType.Treasure;
    point1.CanBeModified = false;
    foreach (MapPoint strayNode in this.GetPointsInRow(this._treasureRow).ToList<MapPoint>())
    {
      if (strayNode != point1)
        this.RedirectToTreasure(strayNode, point1);
    }
    this.ConnectRowToBoss();
    this.ConnectRowToStart();
  }

  private void PathGenerate(MapPoint startingPoint)
  {
    MapPoint point;
    for (MapPoint current = startingPoint; current.coord.row < this._mapLength - 1; current = point)
    {
      MapCoord nextCoord = this.GenerateNextCoord(current);
      point = this.GetOrCreatePoint(nextCoord.col, nextCoord.row);
      current.AddChildPoint(point);
    }
  }

  private MapCoord GenerateNextCoord(MapPoint current)
  {
    int row = current.coord.row + 1;
    (int minCol, int maxCol) = this.GetAllowedColumnsForRow(row);
    int centerCol = this.GetColumnCount() / 2;
    List<int> list = new List<int>() { -1, 0, 1 };
    int num = this._treasureRow - current.coord.row;
    if (num > 3)
      list.StableShuffle<int>(this._rng);
    else if (num > 0)
      list = this.BuildCenteredPriorityList(current.coord.col, centerCol);
    else
      list.StableShuffle<int>(this._rng);
    foreach (int direction in list)
    {
      int nextColumn = this.GetNextColumn(current.coord.col, direction);
      if (nextColumn >= minCol && nextColumn <= maxCol && !this.HasInvalidCrossover(current, nextColumn))
      {
        MapPoint point = this.GetPoint(nextColumn, row);
        if ((point == null || point.parents.Contains(current) || point.parents.Count < 3) && (current == this.StartingMapPoint || current.Children.Count < 3 || point != null && current.Children.Contains(point)))
        {
          if (Math.Abs(nextColumn - current.coord.col) > 1)
            throw new InvalidOperationException($"Invalid step from ({current.coord.col}, {current.coord.row}) to column {nextColumn}");
          MapCoord nextCoord = new MapCoord();
          nextCoord.col = nextColumn;
          nextCoord.row = row;
          nextCoord = nextCoord;
          return nextCoord;
        }
      }
    }
    int targetCol = Math.Clamp(centerCol, minCol, maxCol);
    if (Math.Abs(targetCol - current.coord.col) > 1)
      targetCol = Math.Clamp(current.coord.col + Math.Sign(targetCol - current.coord.col), minCol, maxCol);
    if (this.HasInvalidCrossover(current, targetCol))
      targetCol = Math.Clamp(current.coord.col, minCol, maxCol);
    if (Math.Abs(targetCol - current.coord.col) > 1)
      throw new InvalidOperationException($"Fallback step from ({current.coord.col}, {current.coord.row}) to column {targetCol} exceeds adjacency");
    return new MapCoord() { col = targetCol, row = row };
  }

  private List<int> BuildCenteredPriorityList(int currentCol, int centerCol)
  {
    List<int> intList = new List<int>();
    int num1 = Math.Sign(centerCol - currentCol);
    if (num1 != 0)
      intList.Add(num1);
    intList.Add(0);
    int num2 = -num1;
    if (num1 != 0)
      intList.Add(num2);
    if (!intList.Contains(-1))
      intList.Add(-1);
    if (!intList.Contains(1))
      intList.Add(1);
    return intList;
  }

  private int GetNextColumn(int currentCol, int direction)
  {
    int nextColumn;
    switch (direction)
    {
      case -1:
        nextColumn = Math.Max(0, currentCol - 1);
        break;
      case 0:
        nextColumn = currentCol;
        break;
      case 1:
        nextColumn = Math.Min(6, currentCol + 1);
        break;
      default:
        nextColumn = currentCol;
        break;
    }
    return nextColumn;
  }

  private (int minCol, int maxCol) GetAllowedColumnsForRow(int row)
  {
    int num1 = this.GetColumnCount() / 2;
    int num2 = Math.Abs(row - this._treasureRow);
    int num3 = this._mapLength - 1 - row;
    int num4 = Math.Min(num1, Math.Max(0, num3) + 1);
    int num5 = Math.Min(num1, Math.Min(num2, num4));
    return (Math.Max(0, num1 - num5), Math.Min(6, num1 + num5));
  }

  private bool HasInvalidCrossover(MapPoint current, int targetCol)
  {
    int num = targetCol - current.coord.col;
    if (num == 0)
      return false;
    MapPoint point = this.GetPoint(targetCol, current.coord.row);
    if (point == null)
      return false;
    foreach (MapPoint child in point.Children)
    {
      if (child.coord.col - point.coord.col == -num)
        return true;
    }
    return false;
  }

  private void RedirectToTreasure(MapPoint strayNode, MapPoint treasure)
  {
    foreach (MapPoint mapPoint in strayNode.parents.ToList<MapPoint>())
    {
      mapPoint.RemoveChildPoint(strayNode);
      mapPoint.AddChildPoint(treasure);
    }
    foreach (MapPoint child in strayNode.Children.ToList<MapPoint>())
    {
      strayNode.RemoveChildPoint(child);
      treasure.AddChildPoint(child);
    }
    this.Grid[strayNode.coord.col, strayNode.coord.row] = (MapPoint) null;
  }

  private void ConnectRowToBoss()
  {
    int index1 = this.GetRowCount() - 1;
    for (int index2 = 0; index2 < this.GetColumnCount(); ++index2)
    {
      MapPoint mapPoint = this.Grid[index2, index1];
      if (mapPoint != null && !mapPoint.Children.Contains(this.BossMapPoint))
        mapPoint.AddChildPoint(this.BossMapPoint);
    }
  }

  private void ConnectRowToStart()
  {
    for (int index = 0; index < this.GetColumnCount(); ++index)
    {
      MapPoint child = this.Grid[index, 1];
      if (child != null && !this.StartingMapPoint.Children.Contains(child))
        this.StartingMapPoint.AddChildPoint(child);
    }
  }

  private void AssignPointTypes()
  {
    this.ForEachInRow(this.GetRowCount() - 1, (Action<MapPoint>) (p =>
    {
      p.PointType = MapPointType.RestSite;
      p.CanBeModified = false;
    }));
    this.ForEachInRow(1, (Action<MapPoint>) (p =>
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
    foreach (MapPoint mapPoint in this.GetAllMapPoints().Where<MapPoint>((Func<MapPoint, bool>) (p => p.PointType == MapPointType.Unassigned)))
      mapPoint.PointType = MapPointType.Monster;
    this.BossMapPoint.PointType = MapPointType.Boss;
    this.StartingMapPoint.PointType = MapPointType.Ancient;
  }

  private void ForEachInRow(int rowIndex, Action<MapPoint> processor)
  {
    for (int index = 0; index < this.GetColumnCount(); ++index)
    {
      MapPoint mapPoint = this.Grid[index, rowIndex];
      if (mapPoint != null)
        processor(mapPoint);
    }
  }

  private void AssignRemainingTypesToRandomPoints(Queue<MapPointType> pointTypesToBeAssigned)
  {
    for (int index = 0; index < 3 && pointTypesToBeAssigned.Count > 0; ++index)
    {
      List<MapPoint> list = this.GetAllMapPoints().Where<MapPoint>((Func<MapPoint, bool>) (p => p != this.BossMapPoint && p != this.StartingMapPoint)).Where<MapPoint>((Func<MapPoint, bool>) (p => p.PointType == MapPointType.Unassigned)).ToList<MapPoint>();
      list.StableShuffle<MapPoint>(this._rng);
      foreach (MapPoint mapPoint in list)
      {
        if (pointTypesToBeAssigned.Count != 0)
        {
          MapPointType nextValidPointType = this.GetNextValidPointType(pointTypesToBeAssigned, mapPoint);
          if (nextValidPointType != MapPointType.Unassigned)
            mapPoint.PointType = nextValidPointType;
        }
        else
          break;
      }
    }
  }

  private MapPointType GetNextValidPointType(Queue<MapPointType> pointTypesQueue, MapPoint mapPoint)
  {
    if (pointTypesQueue.Count == 0)
      return MapPointType.Unassigned;
    int count = pointTypesQueue.Count;
    for (int index = 0; index < count; ++index)
    {
      MapPointType pointType = pointTypesQueue.Dequeue();
      if (this._pointTypeCounts.ShouldIgnoreMapPointRulesForMapPointType(pointType) || this.IsValidPointType(pointType, mapPoint))
        return pointType;
      pointTypesQueue.Enqueue(pointType);
    }
    return MapPointType.Unassigned;
  }

  private bool IsValidPointType(MapPointType pointType, MapPoint mapPoint)
  {
    return this.IsValidForUpper(pointType, mapPoint) && SpoilsActMap.IsValidForLower(pointType, mapPoint) && SpoilsActMap.IsValidWithParents(pointType, mapPoint) && SpoilsActMap.IsValidWithChildren(pointType, mapPoint) && SpoilsActMap.IsValidWithSiblings(pointType, mapPoint);
  }

  private static bool IsValidForLower(MapPointType pointType, MapPoint mapPoint)
  {
    return mapPoint.coord.row >= 6 || !SpoilsActMap._lowerMapPointRestrictions.Contains(pointType);
  }

  private bool IsValidForUpper(MapPointType pointType, MapPoint mapPoint)
  {
    return mapPoint.coord.row < this._mapLength - 3 || !SpoilsActMap._upperMapPointRestrictions.Contains(pointType);
  }

  private static bool IsValidWithParents(MapPointType pointType, MapPoint mapPoint)
  {
    return !SpoilsActMap._parentMapPointRestrictions.Contains(pointType) || !mapPoint.parents.Concat<MapPoint>((IEnumerable<MapPoint>) mapPoint.Children).Any<MapPoint>((Func<MapPoint, bool>) (p => pointType == p.PointType));
  }

  private static bool IsValidWithChildren(MapPointType pointType, MapPoint mapPoint)
  {
    return !SpoilsActMap._childMapPointRestrictions.Contains(pointType) || !mapPoint.Children.Any<MapPoint>((Func<MapPoint, bool>) (p => pointType == p.PointType));
  }

  private static bool IsValidWithSiblings(MapPointType pointType, MapPoint mapPoint)
  {
    return !SpoilsActMap._siblingPointTypeRestrictions.Contains(pointType) || !SpoilsActMap.GetSiblings(mapPoint).Any<MapPoint>((Func<MapPoint, bool>) (p => pointType == p.PointType));
  }

  private static IEnumerable<MapPoint> GetSiblings(MapPoint mapPoint)
  {
    return mapPoint.parents.SelectMany<MapPoint, MapPoint>((Func<MapPoint, IEnumerable<MapPoint>>) (x => (IEnumerable<MapPoint>) x.Children)).Where<MapPoint>((Func<MapPoint, bool>) (x => !object.Equals((object) x, (object) mapPoint)));
  }
}
