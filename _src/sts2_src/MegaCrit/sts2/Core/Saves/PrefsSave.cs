// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.PrefsSave
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Settings;
using System.Text.Json.Serialization;

#nullable disable
namespace MegaCrit.Sts2.Core.Saves;

public class PrefsSave : ISaveSchema
{
  [JsonPropertyName("schema_version")]
  public int SchemaVersion { get; set; }

  [JsonPropertyName("fast_mode")]
  public FastModeType FastMode { get; set; } = FastModeType.Normal;

  [JsonPropertyName("phobia_mode")]
  public bool PhobiaMode { get; set; }

  [JsonPropertyName("screenshake")]
  public int ScreenShakeOptionIndex { get; set; } = 2;

  [JsonPropertyName("show_run_timer")]
  public bool ShowRunTimer { get; set; }

  [JsonPropertyName("show_card_indices")]
  public bool ShowCardIndices { get; set; }

  [JsonPropertyName("upload_data")]
  public bool UploadData { get; set; } = true;

  [JsonPropertyName("mute_in_background")]
  public bool MuteInBackground { get; set; } = true;

  [JsonPropertyName("long_press")]
  public bool IsLongPressEnabled { get; set; }

  [JsonPropertyName("text_effects_enabled")]
  public bool TextEffectsEnabled { get; set; } = true;

  [JsonPropertyName("show_mp_drawings")]
  public bool ShowMultiplayerDrawings { get; set; } = true;

  [JsonPropertyName("bestiary_actions_preferred")]
  public bool IsBestiaryActionsPreferred { get; set; } = true;
}
