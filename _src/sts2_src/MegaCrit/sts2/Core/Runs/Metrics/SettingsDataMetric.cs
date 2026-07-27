// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Settings;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Runs.Metrics;

public struct SettingsDataMetric
{
  [JsonPropertyName("buildId")]
  public required string BuildId { get; set; }

  [JsonPropertyName("os")]
  public string Os { get; set; }

  [JsonPropertyName("platform")]
  public string Platform { get; set; }

  [JsonPropertyName("systemRam")]
  public int SystemRam { get; set; }

  [JsonPropertyName("language")]
  public string LanguageCode { get; set; }

  [JsonPropertyName("combatSpeed")]
  public required FastModeType FastModeType { get; set; }

  [JsonPropertyName("screenshake")]
  public int Screenshake { get; set; }

  [JsonPropertyName("runTimer")]
  public bool ShowRunTimer { get; set; }

  [JsonPropertyName("phobiaMode")]
  public bool PhobiaMode { get; set; }

  [JsonPropertyName("cardIndices")]
  public bool ShowCardIndices { get; set; }

  [JsonPropertyName("displayCount")]
  public int DisplayCount { get; set; }

  [JsonPropertyName("displayResolution")]
  public Vector2I DisplayResolution { get; set; }

  [JsonPropertyName("fullscreen")]
  public bool Fullscreen { get; set; }

  [JsonPropertyName("aspectRatio")]
  public AspectRatioSetting AspectRatio { get; set; }

  [JsonPropertyName("resizeWindows")]
  public bool ResizeWindows { get; set; }

  [JsonPropertyName("vSync")]
  public VSyncType VSync { get; set; }

  [JsonPropertyName("fpsLimit")]
  public int FpsLimit { get; set; }

  [JsonPropertyName("msaa")]
  public int Msaa { get; set; }
}
