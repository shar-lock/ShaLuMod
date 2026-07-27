// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.GameInfo.Objects.AncientChoiceInfo
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.GameInfo.Objects;

[Serializable]
public class AncientChoiceInfo : IGameInfo
{
  [JsonPropertyName("name")]
  public required string Name { get; init; }

  [JsonPropertyName("bot_keyword")]
  public required string BotKeyword { get; init; }

  [JsonPropertyName("bot_text")]
  public required string BotText { get; init; }

  [JsonPropertyName("id")]
  public required string Id { get; init; }

  [JsonPropertyName("text")]
  public required string Text { get; init; }

  [JsonPropertyName("ancient")]
  public required string Ancient { get; init; }
}
