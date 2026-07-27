// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Runs.History;

public class MapPointRoomHistoryEntry : IPacketSerializable
{
  [JsonPropertyName("room_type")]
  public RoomType RoomType { get; set; }

  [JsonPropertyName("model_id")]
  [JsonSerializeCondition(SerializationCondition.SaveIfNotTypeDefault)]
  public ModelId? ModelId { get; set; }

  [JsonPropertyName("monster_ids")]
  [JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
  public List<ModelId> MonsterIds { get; set; } = new List<ModelId>();

  [JsonPropertyName("turns_taken")]
  public int TurnsTaken { get; set; }

  public void Serialize(PacketWriter writer)
  {
    writer.WriteEnum<RoomType>(this.RoomType);
    writer.WriteBool(this.ModelId != (ModelId) null);
    if (this.ModelId != (ModelId) null)
      writer.WriteFullModelId(this.ModelId);
    writer.WriteModelEntriesInList((IReadOnlyCollection<ModelId>) this.MonsterIds);
    writer.WriteInt(this.TurnsTaken);
  }

  public void Deserialize(PacketReader reader)
  {
    this.RoomType = reader.ReadEnum<RoomType>();
    if (reader.ReadBool())
      this.ModelId = reader.ReadFullModelId();
    this.MonsterIds = reader.ReadModelIdListAssumingType<MonsterModel>();
    this.TurnsTaken = reader.ReadInt();
  }
}
