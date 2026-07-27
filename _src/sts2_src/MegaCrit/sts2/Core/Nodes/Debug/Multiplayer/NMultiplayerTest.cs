// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Debug.Multiplayer.NMultiplayerTest
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Connection;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Game.Lobby;
using MegaCrit.Sts2.Core.Multiplayer.Replay;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.Unlocks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Debug.Multiplayer;

[ScriptPath("res://src/Core/Nodes/Debug/Multiplayer/NMultiplayerTest.cs")]
public class NMultiplayerTest : Control, IStartRunLobbyListener
{
  private const ushort _port = 33771;
  private TextEdit _ipField;
  private TextEdit _idField;
  private Button _readyButton;
  private Control _readyIndicator;
  private Control _loadingPanel;
  private NMultiplayerTestCharacterPaginator _characterPaginator;
  private readonly List<NMultiplayerTest.CharacterContainer> _characterContainers = new List<NMultiplayerTest.CharacterContainer>();
  private NGame _game;
  private StartRunLobby? _lobby;
  private readonly SerializablePlayer _localPlayerData = new SerializablePlayer();
  private CancellationTokenSource _cts = new CancellationTokenSource();
  private IBootstrapSettings? _settings;
  private bool _ignoreReplayModelIdHash;
  private bool _beginningRun;

  public override void _Ready()
  {
    this._ipField = ((Node) this).GetNode<TextEdit>(NodePath.op_Implicit("IpField"));
    this._idField = ((Node) this).GetNode<TextEdit>(NodePath.op_Implicit("NameField"));
    this._characterPaginator = ((Node) this).GetNode<NMultiplayerTestCharacterPaginator>(NodePath.op_Implicit("CharacterChooser"));
    Button node1 = ((Node) this).GetNode<Button>(NodePath.op_Implicit("HostButton"));
    Button node2 = ((Node) this).GetNode<Button>(NodePath.op_Implicit("JoinButton"));
    this._readyButton = ((Node) this).GetNode<Button>(NodePath.op_Implicit("ReadyButton"));
    this._readyIndicator = ((Node) this).GetNode<Control>(NodePath.op_Implicit("ReadyButton/ReadyIndicator"));
    Button node3 = ((Node) this).GetNode<Button>(NodePath.op_Implicit("ReplayButton"));
    Button node4 = ((Node) this).GetNode<Button>(NodePath.op_Implicit("SaveReplayButton"));
    Button node5 = ((Node) this).GetNode<Button>(NodePath.op_Implicit("DeleteCloudSavesButton"));
    this._loadingPanel = ((Node) this).GetNode<Control>(NodePath.op_Implicit("LoadingPanel"));
    foreach (Node child in ((Node) ((Node) this).GetNode<Control>(NodePath.op_Implicit("Characters"))).GetChildren(false))
      this._characterContainers.Add(new NMultiplayerTest.CharacterContainer()
      {
        characterImage = child.GetNode<TextureRect>(NodePath.op_Implicit("Image")),
        playerName = child.GetNode<Label>(NodePath.op_Implicit("Name"))
      });
    ((GodotObject) node1).Connect(BaseButton.SignalName.ButtonUp, Callable.From(new Action(this.HostButtonPressed)), 0U);
    ((GodotObject) node2).Connect(BaseButton.SignalName.ButtonUp, Callable.From(new Action(this.JoinButtonPressed)), 0U);
    ((GodotObject) this._readyButton).Connect(BaseButton.SignalName.ButtonUp, Callable.From(new Action(this.ReadyButtonPressed)), 0U);
    ((GodotObject) node3).Connect(BaseButton.SignalName.ButtonUp, Callable.From(new Action(this.ChooseReplayToLoad)), 0U);
    ((GodotObject) node4).Connect(BaseButton.SignalName.ButtonUp, Callable.From(new Action(this.ChooseReplayToSave)), 0U);
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    ((GodotObject) node5).Connect(BaseButton.SignalName.ButtonUp, Callable.From(NMultiplayerTest.\u003C\u003EO.\u003C0\u003E__DeleteCloudSaves ?? (NMultiplayerTest.\u003C\u003EO.\u003C0\u003E__DeleteCloudSaves = new Action(CloudConsoleCmd.DeleteCloudSaves))), 0U);
    ((CanvasItem) this._characterPaginator).Visible = false;
    this._characterPaginator.CharacterChanged += new Action<CharacterModel>(this.OnCharacterChanged);
    ((CanvasItem) this._readyButton).Visible = false;
    this._game = ((Node) ((Node) this).GetTree().Root).GetNodeOrNull<NGame>(NodePath.op_Implicit("Game"));
    if (this._game == null)
    {
      this._game = SceneHelper.Instantiate<NGame>("game");
      this._game.StartOnMainMenu = false;
      Callable callable = Callable.From(new Action(this.AddGame));
      ((Callable) ref callable).CallDeferred(Array.Empty<Variant>());
    }
    Type type = BootstrapSettingsUtil.Get();
    if (type != (Type) null)
      this._settings = (IBootstrapSettings) Activator.CreateInstance(type);
    Logger.logLevelTypeMap[LogType.Network] = LogLevel.Debug;
    Logger.logLevelTypeMap[LogType.Actions] = LogLevel.VeryDebug;
    Logger.logLevelTypeMap[LogType.GameSync] = LogLevel.VeryDebug;
  }

  public override void _EnterTree() => this._cts = new CancellationTokenSource();

  public override void _ExitTree()
  {
    this._cts.Cancel();
    this._characterPaginator.CharacterChanged -= new Action<CharacterModel>(this.OnCharacterChanged);
    if (this._beginningRun)
      return;
    this._lobby?.CleanUp(true);
  }

  private void AddGame()
  {
    ((Node) ((Node) this).GetTree().Root).AddChildSafely((Node) this._game);
    this._game.RootSceneContainer.SetCurrentScene((Control) this);
    TaskHelper.RunSafely(this._game.Transition.FadeIn());
  }

  private void HostButtonPressed() => TaskHelper.RunSafely((Task) this.StartHost(false));

  private void JoinButtonPressed()
  {
    TaskHelper.RunSafely(this.JoinToHost((IClientConnectionInitializer) new ENetClientConnectionInitializer(this._idField.Text != string.Empty ? ulong.Parse(this._idField.Text) : 1000UL, this._ipField.Text != string.Empty ? this._ipField.Text : "127.0.0.1", (ushort) 33771)));
  }

  private void ReadyButtonPressed()
  {
    this._localPlayerData.Deck = this._characterPaginator.Character.StartingDeck.Select<CardModel, SerializableCard>((Func<CardModel, SerializableCard>) (c => c.ToMutable().ToSerializable())).ToList<SerializableCard>();
    this._localPlayerData.Relics = this._characterPaginator.Character.StartingRelics.Select<RelicModel, SerializableRelic>((Func<RelicModel, SerializableRelic>) (r => r.ToMutable().ToSerializable())).ToList<SerializableRelic>();
    this._localPlayerData.Potions = new List<SerializablePotion>();
    this._localPlayerData.Rng = new SerializablePlayerRngSet();
    this._localPlayerData.Odds = new SerializablePlayerOddsSet();
    this._localPlayerData.RelicGrabBag = new SerializableRelicGrabBag();
    this._localPlayerData.ExtraFields = new SerializableExtraPlayerFields();
    this._localPlayerData.UnlockState = new SerializableUnlockState();
    this._lobby.SetReady(true);
    ((CanvasItem) this._readyIndicator).Visible = true;
  }

  public void BeginRun(string seed, List<ActModel> acts, IReadOnlyList<ModifierModel> __)
  {
    ((CanvasItem) this._loadingPanel).Visible = true;
    TaskHelper.RunSafely(this.BeginRunAsyncWrapper(seed, acts));
  }

  private async Task BeginRunAsyncWrapper(string seed, List<ActModel> acts)
  {
    try
    {
      await this.BeginRunAsync(seed, acts);
    }
    finally
    {
      if (((Node) this._loadingPanel).IsValid())
        ((CanvasItem) this._loadingPanel).Visible = false;
    }
  }

  private async Task BeginRunAsync(string seed, List<ActModel> acts)
  {
    this._beginningRun = true;
    IBootstrapSettings settings = this._settings;
    if (settings != null && settings.BootstrapInMultiplayer)
    {
      RunState runState;
      using (new NetLoadingHandle(this._lobby.NetService))
      {
        acts[0] = this._settings.Act;
        runState = RunState.CreateForNewRun((IReadOnlyList<Player>) this._lobby.Players.Select<LobbyPlayer, Player>((Func<LobbyPlayer, Player>) (p => Player.CreateForNewRun(p.character, UnlockState.FromSerializable(p.unlockState), p.id))).ToList<Player>(), (IReadOnlyList<ActModel>) acts.Select<ActModel, ActModel>((Func<ActModel, ActModel>) (a => a.ToMutable())).ToList<ActModel>(), (IReadOnlyList<ModifierModel>) this._settings.Modifiers, GameMode.Standard, this._lobby.Ascension, seed);
        RunManager.Instance.SetUpNewMultiplayer(runState, this._lobby, this._settings.SaveRunHistory);
        await PreloadManager.LoadRunAssets(runState.Players.Select<Player, CharacterModel>((Func<Player, CharacterModel>) (p => p.Character)));
        await RunManager.Instance.FinalizeStartingRelics();
        RunManager.Instance.Launch();
        this._game.RootSceneContainer.SetCurrentScene((Control) NRun.Create(runState));
        await RunManager.Instance.SetActInternal(0);
        await SaveManager.Instance.SaveRun((AbstractRoom) null);
        this._lobby.CleanUp(false);
        await this._settings.Setup(LocalContext.GetMe((IPlayerCollection) runState));
        switch (this._settings.RoomType)
        {
          case RoomType.Unassigned:
            await RunManager.Instance.EnterAct(0);
            break;
          case RoomType.Treasure:
          case RoomType.Shop:
          case RoomType.RestSite:
            AbstractRoom abstractRoom1 = await RunManager.Instance.EnterRoomDebug(this._settings.RoomType, showTransition: false);
            RunManager.Instance.ActionExecutor.Unpause();
            break;
          case RoomType.Event:
            AbstractRoom abstractRoom2 = await RunManager.Instance.EnterRoomDebug(this._settings.RoomType, model: (AbstractModel) this._settings.Event, showTransition: false);
            break;
          default:
            AbstractRoom abstractRoom3 = await RunManager.Instance.EnterRoomDebug(this._settings.RoomType, model: this._settings.RoomType.IsCombatRoom() ? (AbstractModel) this._settings.Encounter.ToMutable() : (AbstractModel) null, showTransition: false);
            break;
        }
      }
      runState = (RunState) null;
    }
    else
    {
      RunState runState = await this._game.StartNewMultiplayerRun(this._lobby, true, (IReadOnlyList<ActModel>) acts, (IReadOnlyList<ModifierModel>) Array.Empty<ModifierModel>(), seed, this._lobby.Ascension);
      this._lobby.CleanUp(false);
    }
  }

  private void Disconnect(NetError reason)
  {
    this._lobby?.CleanUp(true, reason);
    this._lobby = (StartRunLobby) null;
  }

  private async Task<bool> StartHost(bool steam)
  {
    IBootstrapSettings settings = this._settings;
    if ((settings != null ? (settings.BootstrapInMultiplayer ? 1 : 0) : 0) != 0)
    {
      PreloadManager.Enabled = this._settings.DoPreloading;
      this._game.DebugSeedOverride = this._settings.Seed;
    }
    this.Disconnect(NetError.Quit);
    NetHostGameService netService = new NetHostGameService();
    NetErrorInfo? nullable;
    if (steam)
      nullable = await netService.StartSteamHost(4);
    else
      nullable = netService.StartENetHost((ushort) 33771, 4);
    if (!nullable.HasValue)
    {
      this._lobby = new StartRunLobby(GameMode.Standard, (INetGameService) netService, (IStartRunLobbyListener) this, 4);
      this._lobby.AddLocalHostPlayer(new UnlockState(SaveManager.Instance.Progress), SaveManager.Instance.Progress.MaxMultiplayerAscension);
      this.AfterMultiplayerStarted();
      Log.Info("Successful host");
    }
    else
    {
      this._lobby = (StartRunLobby) null;
      Log.Info($"Failed host: {nullable}");
    }
    bool flag = !nullable.HasValue;
    netService = (NetHostGameService) null;
    return flag;
  }

  public async Task JoinToHost(IClientConnectionInitializer initializer)
  {
    IBootstrapSettings settings = this._settings;
    if ((settings != null ? (settings.BootstrapInMultiplayer ? 1 : 0) : 0) != 0)
    {
      PreloadManager.Enabled = this._settings.DoPreloading;
      this._game.DebugSeedOverride = this._settings.Seed;
    }
    this.Disconnect(NetError.Quit);
    JoinFlow joinFlow = new JoinFlow((INetClientGameService) new NetClientGameService());
    try
    {
      JoinResult joinResult = await joinFlow.Begin(initializer, ((Node) this).GetTree());
      if (joinResult.sessionState.GetValueOrDefault() == RunSessionState.InLobby)
      {
        Log.Info("Successfully joined lobby");
        this._lobby = new StartRunLobby(joinResult.gameMode, (INetGameService) joinFlow.NetService, (IStartRunLobbyListener) this, -1);
        this._lobby.InitializeFromMessage(joinResult.joinResponse.Value);
        this.AfterMultiplayerStarted();
        joinFlow = (JoinFlow) null;
      }
      else if (joinResult.sessionState.GetValueOrDefault() != RunSessionState.Running)
      {
        joinFlow = (JoinFlow) null;
      }
      else
      {
        Log.Info("Successfully joined run in-progress. Initializing run");
        throw new NotImplementedException("Run re-joining has yet to be implemented!");
      }
    }
    catch (Exception ex)
    {
      joinFlow.NetService.Disconnect(NetError.RunInProgress);
      this._lobby = (StartRunLobby) null;
      Log.Info($"Failed join: {ex}");
      joinFlow = (JoinFlow) null;
    }
  }

  private void AfterMultiplayerStarted()
  {
    foreach (LobbyPlayer player in this._lobby.Players)
    {
      this._characterContainers[player.slotId].characterImage.Texture = player.character.IconTexture;
      this._characterContainers[player.slotId].playerName.Text = PlatformUtil.GetPlayerNameRaw(this._lobby.NetService.Platform, player.id);
    }
    ((CanvasItem) this._readyButton).Visible = true;
    ((CanvasItem) this._characterPaginator).Visible = true;
    this._game.RemoteCursorContainer.Initialize(this._lobby.InputSynchronizer, this._lobby.Players.Select<LobbyPlayer, ulong>((Func<LobbyPlayer, ulong>) (p => p.id)));
    this._game.ReactionContainer.InitializeNetworking(this._lobby.NetService);
    this.OnCharacterChanged(this._lobby.LocalPlayer.character);
  }

  private void ChooseReplayToLoad() => this.ChooseReplay(new Action<string>(this.LoadReplay));

  private void ChooseReplayToSave()
  {
    this.ChooseReplay(new Action<string>(this.WriteReplayAsSave));
  }

  private void ChooseReplay(Action<string> action)
  {
    FileDialog child = new FileDialog();
    child.Filters = new string[1]{ "*.mcr" };
    child.UseNativeDialog = true;
    ((Window) child).Title = "Choose Replay";
    child.Access = (FileDialog.AccessEnum) 2L;
    child.FileMode = (FileDialog.FileModeEnum) 0L;
    child.CurrentDir = ProjectSettings.GlobalizePath("user://");
    ((GodotObject) child).Connect(FileDialog.SignalName.FileSelected, Callable.From<string>(action), 0U);
    ((Node) this).AddChildSafely((Node) child);
    ((Window) child).Show();
  }

  private bool ValidateReplay(CombatReplay replay)
  {
    if ((int) replay.modelIdHash != (int) ModelIdSerializationCache.Hash)
    {
      if (!this._ignoreReplayModelIdHash)
      {
        Log.Error($"Attempting to load replay with Model ID hash {replay.modelIdHash} that does not match ours ({ModelIdSerializationCache.Hash})! The replay will mismatch. If you want to continue anyway, try running the replay again.");
        this._ignoreReplayModelIdHash = true;
        return false;
      }
      Log.Warn("Ignoring model ID hash mismatch in replay.");
    }
    return true;
  }

  private void LoadReplay(string path)
  {
    using (MemoryStream memoryStream = new MemoryStream())
    {
      using (FileAccessStream fileAccessStream = new FileAccessStream(path, (FileAccess.ModeFlags) 1L))
        fileAccessStream.CopyTo((Stream) memoryStream);
      PacketReader packetReader = new PacketReader();
      packetReader.Reset(memoryStream.ToArray());
      CombatReplay replay = packetReader.Read<CombatReplay>();
      Log.Info($"Loaded replay. Game version: {replay.version} Commit: {replay.gitCommit} Model ID hash: {replay.modelIdHash}");
      if (!this.ValidateReplay(replay))
        return;
      int valueOrDefault = ((int?) this._settings?.ReplayPlayerIndex).GetValueOrDefault();
      TaskHelper.RunSafely(NMultiplayerTest.RunReplay(replay, ((Node) this).GetTree(), valueOrDefault));
    }
  }

  private static async Task RunReplay(CombatReplay replay, SceneTree sceneTree, int playerIndex)
  {
    string str = ReleaseInfoManager.Instance.ReleaseInfo?.Commit ?? GitHelper.ShortCommitId;
    if (replay.gitCommit != str)
      Log.Warn($"Git commit in replay {replay.gitCommit} does not match ours ({str}). The replay has a chance of mismatching!");
    RunState runState = RunState.FromSerializable(replay.serializableRun);
    RunManager.Instance.SetUpReplay(runState, replay, runState.Players[playerIndex].NetId);
    RunManager.Instance.CombatStateSynchronizer.IsDisabled = true;
    await PreloadManager.LoadRunAssets(runState.Players.Select<Player, CharacterModel>((Func<Player, CharacterModel>) (p => p.Character)));
    await PreloadManager.LoadActAssets(runState.Act);
    RunManager.Instance.Launch();
    NAudioManager.Instance?.StopMusic();
    NGame.Instance.RootSceneContainer.SetCurrentScene((Control) NRun.Create(runState));
    await RunManager.Instance.GenerateMap();
    RunManager.Instance.ActionQueueSet.FastForwardNextActionId(replay.nextActionId);
    RunManager.Instance.ActionQueueSynchronizer.FastForwardHookId(replay.nextHookId);
    RunManager.Instance.ChecksumTracker.LoadReplayChecksums(replay.checksumData, replay.nextChecksumId);
    RunManager.Instance.PlayerChoiceSynchronizer.FastForwardChoiceIds(replay.choiceIds);
    RunManager.Instance.RewardsSetSynchronizer.FastForwardRewardIds(replay.rewardIds);
    await RunManager.Instance.LoadIntoLatestMapCoord(AbstractRoom.FromSerializable(replay.serializableRun.PreFinishedRoom, (IRunState) runState));
    while (RunManager.Instance.ActionExecutor.IsPaused)
    {
      double num1 = (double) await ((Node) sceneTree.Root).AwaitProcessFrame();
    }
    foreach (CombatReplayEvent combatReplayEvent in replay.events)
    {
      CombatReplayEvent replayEvent = combatReplayEvent;
      switch (replayEvent.eventType)
      {
        case CombatReplayEventType.GameAction:
          while (CombatManager.Instance.EndingPlayerTurnPhaseOne || CombatManager.Instance.EndingPlayerTurnPhaseTwo)
          {
            double num2 = (double) await ((Node) sceneTree.Root).AwaitProcessFrame();
          }
          GameAction action = replayEvent.action.ToGameAction(runState.GetPlayer(replayEvent.playerId.Value));
          if (action.ActionType == GameActionType.CombatPlayPhaseOnly)
          {
            while (CombatManager.Instance.DebugOnlyGetState().CurrentSide == CombatSide.Enemy)
            {
              double num3 = (double) await ((Node) sceneTree.Root).AwaitProcessFrame();
            }
          }
          RunManager.Instance.ActionQueueSet.EnqueueWithoutSynchronizing(action);
          if (action is EndPlayerTurnAction || action is ReadyToBeginEnemyTurnAction)
          {
            await RunManager.Instance.ActionExecutor.FinishedExecutingActions();
            break;
          }
          break;
        case CombatReplayEventType.HookAction:
          RunManager.Instance.ActionQueueSet.EnqueueWithoutSynchronizing((GameAction) RunManager.Instance.ActionQueueSynchronizer.GetHookActionForId(replayEvent.hookId.Value, replayEvent.playerId.Value, replayEvent.gameActionType.Value));
          break;
        case CombatReplayEventType.ResumeAction:
          RunManager.Instance.ActionQueueSet.ResumeActionWithoutSynchronizing(replayEvent.actionId.Value);
          break;
        case CombatReplayEventType.PlayerChoice:
          RunManager.Instance.PlayerChoiceSynchronizer.ReceiveReplayChoice(runState.GetPlayer(replayEvent.playerId.Value), replayEvent.choiceId.Value, replayEvent.playerChoiceResult.Value);
          break;
        default:
          throw new InvalidEnumArgumentException();
      }
      replayEvent = new CombatReplayEvent();
    }
    runState = (RunState) null;
  }

  private void WriteReplayAsSave(string path)
  {
    using (MemoryStream memoryStream = new MemoryStream())
    {
      using (FileAccessStream fileAccessStream = new FileAccessStream(path, (FileAccess.ModeFlags) 1L))
        fileAccessStream.CopyTo((Stream) memoryStream);
      PacketReader packetReader = new PacketReader();
      packetReader.Reset(memoryStream.ToArray());
      CombatReplay replay = packetReader.Read<CombatReplay>();
      if (!this.ValidateReplay(replay))
        return;
      SerializableRun serializableRun = replay.serializableRun;
      string str1 = JsonSerializer.Serialize<SerializableRun>(serializableRun, JsonSerializationUtility.GetTypeInfo<SerializableRun>());
      for (int index = 0; index < serializableRun.Players.Count; ++index)
        str1 = str1.Replace(serializableRun.Players[index].NetId.ToString(), (index == 0 ? 1 : index * 1000).ToString());
      string str2 = serializableRun.Players.Count > 1 ? "current_run_mp.save" : "current_run.save";
      string text = Path.Combine(UserDataPathProvider.GetProfileScopedPath(SaveManager.Instance.CurrentProfileId, "saves"), str2);
      Log.Info(text);
      using (FileAccess fileAccess = FileAccess.Open(text, (FileAccess.ModeFlags) 2L))
      {
        if (fileAccess == null)
          throw new InvalidOperationException($"Couldn't open {text}: {FileAccess.GetOpenError()}.");
        fileAccess.StoreString(str1);
        Log.Info($"Wrote {str1.Length} chars to {text}");
      }
    }
  }

  private void OnCharacterChanged(CharacterModel model)
  {
    this._lobby.SetLocalCharacter(model);
    this._localPlayerData.CharacterId = model.Id;
    this._localPlayerData.CurrentHp = model.StartingHp;
    this._localPlayerData.MaxHp = model.StartingHp;
    this._localPlayerData.MaxEnergy = model.MaxEnergy;
    this._localPlayerData.MaxPotionSlotCount = 3;
    this._localPlayerData.Gold = model.StartingGold;
  }

  public override void _Process(double delta) => this._lobby?.NetService.Update();

  public void PlayerConnected(LobbyPlayer player)
  {
    this._characterContainers[player.slotId].characterImage.Texture = player.character.IconTexture;
    this._characterContainers[player.slotId].playerName.Text = PlatformUtil.GetPlayerNameRaw(this._lobby.NetService.Platform, player.id);
  }

  public void PlayerChanged(LobbyPlayer player, bool isRandomCharacterResolution)
  {
    this._characterContainers[player.slotId].characterImage.Texture = player.character.IconTexture;
    this._characterContainers[player.slotId].playerName.Text = PlatformUtil.GetPlayerNameRaw(this._lobby.NetService.Platform, player.id);
  }

  public void AscensionChanged()
  {
  }

  public void SeedChanged()
  {
  }

  public void ModifiersChanged()
  {
  }

  public void MaxAscensionChanged()
  {
  }

  public void RemotePlayerDisconnected(LobbyPlayer player)
  {
    this._characterContainers[player.slotId].characterImage.Texture = (Texture2D) null;
    this._characterContainers[player.slotId].playerName.Text = "?";
  }

  public void LocalPlayerDisconnected(NetErrorInfo info)
  {
    this._lobby = (StartRunLobby) null;
    ((CanvasItem) this._characterPaginator).Visible = false;
    ((CanvasItem) this._readyButton).Visible = false;
    this._characterPaginator.SetIndex(0);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(18)
    {
      new MethodInfo(NMultiplayerTest.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerTest.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerTest.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerTest.MethodName.AddGame, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerTest.MethodName.HostButtonPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerTest.MethodName.JoinButtonPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerTest.MethodName.ReadyButtonPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerTest.MethodName.Disconnect, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("reason"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMultiplayerTest.MethodName.AfterMultiplayerStarted, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerTest.MethodName.ChooseReplayToLoad, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerTest.MethodName.ChooseReplayToSave, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerTest.MethodName.LoadReplay, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("path"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMultiplayerTest.MethodName.WriteReplayAsSave, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("path"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMultiplayerTest.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMultiplayerTest.MethodName.AscensionChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerTest.MethodName.SeedChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerTest.MethodName.ModifiersChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerTest.MethodName.MaxAscensionChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMultiplayerTest.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerTest.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerTest.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerTest.MethodName.AddGame) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AddGame();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerTest.MethodName.HostButtonPressed) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.HostButtonPressed();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerTest.MethodName.JoinButtonPressed) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.JoinButtonPressed();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerTest.MethodName.ReadyButtonPressed) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ReadyButtonPressed();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerTest.MethodName.Disconnect) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.Disconnect(VariantUtils.ConvertTo<NetError>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerTest.MethodName.AfterMultiplayerStarted) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AfterMultiplayerStarted();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerTest.MethodName.ChooseReplayToLoad) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ChooseReplayToLoad();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerTest.MethodName.ChooseReplayToSave) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ChooseReplayToSave();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerTest.MethodName.LoadReplay) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.LoadReplay(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerTest.MethodName.WriteReplayAsSave) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.WriteReplayAsSave(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerTest.MethodName._Process) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerTest.MethodName.AscensionChanged) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AscensionChanged();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerTest.MethodName.SeedChanged) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SeedChanged();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerTest.MethodName.ModifiersChanged) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ModifiersChanged();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NMultiplayerTest.MethodName.MaxAscensionChanged) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.MaxAscensionChanged();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMultiplayerTest.MethodName._Ready) || StringName.op_Equality(ref method, NMultiplayerTest.MethodName._EnterTree) || StringName.op_Equality(ref method, NMultiplayerTest.MethodName._ExitTree) || StringName.op_Equality(ref method, NMultiplayerTest.MethodName.AddGame) || StringName.op_Equality(ref method, NMultiplayerTest.MethodName.HostButtonPressed) || StringName.op_Equality(ref method, NMultiplayerTest.MethodName.JoinButtonPressed) || StringName.op_Equality(ref method, NMultiplayerTest.MethodName.ReadyButtonPressed) || StringName.op_Equality(ref method, NMultiplayerTest.MethodName.Disconnect) || StringName.op_Equality(ref method, NMultiplayerTest.MethodName.AfterMultiplayerStarted) || StringName.op_Equality(ref method, NMultiplayerTest.MethodName.ChooseReplayToLoad) || StringName.op_Equality(ref method, NMultiplayerTest.MethodName.ChooseReplayToSave) || StringName.op_Equality(ref method, NMultiplayerTest.MethodName.LoadReplay) || StringName.op_Equality(ref method, NMultiplayerTest.MethodName.WriteReplayAsSave) || StringName.op_Equality(ref method, NMultiplayerTest.MethodName._Process) || StringName.op_Equality(ref method, NMultiplayerTest.MethodName.AscensionChanged) || StringName.op_Equality(ref method, NMultiplayerTest.MethodName.SeedChanged) || StringName.op_Equality(ref method, NMultiplayerTest.MethodName.ModifiersChanged) || StringName.op_Equality(ref method, NMultiplayerTest.MethodName.MaxAscensionChanged) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMultiplayerTest.PropertyName._ipField))
    {
      this._ipField = VariantUtils.ConvertTo<TextEdit>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerTest.PropertyName._idField))
    {
      this._idField = VariantUtils.ConvertTo<TextEdit>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerTest.PropertyName._readyButton))
    {
      this._readyButton = VariantUtils.ConvertTo<Button>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerTest.PropertyName._readyIndicator))
    {
      this._readyIndicator = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerTest.PropertyName._loadingPanel))
    {
      this._loadingPanel = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerTest.PropertyName._characterPaginator))
    {
      this._characterPaginator = VariantUtils.ConvertTo<NMultiplayerTestCharacterPaginator>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerTest.PropertyName._game))
    {
      this._game = VariantUtils.ConvertTo<NGame>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerTest.PropertyName._ignoreReplayModelIdHash))
    {
      this._ignoreReplayModelIdHash = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMultiplayerTest.PropertyName._beginningRun))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._beginningRun = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMultiplayerTest.PropertyName._ipField))
    {
      value = VariantUtils.CreateFrom<TextEdit>(ref this._ipField);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerTest.PropertyName._idField))
    {
      value = VariantUtils.CreateFrom<TextEdit>(ref this._idField);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerTest.PropertyName._readyButton))
    {
      value = VariantUtils.CreateFrom<Button>(ref this._readyButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerTest.PropertyName._readyIndicator))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._readyIndicator);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerTest.PropertyName._loadingPanel))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._loadingPanel);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerTest.PropertyName._characterPaginator))
    {
      value = VariantUtils.CreateFrom<NMultiplayerTestCharacterPaginator>(ref this._characterPaginator);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerTest.PropertyName._game))
    {
      value = VariantUtils.CreateFrom<NGame>(ref this._game);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerTest.PropertyName._ignoreReplayModelIdHash))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._ignoreReplayModelIdHash);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMultiplayerTest.PropertyName._beginningRun))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<bool>(ref this._beginningRun);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NMultiplayerTest.PropertyName._ipField, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerTest.PropertyName._idField, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerTest.PropertyName._readyButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerTest.PropertyName._readyIndicator, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerTest.PropertyName._loadingPanel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerTest.PropertyName._characterPaginator, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerTest.PropertyName._game, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NMultiplayerTest.PropertyName._ignoreReplayModelIdHash, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NMultiplayerTest.PropertyName._beginningRun, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NMultiplayerTest.PropertyName._ipField, Variant.From<TextEdit>(ref this._ipField));
    info.AddProperty(NMultiplayerTest.PropertyName._idField, Variant.From<TextEdit>(ref this._idField));
    info.AddProperty(NMultiplayerTest.PropertyName._readyButton, Variant.From<Button>(ref this._readyButton));
    info.AddProperty(NMultiplayerTest.PropertyName._readyIndicator, Variant.From<Control>(ref this._readyIndicator));
    info.AddProperty(NMultiplayerTest.PropertyName._loadingPanel, Variant.From<Control>(ref this._loadingPanel));
    info.AddProperty(NMultiplayerTest.PropertyName._characterPaginator, Variant.From<NMultiplayerTestCharacterPaginator>(ref this._characterPaginator));
    info.AddProperty(NMultiplayerTest.PropertyName._game, Variant.From<NGame>(ref this._game));
    info.AddProperty(NMultiplayerTest.PropertyName._ignoreReplayModelIdHash, Variant.From<bool>(ref this._ignoreReplayModelIdHash));
    info.AddProperty(NMultiplayerTest.PropertyName._beginningRun, Variant.From<bool>(ref this._beginningRun));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NMultiplayerTest.PropertyName._ipField, ref variant1))
      this._ipField = ((Variant) ref variant1).As<TextEdit>();
    Variant variant2;
    if (info.TryGetProperty(NMultiplayerTest.PropertyName._idField, ref variant2))
      this._idField = ((Variant) ref variant2).As<TextEdit>();
    Variant variant3;
    if (info.TryGetProperty(NMultiplayerTest.PropertyName._readyButton, ref variant3))
      this._readyButton = ((Variant) ref variant3).As<Button>();
    Variant variant4;
    if (info.TryGetProperty(NMultiplayerTest.PropertyName._readyIndicator, ref variant4))
      this._readyIndicator = ((Variant) ref variant4).As<Control>();
    Variant variant5;
    if (info.TryGetProperty(NMultiplayerTest.PropertyName._loadingPanel, ref variant5))
      this._loadingPanel = ((Variant) ref variant5).As<Control>();
    Variant variant6;
    if (info.TryGetProperty(NMultiplayerTest.PropertyName._characterPaginator, ref variant6))
      this._characterPaginator = ((Variant) ref variant6).As<NMultiplayerTestCharacterPaginator>();
    Variant variant7;
    if (info.TryGetProperty(NMultiplayerTest.PropertyName._game, ref variant7))
      this._game = ((Variant) ref variant7).As<NGame>();
    Variant variant8;
    if (info.TryGetProperty(NMultiplayerTest.PropertyName._ignoreReplayModelIdHash, ref variant8))
      this._ignoreReplayModelIdHash = ((Variant) ref variant8).As<bool>();
    Variant variant9;
    if (!info.TryGetProperty(NMultiplayerTest.PropertyName._beginningRun, ref variant9))
      return;
    this._beginningRun = ((Variant) ref variant9).As<bool>();
  }

  private struct CharacterContainer
  {
    public 
    #nullable enable
    TextureRect characterImage;
    public Label playerName;
  }

  public class MethodName : Control.MethodName
  {
    public static readonly 
    #nullable disable
    StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName AddGame = StringName.op_Implicit(nameof (AddGame));
    public static readonly StringName HostButtonPressed = StringName.op_Implicit(nameof (HostButtonPressed));
    public static readonly StringName JoinButtonPressed = StringName.op_Implicit(nameof (JoinButtonPressed));
    public static readonly StringName ReadyButtonPressed = StringName.op_Implicit(nameof (ReadyButtonPressed));
    public static readonly StringName Disconnect = StringName.op_Implicit(nameof (Disconnect));
    public static readonly StringName AfterMultiplayerStarted = StringName.op_Implicit(nameof (AfterMultiplayerStarted));
    public static readonly StringName ChooseReplayToLoad = StringName.op_Implicit(nameof (ChooseReplayToLoad));
    public static readonly StringName ChooseReplayToSave = StringName.op_Implicit(nameof (ChooseReplayToSave));
    public static readonly StringName LoadReplay = StringName.op_Implicit(nameof (LoadReplay));
    public static readonly StringName WriteReplayAsSave = StringName.op_Implicit(nameof (WriteReplayAsSave));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
    public static readonly StringName AscensionChanged = StringName.op_Implicit(nameof (AscensionChanged));
    public static readonly StringName SeedChanged = StringName.op_Implicit(nameof (SeedChanged));
    public static readonly StringName ModifiersChanged = StringName.op_Implicit(nameof (ModifiersChanged));
    public static readonly StringName MaxAscensionChanged = StringName.op_Implicit(nameof (MaxAscensionChanged));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _ipField = StringName.op_Implicit(nameof (_ipField));
    public static readonly StringName _idField = StringName.op_Implicit(nameof (_idField));
    public static readonly StringName _readyButton = StringName.op_Implicit(nameof (_readyButton));
    public static readonly StringName _readyIndicator = StringName.op_Implicit(nameof (_readyIndicator));
    public static readonly StringName _loadingPanel = StringName.op_Implicit(nameof (_loadingPanel));
    public static readonly StringName _characterPaginator = StringName.op_Implicit(nameof (_characterPaginator));
    public static readonly StringName _game = StringName.op_Implicit(nameof (_game));
    public static readonly StringName _ignoreReplayModelIdHash = StringName.op_Implicit(nameof (_ignoreReplayModelIdHash));
    public static readonly StringName _beginningRun = StringName.op_Implicit(nameof (_beginningRun));
  }

  public class SignalName : Control.SignalName
  {
  }
}
