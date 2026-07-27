// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.ProgressState
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Achievements;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;
using MegaCrit.Sts2.Core.Saves.Validation;
using MegaCrit.Sts2.Core.Timeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves;

public class ProgressState
{
  private static readonly Dictionary<string, Achievement> _achievementsByName = ProgressState.BuildAchievementLookup();
  private readonly Dictionary<ModelId, MegaCrit.Sts2.Core.Saves.CharacterStats> _characterStats = new Dictionary<ModelId, MegaCrit.Sts2.Core.Saves.CharacterStats>();
  private readonly Dictionary<ModelId, MegaCrit.Sts2.Core.Saves.CardStats> _cardStats = new Dictionary<ModelId, MegaCrit.Sts2.Core.Saves.CardStats>();
  private readonly Dictionary<ModelId, MegaCrit.Sts2.Core.Saves.EncounterStats> _encounterStats = new Dictionary<ModelId, MegaCrit.Sts2.Core.Saves.EncounterStats>();
  private readonly Dictionary<ModelId, MegaCrit.Sts2.Core.Saves.EnemyStats> _enemyStats = new Dictionary<ModelId, MegaCrit.Sts2.Core.Saves.EnemyStats>();
  private readonly Dictionary<ModelId, MegaCrit.Sts2.Core.Saves.AncientStats> _ancientStats = new Dictionary<ModelId, MegaCrit.Sts2.Core.Saves.AncientStats>();
  private readonly HashSet<ModelId> _discoveredCards = new HashSet<ModelId>();
  private readonly HashSet<ModelId> _discoveredRelics = new HashSet<ModelId>();
  private readonly HashSet<ModelId> _discoveredPotions = new HashSet<ModelId>();
  private readonly HashSet<ModelId> _discoveredEvents = new HashSet<ModelId>();
  private readonly HashSet<ModelId> _discoveredActs = new HashSet<ModelId>();
  private readonly List<SerializableEpoch> _epochs = new List<SerializableEpoch>();
  private readonly Dictionary<Achievement, long> _unlockedAchievements = new Dictionary<Achievement, long>();
  private readonly Dictionary<ModelId, MegaCrit.Sts2.Core.Saves.CharacterStats> _unknownCharacterStats = new Dictionary<ModelId, MegaCrit.Sts2.Core.Saves.CharacterStats>();
  private readonly Dictionary<ModelId, MegaCrit.Sts2.Core.Saves.CardStats> _unknownCardStats = new Dictionary<ModelId, MegaCrit.Sts2.Core.Saves.CardStats>();
  private readonly Dictionary<ModelId, MegaCrit.Sts2.Core.Saves.EncounterStats> _unknownEncounterStats = new Dictionary<ModelId, MegaCrit.Sts2.Core.Saves.EncounterStats>();
  private readonly Dictionary<ModelId, MegaCrit.Sts2.Core.Saves.EnemyStats> _unknownEnemyStats = new Dictionary<ModelId, MegaCrit.Sts2.Core.Saves.EnemyStats>();
  private readonly Dictionary<ModelId, MegaCrit.Sts2.Core.Saves.AncientStats> _unknownAncientStats = new Dictionary<ModelId, MegaCrit.Sts2.Core.Saves.AncientStats>();
  private readonly HashSet<ModelId> _unknownDiscoveredCards = new HashSet<ModelId>();
  private readonly HashSet<ModelId> _unknownDiscoveredRelics = new HashSet<ModelId>();
  private readonly HashSet<ModelId> _unknownDiscoveredPotions = new HashSet<ModelId>();
  private readonly HashSet<ModelId> _unknownDiscoveredEvents = new HashSet<ModelId>();
  private readonly HashSet<ModelId> _unknownDiscoveredActs = new HashSet<ModelId>();
  private readonly List<SerializableEpoch> _unknownEpochs = new List<SerializableEpoch>();
  private readonly List<SerializableUnlockedAchievement> _unknownUnlockedAchievements = new List<SerializableUnlockedAchievement>();
  private readonly HashSet<string> _ftueCompleted = new HashSet<string>();

  public IReadOnlyDictionary<ModelId, MegaCrit.Sts2.Core.Saves.CharacterStats> CharacterStats
  {
    get => (IReadOnlyDictionary<ModelId, MegaCrit.Sts2.Core.Saves.CharacterStats>) this._characterStats;
  }

  public IReadOnlyDictionary<ModelId, MegaCrit.Sts2.Core.Saves.CardStats> CardStats
  {
    get => (IReadOnlyDictionary<ModelId, MegaCrit.Sts2.Core.Saves.CardStats>) this._cardStats;
  }

  public IReadOnlyDictionary<ModelId, MegaCrit.Sts2.Core.Saves.EncounterStats> EncounterStats
  {
    get => (IReadOnlyDictionary<ModelId, MegaCrit.Sts2.Core.Saves.EncounterStats>) this._encounterStats;
  }

  public IReadOnlyDictionary<ModelId, MegaCrit.Sts2.Core.Saves.EnemyStats> EnemyStats
  {
    get => (IReadOnlyDictionary<ModelId, MegaCrit.Sts2.Core.Saves.EnemyStats>) this._enemyStats;
  }

  public IReadOnlyDictionary<ModelId, MegaCrit.Sts2.Core.Saves.AncientStats> AncientStats
  {
    get => (IReadOnlyDictionary<ModelId, MegaCrit.Sts2.Core.Saves.AncientStats>) this._ancientStats;
  }

  public IReadOnlySet<ModelId> DiscoveredCards => (IReadOnlySet<ModelId>) this._discoveredCards;

  public IReadOnlySet<ModelId> DiscoveredRelics => (IReadOnlySet<ModelId>) this._discoveredRelics;

  public IReadOnlySet<ModelId> DiscoveredPotions => (IReadOnlySet<ModelId>) this._discoveredPotions;

  public IReadOnlySet<ModelId> DiscoveredEvents => (IReadOnlySet<ModelId>) this._discoveredEvents;

  public IReadOnlySet<ModelId> DiscoveredActs => (IReadOnlySet<ModelId>) this._discoveredActs;

  public IReadOnlyList<SerializableEpoch> Epochs => (IReadOnlyList<SerializableEpoch>) this._epochs;

  public IReadOnlyDictionary<Achievement, long> UnlockedAchievements
  {
    get => (IReadOnlyDictionary<Achievement, long>) this._unlockedAchievements;
  }

  public IReadOnlySet<string> FtueCompleted => (IReadOnlySet<string>) this._ftueCompleted;

  public string UniqueId { get; init; } = "";

  public bool EnableFtues { get; set; } = true;

  public long TotalPlaytime { get; set; }

  public int TotalUnlocks { get; set; }

  public int CurrentScore { get; set; }

  public long FloorsClimbed { get; set; }

  public long ArchitectDamage { get; set; }

  public int WongoPoints { get; set; }

  public int PreferredMultiplayerAscension { get; set; }

  public int MaxMultiplayerAscension { get; set; }

  public int TestSubjectKills { get; set; }

  public ModelId PendingCharacterUnlock { get; set; } = ModelId.none;

  public int Wins
  {
    get
    {
      return this._characterStats.Values.Sum<MegaCrit.Sts2.Core.Saves.CharacterStats>((Func<MegaCrit.Sts2.Core.Saves.CharacterStats, int>) (c => c.TotalWins));
    }
  }

  public int Losses
  {
    get
    {
      return this._characterStats.Values.Sum<MegaCrit.Sts2.Core.Saves.CharacterStats>((Func<MegaCrit.Sts2.Core.Saves.CharacterStats, int>) (c => c.TotalLosses));
    }
  }

  public long FastestVictory
  {
    get
    {
      return this._characterStats.Count == 0 ? 999999999L : this._characterStats.Values.Min<MegaCrit.Sts2.Core.Saves.CharacterStats>((Func<MegaCrit.Sts2.Core.Saves.CharacterStats, long>) (c => c.FastestWinTime != -1L ? c.FastestWinTime : 999999999L));
    }
  }

  public long BestWinStreak
  {
    get
    {
      return this._characterStats.Count == 0 ? 0L : this._characterStats.Values.Max<MegaCrit.Sts2.Core.Saves.CharacterStats>((Func<MegaCrit.Sts2.Core.Saves.CharacterStats, long>) (c => c.BestWinStreak));
    }
  }

  public int NumberOfRuns => this.Wins + this.Losses;

  public static ProgressState CreateDefault()
  {
    return ProgressState.FromSerializable(new SerializableProgress(), new DeserializationContext());
  }

  public static ProgressState FromSerializable(
    SerializableProgress save,
    DeserializationContext ctx)
  {
    ArgumentNullException.ThrowIfNull((object) save, nameof (save));
    ArgumentNullException.ThrowIfNull((object) ctx, nameof (ctx));
    ProgressState progressState = new ProgressState()
    {
      UniqueId = save.UniqueId,
      EnableFtues = save.EnableFtues,
      TotalPlaytime = ProgressState.ClampNonNegative(save.TotalPlaytime, "TotalPlaytime", ctx),
      TotalUnlocks = ProgressState.ClampNonNegativeInt(save.TotalUnlocks, "TotalUnlocks", ctx),
      CurrentScore = ProgressState.ClampNonNegativeInt(save.CurrentScore, "CurrentScore", ctx),
      FloorsClimbed = ProgressState.ClampNonNegative(save.FloorsClimbed, "FloorsClimbed", ctx),
      ArchitectDamage = ProgressState.ClampNonNegative(save.ArchitectDamage, "ArchitectDamage", ctx),
      WongoPoints = ProgressState.ClampNonNegativeInt(save.WongoPoints, "WongoPoints", ctx),
      PreferredMultiplayerAscension = ProgressState.ClampAscension(save.PreferredMultiplayerAscension, "PreferredMultiplayerAscension", ctx),
      MaxMultiplayerAscension = ProgressState.ClampAscension(save.MaxMultiplayerAscension, "MaxMultiplayerAscension", ctx),
      TestSubjectKills = ProgressState.ClampNonNegativeInt(save.TestSubjectKills, "TestSubjectKills", ctx),
      PendingCharacterUnlock = ProgressState.ValidateModelId<CharacterModel>(save.PendingCharacterUnlock, "PendingCharacterUnlock", ctx)
    };
    ProgressState.ParseCharacterStats(save.CharStats, progressState._characterStats, progressState._unknownCharacterStats, ctx);
    ProgressState.ParseCardStats(save.CardStats, progressState._cardStats, progressState._unknownCardStats, ctx);
    ProgressState.ParseEncounterStats(save.EncounterStats, progressState._encounterStats, progressState._unknownEncounterStats, ctx);
    ProgressState.ParseEnemyStats(save.EnemyStats, progressState._enemyStats, progressState._unknownEnemyStats, ctx);
    ProgressState.ParseAncientStats(save.AncientStats, progressState._ancientStats, progressState._unknownAncientStats, ctx);
    ProgressState.ParseDiscoveredSet<CardModel>(save.DiscoveredCards, progressState._discoveredCards, progressState._unknownDiscoveredCards, "DiscoveredCards", ctx);
    ProgressState.ParseDiscoveredSet<RelicModel>(save.DiscoveredRelics, progressState._discoveredRelics, progressState._unknownDiscoveredRelics, "DiscoveredRelics", ctx);
    ProgressState.ParseDiscoveredSet<PotionModel>(save.DiscoveredPotions, progressState._discoveredPotions, progressState._unknownDiscoveredPotions, "DiscoveredPotions", ctx);
    ProgressState.ParseDiscoveredSet<EventModel>(save.DiscoveredEvents, progressState._discoveredEvents, progressState._unknownDiscoveredEvents, "DiscoveredEvents", ctx);
    ProgressState.ParseDiscoveredSet<ActModel>(save.DiscoveredActs, progressState._discoveredActs, progressState._unknownDiscoveredActs, "DiscoveredActs", ctx);
    ProgressState.ParseEpochs(save.Epochs, progressState._epochs, progressState._unknownEpochs, ctx);
    ProgressState.FixMissingSlots(progressState._epochs, ctx);
    ProgressState.ParseFtues(save.FtueCompleted, progressState._ftueCompleted, ctx);
    ProgressState.ParseAchievements(save.UnlockedAchievements, progressState._unlockedAchievements, progressState._unknownUnlockedAchievements, ctx);
    progressState.FilterAndSortEpochs();
    return progressState;
  }

  public SerializableProgress ToSerializable()
  {
    return new SerializableProgress()
    {
      UniqueId = this.UniqueId,
      SchemaVersion = 0,
      EnableFtues = this.EnableFtues,
      TotalPlaytime = this.TotalPlaytime,
      TotalUnlocks = this.TotalUnlocks,
      CurrentScore = this.CurrentScore,
      FloorsClimbed = this.FloorsClimbed,
      ArchitectDamage = this.ArchitectDamage,
      WongoPoints = this.WongoPoints,
      PreferredMultiplayerAscension = this.PreferredMultiplayerAscension,
      MaxMultiplayerAscension = this.MaxMultiplayerAscension,
      TestSubjectKills = this.TestSubjectKills,
      PendingCharacterUnlock = this.PendingCharacterUnlock,
      CharStats = this._characterStats.Values.Concat<MegaCrit.Sts2.Core.Saves.CharacterStats>((IEnumerable<MegaCrit.Sts2.Core.Saves.CharacterStats>) this._unknownCharacterStats.Values).ToList<MegaCrit.Sts2.Core.Saves.CharacterStats>(),
      CardStats = this._cardStats.Values.Concat<MegaCrit.Sts2.Core.Saves.CardStats>((IEnumerable<MegaCrit.Sts2.Core.Saves.CardStats>) this._unknownCardStats.Values).ToList<MegaCrit.Sts2.Core.Saves.CardStats>(),
      EncounterStats = this._encounterStats.Values.Concat<MegaCrit.Sts2.Core.Saves.EncounterStats>((IEnumerable<MegaCrit.Sts2.Core.Saves.EncounterStats>) this._unknownEncounterStats.Values).ToList<MegaCrit.Sts2.Core.Saves.EncounterStats>(),
      EnemyStats = this._enemyStats.Values.Concat<MegaCrit.Sts2.Core.Saves.EnemyStats>((IEnumerable<MegaCrit.Sts2.Core.Saves.EnemyStats>) this._unknownEnemyStats.Values).ToList<MegaCrit.Sts2.Core.Saves.EnemyStats>(),
      AncientStats = this._ancientStats.Values.Concat<MegaCrit.Sts2.Core.Saves.AncientStats>((IEnumerable<MegaCrit.Sts2.Core.Saves.AncientStats>) this._unknownAncientStats.Values).ToList<MegaCrit.Sts2.Core.Saves.AncientStats>(),
      DiscoveredCards = this._discoveredCards.Concat<ModelId>((IEnumerable<ModelId>) this._unknownDiscoveredCards).ToList<ModelId>(),
      DiscoveredRelics = this._discoveredRelics.Concat<ModelId>((IEnumerable<ModelId>) this._unknownDiscoveredRelics).ToList<ModelId>(),
      DiscoveredPotions = this._discoveredPotions.Concat<ModelId>((IEnumerable<ModelId>) this._unknownDiscoveredPotions).ToList<ModelId>(),
      DiscoveredEvents = this._discoveredEvents.Concat<ModelId>((IEnumerable<ModelId>) this._unknownDiscoveredEvents).ToList<ModelId>(),
      DiscoveredActs = this._discoveredActs.Concat<ModelId>((IEnumerable<ModelId>) this._unknownDiscoveredActs).ToList<ModelId>(),
      Epochs = this._epochs.Concat<SerializableEpoch>((IEnumerable<SerializableEpoch>) this._unknownEpochs).ToList<SerializableEpoch>(),
      FtueCompleted = this._ftueCompleted.ToList<string>(),
      UnlockedAchievements = this._unlockedAchievements.Select<KeyValuePair<Achievement, long>, SerializableUnlockedAchievement>((Func<KeyValuePair<Achievement, long>, SerializableUnlockedAchievement>) (kvp => new SerializableUnlockedAchievement()
      {
        Achievement = JsonNamingPolicy.SnakeCaseLower.ConvertName(kvp.Key.ToString()),
        UnlockTime = kvp.Value
      })).Concat<SerializableUnlockedAchievement>((IEnumerable<SerializableUnlockedAchievement>) this._unknownUnlockedAchievements).ToList<SerializableUnlockedAchievement>()
    };
  }

  public MegaCrit.Sts2.Core.Saves.CharacterStats GetOrCreateCharacterStats(ModelId characterId)
  {
    MegaCrit.Sts2.Core.Saves.CharacterStats characterStats1;
    if (this._characterStats.TryGetValue(characterId, out characterStats1))
      return characterStats1;
    MegaCrit.Sts2.Core.Saves.CharacterStats characterStats2 = new MegaCrit.Sts2.Core.Saves.CharacterStats()
    {
      Id = characterId
    };
    this._characterStats[characterId] = characterStats2;
    return characterStats2;
  }

  public MegaCrit.Sts2.Core.Saves.CardStats GetOrCreateCardStats(ModelId cardId)
  {
    MegaCrit.Sts2.Core.Saves.CardStats cardStats1;
    if (this._cardStats.TryGetValue(cardId, out cardStats1))
      return cardStats1;
    MegaCrit.Sts2.Core.Saves.CardStats cardStats2 = new MegaCrit.Sts2.Core.Saves.CardStats()
    {
      Id = cardId
    };
    this._cardStats[cardId] = cardStats2;
    return cardStats2;
  }

  public bool MarkCardAsSeen(ModelId cardId) => this._discoveredCards.Add(cardId);

  public bool MarkRelicAsSeen(ModelId relicId) => this._discoveredRelics.Add(relicId);

  public bool MarkPotionAsSeen(ModelId potionId) => this._discoveredPotions.Add(potionId);

  public bool MarkEventAsSeen(ModelId eventId) => this._discoveredEvents.Add(eventId);

  public bool MarkActAsSeen(ModelId actId) => this._discoveredActs.Add(actId);

  public bool MarkFtueAsComplete(string ftueId) => this._ftueCompleted.Add(ftueId);

  public void AddUnlockedAchievement(Achievement achievement, long unlockTime)
  {
    this._unlockedAchievements[achievement] = unlockTime;
  }

  public bool RemoveUnlockedAchievement(Achievement achievement)
  {
    return this._unlockedAchievements.Remove(achievement);
  }

  public bool IsAchievementUnlocked(Achievement achievement)
  {
    return this._unlockedAchievements.ContainsKey(achievement);
  }

  public void ObtainEpoch(string epochId)
  {
    SerializableEpoch serializableEpoch = this._epochs.FirstOrDefault<SerializableEpoch>((Func<SerializableEpoch, bool>) (e => e.Id == epochId));
    if (serializableEpoch != null)
    {
      serializableEpoch.SetObtained(EpochState.Obtained);
    }
    else
    {
      this._epochs.Add(new SerializableEpoch(epochId, EpochState.ObtainedNoSlot));
      this.FilterAndSortEpochs();
    }
  }

  public void ObtainEpochOverride(string epochId, EpochState state)
  {
    SerializableEpoch serializableEpoch = this._epochs.FirstOrDefault<SerializableEpoch>((Func<SerializableEpoch, bool>) (e => e.Id == epochId));
    if (serializableEpoch != null)
    {
      if (serializableEpoch.ObtainDate == 0L && state >= EpochState.ObtainedNoSlot)
        serializableEpoch.ObtainDate = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
      serializableEpoch.State = state;
    }
    else
    {
      this._epochs.Add(new SerializableEpoch(epochId, state));
      this.FilterAndSortEpochs();
    }
  }

  public void UnlockSlot(string epochId)
  {
    SerializableEpoch serializableEpoch = this._epochs.FirstOrDefault<SerializableEpoch>((Func<SerializableEpoch, bool>) (e => e.Id == epochId));
    if (serializableEpoch == null)
    {
      this._epochs.Add(new SerializableEpoch(epochId, EpochState.NotObtained));
      this.FilterAndSortEpochs();
    }
    else if (serializableEpoch.State == EpochState.ObtainedNoSlot)
    {
      Log.Info($"Attempted to get slot {epochId} but we already have an Epoch! Set to Obtained");
      serializableEpoch.State = EpochState.Obtained;
    }
    else
      Log.Error($"Slot unlocked for {epochId} but it's in an invalid state: {serializableEpoch.State}");
  }

  public void RevealEpoch(string epochId)
  {
    SerializableEpoch serializableEpoch = this._epochs.FirstOrDefault<SerializableEpoch>((Func<SerializableEpoch, bool>) (e => e.Id == epochId));
    if (serializableEpoch == null)
      throw new InvalidOperationException($"Invalid epoch {epochId} passed to {nameof (RevealEpoch)}!");
    serializableEpoch.State = EpochState.Revealed;
  }

  public void ResetEpochs() => this._epochs.Clear();

  public MegaCrit.Sts2.Core.Saves.CharacterStats? GetStatsForCharacter(ModelId characterId)
  {
    MegaCrit.Sts2.Core.Saves.CharacterStats statsForCharacter;
    this._characterStats.TryGetValue(characterId, out statsForCharacter);
    return statsForCharacter;
  }

  public MegaCrit.Sts2.Core.Saves.EncounterStats GetOrCreateEncounterStats(ModelId encounterId)
  {
    MegaCrit.Sts2.Core.Saves.EncounterStats encounterStats1;
    if (this._encounterStats.TryGetValue(encounterId, out encounterStats1))
      return encounterStats1;
    MegaCrit.Sts2.Core.Saves.EncounterStats encounterStats2 = new MegaCrit.Sts2.Core.Saves.EncounterStats()
    {
      Id = encounterId
    };
    this._encounterStats[encounterId] = encounterStats2;
    return encounterStats2;
  }

  public MegaCrit.Sts2.Core.Saves.EnemyStats GetOrCreateEnemyStats(ModelId enemyId)
  {
    MegaCrit.Sts2.Core.Saves.EnemyStats enemyStats1;
    if (this._enemyStats.TryGetValue(enemyId, out enemyStats1))
      return enemyStats1;
    MegaCrit.Sts2.Core.Saves.EnemyStats enemyStats2 = new MegaCrit.Sts2.Core.Saves.EnemyStats()
    {
      Id = enemyId
    };
    this._enemyStats[enemyId] = enemyStats2;
    return enemyStats2;
  }

  public MegaCrit.Sts2.Core.Saves.AncientStats GetOrCreateAncientStats(ModelId ancientId)
  {
    MegaCrit.Sts2.Core.Saves.AncientStats ancientStats1;
    if (this._ancientStats.TryGetValue(ancientId, out ancientStats1))
      return ancientStats1;
    MegaCrit.Sts2.Core.Saves.AncientStats ancientStats2 = new MegaCrit.Sts2.Core.Saves.AncientStats()
    {
      Id = ancientId
    };
    this._ancientStats[ancientId] = ancientStats2;
    return ancientStats2;
  }

  public MegaCrit.Sts2.Core.Saves.AncientStats? GetStatsForAncient(ModelId ancientId)
  {
    MegaCrit.Sts2.Core.Saves.AncientStats statsForAncient;
    this._ancientStats.TryGetValue(ancientId, out statsForAncient);
    return statsForAncient;
  }

  public void ResetFtues()
  {
    this.EnableFtues = true;
    this._ftueCompleted.Clear();
  }

  public bool HasEpoch(string epochId)
  {
    return this._epochs.Any<SerializableEpoch>((Func<SerializableEpoch, bool>) (e => e.Id == epochId));
  }

  public bool IsEpochObtained(string epochId)
  {
    SerializableEpoch serializableEpoch = this._epochs.FirstOrDefault<SerializableEpoch>((Func<SerializableEpoch, bool>) (e => e.Id == epochId));
    return serializableEpoch != null && serializableEpoch.State >= EpochState.ObtainedNoSlot;
  }

  public bool IsEpochRevealed(string epochId)
  {
    SerializableEpoch serializableEpoch = this._epochs.FirstOrDefault<SerializableEpoch>((Func<SerializableEpoch, bool>) (e => e.Id == epochId));
    return serializableEpoch != null && serializableEpoch.State >= EpochState.Revealed;
  }

  private static void ParseCharacterStats(
    List<MegaCrit.Sts2.Core.Saves.CharacterStats> source,
    Dictionary<ModelId, MegaCrit.Sts2.Core.Saves.CharacterStats> target,
    Dictionary<ModelId, MegaCrit.Sts2.Core.Saves.CharacterStats> unknown,
    DeserializationContext ctx)
  {
    ctx.PushPath("CharStats");
    for (int index = 0; index < source.Count; ++index)
    {
      MegaCrit.Sts2.Core.Saves.CharacterStats characterStats = source[index];
      ctx.PushPath($"[{index}]");
      ModelId id = characterStats.Id;
      if ((object) id == null || id == ModelId.none)
      {
        ctx.Warn("Null or none character ID, skipping");
        ctx.PopPath();
      }
      else if (ModelDb.GetByIdOrNull<CharacterModel>(id) == null)
      {
        ctx.Warn($"Unknown character ID: {id}");
        ProgressState.AddCharacterStats(characterStats, unknown, ctx);
        ctx.PopPath();
      }
      else
      {
        ProgressState.AddCharacterStats(characterStats, target, ctx);
        ProgressState.ClampCharacterStatsFields(characterStats, ctx);
        ctx.PopPath();
      }
    }
    ctx.PopPath();
  }

  private static void AddCharacterStats(
    MegaCrit.Sts2.Core.Saves.CharacterStats entry,
    Dictionary<ModelId, MegaCrit.Sts2.Core.Saves.CharacterStats> target,
    DeserializationContext ctx)
  {
    if (target.TryAdd(entry.Id, entry))
      return;
    ctx.Warn($"Duplicate character stats for {entry.Id}, merging");
    ProgressState.MergeCharacterStats(target[entry.Id], entry);
  }

  private static void ParseCardStats(
    List<MegaCrit.Sts2.Core.Saves.CardStats> source,
    Dictionary<ModelId, MegaCrit.Sts2.Core.Saves.CardStats> target,
    Dictionary<ModelId, MegaCrit.Sts2.Core.Saves.CardStats> unknown,
    DeserializationContext ctx)
  {
    ctx.PushPath("CardStats");
    for (int index = 0; index < source.Count; ++index)
    {
      MegaCrit.Sts2.Core.Saves.CardStats cardStats = source[index];
      ctx.PushPath($"[{index}]");
      ModelId id = cardStats.Id;
      if ((object) id == null || id == ModelId.none)
      {
        ctx.Warn("Null or none card ID, skipping");
        ctx.PopPath();
      }
      else if (ModelDb.GetByIdOrNull<CardModel>(id) == null)
      {
        ctx.Warn($"Unknown card ID: {id}");
        ProgressState.AddCardStats(cardStats, unknown, ctx);
        ctx.PopPath();
      }
      else
      {
        ProgressState.AddCardStats(cardStats, target, ctx);
        ProgressState.ClampCardStatsFields(cardStats, ctx);
        ctx.PopPath();
      }
    }
    ctx.PopPath();
  }

  private static void AddCardStats(
    MegaCrit.Sts2.Core.Saves.CardStats entry,
    Dictionary<ModelId, MegaCrit.Sts2.Core.Saves.CardStats> target,
    DeserializationContext ctx)
  {
    if (target.TryAdd(entry.Id, entry))
      return;
    ctx.Warn($"Duplicate card stats for {entry.Id}, merging");
    ProgressState.MergeCardStats(target[entry.Id], entry);
  }

  private static void ParseEncounterStats(
    List<MegaCrit.Sts2.Core.Saves.EncounterStats> source,
    Dictionary<ModelId, MegaCrit.Sts2.Core.Saves.EncounterStats> target,
    Dictionary<ModelId, MegaCrit.Sts2.Core.Saves.EncounterStats> unknown,
    DeserializationContext ctx)
  {
    ctx.PushPath("EncounterStats");
    for (int index = 0; index < source.Count; ++index)
    {
      MegaCrit.Sts2.Core.Saves.EncounterStats entry = source[index];
      ctx.PushPath($"[{index}]");
      if ((object) entry.Id == null || entry.Id == ModelId.none)
      {
        ctx.Warn("Null or none encounter ID, skipping");
        ctx.PopPath();
      }
      else if (ModelDb.GetByIdOrNull<EncounterModel>(entry.Id) == null)
      {
        ctx.Warn($"Unknown encounter ID: {entry.Id}");
        ProgressState.AddEncounterStats(entry, unknown, ctx);
        ctx.PopPath();
      }
      else
      {
        ProgressState.AddEncounterStats(entry, target, ctx);
        ProgressState.ClampFightStatsFields(entry.FightStats, ctx);
        ctx.PopPath();
      }
    }
    ctx.PopPath();
  }

  private static void AddEncounterStats(
    MegaCrit.Sts2.Core.Saves.EncounterStats entry,
    Dictionary<ModelId, MegaCrit.Sts2.Core.Saves.EncounterStats> target,
    DeserializationContext ctx)
  {
    if (target.TryAdd(entry.Id, entry))
      return;
    ctx.Warn($"Duplicate encounter stats for {entry.Id}, merging");
    ProgressState.MergeFightStatsList(target[entry.Id].FightStats, entry.FightStats);
  }

  private static void ParseEnemyStats(
    List<MegaCrit.Sts2.Core.Saves.EnemyStats> source,
    Dictionary<ModelId, MegaCrit.Sts2.Core.Saves.EnemyStats> target,
    Dictionary<ModelId, MegaCrit.Sts2.Core.Saves.EnemyStats> unknown,
    DeserializationContext ctx)
  {
    ctx.PushPath("EnemyStats");
    for (int index = 0; index < source.Count; ++index)
    {
      MegaCrit.Sts2.Core.Saves.EnemyStats entry = source[index];
      ctx.PushPath($"[{index}]");
      if ((object) entry.Id == null || entry.Id == ModelId.none)
      {
        ctx.Warn("Null or none enemy ID, skipping");
        ctx.PopPath();
      }
      else if (ModelDb.GetByIdOrNull<MonsterModel>(entry.Id) == null)
      {
        ctx.Warn($"Unknown enemy ID: {entry.Id}");
        ProgressState.AddEnemyStats(entry, unknown, ctx);
        ctx.PopPath();
      }
      else
      {
        ProgressState.AddEnemyStats(entry, target, ctx);
        ProgressState.ClampFightStatsFields(entry.FightStats, ctx);
        ctx.PopPath();
      }
    }
    ctx.PopPath();
  }

  private static void AddEnemyStats(
    MegaCrit.Sts2.Core.Saves.EnemyStats entry,
    Dictionary<ModelId, MegaCrit.Sts2.Core.Saves.EnemyStats> target,
    DeserializationContext ctx)
  {
    if (target.TryAdd(entry.Id, entry))
      return;
    ctx.Warn($"Duplicate enemy stats for {entry.Id}, merging");
    ProgressState.MergeFightStatsList(target[entry.Id].FightStats, entry.FightStats);
  }

  private static void ParseAncientStats(
    List<MegaCrit.Sts2.Core.Saves.AncientStats> source,
    Dictionary<ModelId, MegaCrit.Sts2.Core.Saves.AncientStats> target,
    Dictionary<ModelId, MegaCrit.Sts2.Core.Saves.AncientStats> unknown,
    DeserializationContext ctx)
  {
    ctx.PushPath("AncientStats");
    for (int index = 0; index < source.Count; ++index)
    {
      MegaCrit.Sts2.Core.Saves.AncientStats entry = source[index];
      ctx.PushPath($"[{index}]");
      if ((object) entry.Id == null || entry.Id == ModelId.none)
      {
        ctx.Warn("Null or none ancient ID, skipping");
        ctx.PopPath();
      }
      else if (ModelDb.GetByIdOrNull<EventModel>(entry.Id) == null)
      {
        ctx.Warn($"Unknown ancient event ID: {entry.Id}");
        ProgressState.AddAncientStats(entry, unknown, ctx);
        ctx.PopPath();
      }
      else
      {
        ProgressState.ClampAncientCharacterStatsFields(entry.CharStats, ctx);
        ProgressState.AddAncientStats(entry, target, ctx);
        ctx.PopPath();
      }
    }
    ctx.PopPath();
  }

  private static void AddAncientStats(
    MegaCrit.Sts2.Core.Saves.AncientStats entry,
    Dictionary<ModelId, MegaCrit.Sts2.Core.Saves.AncientStats> target,
    DeserializationContext ctx)
  {
    if (target.TryAdd(entry.Id, entry))
      return;
    ctx.Warn($"Duplicate ancient stats for {entry.Id}, merging");
    ProgressState.MergeAncientCharacterStatsList(target[entry.Id].CharStats, entry.CharStats);
  }

  private static void ParseDiscoveredSet<TModel>(
    List<ModelId> source,
    HashSet<ModelId> target,
    HashSet<ModelId> unknown,
    string fieldName,
    DeserializationContext ctx)
    where TModel : AbstractModel
  {
    ctx.PushPath(fieldName);
    for (int index = 0; index < source.Count; ++index)
    {
      ModelId id = source[index];
      if ((object) id == null || id == ModelId.none)
        ctx.Warn($"Null or none ID at index {index}, skipping");
      else if ((object) ModelDb.GetByIdOrNull<TModel>(id) == null)
      {
        ctx.Warn($"Unknown {typeof (TModel).Name} ID: {id}");
        unknown.Add(id);
      }
      else if (!target.Add(id))
        ctx.Warn($"Duplicate ID: {id}, skipping");
    }
    ctx.PopPath();
  }

  private static void ParseEpochs(
    List<SerializableEpoch> source,
    List<SerializableEpoch> target,
    List<SerializableEpoch> unknown,
    DeserializationContext ctx)
  {
    ctx.PushPath("Epochs");
    HashSet<string> stringSet = new HashSet<string>();
    for (int index = 0; index < source.Count; ++index)
    {
      SerializableEpoch serializableEpoch = source[index];
      ctx.PushPath($"[{index}]");
      if (!EpochModel.IsValid(serializableEpoch.Id))
      {
        ctx.Warn("Unknown epoch ID: " + serializableEpoch.Id);
        unknown.Add(serializableEpoch);
        ctx.PopPath();
      }
      else if (!Enum.IsDefined<EpochState>(serializableEpoch.State))
      {
        ctx.Warn($"Invalid epoch state {(int) serializableEpoch.State} for {serializableEpoch.Id}, skipping");
        ctx.PopPath();
      }
      else if (serializableEpoch.State < EpochState.NotObtained)
      {
        ctx.Warn($"Epoch {serializableEpoch.Id} has unused state {serializableEpoch.State}, skipping");
        ctx.PopPath();
      }
      else if (!stringSet.Add(serializableEpoch.Id))
      {
        ctx.Warn($"Duplicate epoch ID: {serializableEpoch.Id}, keeping first");
        ctx.PopPath();
      }
      else
      {
        target.Add(serializableEpoch);
        ctx.PopPath();
      }
    }
    ctx.PopPath();
  }

  private static void FixMissingSlots(List<SerializableEpoch> epochs, DeserializationContext ctx)
  {
    Dictionary<string, SerializableEpoch> dictionary = new Dictionary<string, SerializableEpoch>();
    foreach (SerializableEpoch epoch in epochs)
      dictionary[epoch.Id] = epoch;
    foreach (SerializableEpoch serializableEpoch1 in epochs.ToList<SerializableEpoch>())
    {
      if (serializableEpoch1.State >= EpochState.Revealed)
      {
        foreach (EpochModel epochModel in EpochModel.Get(serializableEpoch1.Id).GetTimelineExpansion())
        {
          SerializableEpoch serializableEpoch2;
          if (dictionary.TryGetValue(epochModel.Id, out serializableEpoch2))
          {
            if (serializableEpoch2.State == EpochState.ObtainedNoSlot)
            {
              ctx.Warn($"Epoch {epochModel.Id} was ObtainedNoSlot but parent {serializableEpoch1.Id} is Revealed, promoting to Obtained");
              serializableEpoch2.State = EpochState.Obtained;
            }
          }
          else
          {
            ctx.Warn($"Epoch {epochModel.Id} slot missing but parent {serializableEpoch1.Id} is Revealed, creating as NotObtained");
            SerializableEpoch serializableEpoch3 = new SerializableEpoch(epochModel.Id, EpochState.NotObtained);
            epochs.Add(serializableEpoch3);
            dictionary[epochModel.Id] = serializableEpoch3;
          }
        }
      }
    }
  }

  private static void ParseFtues(
    List<string> source,
    HashSet<string> target,
    DeserializationContext ctx)
  {
    ctx.PushPath("FtueCompleted");
    for (int index = 0; index < source.Count; ++index)
    {
      if (!target.Add(source[index]))
        ctx.Warn($"Duplicate FTUE: {source[index]}, skipping");
    }
    ctx.PopPath();
  }

  private static void ParseAchievements(
    List<SerializableUnlockedAchievement>? source,
    Dictionary<Achievement, long> target,
    List<SerializableUnlockedAchievement> unknown,
    DeserializationContext ctx)
  {
    if (source == null)
      return;
    ctx.PushPath("UnlockedAchievements");
    for (int index = 0; index < source.Count; ++index)
    {
      SerializableUnlockedAchievement unlockedAchievement = source[index];
      Achievement achievement;
      if (!ProgressState._achievementsByName.TryGetValue(unlockedAchievement.Achievement, out achievement))
      {
        ctx.Warn($"Unknown achievement \"{unlockedAchievement.Achievement}\" at index {index}");
        unknown.Add(unlockedAchievement);
      }
      else if (!target.TryAdd(achievement, unlockedAchievement.UnlockTime))
        ctx.Warn($"Duplicate achievement {achievement} at index {index}, keeping first");
    }
    ctx.PopPath();
  }

  private static Dictionary<string, Achievement> BuildAchievementLookup()
  {
    Dictionary<string, Achievement> dictionary = new Dictionary<string, Achievement>();
    foreach (Achievement achievement in Enum.GetValues<Achievement>())
    {
      string key = JsonNamingPolicy.SnakeCaseLower.ConvertName(achievement.ToString());
      dictionary[key] = achievement;
    }
    return dictionary;
  }

  private static void ClampCharacterStatsFields(MegaCrit.Sts2.Core.Saves.CharacterStats stats, DeserializationContext ctx)
  {
    if (stats.TotalWins < 0)
    {
      ctx.Warn($"Negative TotalWins ({stats.TotalWins}), clamping to 0");
      stats.TotalWins = 0;
    }
    if (stats.TotalLosses < 0)
    {
      ctx.Warn($"Negative TotalLosses ({stats.TotalLosses}), clamping to 0");
      stats.TotalLosses = 0;
    }
    if (stats.Playtime < 0L)
    {
      ctx.Warn($"Negative Playtime ({stats.Playtime}), clamping to 0");
      stats.Playtime = 0L;
    }
    if (stats.MaxAscension < 0)
    {
      ctx.Warn($"Negative MaxAscension ({stats.MaxAscension}), clamping to 0");
      stats.MaxAscension = 0;
    }
    if (stats.MaxAscension > 10)
    {
      ctx.Warn($"MaxAscension ({stats.MaxAscension}) exceeds allowed ({10}), clamping");
      stats.MaxAscension = 10;
    }
    if (stats.PreferredAscension < 0)
    {
      ctx.Warn($"Negative PreferredAscension ({stats.PreferredAscension}), clamping to 0");
      stats.PreferredAscension = 0;
    }
    if (stats.PreferredAscension > 10)
    {
      ctx.Warn($"PreferredAscension ({stats.PreferredAscension}) exceeds allowed ({10}), clamping");
      stats.PreferredAscension = 10;
    }
    if (stats.BestWinStreak < 0L)
    {
      ctx.Warn($"Negative BestWinStreak ({stats.BestWinStreak}), clamping to 0");
      stats.BestWinStreak = 0L;
    }
    if (stats.CurrentWinStreak < 0L)
    {
      ctx.Warn($"Negative CurrentWinStreak ({stats.CurrentWinStreak}), clamping to 0");
      stats.CurrentWinStreak = 0L;
    }
    if (stats.FastestWinTime >= -1L)
      return;
    ctx.Warn($"Invalid FastestWinTime ({stats.FastestWinTime}), resetting to -1");
    stats.FastestWinTime = -1L;
  }

  private static void ClampCardStatsFields(MegaCrit.Sts2.Core.Saves.CardStats stats, DeserializationContext ctx)
  {
    if (stats.TimesPicked < 0L)
    {
      ctx.Warn($"Negative TimesPicked ({stats.TimesPicked}), clamping to 0");
      stats.TimesPicked = 0L;
    }
    if (stats.TimesSkipped < 0L)
    {
      ctx.Warn($"Negative TimesSkipped ({stats.TimesSkipped}), clamping to 0");
      stats.TimesSkipped = 0L;
    }
    if (stats.TimesWon < 0L)
    {
      ctx.Warn($"Negative TimesWon ({stats.TimesWon}), clamping to 0");
      stats.TimesWon = 0L;
    }
    if (stats.TimesLost >= 0L)
      return;
    ctx.Warn($"Negative TimesLost ({stats.TimesLost}), clamping to 0");
    stats.TimesLost = 0L;
  }

  private static void ClampFightStatsFields(List<FightStats>? fightStats, DeserializationContext ctx)
  {
    if (fightStats == null)
      return;
    for (int index = fightStats.Count - 1; index >= 0; --index)
    {
      FightStats fight = fightStats[index];
      ctx.PushPath($"FightStats[{index}]");
      if (fight.Character == ModelId.none || ModelDb.GetByIdOrNull<CharacterModel>(fight.Character) == null)
      {
        ctx.Warn($"Unknown character ID: {fight.Character}, removing");
        fightStats.RemoveAt(index);
        ctx.PopPath();
      }
      else
      {
        FightStats fightStats1 = fightStats.Take<FightStats>(index).FirstOrDefault<FightStats>((Func<FightStats, bool>) (f => f.Character == fight.Character));
        if (fightStats1 != null)
        {
          ctx.Warn($"Duplicate character {fight.Character}, merging into earlier entry");
          fightStats1.Wins = Math.Max(fightStats1.Wins, fight.Wins);
          fightStats1.Losses = Math.Max(fightStats1.Losses, fight.Losses);
          fightStats.RemoveAt(index);
          ctx.PopPath();
        }
        else
        {
          if (fight.Wins < 0)
          {
            ctx.Warn($"Negative Wins ({fight.Wins}), clamping to 0");
            fight.Wins = 0;
          }
          if (fight.Losses < 0)
          {
            ctx.Warn($"Negative Losses ({fight.Losses}), clamping to 0");
            fight.Losses = 0;
          }
          ctx.PopPath();
        }
      }
    }
  }

  private static void ClampAncientCharacterStatsFields(
    List<AncientCharacterStats>? charStats,
    DeserializationContext ctx)
  {
    if (charStats == null)
      return;
    for (int index = charStats.Count - 1; index >= 0; --index)
    {
      AncientCharacterStats stats = charStats[index];
      ctx.PushPath($"CharStats[{index}]");
      if (stats.Character == ModelId.none || ModelDb.GetByIdOrNull<CharacterModel>(stats.Character) == null)
      {
        ctx.Warn($"Unknown character ID: {stats.Character}, removing");
        charStats.RemoveAt(index);
        ctx.PopPath();
      }
      else
      {
        AncientCharacterStats ancientCharacterStats = charStats.Take<AncientCharacterStats>(index).FirstOrDefault<AncientCharacterStats>((Func<AncientCharacterStats, bool>) (c => c.Character == stats.Character));
        if (ancientCharacterStats != null)
        {
          ctx.Warn($"Duplicate character {stats.Character}, merging into earlier entry");
          ancientCharacterStats.Wins = Math.Max(ancientCharacterStats.Wins, stats.Wins);
          ancientCharacterStats.Losses = Math.Max(ancientCharacterStats.Losses, stats.Losses);
          charStats.RemoveAt(index);
          ctx.PopPath();
        }
        else
        {
          if (stats.Wins < 0)
          {
            ctx.Warn($"Negative Wins ({stats.Wins}), clamping to 0");
            stats.Wins = 0;
          }
          if (stats.Losses < 0)
          {
            ctx.Warn($"Negative Losses ({stats.Losses}), clamping to 0");
            stats.Losses = 0;
          }
          ctx.PopPath();
        }
      }
    }
  }

  private static void MergeCharacterStats(MegaCrit.Sts2.Core.Saves.CharacterStats existing, MegaCrit.Sts2.Core.Saves.CharacterStats incoming)
  {
    existing.TotalWins = Math.Max(existing.TotalWins, incoming.TotalWins);
    existing.TotalLosses = Math.Max(existing.TotalLosses, incoming.TotalLosses);
    existing.Playtime = Math.Max(existing.Playtime, incoming.Playtime);
    existing.MaxAscension = Math.Max(existing.MaxAscension, incoming.MaxAscension);
    existing.BestWinStreak = Math.Max(existing.BestWinStreak, incoming.BestWinStreak);
    existing.CurrentWinStreak = Math.Max(existing.CurrentWinStreak, incoming.CurrentWinStreak);
    existing.PreferredAscension = Math.Max(existing.PreferredAscension, incoming.PreferredAscension);
    existing.FastestWinTime = ProgressState.MergeFastestWinTime(existing.FastestWinTime, incoming.FastestWinTime);
  }

  private static long MergeFastestWinTime(long a, long b)
  {
    if (a == -1L)
      return b;
    return b == -1L ? a : Math.Min(a, b);
  }

  private static void MergeCardStats(MegaCrit.Sts2.Core.Saves.CardStats existing, MegaCrit.Sts2.Core.Saves.CardStats incoming)
  {
    existing.TimesPicked = Math.Max(existing.TimesPicked, incoming.TimesPicked);
    existing.TimesSkipped = Math.Max(existing.TimesSkipped, incoming.TimesSkipped);
    existing.TimesWon = Math.Max(existing.TimesWon, incoming.TimesWon);
    existing.TimesLost = Math.Max(existing.TimesLost, incoming.TimesLost);
  }

  private static void MergeFightStatsList(List<FightStats>? existing, List<FightStats>? incoming)
  {
    if (existing == null || incoming == null)
      return;
    foreach (FightStats fightStats1 in incoming)
    {
      FightStats incomingFight = fightStats1;
      FightStats fightStats2 = existing.FirstOrDefault<FightStats>((Func<FightStats, bool>) (f => f.Character == incomingFight.Character));
      if (fightStats2 != null)
      {
        fightStats2.Wins = Math.Max(fightStats2.Wins, incomingFight.Wins);
        fightStats2.Losses = Math.Max(fightStats2.Losses, incomingFight.Losses);
      }
      else
        existing.Add(incomingFight);
    }
  }

  private static void MergeAncientCharacterStatsList(
    List<AncientCharacterStats>? existing,
    List<AncientCharacterStats>? incoming)
  {
    if (existing == null || incoming == null)
      return;
    foreach (AncientCharacterStats ancientCharacterStats1 in incoming)
    {
      AncientCharacterStats incomingStats = ancientCharacterStats1;
      AncientCharacterStats ancientCharacterStats2 = existing.FirstOrDefault<AncientCharacterStats>((Func<AncientCharacterStats, bool>) (c => c.Character == incomingStats.Character));
      if (ancientCharacterStats2 != null)
      {
        ancientCharacterStats2.Wins = Math.Max(ancientCharacterStats2.Wins, incomingStats.Wins);
        ancientCharacterStats2.Losses = Math.Max(ancientCharacterStats2.Losses, incomingStats.Losses);
      }
      else
        existing.Add(incomingStats);
    }
  }

  private static ModelId ValidateModelId<TModel>(
    ModelId id,
    string fieldName,
    DeserializationContext ctx)
    where TModel : AbstractModel
  {
    if (id == ModelId.none)
      return ModelId.none;
    if ((object) ModelDb.GetByIdOrNull<TModel>(id) != null)
      return id;
    ctx.PushPath(fieldName);
    ctx.Warn($"Unknown ID: {id}, resetting to none");
    ctx.PopPath();
    return ModelId.none;
  }

  private static long ClampNonNegative(long value, string fieldName, DeserializationContext ctx)
  {
    if (value >= 0L)
      return value;
    ctx.PushPath(fieldName);
    ctx.Warn($"Negative value ({value}), clamping to 0");
    ctx.PopPath();
    return 0;
  }

  private static int ClampNonNegativeInt(int value, string fieldName, DeserializationContext ctx)
  {
    if (value >= 0)
      return value;
    ctx.PushPath(fieldName);
    ctx.Warn($"Negative value ({value}), clamping to 0");
    ctx.PopPath();
    return 0;
  }

  private static int ClampAscension(int value, string fieldName, DeserializationContext ctx)
  {
    value = ProgressState.ClampNonNegativeInt(value, fieldName, ctx);
    if (value <= 10)
      return value;
    ctx.PushPath(fieldName);
    ctx.Warn($"Value ({value}) exceeds allowed ({10}), clamping");
    ctx.PopPath();
    return 10;
  }

  private void FilterAndSortEpochs()
  {
    int num = this._epochs.RemoveAll((Predicate<SerializableEpoch>) (e => !EpochModel.IsValid(e.Id)));
    if (num > 0)
      Log.Warn($"Removed {num} invalid epoch(s) from progress state");
    this._epochs.Sort((IComparer<SerializableEpoch>) new EpochComparer());
  }
}
