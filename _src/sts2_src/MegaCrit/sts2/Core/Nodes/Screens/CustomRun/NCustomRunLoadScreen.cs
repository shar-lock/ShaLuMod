// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.CustomRun.NCustomRunLoadScreen
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
using MegaCrit.Sts2.Core.Entities.UI;
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
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.CustomRun;

[ScriptPath("res://src/Core/Nodes/Screens/CustomRun/NCustomRunLoadScreen.cs")]
public class NCustomRunLoadScreen : NSubmenu, ILoadRunLobbyListener
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("screens/custom_run/custom_run_load_screen");
  private NConfirmButton _confirmButton;
  private NBackButton _backButton;
  private NBackButton _unreadyButton;
  private NAscensionPanel _ascensionPanel;
  private Control _readyAndWaitingContainer;
  private LineEdit _seedInput;
  private NRemoteLoadLobbyPlayerContainer _remotePlayerContainer;
  private NCustomRunModifiersList _modifiersList;
  private LoadRunLobby _lobby;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[2]
      {
        NCustomRunLoadScreen._scenePath,
        "res://scenes/screens/custom_run/modifier_tickbox.tscn"
      });
    }
  }

  protected override Control? InitialFocusedControl => (Control) null;

  public static NCustomRunLoadScreen? Create()
  {
    return TestMode.IsOn ? (NCustomRunLoadScreen) null : PreloadManager.Cache.GetScene(NCustomRunLoadScreen._scenePath).Instantiate<NCustomRunLoadScreen>((PackedScene.GenEditState) 0L);
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._ascensionPanel = ((Node) this).GetNode<NAscensionPanel>(NodePath.op_Implicit("%AscensionPanel"));
    this._remotePlayerContainer = ((Node) this).GetNode<NRemoteLoadLobbyPlayerContainer>(NodePath.op_Implicit("LeftContainer/RemotePlayerLoadContainer"));
    this._readyAndWaitingContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%ReadyAndWaitingPanel"));
    this._modifiersList = ((Node) this).GetNode<NCustomRunModifiersList>(NodePath.op_Implicit("%ModifiersList"));
    this._seedInput = ((Node) this).GetNode<LineEdit>(NodePath.op_Implicit("%SeedInput"));
    this._confirmButton = ((Node) this).GetNode<NConfirmButton>(NodePath.op_Implicit("ConfirmButton"));
    this._backButton = ((Node) this).GetNode<NBackButton>(NodePath.op_Implicit("BackButton"));
    this._unreadyButton = ((Node) this).GetNode<NBackButton>(NodePath.op_Implicit("UnreadyButton"));
    ((GodotObject) this._confirmButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnEmbarkPressed)), 0U);
    ((GodotObject) this._unreadyButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnUnreadyPressed)), 0U);
    this._unreadyButton.Disable();
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%CustomModeTitle")).SetTextAutoSize(new LocString("main_menu_ui", "CUSTOM_RUN_SCREEN.CUSTOM_MODE_TITLE").GetFormattedText());
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%ModifiersTitle")).SetTextAutoSize(new LocString("main_menu_ui", "CUSTOM_RUN_SCREEN.MODIFIERS_TITLE").GetFormattedText());
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%SeedLabel")).SetTextAutoSize(new LocString("main_menu_ui", "CUSTOM_RUN_SCREEN.SEED_LABEL").GetFormattedText());
    this._seedInput.PlaceholderText = new LocString("main_menu_ui", "CUSTOM_RUN_SCREEN.SEED_RANDOM_PLACEHOLDER").GetFormattedText();
  }

  public void InitializeAsHost(INetGameService gameService, SerializableRun run)
  {
    this._lobby = gameService.Type == NetGameType.Host ? new LoadRunLobby(gameService, (ILoadRunLobbyListener) this, run) : throw new InvalidOperationException($"Initialized custom run screen with NetService of type {gameService.Type} when hosting!");
    try
    {
      this._lobby.AddLocalHostPlayer();
      this.AfterInitialized();
    }
    catch
    {
      this.CleanUpLobby(true, NetError.InternalError);
      throw;
    }
  }

  public void InitializeAsClient(INetGameService gameService, ClientLoadJoinResponseMessage message)
  {
    this._lobby = gameService.Type == NetGameType.Client ? new LoadRunLobby(gameService, (ILoadRunLobbyListener) this, message) : throw new InvalidOperationException($"Initialized character select screen with NetService of type {gameService.Type} when joining!");
    this.AfterInitialized();
  }

  public override void OnSubmenuOpened()
  {
    base.OnSubmenuOpened();
    this._confirmButton.Enable();
    this._remotePlayerContainer.Initialize(this._lobby, true);
    this._ascensionPanel.Initialize(MultiplayerUiMode.Load);
    this._ascensionPanel.SetAscensionLevel(this._lobby.Run.Ascension);
    this._modifiersList.Initialize(MultiplayerUiMode.Load);
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    this._modifiersList.SyncModifierList((IReadOnlyCollection<ModifierModel>) this._lobby.Run.Modifiers.Select<SerializableModifier, ModifierModel>(NCustomRunLoadScreen.\u003C\u003EO.\u003C0\u003E__FromSerializable ?? (NCustomRunLoadScreen.\u003C\u003EO.\u003C0\u003E__FromSerializable = new Func<SerializableModifier, ModifierModel>(ModifierModel.FromSerializable))).ToList<ModifierModel>());
    ((CanvasItem) this._readyAndWaitingContainer).Visible = false;
  }

  public override void OnSubmenuClosed()
  {
    base.OnSubmenuClosed();
    this._confirmButton.Disable();
    this._remotePlayerContainer.Cleanup();
    if (this._lobby.NetService.Type.IsMultiplayer())
      PlatformUtil.SetRichPresence("MAIN_MENU", (string) null, new int?());
    this.CleanUpLobby(true);
  }

  private void OnEmbarkPressed(NButton _)
  {
    this._confirmButton.Disable();
    this._backButton.Disable();
    this._lobby.SetReady(true);
    if (this._lobby.IsAboutToBeginGame())
      return;
    this._unreadyButton.Enable();
    ((CanvasItem) this._readyAndWaitingContainer).Visible = true;
  }

  private void OnUnreadyPressed(NButton _)
  {
    this._confirmButton.Enable();
    this._backButton.Enable();
    this._unreadyButton.Disable();
    this._lobby.SetReady(false);
    ((CanvasItem) this._readyAndWaitingContainer).Visible = false;
  }

  private void UpdateRichPresence()
  {
    if (!this._lobby.NetService.Type.IsMultiplayer())
      return;
    PlatformUtil.SetRichPresence("LOADING_MP_LOBBY", this._lobby.NetService.GetRawLobbyIdentifier(), new int?(this._lobby.ConnectedPlayerIds.Count));
  }

  public override void _Process(double delta)
  {
    if (this._lobby == null || !this._lobby.NetService.IsConnected)
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
      Log.Info($"Loading a custom multiplayer run. Players: {string.Join<ulong>(",", (IEnumerable<ulong>) this._lobby.ConnectedPlayerIds)}.");
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
      Log.Error($"Exception loading custom multiplayer run : {e}");
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
    if ((long) playerId != (long) this._lobby.NetService.NetId || this._lobby.IsPlayerReady(playerId))
      return;
    this._confirmButton.Enable();
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
    this._confirmButton.Disable();
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

  private void AfterInitialized()
  {
    NGame.Instance.RemoteCursorContainer.Initialize(this._lobby.InputSynchronizer, (IEnumerable<ulong>) this._lobby.ConnectedPlayerIds);
    NGame.Instance.ReactionContainer.InitializeNetworking(this._lobby.NetService);
    this.UpdateRichPresence();
    Logger.logLevelTypeMap[LogType.Network] = this._lobby.NetService.Type == NetGameType.Singleplayer ? LogLevel.Info : LogLevel.Debug;
    Logger.logLevelTypeMap[LogType.Actions] = this._lobby.NetService.Type == NetGameType.Singleplayer ? LogLevel.Info : LogLevel.VeryDebug;
    Logger.logLevelTypeMap[LogType.GameSync] = this._lobby.NetService.Type == NetGameType.Singleplayer ? LogLevel.Info : LogLevel.VeryDebug;
    NGame.Instance.DebugSeedOverride = (string) null;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(14)
    {
      new MethodInfo(NCustomRunLoadScreen.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCustomRunLoadScreen.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCustomRunLoadScreen.MethodName.OnSubmenuOpened, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCustomRunLoadScreen.MethodName.OnSubmenuClosed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCustomRunLoadScreen.MethodName.OnEmbarkPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCustomRunLoadScreen.MethodName.OnUnreadyPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCustomRunLoadScreen.MethodName.UpdateRichPresence, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCustomRunLoadScreen.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCustomRunLoadScreen.MethodName.CleanUpLobby, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("disconnectSession"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("error"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCustomRunLoadScreen.MethodName.PlayerConnected, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("playerId"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCustomRunLoadScreen.MethodName.PlayerReadyChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("playerId"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCustomRunLoadScreen.MethodName.RemotePlayerDisconnected, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("playerId"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCustomRunLoadScreen.MethodName.BeginRun, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCustomRunLoadScreen.MethodName.AfterInitialized, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCustomRunLoadScreen.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NCustomRunLoadScreen ncustomRunLoadScreen = NCustomRunLoadScreen.Create();
      ret = VariantUtils.CreateFrom<NCustomRunLoadScreen>(ref ncustomRunLoadScreen);
      return true;
    }
    if (StringName.op_Equality(ref method, NCustomRunLoadScreen.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCustomRunLoadScreen.MethodName.OnSubmenuOpened) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSubmenuOpened();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCustomRunLoadScreen.MethodName.OnSubmenuClosed) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSubmenuClosed();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCustomRunLoadScreen.MethodName.OnEmbarkPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnEmbarkPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCustomRunLoadScreen.MethodName.OnUnreadyPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnUnreadyPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCustomRunLoadScreen.MethodName.UpdateRichPresence) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateRichPresence();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCustomRunLoadScreen.MethodName._Process) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCustomRunLoadScreen.MethodName.CleanUpLobby) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.CleanUpLobby(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<NetError>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCustomRunLoadScreen.MethodName.PlayerConnected) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.PlayerConnected(VariantUtils.ConvertTo<ulong>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCustomRunLoadScreen.MethodName.PlayerReadyChanged) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.PlayerReadyChanged(VariantUtils.ConvertTo<ulong>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCustomRunLoadScreen.MethodName.RemotePlayerDisconnected) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.RemotePlayerDisconnected(VariantUtils.ConvertTo<ulong>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCustomRunLoadScreen.MethodName.BeginRun) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.BeginRun();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCustomRunLoadScreen.MethodName.AfterInitialized) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.AfterInitialized();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCustomRunLoadScreen.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NCustomRunLoadScreen ncustomRunLoadScreen = NCustomRunLoadScreen.Create();
      ret = VariantUtils.CreateFrom<NCustomRunLoadScreen>(ref ncustomRunLoadScreen);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCustomRunLoadScreen.MethodName.Create) || StringName.op_Equality(ref method, NCustomRunLoadScreen.MethodName._Ready) || StringName.op_Equality(ref method, NCustomRunLoadScreen.MethodName.OnSubmenuOpened) || StringName.op_Equality(ref method, NCustomRunLoadScreen.MethodName.OnSubmenuClosed) || StringName.op_Equality(ref method, NCustomRunLoadScreen.MethodName.OnEmbarkPressed) || StringName.op_Equality(ref method, NCustomRunLoadScreen.MethodName.OnUnreadyPressed) || StringName.op_Equality(ref method, NCustomRunLoadScreen.MethodName.UpdateRichPresence) || StringName.op_Equality(ref method, NCustomRunLoadScreen.MethodName._Process) || StringName.op_Equality(ref method, NCustomRunLoadScreen.MethodName.CleanUpLobby) || StringName.op_Equality(ref method, NCustomRunLoadScreen.MethodName.PlayerConnected) || StringName.op_Equality(ref method, NCustomRunLoadScreen.MethodName.PlayerReadyChanged) || StringName.op_Equality(ref method, NCustomRunLoadScreen.MethodName.RemotePlayerDisconnected) || StringName.op_Equality(ref method, NCustomRunLoadScreen.MethodName.BeginRun) || StringName.op_Equality(ref method, NCustomRunLoadScreen.MethodName.AfterInitialized) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCustomRunLoadScreen.PropertyName._confirmButton))
    {
      this._confirmButton = VariantUtils.ConvertTo<NConfirmButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCustomRunLoadScreen.PropertyName._backButton))
    {
      this._backButton = VariantUtils.ConvertTo<NBackButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCustomRunLoadScreen.PropertyName._unreadyButton))
    {
      this._unreadyButton = VariantUtils.ConvertTo<NBackButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCustomRunLoadScreen.PropertyName._ascensionPanel))
    {
      this._ascensionPanel = VariantUtils.ConvertTo<NAscensionPanel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCustomRunLoadScreen.PropertyName._readyAndWaitingContainer))
    {
      this._readyAndWaitingContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCustomRunLoadScreen.PropertyName._seedInput))
    {
      this._seedInput = VariantUtils.ConvertTo<LineEdit>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCustomRunLoadScreen.PropertyName._remotePlayerContainer))
    {
      this._remotePlayerContainer = VariantUtils.ConvertTo<NRemoteLoadLobbyPlayerContainer>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCustomRunLoadScreen.PropertyName._modifiersList))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._modifiersList = VariantUtils.ConvertTo<NCustomRunModifiersList>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCustomRunLoadScreen.PropertyName.InitialFocusedControl))
    {
      ref godot_variant local = ref value;
      Control initialFocusedControl = this.InitialFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref initialFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCustomRunLoadScreen.PropertyName._confirmButton))
    {
      value = VariantUtils.CreateFrom<NConfirmButton>(ref this._confirmButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NCustomRunLoadScreen.PropertyName._backButton))
    {
      value = VariantUtils.CreateFrom<NBackButton>(ref this._backButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NCustomRunLoadScreen.PropertyName._unreadyButton))
    {
      value = VariantUtils.CreateFrom<NBackButton>(ref this._unreadyButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NCustomRunLoadScreen.PropertyName._ascensionPanel))
    {
      value = VariantUtils.CreateFrom<NAscensionPanel>(ref this._ascensionPanel);
      return true;
    }
    if (StringName.op_Equality(ref name, NCustomRunLoadScreen.PropertyName._readyAndWaitingContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._readyAndWaitingContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NCustomRunLoadScreen.PropertyName._seedInput))
    {
      value = VariantUtils.CreateFrom<LineEdit>(ref this._seedInput);
      return true;
    }
    if (StringName.op_Equality(ref name, NCustomRunLoadScreen.PropertyName._remotePlayerContainer))
    {
      value = VariantUtils.CreateFrom<NRemoteLoadLobbyPlayerContainer>(ref this._remotePlayerContainer);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCustomRunLoadScreen.PropertyName._modifiersList))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<NCustomRunModifiersList>(ref this._modifiersList);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCustomRunLoadScreen.PropertyName._confirmButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCustomRunLoadScreen.PropertyName._backButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCustomRunLoadScreen.PropertyName._unreadyButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCustomRunLoadScreen.PropertyName._ascensionPanel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCustomRunLoadScreen.PropertyName._readyAndWaitingContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCustomRunLoadScreen.PropertyName._seedInput, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCustomRunLoadScreen.PropertyName._remotePlayerContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCustomRunLoadScreen.PropertyName._modifiersList, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCustomRunLoadScreen.PropertyName.InitialFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NCustomRunLoadScreen.PropertyName._confirmButton, Variant.From<NConfirmButton>(ref this._confirmButton));
    info.AddProperty(NCustomRunLoadScreen.PropertyName._backButton, Variant.From<NBackButton>(ref this._backButton));
    info.AddProperty(NCustomRunLoadScreen.PropertyName._unreadyButton, Variant.From<NBackButton>(ref this._unreadyButton));
    info.AddProperty(NCustomRunLoadScreen.PropertyName._ascensionPanel, Variant.From<NAscensionPanel>(ref this._ascensionPanel));
    info.AddProperty(NCustomRunLoadScreen.PropertyName._readyAndWaitingContainer, Variant.From<Control>(ref this._readyAndWaitingContainer));
    info.AddProperty(NCustomRunLoadScreen.PropertyName._seedInput, Variant.From<LineEdit>(ref this._seedInput));
    info.AddProperty(NCustomRunLoadScreen.PropertyName._remotePlayerContainer, Variant.From<NRemoteLoadLobbyPlayerContainer>(ref this._remotePlayerContainer));
    info.AddProperty(NCustomRunLoadScreen.PropertyName._modifiersList, Variant.From<NCustomRunModifiersList>(ref this._modifiersList));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCustomRunLoadScreen.PropertyName._confirmButton, ref variant1))
      this._confirmButton = ((Variant) ref variant1).As<NConfirmButton>();
    Variant variant2;
    if (info.TryGetProperty(NCustomRunLoadScreen.PropertyName._backButton, ref variant2))
      this._backButton = ((Variant) ref variant2).As<NBackButton>();
    Variant variant3;
    if (info.TryGetProperty(NCustomRunLoadScreen.PropertyName._unreadyButton, ref variant3))
      this._unreadyButton = ((Variant) ref variant3).As<NBackButton>();
    Variant variant4;
    if (info.TryGetProperty(NCustomRunLoadScreen.PropertyName._ascensionPanel, ref variant4))
      this._ascensionPanel = ((Variant) ref variant4).As<NAscensionPanel>();
    Variant variant5;
    if (info.TryGetProperty(NCustomRunLoadScreen.PropertyName._readyAndWaitingContainer, ref variant5))
      this._readyAndWaitingContainer = ((Variant) ref variant5).As<Control>();
    Variant variant6;
    if (info.TryGetProperty(NCustomRunLoadScreen.PropertyName._seedInput, ref variant6))
      this._seedInput = ((Variant) ref variant6).As<LineEdit>();
    Variant variant7;
    if (info.TryGetProperty(NCustomRunLoadScreen.PropertyName._remotePlayerContainer, ref variant7))
      this._remotePlayerContainer = ((Variant) ref variant7).As<NRemoteLoadLobbyPlayerContainer>();
    Variant variant8;
    if (!info.TryGetProperty(NCustomRunLoadScreen.PropertyName._modifiersList, ref variant8))
      return;
    this._modifiersList = ((Variant) ref variant8).As<NCustomRunModifiersList>();
  }

  public new class MethodName : NSubmenu.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName OnSubmenuOpened = StringName.op_Implicit(nameof (OnSubmenuOpened));
    public new static readonly StringName OnSubmenuClosed = StringName.op_Implicit(nameof (OnSubmenuClosed));
    public static readonly StringName OnEmbarkPressed = StringName.op_Implicit(nameof (OnEmbarkPressed));
    public static readonly StringName OnUnreadyPressed = StringName.op_Implicit(nameof (OnUnreadyPressed));
    public static readonly StringName UpdateRichPresence = StringName.op_Implicit(nameof (UpdateRichPresence));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
    public static readonly StringName CleanUpLobby = StringName.op_Implicit(nameof (CleanUpLobby));
    public static readonly StringName PlayerConnected = StringName.op_Implicit(nameof (PlayerConnected));
    public static readonly StringName PlayerReadyChanged = StringName.op_Implicit(nameof (PlayerReadyChanged));
    public static readonly StringName RemotePlayerDisconnected = StringName.op_Implicit(nameof (RemotePlayerDisconnected));
    public static readonly StringName BeginRun = StringName.op_Implicit(nameof (BeginRun));
    public static readonly StringName AfterInitialized = StringName.op_Implicit(nameof (AfterInitialized));
  }

  public new class PropertyName : NSubmenu.PropertyName
  {
    public new static readonly StringName InitialFocusedControl = StringName.op_Implicit(nameof (InitialFocusedControl));
    public static readonly StringName _confirmButton = StringName.op_Implicit(nameof (_confirmButton));
    public new static readonly StringName _backButton = StringName.op_Implicit(nameof (_backButton));
    public static readonly StringName _unreadyButton = StringName.op_Implicit(nameof (_unreadyButton));
    public static readonly StringName _ascensionPanel = StringName.op_Implicit(nameof (_ascensionPanel));
    public static readonly StringName _readyAndWaitingContainer = StringName.op_Implicit(nameof (_readyAndWaitingContainer));
    public static readonly StringName _seedInput = StringName.op_Implicit(nameof (_seedInput));
    public static readonly StringName _remotePlayerContainer = StringName.op_Implicit(nameof (_remotePlayerContainer));
    public static readonly StringName _modifiersList = StringName.op_Implicit(nameof (_modifiersList));
  }

  public new class SignalName : NSubmenu.SignalName
  {
  }
}
