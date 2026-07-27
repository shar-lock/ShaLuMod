// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Map.MockCraftedActMap
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

#nullable enable
namespace MegaCrit.Sts2.Core.Map;

public sealed class MockCraftedActMap : ActMap
{
  public override MapPoint BossMapPoint { get; }

  public override MapPoint StartingMapPoint { get; }

  protected override MapPoint?[,] Grid { get; }

  public MockCraftedActMap(int width, int height, MapPoint startingPoint, MapPoint bossPoint)
  {
    this.Grid = new MapPoint[width, height];
    this.StartingMapPoint = startingPoint;
    this.BossMapPoint = bossPoint;
  }

  public void Put(int col, int row, MapPointType type = MapPointType.Monster)
  {
    MapPoint mapPoint = new MapPoint(col, row)
    {
      PointType = type
    };
    this.Grid[col, row] = mapPoint;
  }
}
