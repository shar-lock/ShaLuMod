// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.Managers.ProgressSaveManager
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves.Migrations;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.Saves.Validation;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.Timeline;
using MegaCrit.Sts2.Core.Timeline.Epochs;
using MegaCrit.Sts2.Core.Unlocks;
using Sentry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves.Managers;

public class ProgressSaveManager
{
  public const string fileName = "progress.save";
  private readonly ISaveStore _saveStore;
  private readonly MigrationManager _migrationManager;
  private readonly IProfileIdProvider _profileIdProvider;
  private static HashSet<ModelId>? _eliteEncounters;

  public ProgressState Progress { get; set; } = ProgressState.CreateDefault();

  public ProgressSaveManager(
    int profileId,
    ISaveStore saveStore,
    MigrationManager migrationManager)
    : this(saveStore, migrationManager, (IProfileIdProvider) new StaticProfileIdProvider(profileId))
  {
  }

  public ProgressSaveManager(
    ISaveStore saveStore,
    MigrationManager migrationManager,
    IProfileIdProvider profileIdProvider)
  {
    this._saveStore = saveStore;
    this._migrationManager = migrationManager;
    this._profileIdProvider = profileIdProvider;
  }

  public static string GetProgressPathForProfile(int profileId, bool? forceModState = null)
  {
    return StringExtensions.PathJoin(StringExtensions.PathJoin(UserDataPathProvider.GetProfileDir(profileId, forceModState), UserDataPathProvider.SavesDir), "progress.save");
  }

  public void SaveProgress()
  {
    try
    {
      SerializableProgress serializable = this.Progress.ToSerializable();
      serializable.SchemaVersion = this._migrationManager.GetLatestVersion<SerializableProgress>();
      string json = JsonSerializationUtility.ToJson<SerializableProgress>(serializable);
      this._saveStore.WriteFile(ProgressSaveManager.GetProgressPathForProfile(this._profileIdProvider.CurrentProfileId), json);
    }
    catch (Exception ex)
    {
      Log.Error($"Failed to save progress: {ex}");
      SentryService.CaptureException(ex);
    }
  }

  public ReadSaveResult<SerializableProgress> LoadProgress()
  {
    ReadSaveResult<SerializableProgress> readSaveResult = this._migrationManager.LoadSave<SerializableProgress>(ProgressSaveManager.GetProgressPathForProfile(this._profileIdProvider.CurrentProfileId));
    if (!readSaveResult.Success || readSaveResult.SaveData == null)
    {
      this.Progress = ProgressState.CreateDefault();
      Ironclad ironclad = ModelDb.Character<Ironclad>();
      foreach (AbstractModel abstractModel in ironclad.StartingDeck)
        this.Progress.MarkCardAsSeen(abstractModel.Id);
      foreach (AbstractModel startingRelic in (IEnumerable<RelicModel>) ironclad.StartingRelics)
        this.Progress.MarkRelicAsSeen(startingRelic.Id);
      this.SaveProgress();
    }
    else
    {
      DeserializationContext ctx = new DeserializationContext();
      this.Progress = ProgressState.FromSerializable(readSaveResult.SaveData, ctx);
      foreach (ValidationError error in (IEnumerable<ValidationError>) ctx.Errors)
        Log.Warn($"Progress parse: {error}");
    }
    return readSaveResult;
  }

  public UnlockState GenerateUnlockState() => new UnlockState(this.Progress);

  private void IncrementEncounterLoss(ModelId characterId, ModelId encounterId)
  {
    EncounterStats encounterStats = this.Progress.GetOrCreateEncounterStats(encounterId);
    if (encounterStats.FightStats.FirstOrDefault<FightStats>((Func<FightStats, bool>) (f => f.Character == characterId)) == null)
    {
      Log.Info($"{characterId} fought {encounterId} for the first time and LOST :(");
      FightStats fightStats = new FightStats()
      {
        Character = characterId,
        Wins = 0,
        Losses = 1
      };
      encounterStats.FightStats.Add(fightStats);
    }
    else
      encounterStats.IncrementLoss(characterId);
  }

  private void IncrementEnemyFightLoss(ModelId characterId, ModelId monster)
  {
    EnemyStats enemyStats = this.Progress.GetOrCreateEnemyStats(monster);
    if (enemyStats.FightStats.FirstOrDefault<FightStats>((Func<FightStats, bool>) (f => f.Character == characterId)) == null)
    {
      Log.Info($"{characterId} fought {monster} for the first time and LOST >:(");
      FightStats fightStats = new FightStats()
      {
        Character = characterId,
        Wins = 0,
        Losses = 1
      };
      enemyStats.FightStats.Add(fightStats);
    }
    else
      enemyStats.IncrementLoss(characterId);
  }

  public void MarkPotionAsSeen(PotionModel potion)
  {
    if (!this.Progress.MarkPotionAsSeen(potion.Id) || !LocalContext.IsMine(potion))
      return;
    potion.Owner.DiscoveredPotions.Add(potion.Id);
  }

  public void MarkCardAsSeen(CardModel card)
  {
    if (!this.Progress.MarkCardAsSeen(card.Id) || !LocalContext.IsMine(card))
      return;
    card.Owner.DiscoveredCards.Add(card.Id);
  }

  public void MarkRelicAsSeen(RelicModel relic)
  {
    if (!this.Progress.MarkRelicAsSeen(relic.Id) || !LocalContext.IsMine(relic))
      return;
    relic.Owner.DiscoveredRelics.Add(relic.Id);
  }

  public void UpdateWithRunData(SerializableRun serializableRun, bool victory)
  {
    bool flag1 = serializableRun.Players.Count == 1;
    bool flag2 = serializableRun.GameMode == GameMode.Daily;
    bool flag3 = serializableRun.GameMode == GameMode.Custom;
    SerializablePlayer serializablePlayer;
    if (flag1)
    {
      serializablePlayer = serializableRun.Players.First<SerializablePlayer>();
    }
    else
    {
      ulong playerId = PlatformUtil.GetLocalPlayerId(serializableRun.PlatformType);
      serializablePlayer = serializableRun.Players.FirstOrDefault<SerializablePlayer>((Func<SerializablePlayer, bool>) (p => (long) p.NetId == (long) playerId));
      if (serializablePlayer == null)
      {
        Log.Warn($"Local player with net id {playerId} not found in run! Progress will not be updated");
        return;
      }
    }
    for (int index = 0; index < serializableRun.MapPointHistory.Count; ++index)
    {
      if (index >= serializableRun.Acts.Count)
        Log.Warn($"There are {serializableRun.MapPointHistory.Count} acts in the map point history, but {serializableRun.Acts.Count} acts in the act array! This is unexpected");
      else
        this.Progress.MarkActAsSeen(serializableRun.Acts[index].Id);
    }
    List<MapPointHistoryEntry> list = serializableRun.MapPointHistory.SelectMany<List<MapPointHistoryEntry>, MapPointHistoryEntry>((Func<List<MapPointHistoryEntry>, IEnumerable<MapPointHistoryEntry>>) (act => (IEnumerable<MapPointHistoryEntry>) act)).ToList<MapPointHistoryEntry>();
    this.Progress.TotalPlaytime += serializableRun.WinTime > 0L ? serializableRun.WinTime : serializableRun.RunTime;
    this.Progress.WongoPoints += serializablePlayer.ExtraFields.WongoPoints;
    this.Progress.TestSubjectKills += serializableRun.ExtraFields.TestSubjectKills;
    this.Progress.FloorsClimbed += (long) list.Count;
    CharacterStats characterStats = this.Progress.GetOrCreateCharacterStats(serializablePlayer.CharacterId);
    characterStats.Playtime += serializableRun.WinTime > 0L ? serializableRun.WinTime : serializableRun.RunTime;
    if (victory)
    {
      this.Progress.ArchitectDamage += (long) ScoreUtility.CalculateScore(serializableRun, victory);
      if (!flag2 && !flag3)
      {
        if (flag1)
          ProgressSaveManager.IncrementSingleplayerAscension(serializableRun, characterStats);
        else
          this.IncrementMultiplayerAscension(serializableRun);
        ++characterStats.TotalWins;
        ++characterStats.CurrentWinStreak;
        characterStats.BestWinStreak = Math.Max(characterStats.BestWinStreak, characterStats.CurrentWinStreak);
        if (characterStats.FastestWinTime < 0L || characterStats.FastestWinTime > serializableRun.RunTime)
          characterStats.FastestWinTime = serializableRun.RunTime;
      }
    }
    else
    {
      if (!flag2 && !flag3)
      {
        characterStats.CurrentWinStreak = 0L;
        ++characterStats.TotalLosses;
      }
      List<MapPointHistoryEntry> source = serializableRun.MapPointHistory.LastOrDefault<List<MapPointHistoryEntry>>();
      MapPointHistoryEntry pointHistoryEntry = source != null ? source.LastOrDefault<MapPointHistoryEntry>() : (MapPointHistoryEntry) null;
      if (pointHistoryEntry != null && pointHistoryEntry.Rooms.Last<MapPointRoomHistoryEntry>().RoomType.IsCombatRoom())
      {
        ModelId modelId = pointHistoryEntry.Rooms.Last<MapPointRoomHistoryEntry>().ModelId;
        ModelId characterId = serializablePlayer.CharacterId;
        this.IncrementEncounterLoss(characterId, modelId);
        foreach (ModelId monsterId in pointHistoryEntry.Rooms.Last<MapPointRoomHistoryEntry>().MonsterIds)
          this.IncrementEnemyFightLoss(characterId, monsterId);
      }
    }
    foreach (MapPointHistoryEntry pointHistoryEntry in list)
    {
      MapPointRoomHistoryEntry roomHistoryEntry = pointHistoryEntry.FirstRoomOfType(RoomType.Event);
      if (roomHistoryEntry != null)
        this.Progress.MarkEventAsSeen(roomHistoryEntry.ModelId);
    }
    if (!flag2 && !flag3)
    {
      foreach (ModelId hash in serializablePlayer.Deck.Select<SerializableCard, ModelId>((Func<SerializableCard, ModelId>) (c => c.Id)).ToHashSet<ModelId>())
      {
        CardStats cardStats = this.Progress.GetOrCreateCardStats(hash);
        if (victory)
          ++cardStats.TimesWon;
        else
          ++cardStats.TimesLost;
      }
      foreach (MapPointHistoryEntry pointHistoryEntry in list)
      {
        foreach (CardChoiceHistoryEntry cardChoice in pointHistoryEntry.GetEntry(serializablePlayer.NetId).CardChoices)
        {
          CardStats cardStats = this.Progress.GetOrCreateCardStats(cardChoice.Card.Id);
          if (cardChoice.wasPicked)
            ++cardStats.TimesPicked;
          else
            ++cardStats.TimesSkipped;
        }
        if (pointHistoryEntry.MapPointType == MapPointType.Ancient)
        {
          AncientStats ancientStats = this.Progress.GetOrCreateAncientStats(pointHistoryEntry.FirstRoomOfType(RoomType.Event).ModelId);
          if (victory)
            ancientStats.IncrementWin(serializablePlayer.CharacterId);
          else
            ancientStats.IncrementLoss(serializablePlayer.CharacterId);
        }
      }
    }
    this.UpdateEpochsPostRun(serializablePlayer, serializableRun, victory);
    this.SaveProgress();
  }

  private void UpdateEpochsPostRun(
    SerializablePlayer serializablePlayer,
    SerializableRun serializableRun,
    bool victory)
  {
    this.TryObtainEpochPostRun(EpochModel.Get<NeowEpoch>(), serializablePlayer, serializableRun);
    this.PostRunUnlockCharacterEpochCheck(serializablePlayer, serializableRun);
    this.PostRunCharacterEpochChecks(serializablePlayer, serializableRun, victory);
    int num = ModelDb.AllCharacters.Count<CharacterModel>();
    if (victory && this.Progress.CharacterStats.Count >= num)
      this.TryObtainEpochPostRun(EpochModel.Get<DailyRunEpoch>(), serializablePlayer, serializableRun);
    if (this.Progress.CharacterStats.Count >= num)
      this.TryObtainEpochPostRun(EpochModel.Get<OrobasEpoch>(), serializablePlayer, serializableRun);
    if (!ModelDb.AllAncients.All<AncientEventModel>((Func<AncientEventModel, bool>) (a => this.Progress.AncientStats.ContainsKey(a.Id) || a is Darv)))
      return;
    this.TryObtainEpochPostRun(EpochModel.Get<DarvEpoch>(), serializablePlayer, serializableRun);
  }

  private void PostRunUnlockCharacterEpochCheck(
    SerializablePlayer serializablePlayer,
    SerializableRun serializableRun)
  {
    string str;
    switch (ModelDb.GetById<CharacterModel>(serializablePlayer.CharacterId))
    {
      case Silent _:
        str = EpochModel.GetId<Regent1Epoch>();
        break;
      case Regent _:
        str = EpochModel.GetId<Necrobinder1Epoch>();
        break;
      case Necrobinder _:
        str = EpochModel.GetId<Defect1Epoch>();
        break;
      default:
        str = (string) null;
        break;
    }
    string id = str;
    if (id == null || !this.TryObtainEpochPostRun(EpochModel.Get(id), serializablePlayer, serializableRun))
      return;
    Log.Info($"Epoch obtained for playing a run as {serializablePlayer.CharacterId}");
  }

  private void PostRunCharacterEpochChecks(
    SerializablePlayer serializablePlayer,
    SerializableRun serializableRun,
    bool victory)
  {
    if (!victory)
      return;
    this.CheckAscensionOneCompleted(serializablePlayer, serializableRun);
    string id = EpochModel.GetId<CustomAndSeedsEpoch>();
    if (this.Progress.Wins < 3)
      return;
    this.TryObtainEpochPostRun(EpochModel.Get(id), serializablePlayer, serializableRun);
  }

  private void CheckAscensionOneCompleted(
    SerializablePlayer serializablePlayer,
    SerializableRun serializableRun)
  {
    if (serializableRun.Ascension != 1)
      return;
    string str;
    switch (ModelDb.GetById<CharacterModel>(serializablePlayer.CharacterId))
    {
      case Ironclad _:
        str = EpochModel.GetId<Ironclad7Epoch>();
        break;
      case Silent _:
        str = EpochModel.GetId<Silent7Epoch>();
        break;
      case Regent _:
        str = EpochModel.GetId<Regent7Epoch>();
        break;
      case Defect _:
        str = EpochModel.GetId<Defect7Epoch>();
        break;
      case Necrobinder _:
        str = EpochModel.GetId<Necrobinder7Epoch>();
        break;
      default:
        str = (string) null;
        break;
    }
    string id = str;
    if (id == null)
      return;
    this.TryObtainEpochPostRun(EpochModel.Get(id), serializablePlayer, serializableRun);
  }

  private void CheckFifteenElitesDefeatedEpoch(Player localPlayer)
  {
    CharacterModel character = localPlayer.Character;
    EpochModel epochModel;
    switch (character)
    {
      case Ironclad _:
        epochModel = EpochModel.Get(EpochModel.GetId<Ironclad5Epoch>());
        break;
      case Silent _:
        epochModel = EpochModel.Get(EpochModel.GetId<Silent5Epoch>());
        break;
      case Regent _:
        epochModel = EpochModel.Get(EpochModel.GetId<Regent5Epoch>());
        break;
      case Defect _:
        epochModel = EpochModel.Get(EpochModel.GetId<Defect5Epoch>());
        break;
      case Necrobinder _:
        epochModel = EpochModel.Get(EpochModel.GetId<Necrobinder5Epoch>());
        break;
      case Deprived _:
        epochModel = (EpochModel) null;
        break;
      default:
        throw new ArgumentOutOfRangeException("character", (object) character, (string) null);
    }
    EpochModel epoch = epochModel;
    if (epoch == null)
      return;
    HashSet<ModelId> eliteEncounters = ProgressSaveManager.GetEliteEncounters();
    int num = 0;
    foreach (EncounterStats encounterStats in this.Progress.EncounterStats.Values)
    {
      if (eliteEncounters.Contains(encounterStats.Id))
      {
        foreach (FightStats fightStat in encounterStats.FightStats)
        {
          if (fightStat.Character == character.Id)
          {
            num += fightStat.Wins;
            break;
          }
        }
      }
    }
    Log.Info($"Elites Defeated: {num}/{eliteEncounters.Count}");
    if (num < 15)
      return;
    this.TryObtainEpochMidRun(epoch, localPlayer);
  }

  private void CheckFifteenBossesDefeatedEpoch(Player localPlayer)
  {
    CharacterModel character = localPlayer.Character;
    EpochModel epochModel;
    switch (character)
    {
      case Ironclad _:
        epochModel = EpochModel.Get(EpochModel.GetId<Ironclad6Epoch>());
        break;
      case Silent _:
        epochModel = EpochModel.Get(EpochModel.GetId<Silent6Epoch>());
        break;
      case Regent _:
        epochModel = EpochModel.Get(EpochModel.GetId<Regent6Epoch>());
        break;
      case Defect _:
        epochModel = EpochModel.Get(EpochModel.GetId<Defect6Epoch>());
        break;
      case Necrobinder _:
        epochModel = EpochModel.Get(EpochModel.GetId<Necrobinder6Epoch>());
        break;
      case Deprived _:
        epochModel = (EpochModel) null;
        break;
      default:
        throw new ArgumentOutOfRangeException("character", (object) character, (string) null);
    }
    EpochModel epoch = epochModel;
    if (epoch == null)
      return;
    HashSet<ModelId> hashSet = ModelDb.Acts.SelectMany<ActModel, ModelId>((Func<ActModel, IEnumerable<ModelId>>) (a => a.AllBossEncounters.Select<EncounterModel, ModelId>((Func<EncounterModel, ModelId>) (e => e.Id)))).ToHashSet<ModelId>();
    int num = 0;
    foreach (EncounterStats encounterStats in this.Progress.EncounterStats.Values)
    {
      if (hashSet.Contains(encounterStats.Id))
      {
        foreach (FightStats fightStat in encounterStats.FightStats)
        {
          if (fightStat.Character == character.Id)
          {
            num += fightStat.Wins;
            break;
          }
        }
      }
    }
    if (num < 15)
      return;
    this.TryObtainEpochMidRun(epoch, localPlayer);
  }

  private void ObtainCharUnlockEpoch(Player localPlayer, int act)
  {
    if (!localPlayer.Character.IsPlayable)
      return;
    string upperInvariant = localPlayer.Character.Id.Entry.ToUpperInvariant();
    EpochModel epoch = (EpochModel) null;
    switch (act)
    {
      case 0:
        epoch = EpochModel.Get(upperInvariant + "2_EPOCH");
        break;
      case 1:
        epoch = EpochModel.Get(upperInvariant + "3_EPOCH");
        break;
      case 2:
        epoch = EpochModel.Get(upperInvariant + "4_EPOCH");
        break;
      case 3:
        Log.Error($"Act {act + 1} is not yet implemented.");
        break;
      default:
        Log.Error($"Unsupported Act: {act}");
        break;
    }
    if (epoch == null)
    {
      Log.Error("EpochModel was not found :(");
    }
    else
    {
      if (!this.TryObtainEpochMidRun(epoch, localPlayer))
        return;
      Log.Info($"Epoch obtained for completing Act {act + 1}");
    }
  }

  private bool TryObtainEpochMidRun(EpochModel epoch, Player localPlayer)
  {
    if (localPlayer.RunState.GameMode.AreAchievementsAndEpochsLocked() || !this.TryObtainEpochInternal(epoch))
      return false;
    localPlayer.DiscoveredEpochs.Add(epoch.Id);
    return true;
  }

  private bool TryObtainEpochPostRun(
    EpochModel epoch,
    SerializablePlayer serializablePlayer,
    SerializableRun serializableRun)
  {
    if (serializableRun.GameMode.AreAchievementsAndEpochsLocked() || !this.TryObtainEpochInternal(epoch))
      return false;
    serializablePlayer.DiscoveredEpochs.Add(epoch.Id);
    return true;
  }

  private bool TryObtainEpochInternal(EpochModel epoch)
  {
    if (this.Progress.IsEpochObtained(epoch.Id))
    {
      Log.Info("Player already has Epoch: " + epoch.Id);
      return false;
    }
    this.Progress.ObtainEpoch(epoch.Id);
    NGame instance = NGame.Instance;
    if (instance != null)
      ((Node) instance).AddChildSafely((Node) NGainEpochVfx.Create(epoch));
    if (this.GetRevealableEpochs().All<SerializableEpoch>((Func<SerializableEpoch, bool>) (e => e.Id != epoch.Id)))
    {
      string str = $"Epoch {epoch.Id} was obtained, but is not yet revealable by the player!";
      Log.Warn(str);
      SentryService.CaptureMessage(str, (SentryLevel) 1, (Action<Scope>) (scope =>
      {
        string text = JsonSerializer.Serialize((object) this.Progress.Epochs, (JsonTypeInfo) JsonSerializationUtility.GetTypeInfo<List<SerializableEpoch>>());
        scope.AddCompressedAttachment(text, "epochs.txt.gz");
      }));
    }
    return true;
  }

  public IEnumerable<SerializableEpoch> GetRevealableEpochs()
  {
    HashSet<string> satisfiedEpochIds = new HashSet<string>(this.Progress.Epochs.Where<SerializableEpoch>((Func<SerializableEpoch, bool>) (e =>
    {
      bool revealableEpochs;
      switch (e.State)
      {
        case EpochState.ObtainedNoSlot:
        case EpochState.Obtained:
          revealableEpochs = true;
          break;
        default:
          revealableEpochs = false;
          break;
      }
      return revealableEpochs;
    })).Select<SerializableEpoch, string>((Func<SerializableEpoch, string>) (e => e.Id)));
    HashSet<string> reachableSet = new HashSet<string>();
    Queue<string> stringQueue = new Queue<string>();
    string id = EpochModel.Get<NeowEpoch>().Id;
    reachableSet.Add(id);
    stringQueue.Enqueue(id);
    foreach (SerializableEpoch epoch in (IEnumerable<SerializableEpoch>) this.Progress.Epochs)
    {
      bool flag1 = epoch.Id == id;
      if (!flag1)
      {
        bool flag2;
        switch (epoch.State)
        {
          case EpochState.None:
          case EpochState.NotObtained:
          case EpochState.ObtainedNoSlot:
            flag2 = true;
            break;
          default:
            flag2 = false;
            break;
        }
        flag1 = flag2;
      }
      if (!flag1)
      {
        reachableSet.Add(epoch.Id);
        stringQueue.Enqueue(epoch.Id);
      }
    }
    while (stringQueue.Count > 0)
    {
      foreach (EpochModel epochModel in EpochModel.Get(stringQueue.Dequeue()).GetTimelineExpansion())
      {
        if (!reachableSet.Contains(epochModel.Id))
        {
          reachableSet.Add(epochModel.Id);
          if (satisfiedEpochIds.Contains(epochModel.Id))
            stringQueue.Enqueue(epochModel.Id);
        }
      }
    }
    return this.Progress.Epochs.Where<SerializableEpoch>((Func<SerializableEpoch, bool>) (e => satisfiedEpochIds.Contains(e.Id) && reachableSet.Contains(e.Id)));
  }

  public void UpdateAfterCombatWon(Player localPlayer, CombatRoom room)
  {
    CombatState combatState = room.CombatState;
    IRunState runState = combatState.RunState;
    CharacterModel character = localPlayer.Character;
    ModelId id = combatState.Encounter.Id;
    EncounterStats encounterStats = this.Progress.GetOrCreateEncounterStats(id);
    if (encounterStats.FightStats.FirstOrDefault<FightStats>((Func<FightStats, bool>) (f => f.Character == character.Id)) == null)
    {
      Log.Info($"{character.Id} fought {id} for the first time and WON >:)");
      FightStats fightStats = new FightStats()
      {
        Character = character.Id,
        Wins = 1,
        Losses = 0
      };
      encounterStats.FightStats.Add(fightStats);
    }
    else
      encounterStats.IncrementWin(character.Id);
    if (room.RoomType == RoomType.Boss)
    {
      this.ObtainCharUnlockEpoch(localPlayer, runState.CurrentActIndex);
      this.CheckFifteenBossesDefeatedEpoch(localPlayer);
    }
    else if (room.RoomType == RoomType.Elite)
      this.CheckFifteenElitesDefeatedEpoch(localPlayer);
    foreach (MonsterModel spawnedEnemy in (IEnumerable<MonsterModel>) room.Encounter.SpawnedEnemies)
    {
      EnemyStats enemyStats = this.Progress.GetOrCreateEnemyStats(spawnedEnemy.Id);
      bool flag = enemyStats.FightStats.Count == 0;
      if (enemyStats.FightStats.FirstOrDefault<FightStats>((Func<FightStats, bool>) (f => f.Character == character.Id)) == null)
      {
        Log.Info($"{character.Id} fought {spawnedEnemy.Id} for the first time and WON >:(");
        FightStats fightStats = new FightStats()
        {
          Character = character.Id,
          Wins = 1,
          Losses = 0
        };
        enemyStats.FightStats.Add(fightStats);
        if (flag)
          localPlayer.DiscoveredEnemies.Add(spawnedEnemy.Id);
      }
      else
        enemyStats.IncrementWin(character.Id);
    }
  }

  private static void IncrementSingleplayerAscension(SerializableRun run, CharacterStats charStats)
  {
    if (run.Ascension == charStats.MaxAscension)
    {
      if (charStats.MaxAscension >= 10)
        return;
      ++charStats.MaxAscension;
      charStats.PreferredAscension = charStats.MaxAscension;
    }
    else
      Log.Info($"Not playing on max singleplayer ascension ({charStats.MaxAscension})");
  }

  private void IncrementMultiplayerAscension(SerializableRun run)
  {
    if (run.Ascension == this.Progress.MaxMultiplayerAscension)
    {
      if (this.Progress.MaxMultiplayerAscension >= 10)
        return;
      ++this.Progress.MaxMultiplayerAscension;
      this.Progress.PreferredMultiplayerAscension = this.Progress.MaxMultiplayerAscension;
    }
    else
      Log.Info($"Not playing on max multiplayer ascension ({this.Progress.MaxMultiplayerAscension})");
  }

  public bool SeenFtue(string ftueKey)
  {
    return !this.Progress.EnableFtues || this.Progress.FtueCompleted.Contains(ftueKey);
  }

  public bool SeenPopup(string popupKey)
  {
    return TestMode.IsOn || this.Progress.FtueCompleted.Contains(popupKey);
  }

  public void MarkFtueAsComplete(string ftueId)
  {
    if (!this.Progress.MarkFtueAsComplete(ftueId))
      return;
    Log.Info($"Player has seen ftue {ftueId}!");
    this.SaveProgress();
  }

  public void SetFtuesEnabled(bool enabled)
  {
    if (this.Progress.EnableFtues == enabled)
      return;
    Log.Info($"Player has set FTUEs enabled: {enabled}");
    this.Progress.EnableFtues = enabled;
    this.SaveProgress();
  }

  public void ResetFtues()
  {
    Log.Info("Player has reset FTUEs to enabled");
    this.Progress.ResetFtues();
    this.SaveProgress();
  }

  private static HashSet<ModelId> GetEliteEncounters()
  {
    HashSet<ModelId> eliteEncounters = ProgressSaveManager._eliteEncounters;
    if (eliteEncounters != null)
      return eliteEncounters;
    IEnumerable<EncounterModel> source = ModelDb.AllEncounters.Where<EncounterModel>((Func<EncounterModel, bool>) (e => e.RoomType == RoomType.Elite));
    return ProgressSaveManager._eliteEncounters = source.Select<EncounterModel, ModelId>((Func<EncounterModel, ModelId>) (e => e.Id)).Distinct<ModelId>().ToHashSet<ModelId>();
  }
}
