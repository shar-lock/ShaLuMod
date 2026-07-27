// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves.Runs;

public class SerializableRoomSet : IPacketSerializable
{
  [JsonPropertyName("event_ids")]
  [JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
  public List<ModelId> EventIds { get; set; }

  [JsonPropertyName("events_visited")]
  public int EventsVisited { get; set; }

  [JsonPropertyName("normal_encounter_ids")]
  [JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
  public List<ModelId> NormalEncounterIds { get; set; }

  [JsonPropertyName("normal_encounters_visited")]
  public int NormalEncountersVisited { get; set; }

  [JsonPropertyName("elite_encounter_ids")]
  [JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
  public List<ModelId> EliteEncounterIds { get; set; }

  [JsonPropertyName("elite_encounters_visited")]
  public int EliteEncountersVisited { get; set; }

  [JsonPropertyName("boss_encounters_visited")]
  public int BossEncountersVisited { get; set; }

  [JsonPropertyName("boss_id")]
  public ModelId? BossId { get; set; }

  [JsonPropertyName("second_boss_id")]
  public ModelId? SecondBossId { get; set; }

  [JsonPropertyName("ancient_id")]
  public ModelId? AncientId { get; set; }

  public void Serialize(PacketWriter writer)
  {
    writer.WriteModelEntriesInList((IReadOnlyCollection<ModelId>) this.EventIds);
    writer.WriteInt(this.EventsVisited);
    writer.WriteModelEntriesInList((IReadOnlyCollection<ModelId>) this.NormalEncounterIds);
    writer.WriteInt(this.NormalEncountersVisited);
    writer.WriteModelEntriesInList((IReadOnlyCollection<ModelId>) this.EliteEncounterIds);
    writer.WriteInt(this.EliteEncountersVisited);
    writer.WriteInt(this.BossEncountersVisited);
    writer.WriteBool(this.BossId != (ModelId) null);
    if (this.BossId != (ModelId) null)
      writer.WriteModelEntry(this.BossId);
    writer.WriteBool(this.SecondBossId != (ModelId) null);
    if (this.SecondBossId != (ModelId) null)
      writer.WriteModelEntry(this.SecondBossId);
    writer.WriteBool(this.AncientId != (ModelId) null);
    if (!(this.AncientId != (ModelId) null))
      return;
    writer.WriteModelEntry(this.AncientId);
  }

  public void Deserialize(PacketReader reader)
  {
    this.EventIds = reader.ReadModelIdListAssumingType<EventModel>();
    this.EventsVisited = reader.ReadInt();
    this.NormalEncounterIds = reader.ReadModelIdListAssumingType<EncounterModel>();
    this.NormalEncountersVisited = reader.ReadInt();
    this.EliteEncounterIds = reader.ReadModelIdListAssumingType<EncounterModel>();
    this.EliteEncountersVisited = reader.ReadInt();
    this.BossEncountersVisited = reader.ReadInt();
    if (reader.ReadBool())
      this.BossId = reader.ReadModelIdAssumingType<EncounterModel>();
    if (reader.ReadBool())
      this.SecondBossId = reader.ReadModelIdAssumingType<EncounterModel>();
    if (!reader.ReadBool())
      return;
    this.AncientId = reader.ReadModelIdAssumingType<AncientEventModel>();
  }
}
