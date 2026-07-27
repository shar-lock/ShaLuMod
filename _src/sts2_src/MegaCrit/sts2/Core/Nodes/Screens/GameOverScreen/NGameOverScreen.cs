// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.GameOverScreen.NGameOverScreen
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
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Badges;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.RestSite;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.DailyRun;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Nodes.Screens.RunHistoryScreen;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Nodes.Screens.Shops;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.Timeline;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.GameOverScreen;

[ScriptPath("res://src/Core/Nodes/Screens/GameOverScreen/NGameOverScreen.cs")]
public class NGameOverScreen : NClickableControl, IOverlayScreen, IScreenContext
{
  private static readonly StringName _threshold = new StringName("threshold");
  private RunState _runState;
  private SerializableRun _serializableRun;
  private RunHistory _history;
  private Player _localPlayer;
  private NGameOverContinueButton _continueButton;
  private NViewRunButton _viewRunButton;
  private NReturnToMainMenuButton _mainMenuButton;
  private NGameOverContinueButton _leaderboardButton;
  private Control _badgeContainer;
  private GridContainer _scoreLineContainer;
  private readonly List<NScoreLine> _scoreLines = new List<NScoreLine>();
  private Control _scoreBar;
  private Control _scoreFg;
  private MegaLabel _scoreProgress;
  private MegaLabel _unlocksRemaining;
  private int _score;
  private int _scoreThreshold;
  private string? _scoreUnlockedEpochId;
  private NDailyRunLeaderboard _leaderboard;
  private Control _creatureContainer;
  private NRunSummary _summaryContainer;
  private ColorRect _fullBlackBackstop;
  private ColorRect _summaryBackstop;
  private ColorRect _backstop;
  private NCommonBanner _banner;
  private MegaRichTextLabel _deathQuote;
  private MegaRichTextLabel _victoryDamageLabel;
  private Control _uiNode;
  private Control _screenshakeContainer;
  private MegaLabel _discoveryLabel;
  private string _encounterQuote;
  private bool _isAnimatingSummary;
  private ShaderMaterial _backstopMaterial;
  private Tween? _quoteTween;
  private readonly CancellationTokenSource _cts = new CancellationTokenSource();

  private static string ScenePath => SceneHelper.GetScenePath("screens/game_over_screen");

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NGameOverScreen.ScenePath);
    }
  }

  public NetScreenType ScreenType => NetScreenType.GameOver;

  public override void _Ready()
  {
    AbstractRoom currentRoom = this._runState.CurrentRoom;
    bool flag = currentRoom != null && currentRoom.IsVictoryRoom;
    RunHistory runHistory = RunManager.Instance.History;
    if (runHistory == null)
      runHistory = new RunHistory() { Win = flag };
    this._history = runHistory;
    this._score = ScoreUtility.CalculateScore(this._serializableRun, this._history.Win);
    this._uiNode = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Ui"));
    this._continueButton = ((Node) this).GetNode<NGameOverContinueButton>(NodePath.op_Implicit("%ContinueButton"));
    ((GodotObject) this._continueButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OpenSummaryScreen)), 0U);
    this._continueButton.Disable();
    this._viewRunButton = ((Node) this).GetNode<NViewRunButton>(NodePath.op_Implicit("%ViewRunButton"));
    ((GodotObject) this._viewRunButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OpenRunHistoryScreen)), 0U);
    this._mainMenuButton = ((Node) this).GetNode<NReturnToMainMenuButton>(NodePath.op_Implicit("%MainMenuButton"));
    ((GodotObject) this._mainMenuButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnMainMenuButtonPressed)), 0U);
    this._scoreLineContainer = ((Node) this).GetNode<GridContainer>(NodePath.op_Implicit("%ScoreLineContainer"));
    this._badgeContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%BadgeContainer"));
    this._scoreBar = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%ScoreBar"));
    this._scoreFg = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%ScoreFg"));
    this._scoreProgress = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%ScoreProgress"));
    this._unlocksRemaining = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%UnlocksRemaining"));
    this._screenshakeContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%ScreenshakeContainer"));
    this._leaderboardButton = ((Node) this).GetNode<NGameOverContinueButton>(NodePath.op_Implicit("%LeaderboardButton"));
    ((GodotObject) this._leaderboardButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.ShowLeaderboard)), 0U);
    this._creatureContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%CreatureContainer"));
    this._summaryContainer = ((Node) this).GetNode<NRunSummary>(NodePath.op_Implicit("%RunSummaryContainer"));
    this._backstop = ((Node) this).GetNode<ColorRect>(NodePath.op_Implicit("%Backstop"));
    this._fullBlackBackstop = ((Node) this).GetNode<ColorRect>(NodePath.op_Implicit("%FullBlackBackstop"));
    this._backstopMaterial = (ShaderMaterial) ((CanvasItem) this._backstop).Material;
    this._summaryBackstop = ((Node) this).GetNode<ColorRect>(NodePath.op_Implicit("%SummaryBackstop"));
    this._leaderboard = ((Node) this).GetNode<NDailyRunLeaderboard>(NodePath.op_Implicit("%DailyRunLeaderboard"));
    this._banner = ((Node) this).GetNode<NCommonBanner>(NodePath.op_Implicit("%Banner"));
    this._victoryDamageLabel = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%VictoryDamageLabel"));
    this._discoveryLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%DiscoveryLabel"));
    this._discoveryLabel.SetTextAutoSize(new LocString("game_over_screen", "DISCOVERY_HEADER").GetFormattedText());
    this._deathQuote = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%DeathQuoteLabel"));
    this.InitializeBannerAndQuote();
    this._leaderboardButton.Disable();
    this._viewRunButton.Disable();
    this._mainMenuButton.Disable();
    ((CanvasItem) this._leaderboard).Visible = false;
  }

  public override void _ExitTree() => this._cts.Cancel();

  private bool DiscoveredAnyEpochs() => this._localPlayer.DiscoveredEpochs.Count > 0;

  private void InitializeBannerAndQuote()
  {
    ModelId id = this._localPlayer.Character.Id;
    if (this._history.Win)
    {
      this._banner.label.SetTextAutoSize(new LocString("game_over_screen", "BANNER.falseWin").GetRawText());
      this._deathQuote.Text = string.Empty;
      long personalArchitectDamage = StatsManager.GetPersonalArchitectDamage();
      long? globalArchitectDamage = StatsManager.GetGlobalArchitectDamage();
      StringBuilder stringBuilder = new StringBuilder();
      LocString locString1;
      if (globalArchitectDamage.HasValue)
      {
        locString1 = new LocString("game_over_screen", "VICTORY_DAMAGE");
        locString1.Add("TotalDamage", (Decimal) globalArchitectDamage.Value);
      }
      else
        locString1 = new LocString("game_over_screen", "VICTORY_DAMAGE_LOCAL");
      locString1.Add("PlayerDamage", (Decimal) this._score);
      locString1.Add("PersonalDamage", (Decimal) personalArchitectDamage);
      stringBuilder.Append(locString1.GetFormattedText());
      int ascensionLevel = this._runState.AscensionLevel;
      if (ascensionLevel < 10 && ascensionLevel > 0 && this._runState.AscensionLevel >= this._localPlayer.MaxAscensionWhenRunStarted && this._runState.GameMode == GameMode.Standard)
      {
        stringBuilder.Append("\n\n");
        LocString locString2 = new LocString("game_over_screen", "VICTORY_UNLOCKED_ASCENSION");
        locString2.Add("AscensionLevel", (Decimal) (this._runState.AscensionLevel + 1));
        stringBuilder.Append(locString2.GetFormattedText());
      }
      this._victoryDamageLabel.Text = stringBuilder.ToString();
    }
    else
    {
      LocTable table = LocManager.Instance.GetTable("game_over_screen");
      this._banner.label.SetTextAutoSize(Rng.Chaotic.NextItem<LocString>((IEnumerable<LocString>) table.GetLocStringsWithPrefix("BANNER.lose")).GetRawText());
      this._deathQuote.Text = Rng.Chaotic.NextItem<LocString>((IEnumerable<LocString>) table.GetLocStringsWithPrefix("QUOTES")).GetFormattedText();
    }
    this._encounterQuote = NRunHistory.GetDeathQuote(this._history, id, NRunHistory.GetGameOverType(this._history));
  }

  private async Task AnimateInQuote()
  {
    if ((double) ((CanvasItem) this._deathQuote).Modulate.A != 0.0)
    {
      this._quoteTween?.Kill();
      this._quoteTween = ((Node) this).CreateTween();
      this._quoteTween.TweenProperty((GodotObject) this._deathQuote, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.25);
      if (!await this._quoteTween.AwaitFinished((Node) this))
        return;
      this._deathQuote.Text = this._encounterQuote;
      this._quoteTween.Kill();
      await Cmd.Wait(1f, this._cts.Token);
    }
    if (!((Node) this).IsValid())
      return;
    this._quoteTween?.Kill();
    this._quoteTween = ((Node) this).CreateTween().SetParallel(true);
    if (this._history.Win)
    {
      this._quoteTween.TweenProperty((GodotObject) this._victoryDamageLabel, NodePath.op_Implicit("visible_ratio"), Variant.op_Implicit(1f), 2.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 1L);
      this._quoteTween.TweenProperty((GodotObject) this._victoryDamageLabel, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 2.0);
      bool flag = await this._quoteTween.AwaitFinished((Node) this);
    }
    else
    {
      this._quoteTween.TweenProperty((GodotObject) this._deathQuote, NodePath.op_Implicit("position:y"), Variant.op_Implicit(156f), 2.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L).From(Variant.op_Implicit(90f));
      this._quoteTween.TweenProperty((GodotObject) this._deathQuote, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 1.5);
    }
  }

  public static NGameOverScreen? Create(RunState runState, SerializableRun serializableRun)
  {
    if (TestMode.IsOn)
      return (NGameOverScreen) null;
    NGameOverScreen ngameOverScreen = PreloadManager.Cache.GetScene(NGameOverScreen.ScenePath).Instantiate<NGameOverScreen>((PackedScene.GenEditState) 0L);
    ngameOverScreen._runState = runState;
    ngameOverScreen._serializableRun = serializableRun;
    ngameOverScreen._localPlayer = LocalContext.GetMe((IPlayerCollection) runState);
    return ngameOverScreen;
  }

  private void OpenSummaryScreen(NButton _)
  {
    this._isAnimatingSummary = true;
    this._continueButton.Disable();
    ((CanvasItem) this._victoryDamageLabel).Visible = false;
    ((Node) this).CreateTween().TweenProperty((GodotObject) this._summaryBackstop, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.5);
    TaskHelper.RunSafely(this.AnimateInQuote());
    TaskHelper.RunSafely(this.AnimateRunSummary());
  }

  private async Task AnimateRunSummary()
  {
    ((Node) this).CreateTween().TweenProperty((GodotObject) this._banner, NodePath.op_Implicit("position:y"), Variant.op_Implicit(this._banner.Position.Y - 32f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    ((CanvasItem) this._summaryContainer).Visible = true;
    await this.AnimateScoreLines();
    await this.AnimateBadges();
    await this.AnimateScoreBar();
    await this.AnimateDiscoveries();
    for (int index = 0; index < ((Node) this._badgeContainer).GetChildCount(false); ++index)
    {
      ((Node) this._badgeContainer).GetChild<NBadge>(index, false).FocusNeighborLeft = index > 0 ? ((Node) ((Node) this._badgeContainer).GetChild<NBadge>(index - 1, false)).GetPath() : ((Node) ((Node) this._badgeContainer).GetChild<NBadge>(((Node) this._badgeContainer).GetChildCount(false) - 1, false)).GetPath();
      ((Node) this._badgeContainer).GetChild<NBadge>(index, false).FocusNeighborRight = index < ((Node) this._badgeContainer).GetChildCount(false) - 1 ? ((Node) ((Node) this._badgeContainer).GetChild<NBadge>(index + 1, false)).GetPath() : ((Node) ((Node) this._badgeContainer).GetChild<NBadge>(0, false)).GetPath();
      ((Node) this._badgeContainer).GetChild<NBadge>(index, false).FocusNeighborTop = ((Node) ((Node) this._badgeContainer).GetChild<NBadge>(index, false)).GetPath();
      ((Node) this._badgeContainer).GetChild<NBadge>(index, false).FocusNeighborBottom = this._summaryContainer.DefaultFocusedControl != null ? ((Node) this._summaryContainer.DefaultFocusedControl).GetPath() : ((Node) ((Node) this._badgeContainer).GetChild<NBadge>(index, false)).GetPath();
    }
    this._summaryContainer.SetControllerNav(((Node) this._badgeContainer).GetChildCount(false) > 0 ? ((Node) this._badgeContainer).GetChild<Control>(0, false) : (Control) null);
    if (((Node) this._badgeContainer).GetChildCount(false) > 0)
    {
      this.FocusNeighborBottom = ((Node) ((Node) this._badgeContainer).GetChild<NBadge>(0, false)).GetPath();
      this.FocusNeighborTop = ((Node) ((Node) this._badgeContainer).GetChild<NBadge>(0, false)).GetPath();
      this.FocusNeighborLeft = ((Node) ((Node) this._badgeContainer).GetChild<NBadge>(0, false)).GetPath();
      this.FocusNeighborRight = ((Node) ((Node) this._badgeContainer).GetChild<NBadge>(0, false)).GetPath();
    }
    else if (this._summaryContainer.DefaultFocusedControl != null)
    {
      this.FocusNeighborBottom = ((Node) this._summaryContainer.DefaultFocusedControl).GetPath();
      this.FocusNeighborTop = ((Node) this._summaryContainer.DefaultFocusedControl).GetPath();
      this.FocusNeighborLeft = ((Node) this._summaryContainer.DefaultFocusedControl).GetPath();
      this.FocusNeighborRight = ((Node) this._summaryContainer.DefaultFocusedControl).GetPath();
    }
    if (this._history.GameMode == GameMode.Daily)
    {
      this._leaderboard.Initialize(RunManager.Instance.DailyTime.Value, this._runState.Players.Select<Player, ulong>((Func<Player, ulong>) (p => p.NetId)), false);
      ((CanvasItem) this._leaderboardButton).Visible = true;
      this._leaderboardButton.Enable();
    }
    else
    {
      if (this.DiscoveredAnyEpochs())
        this._mainMenuButton.SetLabelForUnlock();
      ((CanvasItem) this._mainMenuButton).Visible = true;
      this._mainMenuButton.Enable();
    }
  }

  private async Task AnimateScoreLines()
  {
    this._scoreLines.Clear();
    this.AddScoreLine("SCORE_LINE.floorsClimbed", "FloorCount", this._runState.TotalFloor, $"+{ScoreUtility.GetScoreForFloor((IReadOnlyList<IReadOnlyList<MapPointHistoryEntry>>) this._serializableRun.MapPointHistory)}", "res://images/ui/game_over_screen/score_floor.png");
    this.AddScoreLine("SCORE_LINE.goldGained", "GoldAmount", this._serializableRun.MapPointHistory.SelectMany<List<MapPointHistoryEntry>, MapPointHistoryEntry>((Func<List<MapPointHistoryEntry>, IEnumerable<MapPointHistoryEntry>>) (actEntries => (IEnumerable<MapPointHistoryEntry>) actEntries)).Sum<MapPointHistoryEntry>((Func<MapPointHistoryEntry, int>) (e => e.GetEntry(this._localPlayer.NetId).GoldGained)), $"+{ScoreUtility.GetScoreForGoldGained((IReadOnlyList<IReadOnlyList<MapPointHistoryEntry>>) this._serializableRun.MapPointHistory, this._serializableRun.Players.Count)}", "res://images/ui/game_over_screen/score_gold.png");
    int elitesKilledCount = ScoreUtility.GetElitesKilledCount((IReadOnlyList<IReadOnlyList<MapPointHistoryEntry>>) this._serializableRun.MapPointHistory);
    if (elitesKilledCount > 0)
      this.AddScoreLine("SCORE_LINE.elitesKilled", "EliteCount", elitesKilledCount, $"+{ScoreUtility.GetScoreForElitesKilled(elitesKilledCount)}", "res://images/ui/game_over_screen/score_elite.png");
    int bossesSlainCount = ScoreUtility.GetBossesSlainCount((IReadOnlyList<IReadOnlyList<MapPointHistoryEntry>>) this._serializableRun.MapPointHistory, this._history.Win);
    if (bossesSlainCount > 0)
      this.AddScoreLine("SCORE_LINE.bossesSlain", "BossCount", bossesSlainCount, $"+{ScoreUtility.GetScoreForBossesSlain(bossesSlainCount)}", "res://images/ui/game_over_screen/score_boss.png");
    int ascension = this._history.Ascension;
    if (ascension > 0)
      this.AddScoreLine("SCORE_LINE.ascension", "AscensionLevel", ascension, "x" + this.GetAscensionMulti(ascension), "res://images/ui/game_over_screen/score_ascension.png");
    foreach (NScoreLine scoreLine in this._scoreLines)
      await scoreLine.AnimateIn();
    await Cmd.Wait(0.5f, this._cts.Token);
  }

  private async Task AnimateBadges()
  {
    List<Badge> badges = ScoreUtility.GetBadges(this._serializableRun, this._localPlayer.NetId, this._history.Win);
    foreach (Badge badgeModel in badges)
      ((Node) this._badgeContainer).AddChildSafely((Node) NBadge.Create(badgeModel));
    if (!this._serializableRun.GameMode.AreAchievementsAndEpochsLocked())
      this.SaveBadgesToProgress(badges);
    await Cmd.Wait(0.25f, this._cts.Token);
    foreach (NBadge nbadge in ((IEnumerable) ((Node) this._badgeContainer).GetChildren(false)).OfType<NBadge>())
      await nbadge.AnimateIn();
    await Cmd.Wait(0.5f, this._cts.Token);
  }

  private void SaveBadgesToProgress(List<Badge> badgesToSave)
  {
    CharacterStats characterStat = SaveManager.Instance.Progress.CharacterStats[this._localPlayer.Character.Id];
    foreach (Badge badge1 in badgesToSave)
    {
      Badge badge = badge1;
      BadgeStats badgeStats1 = characterStat.Badges.FirstOrDefault<BadgeStats>((Func<BadgeStats, bool>) (b => b.Id.Equals(badge.Id) && b.Rarity == badge.Rarity));
      if (badgeStats1 == null)
      {
        BadgeStats badgeStats2 = new BadgeStats()
        {
          Id = badge.Id,
          Count = 1,
          Rarity = badge.Rarity
        };
        characterStat.Badges.Add(badgeStats2);
        Log.Info("You got a new badge: " + badge.Id);
      }
      else
      {
        ++badgeStats1.Count;
        Log.Info($"You got badge: {badge.Id} again. Now you have {badgeStats1.Count}!");
      }
    }
  }

  private void AddScoreLine(
    string locEntryKey,
    string? locAmountKey = null,
    int amount = 0,
    string scoreLabel = "ERROR",
    string? iconPath = null)
  {
    LocString locString = new LocString("game_over_screen", locEntryKey);
    if (locAmountKey != null)
      locString.Add(locAmountKey, (Decimal) amount);
    Texture2D texture2D = iconPath == null ? (Texture2D) null : PreloadManager.Cache.GetTexture2D(iconPath);
    NScoreLine child = NScoreLine.Create(locString.GetFormattedText(), scoreLabel, texture2D);
    ((Node) this._scoreLineContainer).AddChildSafely((Node) child);
    this._scoreLines.Add(child);
  }

  private async Task AnimateScoreBar()
  {
    int unlocksRemaining = SaveManager.Instance.GetUnlocksRemaining();
    LocString locString1 = new LocString("game_over_screen", "SCORE.unlocksRemaining");
    locString1.Add("UnlockCount", (Decimal) unlocksRemaining);
    this._unlocksRemaining.SetTextAutoSize(locString1.GetFormattedText());
    if (unlocksRemaining > 0)
    {
      int currentScore = SaveManager.Instance.GetCurrentScore();
      this._scoreThreshold = this.GetScoreThreshold(unlocksRemaining);
      this._scoreProgress.SetTextAutoSize($"[{currentScore}/{this._scoreThreshold}]");
      this._scoreFg.Scale = new Vector2((float) currentScore / (float) this._scoreThreshold, 1f);
      Tween scoreTween = ((Node) this).CreateTween();
      scoreTween.TweenProperty((GodotObject) this._scoreBar, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.3);
      if (!await scoreTween.AwaitFinished((Node) this))
        return;
      if (currentScore + this._score >= this._scoreThreshold)
      {
        Log.Info("New Unlock, yay!");
        MegaLabel node = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%UnlockText"));
        this._scoreUnlockedEpochId = SaveManager.Instance.IncrementUnlock();
        currentScore -= this._scoreThreshold;
        int newThreshold = this.GetScoreThreshold(unlocksRemaining - 1);
        string locEntryKey = newThreshold == 0 ? "SCORE.unlockedAllMessage" : "SCORE.unlockedEpochMessage";
        node.SetTextAutoSize(new LocString("game_over_screen", locEntryKey).GetFormattedText());
        scoreTween = ((Node) this).CreateTween().SetParallel(true);
        scoreTween.TweenInterval(1.0);
        scoreTween.Chain();
        scoreTween.TweenMethod(Callable.From<int>(new Action<int>(this.TweenScore)), Variant.op_Implicit(currentScore + this._scoreThreshold), Variant.op_Implicit(this._scoreThreshold), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
        scoreTween.TweenProperty((GodotObject) this._scoreFg, NodePath.op_Implicit("scale:x"), Variant.op_Implicit(1f), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
        scoreTween.Chain();
        scoreTween.TweenCallback(Callable.From(new Action(this.PlayUnlockSfx)));
        scoreTween.TweenProperty((GodotObject) node, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.25);
        scoreTween.TweenProperty((GodotObject) node, NodePath.op_Implicit("position:y"), Variant.op_Implicit(-60f), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 11L);
        if (!await scoreTween.AwaitFinished((Node) this))
          return;
        if (this._scoreUnlockedEpochId != null && !SaveManager.Instance.IsEpochRevealed(this._scoreUnlockedEpochId))
        {
          EpochModel model = EpochModel.Get(this._scoreUnlockedEpochId);
          SaveManager.Instance.ObtainEpoch(this._scoreUnlockedEpochId);
          ((Node) NGame.Instance).AddChildSafely((Node) NGainEpochVfx.Create(model));
          this._localPlayer.DiscoveredEpochs.Add(model.Id);
          LocalContext.GetMe(this._serializableRun).DiscoveredEpochs.Add(model.Id);
        }
        LocString locString2 = new LocString("game_over_screen", "SCORE.unlocksRemaining");
        locString2.Add("UnlockCount", (Decimal) (unlocksRemaining - 1));
        this._unlocksRemaining.SetTextAutoSize(locString2.GetFormattedText());
        this._scoreThreshold = newThreshold;
        currentScore += this._score;
        if (newThreshold == 0 || currentScore == 0)
        {
          Log.Info("Player has gotten all unlocks or they've overflowed exactly 0");
          SaveManager.Instance.Progress.CurrentScore = 0;
        }
        else if (currentScore >= newThreshold)
        {
          Log.Info("Score is too awesome. Disallow double unlock.");
          scoreTween.Kill();
          scoreTween = ((Node) this).CreateTween().SetParallel(true);
          scoreTween.TweenInterval(0.5);
          scoreTween.Chain();
          scoreTween.TweenMethod(Callable.From<int>(new Action<int>(this.TweenScore)), Variant.op_Implicit(0), Variant.op_Implicit(newThreshold * 99 / 100), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
          scoreTween.TweenProperty((GodotObject) this._scoreFg, NodePath.op_Implicit("scale:x"), Variant.op_Implicit(1f), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L).From(Variant.op_Implicit(0.0f));
          if (!await scoreTween.AwaitFinished((Node) this))
            return;
          SaveManager.Instance.Progress.CurrentScore = newThreshold - 1;
        }
        else
        {
          Log.Info("Animate overflow score.");
          scoreTween.Kill();
          scoreTween = ((Node) this).CreateTween().SetParallel(true);
          scoreTween.Chain();
          scoreTween.TweenInterval(0.5);
          scoreTween.Chain();
          scoreTween.TweenMethod(Callable.From<int>(new Action<int>(this.TweenScore)), Variant.op_Implicit(0), Variant.op_Implicit(currentScore), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
          scoreTween.TweenProperty((GodotObject) this._scoreFg, NodePath.op_Implicit("scale:x"), Variant.op_Implicit((float) currentScore / (float) newThreshold), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L).From(Variant.op_Implicit(0.0f));
          if (!await scoreTween.AwaitFinished((Node) this))
            return;
          SaveManager.Instance.Progress.CurrentScore = currentScore;
        }
      }
      else
      {
        Log.Info("Not enough score to level up");
        scoreTween = ((Node) this).CreateTween().SetParallel(true);
        scoreTween.TweenInterval(0.5);
        scoreTween.TweenMethod(Callable.From<int>(new Action<int>(this.TweenScore)), Variant.op_Implicit(currentScore), Variant.op_Implicit(currentScore + this._score), 1.0);
        scoreTween.TweenProperty((GodotObject) this._scoreFg, NodePath.op_Implicit("scale:x"), Variant.op_Implicit((float) (currentScore + this._score) / (float) this._scoreThreshold), 1.0);
        SaveManager.Instance.Progress.CurrentScore += this._score;
      }
      SaveManager.Instance.SaveProgressFile();
      scoreTween = (Tween) null;
    }
    else
      Log.Info("This player has all unlocks. No action");
  }

  private void PlayUnlockSfx() => Log.Info("TODO: Play the ding unlock sfx here pls");

  private void TweenScore(int value)
  {
    this._scoreProgress.SetTextAutoSize($"[{value}/{this._scoreThreshold}]");
  }

  private int GetScoreThreshold(int unlocksRemaining)
  {
    int scoreThreshold;
    switch (18 - unlocksRemaining)
    {
      case 0:
        scoreThreshold = 200;
        break;
      case 1:
        scoreThreshold = 500;
        break;
      case 2:
        scoreThreshold = 750;
        break;
      case 3:
        scoreThreshold = 1000;
        break;
      case 4:
        scoreThreshold = 1250;
        break;
      case 5:
        scoreThreshold = 1500;
        break;
      case 6:
        scoreThreshold = 1600;
        break;
      case 7:
        scoreThreshold = 1700;
        break;
      case 8:
        scoreThreshold = 1800;
        break;
      case 9:
        scoreThreshold = 1900;
        break;
      case 10:
        scoreThreshold = 2000;
        break;
      case 11:
        scoreThreshold = 2100;
        break;
      case 12:
        scoreThreshold = 2200;
        break;
      case 13:
        scoreThreshold = 2300;
        break;
      case 14:
        scoreThreshold = 2400;
        break;
      case 15:
        scoreThreshold = 2500;
        break;
      case 16 /*0x10*/:
        scoreThreshold = 2500;
        break;
      case 17:
        scoreThreshold = 2500;
        break;
      default:
        scoreThreshold = 0;
        break;
    }
    return scoreThreshold;
  }

  private void ShowLeaderboard(NButton _)
  {
    this._banner.ChangeText(new LocString("main_menu_ui", "DAILY_RUN_MENU.LEADERBOARDS.title").GetRawText());
    Tween tween = ((Node) this).CreateTween().SetParallel(true);
    NDailyRunLeaderboard leaderboard = this._leaderboard;
    Color modulate = ((CanvasItem) this._leaderboard).Modulate;
    modulate.A = 0.0f;
    Color color = modulate;
    ((CanvasItem) leaderboard).Modulate = color;
    tween.TweenProperty((GodotObject) this._leaderboard, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.5);
    tween.TweenProperty((GodotObject) this._summaryContainer, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.5);
    tween.TweenProperty((GodotObject) this._deathQuote, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.5);
    tween.Chain().TweenCallback(Callable.From(new Action(this.HideSummary)));
    ((CanvasItem) this._leaderboard).Visible = true;
    this._leaderboardButton.Disable();
    if (this.DiscoveredAnyEpochs())
      this._mainMenuButton.SetLabelForUnlock();
    ((CanvasItem) this._mainMenuButton).Visible = true;
    this._mainMenuButton.Enable();
  }

  private void HideSummary()
  {
    ((CanvasItem) this._summaryContainer).Visible = false;
    ((CanvasItem) this._deathQuote).Visible = false;
  }

  private async Task AnimateDiscoveries()
  {
    await this._summaryContainer.AnimateInDiscoveries(this._runState, this._cts.Token);
    this._isAnimatingSummary = false;
  }

  private void OpenRunHistoryScreen(NButton _)
  {
    ((Node) this).AddChildSafely((Node) ResourceLoader.Load<PackedScene>("res://scenes/screens/run_history_screen/run_history_screen_via_game_over_screen.tscn", (string) null, (ResourceLoader.CacheMode) 1L).Instantiate<Control>((PackedScene.GenEditState) 0L));
  }

  private void OnMainMenuButtonPressed(NButton _)
  {
    if (RunManager.Instance.NetService.Type == NetGameType.Host)
      RunManager.Instance.NetService.Disconnect(NetError.QuitGameOver);
    this._mainMenuButton.Disable();
    if (this.DiscoveredAnyEpochs())
      this.OpenTimeline();
    else
      this.ReturnToMainMenu();
  }

  private void OpenTimeline() => TaskHelper.RunSafely(this.TransitionOutToTimeline());

  private void ReturnToMainMenu() => TaskHelper.RunSafely(this.TransitionOutToMainMenu());

  private async Task TransitionOutToTimeline() => await NGame.Instance.GoToTimelineAfterRun();

  private async Task TransitionOutToMainMenu() => await NGame.Instance.ReturnToMainMenuAfterRun();

  public void AfterOverlayOpened()
  {
    this.MoveCreaturesToDifferentLayerAndDisableUi();
    TaskHelper.RunSafely(this.AnimateIn());
  }

  private void MoveCreaturesToDifferentLayerAndDisableUi()
  {
    List<NCreatureVisuals> ncreatureVisualsList = new List<NCreatureVisuals>();
    List<NCreature> source;
    if (NCombatRoom.Instance != null)
    {
      if (NCombatRoom.Instance.Mode == CombatRoomMode.ActiveCombat)
        NCombatRoom.Instance.Ui.AnimOut();
      source = NCombatRoom.Instance.CreatureNodes.ToList<NCreature>();
      ncreatureVisualsList = source.Select<NCreature, NCreatureVisuals>((Func<NCreature, NCreatureVisuals>) (c => c.Visuals)).ToList<NCreatureVisuals>();
    }
    else if (NMerchantRoom.Instance != null)
    {
      source = new List<NCreature>();
      foreach (NMerchantCharacter playerVisual in (IEnumerable<NMerchantCharacter>) NMerchantRoom.Instance.PlayerVisuals)
      {
        playerVisual.PlayAnimation("die");
        ((Node) playerVisual).Reparent((Node) this._creatureContainer, true);
      }
    }
    else if (NRestSiteRoom.Instance != null)
    {
      source = new List<NCreature>();
      ncreatureVisualsList = new List<NCreatureVisuals>();
      foreach (Player player in (IEnumerable<Player>) this._runState.Players)
      {
        NCreatureVisuals visuals = player.Creature.CreateVisuals();
        ncreatureVisualsList.Add(visuals);
        ((Node) this._creatureContainer).AddChildSafely((Node) visuals);
        visuals.SpineAnimation.SetAnimation("die", false);
        NRestSiteCharacter characterForPlayer = NRestSiteRoom.Instance.GetCharacterForPlayer(player);
        visuals.GlobalPosition = characterForPlayer.GlobalPosition;
        visuals.Scale = characterForPlayer.Scale;
        ((CanvasItem) characterForPlayer).Visible = false;
        Vector2 vector2;
        // ISSUE: explicit constructor call
        ((Vector2) ref vector2).\u002Ector(100f, 100f);
        NCreatureVisuals ncreatureVisuals = visuals;
        ncreatureVisuals.Position = Vector2.op_Addition(ncreatureVisuals.Position, Vector2.op_Multiply(vector2, new Vector2((float) Math.Sign(visuals.Scale.X), (float) Math.Sign(visuals.Scale.Y))));
      }
    }
    else
    {
      source = new List<NCreature>();
      ncreatureVisualsList = new List<NCreatureVisuals>();
      foreach (Player player in (IEnumerable<Player>) this._runState.Players)
      {
        NCreatureVisuals visuals = player.Creature.CreateVisuals();
        ncreatureVisualsList.Add(visuals);
        ((Node) this._creatureContainer).AddChildSafely((Node) visuals);
        visuals.SpineAnimation.SetAnimation("die", false);
      }
      float num1 = Math.Min(250f, (this.Size.X - 200f) / (float) (ncreatureVisualsList.Count - 1));
      float num2 = (float) ((double) (ncreatureVisualsList.Count - 1) * -(double) num1 * 0.5);
      foreach (Node2D node2D in ncreatureVisualsList)
      {
        node2D.Position = Vector2.op_Addition(Vector2.op_Multiply(this._creatureContainer.Size, 0.5f), new Vector2(num2, 200f));
        num2 += num1;
      }
    }
    source.Sort((Comparison<NCreature>) ((c1, c2) => ((Node) c1).GetIndex(false).CompareTo(((Node) c2).GetIndex(false))));
    foreach (NCreature ncreature in source)
    {
      ncreature.AnimHideIntent();
      ncreature.AnimDisableUi();
    }
    foreach (Node node in ncreatureVisualsList)
      node.Reparent((Node) this._creatureContainer, true);
  }

  private async Task AnimateIn()
  {
    Tween backstopTween = ((Node) this).CreateTween();
    ((CanvasItem) this._uiNode).Modulate = StsColors.transparentWhite;
    if (NEventRoom.Instance != null)
    {
      ColorRect fullBlackBackstop = this._fullBlackBackstop;
      Color modulate1 = ((CanvasItem) this._fullBlackBackstop).Modulate;
      modulate1.A = 0.0f;
      Color color1 = modulate1;
      ((CanvasItem) fullBlackBackstop).Modulate = color1;
      ((CanvasItem) this._fullBlackBackstop).Visible = true;
      backstopTween.TweenProperty((GodotObject) this._fullBlackBackstop, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.2);
      foreach (NCreatureVisuals ncreatureVisuals1 in ((IEnumerable) ((Node) this._creatureContainer).GetChildren(false)).OfType<NCreatureVisuals>())
      {
        NCreatureVisuals ncreatureVisuals2 = ncreatureVisuals1;
        Color modulate2 = ((CanvasItem) ncreatureVisuals1).Modulate;
        modulate2.A = 0.0f;
        Color color2 = modulate2;
        ((CanvasItem) ncreatureVisuals2).Modulate = color2;
        backstopTween.Parallel().TweenProperty((GodotObject) ncreatureVisuals1, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.2);
      }
    }
    Variant shaderParameter = this._backstopMaterial.GetShaderParameter(NGameOverScreen._threshold);
    backstopTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateBackstopMaterial)), shaderParameter, Variant.op_Implicit(1f), 1.5).SetEase((Tween.EaseType) 2L).SetTrans((Tween.TransitionType) 1L);
    if (!await backstopTween.AwaitFinished((Node) this))
    {
      backstopTween = (Tween) null;
    }
    else
    {
      this._banner.AnimateIn();
      backstopTween.Kill();
      Tween tween = ((Node) this).CreateTween();
      tween.TweenProperty((GodotObject) this._uiNode, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.25);
      if (!await tween.AwaitFinished((Node) this))
      {
        backstopTween = (Tween) null;
      }
      else
      {
        TaskHelper.RunSafely(this.AnimateInQuote());
        this._continueButton.Enable();
        backstopTween = (Tween) null;
      }
    }
  }

  private void UpdateBackstopMaterial(float value)
  {
    this._backstopMaterial.SetShaderParameter(NGameOverScreen._threshold, Variant.op_Implicit(value));
  }

  public void AfterOverlayClosed() => ((Node) this).QueueFreeSafely();

  public void AfterOverlayShown()
  {
    NGame.Instance.SetScreenShakeTarget(this._screenshakeContainer);
    ((CanvasItem) this).Visible = true;
  }

  public void AfterOverlayHidden() => ((CanvasItem) this).Visible = false;

  public bool UseSharedBackstop => false;

  public Control? FocusedControlFromTopBar
  {
    get
    {
      if (((Node) this._badgeContainer).GetChildCount(false) > 0)
        return (Control) ((Node) this._badgeContainer).GetChild<NBadge>(0, false);
      return this._summaryContainer.DefaultFocusedControl != null ? this._summaryContainer.DefaultFocusedControl : (Control) this;
    }
  }

  public Control DefaultFocusedControl => (Control) this;

  private string GetAscensionMulti(int ascension)
  {
    int num1 = ascension / 10 + 1;
    int num2 = ascension % 10;
    if (num2 == 0)
      return num1.ToString();
    return $"{num1}.{num2}";
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(22)
    {
      new MethodInfo(NGameOverScreen.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGameOverScreen.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGameOverScreen.MethodName.DiscoveredAnyEpochs, new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGameOverScreen.MethodName.InitializeBannerAndQuote, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGameOverScreen.MethodName.OpenSummaryScreen, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NGameOverScreen.MethodName.AddScoreLine, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("locEntryKey"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("locAmountKey"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("amount"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("scoreLabel"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("iconPath"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NGameOverScreen.MethodName.PlayUnlockSfx, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGameOverScreen.MethodName.TweenScore, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("value"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NGameOverScreen.MethodName.GetScoreThreshold, new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("unlocksRemaining"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NGameOverScreen.MethodName.ShowLeaderboard, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NGameOverScreen.MethodName.HideSummary, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGameOverScreen.MethodName.OpenRunHistoryScreen, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NGameOverScreen.MethodName.OnMainMenuButtonPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NGameOverScreen.MethodName.OpenTimeline, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGameOverScreen.MethodName.ReturnToMainMenu, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGameOverScreen.MethodName.AfterOverlayOpened, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGameOverScreen.MethodName.MoveCreaturesToDifferentLayerAndDisableUi, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGameOverScreen.MethodName.UpdateBackstopMaterial, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("value"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NGameOverScreen.MethodName.AfterOverlayClosed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGameOverScreen.MethodName.AfterOverlayShown, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGameOverScreen.MethodName.AfterOverlayHidden, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGameOverScreen.MethodName.GetAscensionMulti, new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("ascension"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NGameOverScreen.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGameOverScreen.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGameOverScreen.MethodName.DiscoveredAnyEpochs) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      bool flag = this.DiscoveredAnyEpochs();
      ret = VariantUtils.CreateFrom<bool>(ref flag);
      return true;
    }
    if (StringName.op_Equality(ref method, NGameOverScreen.MethodName.InitializeBannerAndQuote) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.InitializeBannerAndQuote();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGameOverScreen.MethodName.OpenSummaryScreen) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OpenSummaryScreen(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGameOverScreen.MethodName.AddScoreLine) && ((NativeVariantPtrArgs) ref args).Count == 5)
    {
      this.AddScoreLine(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[2]), VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[3]), VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[4]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGameOverScreen.MethodName.PlayUnlockSfx) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.PlayUnlockSfx();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGameOverScreen.MethodName.TweenScore) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.TweenScore(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGameOverScreen.MethodName.GetScoreThreshold) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      int scoreThreshold = this.GetScoreThreshold(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<int>(ref scoreThreshold);
      return true;
    }
    if (StringName.op_Equality(ref method, NGameOverScreen.MethodName.ShowLeaderboard) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ShowLeaderboard(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGameOverScreen.MethodName.HideSummary) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.HideSummary();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGameOverScreen.MethodName.OpenRunHistoryScreen) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OpenRunHistoryScreen(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGameOverScreen.MethodName.OnMainMenuButtonPressed) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnMainMenuButtonPressed(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGameOverScreen.MethodName.OpenTimeline) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OpenTimeline();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGameOverScreen.MethodName.ReturnToMainMenu) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ReturnToMainMenu();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGameOverScreen.MethodName.AfterOverlayOpened) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AfterOverlayOpened();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGameOverScreen.MethodName.MoveCreaturesToDifferentLayerAndDisableUi) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.MoveCreaturesToDifferentLayerAndDisableUi();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGameOverScreen.MethodName.UpdateBackstopMaterial) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateBackstopMaterial(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGameOverScreen.MethodName.AfterOverlayClosed) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AfterOverlayClosed();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGameOverScreen.MethodName.AfterOverlayShown) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AfterOverlayShown();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGameOverScreen.MethodName.AfterOverlayHidden) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AfterOverlayHidden();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NGameOverScreen.MethodName.GetAscensionMulti) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    string ascensionMulti = this.GetAscensionMulti(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = VariantUtils.CreateFrom<string>(ref ascensionMulti);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NGameOverScreen.MethodName._Ready) || StringName.op_Equality(ref method, NGameOverScreen.MethodName._ExitTree) || StringName.op_Equality(ref method, NGameOverScreen.MethodName.DiscoveredAnyEpochs) || StringName.op_Equality(ref method, NGameOverScreen.MethodName.InitializeBannerAndQuote) || StringName.op_Equality(ref method, NGameOverScreen.MethodName.OpenSummaryScreen) || StringName.op_Equality(ref method, NGameOverScreen.MethodName.AddScoreLine) || StringName.op_Equality(ref method, NGameOverScreen.MethodName.PlayUnlockSfx) || StringName.op_Equality(ref method, NGameOverScreen.MethodName.TweenScore) || StringName.op_Equality(ref method, NGameOverScreen.MethodName.GetScoreThreshold) || StringName.op_Equality(ref method, NGameOverScreen.MethodName.ShowLeaderboard) || StringName.op_Equality(ref method, NGameOverScreen.MethodName.HideSummary) || StringName.op_Equality(ref method, NGameOverScreen.MethodName.OpenRunHistoryScreen) || StringName.op_Equality(ref method, NGameOverScreen.MethodName.OnMainMenuButtonPressed) || StringName.op_Equality(ref method, NGameOverScreen.MethodName.OpenTimeline) || StringName.op_Equality(ref method, NGameOverScreen.MethodName.ReturnToMainMenu) || StringName.op_Equality(ref method, NGameOverScreen.MethodName.AfterOverlayOpened) || StringName.op_Equality(ref method, NGameOverScreen.MethodName.MoveCreaturesToDifferentLayerAndDisableUi) || StringName.op_Equality(ref method, NGameOverScreen.MethodName.UpdateBackstopMaterial) || StringName.op_Equality(ref method, NGameOverScreen.MethodName.AfterOverlayClosed) || StringName.op_Equality(ref method, NGameOverScreen.MethodName.AfterOverlayShown) || StringName.op_Equality(ref method, NGameOverScreen.MethodName.AfterOverlayHidden) || StringName.op_Equality(ref method, NGameOverScreen.MethodName.GetAscensionMulti) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._continueButton))
    {
      this._continueButton = VariantUtils.ConvertTo<NGameOverContinueButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._viewRunButton))
    {
      this._viewRunButton = VariantUtils.ConvertTo<NViewRunButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._mainMenuButton))
    {
      this._mainMenuButton = VariantUtils.ConvertTo<NReturnToMainMenuButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._leaderboardButton))
    {
      this._leaderboardButton = VariantUtils.ConvertTo<NGameOverContinueButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._badgeContainer))
    {
      this._badgeContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._scoreLineContainer))
    {
      this._scoreLineContainer = VariantUtils.ConvertTo<GridContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._scoreBar))
    {
      this._scoreBar = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._scoreFg))
    {
      this._scoreFg = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._scoreProgress))
    {
      this._scoreProgress = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._unlocksRemaining))
    {
      this._unlocksRemaining = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._score))
    {
      this._score = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._scoreThreshold))
    {
      this._scoreThreshold = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._scoreUnlockedEpochId))
    {
      this._scoreUnlockedEpochId = VariantUtils.ConvertTo<string>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._leaderboard))
    {
      this._leaderboard = VariantUtils.ConvertTo<NDailyRunLeaderboard>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._creatureContainer))
    {
      this._creatureContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._summaryContainer))
    {
      this._summaryContainer = VariantUtils.ConvertTo<NRunSummary>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._fullBlackBackstop))
    {
      this._fullBlackBackstop = VariantUtils.ConvertTo<ColorRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._summaryBackstop))
    {
      this._summaryBackstop = VariantUtils.ConvertTo<ColorRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._backstop))
    {
      this._backstop = VariantUtils.ConvertTo<ColorRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._banner))
    {
      this._banner = VariantUtils.ConvertTo<NCommonBanner>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._deathQuote))
    {
      this._deathQuote = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._victoryDamageLabel))
    {
      this._victoryDamageLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._uiNode))
    {
      this._uiNode = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._screenshakeContainer))
    {
      this._screenshakeContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._discoveryLabel))
    {
      this._discoveryLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._encounterQuote))
    {
      this._encounterQuote = VariantUtils.ConvertTo<string>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._isAnimatingSummary))
    {
      this._isAnimatingSummary = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._backstopMaterial))
    {
      this._backstopMaterial = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NGameOverScreen.PropertyName._quoteTween))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._quoteTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName.ScreenType))
    {
      ref godot_variant local = ref value;
      NetScreenType screenType = this.ScreenType;
      godot_variant from = VariantUtils.CreateFrom<NetScreenType>(ref screenType);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName.UseSharedBackstop))
    {
      ref godot_variant local = ref value;
      bool useSharedBackstop = this.UseSharedBackstop;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref useSharedBackstop);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName.FocusedControlFromTopBar))
    {
      ref godot_variant local = ref value;
      Control controlFromTopBar = this.FocusedControlFromTopBar;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref controlFromTopBar);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._continueButton))
    {
      value = VariantUtils.CreateFrom<NGameOverContinueButton>(ref this._continueButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._viewRunButton))
    {
      value = VariantUtils.CreateFrom<NViewRunButton>(ref this._viewRunButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._mainMenuButton))
    {
      value = VariantUtils.CreateFrom<NReturnToMainMenuButton>(ref this._mainMenuButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._leaderboardButton))
    {
      value = VariantUtils.CreateFrom<NGameOverContinueButton>(ref this._leaderboardButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._badgeContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._badgeContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._scoreLineContainer))
    {
      value = VariantUtils.CreateFrom<GridContainer>(ref this._scoreLineContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._scoreBar))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._scoreBar);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._scoreFg))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._scoreFg);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._scoreProgress))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._scoreProgress);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._unlocksRemaining))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._unlocksRemaining);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._score))
    {
      value = VariantUtils.CreateFrom<int>(ref this._score);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._scoreThreshold))
    {
      value = VariantUtils.CreateFrom<int>(ref this._scoreThreshold);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._scoreUnlockedEpochId))
    {
      value = VariantUtils.CreateFrom<string>(ref this._scoreUnlockedEpochId);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._leaderboard))
    {
      value = VariantUtils.CreateFrom<NDailyRunLeaderboard>(ref this._leaderboard);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._creatureContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._creatureContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._summaryContainer))
    {
      value = VariantUtils.CreateFrom<NRunSummary>(ref this._summaryContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._fullBlackBackstop))
    {
      value = VariantUtils.CreateFrom<ColorRect>(ref this._fullBlackBackstop);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._summaryBackstop))
    {
      value = VariantUtils.CreateFrom<ColorRect>(ref this._summaryBackstop);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._backstop))
    {
      value = VariantUtils.CreateFrom<ColorRect>(ref this._backstop);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._banner))
    {
      value = VariantUtils.CreateFrom<NCommonBanner>(ref this._banner);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._deathQuote))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._deathQuote);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._victoryDamageLabel))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._victoryDamageLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._uiNode))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._uiNode);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._screenshakeContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._screenshakeContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._discoveryLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._discoveryLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._encounterQuote))
    {
      value = VariantUtils.CreateFrom<string>(ref this._encounterQuote);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._isAnimatingSummary))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isAnimatingSummary);
      return true;
    }
    if (StringName.op_Equality(ref name, NGameOverScreen.PropertyName._backstopMaterial))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._backstopMaterial);
      return true;
    }
    if (!StringName.op_Equality(ref name, NGameOverScreen.PropertyName._quoteTween))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Tween>(ref this._quoteTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NGameOverScreen.PropertyName._continueButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGameOverScreen.PropertyName._viewRunButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGameOverScreen.PropertyName._mainMenuButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGameOverScreen.PropertyName._leaderboardButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGameOverScreen.PropertyName._badgeContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGameOverScreen.PropertyName._scoreLineContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGameOverScreen.PropertyName._scoreBar, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGameOverScreen.PropertyName._scoreFg, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGameOverScreen.PropertyName._scoreProgress, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGameOverScreen.PropertyName._unlocksRemaining, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NGameOverScreen.PropertyName._score, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NGameOverScreen.PropertyName._scoreThreshold, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 4L, NGameOverScreen.PropertyName._scoreUnlockedEpochId, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGameOverScreen.PropertyName._leaderboard, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGameOverScreen.PropertyName._creatureContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGameOverScreen.PropertyName._summaryContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGameOverScreen.PropertyName._fullBlackBackstop, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGameOverScreen.PropertyName._summaryBackstop, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGameOverScreen.PropertyName._backstop, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGameOverScreen.PropertyName._banner, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGameOverScreen.PropertyName._deathQuote, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGameOverScreen.PropertyName._victoryDamageLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGameOverScreen.PropertyName._uiNode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGameOverScreen.PropertyName._screenshakeContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGameOverScreen.PropertyName._discoveryLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 4L, NGameOverScreen.PropertyName._encounterQuote, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NGameOverScreen.PropertyName._isAnimatingSummary, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGameOverScreen.PropertyName._backstopMaterial, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGameOverScreen.PropertyName._quoteTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NGameOverScreen.PropertyName.ScreenType, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NGameOverScreen.PropertyName.UseSharedBackstop, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGameOverScreen.PropertyName.FocusedControlFromTopBar, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGameOverScreen.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NGameOverScreen.PropertyName._continueButton, Variant.From<NGameOverContinueButton>(ref this._continueButton));
    info.AddProperty(NGameOverScreen.PropertyName._viewRunButton, Variant.From<NViewRunButton>(ref this._viewRunButton));
    info.AddProperty(NGameOverScreen.PropertyName._mainMenuButton, Variant.From<NReturnToMainMenuButton>(ref this._mainMenuButton));
    info.AddProperty(NGameOverScreen.PropertyName._leaderboardButton, Variant.From<NGameOverContinueButton>(ref this._leaderboardButton));
    info.AddProperty(NGameOverScreen.PropertyName._badgeContainer, Variant.From<Control>(ref this._badgeContainer));
    info.AddProperty(NGameOverScreen.PropertyName._scoreLineContainer, Variant.From<GridContainer>(ref this._scoreLineContainer));
    info.AddProperty(NGameOverScreen.PropertyName._scoreBar, Variant.From<Control>(ref this._scoreBar));
    info.AddProperty(NGameOverScreen.PropertyName._scoreFg, Variant.From<Control>(ref this._scoreFg));
    info.AddProperty(NGameOverScreen.PropertyName._scoreProgress, Variant.From<MegaLabel>(ref this._scoreProgress));
    info.AddProperty(NGameOverScreen.PropertyName._unlocksRemaining, Variant.From<MegaLabel>(ref this._unlocksRemaining));
    info.AddProperty(NGameOverScreen.PropertyName._score, Variant.From<int>(ref this._score));
    info.AddProperty(NGameOverScreen.PropertyName._scoreThreshold, Variant.From<int>(ref this._scoreThreshold));
    info.AddProperty(NGameOverScreen.PropertyName._scoreUnlockedEpochId, Variant.From<string>(ref this._scoreUnlockedEpochId));
    info.AddProperty(NGameOverScreen.PropertyName._leaderboard, Variant.From<NDailyRunLeaderboard>(ref this._leaderboard));
    info.AddProperty(NGameOverScreen.PropertyName._creatureContainer, Variant.From<Control>(ref this._creatureContainer));
    info.AddProperty(NGameOverScreen.PropertyName._summaryContainer, Variant.From<NRunSummary>(ref this._summaryContainer));
    info.AddProperty(NGameOverScreen.PropertyName._fullBlackBackstop, Variant.From<ColorRect>(ref this._fullBlackBackstop));
    info.AddProperty(NGameOverScreen.PropertyName._summaryBackstop, Variant.From<ColorRect>(ref this._summaryBackstop));
    info.AddProperty(NGameOverScreen.PropertyName._backstop, Variant.From<ColorRect>(ref this._backstop));
    info.AddProperty(NGameOverScreen.PropertyName._banner, Variant.From<NCommonBanner>(ref this._banner));
    info.AddProperty(NGameOverScreen.PropertyName._deathQuote, Variant.From<MegaRichTextLabel>(ref this._deathQuote));
    info.AddProperty(NGameOverScreen.PropertyName._victoryDamageLabel, Variant.From<MegaRichTextLabel>(ref this._victoryDamageLabel));
    info.AddProperty(NGameOverScreen.PropertyName._uiNode, Variant.From<Control>(ref this._uiNode));
    info.AddProperty(NGameOverScreen.PropertyName._screenshakeContainer, Variant.From<Control>(ref this._screenshakeContainer));
    info.AddProperty(NGameOverScreen.PropertyName._discoveryLabel, Variant.From<MegaLabel>(ref this._discoveryLabel));
    info.AddProperty(NGameOverScreen.PropertyName._encounterQuote, Variant.From<string>(ref this._encounterQuote));
    info.AddProperty(NGameOverScreen.PropertyName._isAnimatingSummary, Variant.From<bool>(ref this._isAnimatingSummary));
    info.AddProperty(NGameOverScreen.PropertyName._backstopMaterial, Variant.From<ShaderMaterial>(ref this._backstopMaterial));
    info.AddProperty(NGameOverScreen.PropertyName._quoteTween, Variant.From<Tween>(ref this._quoteTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NGameOverScreen.PropertyName._continueButton, ref variant1))
      this._continueButton = ((Variant) ref variant1).As<NGameOverContinueButton>();
    Variant variant2;
    if (info.TryGetProperty(NGameOverScreen.PropertyName._viewRunButton, ref variant2))
      this._viewRunButton = ((Variant) ref variant2).As<NViewRunButton>();
    Variant variant3;
    if (info.TryGetProperty(NGameOverScreen.PropertyName._mainMenuButton, ref variant3))
      this._mainMenuButton = ((Variant) ref variant3).As<NReturnToMainMenuButton>();
    Variant variant4;
    if (info.TryGetProperty(NGameOverScreen.PropertyName._leaderboardButton, ref variant4))
      this._leaderboardButton = ((Variant) ref variant4).As<NGameOverContinueButton>();
    Variant variant5;
    if (info.TryGetProperty(NGameOverScreen.PropertyName._badgeContainer, ref variant5))
      this._badgeContainer = ((Variant) ref variant5).As<Control>();
    Variant variant6;
    if (info.TryGetProperty(NGameOverScreen.PropertyName._scoreLineContainer, ref variant6))
      this._scoreLineContainer = ((Variant) ref variant6).As<GridContainer>();
    Variant variant7;
    if (info.TryGetProperty(NGameOverScreen.PropertyName._scoreBar, ref variant7))
      this._scoreBar = ((Variant) ref variant7).As<Control>();
    Variant variant8;
    if (info.TryGetProperty(NGameOverScreen.PropertyName._scoreFg, ref variant8))
      this._scoreFg = ((Variant) ref variant8).As<Control>();
    Variant variant9;
    if (info.TryGetProperty(NGameOverScreen.PropertyName._scoreProgress, ref variant9))
      this._scoreProgress = ((Variant) ref variant9).As<MegaLabel>();
    Variant variant10;
    if (info.TryGetProperty(NGameOverScreen.PropertyName._unlocksRemaining, ref variant10))
      this._unlocksRemaining = ((Variant) ref variant10).As<MegaLabel>();
    Variant variant11;
    if (info.TryGetProperty(NGameOverScreen.PropertyName._score, ref variant11))
      this._score = ((Variant) ref variant11).As<int>();
    Variant variant12;
    if (info.TryGetProperty(NGameOverScreen.PropertyName._scoreThreshold, ref variant12))
      this._scoreThreshold = ((Variant) ref variant12).As<int>();
    Variant variant13;
    if (info.TryGetProperty(NGameOverScreen.PropertyName._scoreUnlockedEpochId, ref variant13))
      this._scoreUnlockedEpochId = ((Variant) ref variant13).As<string>();
    Variant variant14;
    if (info.TryGetProperty(NGameOverScreen.PropertyName._leaderboard, ref variant14))
      this._leaderboard = ((Variant) ref variant14).As<NDailyRunLeaderboard>();
    Variant variant15;
    if (info.TryGetProperty(NGameOverScreen.PropertyName._creatureContainer, ref variant15))
      this._creatureContainer = ((Variant) ref variant15).As<Control>();
    Variant variant16;
    if (info.TryGetProperty(NGameOverScreen.PropertyName._summaryContainer, ref variant16))
      this._summaryContainer = ((Variant) ref variant16).As<NRunSummary>();
    Variant variant17;
    if (info.TryGetProperty(NGameOverScreen.PropertyName._fullBlackBackstop, ref variant17))
      this._fullBlackBackstop = ((Variant) ref variant17).As<ColorRect>();
    Variant variant18;
    if (info.TryGetProperty(NGameOverScreen.PropertyName._summaryBackstop, ref variant18))
      this._summaryBackstop = ((Variant) ref variant18).As<ColorRect>();
    Variant variant19;
    if (info.TryGetProperty(NGameOverScreen.PropertyName._backstop, ref variant19))
      this._backstop = ((Variant) ref variant19).As<ColorRect>();
    Variant variant20;
    if (info.TryGetProperty(NGameOverScreen.PropertyName._banner, ref variant20))
      this._banner = ((Variant) ref variant20).As<NCommonBanner>();
    Variant variant21;
    if (info.TryGetProperty(NGameOverScreen.PropertyName._deathQuote, ref variant21))
      this._deathQuote = ((Variant) ref variant21).As<MegaRichTextLabel>();
    Variant variant22;
    if (info.TryGetProperty(NGameOverScreen.PropertyName._victoryDamageLabel, ref variant22))
      this._victoryDamageLabel = ((Variant) ref variant22).As<MegaRichTextLabel>();
    Variant variant23;
    if (info.TryGetProperty(NGameOverScreen.PropertyName._uiNode, ref variant23))
      this._uiNode = ((Variant) ref variant23).As<Control>();
    Variant variant24;
    if (info.TryGetProperty(NGameOverScreen.PropertyName._screenshakeContainer, ref variant24))
      this._screenshakeContainer = ((Variant) ref variant24).As<Control>();
    Variant variant25;
    if (info.TryGetProperty(NGameOverScreen.PropertyName._discoveryLabel, ref variant25))
      this._discoveryLabel = ((Variant) ref variant25).As<MegaLabel>();
    Variant variant26;
    if (info.TryGetProperty(NGameOverScreen.PropertyName._encounterQuote, ref variant26))
      this._encounterQuote = ((Variant) ref variant26).As<string>();
    Variant variant27;
    if (info.TryGetProperty(NGameOverScreen.PropertyName._isAnimatingSummary, ref variant27))
      this._isAnimatingSummary = ((Variant) ref variant27).As<bool>();
    Variant variant28;
    if (info.TryGetProperty(NGameOverScreen.PropertyName._backstopMaterial, ref variant28))
      this._backstopMaterial = ((Variant) ref variant28).As<ShaderMaterial>();
    Variant variant29;
    if (!info.TryGetProperty(NGameOverScreen.PropertyName._quoteTween, ref variant29))
      return;
    this._quoteTween = ((Variant) ref variant29).As<Tween>();
  }

  public new class MethodName : NClickableControl.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName DiscoveredAnyEpochs = StringName.op_Implicit(nameof (DiscoveredAnyEpochs));
    public static readonly StringName InitializeBannerAndQuote = StringName.op_Implicit(nameof (InitializeBannerAndQuote));
    public static readonly StringName OpenSummaryScreen = StringName.op_Implicit(nameof (OpenSummaryScreen));
    public static readonly StringName AddScoreLine = StringName.op_Implicit(nameof (AddScoreLine));
    public static readonly StringName PlayUnlockSfx = StringName.op_Implicit(nameof (PlayUnlockSfx));
    public static readonly StringName TweenScore = StringName.op_Implicit(nameof (TweenScore));
    public static readonly StringName GetScoreThreshold = StringName.op_Implicit(nameof (GetScoreThreshold));
    public static readonly StringName ShowLeaderboard = StringName.op_Implicit(nameof (ShowLeaderboard));
    public static readonly StringName HideSummary = StringName.op_Implicit(nameof (HideSummary));
    public static readonly StringName OpenRunHistoryScreen = StringName.op_Implicit(nameof (OpenRunHistoryScreen));
    public static readonly StringName OnMainMenuButtonPressed = StringName.op_Implicit(nameof (OnMainMenuButtonPressed));
    public static readonly StringName OpenTimeline = StringName.op_Implicit(nameof (OpenTimeline));
    public static readonly StringName ReturnToMainMenu = StringName.op_Implicit(nameof (ReturnToMainMenu));
    public static readonly StringName AfterOverlayOpened = StringName.op_Implicit(nameof (AfterOverlayOpened));
    public static readonly StringName MoveCreaturesToDifferentLayerAndDisableUi = StringName.op_Implicit(nameof (MoveCreaturesToDifferentLayerAndDisableUi));
    public static readonly StringName UpdateBackstopMaterial = StringName.op_Implicit(nameof (UpdateBackstopMaterial));
    public static readonly StringName AfterOverlayClosed = StringName.op_Implicit(nameof (AfterOverlayClosed));
    public static readonly StringName AfterOverlayShown = StringName.op_Implicit(nameof (AfterOverlayShown));
    public static readonly StringName AfterOverlayHidden = StringName.op_Implicit(nameof (AfterOverlayHidden));
    public static readonly StringName GetAscensionMulti = StringName.op_Implicit(nameof (GetAscensionMulti));
  }

  public new class PropertyName : NClickableControl.PropertyName
  {
    public static readonly StringName ScreenType = StringName.op_Implicit(nameof (ScreenType));
    public static readonly StringName UseSharedBackstop = StringName.op_Implicit(nameof (UseSharedBackstop));
    public static readonly StringName FocusedControlFromTopBar = StringName.op_Implicit(nameof (FocusedControlFromTopBar));
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _continueButton = StringName.op_Implicit(nameof (_continueButton));
    public static readonly StringName _viewRunButton = StringName.op_Implicit(nameof (_viewRunButton));
    public static readonly StringName _mainMenuButton = StringName.op_Implicit(nameof (_mainMenuButton));
    public static readonly StringName _leaderboardButton = StringName.op_Implicit(nameof (_leaderboardButton));
    public static readonly StringName _badgeContainer = StringName.op_Implicit(nameof (_badgeContainer));
    public static readonly StringName _scoreLineContainer = StringName.op_Implicit(nameof (_scoreLineContainer));
    public static readonly StringName _scoreBar = StringName.op_Implicit(nameof (_scoreBar));
    public static readonly StringName _scoreFg = StringName.op_Implicit(nameof (_scoreFg));
    public static readonly StringName _scoreProgress = StringName.op_Implicit(nameof (_scoreProgress));
    public static readonly StringName _unlocksRemaining = StringName.op_Implicit(nameof (_unlocksRemaining));
    public static readonly StringName _score = StringName.op_Implicit(nameof (_score));
    public static readonly StringName _scoreThreshold = StringName.op_Implicit(nameof (_scoreThreshold));
    public static readonly StringName _scoreUnlockedEpochId = StringName.op_Implicit(nameof (_scoreUnlockedEpochId));
    public static readonly StringName _leaderboard = StringName.op_Implicit(nameof (_leaderboard));
    public static readonly StringName _creatureContainer = StringName.op_Implicit(nameof (_creatureContainer));
    public static readonly StringName _summaryContainer = StringName.op_Implicit(nameof (_summaryContainer));
    public static readonly StringName _fullBlackBackstop = StringName.op_Implicit(nameof (_fullBlackBackstop));
    public static readonly StringName _summaryBackstop = StringName.op_Implicit(nameof (_summaryBackstop));
    public static readonly StringName _backstop = StringName.op_Implicit(nameof (_backstop));
    public static readonly StringName _banner = StringName.op_Implicit(nameof (_banner));
    public static readonly StringName _deathQuote = StringName.op_Implicit(nameof (_deathQuote));
    public static readonly StringName _victoryDamageLabel = StringName.op_Implicit(nameof (_victoryDamageLabel));
    public static readonly StringName _uiNode = StringName.op_Implicit(nameof (_uiNode));
    public static readonly StringName _screenshakeContainer = StringName.op_Implicit(nameof (_screenshakeContainer));
    public static readonly StringName _discoveryLabel = StringName.op_Implicit(nameof (_discoveryLabel));
    public static readonly StringName _encounterQuote = StringName.op_Implicit(nameof (_encounterQuote));
    public static readonly StringName _isAnimatingSummary = StringName.op_Implicit(nameof (_isAnimatingSummary));
    public static readonly StringName _backstopMaterial = StringName.op_Implicit(nameof (_backstopMaterial));
    public static readonly StringName _quoteTween = StringName.op_Implicit(nameof (_quoteTween));
  }

  public new class SignalName : NClickableControl.SignalName
  {
  }
}
