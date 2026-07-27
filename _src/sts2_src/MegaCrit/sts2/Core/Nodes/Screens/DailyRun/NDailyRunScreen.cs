// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.DailyRun.NDailyRunScreen
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Daily;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Game.Lobby;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.Unlocks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.DailyRun;

[ScriptPath("res://src/Core/Nodes/Screens/DailyRun/NDailyRunScreen.cs")]
public class NDailyRunScreen : NSubmenu, IStartRunLobbyListener
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("screens/daily_run/daily_run_screen");
  private static readonly LocString _timeLeftLoc = new LocString("main_menu_ui", "DAILY_RUN_MENU.TIME_LEFT");
  public static readonly string dateFormat = LocManager.Instance.GetTable("main_menu_ui").GetRawText("DAILY_RUN_MENU.DATE_FORMAT");
  private static readonly string _timeLeftFormat = LocManager.Instance.GetTable("main_menu_ui").GetRawText("DAILY_RUN_MENU.TIME_FORMAT");
  private MegaLabel _titleLabel;
  private MegaLabel _disclaimer;
  private MegaRichTextLabel _dateLabel;
  private MegaRichTextLabel _timeLeftLabel;
  private NDailyRunCharacterContainer _characterContainer;
  private NConfirmButton _embarkButton;
  private NBackButton _backButton;
  private NBackButton _unreadyButton;
  private NDailyRunLeaderboard _leaderboard;
  private MegaLabel _modifiersTitleLabel;
  private Control _modifiersContainer;
  private readonly List<NDailyRunScreenModifier> _modifierContainers = new List<NDailyRunScreenModifier>();
  private NRemoteLobbyPlayerContainer _remotePlayerContainer;
  private Control _readyAndWaitingContainer;
  private DateTimeOffset _endOfDay;
  private INetGameService _netService;
  private StartRunLobby? _lobby;
  private int? _lastSetTimeLeftSecond;

  public static string[] AssetPaths
  {
    get => new string[1]{ NDailyRunScreen._scenePath };
  }

  protected override Control? InitialFocusedControl => (Control) null;

  public static NDailyRunScreen? Create()
  {
    return TestMode.IsOn ? (NDailyRunScreen) null : PreloadManager.Cache.GetScene(NDailyRunScreen._scenePath).Instantiate<NDailyRunScreen>((PackedScene.GenEditState) 0L);
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._titleLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%Title"));
    this._disclaimer = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%Disclaimer"));
    this._dateLabel = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%Date"));
    this._embarkButton = ((Node) this).GetNode<NConfirmButton>(NodePath.op_Implicit("%ConfirmButton"));
    this._backButton = ((Node) this).GetNode<NBackButton>(NodePath.op_Implicit("%BackButton"));
    this._unreadyButton = ((Node) this).GetNode<NBackButton>(NodePath.op_Implicit("%UnreadyButton"));
    this._timeLeftLabel = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%TimeLeft"));
    this._leaderboard = ((Node) this).GetNode<NDailyRunLeaderboard>(NodePath.op_Implicit("%Leaderboards"));
    this._modifiersTitleLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%ModifiersLabel"));
    this._modifiersContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%ModifiersContainer"));
    this._characterContainer = ((Node) this).GetNode<NDailyRunCharacterContainer>(NodePath.op_Implicit("ChallengeContainer/CenterContainer/HBoxContainer/CharacterContainer"));
    this._remotePlayerContainer = ((Node) this).GetNode<NRemoteLobbyPlayerContainer>(NodePath.op_Implicit("%RemotePlayerContainer"));
    this._readyAndWaitingContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%ReadyAndWaitingPanel"));
    this._titleLabel.SetTextAutoSize(new LocString("main_menu_ui", "DAILY_RUN_MENU.DAILY_TITLE").GetFormattedText());
    this._disclaimer.SetTextAutoSize(new LocString("main_menu_ui", "DAILY_RUN_MENU.disclaimer").GetFormattedText());
    this._modifiersTitleLabel.SetTextAutoSize(new LocString("main_menu_ui", "DAILY_RUN_MENU.MODIFIERS").GetFormattedText());
    this._dateLabel.SetTextAutoSize(new LocString("main_menu_ui", "DAILY_RUN_MENU.FETCHING_TIME").GetFormattedText());
    foreach (NDailyRunScreenModifier runScreenModifier in ((IEnumerable) ((Node) this._modifiersContainer).GetChildren(false)).OfType<NDailyRunScreenModifier>())
      this._modifierContainers.Add(runScreenModifier);
    ((GodotObject) this._embarkButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnEmbarkPressed)), 0U);
    this._embarkButton.Disable();
    ((GodotObject) this._unreadyButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnUnreadyPressed)), 0U);
    this._unreadyButton.Disable();
    ((CanvasItem) this._remotePlayerContainer).Visible = false;
    ((CanvasItem) this._readyAndWaitingContainer).Visible = false;
    this._leaderboard.Cleanup();
  }

  public void InitializeMultiplayerAsHost(INetGameService gameService)
  {
    this._netService = gameService.Type == NetGameType.Host ? gameService : throw new InvalidOperationException($"Initialized character select screen with GameService of type {gameService.Type} when hosting!");
  }

  public void InitializeMultiplayerAsClient(
    INetGameService gameService,
    ClientLobbyJoinResponseMessage message)
  {
    this._netService = gameService.Type == NetGameType.Client ? gameService : throw new InvalidOperationException($"Initialized character select screen with GameService of type {gameService.Type} when joining!");
    this._lobby = new StartRunLobby(GameMode.Daily, gameService, (IStartRunLobbyListener) this, message.dailyTime.Value, -1);
    this._lobby.InitializeFromMessage(message);
    this.SetupLobbyParams(this._lobby);
    this.AfterLobbyInitialized();
  }

  public void InitializeSingleplayer()
  {
    this._netService = (INetGameService) new NetSingleplayerGameService();
  }

  public override void OnSubmenuOpened()
  {
    base.OnSubmenuOpened();
    bool flag;
    switch (this._netService.Type)
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
      TaskHelper.RunSafely(this.SetupLobbyForHostOrSingleplayer());
    else
      this.SetIsLoading(false);
  }

  public override void OnSubmenuClosed()
  {
    this._embarkButton.Disable();
    this._remotePlayerContainer.Cleanup();
    this._leaderboard.Cleanup();
    StartRunLobby lobby = this._lobby;
    if ((lobby != null ? (lobby.NetService.Type.IsMultiplayer() ? 1 : 0) : 0) != 0)
      PlatformUtil.SetRichPresence("MAIN_MENU", (string) null, new int?());
    this.CleanUpLobby(true);
  }

  private void InitializeLeaderboard()
  {
    this._leaderboard.Initialize(this._lobby.DailyTime.Value.serverTime, this._lobby.Players.Select<LobbyPlayer, ulong>((Func<LobbyPlayer, ulong>) (p => p.id)), false);
  }

  private async Task SetupLobbyForHostOrSingleplayer()
  {
    if (this._netService.Type != NetGameType.Host && this._netService.Type != NetGameType.Singleplayer)
      throw new InvalidOperationException("Should only be called as host or singleplayer!");
    this.SetIsLoading(true);
    TimeServerResult timeServerTime = await this.GetTimeServerTime();
    if (!GodotObject.IsInstanceValid((GodotObject) this))
      return;
    this._lobby = new StartRunLobby(GameMode.Daily, this._netService, (IStartRunLobbyListener) this, timeServerTime, 4);
    this._lobby.AddLocalHostPlayer(new UnlockState(SaveManager.Instance.Progress), SaveManager.Instance.Progress.MaxMultiplayerAscension);
    this.SetupLobbyParams(this._lobby);
    this.AfterLobbyInitialized();
    this.SetIsLoading(false);
    Log.Info($"Daily initialized with seed: {this._lobby.Seed} time: {this.GetServerRelativeTime()}");
  }

  private async Task<TimeServerResult> GetTimeServerTime()
  {
    TimeServerResult? result = new TimeServerResult?();
    Task<TimeServerResult?> requestTimeTask = TimeServer.RequestTimeTask;
    if ((requestTimeTask != null ? (((Task) requestTimeTask).IsCompleted ? 1 : 0) : 0) != 0)
    {
      if (!((Task) TimeServer.RequestTimeTask).IsFaulted)
        result = await TimeServer.RequestTimeTask;
      if (!result.HasValue)
      {
        try
        {
          result = await TimeServer.FetchDailyTime();
        }
        catch (Exception ex) when (
        {
          // ISSUE: unable to correctly present filter
          bool flag;
          switch (ex)
          {
            case HttpRequestException _:
            case TaskCanceledException _:
              flag = true;
              break;
            default:
              flag = false;
              break;
          }
          if (flag)
          {
            SuccessfulFiltering;
          }
          else
            throw;
        }
        )
        {
          Log.Error(ex.ToString());
        }
      }
    }
    else
    {
      try
      {
        result = await TimeServer.FetchDailyTime();
      }
      catch (Exception ex) when (
      {
        // ISSUE: unable to correctly present filter
        bool flag;
        switch (ex)
        {
          case HttpRequestException _:
          case TaskCanceledException _:
            flag = true;
            break;
          default:
            flag = false;
            break;
        }
        if (flag)
        {
          SuccessfulFiltering;
        }
        else
          throw;
      }
      )
      {
        Log.Error(ex.ToString());
      }
    }
    if (!result.HasValue)
    {
      Log.Info("Couldn't retrieve time from time server, using local time");
      result = new TimeServerResult?(new TimeServerResult()
      {
        serverTime = DateTimeOffset.UtcNow,
        localReceivedTime = DateTimeOffset.UtcNow
      });
    }
    return result.Value;
  }

  private DateTimeOffset GetServerRelativeTime()
  {
    TimeServerResult? dailyTime = this._lobby.DailyTime;
    DateTimeOffset serverTime = dailyTime.Value.serverTime;
    DateTimeOffset utcNow = DateTimeOffset.UtcNow;
    dailyTime = this._lobby.DailyTime;
    DateTimeOffset localReceivedTime = dailyTime.Value.localReceivedTime;
    TimeSpan timeSpan = utcNow - localReceivedTime;
    return serverTime + timeSpan;
  }

  private void SetupLobbyParams(StartRunLobby lobby)
  {
    DateTimeOffset serverRelativeTime = this.GetServerRelativeTime();
    string str = SeedHelper.CanonicalizeSeed(serverRelativeTime.ToString("dd_MM_yyyy"));
    // ISSUE: explicit reference operation
    string seed = SeedHelper.CanonicalizeSeed((^ref serverRelativeTime).ToString($"dd_MM_yyyy_{lobby.Players.Count}p"));
    Rng rng1 = new Rng(StringHelper.GetDeterministicHashCode(str));
    Rng rng2 = new Rng(rng1.NextUnsignedLong());
    Rng rng3 = new Rng(rng1.NextUnsignedLong());
    Rng rng4 = new Rng(rng1.NextUnsignedLong());
    CharacterModel character = (CharacterModel) null;
    foreach (LobbyPlayer player in lobby.Players)
    {
      CharacterModel characterModel = rng2.NextItem<CharacterModel>(ModelDb.AllCharacters);
      if ((long) player.id == (long) lobby.LocalPlayer.id)
        character = characterModel;
    }
    int ascension = rng3.NextInt(0, 11);
    IReadOnlyCollection<ModifierModel> modifierModels = ModifierModel.Pick2Good1Bad(rng4, this._lobby.Players.Select<LobbyPlayer, CharacterModel>((Func<LobbyPlayer, CharacterModel>) (p => p.character)));
    bool flag;
    switch (lobby.NetService.Type)
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
    {
      if (lobby.Seed != seed)
        lobby.SetSeed(seed);
      if (lobby.Ascension != ascension)
        lobby.SyncAscensionChange(ascension);
      if (modifierModels.Any<ModifierModel>((Func<ModifierModel, bool>) (m => lobby.Modifiers.FirstOrDefault<ModifierModel>(new Func<ModifierModel, bool>(m.IsEquivalent)) == null)))
        lobby.SetModifiers(modifierModels);
    }
    if (lobby.LocalPlayer.character != character)
      lobby.SetLocalCharacter(character);
    this.InitializeDisplay();
  }

  private void InitializeDisplay()
  {
    if (this._lobby == null)
      throw new InvalidOperationException("Tried to initialize daily run display before lobby was initialized!");
    DateTimeOffset serverRelativeTime = this.GetServerRelativeTime();
    this._endOfDay = new DateTimeOffset(serverRelativeTime.Year, serverRelativeTime.Month, serverRelativeTime.Day, 0, 0, 0, TimeSpan.Zero) + TimeSpan.FromDays(1);
    ((CanvasItem) this._remotePlayerContainer).Visible = this._lobby.NetService.Type.IsMultiplayer();
    this._characterContainer.Fill(this._lobby.LocalPlayer.character, this._lobby.LocalPlayer.id, this._lobby.Ascension, this._lobby.NetService);
    ((CanvasItem) this._dateLabel).Modulate = StsColors.blue;
    this._dateLabel.Text = serverRelativeTime.ToString(NDailyRunScreen.dateFormat);
    for (int index = 0; index < this._lobby.Modifiers.Count; ++index)
      this._modifierContainers[index].Fill(this._lobby.Modifiers[index]);
  }

  private void SetIsLoading(bool isLoading)
  {
    if (isLoading)
    {
      ((CanvasItem) this._remotePlayerContainer).Visible = false;
      ((CanvasItem) this._readyAndWaitingContainer).Visible = false;
    }
    ((CanvasItem) this._timeLeftLabel).Visible = !isLoading;
    ((CanvasItem) this._characterContainer).Visible = !isLoading;
    ((CanvasItem) this._modifiersTitleLabel).Visible = !isLoading;
    ((CanvasItem) this._modifiersContainer).Visible = !isLoading;
    if (isLoading)
      this._embarkButton.Disable();
    else
      this._embarkButton.Enable();
  }

  public override void _Process(double delta)
  {
    if (this._lobby == null)
      return;
    DateTimeOffset serverRelativeTime = this.GetServerRelativeTime();
    if (serverRelativeTime > this._endOfDay)
      this.SetupLobbyParams(this._lobby);
    TimeSpan timeSpan = this._endOfDay - serverRelativeTime;
    int? setTimeLeftSecond = this._lastSetTimeLeftSecond;
    int seconds = timeSpan.Seconds;
    if (!(setTimeLeftSecond.GetValueOrDefault() == seconds & setTimeLeftSecond.HasValue))
    {
      string variable = timeSpan.ToString(NDailyRunScreen._timeLeftFormat);
      NDailyRunScreen._timeLeftLoc.Add("time", variable);
      this._timeLeftLabel.Text = NDailyRunScreen._timeLeftLoc.GetFormattedText();
      this._lastSetTimeLeftSecond = new int?(timeSpan.Seconds);
    }
    if (!this._lobby.NetService.IsConnected)
      return;
    this._lobby.NetService.Update();
  }

  public void PlayerConnected(LobbyPlayer player)
  {
    this._remotePlayerContainer.OnPlayerConnected(player);
    this.SetupLobbyParams(this._lobby);
    this.InitializeLeaderboard();
    this.UpdateRichPresence();
  }

  public void PlayerChanged(LobbyPlayer player, bool isRandomCharacterResolution)
  {
    if (isRandomCharacterResolution)
      throw new InvalidOperationException("Random character is not currently allowed in daily!");
    this._remotePlayerContainer.OnPlayerChanged(player);
    if ((long) player.id != (long) this._netService.NetId || !this._netService.Type.IsMultiplayer())
      return;
    this._characterContainer.SetIsReady(player.isReady);
  }

  public void MaxAscensionChanged()
  {
  }

  public void AscensionChanged() => this.InitializeDisplay();

  public void SeedChanged()
  {
  }

  public void ModifiersChanged() => this.InitializeDisplay();

  public void RemotePlayerDisconnected(LobbyPlayer player)
  {
    this._remotePlayerContainer.OnPlayerDisconnected(player);
    this.SetupLobbyParams(this._lobby);
    this.InitializeLeaderboard();
    this.UpdateRichPresence();
  }

  public void BeginRun(string seed, List<ActModel> acts, IReadOnlyList<ModifierModel> modifiers)
  {
    NAudioManager.Instance?.StopMusic();
    this._embarkButton.Disable();
    this._unreadyButton.Disable();
    if (this._lobby.NetService.Type == NetGameType.Singleplayer)
      TaskHelper.RunSafely(this.StartNewSingleplayerRun(seed, acts, modifiers));
    else
      TaskHelper.RunSafely(this.StartNewMultiplayerRun(seed, acts, modifiers));
  }

  public void LocalPlayerDisconnected(NetErrorInfo info)
  {
    if (info.SelfInitiated && info.GetReason() == NetError.Quit || !((Node) this).IsValid() || this._stack == null)
      return;
    if (this._stack.Peek() == this)
      this._stack.Pop();
    if (!TestMode.IsOff)
      return;
    NErrorPopup modalToCreate = NErrorPopup.Create(info);
    if (modalToCreate == null)
      return;
    NModalContainer.Instance.Add((Node) modalToCreate);
  }

  private void OnEmbarkPressed(NButton _)
  {
    this._embarkButton.Disable();
    this._backButton.Disable();
    this._lobby.SetReady(true);
    if (this._lobby.NetService.Type == NetGameType.Singleplayer || this._lobby.IsAboutToBeginGame())
      return;
    ((CanvasItem) this._readyAndWaitingContainer).Visible = true;
    this._unreadyButton.Enable();
  }

  private void OnUnreadyPressed(NButton _)
  {
    this._lobby.SetReady(false);
    ((CanvasItem) this._readyAndWaitingContainer).Visible = false;
    this._embarkButton.Enable();
    this._backButton.Enable();
    this._unreadyButton.Disable();
  }

  private void UpdateRichPresence()
  {
    StartRunLobby lobby = this._lobby;
    if ((lobby != null ? (lobby.NetService.Type.IsMultiplayer() ? 1 : 0) : 0) == 0)
      return;
    PlatformUtil.SetRichPresence("DAILY_MP_LOBBY", this._lobby.NetService.GetRawLobbyIdentifier(), new int?(this._lobby.Players.Count));
  }

  public async Task StartNewSingleplayerRun(
    string seed,
    List<ActModel> acts,
    IReadOnlyList<ModifierModel> modifiers)
  {
    int num = 0;
    object obj;
    try
    {
      Log.Info($"Embarking on a DAILY {this._lobby.LocalPlayer.character.Id.Entry} run with {this._lobby.Players.Count} players. Ascension: {this._lobby.Ascension} Seed: {seed}");
      SfxCmd.Play(this._lobby.LocalPlayer.character.CharacterTransitionSfx);
      await NGame.Instance.Transition.FadeOut(transitionPath: this._lobby.LocalPlayer.character.CharacterSelectTransitionPath);
      RunState runState = await NGame.Instance.StartNewSingleplayerRun(this._lobby.LocalPlayer.character, true, (IReadOnlyList<ActModel>) acts, modifiers, seed, GameMode.Daily, this._lobby.Ascension, new DateTimeOffset?(this._lobby.DailyTime.Value.serverTime));
    }
    catch (Exception ex)
    {
      obj = (object) ex;
      num = 1;
    }
    if (num == 1)
    {
      Exception e = (Exception) obj;
      Log.Error($"Exception starting daily singleplayer run : {e}");
      this.CleanUpLobby(true, NetError.InternalError);
      await NGame.Instance.ReturnToMainMenuWithInternalError(e);
    }
    else
    {
      obj = (object) null;
      this.CleanUpLobby(false);
    }
  }

  public async Task StartNewMultiplayerRun(
    string seed,
    List<ActModel> acts,
    IReadOnlyList<ModifierModel> modifiers)
  {
    int num = 0;
    object obj;
    try
    {
      Log.Info($"Embarking on a DAILY multiplayer run. Players: {string.Join<LobbyPlayer>(",", (IEnumerable<LobbyPlayer>) this._lobby.Players)}. Ascension: {this._lobby.Ascension} Seed: {seed}");
      SfxCmd.Play(this._lobby.LocalPlayer.character.CharacterTransitionSfx);
      await NGame.Instance.Transition.FadeOut(transitionPath: this._lobby.LocalPlayer.character.CharacterSelectTransitionPath);
      RunState runState = await NGame.Instance.StartNewMultiplayerRun(this._lobby, true, (IReadOnlyList<ActModel>) acts, modifiers, seed, this._lobby.Ascension, new DateTimeOffset?(this._lobby.DailyTime.Value.serverTime));
    }
    catch (Exception ex)
    {
      obj = (object) ex;
      num = 1;
    }
    if (num == 1)
    {
      Exception e = (Exception) obj;
      Log.Error($"Exception starting daily multiplayer run : {e}");
      this.CleanUpLobby(true, NetError.InternalError);
      await NGame.Instance.ReturnToMainMenuWithInternalError(e);
    }
    else
    {
      obj = (object) null;
      this.CleanUpLobby(false);
    }
  }

  private void CleanUpLobby(bool disconnectSession, NetError error = NetError.Quit)
  {
    this._lobby?.CleanUp(disconnectSession, error);
    this._lobby = (StartRunLobby) null;
  }

  private void AfterLobbyInitialized()
  {
    NGame.Instance.RemoteCursorContainer.Initialize(this._lobby.InputSynchronizer, this._lobby.Players.Select<LobbyPlayer, ulong>((Func<LobbyPlayer, ulong>) (p => p.id)));
    NGame.Instance.ReactionContainer.InitializeNetworking(this._lobby.NetService);
    NGame.Instance.TimeoutOverlay.Initialize(this._lobby.NetService, true);
    this._remotePlayerContainer.Initialize(this._lobby, false);
    this.UpdateRichPresence();
    Logger.logLevelTypeMap[LogType.Network] = this._lobby.NetService.Type == NetGameType.Singleplayer ? LogLevel.Info : LogLevel.Debug;
    Logger.logLevelTypeMap[LogType.Actions] = this._lobby.NetService.Type == NetGameType.Singleplayer ? LogLevel.Info : LogLevel.VeryDebug;
    Logger.logLevelTypeMap[LogType.GameSync] = this._lobby.NetService.Type == NetGameType.Singleplayer ? LogLevel.Info : LogLevel.VeryDebug;
    NGame.Instance.DebugSeedOverride = (string) null;
    this._embarkButton.Enable();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(18)
    {
      new MethodInfo(NDailyRunScreen.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDailyRunScreen.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDailyRunScreen.MethodName.InitializeSingleplayer, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDailyRunScreen.MethodName.OnSubmenuOpened, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDailyRunScreen.MethodName.OnSubmenuClosed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDailyRunScreen.MethodName.InitializeLeaderboard, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDailyRunScreen.MethodName.InitializeDisplay, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDailyRunScreen.MethodName.SetIsLoading, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("isLoading"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NDailyRunScreen.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NDailyRunScreen.MethodName.MaxAscensionChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDailyRunScreen.MethodName.AscensionChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDailyRunScreen.MethodName.SeedChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDailyRunScreen.MethodName.ModifiersChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDailyRunScreen.MethodName.OnEmbarkPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NDailyRunScreen.MethodName.OnUnreadyPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NDailyRunScreen.MethodName.UpdateRichPresence, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDailyRunScreen.MethodName.CleanUpLobby, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("disconnectSession"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("error"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NDailyRunScreen.MethodName.AfterLobbyInitialized, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NDailyRunScreen.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NDailyRunScreen ndailyRunScreen = NDailyRunScreen.Create();
      ret = VariantUtils.CreateFrom<NDailyRunScreen>(ref ndailyRunScreen);
      return true;
    }
    if (StringName.op_Equality(ref method, NDailyRunScreen.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDailyRunScreen.MethodName.InitializeSingleplayer) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.InitializeSingleplayer();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDailyRunScreen.MethodName.OnSubmenuOpened) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSubmenuOpened();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDailyRunScreen.MethodName.OnSubmenuClosed) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSubmenuClosed();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDailyRunScreen.MethodName.InitializeLeaderboard) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.InitializeLeaderboard();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDailyRunScreen.MethodName.InitializeDisplay) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.InitializeDisplay();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDailyRunScreen.MethodName.SetIsLoading) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetIsLoading(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDailyRunScreen.MethodName._Process) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDailyRunScreen.MethodName.MaxAscensionChanged) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.MaxAscensionChanged();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDailyRunScreen.MethodName.AscensionChanged) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AscensionChanged();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDailyRunScreen.MethodName.SeedChanged) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SeedChanged();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDailyRunScreen.MethodName.ModifiersChanged) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ModifiersChanged();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDailyRunScreen.MethodName.OnEmbarkPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnEmbarkPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDailyRunScreen.MethodName.OnUnreadyPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnUnreadyPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDailyRunScreen.MethodName.UpdateRichPresence) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateRichPresence();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDailyRunScreen.MethodName.CleanUpLobby) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.CleanUpLobby(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<NetError>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NDailyRunScreen.MethodName.AfterLobbyInitialized) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.AfterLobbyInitialized();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NDailyRunScreen.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NDailyRunScreen ndailyRunScreen = NDailyRunScreen.Create();
      ret = VariantUtils.CreateFrom<NDailyRunScreen>(ref ndailyRunScreen);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NDailyRunScreen.MethodName.Create) || StringName.op_Equality(ref method, NDailyRunScreen.MethodName._Ready) || StringName.op_Equality(ref method, NDailyRunScreen.MethodName.InitializeSingleplayer) || StringName.op_Equality(ref method, NDailyRunScreen.MethodName.OnSubmenuOpened) || StringName.op_Equality(ref method, NDailyRunScreen.MethodName.OnSubmenuClosed) || StringName.op_Equality(ref method, NDailyRunScreen.MethodName.InitializeLeaderboard) || StringName.op_Equality(ref method, NDailyRunScreen.MethodName.InitializeDisplay) || StringName.op_Equality(ref method, NDailyRunScreen.MethodName.SetIsLoading) || StringName.op_Equality(ref method, NDailyRunScreen.MethodName._Process) || StringName.op_Equality(ref method, NDailyRunScreen.MethodName.MaxAscensionChanged) || StringName.op_Equality(ref method, NDailyRunScreen.MethodName.AscensionChanged) || StringName.op_Equality(ref method, NDailyRunScreen.MethodName.SeedChanged) || StringName.op_Equality(ref method, NDailyRunScreen.MethodName.ModifiersChanged) || StringName.op_Equality(ref method, NDailyRunScreen.MethodName.OnEmbarkPressed) || StringName.op_Equality(ref method, NDailyRunScreen.MethodName.OnUnreadyPressed) || StringName.op_Equality(ref method, NDailyRunScreen.MethodName.UpdateRichPresence) || StringName.op_Equality(ref method, NDailyRunScreen.MethodName.CleanUpLobby) || StringName.op_Equality(ref method, NDailyRunScreen.MethodName.AfterLobbyInitialized) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDailyRunScreen.PropertyName._titleLabel))
    {
      this._titleLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunScreen.PropertyName._disclaimer))
    {
      this._disclaimer = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunScreen.PropertyName._dateLabel))
    {
      this._dateLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunScreen.PropertyName._timeLeftLabel))
    {
      this._timeLeftLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunScreen.PropertyName._characterContainer))
    {
      this._characterContainer = VariantUtils.ConvertTo<NDailyRunCharacterContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunScreen.PropertyName._embarkButton))
    {
      this._embarkButton = VariantUtils.ConvertTo<NConfirmButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunScreen.PropertyName._backButton))
    {
      this._backButton = VariantUtils.ConvertTo<NBackButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunScreen.PropertyName._unreadyButton))
    {
      this._unreadyButton = VariantUtils.ConvertTo<NBackButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunScreen.PropertyName._leaderboard))
    {
      this._leaderboard = VariantUtils.ConvertTo<NDailyRunLeaderboard>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunScreen.PropertyName._modifiersTitleLabel))
    {
      this._modifiersTitleLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunScreen.PropertyName._modifiersContainer))
    {
      this._modifiersContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunScreen.PropertyName._remotePlayerContainer))
    {
      this._remotePlayerContainer = VariantUtils.ConvertTo<NRemoteLobbyPlayerContainer>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDailyRunScreen.PropertyName._readyAndWaitingContainer))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._readyAndWaitingContainer = VariantUtils.ConvertTo<Control>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDailyRunScreen.PropertyName.InitialFocusedControl))
    {
      ref godot_variant local = ref value;
      Control initialFocusedControl = this.InitialFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref initialFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunScreen.PropertyName._titleLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._titleLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunScreen.PropertyName._disclaimer))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._disclaimer);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunScreen.PropertyName._dateLabel))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._dateLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunScreen.PropertyName._timeLeftLabel))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._timeLeftLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunScreen.PropertyName._characterContainer))
    {
      value = VariantUtils.CreateFrom<NDailyRunCharacterContainer>(ref this._characterContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunScreen.PropertyName._embarkButton))
    {
      value = VariantUtils.CreateFrom<NConfirmButton>(ref this._embarkButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunScreen.PropertyName._backButton))
    {
      value = VariantUtils.CreateFrom<NBackButton>(ref this._backButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunScreen.PropertyName._unreadyButton))
    {
      value = VariantUtils.CreateFrom<NBackButton>(ref this._unreadyButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunScreen.PropertyName._leaderboard))
    {
      value = VariantUtils.CreateFrom<NDailyRunLeaderboard>(ref this._leaderboard);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunScreen.PropertyName._modifiersTitleLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._modifiersTitleLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunScreen.PropertyName._modifiersContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._modifiersContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunScreen.PropertyName._remotePlayerContainer))
    {
      value = VariantUtils.CreateFrom<NRemoteLobbyPlayerContainer>(ref this._remotePlayerContainer);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDailyRunScreen.PropertyName._readyAndWaitingContainer))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Control>(ref this._readyAndWaitingContainer);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NDailyRunScreen.PropertyName.InitialFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDailyRunScreen.PropertyName._titleLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDailyRunScreen.PropertyName._disclaimer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDailyRunScreen.PropertyName._dateLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDailyRunScreen.PropertyName._timeLeftLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDailyRunScreen.PropertyName._characterContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDailyRunScreen.PropertyName._embarkButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDailyRunScreen.PropertyName._backButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDailyRunScreen.PropertyName._unreadyButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDailyRunScreen.PropertyName._leaderboard, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDailyRunScreen.PropertyName._modifiersTitleLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDailyRunScreen.PropertyName._modifiersContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDailyRunScreen.PropertyName._remotePlayerContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDailyRunScreen.PropertyName._readyAndWaitingContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NDailyRunScreen.PropertyName._titleLabel, Variant.From<MegaLabel>(ref this._titleLabel));
    info.AddProperty(NDailyRunScreen.PropertyName._disclaimer, Variant.From<MegaLabel>(ref this._disclaimer));
    info.AddProperty(NDailyRunScreen.PropertyName._dateLabel, Variant.From<MegaRichTextLabel>(ref this._dateLabel));
    info.AddProperty(NDailyRunScreen.PropertyName._timeLeftLabel, Variant.From<MegaRichTextLabel>(ref this._timeLeftLabel));
    info.AddProperty(NDailyRunScreen.PropertyName._characterContainer, Variant.From<NDailyRunCharacterContainer>(ref this._characterContainer));
    info.AddProperty(NDailyRunScreen.PropertyName._embarkButton, Variant.From<NConfirmButton>(ref this._embarkButton));
    info.AddProperty(NDailyRunScreen.PropertyName._backButton, Variant.From<NBackButton>(ref this._backButton));
    info.AddProperty(NDailyRunScreen.PropertyName._unreadyButton, Variant.From<NBackButton>(ref this._unreadyButton));
    info.AddProperty(NDailyRunScreen.PropertyName._leaderboard, Variant.From<NDailyRunLeaderboard>(ref this._leaderboard));
    info.AddProperty(NDailyRunScreen.PropertyName._modifiersTitleLabel, Variant.From<MegaLabel>(ref this._modifiersTitleLabel));
    info.AddProperty(NDailyRunScreen.PropertyName._modifiersContainer, Variant.From<Control>(ref this._modifiersContainer));
    info.AddProperty(NDailyRunScreen.PropertyName._remotePlayerContainer, Variant.From<NRemoteLobbyPlayerContainer>(ref this._remotePlayerContainer));
    info.AddProperty(NDailyRunScreen.PropertyName._readyAndWaitingContainer, Variant.From<Control>(ref this._readyAndWaitingContainer));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NDailyRunScreen.PropertyName._titleLabel, ref variant1))
      this._titleLabel = ((Variant) ref variant1).As<MegaLabel>();
    Variant variant2;
    if (info.TryGetProperty(NDailyRunScreen.PropertyName._disclaimer, ref variant2))
      this._disclaimer = ((Variant) ref variant2).As<MegaLabel>();
    Variant variant3;
    if (info.TryGetProperty(NDailyRunScreen.PropertyName._dateLabel, ref variant3))
      this._dateLabel = ((Variant) ref variant3).As<MegaRichTextLabel>();
    Variant variant4;
    if (info.TryGetProperty(NDailyRunScreen.PropertyName._timeLeftLabel, ref variant4))
      this._timeLeftLabel = ((Variant) ref variant4).As<MegaRichTextLabel>();
    Variant variant5;
    if (info.TryGetProperty(NDailyRunScreen.PropertyName._characterContainer, ref variant5))
      this._characterContainer = ((Variant) ref variant5).As<NDailyRunCharacterContainer>();
    Variant variant6;
    if (info.TryGetProperty(NDailyRunScreen.PropertyName._embarkButton, ref variant6))
      this._embarkButton = ((Variant) ref variant6).As<NConfirmButton>();
    Variant variant7;
    if (info.TryGetProperty(NDailyRunScreen.PropertyName._backButton, ref variant7))
      this._backButton = ((Variant) ref variant7).As<NBackButton>();
    Variant variant8;
    if (info.TryGetProperty(NDailyRunScreen.PropertyName._unreadyButton, ref variant8))
      this._unreadyButton = ((Variant) ref variant8).As<NBackButton>();
    Variant variant9;
    if (info.TryGetProperty(NDailyRunScreen.PropertyName._leaderboard, ref variant9))
      this._leaderboard = ((Variant) ref variant9).As<NDailyRunLeaderboard>();
    Variant variant10;
    if (info.TryGetProperty(NDailyRunScreen.PropertyName._modifiersTitleLabel, ref variant10))
      this._modifiersTitleLabel = ((Variant) ref variant10).As<MegaLabel>();
    Variant variant11;
    if (info.TryGetProperty(NDailyRunScreen.PropertyName._modifiersContainer, ref variant11))
      this._modifiersContainer = ((Variant) ref variant11).As<Control>();
    Variant variant12;
    if (info.TryGetProperty(NDailyRunScreen.PropertyName._remotePlayerContainer, ref variant12))
      this._remotePlayerContainer = ((Variant) ref variant12).As<NRemoteLobbyPlayerContainer>();
    Variant variant13;
    if (!info.TryGetProperty(NDailyRunScreen.PropertyName._readyAndWaitingContainer, ref variant13))
      return;
    this._readyAndWaitingContainer = ((Variant) ref variant13).As<Control>();
  }

  public new class MethodName : NSubmenu.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName InitializeSingleplayer = StringName.op_Implicit(nameof (InitializeSingleplayer));
    public new static readonly StringName OnSubmenuOpened = StringName.op_Implicit(nameof (OnSubmenuOpened));
    public new static readonly StringName OnSubmenuClosed = StringName.op_Implicit(nameof (OnSubmenuClosed));
    public static readonly StringName InitializeLeaderboard = StringName.op_Implicit(nameof (InitializeLeaderboard));
    public static readonly StringName InitializeDisplay = StringName.op_Implicit(nameof (InitializeDisplay));
    public static readonly StringName SetIsLoading = StringName.op_Implicit(nameof (SetIsLoading));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
    public static readonly StringName MaxAscensionChanged = StringName.op_Implicit(nameof (MaxAscensionChanged));
    public static readonly StringName AscensionChanged = StringName.op_Implicit(nameof (AscensionChanged));
    public static readonly StringName SeedChanged = StringName.op_Implicit(nameof (SeedChanged));
    public static readonly StringName ModifiersChanged = StringName.op_Implicit(nameof (ModifiersChanged));
    public static readonly StringName OnEmbarkPressed = StringName.op_Implicit(nameof (OnEmbarkPressed));
    public static readonly StringName OnUnreadyPressed = StringName.op_Implicit(nameof (OnUnreadyPressed));
    public static readonly StringName UpdateRichPresence = StringName.op_Implicit(nameof (UpdateRichPresence));
    public static readonly StringName CleanUpLobby = StringName.op_Implicit(nameof (CleanUpLobby));
    public static readonly StringName AfterLobbyInitialized = StringName.op_Implicit(nameof (AfterLobbyInitialized));
  }

  public new class PropertyName : NSubmenu.PropertyName
  {
    public new static readonly StringName InitialFocusedControl = StringName.op_Implicit(nameof (InitialFocusedControl));
    public static readonly StringName _titleLabel = StringName.op_Implicit(nameof (_titleLabel));
    public static readonly StringName _disclaimer = StringName.op_Implicit(nameof (_disclaimer));
    public static readonly StringName _dateLabel = StringName.op_Implicit(nameof (_dateLabel));
    public static readonly StringName _timeLeftLabel = StringName.op_Implicit(nameof (_timeLeftLabel));
    public static readonly StringName _characterContainer = StringName.op_Implicit(nameof (_characterContainer));
    public static readonly StringName _embarkButton = StringName.op_Implicit(nameof (_embarkButton));
    public new static readonly StringName _backButton = StringName.op_Implicit(nameof (_backButton));
    public static readonly StringName _unreadyButton = StringName.op_Implicit(nameof (_unreadyButton));
    public static readonly StringName _leaderboard = StringName.op_Implicit(nameof (_leaderboard));
    public static readonly StringName _modifiersTitleLabel = StringName.op_Implicit(nameof (_modifiersTitleLabel));
    public static readonly StringName _modifiersContainer = StringName.op_Implicit(nameof (_modifiersContainer));
    public static readonly StringName _remotePlayerContainer = StringName.op_Implicit(nameof (_remotePlayerContainer));
    public static readonly StringName _readyAndWaitingContainer = StringName.op_Implicit(nameof (_readyAndWaitingContainer));
  }

  public new class SignalName : NSubmenu.SignalName
  {
  }
}
