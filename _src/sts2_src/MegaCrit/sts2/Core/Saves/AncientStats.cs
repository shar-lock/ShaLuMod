// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.AncientStats
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves;

public class AncientStats
{
  [JsonPropertyName("ancient_id")]
  public required ModelId Id { get; init; }

  [JsonPropertyName("character_stats")]
  public List<AncientCharacterStats> CharStats { get; init; } = new List<AncientCharacterStats>();

  [JsonIgnore]
  public int TotalVisits
  {
    get
    {
      return this.CharStats.Count == 0 ? 0 : this.CharStats.Sum<AncientCharacterStats>((Func<AncientCharacterStats, int>) (c => c.Visits));
    }
  }

  [JsonIgnore]
  public int TotalWins
  {
    get
    {
      return this.CharStats.Count == 0 ? 0 : this.CharStats.Sum<AncientCharacterStats>((Func<AncientCharacterStats, int>) (fight => fight.Wins));
    }
  }

  [JsonIgnore]
  public int TotalLosses
  {
    get
    {
      return this.CharStats.Count == 0 ? 0 : this.CharStats.Sum<AncientCharacterStats>((Func<AncientCharacterStats, int>) (fight => fight.Losses));
    }
  }

  public void IncrementWin(ModelId characterId)
  {
    AncientCharacterStats ancientCharacterStats = this.GetStats(characterId);
    if (ancientCharacterStats == null)
    {
      ancientCharacterStats = new AncientCharacterStats()
      {
        Character = characterId
      };
      this.CharStats.Add(ancientCharacterStats);
    }
    ++ancientCharacterStats.Wins;
    Log.Info($"{characterId} has won a run with ancient {this.Id}. That's {ancientCharacterStats.Wins} wins");
  }

  public void IncrementLoss(ModelId characterId)
  {
    AncientCharacterStats ancientCharacterStats = this.GetStats(characterId);
    if (ancientCharacterStats == null)
    {
      ancientCharacterStats = new AncientCharacterStats()
      {
        Character = characterId
      };
      this.CharStats.Add(ancientCharacterStats);
    }
    ++ancientCharacterStats.Losses;
  }

  public int GetVisitsAs(ModelId characterId)
  {
    AncientCharacterStats stats = this.GetStats(characterId);
    return stats == null ? 0 : stats.Visits;
  }

  private AncientCharacterStats? GetStats(ModelId characterId)
  {
    return this.CharStats.FirstOrDefault<AncientCharacterStats>((Func<AncientCharacterStats, bool>) (c => c.Character == characterId));
  }
}
