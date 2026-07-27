// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.EncounterStats
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

public class EncounterStats
{
  [JsonPropertyName("encounter_id")]
  public required ModelId Id { get; init; }

  [JsonPropertyName("fight_stats")]
  public List<MegaCrit.Sts2.Core.Saves.FightStats> FightStats { get; init; } = new List<MegaCrit.Sts2.Core.Saves.FightStats>();

  [JsonIgnore]
  public int TotalWins
  {
    get
    {
      return this.FightStats.Count == 0 ? 0 : this.FightStats.Sum<MegaCrit.Sts2.Core.Saves.FightStats>((Func<MegaCrit.Sts2.Core.Saves.FightStats, int>) (fight => fight.Wins));
    }
  }

  [JsonIgnore]
  public int TotalLosses
  {
    get
    {
      return this.FightStats.Count == 0 ? 0 : this.FightStats.Sum<MegaCrit.Sts2.Core.Saves.FightStats>((Func<MegaCrit.Sts2.Core.Saves.FightStats, int>) (fight => fight.Losses));
    }
  }

  public void IncrementWin(ModelId characterId)
  {
    MegaCrit.Sts2.Core.Saves.FightStats fightStats = this.FightStats.First<MegaCrit.Sts2.Core.Saves.FightStats>((Func<MegaCrit.Sts2.Core.Saves.FightStats, bool>) (fight => fight.Character == characterId));
    ++fightStats.Wins;
    Log.Info($"{characterId} has won against encounter {this.Id}. That's {fightStats.Wins} wins");
  }

  public void IncrementLoss(ModelId characterId)
  {
    MegaCrit.Sts2.Core.Saves.FightStats fightStats = this.FightStats.First<MegaCrit.Sts2.Core.Saves.FightStats>((Func<MegaCrit.Sts2.Core.Saves.FightStats, bool>) (fight => fight.Character == characterId));
    ++fightStats.Losses;
    Log.Info($"{characterId} has lost to encounter {this.Id}. That's {fightStats.Losses} losses");
  }
}
