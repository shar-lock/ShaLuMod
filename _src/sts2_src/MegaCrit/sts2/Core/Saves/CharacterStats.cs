// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.CharacterStats
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves;

public class CharacterStats
{
  [JsonPropertyName("id")]
  public ModelId? Id { get; init; }

  [JsonPropertyName("max_ascension")]
  public int MaxAscension { get; set; }

  [JsonPropertyName("preferred_ascension")]
  public int PreferredAscension { get; set; }

  [JsonPropertyName("total_wins")]
  public int TotalWins { get; set; }

  [JsonPropertyName("total_losses")]
  public int TotalLosses { get; set; }

  [JsonPropertyName("fastest_win_time")]
  public long FastestWinTime { get; set; } = -1;

  [JsonPropertyName("best_win_streak")]
  public long BestWinStreak { get; set; }

  [JsonPropertyName("current_streak")]
  public long CurrentWinStreak { get; set; }

  [JsonPropertyName("playtime")]
  public long Playtime { get; set; }

  [JsonPropertyName("badges")]
  public List<BadgeStats> Badges { get; set; } = new List<BadgeStats>();
}
