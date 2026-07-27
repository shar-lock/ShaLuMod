// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves.Runs;

public class SerializableMapPoint : IPacketSerializable
{
  [JsonPropertyName("coord")]
  public MapCoord Coord { get; set; }

  [JsonPropertyName("type")]
  public MapPointType PointType { get; set; }

  [JsonPropertyName("can_modify")]
  [JsonIgnore]
  public bool CanBeModified { get; set; } = true;

  [JsonPropertyName("children")]
  [JsonIgnore]
  public List<MapCoord>? ChildCoords { get; set; }

  public void Serialize(PacketWriter writer)
  {
    writer.Write<MapCoord>(this.Coord);
    writer.WriteInt((int) this.PointType, 8);
    writer.WriteBool(this.CanBeModified);
    List<MapCoord> childCoords = this.ChildCoords;
    // ISSUE: explicit non-virtual call
    int count = childCoords != null ? __nonvirtual (childCoords.Count) : 0;
    writer.WriteInt(count, 8);
    if (this.ChildCoords == null)
      return;
    foreach (MapCoord childCoord in this.ChildCoords)
      writer.Write<MapCoord>(childCoord);
  }

  public void Deserialize(PacketReader reader)
  {
    this.Coord = reader.Read<MapCoord>();
    this.PointType = (MapPointType) reader.ReadInt(8);
    this.CanBeModified = reader.ReadBool();
    int capacity = reader.ReadInt(8);
    if (capacity <= 0)
      return;
    this.ChildCoords = new List<MapCoord>(capacity);
    for (int index = 0; index < capacity; ++index)
      this.ChildCoords.Add(reader.Read<MapCoord>());
  }

  public static SerializableMapPoint FromMapPoint(MapPoint point)
  {
    List<MapCoord> mapCoordList = (List<MapCoord>) null;
    if (point.Children.Count > 0)
    {
      mapCoordList = new List<MapCoord>(point.Children.Count);
      foreach (MapPoint child in point.Children)
        mapCoordList.Add(child.coord);
    }
    return new SerializableMapPoint()
    {
      Coord = point.coord,
      PointType = point.PointType,
      CanBeModified = point.CanBeModified,
      ChildCoords = mapCoordList
    };
  }
}
