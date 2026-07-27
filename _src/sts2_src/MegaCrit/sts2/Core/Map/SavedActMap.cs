// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Map.SavedActMap
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Saves.Runs;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Map;

public sealed class SavedActMap : ActMap
{
  public override MapPoint BossMapPoint { get; }

  public override MapPoint StartingMapPoint { get; }

  public override MapPoint? SecondBossMapPoint { get; }

  protected override MapPoint?[,] Grid { get; }

  public SavedActMap(SerializableActMap saved)
  {
    this.Grid = new MapPoint[saved.GridWidth, saved.GridHeight];
    Dictionary<MapCoord, MapPoint> lookup = new Dictionary<MapCoord, MapPoint>();
    foreach (SerializableMapPoint point1 in saved.Points)
    {
      MapPoint point2 = SavedActMap.CreatePoint(point1);
      this.Grid[point1.Coord.col, point1.Coord.row] = point2;
      lookup[point1.Coord] = point2;
    }
    this.BossMapPoint = SavedActMap.CreatePoint(saved.BossPoint);
    lookup[saved.BossPoint.Coord] = this.BossMapPoint;
    this.StartingMapPoint = SavedActMap.CreatePoint(saved.StartingPoint);
    lookup[saved.StartingPoint.Coord] = this.StartingMapPoint;
    if (saved.SecondBossPoint != null)
    {
      this.SecondBossMapPoint = SavedActMap.CreatePoint(saved.SecondBossPoint);
      lookup[saved.SecondBossPoint.Coord] = this.SecondBossMapPoint;
    }
    SavedActMap.WireChildren((IEnumerable<SerializableMapPoint>) saved.Points, lookup);
    SavedActMap.WireChildren(saved.BossPoint, lookup);
    SavedActMap.WireChildren(saved.StartingPoint, lookup);
    if (saved.SecondBossPoint != null)
      SavedActMap.WireChildren(saved.SecondBossPoint, lookup);
    if (saved.StartMapPointCoords == null)
      return;
    foreach (MapCoord startMapPointCoord in saved.StartMapPointCoords)
    {
      MapPoint mapPoint;
      if (lookup.TryGetValue(startMapPointCoord, out mapPoint))
        this.startMapPoints.Add(mapPoint);
    }
  }

  private static MapPoint CreatePoint(SerializableMapPoint saved)
  {
    return new MapPoint(saved.Coord.col, saved.Coord.row)
    {
      PointType = saved.PointType,
      CanBeModified = saved.CanBeModified
    };
  }

  private static void WireChildren(
    IEnumerable<SerializableMapPoint> points,
    Dictionary<MapCoord, MapPoint> lookup)
  {
    foreach (SerializableMapPoint point in points)
      SavedActMap.WireChildren(point, lookup);
  }

  private static void WireChildren(
    SerializableMapPoint savedPoint,
    Dictionary<MapCoord, MapPoint> lookup)
  {
    if (savedPoint.ChildCoords == null)
      return;
    MapPoint mapPoint = lookup[savedPoint.Coord];
    foreach (MapCoord childCoord in savedPoint.ChildCoords)
    {
      MapPoint child;
      if (lookup.TryGetValue(childCoord, out child))
        mapPoint.AddChildPoint(child);
    }
  }
}
