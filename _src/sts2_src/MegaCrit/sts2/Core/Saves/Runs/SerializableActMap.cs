// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves.Runs;

public class SerializableActMap : IPacketSerializable
{
  [JsonPropertyName("points")]
  public List<SerializableMapPoint> Points { get; set; } = new List<SerializableMapPoint>();

  [JsonPropertyName("boss")]
  public SerializableMapPoint BossPoint { get; set; }

  [JsonPropertyName("second_boss")]
  [JsonIgnore]
  public SerializableMapPoint? SecondBossPoint { get; set; }

  [JsonPropertyName("start")]
  public SerializableMapPoint StartingPoint { get; set; }

  [JsonPropertyName("start_coords")]
  [JsonIgnore]
  public List<MapCoord>? StartMapPointCoords { get; set; }

  [JsonPropertyName("width")]
  public int GridWidth { get; set; }

  [JsonPropertyName("height")]
  public int GridHeight { get; set; }

  public void Serialize(PacketWriter writer)
  {
    writer.WriteInt(this.GridWidth, 8);
    writer.WriteInt(this.GridHeight, 8);
    writer.Write<SerializableMapPoint>(this.BossPoint);
    writer.Write<SerializableMapPoint>(this.StartingPoint);
    writer.WriteBool(this.SecondBossPoint != null);
    if (this.SecondBossPoint != null)
      writer.Write<SerializableMapPoint>(this.SecondBossPoint);
    writer.WriteInt(this.Points.Count, 16 /*0x10*/);
    foreach (SerializableMapPoint point in this.Points)
      writer.Write<SerializableMapPoint>(point);
    List<MapCoord> startMapPointCoords = this.StartMapPointCoords;
    // ISSUE: explicit non-virtual call
    int count = startMapPointCoords != null ? __nonvirtual (startMapPointCoords.Count) : 0;
    writer.WriteInt(count, 8);
    if (this.StartMapPointCoords == null)
      return;
    foreach (MapCoord startMapPointCoord in this.StartMapPointCoords)
      writer.Write<MapCoord>(startMapPointCoord);
  }

  public void Deserialize(PacketReader reader)
  {
    this.GridWidth = reader.ReadInt(8);
    this.GridHeight = reader.ReadInt(8);
    this.BossPoint = reader.Read<SerializableMapPoint>();
    this.StartingPoint = reader.Read<SerializableMapPoint>();
    if (reader.ReadBool())
      this.SecondBossPoint = reader.Read<SerializableMapPoint>();
    int capacity1 = reader.ReadInt(16 /*0x10*/);
    this.Points = new List<SerializableMapPoint>(capacity1);
    for (int index = 0; index < capacity1; ++index)
      this.Points.Add(reader.Read<SerializableMapPoint>());
    int capacity2 = reader.ReadInt(8);
    if (capacity2 <= 0)
      return;
    this.StartMapPointCoords = new List<MapCoord>(capacity2);
    for (int index = 0; index < capacity2; ++index)
      this.StartMapPointCoords.Add(reader.Read<MapCoord>());
  }

  public static SerializableActMap FromActMap(ActMap map)
  {
    List<SerializableMapPoint> serializableMapPointList = new List<SerializableMapPoint>();
    foreach (MapPoint allMapPoint in map.GetAllMapPoints())
      serializableMapPointList.Add(SerializableMapPoint.FromMapPoint(allMapPoint));
    List<MapCoord> mapCoordList = (List<MapCoord>) null;
    if (map.startMapPoints.Count > 0)
    {
      mapCoordList = new List<MapCoord>();
      foreach (MapPoint startMapPoint in map.startMapPoints)
        mapCoordList.Add(startMapPoint.coord);
    }
    return new SerializableActMap()
    {
      Points = serializableMapPointList,
      BossPoint = SerializableMapPoint.FromMapPoint(map.BossMapPoint),
      SecondBossPoint = map.SecondBossMapPoint != null ? SerializableMapPoint.FromMapPoint(map.SecondBossMapPoint) : (SerializableMapPoint) null,
      StartingPoint = SerializableMapPoint.FromMapPoint(map.StartingMapPoint),
      StartMapPointCoords = mapCoordList,
      GridWidth = map.GetColumnCount(),
      GridHeight = map.GetRowCount()
    };
  }
}
