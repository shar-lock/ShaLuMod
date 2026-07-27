// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Runs.RunManager
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Achievements;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Daily;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Game.Lobby;
using MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby;
using MegaCrit.Sts2.Core.Multiplayer.Replay;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Runs.Metrics;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.MapDrawing;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.Timeline.Epochs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Runs;

public class RunManager : IRunLobbyListener
{
  private long _startTime;
  private long _prevSessionRunTime;
  private long _activeRunTime;
  private long _startOfCurrentActiveRunTime;
  private bool _isPaused;
  private bool _runHistoryWasUploaded;
  private int _numReloads;
  public Action? debugAfterCombatRewardsOverride;

  public static RunManager Instance { get; } = new RunManager();

  public AscensionManager AscensionManager { get; private set; }

  public bool ShouldSave { get; private set; }

  public DateTimeOffset? DailyTime { get; private set; }

  public bool IsInProgress => this.State != null;

  public bool IsCleaningUp { get; private set; }

  public bool ForceDiscoveryOrderModifications { get; set; }

  public bool IsGameOver => this.IsInProgress && this.State.IsGameOver;

  public bool IsAbandoned { get; private set; }

  public bool IsPaused
  {
    get => this._isPaused;
    set
    {
      this._isPaused = value;
      RunState state = this.State;
      if (state == null)
        return;
      IReadOnlyList<Player> players = state.Players;
      if (players == null || players.Count != 1)
        return;
      if (this._isPaused)
        this._activeRunTime += DateTimeOffset.UtcNow.ToUnixTimeSeconds() - this._startOfCurrentActiveRunTime;
      else
        this._startOfCurrentActiveRunTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }
  }

  public RunHistory? History { get; set; }

  public INetGameService NetService { get; private set; }

  public ChecksumTracker ChecksumTracker { get; private set; }

  public RunLocationTargetedMessageBuffer RunLocationTargetedBuffer { get; private set; }

  public CombatReplayWriter CombatReplayWriter { get; private set; }

  public RunLobby? RunLobby { get; private set; }

  public CombatStateSynchronizer CombatStateSynchronizer { get; private set; }

  public MapSelectionSynchronizer MapSelectionSynchronizer { get; private set; }

  public ActChangeSynchronizer ActChangeSynchronizer { get; private set; }

  public PlayerChoiceSynchronizer PlayerChoiceSynchronizer { get; private set; }

  public EventSynchronizer EventSynchronizer { get; private set; }

  public RewardSynchronizer RewardSynchronizer { get; private set; }

  public RewardsSetSynchronizer RewardsSetSynchronizer { get; private set; }

  public RestSiteSynchronizer RestSiteSynchronizer { get; private set; }

  public OneOffSynchronizer OneOffSynchronizer { get; private set; }

  public TreasureRoomRelicSynchronizer TreasureRoomRelicSynchronizer { get; private set; }

  public FlavorSynchronizer FlavorSynchronizer { get; private set; }

  public PeerInputSynchronizer InputSynchronizer { get; private set; }

  public HoveredModelTracker HoveredModelTracker { get; private set; }

  public ActionQueueSet ActionQueueSet { get; private set; }

  public ActionExecutor ActionExecutor { get; private set; }

  public ActionQueueSynchronizer ActionQueueSynchronizer { get; private set; }

  public long WinTime { get; set; }

  public long RunTime
  {
    get
    {
      if (this.WinTime > 0L)
        return this.WinTime;
      if (this.IsPaused)
      {
        RunState state = this.State;
        if (state != null)
        {
          IReadOnlyList<Player> players = state.Players;
          if (players != null && players.Count == 1)
            return this._activeRunTime + this._prevSessionRunTime;
        }
      }
      return DateTimeOffset.UtcNow.ToUnixTimeSeconds() - this._startOfCurrentActiveRunTime + this._activeRunTime + this._prevSessionRunTime;
    }
  }

  public bool IsSingleplayerOrFakeMultiplayer
  {
    get => this.IsInProgress && this.NetService.Type == NetGameType.Singleplayer;
  }

  public SerializableMapDrawings? MapDrawingsToLoad { get; set; }

  public Dictionary<int, SerializableActMap>? SavedMapsToLoad { get; set; }

  public event Action<RunState>? RunStarted;

  public event Action? RoomEntered;

  public event Action? RoomExited;

  public event Action? ActEntered;

  public event Func<Task>? TestFadeOut;

  public event Func<Task>? TestFadeIn;

  private RunState? State { get; set; }

  private RunManager()
  {
  }

  public void SetUpNewSingleplayer(RunState state, bool shouldSave, DateTimeOffset? dailyTime = null)
  {
    this.State = this.State == null ? state : throw new InvalidOperationException("State is already set.");
    INetGameService netService = (INetGameService) new NetSingleplayerGameService();
    this.InitializeShared(netService, new PeerInputSynchronizer(netService), shouldSave, dailyTime, DateTimeOffset.UtcNow.ToUnixTimeSeconds(), 0L, 0L, 0);
    this.InitializeRunLobby(netService, state);
    this.InitializeNewRun();
    this.GenerateRooms();
  }

  public void SetUpNewMultiplayer(
    RunState state,
    StartRunLobby lobby,
    bool shouldSave,
    DateTimeOffset? dailyTime = null)
  {
    this.State = this.State == null ? state : throw new InvalidOperationException("State is already set.");
    this.InitializeShared(lobby.NetService, lobby.InputSynchronizer, shouldSave, dailyTime, DateTimeOffset.UtcNow.ToUnixTimeSeconds(), 0L, 0L, 0);
    this.InitializeRunLobby(lobby.NetService, state);
    this.InitializeNewRun();
    this.GenerateRooms();
  }

  public async Task SetUpSavedSingleplayer(RunState state, SerializableRun save)
  {
    this.State = this.State == null ? state : throw new InvalidOperationException("State is already set.");
    await SaveManager.Instance.IncrementNumReloads(save, NetGameType.Singleplayer);
    INetGameService netService = (INetGameService) new NetSingleplayerGameService();
    this.InitializeShared(netService, new PeerInputSynchronizer(netService), true, save.DailyTime, save.StartTime, save.RunTime, save.WinTime, save.NumReloads);
    this.InitializeRunLobby(netService, state);
    this.InitializeSavedRun(save);
  }

  public async Task SetUpSavedMultiplayer(RunState state, LoadRunLobby lobby)
  {
    this.State = this.State == null ? state : throw new InvalidOperationException("State is already set.");
    SerializableRun save = lobby.Run;
    await SaveManager.Instance.IncrementNumReloads(save, lobby.NetService.Type);
    this.InitializeShared(lobby.NetService, lobby.InputSynchronizer, true, save.DailyTime, save.StartTime, save.RunTime, save.WinTime, save.NumReloads);
    this.InitializeRunLobby(lobby.NetService, state);
    this.InitializeSavedRun(save);
    save = (SerializableRun) null;
  }

  public void SetUpReplay(RunState state, CombatReplay replay, ulong playerIdToLoad)
  {
    this.State = this.State == null ? state : throw new InvalidOperationException("State is already set.");
    SerializableRun serializableRun = replay.serializableRun;
    NetReplayGameService netService = new NetReplayGameService(playerIdToLoad);
    this.InitializeShared((INetGameService) netService, new PeerInputSynchronizer((INetGameService) netService), true, serializableRun.DailyTime, serializableRun.StartTime, serializableRun.RunTime, serializableRun.WinTime, serializableRun.NumReloads);
    this.InitializeRunLobby((INetGameService) netService, state);
    this.InitializeSavedRun(serializableRun);
  }

  public void SetUpTest(
    RunState state,
    INetGameService gameService,
    bool disableCombatStateSync = true,
    bool shouldSave = false)
  {
    this.State = this.State == null ? state : throw new InvalidOperationException("State is already set.");
    this.InitializeShared(gameService, new PeerInputSynchronizer(gameService), shouldSave, new DateTimeOffset?(), DateTimeOffset.UtcNow.ToUnixTimeSeconds(), 0L, 0L, 0);
    this.InitializeRunLobby(gameService, state);
    this.CombatStateSynchronizer.IsDisabled = disableCombatStateSync;
    this.InitializeNewRun();
  }

  private void InitializeShared(
    INetGameService netService,
    PeerInputSynchronizer inputSynchronizer,
    bool shouldSave,
    DateTimeOffset? dailyTime,
    long startTime,
    long runTime,
    long winTime,
    int numReloads)
  {
    if (this.State == null)
      throw new InvalidOperationException("State is not set.");
    this.NetService = netService;
    ulong netId = this.NetService.NetId;
    this.ChecksumTracker = new ChecksumTracker(this.NetService, (IRunState) this.State);
    this.ChecksumTracker.IsEnabled = !TestMode.IsOn;
    this.RunLocationTargetedBuffer = new RunLocationTargetedMessageBuffer(this.NetService);
    this.FlavorSynchronizer = new FlavorSynchronizer(this.NetService, (IPlayerCollection) this.State, netId);
    this.ActionQueueSet = new ActionQueueSet(this.State.Players);
    this.ActionExecutor = new ActionExecutor(this.ActionQueueSet);
    this.ActionQueueSynchronizer = new ActionQueueSynchronizer((IPlayerCollection) this.State, this.ActionQueueSet, this.RunLocationTargetedBuffer, this.NetService);
    this.PlayerChoiceSynchronizer = new PlayerChoiceSynchronizer(this.NetService, (IPlayerCollection) this.State);
    this.MapSelectionSynchronizer = new MapSelectionSynchronizer(this.NetService, this.ActionQueueSynchronizer, this.State);
    this.ActChangeSynchronizer = new ActChangeSynchronizer(this.State);
    this.EventSynchronizer = new EventSynchronizer(this.RunLocationTargetedBuffer, this.NetService, (IPlayerCollection) this.State, (IRunState) this.State, netId, this.State.Rng.Seed);
    this.RewardSynchronizer = new RewardSynchronizer(this.RunLocationTargetedBuffer, this.NetService, (IPlayerCollection) this.State, netId);
    this.RewardsSetSynchronizer = new RewardsSetSynchronizer(this.RunLocationTargetedBuffer, this.NetService, (IPlayerCollection) this.State, netId);
    this.OneOffSynchronizer = new OneOffSynchronizer(this.RunLocationTargetedBuffer, this.NetService, (IPlayerCollection) this.State, netId);
    this.TreasureRoomRelicSynchronizer = new TreasureRoomRelicSynchronizer((IPlayerCollection) this.State, netId, this.ActionQueueSynchronizer, this.State.SharedRelicGrabBag, this.State.Rng.TreasureRoomRelics);
    this.CombatReplayWriter = new CombatReplayWriter(this.PlayerChoiceSynchronizer, this.RewardsSetSynchronizer, this.ActionQueueSet, this.ActionQueueSynchronizer, this.ChecksumTracker);
    this.CombatReplayWriter.IsEnabled = !TestMode.IsOn;
    this.ActionExecutor.JustBeforeActionFinishedExecuting += new Action<GameAction>(this.SendPostActionChecksum);
    this.ChecksumTracker.StateDiverged += new Action<NetFullCombatState>(this.StateDiverged);
    this.ActionExecutor.Pause();
    this.IsAbandoned = false;
    this.AscensionManager = new AscensionManager(this.State.AscensionLevel);
    this.ShouldSave = shouldSave;
    this.DailyTime = dailyTime;
    this._startTime = startTime;
    this._prevSessionRunTime = runTime;
    this._activeRunTime = 0L;
    this.WinTime = winTime;
    this._numReloads = numReloads;
    this._startOfCurrentActiveRunTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    this.InputSynchronizer = inputSynchronizer;
    this.HoveredModelTracker = new HoveredModelTracker(this.InputSynchronizer, (IPlayerCollection) this.State);
  }

  private void InitializeRunLobby(INetGameService netService, RunState state)
  {
    if (netService.Type.IsMultiplayer())
    {
      this.RunLobby = new RunLobby(state.GameMode, netService, (IRunLobbyListener) this, (IPlayerCollection) state, state.Players.Select<Player, ulong>((Func<Player, ulong>) (p => p.NetId)));
      this.RunLobby.RemotePlayerDisconnected += new Action<ulong>(this.RemotePlayerDisconnected);
    }
    this.CombatStateSynchronizer = new CombatStateSynchronizer(this.NetService, this.RunLobby, state);
    this.RestSiteSynchronizer = new RestSiteSynchronizer(this.RunLocationTargetedBuffer, this.NetService, (IPlayerCollection) state, this.NetService.NetId, this.RunLobby);
  }

  private void InitializeNewRun()
  {
    this.State.SharedRelicGrabBag.Populate(ModelDb.RelicPool<SharedRelicPool>().GetUnlockedRelics(this.State.UnlockState), this.State.Rng.UpFront);
    foreach (Player player in (IEnumerable<Player>) this.State.Players)
      player.PopulateRelicGrabBagIfNecessary(this.State.Rng.UpFront);
    this.SetStartedWithNeowFlag();
    foreach (ModifierModel modifier in (IEnumerable<ModifierModel>) this.State.Modifiers)
      modifier.OnRunCreated(this.State);
    foreach (Player player in (IEnumerable<Player>) this.State.Players)
      this.ApplyAscensionEffects(player);
  }

  private void InitializeSavedRun(SerializableRun save)
  {
    foreach (ActModel act in (IEnumerable<ActModel>) this.State.Acts)
      act.ValidateRoomsAfterLoad(this.State.Rng.UpFront);
    this.AfterMapLocationChanged();
    this.MapDrawingsToLoad = save.MapDrawings;
    this.SavedMapsToLoad = (Dictionary<int, SerializableActMap>) null;
    for (int index = 0; index < save.Acts.Count; ++index)
    {
      SerializableActMap savedMap = save.Acts[index].SavedMap;
      if (savedMap != null)
      {
        if (this.SavedMapsToLoad == null)
        {
          Dictionary<int, SerializableActMap> dictionary;
          this.SavedMapsToLoad = dictionary = new Dictionary<int, SerializableActMap>();
        }
        this.SavedMapsToLoad[index] = savedMap;
      }
    }
    foreach (ModifierModel modifier in (IEnumerable<ModifierModel>) this.State.Modifiers)
      modifier.OnRunLoaded(this.State);
  }

  private void SendPostActionChecksum(GameAction action)
  {
    if (!CombatManager.Instance.IsInProgress)
      return;
    bool flag;
    switch (action)
    {
      case EndPlayerTurnAction _:
      case ReadyToBeginEnemyTurnAction _:
        flag = true;
        break;
      default:
        flag = false;
        break;
    }
    if (flag)
      return;
    this.ChecksumTracker.GenerateChecksum($"finished action execution {action}", action);
  }

  private void SetStartedWithNeowFlag()
  {
    this.State.ExtraFields.StartedWithNeow = this.State.UnlockState.IsEpochRevealed<NeowEpoch>();
  }

  public static SerializableRun CanonicalizeSave(SerializableRun save, ulong localPlayerId)
  {
    if (save.Players.FirstOrDefault<SerializablePlayer>((Func<SerializablePlayer, bool>) (p => (long) p.NetId == (long) localPlayerId)) == null)
      throw new InvalidOperationException($"Save is invalid! Players does not contain local player Id. IDs in save file: {string.Join<ulong>(",", save.Players.Select<SerializablePlayer, ulong>((Func<SerializablePlayer, ulong>) (p => p.NetId)))}. Local ID: {localPlayerId}.");
    RunState runState = RunState.FromSerializable(save);
    int latestSchemaVersion = SaveManager.Instance.GetLatestSchemaVersion<SerializableRun>();
    SerializableRun val = new SerializableRun()
    {
      SchemaVersion = latestSchemaVersion,
      Acts = runState.Acts.Zip<ActModel, SerializableActModel, SerializableActModel>((IEnumerable<SerializableActModel>) save.Acts, (Func<ActModel, SerializableActModel, SerializableActModel>) ((act, savedAct) =>
      {
        SerializableActModel save1 = act.ToSave();
        save1.SavedMap = savedAct.SavedMap;
        return save1;
      })).ToList<SerializableActModel>(),
      Modifiers = runState.Modifiers.Select<ModifierModel, SerializableModifier>((Func<ModifierModel, SerializableModifier>) (m => m.ToSerializable())).ToList<SerializableModifier>(),
      DailyTime = save.DailyTime,
      GameMode = runState.GameMode,
      CurrentActIndex = runState.CurrentActIndex,
      EventsSeen = ((IEnumerable<ModelId>) runState.VisitedEventIds).ToList<ModelId>(),
      SerializableOdds = runState.Odds.ToSerializable(),
      SerializableSharedRelicGrabBag = runState.SharedRelicGrabBag.ToSerializable(),
      Players = runState.Players.Select<Player, SerializablePlayer>((Func<Player, SerializablePlayer>) (p => p.ToSerializable())).ToList<SerializablePlayer>(),
      SerializableRng = runState.Rng.ToSerializable(),
      VisitedMapCoords = runState.VisitedMapCoords.ToList<MapCoord>(),
      MapPointHistory = runState.MapPointHistory.Select<IReadOnlyList<MapPointHistoryEntry>, List<MapPointHistoryEntry>>((Func<IReadOnlyList<MapPointHistoryEntry>, List<MapPointHistoryEntry>>) (l => l.ToList<MapPointHistoryEntry>())).ToList<List<MapPointHistoryEntry>>(),
      SaveTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
      StartTime = save.StartTime,
      RunTime = save.RunTime,
      WinTime = save.WinTime,
      Ascension = runState.AscensionLevel,
      PlatformType = save.PlatformType,
      MapDrawings = save.MapDrawings,
      NumReloads = save.NumReloads,
      ExtraFields = runState.ExtraFields.ToSerializable(),
      PreFinishedRoom = save.PreFinishedRoom
    };
    new PacketWriter().Write<SerializableRun>(val);
    return val;
  }

  public static HashSet<RoomType> BuildRoomTypeBlacklist(
    MapPointHistoryEntry? previousMapPointEntry,
    IReadOnlyCollection<MapPoint> nextMapPoints)
  {
    HashSet<RoomType> roomTypeSet = new HashSet<RoomType>();
    if (previousMapPointEntry != null && previousMapPointEntry.HasRoomOfType(RoomType.Shop) || nextMapPoints.Count > 0 && nextMapPoints.All<MapPoint>((Func<MapPoint, bool>) (p => p.PointType == MapPointType.Shop)))
      roomTypeSet.Add(RoomType.Shop);
    return roomTypeSet;
  }

  public SerializableRun ToSave(AbstractRoom? preFinishedRoom)
  {
    int latestSchemaVersion = SaveManager.Instance.GetLatestSchemaVersion<SerializableRun>();
    List<SerializableActModel> serializableActModelList = new List<SerializableActModel>();
    for (int index = 0; index < this.State.Acts.Count; ++index)
    {
      SerializableActModel save = this.State.Acts[index].ToSave();
      if (index == this.State.CurrentActIndex && this.State.Map != null)
        save.SavedMap = SerializableActMap.FromActMap(this.State.Map);
      serializableActModelList.Add(save);
    }
    return new SerializableRun()
    {
      SchemaVersion = latestSchemaVersion,
      Acts = serializableActModelList,
      Modifiers = this.State.Modifiers.Select<ModifierModel, SerializableModifier>((Func<ModifierModel, SerializableModifier>) (m => m.ToSerializable())).ToList<SerializableModifier>(),
      DailyTime = this.DailyTime,
      CurrentActIndex = this.State.CurrentActIndex,
      EventsSeen = ((IEnumerable<ModelId>) this.State.VisitedEventIds).ToList<ModelId>(),
      GameMode = this.State.GameMode,
      SerializableOdds = this.State.Odds.ToSerializable(),
      SerializableSharedRelicGrabBag = this.State.SharedRelicGrabBag.ToSerializable(),
      Players = this.State.Players.Select<Player, SerializablePlayer>((Func<Player, SerializablePlayer>) (p => p.ToSerializable())).ToList<SerializablePlayer>(),
      SerializableRng = this.State.Rng.ToSerializable(),
      VisitedMapCoords = this.State.VisitedMapCoords.ToList<MapCoord>(),
      MapPointHistory = this.State.MapPointHistory.Select<IReadOnlyList<MapPointHistoryEntry>, List<MapPointHistoryEntry>>((Func<IReadOnlyList<MapPointHistoryEntry>, List<MapPointHistoryEntry>>) (l => l.ToList<MapPointHistoryEntry>())).ToList<List<MapPointHistoryEntry>>(),
      SaveTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
      StartTime = this._startTime,
      RunTime = this.RunTime,
      WinTime = this.WinTime,
      NumReloads = this._numReloads,
      Ascension = this.State.AscensionLevel,
      PlatformType = this.NetService.Platform,
      MapDrawings = NRun.Instance?.GlobalUi.MapScreen.Drawings.GetSerializableMapDrawings(),
      ExtraFields = this.State.ExtraFields.ToSerializable(),
      PreFinishedRoom = preFinishedRoom?.ToSerializable()
    };
  }

  public RunState Launch()
  {
    LocalContext.NetId = new ulong?(this.NetService.NetId);
    this.NetService.SetBufferMessages(false);
    Action<RunState> runStarted = this.RunStarted;
    if (runStarted != null)
      runStarted(this.State);
    this.UpdateRichPresence();
    return this.State;
  }

  public async Task FinalizeStartingRelics()
  {
    if (this.State == null)
      return;
    foreach (Player player in (IEnumerable<Player>) this.State.Players)
    {
      foreach (RelicModel relic in (IEnumerable<RelicModel>) player.Relics)
        await relic.AfterObtained();
    }
  }

  public void GenerateRooms()
  {
    List<AncientEventModel> ancientEventModelList = this.State.UnlockState.SharedAncients.ToList<AncientEventModel>().UnstableShuffle<AncientEventModel>(this.State.Rng.UpFront);
    foreach (ActModel actModel in this.State.Acts.Skip<ActModel>(1))
    {
      int count = this.State.Rng.UpFront.NextInt(ancientEventModelList.Count + 1);
      List<AncientEventModel> list = ancientEventModelList.Take<AncientEventModel>(count).ToList<AncientEventModel>();
      ancientEventModelList = ancientEventModelList.Except<AncientEventModel>((IEnumerable<AncientEventModel>) list).ToList<AncientEventModel>();
      actModel.SetSharedAncientSubset(list);
    }
    for (int index = 0; index < this.State.Acts.Count; ++index)
    {
      ActModel act = this.State.Acts[index];
      act.GenerateRooms(this.State.Rng.UpFront, this.State.UnlockState, this.State.Players.Count > 1);
      if (this.ShouldApplyTutorialModifications())
        act.ApplyDiscoveryOrderModifications(this.State.UnlockState);
      if (index == this.State.Acts.Count - 1 && this.AscensionManager.HasLevel(AscensionLevel.DoubleBoss))
      {
        EncounterModel encounter = this.State.Rng.UpFront.NextItem<EncounterModel>(act.AllBossEncounters.Where<EncounterModel>((Func<EncounterModel, bool>) (e => e.Id != act.BossEncounter.Id)));
        act.SetSecondBossEncounter(encounter);
      }
    }
  }

  public bool ShouldApplyTutorialModifications()
  {
    return this.ForceDiscoveryOrderModifications || !TestMode.IsOn && this.State != null && this.State.GameMode == GameMode.Standard;
  }

  public async Task GenerateMap()
  {
    if (this.State == null)
      throw new InvalidOperationException("State is not set.");
    this.MapSelectionSynchronizer.BeforeMapGenerated();
    ActMap map;
    SerializableActMap saved;
    if (this.SavedMapsToLoad != null && this.SavedMapsToLoad.TryGetValue(this.State.CurrentActIndex, out saved))
    {
      map = (ActMap) new SavedActMap(saved);
      this.SavedMapsToLoad.Remove(this.State.CurrentActIndex);
      if (this.SavedMapsToLoad.Count == 0)
        this.SavedMapsToLoad = (Dictionary<int, SerializableActMap>) null;
      map = Hook.ModifyGeneratedMapLate((IRunState) this.State, map, this.State.CurrentActIndex);
      await Hook.AfterMapGenerated((IRunState) this.State, map, this.State.CurrentActIndex);
    }
    else
    {
      map = Hook.ModifyGeneratedMap((IRunState) this.State, this.State.Act.CreateMap(this.State, false), this.State.CurrentActIndex);
      await Hook.AfterMapGenerated((IRunState) this.State, map, this.State.CurrentActIndex);
      if (!this.State.ExtraFields.StartedWithNeow && this.State.CurrentActIndex == 0)
        map.StartingMapPoint.PointType = MapPointType.Monster;
    }
    this.State.Map = map;
    this.State.RemoveStaleVisitedMapCoords(map);
    NMapScreen instance = NMapScreen.Instance;
    if (instance == null)
    {
      map = (ActMap) null;
    }
    else
    {
      instance.SetMap(map, this.State.Rng.Seed, true);
      map = (ActMap) null;
    }
  }

  public Task EnterMapCoord(MapCoord coord)
  {
    return this.State == null || !this.State.AddVisitedMapCoord(coord) ? Task.CompletedTask : this.EnterMapCoordInternal(coord, (AbstractRoom) null, true);
  }

  public async Task LoadIntoLatestMapCoord(AbstractRoom? preFinishedRoom)
  {
    if (this.State == null)
      return;
    if (this.State.VisitedMapCoords.Count > 0)
    {
      IReadOnlyList<MapCoord> visitedMapCoords = this.State.VisitedMapCoords;
      await this.EnterMapCoordInternal(visitedMapCoords[visitedMapCoords.Count - 1], preFinishedRoom, false);
    }
    else
      await this.EnterRoomInternal((AbstractRoom) new MapRoom());
  }

  private Task EnterMapCoordInternal(MapCoord coord, AbstractRoom? preFinishedRoom, bool saveGame)
  {
    if (this.State == null)
      return Task.CompletedTask;
    MapPoint point = this.State.Map.GetPoint(coord);
    return this.EnterMapPointInternal(coord.row + 1, point.PointType, preFinishedRoom, saveGame);
  }

  public async Task EnterMapPointInternal(
    int actFloor,
    MapPointType pointType,
    AbstractRoom? preFinishedRoom,
    bool saveGame)
  {
    NetLoadingHandle loadHandle;
    CombatRoom combatRoom;
    if (this.State == null)
    {
      loadHandle = (NetLoadingHandle) null;
      combatRoom = (CombatRoom) null;
    }
    else
    {
      loadHandle = new NetLoadingHandle(this.NetService);
      try
      {
        if (this.State.MapPointHistory.Count > 0)
          this.UpdatePlayerStatsInMapPointHistory();
        this.State.ActFloor = actFloor;
        await this.ExitCurrentRooms();
        if (preFinishedRoom == null)
          this.CombatStateSynchronizer.StartSync();
        this.ClearScreens();
        if (preFinishedRoom == null)
          await this.CombatStateSynchronizer.WaitForSync();
        if (saveGame)
          await SaveManager.Instance.SaveRun((AbstractRoom) null);
        if (this.CombatReplayWriter.IsEnabled)
          this.CombatReplayWriter.RecordInitialState(this.ToSave((AbstractRoom) null));
        RoomType roomType;
        if (pointType == MapPointType.Unknown && preFinishedRoom != null)
        {
          roomType = RoomType.Monster;
        }
        else
        {
          HashSet<RoomType> blacklist = RunManager.BuildRoomTypeBlacklist(this.State.CurrentMapPointHistoryEntry, (IReadOnlyCollection<MapPoint>) (this.State.CurrentMapPoint?.Children ?? new HashSet<MapPoint>()));
          roomType = this.RollRoomTypeFor(pointType, (IEnumerable<RoomType>) blacklist);
        }
        AbstractRoom room = preFinishedRoom == null ? this.CreateRoom(roomType, pointType) : preFinishedRoom;
        this.ActionExecutor.Pause();
        if (preFinishedRoom == null)
          this.State.AppendToMapPointHistory(pointType, room.RoomType, room.ModelId);
        combatRoom = room as CombatRoom;
        if (combatRoom != null && combatRoom.IsPreFinished && (object) combatRoom.ParentEventId != null)
        {
          await this.EnterRoomInternal((AbstractRoom) new EventRoom(ModelDb.GetById<EventModel>(combatRoom.ParentEventId)), true);
          await this.EnterRoomInternal((AbstractRoom) combatRoom);
        }
        else
          await this.EnterRoom(room);
        if (NRun.Instance != null)
          NRun.Instance.GlobalUi.MapScreen.IsTraveling = false;
        this.AfterMapLocationChanged();
        loadHandle = (NetLoadingHandle) null;
        combatRoom = (CombatRoom) null;
      }
      finally
      {
        loadHandle?.Dispose();
      }
    }
  }

  private AbstractRoom CreateRoom(
    RoomType roomType,
    MapPointType mapPointType = MapPointType.Unassigned,
    AbstractModel? model = null)
  {
    if (this.State == null)
      throw new InvalidOperationException("RunState is not set.");
    switch (roomType)
    {
      case RoomType.Monster:
      case RoomType.Elite:
      case RoomType.Boss:
        if (!(model is EncounterModel encounter))
          encounter = this.State.Act.PullNextEncounter(roomType).ToMutable();
        return (AbstractRoom) new CombatRoom(encounter, (IRunState) this.State);
      case RoomType.Treasure:
        return (AbstractRoom) new TreasureRoom(this.State.CurrentActIndex);
      case RoomType.Shop:
        return (AbstractRoom) new MerchantRoom();
      case RoomType.Event:
        if (!(model is EventModel eventModel))
          eventModel = mapPointType == MapPointType.Ancient ? this.State.Act.PullAncient() : this.State.Act.PullNextEvent(this.State);
        return (AbstractRoom) new EventRoom(eventModel);
      case RoomType.RestSite:
        return (AbstractRoom) new RestSiteRoom();
      case RoomType.Map:
        return (AbstractRoom) new MapRoom();
      default:
        throw new InvalidOperationException($"Unexpected RoomType: {roomType}");
    }
  }

  private RoomType RollRoomTypeFor(MapPointType pointType, IEnumerable<RoomType> blacklist)
  {
    RoomType roomType;
    if (this.TryGetRoomTypeForTutorial(pointType, out roomType))
      return roomType;
    switch (pointType)
    {
      case MapPointType.Unassigned:
        return RoomType.Unassigned;
      case MapPointType.Unknown:
        return this.State.Odds.UnknownMapPoint.Roll(blacklist, (IRunState) this.State);
      case MapPointType.Shop:
        return RoomType.Shop;
      case MapPointType.Treasure:
        return RoomType.Treasure;
      case MapPointType.RestSite:
        return RoomType.RestSite;
      case MapPointType.Monster:
        return RoomType.Monster;
      case MapPointType.Elite:
        return RoomType.Elite;
      case MapPointType.Boss:
        return RoomType.Boss;
      case MapPointType.Ancient:
        return RoomType.Event;
      default:
        throw new ArgumentOutOfRangeException(nameof (pointType), (object) pointType, (string) null);
    }
  }

  private bool TryGetRoomTypeForTutorial(MapPointType pointType, out RoomType roomType)
  {
    roomType = RoomType.Unassigned;
    if (!TestMode.IsOn)
    {
      RunState state1 = this.State;
      // ISSUE: explicit non-virtual call
      if ((state1 != null ? (__nonvirtual (state1.Players).Count > 1 ? 1 : 0) : 0) == 0 && pointType == MapPointType.Unassigned && SaveManager.Instance.Progress.NumberOfRuns <= 0)
      {
        RunState state2 = this.State;
        // ISSUE: explicit non-virtual call
        if ((state2 != null ? (__nonvirtual (state2.MapPointHistory).SelectMany<IReadOnlyList<MapPointHistoryEntry>, MapPointHistoryEntry>((Func<IReadOnlyList<MapPointHistoryEntry>, IEnumerable<MapPointHistoryEntry>>) (l => (IEnumerable<MapPointHistoryEntry>) l)).Any<MapPointHistoryEntry>((Func<MapPointHistoryEntry, bool>) (e => e.MapPointType == MapPointType.Unassigned)) ? 1 : 0) : 0) != 0)
          return false;
        roomType = RoomType.Event;
        return true;
      }
    }
    return false;
  }

  public async Task FadeIn(bool showTransition = true)
  {
    if (TestMode.IsOn)
    {
      Func<Task> testFadeIn = this.TestFadeIn;
      Task task = testFadeIn != null ? testFadeIn() : (Task) null;
      if (task == null)
        return;
      await task;
    }
    else
      await NGame.Instance.Transition.RoomFadeIn(showTransition);
  }

  public async Task FadeOut()
  {
    if (TestMode.IsOn)
    {
      Func<Task> testFadeOut = this.TestFadeOut;
      Task task = testFadeOut != null ? testFadeOut() : (Task) null;
      if (task == null)
        return;
      await task;
    }
    else
      await NGame.Instance.Transition.RoomFadeOut();
  }

  private void ClearScreens()
  {
    if (TestMode.IsOn)
      return;
    NOverlayStack.Instance.Clear();
    NCapstoneContainer.Instance.Close();
    NMapScreen.Instance.Close(false);
  }

  public async Task EnterMapCoordDebug(
    MapCoord coord,
    RoomType roomType,
    MapPointType pointType = MapPointType.Unassigned,
    AbstractModel? model = null,
    bool showTransition = true)
  {
    this.State.AddVisitedMapCoord(coord);
    AbstractRoom abstractRoom = await this.EnterRoomDebug(roomType, pointType, model, showTransition);
  }

  public async Task<AbstractRoom> EnterRoomDebug(
    RoomType roomType,
    MapPointType pointType = MapPointType.Unassigned,
    AbstractModel? model = null,
    bool showTransition = true)
  {
    AbstractRoom room;
    AbstractRoom abstractRoom;
    using (new NetLoadingHandle(this.NetService))
    {
      this.ClearScreens();
      await this.ExitCurrentRooms();
      this.CombatStateSynchronizer.StartSync();
      if (model is EncounterModel encounterModel)
        roomType = encounterModel.RoomType;
      else if (model is EventModel)
        roomType = RoomType.Event;
      if (pointType == MapPointType.Unassigned)
      {
        MapPointType mapPointType;
        switch (roomType)
        {
          case RoomType.Unassigned:
            mapPointType = MapPointType.Unassigned;
            break;
          case RoomType.Monster:
            mapPointType = MapPointType.Monster;
            break;
          case RoomType.Elite:
            mapPointType = MapPointType.Elite;
            break;
          case RoomType.Boss:
            mapPointType = MapPointType.Boss;
            break;
          case RoomType.Treasure:
            mapPointType = MapPointType.Treasure;
            break;
          case RoomType.Shop:
            mapPointType = MapPointType.Shop;
            break;
          case RoomType.Event:
            mapPointType = MapPointType.Unknown;
            break;
          case RoomType.RestSite:
            mapPointType = MapPointType.RestSite;
            break;
          case RoomType.Map:
            mapPointType = MapPointType.Unassigned;
            break;
          default:
            // ISSUE: reference to a compiler-generated method
            \u003CPrivateImplementationDetails\u003E.ThrowSwitchExpressionException((object) roomType);
            break;
        }
        pointType = mapPointType;
      }
      if (this.CombatReplayWriter.IsEnabled)
        this.CombatReplayWriter.RecordInitialState(this.ToSave((AbstractRoom) null));
      this.State.AppendToMapPointHistory(pointType, roomType, model?.Id);
      NRun.Instance?.GlobalUi.TopBar.RoomIcon.DebugSetMapPointTypeOverride(pointType);
      if (this.State.Map is MockSinglePointActMap map)
        map.MockCurrentMapPointType(pointType);
      await this.CombatStateSynchronizer.WaitForSync();
      room = this.CreateRoom(roomType, model: model);
      await this.EnterRoom(room);
      await this.FadeIn(showTransition);
      abstractRoom = room;
    }
    room = (AbstractRoom) null;
    return abstractRoom;
  }

  private async Task ExitCurrentRooms()
  {
    if (this.State == null)
      return;
    while (this.State.CurrentRoomCount > 0)
    {
      AbstractRoom abstractRoom = await this.ExitCurrentRoom();
    }
    NRun.Instance?.GlobalUi.TopBar.RoomIcon.DebugClearMapPointTypeOverride();
  }

  private async Task<AbstractRoom?> ExitCurrentRoom()
  {
    if (this.State == null)
      return (AbstractRoom) null;
    this.RewardsSetSynchronizer.BeforeLeavingRoom();
    AbstractRoom currentRoom = this.State.PopCurrentRoom();
    await currentRoom.Exit((IRunState) this.State);
    Action roomExited = this.RoomExited;
    if (roomExited != null)
      roomExited();
    return currentRoom;
  }

  private async Task EnterRoomInternal(AbstractRoom room, bool isRestoringRoomStackBase = false)
  {
    if (this.State == null)
      return;
    bool flag1 = isRestoringRoomStackBase;
    if (!flag1)
    {
      bool flag2;
      switch (room)
      {
        case CombatRoom combatRoom:
          if (combatRoom.IsPreFinished)
            goto label_4;
          goto default;
        case EventRoom eventRoom when eventRoom.IsPreFinished:
label_4:
          flag2 = true;
          break;
        default:
          flag2 = false;
          break;
      }
      flag1 = flag2;
    }
    bool runExternalEffects = !flag1;
    this.State.PushRoom(room);
    if (runExternalEffects && !(room is MapRoom))
      await Hook.BeforeRoomEntered((IRunState) this.State, room);
    await room.Enter((IRunState) this.State, isRestoringRoomStackBase);
    if (runExternalEffects)
    {
      NRunMusicController.Instance?.UpdateTrack();
      if (this.State.CurrentRoomCount == 1)
        this.State.Act.MarkRoomVisited(room.RoomType);
    }
    this.RunLocationTargetedBuffer.OnLocationChanged(this.State.RunLocation);
    if (!(room is CombatRoom))
      this.ActionExecutor.Unpause();
    NRunMusicController.Instance?.UpdateAmbience();
    Action roomEntered = this.RoomEntered;
    if (roomEntered == null)
      return;
    roomEntered();
  }

  public async Task EnterRoom(AbstractRoom room)
  {
    await this.ExitCurrentRooms();
    await this.EnterRoomInternal(room);
  }

  public async Task EnterRoomWithoutExitingCurrentRoom(AbstractRoom room, bool fadeToBlack)
  {
    NetLoadingHandle loadHandle;
    if (this.State == null)
    {
      loadHandle = (NetLoadingHandle) null;
    }
    else
    {
      this.ActionExecutor.Pause();
      this.CombatStateSynchronizer.StartSync();
      loadHandle = new NetLoadingHandle(this.NetService);
      try
      {
        if (fadeToBlack)
        {
          await this.FadeOut();
          this.ClearScreens();
        }
        await this.CombatStateSynchronizer.WaitForSync();
        MapPointHistoryEntry pointHistoryEntry = this.State.CurrentMapPointHistoryEntry;
        if (pointHistoryEntry != null)
          pointHistoryEntry.Rooms.Add(new MapPointRoomHistoryEntry()
          {
            RoomType = room.RoomType,
            ModelId = room.ModelId
          });
        await this.EnterRoomInternal(room);
        ActiveScreenContext.Instance.Update();
        if (!fadeToBlack)
        {
          loadHandle = (NetLoadingHandle) null;
        }
        else
        {
          await this.FadeIn();
          loadHandle = (NetLoadingHandle) null;
        }
      }
      finally
      {
        loadHandle?.Dispose();
      }
    }
  }

  public async Task EnterNextAct()
  {
    NetLoadingHandle loadHandle;
    if (this.State == null)
    {
      loadHandle = (NetLoadingHandle) null;
    }
    else
    {
      loadHandle = new NetLoadingHandle(this.NetService);
      try
      {
        if (this.State.CurrentActIndex >= this.State.Acts.Count - 1)
        {
          AbstractRoom currentRoom = this.State.CurrentRoom;
          if (currentRoom != null && currentRoom.IsVictoryRoom)
          {
            await this.WinRun();
            loadHandle = (NetLoadingHandle) null;
          }
          else
          {
            await this.FadeOut();
            this.ClearScreens();
            await this.EnterRoom((AbstractRoom) new EventRoom((EventModel) ModelDb.Event<TheArchitect>()));
            await this.FadeIn();
            loadHandle = (NetLoadingHandle) null;
          }
        }
        else
        {
          await this.EnterAct(this.State.CurrentActIndex + 1);
          loadHandle = (NetLoadingHandle) null;
        }
      }
      finally
      {
        loadHandle?.Dispose();
      }
    }
  }

  private async Task WinRun()
  {
    if (this.State == null)
      return;
    ((TheArchitect) ((EventRoom) this.State.CurrentRoom).LocalMutableEvent).TriggerVictory();
    this.OnEnded(true);
    await this.GuaranteeKillAllPlayers();
  }

  public async Task EnterAct(int currentActIndex, bool doTransition = true)
  {
    NetLoadingHandle loadHandle;
    if (this.State == null)
    {
      loadHandle = (NetLoadingHandle) null;
    }
    else
    {
      await this.FadeOut();
      loadHandle = new NetLoadingHandle(this.NetService);
      try
      {
        this.ClearScreens();
        await this.ExitCurrentRooms();
        await this.SetActInternal(currentActIndex);
        if (currentActIndex == 0 && this.State.ExtraFields.StartedWithNeow)
        {
          if (NRun.Instance != null)
            NMapScreen.Instance?.InitMarker(this.State.Map.StartingMapPoint.coord);
          await this.EnterMapCoord(this.State.Map.StartingMapPoint.coord);
          NMapScreen.Instance?.RefreshAllMapPointVotes();
        }
        else
        {
          await this.EnterRoomInternal((AbstractRoom) new MapRoom());
          Action actEntered = this.ActEntered;
          if (actEntered != null)
            actEntered();
        }
        await this.FadeIn(doTransition);
        await Hook.AfterActEntered((IRunState) this.State);
        loadHandle = (NetLoadingHandle) null;
      }
      finally
      {
        loadHandle?.Dispose();
      }
    }
  }

  public async Task SetActInternal(int actIndex)
  {
    if (this.State == null)
      return;
    this.State.CurrentActIndex = actIndex;
    this.State.ClearVisitedMapCoordsDebug();
    this.State.Odds.UnknownMapPoint.ResetToBase();
    this.AfterMapLocationChanged();
    await PreloadManager.LoadActAssets(this.State.Act);
    await this.GenerateMap();
    NMapScreen.Instance?.SetTravelEnabled(false);
    NRunMusicController.Instance?.UpdateMusic();
    this.UpdateRichPresence();
  }

  private void UpdateRichPresence()
  {
    if (TestMode.IsOn || this.State == null)
      return;
    PlatformUtil.SetRichPresence("IN_RUN", this.NetService.GetRawLobbyIdentifier(), new int?(this.State.Players.Count));
    PlatformUtil.SetRichPresenceValue("Character", LocalContext.GetMe((IPlayerCollection) this.State).Character.Id.Entry);
    PlatformUtil.SetRichPresenceValue("Act", this.State.Act.Id.Entry);
    PlatformUtil.SetRichPresenceValue("Ascension", this.State.AscensionLevel.ToString());
  }

  public async Task ProceedFromTerminalRewardsScreen()
  {
    if (this.State == null)
      return;
    if (this.State.CurrentRoomCount > 1)
    {
      if (this.State.CurrentRoom is CombatRoom currentRoom && currentRoom.ShouldResumeParentEventAfterCombat)
      {
        await this.ResumePreviousRoom();
      }
      else
      {
        NMapScreen.Instance?.SetTravelEnabled(true);
        NMapScreen.Instance?.Open();
      }
    }
    else
      NMapScreen.Instance?.Open();
  }

  private async Task ResumePreviousRoom()
  {
    if (this.State == null)
      return;
    this.ClearScreens();
    AbstractRoom exitedRoom = await this.ExitCurrentRoom();
    if (exitedRoom != null)
    {
      await this.State.CurrentRoom.Resume(exitedRoom, (IRunState) this.State);
      NRunMusicController.Instance?.UpdateTrack();
      await this.FadeIn();
    }
    else
      Log.Error("Current room returned null while exiting.");
  }

  private void AfterMapLocationChanged()
  {
    this.MapSelectionSynchronizer.OnLocationChanged(this.State.MapLocation);
    this.RunLocationTargetedBuffer.OnLocationChanged(this.State.RunLocation);
  }

  public void Abandon()
  {
    Log.Info("Abandoning an in-progress run (player-initiated)");
    if (this.NetService.Type == NetGameType.Singleplayer)
      TaskHelper.RunSafely(this.AbandonInternal());
    else
      this.RunLobby.AbandonRun();
  }

  void IRunLobbyListener.RunAbandoned()
  {
    Log.Info("The host told us to abandon the run");
    NMapScreen.Instance?.Close(false);
    NCapstoneContainer.Instance?.Close();
    TaskHelper.RunSafely(this.AbandonInternal());
  }

  private async Task AbandonInternal()
  {
    try
    {
      NCapstoneContainer.Instance.Close();
      NMapScreen.Instance.Close(false);
    }
    catch (Exception ex)
    {
      Log.Error($"Exception thrown while trying to abandon run: {ex}");
    }
    this.IsAbandoned = true;
    await this.GuaranteeKillAllPlayers();
    if (this.NetService.Type != NetGameType.Client)
      return;
    NErrorPopup modalToCreate = NErrorPopup.Create(new NetErrorInfo(NetError.HostAbandoned, false));
    if (modalToCreate == null)
      return;
    NModalContainer.Instance.Add((Node) modalToCreate);
  }

  private async Task GuaranteeKillAllPlayers()
  {
    if (this.State == null)
      return;
    foreach (Player player in (IEnumerable<Player>) this.State.Players)
    {
      await CreatureCmd.Kill(player.Creature, true);
      await Cmd.CustomScaledWait(0.25f, 0.5f);
    }
  }

  private void StateDiverged(NetFullCombatState state)
  {
    if (this.NetService.Type == NetGameType.Replay)
      return;
    Log.Info("Abandoning run and returning to main menu because our state diverged from host's");
    this.WriteReplay(false);
  }

  public void WriteReplay(bool stopRecording)
  {
    this.CombatReplayWriter.WriteReplay(SaveManager.Instance.GetProfileScopedPath("replays/latest.mcr"), stopRecording);
  }

  public void CleanUp(bool graceful = true)
  {
    if (this.State == null)
      return;
    this.ShouldSave = false;
    this.IsCleaningUp = true;
    try
    {
      this._runHistoryWasUploaded = false;
      this.ActionQueueSet.Reset();
      CardSelectCmd.Reset();
      NAudioManager.Instance?.StopAllLoops();
      NOverlayStack.Instance?.Clear();
      NCapstoneContainer.Instance?.CleanUp();
      NMapScreen.Instance?.CleanUp();
      NModalContainer.Instance?.Clear();
      CombatManager.Instance.Reset(graceful);
      if (this.CombatReplayWriter.IsRecordingReplay)
        this.WriteReplay(true);
      this.ActionExecutor.JustBeforeActionFinishedExecuting -= new Action<GameAction>(this.SendPostActionChecksum);
      this.CombatReplayWriter.Dispose();
      this.ActionQueueSynchronizer.Dispose();
      this.PlayerChoiceSynchronizer.Dispose();
      this.RewardSynchronizer.Dispose();
      this.RewardsSetSynchronizer.Dispose();
      this.RestSiteSynchronizer.Dispose();
      this.FlavorSynchronizer.Dispose();
      this.ChecksumTracker.Dispose();
      if (this.RunLobby != null)
      {
        this.RunLobby.RemotePlayerDisconnected -= new Action<ulong>(this.RemotePlayerDisconnected);
        this.RunLobby.Dispose();
      }
      this.NetService.Disconnect(NetError.Quit, !graceful);
    }
    finally
    {
      this.IsCleaningUp = false;
      LocalContext.NetId = new ulong?();
      this.State = (RunState) null;
      this.DailyTime = new DateTimeOffset?();
    }
  }

  public SerializableRun OnEnded(bool isVictory)
  {
    this.UpdatePlayerStatsInMapPointHistory();
    RunState state = this.State;
    Player me1 = LocalContext.GetMe((IPlayerCollection) state);
    if (state.CurrentRoom is CombatRoom currentRoom1)
    {
      MapPointRoomHistoryEntry roomHistoryEntry = state.CurrentMapPointHistoryEntry.Rooms.Last<MapPointRoomHistoryEntry>();
      PlayerCombatState playerCombatState = me1.PlayerCombatState;
      int num = playerCombatState != null ? playerCombatState.TurnNumber : currentRoom1.CombatState.RoundNumber;
      roomHistoryEntry.TurnsTaken = num;
    }
    SerializableRun save = this.ToSave((AbstractRoom) null);
    SerializablePlayer me2 = LocalContext.GetMe(save);
    if (this._runHistoryWasUploaded)
      return save;
    this._runHistoryWasUploaded = true;
    if (!isVictory && state.CurrentRoom is CombatRoom currentRoom2)
    {
      foreach ((MonsterModel monsterModel, string _) in (IEnumerable<(MonsterModel, string)>) currentRoom2.Encounter.MonstersWithSlots)
        RunManager.CheckUpdateEnemyDiscoveryAfterLoss(me1, monsterModel.Id);
    }
    if (this.ShouldSave)
    {
      using (SaveManager.Instance.BeginSaveBatch())
      {
        SaveManager.Instance.UpdateProgressWithRunData(save, isVictory);
        foreach (string discoveredEpoch in me2.DiscoveredEpochs)
        {
          if (!me1.DiscoveredEpochs.Contains(discoveredEpoch))
            me1.DiscoveredEpochs.Add(discoveredEpoch);
        }
        AchievementsHelper.AfterRunEnded(state, me1, isVictory);
        RunHistoryUtilities.CreateRunHistoryEntry(save, isVictory, this.IsAbandoned, this.NetService.Platform);
        MetricUtilities.UploadRunMetrics(save, isVictory, this.NetService.NetId);
        if (SaveManager.Instance.Progress.NumberOfRuns == 5)
          MetricUtilities.UploadSettingsMetric();
        if (this.NetService.Type == NetGameType.Singleplayer)
          SaveManager.Instance.DeleteCurrentRun();
        else if (this.NetService.Type == NetGameType.Host)
          SaveManager.Instance.DeleteCurrentMultiplayerRun();
      }
      if (isVictory)
        StatsManager.IncrementArchitectDamage(ScoreUtility.CalculateScore(save, isVictory));
    }
    if (this.DailyTime.HasValue)
    {
      bool flag;
      switch (this.NetService.Type)
      {
        case NetGameType.Singleplayer:
        case NetGameType.Host:
          flag = true;
          break;
        default:
          flag = false;
          break;
      }
      if (flag)
        TaskHelper.RunSafely(DailyRunUtility.UploadScore(this.DailyTime.Value, ScoreUtility.CalculateDailyScore(save, me1.NetId, isVictory), save.Players));
      else if (this.NetService.Type == NetGameType.Client)
        TaskHelper.RunSafely(DailyRunUtility.UploadScore(this.DailyTime.Value, -999999999, save.Players));
    }
    return save;
  }

  private static void CheckUpdateEnemyDiscoveryAfterLoss(Player player, ModelId monster)
  {
    EnemyStats enemyStats;
    if ((SaveManager.Instance.Progress.EnemyStats.TryGetValue(monster, out enemyStats) ? enemyStats : (EnemyStats) null) != null)
      return;
    player.DiscoveredEnemies.Add(monster);
  }

  private void UpdatePlayerStatsInMapPointHistory()
  {
    if (TestMode.IsOn || this.State == null)
      return;
    foreach (Player player in (IEnumerable<Player>) this.State.Players)
    {
      PlayerMapPointHistoryEntry entry = this.State.CurrentMapPointHistoryEntry?.GetEntry(player.NetId);
      if (entry != null)
      {
        entry.CurrentGold = player.Gold;
        entry.CurrentHp = player.Creature.CurrentHp;
        entry.MaxHp = player.Creature.MaxHp;
      }
    }
  }

  public bool HasAscension(AscensionLevel level)
  {
    return this.IsInProgress && this.AscensionManager.HasLevel(level);
  }

  public void ApplyAscensionEffects(Player player) => this.AscensionManager.ApplyEffectsTo(player);

  public ClientRejoinResponseMessage GetRejoinMessage()
  {
    return new ClientRejoinResponseMessage()
    {
      serializableRun = this.ToSave((AbstractRoom) null),
      combatState = NetFullCombatState.FromRun((IRunState) this.State, (GameAction) null)
    };
  }

  public void LocalPlayerDisconnected(NetErrorInfo info)
  {
    foreach (Player player in (IEnumerable<Player>) this.State.Players)
    {
      if (!LocalContext.IsMe(player))
        this.InputSynchronizer.OnPlayerDisconnected(player.NetId);
    }
    if (info.GetReason() == NetError.QuitGameOver || this.IsAbandoned || this.State.IsGameOver)
      return;
    TaskHelper.RunSafely(this.ReturnToMainMenuWithError(info));
  }

  private void RemotePlayerDisconnected(ulong playerId)
  {
    this.InputSynchronizer.OnPlayerDisconnected(playerId);
  }

  private async Task ReturnToMainMenuWithError(NetErrorInfo info)
  {
    NCapstoneContainer.Instance?.Close();
    NMapScreen.Instance?.Close(false);
    if (!TestMode.IsOff)
      return;
    await NGame.Instance.ReturnToMainMenuAfterRun();
    NErrorPopup modalToCreate = NErrorPopup.Create(info);
    if (modalToCreate == null)
      return;
    NModalContainer.Instance.Add((Node) modalToCreate);
  }

  public string? GetLocalCharacterEnergyIconPrefix()
  {
    CardPoolModel cardPool = LocalContext.GetMe((IPlayerCollection) this.State)?.Character.CardPool;
    return cardPool != null ? EnergyIconHelper.GetPrefix((AbstractModel) cardPool) : (string) null;
  }

  public RunState? DebugOnlyGetState() => this.State;
}
