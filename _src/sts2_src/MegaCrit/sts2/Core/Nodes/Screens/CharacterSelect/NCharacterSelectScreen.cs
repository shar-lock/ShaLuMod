// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect.NCharacterSelectScreen
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.UI;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Game.Lobby;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Debug;
using MegaCrit.Sts2.Core.Nodes.Ftue;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Rooms;
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
namespace MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;

[ScriptPath("res://src/Core/Nodes/Screens/CharacterSelect/NCharacterSelectScreen.cs")]
public class NCharacterSelectScreen : 
  NSubmenu,
  IStartRunLobbyListener,
  ICharacterSelectButtonDelegate
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("screens/character_select_screen");
  private MegaLabel _name;
  private Control _infoPanel;
  private MegaRichTextLabel _description;
  private MegaLabel _hp;
  private MegaLabel _gold;
  private MegaRichTextLabel _relicTitle;
  private MegaRichTextLabel _relicDescription;
  private TextureRect _relicIcon;
  private TextureRect _relicIconOutline;
  private NCharacterSelectButton? _selectedButton;
  private Control _charButtonContainer;
  private Control _bgContainer;
  private Control _readyAndWaitingContainer;
  private NBackButton _backButton;
  private NBackButton _unreadyButton;
  private NConfirmButton _embarkButton;
  private NAscensionPanel _ascensionPanel;
  private NActDropdown _actDropdown;
  private MegaRichTextLabel _actDropdownLabel;
  private NRemoteLobbyPlayerContainer _remotePlayerContainer;
  private Control _characterUnlockAnimationBackstop;
  private NCharacterSelectButton _randomCharacterButton;
  private Tween? _infoPanelTween;
  private Vector2 _infoPanelPosFinalVal;
  private const string _sceneCharSelectButtonPath = "res://scenes/screens/char_select/char_select_button.tscn";
  private bool _delayEmbarkForCharacterSelect;
  [Export]
  private PackedScene _charSelectButtonScene;
  private IBootstrapSettings? _settings;
  private StartRunLobby _lobby;

  public StartRunLobby Lobby => this._lobby;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      List<string> items = new List<string>();
      items.Add(NCharacterSelectScreen._scenePath);
      items.Add("res://scenes/screens/char_select/char_select_button.tscn");
      items.AddRange(NCharacterSelectButton.AssetPaths);
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlyList<string>(items);
    }
  }

  protected override Control InitialFocusedControl
  {
    get => ((Node) this._charButtonContainer).GetChild<Control>(0, false);
  }

  public static NCharacterSelectScreen? Create()
  {
    return TestMode.IsOn ? (NCharacterSelectScreen) null : ResourceLoader.Load<PackedScene>(NCharacterSelectScreen._scenePath, (string) null, (ResourceLoader.CacheMode) 1L).Instantiate<NCharacterSelectScreen>((PackedScene.GenEditState) 0L);
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._infoPanel = ((Node) this).GetNode<Control>(NodePath.op_Implicit("InfoPanel"));
    this._name = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("InfoPanel/VBoxContainer/Name"));
    this._description = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("InfoPanel/VBoxContainer/DescriptionLabel"));
    this._hp = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("InfoPanel/VBoxContainer/HpGoldSpacer/HpGold/Hp/Label"));
    this._gold = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("InfoPanel/VBoxContainer/HpGoldSpacer/HpGold/Gold/Label"));
    this._relicTitle = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("InfoPanel/VBoxContainer/Relic/Name/RichTextLabel"));
    this._relicDescription = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("InfoPanel/VBoxContainer/Relic/Description"));
    this._relicIcon = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("InfoPanel/VBoxContainer/Relic/Icon"));
    this._relicIconOutline = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("InfoPanel/VBoxContainer/Relic/Icon/Outline"));
    this._bgContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("AnimatedBg"));
    this._charButtonContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("CharSelectButtons/ButtonContainer"));
    this._ascensionPanel = ((Node) this).GetNode<NAscensionPanel>(NodePath.op_Implicit("%AscensionPanel"));
    this._actDropdown = ((Node) this).GetNode<NActDropdown>(NodePath.op_Implicit("%ActDropdown"));
    this._actDropdownLabel = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("ActLabel"));
    this._remotePlayerContainer = ((Node) this).GetNode<NRemoteLobbyPlayerContainer>(NodePath.op_Implicit("RemotePlayerContainer"));
    this._readyAndWaitingContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("ReadyAndWaitingPanel"));
    ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%WaitingForPlayers")).Text = new LocString("main_menu_ui", "CHARACTER_SELECT.waitingForPlayers").GetFormattedText();
    this._characterUnlockAnimationBackstop = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%CharacterUnlockAnimationBackstop"));
    this._backButton = ((Node) this).GetNode<NBackButton>(NodePath.op_Implicit("BackButton"));
    this._unreadyButton = ((Node) this).GetNode<NBackButton>(NodePath.op_Implicit("UnreadyButton"));
    this._embarkButton = ((Node) this).GetNode<NConfirmButton>(NodePath.op_Implicit("ConfirmButton"));
    this._embarkButton.OverrideHotkeys(new string[1]
    {
      StringName.op_Implicit(MegaInput.select)
    });
    ((GodotObject) this._embarkButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnEmbarkPressed)), 0U);
    ((GodotObject) this._ascensionPanel).Connect(NAscensionPanel.SignalName.AscensionLevelChanged, Callable.From(new Action(this.OnAscensionPanelLevelChanged)), 0U);
    ((GodotObject) this._unreadyButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnUnreadyPressed)), 0U);
    this._unreadyButton.Disable();
    this.InitCharacterButtons();
    Type type = BootstrapSettingsUtil.Get();
    if (!(type != (Type) null))
      return;
    this._settings = (IBootstrapSettings) Activator.CreateInstance(type);
    PreloadManager.Enabled = this._settings.DoPreloading;
  }

  public void InitializeMultiplayerAsHost(INetGameService gameService, int maxPlayers)
  {
    this._lobby = gameService.Type == NetGameType.Host ? new StartRunLobby(GameMode.Standard, gameService, (IStartRunLobbyListener) this, maxPlayers) : throw new InvalidOperationException($"Initialized character select screen with GameService of type {gameService.Type} when hosting!");
    this._ascensionPanel.Initialize(MultiplayerUiMode.Host);
    this._lobby.AddLocalHostPlayer(new UnlockState(SaveManager.Instance.Progress), SaveManager.Instance.Progress.MaxMultiplayerAscension);
    this.OnAscensionPanelLevelChanged();
    this.CheckForMultiplayerAscensionPopup();
    this.AfterInitialized();
  }

  public void InitializeMultiplayerAsClient(
    INetGameService gameService,
    ClientLobbyJoinResponseMessage message)
  {
    this._lobby = gameService.Type == NetGameType.Client ? new StartRunLobby(GameMode.Standard, gameService, (IStartRunLobbyListener) this, -1) : throw new InvalidOperationException($"Initialized character select screen with GameService of type {gameService.Type} when joining!");
    this._ascensionPanel.Initialize(MultiplayerUiMode.Client);
    this._lobby.InitializeFromMessage(message);
    this.CheckForMultiplayerAscensionPopup();
    this.AfterInitialized();
  }

  private void CheckForMultiplayerAscensionPopup()
  {
    if (SaveManager.Instance.Progress.MaxMultiplayerAscension <= 0 || SaveManager.Instance.SeenPopup("ascension_multiplayer_ftue"))
      return;
    NAscensionMultiplayerFtue modalToCreate = NAscensionMultiplayerFtue.Create();
    if (modalToCreate == null)
      return;
    NModalContainer.Instance.Add((Node) modalToCreate);
  }

  public void InitializeSingleplayer()
  {
    this._lobby = new StartRunLobby(GameMode.Standard, (INetGameService) new NetSingleplayerGameService(), (IStartRunLobbyListener) this, 1);
    this._ascensionPanel.Initialize(MultiplayerUiMode.Singleplayer);
    this._lobby.AddLocalHostPlayer(new UnlockState(SaveManager.Instance.Progress), 0);
    this.AfterInitialized();
  }

  private void InitCharacterButtons()
  {
    foreach (CharacterModel allCharacter in ModelDb.AllCharacters)
    {
      NCharacterSelectButton child = this._charSelectButtonScene.Instantiate<NCharacterSelectButton>((PackedScene.GenEditState) 0L);
      ((Node) child).Name = StringName.op_Implicit(allCharacter.Id.Entry + "_button");
      ((Node) this._charButtonContainer).AddChildSafely((Node) child);
      child.Init(allCharacter, (ICharacterSelectButtonDelegate) this);
    }
    this._randomCharacterButton = this._charSelectButtonScene.Instantiate<NCharacterSelectButton>((PackedScene.GenEditState) 0L);
    ((Node) this._charButtonContainer).AddChildSafely((Node) this._randomCharacterButton);
    this._randomCharacterButton.Init((CharacterModel) ModelDb.Character<RandomCharacter>(), (ICharacterSelectButtonDelegate) this);
    this.UpdateRandomCharacterVisibility();
    List<NCharacterSelectButton> list = ((IEnumerable) ((Node) this._charButtonContainer).GetChildren(false)).OfType<NCharacterSelectButton>().Where<NCharacterSelectButton>((Func<NCharacterSelectButton, bool>) (c => ((CanvasItem) c).Visible)).ToList<NCharacterSelectButton>();
    for (int index = 0; index < list.Count; ++index)
    {
      list[index].FocusNeighborTop = ((Node) list[index]).GetPath();
      list[index].FocusNeighborBottom = ((Node) list[index]).GetPath();
      NCharacterSelectButton ncharacterSelectButton = list[index];
      NodePath path;
      if (index <= 0)
      {
        List<NCharacterSelectButton> ncharacterSelectButtonList = list;
        path = ((Node) ncharacterSelectButtonList[ncharacterSelectButtonList.Count - 1]).GetPath();
      }
      else
        path = ((Node) list[index - 1]).GetPath();
      ncharacterSelectButton.FocusNeighborLeft = path;
      list[index].FocusNeighborRight = index < list.Count - 1 ? ((Node) list[index + 1]).GetPath() : ((Node) list[0]).GetPath();
    }
  }

  private void UpdateRandomCharacterVisibility()
  {
    if (this._lobby == null)
      return;
    bool flag1 = false;
    foreach (LobbyPlayer player in this._lobby.Players)
    {
      UnlockState unlockState = UnlockState.FromSerializable(player.unlockState);
      bool flag2 = true;
      foreach (CharacterModel allCharacter in ModelDb.AllCharacters)
      {
        if (!unlockState.Characters.Contains<CharacterModel>(allCharacter))
        {
          flag2 = false;
          break;
        }
      }
      if (flag2)
      {
        flag1 = true;
        break;
      }
    }
    ((CanvasItem) this._randomCharacterButton).Visible = flag1;
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
        ncharacterSelectButton.Enable();
      else
        ncharacterSelectButton.UnlockIfPossible();
      ncharacterSelectButton.Reset();
    }
    this._embarkButton.Enable();
    if (SaveManager.Instance.Progress.PendingCharacterUnlock == ModelId.none)
      ((Node) this._charButtonContainer).GetChild<NCharacterSelectButton>(0, false).Select();
    else
      TaskHelper.RunSafely(this.PlayUnlockCharacterAnimation(SaveManager.Instance.Progress.PendingCharacterUnlock));
    ((CanvasItem) this._remotePlayerContainer).Visible = this._lobby.NetService.Type != NetGameType.Singleplayer;
    this._remotePlayerContainer.Initialize(this._lobby, true);
    if (this._lobby.NetService.Type == NetGameType.Client)
      this._ascensionPanel.SetAscensionLevel(this._lobby.Ascension);
    ((CanvasItem) this._actDropdown).Visible = this.ShouldShowActDropdown;
    ((CanvasItem) this._actDropdownLabel).Visible = ((CanvasItem) this._actDropdown).Visible;
    ((CanvasItem) this._readyAndWaitingContainer).Visible = false;
    foreach (LobbyPlayer player in this._lobby.Players)
      this.RefreshButtonSelectionForPlayer(player);
  }

  private async Task PlayUnlockCharacterAnimation(ModelId character)
  {
    double num = (double) await ((Node) this).AwaitProcessFrame();
    this._backButton.Disable();
    this._embarkButton.Disable();
    ((CanvasItem) this._infoPanel).Visible = false;
    ((CanvasItem) this._characterUnlockAnimationBackstop).Visible = true;
    foreach (NCharacterSelectButton button in ((IEnumerable) ((Node) this._charButtonContainer).GetChildren(false)).OfType<NCharacterSelectButton>())
    {
      if (button.Character.Id == character)
      {
        button.LockForAnimation();
        await Cmd.Wait(0.3f);
        await button.AnimateUnlock();
        button.Select();
      }
    }
    ((CanvasItem) this._infoPanel).Visible = true;
    ((CanvasItem) this._characterUnlockAnimationBackstop).Visible = false;
    this._backButton.Enable();
    this._embarkButton.Enable();
    SaveManager.Instance.Progress.PendingCharacterUnlock = ModelId.none;
  }

  private bool ShouldShowActDropdown => false;

  public override void OnSubmenuClosed()
  {
    base.OnSubmenuClosed();
    this._embarkButton.Disable();
    this._remotePlayerContainer.Cleanup();
    this._ascensionPanel.Cleanup();
    if (this._lobby.NetService.Type.IsMultiplayer())
      PlatformUtil.SetRichPresence("MAIN_MENU", (string) null, new int?());
    this.CleanUpLobby(true);
  }

  private void OnEmbarkPressed(NButton _)
  {
    this._embarkButton.Disable();
    if (!SaveManager.Instance.SeenFtue("accept_tutorials_ftue"))
    {
      NModalContainer.Instance.Add((Node) NAcceptTutorialsFtue.Create(this, (Action) (() => this.OnEmbarkPressed((NButton) null))));
    }
    else
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
        this._lobby.Act1 = this._actDropdown.CurrentOption;
      this._lobby.SetReady(true);
      foreach (NClickableControl nclickableControl in ((IEnumerable) ((Node) this._charButtonContainer).GetChildren(false)).OfType<NCharacterSelectButton>())
        nclickableControl.Disable();
      this._backButton.Disable();
      if (!this._lobby.NetService.Type.IsMultiplayer() || this._lobby.IsAboutToBeginGame())
        return;
      ((CanvasItem) this._readyAndWaitingContainer).Visible = true;
      this._unreadyButton.Enable();
    }
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

  private void OnLocalCharacterChangedForRandom(CharacterModel characterModel)
  {
    NGame.Instance?.ScreenShake(ShakeStrength.Weak, ShakeDuration.Short, 90f);
    SfxCmd.Play(characterModel.CharacterSelectSfx);
    Control child = PreloadManager.Cache.GetScene(characterModel.CharacterSelectBg).Instantiate<Control>((PackedScene.GenEditState) 0L);
    ((Node) child).Name = StringName.op_Implicit(characterModel.Id.Entry + "_bg");
    ((Node) this._bgContainer).AddChildSafely((Node) child);
    this._delayEmbarkForCharacterSelect = true;
  }

  private async Task StartNewSingleplayerRun(string seed, List<ActModel> acts)
  {
    Log.Info($"Embarking on a singleplayer {this._lobby.LocalPlayer.character.Id.Entry} run. Ascension: {this._lobby.Ascension} Seed: {seed}");
    int ascensionToEmbark = this._lobby.Ascension;
    if (this._delayEmbarkForCharacterSelect)
    {
      await Cmd.Wait(1f);
      this._delayEmbarkForCharacterSelect = false;
    }
    int num = 0;
    object obj;
    try
    {
      SfxCmd.Play(this._lobby.LocalPlayer.character.CharacterTransitionSfx);
      await NGame.Instance.Transition.FadeOut(transitionPath: this._lobby.LocalPlayer.character.CharacterSelectTransitionPath);
      RunState runState = await NGame.Instance.StartNewSingleplayerRun(this._lobby.LocalPlayer.character, true, (IReadOnlyList<ActModel>) acts, (IReadOnlyList<ModifierModel>) Array.Empty<ModifierModel>(), seed, GameMode.Standard, ascensionToEmbark);
    }
    catch (Exception ex)
    {
      obj = (object) ex;
      num = 1;
    }
    if (num == 1)
    {
      Exception e = (Exception) obj;
      Log.Error($"Exception starting singleplayer run : {e}");
      this.CleanUpLobby(true, NetError.InternalError);
      await NGame.Instance.ReturnToMainMenuWithInternalError(e);
    }
    else
    {
      obj = (object) null;
      this.CleanUpLobby(false);
    }
  }

  private async Task StartNewMultiplayerRun(string seed, List<ActModel> acts)
  {
    Log.Info($"Embarking on a multiplayer run. Players: {string.Join<LobbyPlayer>(",", (IEnumerable<LobbyPlayer>) this._lobby.Players)}. Ascension: {this._lobby.Ascension} Seed: {seed}");
    if (this._delayEmbarkForCharacterSelect)
    {
      await Cmd.Wait(1f);
      this._delayEmbarkForCharacterSelect = false;
    }
    SfxCmd.Play(this._lobby.LocalPlayer.character.CharacterTransitionSfx);
    await NGame.Instance.Transition.FadeOut(transitionPath: this._lobby.LocalPlayer.character.CharacterSelectTransitionPath);
    IBootstrapSettings settings = this._settings;
    if (settings != null && settings.BootstrapInMultiplayer)
    {
      RunState runState;
      using (new NetLoadingHandle(this._lobby.NetService))
      {
        acts[0] = this._settings.Act;
        try
        {
          runState = RunState.CreateForNewRun((IReadOnlyList<Player>) this._lobby.Players.Select<LobbyPlayer, Player>((Func<LobbyPlayer, Player>) (p => Player.CreateForNewRun(p.character, UnlockState.FromSerializable(p.unlockState), p.id))).ToList<Player>(), (IReadOnlyList<ActModel>) acts.Select<ActModel, ActModel>((Func<ActModel, ActModel>) (a => a.ToMutable())).ToList<ActModel>(), (IReadOnlyList<ModifierModel>) this._settings.Modifiers, GameMode.Standard, this._lobby.Ascension, seed);
          RunManager.Instance.SetUpNewMultiplayer(runState, this._lobby, this._settings.SaveRunHistory);
          await PreloadManager.LoadRunAssets(runState.Players.Select<Player, CharacterModel>((Func<Player, CharacterModel>) (p => p.Character)));
          await RunManager.Instance.FinalizeStartingRelics();
          RunManager.Instance.Launch();
          NGame.Instance.RootSceneContainer.SetCurrentScene((Control) NRun.Create(runState));
          await RunManager.Instance.SetActInternal(0);
          await SaveManager.Instance.SaveRun((AbstractRoom) null);
        }
        catch (Exception ex)
        {
          Log.Error($"Exception starting bootstrap multiplayer run : {ex}");
          this.CleanUpLobby(true, NetError.InternalError);
          await NGame.Instance.ReturnToMainMenuWithInternalError(ex);
          return;
        }
        this.CleanUpLobby(false);
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
      int num = 0;
      object obj;
      try
      {
        RunState runState = await NGame.Instance.StartNewMultiplayerRun(this._lobby, true, (IReadOnlyList<ActModel>) acts, (IReadOnlyList<ModifierModel>) Array.Empty<ModifierModel>(), seed, this._lobby.Ascension);
      }
      catch (Exception ex)
      {
        obj = (object) ex;
        num = 1;
      }
      if (num == 1)
      {
        Exception e = (Exception) obj;
        Log.Error($"Exception starting multiplayer run : {e}");
        this.CleanUpLobby(true, NetError.InternalError);
        await NGame.Instance.ReturnToMainMenuWithInternalError(e);
      }
      else
      {
        obj = (object) null;
        this.CleanUpLobby(false);
      }
    }
  }

  public void SelectCharacter(
    NCharacterSelectButton charSelectButton,
    CharacterModel characterModel)
  {
    if (!charSelectButton.IsRandom)
      SfxCmd.Play(characterModel.CharacterSelectSfx);
    NGame.Instance?.ScreenShake(ShakeStrength.Weak, ShakeDuration.Short, 90f);
    if (this._infoPanelTween != null)
      this._infoPanel.Position = this._infoPanelPosFinalVal;
    this._infoPanelPosFinalVal = this._infoPanel.Position;
    this._infoPanelTween?.Kill();
    this._infoPanelTween = ((Node) this).CreateTween().SetParallel(true);
    this._infoPanelTween.TweenProperty((GodotObject) this._infoPanel, NodePath.op_Implicit("position"), Variant.op_Implicit(this._infoPanel.Position), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L).From(Variant.op_Implicit(Vector2.op_Subtraction(this._infoPanel.Position, new Vector2(300f, 0.0f))));
    foreach (Node child in ((Node) this._bgContainer).GetChildren(false))
    {
      ((Node) this._bgContainer).RemoveChildSafely(child);
      child.QueueFreeSafely();
    }
    this._selectedButton = charSelectButton;
    if (!charSelectButton.IsLocked)
    {
      this._embarkButton.Enable();
      Control child = PreloadManager.Cache.GetScene(characterModel.CharacterSelectBg).Instantiate<Control>((PackedScene.GenEditState) 0L);
      ((Node) child).Name = StringName.op_Implicit(characterModel.Id.Entry + "_bg");
      ((Node) this._bgContainer).AddChildSafely((Node) child);
      this._name.SetTextAutoSize(new LocString("characters", characterModel.CharacterSelectTitle).GetFormattedText());
      this._description.Text = new LocString("characters", characterModel.CharacterSelectDesc).GetFormattedText();
      if (!this._selectedButton.IsRandom)
      {
        this._hp.SetTextAutoSize($"{characterModel.StartingHp}/{characterModel.StartingHp}");
        this._gold.SetTextAutoSize($"{characterModel.StartingGold}");
        RelicModel startingRelic = characterModel.StartingRelics[0];
        this._relicTitle.Text = startingRelic.Title.GetFormattedText();
        this._relicDescription.Text = startingRelic.DynamicDescription.GetFormattedText();
        this._relicIcon.Texture = startingRelic.Icon;
        this._relicIconOutline.Texture = startingRelic.IconOutline;
        ((CanvasItem) this._relicIcon).SelfModulate = Colors.White;
        ((CanvasItem) this._relicIconOutline).SelfModulate = StsColors.halfTransparentBlack;
      }
      else
      {
        this._hp.SetTextAutoSize("??/??");
        this._gold.SetTextAutoSize("???");
        ((CanvasItem) this._relicIcon).SelfModulate = StsColors.transparentBlack;
        ((CanvasItem) this._relicIconOutline).SelfModulate = StsColors.transparentBlack;
        this._relicTitle.Text = string.Empty;
        this._relicDescription.Text = string.Empty;
      }
      this._lobby.SetLocalCharacter(characterModel);
      if (!this._lobby.NetService.Type.IsMultiplayer())
        this._ascensionPanel.AnimIn();
    }
    else
    {
      this._embarkButton.Disable();
      this._name.SetTextAutoSize(new LocString("main_menu_ui", "CHARACTER_SELECT.locked.title").GetFormattedText());
      this._description.Text = characterModel.GetUnlockText().GetFormattedText();
      this._hp.SetTextAutoSize("??/??");
      this._gold.SetTextAutoSize("???");
      if (!this._selectedButton.IsRandom)
      {
        RelicModel startingRelic = characterModel.StartingRelics[0];
        this._relicTitle.Text = new LocString("main_menu_ui", "CHARACTER_SELECT.lockedRelic.title").GetFormattedText();
        this._relicDescription.Text = new LocString("main_menu_ui", "CHARACTER_SELECT.lockedRelic.description").GetFormattedText();
        this._relicIcon.Texture = startingRelic.Icon;
        this._relicIconOutline.Texture = startingRelic.IconOutline;
        ((CanvasItem) this._relicIcon).SelfModulate = StsColors.ninetyPercentBlack;
        ((CanvasItem) this._relicIconOutline).SelfModulate = StsColors.halfTransparentWhite;
      }
      else
      {
        ((CanvasItem) this._relicIcon).SelfModulate = StsColors.transparentBlack;
        ((CanvasItem) this._relicIconOutline).SelfModulate = StsColors.transparentBlack;
        this._relicTitle.Text = string.Empty;
        this._relicDescription.Text = string.Empty;
      }
      ((CanvasItem) this._ascensionPanel).Visible = false;
    }
    foreach (NCharacterSelectButton ncharacterSelectButton in ((IEnumerable) ((Node) this._charButtonContainer).GetChildren(false)).OfType<NCharacterSelectButton>())
    {
      if (ncharacterSelectButton != this._selectedButton)
        ncharacterSelectButton.Deselect();
    }
  }

  private void OnAscensionPanelLevelChanged()
  {
    if (this._lobby.NetService.Type == NetGameType.Client || this._lobby.Ascension == this._ascensionPanel.Ascension)
      return;
    this._lobby.SyncAscensionChange(this._ascensionPanel.Ascension);
  }

  private void OnUnreadyPressed(NButton _)
  {
    this._lobby.SetReady(false);
    foreach (NClickableControl nclickableControl in ((IEnumerable) ((Node) this._charButtonContainer).GetChildren(false)).OfType<NCharacterSelectButton>())
      nclickableControl.Enable();
    NCharacterSelectButton selectedButton = this._selectedButton;
    if (selectedButton != null)
      selectedButton.TryGrabFocus();
    ((CanvasItem) this._readyAndWaitingContainer).Visible = false;
    this._embarkButton.Enable();
    this._backButton.Enable();
    this._unreadyButton.Disable();
  }

  private void UpdateRichPresence()
  {
    if (!this._lobby.NetService.Type.IsMultiplayer())
      return;
    PlatformUtil.SetRichPresence("STANDARD_MP_LOBBY", this._lobby.NetService.GetRawLobbyIdentifier(), new int?(this._lobby.Players.Count));
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
    this.UpdateRandomCharacterVisibility();
  }

  public void PlayerChanged(LobbyPlayer player, bool isRandomCharacterResolution)
  {
    if ((long) player.id == (long) this._lobby.LocalPlayer.id && isRandomCharacterResolution)
      this.OnLocalCharacterChangedForRandom(player.character);
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
    throw new NotImplementedException("Seed should not be changed in standard mode!");
  }

  public void ModifiersChanged()
  {
    throw new NotImplementedException("Modifiers should not be changed in standard mode!");
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
    this.UpdateRandomCharacterVisibility();
  }

  public void BeginRun(string seed, List<ActModel> acts, IReadOnlyList<ModifierModel> modifiers)
  {
    if (modifiers.Count > 0)
      Log.Error("Modifiers list is not empty while starting a standard run, ignoring!");
    NAudioManager.Instance?.StopMusic();
    this._ascensionPanel.Cleanup();
    this._ascensionPanel.Disable();
    this._embarkButton.Disable();
    this._unreadyButton.Disable();
    if (this._lobby.NetService.Type == NetGameType.Singleplayer)
      TaskHelper.RunSafely(this.StartNewSingleplayerRun(seed, acts));
    else
      TaskHelper.RunSafely(this.StartNewMultiplayerRun(seed, acts));
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
    this.UpdateRandomCharacterVisibility();
    Logger.logLevelTypeMap[LogType.Network] = this._lobby.NetService.Type == NetGameType.Singleplayer ? LogLevel.Info : LogLevel.Debug;
    Logger.logLevelTypeMap[LogType.Actions] = this._lobby.NetService.Type == NetGameType.Singleplayer ? LogLevel.Info : LogLevel.VeryDebug;
    Logger.logLevelTypeMap[LogType.GameSync] = this._lobby.NetService.Type == NetGameType.Singleplayer ? LogLevel.Info : LogLevel.VeryDebug;
    if (this._lobby.NetService.Type != NetGameType.Singleplayer)
    {
      IBootstrapSettings settings = this._settings;
      if ((settings != null ? (settings.BootstrapInMultiplayer ? 1 : 0) : 0) != 0)
      {
        NGame.Instance.DebugSeedOverride = this._settings.Seed;
        return;
      }
    }
    NGame.Instance.DebugSeedOverride = (string) null;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(21)
    {
      new MethodInfo(NCharacterSelectScreen.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCharacterSelectScreen.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCharacterSelectScreen.MethodName.CheckForMultiplayerAscensionPopup, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCharacterSelectScreen.MethodName.InitializeSingleplayer, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCharacterSelectScreen.MethodName.InitCharacterButtons, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCharacterSelectScreen.MethodName.UpdateRandomCharacterVisibility, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCharacterSelectScreen.MethodName._Input, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCharacterSelectScreen.MethodName.DebugUnlockAllCharacters, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCharacterSelectScreen.MethodName.OnSubmenuOpened, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCharacterSelectScreen.MethodName.OnSubmenuClosed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCharacterSelectScreen.MethodName.OnEmbarkPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCharacterSelectScreen.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCharacterSelectScreen.MethodName.CleanUpLobby, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("disconnectSession"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("error"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCharacterSelectScreen.MethodName.OnAscensionPanelLevelChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCharacterSelectScreen.MethodName.OnUnreadyPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCharacterSelectScreen.MethodName.UpdateRichPresence, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCharacterSelectScreen.MethodName.MaxAscensionChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCharacterSelectScreen.MethodName.AscensionChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCharacterSelectScreen.MethodName.SeedChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCharacterSelectScreen.MethodName.ModifiersChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCharacterSelectScreen.MethodName.AfterInitialized, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCharacterSelectScreen.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NCharacterSelectScreen ncharacterSelectScreen = NCharacterSelectScreen.Create();
      ret = VariantUtils.CreateFrom<NCharacterSelectScreen>(ref ncharacterSelectScreen);
      return true;
    }
    if (StringName.op_Equality(ref method, NCharacterSelectScreen.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCharacterSelectScreen.MethodName.CheckForMultiplayerAscensionPopup) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.CheckForMultiplayerAscensionPopup();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCharacterSelectScreen.MethodName.InitializeSingleplayer) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.InitializeSingleplayer();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCharacterSelectScreen.MethodName.InitCharacterButtons) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.InitCharacterButtons();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCharacterSelectScreen.MethodName.UpdateRandomCharacterVisibility) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateRandomCharacterVisibility();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCharacterSelectScreen.MethodName._Input) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Input(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCharacterSelectScreen.MethodName.DebugUnlockAllCharacters) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.DebugUnlockAllCharacters();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCharacterSelectScreen.MethodName.OnSubmenuOpened) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSubmenuOpened();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCharacterSelectScreen.MethodName.OnSubmenuClosed) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSubmenuClosed();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCharacterSelectScreen.MethodName.OnEmbarkPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnEmbarkPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCharacterSelectScreen.MethodName._Process) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCharacterSelectScreen.MethodName.CleanUpLobby) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.CleanUpLobby(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<NetError>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCharacterSelectScreen.MethodName.OnAscensionPanelLevelChanged) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnAscensionPanelLevelChanged();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCharacterSelectScreen.MethodName.OnUnreadyPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnUnreadyPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCharacterSelectScreen.MethodName.UpdateRichPresence) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateRichPresence();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCharacterSelectScreen.MethodName.MaxAscensionChanged) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.MaxAscensionChanged();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCharacterSelectScreen.MethodName.AscensionChanged) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AscensionChanged();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCharacterSelectScreen.MethodName.SeedChanged) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SeedChanged();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCharacterSelectScreen.MethodName.ModifiersChanged) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ModifiersChanged();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCharacterSelectScreen.MethodName.AfterInitialized) || ((NativeVariantPtrArgs) ref args).Count != 0)
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
    if (StringName.op_Equality(ref method, NCharacterSelectScreen.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NCharacterSelectScreen ncharacterSelectScreen = NCharacterSelectScreen.Create();
      ret = VariantUtils.CreateFrom<NCharacterSelectScreen>(ref ncharacterSelectScreen);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCharacterSelectScreen.MethodName.Create) || StringName.op_Equality(ref method, NCharacterSelectScreen.MethodName._Ready) || StringName.op_Equality(ref method, NCharacterSelectScreen.MethodName.CheckForMultiplayerAscensionPopup) || StringName.op_Equality(ref method, NCharacterSelectScreen.MethodName.InitializeSingleplayer) || StringName.op_Equality(ref method, NCharacterSelectScreen.MethodName.InitCharacterButtons) || StringName.op_Equality(ref method, NCharacterSelectScreen.MethodName.UpdateRandomCharacterVisibility) || StringName.op_Equality(ref method, NCharacterSelectScreen.MethodName._Input) || StringName.op_Equality(ref method, NCharacterSelectScreen.MethodName.DebugUnlockAllCharacters) || StringName.op_Equality(ref method, NCharacterSelectScreen.MethodName.OnSubmenuOpened) || StringName.op_Equality(ref method, NCharacterSelectScreen.MethodName.OnSubmenuClosed) || StringName.op_Equality(ref method, NCharacterSelectScreen.MethodName.OnEmbarkPressed) || StringName.op_Equality(ref method, NCharacterSelectScreen.MethodName._Process) || StringName.op_Equality(ref method, NCharacterSelectScreen.MethodName.CleanUpLobby) || StringName.op_Equality(ref method, NCharacterSelectScreen.MethodName.OnAscensionPanelLevelChanged) || StringName.op_Equality(ref method, NCharacterSelectScreen.MethodName.OnUnreadyPressed) || StringName.op_Equality(ref method, NCharacterSelectScreen.MethodName.UpdateRichPresence) || StringName.op_Equality(ref method, NCharacterSelectScreen.MethodName.MaxAscensionChanged) || StringName.op_Equality(ref method, NCharacterSelectScreen.MethodName.AscensionChanged) || StringName.op_Equality(ref method, NCharacterSelectScreen.MethodName.SeedChanged) || StringName.op_Equality(ref method, NCharacterSelectScreen.MethodName.ModifiersChanged) || StringName.op_Equality(ref method, NCharacterSelectScreen.MethodName.AfterInitialized) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._name))
    {
      this._name = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._infoPanel))
    {
      this._infoPanel = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._description))
    {
      this._description = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._hp))
    {
      this._hp = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._gold))
    {
      this._gold = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._relicTitle))
    {
      this._relicTitle = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._relicDescription))
    {
      this._relicDescription = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._relicIcon))
    {
      this._relicIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._relicIconOutline))
    {
      this._relicIconOutline = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._selectedButton))
    {
      this._selectedButton = VariantUtils.ConvertTo<NCharacterSelectButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._charButtonContainer))
    {
      this._charButtonContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._bgContainer))
    {
      this._bgContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._readyAndWaitingContainer))
    {
      this._readyAndWaitingContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._backButton))
    {
      this._backButton = VariantUtils.ConvertTo<NBackButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._unreadyButton))
    {
      this._unreadyButton = VariantUtils.ConvertTo<NBackButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._embarkButton))
    {
      this._embarkButton = VariantUtils.ConvertTo<NConfirmButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._ascensionPanel))
    {
      this._ascensionPanel = VariantUtils.ConvertTo<NAscensionPanel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._actDropdown))
    {
      this._actDropdown = VariantUtils.ConvertTo<NActDropdown>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._actDropdownLabel))
    {
      this._actDropdownLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._remotePlayerContainer))
    {
      this._remotePlayerContainer = VariantUtils.ConvertTo<NRemoteLobbyPlayerContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._characterUnlockAnimationBackstop))
    {
      this._characterUnlockAnimationBackstop = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._randomCharacterButton))
    {
      this._randomCharacterButton = VariantUtils.ConvertTo<NCharacterSelectButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._infoPanelTween))
    {
      this._infoPanelTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._infoPanelPosFinalVal))
    {
      this._infoPanelPosFinalVal = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._delayEmbarkForCharacterSelect))
    {
      this._delayEmbarkForCharacterSelect = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._charSelectButtonScene))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._charSelectButtonScene = VariantUtils.ConvertTo<PackedScene>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName.InitialFocusedControl))
    {
      ref godot_variant local = ref value;
      Control initialFocusedControl = this.InitialFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref initialFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName.ShouldShowActDropdown))
    {
      ref godot_variant local = ref value;
      bool shouldShowActDropdown = this.ShouldShowActDropdown;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref shouldShowActDropdown);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._name))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._name);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._infoPanel))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._infoPanel);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._description))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._description);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._hp))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._hp);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._gold))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._gold);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._relicTitle))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._relicTitle);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._relicDescription))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._relicDescription);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._relicIcon))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._relicIcon);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._relicIconOutline))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._relicIconOutline);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._selectedButton))
    {
      value = VariantUtils.CreateFrom<NCharacterSelectButton>(ref this._selectedButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._charButtonContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._charButtonContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._bgContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._bgContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._readyAndWaitingContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._readyAndWaitingContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._backButton))
    {
      value = VariantUtils.CreateFrom<NBackButton>(ref this._backButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._unreadyButton))
    {
      value = VariantUtils.CreateFrom<NBackButton>(ref this._unreadyButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._embarkButton))
    {
      value = VariantUtils.CreateFrom<NConfirmButton>(ref this._embarkButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._ascensionPanel))
    {
      value = VariantUtils.CreateFrom<NAscensionPanel>(ref this._ascensionPanel);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._actDropdown))
    {
      value = VariantUtils.CreateFrom<NActDropdown>(ref this._actDropdown);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._actDropdownLabel))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._actDropdownLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._remotePlayerContainer))
    {
      value = VariantUtils.CreateFrom<NRemoteLobbyPlayerContainer>(ref this._remotePlayerContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._characterUnlockAnimationBackstop))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._characterUnlockAnimationBackstop);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._randomCharacterButton))
    {
      value = VariantUtils.CreateFrom<NCharacterSelectButton>(ref this._randomCharacterButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._infoPanelTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._infoPanelTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._infoPanelPosFinalVal))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._infoPanelPosFinalVal);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._delayEmbarkForCharacterSelect))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._delayEmbarkForCharacterSelect);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCharacterSelectScreen.PropertyName._charSelectButtonScene))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<PackedScene>(ref this._charSelectButtonScene);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCharacterSelectScreen.PropertyName._name, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCharacterSelectScreen.PropertyName._infoPanel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCharacterSelectScreen.PropertyName._description, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCharacterSelectScreen.PropertyName._hp, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCharacterSelectScreen.PropertyName._gold, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCharacterSelectScreen.PropertyName._relicTitle, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCharacterSelectScreen.PropertyName._relicDescription, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCharacterSelectScreen.PropertyName._relicIcon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCharacterSelectScreen.PropertyName._relicIconOutline, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCharacterSelectScreen.PropertyName._selectedButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCharacterSelectScreen.PropertyName._charButtonContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCharacterSelectScreen.PropertyName._bgContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCharacterSelectScreen.PropertyName._readyAndWaitingContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCharacterSelectScreen.PropertyName._backButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCharacterSelectScreen.PropertyName._unreadyButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCharacterSelectScreen.PropertyName._embarkButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCharacterSelectScreen.PropertyName._ascensionPanel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCharacterSelectScreen.PropertyName._actDropdown, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCharacterSelectScreen.PropertyName._actDropdownLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCharacterSelectScreen.PropertyName._remotePlayerContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCharacterSelectScreen.PropertyName._characterUnlockAnimationBackstop, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCharacterSelectScreen.PropertyName._randomCharacterButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCharacterSelectScreen.PropertyName._infoPanelTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NCharacterSelectScreen.PropertyName._infoPanelPosFinalVal, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCharacterSelectScreen.PropertyName._delayEmbarkForCharacterSelect, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCharacterSelectScreen.PropertyName._charSelectButtonScene, (PropertyHint) 17L, "PackedScene", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NCharacterSelectScreen.PropertyName.InitialFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCharacterSelectScreen.PropertyName.ShouldShowActDropdown, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NCharacterSelectScreen.PropertyName._name, Variant.From<MegaLabel>(ref this._name));
    info.AddProperty(NCharacterSelectScreen.PropertyName._infoPanel, Variant.From<Control>(ref this._infoPanel));
    info.AddProperty(NCharacterSelectScreen.PropertyName._description, Variant.From<MegaRichTextLabel>(ref this._description));
    info.AddProperty(NCharacterSelectScreen.PropertyName._hp, Variant.From<MegaLabel>(ref this._hp));
    info.AddProperty(NCharacterSelectScreen.PropertyName._gold, Variant.From<MegaLabel>(ref this._gold));
    info.AddProperty(NCharacterSelectScreen.PropertyName._relicTitle, Variant.From<MegaRichTextLabel>(ref this._relicTitle));
    info.AddProperty(NCharacterSelectScreen.PropertyName._relicDescription, Variant.From<MegaRichTextLabel>(ref this._relicDescription));
    info.AddProperty(NCharacterSelectScreen.PropertyName._relicIcon, Variant.From<TextureRect>(ref this._relicIcon));
    info.AddProperty(NCharacterSelectScreen.PropertyName._relicIconOutline, Variant.From<TextureRect>(ref this._relicIconOutline));
    info.AddProperty(NCharacterSelectScreen.PropertyName._selectedButton, Variant.From<NCharacterSelectButton>(ref this._selectedButton));
    info.AddProperty(NCharacterSelectScreen.PropertyName._charButtonContainer, Variant.From<Control>(ref this._charButtonContainer));
    info.AddProperty(NCharacterSelectScreen.PropertyName._bgContainer, Variant.From<Control>(ref this._bgContainer));
    info.AddProperty(NCharacterSelectScreen.PropertyName._readyAndWaitingContainer, Variant.From<Control>(ref this._readyAndWaitingContainer));
    info.AddProperty(NCharacterSelectScreen.PropertyName._backButton, Variant.From<NBackButton>(ref this._backButton));
    info.AddProperty(NCharacterSelectScreen.PropertyName._unreadyButton, Variant.From<NBackButton>(ref this._unreadyButton));
    info.AddProperty(NCharacterSelectScreen.PropertyName._embarkButton, Variant.From<NConfirmButton>(ref this._embarkButton));
    info.AddProperty(NCharacterSelectScreen.PropertyName._ascensionPanel, Variant.From<NAscensionPanel>(ref this._ascensionPanel));
    info.AddProperty(NCharacterSelectScreen.PropertyName._actDropdown, Variant.From<NActDropdown>(ref this._actDropdown));
    info.AddProperty(NCharacterSelectScreen.PropertyName._actDropdownLabel, Variant.From<MegaRichTextLabel>(ref this._actDropdownLabel));
    info.AddProperty(NCharacterSelectScreen.PropertyName._remotePlayerContainer, Variant.From<NRemoteLobbyPlayerContainer>(ref this._remotePlayerContainer));
    info.AddProperty(NCharacterSelectScreen.PropertyName._characterUnlockAnimationBackstop, Variant.From<Control>(ref this._characterUnlockAnimationBackstop));
    info.AddProperty(NCharacterSelectScreen.PropertyName._randomCharacterButton, Variant.From<NCharacterSelectButton>(ref this._randomCharacterButton));
    info.AddProperty(NCharacterSelectScreen.PropertyName._infoPanelTween, Variant.From<Tween>(ref this._infoPanelTween));
    info.AddProperty(NCharacterSelectScreen.PropertyName._infoPanelPosFinalVal, Variant.From<Vector2>(ref this._infoPanelPosFinalVal));
    info.AddProperty(NCharacterSelectScreen.PropertyName._delayEmbarkForCharacterSelect, Variant.From<bool>(ref this._delayEmbarkForCharacterSelect));
    info.AddProperty(NCharacterSelectScreen.PropertyName._charSelectButtonScene, Variant.From<PackedScene>(ref this._charSelectButtonScene));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._name, ref variant1))
      this._name = ((Variant) ref variant1).As<MegaLabel>();
    Variant variant2;
    if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._infoPanel, ref variant2))
      this._infoPanel = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._description, ref variant3))
      this._description = ((Variant) ref variant3).As<MegaRichTextLabel>();
    Variant variant4;
    if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._hp, ref variant4))
      this._hp = ((Variant) ref variant4).As<MegaLabel>();
    Variant variant5;
    if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._gold, ref variant5))
      this._gold = ((Variant) ref variant5).As<MegaLabel>();
    Variant variant6;
    if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._relicTitle, ref variant6))
      this._relicTitle = ((Variant) ref variant6).As<MegaRichTextLabel>();
    Variant variant7;
    if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._relicDescription, ref variant7))
      this._relicDescription = ((Variant) ref variant7).As<MegaRichTextLabel>();
    Variant variant8;
    if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._relicIcon, ref variant8))
      this._relicIcon = ((Variant) ref variant8).As<TextureRect>();
    Variant variant9;
    if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._relicIconOutline, ref variant9))
      this._relicIconOutline = ((Variant) ref variant9).As<TextureRect>();
    Variant variant10;
    if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._selectedButton, ref variant10))
      this._selectedButton = ((Variant) ref variant10).As<NCharacterSelectButton>();
    Variant variant11;
    if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._charButtonContainer, ref variant11))
      this._charButtonContainer = ((Variant) ref variant11).As<Control>();
    Variant variant12;
    if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._bgContainer, ref variant12))
      this._bgContainer = ((Variant) ref variant12).As<Control>();
    Variant variant13;
    if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._readyAndWaitingContainer, ref variant13))
      this._readyAndWaitingContainer = ((Variant) ref variant13).As<Control>();
    Variant variant14;
    if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._backButton, ref variant14))
      this._backButton = ((Variant) ref variant14).As<NBackButton>();
    Variant variant15;
    if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._unreadyButton, ref variant15))
      this._unreadyButton = ((Variant) ref variant15).As<NBackButton>();
    Variant variant16;
    if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._embarkButton, ref variant16))
      this._embarkButton = ((Variant) ref variant16).As<NConfirmButton>();
    Variant variant17;
    if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._ascensionPanel, ref variant17))
      this._ascensionPanel = ((Variant) ref variant17).As<NAscensionPanel>();
    Variant variant18;
    if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._actDropdown, ref variant18))
      this._actDropdown = ((Variant) ref variant18).As<NActDropdown>();
    Variant variant19;
    if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._actDropdownLabel, ref variant19))
      this._actDropdownLabel = ((Variant) ref variant19).As<MegaRichTextLabel>();
    Variant variant20;
    if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._remotePlayerContainer, ref variant20))
      this._remotePlayerContainer = ((Variant) ref variant20).As<NRemoteLobbyPlayerContainer>();
    Variant variant21;
    if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._characterUnlockAnimationBackstop, ref variant21))
      this._characterUnlockAnimationBackstop = ((Variant) ref variant21).As<Control>();
    Variant variant22;
    if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._randomCharacterButton, ref variant22))
      this._randomCharacterButton = ((Variant) ref variant22).As<NCharacterSelectButton>();
    Variant variant23;
    if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._infoPanelTween, ref variant23))
      this._infoPanelTween = ((Variant) ref variant23).As<Tween>();
    Variant variant24;
    if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._infoPanelPosFinalVal, ref variant24))
      this._infoPanelPosFinalVal = ((Variant) ref variant24).As<Vector2>();
    Variant variant25;
    if (info.TryGetProperty(NCharacterSelectScreen.PropertyName._delayEmbarkForCharacterSelect, ref variant25))
      this._delayEmbarkForCharacterSelect = ((Variant) ref variant25).As<bool>();
    Variant variant26;
    if (!info.TryGetProperty(NCharacterSelectScreen.PropertyName._charSelectButtonScene, ref variant26))
      return;
    this._charSelectButtonScene = ((Variant) ref variant26).As<PackedScene>();
  }

  public new class MethodName : NSubmenu.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName CheckForMultiplayerAscensionPopup = StringName.op_Implicit(nameof (CheckForMultiplayerAscensionPopup));
    public static readonly StringName InitializeSingleplayer = StringName.op_Implicit(nameof (InitializeSingleplayer));
    public static readonly StringName InitCharacterButtons = StringName.op_Implicit(nameof (InitCharacterButtons));
    public static readonly StringName UpdateRandomCharacterVisibility = StringName.op_Implicit(nameof (UpdateRandomCharacterVisibility));
    public static readonly StringName _Input = StringName.op_Implicit(nameof (_Input));
    public static readonly StringName DebugUnlockAllCharacters = StringName.op_Implicit(nameof (DebugUnlockAllCharacters));
    public new static readonly StringName OnSubmenuOpened = StringName.op_Implicit(nameof (OnSubmenuOpened));
    public new static readonly StringName OnSubmenuClosed = StringName.op_Implicit(nameof (OnSubmenuClosed));
    public static readonly StringName OnEmbarkPressed = StringName.op_Implicit(nameof (OnEmbarkPressed));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
    public static readonly StringName CleanUpLobby = StringName.op_Implicit(nameof (CleanUpLobby));
    public static readonly StringName OnAscensionPanelLevelChanged = StringName.op_Implicit(nameof (OnAscensionPanelLevelChanged));
    public static readonly StringName OnUnreadyPressed = StringName.op_Implicit(nameof (OnUnreadyPressed));
    public static readonly StringName UpdateRichPresence = StringName.op_Implicit(nameof (UpdateRichPresence));
    public static readonly StringName MaxAscensionChanged = StringName.op_Implicit(nameof (MaxAscensionChanged));
    public static readonly StringName AscensionChanged = StringName.op_Implicit(nameof (AscensionChanged));
    public static readonly StringName SeedChanged = StringName.op_Implicit(nameof (SeedChanged));
    public static readonly StringName ModifiersChanged = StringName.op_Implicit(nameof (ModifiersChanged));
    public static readonly StringName AfterInitialized = StringName.op_Implicit(nameof (AfterInitialized));
  }

  public new class PropertyName : NSubmenu.PropertyName
  {
    public new static readonly StringName InitialFocusedControl = StringName.op_Implicit(nameof (InitialFocusedControl));
    public static readonly StringName ShouldShowActDropdown = StringName.op_Implicit(nameof (ShouldShowActDropdown));
    public static readonly StringName _name = StringName.op_Implicit(nameof (_name));
    public static readonly StringName _infoPanel = StringName.op_Implicit(nameof (_infoPanel));
    public static readonly StringName _description = StringName.op_Implicit(nameof (_description));
    public static readonly StringName _hp = StringName.op_Implicit(nameof (_hp));
    public static readonly StringName _gold = StringName.op_Implicit(nameof (_gold));
    public static readonly StringName _relicTitle = StringName.op_Implicit(nameof (_relicTitle));
    public static readonly StringName _relicDescription = StringName.op_Implicit(nameof (_relicDescription));
    public static readonly StringName _relicIcon = StringName.op_Implicit(nameof (_relicIcon));
    public static readonly StringName _relicIconOutline = StringName.op_Implicit(nameof (_relicIconOutline));
    public static readonly StringName _selectedButton = StringName.op_Implicit(nameof (_selectedButton));
    public static readonly StringName _charButtonContainer = StringName.op_Implicit(nameof (_charButtonContainer));
    public static readonly StringName _bgContainer = StringName.op_Implicit(nameof (_bgContainer));
    public static readonly StringName _readyAndWaitingContainer = StringName.op_Implicit(nameof (_readyAndWaitingContainer));
    public new static readonly StringName _backButton = StringName.op_Implicit(nameof (_backButton));
    public static readonly StringName _unreadyButton = StringName.op_Implicit(nameof (_unreadyButton));
    public static readonly StringName _embarkButton = StringName.op_Implicit(nameof (_embarkButton));
    public static readonly StringName _ascensionPanel = StringName.op_Implicit(nameof (_ascensionPanel));
    public static readonly StringName _actDropdown = StringName.op_Implicit(nameof (_actDropdown));
    public static readonly StringName _actDropdownLabel = StringName.op_Implicit(nameof (_actDropdownLabel));
    public static readonly StringName _remotePlayerContainer = StringName.op_Implicit(nameof (_remotePlayerContainer));
    public static readonly StringName _characterUnlockAnimationBackstop = StringName.op_Implicit(nameof (_characterUnlockAnimationBackstop));
    public static readonly StringName _randomCharacterButton = StringName.op_Implicit(nameof (_randomCharacterButton));
    public static readonly StringName _infoPanelTween = StringName.op_Implicit(nameof (_infoPanelTween));
    public static readonly StringName _infoPanelPosFinalVal = StringName.op_Implicit(nameof (_infoPanelPosFinalVal));
    public static readonly StringName _delayEmbarkForCharacterSelect = StringName.op_Implicit(nameof (_delayEmbarkForCharacterSelect));
    public static readonly StringName _charSelectButtonScene = StringName.op_Implicit(nameof (_charSelectButtonScene));
  }

  public new class SignalName : NSubmenu.SignalName
  {
  }
}
