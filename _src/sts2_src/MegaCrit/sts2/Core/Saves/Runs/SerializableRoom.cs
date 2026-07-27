// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Rooms;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves.Runs;

public class SerializableRoom : IPacketSerializable
{
  [JsonPropertyName("room_type")]
  public RoomType RoomType { get; set; }

  [JsonPropertyName("encounter_id")]
  public ModelId? EncounterId { get; set; }

  [JsonPropertyName("event_id")]
  public ModelId? EventId { get; set; }

  [JsonPropertyName("is_pre_finished")]
  public bool IsPreFinished { get; set; }

  [JsonPropertyName("reward_proportion")]
  public float GoldProportion { get; set; }

  [JsonPropertyName("extra_rewards")]
  [JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
  public Dictionary<ulong, List<SerializableReward>> ExtraRewards { get; set; } = new Dictionary<ulong, List<SerializableReward>>();

  [JsonPropertyName("parent_event_id")]
  public ModelId? ParentEventId { get; set; }

  [JsonPropertyName("should_resume_parent_event")]
  public bool ShouldResumeParentEvent { get; set; } = true;

  [JsonPropertyName("encounter_state")]
  [JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
  public Dictionary<string, string> EncounterState { get; set; } = new Dictionary<string, string>();

  public void Serialize(PacketWriter writer)
  {
    writer.WriteInt((int) this.RoomType);
    writer.WriteBool(this.EncounterId != (ModelId) null);
    if (this.EncounterId != (ModelId) null)
      writer.WriteModelEntry(this.EncounterId);
    writer.WriteBool(this.EventId != (ModelId) null);
    if (this.EventId != (ModelId) null)
      writer.WriteModelEntry(this.EventId);
    writer.WriteFloat(this.GoldProportion);
    writer.WriteBool(this.IsPreFinished);
    writer.WriteInt(this.ExtraRewards.Count);
    foreach (KeyValuePair<ulong, List<SerializableReward>> extraReward in this.ExtraRewards)
    {
      ulong num;
      List<SerializableReward> serializableRewardList;
      extraReward.Deconstruct(ref num, ref serializableRewardList);
      ulong val = num;
      List<SerializableReward> list = serializableRewardList;
      writer.WriteULong(val);
      writer.WriteList<SerializableReward>((IReadOnlyList<SerializableReward>) list);
    }
    writer.WriteBool(this.ParentEventId != (ModelId) null);
    if (this.ParentEventId != (ModelId) null)
      writer.WriteModelEntry(this.ParentEventId);
    writer.WriteBool(this.ShouldResumeParentEvent);
    writer.WriteInt(this.EncounterState.Count);
    foreach (KeyValuePair<string, string> keyValuePair in this.EncounterState)
    {
      string str1;
      string str2;
      keyValuePair.Deconstruct(ref str1, ref str2);
      string str3 = str1;
      string str4 = str2;
      writer.WriteString(str3);
      writer.WriteString(str4);
    }
  }

  public void Deserialize(PacketReader reader)
  {
    this.RoomType = (RoomType) reader.ReadInt();
    if (reader.ReadBool())
      this.EncounterId = reader.ReadModelIdAssumingType<EncounterModel>();
    if (reader.ReadBool())
      this.EventId = reader.ReadModelIdAssumingType<EventModel>();
    this.GoldProportion = reader.ReadFloat();
    this.IsPreFinished = reader.ReadBool();
    int num = reader.ReadInt();
    for (int index = 0; index < num; ++index)
      this.ExtraRewards[reader.ReadULong()] = reader.ReadList<SerializableReward>();
    if (reader.ReadBool())
      this.ParentEventId = reader.ReadModelIdAssumingType<EventModel>();
    this.ShouldResumeParentEvent = reader.ReadBool();
    int capacity = reader.ReadInt();
    this.EncounterState = new Dictionary<string, string>(capacity);
    for (int index = 0; index < capacity; ++index)
      this.EncounterState[reader.ReadString()] = reader.ReadString();
  }
}
