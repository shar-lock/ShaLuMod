// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.SerializableProgress
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves;

public class SerializableProgress : ISaveSchema
{
  public SerializableProgress()
  {
    this.UniqueId = GenerateUniqueId();

    static string GenerateUniqueId(int length = 7)
    {
      return new string(Enumerable.Repeat<string>("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", length).Select<string, char>((Func<string, char>) (s => s[Rng.Chaotic.NextInt(s.Length)])).ToArray<char>());
    }
  }

  [JsonPropertyName("schema_version")]
  public int SchemaVersion { get; set; }

  [JsonPropertyName("unique_id")]
  public string UniqueId { get; init; }

  [JsonPropertyName("character_stats")]
  public List<CharacterStats> CharStats { get; set; } = new List<CharacterStats>();

  [JsonPropertyName("card_stats")]
  public List<MegaCrit.Sts2.Core.Saves.CardStats> CardStats { get; set; } = new List<MegaCrit.Sts2.Core.Saves.CardStats>();

  [JsonPropertyName("encounter_stats")]
  public List<MegaCrit.Sts2.Core.Saves.EncounterStats> EncounterStats { get; set; } = new List<MegaCrit.Sts2.Core.Saves.EncounterStats>();

  [JsonPropertyName("enemy_stats")]
  public List<MegaCrit.Sts2.Core.Saves.EnemyStats> EnemyStats { get; set; } = new List<MegaCrit.Sts2.Core.Saves.EnemyStats>();

  [JsonPropertyName("ancient_stats")]
  public List<MegaCrit.Sts2.Core.Saves.AncientStats> AncientStats { get; set; } = new List<MegaCrit.Sts2.Core.Saves.AncientStats>();

  [JsonPropertyName("enable_ftues")]
  public bool EnableFtues { get; set; } = true;

  [JsonPropertyName("epochs")]
  public List<SerializableEpoch> Epochs { get; set; } = new List<SerializableEpoch>();

  [JsonPropertyName("ftue_completed")]
  public List<string> FtueCompleted { get; set; } = new List<string>();

  [JsonPropertyName("unlocked_achievements")]
  public List<SerializableUnlockedAchievement> UnlockedAchievements { get; set; } = new List<SerializableUnlockedAchievement>();

  [JsonPropertyName("discovered_cards")]
  public List<ModelId> DiscoveredCards { get; set; } = new List<ModelId>();

  [JsonPropertyName("discovered_relics")]
  public List<ModelId> DiscoveredRelics { get; set; } = new List<ModelId>();

  [JsonPropertyName("discovered_events")]
  public List<ModelId> DiscoveredEvents { get; set; } = new List<ModelId>();

  [JsonPropertyName("discovered_potions")]
  public List<ModelId> DiscoveredPotions { get; set; } = new List<ModelId>();

  [JsonPropertyName("discovered_acts")]
  public List<ModelId> DiscoveredActs { get; set; } = new List<ModelId>();

  [JsonPropertyName("total_playtime")]
  public long TotalPlaytime { get; set; }

  [JsonPropertyName("total_unlocks")]
  public int TotalUnlocks { get; set; }

  [JsonPropertyName("current_score")]
  public int CurrentScore { get; set; }

  [JsonPropertyName("floors_climbed")]
  public long FloorsClimbed { get; set; }

  [JsonPropertyName("architect_damage")]
  public long ArchitectDamage { get; set; }

  [JsonPropertyName("wongo_points")]
  public int WongoPoints { get; set; }

  [JsonPropertyName("preferred_multiplayer_ascension")]
  public int PreferredMultiplayerAscension { get; set; }

  [JsonPropertyName("max_multiplayer_ascension")]
  public int MaxMultiplayerAscension { get; set; }

  [JsonPropertyName("test_subject_kills")]
  public int TestSubjectKills { get; set; }

  [JsonPropertyName("pending_character_unlock")]
  public ModelId PendingCharacterUnlock { get; set; } = ModelId.none;

  [JsonIgnore]
  public int Wins
  {
    get
    {
      return this.CharStats.Sum<CharacterStats>((Func<CharacterStats, int>) (character => character.TotalWins));
    }
  }

  [JsonIgnore]
  public int Losses
  {
    get
    {
      return this.CharStats.Sum<CharacterStats>((Func<CharacterStats, int>) (character => character.TotalLosses));
    }
  }

  [JsonIgnore]
  public long FastestVictory
  {
    get
    {
      return this.CharStats.Count == 0 ? 999999999L : this.CharStats.Min<CharacterStats>((Func<CharacterStats, long>) (c => c.FastestWinTime != -1L ? c.FastestWinTime : 999999999L));
    }
  }

  [JsonIgnore]
  public long BestWinStreak
  {
    get
    {
      return this.CharStats.Count == 0 ? 0L : this.CharStats.Max<CharacterStats>((Func<CharacterStats, long>) (c => c.BestWinStreak));
    }
  }

  [JsonIgnore]
  public int NumberOfRuns => this.Wins + this.Losses;

  public CharacterStats? GetStatsForCharacter(ModelId characterId)
  {
    return this.CharStats.FirstOrDefault<CharacterStats>((Func<CharacterStats, bool>) (c => c.Id == characterId));
  }

  public MegaCrit.Sts2.Core.Saves.AncientStats? GetStatsForAncient(ModelId ancientId)
  {
    return this.AncientStats.FirstOrDefault<MegaCrit.Sts2.Core.Saves.AncientStats>((Func<MegaCrit.Sts2.Core.Saves.AncientStats, bool>) (a => a.Id == ancientId));
  }
}
