// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Map.ActMap
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Map;

public abstract class ActMap
{
  public readonly HashSet<MapPoint> startMapPoints = new HashSet<MapPoint>();

  public abstract MapPoint BossMapPoint { get; }

  public abstract MapPoint StartingMapPoint { get; }

  public virtual MapPoint? SecondBossMapPoint => (MapPoint) null;

  protected abstract MapPoint?[,] Grid { get; }

  public int GetColumnCount() => this.Grid.GetLength(0);

  public int GetRowCount() => this.Grid.GetLength(1);

  public IEnumerable<MapPoint> GetAllMapPoints()
  {
    for (int c = 0; c < this.GetColumnCount(); ++c)
    {
      for (int r = 0; r < this.Grid.GetLength(1); ++r)
      {
        MapPoint allMapPoint = this.Grid[c, r];
        if (allMapPoint != null)
          yield return allMapPoint;
      }
    }
  }

  public IEnumerable<MapPoint> GetPointsInRow(int row)
  {
    if (row >= 0 && row < this.Grid.GetLength(1))
    {
      for (int c = 0; c < this.GetColumnCount(); ++c)
      {
        MapPoint mapPoint = this.Grid[c, row];
        if (mapPoint != null)
          yield return mapPoint;
      }
    }
  }

  public virtual MapPoint? GetPoint(MapCoord coord) => this.GetPoint(coord.col, coord.row);

  public MapPoint? GetPoint(int col, int row)
  {
    if (col == this.BossMapPoint.coord.col && row == this.BossMapPoint.coord.row)
      return this.BossMapPoint;
    if (this.SecondBossMapPoint != null && col == this.SecondBossMapPoint.coord.col && row == this.SecondBossMapPoint.coord.row)
      return this.SecondBossMapPoint;
    if (col == this.StartingMapPoint.coord.col && row == this.StartingMapPoint.coord.row)
      return this.StartingMapPoint;
    return col >= 0 && col < this.Grid.GetLength(0) && row >= 0 && row < this.Grid.GetLength(1) ? this.Grid[col, row] : (MapPoint) null;
  }

  public bool IsInMap(MapPoint mapPoint)
  {
    if (mapPoint.PointType == MapPointType.Ancient || mapPoint.PointType == MapPointType.Boss)
      return true;
    int col = mapPoint.coord.col;
    int row = mapPoint.coord.row;
    return col >= 0 && col < this.Grid.GetLength(0) && row >= 0 && row < this.Grid.GetLength(1) && this.Grid[col, row] != null;
  }

  public bool HasPoint(MapCoord coord)
  {
    if (coord.col == this.BossMapPoint.coord.col && coord.row == this.BossMapPoint.coord.row || this.SecondBossMapPoint != null && coord.col == this.SecondBossMapPoint.coord.col && coord.row == this.SecondBossMapPoint.coord.row || coord.col == this.StartingMapPoint.coord.col && coord.row == this.StartingMapPoint.coord.row)
      return true;
    return coord.col >= 0 && coord.col < this.Grid.GetLength(0) && coord.row >= 0 && coord.row < this.Grid.GetLength(1) && this.Grid[coord.col, coord.row] != null;
  }
}
