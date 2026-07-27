// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Map.MapPostProcessing
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Map;

public static class MapPostProcessing
{
  public static MapPoint?[,] CenterGrid(MapPoint?[,] grid)
  {
    int length1 = grid.GetLength(0);
    int length2 = grid.GetLength(1);
    bool flag1 = MapPostProcessing.IsColumnEmpty(grid, 0) && MapPostProcessing.IsColumnEmpty(grid, 1);
    bool flag2 = MapPostProcessing.IsColumnEmpty(grid, length1 - 1) && MapPostProcessing.IsColumnEmpty(grid, length1 - 2);
    int num = 0;
    if (flag1 && !flag2)
      num = -1;
    else if (!flag1 & flag2)
      num = 1;
    if (num == 0)
      return grid;
    if (num > 0)
    {
      for (int index1 = 0; index1 < length2; ++index1)
      {
        for (int index2 = length1 - 1; index2 >= 0; --index2)
        {
          MapPoint mapPoint = grid[index2, index1];
          grid[index2, index1] = (MapPoint) null;
          int index3 = index2 + num;
          if (index3 < length1)
          {
            grid[index3, index1] = mapPoint;
            if (mapPoint != null)
              mapPoint.coord.col = index3;
          }
        }
      }
    }
    else
    {
      for (int index4 = 0; index4 < length2; ++index4)
      {
        for (int index5 = 0; index5 < length1; ++index5)
        {
          MapPoint mapPoint = grid[index5, index4];
          grid[index5, index4] = (MapPoint) null;
          int index6 = index5 + num;
          if (index6 >= 0)
          {
            grid[index6, index4] = mapPoint;
            if (mapPoint != null)
              mapPoint.coord.col = index6;
          }
        }
      }
    }
    return grid;
  }

  public static MapPoint?[,] StraightenPaths(MapPoint?[,] grid)
  {
    int length1 = grid.GetLength(0);
    int length2 = grid.GetLength(1);
    for (int index1 = 0; index1 < length2; ++index1)
    {
      for (int index2 = 0; index2 < length1; ++index2)
      {
        MapPoint mapPoint1 = grid[index2, index1];
        if (mapPoint1 != null && mapPoint1.parents.Count == 1 && mapPoint1.Children.Count == 1)
        {
          MapPoint mapPoint2 = mapPoint1.parents.First<MapPoint>();
          MapPoint mapPoint3 = mapPoint1.Children.First<MapPoint>();
          bool flag1 = mapPoint1.coord.col < mapPoint3.coord.col && mapPoint1.coord.col < mapPoint2.coord.col;
          bool flag2 = mapPoint1.coord.col > mapPoint3.coord.col && mapPoint1.coord.col > mapPoint2.coord.col;
          if (flag1 && index2 < length1 - 1)
          {
            int index3 = index2 + 1;
            if (grid[index3, index1] == null)
            {
              mapPoint1.coord.col = index3;
              grid[index2, index1] = (MapPoint) null;
              grid[index3, index1] = mapPoint1;
            }
            else
              continue;
          }
          if (flag2 && index2 > 0)
          {
            int index4 = index2 - 1;
            if (grid[index4, index1] == null)
            {
              mapPoint1.coord.col = index4;
              grid[index2, index1] = (MapPoint) null;
              grid[index4, index1] = mapPoint1;
            }
          }
        }
      }
    }
    return grid;
  }

  private static bool IsColumnEmpty(MapPoint?[,] grid, int col)
  {
    int length = grid.GetLength(1);
    for (int index = 0; index < length; ++index)
    {
      if (grid[col, index] != null)
        return false;
    }
    return true;
  }

  private static HashSet<int> GetNeighborAllowedPositions(int column, int totalColumns)
  {
    HashSet<int> allowedPositions = new HashSet<int>(3);
    for (int index = -1; index <= 1; ++index)
    {
      int num = column + index;
      if (num >= 0 && num < totalColumns)
        allowedPositions.Add(num);
    }
    return allowedPositions;
  }

  private static HashSet<int> GetAllowedPositions(MapPoint node, int totalColumns)
  {
    HashSet<int> allowedPositions = new HashSet<int>(Enumerable.Range(0, totalColumns));
    foreach (MapPoint parent in node.parents)
      allowedPositions.IntersectWith((IEnumerable<int>) MapPostProcessing.GetNeighborAllowedPositions(parent.coord.col, totalColumns));
    foreach (MapPoint child in node.Children)
      allowedPositions.IntersectWith((IEnumerable<int>) MapPostProcessing.GetNeighborAllowedPositions(child.coord.col, totalColumns));
    return allowedPositions;
  }

  public static MapPoint?[,] SpreadAdjacentMapPoints(MapPoint?[,] grid)
  {
    int length1 = grid.GetLength(0);
    int length2 = grid.GetLength(1);
    for (int index1 = 0; index1 < length2; ++index1)
    {
      List<MapPoint> rowNodes = new List<MapPoint>(length2);
      for (int index2 = 0; index2 < length1; ++index2)
      {
        MapPoint mapPoint = grid[index2, index1];
        if (mapPoint != null)
          rowNodes.Add(mapPoint);
      }
      bool flag;
      do
      {
        flag = false;
        foreach (MapPoint mapPoint in rowNodes)
        {
          int col = mapPoint.coord.col;
          HashSet<int> allowedPositions = MapPostProcessing.GetAllowedPositions(mapPoint, length1);
          int gap1 = MapPostProcessing.ComputeGap(col, rowNodes, mapPoint);
          int index3 = col;
          int num = gap1;
          foreach (int candidateCol in allowedPositions)
          {
            if (candidateCol != col && (grid[candidateCol, index1] == null || grid[candidateCol, index1] == mapPoint))
            {
              int gap2 = MapPostProcessing.ComputeGap(candidateCol, rowNodes, mapPoint);
              if (gap2 > num)
              {
                index3 = candidateCol;
                num = gap2;
              }
            }
          }
          if (index3 != col)
          {
            grid[col, index1] = (MapPoint) null;
            grid[index3, index1] = mapPoint;
            mapPoint.coord.col = index3;
            flag = true;
          }
        }
      }
      while (flag);
    }
    return grid;
  }

  private static int ComputeGap(int candidateCol, List<MapPoint> rowNodes, MapPoint currentNode)
  {
    int num = int.MaxValue;
    foreach (MapPoint rowNode in rowNodes)
    {
      if (rowNode != currentNode)
        num = Math.Min(num, Math.Abs(candidateCol - rowNode.coord.col));
    }
    return num != int.MaxValue ? num : int.MaxValue;
  }
}
