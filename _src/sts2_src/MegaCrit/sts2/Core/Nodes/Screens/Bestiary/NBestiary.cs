// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Bestiary.NBestiary
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Bestiary;

[ScriptPath("res://src/Core/Nodes/Screens/Bestiary/NBestiary.cs")]
public class NBestiary : NSubmenu
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("screens/bestiary/bestiary");
  private SerializableProgress _progress;
  private MegaRichTextLabel _monsterNameLabel;
  private MegaLabel _epithet;
  private NScrollableContainer _sidebar;
  private VBoxContainer _bestiaryList;
  private static readonly LocString _locked = new LocString("bestiary", "LOCKED.monsterTitle");
  private Control _selectionArrow;
  private Tween? _arrowTween;
  private static readonly Vector2 _arrowOffset = new Vector2(-42f, 119f);
  private bool _initSelectionArrow = true;
  private Control _layoutContainer;
  private NBestiaryLayout? _currentLayout;
  private Control _characterIcon;
  private TextureRect _iconTexture;
  private TextureRect _iconOutlineTexture;
  private Control _dialogueLine;
  private MegaRichTextLabel _dialogueLabel;
  private Control _dialogueBubble;
  private TextureRect _dialogueTail;
  private TextureRect _dialogueTailShadow;
  private const string _dialogueTailPath = "res://images/ui/dialogue_tail.png";
  private const string _thoughtTailPath = "res://images/ui/thought_tail.png";
  private NButton _modeButton;
  private MegaLabel _modeLabel;
  private bool _isStatsMode;
  private TextureRect _pageLeftIcon;
  private TextureRect _pageRightIcon;
  private static readonly StringName _filterLeftHotkey = MegaInput.viewDeckAndTabLeft;
  private static readonly StringName _filterRightHotkey = MegaInput.viewExhaustPileAndTabRight;
  private Control _moveList;
  private Control _moveContainer;
  private Control _statsContainer;
  private Control _filterContainer;
  private MegaRichTextLabel _statsLabel;
  private NBestiaryCharacterFilter _currentFilter;
  private HashSet<ModelId> _discoveredMonsterIds;
  private HashSet<ModelId> _discoveredEncounterIds;
  private NBestiaryEntry? _selectedEntry;
  private Control? _previousScreenshakeTarget;
  private Tween? _tween;
  private Tween? _dialogueTween;

  public static NBestiary? Instance { get; private set; }

  public static string[] AssetPaths
  {
    get
    {
      List<string> stringList = new List<string>();
      stringList.Add(NBestiary._scenePath);
      stringList.AddRange(NBestiaryEntry.AssetPaths);
      return stringList.ToArray();
    }
  }

  protected override Control? InitialFocusedControl
  {
    get
    {
      return (Control) ((IEnumerable) ((Node) this._bestiaryList).GetChildren(false)).OfType<NBestiaryEntry>().FirstOrDefault<NBestiaryEntry>();
    }
  }

  public Control BackVfxContainer { get; private set; }

  public Control VfxContainer { get; private set; }

  public Control? Layout => (Control) this._currentLayout;

  public static NBestiary? Create()
  {
    return TestMode.IsOn ? (NBestiary) null : PreloadManager.Cache.GetScene(NBestiary._scenePath).Instantiate<NBestiary>((PackedScene.GenEditState) 0L);
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%MoveHeader")).SetTextAutoSize(new LocString("bestiary", "ACTIONS.header").GetFormattedText());
    ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%ConstructionLabel")).SetTextAutoSize(new LocString("bestiary", "UNDER_CONSTRUCTION").GetRawText());
    this._sidebar = ((Node) this).GetNode<NScrollableContainer>(NodePath.op_Implicit("%Sidebar"));
    this._bestiaryList = ((Node) this).GetNode<VBoxContainer>(NodePath.op_Implicit("%BestiaryList"));
    this._monsterNameLabel = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%MonsterName"));
    this._layoutContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%LayoutContainer"));
    this._epithet = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%Epithet"));
    this._characterIcon = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%CharacterIcon"));
    this._iconTexture = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%Icon"));
    this._iconOutlineTexture = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%Outline"));
    this._dialogueLine = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%DialogueLine"));
    this._dialogueLabel = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%DialogueText"));
    this._dialogueBubble = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Bubble"));
    this._dialogueTail = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%DialogueTail"));
    this._dialogueTailShadow = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%DialogueTailShadow"));
    this._modeButton = (NButton) ((Node) this).GetNode<NBestiaryModeButton>(NodePath.op_Implicit("%ModeButton"));
    ((GodotObject) this._modeButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.ToggleMode)), 0U);
    this._modeLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%ModeLabel"));
    this._pageLeftIcon = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%PageLeftIcon"));
    this._pageRightIcon = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%PageRightIcon"));
    ((GodotObject) NControllerManager.Instance).Connect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.UpdatePageIcons)), 0U);
    ((GodotObject) NControllerManager.Instance).Connect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.UpdatePageIcons)), 0U);
    ((GodotObject) NInputManager.Instance).Connect(NInputManager.SignalName.InputRebound, Callable.From(new Action(this.UpdatePageIcons)), 0U);
    this._moveContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%MoveContainer"));
    this._statsContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%StatsContainer"));
    this._selectionArrow = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%SelectionArrow"));
    this._moveList = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%MoveList"));
    this._filterContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%PoolFilters"));
    this._statsLabel = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%StatisticsFull"));
    this.VfxContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%VfxContainer"));
    this.BackVfxContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%BackVfxContainer"));
  }

  private void ToggleMode(NButton _)
  {
    this._isStatsMode = !this._isStatsMode;
    if (this._isStatsMode)
    {
      this._modeLabel.SetTextAutoSize(new LocString("bestiary", "MODE.viewActions").GetRawText());
      this.ShowStatsPanel();
      this.DisplayCharacterData();
      this.EnableStatsModeHotkeys();
      this.DisableMoveButtonHotkeys();
    }
    else
    {
      this._modeLabel.SetTextAutoSize(new LocString("bestiary", "MODE.viewStats").GetRawText());
      this.ShowMovesPanel();
      this.HideDialogue();
      this.DisableStatsModeHotkeys();
      this.EnableMoveButtonHotkeys();
    }
    this.UpdatePageIcons();
  }

  private void EnableMoveButtonHotkeys()
  {
    foreach (NClickableControl child in ((Node) this._moveList).GetChildren(false))
      child.Enable();
  }

  private void DisableMoveButtonHotkeys()
  {
    foreach (NClickableControl child in ((Node) this._moveList).GetChildren(false))
      child.Disable();
  }

  private void ShowMovesPanel()
  {
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    ((CanvasItem) this._statsContainer).Visible = false;
    ((CanvasItem) this._moveContainer).Visible = true;
    Control moveContainer = this._moveContainer;
    Color modulate = ((CanvasItem) this).Modulate;
    modulate.A = 0.0f;
    Color color = modulate;
    ((CanvasItem) moveContainer).Modulate = color;
    this._tween.TweenProperty((GodotObject) this._moveContainer, NodePath.op_Implicit("position:x"), Variant.op_Implicit(242f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L).From(Variant.op_Implicit(210f));
    this._tween.TweenProperty((GodotObject) this._moveContainer, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.5);
  }

  private void ShowStatsPanel()
  {
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    ((CanvasItem) this._moveContainer).Visible = false;
    ((CanvasItem) this._statsContainer).Visible = true;
    Control statsContainer = this._statsContainer;
    Color modulate = ((CanvasItem) this).Modulate;
    modulate.A = 0.0f;
    Color color = modulate;
    ((CanvasItem) statsContainer).Modulate = color;
    this._tween.TweenProperty((GodotObject) this._statsContainer, NodePath.op_Implicit("position:x"), Variant.op_Implicit(242f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L).From(Variant.op_Implicit(210f));
    this._tween.TweenProperty((GodotObject) this._statsContainer, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.5);
  }

  private void RefreshStatisticsText()
  {
    if (this._selectedEntry == null)
    {
      Log.Error("How did this happen?");
    }
    else
    {
      BestiaryEntry entry = this._selectedEntry.Entry;
      EnemyStats enemyStats = (EnemyStats) null;
      if (entry.monsterModel != null)
        enemyStats = this._progress.EnemyStats.FirstOrDefault<EnemyStats>((Func<EnemyStats, bool>) (e => e.Id == entry.monsterModel.Id));
      else
        Log.Warn($"Need to handle special case: {entry.encounterModel.Id}");
      foreach (Node child in ((Node) this._filterContainer).GetChildren(false))
      {
        NBestiaryCharacterFilter filter = child as NBestiaryCharacterFilter;
        if (filter != null && enemyStats != null)
        {
          if (filter.character != null)
          {
            FightStats fightStats = enemyStats.FightStats.FirstOrDefault<FightStats>((Func<FightStats, bool>) (f => f.Character == filter.character.Id));
            filter.kills = fightStats != null ? fightStats.Wins : 0;
            filter.deaths = fightStats != null ? fightStats.Losses : 0;
          }
          else
          {
            filter.kills = enemyStats.TotalWins;
            filter.deaths = enemyStats.TotalLosses;
          }
          filter.IsLocked = filter.kills + filter.deaths <= 0;
        }
      }
    }
  }

  private void DisplayCharacterData()
  {
    if (this._selectedEntry == null)
    {
      Log.Error("How did this happen?");
    }
    else
    {
      if (this._selectedEntry.Entry.monsterModel == null)
        return;
      LocString locString = new LocString("bestiary", "STATS.layout");
      if (this._currentFilter.Total == 0)
      {
        locString.Add("total", 0M);
        locString.Add("kills", 0M);
        locString.Add("deaths", 0M);
        locString.Add("winrate", "--");
      }
      else
      {
        locString.Add("total", (Decimal) this._currentFilter.Total);
        locString.Add("kills", (Decimal) this._currentFilter.kills);
        locString.Add("deaths", (Decimal) this._currentFilter.deaths);
        locString.Add("winrate", this._currentFilter.WinRate);
      }
      this._statsLabel.SetTextAutoSize(locString.GetFormattedText());
      if (this._currentFilter.kills <= 0)
      {
        this._dialogueLabel.SetTextAutoSize(this._currentFilter.BestiarySeenQuote);
      }
      else
      {
        LocString bestiaryKillQuote = this._currentFilter.BestiaryKillQuote;
        if (bestiaryKillQuote == null)
          this._dialogueLabel.SetTextAutoSize(new LocString("bestiary", "QUOTE_PLACEHOLDER").GetFormattedText());
        else
          this._dialogueLabel.SetTextAutoSize(bestiaryKillQuote.GetFormattedText());
      }
      if (this._currentFilter.character == null)
        this.HideDialogue();
      else
        this.ShowDialogue();
      this.UpdateDialogueBubbleStyle();
    }
  }

  private void UpdateDialogueBubbleStyle()
  {
    CharacterModel character = this._currentFilter.character;
    Color color = character != null ? character.DialogueColor : StsColors.transparentWhite;
    ((CanvasItem) this._characterIcon).Modulate = character == null ? StsColors.transparentWhite : Colors.White;
    if (character != null)
    {
      this._iconTexture.Texture = character.IconTexture;
      this._iconOutlineTexture.Texture = character.IconOutlineTexture;
    }
    ((CanvasItem) this._dialogueBubble).SelfModulate = color;
    ((CanvasItem) this._dialogueTail).SelfModulate = color;
    this._dialogueTail.Texture = (Texture2D) PreloadManager.Cache.GetCompressedTexture2D(character is Silent ? "res://images/ui/thought_tail.png" : "res://images/ui/dialogue_tail.png");
  }

  private void ShowDialogue()
  {
    this._dialogueTween?.Kill();
    this._dialogueTween = ((Node) this).CreateTween().SetParallel(true);
    ((CanvasItem) this._dialogueLine).Modulate = StsColors.transparentWhite;
    this._dialogueTween.TweenProperty((GodotObject) this._dialogueLine, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.1).SetDelay(0.1);
    this._dialogueTween.TweenProperty((GodotObject) this._dialogueLine, NodePath.op_Implicit("position:x"), Variant.op_Implicit(560f), 0.4).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L).From(Variant.op_Implicit(528f)).SetDelay(0.1);
  }

  private void HideDialogue()
  {
    this._dialogueTween?.Kill();
    this._dialogueTween = ((Node) this).CreateTween();
    this._dialogueTween.TweenProperty((GodotObject) this._dialogueLine, NodePath.op_Implicit("modulate"), Variant.op_Implicit(StsColors.transparentWhite), 0.1);
  }

  public override void OnSubmenuOpened()
  {
    NBestiary.Instance = this;
    this._progress = SaveManager.Instance.Progress.ToSerializable();
    this._previousScreenshakeTarget = NGame.Instance?.ScreenshakeTarget;
    this._isStatsMode = !SaveManager.Instance.PrefsSave.IsBestiaryActionsPreferred;
    if (this._isStatsMode)
      this._modeLabel.SetTextAutoSize(new LocString("bestiary", "MODE.viewActions").GetRawText());
    else
      this._modeLabel.SetTextAutoSize(new LocString("bestiary", "MODE.viewStats").GetRawText());
    this.CreateFilters();
    this.CreateEntries();
    if (this._isStatsMode)
    {
      this.DisplayCharacterData();
      this.EnableStatsModeHotkeys();
      this.DisableMoveButtonHotkeys();
    }
    this.UpdatePageIcons();
  }

  public override void OnSubmenuClosed()
  {
    this.DisableStatsModeHotkeys();
    this.DisableMoveButtonHotkeys();
    this._initSelectionArrow = true;
    this._selectedEntry = (NBestiaryEntry) null;
    NBestiary.Instance = (NBestiary) null;
    SaveManager.Instance.PrefsSave.IsBestiaryActionsPreferred = !this._isStatsMode;
    SaveManager.Instance.SavePrefsFile();
    this._currentLayout?.Cleanup();
    if (this._previousScreenshakeTarget != null)
    {
      if (((Node) this._previousScreenshakeTarget).IsValid())
      {
        NGame.Instance?.SetScreenShakeTarget(this._previousScreenshakeTarget);
      }
      else
      {
        Log.Warn("The screenshake target is no longer valid. This should never happen.");
        this._previousScreenshakeTarget = (Control) null;
      }
    }
    else
      NGame.Instance?.ClearScreenShakeTarget();
    ((Node) this._bestiaryList).FreeChildren();
    this._lastFocusedControl = (Control) null;
  }

  private void CreateEntries()
  {
    this._discoveredMonsterIds = SaveManager.Instance.Progress.EnemyStats.Values.Where<EnemyStats>((Func<EnemyStats, bool>) (e => e.TotalWins > 0)).Select<EnemyStats, ModelId>((Func<EnemyStats, ModelId>) (e => e.Id)).ToHashSet<ModelId>();
    this._discoveredEncounterIds = SaveManager.Instance.Progress.EncounterStats.Values.Where<EncounterStats>((Func<EncounterStats, bool>) (e => e.TotalWins > 0)).Select<EncounterStats, ModelId>((Func<EncounterStats, ModelId>) (e => e.Id)).ToHashSet<ModelId>();
    foreach (ActModel act in ModelDb.Acts)
      this.AddAct(act);
    this.AddEvents();
    Control node = ((Node) this._sidebar).GetNode<Control>(NodePath.op_Implicit("Content"));
    Control control = node;
    Vector2 position = node.Position;
    position.Y = 0.0f;
    Vector2 vector2 = position;
    control.Position = vector2;
    this._sidebar.InstantlyScrollToTop();
    NBestiaryEntry entry = ((IEnumerable) ((Node) this._bestiaryList).GetChildren(false)).OfType<NBestiaryEntry>().FirstOrDefault<NBestiaryEntry>((Func<NBestiaryEntry, bool>) (e => e.IsDiscovered && e.IsEnabled));
    if (entry == null)
      Log.Error("Should not be possible as the Compendium + Bestiary isn't unlocked by default!");
    else
      this.SelectMonster(entry);
  }

  private void CreateFilters()
  {
    ((Node) this._filterContainer).FreeChildren();
    this.AddFilter((CharacterModel) null);
    this.AddFilter((CharacterModel) ModelDb.Character<Ironclad>());
    this.AddFilter((CharacterModel) ModelDb.Character<Silent>());
    this.AddFilter((CharacterModel) ModelDb.Character<Regent>());
    this.AddFilter((CharacterModel) ModelDb.Character<Necrobinder>());
    this.AddFilter((CharacterModel) ModelDb.Character<Defect>());
  }

  private void AddFilter(CharacterModel? character)
  {
    NBestiaryCharacterFilter child = NBestiaryCharacterFilter.Create(character);
    ((GodotObject) child).Connect(NBestiaryCharacterFilter.SignalName.Toggled, Callable.From<NBestiaryCharacterFilter>(new Action<NBestiaryCharacterFilter>(this.OnCharacterFilterSelected)), 0U);
    ((Node) this._filterContainer).AddChildSafely((Node) child);
    if (character != null)
      return;
    this._currentFilter = child;
    this._currentFilter.IsSelected = true;
  }

  private void OnCharacterFilterSelected(NBestiaryCharacterFilter selectedFilter)
  {
    this._currentFilter = selectedFilter;
    foreach (Node child in ((Node) this._filterContainer).GetChildren(false))
    {
      if (!child.Equals((object) selectedFilter))
        ((NBestiaryCharacterFilter) child).Deselect();
    }
    if (!this._isStatsMode)
      return;
    this.DisplayCharacterData();
  }

  private void AddAct(ActModel act)
  {
    if (!SaveManager.Instance.Progress.DiscoveredActs.Contains(act.Id))
      return;
    ((Node) this._bestiaryList).AddChildSafely((Node) NBestiaryLabelDivider.Create(act));
    HashSet<ModelId> modelIdSet = new HashSet<ModelId>();
    List<BestiaryEntry> entries = new List<BestiaryEntry>();
    foreach (EncounterModel allEncounter in act.AllEncounters)
    {
      foreach (MonsterModel allPossibleMonster in allEncounter.AllPossibleMonsters)
      {
        if (modelIdSet.Add(allPossibleMonster.Id) && allPossibleMonster.ShouldShowInCompendium)
          entries.Add(BestiaryEntry.FromMonster(allPossibleMonster, allEncounter, allEncounter.RoomType));
      }
    }
    if (act is Hive)
      entries.Add(BestiaryEntry.FromEncounter((EncounterModel) ModelDb.Encounter<DecimillipedeElite>(), RoomType.Elite));
    this.AddEntries(entries);
  }

  private void AddEvents()
  {
    ((Node) this._bestiaryList).AddChildSafely((Node) NBestiaryLabelDivider.Create(new LocString("bestiary", "EVENTS.title")));
    HashSet<ModelId> modelIdSet = new HashSet<ModelId>();
    List<BestiaryEntry> entries = new List<BestiaryEntry>();
    foreach (EncounterModel eventEncounter in ModelDb.EventEncounters)
    {
      foreach (MonsterModel allPossibleMonster in eventEncounter.AllPossibleMonsters)
      {
        if (modelIdSet.Add(allPossibleMonster.Id) && allPossibleMonster.ShouldShowInCompendium)
          entries.Add(BestiaryEntry.FromMonster(allPossibleMonster, eventEncounter, eventEncounter.RoomType));
      }
    }
    this.AddEntries(entries);
  }

  private void AddEntries(List<BestiaryEntry> entries)
  {
    entries.Sort((Comparison<BestiaryEntry>) ((e1, e2) =>
    {
      if (e1.roomType != e2.roomType)
        return e1.roomType.CompareTo((object) e2.roomType);
      if (e1.roomType == RoomType.Boss)
      {
        int num = string.Compare(e1.GetEncounterTitle(), e2.GetEncounterTitle(), StringComparison.CurrentCulture);
        if (num != 0)
          return num;
      }
      return string.Compare(e1.GetEntryTitle(), e2.GetEntryTitle(), StringComparison.CurrentCulture);
    }));
    foreach (BestiaryEntry entry in entries)
    {
      NBestiaryEntry child = NBestiaryEntry.Create(entry, entry.IsDiscovered(this._discoveredMonsterIds, this._discoveredEncounterIds));
      ((Node) this._bestiaryList).AddChildSafely((Node) child);
      ((GodotObject) child).Connect(NClickableControl.SignalName.Released, Callable.From<NBestiaryEntry>(new Action<NBestiaryEntry>(this.OnMonsterClicked)), 0U);
    }
  }

  private void OnMonsterClicked(NBestiaryEntry entry) => this.SelectMonster(entry);

  private void SelectMonster(NBestiaryEntry entry)
  {
    if (entry == this._selectedEntry)
      return;
    ((Node) this._moveList).FreeChildren();
    this._selectedEntry = entry;
    if (entry.IsUnderConstruction)
    {
      this._monsterNameLabel.Text = entry.Entry.GetEntryTitle();
      this._currentLayout?.Cleanup();
      NBestiaryLayout currentLayout = this._currentLayout;
      if (currentLayout != null)
        ((Node) currentLayout).QueueFreeSafely();
    }
    else if (!entry.IsDiscovered)
    {
      this._monsterNameLabel.Text = NBestiary._locked.GetFormattedText();
      this._currentLayout?.Cleanup();
      NBestiaryLayout currentLayout = this._currentLayout;
      if (currentLayout != null)
        ((Node) currentLayout).QueueFreeSafely();
    }
    else
    {
      this._tween?.Kill();
      this._tween = ((Node) this).CreateTween().SetParallel(true);
      this._monsterNameLabel.Text = entry.Entry.GetEntryTitle();
      ((CanvasItem) this._monsterNameLabel).SelfModulate = StsColors.transparentWhite;
      ((CanvasItem) this._epithet).Modulate = StsColors.transparentWhite;
      this._tween.TweenProperty((GodotObject) this._monsterNameLabel, NodePath.op_Implicit("position:y"), Variant.op_Implicit(88f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L).From(Variant.op_Implicit(24f));
      this._tween.TweenProperty((GodotObject) this._monsterNameLabel, NodePath.op_Implicit("self_modulate:a"), Variant.op_Implicit(1f), 0.5);
      this._tween.TweenProperty((GodotObject) this._epithet, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.5).SetDelay(0.2);
      this._tween.TweenProperty((GodotObject) this._dialogueLabel, NodePath.op_Implicit("position:y"), Variant.op_Implicit(894f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L).From(Variant.op_Implicit(958f));
      this._tween.TweenProperty((GodotObject) this._dialogueLabel, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.5);
      if (this._isStatsMode)
        this.ShowStatsPanel();
      else
        this.ShowMovesPanel();
      this.RefreshStatisticsText();
      this._currentLayout?.Cleanup();
      if (!entry.Entry.CanReuseLayout(this._currentLayout))
      {
        NBestiaryLayout currentLayout = this._currentLayout;
        if (currentLayout != null)
          ((Node) currentLayout).QueueFreeSafely();
        this._currentLayout = entry.Entry.CreateLayoutNode(this);
        ((Node) this._layoutContainer).AddChildSafely((Node) this._currentLayout);
        NGame.Instance?.SetScreenShakeTarget((Control) this._currentLayout);
      }
      List<BestiaryMonsterMove> bestiaryMonsterMoveList = this._currentLayout.Setup(entry.Entry, this._tween);
      for (int index = 0; index < bestiaryMonsterMoveList.Count; ++index)
      {
        if (index >= 9)
          Log.Error("Hotkeys for monster Actions beyond 9 are not supported!");
        NBestiaryMoveButton bestiaryMoveButton = this.CreateBestiaryMoveButton(bestiaryMonsterMoveList[index], index + 1);
        ((Node) this._moveList).AddChildSafely((Node) bestiaryMoveButton);
        ((GodotObject) bestiaryMoveButton).Connect(NClickableControl.SignalName.Released, Callable.From<NBestiaryMoveButton>(new Action<NBestiaryMoveButton>(this.OnMoveButtonClicked)), 0U);
      }
      if (this._isStatsMode)
      {
        this.DisableMoveButtonHotkeys();
        if (this._currentFilter.IsLocked)
        {
          NBestiaryCharacterFilter child = ((Node) this._filterContainer).GetChild<NBestiaryCharacterFilter>(0, false);
          child.IsSelected = true;
          this.OnCharacterFilterSelected(child);
        }
        this.DisplayCharacterData();
      }
    }
    if (this._initSelectionArrow)
    {
      Control selectionArrow = this._selectionArrow;
      Color modulate = ((CanvasItem) this._selectionArrow).Modulate;
      modulate.A = 0.0f;
      Color color = modulate;
      ((CanvasItem) selectionArrow).Modulate = color;
      this._initSelectionArrow = false;
      TaskHelper.RunSafely(this.InitializeSelectorArrow(entry));
    }
    else
    {
      Control selectionArrow = this._selectionArrow;
      Color modulate = ((CanvasItem) this._selectionArrow).Modulate;
      modulate.A = 1f;
      Color color = modulate;
      ((CanvasItem) selectionArrow).Modulate = color;
      this._arrowTween?.Kill();
      this._arrowTween = ((Node) this).CreateTween().SetParallel(true);
      this._arrowTween.TweenProperty((GodotObject) this._selectionArrow, NodePath.op_Implicit("position"), Variant.op_Implicit(Vector2.op_Addition(entry.Position, NBestiary._arrowOffset)), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    }
  }

  private NBestiaryMoveButton CreateBestiaryMoveButton(BestiaryMonsterMove move, int moveIndex)
  {
    bool? isUsingController = NControllerManager.Instance?.IsUsingController;
    if (isUsingController.HasValue && isUsingController.GetValueOrDefault())
    {
      NBestiaryMoveButton bestiaryMoveButton;
      switch (moveIndex)
      {
        case 1:
          bestiaryMoveButton = NBestiaryMoveButton.Create(move, MegaInput.viewDeckAndTabLeft);
          break;
        case 2:
          bestiaryMoveButton = NBestiaryMoveButton.Create(move, MegaInput.viewExhaustPileAndTabRight);
          break;
        case 3:
          bestiaryMoveButton = NBestiaryMoveButton.Create(move, MegaInput.viewDiscardPile);
          break;
        case 4:
          bestiaryMoveButton = NBestiaryMoveButton.Create(move, MegaInput.viewDrawPile);
          break;
        case 5:
          bestiaryMoveButton = NBestiaryMoveButton.Create(move, MegaInput.topPanel);
          break;
        case 6:
          bestiaryMoveButton = NBestiaryMoveButton.Create(move, MegaInput.altUp);
          break;
        case 7:
          bestiaryMoveButton = NBestiaryMoveButton.Create(move, MegaInput.altDown);
          break;
        case 8:
          bestiaryMoveButton = NBestiaryMoveButton.Create(move, MegaInput.altLeft);
          break;
        case 9:
          bestiaryMoveButton = NBestiaryMoveButton.Create(move, MegaInput.altRight);
          break;
        default:
          bestiaryMoveButton = NBestiaryMoveButton.Create(move, StringName.op_Implicit($"{moveIndex}"));
          break;
      }
      return bestiaryMoveButton;
    }
    return NBestiaryMoveButton.Create(move, StringName.op_Implicit($"mega_select_card_{moveIndex}"));
  }

  private async Task InitializeSelectorArrow(NBestiaryEntry entry)
  {
    Variant[] signal = await ((GodotObject) this).ToSignal((GodotObject) ((Node) this).GetTree(), SceneTree.SignalName.ProcessFrame);
    this._selectionArrow.Position = Vector2.op_Addition(entry.Position, NBestiary._arrowOffset);
    this._arrowTween?.Kill();
    this._arrowTween = ((Node) this).CreateTween().SetParallel(true);
    this._arrowTween.TweenProperty((GodotObject) this._selectionArrow, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.2);
  }

  private void OnMoveButtonClicked(NButton button)
  {
    NBestiaryMoveButton nbestiaryMoveButton = (NBestiaryMoveButton) button;
    NBestiaryLayout currentLayout = this._currentLayout;
    NBestiary.PlayMoveAnim((IEnumerable<NCreature>) ((currentLayout != null ? (object) currentLayout.GetCreatures() : (object) null) ?? (object) Array.Empty<NCreature>()), nbestiaryMoveButton.Move);
  }

  private static void PlayMoveAnim(IEnumerable<NCreature> creatures, BestiaryMonsterMove move)
  {
    foreach (NCreature creature in creatures)
    {
      if (move.stateId != null)
      {
        MonsterModel monster = creature.Entity.Monster;
        if (monster == null)
          throw new InvalidOperationException($"Non-monster creature {creature} is in the bestiary!");
        monster.SetMoveImmediate((MoveState) monster.MoveStateMachine.States[move.stateId], true);
        TaskHelper.RunSafely(monster.PerformMove());
      }
      else if (move.nonStateMove != null)
        TaskHelper.RunSafely(move.nonStateMove((IReadOnlyList<Creature>) Array.Empty<Creature>()));
      else if (move.action != null)
        TaskHelper.RunSafely(move.action());
      else if (move.animId != null)
      {
        creature.Visuals.SpineBody?.GetAnimationState().SetAnimation(move.animId, false);
        if (move.animId != "die")
          creature.Visuals.SpineBody?.GetAnimationState().AddAnimation("idle_loop");
        if (move.sfx != null)
          NAudioManager.Instance.PlayOneShot(move.sfx);
      }
      if (move.stopSfxLoops)
        creature.StopAllSfxLoops();
    }
  }

  public NCreature? GetCreatureNode(Creature? creature)
  {
    NBestiaryLayout currentLayout = this._currentLayout;
    foreach (NCreature creatureNode in (IEnumerable<NCreature>) ((currentLayout != null ? (object) currentLayout.GetCreatures() : (object) null) ?? (object) Array.Empty<NCreature>()))
    {
      if (creatureNode.Entity == creature)
        return creatureNode;
    }
    return (NCreature) null;
  }

  public Vector2 GetSideCenter()
  {
    if (this._currentLayout == null)
    {
      Log.Error("Tried to get current side center, but we're not showing anything!");
      return Vector2.Zero;
    }
    Vector2 vector2 = Vector2.Zero;
    int num = 0;
    foreach (NCreature creature in this._currentLayout.GetCreatures())
    {
      vector2 = Vector2.op_Addition(vector2, creature.VfxSpawnPosition);
      ++num;
    }
    return Vector2.op_Division(vector2, (float) num);
  }

  public Vector2 GetSideFloor()
  {
    if (this._currentLayout == null)
    {
      Log.Error("Tried to get current side floor, but we're not showing anything!");
      return Vector2.Zero;
    }
    Vector2 vector2 = Vector2.Zero;
    int num = 0;
    foreach (NCreature creature in this._currentLayout.GetCreatures())
    {
      vector2 = Vector2.op_Addition(vector2, creature.GetBottomOfHitbox());
      ++num;
    }
    return Vector2.op_Division(vector2, (float) num);
  }

  private void EnableStatsModeHotkeys()
  {
    NHotkeyManager.Instance.PushHotkeyPressedBinding(StringName.op_Implicit(NBestiary._filterLeftHotkey), new Action(this.FilterLeft));
    NHotkeyManager.Instance.PushHotkeyPressedBinding(StringName.op_Implicit(NBestiary._filterRightHotkey), new Action(this.FilterRight));
  }

  private void DisableStatsModeHotkeys()
  {
    NHotkeyManager.Instance.RemoveHotkeyPressedBinding(StringName.op_Implicit(NBestiary._filterLeftHotkey), new Action(this.FilterLeft));
    NHotkeyManager.Instance.RemoveHotkeyPressedBinding(StringName.op_Implicit(NBestiary._filterRightHotkey), new Action(this.FilterRight));
  }

  private void FilterLeft()
  {
    List<NBestiaryCharacterFilter> list = ((IEnumerable) ((Node) this._filterContainer).GetChildren(false)).OfType<NBestiaryCharacterFilter>().ToList<NBestiaryCharacterFilter>();
    int num = list.IndexOf(this._currentFilter);
    for (int index1 = 1; index1 <= list.Count; ++index1)
    {
      int index2 = (num - index1 + list.Count) % list.Count;
      if (!list[index2].IsLocked)
      {
        this.SelectFilter(list[index2]);
        break;
      }
    }
  }

  private void FilterRight()
  {
    List<NBestiaryCharacterFilter> list = ((IEnumerable) ((Node) this._filterContainer).GetChildren(false)).OfType<NBestiaryCharacterFilter>().ToList<NBestiaryCharacterFilter>();
    int num = list.IndexOf(this._currentFilter);
    for (int index1 = 1; index1 <= list.Count; ++index1)
    {
      int index2 = (num + index1) % list.Count;
      if (!list[index2].IsLocked)
      {
        this.SelectFilter(list[index2]);
        break;
      }
    }
  }

  private void SelectFilter(NBestiaryCharacterFilter filter)
  {
    filter.IsSelected = true;
    this.OnCharacterFilterSelected(filter);
  }

  private void UpdatePageIcons()
  {
    bool flag = this._isStatsMode && NControllerManager.Instance.IsUsingController;
    ((CanvasItem) this._pageLeftIcon).Visible = flag;
    ((CanvasItem) this._pageRightIcon).Visible = flag;
    if (!flag)
      return;
    this._pageLeftIcon.Texture = NInputManager.Instance.GetHotkeyIcon(StringName.op_Implicit(MegaInput.viewDeckAndTabLeft));
    this._pageRightIcon.Texture = NInputManager.Instance.GetHotkeyIcon(StringName.op_Implicit(MegaInput.viewExhaustPileAndTabRight));
  }

  public static bool CanBeShown()
  {
    return ((IReadOnlyCollection<ModelId>) SaveManager.Instance.Progress.DiscoveredActs).Count != 0 && SaveManager.Instance.Progress.EnemyStats.Values.Any<EnemyStats>((Func<EnemyStats, bool>) (e => e.TotalWins > 0));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(30)
    {
      new MethodInfo(NBestiary.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBestiary.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBestiary.MethodName.ToggleMode, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NBestiary.MethodName.EnableMoveButtonHotkeys, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBestiary.MethodName.DisableMoveButtonHotkeys, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBestiary.MethodName.ShowMovesPanel, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBestiary.MethodName.ShowStatsPanel, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBestiary.MethodName.RefreshStatisticsText, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBestiary.MethodName.DisplayCharacterData, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBestiary.MethodName.UpdateDialogueBubbleStyle, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBestiary.MethodName.ShowDialogue, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBestiary.MethodName.HideDialogue, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBestiary.MethodName.OnSubmenuOpened, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBestiary.MethodName.OnSubmenuClosed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBestiary.MethodName.CreateEntries, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBestiary.MethodName.CreateFilters, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBestiary.MethodName.OnCharacterFilterSelected, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("selectedFilter"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NBestiary.MethodName.AddEvents, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBestiary.MethodName.OnMonsterClicked, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("entry"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NBestiary.MethodName.SelectMonster, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("entry"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NBestiary.MethodName.OnMoveButtonClicked, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("button"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NBestiary.MethodName.GetSideCenter, new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBestiary.MethodName.GetSideFloor, new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBestiary.MethodName.EnableStatsModeHotkeys, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBestiary.MethodName.DisableStatsModeHotkeys, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBestiary.MethodName.FilterLeft, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBestiary.MethodName.FilterRight, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBestiary.MethodName.SelectFilter, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("filter"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NBestiary.MethodName.UpdatePageIcons, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBestiary.MethodName.CanBeShown, new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NBestiary.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NBestiary nbestiary = NBestiary.Create();
      ret = VariantUtils.CreateFrom<NBestiary>(ref nbestiary);
      return true;
    }
    if (StringName.op_Equality(ref method, NBestiary.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBestiary.MethodName.ToggleMode) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ToggleMode(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBestiary.MethodName.EnableMoveButtonHotkeys) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.EnableMoveButtonHotkeys();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBestiary.MethodName.DisableMoveButtonHotkeys) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.DisableMoveButtonHotkeys();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBestiary.MethodName.ShowMovesPanel) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ShowMovesPanel();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBestiary.MethodName.ShowStatsPanel) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ShowStatsPanel();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBestiary.MethodName.RefreshStatisticsText) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshStatisticsText();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBestiary.MethodName.DisplayCharacterData) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.DisplayCharacterData();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBestiary.MethodName.UpdateDialogueBubbleStyle) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateDialogueBubbleStyle();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBestiary.MethodName.ShowDialogue) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ShowDialogue();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBestiary.MethodName.HideDialogue) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.HideDialogue();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBestiary.MethodName.OnSubmenuOpened) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSubmenuOpened();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBestiary.MethodName.OnSubmenuClosed) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnSubmenuClosed();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBestiary.MethodName.CreateEntries) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.CreateEntries();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBestiary.MethodName.CreateFilters) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.CreateFilters();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBestiary.MethodName.OnCharacterFilterSelected) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnCharacterFilterSelected(VariantUtils.ConvertTo<NBestiaryCharacterFilter>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBestiary.MethodName.AddEvents) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AddEvents();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBestiary.MethodName.OnMonsterClicked) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnMonsterClicked(VariantUtils.ConvertTo<NBestiaryEntry>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBestiary.MethodName.SelectMonster) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SelectMonster(VariantUtils.ConvertTo<NBestiaryEntry>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBestiary.MethodName.OnMoveButtonClicked) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnMoveButtonClicked(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBestiary.MethodName.GetSideCenter) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      Vector2 sideCenter = this.GetSideCenter();
      ret = VariantUtils.CreateFrom<Vector2>(ref sideCenter);
      return true;
    }
    if (StringName.op_Equality(ref method, NBestiary.MethodName.GetSideFloor) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      Vector2 sideFloor = this.GetSideFloor();
      ret = VariantUtils.CreateFrom<Vector2>(ref sideFloor);
      return true;
    }
    if (StringName.op_Equality(ref method, NBestiary.MethodName.EnableStatsModeHotkeys) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.EnableStatsModeHotkeys();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBestiary.MethodName.DisableStatsModeHotkeys) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.DisableStatsModeHotkeys();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBestiary.MethodName.FilterLeft) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.FilterLeft();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBestiary.MethodName.FilterRight) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.FilterRight();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBestiary.MethodName.SelectFilter) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SelectFilter(VariantUtils.ConvertTo<NBestiaryCharacterFilter>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBestiary.MethodName.UpdatePageIcons) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdatePageIcons();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NBestiary.MethodName.CanBeShown) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    bool flag = NBestiary.CanBeShown();
    ret = VariantUtils.CreateFrom<bool>(ref flag);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NBestiary.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NBestiary nbestiary = NBestiary.Create();
      ret = VariantUtils.CreateFrom<NBestiary>(ref nbestiary);
      return true;
    }
    if (StringName.op_Equality(ref method, NBestiary.MethodName.CanBeShown) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      bool flag = NBestiary.CanBeShown();
      ret = VariantUtils.CreateFrom<bool>(ref flag);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NBestiary.MethodName.Create) || StringName.op_Equality(ref method, NBestiary.MethodName._Ready) || StringName.op_Equality(ref method, NBestiary.MethodName.ToggleMode) || StringName.op_Equality(ref method, NBestiary.MethodName.EnableMoveButtonHotkeys) || StringName.op_Equality(ref method, NBestiary.MethodName.DisableMoveButtonHotkeys) || StringName.op_Equality(ref method, NBestiary.MethodName.ShowMovesPanel) || StringName.op_Equality(ref method, NBestiary.MethodName.ShowStatsPanel) || StringName.op_Equality(ref method, NBestiary.MethodName.RefreshStatisticsText) || StringName.op_Equality(ref method, NBestiary.MethodName.DisplayCharacterData) || StringName.op_Equality(ref method, NBestiary.MethodName.UpdateDialogueBubbleStyle) || StringName.op_Equality(ref method, NBestiary.MethodName.ShowDialogue) || StringName.op_Equality(ref method, NBestiary.MethodName.HideDialogue) || StringName.op_Equality(ref method, NBestiary.MethodName.OnSubmenuOpened) || StringName.op_Equality(ref method, NBestiary.MethodName.OnSubmenuClosed) || StringName.op_Equality(ref method, NBestiary.MethodName.CreateEntries) || StringName.op_Equality(ref method, NBestiary.MethodName.CreateFilters) || StringName.op_Equality(ref method, NBestiary.MethodName.OnCharacterFilterSelected) || StringName.op_Equality(ref method, NBestiary.MethodName.AddEvents) || StringName.op_Equality(ref method, NBestiary.MethodName.OnMonsterClicked) || StringName.op_Equality(ref method, NBestiary.MethodName.SelectMonster) || StringName.op_Equality(ref method, NBestiary.MethodName.OnMoveButtonClicked) || StringName.op_Equality(ref method, NBestiary.MethodName.GetSideCenter) || StringName.op_Equality(ref method, NBestiary.MethodName.GetSideFloor) || StringName.op_Equality(ref method, NBestiary.MethodName.EnableStatsModeHotkeys) || StringName.op_Equality(ref method, NBestiary.MethodName.DisableStatsModeHotkeys) || StringName.op_Equality(ref method, NBestiary.MethodName.FilterLeft) || StringName.op_Equality(ref method, NBestiary.MethodName.FilterRight) || StringName.op_Equality(ref method, NBestiary.MethodName.SelectFilter) || StringName.op_Equality(ref method, NBestiary.MethodName.UpdatePageIcons) || StringName.op_Equality(ref method, NBestiary.MethodName.CanBeShown) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NBestiary.PropertyName.BackVfxContainer))
    {
      this.BackVfxContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName.VfxContainer))
    {
      this.VfxContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._monsterNameLabel))
    {
      this._monsterNameLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._epithet))
    {
      this._epithet = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._sidebar))
    {
      this._sidebar = VariantUtils.ConvertTo<NScrollableContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._bestiaryList))
    {
      this._bestiaryList = VariantUtils.ConvertTo<VBoxContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._selectionArrow))
    {
      this._selectionArrow = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._arrowTween))
    {
      this._arrowTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._initSelectionArrow))
    {
      this._initSelectionArrow = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._layoutContainer))
    {
      this._layoutContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._currentLayout))
    {
      this._currentLayout = VariantUtils.ConvertTo<NBestiaryLayout>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._characterIcon))
    {
      this._characterIcon = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._iconTexture))
    {
      this._iconTexture = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._iconOutlineTexture))
    {
      this._iconOutlineTexture = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._dialogueLine))
    {
      this._dialogueLine = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._dialogueLabel))
    {
      this._dialogueLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._dialogueBubble))
    {
      this._dialogueBubble = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._dialogueTail))
    {
      this._dialogueTail = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._dialogueTailShadow))
    {
      this._dialogueTailShadow = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._modeButton))
    {
      this._modeButton = VariantUtils.ConvertTo<NButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._modeLabel))
    {
      this._modeLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._isStatsMode))
    {
      this._isStatsMode = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._pageLeftIcon))
    {
      this._pageLeftIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._pageRightIcon))
    {
      this._pageRightIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._moveList))
    {
      this._moveList = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._moveContainer))
    {
      this._moveContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._statsContainer))
    {
      this._statsContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._filterContainer))
    {
      this._filterContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._statsLabel))
    {
      this._statsLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._currentFilter))
    {
      this._currentFilter = VariantUtils.ConvertTo<NBestiaryCharacterFilter>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._selectedEntry))
    {
      this._selectedEntry = VariantUtils.ConvertTo<NBestiaryEntry>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._previousScreenshakeTarget))
    {
      this._previousScreenshakeTarget = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NBestiary.PropertyName._dialogueTween))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._dialogueTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NBestiary.PropertyName.InitialFocusedControl))
    {
      ref godot_variant local = ref value;
      Control initialFocusedControl = this.InitialFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref initialFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName.BackVfxContainer))
    {
      ref godot_variant local = ref value;
      Control backVfxContainer = this.BackVfxContainer;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref backVfxContainer);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName.VfxContainer))
    {
      ref godot_variant local = ref value;
      Control vfxContainer = this.VfxContainer;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref vfxContainer);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName.Layout))
    {
      ref godot_variant local = ref value;
      Control layout = this.Layout;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref layout);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._monsterNameLabel))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._monsterNameLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._epithet))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._epithet);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._sidebar))
    {
      value = VariantUtils.CreateFrom<NScrollableContainer>(ref this._sidebar);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._bestiaryList))
    {
      value = VariantUtils.CreateFrom<VBoxContainer>(ref this._bestiaryList);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._selectionArrow))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._selectionArrow);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._arrowTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._arrowTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._initSelectionArrow))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._initSelectionArrow);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._layoutContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._layoutContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._currentLayout))
    {
      value = VariantUtils.CreateFrom<NBestiaryLayout>(ref this._currentLayout);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._characterIcon))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._characterIcon);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._iconTexture))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._iconTexture);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._iconOutlineTexture))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._iconOutlineTexture);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._dialogueLine))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._dialogueLine);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._dialogueLabel))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._dialogueLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._dialogueBubble))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._dialogueBubble);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._dialogueTail))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._dialogueTail);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._dialogueTailShadow))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._dialogueTailShadow);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._modeButton))
    {
      value = VariantUtils.CreateFrom<NButton>(ref this._modeButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._modeLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._modeLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._isStatsMode))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isStatsMode);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._pageLeftIcon))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._pageLeftIcon);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._pageRightIcon))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._pageRightIcon);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._moveList))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._moveList);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._moveContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._moveContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._statsContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._statsContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._filterContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._filterContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._statsLabel))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._statsLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._currentFilter))
    {
      value = VariantUtils.CreateFrom<NBestiaryCharacterFilter>(ref this._currentFilter);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._selectedEntry))
    {
      value = VariantUtils.CreateFrom<NBestiaryEntry>(ref this._selectedEntry);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._previousScreenshakeTarget))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._previousScreenshakeTarget);
      return true;
    }
    if (StringName.op_Equality(ref name, NBestiary.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NBestiary.PropertyName._dialogueTween))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Tween>(ref this._dialogueTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NBestiary.PropertyName._monsterNameLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiary.PropertyName._epithet, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiary.PropertyName.InitialFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiary.PropertyName._sidebar, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiary.PropertyName._bestiaryList, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiary.PropertyName._selectionArrow, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiary.PropertyName._arrowTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NBestiary.PropertyName._initSelectionArrow, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiary.PropertyName._layoutContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiary.PropertyName._currentLayout, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiary.PropertyName._characterIcon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiary.PropertyName._iconTexture, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiary.PropertyName._iconOutlineTexture, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiary.PropertyName._dialogueLine, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiary.PropertyName._dialogueLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiary.PropertyName._dialogueBubble, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiary.PropertyName._dialogueTail, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiary.PropertyName._dialogueTailShadow, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiary.PropertyName._modeButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiary.PropertyName._modeLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NBestiary.PropertyName._isStatsMode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiary.PropertyName._pageLeftIcon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiary.PropertyName._pageRightIcon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiary.PropertyName._moveList, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiary.PropertyName._moveContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiary.PropertyName._statsContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiary.PropertyName._filterContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiary.PropertyName._statsLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiary.PropertyName._currentFilter, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiary.PropertyName._selectedEntry, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiary.PropertyName._previousScreenshakeTarget, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiary.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiary.PropertyName._dialogueTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiary.PropertyName.BackVfxContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiary.PropertyName.VfxContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBestiary.PropertyName.Layout, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo1 = info;
    StringName backVfxContainer1 = NBestiary.PropertyName.BackVfxContainer;
    Control backVfxContainer2 = this.BackVfxContainer;
    Variant variant1 = Variant.From<Control>(ref backVfxContainer2);
    serializationInfo1.AddProperty(backVfxContainer1, variant1);
    GodotSerializationInfo serializationInfo2 = info;
    StringName vfxContainer1 = NBestiary.PropertyName.VfxContainer;
    Control vfxContainer2 = this.VfxContainer;
    Variant variant2 = Variant.From<Control>(ref vfxContainer2);
    serializationInfo2.AddProperty(vfxContainer1, variant2);
    info.AddProperty(NBestiary.PropertyName._monsterNameLabel, Variant.From<MegaRichTextLabel>(ref this._monsterNameLabel));
    info.AddProperty(NBestiary.PropertyName._epithet, Variant.From<MegaLabel>(ref this._epithet));
    info.AddProperty(NBestiary.PropertyName._sidebar, Variant.From<NScrollableContainer>(ref this._sidebar));
    info.AddProperty(NBestiary.PropertyName._bestiaryList, Variant.From<VBoxContainer>(ref this._bestiaryList));
    info.AddProperty(NBestiary.PropertyName._selectionArrow, Variant.From<Control>(ref this._selectionArrow));
    info.AddProperty(NBestiary.PropertyName._arrowTween, Variant.From<Tween>(ref this._arrowTween));
    info.AddProperty(NBestiary.PropertyName._initSelectionArrow, Variant.From<bool>(ref this._initSelectionArrow));
    info.AddProperty(NBestiary.PropertyName._layoutContainer, Variant.From<Control>(ref this._layoutContainer));
    info.AddProperty(NBestiary.PropertyName._currentLayout, Variant.From<NBestiaryLayout>(ref this._currentLayout));
    info.AddProperty(NBestiary.PropertyName._characterIcon, Variant.From<Control>(ref this._characterIcon));
    info.AddProperty(NBestiary.PropertyName._iconTexture, Variant.From<TextureRect>(ref this._iconTexture));
    info.AddProperty(NBestiary.PropertyName._iconOutlineTexture, Variant.From<TextureRect>(ref this._iconOutlineTexture));
    info.AddProperty(NBestiary.PropertyName._dialogueLine, Variant.From<Control>(ref this._dialogueLine));
    info.AddProperty(NBestiary.PropertyName._dialogueLabel, Variant.From<MegaRichTextLabel>(ref this._dialogueLabel));
    info.AddProperty(NBestiary.PropertyName._dialogueBubble, Variant.From<Control>(ref this._dialogueBubble));
    info.AddProperty(NBestiary.PropertyName._dialogueTail, Variant.From<TextureRect>(ref this._dialogueTail));
    info.AddProperty(NBestiary.PropertyName._dialogueTailShadow, Variant.From<TextureRect>(ref this._dialogueTailShadow));
    info.AddProperty(NBestiary.PropertyName._modeButton, Variant.From<NButton>(ref this._modeButton));
    info.AddProperty(NBestiary.PropertyName._modeLabel, Variant.From<MegaLabel>(ref this._modeLabel));
    info.AddProperty(NBestiary.PropertyName._isStatsMode, Variant.From<bool>(ref this._isStatsMode));
    info.AddProperty(NBestiary.PropertyName._pageLeftIcon, Variant.From<TextureRect>(ref this._pageLeftIcon));
    info.AddProperty(NBestiary.PropertyName._pageRightIcon, Variant.From<TextureRect>(ref this._pageRightIcon));
    info.AddProperty(NBestiary.PropertyName._moveList, Variant.From<Control>(ref this._moveList));
    info.AddProperty(NBestiary.PropertyName._moveContainer, Variant.From<Control>(ref this._moveContainer));
    info.AddProperty(NBestiary.PropertyName._statsContainer, Variant.From<Control>(ref this._statsContainer));
    info.AddProperty(NBestiary.PropertyName._filterContainer, Variant.From<Control>(ref this._filterContainer));
    info.AddProperty(NBestiary.PropertyName._statsLabel, Variant.From<MegaRichTextLabel>(ref this._statsLabel));
    info.AddProperty(NBestiary.PropertyName._currentFilter, Variant.From<NBestiaryCharacterFilter>(ref this._currentFilter));
    info.AddProperty(NBestiary.PropertyName._selectedEntry, Variant.From<NBestiaryEntry>(ref this._selectedEntry));
    info.AddProperty(NBestiary.PropertyName._previousScreenshakeTarget, Variant.From<Control>(ref this._previousScreenshakeTarget));
    info.AddProperty(NBestiary.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NBestiary.PropertyName._dialogueTween, Variant.From<Tween>(ref this._dialogueTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NBestiary.PropertyName.BackVfxContainer, ref variant1))
      this.BackVfxContainer = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NBestiary.PropertyName.VfxContainer, ref variant2))
      this.VfxContainer = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NBestiary.PropertyName._monsterNameLabel, ref variant3))
      this._monsterNameLabel = ((Variant) ref variant3).As<MegaRichTextLabel>();
    Variant variant4;
    if (info.TryGetProperty(NBestiary.PropertyName._epithet, ref variant4))
      this._epithet = ((Variant) ref variant4).As<MegaLabel>();
    Variant variant5;
    if (info.TryGetProperty(NBestiary.PropertyName._sidebar, ref variant5))
      this._sidebar = ((Variant) ref variant5).As<NScrollableContainer>();
    Variant variant6;
    if (info.TryGetProperty(NBestiary.PropertyName._bestiaryList, ref variant6))
      this._bestiaryList = ((Variant) ref variant6).As<VBoxContainer>();
    Variant variant7;
    if (info.TryGetProperty(NBestiary.PropertyName._selectionArrow, ref variant7))
      this._selectionArrow = ((Variant) ref variant7).As<Control>();
    Variant variant8;
    if (info.TryGetProperty(NBestiary.PropertyName._arrowTween, ref variant8))
      this._arrowTween = ((Variant) ref variant8).As<Tween>();
    Variant variant9;
    if (info.TryGetProperty(NBestiary.PropertyName._initSelectionArrow, ref variant9))
      this._initSelectionArrow = ((Variant) ref variant9).As<bool>();
    Variant variant10;
    if (info.TryGetProperty(NBestiary.PropertyName._layoutContainer, ref variant10))
      this._layoutContainer = ((Variant) ref variant10).As<Control>();
    Variant variant11;
    if (info.TryGetProperty(NBestiary.PropertyName._currentLayout, ref variant11))
      this._currentLayout = ((Variant) ref variant11).As<NBestiaryLayout>();
    Variant variant12;
    if (info.TryGetProperty(NBestiary.PropertyName._characterIcon, ref variant12))
      this._characterIcon = ((Variant) ref variant12).As<Control>();
    Variant variant13;
    if (info.TryGetProperty(NBestiary.PropertyName._iconTexture, ref variant13))
      this._iconTexture = ((Variant) ref variant13).As<TextureRect>();
    Variant variant14;
    if (info.TryGetProperty(NBestiary.PropertyName._iconOutlineTexture, ref variant14))
      this._iconOutlineTexture = ((Variant) ref variant14).As<TextureRect>();
    Variant variant15;
    if (info.TryGetProperty(NBestiary.PropertyName._dialogueLine, ref variant15))
      this._dialogueLine = ((Variant) ref variant15).As<Control>();
    Variant variant16;
    if (info.TryGetProperty(NBestiary.PropertyName._dialogueLabel, ref variant16))
      this._dialogueLabel = ((Variant) ref variant16).As<MegaRichTextLabel>();
    Variant variant17;
    if (info.TryGetProperty(NBestiary.PropertyName._dialogueBubble, ref variant17))
      this._dialogueBubble = ((Variant) ref variant17).As<Control>();
    Variant variant18;
    if (info.TryGetProperty(NBestiary.PropertyName._dialogueTail, ref variant18))
      this._dialogueTail = ((Variant) ref variant18).As<TextureRect>();
    Variant variant19;
    if (info.TryGetProperty(NBestiary.PropertyName._dialogueTailShadow, ref variant19))
      this._dialogueTailShadow = ((Variant) ref variant19).As<TextureRect>();
    Variant variant20;
    if (info.TryGetProperty(NBestiary.PropertyName._modeButton, ref variant20))
      this._modeButton = ((Variant) ref variant20).As<NButton>();
    Variant variant21;
    if (info.TryGetProperty(NBestiary.PropertyName._modeLabel, ref variant21))
      this._modeLabel = ((Variant) ref variant21).As<MegaLabel>();
    Variant variant22;
    if (info.TryGetProperty(NBestiary.PropertyName._isStatsMode, ref variant22))
      this._isStatsMode = ((Variant) ref variant22).As<bool>();
    Variant variant23;
    if (info.TryGetProperty(NBestiary.PropertyName._pageLeftIcon, ref variant23))
      this._pageLeftIcon = ((Variant) ref variant23).As<TextureRect>();
    Variant variant24;
    if (info.TryGetProperty(NBestiary.PropertyName._pageRightIcon, ref variant24))
      this._pageRightIcon = ((Variant) ref variant24).As<TextureRect>();
    Variant variant25;
    if (info.TryGetProperty(NBestiary.PropertyName._moveList, ref variant25))
      this._moveList = ((Variant) ref variant25).As<Control>();
    Variant variant26;
    if (info.TryGetProperty(NBestiary.PropertyName._moveContainer, ref variant26))
      this._moveContainer = ((Variant) ref variant26).As<Control>();
    Variant variant27;
    if (info.TryGetProperty(NBestiary.PropertyName._statsContainer, ref variant27))
      this._statsContainer = ((Variant) ref variant27).As<Control>();
    Variant variant28;
    if (info.TryGetProperty(NBestiary.PropertyName._filterContainer, ref variant28))
      this._filterContainer = ((Variant) ref variant28).As<Control>();
    Variant variant29;
    if (info.TryGetProperty(NBestiary.PropertyName._statsLabel, ref variant29))
      this._statsLabel = ((Variant) ref variant29).As<MegaRichTextLabel>();
    Variant variant30;
    if (info.TryGetProperty(NBestiary.PropertyName._currentFilter, ref variant30))
      this._currentFilter = ((Variant) ref variant30).As<NBestiaryCharacterFilter>();
    Variant variant31;
    if (info.TryGetProperty(NBestiary.PropertyName._selectedEntry, ref variant31))
      this._selectedEntry = ((Variant) ref variant31).As<NBestiaryEntry>();
    Variant variant32;
    if (info.TryGetProperty(NBestiary.PropertyName._previousScreenshakeTarget, ref variant32))
      this._previousScreenshakeTarget = ((Variant) ref variant32).As<Control>();
    Variant variant33;
    if (info.TryGetProperty(NBestiary.PropertyName._tween, ref variant33))
      this._tween = ((Variant) ref variant33).As<Tween>();
    Variant variant34;
    if (!info.TryGetProperty(NBestiary.PropertyName._dialogueTween, ref variant34))
      return;
    this._dialogueTween = ((Variant) ref variant34).As<Tween>();
  }

  public new class MethodName : NSubmenu.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName ToggleMode = StringName.op_Implicit(nameof (ToggleMode));
    public static readonly StringName EnableMoveButtonHotkeys = StringName.op_Implicit(nameof (EnableMoveButtonHotkeys));
    public static readonly StringName DisableMoveButtonHotkeys = StringName.op_Implicit(nameof (DisableMoveButtonHotkeys));
    public static readonly StringName ShowMovesPanel = StringName.op_Implicit(nameof (ShowMovesPanel));
    public static readonly StringName ShowStatsPanel = StringName.op_Implicit(nameof (ShowStatsPanel));
    public static readonly StringName RefreshStatisticsText = StringName.op_Implicit(nameof (RefreshStatisticsText));
    public static readonly StringName DisplayCharacterData = StringName.op_Implicit(nameof (DisplayCharacterData));
    public static readonly StringName UpdateDialogueBubbleStyle = StringName.op_Implicit(nameof (UpdateDialogueBubbleStyle));
    public static readonly StringName ShowDialogue = StringName.op_Implicit(nameof (ShowDialogue));
    public static readonly StringName HideDialogue = StringName.op_Implicit(nameof (HideDialogue));
    public new static readonly StringName OnSubmenuOpened = StringName.op_Implicit(nameof (OnSubmenuOpened));
    public new static readonly StringName OnSubmenuClosed = StringName.op_Implicit(nameof (OnSubmenuClosed));
    public static readonly StringName CreateEntries = StringName.op_Implicit(nameof (CreateEntries));
    public static readonly StringName CreateFilters = StringName.op_Implicit(nameof (CreateFilters));
    public static readonly StringName OnCharacterFilterSelected = StringName.op_Implicit(nameof (OnCharacterFilterSelected));
    public static readonly StringName AddEvents = StringName.op_Implicit(nameof (AddEvents));
    public static readonly StringName OnMonsterClicked = StringName.op_Implicit(nameof (OnMonsterClicked));
    public static readonly StringName SelectMonster = StringName.op_Implicit(nameof (SelectMonster));
    public static readonly StringName OnMoveButtonClicked = StringName.op_Implicit(nameof (OnMoveButtonClicked));
    public static readonly StringName GetSideCenter = StringName.op_Implicit(nameof (GetSideCenter));
    public static readonly StringName GetSideFloor = StringName.op_Implicit(nameof (GetSideFloor));
    public static readonly StringName EnableStatsModeHotkeys = StringName.op_Implicit(nameof (EnableStatsModeHotkeys));
    public static readonly StringName DisableStatsModeHotkeys = StringName.op_Implicit(nameof (DisableStatsModeHotkeys));
    public static readonly StringName FilterLeft = StringName.op_Implicit(nameof (FilterLeft));
    public static readonly StringName FilterRight = StringName.op_Implicit(nameof (FilterRight));
    public static readonly StringName SelectFilter = StringName.op_Implicit(nameof (SelectFilter));
    public static readonly StringName UpdatePageIcons = StringName.op_Implicit(nameof (UpdatePageIcons));
    public static readonly StringName CanBeShown = StringName.op_Implicit(nameof (CanBeShown));
  }

  public new class PropertyName : NSubmenu.PropertyName
  {
    public new static readonly StringName InitialFocusedControl = StringName.op_Implicit(nameof (InitialFocusedControl));
    public static readonly StringName BackVfxContainer = StringName.op_Implicit(nameof (BackVfxContainer));
    public static readonly StringName VfxContainer = StringName.op_Implicit(nameof (VfxContainer));
    public static readonly StringName Layout = StringName.op_Implicit(nameof (Layout));
    public static readonly StringName _monsterNameLabel = StringName.op_Implicit(nameof (_monsterNameLabel));
    public static readonly StringName _epithet = StringName.op_Implicit(nameof (_epithet));
    public static readonly StringName _sidebar = StringName.op_Implicit(nameof (_sidebar));
    public static readonly StringName _bestiaryList = StringName.op_Implicit(nameof (_bestiaryList));
    public static readonly StringName _selectionArrow = StringName.op_Implicit(nameof (_selectionArrow));
    public static readonly StringName _arrowTween = StringName.op_Implicit(nameof (_arrowTween));
    public static readonly StringName _initSelectionArrow = StringName.op_Implicit(nameof (_initSelectionArrow));
    public static readonly StringName _layoutContainer = StringName.op_Implicit(nameof (_layoutContainer));
    public static readonly StringName _currentLayout = StringName.op_Implicit(nameof (_currentLayout));
    public static readonly StringName _characterIcon = StringName.op_Implicit(nameof (_characterIcon));
    public static readonly StringName _iconTexture = StringName.op_Implicit(nameof (_iconTexture));
    public static readonly StringName _iconOutlineTexture = StringName.op_Implicit(nameof (_iconOutlineTexture));
    public static readonly StringName _dialogueLine = StringName.op_Implicit(nameof (_dialogueLine));
    public static readonly StringName _dialogueLabel = StringName.op_Implicit(nameof (_dialogueLabel));
    public static readonly StringName _dialogueBubble = StringName.op_Implicit(nameof (_dialogueBubble));
    public static readonly StringName _dialogueTail = StringName.op_Implicit(nameof (_dialogueTail));
    public static readonly StringName _dialogueTailShadow = StringName.op_Implicit(nameof (_dialogueTailShadow));
    public static readonly StringName _modeButton = StringName.op_Implicit(nameof (_modeButton));
    public static readonly StringName _modeLabel = StringName.op_Implicit(nameof (_modeLabel));
    public static readonly StringName _isStatsMode = StringName.op_Implicit(nameof (_isStatsMode));
    public static readonly StringName _pageLeftIcon = StringName.op_Implicit(nameof (_pageLeftIcon));
    public static readonly StringName _pageRightIcon = StringName.op_Implicit(nameof (_pageRightIcon));
    public static readonly StringName _moveList = StringName.op_Implicit(nameof (_moveList));
    public static readonly StringName _moveContainer = StringName.op_Implicit(nameof (_moveContainer));
    public static readonly StringName _statsContainer = StringName.op_Implicit(nameof (_statsContainer));
    public static readonly StringName _filterContainer = StringName.op_Implicit(nameof (_filterContainer));
    public static readonly StringName _statsLabel = StringName.op_Implicit(nameof (_statsLabel));
    public static readonly StringName _currentFilter = StringName.op_Implicit(nameof (_currentFilter));
    public static readonly StringName _selectedEntry = StringName.op_Implicit(nameof (_selectedEntry));
    public static readonly StringName _previousScreenshakeTarget = StringName.op_Implicit(nameof (_previousScreenshakeTarget));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _dialogueTween = StringName.op_Implicit(nameof (_dialogueTween));
  }

  public new class SignalName : NSubmenu.SignalName
  {
  }
}
