// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.BadgeStats
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models.Badges;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves;

public class BadgeStats
{
  [JsonPropertyName("id")]
  public required string Id { get; init; }

  [JsonPropertyName("count")]
  public required int Count { get; set; }

  [JsonPropertyName("rarity")]
  public required BadgeRarity Rarity { get; set; }
}
