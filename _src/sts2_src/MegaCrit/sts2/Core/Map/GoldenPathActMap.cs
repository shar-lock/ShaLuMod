// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Map.GoldenPathActMap
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Runs;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Map;

public sealed class GoldenPathActMap : ActMap
{
  private readonly MapPointType[] _defaultPointTypes = new MapPointType[16 /*0x10*/]
  {
    MapPointType.Monster,
    MapPointType.Unknown,
    MapPointType.Monster,
    MapPointType.RestSite,
    MapPointType.Monster,
    MapPointType.RestSite,
    MapPointType.Unknown,
    MapPointType.Treasure,
    MapPointType.Unknown,
    MapPointType.Treasure,
    MapPointType.Unknown,
    MapPointType.Shop,
    MapPointType.Elite,
    MapPointType.RestSite,
    MapPointType.Elite,
    MapPointType.RestSite
  };
  private const int _width = 7;
  private const int _middle = 3;

  public override MapPoint BossMapPoint { get; }

  public override MapPoint StartingMapPoint { get; }

  protected override MapPoint?[,] Grid { get; }

  public GoldenPathActMap(IRunState runState)
  {
    List<MapPointType> list = ((IEnumerable<MapPointType>) this._defaultPointTypes).ToList<MapPointType>();
    if (runState.Players.Count > 1)
      list.RemoveAt(2);
    this.Grid = new MapPoint[7, list.Count + 1];
    this.BossMapPoint = new MapPoint(this.GetColumnCount() / 2, this.GetRowCount())
    {
      PointType = MapPointType.Boss
    };
    this.StartingMapPoint = new MapPoint(this.GetColumnCount() / 2, 0)
    {
      PointType = MapPointType.Ancient
    };
    for (int index = 0; index < list.Count; ++index)
    {
      MapPoint child = new MapPoint(3, index + 1);
      this.Grid[3, index + 1] = child;
      child.PointType = list[index];
      if (index > 0)
        this.Grid[3, index].AddChildPoint(child);
    }
    this.startMapPoints.Add(this.Grid[3, 1]);
    this.Grid[3, this.GetRowCount() - 1].AddChildPoint(this.BossMapPoint);
    this.StartingMapPoint.AddChildPoint(this.Grid[3, 1]);
  }
}
