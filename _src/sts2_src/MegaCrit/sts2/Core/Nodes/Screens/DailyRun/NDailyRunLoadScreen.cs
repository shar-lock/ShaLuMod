// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.DailyRun.NDailyRunLoadScreen
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Game.Lobby;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.DailyRun;

[ScriptPath("res://src/Core/Nodes/Screens/DailyRun/NDailyRunLoadScreen.cs")]
public class NDailyRunLoadScreen : NSubmenu, ILoadRunLobbyListener
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("screens/daily_run/daily_run_load_screen");
  private static readonly LocString _ascensionLoc = new LocString("main_menu_ui", "DAILY_RUN_MENU.ASCENSION");
  public static readonly string dateFormat = LocManager.Instance.GetTable("main_menu_ui").GetRawText("DAILY_RUN_MENU.DATE_FORMAT");
  private MegaRichTextLabel _dateLabel;
  private NConfirmButton _embarkButton;
  private NBackButton _backButton;
  private NBackButton _unreadyButton;
  private NDailyRunCharacterContainer _characterContainer;
  private NDailyRunLeaderboard _leaderboard;
  private MegaLabel _modifiersTitleLabel;
  private Control _modifiersContainer;
  private readonly List<NDailyRunScreenModifier> _modifierContainers = new List<NDailyRunScreenModifier>();
  private NRemoteLoadLobbyPlayerContainer _remotePlayerContainer;
  private Control _readyAndWaitingContainer;
  private LoadRunLobby? _lobby;

  public static string[] AssetPaths
  {
    get => new string[1]{ NDailyRunLoadScreen._scenePath };
  }

  protected override Control? InitialFocusedControl => (Control) null;

  public static NDailyRunLoadScreen? Create()
  {
    return TestMode.IsOn ? (NDailyRunLoadScreen) null : PreloadManager.Cache.GetScene(NDailyRunLoadScreen._scenePath).Instantiate<NDailyRunLoadScreen>((PackedScene.GenEditState) 0L);
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._embarkButton = ((Node) this).GetNode<NConfirmButton>(NodePath.op_Implicit("%ConfirmButton"));
    this._backButton = ((Node) this).GetNode<NBackButton>(NodePath.op_Implicit("%BackButton"));
    this._unreadyButton = ((Node) this).GetNode<NBackButton>(NodePath.op_Implicit("%UnreadyButton"));
    this._dateLabel = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%Date"));
    this._leaderboard = ((Node) this).GetNode<NDailyRunLeaderboard>(NodePath.op_Implicit("%Leaderboards"));
    this._modifiersTitleLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%ModifiersLabel"));
    this._modifiersContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%ModifiersContainer"));
    this._characterContainer = ((Node) this).GetNode<NDailyRunCharacterContainer>(NodePath.op_Implicit("ChallengeContainer/CenterContainer/HBoxContainer/CharacterContainer"));
    this._remotePlayerContainer = ((Node) this).GetNode<NRemoteLoadLobbyPlayerContainer>(NodePath.op_Implicit("%RemotePlayerLoadContainer"));
    this._readyAndWaitingContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%ReadyAndWaitingPanel"));
    foreach (NDailyRunScreenModifier runScreenModifier in ((IEnumerable) ((Node) this._modifiersContainer).GetChildren(false)).OfType<NDailyRunScreenModifier>())
      this._modifierContainers.Add(runScreenModifier);
    ((CanvasItem) this._readyAndWaitingContainer).Visible = false;
    ((GodotObject) this._embarkButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnEmbarkPressed)), 0U);
    ((GodotObject) this._unreadyButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnUnreadyPressed)), 0U);
    this._unreadyButton.Disable();
    this._leaderboard.Cleanup();
  }

  public void InitializeAsHost(INetGameService gameService, SerializableRun run)
  {
    this._lobby = gameService.Type == NetGameType.Host ? new LoadRunLobby(gameService, (ILoadRunLobbyListener) this, run) : throw new InvalidOperationException($"Initialized daily run load screen with net service of type {gameService.Type} when hosting!");
    try
    {
      this._lobby.AddLocalHostPlayer();
      this.AfterMultiplayerStarted();
    }
    catch
    {
      this.CleanUpLobby(true, NetError.InternalError);
      throw;
    }
  }

  public void InitializeAsClient(INetGameService gameService, ClientLoadJoinResponseMessage message)
  {
    this._lobby = gameService.Type == NetGameType.Client ? new LoadRunLobby(gameService, (ILoadRunLobbyListener) this, message) : throw new InvalidOperationException($"Initialized daily run load screen with net service of type {gameService.Type} when joining!");
    this.AfterMultiplayerStarted();
  }

  public override void OnSubmenuOpened()
  {
    base.OnSubmenuOpened();
    this._leaderboard.Initialize(this._lobby.Run.DailyTime.Value, this._lobby.Run.Players.Select<SerializablePlayer, ulong>((Func<SerializablePlayer, ulong>) (p => p.NetId)), true);
    this._embarkButton.Enable();
    this._remotePlayerContainer.Initialize(this._lobby, false);
  }

  public override void OnSubmenuClosed()
  {
    this._embarkButton.Disable();
    this._remotePlayerContainer.Cleanup();
    this._leaderboard.Cleanup();
    LoadRunLobby lobby = this._lobby;
    if ((lobby != null ? (lobby.NetService.Type.IsMultiplayer() ? 1 : 0) : 0) != 0)
      PlatformUtil.SetRichPresence("MAIN_MENU", (string) null, new int?());
    this.CleanUpLobby(true);
  }

  private void InitializeDisplay()
  {
    if (this._lobby == null)
      throw new InvalidOperationException("Tried to initialize daily run display before lobby was initialized!");
    NDailyRunLoadScreen._ascensionLoc.Add("ascension", (Decimal) this._lobby.Run.Ascension);
    DateTimeOffset dateTimeOffset = this._lobby.Run.DailyTime.Value;
    SerializablePlayer serializablePlayer = this._lobby.Run.Players.First<SerializablePlayer>((Func<SerializablePlayer, bool>) (p => (long) p.NetId == (long) this._lobby.NetService.NetId));
    this._characterContainer.Fill(ModelDb.GetById<CharacterModel>(serializablePlayer.CharacterId), serializablePlayer.NetId, this._lobby.Run.Ascension, this._lobby.NetService);
    this._dateLabel.SetTextAutoSize(dateTimeOffset.ToString(NDailyRunLoadScreen.dateFormat));
    this._embarkButton.Enable();
    for (int index = 0; index < this._lobby.Run.Modifiers.Count; ++index)
    {
      ModifierModel modifier = ModifierModel.FromSerializable(this._lobby.Run.Modifiers[index]);
      this._modifierContainers[index].Fill(modifier);
    }
  }

  private void OnEmbarkPressed(NButton _)
  {
    this._embarkButton.Disable();
    this._backButton.Disable();
    this._lobby.SetReady(true);
    if (this._lobby.IsAboutToBeginGame())
      return;
    ((CanvasItem) this._readyAndWaitingContainer).Visible = true;
    this._unreadyButton.Enable();
  }

  private void OnUnreadyPressed(NButton _)
  {
    this._embarkButton.Enable();
    this._unreadyButton.Disable();
    this._backButton.Enable();
    this._lobby.SetReady(true);
    ((CanvasItem) this._readyAndWaitingContainer).Visible = false;
  }

  private void UpdateRichPresence()
  {
    LoadRunLobby lobby = this._lobby;
    if ((lobby != null ? (lobby.NetService.Type.IsMultiplayer() ? 1 : 0) : 0) == 0)
      return;
    PlatformUtil.SetRichPresence("LOADING_MP_LOBBY", this._lobby.NetService.GetRawLobbyIdentifier(), new int?(this._lobby.ConnectedPlayerIds.Count));
  }

  public override void _Process(double delta)
  {
    LoadRunLobby lobby = this._lobby;
    if ((lobby != null ? (lobby.NetService.IsConnected ? 1 : 0) : 0) == 0)
      return;
    this._lobby.NetService.Update();
  }

  private void CleanUpLobby(bool disconnectSession, NetError error = NetError.Quit)
  {
    this._lobby.CleanUp(disconnectSession, error);
    this._lobby = (LoadRunLobby) null;
  }

  public async Task<bool> ShouldAllowRunToBegin()
  {
    if (this._lobby.ConnectedPlayerIds.Count >= this._lobby.Run.Players.Count)
      return true;
    LocString body = new LocString("gameplay_ui", "CONFIRM_LOAD_SAVE.body");
    body.Add("MissingCount", (Decimal) (this._lobby.Run.Players.Count - this._lobby.ConnectedPlayerIds.Count));
    NGenericPopup modalToCreate = NGenericPopup.Create();
    NModalContainer.Instance.Add((Node) modalToCreate);
    return await modalToCreate.WaitForConfirmation(body, new LocString("gameplay_ui", "CONFIRM_LOAD_SAVE.header"), new LocString("gameplay_ui", "CONFIRM_LOAD_SAVE.cancel"), new LocString("gameplay_ui", "CONFIRM_LOAD_SAVE.confirm"));
  }

  private async Task StartRun()
  {
    int num = 0;
    object obj;
    try
    {
      Log.Info($"Loading a multiplayer run. Players: {string.Join<ulong>(",", (IEnumerable<ulong>) this._lobby.ConnectedPlayerIds)}.");
      SerializablePlayer serializablePlayer = this._lobby.Run.Players.First<SerializablePlayer>((Func<SerializablePlayer, bool>) (p => (long) p.NetId == (long) this._lobby.NetService.NetId));
      SfxCmd.Play(ModelDb.GetById<CharacterModel>(serializablePlayer.CharacterId).CharacterTransitionSfx);
      await NGame.Instance.Transition.FadeOut(transitionPath: ModelDb.GetById<CharacterModel>(serializablePlayer.CharacterId).CharacterSelectTransitionPath);
      RunState runState = RunState.FromSerializable(this._lobby.Run);
      await RunManager.Instance.SetUpSavedMultiplayer(runState, this._lobby);
      await NGame.Instance.LoadRun(runState, this._lobby.Run.PreFinishedRoom);
      runState = (RunState) null;
    }
    catch (Exception ex)
    {
      obj = (object) ex;
      num = 1;
    }
    if (num == 1)
    {
      Exception e = (Exception) obj;
      Log.Error($"Exception loading daily multiplayer run : {e}");
      this.CleanUpLobby(true, NetError.InternalError);
      await NGame.Instance.ReturnToMainMenuWithInternalError(e);
    }
    else
    {
      obj = (object) null;
      this.CleanUpLobby(false);
      await NGame.Instance.Transition.FadeIn();
    }
  }

  public void PlayerConnected(ulong playerId)
  {
    Log.Info($"Player connected: {playerId}");
    this._remotePlayerContainer.OnPlayerConnected(playerId);
    this.UpdateRichPresence();
  }

  public void PlayerReadyChanged(ulong playerId)
  {
    Log.Info($"Player ready changed: {playerId}");
    this._remotePlayerContainer.OnPlayerChanged(playerId);
    if ((long) playerId == (long) this._lobby.NetService.NetId && !this._lobby.IsPlayerReady(playerId))
      this._embarkButton.Enable();
    if ((long) playerId != (long) this._lobby.NetService.NetId || !this._lobby.NetService.Type.IsMultiplayer())
      return;
    this._characterContainer.SetIsReady(this._lobby.IsPlayerReady(playerId));
  }

  public void RemotePlayerDisconnected(ulong playerId)
  {
    Log.Info($"Player disconnected: {playerId}");
    this._remotePlayerContainer.OnPlayerDisconnected(playerId);
    this.UpdateRichPresence();
  }

  public void BeginRun()
  {
    NAudioManager.Instance?.StopMusic();
    this._embarkButton.Disable();
    this._unreadyButton.Disable();
    TaskHelper.RunSafely(this.StartRun());
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

  private void AfterMultiplayerStarted()
  {
    NGame.Instance.RemoteCursorContainer.Initialize(this._lobby.InputSynchronizer, (IEnumerable<ulong>) this._lobby.ConnectedPlayerIds);
    NGame.Instance.ReactionContainer.InitializeNetworking(this._lobby.NetService);
    this.InitializeDisplay();
    this.UpdateRichPresence();
    Logger.logLevelTypeMap[LogType.Network] = LogLevel.Debug;
    Logger.logLevelTypeMap[LogType.Actions] = LogLevel.VeryDebug;
    Logger.logLevelTypeMap[LogType.GameSync] = LogLevel.VeryDebug;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(15)
    {
      new MethodInfo(NDailyRunLoadScreen.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDailyRunLoadScreen.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDailyRunLoadScreen.MethodName.OnSubmenuOpened, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDailyRunLoadScreen.MethodName.OnSubmenuClosed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDailyRunLoadScreen.MethodName.InitializeDisplay, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDailyRunLoadScreen.MethodName.OnEmbarkPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NDailyRunLoadScreen.MethodName.OnUnreadyPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NDailyRunLoadScreen.MethodName.UpdateRichPresence, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDailyRunLoadScreen.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NDailyRunLoadScreen.MethodName.CleanUpLobby, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("disconnectSession"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("error"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NDailyRunLoadScreen.MethodName.PlayerConnected, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("playerId"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NDailyRunLoadScreen.MethodName.PlayerReadyChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("playerId"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NDailyRunLoadScreen.MethodName.RemotePlayerDisconnected, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("playerId"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NDailyRunLoadScreen.MethodName.BeginRun, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDailyRunLoadScreen.MethodName.AfterMultiplayerStarted, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NDailyRunLoadScreen.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NDailyRunLoadScreen ndailyRunLoadScreen = NDailyRunLoadScreen.Create();
      ret = VariantUtils.CreateFrom<NDailyRunLoadScreen>(ref ndailyRunLoadScreen);
      return true;
    }
    if (StringName.op_Equality(ref method, NDailyRunLoadScreen.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDailyRunLoadScreen.MethodName.OnSubmenuOpened) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSubmenuOpened();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDailyRunLoadScreen.MethodName.OnSubmenuClosed) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSubmenuClosed();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDailyRunLoadScreen.MethodName.InitializeDisplay) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.InitializeDisplay();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDailyRunLoadScreen.MethodName.OnEmbarkPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnEmbarkPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDailyRunLoadScreen.MethodName.OnUnreadyPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnUnreadyPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDailyRunLoadScreen.MethodName.UpdateRichPresence) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateRichPresence();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDailyRunLoadScreen.MethodName._Process) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDailyRunLoadScreen.MethodName.CleanUpLobby) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.CleanUpLobby(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<NetError>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDailyRunLoadScreen.MethodName.PlayerConnected) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.PlayerConnected(VariantUtils.ConvertTo<ulong>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDailyRunLoadScreen.MethodName.PlayerReadyChanged) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.PlayerReadyChanged(VariantUtils.ConvertTo<ulong>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDailyRunLoadScreen.MethodName.RemotePlayerDisconnected) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.RemotePlayerDisconnected(VariantUtils.ConvertTo<ulong>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDailyRunLoadScreen.MethodName.BeginRun) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.BeginRun();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NDailyRunLoadScreen.MethodName.AfterMultiplayerStarted) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.AfterMultiplayerStarted();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NDailyRunLoadScreen.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NDailyRunLoadScreen ndailyRunLoadScreen = NDailyRunLoadScreen.Create();
      ret = VariantUtils.CreateFrom<NDailyRunLoadScreen>(ref ndailyRunLoadScreen);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NDailyRunLoadScreen.MethodName.Create) || StringName.op_Equality(ref method, NDailyRunLoadScreen.MethodName._Ready) || StringName.op_Equality(ref method, NDailyRunLoadScreen.MethodName.OnSubmenuOpened) || StringName.op_Equality(ref method, NDailyRunLoadScreen.MethodName.OnSubmenuClosed) || StringName.op_Equality(ref method, NDailyRunLoadScreen.MethodName.InitializeDisplay) || StringName.op_Equality(ref method, NDailyRunLoadScreen.MethodName.OnEmbarkPressed) || StringName.op_Equality(ref method, NDailyRunLoadScreen.MethodName.OnUnreadyPressed) || StringName.op_Equality(ref method, NDailyRunLoadScreen.MethodName.UpdateRichPresence) || StringName.op_Equality(ref method, NDailyRunLoadScreen.MethodName._Process) || StringName.op_Equality(ref method, NDailyRunLoadScreen.MethodName.CleanUpLobby) || StringName.op_Equality(ref method, NDailyRunLoadScreen.MethodName.PlayerConnected) || StringName.op_Equality(ref method, NDailyRunLoadScreen.MethodName.PlayerReadyChanged) || StringName.op_Equality(ref method, NDailyRunLoadScreen.MethodName.RemotePlayerDisconnected) || StringName.op_Equality(ref method, NDailyRunLoadScreen.MethodName.BeginRun) || StringName.op_Equality(ref method, NDailyRunLoadScreen.MethodName.AfterMultiplayerStarted) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDailyRunLoadScreen.PropertyName._dateLabel))
    {
      this._dateLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunLoadScreen.PropertyName._embarkButton))
    {
      this._embarkButton = VariantUtils.ConvertTo<NConfirmButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunLoadScreen.PropertyName._backButton))
    {
      this._backButton = VariantUtils.ConvertTo<NBackButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunLoadScreen.PropertyName._unreadyButton))
    {
      this._unreadyButton = VariantUtils.ConvertTo<NBackButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunLoadScreen.PropertyName._characterContainer))
    {
      this._characterContainer = VariantUtils.ConvertTo<NDailyRunCharacterContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunLoadScreen.PropertyName._leaderboard))
    {
      this._leaderboard = VariantUtils.ConvertTo<NDailyRunLeaderboard>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunLoadScreen.PropertyName._modifiersTitleLabel))
    {
      this._modifiersTitleLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunLoadScreen.PropertyName._modifiersContainer))
    {
      this._modifiersContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunLoadScreen.PropertyName._remotePlayerContainer))
    {
      this._remotePlayerContainer = VariantUtils.ConvertTo<NRemoteLoadLobbyPlayerContainer>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDailyRunLoadScreen.PropertyName._readyAndWaitingContainer))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._readyAndWaitingContainer = VariantUtils.ConvertTo<Control>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDailyRunLoadScreen.PropertyName.InitialFocusedControl))
    {
      ref godot_variant local = ref value;
      Control initialFocusedControl = this.InitialFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref initialFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunLoadScreen.PropertyName._dateLabel))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._dateLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunLoadScreen.PropertyName._embarkButton))
    {
      value = VariantUtils.CreateFrom<NConfirmButton>(ref this._embarkButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunLoadScreen.PropertyName._backButton))
    {
      value = VariantUtils.CreateFrom<NBackButton>(ref this._backButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunLoadScreen.PropertyName._unreadyButton))
    {
      value = VariantUtils.CreateFrom<NBackButton>(ref this._unreadyButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunLoadScreen.PropertyName._characterContainer))
    {
      value = VariantUtils.CreateFrom<NDailyRunCharacterContainer>(ref this._characterContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunLoadScreen.PropertyName._leaderboard))
    {
      value = VariantUtils.CreateFrom<NDailyRunLeaderboard>(ref this._leaderboard);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunLoadScreen.PropertyName._modifiersTitleLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._modifiersTitleLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunLoadScreen.PropertyName._modifiersContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._modifiersContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunLoadScreen.PropertyName._remotePlayerContainer))
    {
      value = VariantUtils.CreateFrom<NRemoteLoadLobbyPlayerContainer>(ref this._remotePlayerContainer);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDailyRunLoadScreen.PropertyName._readyAndWaitingContainer))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Control>(ref this._readyAndWaitingContainer);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NDailyRunLoadScreen.PropertyName.InitialFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDailyRunLoadScreen.PropertyName._dateLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDailyRunLoadScreen.PropertyName._embarkButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDailyRunLoadScreen.PropertyName._backButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDailyRunLoadScreen.PropertyName._unreadyButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDailyRunLoadScreen.PropertyName._characterContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDailyRunLoadScreen.PropertyName._leaderboard, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDailyRunLoadScreen.PropertyName._modifiersTitleLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDailyRunLoadScreen.PropertyName._modifiersContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDailyRunLoadScreen.PropertyName._remotePlayerContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDailyRunLoadScreen.PropertyName._readyAndWaitingContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NDailyRunLoadScreen.PropertyName._dateLabel, Variant.From<MegaRichTextLabel>(ref this._dateLabel));
    info.AddProperty(NDailyRunLoadScreen.PropertyName._embarkButton, Variant.From<NConfirmButton>(ref this._embarkButton));
    info.AddProperty(NDailyRunLoadScreen.PropertyName._backButton, Variant.From<NBackButton>(ref this._backButton));
    info.AddProperty(NDailyRunLoadScreen.PropertyName._unreadyButton, Variant.From<NBackButton>(ref this._unreadyButton));
    info.AddProperty(NDailyRunLoadScreen.PropertyName._characterContainer, Variant.From<NDailyRunCharacterContainer>(ref this._characterContainer));
    info.AddProperty(NDailyRunLoadScreen.PropertyName._leaderboard, Variant.From<NDailyRunLeaderboard>(ref this._leaderboard));
    info.AddProperty(NDailyRunLoadScreen.PropertyName._modifiersTitleLabel, Variant.From<MegaLabel>(ref this._modifiersTitleLabel));
    info.AddProperty(NDailyRunLoadScreen.PropertyName._modifiersContainer, Variant.From<Control>(ref this._modifiersContainer));
    info.AddProperty(NDailyRunLoadScreen.PropertyName._remotePlayerContainer, Variant.From<NRemoteLoadLobbyPlayerContainer>(ref this._remotePlayerContainer));
    info.AddProperty(NDailyRunLoadScreen.PropertyName._readyAndWaitingContainer, Variant.From<Control>(ref this._readyAndWaitingContainer));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NDailyRunLoadScreen.PropertyName._dateLabel, ref variant1))
      this._dateLabel = ((Variant) ref variant1).As<MegaRichTextLabel>();
    Variant variant2;
    if (info.TryGetProperty(NDailyRunLoadScreen.PropertyName._embarkButton, ref variant2))
      this._embarkButton = ((Variant) ref variant2).As<NConfirmButton>();
    Variant variant3;
    if (info.TryGetProperty(NDailyRunLoadScreen.PropertyName._backButton, ref variant3))
      this._backButton = ((Variant) ref variant3).As<NBackButton>();
    Variant variant4;
    if (info.TryGetProperty(NDailyRunLoadScreen.PropertyName._unreadyButton, ref variant4))
      this._unreadyButton = ((Variant) ref variant4).As<NBackButton>();
    Variant variant5;
    if (info.TryGetProperty(NDailyRunLoadScreen.PropertyName._characterContainer, ref variant5))
      this._characterContainer = ((Variant) ref variant5).As<NDailyRunCharacterContainer>();
    Variant variant6;
    if (info.TryGetProperty(NDailyRunLoadScreen.PropertyName._leaderboard, ref variant6))
      this._leaderboard = ((Variant) ref variant6).As<NDailyRunLeaderboard>();
    Variant variant7;
    if (info.TryGetProperty(NDailyRunLoadScreen.PropertyName._modifiersTitleLabel, ref variant7))
      this._modifiersTitleLabel = ((Variant) ref variant7).As<MegaLabel>();
    Variant variant8;
    if (info.TryGetProperty(NDailyRunLoadScreen.PropertyName._modifiersContainer, ref variant8))
      this._modifiersContainer = ((Variant) ref variant8).As<Control>();
    Variant variant9;
    if (info.TryGetProperty(NDailyRunLoadScreen.PropertyName._remotePlayerContainer, ref variant9))
      this._remotePlayerContainer = ((Variant) ref variant9).As<NRemoteLoadLobbyPlayerContainer>();
    Variant variant10;
    if (!info.TryGetProperty(NDailyRunLoadScreen.PropertyName._readyAndWaitingContainer, ref variant10))
      return;
    this._readyAndWaitingContainer = ((Variant) ref variant10).As<Control>();
  }

  public new class MethodName : NSubmenu.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName OnSubmenuOpened = StringName.op_Implicit(nameof (OnSubmenuOpened));
    public new static readonly StringName OnSubmenuClosed = StringName.op_Implicit(nameof (OnSubmenuClosed));
    public static readonly StringName InitializeDisplay = StringName.op_Implicit(nameof (InitializeDisplay));
    public static readonly StringName OnEmbarkPressed = StringName.op_Implicit(nameof (OnEmbarkPressed));
    public static readonly StringName OnUnreadyPressed = StringName.op_Implicit(nameof (OnUnreadyPressed));
    public static readonly StringName UpdateRichPresence = StringName.op_Implicit(nameof (UpdateRichPresence));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
    public static readonly StringName CleanUpLobby = StringName.op_Implicit(nameof (CleanUpLobby));
    public static readonly StringName PlayerConnected = StringName.op_Implicit(nameof (PlayerConnected));
    public static readonly StringName PlayerReadyChanged = StringName.op_Implicit(nameof (PlayerReadyChanged));
    public static readonly StringName RemotePlayerDisconnected = StringName.op_Implicit(nameof (RemotePlayerDisconnected));
    public static readonly StringName BeginRun = StringName.op_Implicit(nameof (BeginRun));
    public static readonly StringName AfterMultiplayerStarted = StringName.op_Implicit(nameof (AfterMultiplayerStarted));
  }

  public new class PropertyName : NSubmenu.PropertyName
  {
    public new static readonly StringName InitialFocusedControl = StringName.op_Implicit(nameof (InitialFocusedControl));
    public static readonly StringName _dateLabel = StringName.op_Implicit(nameof (_dateLabel));
    public static readonly StringName _embarkButton = StringName.op_Implicit(nameof (_embarkButton));
    public new static readonly StringName _backButton = StringName.op_Implicit(nameof (_backButton));
    public static readonly StringName _unreadyButton = StringName.op_Implicit(nameof (_unreadyButton));
    public static readonly StringName _characterContainer = StringName.op_Implicit(nameof (_characterContainer));
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
