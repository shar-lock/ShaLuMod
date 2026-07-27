// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.GameInfo.Objects.CardInfo
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models;
using System;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.GameInfo.Objects;

[Serializable]
public class CardInfo : IGameInfo
{
  [JsonPropertyName("name")]
  public required string Name { get; init; }

  [JsonPropertyName("bot_keyword")]
  public required string BotKeyword { get; init; }

  [JsonPropertyName("bot_text")]
  public required string BotText { get; init; }

  [JsonPropertyName("id")]
  public required ModelId Id { get; init; }

  [JsonPropertyName("upgraded")]
  public required bool Upgraded { get; init; }

  [JsonPropertyName("color")]
  public required string Color { get; init; }

  [JsonPropertyName("rarity")]
  public required string Rarity { get; init; }

  [JsonPropertyName("type")]
  public required string Type { get; init; }

  [JsonPropertyName("base_damage")]
  public required int BaseDamage { get; init; }

  [JsonPropertyName("energy")]
  public required int Energy { get; init; }

  [JsonPropertyName("star_cost")]
  public required int StarCost { get; init; }

  [JsonPropertyName("text")]
  public required string Text { get; init; }

  [JsonPropertyName("has_art")]
  public required bool HasArt { get; init; }

  [JsonPropertyName("has_joke_art")]
  public required bool HasJokeArt { get; init; }
}
