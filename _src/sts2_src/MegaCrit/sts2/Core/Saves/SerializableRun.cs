// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.SerializableRun
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves.MapDrawing;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves;

public class SerializableRun : ISaveSchema, IPacketSerializable
{
  [JsonPropertyName("schema_version")]
  public int SchemaVersion { get; set; }

  [JsonPropertyName("acts")]
  [JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
  public List<SerializableActModel> Acts { get; set; } = new List<SerializableActModel>();

  [JsonPropertyName("modifiers")]
  public List<SerializableModifier> Modifiers { get; set; } = new List<SerializableModifier>();

  [JsonPropertyName("dailyTime")]
  [JsonSerializeCondition(SerializationCondition.SaveIfNotTypeDefault)]
  public DateTimeOffset? DailyTime { get; set; }

  [JsonPropertyName("current_act_index")]
  public int CurrentActIndex { get; set; }

  [JsonPropertyName("events_seen")]
  [JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
  public List<ModelId> EventsSeen { get; set; } = new List<ModelId>();

  [JsonPropertyName("pre_finished_room")]
  public SerializableRoom? PreFinishedRoom { get; set; }

  [JsonPropertyName("odds")]
  public SerializableRunOddsSet SerializableOdds { get; set; }

  [JsonPropertyName("shared_relic_grab_bag")]
  public SerializableRelicGrabBag SerializableSharedRelicGrabBag { get; set; }

  [JsonPropertyName("players")]
  public List<SerializablePlayer> Players { get; set; }

  [JsonPropertyName("rng")]
  public SerializableRunRngSet SerializableRng { get; set; }

  [JsonPropertyName("visited_map_coords")]
  [JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
  public List<MapCoord> VisitedMapCoords { get; set; } = new List<MapCoord>();

  [JsonPropertyName("map_point_history")]
  [JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
  public List<List<MapPointHistoryEntry>> MapPointHistory { get; set; } = new List<List<MapPointHistoryEntry>>();

  [JsonPropertyName("save_time")]
  public long SaveTime { get; set; }

  [JsonPropertyName("start_time")]
  public long StartTime { get; set; }

  [JsonPropertyName("run_time")]
  public long RunTime { get; set; }

  [JsonPropertyName("win_time")]
  public long WinTime { get; set; }

  [JsonPropertyName("ascension")]
  public int Ascension { get; set; }

  [JsonPropertyName("num_reloads")]
  public int NumReloads { get; set; }

  [JsonPropertyName("platform_type")]
  public PlatformType PlatformType { get; set; }

  [JsonConverter(typeof (SerializableMapDrawingsJsonConverter))]
  [JsonPropertyName("map_drawings")]
  public SerializableMapDrawings? MapDrawings { get; set; }

  [JsonPropertyName("extra_fields")]
  public SerializableExtraRunFields ExtraFields { get; set; } = new SerializableExtraRunFields();

  [JsonPropertyName("game_mode")]
  public GameMode GameMode { get; set; }

  public void Serialize(PacketWriter writer)
  {
    writer.WriteInt(this.SchemaVersion);
    writer.WriteList<SerializableActModel>((IReadOnlyList<SerializableActModel>) this.Acts);
    writer.WriteList<SerializableModifier>((IReadOnlyList<SerializableModifier>) this.Modifiers);
    writer.WriteBool(this.DailyTime.HasValue);
    if (this.DailyTime.HasValue)
      writer.WriteLong(this.DailyTime.Value.ToUnixTimeSeconds());
    writer.WriteEnum<GameMode>(this.GameMode);
    writer.WriteInt(this.CurrentActIndex, 4);
    writer.WriteModelEntriesInList((IReadOnlyCollection<ModelId>) this.EventsSeen);
    writer.WriteBool(this.PreFinishedRoom != null);
    if (this.PreFinishedRoom != null)
      writer.Write<SerializableRoom>(this.PreFinishedRoom);
    writer.Write<SerializableRunOddsSet>(this.SerializableOdds);
    writer.WriteList<SerializablePlayer>((IReadOnlyList<SerializablePlayer>) this.Players);
    writer.Write<SerializableRunRngSet>(this.SerializableRng);
    writer.Write<SerializableRelicGrabBag>(this.SerializableSharedRelicGrabBag);
    writer.WriteList<MapCoord>((IReadOnlyList<MapCoord>) this.VisitedMapCoords);
    writer.WriteInt(this.MapPointHistory.Count);
    foreach (List<MapPointHistoryEntry> list in this.MapPointHistory)
      writer.WriteList<MapPointHistoryEntry>((IReadOnlyList<MapPointHistoryEntry>) list);
    writer.WriteLong(this.SaveTime);
    writer.WriteLong(this.StartTime);
    writer.WriteLong(this.RunTime);
    writer.WriteLong(this.WinTime);
    writer.WriteInt(this.Ascension, 8);
    writer.WriteBool(this.MapDrawings != null);
    if (this.MapDrawings != null)
      writer.Write<SerializableMapDrawings>(this.MapDrawings);
    writer.Write<SerializableExtraRunFields>(this.ExtraFields);
    writer.WriteInt(this.NumReloads);
  }

  public void Deserialize(PacketReader reader)
  {
    this.SchemaVersion = reader.ReadInt();
    this.Acts = reader.ReadList<SerializableActModel>();
    this.Modifiers = reader.ReadList<SerializableModifier>();
    if (reader.ReadBool())
      this.DailyTime = new DateTimeOffset?(DateTimeOffset.FromUnixTimeSeconds(reader.ReadLong()));
    this.GameMode = reader.ReadEnum<GameMode>();
    this.CurrentActIndex = reader.ReadInt(4);
    this.EventsSeen = reader.ReadModelIdListAssumingType<EventModel>();
    if (reader.ReadBool())
      this.PreFinishedRoom = reader.Read<SerializableRoom>();
    this.SerializableOdds = reader.Read<SerializableRunOddsSet>();
    this.Players = reader.ReadList<SerializablePlayer>();
    this.SerializableRng = reader.Read<SerializableRunRngSet>();
    this.SerializableSharedRelicGrabBag = reader.Read<SerializableRelicGrabBag>();
    this.VisitedMapCoords = reader.ReadList<MapCoord>();
    int num = reader.ReadInt();
    for (int index = 0; index < num; ++index)
      this.MapPointHistory.Add(reader.ReadList<MapPointHistoryEntry>());
    this.SaveTime = reader.ReadLong();
    this.StartTime = reader.ReadLong();
    this.RunTime = reader.ReadLong();
    this.WinTime = reader.ReadLong();
    this.Ascension = reader.ReadInt(8);
    if (reader.ReadBool())
      this.MapDrawings = reader.Read<SerializableMapDrawings>();
    this.ExtraFields = reader.Read<SerializableExtraRunFields>();
    this.NumReloads = reader.ReadInt();
  }

  public SerializableRun Anonymized()
  {
    return new SerializableRun()
    {
      SchemaVersion = this.SchemaVersion,
      Acts = this.Acts,
      Modifiers = this.Modifiers,
      DailyTime = this.DailyTime,
      CurrentActIndex = this.CurrentActIndex,
      EventsSeen = this.EventsSeen,
      GameMode = this.GameMode,
      PreFinishedRoom = this.PreFinishedRoom,
      SerializableOdds = this.SerializableOdds,
      SerializableSharedRelicGrabBag = this.SerializableSharedRelicGrabBag,
      Players = this.Players.Select<SerializablePlayer, SerializablePlayer>((Func<SerializablePlayer, SerializablePlayer>) (p => p.Anonymized())).ToList<SerializablePlayer>(),
      SerializableRng = this.SerializableRng,
      VisitedMapCoords = this.VisitedMapCoords,
      MapPointHistory = this.MapPointHistory.Select<List<MapPointHistoryEntry>, List<MapPointHistoryEntry>>((Func<List<MapPointHistoryEntry>, List<MapPointHistoryEntry>>) (l => l.Select<MapPointHistoryEntry, MapPointHistoryEntry>((Func<MapPointHistoryEntry, MapPointHistoryEntry>) (h => h.Anonymized())).ToList<MapPointHistoryEntry>())).ToList<List<MapPointHistoryEntry>>(),
      SaveTime = this.SaveTime,
      StartTime = this.StartTime,
      RunTime = this.RunTime,
      WinTime = this.WinTime,
      Ascension = this.Ascension,
      PlatformType = this.PlatformType,
      MapDrawings = this.MapDrawings?.Anonymized(),
      ExtraFields = this.ExtraFields
    };
  }

  [JsonIgnore]
  public int FloorReached
  {
    get
    {
      return this.MapPointHistory.Sum<List<MapPointHistoryEntry>>((Func<List<MapPointHistoryEntry>, int>) (c => c.Count));
    }
  }
}
