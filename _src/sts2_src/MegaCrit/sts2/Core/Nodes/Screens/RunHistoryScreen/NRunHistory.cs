// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.RunHistoryScreen.NRunHistory
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.Exceptions;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Debug;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Potions;
using MegaCrit.Sts2.Core.Nodes.Screens.GameOverScreen;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.Unlocks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.RunHistoryScreen;

[ScriptPath("res://src/Core/Nodes/Screens/RunHistoryScreen/NRunHistory.cs")]
public class NRunHistory : NSubmenu
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("screens/run_history_screen/run_history");
  private const float _bottomScrollbarPadding = 25f;
  public const string locTable = "run_history";
  private static readonly LocString _leftQuote = new LocString("game_over_screen", "ENCOUNTER_QUOTE_LEFT");
  private static readonly LocString _rightQuote = new LocString("game_over_screen", "ENCOUNTER_QUOTE_RIGHT");
  private readonly LocString _dateFormat = new LocString("run_history", "INFO.DATE_FORMAT");
  private readonly LocString _timeFormat = new LocString("run_history", "INFO.TIME_FORMAT");
  private readonly LocString _dateTimeLocString = new LocString("run_history", "INFO.DATE_TIME");
  private readonly LocString _seedLocString = new LocString("run_history", "INFO.SEED");
  private NScrollableContainer _screenContents;
  private Control _playerIconContainer;
  private MegaLabel _hpLabel;
  private MegaLabel _goldLabel;
  private Control _potionHolder;
  private MegaLabel _floorLabel;
  private MegaLabel _timeLabel;
  private MegaRichTextLabel _dateLabel;
  private MegaRichTextLabel _seedLabel;
  private MegaRichTextLabel _gameModeLabel;
  private MegaRichTextLabel _buildLabel;
  private Control _badgeContainer;
  private MegaRichTextLabel _deathQuoteLabel;
  private NMapPointHistory _mapPointHistory;
  private NRelicHistory _relicHistory;
  private NDeckHistory _deckHistory;
  private Control _outOfDateVisual;
  private readonly List<string> _runNames = new List<string>();
  private int _index;
  private RunHistory _history;
  private NRunHistoryArrowButton _prevButton;
  private NRunHistoryArrowButton _nextButton;
  private NRunHistoryPlayerIcon? _selectedPlayerIcon;
  private Tween? _screenTween;

  protected override Control? InitialFocusedControl => (Control) null;

  public static string[] AssetPaths
  {
    get => new string[1]{ NRunHistory._scenePath };
  }

  public static NRunHistory? Create()
  {
    return TestMode.IsOn ? (NRunHistory) null : PreloadManager.Cache.GetScene(NRunHistory._scenePath).Instantiate<NRunHistory>((PackedScene.GenEditState) 0L);
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._screenContents = ((Node) this).GetNode<NScrollableContainer>(NodePath.op_Implicit("ScreenContents"));
    this._playerIconContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%PlayerIconContainer"));
    this._hpLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%HpLabel"));
    this._goldLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%GoldLabel"));
    this._potionHolder = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%PotionHolders"));
    this._floorLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%FloorNumLabel"));
    this._timeLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%RunTimeLabel"));
    this._dateLabel = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%DateLabel"));
    this._seedLabel = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%SeedLabel"));
    this._gameModeLabel = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%GameModeLabel"));
    this._buildLabel = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%BuildLabel"));
    this._deathQuoteLabel = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%DeathQuoteLabel"));
    this._badgeContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%BadgeContainer"));
    this._mapPointHistory = ((Node) this).GetNode<NMapPointHistory>(NodePath.op_Implicit("%MapPointHistory"));
    this._relicHistory = ((Node) this).GetNode<NRelicHistory>(NodePath.op_Implicit("%RelicHistory"));
    this._deckHistory = ((Node) this).GetNode<NDeckHistory>(NodePath.op_Implicit("%DeckHistory"));
    this._outOfDateVisual = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%OutOfDateVisual"));
    this._prevButton = ((Node) this).GetNode<NRunHistoryArrowButton>(NodePath.op_Implicit("LeftArrow"));
    ((GodotObject) this._prevButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnLeftButtonButtonReleased)), 0U);
    this._nextButton = ((Node) this).GetNode<NRunHistoryArrowButton>(NodePath.op_Implicit("RightArrow"));
    ((GodotObject) this._nextButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnRightButtonButtonReleased)), 0U);
    this._prevButton.IsLeft = true;
    this._mapPointHistory.SetDeckHistory(this._deckHistory);
    this._mapPointHistory.SetRelicHistory(this._relicHistory);
    this._screenContents.DisableScrollingIfContentFits();
    this._screenContents.UpdatePadding(paddingBottom: 25f);
  }

  private void OnLeftButtonButtonReleased(NButton _)
  {
    TaskHelper.RunSafely(this.RefreshAndSelectRun(this._index + 1));
    this._screenTween?.Kill();
    this._screenTween = ((Node) this).CreateTween().SetParallel(true);
    this._screenTween.TweenProperty((GodotObject) this._screenContents, NodePath.op_Implicit("position"), Variant.op_Implicit(Vector2.Zero), 0.5).SetTrans((Tween.TransitionType) 5L).SetEase((Tween.EaseType) 1L).From(Variant.op_Implicit(Vector2.op_Addition(Vector2.Zero, new Vector2(-1000f, 0.0f))));
    this._screenTween.TweenProperty((GodotObject) this._screenContents, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.4).SetTrans((Tween.TransitionType) 0L).From(Variant.op_Implicit(0.0f));
  }

  private void OnRightButtonButtonReleased(NButton _)
  {
    TaskHelper.RunSafely(this.RefreshAndSelectRun(this._index - 1));
    this._screenTween?.Kill();
    this._screenTween = ((Node) this).CreateTween().SetParallel(true);
    this._screenTween.TweenProperty((GodotObject) this._screenContents, NodePath.op_Implicit("position"), Variant.op_Implicit(Vector2.Zero), 0.5).SetTrans((Tween.TransitionType) 5L).SetEase((Tween.EaseType) 1L).From(Variant.op_Implicit(Vector2.op_Addition(Vector2.Zero, new Vector2(1000f, 0.0f))));
    this._screenTween.TweenProperty((GodotObject) this._screenContents, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.4).SetTrans((Tween.TransitionType) 0L).From(Variant.op_Implicit(0.0f));
  }

  public static bool CanBeShown() => SaveManager.Instance.GetRunHistoryCount() > 0;

  public override void OnSubmenuOpened()
  {
    this._runNames.Clear();
    this._runNames.AddRange((IEnumerable<string>) SaveManager.Instance.GetAllRunHistoryNames());
    this._runNames.Reverse();
    TaskHelper.RunSafely(this.RefreshAndSelectRun(0));
  }

  protected override void OnSubmenuShown()
  {
    if (!NRunHistory.CanBeShown())
      throw new InvalidOperationException("Tried to show run history screen with no runs!");
    this._screenTween?.Kill();
    this._screenTween = ((Node) this).CreateTween();
    this._screenTween.TweenProperty((GodotObject) this._screenContents, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.4).From(Variant.op_Implicit(0.0f));
  }

  protected override void OnSubmenuHidden() => this._screenTween?.Kill();

  private Task RefreshAndSelectRun(int index)
  {
    if (index < 0 || index >= this._runNames.Count)
    {
      Log.Error($"Invalid run index {index}, valid range is 0-{this._runNames.Count - 1}");
      return Task.CompletedTask;
    }
    this._prevButton.Disable();
    this._nextButton.Disable();
    ((CanvasItem) this._outOfDateVisual).Visible = false;
    try
    {
      ReadSaveResult<RunHistory> readSaveResult = SaveManager.Instance.LoadRunHistory(this._runNames[index]);
      if (readSaveResult.Success)
      {
        this.DisplayRun(readSaveResult.SaveData);
      }
      else
      {
        Log.Error($"Could not load run {this._runNames[index]} at index {index}: {readSaveResult.ErrorMessage} ({readSaveResult.Status})");
        ((CanvasItem) this._outOfDateVisual).Visible = true;
      }
    }
    catch (Exception ex)
    {
      Log.Error($"Exception {ex} while loading run at index {index}");
      ((CanvasItem) this._outOfDateVisual).Visible = true;
      throw;
    }
    finally
    {
      this._index = index;
      if (index < this._runNames.Count - 1)
        this._prevButton.Enable();
      if (index > 0)
        this._nextButton.Enable();
      ((CanvasItem) this._prevButton).Visible = index < this._runNames.Count - 1;
      ((CanvasItem) this._nextButton).Visible = index > 0;
    }
    return Task.CompletedTask;
  }

  public override void _Input(InputEvent inputEvent)
  {
    if (!((CanvasItem) this).IsVisibleInTree() || NDevConsole.IsConsoleVisible || !NControllerManager.Instance.IsUsingController)
      return;
    bool flag;
    switch (((Node) this).GetViewport().GuiGetFocusOwner())
    {
      case TextEdit _:
      case LineEdit _:
        flag = true;
        break;
      default:
        flag = false;
        break;
    }
    if (flag || !ActiveScreenContext.Instance.IsCurrent((IScreenContext) this))
      return;
    Control focusOwner = ((Node) this).GetViewport().GuiGetFocusOwner();
    if (focusOwner != null && ((Node) this).IsAncestorOf((Node) focusOwner) || !inputEvent.IsActionPressed(MegaInput.left, false, false) && !inputEvent.IsActionPressed(MegaInput.right, false, false) && !inputEvent.IsActionPressed(MegaInput.up, false, false) && !inputEvent.IsActionPressed(MegaInput.down, false, false) && !inputEvent.IsActionPressed(MegaInput.select, false, false))
      return;
    ((Node) this).GetViewport()?.SetInputAsHandled();
    Control defaultFocusedControl = this._mapPointHistory.DefaultFocusedControl;
    if (defaultFocusedControl == null)
      return;
    defaultFocusedControl.TryGrabFocus();
  }

  private void DisplayRun(RunHistory history)
  {
    this._selectedPlayerIcon?.Deselect();
    this._selectedPlayerIcon = (NRunHistoryPlayerIcon) null;
    foreach (Node node in ((IEnumerable) ((Node) this._playerIconContainer).GetChildren(false)).OfType<NRunHistoryPlayerIcon>())
      node.QueueFreeSafely();
    this._history = history;
    ulong localPlayerId = PlatformUtil.GetLocalPlayerId(history.PlatformType);
    this.LoadPlayerFloor(history);
    this.LoadGameModeDetails(history);
    this.LoadTimeDetails(history);
    this._mapPointHistory.LoadHistory(history);
    bool flag = false;
    NRunHistoryPlayerIcon playerIcon1 = (NRunHistoryPlayerIcon) null;
    foreach (RunHistoryPlayer player in history.Players)
    {
      NRunHistoryPlayerIcon playerIcon = PreloadManager.Cache.GetScene(NRunHistoryPlayerIcon.scenePath).Instantiate<NRunHistoryPlayerIcon>((PackedScene.GenEditState) 0L);
      if (playerIcon1 == null)
        playerIcon1 = playerIcon;
      ((Node) this._playerIconContainer).AddChildSafely((Node) playerIcon);
      playerIcon.LoadRun(player, history);
      ((GodotObject) playerIcon).Connect(NClickableControl.SignalName.Released, Callable.From<NClickableControl>((Action<NClickableControl>) (_ => this.SelectPlayer(playerIcon))), 0U);
      if ((long) player.Id == (long) localPlayerId)
      {
        flag = true;
        this.SelectPlayer(playerIcon);
      }
    }
    for (int index = 0; index < ((Node) this._playerIconContainer).GetChildCount(false); ++index)
    {
      ((Node) this._playerIconContainer).GetChild<Control>(index, false).FocusNeighborLeft = index > 0 ? ((Node) ((Node) this._playerIconContainer).GetChild<Control>(index - 1, false)).GetPath() : ((Node) ((Node) this._playerIconContainer).GetChild<Control>(((Node) this._playerIconContainer).GetChildCount(false) - 1, false)).GetPath();
      ((Node) this._playerIconContainer).GetChild<Control>(index, false).FocusNeighborRight = index < ((Node) this._playerIconContainer).GetChildCount(false) - 1 ? ((Node) ((Node) this._playerIconContainer).GetChild<Control>(index + 1, false)).GetPath() : ((Node) ((Node) this._playerIconContainer).GetChild<Control>(0, false)).GetPath();
    }
    if (flag)
      return;
    if (history.Players.Count > 1)
      Log.Warn($"Local player with ID {localPlayerId} not found in multiplayer run history file! Defaulting to first player");
    this.SelectPlayer(playerIcon1);
  }

  private void SelectPlayer(NRunHistoryPlayerIcon playerIcon)
  {
    this._selectedPlayerIcon?.Deselect();
    this._selectedPlayerIcon = playerIcon;
    playerIcon.Select();
    if (this._history.Players.Count != 1)
      new LocString("run_history", "PLAYER_NAME").Add("PlayerName", PlatformUtil.GetPlayerName(this._history.PlatformType, playerIcon.Player.Id));
    UnlockState stateFromProgress = SaveManager.Instance.GenerateUnlockStateFromProgress();
    Player player = Player.CreateForNewRun(SaveUtil.CharacterOrDeprecated(playerIcon.Player.Character), stateFromProgress, playerIcon.Player.Id);
    this.LoadGoldHpAndPotionInfo(playerIcon);
    this.LoadDeathQuote(this._history, playerIcon.Player.Character);
    this.LoadBadges(this._history.Players.FirstOrDefault<RunHistoryPlayer>((Func<RunHistoryPlayer, bool>) (p => (long) p.Id == (long) player.NetId)).Badges.ToList<SerializableBadge>());
    this._mapPointHistory.SetPlayer(playerIcon.Player);
    this._relicHistory.LoadRelics(player, playerIcon.Player.Relics);
    this._deckHistory.LoadDeck(player, playerIcon.Player.Deck);
    TaskHelper.RunSafely(this.ResizeScreen());
  }

  private async Task ResizeScreen()
  {
    double num = (double) await ((Node) this).AwaitProcessFrame();
    ((Node) this._screenContents).GetNode<Control>(NodePath.op_Implicit("Content")).ResetSize();
    this._screenContents.InstantlyScrollToTop();
  }

  private void LoadGoldHpAndPotionInfo(NRunHistoryPlayerIcon icon)
  {
    if (!this._history.MapPointHistory.Any<List<MapPointHistoryEntry>>())
    {
      CharacterModel characterModel = SaveUtil.CharacterOrDeprecated(icon.Player.Character);
      this._hpLabel.SetTextAutoSize($"{characterModel.StartingHp}/{characterModel.StartingHp}");
      this._goldLabel.SetTextAutoSize($"{characterModel.StartingGold}");
    }
    else
    {
      PlayerMapPointHistoryEntry pointHistoryEntry = this._history.MapPointHistory.Last<List<MapPointHistoryEntry>>().Last<MapPointHistoryEntry>().PlayerStats.First<PlayerMapPointHistoryEntry>((Func<PlayerMapPointHistoryEntry, bool>) (stat => (long) stat.PlayerId == (long) icon.Player.Id));
      this._hpLabel.SetTextAutoSize($"{pointHistoryEntry.CurrentHp}/{pointHistoryEntry.MaxHp}");
      this._goldLabel.SetTextAutoSize($"{pointHistoryEntry.CurrentGold}");
    }
    ((Node) this._potionHolder).FreeChildren();
    RunHistoryPlayer runHistoryPlayer = this._history.Players.First<RunHistoryPlayer>((Func<RunHistoryPlayer, bool>) (player => (long) player.Id == (long) icon.Player.Id));
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    List<PotionModel> list = runHistoryPlayer.Potions.Select<SerializablePotion, PotionModel>(NRunHistory.\u003C\u003EO.\u003C0\u003E__FromSerializable ?? (NRunHistory.\u003C\u003EO.\u003C0\u003E__FromSerializable = new Func<SerializablePotion, PotionModel>(PotionModel.FromSerializable))).ToList<PotionModel>();
    List<NPotionHolder> npotionHolderList = new List<NPotionHolder>();
    for (int index = 0; index < runHistoryPlayer.MaxPotionSlotCount; ++index)
    {
      NPotionHolder child = NPotionHolder.Create(false);
      ((Node) this._potionHolder).AddChildSafely((Node) child);
      npotionHolderList.Add(child);
    }
    UnlockState stateFromProgress = SaveManager.Instance.GenerateUnlockStateFromProgress();
    Player forNewRun = Player.CreateForNewRun(SaveUtil.CharacterOrDeprecated(icon.Player.Character), stateFromProgress, icon.Player.Id);
    for (int index = 0; index < list.Count && index < runHistoryPlayer.MaxPotionSlotCount; ++index)
    {
      NPotion potion = NPotion.Create(list[index]);
      potion.Model.Owner = forNewRun;
      npotionHolderList[index].AddPotion(potion);
      potion.Position = Vector2.Zero;
    }
  }

  private void LoadPlayerFloor(RunHistory history)
  {
    this._floorLabel.SetTextAutoSize($"{history.MapPointHistory.Sum<List<MapPointHistoryEntry>>((Func<List<MapPointHistoryEntry>, int>) (rooms => rooms.Count))}");
  }

  private void LoadGameModeDetails(RunHistory history)
  {
    LocString locString = new LocString("run_history", "GAME_MODE.title");
    if (history.Players.Count > 1)
      locString.Add("PlayerCount", new LocString("run_history", "PLAYER_COUNT.multiplayer"));
    else
      locString.Add("PlayerCount", new LocString("run_history", "PLAYER_COUNT.singleplayer"));
    switch (history.GameMode)
    {
      case GameMode.Standard:
        locString.Add("GameMode", new LocString("run_history", "GAME_MODE.standard"));
        break;
      case GameMode.Daily:
        locString.Add("GameMode", new LocString("run_history", "GAME_MODE.daily"));
        break;
      case GameMode.Custom:
        locString.Add("GameMode", new LocString("run_history", "GAME_MODE.custom"));
        break;
      default:
        locString.Add("GameMode", new LocString("run_history", "GAME_MODE.unknown"));
        break;
    }
    this._gameModeLabel.Text = locString.GetFormattedText() ?? "";
  }

  public static GameOverType GetGameOverType(RunHistory history)
  {
    if (history.Win)
      return GameOverType.FalseVictory;
    if (history.WasAbandoned)
      return GameOverType.AbandonedRun;
    if (history.KilledByEncounter != ModelId.none)
      return GameOverType.CombatDeath;
    if (history.KilledByEvent != ModelId.none)
      return GameOverType.EventDeath;
    Log.Warn("How did the game end??");
    return GameOverType.None;
  }

  public static string GetDeathQuote(
    RunHistory history,
    ModelId characterId,
    GameOverType gameOverType)
  {
    CharacterModel character = SaveUtil.CharacterOrDeprecated(characterId);
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(NRunHistory._leftQuote.GetRawText());
    Rng rng = new Rng(StringHelper.GetDeterministicHashCode(history.Seed));
    switch (gameOverType)
    {
      case GameOverType.None:
      case GameOverType.TrueVictory:
        LocString deathDebugMessage = NRunHistory.GetDeathDebugMessage();
        character.AddDetailsTo(deathDebugMessage);
        stringBuilder.Append(deathDebugMessage.GetFormattedText());
        break;
      case GameOverType.AbandonedRun:
        LocString randomWithPrefix1 = LocString.GetRandomWithPrefix("run_history", "MAP_POINT_HISTORY.abandon", rng);
        character.AddDetailsTo(randomWithPrefix1);
        stringBuilder.Append(randomWithPrefix1.GetFormattedText());
        break;
      case GameOverType.EventDeath:
        EventModel eventModel;
        try
        {
          eventModel = ModelDb.GetById<EventModel>(history.KilledByEvent);
        }
        catch (ModelNotFoundException ex)
        {
          eventModel = (EventModel) ModelDb.Event<DeprecatedEvent>();
        }
        LocString str = LocString.GetIfExists(eventModel.LocTable, eventModel.Id.Entry + ".loss") ?? NRunHistory.GetDeathDebugMessage();
        character.AddDetailsTo(str);
        str.Add("event", eventModel.Title);
        stringBuilder.Append(str.GetFormattedText());
        break;
      case GameOverType.CombatDeath:
        LocString lossMessageFor = SaveUtil.EncounterOrDeprecated(history.KilledByEncounter).GetLossMessageFor(character);
        stringBuilder.Append(lossMessageFor.GetFormattedText());
        break;
      case GameOverType.FalseVictory:
        LocString randomWithPrefix2 = LocString.GetRandomWithPrefix("run_history", "MAP_POINT_HISTORY.falseVictory", rng);
        character.AddDetailsTo(randomWithPrefix2);
        stringBuilder.Append(randomWithPrefix2.GetFormattedText());
        break;
      default:
        Log.Error("Unimplemented GameOverType: " + gameOverType.ToString());
        throw new ArgumentOutOfRangeException(nameof (gameOverType), (object) gameOverType, (string) null);
    }
    stringBuilder.Append(NRunHistory._rightQuote.GetRawText());
    return stringBuilder.ToString();
  }

  private void LoadDeathQuote(RunHistory history, ModelId characterId)
  {
    CharacterModel character = SaveUtil.CharacterOrDeprecated(characterId);
    StringBuilder stringBuilder1 = new StringBuilder();
    stringBuilder1.Append(NRunHistory._leftQuote.GetRawText());
    Rng rng = new Rng(StringHelper.GetDeterministicHashCode(history.Seed));
    if (history.Win)
    {
      ((Control) this._deathQuoteLabel).AddThemeColorOverride(ThemeConstants.RichTextLabel.DefaultColor, StsColors.green);
      LocString randomWithPrefix = LocString.GetRandomWithPrefix("run_history", "MAP_POINT_HISTORY.falseVictory", rng);
      character.AddDetailsTo(randomWithPrefix);
      StringBuilder stringBuilder2 = stringBuilder1;
      StringBuilder stringBuilder3 = stringBuilder2;
      StringBuilder.AppendInterpolatedStringHandler interpolatedStringHandler;
      // ISSUE: explicit constructor call
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(0, 1, stringBuilder2);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(randomWithPrefix.GetFormattedText());
      ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
      stringBuilder3.Append(ref local);
    }
    else if (history.WasAbandoned)
    {
      ((Control) this._deathQuoteLabel).AddThemeColorOverride(ThemeConstants.RichTextLabel.DefaultColor, StsColors.red);
      LocString randomWithPrefix = LocString.GetRandomWithPrefix("run_history", "MAP_POINT_HISTORY.abandon", rng);
      character.AddDetailsTo(randomWithPrefix);
      stringBuilder1.Append(randomWithPrefix.GetFormattedText());
    }
    else if (history.KilledByEncounter != ModelId.none)
    {
      ((Control) this._deathQuoteLabel).AddThemeColorOverride(ThemeConstants.RichTextLabel.DefaultColor, StsColors.red);
      LocString lossMessageFor = SaveUtil.EncounterOrDeprecated(history.KilledByEncounter).GetLossMessageFor(character);
      StringBuilder stringBuilder4 = stringBuilder1;
      StringBuilder stringBuilder5 = stringBuilder4;
      StringBuilder.AppendInterpolatedStringHandler interpolatedStringHandler;
      // ISSUE: explicit constructor call
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(0, 1, stringBuilder4);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(lossMessageFor.GetFormattedText());
      ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
      stringBuilder5.Append(ref local);
    }
    else if (history.KilledByEvent != ModelId.none)
    {
      ((Control) this._deathQuoteLabel).AddThemeColorOverride(ThemeConstants.RichTextLabel.DefaultColor, StsColors.red);
      EventModel eventModel;
      try
      {
        eventModel = ModelDb.GetById<EventModel>(history.KilledByEvent);
      }
      catch (ModelNotFoundException ex)
      {
        eventModel = (EventModel) ModelDb.Event<DeprecatedEvent>();
      }
      string str1 = eventModel.Id.Entry + ".loss";
      LocString str2 = !LocString.Exists("events", str1) ? new LocString("run_history", "DEFAULT_EVENT_LOSS_MESSAGE") : new LocString("events", str1);
      character.AddDetailsTo(str2);
      str2.Add("event", eventModel.Title);
      StringBuilder stringBuilder6 = stringBuilder1;
      StringBuilder stringBuilder7 = stringBuilder6;
      StringBuilder.AppendInterpolatedStringHandler interpolatedStringHandler;
      // ISSUE: explicit constructor call
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(0, 1, stringBuilder6);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(str2.GetFormattedText());
      ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
      stringBuilder7.Append(ref local);
    }
    stringBuilder1.Append(NRunHistory._rightQuote.GetRawText());
    this._deathQuoteLabel.Text = stringBuilder1.ToString();
  }

  private static LocString GetDeathDebugMessage()
  {
    return new LocString("run_history", "MAP_POINT_HISTORY.debug");
  }

  private void LoadBadges(List<SerializableBadge> badges)
  {
    if (badges.Count == 0)
    {
      ((CanvasItem) this._badgeContainer).Visible = false;
    }
    else
    {
      ((Node) this._badgeContainer).FreeChildren();
      ((CanvasItem) this._badgeContainer).Visible = true;
      foreach (SerializableBadge badge in badges)
      {
        NBadge child = NBadge.Create(badge.Id, badge.Rarity);
        if (child != null)
        {
          ((Node) this._badgeContainer).AddChildSafely((Node) child);
          ((CanvasItem) child).Modulate = Colors.White;
        }
      }
    }
  }

  private void LoadTimeDetails(RunHistory history)
  {
    DateTimeFormatInfo dateTimeFormat = LocManager.Instance.CultureInfo.DateTimeFormat;
    DateTime dateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTimeOffset.FromUnixTimeSeconds(history.StartTime).UtcDateTime, TimeZoneInfo.Local);
    string variable1 = dateTime.ToString(this._dateFormat.GetRawText(), (IFormatProvider) dateTimeFormat);
    string variable2 = dateTime.ToString(this._timeFormat.GetRawText(), (IFormatProvider) dateTimeFormat);
    this._dateTimeLocString.Add("Date", variable1);
    this._dateTimeLocString.Add("Time", variable2);
    this._seedLocString.Add("Seed", history.Seed);
    this._dateLabel.Text = this._dateTimeLocString.GetFormattedText();
    this._seedLabel.Text = this._seedLocString.GetFormattedText();
    this._buildLabel.Text = history.BuildId;
    this._timeLabel.SetTextAutoSize(TimeFormatting.Format(history.RunTime));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(11)
    {
      new MethodInfo(NRunHistory.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRunHistory.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRunHistory.MethodName.OnLeftButtonButtonReleased, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NRunHistory.MethodName.OnRightButtonButtonReleased, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NRunHistory.MethodName.CanBeShown, new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRunHistory.MethodName.OnSubmenuOpened, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRunHistory.MethodName.OnSubmenuShown, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRunHistory.MethodName.OnSubmenuHidden, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRunHistory.MethodName._Input, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NRunHistory.MethodName.SelectPlayer, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("playerIcon"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NRunHistory.MethodName.LoadGoldHpAndPotionInfo, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("icon"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NRunHistory.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NRunHistory nrunHistory = NRunHistory.Create();
      ret = VariantUtils.CreateFrom<NRunHistory>(ref nrunHistory);
      return true;
    }
    if (StringName.op_Equality(ref method, NRunHistory.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRunHistory.MethodName.OnLeftButtonButtonReleased) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnLeftButtonButtonReleased(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRunHistory.MethodName.OnRightButtonButtonReleased) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnRightButtonButtonReleased(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRunHistory.MethodName.CanBeShown) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      bool flag = NRunHistory.CanBeShown();
      ret = VariantUtils.CreateFrom<bool>(ref flag);
      return true;
    }
    if (StringName.op_Equality(ref method, NRunHistory.MethodName.OnSubmenuOpened) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSubmenuOpened();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRunHistory.MethodName.OnSubmenuShown) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSubmenuShown();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRunHistory.MethodName.OnSubmenuHidden) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSubmenuHidden();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRunHistory.MethodName._Input) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Input(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRunHistory.MethodName.SelectPlayer) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SelectPlayer(VariantUtils.ConvertTo<NRunHistoryPlayerIcon>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NRunHistory.MethodName.LoadGoldHpAndPotionInfo) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.LoadGoldHpAndPotionInfo(VariantUtils.ConvertTo<NRunHistoryPlayerIcon>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NRunHistory.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NRunHistory nrunHistory = NRunHistory.Create();
      ret = VariantUtils.CreateFrom<NRunHistory>(ref nrunHistory);
      return true;
    }
    if (StringName.op_Equality(ref method, NRunHistory.MethodName.CanBeShown) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      bool flag = NRunHistory.CanBeShown();
      ret = VariantUtils.CreateFrom<bool>(ref flag);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NRunHistory.MethodName.Create) || StringName.op_Equality(ref method, NRunHistory.MethodName._Ready) || StringName.op_Equality(ref method, NRunHistory.MethodName.OnLeftButtonButtonReleased) || StringName.op_Equality(ref method, NRunHistory.MethodName.OnRightButtonButtonReleased) || StringName.op_Equality(ref method, NRunHistory.MethodName.CanBeShown) || StringName.op_Equality(ref method, NRunHistory.MethodName.OnSubmenuOpened) || StringName.op_Equality(ref method, NRunHistory.MethodName.OnSubmenuShown) || StringName.op_Equality(ref method, NRunHistory.MethodName.OnSubmenuHidden) || StringName.op_Equality(ref method, NRunHistory.MethodName._Input) || StringName.op_Equality(ref method, NRunHistory.MethodName.SelectPlayer) || StringName.op_Equality(ref method, NRunHistory.MethodName.LoadGoldHpAndPotionInfo) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRunHistory.PropertyName._screenContents))
    {
      this._screenContents = VariantUtils.ConvertTo<NScrollableContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistory.PropertyName._playerIconContainer))
    {
      this._playerIconContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistory.PropertyName._hpLabel))
    {
      this._hpLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistory.PropertyName._goldLabel))
    {
      this._goldLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistory.PropertyName._potionHolder))
    {
      this._potionHolder = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistory.PropertyName._floorLabel))
    {
      this._floorLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistory.PropertyName._timeLabel))
    {
      this._timeLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistory.PropertyName._dateLabel))
    {
      this._dateLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistory.PropertyName._seedLabel))
    {
      this._seedLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistory.PropertyName._gameModeLabel))
    {
      this._gameModeLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistory.PropertyName._buildLabel))
    {
      this._buildLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistory.PropertyName._badgeContainer))
    {
      this._badgeContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistory.PropertyName._deathQuoteLabel))
    {
      this._deathQuoteLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistory.PropertyName._mapPointHistory))
    {
      this._mapPointHistory = VariantUtils.ConvertTo<NMapPointHistory>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistory.PropertyName._relicHistory))
    {
      this._relicHistory = VariantUtils.ConvertTo<NRelicHistory>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistory.PropertyName._deckHistory))
    {
      this._deckHistory = VariantUtils.ConvertTo<NDeckHistory>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistory.PropertyName._outOfDateVisual))
    {
      this._outOfDateVisual = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistory.PropertyName._index))
    {
      this._index = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistory.PropertyName._prevButton))
    {
      this._prevButton = VariantUtils.ConvertTo<NRunHistoryArrowButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistory.PropertyName._nextButton))
    {
      this._nextButton = VariantUtils.ConvertTo<NRunHistoryArrowButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistory.PropertyName._selectedPlayerIcon))
    {
      this._selectedPlayerIcon = VariantUtils.ConvertTo<NRunHistoryPlayerIcon>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRunHistory.PropertyName._screenTween))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._screenTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRunHistory.PropertyName.InitialFocusedControl))
    {
      ref godot_variant local = ref value;
      Control initialFocusedControl = this.InitialFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref initialFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistory.PropertyName._screenContents))
    {
      value = VariantUtils.CreateFrom<NScrollableContainer>(ref this._screenContents);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistory.PropertyName._playerIconContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._playerIconContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistory.PropertyName._hpLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._hpLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistory.PropertyName._goldLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._goldLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistory.PropertyName._potionHolder))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._potionHolder);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistory.PropertyName._floorLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._floorLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistory.PropertyName._timeLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._timeLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistory.PropertyName._dateLabel))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._dateLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistory.PropertyName._seedLabel))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._seedLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistory.PropertyName._gameModeLabel))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._gameModeLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistory.PropertyName._buildLabel))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._buildLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistory.PropertyName._badgeContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._badgeContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistory.PropertyName._deathQuoteLabel))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._deathQuoteLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistory.PropertyName._mapPointHistory))
    {
      value = VariantUtils.CreateFrom<NMapPointHistory>(ref this._mapPointHistory);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistory.PropertyName._relicHistory))
    {
      value = VariantUtils.CreateFrom<NRelicHistory>(ref this._relicHistory);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistory.PropertyName._deckHistory))
    {
      value = VariantUtils.CreateFrom<NDeckHistory>(ref this._deckHistory);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistory.PropertyName._outOfDateVisual))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._outOfDateVisual);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistory.PropertyName._index))
    {
      value = VariantUtils.CreateFrom<int>(ref this._index);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistory.PropertyName._prevButton))
    {
      value = VariantUtils.CreateFrom<NRunHistoryArrowButton>(ref this._prevButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistory.PropertyName._nextButton))
    {
      value = VariantUtils.CreateFrom<NRunHistoryArrowButton>(ref this._nextButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NRunHistory.PropertyName._selectedPlayerIcon))
    {
      value = VariantUtils.CreateFrom<NRunHistoryPlayerIcon>(ref this._selectedPlayerIcon);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRunHistory.PropertyName._screenTween))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Tween>(ref this._screenTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NRunHistory.PropertyName.InitialFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRunHistory.PropertyName._screenContents, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRunHistory.PropertyName._playerIconContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRunHistory.PropertyName._hpLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRunHistory.PropertyName._goldLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRunHistory.PropertyName._potionHolder, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRunHistory.PropertyName._floorLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRunHistory.PropertyName._timeLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRunHistory.PropertyName._dateLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRunHistory.PropertyName._seedLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRunHistory.PropertyName._gameModeLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRunHistory.PropertyName._buildLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRunHistory.PropertyName._badgeContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRunHistory.PropertyName._deathQuoteLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRunHistory.PropertyName._mapPointHistory, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRunHistory.PropertyName._relicHistory, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRunHistory.PropertyName._deckHistory, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRunHistory.PropertyName._outOfDateVisual, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NRunHistory.PropertyName._index, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRunHistory.PropertyName._prevButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRunHistory.PropertyName._nextButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRunHistory.PropertyName._selectedPlayerIcon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRunHistory.PropertyName._screenTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NRunHistory.PropertyName._screenContents, Variant.From<NScrollableContainer>(ref this._screenContents));
    info.AddProperty(NRunHistory.PropertyName._playerIconContainer, Variant.From<Control>(ref this._playerIconContainer));
    info.AddProperty(NRunHistory.PropertyName._hpLabel, Variant.From<MegaLabel>(ref this._hpLabel));
    info.AddProperty(NRunHistory.PropertyName._goldLabel, Variant.From<MegaLabel>(ref this._goldLabel));
    info.AddProperty(NRunHistory.PropertyName._potionHolder, Variant.From<Control>(ref this._potionHolder));
    info.AddProperty(NRunHistory.PropertyName._floorLabel, Variant.From<MegaLabel>(ref this._floorLabel));
    info.AddProperty(NRunHistory.PropertyName._timeLabel, Variant.From<MegaLabel>(ref this._timeLabel));
    info.AddProperty(NRunHistory.PropertyName._dateLabel, Variant.From<MegaRichTextLabel>(ref this._dateLabel));
    info.AddProperty(NRunHistory.PropertyName._seedLabel, Variant.From<MegaRichTextLabel>(ref this._seedLabel));
    info.AddProperty(NRunHistory.PropertyName._gameModeLabel, Variant.From<MegaRichTextLabel>(ref this._gameModeLabel));
    info.AddProperty(NRunHistory.PropertyName._buildLabel, Variant.From<MegaRichTextLabel>(ref this._buildLabel));
    info.AddProperty(NRunHistory.PropertyName._badgeContainer, Variant.From<Control>(ref this._badgeContainer));
    info.AddProperty(NRunHistory.PropertyName._deathQuoteLabel, Variant.From<MegaRichTextLabel>(ref this._deathQuoteLabel));
    info.AddProperty(NRunHistory.PropertyName._mapPointHistory, Variant.From<NMapPointHistory>(ref this._mapPointHistory));
    info.AddProperty(NRunHistory.PropertyName._relicHistory, Variant.From<NRelicHistory>(ref this._relicHistory));
    info.AddProperty(NRunHistory.PropertyName._deckHistory, Variant.From<NDeckHistory>(ref this._deckHistory));
    info.AddProperty(NRunHistory.PropertyName._outOfDateVisual, Variant.From<Control>(ref this._outOfDateVisual));
    info.AddProperty(NRunHistory.PropertyName._index, Variant.From<int>(ref this._index));
    info.AddProperty(NRunHistory.PropertyName._prevButton, Variant.From<NRunHistoryArrowButton>(ref this._prevButton));
    info.AddProperty(NRunHistory.PropertyName._nextButton, Variant.From<NRunHistoryArrowButton>(ref this._nextButton));
    info.AddProperty(NRunHistory.PropertyName._selectedPlayerIcon, Variant.From<NRunHistoryPlayerIcon>(ref this._selectedPlayerIcon));
    info.AddProperty(NRunHistory.PropertyName._screenTween, Variant.From<Tween>(ref this._screenTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NRunHistory.PropertyName._screenContents, ref variant1))
      this._screenContents = ((Variant) ref variant1).As<NScrollableContainer>();
    Variant variant2;
    if (info.TryGetProperty(NRunHistory.PropertyName._playerIconContainer, ref variant2))
      this._playerIconContainer = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NRunHistory.PropertyName._hpLabel, ref variant3))
      this._hpLabel = ((Variant) ref variant3).As<MegaLabel>();
    Variant variant4;
    if (info.TryGetProperty(NRunHistory.PropertyName._goldLabel, ref variant4))
      this._goldLabel = ((Variant) ref variant4).As<MegaLabel>();
    Variant variant5;
    if (info.TryGetProperty(NRunHistory.PropertyName._potionHolder, ref variant5))
      this._potionHolder = ((Variant) ref variant5).As<Control>();
    Variant variant6;
    if (info.TryGetProperty(NRunHistory.PropertyName._floorLabel, ref variant6))
      this._floorLabel = ((Variant) ref variant6).As<MegaLabel>();
    Variant variant7;
    if (info.TryGetProperty(NRunHistory.PropertyName._timeLabel, ref variant7))
      this._timeLabel = ((Variant) ref variant7).As<MegaLabel>();
    Variant variant8;
    if (info.TryGetProperty(NRunHistory.PropertyName._dateLabel, ref variant8))
      this._dateLabel = ((Variant) ref variant8).As<MegaRichTextLabel>();
    Variant variant9;
    if (info.TryGetProperty(NRunHistory.PropertyName._seedLabel, ref variant9))
      this._seedLabel = ((Variant) ref variant9).As<MegaRichTextLabel>();
    Variant variant10;
    if (info.TryGetProperty(NRunHistory.PropertyName._gameModeLabel, ref variant10))
      this._gameModeLabel = ((Variant) ref variant10).As<MegaRichTextLabel>();
    Variant variant11;
    if (info.TryGetProperty(NRunHistory.PropertyName._buildLabel, ref variant11))
      this._buildLabel = ((Variant) ref variant11).As<MegaRichTextLabel>();
    Variant variant12;
    if (info.TryGetProperty(NRunHistory.PropertyName._badgeContainer, ref variant12))
      this._badgeContainer = ((Variant) ref variant12).As<Control>();
    Variant variant13;
    if (info.TryGetProperty(NRunHistory.PropertyName._deathQuoteLabel, ref variant13))
      this._deathQuoteLabel = ((Variant) ref variant13).As<MegaRichTextLabel>();
    Variant variant14;
    if (info.TryGetProperty(NRunHistory.PropertyName._mapPointHistory, ref variant14))
      this._mapPointHistory = ((Variant) ref variant14).As<NMapPointHistory>();
    Variant variant15;
    if (info.TryGetProperty(NRunHistory.PropertyName._relicHistory, ref variant15))
      this._relicHistory = ((Variant) ref variant15).As<NRelicHistory>();
    Variant variant16;
    if (info.TryGetProperty(NRunHistory.PropertyName._deckHistory, ref variant16))
      this._deckHistory = ((Variant) ref variant16).As<NDeckHistory>();
    Variant variant17;
    if (info.TryGetProperty(NRunHistory.PropertyName._outOfDateVisual, ref variant17))
      this._outOfDateVisual = ((Variant) ref variant17).As<Control>();
    Variant variant18;
    if (info.TryGetProperty(NRunHistory.PropertyName._index, ref variant18))
      this._index = ((Variant) ref variant18).As<int>();
    Variant variant19;
    if (info.TryGetProperty(NRunHistory.PropertyName._prevButton, ref variant19))
      this._prevButton = ((Variant) ref variant19).As<NRunHistoryArrowButton>();
    Variant variant20;
    if (info.TryGetProperty(NRunHistory.PropertyName._nextButton, ref variant20))
      this._nextButton = ((Variant) ref variant20).As<NRunHistoryArrowButton>();
    Variant variant21;
    if (info.TryGetProperty(NRunHistory.PropertyName._selectedPlayerIcon, ref variant21))
      this._selectedPlayerIcon = ((Variant) ref variant21).As<NRunHistoryPlayerIcon>();
    Variant variant22;
    if (!info.TryGetProperty(NRunHistory.PropertyName._screenTween, ref variant22))
      return;
    this._screenTween = ((Variant) ref variant22).As<Tween>();
  }

  public new class MethodName : NSubmenu.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnLeftButtonButtonReleased = StringName.op_Implicit(nameof (OnLeftButtonButtonReleased));
    public static readonly StringName OnRightButtonButtonReleased = StringName.op_Implicit(nameof (OnRightButtonButtonReleased));
    public static readonly StringName CanBeShown = StringName.op_Implicit(nameof (CanBeShown));
    public new static readonly StringName OnSubmenuOpened = StringName.op_Implicit(nameof (OnSubmenuOpened));
    public new static readonly StringName OnSubmenuShown = StringName.op_Implicit(nameof (OnSubmenuShown));
    public new static readonly StringName OnSubmenuHidden = StringName.op_Implicit(nameof (OnSubmenuHidden));
    public static readonly StringName _Input = StringName.op_Implicit(nameof (_Input));
    public static readonly StringName SelectPlayer = StringName.op_Implicit(nameof (SelectPlayer));
    public static readonly StringName LoadGoldHpAndPotionInfo = StringName.op_Implicit(nameof (LoadGoldHpAndPotionInfo));
  }

  public new class PropertyName : NSubmenu.PropertyName
  {
    public new static readonly StringName InitialFocusedControl = StringName.op_Implicit(nameof (InitialFocusedControl));
    public static readonly StringName _screenContents = StringName.op_Implicit(nameof (_screenContents));
    public static readonly StringName _playerIconContainer = StringName.op_Implicit(nameof (_playerIconContainer));
    public static readonly StringName _hpLabel = StringName.op_Implicit(nameof (_hpLabel));
    public static readonly StringName _goldLabel = StringName.op_Implicit(nameof (_goldLabel));
    public static readonly StringName _potionHolder = StringName.op_Implicit(nameof (_potionHolder));
    public static readonly StringName _floorLabel = StringName.op_Implicit(nameof (_floorLabel));
    public static readonly StringName _timeLabel = StringName.op_Implicit(nameof (_timeLabel));
    public static readonly StringName _dateLabel = StringName.op_Implicit(nameof (_dateLabel));
    public static readonly StringName _seedLabel = StringName.op_Implicit(nameof (_seedLabel));
    public static readonly StringName _gameModeLabel = StringName.op_Implicit(nameof (_gameModeLabel));
    public static readonly StringName _buildLabel = StringName.op_Implicit(nameof (_buildLabel));
    public static readonly StringName _badgeContainer = StringName.op_Implicit(nameof (_badgeContainer));
    public static readonly StringName _deathQuoteLabel = StringName.op_Implicit(nameof (_deathQuoteLabel));
    public static readonly StringName _mapPointHistory = StringName.op_Implicit(nameof (_mapPointHistory));
    public static readonly StringName _relicHistory = StringName.op_Implicit(nameof (_relicHistory));
    public static readonly StringName _deckHistory = StringName.op_Implicit(nameof (_deckHistory));
    public static readonly StringName _outOfDateVisual = StringName.op_Implicit(nameof (_outOfDateVisual));
    public static readonly StringName _index = StringName.op_Implicit(nameof (_index));
    public static readonly StringName _prevButton = StringName.op_Implicit(nameof (_prevButton));
    public static readonly StringName _nextButton = StringName.op_Implicit(nameof (_nextButton));
    public static readonly StringName _selectedPlayerIcon = StringName.op_Implicit(nameof (_selectedPlayerIcon));
    public static readonly StringName _screenTween = StringName.op_Implicit(nameof (_screenTween));
  }

  public new class SignalName : NSubmenu.SignalName
  {
  }
}
