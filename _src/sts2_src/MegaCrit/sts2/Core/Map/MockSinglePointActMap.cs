// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Map.MockSinglePointActMap
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

#nullable enable
namespace MegaCrit.Sts2.Core.Map;

public class MockSinglePointActMap : ActMap
{
  private readonly MapPoint _currentMapPoint = new MapPoint(0, 0);

  public override MapPoint BossMapPoint { get; } = new MapPoint(0, 0)
  {
    PointType = MapPointType.Boss
  };

  public override MapPoint StartingMapPoint { get; } = new MapPoint(0, 0)
  {
    PointType = MapPointType.Ancient
  };

  protected override MapPoint?[,] Grid { get; } = new MapPoint[0, 0];

  public override MapPoint GetPoint(MapCoord coord) => this._currentMapPoint;

  public void MockCurrentMapPointType(MapPointType pointType)
  {
    this._currentMapPoint.PointType = pointType;
  }
}
