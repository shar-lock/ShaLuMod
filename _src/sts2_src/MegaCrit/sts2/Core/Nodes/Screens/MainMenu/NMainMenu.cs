// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NMainMenu
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
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Connection;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;
using MegaCrit.Sts2.Core.Nodes.Screens.ModdingScreen;
using MegaCrit.Sts2.Core.Nodes.Screens.ProfileScreen;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Nodes.Screens.Settings;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Platform.Steam;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.Settings;
using MegaCrit.Sts2.Core.Timeline.Epochs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;

[ScriptPath("res://src/Core/Nodes/Screens/MainMenu/NMainMenu.cs")]
public class NMainMenu : Control, IScreenContext
{
  private static readonly StringName _lod = new StringName("lod");
  private static readonly StringName _mixPercentage = new StringName("mix_percentage");
  private const string _scenePath = "res://scenes/screens/main_menu.tscn";
  private const string _menuMusicParam = "menu_progress";
  private Window _window;
  private NMainMenuTextButton _continueButton;
  private NMainMenuTextButton _abandonRunButton;
  private NMainMenuTextButton _singleplayerButton;
  private NMainMenuTextButton _compendiumButton;
  private NMainMenuTextButton _timelineButton;
  private NMainMenuTextButton _settingsButton;
  private NMainMenuTextButton _quitButton;
  private NMainMenuTextButton _multiplayerButton;
  private const float _reticleYOffset = 5f;
  private const float _reticlePadding = 28f;
  private Control _buttonReticleLeft;
  private Control _buttonReticleRight;
  private Tween? _reticleTween;
  private NPatchNotesButton _patchNotesButtonNode;
  private NOpenProfileScreenButton _openProfileScreenButton;
  private NMainMenuTextButton? _lastHitButton;
  private NContinueRunInfo _runInfo;
  private Control _timelineNotificationDot;
  private Tween? _backstopTween;
  private NMainMenuBg _bg;
  private ShaderMaterial _blur;
  private bool _openTimeline;
  private ReadSaveResult<SerializableRun>? _readRunSaveResult;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>("res://scenes/screens/main_menu.tscn");
    }
  }

  public NPatchNotesScreen PatchNotesScreen { get; private set; }

  public Control BlurBackstop { get; private set; }

  private NButton[] MainMenuButtons
  {
    get
    {
      return new NButton[8]
      {
        (NButton) this._continueButton,
        (NButton) this._abandonRunButton,
        (NButton) this._singleplayerButton,
        (NButton) this._multiplayerButton,
        (NButton) this._timelineButton,
        (NButton) this._settingsButton,
        (NButton) this._compendiumButton,
        (NButton) this._quitButton
      };
    }
  }

  public NMainMenuSubmenuStack SubmenuStack { get; private set; }

  public NContinueRunInfo ContinueRunInfo => this._runInfo;

  public static NMainMenu Create(bool openTimeline)
  {
    NMainMenu nmainMenu = PreloadManager.Cache.GetScene("res://scenes/screens/main_menu.tscn").Instantiate<NMainMenu>((PackedScene.GenEditState) 0L);
    nmainMenu._openTimeline = openTimeline;
    return nmainMenu;
  }

  public override void _Ready()
  {
    Log.Info($"[Startup] Time to main menu (Godot ticks): {Time.GetTicksMsec()}ms");
    this._window = ((Node) this).GetTree().Root;
    ((GodotObject) NGame.Instance).Connect(NGame.SignalName.WindowChange, Callable.From<bool>(new Action<bool>(this.OnWindowChange)), 0U);
    if (SaveManager.Instance.SettingsSave.AspectRatioSetting == AspectRatioSetting.Auto)
      this.OnWindowChange(true);
    this._continueButton = ((Node) this).GetNode<NMainMenuTextButton>(NodePath.op_Implicit("MainMenuTextButtons/ContinueButton"));
    ((GodotObject) this._continueButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnContinueButtonPressed)), 0U);
    this._continueButton.SetLocalization("CONTINUE");
    this._abandonRunButton = ((Node) this).GetNode<NMainMenuTextButton>(NodePath.op_Implicit("MainMenuTextButtons/AbandonRunButton"));
    ((GodotObject) this._abandonRunButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnAbandonRunButtonPressed)), 0U);
    this._abandonRunButton.SetLocalization("ABANDON_RUN");
    this._singleplayerButton = ((Node) this).GetNode<NMainMenuTextButton>(NodePath.op_Implicit("MainMenuTextButtons/SingleplayerButton"));
    ((GodotObject) this._singleplayerButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.SingleplayerButtonPressed)), 0U);
    this._singleplayerButton.SetLocalization("SINGLE_PLAYER");
    this._multiplayerButton = ((Node) this).GetNode<NMainMenuTextButton>(NodePath.op_Implicit("MainMenuTextButtons/MultiplayerButton"));
    ((GodotObject) this._multiplayerButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OpenMultiplayerSubmenu)), 0U);
    this._multiplayerButton.SetLocalization("MULTIPLAYER");
    this._compendiumButton = ((Node) this).GetNode<NMainMenuTextButton>(NodePath.op_Implicit("MainMenuTextButtons/CompendiumButton"));
    ((GodotObject) this._compendiumButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OpenCompendiumSubmenu)), 0U);
    this._compendiumButton.SetLocalization("COMPENDIUM");
    this._timelineButton = ((Node) this).GetNode<NMainMenuTextButton>(NodePath.op_Implicit("MainMenuTextButtons/TimelineButton"));
    ((GodotObject) this._timelineButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OpenTimelineScreen)), 0U);
    this._timelineButton.SetLocalization("TIMELINE");
    this._timelineNotificationDot = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%TimelineNotificationDot"));
    ((CanvasItem) this._timelineNotificationDot).Visible = SaveManager.Instance.GetDiscoveredEpochCount() > 0;
    this._settingsButton = ((Node) this).GetNode<NMainMenuTextButton>(NodePath.op_Implicit("MainMenuTextButtons/SettingsButton"));
    ((GodotObject) this._settingsButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OpenSettingsMenu)), 0U);
    this._settingsButton.SetLocalization("SETTINGS");
    this._quitButton = ((Node) this).GetNode<NMainMenuTextButton>(NodePath.op_Implicit("MainMenuTextButtons/QuitButton"));
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    ((GodotObject) this._quitButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(NMainMenu.\u003C\u003EO.\u003C0\u003E__Quit ?? (NMainMenu.\u003C\u003EO.\u003C0\u003E__Quit = new Action<NButton>(NMainMenu.Quit))), 0U);
    this._quitButton.SetLocalization("QUIT");
    this._buttonReticleLeft = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%ButtonReticleLeft"));
    this._buttonReticleRight = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%ButtonReticleRight"));
    this.ConnectMainMenuTextButtonFocusLogic();
    this.PatchNotesScreen = ((Node) this).GetNode<NPatchNotesScreen>(NodePath.op_Implicit("%PatchNotesScreen"));
    this.SubmenuStack = ((Node) this).GetNode<NMainMenuSubmenuStack>(NodePath.op_Implicit("%Submenus"));
    this._runInfo = ((Node) this).GetNode<NContinueRunInfo>(NodePath.op_Implicit("%ContinueRunInfo"));
    this._patchNotesButtonNode = ((Node) this).GetNode<NPatchNotesButton>(NodePath.op_Implicit("%PatchNotesButton"));
    ((GodotObject) this._patchNotesButtonNode).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OpenPatchNotes)), 0U);
    this._openProfileScreenButton = ((Node) this).GetNode<NOpenProfileScreenButton>(NodePath.op_Implicit("%ChangeProfileButton"));
    this._bg = ((Node) this).GetNode<NMainMenuBg>(NodePath.op_Implicit("%MainMenuBg"));
    this.BlurBackstop = ((Node) this).GetNode<Control>(NodePath.op_Implicit("BlurBackstop"));
    this._blur = (ShaderMaterial) ((CanvasItem) this.BlurBackstop).Material;
    ((CanvasItem) this._timelineButton).Visible = SaveManager.Instance.Progress.Epochs.Count > 0;
    NGame.Instance.SetScreenShakeTarget((Control) this);
    NAudioManager.Instance?.PlayMusic("event:/music/menu_update");
    this.SubmenuStack.InitializeForMainMenu(this);
    ((GodotObject) this.SubmenuStack).Connect(NSubmenuStack.SignalName.StackModified, Callable.From(new Action(this.OnSubmenuStackChanged)), 0U);
    this.OnSubmenuStackChanged();
    ActiveScreenContext.Instance.Update();
    this.RefreshButtons();
    this.CheckCommandLineArgs();
    if (SaveManager.Instance.SettingsSave.ModSettings == null && ModManager.Mods.Count > 0)
      NModalContainer.Instance.Add((Node) NConfirmModLoadingPopup.Create());
    TaskHelper.RunSafely(NGame.Instance.Transition.FadeIn(2f));
    PlatformUtil.SetRichPresence("MAIN_MENU", (string) null, new int?());
    if (this._openTimeline)
      TaskHelper.RunSafely(this.OpenTimelineFromGameOverScreen());
    if (!NGame.IsReleaseGame() || SaveManager.Instance.SettingsSave.SeenEaDisclaimer)
      return;
    NModalContainer.Instance.Add((Node) NEarlyAccessDisclaimer.Create());
  }

  private void ConnectMainMenuTextButtonFocusLogic()
  {
    foreach (NMainMenuTextButton nmainMenuTextButton in ((IEnumerable) ((Node) ((Node) this).GetNode<Control>(NodePath.op_Implicit("%MainMenuTextButtons"))).GetChildren(false)).OfType<NMainMenuTextButton>())
    {
      ((GodotObject) nmainMenuTextButton).Connect(NClickableControl.SignalName.Unfocused, Callable.From<NMainMenuTextButton>(new Action<NMainMenuTextButton>(this.MainMenuButtonUnfocused)), 0U);
      ((GodotObject) nmainMenuTextButton).Connect(NClickableControl.SignalName.Focused, Callable.From<NMainMenuTextButton>((Action<NMainMenuTextButton>) (b =>
      {
        Callable callable = Callable.From((Action) (() => this.MainMenuButtonFocused(b)));
        ((Callable) ref callable).CallDeferred(Array.Empty<Variant>());
      })), 0U);
    }
  }

  private void MainMenuButtonFocused(NMainMenuTextButton button)
  {
    this._reticleTween?.Kill();
    this._reticleTween = ((Node) this).CreateTween().SetParallel(true);
    this._buttonReticleLeft.GlobalPosition = new Vector2(0.0f, button.GlobalPosition.Y + 5f);
    this._buttonReticleRight.GlobalPosition = new Vector2(0.0f, button.GlobalPosition.Y + 5f);
    MegaLabel label1 = button.label;
    float num1 = label1 != null ? ((Control) label1).GlobalPosition.X : 0.0f;
    MegaLabel label2 = button.label;
    float num2 = label2 != null ? ((Control) label2).Size.X : 0.0f;
    float num3 = (float) ((double) num1 - 20.0 - 6.0);
    float num4 = (float) ((double) num1 + (double) num2 - 20.0 + 6.0);
    this._reticleTween.TweenProperty((GodotObject) this._buttonReticleLeft, NodePath.op_Implicit("global_position:x"), Variant.op_Implicit(num3 - 28f), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L).From(Variant.op_Implicit(num3));
    this._reticleTween.TweenProperty((GodotObject) this._buttonReticleLeft, NodePath.op_Implicit("modulate"), Variant.op_Implicit(StsColors.gold), 0.05).From(Variant.op_Implicit(StsColors.transparentWhite));
    this._reticleTween.TweenProperty((GodotObject) this._buttonReticleRight, NodePath.op_Implicit("global_position:x"), Variant.op_Implicit(num4 + 28f), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L).From(Variant.op_Implicit(num4));
    this._reticleTween.TweenProperty((GodotObject) this._buttonReticleRight, NodePath.op_Implicit("modulate"), Variant.op_Implicit(StsColors.gold), 0.05).From(Variant.op_Implicit(StsColors.transparentWhite));
  }

  private void MainMenuButtonUnfocused(NMainMenuTextButton obj)
  {
    this._reticleTween?.Kill();
    this._reticleTween = ((Node) this).CreateTween().SetParallel(true);
    this._reticleTween.TweenProperty((GodotObject) this._buttonReticleLeft, NodePath.op_Implicit("modulate"), Variant.op_Implicit(StsColors.transparentWhite), 0.25);
    this._reticleTween.TweenProperty((GodotObject) this._buttonReticleRight, NodePath.op_Implicit("modulate"), Variant.op_Implicit(StsColors.transparentWhite), 0.25);
  }

  private async Task OpenTimelineFromGameOverScreen()
  {
    if (SaveManager.Instance.PrefsSave.FastMode != FastModeType.Instant)
      await Task.Delay(500);
    this.SubmenuStack.PushSubmenuType<NTimelineScreen>();
  }

  public void EnableBackstop()
  {
    this._bg.HideLogo();
    this.BlurBackstop.MouseFilter = (Control.MouseFilterEnum) 0L;
    this._backstopTween?.Kill();
    this._backstopTween = ((Node) this).CreateTween().SetParallel(true);
    this._backstopTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderLod)), this._blur.GetShaderParameter(NMainMenu._lod), Variant.op_Implicit(3f), 0.25);
    this._backstopTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderMix)), this._blur.GetShaderParameter(NMainMenu._mixPercentage), Variant.op_Implicit(0.7f), 0.25);
  }

  public void DisableBackstop()
  {
    this._bg.ShowLogo();
    this.BlurBackstop.MouseFilter = (Control.MouseFilterEnum) 2L;
    this._backstopTween?.Kill();
    this._backstopTween = ((Node) this).CreateTween().SetParallel(true);
    this._backstopTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderLod)), this._blur.GetShaderParameter(NMainMenu._lod), Variant.op_Implicit(0.0f), 0.15);
    this._backstopTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderMix)), this._blur.GetShaderParameter(NMainMenu._mixPercentage), Variant.op_Implicit(0.0f), 0.15);
  }

  public void DisableBackstopInstantly()
  {
    this._backstopTween?.Kill();
    this._blur.SetShaderParameter(NMainMenu._lod, Variant.op_Implicit(0.0f));
    this._blur.SetShaderParameter(NMainMenu._mixPercentage, Variant.op_Implicit(0.0f));
  }

  public void EnableBackstopInstantly()
  {
    this._backstopTween?.Kill();
    this._blur.SetShaderParameter(NMainMenu._lod, Variant.op_Implicit(3f));
    this._blur.SetShaderParameter(NMainMenu._mixPercentage, Variant.op_Implicit(0.7f));
  }

  private void UpdateShaderMix(float obj)
  {
    this._blur.SetShaderParameter(NMainMenu._mixPercentage, Variant.op_Implicit(obj));
  }

  private void UpdateShaderLod(float obj)
  {
    this._blur.SetShaderParameter(NMainMenu._lod, Variant.op_Implicit(obj));
  }

  public void RefreshButtons()
  {
    if (SaveManager.Instance.HasRunSave)
    {
      this._readRunSaveResult = SaveManager.Instance.LoadRunSave();
      ((CanvasItem) this._singleplayerButton).Visible = false;
      ((CanvasItem) this._abandonRunButton).Visible = true;
      ((CanvasItem) this._continueButton).Visible = true;
      this._continueButton.SetEnabled(true);
      this._runInfo.SetResult(this._readRunSaveResult);
    }
    else
    {
      this._readRunSaveResult = (ReadSaveResult<SerializableRun>) null;
      ((CanvasItem) this._singleplayerButton).Visible = true;
      ((CanvasItem) this._abandonRunButton).Visible = false;
      ((CanvasItem) this._continueButton).Visible = false;
      this._continueButton.SetEnabled(false);
      this._runInfo.SetResult((ReadSaveResult<SerializableRun>) null);
    }
    this.UpdateTimelineButtonBehavior();
    ((CanvasItem) this._compendiumButton).Visible = SaveManager.Instance.IsCompendiumAvailable();
    ActiveScreenContext.Instance.Update();
  }

  private void UpdateTimelineButtonBehavior()
  {
    if (!DebugSettings.DevSkip && SaveManager.Instance.GetDiscoveredEpochCount() > 0 && !SaveManager.Instance.HasRunSave)
    {
      this._timelineButton.Enable();
      this._singleplayerButton.Disable();
      this._multiplayerButton.Disable();
      this._compendiumButton.Disable();
      ((CanvasItem) this._timelineButton).Visible = true;
      ((CanvasItem) this._timelineNotificationDot).Visible = true;
    }
    else if (SaveManager.Instance.Progress.Epochs.Count > 1 && SaveManager.Instance.IsEpochRevealed<NeowEpoch>())
    {
      ((CanvasItem) this._timelineButton).Visible = true;
      if (SaveManager.Instance.GetDiscoveredEpochCount() == 0)
        this._timelineButton.Enable();
      else
        this._timelineButton.Disable();
      ((CanvasItem) this._timelineNotificationDot).Visible = false;
      this._singleplayerButton.Enable();
      this._multiplayerButton.Enable();
      this._compendiumButton.Enable();
    }
    else if (SaveManager.Instance.Progress.Epochs.Count > 1)
      this._timelineButton.Disable();
    else
      ((CanvasItem) this._timelineButton).Visible = false;
  }

  public Control DefaultFocusedControl
  {
    get
    {
      return this._lastHitButton == null || !((CanvasItem) this._lastHitButton).IsVisible() ? (Control) ((IEnumerable<NButton>) this.MainMenuButtons).First<NButton>((Func<NButton, bool>) (b => b.IsEnabled && ((CanvasItem) b).IsVisible())) : (Control) this._lastHitButton;
    }
  }

  private void OnSubmenuStackChanged()
  {
    ((CanvasItem) ((Node) this).GetNode<Control>(NodePath.op_Implicit("MainMenuTextButtons"))).Visible = !this.SubmenuStack.SubmenusOpen;
    ((CanvasItem) this._patchNotesButtonNode).Visible = !this.SubmenuStack.SubmenusOpen;
    if (this.SubmenuStack.SubmenusOpen)
    {
      ((CanvasItem) this._openProfileScreenButton).Visible = false;
      ((CanvasItem) this._patchNotesButtonNode).Visible = false;
      this._openProfileScreenButton.Disable();
      this._patchNotesButtonNode.Disable();
    }
    else
    {
      ((CanvasItem) this._openProfileScreenButton).Visible = true;
      ((CanvasItem) this._patchNotesButtonNode).Visible = true;
      this._openProfileScreenButton.Enable();
      this._patchNotesButtonNode.Enable();
      NAudioManager.Instance?.UpdateMusicParameter("menu_progress", "main");
    }
  }

  private void OnContinueButtonPressed(NButton _)
  {
    if (this._readRunSaveResult == null || !this._readRunSaveResult.Success || this._readRunSaveResult.SaveData == null)
      this.DisplayLoadSaveError();
    else
      TaskHelper.RunSafely(this.OnContinueButtonPressedAsync());
  }

  private async Task OnContinueButtonPressedAsync()
  {
    try
    {
      this._continueButton.Disable();
      NAudioManager.Instance?.StopMusic();
      SerializableRun serializableRun = this._readRunSaveResult.SaveData;
      RunState runState = RunState.FromSerializable(serializableRun);
      await RunManager.Instance.SetUpSavedSingleplayer(runState, serializableRun);
      Log.Info($"Continuing run with character: {serializableRun.Players[0].CharacterId}");
      SfxCmd.Play(runState.Players[0].Character.CharacterTransitionSfx);
      await NGame.Instance.Transition.FadeOut(transitionPath: runState.Players[0].Character.CharacterSelectTransitionPath);
      NGame.Instance.ReactionContainer.InitializeNetworking((INetGameService) new NetSingleplayerGameService());
      await NGame.Instance.LoadRun(runState, serializableRun.PreFinishedRoom);
      await NGame.Instance.Transition.FadeIn();
      serializableRun = (SerializableRun) null;
      runState = (RunState) null;
    }
    catch (Exception ex)
    {
      this.DisplayLoadSaveError();
      RunManager.Instance.CleanUp();
      throw;
    }
  }

  private void DisplayLoadSaveError()
  {
    NModalContainer.Instance.Add((Node) NErrorPopup.Create(new LocString("main_menu_ui", "INVALID_SAVE_POPUP.title"), new LocString("main_menu_ui", "INVALID_SAVE_POPUP.description_run"), new LocString("main_menu_ui", "INVALID_SAVE_POPUP.dismiss"), true));
    NModalContainer.Instance.ShowBackstop();
    this._continueButton.Disable();
  }

  private void OnAbandonRunButtonPressed(NButton _)
  {
    NModalContainer.Instance.Add((Node) NAbandonRunConfirmPopup.Create(this));
    this._lastHitButton = this._abandonRunButton;
  }

  public void AbandonRun()
  {
    if (this._readRunSaveResult == null)
      return;
    if (this._readRunSaveResult.Success)
    {
      if (this._readRunSaveResult.SaveData != null)
      {
        try
        {
          Log.Info("Abandoning run from main menu");
          SerializableRun saveData = this._readRunSaveResult.SaveData;
          SaveManager.Instance.UpdateProgressWithRunData(saveData, false);
          RunHistoryUtilities.CreateRunHistoryEntry(saveData, false, true, saveData.PlatformType);
          if (saveData.DailyTime.HasValue)
          {
            int dailyScore = ScoreUtility.CalculateDailyScore(saveData, saveData.Players.First<SerializablePlayer>().NetId, false);
            TaskHelper.RunSafely(DailyRunUtility.UploadScore(saveData.DailyTime.Value, dailyScore, saveData.Players));
            goto label_8;
          }
          goto label_8;
        }
        catch (Exception ex)
        {
          Log.Error($"ERROR: Failed to upload run history/metrics: {ex}");
          goto label_8;
        }
      }
    }
    Log.Info($"Abandoning run with invalid save (status={this._readRunSaveResult.Status})");
label_8:
    SaveManager.Instance.DeleteCurrentRun();
    this.RefreshButtons();
    GC.Collect();
  }

  private void SingleplayerButtonPressed(NButton _)
  {
    if (SaveManager.Instance.Progress.NumberOfRuns > 0)
    {
      this.OpenSingleplayerSubmenu();
    }
    else
    {
      NCharacterSelectScreen submenuType = this.SubmenuStack.GetSubmenuType<NCharacterSelectScreen>();
      submenuType.InitializeSingleplayer();
      this.SubmenuStack.Push((NSubmenu) submenuType);
      this._lastHitButton = this._singleplayerButton;
    }
  }

  public NSingleplayerSubmenu OpenSingleplayerSubmenu()
  {
    this._lastHitButton = this._singleplayerButton;
    return this.SubmenuStack.PushSubmenuType<NSingleplayerSubmenu>();
  }

  public void OpenMultiplayerSubmenu(NButton _) => this.OpenMultiplayerSubmenu();

  public NMultiplayerSubmenu OpenMultiplayerSubmenu()
  {
    this._lastHitButton = this._multiplayerButton;
    return this.SubmenuStack.PushSubmenuType<NMultiplayerSubmenu>();
  }

  private void OpenCompendiumSubmenu(NButton _) => this.OpenCompendiumSubmenu();

  public NCompendiumSubmenu OpenCompendiumSubmenu()
  {
    this._lastHitButton = this._compendiumButton;
    return this.SubmenuStack.PushSubmenuType<NCompendiumSubmenu>();
  }

  private void OpenTimelineScreen(NButton obj)
  {
    this._lastHitButton = this._timelineButton;
    NAudioManager.Instance?.UpdateMusicParameter("menu_progress", "timeline");
    this.SubmenuStack.PushSubmenuType<NTimelineScreen>();
  }

  private void OpenSettingsMenu(NButton _) => this.OpenSettingsMenu();

  public void OpenProfileScreen() => this.SubmenuStack.PushSubmenuType<NProfileScreen>();

  public void OpenSettingsMenu()
  {
    this._lastHitButton = this._settingsButton;
    this.SubmenuStack.PushSubmenuType<NSettingsScreen>();
  }

  private void OpenPatchNotes(NButton _) => this.PatchNotesScreen.Open();

  public async Task JoinGame(IClientConnectionInitializer connInitializer)
  {
    await this.OpenMultiplayerSubmenu().OnJoinFriendsPressed().JoinGameAsync(connInitializer);
  }

  private static void Quit(NButton _)
  {
    Log.Info("Quit button pressed");
    TaskHelper.RunSafely(NMainMenu.ConfirmAndQuit());
  }

  private static async Task ConfirmAndQuit()
  {
    NGenericPopup modalToCreate = NGenericPopup.Create();
    NModalContainer.Instance.Add((Node) modalToCreate);
    if (await modalToCreate.WaitForConfirmation(new LocString("main_menu_ui", "QUIT_CONFIRM_POPUP.body"), new LocString("main_menu_ui", "QUIT_CONFIRM_POPUP.header"), new LocString("main_menu_ui", "GENERIC_POPUP.cancel"), new LocString("main_menu_ui", "GENERIC_POPUP.confirm")))
    {
      Log.Info("Quit confirmed");
      NGame.Instance?.Quit();
    }
    else
      Log.Info("Quit cancelled");
  }

  private void OnWindowChange(bool isAspectRatioAuto)
  {
    if (!isAspectRatioAuto)
      return;
    float num = (float) this._window.Size.X / (float) this._window.Size.Y;
    if ((double) num > 2.3888888359069824)
    {
      this._window.ContentScaleAspect = (Window.ContentScaleAspectEnum) 2L;
      this._window.ContentScaleSize = new Vector2I(2580, 1080);
    }
    else if ((double) num < 1.3333333730697632)
    {
      this._window.ContentScaleAspect = (Window.ContentScaleAspectEnum) 3L;
      this._window.ContentScaleSize = new Vector2I(1680, 1260);
    }
    else
    {
      this._window.ContentScaleAspect = (Window.ContentScaleAspectEnum) 4L;
      this._window.ContentScaleSize = new Vector2I(1680, 1080);
    }
  }

  private void CheckCommandLineArgs()
  {
    string str;
    if (!CommandLineHelper.TryGetValue("fastmp", out str))
      return;
    NMultiplayerSubmenu nmultiplayerSubmenu = this.OpenMultiplayerSubmenu();
    switch (str)
    {
      case "host":
      case "host_standard":
      case "host_daily":
      case "host_custom":
        GameMode gameMode = str == "host_standard" ? GameMode.Standard : (str == "host_daily" ? GameMode.Daily : (str == "host_custom" ? GameMode.Custom : GameMode.None));
        if (gameMode == GameMode.None)
          break;
        nmultiplayerSubmenu.FastHost(gameMode);
        break;
      case "load":
        ReadSaveResult<SerializableRun> readSaveResult = SaveManager.Instance.LoadAndCanonicalizeMultiplayerRunSave(PlatformUtil.GetLocalPlayerId(SteamInitializer.Initialized ? PlatformType.Steam : PlatformType.None));
        if (readSaveResult.SaveData != null)
        {
          nmultiplayerSubmenu.StartHost(readSaveResult.SaveData);
          break;
        }
        Log.Error("Failed to load multiplayer save");
        break;
      case "join":
        nmultiplayerSubmenu.OnJoinFriendsPressed();
        break;
      default:
        Log.Error($"fastmp command line argument passed with invalid value: {str}. Expected host, load, or join");
        break;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(32 /*0x20*/)
    {
      new MethodInfo(NMainMenu.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("openTimeline"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMainMenu.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMainMenu.MethodName.ConnectMainMenuTextButtonFocusLogic, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMainMenu.MethodName.MainMenuButtonFocused, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("button"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMainMenu.MethodName.MainMenuButtonUnfocused, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("obj"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMainMenu.MethodName.EnableBackstop, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMainMenu.MethodName.DisableBackstop, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMainMenu.MethodName.DisableBackstopInstantly, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMainMenu.MethodName.EnableBackstopInstantly, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMainMenu.MethodName.UpdateShaderMix, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("obj"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMainMenu.MethodName.UpdateShaderLod, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("obj"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMainMenu.MethodName.RefreshButtons, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMainMenu.MethodName.UpdateTimelineButtonBehavior, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMainMenu.MethodName.OnSubmenuStackChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMainMenu.MethodName.OnContinueButtonPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMainMenu.MethodName.DisplayLoadSaveError, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMainMenu.MethodName.OnAbandonRunButtonPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMainMenu.MethodName.AbandonRun, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMainMenu.MethodName.SingleplayerButtonPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMainMenu.MethodName.OpenSingleplayerSubmenu, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMainMenu.MethodName.OpenMultiplayerSubmenu, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMainMenu.MethodName.OpenMultiplayerSubmenu, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMainMenu.MethodName.OpenCompendiumSubmenu, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMainMenu.MethodName.OpenCompendiumSubmenu, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMainMenu.MethodName.OpenTimelineScreen, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("obj"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMainMenu.MethodName.OpenSettingsMenu, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMainMenu.MethodName.OpenProfileScreen, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMainMenu.MethodName.OpenSettingsMenu, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMainMenu.MethodName.OpenPatchNotes, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMainMenu.MethodName.Quit, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMainMenu.MethodName.OnWindowChange, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("isAspectRatioAuto"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMainMenu.MethodName.CheckCommandLineArgs, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMainMenu.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NMainMenu nmainMenu = NMainMenu.Create(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NMainMenu>(ref nmainMenu);
      return true;
    }
    if (StringName.op_Equality(ref method, NMainMenu.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMainMenu.MethodName.ConnectMainMenuTextButtonFocusLogic) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ConnectMainMenuTextButtonFocusLogic();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMainMenu.MethodName.MainMenuButtonFocused) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.MainMenuButtonFocused(VariantUtils.ConvertTo<NMainMenuTextButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMainMenu.MethodName.MainMenuButtonUnfocused) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.MainMenuButtonUnfocused(VariantUtils.ConvertTo<NMainMenuTextButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMainMenu.MethodName.EnableBackstop) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.EnableBackstop();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMainMenu.MethodName.DisableBackstop) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.DisableBackstop();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMainMenu.MethodName.DisableBackstopInstantly) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.DisableBackstopInstantly();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMainMenu.MethodName.EnableBackstopInstantly) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.EnableBackstopInstantly();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMainMenu.MethodName.UpdateShaderMix) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateShaderMix(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMainMenu.MethodName.UpdateShaderLod) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateShaderLod(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMainMenu.MethodName.RefreshButtons) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshButtons();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMainMenu.MethodName.UpdateTimelineButtonBehavior) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateTimelineButtonBehavior();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMainMenu.MethodName.OnSubmenuStackChanged) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSubmenuStackChanged();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMainMenu.MethodName.OnContinueButtonPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnContinueButtonPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMainMenu.MethodName.DisplayLoadSaveError) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.DisplayLoadSaveError();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMainMenu.MethodName.OnAbandonRunButtonPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnAbandonRunButtonPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMainMenu.MethodName.AbandonRun) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AbandonRun();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMainMenu.MethodName.SingleplayerButtonPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SingleplayerButtonPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMainMenu.MethodName.OpenSingleplayerSubmenu) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NSingleplayerSubmenu nsingleplayerSubmenu = this.OpenSingleplayerSubmenu();
      ret = VariantUtils.CreateFrom<NSingleplayerSubmenu>(ref nsingleplayerSubmenu);
      return true;
    }
    if (StringName.op_Equality(ref method, NMainMenu.MethodName.OpenMultiplayerSubmenu) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OpenMultiplayerSubmenu(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMainMenu.MethodName.OpenMultiplayerSubmenu) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NMultiplayerSubmenu nmultiplayerSubmenu = this.OpenMultiplayerSubmenu();
      ret = VariantUtils.CreateFrom<NMultiplayerSubmenu>(ref nmultiplayerSubmenu);
      return true;
    }
    if (StringName.op_Equality(ref method, NMainMenu.MethodName.OpenCompendiumSubmenu) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OpenCompendiumSubmenu(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMainMenu.MethodName.OpenCompendiumSubmenu) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NCompendiumSubmenu ncompendiumSubmenu = this.OpenCompendiumSubmenu();
      ret = VariantUtils.CreateFrom<NCompendiumSubmenu>(ref ncompendiumSubmenu);
      return true;
    }
    if (StringName.op_Equality(ref method, NMainMenu.MethodName.OpenTimelineScreen) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OpenTimelineScreen(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMainMenu.MethodName.OpenSettingsMenu) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OpenSettingsMenu(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMainMenu.MethodName.OpenProfileScreen) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OpenProfileScreen();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMainMenu.MethodName.OpenSettingsMenu) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OpenSettingsMenu();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMainMenu.MethodName.OpenPatchNotes) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OpenPatchNotes(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMainMenu.MethodName.Quit) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NMainMenu.Quit(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMainMenu.MethodName.OnWindowChange) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnWindowChange(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NMainMenu.MethodName.CheckCommandLineArgs) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.CheckCommandLineArgs();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMainMenu.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NMainMenu nmainMenu = NMainMenu.Create(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NMainMenu>(ref nmainMenu);
      return true;
    }
    if (StringName.op_Equality(ref method, NMainMenu.MethodName.Quit) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NMainMenu.Quit(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMainMenu.MethodName.Create) || StringName.op_Equality(ref method, NMainMenu.MethodName._Ready) || StringName.op_Equality(ref method, NMainMenu.MethodName.ConnectMainMenuTextButtonFocusLogic) || StringName.op_Equality(ref method, NMainMenu.MethodName.MainMenuButtonFocused) || StringName.op_Equality(ref method, NMainMenu.MethodName.MainMenuButtonUnfocused) || StringName.op_Equality(ref method, NMainMenu.MethodName.EnableBackstop) || StringName.op_Equality(ref method, NMainMenu.MethodName.DisableBackstop) || StringName.op_Equality(ref method, NMainMenu.MethodName.DisableBackstopInstantly) || StringName.op_Equality(ref method, NMainMenu.MethodName.EnableBackstopInstantly) || StringName.op_Equality(ref method, NMainMenu.MethodName.UpdateShaderMix) || StringName.op_Equality(ref method, NMainMenu.MethodName.UpdateShaderLod) || StringName.op_Equality(ref method, NMainMenu.MethodName.RefreshButtons) || StringName.op_Equality(ref method, NMainMenu.MethodName.UpdateTimelineButtonBehavior) || StringName.op_Equality(ref method, NMainMenu.MethodName.OnSubmenuStackChanged) || StringName.op_Equality(ref method, NMainMenu.MethodName.OnContinueButtonPressed) || StringName.op_Equality(ref method, NMainMenu.MethodName.DisplayLoadSaveError) || StringName.op_Equality(ref method, NMainMenu.MethodName.OnAbandonRunButtonPressed) || StringName.op_Equality(ref method, NMainMenu.MethodName.AbandonRun) || StringName.op_Equality(ref method, NMainMenu.MethodName.SingleplayerButtonPressed) || StringName.op_Equality(ref method, NMainMenu.MethodName.OpenSingleplayerSubmenu) || StringName.op_Equality(ref method, NMainMenu.MethodName.OpenMultiplayerSubmenu) || StringName.op_Equality(ref method, NMainMenu.MethodName.OpenCompendiumSubmenu) || StringName.op_Equality(ref method, NMainMenu.MethodName.OpenTimelineScreen) || StringName.op_Equality(ref method, NMainMenu.MethodName.OpenSettingsMenu) || StringName.op_Equality(ref method, NMainMenu.MethodName.OpenProfileScreen) || StringName.op_Equality(ref method, NMainMenu.MethodName.OpenPatchNotes) || StringName.op_Equality(ref method, NMainMenu.MethodName.Quit) || StringName.op_Equality(ref method, NMainMenu.MethodName.OnWindowChange) || StringName.op_Equality(ref method, NMainMenu.MethodName.CheckCommandLineArgs) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMainMenu.PropertyName.PatchNotesScreen))
    {
      this.PatchNotesScreen = VariantUtils.ConvertTo<NPatchNotesScreen>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenu.PropertyName.BlurBackstop))
    {
      this.BlurBackstop = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenu.PropertyName.SubmenuStack))
    {
      this.SubmenuStack = VariantUtils.ConvertTo<NMainMenuSubmenuStack>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenu.PropertyName._window))
    {
      this._window = VariantUtils.ConvertTo<Window>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenu.PropertyName._continueButton))
    {
      this._continueButton = VariantUtils.ConvertTo<NMainMenuTextButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenu.PropertyName._abandonRunButton))
    {
      this._abandonRunButton = VariantUtils.ConvertTo<NMainMenuTextButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenu.PropertyName._singleplayerButton))
    {
      this._singleplayerButton = VariantUtils.ConvertTo<NMainMenuTextButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenu.PropertyName._compendiumButton))
    {
      this._compendiumButton = VariantUtils.ConvertTo<NMainMenuTextButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenu.PropertyName._timelineButton))
    {
      this._timelineButton = VariantUtils.ConvertTo<NMainMenuTextButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenu.PropertyName._settingsButton))
    {
      this._settingsButton = VariantUtils.ConvertTo<NMainMenuTextButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenu.PropertyName._quitButton))
    {
      this._quitButton = VariantUtils.ConvertTo<NMainMenuTextButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenu.PropertyName._multiplayerButton))
    {
      this._multiplayerButton = VariantUtils.ConvertTo<NMainMenuTextButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenu.PropertyName._buttonReticleLeft))
    {
      this._buttonReticleLeft = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenu.PropertyName._buttonReticleRight))
    {
      this._buttonReticleRight = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenu.PropertyName._reticleTween))
    {
      this._reticleTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenu.PropertyName._patchNotesButtonNode))
    {
      this._patchNotesButtonNode = VariantUtils.ConvertTo<NPatchNotesButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenu.PropertyName._openProfileScreenButton))
    {
      this._openProfileScreenButton = VariantUtils.ConvertTo<NOpenProfileScreenButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenu.PropertyName._lastHitButton))
    {
      this._lastHitButton = VariantUtils.ConvertTo<NMainMenuTextButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenu.PropertyName._runInfo))
    {
      this._runInfo = VariantUtils.ConvertTo<NContinueRunInfo>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenu.PropertyName._timelineNotificationDot))
    {
      this._timelineNotificationDot = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenu.PropertyName._backstopTween))
    {
      this._backstopTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenu.PropertyName._bg))
    {
      this._bg = VariantUtils.ConvertTo<NMainMenuBg>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenu.PropertyName._blur))
    {
      this._blur = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMainMenu.PropertyName._openTimeline))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._openTimeline = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMainMenu.PropertyName.PatchNotesScreen))
    {
      ref godot_variant local = ref value;
      NPatchNotesScreen patchNotesScreen = this.PatchNotesScreen;
      godot_variant from = VariantUtils.CreateFrom<NPatchNotesScreen>(ref patchNotesScreen);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenu.PropertyName.BlurBackstop))
    {
      ref godot_variant local = ref value;
      Control blurBackstop = this.BlurBackstop;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref blurBackstop);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenu.PropertyName.MainMenuButtons))
    {
      value = VariantUtils.CreateFromSystemArrayOfGodotObject((GodotObject[]) this.MainMenuButtons);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenu.PropertyName.SubmenuStack))
    {
      ref godot_variant local = ref value;
      NMainMenuSubmenuStack submenuStack = this.SubmenuStack;
      godot_variant from = VariantUtils.CreateFrom<NMainMenuSubmenuStack>(ref submenuStack);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenu.PropertyName.ContinueRunInfo))
    {
      ref godot_variant local = ref value;
      NContinueRunInfo continueRunInfo = this.ContinueRunInfo;
      godot_variant from = VariantUtils.CreateFrom<NContinueRunInfo>(ref continueRunInfo);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenu.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenu.PropertyName._window))
    {
      value = VariantUtils.CreateFrom<Window>(ref this._window);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenu.PropertyName._continueButton))
    {
      value = VariantUtils.CreateFrom<NMainMenuTextButton>(ref this._continueButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenu.PropertyName._abandonRunButton))
    {
      value = VariantUtils.CreateFrom<NMainMenuTextButton>(ref this._abandonRunButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenu.PropertyName._singleplayerButton))
    {
      value = VariantUtils.CreateFrom<NMainMenuTextButton>(ref this._singleplayerButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenu.PropertyName._compendiumButton))
    {
      value = VariantUtils.CreateFrom<NMainMenuTextButton>(ref this._compendiumButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenu.PropertyName._timelineButton))
    {
      value = VariantUtils.CreateFrom<NMainMenuTextButton>(ref this._timelineButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenu.PropertyName._settingsButton))
    {
      value = VariantUtils.CreateFrom<NMainMenuTextButton>(ref this._settingsButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenu.PropertyName._quitButton))
    {
      value = VariantUtils.CreateFrom<NMainMenuTextButton>(ref this._quitButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenu.PropertyName._multiplayerButton))
    {
      value = VariantUtils.CreateFrom<NMainMenuTextButton>(ref this._multiplayerButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenu.PropertyName._buttonReticleLeft))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._buttonReticleLeft);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenu.PropertyName._buttonReticleRight))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._buttonReticleRight);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenu.PropertyName._reticleTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._reticleTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenu.PropertyName._patchNotesButtonNode))
    {
      value = VariantUtils.CreateFrom<NPatchNotesButton>(ref this._patchNotesButtonNode);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenu.PropertyName._openProfileScreenButton))
    {
      value = VariantUtils.CreateFrom<NOpenProfileScreenButton>(ref this._openProfileScreenButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenu.PropertyName._lastHitButton))
    {
      value = VariantUtils.CreateFrom<NMainMenuTextButton>(ref this._lastHitButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenu.PropertyName._runInfo))
    {
      value = VariantUtils.CreateFrom<NContinueRunInfo>(ref this._runInfo);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenu.PropertyName._timelineNotificationDot))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._timelineNotificationDot);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenu.PropertyName._backstopTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._backstopTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenu.PropertyName._bg))
    {
      value = VariantUtils.CreateFrom<NMainMenuBg>(ref this._bg);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenu.PropertyName._blur))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._blur);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMainMenu.PropertyName._openTimeline))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<bool>(ref this._openTimeline);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NMainMenu.PropertyName.PatchNotesScreen, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMainMenu.PropertyName._window, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMainMenu.PropertyName._continueButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMainMenu.PropertyName._abandonRunButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMainMenu.PropertyName._singleplayerButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMainMenu.PropertyName._compendiumButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMainMenu.PropertyName._timelineButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMainMenu.PropertyName._settingsButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMainMenu.PropertyName._quitButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMainMenu.PropertyName._multiplayerButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMainMenu.PropertyName._buttonReticleLeft, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMainMenu.PropertyName._buttonReticleRight, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMainMenu.PropertyName._reticleTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMainMenu.PropertyName._patchNotesButtonNode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMainMenu.PropertyName.BlurBackstop, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMainMenu.PropertyName._openProfileScreenButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 28L, NMainMenu.PropertyName.MainMenuButtons, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMainMenu.PropertyName._lastHitButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMainMenu.PropertyName._runInfo, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMainMenu.PropertyName._timelineNotificationDot, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMainMenu.PropertyName._backstopTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMainMenu.PropertyName._bg, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMainMenu.PropertyName._blur, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NMainMenu.PropertyName._openTimeline, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMainMenu.PropertyName.SubmenuStack, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMainMenu.PropertyName.ContinueRunInfo, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMainMenu.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo1 = info;
    StringName patchNotesScreen1 = NMainMenu.PropertyName.PatchNotesScreen;
    NPatchNotesScreen patchNotesScreen2 = this.PatchNotesScreen;
    Variant variant1 = Variant.From<NPatchNotesScreen>(ref patchNotesScreen2);
    serializationInfo1.AddProperty(patchNotesScreen1, variant1);
    GodotSerializationInfo serializationInfo2 = info;
    StringName blurBackstop1 = NMainMenu.PropertyName.BlurBackstop;
    Control blurBackstop2 = this.BlurBackstop;
    Variant variant2 = Variant.From<Control>(ref blurBackstop2);
    serializationInfo2.AddProperty(blurBackstop1, variant2);
    GodotSerializationInfo serializationInfo3 = info;
    StringName submenuStack1 = NMainMenu.PropertyName.SubmenuStack;
    NMainMenuSubmenuStack submenuStack2 = this.SubmenuStack;
    Variant variant3 = Variant.From<NMainMenuSubmenuStack>(ref submenuStack2);
    serializationInfo3.AddProperty(submenuStack1, variant3);
    info.AddProperty(NMainMenu.PropertyName._window, Variant.From<Window>(ref this._window));
    info.AddProperty(NMainMenu.PropertyName._continueButton, Variant.From<NMainMenuTextButton>(ref this._continueButton));
    info.AddProperty(NMainMenu.PropertyName._abandonRunButton, Variant.From<NMainMenuTextButton>(ref this._abandonRunButton));
    info.AddProperty(NMainMenu.PropertyName._singleplayerButton, Variant.From<NMainMenuTextButton>(ref this._singleplayerButton));
    info.AddProperty(NMainMenu.PropertyName._compendiumButton, Variant.From<NMainMenuTextButton>(ref this._compendiumButton));
    info.AddProperty(NMainMenu.PropertyName._timelineButton, Variant.From<NMainMenuTextButton>(ref this._timelineButton));
    info.AddProperty(NMainMenu.PropertyName._settingsButton, Variant.From<NMainMenuTextButton>(ref this._settingsButton));
    info.AddProperty(NMainMenu.PropertyName._quitButton, Variant.From<NMainMenuTextButton>(ref this._quitButton));
    info.AddProperty(NMainMenu.PropertyName._multiplayerButton, Variant.From<NMainMenuTextButton>(ref this._multiplayerButton));
    info.AddProperty(NMainMenu.PropertyName._buttonReticleLeft, Variant.From<Control>(ref this._buttonReticleLeft));
    info.AddProperty(NMainMenu.PropertyName._buttonReticleRight, Variant.From<Control>(ref this._buttonReticleRight));
    info.AddProperty(NMainMenu.PropertyName._reticleTween, Variant.From<Tween>(ref this._reticleTween));
    info.AddProperty(NMainMenu.PropertyName._patchNotesButtonNode, Variant.From<NPatchNotesButton>(ref this._patchNotesButtonNode));
    info.AddProperty(NMainMenu.PropertyName._openProfileScreenButton, Variant.From<NOpenProfileScreenButton>(ref this._openProfileScreenButton));
    info.AddProperty(NMainMenu.PropertyName._lastHitButton, Variant.From<NMainMenuTextButton>(ref this._lastHitButton));
    info.AddProperty(NMainMenu.PropertyName._runInfo, Variant.From<NContinueRunInfo>(ref this._runInfo));
    info.AddProperty(NMainMenu.PropertyName._timelineNotificationDot, Variant.From<Control>(ref this._timelineNotificationDot));
    info.AddProperty(NMainMenu.PropertyName._backstopTween, Variant.From<Tween>(ref this._backstopTween));
    info.AddProperty(NMainMenu.PropertyName._bg, Variant.From<NMainMenuBg>(ref this._bg));
    info.AddProperty(NMainMenu.PropertyName._blur, Variant.From<ShaderMaterial>(ref this._blur));
    info.AddProperty(NMainMenu.PropertyName._openTimeline, Variant.From<bool>(ref this._openTimeline));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NMainMenu.PropertyName.PatchNotesScreen, ref variant1))
      this.PatchNotesScreen = ((Variant) ref variant1).As<NPatchNotesScreen>();
    Variant variant2;
    if (info.TryGetProperty(NMainMenu.PropertyName.BlurBackstop, ref variant2))
      this.BlurBackstop = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NMainMenu.PropertyName.SubmenuStack, ref variant3))
      this.SubmenuStack = ((Variant) ref variant3).As<NMainMenuSubmenuStack>();
    Variant variant4;
    if (info.TryGetProperty(NMainMenu.PropertyName._window, ref variant4))
      this._window = ((Variant) ref variant4).As<Window>();
    Variant variant5;
    if (info.TryGetProperty(NMainMenu.PropertyName._continueButton, ref variant5))
      this._continueButton = ((Variant) ref variant5).As<NMainMenuTextButton>();
    Variant variant6;
    if (info.TryGetProperty(NMainMenu.PropertyName._abandonRunButton, ref variant6))
      this._abandonRunButton = ((Variant) ref variant6).As<NMainMenuTextButton>();
    Variant variant7;
    if (info.TryGetProperty(NMainMenu.PropertyName._singleplayerButton, ref variant7))
      this._singleplayerButton = ((Variant) ref variant7).As<NMainMenuTextButton>();
    Variant variant8;
    if (info.TryGetProperty(NMainMenu.PropertyName._compendiumButton, ref variant8))
      this._compendiumButton = ((Variant) ref variant8).As<NMainMenuTextButton>();
    Variant variant9;
    if (info.TryGetProperty(NMainMenu.PropertyName._timelineButton, ref variant9))
      this._timelineButton = ((Variant) ref variant9).As<NMainMenuTextButton>();
    Variant variant10;
    if (info.TryGetProperty(NMainMenu.PropertyName._settingsButton, ref variant10))
      this._settingsButton = ((Variant) ref variant10).As<NMainMenuTextButton>();
    Variant variant11;
    if (info.TryGetProperty(NMainMenu.PropertyName._quitButton, ref variant11))
      this._quitButton = ((Variant) ref variant11).As<NMainMenuTextButton>();
    Variant variant12;
    if (info.TryGetProperty(NMainMenu.PropertyName._multiplayerButton, ref variant12))
      this._multiplayerButton = ((Variant) ref variant12).As<NMainMenuTextButton>();
    Variant variant13;
    if (info.TryGetProperty(NMainMenu.PropertyName._buttonReticleLeft, ref variant13))
      this._buttonReticleLeft = ((Variant) ref variant13).As<Control>();
    Variant variant14;
    if (info.TryGetProperty(NMainMenu.PropertyName._buttonReticleRight, ref variant14))
      this._buttonReticleRight = ((Variant) ref variant14).As<Control>();
    Variant variant15;
    if (info.TryGetProperty(NMainMenu.PropertyName._reticleTween, ref variant15))
      this._reticleTween = ((Variant) ref variant15).As<Tween>();
    Variant variant16;
    if (info.TryGetProperty(NMainMenu.PropertyName._patchNotesButtonNode, ref variant16))
      this._patchNotesButtonNode = ((Variant) ref variant16).As<NPatchNotesButton>();
    Variant variant17;
    if (info.TryGetProperty(NMainMenu.PropertyName._openProfileScreenButton, ref variant17))
      this._openProfileScreenButton = ((Variant) ref variant17).As<NOpenProfileScreenButton>();
    Variant variant18;
    if (info.TryGetProperty(NMainMenu.PropertyName._lastHitButton, ref variant18))
      this._lastHitButton = ((Variant) ref variant18).As<NMainMenuTextButton>();
    Variant variant19;
    if (info.TryGetProperty(NMainMenu.PropertyName._runInfo, ref variant19))
      this._runInfo = ((Variant) ref variant19).As<NContinueRunInfo>();
    Variant variant20;
    if (info.TryGetProperty(NMainMenu.PropertyName._timelineNotificationDot, ref variant20))
      this._timelineNotificationDot = ((Variant) ref variant20).As<Control>();
    Variant variant21;
    if (info.TryGetProperty(NMainMenu.PropertyName._backstopTween, ref variant21))
      this._backstopTween = ((Variant) ref variant21).As<Tween>();
    Variant variant22;
    if (info.TryGetProperty(NMainMenu.PropertyName._bg, ref variant22))
      this._bg = ((Variant) ref variant22).As<NMainMenuBg>();
    Variant variant23;
    if (info.TryGetProperty(NMainMenu.PropertyName._blur, ref variant23))
      this._blur = ((Variant) ref variant23).As<ShaderMaterial>();
    Variant variant24;
    if (!info.TryGetProperty(NMainMenu.PropertyName._openTimeline, ref variant24))
      return;
    this._openTimeline = ((Variant) ref variant24).As<bool>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName ConnectMainMenuTextButtonFocusLogic = StringName.op_Implicit(nameof (ConnectMainMenuTextButtonFocusLogic));
    public static readonly StringName MainMenuButtonFocused = StringName.op_Implicit(nameof (MainMenuButtonFocused));
    public static readonly StringName MainMenuButtonUnfocused = StringName.op_Implicit(nameof (MainMenuButtonUnfocused));
    public static readonly StringName EnableBackstop = StringName.op_Implicit(nameof (EnableBackstop));
    public static readonly StringName DisableBackstop = StringName.op_Implicit(nameof (DisableBackstop));
    public static readonly StringName DisableBackstopInstantly = StringName.op_Implicit(nameof (DisableBackstopInstantly));
    public static readonly StringName EnableBackstopInstantly = StringName.op_Implicit(nameof (EnableBackstopInstantly));
    public static readonly StringName UpdateShaderMix = StringName.op_Implicit(nameof (UpdateShaderMix));
    public static readonly StringName UpdateShaderLod = StringName.op_Implicit(nameof (UpdateShaderLod));
    public static readonly StringName RefreshButtons = StringName.op_Implicit(nameof (RefreshButtons));
    public static readonly StringName UpdateTimelineButtonBehavior = StringName.op_Implicit(nameof (UpdateTimelineButtonBehavior));
    public static readonly StringName OnSubmenuStackChanged = StringName.op_Implicit(nameof (OnSubmenuStackChanged));
    public static readonly StringName OnContinueButtonPressed = StringName.op_Implicit(nameof (OnContinueButtonPressed));
    public static readonly StringName DisplayLoadSaveError = StringName.op_Implicit(nameof (DisplayLoadSaveError));
    public static readonly StringName OnAbandonRunButtonPressed = StringName.op_Implicit(nameof (OnAbandonRunButtonPressed));
    public static readonly StringName AbandonRun = StringName.op_Implicit(nameof (AbandonRun));
    public static readonly StringName SingleplayerButtonPressed = StringName.op_Implicit(nameof (SingleplayerButtonPressed));
    public static readonly StringName OpenSingleplayerSubmenu = StringName.op_Implicit(nameof (OpenSingleplayerSubmenu));
    public static readonly StringName OpenMultiplayerSubmenu = StringName.op_Implicit(nameof (OpenMultiplayerSubmenu));
    public static readonly StringName OpenCompendiumSubmenu = StringName.op_Implicit(nameof (OpenCompendiumSubmenu));
    public static readonly StringName OpenTimelineScreen = StringName.op_Implicit(nameof (OpenTimelineScreen));
    public static readonly StringName OpenSettingsMenu = StringName.op_Implicit(nameof (OpenSettingsMenu));
    public static readonly StringName OpenProfileScreen = StringName.op_Implicit(nameof (OpenProfileScreen));
    public static readonly StringName OpenPatchNotes = StringName.op_Implicit(nameof (OpenPatchNotes));
    public static readonly StringName Quit = StringName.op_Implicit(nameof (Quit));
    public static readonly StringName OnWindowChange = StringName.op_Implicit(nameof (OnWindowChange));
    public static readonly StringName CheckCommandLineArgs = StringName.op_Implicit(nameof (CheckCommandLineArgs));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName PatchNotesScreen = StringName.op_Implicit(nameof (PatchNotesScreen));
    public static readonly StringName BlurBackstop = StringName.op_Implicit(nameof (BlurBackstop));
    public static readonly StringName MainMenuButtons = StringName.op_Implicit(nameof (MainMenuButtons));
    public static readonly StringName SubmenuStack = StringName.op_Implicit(nameof (SubmenuStack));
    public static readonly StringName ContinueRunInfo = StringName.op_Implicit(nameof (ContinueRunInfo));
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _window = StringName.op_Implicit(nameof (_window));
    public static readonly StringName _continueButton = StringName.op_Implicit(nameof (_continueButton));
    public static readonly StringName _abandonRunButton = StringName.op_Implicit(nameof (_abandonRunButton));
    public static readonly StringName _singleplayerButton = StringName.op_Implicit(nameof (_singleplayerButton));
    public static readonly StringName _compendiumButton = StringName.op_Implicit(nameof (_compendiumButton));
    public static readonly StringName _timelineButton = StringName.op_Implicit(nameof (_timelineButton));
    public static readonly StringName _settingsButton = StringName.op_Implicit(nameof (_settingsButton));
    public static readonly StringName _quitButton = StringName.op_Implicit(nameof (_quitButton));
    public static readonly StringName _multiplayerButton = StringName.op_Implicit(nameof (_multiplayerButton));
    public static readonly StringName _buttonReticleLeft = StringName.op_Implicit(nameof (_buttonReticleLeft));
    public static readonly StringName _buttonReticleRight = StringName.op_Implicit(nameof (_buttonReticleRight));
    public static readonly StringName _reticleTween = StringName.op_Implicit(nameof (_reticleTween));
    public static readonly StringName _patchNotesButtonNode = StringName.op_Implicit(nameof (_patchNotesButtonNode));
    public static readonly StringName _openProfileScreenButton = StringName.op_Implicit(nameof (_openProfileScreenButton));
    public static readonly StringName _lastHitButton = StringName.op_Implicit(nameof (_lastHitButton));
    public static readonly StringName _runInfo = StringName.op_Implicit(nameof (_runInfo));
    public static readonly StringName _timelineNotificationDot = StringName.op_Implicit(nameof (_timelineNotificationDot));
    public static readonly StringName _backstopTween = StringName.op_Implicit(nameof (_backstopTween));
    public static readonly StringName _bg = StringName.op_Implicit(nameof (_bg));
    public static readonly StringName _blur = StringName.op_Implicit(nameof (_blur));
    public static readonly StringName _openTimeline = StringName.op_Implicit(nameof (_openTimeline));
  }

  public class SignalName : Control.SignalName
  {
  }
}
