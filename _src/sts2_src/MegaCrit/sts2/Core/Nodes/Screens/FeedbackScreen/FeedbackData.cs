// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen;

public struct FeedbackData
{
  [JsonPropertyName("description")]
  public string description;
  [JsonPropertyName("category")]
  public string category;
  [JsonPropertyName("game_version")]
  public string gameVersion;
  [JsonPropertyName("unique_id")]
  public string uniqueId;
  [JsonPropertyName("commit")]
  public string commit;
  [JsonPropertyName("platform_branch")]
  public string? platformBranch;
  [JsonPropertyName("session_id")]
  public string sessionId;
  [JsonPropertyName("is_modded")]
  public bool isModded;
  [JsonPropertyName("is_full_console")]
  public bool isFullConsole;
  [JsonPropertyName("lang")]
  public string lang;
}
