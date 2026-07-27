// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.NGame
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.AutoSlay;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Leaderboard;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.Fonts;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Game.Lobby;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Debug;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Reaction;
using MegaCrit.Sts2.Core.Nodes.Screens;
using MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen;
using MegaCrit.Sts2.Core.Nodes.Screens.InspectScreens;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Nodes.Screens.Settings;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Platform.Steam;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.MapDrawing;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.Settings;
using MegaCrit.Sts2.Core.Unlocks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes;

[ScriptPath("res://src/Core/Nodes/NGame.cs")]
public class NGame : Control
{
  public static readonly Vector2 devResolution = new Vector2(1920f, 1080f);
  private readonly TaskCompletionSource _gameStartupComplete = new TaskCompletionSource();
  private Control _inspectionContainer;
  private NScreenShake _screenShake;
  private static int? _mainThreadId;
  private static Window _window = (Window) null;
  private CancellationTokenSource? _logoCancelToken;
  private SteamJoinCallbackHandler? _joinCallbackHandler;
  private 
  #nullable disable
  NGame.WindowChangeEventHandler backing_WindowChange;
  private NGame.PhobiaModeToggledEventHandler backing_PhobiaModeToggled;

  public static 
  #nullable enable
  NGame? Instance { get; private set; }

  public NSceneContainer RootSceneContainer { get; private set; }

  public Node? HoverTipsContainer { get; private set; }

  public NMainMenu? MainMenu => this.RootSceneContainer.CurrentScene as NMainMenu;

  public NRun? CurrentRunNode => this.RootSceneContainer.CurrentScene as NRun;

  public NLogoAnimation? LogoAnimation => this.RootSceneContainer.CurrentScene as NLogoAnimation;

  public NTransition Transition { get; private set; }

  public NMultiplayerTimeoutOverlay TimeoutOverlay { get; private set; }

  public NAudioManager AudioManager { get; private set; }

  public NRemoteMouseCursorContainer RemoteCursorContainer { get; private set; }

  public NInputManager InputManager { get; private set; }

  public NHotkeyManager HotkeyManager { get; private set; }

  public NSendFeedbackScreen? FeedbackScreen { get; private set; }

  public NReactionWheel ReactionWheel { get; private set; }

  public NReactionContainer ReactionContainer { get; private set; }

  public NCursorManager CursorManager { get; private set; }

  public NDebugAudioManager DebugAudio { get; private set; }

  public string? DebugSeedOverride { get; set; }

  public bool StartOnMainMenu { get; set; } = true;

  public Task GameStartupComplete => this._gameStartupComplete.Task;

  public static bool IsTrailerMode { get; private set; }

  public static bool IsDebugHidingHoverTips { get; private set; }

  public static bool IsDebugHidingProceedButton { get; private set; }

  public event Action? DebugToggleProceedButton;

  public NInspectRelicScreen? InspectRelicScreen { get; set; }

  public NInspectCardScreen? InspectCardScreen { get; set; }

  public Control? ScreenshakeTarget => this._screenShake.ShakeTarget;

  private WorldEnvironment WorldEnvironment { get; set; }

  private NHitStop HitStop { get; set; }

  public override void _EnterTree()
  {
    CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
    CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
    if (NGame.Instance != null)
    {
      Log.Error("NGame already exists.");
      ((Node) this).QueueFreeSafely();
    }
    else
    {
      NGame.Instance = this;
      SentryService.Initialize();
      this.RootSceneContainer = ((Node) this).GetNode<NSceneContainer>(NodePath.op_Implicit("%RootSceneContainer"));
      this.HoverTipsContainer = ((Node) this).GetNode<Node>(NodePath.op_Implicit("%HoverTipsContainer"));
      this.DebugAudio = ((Node) this).GetNode<NDebugAudioManager>(NodePath.op_Implicit("%DebugAudioManager"));
      this.AudioManager = ((Node) this).GetNode<NAudioManager>(NodePath.op_Implicit("%AudioManager"));
      this.RemoteCursorContainer = ((Node) this).GetNode<NRemoteMouseCursorContainer>(NodePath.op_Implicit("%RemoteCursorContainer"));
      this.InputManager = ((Node) this).GetNode<NInputManager>(NodePath.op_Implicit("%InputManager"));
      this.CursorManager = ((Node) this).GetNode<NCursorManager>(NodePath.op_Implicit("%CursorManager"));
      this.ReactionWheel = ((Node) this).GetNode<NReactionWheel>(NodePath.op_Implicit("%ReactionWheel"));
      this.ReactionContainer = ((Node) this).GetNode<NReactionContainer>(NodePath.op_Implicit("%ReactionContainer"));
      this.TimeoutOverlay = ((Node) this).GetNode<NMultiplayerTimeoutOverlay>(NodePath.op_Implicit("%MultiplayerTimeoutOverlay"));
      this.WorldEnvironment = ((Node) this).GetNode<WorldEnvironment>(NodePath.op_Implicit("%WorldEnvironment"));
      this.HotkeyManager = ((Node) this).GetNode<NHotkeyManager>(NodePath.op_Implicit("%HotkeyManager"));
      this._inspectionContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%InspectionContainer"));
      this._screenShake = ((Node) this).GetNode<NScreenShake>(NodePath.op_Implicit("ScreenShake"));
      this.HitStop = ((Node) this).GetNode<NHitStop>(NodePath.op_Implicit("HitStop"));
      this.Transition = ((Node) this).GetNode<NTransition>(NodePath.op_Implicit("%GameTransitionRect"));
      NGame._mainThreadId = new int?(Environment.CurrentManagedThreadId);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ((GodotObject) ((Node) this).GetWindow()).Connect(Window.SignalName.FilesDropped, Callable.From<string[]>(NGame.\u003C\u003EO.\u003C0\u003E__OnFilesDropped ?? (NGame.\u003C\u003EO.\u003C0\u003E__OnFilesDropped = new Action<string[]>(FileDropHandler.OnFilesDropped))), 0U);
      TaskHelper.RunSafely(this.GameStartupWrapper());
    }
  }

  public override void _Ready()
  {
    NGame._window = ((Node) this).GetTree().Root;
    ((GodotObject) NGame._window).Connect(Viewport.SignalName.SizeChanged, Callable.From(new Action(this.OnWindowChange)), 0U);
    ((Node) this).RemoveChildSafely((Node) this.WorldEnvironment);
  }

  private async Task GameStartupWrapper()
  {
    if (!await this.InitializePlatform())
    {
      this._gameStartupComplete.TrySetResult();
    }
    else
    {
      TaskHelper.RunSafely(OsDebugInfo.LogSystemInfo());
      TaskHelper.RunSafely(GitHelper.Initialize());
      try
      {
        await this.GameStartup();
        this._gameStartupComplete.TrySetResult();
      }
      catch
      {
        this._gameStartupComplete.TrySetResult();
        TaskHelper.RunSafely(this.GameStartupError());
        throw;
      }
    }
  }

  private async Task TryErrorInit()
  {
    try
    {
      if (SaveManager.Instance.SettingsSave == null)
        SaveManager.Instance.InitSettingsData();
      if (LocManager.Instance == null)
        LocManager.Initialize();
    }
    catch (Exception ex)
    {
      Log.Error($"Failed to show error dialog! Exception: {ex}");
      ((Node) this).GetTree().Quit(0);
      throw;
    }
    if (!((Node) this).IsNodeReady())
    {
      Variant[] signal = await ((GodotObject) this).ToSignal((GodotObject) this, Node.SignalName.Ready);
    }
    ((CanvasItem) this.Transition).Visible = false;
  }

  private async Task GameStartupError()
  {
    Log.Error("Encountered error on game startup! Attempting to show error dialog");
    await this.TryErrorInit();
    NGenericPopup modalToCreate = NGenericPopup.Create();
    if (modalToCreate == null || NModalContainer.Instance == null)
    {
      Log.Error("Cannot show error dialog: UI not initialized. Quitting immediately.");
      ((Node) this).GetTree().Quit(0);
    }
    else
    {
      NModalContainer.Instance.Add((Node) modalToCreate);
      int num = await modalToCreate.WaitForConfirmation(new LocString("main_menu_ui", "STARTUP_ERROR.description"), new LocString("main_menu_ui", "STARTUP_ERROR.title"), (LocString) null, new LocString("main_menu_ui", "QUIT")) ? 1 : 0;
      ((Node) this).GetTree().Quit(0);
    }
  }

  private async Task GameStartup()
  {
    await OneTimeInitialization.ExecuteVeryEarly();
    Node child = (Node) NDevConsole.Create();
    ((Node) this).AddChildSafely(child);
    ((Node) this).MoveChildSafely(child, ((Node) this).GetChildCount(false) - 1);
    AccountScopeUserDataMigrator.MigrateToUserScopedDirectories();
    AccountScopeUserDataMigrator.ArchiveLegacyData();
    ProfileAccountScopeMigrator.MigrateToProfileScopedDirectories();
    ProfileAccountScopeMigrator.ArchiveLegacyData();
    Task cloudSavesTask = this.DoCloudSync();
    this.InitPools();
    OneTimeInitialization.ExecuteEssential();
    if (!((Node) this).IsNodeReady())
    {
      Variant[] signal = await ((GodotObject) this).ToSignal((GodotObject) this, Node.SignalName.Ready);
    }
    Callable callable = Callable.From(new Action(this.InitializeGraphicsPreferences));
    ((Callable) ref callable).CallDeferred(Array.Empty<Variant>());
    this.AudioManager.SetMasterVol(SaveManager.Instance.SettingsSave.VolumeMaster);
    this.AudioManager.SetSfxVol(SaveManager.Instance.SettingsSave.VolumeSfx);
    this.AudioManager.SetAmbienceVol(SaveManager.Instance.SettingsSave.VolumeAmbience);
    this.AudioManager.SetBgmVol(SaveManager.Instance.SettingsSave.VolumeBgm);
    this.DebugAudio.SetMasterAudioVolume(SaveManager.Instance.SettingsSave.VolumeMaster);
    this.DebugAudio.SetSfxAudioVolume(SaveManager.Instance.SettingsSave.VolumeSfx);
    LeaderboardManager.Initialize();
    SteamStatsManager.Initialize();
    this.TimeoutOverlay.Relocalize();
    while (!cloudSavesTask.IsCompleted)
    {
      if (!SteamInitializer.Initialized)
      {
        Log.Error("Steam became uninitialized while the cloud sync was in-progress! This might result in unpredictable behavior");
        break;
      }
      double num = (double) await ((Node) this).AwaitProcessFrame();
    }
    SaveManager.Instance.InitProfileId();
    ReadSaveResult<SerializableProgress> progressReadResult = SaveManager.Instance.InitProgressData();
    ReadSaveResult<PrefsSave> prefsReadResult = SaveManager.Instance.InitPrefsData();
    SentryService.AfterGameInit(PlatformUtil.GetPlatformBranch().ToName(), SaveManager.Instance.Progress.UniqueId, (Node) ((Node) this).GetTree().Root);
    this._screenShake.SetMultiplier(NScreenshakePaginator.GetShakeMultiplier(SaveManager.Instance.PrefsSave.ScreenShakeOptionIndex));
    if (!OS.HasFeature("editor") && SaveManager.Instance.PrefsSave.FastMode == FastModeType.Instant)
      SaveManager.Instance.PrefsSave.FastMode = FastModeType.Fast;
    if (!NGame.IsReleaseGame() && CommandLineHelper.HasArg("autoslay"))
    {
      await this.LaunchMainMenu(true);
      new AutoSlayer().Start(CommandLineHelper.GetValue("seed") ?? SeedHelper.GetRandomSeed(), CommandLineHelper.GetValue("log-file"));
    }
    else if (CommandLineHelper.HasArg("bootstrap"))
      ((Node) this).AddChildSafely((Node) SceneHelper.Instantiate<NSceneBootstrapper>("debug/scene_bootstrapper"));
    else if (this.StartOnMainMenu)
    {
      await this.LaunchMainMenu(DebugSettings.DevSkip || SaveManager.Instance.SettingsSave.SkipIntroLogo || CommandLineHelper.HasArg("fastmp"));
      this.CheckShowSaveFileError(progressReadResult, prefsReadResult, OneTimeInitialization.SettingsReadResult);
      this.CheckShowLocalizationOverrideErrors();
      this.CheckShowModdedSaveFilePopup();
    }
    ModManager.OnModDetected += new Action<Mod>(this.OnNewModDetected);
    cloudSavesTask = (Task) null;
    progressReadResult = (ReadSaveResult<SerializableProgress>) null;
    prefsReadResult = (ReadSaveResult<PrefsSave>) null;
  }

  private async Task DoCloudSync()
  {
    if (SaveManager.Instance.ShouldOverwriteCloudWithLocal())
      await SaveManager.Instance.OverwriteCloudWithLocal();
    else
      await SaveManager.Instance.SyncCloudToLocal();
  }

  private void OnWindowChange()
  {
    Log.Info($"Window changed! New size: {DisplayServer.WindowGetSize(0)}");
    ((GodotObject) this).EmitSignal(NGame.SignalName.WindowChange, new Variant[1]
    {
      Variant.op_Implicit(SaveManager.Instance.SettingsSave.AspectRatioSetting == AspectRatioSetting.Auto)
    });
  }

  public static bool IsMainThread()
  {
    if (!NGame._mainThreadId.HasValue)
    {
      NGame._mainThreadId = new int?(Environment.CurrentManagedThreadId);
      return true;
    }
    int? mainThreadId = NGame._mainThreadId;
    int currentManagedThreadId = Environment.CurrentManagedThreadId;
    return mainThreadId.GetValueOrDefault() == currentManagedThreadId & mainThreadId.HasValue;
  }

  public override void _ExitTree()
  {
    ModManager.OnModDetected -= new Action<Mod>(this.OnNewModDetected);
    ModManager.Dispose();
    this._joinCallbackHandler?.Dispose();
    SteamInitializer.Uninitialize();
    SentryService.Shutdown();
  }

  public static bool IsReleaseGame() => true;

  private void InitializeGraphicsPreferences()
  {
    if (!DisplayServer.GetName().Equals("headless", StringComparison.OrdinalIgnoreCase))
    {
      this.ApplyDisplaySettings();
      NGame.ApplySyncSetting();
    }
    Engine.MaxFps = SaveManager.Instance.SettingsSave.FpsLimit;
  }

  public void ApplyDisplaySettings()
  {
    bool flag1 = false;
    SettingsSave settingsSave = SaveManager.Instance.SettingsSave;
    if (settingsSave.TargetDisplay == -1)
    {
      Log.Info("First time setup for display settings...");
      settingsSave.TargetDisplay = DisplayServer.GetPrimaryScreen();
    }
    bool flag2 = settingsSave.Fullscreen;
    if (PlatformUtil.GetSupportedWindowMode().ShouldForceFullscreen())
    {
      if (!flag2)
        Log.Warn($"Settings has fullscreen set to false, but we're forcing fullscreen because the platform reports our supported window mode as {PlatformUtil.GetSupportedWindowMode()}");
      flag2 = true;
    }
    Log.Info($"Applying display settings...\n  FULLSCREEN: {flag2}\n  ASPECT_RATIO: ({settingsSave.AspectRatioSetting})\n  TARGET_DISPLAY: ({settingsSave.TargetDisplay})\n  WINDOW_SIZE: {settingsSave.WindowSize}\n  POSITION: {settingsSave.WindowPosition}");
    Log.Info($"[Display] Min size: {DisplayServer.WindowGetMinSize(0)} Max size: {DisplayServer.WindowGetMaxSize(0)}");
    if (settingsSave.AspectRatioSetting != AspectRatioSetting.Auto)
      NGame._window.ContentScaleAspect = (Window.ContentScaleAspectEnum) 1L;
    switch (settingsSave.AspectRatioSetting)
    {
      case AspectRatioSetting.FourByThree:
        NGame._window.ContentScaleSize = new Vector2I(1680, 1260);
        break;
      case AspectRatioSetting.SixteenByTen:
        NGame._window.ContentScaleSize = new Vector2I(1920, 1200);
        break;
      case AspectRatioSetting.SixteenByNine:
        NGame._window.ContentScaleSize = new Vector2I(1920, 1080);
        break;
      case AspectRatioSetting.TwentyOneByNine:
        NGame._window.ContentScaleSize = new Vector2I(2580, 1080);
        break;
      case AspectRatioSetting.Auto:
        flag1 = true;
        break;
      default:
        throw new ArgumentOutOfRangeException($"Invalid Aspect Ratio: {settingsSave.AspectRatioSetting}");
    }
    int num = ((IReadOnlyList<string>) Environment.GetCommandLineArgs()).IndexOf<string>("-wpos");
    if (flag2 && num < 0)
    {
      if (NGame._window.Unresizable)
        NGame._window.Unresizable = false;
      Log.Info($"[Display] Setting FULLSCREEN on Display: {settingsSave.TargetDisplay + 1} of {DisplayServer.GetScreenCount()}");
      if (settingsSave.TargetDisplay >= DisplayServer.GetScreenCount())
      {
        Log.Warn($"[Display] FAILED: Display {settingsSave.TargetDisplay} is missing. Fallback to primary.");
        DisplayServer.WindowSetCurrentScreen(DisplayServer.GetPrimaryScreen(), 0);
        settingsSave.TargetDisplay = DisplayServer.GetPrimaryScreen();
      }
      else
        DisplayServer.WindowSetCurrentScreen(settingsSave.TargetDisplay, 0);
      DisplayServer.WindowSetMode((DisplayServer.WindowMode) 3L, 0);
    }
    else
    {
      Log.Info($"[Display] Attempting WINDOWED mode on Display {settingsSave.TargetDisplay + 1} of {DisplayServer.GetScreenCount()} at position {settingsSave.WindowPosition}");
      DisplayServer.WindowSetMode((DisplayServer.WindowMode) 0L, 0);
      if (NGame._window.Unresizable != !settingsSave.ResizeWindows)
        NGame._window.Unresizable = !settingsSave.ResizeWindows;
      if (num >= 0)
      {
        Log.Info("[Display] -wpos called. Applying special logic.");
        settingsSave.Fullscreen = false;
        Vector2I vector2I;
        // ISSUE: explicit constructor call
        ((Vector2I) ref vector2I).\u002Ector(int.Parse(Environment.GetCommandLineArgs()[num + 1]), int.Parse(Environment.GetCommandLineArgs()[num + 2]));
        Vector2I position = DisplayServer.ScreenGetPosition(DisplayServer.WindowGetCurrentScreen(0));
        Log.Info($"Applying window position from command line arg: {vector2I} ({string.Join(",", Environment.GetCommandLineArgs())} {position})");
        DisplayServer.WindowSetPosition(Vector2I.op_Addition(position, vector2I), 0);
        DisplayServer.WindowSetSize(settingsSave.WindowSize, 0);
      }
      else
      {
        Vector2I size = DisplayServer.ScreenGetSize(settingsSave.TargetDisplay);
        Vector2I windowSize = settingsSave.WindowSize;
        if (Vector2I.op_Equality(settingsSave.WindowPosition, new Vector2I(-1, -1)))
        {
          Log.Info($"[Display] Going from fullscreen to windowed. Attempting to center window on screen {settingsSave.TargetDisplay}");
          settingsSave.WindowPosition = Vector2I.op_Subtraction(Vector2I.op_Division(size, 2), Vector2I.op_Division(windowSize, 2));
        }
        Vector2I windowPosition = settingsSave.WindowPosition;
        if (windowPosition.X < 0 || windowPosition.Y < 0 || windowPosition.X > size.X || windowPosition.Y > size.Y)
        {
          Log.Warn("[Display] WARN: Game Window was offscreen. Resetting to top left corner.");
          // ISSUE: explicit constructor call
          ((Vector2I) ref windowPosition).\u002Ector(8, 48 /*0x30*/);
        }
        if (settingsSave.TargetDisplay >= DisplayServer.GetScreenCount())
        {
          Log.Info($"[Display] FAILED: Display {settingsSave.TargetDisplay + 1} is missing. Fallback to primary.");
          settingsSave.WindowPosition = new Vector2I(8, 48 /*0x30*/);
          DisplayServer.WindowSetSize(Vector2I.op_Subtraction(DisplayServer.ScreenGetSize(DisplayServer.GetPrimaryScreen()), new Vector2I(8, 48 /*0x30*/)), 0);
          DisplayServer.WindowSetPosition(Vector2I.op_Addition(DisplayServer.ScreenGetPosition(DisplayServer.GetPrimaryScreen()), settingsSave.WindowPosition), 0);
        }
        else
        {
          Vector2I position = DisplayServer.ScreenGetPosition(settingsSave.TargetDisplay);
          if (windowSize.X > size.X)
            windowSize.X = size.X;
          if (windowSize.Y > size.Y)
            windowSize.Y = size.Y;
          Log.Info($"[Display] SUCCESS: {windowSize} Windowed mode in Display {settingsSave.TargetDisplay}: Position {Vector2I.op_Addition(position, windowPosition)} ({windowPosition})");
          DisplayServer.WindowSetSize(windowSize, 0);
          DisplayServer.WindowSetPosition(Vector2I.op_Addition(position, windowPosition), 0);
          Log.Info($"[Display] New size: {DisplayServer.WindowGetSize(0)} position: {DisplayServer.WindowGetPosition(0)}");
        }
      }
    }
    if (!flag1)
      return;
    Log.Info("Manual window change signal because of auto scaling");
    ((GodotObject) this).EmitSignal(NGame.SignalName.WindowChange, new Variant[1]
    {
      Variant.op_Implicit(settingsSave.AspectRatioSetting == AspectRatioSetting.Auto)
    });
  }

  public NInspectRelicScreen GetInspectRelicScreen()
  {
    if (this.InspectRelicScreen == null)
    {
      this.InspectRelicScreen = NInspectRelicScreen.Create();
      ((Node) this._inspectionContainer).AddChildSafely((Node) this.InspectRelicScreen);
    }
    return this.InspectRelicScreen;
  }

  public NInspectCardScreen GetInspectCardScreen()
  {
    if (this.InspectCardScreen == null)
    {
      this.InspectCardScreen = NInspectCardScreen.Create();
      ((Node) this._inspectionContainer).AddChildSafely((Node) this.InspectCardScreen);
    }
    return this.InspectCardScreen;
  }

  public static void ApplySyncSetting()
  {
    switch (SaveManager.Instance.SettingsSave.VSync)
    {
      case VSyncType.Off:
        Log.Info("VSync: Off");
        DisplayServer.WindowSetVsyncMode((DisplayServer.VSyncMode) 0L, 0);
        break;
      case VSyncType.On:
        Log.Info("VSync: On");
        DisplayServer.WindowSetVsyncMode((DisplayServer.VSyncMode) 1L, 0);
        break;
      case VSyncType.Adaptive:
        Log.Info("VSync: Adaptive");
        DisplayServer.WindowSetVsyncMode((DisplayServer.VSyncMode) 2L, 0);
        break;
      default:
        Log.Error($"Invalid VSync type: {SaveManager.Instance.SettingsSave.VSync}");
        break;
    }
  }

  public static void Reset()
  {
    NGame instance = NGame.Instance;
    if (instance != null)
      ((Node) instance).QueueFreeSafely();
    NGame.Instance = (NGame) null;
  }

  public override void _Notification(int what)
  {
    if (what != 1006)
      return;
    this.Quit();
  }

  public void Quit()
  {
    Log.Info("NGame.Quit called");
    if (!PlatformUtil.GetSupportedWindowMode().ShouldForceFullscreen() && !SaveManager.Instance.SettingsSave.Fullscreen)
    {
      SettingsSave settingsSave = SaveManager.Instance.SettingsSave;
      settingsSave.WindowSize = DisplayServer.WindowGetSize(0);
      settingsSave.TargetDisplay = DisplayServer.WindowGetCurrentScreen(0);
      settingsSave.WindowPosition = Vector2I.op_Subtraction(DisplayServer.WindowGetPosition(0), DisplayServer.ScreenGetPosition(SaveManager.Instance.SettingsSave.TargetDisplay));
      Log.Info($"[Display] On exit, saving window size: {settingsSave.WindowSize} display: {settingsSave.TargetDisplay} position: {settingsSave.WindowPosition}");
    }
    SaveManager.Instance.SaveSettings();
    if (SaveManager.Instance.IsProfileInitialized)
    {
      SaveManager.Instance.SavePrefsFile();
      SaveManager.Instance.SaveProgressFile();
      SaveManager.Instance.SaveProfile();
    }
    SteamInitializer.SteamNoLongerRunning -= new Action(this.OnSteamNoLongerRunning);
    MegaLabel.DisposeCachedParagraph();
    MegaRichTextLabel.DisposeCachedParagraph();
    FontManager.ClearCache();
    ((Node) this).GetTree().Quit(0);
  }

  private async Task LaunchMainMenu(bool skipLogo)
  {
    NLogoAnimation logoAnimation = (NLogoAnimation) null;
    if (!skipLogo)
    {
      await PreloadManager.LoadLogoAnimation();
      logoAnimation = NLogoAnimation.Create();
      this.RootSceneContainer.SetCurrentScene((Control) logoAnimation);
      await PreloadManager.LoadMainMenuEssentials();
    }
    else
      await PreloadManager.LoadMainMenuEssentials();
    if (logoAnimation != null)
    {
      this._logoCancelToken = new CancellationTokenSource();
      await this.Transition.FadeIn(cancelToken: new CancellationToken?(this._logoCancelToken.Token));
      await logoAnimation.PlayAnimation(this._logoCancelToken.Token);
      await this.Transition.FadeOut();
    }
    await this.LoadMainMenu();
    Log.Info($"[Startup] Time to main menu: {Time.GetTicksMsec():N0}ms");
    NGame.LogResourceStats("main menu loaded (essential)");
    TaskHelper.RunSafely(this.LoadDeferredStartupAssetsAsync());
    SteamJoinCallbackHandler joinCallbackHandler = this._joinCallbackHandler;
    if (joinCallbackHandler == null)
    {
      logoAnimation = (NLogoAnimation) null;
    }
    else
    {
      joinCallbackHandler.CheckForCommandLineJoin();
      logoAnimation = (NLogoAnimation) null;
    }
  }

  private async Task LoadDeferredStartupAssetsAsync()
  {
    OneTimeInitialization.ExecuteDeferred();
    await PreloadManager.LoadCommonAndMainMenuAssets();
    NGame.LogResourceStats("main menu loaded (complete)");
  }

  public async Task GoToTimelineAfterRun() => await this.GoToTimeline();

  public async Task ReturnToMainMenuAfterRun() => await this.ReturnToMainMenu();

  public async Task ReturnToMainMenuWithInternalError(Exception e)
  {
    if (this.MainMenu == null)
      await this.ReturnToMainMenu();
    LocString body = new LocString("main_menu_ui", "INTERNAL_ERROR.description");
    body.Add("info", $"{e.GetType().Name}: {e.Message}");
    NModalContainer.Instance.Add((Node) NErrorPopup.Create(new LocString("main_menu_ui", "INTERNAL_ERROR.title"), body, (LocString) null, true));
  }

  public async Task GoToTimeline()
  {
    await this.Transition.FadeOut();
    await PreloadManager.LoadCommonAndMainMenuAssets();
    RunManager.Instance.CleanUp();
    await this.LoadMainMenu(true);
  }

  public async Task ReturnToMainMenu()
  {
    await this.Transition.FadeOut();
    await PreloadManager.LoadCommonAndMainMenuAssets();
    RunManager.Instance.CleanUp();
    await this.LoadMainMenu();
  }

  public void Relocalize()
  {
    if (this.RootSceneContainer.CurrentScene is NMainMenu)
      this.ReloadMainMenu();
    this.FeedbackScreen?.Relocalize();
    this.TimeoutOverlay.Relocalize();
  }

  public void ReloadMainMenu()
  {
    if (this.MainMenu == null)
      throw new InvalidOperationException("Tried to reload main menu when not already on the main menu!");
    TaskHelper.RunSafely(this.LoadMainMenu());
  }

  private async Task LoadMainMenu(bool openTimeline = false)
  {
    Task currentRunSaveTask = SaveManager.Instance.CurrentRunSaveTask;
    if (currentRunSaveTask != null)
    {
      Log.Info("Saving in progress, waiting for it to be finished before loading the main menu");
      try
      {
        await currentRunSaveTask;
      }
      catch (Exception ex)
      {
        Log.Error($"Save task failed while waiting to load main menu: {ex}");
      }
    }
    this.RootSceneContainer.SetCurrentScene((Control) NMainMenu.Create(openTimeline));
  }

  public async Task<RunState> StartNewSingleplayerRun(
    CharacterModel character,
    bool shouldSave,
    IReadOnlyList<ActModel> acts,
    IReadOnlyList<ModifierModel> modifiers,
    string seed,
    GameMode gameMode,
    int ascensionLevel = 0,
    DateTimeOffset? dailyTime = null)
  {
    // ISSUE: object of a compiler-generated type is created
    RunState runState = RunState.CreateForNewRun((IReadOnlyList<Player>) new \u003C\u003Ez__ReadOnlySingleElementList<Player>(Player.CreateForNewRun(character, SaveManager.Instance.GenerateUnlockStateFromProgress(), 1UL)), (IReadOnlyList<ActModel>) acts.Select<ActModel, ActModel>((Func<ActModel, ActModel>) (a => a.ToMutable())).ToList<ActModel>(), modifiers, gameMode, ascensionLevel, seed);
    RunManager.Instance.SetUpNewSingleplayer(runState, shouldSave, dailyTime);
    await this.StartRun(runState);
    RunState runState1 = runState;
    runState = (RunState) null;
    return runState1;
  }

  public async Task<RunState> StartNewMultiplayerRun(
    StartRunLobby lobby,
    bool shouldSave,
    IReadOnlyList<ActModel> acts,
    IReadOnlyList<ModifierModel> modifiers,
    string seed,
    int ascensionLevel,
    DateTimeOffset? dailyTime = null)
  {
    RunState runState = RunState.CreateForNewRun((IReadOnlyList<Player>) lobby.Players.Select<LobbyPlayer, Player>((Func<LobbyPlayer, Player>) (p => Player.CreateForNewRun(p.character, UnlockState.FromSerializable(p.unlockState), p.id))).ToList<Player>(), (IReadOnlyList<ActModel>) acts.Select<ActModel, ActModel>((Func<ActModel, ActModel>) (a => a.ToMutable())).ToList<ActModel>(), modifiers, lobby.GameMode, ascensionLevel, seed);
    RunManager.Instance.SetUpNewMultiplayer(runState, lobby, shouldSave, dailyTime);
    await this.StartRun(runState);
    RunState runState1 = runState;
    runState = (RunState) null;
    return runState1;
  }

  public async Task LoadRun(RunState runState, SerializableRoom? preFinishedRoom)
  {
    await PreloadManager.LoadRunAssets(runState.Players.Select<Player, CharacterModel>((Func<Player, CharacterModel>) (p => p.Character)));
    await PreloadManager.LoadActAssets(runState.Act);
    RunManager.Instance.Launch();
    this.RootSceneContainer.SetCurrentScene((Control) NRun.Create(runState));
    await RunManager.Instance.GenerateMap();
    await RunManager.Instance.LoadIntoLatestMapCoord(AbstractRoom.FromSerializable(preFinishedRoom, (IRunState) runState));
    if (RunManager.Instance.MapDrawingsToLoad == null)
      return;
    NRun.Instance.GlobalUi.MapScreen.Drawings.LoadDrawings(RunManager.Instance.MapDrawingsToLoad);
    RunManager.Instance.MapDrawingsToLoad = (SerializableMapDrawings) null;
  }

  private async Task StartRun(RunState runState)
  {
    using (new NetLoadingHandle(RunManager.Instance.NetService))
    {
      await PreloadManager.LoadRunAssets(runState.Players.Select<Player, CharacterModel>((Func<Player, CharacterModel>) (p => p.Character)));
      await PreloadManager.LoadActAssets(runState.Acts[0]);
      await RunManager.Instance.FinalizeStartingRelics();
      RunManager.Instance.Launch();
      this.RootSceneContainer.SetCurrentScene((Control) NRun.Create(runState));
      await RunManager.Instance.EnterAct(0, false);
    }
  }

  public override void _Input(InputEvent inputEvent)
  {
    if (inputEvent.IsActionReleased(DebugHotkey.speedUp, false))
      this.DebugModifyTimescale(0.1);
    else if (inputEvent.IsActionReleased(DebugHotkey.speedDown, false))
      this.DebugModifyTimescale(-0.1);
    else if (inputEvent.IsActionReleased(DebugHotkey.hideProceedButton, false))
    {
      NGame.IsDebugHidingProceedButton = !NGame.IsDebugHidingProceedButton;
      ((Node) NGame.Instance).AddChildSafely((Node) NFullscreenTextVfx.Create(NGame.IsDebugHidingProceedButton ? "Hide Proceed Button" : "Show Proceed Button"));
      Action toggleProceedButton = this.DebugToggleProceedButton;
      if (toggleProceedButton != null)
        toggleProceedButton();
    }
    else if (inputEvent.IsActionReleased(DebugHotkey.hideHoverTips, false))
    {
      NGame.IsDebugHidingHoverTips = !NGame.IsDebugHidingHoverTips;
      ((Node) NGame.Instance).AddChildSafely((Node) NFullscreenTextVfx.Create(NGame.IsDebugHidingHoverTips ? "Hide HoverTips" : "Show HoverTips"));
    }
    if (inputEvent is InputEventMouseButton eventMouseButton && eventMouseButton.Pressed || inputEvent.IsActionPressed(MegaInput.select, false, false) || inputEvent.IsActionPressed(MegaInput.cancel, false, false))
      this._logoCancelToken?.Cancel();
    if (!(inputEvent is InputEventKey inputEventKey))
      return;
    if (OS.GetName().Contains("Windows"))
    {
      if (!inputEventKey.Pressed || !((InputEventWithModifiers) inputEventKey).AltPressed || inputEventKey.Keycode != 4194309L /*0x400005*/)
        return;
      this.ToggleFullscreen();
    }
    else
    {
      if (!OS.GetName().Contains("macOS") || !inputEventKey.Pressed || !((InputEventWithModifiers) inputEventKey).CtrlPressed || !((InputEventWithModifiers) inputEventKey).MetaPressed || inputEventKey.Keycode != 70L)
        return;
      this.ToggleFullscreen();
    }
  }

  private void ToggleFullscreen()
  {
    Log.Info("Used FULLSCREEN shortcut");
    NFullscreenTickbox.SetFullscreen(!SaveManager.Instance.SettingsSave.Fullscreen);
  }

  private void DebugModifyTimescale(double offset)
  {
    Engine.TimeScale = Math.Clamp(Math.Round(Engine.TimeScale + offset, 1), 0.1, 4.0);
    ((Node) this).AddChildSafely((Node) NFullscreenTextVfx.Create($"TimeScale:{Engine.TimeScale}"));
  }

  public WorldEnvironment ActivateWorldEnvironment()
  {
    ((Node) this).AddChildSafely((Node) this.WorldEnvironment);
    return this.WorldEnvironment;
  }

  public void DeactivateWorldEnvironment()
  {
    ((Node) this).RemoveChildSafely((Node) this.WorldEnvironment);
  }

  public void SetScreenShakeTarget(Control target) => this._screenShake.SetTarget(target);

  public void ClearScreenShakeTarget() => this._screenShake.ClearTarget();

  public void ScreenShake(ShakeStrength strength, ShakeDuration duration, float degAngle = -1f)
  {
    if ((double) degAngle < 0.0)
      degAngle = Rng.Chaotic.NextFloat(360f);
    this._screenShake.Shake(strength, duration, degAngle);
  }

  public void ScreenRumble(ShakeStrength strength, ShakeDuration duration, RumbleStyle style)
  {
    this._screenShake.Rumble(strength, duration, style);
  }

  public void ScreenShakeTrauma(ShakeStrength strength) => this._screenShake.AddTrauma(strength);

  public void DoHitStop(ShakeStrength strength, ShakeDuration duration)
  {
    this.HitStop.DoHitStop(strength, duration);
  }

  public static void ToggleTrailerMode() => NGame.IsTrailerMode = !NGame.IsTrailerMode;

  public void SetScreenshakeMultiplier(float multiplier)
  {
    this._screenShake.SetMultiplier(multiplier);
  }

  private void InitPools()
  {
    NCard.InitPool();
    NGridCardHolder.InitPool();
  }

  private void OnNewModDetected(Mod mod)
  {
    if (((IEnumerable) ((Node) NModalContainer.Instance).GetChildren(false)).OfType<NErrorPopup>().Any<NErrorPopup>())
      return;
    NModalContainer.Instance.Add((Node) NErrorPopup.Create(new LocString("main_menu_ui", "MOD_NOT_LOADED_POPUP.title"), new LocString("main_menu_ui", "MOD_NOT_LOADED_POPUP.description"), (LocString) null, false));
  }

  public void CheckShowSaveFileError(
    ReadSaveResult<SerializableProgress> progressReadResult,
    ReadSaveResult<PrefsSave> prefsReadResult,
    ReadSaveResult<SettingsSave>? settingsReadResult)
  {
    LocString body = (LocString) null;
    if (!progressReadResult.Success && progressReadResult.Status != ReadSaveStatus.FileNotFound)
      body = new LocString("main_menu_ui", "INVALID_SAVE_POPUP.description_progress");
    else if (settingsReadResult != null && !settingsReadResult.Success && settingsReadResult.Status != ReadSaveStatus.FileNotFound)
      body = new LocString("main_menu_ui", "INVALID_SAVE_POPUP.description_settings");
    else if (!prefsReadResult.Success && prefsReadResult.Status != ReadSaveStatus.FileNotFound)
      body = new LocString("main_menu_ui", "INVALID_SAVE_POPUP.description_settings");
    if (body == null)
      return;
    NModalContainer.Instance.Add((Node) NErrorPopup.Create(new LocString("main_menu_ui", "INVALID_SAVE_POPUP.title"), body, new LocString("main_menu_ui", "INVALID_SAVE_POPUP.dismiss"), true));
  }

  private void CheckShowLocalizationOverrideErrors()
  {
    if (LocManager.Instance.ValidationErrors.Count == 0)
      return;
    List<IGrouping<string, LocValidationError>> list = LocManager.Instance.ValidationErrors.GroupBy<LocValidationError, string>((Func<LocValidationError, string>) (e => e.FilePath)).ToList<IGrouping<string, LocValidationError>>();
    string str = string.Join("\n", list.Take<IGrouping<string, LocValidationError>>(5).Select<IGrouping<string, LocValidationError>, string>((Func<IGrouping<string, LocValidationError>, string>) (g => $"{Path.GetFileName(g.Key)} ({g.Count<LocValidationError>()} errors)")));
    if (list.Count > 5)
      str += $"\n... and {list.Count - 5} more files";
    NModalContainer.Instance.Add((Node) NErrorPopup.Create("Localization Override Errors", $"Errors found in the following localization override files:\n\n{str}\n\n[gold]Check the console logs for detailed error messages.[/gold]\n\nTo fix: Remove or correct invalid override files in your localization_override folder.", false));
  }

  private void CheckShowModdedSaveFilePopup()
  {
    if (!ModManager.UnmoddedSavesWereCopied)
      return;
    NModalContainer.Instance.Add((Node) NErrorPopup.Create(new LocString("main_menu_ui", "MODDED_SAVE_COPY_POPUP.title"), new LocString("main_menu_ui", "MODDED_SAVE_COPY_POPUP.body"), (LocString) null, false));
  }

  private async Task<bool> InitializePlatform()
  {
    bool flag = CommandLineHelper.HasArg("force-steam");
    string str = CommandLineHelper.GetValue("force-steam") ?? "";
    if (!str.Equals("on", StringComparison.OrdinalIgnoreCase) && (!flag || !(str == string.Empty)) && (str.Equals("off", StringComparison.OrdinalIgnoreCase) || OS.HasFeature("editor")))
    {
      Log.Info("Steam initialization skipped (editor mode). Use --force-steam to enable.");
      return true;
    }
    bool steamInitialized = SteamInitializer.Initialize((Node) this);
    if (!steamInitialized)
    {
      Log.Error("Failed to initialize Steam! Attempting to show error popup");
      await this.TryErrorInit();
      NGenericPopup modalToCreate = NGenericPopup.Create();
      if (modalToCreate == null || NModalContainer.Instance == null)
      {
        Log.Error("Cannot show Steam error dialog: UI not initialized. Quitting immediately.");
        ((Node) this).GetTree().Quit(0);
        return false;
      }
      NModalContainer.Instance.Add((Node) modalToCreate);
      LocString body = new LocString("main_menu_ui", "STEAM_INIT_ERROR.description");
      body.Add("details", SteamInitializer.InitErrorMessage ?? "<null>");
      int num = await modalToCreate.WaitForConfirmation(body, new LocString("main_menu_ui", "STEAM_INIT_ERROR.title"), (LocString) null, new LocString("main_menu_ui", "QUIT")) ? 1 : 0;
      ((Node) this).GetTree().Quit(0);
    }
    else
    {
      this._joinCallbackHandler = new SteamJoinCallbackHandler();
      SteamInitializer.SteamNoLongerRunning += new Action(this.OnSteamNoLongerRunning);
    }
    return steamInitialized;
  }

  private void OnSteamNoLongerRunning()
  {
    NGenericPopup modalToCreate = NGenericPopup.Create();
    NModalContainer.Instance?.Add((Node) modalToCreate);
    LocString body = new LocString("main_menu_ui", "STEAM_STOPPED_ERROR.description");
    body.Add("details", $"{SteamInitializer.InitResult}: {SteamInitializer.InitErrorMessage}");
    TaskHelper.RunSafely((Task) modalToCreate.WaitForConfirmation(body, new LocString("main_menu_ui", "STEAM_STOPPED_ERROR.title"), (LocString) null, new LocString("main_menu_ui", "GENERIC_POPUP.ok")));
    SteamInitializer.SteamNoLongerRunning -= new Action(this.OnSteamNoLongerRunning);
  }

  public static void LogResourceStats(string context)
  {
    ulong staticMemoryUsage = OS.GetStaticMemoryUsage();
    ulong renderingInfo = RenderingServer.GetRenderingInfo((RenderingServer.RenderingInfo) 5L);
    int monitor1 = (int) Performance.GetMonitor((Performance.Monitor) 7L);
    int monitor2 = (int) Performance.GetMonitor((Performance.Monitor) 8L);
    int monitor3 = (int) Performance.GetMonitor((Performance.Monitor) 9L);
    int num = PreloadManager.Cache.GetCacheKeys().Count<string>();
    Log.Info($"[Startup] Resource stats ({context}): StaticMem={NGame.FormatBytes(staticMemoryUsage)}, VRAM={NGame.FormatBytes(renderingInfo)}, Objects={monitor1:N0}, Resources={monitor2:N0}, Nodes={monitor3:N0}, CachedAssets={num:N0}");
  }

  internal static string FormatBytes(ulong bytes)
  {
    string[] strArray = new string[4]
    {
      "B",
      "KB",
      "MB",
      "GB"
    };
    int index = 0;
    double num;
    for (num = (double) bytes; num >= 1024.0 && index < strArray.Length - 1; ++index)
      num /= 1024.0;
    return $"{num:0.#}{strArray[index]}";
  }

  public static bool IsGameFocusedWindow()
  {
    return !PlatformUtil.IsPlatformOverlayOpen() && DisplayServer.WindowIsFocused(0);
  }

  public NSendFeedbackScreen GetOrCreateFeedbackScreen()
  {
    if (this.FeedbackScreen == null)
    {
      this.FeedbackScreen = NSendFeedbackScreen.Create();
      ((Node) this).AddChildSafely((Node) this.FeedbackScreen);
      ((Node) this).MoveChildSafely((Node) this.FeedbackScreen, ((Node) this.Transition).GetIndex(false) - 1);
    }
    return this.FeedbackScreen;
  }

  public static string GetGameVersion()
  {
    return ReleaseInfoManager.Instance.ReleaseInfo?.Version ?? GitHelper.ShortCommitId ?? "UNKNOWN";
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(38)
    {
      new MethodInfo(NGame.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGame.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGame.MethodName.OnWindowChange, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGame.MethodName.IsMainThread, new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGame.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGame.MethodName.IsReleaseGame, new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGame.MethodName.InitializeGraphicsPreferences, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGame.MethodName.ApplyDisplaySettings, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGame.MethodName.GetInspectRelicScreen, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGame.MethodName.GetInspectCardScreen, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGame.MethodName.ApplySyncSetting, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGame.MethodName.Reset, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGame.MethodName._Notification, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("what"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NGame.MethodName.Quit, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGame.MethodName.Relocalize, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGame.MethodName.ReloadMainMenu, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGame.MethodName._Input, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NGame.MethodName.ToggleFullscreen, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGame.MethodName.DebugModifyTimescale, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("offset"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NGame.MethodName.ActivateWorldEnvironment, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("WorldEnvironment"), false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGame.MethodName.DeactivateWorldEnvironment, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGame.MethodName.SetScreenShakeTarget, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("target"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NGame.MethodName.ClearScreenShakeTarget, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGame.MethodName.ScreenShake, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("strength"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("duration"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("degAngle"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NGame.MethodName.ScreenRumble, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("strength"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("duration"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("style"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NGame.MethodName.ScreenShakeTrauma, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("strength"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NGame.MethodName.DoHitStop, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("strength"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("duration"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NGame.MethodName.ToggleTrailerMode, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGame.MethodName.SetScreenshakeMultiplier, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("multiplier"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NGame.MethodName.InitPools, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGame.MethodName.CheckShowLocalizationOverrideErrors, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGame.MethodName.CheckShowModdedSaveFilePopup, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGame.MethodName.OnSteamNoLongerRunning, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGame.MethodName.LogResourceStats, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("context"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NGame.MethodName.FormatBytes, new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("bytes"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NGame.MethodName.IsGameFocusedWindow, new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGame.MethodName.GetOrCreateFeedbackScreen, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGame.MethodName.GetGameVersion, new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NGame.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGame.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGame.MethodName.OnWindowChange) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnWindowChange();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGame.MethodName.IsMainThread) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      bool flag = NGame.IsMainThread();
      ret = VariantUtils.CreateFrom<bool>(ref flag);
      return true;
    }
    if (StringName.op_Equality(ref method, NGame.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGame.MethodName.IsReleaseGame) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      bool flag = NGame.IsReleaseGame();
      ret = VariantUtils.CreateFrom<bool>(ref flag);
      return true;
    }
    if (StringName.op_Equality(ref method, NGame.MethodName.InitializeGraphicsPreferences) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.InitializeGraphicsPreferences();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGame.MethodName.ApplyDisplaySettings) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ApplyDisplaySettings();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGame.MethodName.GetInspectRelicScreen) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NInspectRelicScreen inspectRelicScreen = this.GetInspectRelicScreen();
      ret = VariantUtils.CreateFrom<NInspectRelicScreen>(ref inspectRelicScreen);
      return true;
    }
    if (StringName.op_Equality(ref method, NGame.MethodName.GetInspectCardScreen) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NInspectCardScreen inspectCardScreen = this.GetInspectCardScreen();
      ret = VariantUtils.CreateFrom<NInspectCardScreen>(ref inspectCardScreen);
      return true;
    }
    if (StringName.op_Equality(ref method, NGame.MethodName.ApplySyncSetting) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NGame.ApplySyncSetting();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGame.MethodName.Reset) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NGame.Reset();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGame.MethodName._Notification) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((GodotObject) this)._Notification(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGame.MethodName.Quit) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Quit();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGame.MethodName.Relocalize) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Relocalize();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGame.MethodName.ReloadMainMenu) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ReloadMainMenu();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGame.MethodName._Input) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Input(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGame.MethodName.ToggleFullscreen) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ToggleFullscreen();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGame.MethodName.DebugModifyTimescale) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.DebugModifyTimescale(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGame.MethodName.ActivateWorldEnvironment) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      WorldEnvironment worldEnvironment = this.ActivateWorldEnvironment();
      ret = VariantUtils.CreateFrom<WorldEnvironment>(ref worldEnvironment);
      return true;
    }
    if (StringName.op_Equality(ref method, NGame.MethodName.DeactivateWorldEnvironment) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.DeactivateWorldEnvironment();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGame.MethodName.SetScreenShakeTarget) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetScreenShakeTarget(VariantUtils.ConvertTo<Control>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGame.MethodName.ClearScreenShakeTarget) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ClearScreenShakeTarget();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGame.MethodName.ScreenShake) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      this.ScreenShake(VariantUtils.ConvertTo<ShakeStrength>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<ShakeDuration>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGame.MethodName.ScreenRumble) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      this.ScreenRumble(VariantUtils.ConvertTo<ShakeStrength>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<ShakeDuration>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<RumbleStyle>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGame.MethodName.ScreenShakeTrauma) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ScreenShakeTrauma(VariantUtils.ConvertTo<ShakeStrength>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGame.MethodName.DoHitStop) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.DoHitStop(VariantUtils.ConvertTo<ShakeStrength>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<ShakeDuration>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGame.MethodName.ToggleTrailerMode) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NGame.ToggleTrailerMode();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGame.MethodName.SetScreenshakeMultiplier) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetScreenshakeMultiplier(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGame.MethodName.InitPools) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.InitPools();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGame.MethodName.CheckShowLocalizationOverrideErrors) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.CheckShowLocalizationOverrideErrors();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGame.MethodName.CheckShowModdedSaveFilePopup) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.CheckShowModdedSaveFilePopup();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGame.MethodName.OnSteamNoLongerRunning) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSteamNoLongerRunning();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGame.MethodName.LogResourceStats) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NGame.LogResourceStats(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGame.MethodName.FormatBytes) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      string str = NGame.FormatBytes(VariantUtils.ConvertTo<ulong>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<string>(ref str);
      return true;
    }
    if (StringName.op_Equality(ref method, NGame.MethodName.IsGameFocusedWindow) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      bool flag = NGame.IsGameFocusedWindow();
      ret = VariantUtils.CreateFrom<bool>(ref flag);
      return true;
    }
    if (StringName.op_Equality(ref method, NGame.MethodName.GetOrCreateFeedbackScreen) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NSendFeedbackScreen feedbackScreen = this.GetOrCreateFeedbackScreen();
      ret = VariantUtils.CreateFrom<NSendFeedbackScreen>(ref feedbackScreen);
      return true;
    }
    if (!StringName.op_Equality(ref method, NGame.MethodName.GetGameVersion) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    string gameVersion = NGame.GetGameVersion();
    ret = VariantUtils.CreateFrom<string>(ref gameVersion);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NGame.MethodName.IsMainThread) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      bool flag = NGame.IsMainThread();
      ret = VariantUtils.CreateFrom<bool>(ref flag);
      return true;
    }
    if (StringName.op_Equality(ref method, NGame.MethodName.IsReleaseGame) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      bool flag = NGame.IsReleaseGame();
      ret = VariantUtils.CreateFrom<bool>(ref flag);
      return true;
    }
    if (StringName.op_Equality(ref method, NGame.MethodName.ApplySyncSetting) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NGame.ApplySyncSetting();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGame.MethodName.Reset) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NGame.Reset();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGame.MethodName.ToggleTrailerMode) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NGame.ToggleTrailerMode();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGame.MethodName.LogResourceStats) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NGame.LogResourceStats(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGame.MethodName.FormatBytes) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      string str = NGame.FormatBytes(VariantUtils.ConvertTo<ulong>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<string>(ref str);
      return true;
    }
    if (StringName.op_Equality(ref method, NGame.MethodName.IsGameFocusedWindow) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      bool flag = NGame.IsGameFocusedWindow();
      ret = VariantUtils.CreateFrom<bool>(ref flag);
      return true;
    }
    if (StringName.op_Equality(ref method, NGame.MethodName.GetGameVersion) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      string gameVersion = NGame.GetGameVersion();
      ret = VariantUtils.CreateFrom<string>(ref gameVersion);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NGame.MethodName._EnterTree) || StringName.op_Equality(ref method, NGame.MethodName._Ready) || StringName.op_Equality(ref method, NGame.MethodName.OnWindowChange) || StringName.op_Equality(ref method, NGame.MethodName.IsMainThread) || StringName.op_Equality(ref method, NGame.MethodName._ExitTree) || StringName.op_Equality(ref method, NGame.MethodName.IsReleaseGame) || StringName.op_Equality(ref method, NGame.MethodName.InitializeGraphicsPreferences) || StringName.op_Equality(ref method, NGame.MethodName.ApplyDisplaySettings) || StringName.op_Equality(ref method, NGame.MethodName.GetInspectRelicScreen) || StringName.op_Equality(ref method, NGame.MethodName.GetInspectCardScreen) || StringName.op_Equality(ref method, NGame.MethodName.ApplySyncSetting) || StringName.op_Equality(ref method, NGame.MethodName.Reset) || StringName.op_Equality(ref method, NGame.MethodName._Notification) || StringName.op_Equality(ref method, NGame.MethodName.Quit) || StringName.op_Equality(ref method, NGame.MethodName.Relocalize) || StringName.op_Equality(ref method, NGame.MethodName.ReloadMainMenu) || StringName.op_Equality(ref method, NGame.MethodName._Input) || StringName.op_Equality(ref method, NGame.MethodName.ToggleFullscreen) || StringName.op_Equality(ref method, NGame.MethodName.DebugModifyTimescale) || StringName.op_Equality(ref method, NGame.MethodName.ActivateWorldEnvironment) || StringName.op_Equality(ref method, NGame.MethodName.DeactivateWorldEnvironment) || StringName.op_Equality(ref method, NGame.MethodName.SetScreenShakeTarget) || StringName.op_Equality(ref method, NGame.MethodName.ClearScreenShakeTarget) || StringName.op_Equality(ref method, NGame.MethodName.ScreenShake) || StringName.op_Equality(ref method, NGame.MethodName.ScreenRumble) || StringName.op_Equality(ref method, NGame.MethodName.ScreenShakeTrauma) || StringName.op_Equality(ref method, NGame.MethodName.DoHitStop) || StringName.op_Equality(ref method, NGame.MethodName.ToggleTrailerMode) || StringName.op_Equality(ref method, NGame.MethodName.SetScreenshakeMultiplier) || StringName.op_Equality(ref method, NGame.MethodName.InitPools) || StringName.op_Equality(ref method, NGame.MethodName.CheckShowLocalizationOverrideErrors) || StringName.op_Equality(ref method, NGame.MethodName.CheckShowModdedSaveFilePopup) || StringName.op_Equality(ref method, NGame.MethodName.OnSteamNoLongerRunning) || StringName.op_Equality(ref method, NGame.MethodName.LogResourceStats) || StringName.op_Equality(ref method, NGame.MethodName.FormatBytes) || StringName.op_Equality(ref method, NGame.MethodName.IsGameFocusedWindow) || StringName.op_Equality(ref method, NGame.MethodName.GetOrCreateFeedbackScreen) || StringName.op_Equality(ref method, NGame.MethodName.GetGameVersion) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NGame.PropertyName.RootSceneContainer))
    {
      this.RootSceneContainer = VariantUtils.ConvertTo<NSceneContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGame.PropertyName.HoverTipsContainer))
    {
      this.HoverTipsContainer = VariantUtils.ConvertTo<Node>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGame.PropertyName.Transition))
    {
      this.Transition = VariantUtils.ConvertTo<NTransition>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGame.PropertyName.TimeoutOverlay))
    {
      this.TimeoutOverlay = VariantUtils.ConvertTo<NMultiplayerTimeoutOverlay>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGame.PropertyName.AudioManager))
    {
      this.AudioManager = VariantUtils.ConvertTo<NAudioManager>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGame.PropertyName.RemoteCursorContainer))
    {
      this.RemoteCursorContainer = VariantUtils.ConvertTo<NRemoteMouseCursorContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGame.PropertyName.InputManager))
    {
      this.InputManager = VariantUtils.ConvertTo<NInputManager>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGame.PropertyName.HotkeyManager))
    {
      this.HotkeyManager = VariantUtils.ConvertTo<NHotkeyManager>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGame.PropertyName.FeedbackScreen))
    {
      this.FeedbackScreen = VariantUtils.ConvertTo<NSendFeedbackScreen>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGame.PropertyName.ReactionWheel))
    {
      this.ReactionWheel = VariantUtils.ConvertTo<NReactionWheel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGame.PropertyName.ReactionContainer))
    {
      this.ReactionContainer = VariantUtils.ConvertTo<NReactionContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGame.PropertyName.CursorManager))
    {
      this.CursorManager = VariantUtils.ConvertTo<NCursorManager>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGame.PropertyName.DebugAudio))
    {
      this.DebugAudio = VariantUtils.ConvertTo<NDebugAudioManager>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGame.PropertyName.DebugSeedOverride))
    {
      this.DebugSeedOverride = VariantUtils.ConvertTo<string>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGame.PropertyName.StartOnMainMenu))
    {
      this.StartOnMainMenu = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGame.PropertyName.InspectRelicScreen))
    {
      this.InspectRelicScreen = VariantUtils.ConvertTo<NInspectRelicScreen>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGame.PropertyName.InspectCardScreen))
    {
      this.InspectCardScreen = VariantUtils.ConvertTo<NInspectCardScreen>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGame.PropertyName.WorldEnvironment))
    {
      this.WorldEnvironment = VariantUtils.ConvertTo<WorldEnvironment>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGame.PropertyName.HitStop))
    {
      this.HitStop = VariantUtils.ConvertTo<NHitStop>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGame.PropertyName._inspectionContainer))
    {
      this._inspectionContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NGame.PropertyName._screenShake))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._screenShake = VariantUtils.ConvertTo<NScreenShake>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NGame.PropertyName.RootSceneContainer))
    {
      ref godot_variant local = ref value;
      NSceneContainer rootSceneContainer = this.RootSceneContainer;
      godot_variant from = VariantUtils.CreateFrom<NSceneContainer>(ref rootSceneContainer);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NGame.PropertyName.HoverTipsContainer))
    {
      ref godot_variant local = ref value;
      Node hoverTipsContainer = this.HoverTipsContainer;
      godot_variant from = VariantUtils.CreateFrom<Node>(ref hoverTipsContainer);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NGame.PropertyName.MainMenu))
    {
      ref godot_variant local = ref value;
      NMainMenu mainMenu = this.MainMenu;
      godot_variant from = VariantUtils.CreateFrom<NMainMenu>(ref mainMenu);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NGame.PropertyName.CurrentRunNode))
    {
      ref godot_variant local = ref value;
      NRun currentRunNode = this.CurrentRunNode;
      godot_variant from = VariantUtils.CreateFrom<NRun>(ref currentRunNode);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NGame.PropertyName.LogoAnimation))
    {
      ref godot_variant local = ref value;
      NLogoAnimation logoAnimation = this.LogoAnimation;
      godot_variant from = VariantUtils.CreateFrom<NLogoAnimation>(ref logoAnimation);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NGame.PropertyName.Transition))
    {
      ref godot_variant local = ref value;
      NTransition transition = this.Transition;
      godot_variant from = VariantUtils.CreateFrom<NTransition>(ref transition);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NGame.PropertyName.TimeoutOverlay))
    {
      ref godot_variant local = ref value;
      NMultiplayerTimeoutOverlay timeoutOverlay = this.TimeoutOverlay;
      godot_variant from = VariantUtils.CreateFrom<NMultiplayerTimeoutOverlay>(ref timeoutOverlay);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NGame.PropertyName.AudioManager))
    {
      ref godot_variant local = ref value;
      NAudioManager audioManager = this.AudioManager;
      godot_variant from = VariantUtils.CreateFrom<NAudioManager>(ref audioManager);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NGame.PropertyName.RemoteCursorContainer))
    {
      ref godot_variant local = ref value;
      NRemoteMouseCursorContainer remoteCursorContainer = this.RemoteCursorContainer;
      godot_variant from = VariantUtils.CreateFrom<NRemoteMouseCursorContainer>(ref remoteCursorContainer);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NGame.PropertyName.InputManager))
    {
      ref godot_variant local = ref value;
      NInputManager inputManager = this.InputManager;
      godot_variant from = VariantUtils.CreateFrom<NInputManager>(ref inputManager);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NGame.PropertyName.HotkeyManager))
    {
      ref godot_variant local = ref value;
      NHotkeyManager hotkeyManager = this.HotkeyManager;
      godot_variant from = VariantUtils.CreateFrom<NHotkeyManager>(ref hotkeyManager);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NGame.PropertyName.FeedbackScreen))
    {
      ref godot_variant local = ref value;
      NSendFeedbackScreen feedbackScreen = this.FeedbackScreen;
      godot_variant from = VariantUtils.CreateFrom<NSendFeedbackScreen>(ref feedbackScreen);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NGame.PropertyName.ReactionWheel))
    {
      ref godot_variant local = ref value;
      NReactionWheel reactionWheel = this.ReactionWheel;
      godot_variant from = VariantUtils.CreateFrom<NReactionWheel>(ref reactionWheel);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NGame.PropertyName.ReactionContainer))
    {
      ref godot_variant local = ref value;
      NReactionContainer reactionContainer = this.ReactionContainer;
      godot_variant from = VariantUtils.CreateFrom<NReactionContainer>(ref reactionContainer);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NGame.PropertyName.CursorManager))
    {
      ref godot_variant local = ref value;
      NCursorManager cursorManager = this.CursorManager;
      godot_variant from = VariantUtils.CreateFrom<NCursorManager>(ref cursorManager);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NGame.PropertyName.DebugAudio))
    {
      ref godot_variant local = ref value;
      NDebugAudioManager debugAudio = this.DebugAudio;
      godot_variant from = VariantUtils.CreateFrom<NDebugAudioManager>(ref debugAudio);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NGame.PropertyName.DebugSeedOverride))
    {
      ref godot_variant local = ref value;
      string debugSeedOverride = this.DebugSeedOverride;
      godot_variant from = VariantUtils.CreateFrom<string>(ref debugSeedOverride);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NGame.PropertyName.StartOnMainMenu))
    {
      ref godot_variant local = ref value;
      bool startOnMainMenu = this.StartOnMainMenu;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref startOnMainMenu);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NGame.PropertyName.InspectRelicScreen))
    {
      ref godot_variant local = ref value;
      NInspectRelicScreen inspectRelicScreen = this.InspectRelicScreen;
      godot_variant from = VariantUtils.CreateFrom<NInspectRelicScreen>(ref inspectRelicScreen);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NGame.PropertyName.InspectCardScreen))
    {
      ref godot_variant local = ref value;
      NInspectCardScreen inspectCardScreen = this.InspectCardScreen;
      godot_variant from = VariantUtils.CreateFrom<NInspectCardScreen>(ref inspectCardScreen);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NGame.PropertyName.ScreenshakeTarget))
    {
      ref godot_variant local = ref value;
      Control screenshakeTarget = this.ScreenshakeTarget;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref screenshakeTarget);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NGame.PropertyName.WorldEnvironment))
    {
      ref godot_variant local = ref value;
      WorldEnvironment worldEnvironment = this.WorldEnvironment;
      godot_variant from = VariantUtils.CreateFrom<WorldEnvironment>(ref worldEnvironment);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NGame.PropertyName.HitStop))
    {
      ref godot_variant local = ref value;
      NHitStop hitStop = this.HitStop;
      godot_variant from = VariantUtils.CreateFrom<NHitStop>(ref hitStop);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NGame.PropertyName._inspectionContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._inspectionContainer);
      return true;
    }
    if (!StringName.op_Equality(ref name, NGame.PropertyName._screenShake))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<NScreenShake>(ref this._screenShake);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NGame.PropertyName.RootSceneContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGame.PropertyName.HoverTipsContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGame.PropertyName.MainMenu, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGame.PropertyName.CurrentRunNode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGame.PropertyName.LogoAnimation, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGame.PropertyName.Transition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGame.PropertyName.TimeoutOverlay, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGame.PropertyName.AudioManager, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGame.PropertyName.RemoteCursorContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGame.PropertyName.InputManager, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGame.PropertyName.HotkeyManager, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGame.PropertyName.FeedbackScreen, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGame.PropertyName.ReactionWheel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGame.PropertyName.ReactionContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGame.PropertyName.CursorManager, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGame.PropertyName.DebugAudio, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 4L, NGame.PropertyName.DebugSeedOverride, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NGame.PropertyName.StartOnMainMenu, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGame.PropertyName.InspectRelicScreen, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGame.PropertyName.InspectCardScreen, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGame.PropertyName.ScreenshakeTarget, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGame.PropertyName.WorldEnvironment, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGame.PropertyName.HitStop, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGame.PropertyName._inspectionContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGame.PropertyName._screenShake, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo1 = info;
    StringName rootSceneContainer1 = NGame.PropertyName.RootSceneContainer;
    NSceneContainer rootSceneContainer2 = this.RootSceneContainer;
    Variant variant1 = Variant.From<NSceneContainer>(ref rootSceneContainer2);
    serializationInfo1.AddProperty(rootSceneContainer1, variant1);
    GodotSerializationInfo serializationInfo2 = info;
    StringName hoverTipsContainer1 = NGame.PropertyName.HoverTipsContainer;
    Node hoverTipsContainer2 = this.HoverTipsContainer;
    Variant variant2 = Variant.From<Node>(ref hoverTipsContainer2);
    serializationInfo2.AddProperty(hoverTipsContainer1, variant2);
    GodotSerializationInfo serializationInfo3 = info;
    StringName transition1 = NGame.PropertyName.Transition;
    NTransition transition2 = this.Transition;
    Variant variant3 = Variant.From<NTransition>(ref transition2);
    serializationInfo3.AddProperty(transition1, variant3);
    GodotSerializationInfo serializationInfo4 = info;
    StringName timeoutOverlay1 = NGame.PropertyName.TimeoutOverlay;
    NMultiplayerTimeoutOverlay timeoutOverlay2 = this.TimeoutOverlay;
    Variant variant4 = Variant.From<NMultiplayerTimeoutOverlay>(ref timeoutOverlay2);
    serializationInfo4.AddProperty(timeoutOverlay1, variant4);
    GodotSerializationInfo serializationInfo5 = info;
    StringName audioManager1 = NGame.PropertyName.AudioManager;
    NAudioManager audioManager2 = this.AudioManager;
    Variant variant5 = Variant.From<NAudioManager>(ref audioManager2);
    serializationInfo5.AddProperty(audioManager1, variant5);
    GodotSerializationInfo serializationInfo6 = info;
    StringName remoteCursorContainer1 = NGame.PropertyName.RemoteCursorContainer;
    NRemoteMouseCursorContainer remoteCursorContainer2 = this.RemoteCursorContainer;
    Variant variant6 = Variant.From<NRemoteMouseCursorContainer>(ref remoteCursorContainer2);
    serializationInfo6.AddProperty(remoteCursorContainer1, variant6);
    GodotSerializationInfo serializationInfo7 = info;
    StringName inputManager1 = NGame.PropertyName.InputManager;
    NInputManager inputManager2 = this.InputManager;
    Variant variant7 = Variant.From<NInputManager>(ref inputManager2);
    serializationInfo7.AddProperty(inputManager1, variant7);
    GodotSerializationInfo serializationInfo8 = info;
    StringName hotkeyManager1 = NGame.PropertyName.HotkeyManager;
    NHotkeyManager hotkeyManager2 = this.HotkeyManager;
    Variant variant8 = Variant.From<NHotkeyManager>(ref hotkeyManager2);
    serializationInfo8.AddProperty(hotkeyManager1, variant8);
    GodotSerializationInfo serializationInfo9 = info;
    StringName feedbackScreen1 = NGame.PropertyName.FeedbackScreen;
    NSendFeedbackScreen feedbackScreen2 = this.FeedbackScreen;
    Variant variant9 = Variant.From<NSendFeedbackScreen>(ref feedbackScreen2);
    serializationInfo9.AddProperty(feedbackScreen1, variant9);
    GodotSerializationInfo serializationInfo10 = info;
    StringName reactionWheel1 = NGame.PropertyName.ReactionWheel;
    NReactionWheel reactionWheel2 = this.ReactionWheel;
    Variant variant10 = Variant.From<NReactionWheel>(ref reactionWheel2);
    serializationInfo10.AddProperty(reactionWheel1, variant10);
    GodotSerializationInfo serializationInfo11 = info;
    StringName reactionContainer1 = NGame.PropertyName.ReactionContainer;
    NReactionContainer reactionContainer2 = this.ReactionContainer;
    Variant variant11 = Variant.From<NReactionContainer>(ref reactionContainer2);
    serializationInfo11.AddProperty(reactionContainer1, variant11);
    GodotSerializationInfo serializationInfo12 = info;
    StringName cursorManager1 = NGame.PropertyName.CursorManager;
    NCursorManager cursorManager2 = this.CursorManager;
    Variant variant12 = Variant.From<NCursorManager>(ref cursorManager2);
    serializationInfo12.AddProperty(cursorManager1, variant12);
    GodotSerializationInfo serializationInfo13 = info;
    StringName debugAudio1 = NGame.PropertyName.DebugAudio;
    NDebugAudioManager debugAudio2 = this.DebugAudio;
    Variant variant13 = Variant.From<NDebugAudioManager>(ref debugAudio2);
    serializationInfo13.AddProperty(debugAudio1, variant13);
    GodotSerializationInfo serializationInfo14 = info;
    StringName debugSeedOverride1 = NGame.PropertyName.DebugSeedOverride;
    string debugSeedOverride2 = this.DebugSeedOverride;
    Variant variant14 = Variant.From<string>(ref debugSeedOverride2);
    serializationInfo14.AddProperty(debugSeedOverride1, variant14);
    GodotSerializationInfo serializationInfo15 = info;
    StringName startOnMainMenu1 = NGame.PropertyName.StartOnMainMenu;
    bool startOnMainMenu2 = this.StartOnMainMenu;
    Variant variant15 = Variant.From<bool>(ref startOnMainMenu2);
    serializationInfo15.AddProperty(startOnMainMenu1, variant15);
    GodotSerializationInfo serializationInfo16 = info;
    StringName inspectRelicScreen1 = NGame.PropertyName.InspectRelicScreen;
    NInspectRelicScreen inspectRelicScreen2 = this.InspectRelicScreen;
    Variant variant16 = Variant.From<NInspectRelicScreen>(ref inspectRelicScreen2);
    serializationInfo16.AddProperty(inspectRelicScreen1, variant16);
    GodotSerializationInfo serializationInfo17 = info;
    StringName inspectCardScreen1 = NGame.PropertyName.InspectCardScreen;
    NInspectCardScreen inspectCardScreen2 = this.InspectCardScreen;
    Variant variant17 = Variant.From<NInspectCardScreen>(ref inspectCardScreen2);
    serializationInfo17.AddProperty(inspectCardScreen1, variant17);
    GodotSerializationInfo serializationInfo18 = info;
    StringName worldEnvironment1 = NGame.PropertyName.WorldEnvironment;
    WorldEnvironment worldEnvironment2 = this.WorldEnvironment;
    Variant variant18 = Variant.From<WorldEnvironment>(ref worldEnvironment2);
    serializationInfo18.AddProperty(worldEnvironment1, variant18);
    GodotSerializationInfo serializationInfo19 = info;
    StringName hitStop1 = NGame.PropertyName.HitStop;
    NHitStop hitStop2 = this.HitStop;
    Variant variant19 = Variant.From<NHitStop>(ref hitStop2);
    serializationInfo19.AddProperty(hitStop1, variant19);
    info.AddProperty(NGame.PropertyName._inspectionContainer, Variant.From<Control>(ref this._inspectionContainer));
    info.AddProperty(NGame.PropertyName._screenShake, Variant.From<NScreenShake>(ref this._screenShake));
    info.AddSignalEventDelegate(NGame.SignalName.WindowChange, (Delegate) this.backing_WindowChange);
    info.AddSignalEventDelegate(NGame.SignalName.PhobiaModeToggled, (Delegate) this.backing_PhobiaModeToggled);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NGame.PropertyName.RootSceneContainer, ref variant1))
      this.RootSceneContainer = ((Variant) ref variant1).As<NSceneContainer>();
    Variant variant2;
    if (info.TryGetProperty(NGame.PropertyName.HoverTipsContainer, ref variant2))
      this.HoverTipsContainer = ((Variant) ref variant2).As<Node>();
    Variant variant3;
    if (info.TryGetProperty(NGame.PropertyName.Transition, ref variant3))
      this.Transition = ((Variant) ref variant3).As<NTransition>();
    Variant variant4;
    if (info.TryGetProperty(NGame.PropertyName.TimeoutOverlay, ref variant4))
      this.TimeoutOverlay = ((Variant) ref variant4).As<NMultiplayerTimeoutOverlay>();
    Variant variant5;
    if (info.TryGetProperty(NGame.PropertyName.AudioManager, ref variant5))
      this.AudioManager = ((Variant) ref variant5).As<NAudioManager>();
    Variant variant6;
    if (info.TryGetProperty(NGame.PropertyName.RemoteCursorContainer, ref variant6))
      this.RemoteCursorContainer = ((Variant) ref variant6).As<NRemoteMouseCursorContainer>();
    Variant variant7;
    if (info.TryGetProperty(NGame.PropertyName.InputManager, ref variant7))
      this.InputManager = ((Variant) ref variant7).As<NInputManager>();
    Variant variant8;
    if (info.TryGetProperty(NGame.PropertyName.HotkeyManager, ref variant8))
      this.HotkeyManager = ((Variant) ref variant8).As<NHotkeyManager>();
    Variant variant9;
    if (info.TryGetProperty(NGame.PropertyName.FeedbackScreen, ref variant9))
      this.FeedbackScreen = ((Variant) ref variant9).As<NSendFeedbackScreen>();
    Variant variant10;
    if (info.TryGetProperty(NGame.PropertyName.ReactionWheel, ref variant10))
      this.ReactionWheel = ((Variant) ref variant10).As<NReactionWheel>();
    Variant variant11;
    if (info.TryGetProperty(NGame.PropertyName.ReactionContainer, ref variant11))
      this.ReactionContainer = ((Variant) ref variant11).As<NReactionContainer>();
    Variant variant12;
    if (info.TryGetProperty(NGame.PropertyName.CursorManager, ref variant12))
      this.CursorManager = ((Variant) ref variant12).As<NCursorManager>();
    Variant variant13;
    if (info.TryGetProperty(NGame.PropertyName.DebugAudio, ref variant13))
      this.DebugAudio = ((Variant) ref variant13).As<NDebugAudioManager>();
    Variant variant14;
    if (info.TryGetProperty(NGame.PropertyName.DebugSeedOverride, ref variant14))
      this.DebugSeedOverride = ((Variant) ref variant14).As<string>();
    Variant variant15;
    if (info.TryGetProperty(NGame.PropertyName.StartOnMainMenu, ref variant15))
      this.StartOnMainMenu = ((Variant) ref variant15).As<bool>();
    Variant variant16;
    if (info.TryGetProperty(NGame.PropertyName.InspectRelicScreen, ref variant16))
      this.InspectRelicScreen = ((Variant) ref variant16).As<NInspectRelicScreen>();
    Variant variant17;
    if (info.TryGetProperty(NGame.PropertyName.InspectCardScreen, ref variant17))
      this.InspectCardScreen = ((Variant) ref variant17).As<NInspectCardScreen>();
    Variant variant18;
    if (info.TryGetProperty(NGame.PropertyName.WorldEnvironment, ref variant18))
      this.WorldEnvironment = ((Variant) ref variant18).As<WorldEnvironment>();
    Variant variant19;
    if (info.TryGetProperty(NGame.PropertyName.HitStop, ref variant19))
      this.HitStop = ((Variant) ref variant19).As<NHitStop>();
    Variant variant20;
    if (info.TryGetProperty(NGame.PropertyName._inspectionContainer, ref variant20))
      this._inspectionContainer = ((Variant) ref variant20).As<Control>();
    Variant variant21;
    if (info.TryGetProperty(NGame.PropertyName._screenShake, ref variant21))
      this._screenShake = ((Variant) ref variant21).As<NScreenShake>();
    NGame.WindowChangeEventHandler changeEventHandler;
    if (info.TryGetSignalEventDelegate<NGame.WindowChangeEventHandler>(NGame.SignalName.WindowChange, ref changeEventHandler))
      this.backing_WindowChange = changeEventHandler;
    NGame.PhobiaModeToggledEventHandler toggledEventHandler;
    if (!info.TryGetSignalEventDelegate<NGame.PhobiaModeToggledEventHandler>(NGame.SignalName.PhobiaModeToggled, ref toggledEventHandler))
      return;
    this.backing_PhobiaModeToggled = toggledEventHandler;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<MethodInfo> GetGodotSignalList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NGame.SignalName.WindowChange, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGame.SignalName.PhobiaModeToggled, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  public event NGame.WindowChangeEventHandler WindowChange
  {
    add => this.backing_WindowChange += value;
    remove => this.backing_WindowChange -= value;
  }

  protected void EmitSignalWindowChange()
  {
    ((GodotObject) this).EmitSignal(NGame.SignalName.WindowChange, Array.Empty<Variant>());
  }

  public event NGame.PhobiaModeToggledEventHandler PhobiaModeToggled
  {
    add => this.backing_PhobiaModeToggled += value;
    remove => this.backing_PhobiaModeToggled -= value;
  }

  protected void EmitSignalPhobiaModeToggled()
  {
    ((GodotObject) this).EmitSignal(NGame.SignalName.PhobiaModeToggled, Array.Empty<Variant>());
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RaiseGodotClassSignalCallbacks(
    in godot_string_name signal,
    NativeVariantPtrArgs args)
  {
    if (StringName.op_Equality(ref signal, NGame.SignalName.WindowChange) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NGame.WindowChangeEventHandler backingWindowChange = this.backing_WindowChange;
      if (backingWindowChange == null)
        return;
      backingWindowChange();
    }
    else if (StringName.op_Equality(ref signal, NGame.SignalName.PhobiaModeToggled) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NGame.PhobiaModeToggledEventHandler phobiaModeToggled = this.backing_PhobiaModeToggled;
      if (phobiaModeToggled == null)
        return;
      phobiaModeToggled();
    }
    else
      ((GodotObject) this).RaiseGodotClassSignalCallbacks(ref signal, args);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassSignal(in godot_string_name signal)
  {
    return StringName.op_Equality(ref signal, NGame.SignalName.WindowChange) || StringName.op_Equality(ref signal, NGame.SignalName.PhobiaModeToggled) || base.HasGodotClassSignal(ref signal);
  }

  [Signal]
  public delegate void WindowChangeEventHandler();

  [Signal]
  public delegate void PhobiaModeToggledEventHandler();

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnWindowChange = StringName.op_Implicit(nameof (OnWindowChange));
    public static readonly StringName IsMainThread = StringName.op_Implicit(nameof (IsMainThread));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName IsReleaseGame = StringName.op_Implicit(nameof (IsReleaseGame));
    public static readonly StringName InitializeGraphicsPreferences = StringName.op_Implicit(nameof (InitializeGraphicsPreferences));
    public static readonly StringName ApplyDisplaySettings = StringName.op_Implicit(nameof (ApplyDisplaySettings));
    public static readonly StringName GetInspectRelicScreen = StringName.op_Implicit(nameof (GetInspectRelicScreen));
    public static readonly StringName GetInspectCardScreen = StringName.op_Implicit(nameof (GetInspectCardScreen));
    public static readonly StringName ApplySyncSetting = StringName.op_Implicit(nameof (ApplySyncSetting));
    public static readonly StringName Reset = StringName.op_Implicit(nameof (Reset));
    public static readonly StringName _Notification = StringName.op_Implicit(nameof (_Notification));
    public static readonly StringName Quit = StringName.op_Implicit(nameof (Quit));
    public static readonly StringName Relocalize = StringName.op_Implicit(nameof (Relocalize));
    public static readonly StringName ReloadMainMenu = StringName.op_Implicit(nameof (ReloadMainMenu));
    public static readonly StringName _Input = StringName.op_Implicit(nameof (_Input));
    public static readonly StringName ToggleFullscreen = StringName.op_Implicit(nameof (ToggleFullscreen));
    public static readonly StringName DebugModifyTimescale = StringName.op_Implicit(nameof (DebugModifyTimescale));
    public static readonly StringName ActivateWorldEnvironment = StringName.op_Implicit(nameof (ActivateWorldEnvironment));
    public static readonly StringName DeactivateWorldEnvironment = StringName.op_Implicit(nameof (DeactivateWorldEnvironment));
    public static readonly StringName SetScreenShakeTarget = StringName.op_Implicit(nameof (SetScreenShakeTarget));
    public static readonly StringName ClearScreenShakeTarget = StringName.op_Implicit(nameof (ClearScreenShakeTarget));
    public static readonly StringName ScreenShake = StringName.op_Implicit(nameof (ScreenShake));
    public static readonly StringName ScreenRumble = StringName.op_Implicit(nameof (ScreenRumble));
    public static readonly StringName ScreenShakeTrauma = StringName.op_Implicit(nameof (ScreenShakeTrauma));
    public static readonly StringName DoHitStop = StringName.op_Implicit(nameof (DoHitStop));
    public static readonly StringName ToggleTrailerMode = StringName.op_Implicit(nameof (ToggleTrailerMode));
    public static readonly StringName SetScreenshakeMultiplier = StringName.op_Implicit(nameof (SetScreenshakeMultiplier));
    public static readonly StringName InitPools = StringName.op_Implicit(nameof (InitPools));
    public static readonly StringName CheckShowLocalizationOverrideErrors = StringName.op_Implicit(nameof (CheckShowLocalizationOverrideErrors));
    public static readonly StringName CheckShowModdedSaveFilePopup = StringName.op_Implicit(nameof (CheckShowModdedSaveFilePopup));
    public static readonly StringName OnSteamNoLongerRunning = StringName.op_Implicit(nameof (OnSteamNoLongerRunning));
    public static readonly StringName LogResourceStats = StringName.op_Implicit(nameof (LogResourceStats));
    public static readonly StringName FormatBytes = StringName.op_Implicit(nameof (FormatBytes));
    public static readonly StringName IsGameFocusedWindow = StringName.op_Implicit(nameof (IsGameFocusedWindow));
    public static readonly StringName GetOrCreateFeedbackScreen = StringName.op_Implicit(nameof (GetOrCreateFeedbackScreen));
    public static readonly StringName GetGameVersion = StringName.op_Implicit(nameof (GetGameVersion));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName RootSceneContainer = StringName.op_Implicit(nameof (RootSceneContainer));
    public static readonly StringName HoverTipsContainer = StringName.op_Implicit(nameof (HoverTipsContainer));
    public static readonly StringName MainMenu = StringName.op_Implicit(nameof (MainMenu));
    public static readonly StringName CurrentRunNode = StringName.op_Implicit(nameof (CurrentRunNode));
    public static readonly StringName LogoAnimation = StringName.op_Implicit(nameof (LogoAnimation));
    public static readonly StringName Transition = StringName.op_Implicit(nameof (Transition));
    public static readonly StringName TimeoutOverlay = StringName.op_Implicit(nameof (TimeoutOverlay));
    public static readonly StringName AudioManager = StringName.op_Implicit(nameof (AudioManager));
    public static readonly StringName RemoteCursorContainer = StringName.op_Implicit(nameof (RemoteCursorContainer));
    public static readonly StringName InputManager = StringName.op_Implicit(nameof (InputManager));
    public static readonly StringName HotkeyManager = StringName.op_Implicit(nameof (HotkeyManager));
    public static readonly StringName FeedbackScreen = StringName.op_Implicit(nameof (FeedbackScreen));
    public static readonly StringName ReactionWheel = StringName.op_Implicit(nameof (ReactionWheel));
    public static readonly StringName ReactionContainer = StringName.op_Implicit(nameof (ReactionContainer));
    public static readonly StringName CursorManager = StringName.op_Implicit(nameof (CursorManager));
    public static readonly StringName DebugAudio = StringName.op_Implicit(nameof (DebugAudio));
    public static readonly StringName DebugSeedOverride = StringName.op_Implicit(nameof (DebugSeedOverride));
    public static readonly StringName StartOnMainMenu = StringName.op_Implicit(nameof (StartOnMainMenu));
    public static readonly StringName InspectRelicScreen = StringName.op_Implicit(nameof (InspectRelicScreen));
    public static readonly StringName InspectCardScreen = StringName.op_Implicit(nameof (InspectCardScreen));
    public static readonly StringName ScreenshakeTarget = StringName.op_Implicit(nameof (ScreenshakeTarget));
    public static readonly StringName WorldEnvironment = StringName.op_Implicit(nameof (WorldEnvironment));
    public static readonly StringName HitStop = StringName.op_Implicit(nameof (HitStop));
    public static readonly StringName _inspectionContainer = StringName.op_Implicit(nameof (_inspectionContainer));
    public static readonly StringName _screenShake = StringName.op_Implicit(nameof (_screenShake));
  }

  public class SignalName : Control.SignalName
  {
    public static readonly StringName WindowChange = StringName.op_Implicit(nameof (WindowChange));
    public static readonly StringName PhobiaModeToggled = StringName.op_Implicit(nameof (PhobiaModeToggled));
  }
}
