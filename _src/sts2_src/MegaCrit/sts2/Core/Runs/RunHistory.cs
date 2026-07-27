// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Runs.RunHistory
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Runs;

public class RunHistory : ISaveSchema
{
  [JsonPropertyName("schema_version")]
  public int SchemaVersion { get; set; }

  [JsonPropertyName("platform_type")]
  public PlatformType PlatformType { get; init; }

  [JsonPropertyName("game_mode")]
  public GameMode GameMode { get; init; } = GameMode.Standard;

  [JsonPropertyName("win")]
  public bool Win { get; init; }

  [JsonPropertyName("seed")]
  public string Seed { get; init; } = "";

  [JsonPropertyName("start_time")]
  public long StartTime { get; init; }

  [JsonPropertyName("run_time")]
  public float RunTime { get; init; }

  [JsonPropertyName("ascension")]
  public int Ascension { get; init; }

  [JsonPropertyName("build_id")]
  public string BuildId { get; init; } = "pre-v0.42";

  [JsonPropertyName("was_abandoned")]
  public bool WasAbandoned { get; init; }

  [JsonPropertyName("killed_by_encounter")]
  public ModelId KilledByEncounter { get; init; } = ModelId.none;

  [JsonPropertyName("killed_by_event")]
  public ModelId KilledByEvent { get; init; } = ModelId.none;

  [JsonPropertyName("players")]
  public List<RunHistoryPlayer> Players { get; init; } = new List<RunHistoryPlayer>();

  [JsonPropertyName("acts")]
  public List<ModelId> Acts { get; init; } = new List<ModelId>();

  [JsonPropertyName("modifiers")]
  public List<SerializableModifier> Modifiers { get; init; } = new List<SerializableModifier>();

  [JsonPropertyName("map_point_history")]
  public List<List<MapPointHistoryEntry>> MapPointHistory { get; init; } = new List<List<MapPointHistoryEntry>>();
}
