// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect.NMultiplayerLoadGameScreen
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
namespace MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;

[ScriptPath("res://src/Core/Nodes/Screens/CharacterSelect/NMultiplayerLoadGameScreen.cs")]
public class NMultiplayerLoadGameScreen : NSubmenu, ILoadRunLobbyListener
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("screens/multiplayer_load_game_screen");
  private MegaLabel _name;
  private Control _infoPanel;
  private MegaLabel _hp;
  private MegaLabel _gold;
  private NCharacterSelectButton? _selectedButton;
  private Control _bgContainer;
  private NConfirmButton _confirmButton;
  private NBackButton _backButton;
  private NBackButton _unreadyButton;
  private NAscensionPanel _ascensionPanel;
  private MegaRichTextLabel _floorLabel;
  private MegaRichTextLabel _actLabel;
  private NRemoteLoadLobbyPlayerContainer _remotePlayerContainer;
  private Tween? _infoPanelTween;
  private Vector2 _infoPanelPosFinalVal;
  private const string _sceneCharSelectButtonPath = "res://scenes/screens/char_select/char_select_button.tscn";
  private LoadRunLobby _runLobby;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[2]
      {
        NMultiplayerLoadGameScreen._scenePath,
        "res://scenes/screens/char_select/char_select_button.tscn"
      });
    }
  }

  protected override Control? InitialFocusedControl => (Control) null;

  public static NMultiplayerLoadGameScreen? Create()
  {
    return TestMode.IsOn ? (NMultiplayerLoadGameScreen) null : PreloadManager.Cache.GetScene(NMultiplayerLoadGameScreen._scenePath).Instantiate<NMultiplayerLoadGameScreen>((PackedScene.GenEditState) 0L);
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._infoPanel = ((Node) this).GetNode<Control>(NodePath.op_Implicit("InfoPanel"));
    this._name = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("InfoPanel/VBoxContainer/Name"));
    this._hp = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("InfoPanel/VBoxContainer/HpGoldSpacer/HpGold/Hp/Label"));
    this._gold = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("InfoPanel/VBoxContainer/HpGoldSpacer/HpGold/Gold/Label"));
    this._actLabel = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("InfoPanel/VBoxContainer/RunLocation/ActLabel"));
    this._floorLabel = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("InfoPanel/VBoxContainer/RunLocation/FloorLabel"));
    this._bgContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("AnimatedBg"));
    this._ascensionPanel = ((Node) this).GetNode<NAscensionPanel>(NodePath.op_Implicit("%AscensionPanel"));
    this._remotePlayerContainer = ((Node) this).GetNode<NRemoteLoadLobbyPlayerContainer>(NodePath.op_Implicit("RemotePlayerLoadContainer"));
    this._confirmButton = ((Node) this).GetNode<NConfirmButton>(NodePath.op_Implicit("ConfirmButton"));
    this._backButton = ((Node) this).GetNode<NBackButton>(NodePath.op_Implicit("BackButton"));
    this._unreadyButton = ((Node) this).GetNode<NBackButton>(NodePath.op_Implicit("UnreadyButton"));
    ((GodotObject) this._confirmButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnEmbarkPressed)), 0U);
    ((GodotObject) this._unreadyButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnUnreadyPressed)), 0U);
    this._unreadyButton.Disable();
  }

  public void InitializeAsHost(INetGameService gameService, SerializableRun run)
  {
    this._runLobby = gameService.Type == NetGameType.Host ? new LoadRunLobby(gameService, (ILoadRunLobbyListener) this, run) : throw new InvalidOperationException($"Initialized character select screen with GameService of type {gameService.Type} when hosting!");
    try
    {
      this._runLobby.AddLocalHostPlayer();
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
    this._runLobby = gameService.Type == NetGameType.Client ? new LoadRunLobby(gameService, (ILoadRunLobbyListener) this, message) : throw new InvalidOperationException($"Initialized character select screen with GameService of type {gameService.Type} when joining!");
    this.AfterMultiplayerStarted();
  }

  public override void OnSubmenuOpened()
  {
    base.OnSubmenuOpened();
    this._confirmButton.Enable();
    this._remotePlayerContainer.Initialize(this._runLobby, false);
    this._ascensionPanel.Initialize(MultiplayerUiMode.Load);
    this._ascensionPanel.SetAscensionLevel(this._runLobby.Run.Ascension);
  }

  public override void OnSubmenuClosed()
  {
    base.OnSubmenuClosed();
    this._confirmButton.Disable();
    this._remotePlayerContainer.Cleanup();
    if (this._runLobby.NetService.Type.IsMultiplayer())
      PlatformUtil.SetRichPresence("MAIN_MENU", (string) null, new int?());
    this.CleanUpLobby(true);
  }

  private void OnEmbarkPressed(NButton _)
  {
    this._confirmButton.Disable();
    this._backButton.Disable();
    this._runLobby.SetReady(true);
    if (this._runLobby.IsAboutToBeginGame())
      return;
    this._unreadyButton.Enable();
  }

  private void OnUnreadyPressed(NButton _)
  {
    this._confirmButton.Enable();
    this._backButton.Enable();
    this._unreadyButton.Disable();
    this._runLobby.SetReady(false);
  }

  private void UpdateRichPresence()
  {
    if (!this._runLobby.NetService.Type.IsMultiplayer())
      return;
    PlatformUtil.SetRichPresence("LOADING_MP_LOBBY", this._runLobby.NetService.GetRawLobbyIdentifier(), new int?(this._runLobby.ConnectedPlayerIds.Count));
  }

  public override void _Process(double delta)
  {
    if (this._runLobby == null || !this._runLobby.NetService.IsConnected)
      return;
    this._runLobby.NetService.Update();
  }

  private void CleanUpLobby(bool disconnectSession, NetError error = NetError.Quit)
  {
    this._runLobby.CleanUp(disconnectSession, error);
    this._runLobby = (LoadRunLobby) null;
  }

  public async Task<bool> ShouldAllowRunToBegin()
  {
    if (this._runLobby.ConnectedPlayerIds.Count >= this._runLobby.Run.Players.Count)
      return true;
    LocString body = new LocString("gameplay_ui", "CONFIRM_LOAD_SAVE.body");
    body.Add("MissingCount", (Decimal) (this._runLobby.Run.Players.Count - this._runLobby.ConnectedPlayerIds.Count));
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
      Log.Info($"Loading a multiplayer run. Players: {string.Join<ulong>(",", (IEnumerable<ulong>) this._runLobby.ConnectedPlayerIds)}.");
      SerializablePlayer serializablePlayer = this._runLobby.Run.Players.First<SerializablePlayer>((Func<SerializablePlayer, bool>) (p => (long) p.NetId == (long) this._runLobby.NetService.NetId));
      SfxCmd.Play(ModelDb.GetById<CharacterModel>(serializablePlayer.CharacterId).CharacterTransitionSfx);
      await NGame.Instance.Transition.FadeOut(transitionPath: ModelDb.GetById<CharacterModel>(serializablePlayer.CharacterId).CharacterSelectTransitionPath);
      RunState runState = RunState.FromSerializable(this._runLobby.Run);
      await RunManager.Instance.SetUpSavedMultiplayer(runState, this._runLobby);
      await NGame.Instance.LoadRun(runState, this._runLobby.Run.PreFinishedRoom);
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
      Log.Error($"Exception loading multiplayer run : {e}");
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
    if ((long) playerId != (long) this._runLobby.NetService.NetId || this._runLobby.IsPlayerReady(playerId))
      return;
    this._confirmButton.Enable();
    this._backButton.Enable();
    this._unreadyButton.Disable();
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

  private void AfterMultiplayerStarted()
  {
    NGame.Instance.RemoteCursorContainer.Initialize(this._runLobby.InputSynchronizer, (IEnumerable<ulong>) this._runLobby.ConnectedPlayerIds);
    NGame.Instance.ReactionContainer.InitializeNetworking(this._runLobby.NetService);
    SerializablePlayer serializablePlayer = this._runLobby.Run.Players.First<SerializablePlayer>((Func<SerializablePlayer, bool>) (p => (long) p.NetId == (long) this._runLobby.NetService.NetId));
    CharacterModel byId = ModelDb.GetById<CharacterModel>(serializablePlayer.CharacterId);
    SfxCmd.Play(byId.CharacterSelectSfx);
    foreach (Node child in ((Node) this._bgContainer).GetChildren(false))
    {
      ((Node) this._bgContainer).RemoveChildSafely(child);
      child.QueueFreeSafely();
    }
    Control child1 = PreloadManager.Cache.GetScene(byId.CharacterSelectBg).Instantiate<Control>((PackedScene.GenEditState) 0L);
    ((Node) child1).Name = StringName.op_Implicit(byId.Id.Entry + "_bg");
    ((Node) this._bgContainer).AddChildSafely((Node) child1);
    this._name.SetTextAutoSize(byId.Title.GetFormattedText());
    this._hp.SetTextAutoSize($"{serializablePlayer.CurrentHp}/{serializablePlayer.MaxHp}");
    this._gold.SetTextAutoSize($"{serializablePlayer.Gold}");
    LocString locString1 = new LocString("main_menu_ui", "MULTIPLAYER_LOAD_MENU.FLOOR");
    locString1.Add("floor", (Decimal) this._runLobby.Run.VisitedMapCoords.Count);
    this._floorLabel.Text = locString1.GetFormattedText();
    LocString locString2 = new LocString("main_menu_ui", "MULTIPLAYER_LOAD_MENU.ACT");
    locString2.Add("act", (Decimal) (this._runLobby.Run.CurrentActIndex + 1));
    this._actLabel.Text = locString2.GetFormattedText();
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
    return new List<MethodInfo>(14)
    {
      new MethodInfo(NMultiplayerLoadGameScreen.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerLoadGameScreen.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerLoadGameScreen.MethodName.OnSubmenuOpened, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerLoadGameScreen.MethodName.OnSubmenuClosed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerLoadGameScreen.MethodName.OnEmbarkPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMultiplayerLoadGameScreen.MethodName.OnUnreadyPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMultiplayerLoadGameScreen.MethodName.UpdateRichPresence, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerLoadGameScreen.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMultiplayerLoadGameScreen.MethodName.CleanUpLobby, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("disconnectSession"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("error"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMultiplayerLoadGameScreen.MethodName.PlayerConnected, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("playerId"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMultiplayerLoadGameScreen.MethodName.PlayerReadyChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("playerId"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMultiplayerLoadGameScreen.MethodName.RemotePlayerDisconnected, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("playerId"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMultiplayerLoadGameScreen.MethodName.BeginRun, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMultiplayerLoadGameScreen.MethodName.AfterMultiplayerStarted, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMultiplayerLoadGameScreen.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NMultiplayerLoadGameScreen nmultiplayerLoadGameScreen = NMultiplayerLoadGameScreen.Create();
      ret = VariantUtils.CreateFrom<NMultiplayerLoadGameScreen>(ref nmultiplayerLoadGameScreen);
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerLoadGameScreen.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerLoadGameScreen.MethodName.OnSubmenuOpened) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSubmenuOpened();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerLoadGameScreen.MethodName.OnSubmenuClosed) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSubmenuClosed();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerLoadGameScreen.MethodName.OnEmbarkPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnEmbarkPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerLoadGameScreen.MethodName.OnUnreadyPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnUnreadyPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerLoadGameScreen.MethodName.UpdateRichPresence) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateRichPresence();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerLoadGameScreen.MethodName._Process) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerLoadGameScreen.MethodName.CleanUpLobby) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.CleanUpLobby(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<NetError>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerLoadGameScreen.MethodName.PlayerConnected) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.PlayerConnected(VariantUtils.ConvertTo<ulong>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerLoadGameScreen.MethodName.PlayerReadyChanged) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.PlayerReadyChanged(VariantUtils.ConvertTo<ulong>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerLoadGameScreen.MethodName.RemotePlayerDisconnected) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.RemotePlayerDisconnected(VariantUtils.ConvertTo<ulong>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMultiplayerLoadGameScreen.MethodName.BeginRun) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.BeginRun();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NMultiplayerLoadGameScreen.MethodName.AfterMultiplayerStarted) || ((NativeVariantPtrArgs) ref args).Count != 0)
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
    if (StringName.op_Equality(ref method, NMultiplayerLoadGameScreen.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NMultiplayerLoadGameScreen nmultiplayerLoadGameScreen = NMultiplayerLoadGameScreen.Create();
      ret = VariantUtils.CreateFrom<NMultiplayerLoadGameScreen>(ref nmultiplayerLoadGameScreen);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMultiplayerLoadGameScreen.MethodName.Create) || StringName.op_Equality(ref method, NMultiplayerLoadGameScreen.MethodName._Ready) || StringName.op_Equality(ref method, NMultiplayerLoadGameScreen.MethodName.OnSubmenuOpened) || StringName.op_Equality(ref method, NMultiplayerLoadGameScreen.MethodName.OnSubmenuClosed) || StringName.op_Equality(ref method, NMultiplayerLoadGameScreen.MethodName.OnEmbarkPressed) || StringName.op_Equality(ref method, NMultiplayerLoadGameScreen.MethodName.OnUnreadyPressed) || StringName.op_Equality(ref method, NMultiplayerLoadGameScreen.MethodName.UpdateRichPresence) || StringName.op_Equality(ref method, NMultiplayerLoadGameScreen.MethodName._Process) || StringName.op_Equality(ref method, NMultiplayerLoadGameScreen.MethodName.CleanUpLobby) || StringName.op_Equality(ref method, NMultiplayerLoadGameScreen.MethodName.PlayerConnected) || StringName.op_Equality(ref method, NMultiplayerLoadGameScreen.MethodName.PlayerReadyChanged) || StringName.op_Equality(ref method, NMultiplayerLoadGameScreen.MethodName.RemotePlayerDisconnected) || StringName.op_Equality(ref method, NMultiplayerLoadGameScreen.MethodName.BeginRun) || StringName.op_Equality(ref method, NMultiplayerLoadGameScreen.MethodName.AfterMultiplayerStarted) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMultiplayerLoadGameScreen.PropertyName._name))
    {
      this._name = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerLoadGameScreen.PropertyName._infoPanel))
    {
      this._infoPanel = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerLoadGameScreen.PropertyName._hp))
    {
      this._hp = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerLoadGameScreen.PropertyName._gold))
    {
      this._gold = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerLoadGameScreen.PropertyName._selectedButton))
    {
      this._selectedButton = VariantUtils.ConvertTo<NCharacterSelectButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerLoadGameScreen.PropertyName._bgContainer))
    {
      this._bgContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerLoadGameScreen.PropertyName._confirmButton))
    {
      this._confirmButton = VariantUtils.ConvertTo<NConfirmButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerLoadGameScreen.PropertyName._backButton))
    {
      this._backButton = VariantUtils.ConvertTo<NBackButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerLoadGameScreen.PropertyName._unreadyButton))
    {
      this._unreadyButton = VariantUtils.ConvertTo<NBackButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerLoadGameScreen.PropertyName._ascensionPanel))
    {
      this._ascensionPanel = VariantUtils.ConvertTo<NAscensionPanel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerLoadGameScreen.PropertyName._floorLabel))
    {
      this._floorLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerLoadGameScreen.PropertyName._actLabel))
    {
      this._actLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerLoadGameScreen.PropertyName._remotePlayerContainer))
    {
      this._remotePlayerContainer = VariantUtils.ConvertTo<NRemoteLoadLobbyPlayerContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerLoadGameScreen.PropertyName._infoPanelTween))
    {
      this._infoPanelTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMultiplayerLoadGameScreen.PropertyName._infoPanelPosFinalVal))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._infoPanelPosFinalVal = VariantUtils.ConvertTo<Vector2>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMultiplayerLoadGameScreen.PropertyName.InitialFocusedControl))
    {
      ref godot_variant local = ref value;
      Control initialFocusedControl = this.InitialFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref initialFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerLoadGameScreen.PropertyName._name))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._name);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerLoadGameScreen.PropertyName._infoPanel))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._infoPanel);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerLoadGameScreen.PropertyName._hp))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._hp);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerLoadGameScreen.PropertyName._gold))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._gold);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerLoadGameScreen.PropertyName._selectedButton))
    {
      value = VariantUtils.CreateFrom<NCharacterSelectButton>(ref this._selectedButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerLoadGameScreen.PropertyName._bgContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._bgContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerLoadGameScreen.PropertyName._confirmButton))
    {
      value = VariantUtils.CreateFrom<NConfirmButton>(ref this._confirmButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerLoadGameScreen.PropertyName._backButton))
    {
      value = VariantUtils.CreateFrom<NBackButton>(ref this._backButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerLoadGameScreen.PropertyName._unreadyButton))
    {
      value = VariantUtils.CreateFrom<NBackButton>(ref this._unreadyButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerLoadGameScreen.PropertyName._ascensionPanel))
    {
      value = VariantUtils.CreateFrom<NAscensionPanel>(ref this._ascensionPanel);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerLoadGameScreen.PropertyName._floorLabel))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._floorLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerLoadGameScreen.PropertyName._actLabel))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._actLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerLoadGameScreen.PropertyName._remotePlayerContainer))
    {
      value = VariantUtils.CreateFrom<NRemoteLoadLobbyPlayerContainer>(ref this._remotePlayerContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NMultiplayerLoadGameScreen.PropertyName._infoPanelTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._infoPanelTween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMultiplayerLoadGameScreen.PropertyName._infoPanelPosFinalVal))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Vector2>(ref this._infoPanelPosFinalVal);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NMultiplayerLoadGameScreen.PropertyName._name, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerLoadGameScreen.PropertyName._infoPanel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerLoadGameScreen.PropertyName._hp, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerLoadGameScreen.PropertyName._gold, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerLoadGameScreen.PropertyName._selectedButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerLoadGameScreen.PropertyName._bgContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerLoadGameScreen.PropertyName._confirmButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerLoadGameScreen.PropertyName._backButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerLoadGameScreen.PropertyName._unreadyButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerLoadGameScreen.PropertyName._ascensionPanel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerLoadGameScreen.PropertyName._floorLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerLoadGameScreen.PropertyName._actLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerLoadGameScreen.PropertyName._remotePlayerContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerLoadGameScreen.PropertyName._infoPanelTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NMultiplayerLoadGameScreen.PropertyName._infoPanelPosFinalVal, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMultiplayerLoadGameScreen.PropertyName.InitialFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NMultiplayerLoadGameScreen.PropertyName._name, Variant.From<MegaLabel>(ref this._name));
    info.AddProperty(NMultiplayerLoadGameScreen.PropertyName._infoPanel, Variant.From<Control>(ref this._infoPanel));
    info.AddProperty(NMultiplayerLoadGameScreen.PropertyName._hp, Variant.From<MegaLabel>(ref this._hp));
    info.AddProperty(NMultiplayerLoadGameScreen.PropertyName._gold, Variant.From<MegaLabel>(ref this._gold));
    info.AddProperty(NMultiplayerLoadGameScreen.PropertyName._selectedButton, Variant.From<NCharacterSelectButton>(ref this._selectedButton));
    info.AddProperty(NMultiplayerLoadGameScreen.PropertyName._bgContainer, Variant.From<Control>(ref this._bgContainer));
    info.AddProperty(NMultiplayerLoadGameScreen.PropertyName._confirmButton, Variant.From<NConfirmButton>(ref this._confirmButton));
    info.AddProperty(NMultiplayerLoadGameScreen.PropertyName._backButton, Variant.From<NBackButton>(ref this._backButton));
    info.AddProperty(NMultiplayerLoadGameScreen.PropertyName._unreadyButton, Variant.From<NBackButton>(ref this._unreadyButton));
    info.AddProperty(NMultiplayerLoadGameScreen.PropertyName._ascensionPanel, Variant.From<NAscensionPanel>(ref this._ascensionPanel));
    info.AddProperty(NMultiplayerLoadGameScreen.PropertyName._floorLabel, Variant.From<MegaRichTextLabel>(ref this._floorLabel));
    info.AddProperty(NMultiplayerLoadGameScreen.PropertyName._actLabel, Variant.From<MegaRichTextLabel>(ref this._actLabel));
    info.AddProperty(NMultiplayerLoadGameScreen.PropertyName._remotePlayerContainer, Variant.From<NRemoteLoadLobbyPlayerContainer>(ref this._remotePlayerContainer));
    info.AddProperty(NMultiplayerLoadGameScreen.PropertyName._infoPanelTween, Variant.From<Tween>(ref this._infoPanelTween));
    info.AddProperty(NMultiplayerLoadGameScreen.PropertyName._infoPanelPosFinalVal, Variant.From<Vector2>(ref this._infoPanelPosFinalVal));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NMultiplayerLoadGameScreen.PropertyName._name, ref variant1))
      this._name = ((Variant) ref variant1).As<MegaLabel>();
    Variant variant2;
    if (info.TryGetProperty(NMultiplayerLoadGameScreen.PropertyName._infoPanel, ref variant2))
      this._infoPanel = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NMultiplayerLoadGameScreen.PropertyName._hp, ref variant3))
      this._hp = ((Variant) ref variant3).As<MegaLabel>();
    Variant variant4;
    if (info.TryGetProperty(NMultiplayerLoadGameScreen.PropertyName._gold, ref variant4))
      this._gold = ((Variant) ref variant4).As<MegaLabel>();
    Variant variant5;
    if (info.TryGetProperty(NMultiplayerLoadGameScreen.PropertyName._selectedButton, ref variant5))
      this._selectedButton = ((Variant) ref variant5).As<NCharacterSelectButton>();
    Variant variant6;
    if (info.TryGetProperty(NMultiplayerLoadGameScreen.PropertyName._bgContainer, ref variant6))
      this._bgContainer = ((Variant) ref variant6).As<Control>();
    Variant variant7;
    if (info.TryGetProperty(NMultiplayerLoadGameScreen.PropertyName._confirmButton, ref variant7))
      this._confirmButton = ((Variant) ref variant7).As<NConfirmButton>();
    Variant variant8;
    if (info.TryGetProperty(NMultiplayerLoadGameScreen.PropertyName._backButton, ref variant8))
      this._backButton = ((Variant) ref variant8).As<NBackButton>();
    Variant variant9;
    if (info.TryGetProperty(NMultiplayerLoadGameScreen.PropertyName._unreadyButton, ref variant9))
      this._unreadyButton = ((Variant) ref variant9).As<NBackButton>();
    Variant variant10;
    if (info.TryGetProperty(NMultiplayerLoadGameScreen.PropertyName._ascensionPanel, ref variant10))
      this._ascensionPanel = ((Variant) ref variant10).As<NAscensionPanel>();
    Variant variant11;
    if (info.TryGetProperty(NMultiplayerLoadGameScreen.PropertyName._floorLabel, ref variant11))
      this._floorLabel = ((Variant) ref variant11).As<MegaRichTextLabel>();
    Variant variant12;
    if (info.TryGetProperty(NMultiplayerLoadGameScreen.PropertyName._actLabel, ref variant12))
      this._actLabel = ((Variant) ref variant12).As<MegaRichTextLabel>();
    Variant variant13;
    if (info.TryGetProperty(NMultiplayerLoadGameScreen.PropertyName._remotePlayerContainer, ref variant13))
      this._remotePlayerContainer = ((Variant) ref variant13).As<NRemoteLoadLobbyPlayerContainer>();
    Variant variant14;
    if (info.TryGetProperty(NMultiplayerLoadGameScreen.PropertyName._infoPanelTween, ref variant14))
      this._infoPanelTween = ((Variant) ref variant14).As<Tween>();
    Variant variant15;
    if (!info.TryGetProperty(NMultiplayerLoadGameScreen.PropertyName._infoPanelPosFinalVal, ref variant15))
      return;
    this._infoPanelPosFinalVal = ((Variant) ref variant15).As<Vector2>();
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
    public static readonly StringName AfterMultiplayerStarted = StringName.op_Implicit(nameof (AfterMultiplayerStarted));
  }

  public new class PropertyName : NSubmenu.PropertyName
  {
    public new static readonly StringName InitialFocusedControl = StringName.op_Implicit(nameof (InitialFocusedControl));
    public static readonly StringName _name = StringName.op_Implicit(nameof (_name));
    public static readonly StringName _infoPanel = StringName.op_Implicit(nameof (_infoPanel));
    public static readonly StringName _hp = StringName.op_Implicit(nameof (_hp));
    public static readonly StringName _gold = StringName.op_Implicit(nameof (_gold));
    public static readonly StringName _selectedButton = StringName.op_Implicit(nameof (_selectedButton));
    public static readonly StringName _bgContainer = StringName.op_Implicit(nameof (_bgContainer));
    public static readonly StringName _confirmButton = StringName.op_Implicit(nameof (_confirmButton));
    public new static readonly StringName _backButton = StringName.op_Implicit(nameof (_backButton));
    public static readonly StringName _unreadyButton = StringName.op_Implicit(nameof (_unreadyButton));
    public static readonly StringName _ascensionPanel = StringName.op_Implicit(nameof (_ascensionPanel));
    public static readonly StringName _floorLabel = StringName.op_Implicit(nameof (_floorLabel));
    public static readonly StringName _actLabel = StringName.op_Implicit(nameof (_actLabel));
    public static readonly StringName _remotePlayerContainer = StringName.op_Implicit(nameof (_remotePlayerContainer));
    public static readonly StringName _infoPanelTween = StringName.op_Implicit(nameof (_infoPanelTween));
    public static readonly StringName _infoPanelPosFinalVal = StringName.op_Implicit(nameof (_infoPanelPosFinalVal));
  }

  public new class SignalName : NSubmenu.SignalName
  {
  }
}
