// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.CustomRun.NCustomRunScreen
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.UI;
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
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;
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
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.CustomRun;

[ScriptPath("res://src/Core/Nodes/Screens/CustomRun/NCustomRunScreen.cs")]
public class NCustomRunScreen : NSubmenu, IStartRunLobbyListener, ICharacterSelectButtonDelegate
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("screens/custom_run/custom_run_screen");
  private const string _sceneCharSelectButtonPath = "res://scenes/screens/char_select/char_select_button.tscn";
  private MegaLabel _disclaimer;
  private NCharacterSelectButton? _selectedButton;
  private Control _charButtonContainer;
  private NCustomRunRandomizeButton _randomizeButton;
  private NConfirmButton _confirmButton;
  private NBackButton _backButton;
  private NBackButton _unreadyButton;
  private NAscensionPanel _ascensionPanel;
  private Control _readyAndWaitingContainer;
  private LineEdit _seedInput;
  private NRemoteLobbyPlayerContainer _remotePlayerContainer;
  private NCustomRunModifiersList _modifiersList;
  private TextureRect _modifiersHotkeyIcon;
  private StartRunLobby _lobby;
  private MultiplayerUiMode _uiMode;

  private string ModifiersHotkey => StringName.op_Implicit(MegaInput.topPanel);

  public StartRunLobby Lobby => this._lobby;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[3]
      {
        NCustomRunScreen._scenePath,
        "res://scenes/screens/char_select/char_select_button.tscn",
        "res://scenes/screens/custom_run/modifier_tickbox.tscn"
      });
    }
  }

  protected override Control InitialFocusedControl
  {
    get => ((Node) this._charButtonContainer).GetChild<Control>(0, false);
  }

  public static NCustomRunScreen? Create()
  {
    return TestMode.IsOn ? (NCustomRunScreen) null : PreloadManager.Cache.GetScene(NCustomRunScreen._scenePath).Instantiate<NCustomRunScreen>((PackedScene.GenEditState) 0L);
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._disclaimer = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%Disclaimer"));
    this._charButtonContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("LeftContainer/CharSelectButtons/ButtonContainer"));
    this._ascensionPanel = ((Node) this).GetNode<NAscensionPanel>(NodePath.op_Implicit("%AscensionPanel"));
    this._remotePlayerContainer = ((Node) this).GetNode<NRemoteLobbyPlayerContainer>(NodePath.op_Implicit("%RemotePlayerContainer"));
    this._readyAndWaitingContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%ReadyAndWaitingPanel"));
    this._modifiersList = ((Node) this).GetNode<NCustomRunModifiersList>(NodePath.op_Implicit("%ModifiersList"));
    this._seedInput = ((Node) this).GetNode<LineEdit>(NodePath.op_Implicit("%SeedInput"));
    this._randomizeButton = ((Node) this).GetNode<NCustomRunRandomizeButton>(NodePath.op_Implicit("%CustomRunRandomizeButton"));
    this._confirmButton = ((Node) this).GetNode<NConfirmButton>(NodePath.op_Implicit("ConfirmButton"));
    this._backButton = ((Node) this).GetNode<NBackButton>(NodePath.op_Implicit("BackButton"));
    this._unreadyButton = ((Node) this).GetNode<NBackButton>(NodePath.op_Implicit("UnreadyButton"));
    this._modifiersHotkeyIcon = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%ModifiersHotkeyIcon"));
    ((GodotObject) this._randomizeButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnRandomizePressed)), 0U);
    ((GodotObject) this._confirmButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnEmbarkPressed)), 0U);
    ((GodotObject) this._unreadyButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnUnreadyPressed)), 0U);
    ((GodotObject) this._ascensionPanel).Connect(NAscensionPanel.SignalName.AscensionLevelChanged, Callable.From(new Action(this.OnAscensionPanelLevelChanged)), 0U);
    ((GodotObject) this._modifiersList).Connect(NCustomRunModifiersList.SignalName.ModifiersChanged, Callable.From(new Action(this.OnModifiersListChanged)), 0U);
    this._disclaimer.SetTextAutoSize(new LocString("main_menu_ui", "CUSTOM_RUN_SCREEN.disclaimer").GetFormattedText());
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%CustomModeTitle")).SetTextAutoSize(new LocString("main_menu_ui", "CUSTOM_RUN_SCREEN.CUSTOM_MODE_TITLE").GetFormattedText());
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%ModifiersTitle")).SetTextAutoSize(new LocString("main_menu_ui", "CUSTOM_RUN_SCREEN.MODIFIERS_TITLE").GetFormattedText());
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%SeedLabel")).SetTextAutoSize(new LocString("main_menu_ui", "CUSTOM_RUN_SCREEN.SEED_LABEL").GetFormattedText());
    this._seedInput.PlaceholderText = new LocString("main_menu_ui", "CUSTOM_RUN_SCREEN.SEED_RANDOM_PLACEHOLDER").GetFormattedText();
    ((GodotObject) this._seedInput).Connect(LineEdit.SignalName.TextChanged, Callable.From<string>(new Action<string>(this.OnSeedInputSubmitted)), 0U);
    this.InitCharacterButtons();
    ((GodotObject) NControllerManager.Instance).Connect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.UpdateControllerButton)), 0U);
    ((GodotObject) NControllerManager.Instance).Connect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.UpdateControllerButton)), 0U);
    ((GodotObject) NInputManager.Instance).Connect(NInputManager.SignalName.InputRebound, Callable.From(new Action(this.UpdateControllerButton)), 0U);
  }

  public void InitializeMultiplayerAsHost(INetGameService gameService, int maxPlayers)
  {
    this._lobby = gameService.Type == NetGameType.Host ? new StartRunLobby(GameMode.Custom, gameService, (IStartRunLobbyListener) this, maxPlayers) : throw new InvalidOperationException($"Initialized character select screen with GameService of type {gameService.Type} when hosting!");
    this._ascensionPanel.Initialize(MultiplayerUiMode.Host);
    this._modifiersList.Initialize(MultiplayerUiMode.Host);
    this._lobby.AddLocalHostPlayer(new UnlockState(SaveManager.Instance.Progress), SaveManager.Instance.Progress.MaxMultiplayerAscension);
    this._uiMode = MultiplayerUiMode.Host;
    ((CanvasItem) this._remotePlayerContainer).Visible = true;
    this.UpdateControllerButton();
    this.OnAscensionPanelLevelChanged();
    this.AfterInitialized();
  }

  public void InitializeMultiplayerAsClient(
    INetGameService gameService,
    ClientLobbyJoinResponseMessage message)
  {
    this._lobby = gameService.Type == NetGameType.Client ? new StartRunLobby(GameMode.Custom, gameService, (IStartRunLobbyListener) this, -1) : throw new InvalidOperationException($"Initialized character select screen with GameService of type {gameService.Type} when joining!");
    this._ascensionPanel.Initialize(MultiplayerUiMode.Client);
    this._modifiersList.Initialize(MultiplayerUiMode.Client);
    this._lobby.InitializeFromMessage(message);
    this._seedInput.Editable = false;
    this._randomizeButton.Disable();
    this._uiMode = MultiplayerUiMode.Client;
    this.UpdateControllerButton();
    this.AfterInitialized();
  }

  public void InitializeSingleplayer()
  {
    this._lobby = new StartRunLobby(GameMode.Custom, (INetGameService) new NetSingleplayerGameService(), (IStartRunLobbyListener) this, 1);
    ((CanvasItem) this._remotePlayerContainer).Visible = false;
    this._ascensionPanel.Initialize(MultiplayerUiMode.Singleplayer);
    this._modifiersList.Initialize(MultiplayerUiMode.Singleplayer);
    this._lobby.AddLocalHostPlayer(new UnlockState(SaveManager.Instance.Progress), 0);
    this._uiMode = MultiplayerUiMode.Singleplayer;
    this.UpdateControllerButton();
    this.AfterInitialized();
  }

  private void OnSeedInputSubmitted(string newText)
  {
    if (newText != string.Empty)
      this.Lobby.SetSeed(newText);
    else
      this.Lobby.SetSeed((string) null);
  }

  private void InitCharacterButtons()
  {
    foreach (CharacterModel allCharacter in ModelDb.AllCharacters)
    {
      NCharacterSelectButton child = PreloadManager.Cache.GetScene("res://scenes/screens/char_select/char_select_button.tscn").Instantiate<NCharacterSelectButton>((PackedScene.GenEditState) 0L);
      ((Node) child).Name = StringName.op_Implicit(allCharacter.Id.Entry + "_button");
      ((Node) this._charButtonContainer).AddChildSafely((Node) child);
      child.Init(allCharacter, (ICharacterSelectButtonDelegate) this);
    }
    for (int index = 0; index < ((Node) this._charButtonContainer).GetChildCount(false); ++index)
    {
      Control child = ((Node) this._charButtonContainer).GetChild<Control>(index, false);
      child.FocusNeighborLeft = index > 0 ? ((Node) ((Node) this._charButtonContainer).GetChild<Control>(index - 1, false)).GetPath() : ((Node) child).GetPath();
      child.FocusNeighborRight = index < ((Node) this._charButtonContainer).GetChildCount(false) - 1 ? ((Node) ((Node) this._charButtonContainer).GetChild<Control>(index + 1, false)).GetPath() : ((Node) child).GetPath();
      child.FocusNeighborTop = ((Node) this._seedInput).GetPath();
      child.FocusNeighborBottom = ((Node) child).GetPath();
    }
  }

  public override void _Input(InputEvent inputEvent)
  {
    if (!inputEvent.IsActionReleased(DebugHotkey.unlockCharacters, false))
      return;
    this.DebugUnlockAllCharacters();
  }

  private void DebugUnlockAllCharacters()
  {
    foreach (NCharacterSelectButton ncharacterSelectButton in ((IEnumerable) ((Node) this._charButtonContainer).GetChildren(false)).OfType<NCharacterSelectButton>())
      ncharacterSelectButton.DebugUnlock();
  }

  public override void OnSubmenuOpened()
  {
    base.OnSubmenuOpened();
    foreach (NCharacterSelectButton ncharacterSelectButton in ((IEnumerable) ((Node) this._charButtonContainer).GetChildren(false)).OfType<NCharacterSelectButton>())
    {
      if (!ncharacterSelectButton.IsLocked)
      {
        ncharacterSelectButton.Enable();
        ncharacterSelectButton.Reset();
      }
      else
        ncharacterSelectButton.UnlockIfPossible();
    }
    this._confirmButton.Enable();
    ((Node) this._charButtonContainer).GetChild<NCharacterSelectButton>(0, false).Select();
    this._remotePlayerContainer.Initialize(this._lobby, true);
    if (this._lobby.NetService.Type == NetGameType.Client)
    {
      this._ascensionPanel.SetAscensionLevel(this._lobby.Ascension);
      this._seedInput.Text = this._lobby.Seed ?? "";
    }
    ((CanvasItem) this._readyAndWaitingContainer).Visible = false;
    foreach (LobbyPlayer player in this._lobby.Players)
      this.RefreshButtonSelectionForPlayer(player);
    NHotkeyManager.Instance.PushHotkeyPressedBinding(this.ModifiersHotkey, new Action(this.TryFocusOnModifiersList));
  }

  public override void OnSubmenuClosed()
  {
    base.OnSubmenuClosed();
    this._confirmButton.Disable();
    this._remotePlayerContainer.Cleanup();
    if (this._lobby.NetService.Type.IsMultiplayer())
      PlatformUtil.SetRichPresence("MAIN_MENU", (string) null, new int?());
    this.CleanUpLobby(true);
    NHotkeyManager.Instance.RemoveHotkeyPressedBinding(this.ModifiersHotkey, new Action(this.TryFocusOnModifiersList));
  }

  private void OnRandomizePressed(NButton _)
  {
    if (this._lobby.NetService.Type == NetGameType.Client)
      throw new InvalidOperationException("In multiplayer, only the host can randomize custom runs.");
    if (this._lobby.NetService.Type == NetGameType.Singleplayer)
      this.RandomizeLocalCharacter();
    this._modifiersList.SetTickedModifiers(ModifierModel.Pick2Good1Bad(Rng.Chaotic, this._lobby.Players.Select<LobbyPlayer, CharacterModel>((Func<LobbyPlayer, CharacterModel>) (p => p.character))));
  }

  private void RandomizeLocalCharacter()
  {
    Rng.Chaotic.NextItem<NCharacterSelectButton>(((IEnumerable) ((Node) this._charButtonContainer).GetChildren(false)).OfType<NCharacterSelectButton>().Where<NCharacterSelectButton>((Func<NCharacterSelectButton, bool>) (b => b != null && !b.IsLocked && !b.IsRandom)))?.Select();
  }

  private void OnEmbarkPressed(NButton _)
  {
    this._randomizeButton.Disable();
    this._confirmButton.Disable();
    this._backButton.Disable();
    this._lobby.SetReady(true);
    foreach (NClickableControl nclickableControl in ((IEnumerable) ((Node) this._charButtonContainer).GetChildren(false)).OfType<NCharacterSelectButton>())
      nclickableControl.Disable();
    if (!this._lobby.NetService.Type.IsMultiplayer() || this._lobby.IsAboutToBeginGame())
      return;
    this._unreadyButton.Enable();
    ((CanvasItem) this._readyAndWaitingContainer).Visible = true;
  }

  private void OnUnreadyPressed(NButton _)
  {
    if (this._lobby.NetService.Type != NetGameType.Client)
      this._randomizeButton.Enable();
    this._confirmButton.Enable();
    this._backButton.Enable();
    this._unreadyButton.Disable();
    this._lobby.SetReady(false);
    ((CanvasItem) this._readyAndWaitingContainer).Visible = false;
    foreach (NClickableControl nclickableControl in ((IEnumerable) ((Node) this._charButtonContainer).GetChildren(false)).OfType<NCharacterSelectButton>())
      nclickableControl.Enable();
  }

  private void UpdateRichPresence()
  {
    if (!this._lobby.NetService.Type.IsMultiplayer())
      return;
    PlatformUtil.SetRichPresence("CUSTOM_MP_LOBBY", this._lobby.NetService.GetRawLobbyIdentifier(), new int?(this._lobby.Players.Count));
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
    this._lobby = (StartRunLobby) null;
  }

  private async Task StartNewSingleplayerRun(
    string seed,
    List<ActModel> acts,
    IReadOnlyList<ModifierModel> modifiers)
  {
    int num = 0;
    object obj;
    try
    {
      Log.Info($"Embarking on a CUSTOM {this._lobby.LocalPlayer.character.Id.Entry} run. Ascension: {this._lobby.Ascension} Seed: {this._lobby.Seed} Modifiers: {this.GetModifiersString()}");
      SfxCmd.Play(this._lobby.LocalPlayer.character.CharacterTransitionSfx);
      await NGame.Instance.Transition.FadeOut(transitionPath: this._lobby.LocalPlayer.character.CharacterSelectTransitionPath);
      RunState runState = await NGame.Instance.StartNewSingleplayerRun(this._lobby.LocalPlayer.character, true, (IReadOnlyList<ActModel>) acts, modifiers, seed, GameMode.Custom, this._lobby.Ascension);
    }
    catch (Exception ex)
    {
      obj = (object) ex;
      num = 1;
    }
    if (num == 1)
    {
      Exception e = (Exception) obj;
      Log.Error($"Exception starting custom singleplayer run : {e}");
      this.CleanUpLobby(true, NetError.InternalError);
      await NGame.Instance.ReturnToMainMenuWithInternalError(e);
    }
    else
    {
      obj = (object) null;
      this.CleanUpLobby(false);
    }
  }

  private async Task StartNewMultiplayerRun(
    string seed,
    List<ActModel> acts,
    IReadOnlyList<ModifierModel> modifiers)
  {
    int num = 0;
    object obj;
    try
    {
      Log.Info($"Embarking on a CUSTOM multiplayer run. Players: {string.Join<LobbyPlayer>(",", (IEnumerable<LobbyPlayer>) this._lobby.Players)}. Ascension: {this._lobby.Ascension} Seed: {this._lobby.Seed} Modifiers: {this.GetModifiersString()}");
      SfxCmd.Play(this._lobby.LocalPlayer.character.CharacterTransitionSfx);
      await NGame.Instance.Transition.FadeOut(transitionPath: this._lobby.LocalPlayer.character.CharacterSelectTransitionPath);
      RunState runState = await NGame.Instance.StartNewMultiplayerRun(this._lobby, true, (IReadOnlyList<ActModel>) acts, modifiers, seed, this._lobby.Ascension);
    }
    catch (Exception ex)
    {
      obj = (object) ex;
      num = 1;
    }
    if (num == 1)
    {
      Exception e = (Exception) obj;
      Log.Error($"Exception starting custom multiplayer run : {e}");
      this.CleanUpLobby(true, NetError.InternalError);
      await NGame.Instance.ReturnToMainMenuWithInternalError(e);
    }
    else
    {
      obj = (object) null;
      this.CleanUpLobby(false);
    }
  }

  private string GetModifiersString()
  {
    return string.Join<ModelId>(",", this._lobby.Modifiers.Select<ModifierModel, ModelId>((Func<ModifierModel, ModelId>) (m => m.Id)));
  }

  public void SelectCharacter(
    NCharacterSelectButton charSelectButton,
    CharacterModel characterModel)
  {
    if (this._lobby == null)
      throw new InvalidOperationException("Cannot select character while loading!");
    SfxCmd.Play(characterModel.CharacterSelectSfx);
    this._selectedButton = charSelectButton;
    foreach (NCharacterSelectButton ncharacterSelectButton in ((IEnumerable) ((Node) this._charButtonContainer).GetChildren(false)).OfType<NCharacterSelectButton>())
    {
      if (ncharacterSelectButton != this._selectedButton)
        ncharacterSelectButton.Deselect();
    }
    this._lobby.SetLocalCharacter(characterModel);
  }

  private void OnAscensionPanelLevelChanged()
  {
    if (this._lobby.NetService.Type == NetGameType.Client || this._lobby.Ascension == this._ascensionPanel.Ascension)
      return;
    this._lobby.SyncAscensionChange(this._ascensionPanel.Ascension);
  }

  private void OnModifiersListChanged()
  {
    if (this._lobby.NetService.Type == NetGameType.Client)
      return;
    this.Lobby.SetModifiers((IReadOnlyCollection<ModifierModel>) this._modifiersList.GetModifiersTickedOn());
  }

  public void MaxAscensionChanged()
  {
    this._ascensionPanel.SetMaxAscension(this._lobby.MaxAscension);
  }

  public void PlayerConnected(LobbyPlayer player)
  {
    this._remotePlayerContainer.OnPlayerConnected(player);
    this.RefreshButtonSelectionForPlayer(player);
    this.UpdateRichPresence();
  }

  public void PlayerChanged(LobbyPlayer player, bool isRandomCharacterResolution)
  {
    if (isRandomCharacterResolution)
      throw new InvalidOperationException("Random character is not currently allowed in custom!");
    this._remotePlayerContainer.OnPlayerChanged(player);
    this.RefreshButtonSelectionForPlayer(player);
  }

  private void RefreshButtonSelectionForPlayer(LobbyPlayer player)
  {
    if ((long) player.id == (long) this._lobby.LocalPlayer.id)
      return;
    foreach (NCharacterSelectButton ncharacterSelectButton in ((IEnumerable) ((Node) this._charButtonContainer).GetChildren(false)).OfType<NCharacterSelectButton>())
    {
      if (ncharacterSelectButton.RemoteSelectedPlayers.Contains<ulong>(player.id) && player.character != ncharacterSelectButton.Character)
        ncharacterSelectButton.OnRemotePlayerDeselected(player.id);
      else if (player.character == ncharacterSelectButton.Character)
        ncharacterSelectButton.OnRemotePlayerSelected(player.id);
    }
  }

  public void AscensionChanged()
  {
    if (this._lobby.NetService.Type == NetGameType.Client)
      ((CanvasItem) this._ascensionPanel).Visible = this._lobby.Ascension > 0;
    this._ascensionPanel.SetAscensionLevel(this._lobby.Ascension);
  }

  public void SeedChanged()
  {
    bool flag;
    switch (this._lobby.NetService.Type)
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
      return;
    this._seedInput.Text = this.Lobby.Seed;
  }

  public void ModifiersChanged()
  {
    bool flag;
    switch (this._lobby.NetService.Type)
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
      return;
    this._modifiersList.SyncModifierList((IReadOnlyCollection<ModifierModel>) this.Lobby.Modifiers);
  }

  public void RemotePlayerDisconnected(LobbyPlayer player)
  {
    this._remotePlayerContainer.OnPlayerDisconnected(player);
    foreach (NCharacterSelectButton ncharacterSelectButton in ((IEnumerable) ((Node) this._charButtonContainer).GetChildren(false)).OfType<NCharacterSelectButton>())
    {
      if (ncharacterSelectButton.RemoteSelectedPlayers.Contains<ulong>(player.id) && player.character == ncharacterSelectButton.Character)
        ncharacterSelectButton.OnRemotePlayerDeselected(player.id);
    }
    this.UpdateRichPresence();
  }

  public void BeginRun(string seed, List<ActModel> acts, IReadOnlyList<ModifierModel> modifiers)
  {
    NAudioManager.Instance?.StopMusic();
    this._confirmButton.Disable();
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

  private void AfterInitialized()
  {
    NGame.Instance.RemoteCursorContainer.Initialize(this._lobby.InputSynchronizer, this._lobby.Players.Select<LobbyPlayer, ulong>((Func<LobbyPlayer, ulong>) (p => p.id)));
    NGame.Instance.ReactionContainer.InitializeNetworking(this._lobby.NetService);
    NGame.Instance.TimeoutOverlay.Initialize(this._lobby.NetService, true);
    this.UpdateRichPresence();
    if (!string.IsNullOrEmpty(this._seedInput.Text))
      this._lobby.SetSeed(this._seedInput.Text);
    Logger.logLevelTypeMap[LogType.Network] = this._lobby.NetService.Type == NetGameType.Singleplayer ? LogLevel.Info : LogLevel.Debug;
    Logger.logLevelTypeMap[LogType.Actions] = this._lobby.NetService.Type == NetGameType.Singleplayer ? LogLevel.Info : LogLevel.VeryDebug;
    Logger.logLevelTypeMap[LogType.GameSync] = this._lobby.NetService.Type == NetGameType.Singleplayer ? LogLevel.Info : LogLevel.VeryDebug;
    NGame.Instance.DebugSeedOverride = (string) null;
  }

  private void UpdateControllerButton()
  {
    bool flag;
    switch (this._uiMode)
    {
      case MultiplayerUiMode.Singleplayer:
      case MultiplayerUiMode.Host:
        flag = true;
        break;
      default:
        flag = false;
        break;
    }
    if (flag)
    {
      ((CanvasItem) this._modifiersHotkeyIcon).Visible = NControllerManager.Instance.IsUsingController;
      this._modifiersHotkeyIcon.Texture = NInputManager.Instance.GetHotkeyIcon(this.ModifiersHotkey);
    }
    else
      ((CanvasItem) this._modifiersHotkeyIcon).Visible = false;
  }

  private void TryFocusOnModifiersList()
  {
    Control focusOwner = ((Node) this).GetViewport().GuiGetFocusOwner();
    if (focusOwner != null && ((Node) this._modifiersList).IsAncestorOf((Node) focusOwner))
      return;
    bool flag;
    switch (this._uiMode)
    {
      case MultiplayerUiMode.Singleplayer:
      case MultiplayerUiMode.Host:
        flag = true;
        break;
      default:
        flag = false;
        break;
    }
    if (!flag)
      return;
    Control defaultFocusedControl = this._modifiersList.DefaultFocusedControl;
    if (defaultFocusedControl == null)
      return;
    defaultFocusedControl.TryGrabFocus();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(26)
    {
      new MethodInfo(NCustomRunScreen.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCustomRunScreen.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCustomRunScreen.MethodName.InitializeSingleplayer, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCustomRunScreen.MethodName.OnSeedInputSubmitted, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("newText"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCustomRunScreen.MethodName.InitCharacterButtons, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCustomRunScreen.MethodName._Input, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCustomRunScreen.MethodName.DebugUnlockAllCharacters, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCustomRunScreen.MethodName.OnSubmenuOpened, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCustomRunScreen.MethodName.OnSubmenuClosed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCustomRunScreen.MethodName.OnRandomizePressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCustomRunScreen.MethodName.RandomizeLocalCharacter, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCustomRunScreen.MethodName.OnEmbarkPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCustomRunScreen.MethodName.OnUnreadyPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCustomRunScreen.MethodName.UpdateRichPresence, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCustomRunScreen.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCustomRunScreen.MethodName.CleanUpLobby, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("disconnectSession"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("error"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCustomRunScreen.MethodName.GetModifiersString, new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCustomRunScreen.MethodName.OnAscensionPanelLevelChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCustomRunScreen.MethodName.OnModifiersListChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCustomRunScreen.MethodName.MaxAscensionChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCustomRunScreen.MethodName.AscensionChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCustomRunScreen.MethodName.SeedChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCustomRunScreen.MethodName.ModifiersChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCustomRunScreen.MethodName.AfterInitialized, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCustomRunScreen.MethodName.UpdateControllerButton, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCustomRunScreen.MethodName.TryFocusOnModifiersList, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCustomRunScreen.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NCustomRunScreen ncustomRunScreen = NCustomRunScreen.Create();
      ret = VariantUtils.CreateFrom<NCustomRunScreen>(ref ncustomRunScreen);
      return true;
    }
    if (StringName.op_Equality(ref method, NCustomRunScreen.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCustomRunScreen.MethodName.InitializeSingleplayer) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.InitializeSingleplayer();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCustomRunScreen.MethodName.OnSeedInputSubmitted) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnSeedInputSubmitted(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCustomRunScreen.MethodName.InitCharacterButtons) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.InitCharacterButtons();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCustomRunScreen.MethodName._Input) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Input(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCustomRunScreen.MethodName.DebugUnlockAllCharacters) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.DebugUnlockAllCharacters();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCustomRunScreen.MethodName.OnSubmenuOpened) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSubmenuOpened();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCustomRunScreen.MethodName.OnSubmenuClosed) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSubmenuClosed();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCustomRunScreen.MethodName.OnRandomizePressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnRandomizePressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCustomRunScreen.MethodName.RandomizeLocalCharacter) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RandomizeLocalCharacter();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCustomRunScreen.MethodName.OnEmbarkPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnEmbarkPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCustomRunScreen.MethodName.OnUnreadyPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnUnreadyPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCustomRunScreen.MethodName.UpdateRichPresence) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateRichPresence();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCustomRunScreen.MethodName._Process) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCustomRunScreen.MethodName.CleanUpLobby) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.CleanUpLobby(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<NetError>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCustomRunScreen.MethodName.GetModifiersString) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      string modifiersString = this.GetModifiersString();
      ret = VariantUtils.CreateFrom<string>(ref modifiersString);
      return true;
    }
    if (StringName.op_Equality(ref method, NCustomRunScreen.MethodName.OnAscensionPanelLevelChanged) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnAscensionPanelLevelChanged();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCustomRunScreen.MethodName.OnModifiersListChanged) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnModifiersListChanged();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCustomRunScreen.MethodName.MaxAscensionChanged) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.MaxAscensionChanged();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCustomRunScreen.MethodName.AscensionChanged) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AscensionChanged();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCustomRunScreen.MethodName.SeedChanged) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SeedChanged();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCustomRunScreen.MethodName.ModifiersChanged) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ModifiersChanged();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCustomRunScreen.MethodName.AfterInitialized) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AfterInitialized();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCustomRunScreen.MethodName.UpdateControllerButton) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateControllerButton();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCustomRunScreen.MethodName.TryFocusOnModifiersList) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.TryFocusOnModifiersList();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCustomRunScreen.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NCustomRunScreen ncustomRunScreen = NCustomRunScreen.Create();
      ret = VariantUtils.CreateFrom<NCustomRunScreen>(ref ncustomRunScreen);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCustomRunScreen.MethodName.Create) || StringName.op_Equality(ref method, NCustomRunScreen.MethodName._Ready) || StringName.op_Equality(ref method, NCustomRunScreen.MethodName.InitializeSingleplayer) || StringName.op_Equality(ref method, NCustomRunScreen.MethodName.OnSeedInputSubmitted) || StringName.op_Equality(ref method, NCustomRunScreen.MethodName.InitCharacterButtons) || StringName.op_Equality(ref method, NCustomRunScreen.MethodName._Input) || StringName.op_Equality(ref method, NCustomRunScreen.MethodName.DebugUnlockAllCharacters) || StringName.op_Equality(ref method, NCustomRunScreen.MethodName.OnSubmenuOpened) || StringName.op_Equality(ref method, NCustomRunScreen.MethodName.OnSubmenuClosed) || StringName.op_Equality(ref method, NCustomRunScreen.MethodName.OnRandomizePressed) || StringName.op_Equality(ref method, NCustomRunScreen.MethodName.RandomizeLocalCharacter) || StringName.op_Equality(ref method, NCustomRunScreen.MethodName.OnEmbarkPressed) || StringName.op_Equality(ref method, NCustomRunScreen.MethodName.OnUnreadyPressed) || StringName.op_Equality(ref method, NCustomRunScreen.MethodName.UpdateRichPresence) || StringName.op_Equality(ref method, NCustomRunScreen.MethodName._Process) || StringName.op_Equality(ref method, NCustomRunScreen.MethodName.CleanUpLobby) || StringName.op_Equality(ref method, NCustomRunScreen.MethodName.GetModifiersString) || StringName.op_Equality(ref method, NCustomRunScreen.MethodName.OnAscensionPanelLevelChanged) || StringName.op_Equality(ref method, NCustomRunScreen.MethodName.OnModifiersListChanged) || StringName.op_Equality(ref method, NCustomRunScreen.MethodName.MaxAscensionChanged) || StringName.op_Equality(ref method, NCustomRunScreen.MethodName.AscensionChanged) || StringName.op_Equality(ref method, NCustomRunScreen.MethodName.SeedChanged) || StringName.op_Equality(ref method, NCustomRunScreen.MethodName.ModifiersChanged) || StringName.op_Equality(ref method, NCustomRunScreen.MethodName.AfterInitialized) || StringName.op_Equality(ref method, NCustomRunScreen.MethodName.UpdateControllerButton) || StringName.op_Equality(ref method, NCustomRunScreen.MethodName.TryFocusOnModifiersList) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCustomRunScreen.PropertyName._disclaimer))
    {
      this._disclaimer = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCustomRunScreen.PropertyName._selectedButton))
    {
      this._selectedButton = VariantUtils.ConvertTo<NCharacterSelectButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCustomRunScreen.PropertyName._charButtonContainer))
    {
      this._charButtonContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCustomRunScreen.PropertyName._randomizeButton))
    {
      this._randomizeButton = VariantUtils.ConvertTo<NCustomRunRandomizeButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCustomRunScreen.PropertyName._confirmButton))
    {
      this._confirmButton = VariantUtils.ConvertTo<NConfirmButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCustomRunScreen.PropertyName._backButton))
    {
      this._backButton = VariantUtils.ConvertTo<NBackButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCustomRunScreen.PropertyName._unreadyButton))
    {
      this._unreadyButton = VariantUtils.ConvertTo<NBackButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCustomRunScreen.PropertyName._ascensionPanel))
    {
      this._ascensionPanel = VariantUtils.ConvertTo<NAscensionPanel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCustomRunScreen.PropertyName._readyAndWaitingContainer))
    {
      this._readyAndWaitingContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCustomRunScreen.PropertyName._seedInput))
    {
      this._seedInput = VariantUtils.ConvertTo<LineEdit>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCustomRunScreen.PropertyName._remotePlayerContainer))
    {
      this._remotePlayerContainer = VariantUtils.ConvertTo<NRemoteLobbyPlayerContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCustomRunScreen.PropertyName._modifiersList))
    {
      this._modifiersList = VariantUtils.ConvertTo<NCustomRunModifiersList>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCustomRunScreen.PropertyName._modifiersHotkeyIcon))
    {
      this._modifiersHotkeyIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCustomRunScreen.PropertyName._uiMode))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._uiMode = VariantUtils.ConvertTo<MultiplayerUiMode>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCustomRunScreen.PropertyName.ModifiersHotkey))
    {
      ref godot_variant local = ref value;
      string modifiersHotkey = this.ModifiersHotkey;
      godot_variant from = VariantUtils.CreateFrom<string>(ref modifiersHotkey);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCustomRunScreen.PropertyName.InitialFocusedControl))
    {
      ref godot_variant local = ref value;
      Control initialFocusedControl = this.InitialFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref initialFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCustomRunScreen.PropertyName._disclaimer))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._disclaimer);
      return true;
    }
    if (StringName.op_Equality(ref name, NCustomRunScreen.PropertyName._selectedButton))
    {
      value = VariantUtils.CreateFrom<NCharacterSelectButton>(ref this._selectedButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NCustomRunScreen.PropertyName._charButtonContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._charButtonContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NCustomRunScreen.PropertyName._randomizeButton))
    {
      value = VariantUtils.CreateFrom<NCustomRunRandomizeButton>(ref this._randomizeButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NCustomRunScreen.PropertyName._confirmButton))
    {
      value = VariantUtils.CreateFrom<NConfirmButton>(ref this._confirmButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NCustomRunScreen.PropertyName._backButton))
    {
      value = VariantUtils.CreateFrom<NBackButton>(ref this._backButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NCustomRunScreen.PropertyName._unreadyButton))
    {
      value = VariantUtils.CreateFrom<NBackButton>(ref this._unreadyButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NCustomRunScreen.PropertyName._ascensionPanel))
    {
      value = VariantUtils.CreateFrom<NAscensionPanel>(ref this._ascensionPanel);
      return true;
    }
    if (StringName.op_Equality(ref name, NCustomRunScreen.PropertyName._readyAndWaitingContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._readyAndWaitingContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NCustomRunScreen.PropertyName._seedInput))
    {
      value = VariantUtils.CreateFrom<LineEdit>(ref this._seedInput);
      return true;
    }
    if (StringName.op_Equality(ref name, NCustomRunScreen.PropertyName._remotePlayerContainer))
    {
      value = VariantUtils.CreateFrom<NRemoteLobbyPlayerContainer>(ref this._remotePlayerContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NCustomRunScreen.PropertyName._modifiersList))
    {
      value = VariantUtils.CreateFrom<NCustomRunModifiersList>(ref this._modifiersList);
      return true;
    }
    if (StringName.op_Equality(ref name, NCustomRunScreen.PropertyName._modifiersHotkeyIcon))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._modifiersHotkeyIcon);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCustomRunScreen.PropertyName._uiMode))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<MultiplayerUiMode>(ref this._uiMode);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 4L, NCustomRunScreen.PropertyName.ModifiersHotkey, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCustomRunScreen.PropertyName._disclaimer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCustomRunScreen.PropertyName._selectedButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCustomRunScreen.PropertyName._charButtonContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCustomRunScreen.PropertyName._randomizeButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCustomRunScreen.PropertyName._confirmButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCustomRunScreen.PropertyName._backButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCustomRunScreen.PropertyName._unreadyButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCustomRunScreen.PropertyName._ascensionPanel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCustomRunScreen.PropertyName._readyAndWaitingContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCustomRunScreen.PropertyName._seedInput, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCustomRunScreen.PropertyName._remotePlayerContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCustomRunScreen.PropertyName._modifiersList, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCustomRunScreen.PropertyName._modifiersHotkeyIcon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCustomRunScreen.PropertyName.InitialFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NCustomRunScreen.PropertyName._uiMode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NCustomRunScreen.PropertyName._disclaimer, Variant.From<MegaLabel>(ref this._disclaimer));
    info.AddProperty(NCustomRunScreen.PropertyName._selectedButton, Variant.From<NCharacterSelectButton>(ref this._selectedButton));
    info.AddProperty(NCustomRunScreen.PropertyName._charButtonContainer, Variant.From<Control>(ref this._charButtonContainer));
    info.AddProperty(NCustomRunScreen.PropertyName._randomizeButton, Variant.From<NCustomRunRandomizeButton>(ref this._randomizeButton));
    info.AddProperty(NCustomRunScreen.PropertyName._confirmButton, Variant.From<NConfirmButton>(ref this._confirmButton));
    info.AddProperty(NCustomRunScreen.PropertyName._backButton, Variant.From<NBackButton>(ref this._backButton));
    info.AddProperty(NCustomRunScreen.PropertyName._unreadyButton, Variant.From<NBackButton>(ref this._unreadyButton));
    info.AddProperty(NCustomRunScreen.PropertyName._ascensionPanel, Variant.From<NAscensionPanel>(ref this._ascensionPanel));
    info.AddProperty(NCustomRunScreen.PropertyName._readyAndWaitingContainer, Variant.From<Control>(ref this._readyAndWaitingContainer));
    info.AddProperty(NCustomRunScreen.PropertyName._seedInput, Variant.From<LineEdit>(ref this._seedInput));
    info.AddProperty(NCustomRunScreen.PropertyName._remotePlayerContainer, Variant.From<NRemoteLobbyPlayerContainer>(ref this._remotePlayerContainer));
    info.AddProperty(NCustomRunScreen.PropertyName._modifiersList, Variant.From<NCustomRunModifiersList>(ref this._modifiersList));
    info.AddProperty(NCustomRunScreen.PropertyName._modifiersHotkeyIcon, Variant.From<TextureRect>(ref this._modifiersHotkeyIcon));
    info.AddProperty(NCustomRunScreen.PropertyName._uiMode, Variant.From<MultiplayerUiMode>(ref this._uiMode));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCustomRunScreen.PropertyName._disclaimer, ref variant1))
      this._disclaimer = ((Variant) ref variant1).As<MegaLabel>();
    Variant variant2;
    if (info.TryGetProperty(NCustomRunScreen.PropertyName._selectedButton, ref variant2))
      this._selectedButton = ((Variant) ref variant2).As<NCharacterSelectButton>();
    Variant variant3;
    if (info.TryGetProperty(NCustomRunScreen.PropertyName._charButtonContainer, ref variant3))
      this._charButtonContainer = ((Variant) ref variant3).As<Control>();
    Variant variant4;
    if (info.TryGetProperty(NCustomRunScreen.PropertyName._randomizeButton, ref variant4))
      this._randomizeButton = ((Variant) ref variant4).As<NCustomRunRandomizeButton>();
    Variant variant5;
    if (info.TryGetProperty(NCustomRunScreen.PropertyName._confirmButton, ref variant5))
      this._confirmButton = ((Variant) ref variant5).As<NConfirmButton>();
    Variant variant6;
    if (info.TryGetProperty(NCustomRunScreen.PropertyName._backButton, ref variant6))
      this._backButton = ((Variant) ref variant6).As<NBackButton>();
    Variant variant7;
    if (info.TryGetProperty(NCustomRunScreen.PropertyName._unreadyButton, ref variant7))
      this._unreadyButton = ((Variant) ref variant7).As<NBackButton>();
    Variant variant8;
    if (info.TryGetProperty(NCustomRunScreen.PropertyName._ascensionPanel, ref variant8))
      this._ascensionPanel = ((Variant) ref variant8).As<NAscensionPanel>();
    Variant variant9;
    if (info.TryGetProperty(NCustomRunScreen.PropertyName._readyAndWaitingContainer, ref variant9))
      this._readyAndWaitingContainer = ((Variant) ref variant9).As<Control>();
    Variant variant10;
    if (info.TryGetProperty(NCustomRunScreen.PropertyName._seedInput, ref variant10))
      this._seedInput = ((Variant) ref variant10).As<LineEdit>();
    Variant variant11;
    if (info.TryGetProperty(NCustomRunScreen.PropertyName._remotePlayerContainer, ref variant11))
      this._remotePlayerContainer = ((Variant) ref variant11).As<NRemoteLobbyPlayerContainer>();
    Variant variant12;
    if (info.TryGetProperty(NCustomRunScreen.PropertyName._modifiersList, ref variant12))
      this._modifiersList = ((Variant) ref variant12).As<NCustomRunModifiersList>();
    Variant variant13;
    if (info.TryGetProperty(NCustomRunScreen.PropertyName._modifiersHotkeyIcon, ref variant13))
      this._modifiersHotkeyIcon = ((Variant) ref variant13).As<TextureRect>();
    Variant variant14;
    if (!info.TryGetProperty(NCustomRunScreen.PropertyName._uiMode, ref variant14))
      return;
    this._uiMode = ((Variant) ref variant14).As<MultiplayerUiMode>();
  }

  public new class MethodName : NSubmenu.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName InitializeSingleplayer = StringName.op_Implicit(nameof (InitializeSingleplayer));
    public static readonly StringName OnSeedInputSubmitted = StringName.op_Implicit(nameof (OnSeedInputSubmitted));
    public static readonly StringName InitCharacterButtons = StringName.op_Implicit(nameof (InitCharacterButtons));
    public static readonly StringName _Input = StringName.op_Implicit(nameof (_Input));
    public static readonly StringName DebugUnlockAllCharacters = StringName.op_Implicit(nameof (DebugUnlockAllCharacters));
    public new static readonly StringName OnSubmenuOpened = StringName.op_Implicit(nameof (OnSubmenuOpened));
    public new static readonly StringName OnSubmenuClosed = StringName.op_Implicit(nameof (OnSubmenuClosed));
    public static readonly StringName OnRandomizePressed = StringName.op_Implicit(nameof (OnRandomizePressed));
    public static readonly StringName RandomizeLocalCharacter = StringName.op_Implicit(nameof (RandomizeLocalCharacter));
    public static readonly StringName OnEmbarkPressed = StringName.op_Implicit(nameof (OnEmbarkPressed));
    public static readonly StringName OnUnreadyPressed = StringName.op_Implicit(nameof (OnUnreadyPressed));
    public static readonly StringName UpdateRichPresence = StringName.op_Implicit(nameof (UpdateRichPresence));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
    public static readonly StringName CleanUpLobby = StringName.op_Implicit(nameof (CleanUpLobby));
    public static readonly StringName GetModifiersString = StringName.op_Implicit(nameof (GetModifiersString));
    public static readonly StringName OnAscensionPanelLevelChanged = StringName.op_Implicit(nameof (OnAscensionPanelLevelChanged));
    public static readonly StringName OnModifiersListChanged = StringName.op_Implicit(nameof (OnModifiersListChanged));
    public static readonly StringName MaxAscensionChanged = StringName.op_Implicit(nameof (MaxAscensionChanged));
    public static readonly StringName AscensionChanged = StringName.op_Implicit(nameof (AscensionChanged));
    public static readonly StringName SeedChanged = StringName.op_Implicit(nameof (SeedChanged));
    public static readonly StringName ModifiersChanged = StringName.op_Implicit(nameof (ModifiersChanged));
    public static readonly StringName AfterInitialized = StringName.op_Implicit(nameof (AfterInitialized));
    public static readonly StringName UpdateControllerButton = StringName.op_Implicit(nameof (UpdateControllerButton));
    public static readonly StringName TryFocusOnModifiersList = StringName.op_Implicit(nameof (TryFocusOnModifiersList));
  }

  public new class PropertyName : NSubmenu.PropertyName
  {
    public static readonly StringName ModifiersHotkey = StringName.op_Implicit(nameof (ModifiersHotkey));
    public new static readonly StringName InitialFocusedControl = StringName.op_Implicit(nameof (InitialFocusedControl));
    public static readonly StringName _disclaimer = StringName.op_Implicit(nameof (_disclaimer));
    public static readonly StringName _selectedButton = StringName.op_Implicit(nameof (_selectedButton));
    public static readonly StringName _charButtonContainer = StringName.op_Implicit(nameof (_charButtonContainer));
    public static readonly StringName _randomizeButton = StringName.op_Implicit(nameof (_randomizeButton));
    public static readonly StringName _confirmButton = StringName.op_Implicit(nameof (_confirmButton));
    public new static readonly StringName _backButton = StringName.op_Implicit(nameof (_backButton));
    public static readonly StringName _unreadyButton = StringName.op_Implicit(nameof (_unreadyButton));
    public static readonly StringName _ascensionPanel = StringName.op_Implicit(nameof (_ascensionPanel));
    public static readonly StringName _readyAndWaitingContainer = StringName.op_Implicit(nameof (_readyAndWaitingContainer));
    public static readonly StringName _seedInput = StringName.op_Implicit(nameof (_seedInput));
    public static readonly StringName _remotePlayerContainer = StringName.op_Implicit(nameof (_remotePlayerContainer));
    public static readonly StringName _modifiersList = StringName.op_Implicit(nameof (_modifiersList));
    public static readonly StringName _modifiersHotkeyIcon = StringName.op_Implicit(nameof (_modifiersHotkeyIcon));
    public static readonly StringName _uiMode = StringName.op_Implicit(nameof (_uiMode));
  }

  public new class SignalName : NSubmenu.SignalName
  {
  }
}
