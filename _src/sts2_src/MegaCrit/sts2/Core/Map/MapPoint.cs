// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Map.MapPoint
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Map;

public class MapPoint : IComparable<MapPoint>
{
  private readonly List<AbstractModel> _quests = new List<AbstractModel>();
  public readonly HashSet<MapPoint> parents;
  public MapCoord coord;

  public bool CanBeModified { get; set; } = true;

  public MapPointType PointType { get; set; }

  public IReadOnlyList<AbstractModel> Quests => (IReadOnlyList<AbstractModel>) this._quests;

  public event Action? NodeMarkedChanged;

  private bool Equals(MapPoint other) => this.coord.Equals(other.coord);

  public int CompareTo(MapPoint? other) => other == null ? 1 : this.coord.CompareTo(other.coord);

  public MapPoint(int col, int row)
  {
    this.coord.row = row;
    this.coord.col = col;
    this.parents = new HashSet<MapPoint>();
    this.Children = new HashSet<MapPoint>();
  }

  public HashSet<MapPoint> Children { get; }

  public void AddChildPoint(MapPoint child)
  {
    this.Children.Add(child);
    child.parents.Add(this);
  }

  public void RemoveChildPoint(MapPoint child)
  {
    this.Children.Remove(child);
    child.parents.Remove(this);
  }

  public override string ToString() => $"Point[{this.coord.col},{this.coord.row}]";

  public MapPoint? LeftChild()
  {
    foreach (MapPoint child in this.Children)
    {
      if (child.IsAdjacentLeft(this))
        return child;
    }
    return (MapPoint) null;
  }

  public MapPoint? RightChild()
  {
    foreach (MapPoint child in this.Children)
    {
      if (child.IsAdjacentRight(this))
        return child;
    }
    return (MapPoint) null;
  }

  public bool IsAdjacentLeft(MapPoint sibling) => this.coord.col - 1 == sibling.coord.col;

  public bool IsAdjacentRight(MapPoint sibling) => this.coord.col + 1 == sibling.coord.col;

  public bool IsToTheLeft(MapPoint sibling) => this.coord.col < sibling.coord.col;

  public bool IsToTheRight(MapPoint sibling) => this.coord.col > sibling.coord.col;

  public bool IsInTheSameRow(MapPoint sibling) => sibling.coord.row == this.coord.row;

  public MapPoint? GetFirstCommonDescendant(MapPoint b)
  {
    if (this.Equals(b))
      return b;
    HashSet<MapPoint> allDescendants = this.GetAllDescendants();
    Queue<MapPoint> mapPointQueue = new Queue<MapPoint>();
    mapPointQueue.Enqueue(b);
    while (mapPointQueue.Count > 0)
    {
      b = mapPointQueue.Dequeue();
      if (allDescendants.Contains(b))
        return b;
      foreach (MapPoint child in b.Children)
        mapPointQueue.Enqueue(child);
    }
    return (MapPoint) null;
  }

  private HashSet<MapPoint> GetAllDescendants()
  {
    HashSet<MapPoint> allDescendants = new HashSet<MapPoint>();
    Queue<MapPoint> mapPointQueue = new Queue<MapPoint>();
    mapPointQueue.Enqueue(this);
    while (mapPointQueue.Count > 0)
    {
      MapPoint mapPoint = mapPointQueue.Dequeue();
      if (allDescendants.Add(mapPoint))
      {
        foreach (MapPoint child in mapPoint.Children)
          mapPointQueue.Enqueue(child);
      }
    }
    return allDescendants;
  }

  public MapPoint? GetCommonAncestor(MapPoint b)
  {
    if (this.coord.Equals(b.coord))
      return b;
    HashSet<MapPoint> allAncestors = this.GetAllAncestors();
    Queue<MapPoint> mapPointQueue = new Queue<MapPoint>();
    mapPointQueue.Enqueue(b);
    while (mapPointQueue.Count > 0)
    {
      b = mapPointQueue.Dequeue();
      if (allAncestors.Contains(b))
        return b;
      foreach (MapPoint parent in b.parents)
        mapPointQueue.Enqueue(parent);
    }
    return (MapPoint) null;
  }

  public int GetLastJunctionLength()
  {
    Queue<MapPoint> mapPointQueue = new Queue<MapPoint>();
    mapPointQueue.Enqueue(this);
    while (mapPointQueue.Count > 0)
    {
      MapPoint mapPoint = mapPointQueue.Dequeue();
      if (mapPoint.Children.Count > 1)
        return this.coord.row - mapPoint.coord.row;
      foreach (MapPoint parent in mapPoint.parents)
        mapPointQueue.Enqueue(parent);
    }
    return this.coord.row;
  }

  public HashSet<MapPoint> GetAllAncestors()
  {
    HashSet<MapPoint> allAncestors = new HashSet<MapPoint>();
    Queue<MapPoint> mapPointQueue = new Queue<MapPoint>();
    mapPointQueue.Enqueue(this);
    while (mapPointQueue.Count > 0)
    {
      MapPoint mapPoint = mapPointQueue.Dequeue();
      if (allAncestors.Add(mapPoint))
      {
        foreach (MapPoint parent in mapPoint.parents)
          mapPointQueue.Enqueue(parent);
      }
    }
    return allAncestors;
  }

  public IEnumerable<MapPoint> BFS_FindPath(MapPoint target)
  {
    Queue<MapPoint> mapPointQueue = new Queue<MapPoint>();
    Dictionary<MapPoint, MapPoint> parentPoint = new Dictionary<MapPoint, MapPoint>();
    mapPointQueue.Enqueue(this);
    while (mapPointQueue.Count > 0)
    {
      MapPoint mapPoint = mapPointQueue.Dequeue();
      if (mapPoint.Equals(target))
        return this.BuildPath((IReadOnlyDictionary<MapPoint, MapPoint>) parentPoint, target);
      foreach (MapPoint child in mapPoint.Children)
      {
        if (!parentPoint.ContainsKey(child))
        {
          parentPoint[child] = mapPoint;
          mapPointQueue.Enqueue(child);
        }
      }
    }
    return (IEnumerable<MapPoint>) new List<MapPoint>();
  }

  private IEnumerable<MapPoint> BuildPath(
    IReadOnlyDictionary<MapPoint, MapPoint> parentPoint,
    MapPoint target)
  {
    List<MapPoint> mapPointList = new List<MapPoint>();
    for (MapPoint mapPoint = target; !this.Equals(mapPoint); mapPoint = parentPoint[mapPoint])
      mapPointList.Add(mapPoint);
    mapPointList.Add(this);
    mapPointList.Reverse();
    return (IEnumerable<MapPoint>) mapPointList;
  }

  public bool IsDescendantPathSame(MapPoint? other)
  {
    if (other == null)
      return false;
    MapPoint commonDescendant = this.GetFirstCommonDescendant(other);
    if (commonDescendant == null)
      return false;
    MapPointType[] array1 = this.BFS_FindPath(commonDescendant).Where<MapPoint>((Func<MapPoint, bool>) (n => n.PointType != 0)).Select<MapPoint, MapPointType>((Func<MapPoint, MapPointType>) (n => n.PointType)).ToArray<MapPointType>();
    MapPointType[] array2 = other.BFS_FindPath(commonDescendant).Where<MapPoint>((Func<MapPoint, bool>) (n => n.PointType != 0)).Select<MapPoint, MapPointType>((Func<MapPoint, MapPointType>) (n => n.PointType)).ToArray<MapPointType>();
    if (array1.Length != array2.Length)
      return false;
    for (int index = 0; index < array1.Length; ++index)
    {
      if (array1[index] != array2[index])
        return false;
    }
    return true;
  }

  public void AddQuest(AbstractModel model)
  {
    this._quests.Add(model);
    Action nodeMarkedChanged = this.NodeMarkedChanged;
    if (nodeMarkedChanged == null)
      return;
    nodeMarkedChanged();
  }

  public void RemoveQuest(AbstractModel model)
  {
    this._quests.Remove(model);
    Action nodeMarkedChanged = this.NodeMarkedChanged;
    if (nodeMarkedChanged == null)
      return;
    nodeMarkedChanged();
  }
}
