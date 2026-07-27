// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.AutoSlay.AutoSlayer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.AutoSlay.Handlers;
using MegaCrit.Sts2.Core.AutoSlay.Handlers.Rooms;
using MegaCrit.Sts2.Core.AutoSlay.Handlers.Screens;
using MegaCrit.Sts2.Core.AutoSlay.Helpers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.Fonts;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Events;
using MegaCrit.Sts2.Core.Nodes.Events.Custom.CrystalSphere;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;
using MegaCrit.Sts2.Core.Nodes.Screens.GameOverScreen;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Nodes.Screens.PauseMenu;
using MegaCrit.Sts2.Core.Nodes.TopBar;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using MegaCrit.Sts2.Core.Timeline;
using MegaCrit.Sts2.Core.Timeline.Epochs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.AutoSlay;

public class AutoSlayer
{
  private readonly Dictionary<RoomType, IRoomHandler> _roomHandlers;
  private readonly Dictionary<Type, IScreenHandler> _screenHandlers;
  private readonly MapScreenHandler _mapHandler;
  private CancellationTokenSource? _cts;
  private Rng? _random;
  private Watchdog? _watchdog;
  private IDisposable? _cardSelectorScope;
  private static int _exitCode;

  static AutoSlayer()
  {
    NonInteractiveMode.AutoSlayerCheck = (Func<bool>) (() => AutoSlayer.IsActive);
  }

  public static bool IsActive { get; private set; }

  public static Watchdog? CurrentWatchdog { get; private set; }

  public AutoSlayer()
  {
    CombatRoomHandler combatRoomHandler = new CombatRoomHandler();
    this._roomHandlers = new Dictionary<RoomType, IRoomHandler>()
    {
      [RoomType.Monster] = (IRoomHandler) combatRoomHandler,
      [RoomType.Elite] = (IRoomHandler) combatRoomHandler,
      [RoomType.Boss] = (IRoomHandler) combatRoomHandler,
      [RoomType.Event] = (IRoomHandler) new EventRoomHandler(),
      [RoomType.Shop] = (IRoomHandler) new ShopRoomHandler(),
      [RoomType.Treasure] = (IRoomHandler) new TreasureRoomHandler(),
      [RoomType.RestSite] = (IRoomHandler) new RestSiteRoomHandler()
    };
    this._mapHandler = new MapScreenHandler();
    Dictionary<Type, IScreenHandler> dictionary = new Dictionary<Type, IScreenHandler>();
    Type key1 = typeof (NRewardsScreen);
    dictionary[key1] = (IScreenHandler) new RewardsScreenHandler();
    Type key2 = typeof (NCardRewardSelectionScreen);
    dictionary[key2] = (IScreenHandler) new CardRewardScreenHandler();
    Type key3 = typeof (NDeckUpgradeSelectScreen);
    dictionary[key3] = (IScreenHandler) new DeckUpgradeScreenHandler();
    Type key4 = typeof (NDeckTransformSelectScreen);
    dictionary[key4] = (IScreenHandler) new DeckTransformScreenHandler();
    Type key5 = typeof (NDeckEnchantSelectScreen);
    dictionary[key5] = (IScreenHandler) new DeckEnchantScreenHandler();
    Type key6 = typeof (NDeckCardSelectScreen);
    dictionary[key6] = (IScreenHandler) new DeckCardSelectScreenHandler();
    Type key7 = typeof (NSimpleCardSelectScreen);
    dictionary[key7] = (IScreenHandler) new SimpleCardSelectScreenHandler();
    Type key8 = typeof (NChooseACardSelectionScreen);
    dictionary[key8] = (IScreenHandler) new ChooseACardScreenHandler();
    Type key9 = typeof (NChooseABundleSelectionScreen);
    dictionary[key9] = (IScreenHandler) new ChooseABundleScreenHandler();
    Type key10 = typeof (NChooseARelicSelection);
    dictionary[key10] = (IScreenHandler) new ChooseARelicScreenHandler();
    Type key11 = typeof (NGameOverScreen);
    dictionary[key11] = (IScreenHandler) new GameOverScreenHandler();
    Type key12 = typeof (NCrystalSphereScreen);
    dictionary[key12] = (IScreenHandler) new CrystalSphereScreenHandler();
    this._screenHandlers = dictionary;
  }

  public void Start(string seed, string? logFile = null)
  {
    if (logFile != null)
      AutoSlayLog.OpenLogFile(logFile);
    SentryService.SetTag("autoslay", "true");
    SentryService.SetTag("autoslay.seed", seed);
    AutoSlayer.IsActive = true;
    this._cts = new CancellationTokenSource();
    TaskHelper.RunSafely(this.RunAsync(seed, this._cts.Token));
  }

  public void Stop()
  {
    AutoSlayer.IsActive = false;
    this._cts?.Cancel();
    this._cts = (CancellationTokenSource) null;
  }

  public static T GetCurrentScreen<T>() where T : Node => (T) NOverlayStack.Instance.Peek();

  private async Task RunAsync(string seed, CancellationToken ct)
  {
    AutoSlayLog.RunStarted(seed);
    try
    {
      await WaitHelper.WithTimeout((Func<CancellationToken, Task>) (token => this.PlayRunAsync(seed, token)), AutoSlayConfig.runTimeout, ct);
      AutoSlayLog.RunCompleted(seed);
    }
    catch (Exception ex)
    {
      AutoSlayer._exitCode = 1;
      AutoSlayLog.RunFailed(seed, ex);
      throw;
    }
    finally
    {
      AutoSlayer.IsActive = false;
      AutoSlayer.CurrentWatchdog = (Watchdog) null;
      this._watchdog = (Watchdog) null;
      this._cardSelectorScope?.Dispose();
      this._cardSelectorScope = (IDisposable) null;
      MemoryProfiler.Reset();
      AutoSlayLog.CloseLogFile();
      AutoSlayer.QuitGame(AutoSlayer._exitCode);
    }
  }

  private async Task PlayRunAsync(string seed, CancellationToken ct)
  {
    await WaitHelper.Until((Func<bool>) (() => NGame.Instance != null), ct, new TimeSpan?(AutoSlayConfig.gameInitTimeout), "Game instance not initialized");
    NGame.Instance.DebugSeedOverride = seed;
    SaveManager.Instance.PrefsSave.FastMode = FastModeType.Fast;
    SaveManager.Instance.SetFtuesEnabled(false);
    SaveManager.Instance.ObtainEpochOverride(EpochModel.GetId<Silent1Epoch>(), EpochState.Revealed);
    SaveManager.Instance.ObtainEpochOverride(EpochModel.GetId<Regent1Epoch>(), EpochState.Revealed);
    SaveManager.Instance.ObtainEpochOverride(EpochModel.GetId<Defect1Epoch>(), EpochState.Revealed);
    SaveManager.Instance.ObtainEpochOverride(EpochModel.GetId<Necrobinder1Epoch>(), EpochState.Revealed);
    this._random = new Rng(StringHelper.GetDeterministicHashCode(seed));
    this._cardSelectorScope = CardSelectCmd.UseSelector((MegaCrit.Sts2.Core.TestSupport.ICardSelector) new AutoSlayCardSelector(this._random));
    this._watchdog = new Watchdog();
    AutoSlayer.CurrentWatchdog = this._watchdog;
    this._watchdog.Reset("Playing main menu");
    await this.PlayMainMenuAsync(ct);
    await WaitHelper.Until((Func<bool>) (() => RunManager.Instance.DebugOnlyGetState() != null), ct, new TimeSpan?(AutoSlayConfig.runStateTimeout), "Run state not initialized");
    RunState runState = RunManager.Instance.DebugOnlyGetState();
    MemoryProfiler.SetBaseline();
    await WaitHelper.Until((Func<bool>) (() => runState.CurrentRoom != null && runState.CurrentRoom.RoomType != 0), ct, new TimeSpan?(AutoSlayConfig.nodeWaitTimeout), "Room type not assigned");
    while (runState.TotalFloor < 49)
    {
      ct.ThrowIfCancellationRequested();
      RoomType roomType = runState.CurrentRoom.RoomType;
      this._watchdog.Reset($"Entering {roomType} room (Act {runState.CurrentActIndex + 1}, Floor {runState.ActFloor})");
      AutoSlayLog.EnterRoom(roomType, runState.CurrentActIndex, runState.ActFloor);
      MemoryProfiler.LogSnapshot($"pre-room:{roomType}:Act{runState.CurrentActIndex + 1}:F{runState.ActFloor}");
      await this.HandleRoomAsync(roomType, ct);
      bool flag1;
      switch (roomType)
      {
        case RoomType.Monster:
        case RoomType.Elite:
        case RoomType.Boss:
          flag1 = true;
          break;
        default:
          flag1 = false;
          break;
      }
      if (flag1)
        await this.WaitForRewardsScreenAsync(ct);
      else
        await Task.Delay(500, ct);
      await this.DrainOverlayScreensAsync(ct);
      if (roomType == RoomType.RestSite)
        await this.ClickRestSiteProceedIfNeeded(ct);
      if (roomType == RoomType.Event)
        await this.ClickEventProceedIfNeeded(ct);
      MemoryProfiler.LogSnapshot($"post-room:{roomType}:Act{runState.CurrentActIndex + 1}:F{runState.ActFloor}");
      int num;
      if (roomType == RoomType.Boss && runState.Map.SecondBossMapPoint != null)
      {
        MapCoord? currentMapCoord = runState.CurrentMapCoord;
        MapCoord coord = runState.Map.BossMapPoint.coord;
        num = currentMapCoord.HasValue ? (currentMapCoord.GetValueOrDefault() == coord ? 1 : 0) : 0;
      }
      else
        num = 0;
      bool flag2 = num != 0;
      if (roomType == RoomType.Boss && !flag2)
      {
        this._watchdog.Reset("Waiting for act transition after boss");
        RoomType postBossRoomType = RoomType.Boss;
        await WaitHelper.Until((Func<bool>) (() =>
        {
          AbstractRoom currentRoom = runState.CurrentRoom;
          if (currentRoom == null)
            return false;
          postBossRoomType = currentRoom.RoomType;
          return postBossRoomType != RoomType.Boss;
        }), ct, new TimeSpan?(TimeSpan.FromSeconds(10L)), "Act transition did not start after boss");
        AutoSlayLog.Info($"Post-boss transition: room type is now {postBossRoomType}");
        if (postBossRoomType == RoomType.Event && runState.CurrentActIndex >= runState.Acts.Count - 1)
        {
          this._watchdog.Reset($"Entering {postBossRoomType} room (Act {runState.CurrentActIndex + 1}, Floor {runState.ActFloor})");
          AutoSlayLog.EnterRoom(postBossRoomType, runState.CurrentActIndex, runState.ActFloor);
          await this.HandleRoomAsync(postBossRoomType, ct);
          await this.WaitForGameOverScreenAsync(ct);
          await this.DrainOverlayScreensAsync(ct);
          this._watchdog.Reset("Waiting for main menu after victory");
          await this.WaitForMainMenuAsync(ct);
          AutoSlayLog.Action("Victory! Run completed and returned to main menu");
          return;
        }
        await WaitHelper.Until((Func<bool>) (() => runState.VisitedMapCoords.Count == 0), ct, new TimeSpan?(TimeSpan.FromSeconds(5L)), "Act transition did not complete (VisitedMapCoords not cleared)");
        MemoryProfiler.LogSnapshot($"act-transition:Act{runState.CurrentActIndex + 1}");
      }
      this._watchdog.Reset("Navigating map");
      await this._mapHandler.HandleAsync(this._random, ct);
    }
    AutoSlayLog.Action("Run completed (max floor reached). Abandoning");
    await this.AbandonRunAsync(ct);
  }

  private async Task HandleRoomAsync(RoomType roomType, CancellationToken ct)
  {
    IRoomHandler handler;
    if (!this._roomHandlers.TryGetValue(roomType, out handler))
    {
      AutoSlayLog.Warn($"No handler for room type: {roomType}");
    }
    else
    {
      await WaitHelper.WithTimeout((Func<CancellationToken, Task>) (token => handler.HandleAsync(this._random, token)), handler.Timeout, ct);
      AutoSlayLog.ExitRoom(roomType);
    }
  }

  private async Task DrainOverlayScreensAsync(CancellationToken ct)
  {
    if (NOverlayStack.Instance == null)
      await WaitHelper.Until((Func<bool>) (() => NOverlayStack.Instance != null), ct, new TimeSpan?(AutoSlayConfig.nodeWaitTimeout), "Overlay stack not initialized");
    HashSet<IOverlayScreen> handledScreens = new HashSet<IOverlayScreen>();
    int consecutiveFailures = 0;
    IOverlayScreen currentOverlay;
    Type type;
    while (true)
    {
      NOverlayStack instance1 = NOverlayStack.Instance;
      if ((instance1 != null ? (instance1.ScreenCount > 0 ? 1 : 0) : 0) != 0)
      {
        ct.ThrowIfCancellationRequested();
        currentOverlay = NOverlayStack.Instance.Peek();
        if (currentOverlay != null)
        {
          if (handledScreens.Contains(currentOverlay))
          {
            ++consecutiveFailures;
            if (consecutiveFailures < 3)
              AutoSlayLog.Warn($"Screen {currentOverlay.GetType().Name} still present after handling (attempt {consecutiveFailures})");
            else
              goto label_6;
          }
          else
          {
            handledScreens.Add(currentOverlay);
            consecutiveFailures = 0;
          }
          type = ((Node) currentOverlay).GetType();
          IScreenHandler handler;
          if (this._screenHandlers.TryGetValue(type, out handler))
          {
            this._watchdog.Reset("Handling screen: " + type.Name);
            AutoSlayLog.Info("Handling screen: " + type.Name);
            await WaitHelper.WithTimeout((Func<CancellationToken, Task>) (token => handler.HandleAsync(this._random, token)), handler.Timeout, ct);
            if (currentOverlay is NRewardsScreen)
            {
              NMapScreen instance2 = NMapScreen.Instance;
              if ((instance2 != null ? (instance2.IsOpen ? 1 : 0) : 0) != 0)
                goto label_14;
            }
            await Task.Delay(100, ct);
            currentOverlay = (IOverlayScreen) null;
          }
          else
            goto label_10;
        }
        else
          break;
      }
      else
        goto label_19;
    }
    handledScreens = (HashSet<IOverlayScreen>) null;
    return;
label_6:
    AutoSlayLog.Error($"Infinite loop detected: screen {currentOverlay.GetType().Name} not closing after {3} attempts");
    throw new InvalidOperationException($"Screen {currentOverlay.GetType().Name} not closing after being handled");
label_10:
    AutoSlayLog.Warn("No handler for screen type: " + type.Name);
    handledScreens = (HashSet<IOverlayScreen>) null;
    return;
label_14:
    AutoSlayLog.Info("Rewards screen handled and map is open, exiting drain loop");
    handledScreens = (HashSet<IOverlayScreen>) null;
    return;
label_19:
    handledScreens = (HashSet<IOverlayScreen>) null;
  }

  private async Task ClickRestSiteProceedIfNeeded(CancellationToken ct)
  {
    NProceedButton nodeOrNull = ((Node) ((SceneTree) Engine.GetMainLoop()).Root).GetNodeOrNull<NProceedButton>(NodePath.op_Implicit("/root/Game/RootSceneContainer/Run/RoomContainer/RestSiteRoom/ProceedButton"));
    if (nodeOrNull == null || !nodeOrNull.IsEnabled)
      return;
    AutoSlayLog.Action("Clicking rest site proceed button");
    await UiHelper.Click((NClickableControl) nodeOrNull);
  }

  private async Task ClickEventProceedIfNeeded(CancellationToken ct)
  {
    Node eventRoom = ((Node) ((SceneTree) Engine.GetMainLoop()).Root).GetNodeOrNull(NodePath.op_Implicit("/root/Game/RootSceneContainer/Run/RoomContainer/EventRoom"));
    if (eventRoom == null)
    {
      AutoSlayLog.Info("Event room not found for proceed check");
    }
    else
    {
      NEventOptionButton proceedOption = (NEventOptionButton) null;
      await WaitHelper.Until((Func<bool>) (() =>
      {
        NMapScreen instance = NMapScreen.Instance;
        if ((instance != null ? (instance.IsOpen ? 1 : 0) : 0) != 0)
          return true;
        List<NEventOptionButton> list = UiHelper.FindAll<NEventOptionButton>(eventRoom).Where<NEventOptionButton>((Func<NEventOptionButton, bool>) (o => !o.Option.IsLocked && o.Option.IsProceed)).ToList<NEventOptionButton>();
        if (list.Count <= 0)
          return false;
        proceedOption = list[0];
        return true;
      }), ct, new TimeSpan?(TimeSpan.FromSeconds(5L)), "Event proceed option or map did not appear");
      if (proceedOption != null)
      {
        AutoSlayLog.Action("Clicking event proceed option");
        await UiHelper.Click((NClickableControl) proceedOption);
      }
      else
        AutoSlayLog.Info("Map already open, no proceed needed");
    }
  }

  private async Task WaitForRewardsScreenAsync(CancellationToken ct)
  {
    AutoSlayLog.Action("Waiting for rewards screen");
    await WaitHelper.Until((Func<bool>) (() =>
    {
      if (NOverlayStack.Instance?.Peek() is NRewardsScreen)
        return true;
      NMapScreen instance = NMapScreen.Instance;
      return instance != null && instance.IsOpen;
    }), ct, new TimeSpan?(TimeSpan.FromSeconds(10L)), "Rewards screen did not appear after combat");
  }

  private async Task WaitForGameOverScreenAsync(CancellationToken ct)
  {
    AutoSlayLog.Action("Waiting for game over screen");
    await WaitHelper.Until((Func<bool>) (() => NOverlayStack.Instance?.Peek() is NGameOverScreen), ct, new TimeSpan?(TimeSpan.FromSeconds(10L)), "Game over screen did not appear");
  }

  private async Task WaitForMainMenuAsync(CancellationToken ct)
  {
    AutoSlayLog.Action("Waiting for main menu");
    Node root = (Node) ((SceneTree) Engine.GetMainLoop()).Root;
    await WaitHelper.Until((Func<bool>) (() =>
    {
      Control nodeOrNull = root.GetNodeOrNull<Control>(NodePath.op_Implicit("/root/Game/RootSceneContainer/MainMenu"));
      return nodeOrNull != null && ((CanvasItem) nodeOrNull).IsVisibleInTree();
    }), ct, new TimeSpan?(TimeSpan.FromSeconds(30L)), "Main menu did not appear after game over");
    AutoSlayLog.Action("Main menu appeared");
  }

  private async Task PlayMainMenuAsync(CancellationToken ct)
  {
    AutoSlayLog.Action("Playing main menu");
    Control mainMenu = await WaitHelper.ForNode<Control>((Node) ((SceneTree) Engine.GetMainLoop()).Root, "/root/Game/RootSceneContainer/MainMenu", ct, new TimeSpan?(TimeSpan.FromSeconds(30L)));
    NButton node1 = ((Node) mainMenu).GetNode<NButton>(NodePath.op_Implicit("MainMenuTextButtons/AbandonRunButton"));
    if (((CanvasItem) node1).Visible)
    {
      AutoSlayLog.Action("Abandoning existing run");
      await UiHelper.Click((NClickableControl) node1);
      await WaitHelper.Until((Func<bool>) (() => NModalContainer.Instance?.OpenModal != null), ct, new TimeSpan?(AutoSlayConfig.nodeWaitTimeout), "Abandon run confirmation popup did not appear");
      NButton node2 = ((Node) NModalContainer.Instance.OpenModal).GetNode<NButton>(NodePath.op_Implicit("VerticalPopup/YesButton"));
      AutoSlayLog.Action("Confirming abandon");
      await UiHelper.Click((NClickableControl) node2);
      await WaitHelper.Until((Func<bool>) (() => NModalContainer.Instance.OpenModal == null), ct, new TimeSpan?(AutoSlayConfig.nodeWaitTimeout), "Abandon run confirmation popup did not close");
    }
    NButton node3 = ((Node) mainMenu).GetNode<NButton>(NodePath.op_Implicit("MainMenuTextButtons/SingleplayerButton"));
    AutoSlayLog.Action("Clicking singleplayer");
    await UiHelper.Click((NClickableControl) node3);
    Control charSelectScreen = ((Node) mainMenu).GetNodeOrNull<Control>(NodePath.op_Implicit("Submenus/CharacterSelectScreen"));
    NButton standardButton = ((Node) mainMenu).GetNodeOrNull<NButton>(NodePath.op_Implicit("Submenus/SingleplayerSubmenu/StandardButton"));
    await WaitHelper.Until((Func<bool>) (() =>
    {
      charSelectScreen = ((Node) mainMenu).GetNodeOrNull<Control>(NodePath.op_Implicit("Submenus/CharacterSelectScreen"));
      standardButton = ((Node) mainMenu).GetNodeOrNull<NButton>(NodePath.op_Implicit("Submenus/SingleplayerSubmenu/StandardButton"));
      Control control = charSelectScreen;
      bool flag1 = control != null && ((CanvasItem) control).Visible;
      NButton nbutton = standardButton;
      bool flag2 = nbutton != null && ((CanvasItem) nbutton).Visible;
      return flag1 | flag2;
    }), ct, new TimeSpan?(AutoSlayConfig.nodeWaitTimeout), "Neither CharacterSelectScreen nor SingleplayerSubmenu became visible");
    NButton nbutton1 = standardButton;
    if ((nbutton1 != null ? (((CanvasItem) nbutton1).Visible ? 1 : 0) : 0) != 0)
    {
      Control control = charSelectScreen;
      if ((control != null ? (!((CanvasItem) control).Visible ? 1 : 0) : 1) != 0)
      {
        AutoSlayLog.Action("Clicking standard run");
        await UiHelper.Click((NClickableControl) standardButton);
        await WaitHelper.Until((Func<bool>) (() =>
        {
          Control nodeOrNull = ((Node) mainMenu).GetNodeOrNull<Control>(NodePath.op_Implicit("Submenus/CharacterSelectScreen"));
          return nodeOrNull != null && ((CanvasItem) nodeOrNull).Visible;
        }), ct, new TimeSpan?(AutoSlayConfig.nodeWaitTimeout), "CharacterSelectScreen did not become visible");
        charSelectScreen = ((Node) mainMenu).GetNode<Control>(NodePath.op_Implicit("Submenus/CharacterSelectScreen"));
        goto label_15;
      }
    }
    AutoSlayLog.Action("Skipping submenu (first run)");
label_15:
    List<NCharacterSelectButton> all = UiHelper.FindAll<NCharacterSelectButton>(((Node) charSelectScreen).GetNode(NodePath.op_Implicit("CharSelectButtons/ButtonContainer")));
    foreach (NCharacterSelectButton ncharacterSelectButton in all)
      ncharacterSelectButton.UnlockIfPossible();
    NCharacterSelectButton ncharacterSelectButton1 = this._random.NextItem<NCharacterSelectButton>((IEnumerable<NCharacterSelectButton>) all.Where<NCharacterSelectButton>((Func<NCharacterSelectButton, bool>) (b => !b.IsLocked)).ToList<NCharacterSelectButton>());
    AutoSlayLog.Action($"Selecting character: {ncharacterSelectButton1.Character.Id}");
    ncharacterSelectButton1.Select();
    await Task.Delay(100, ct);
    NButton button = await WaitHelper.ForNode<NButton>((Node) mainMenu, "Submenus/CharacterSelectScreen/ConfirmButton", ct);
    AutoSlayLog.Action("Confirming character");
    await UiHelper.Click((NClickableControl) button);
  }

  private async Task AbandonRunAsync(CancellationToken ct)
  {
    Node root = (Node) ((SceneTree) Engine.GetMainLoop()).Root;
    await Task.Delay(1000, ct);
    await UiHelper.Click((NClickableControl) await WaitHelper.ForNode<NTopBarPauseButton>(root, "/root/Game/RootSceneContainer/Run/GlobalUi/TopBar/RightAlignedStuff/PauseButton", ct));
    NPauseMenu pauseMenu = (NPauseMenu) null;
    await WaitHelper.Until((Func<bool>) (() => (pauseMenu = UiHelper.FindFirst<NPauseMenu>(root)) != null && ((CanvasItem) pauseMenu).IsVisibleInTree()), ct, timeoutMessage: "Pause menu did not open");
    await UiHelper.Click((NClickableControl) ((Node) ((Node) pauseMenu).GetNode<Control>(NodePath.op_Implicit("%ButtonContainer"))).GetNode<NPauseMenuButton>(NodePath.op_Implicit("GiveUp")));
    NAbandonRunConfirmPopup confirmPopup = (NAbandonRunConfirmPopup) null;
    await WaitHelper.Until((Func<bool>) (() => (confirmPopup = UiHelper.FindFirst<NAbandonRunConfirmPopup>(root)) != null), ct, timeoutMessage: "Abandon confirm popup did not appear");
    await UiHelper.Click((NClickableControl) ((Node) confirmPopup).GetNode<NVerticalPopup>(NodePath.op_Implicit("VerticalPopup")).YesButton);
    await this.WaitForGameOverScreenAsync(ct);
    await this.DrainOverlayScreensAsync(ct);
    await this.WaitForMainMenuAsync(ct);
  }

  private static void QuitGame(int exitCode)
  {
    AutoSlayLog.Action($"Quitting game with exit code {exitCode}");
    MegaLabel.DisposeCachedParagraph();
    MegaRichTextLabel.DisposeCachedParagraph();
    FontManager.ClearCache();
    ((Node) NGame.Instance)?.GetTree().Quit(exitCode);
  }
}
