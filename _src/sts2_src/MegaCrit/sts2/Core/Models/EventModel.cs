// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.EventModel
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Nodes.Events;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models;

public abstract class EventModel : AbstractModel
{
  protected const string _initialPageKey = "INITIAL";
  protected EventCombatSynchronizer? _combatSynchronizer;
  private List<EventOption>? _currentOptions;
  private bool _isFinished;
  private bool _cleanupCalled;
  private DynamicVarSet? _dynamicVars;
  private EventModel _canonicalInstance;

  public virtual Color ButtonColor => new Color(1f, 1f, 1f, 0.9f);

  public virtual bool IsDeterministic => !this.IsShared;

  public override bool ShouldReceiveCombatHooks => false;

  public virtual string LocTable => "events";

  public LocString Title => this.L10NLookup(this.Id.Entry + ".title");

  public virtual LocString InitialDescription
  {
    get => this.L10NLookup(this.Id.Entry + ".pages.INITIAL.description");
  }

  public LocString? GetOptionTitle(string key)
  {
    return LocString.GetIfExists(this.LocTable, key + ".title");
  }

  public LocString? GetOptionDescription(string key)
  {
    return LocString.GetIfExists(this.LocTable, key + ".description");
  }

  public Player? Owner { get; private set; }

  public virtual bool IsShared => false;

  public LocString? Description { get; private set; }

  public virtual EncounterModel? CanonicalEncounter => (EncounterModel) null;

  public async Task BeginEvent(
    Player player,
    EventCombatSynchronizer? combatSynchronizer,
    bool isPreFinished)
  {
    this.AssertMutable();
    this.Owner = this.Owner == null ? player : throw new InvalidOperationException("Tried to begin event, but it already has an owner!");
    this.Rng = new Rng(this.Owner.RunState.Rng.Seed + (this.IsShared ? 0UL : (ulong) this.Owner.RunState.GetPlayerSlotIndex(this.Owner)) + StringHelper.GetDeterministicHashCode(this.Id.Entry));
    this._combatSynchronizer = this.CanonicalEncounter == null || combatSynchronizer != null ? combatSynchronizer : throw new InvalidOperationException("Combat synchronizer must be passed to events that may transition to combat!");
    try
    {
      await this.BeforeEventStarted(isPreFinished);
      this.CalculateVars();
      if (player.Creature.IsDead)
      {
        Log.Error("The generic event death message should not appear!");
        this.SetEventFinished(this.L10NLookup("GENERIC.youAreDead.description"));
      }
      else
        this.SetInitialEventState(isPreFinished);
    }
    catch
    {
      this.EnsureCleanup();
      throw;
    }
  }

  protected virtual void SetInitialEventState(bool isPreFinished)
  {
    if (isPreFinished && !(this is AncientEventModel))
      throw new InvalidOperationException($"Tried to load into pre-finished event {this}! Only ancient events can be pre-finished.");
    this.SetEventState(this.InitialDescription, (IEnumerable<EventOption>) this.GenerateInitialOptionsWrapper());
  }

  protected virtual IReadOnlyList<EventOption> GenerateInitialOptionsWrapper()
  {
    this.AssertMutable();
    List<EventOption> list = this.GenerateInitialOptions().ToList<EventOption>();
    this.ReplaceNullOptions(list);
    return (IReadOnlyList<EventOption>) list;
  }

  protected void ReplaceNullOptions(List<EventOption> options)
  {
    for (int index = 0; index < options.Count; ++index)
    {
      if (options[index] == null)
      {
        string str = $"Event {this.Id.Entry} has a null option at index {index}!";
        Log.Error(str);
        SentryService.CaptureException((Exception) new NullReferenceException(str));
        EventOption eventOption = new EventOption(this, (Func<Task>) null, "ERROR", Array.Empty<IHoverTip>());
        options[index] = eventOption;
      }
    }
  }

  protected abstract IReadOnlyList<EventOption> GenerateInitialOptions();

  public bool IsFinished
  {
    get => this._isFinished;
    private set
    {
      this.AssertMutable();
      this._isFinished = value;
    }
  }

  public IReadOnlyList<EventOption> CurrentOptions
  {
    get
    {
      this.AssertMutable();
      if (this._currentOptions == null)
        this._currentOptions = new List<EventOption>();
      return (IReadOnlyList<EventOption>) this._currentOptions;
    }
  }

  protected void ClearCurrentOptions()
  {
    this.AssertMutable();
    if (this._currentOptions == null)
      this._currentOptions = new List<EventOption>();
    this._currentOptions.Clear();
  }

  public event Action<EventModel>? StateChanged;

  public event Action? EnteringEventCombat;

  public DynamicVarSet DynamicVars
  {
    get
    {
      if (this._dynamicVars != null)
        return this._dynamicVars;
      this._dynamicVars = new DynamicVarSet(this.CanonicalVars);
      this._dynamicVars.InitializeWithOwner((AbstractModel) this);
      return this._dynamicVars;
    }
  }

  protected virtual IEnumerable<DynamicVar> CanonicalVars
  {
    get => (IEnumerable<DynamicVar>) Array.Empty<DynamicVar>();
  }

  public virtual bool IsAllowed(IRunState runState) => true;

  public Rng Rng { get; private set; }

  public virtual IEnumerable<LocString> GameInfoOptions
  {
    get
    {
      List<LocString> list = LocManager.Instance.GetTable(this.LocTable).Keys.Where<string>((Func<string, bool>) (k => k.StartsWith(this.Id.Entry + ".pages.INITIAL.options"))).Select<string, LocString>((Func<string, LocString>) (k => new LocString(this.LocTable, k))).ToList<LocString>();
      if (list.Count == 0)
        throw new LocException($"Event Loc for {this.Id.Entry} does not conform to the common format");
      foreach (LocString str in list)
        this.DynamicVars.AddTo(str);
      return (IEnumerable<LocString>) list;
    }
  }

  public virtual EventLayoutType LayoutType => EventLayoutType.Default;

  public PackedScene CreateScene() => PreloadManager.Cache.GetScene(this.LayoutScenePath);

  public Control? Node { get; private set; }

  public void SetNode(Control node)
  {
    this.AssertMutable();
    this.Node = this.Node == null ? node : throw new InvalidOperationException("Tried to set node, but it has already been set!");
    if (this.LayoutType != EventLayoutType.Custom)
      return;
    ((ICustomEventNode) this.Node).Initialize(this);
  }

  private string LayoutScenePath
  {
    get
    {
      switch (this.LayoutType)
      {
        case EventLayoutType.Default:
          return "res://scenes/events/default_event_layout.tscn";
        case EventLayoutType.Combat:
          return "res://scenes/events/combat_event_layout.tscn";
        case EventLayoutType.Ancient:
          return "res://scenes/events/ancient_event_layout.tscn";
        case EventLayoutType.Custom:
          return SceneHelper.GetScenePath("events/custom/" + this.Id.Entry.ToLowerInvariant());
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
  }

  public EventModel CanonicalInstance
  {
    get => !this.IsMutable ? this : this._canonicalInstance;
    private set
    {
      this.AssertMutable();
      this._canonicalInstance = value;
    }
  }

  private string InitialPortraitPath
  {
    get => ImageHelper.GetImagePath($"events/{this.Id.Entry.ToLowerInvariant()}.png");
  }

  private string InitialPhobiaModePortraitPath
  {
    get => ImageHelper.GetImagePath($"events/{this.Id.Entry.ToLowerInvariant()}_phobia_mode.png");
  }

  public bool HasPhobiaModePortrait
  {
    get => ResourceLoader.Exists(this.InitialPhobiaModePortraitPath, "");
  }

  private string BackgroundScenePath
  {
    get => SceneHelper.GetScenePath("events/background_scenes/" + this.Id.Entry.ToLowerInvariant());
  }

  private string VfxPath
  {
    get => SceneHelper.GetScenePath($"vfx/events/{this.Id.Entry.ToLowerInvariant()}_vfx");
  }

  public bool HasVfx => ResourceLoader.Exists(this.VfxPath, "");

  public static Vector2 VfxOffset => new Vector2(268f, 49f);

  public Texture2D CreateInitialPortrait()
  {
    return PreloadManager.Cache.GetTexture2D(this.InitialPortraitPath);
  }

  public Texture2D CreateInitialPhobiaModePortrait()
  {
    return PreloadManager.Cache.GetTexture2D(this.InitialPhobiaModePortraitPath);
  }

  public PackedScene CreateBackgroundScene()
  {
    return PreloadManager.Cache.GetScene(this.BackgroundScenePath);
  }

  public Node2D CreateVfx()
  {
    return PreloadManager.Cache.GetScene(this.VfxPath).Instantiate<Node2D>((PackedScene.GenEditState) 0L);
  }

  public ICombatRoomVisuals CreateCombatRoomVisuals(IEnumerable<Player> players, ActModel act)
  {
    if (this.LayoutType != EventLayoutType.Combat)
      throw new InvalidOperationException("Tried to create combat room visuals for non-combat event!");
    return (ICombatRoomVisuals) new CombatEventVisuals(this._combatSynchronizer.MutableEncounterForLayout, players, act);
  }

  public EventModel ToMutable()
  {
    this.AssertCanonical();
    EventModel mutable = (EventModel) this.MutableClone();
    mutable.CanonicalInstance = this;
    return mutable;
  }

  protected override void DeepCloneFields()
  {
    base.DeepCloneFields();
    this._dynamicVars = this.DynamicVars.Clone((AbstractModel) this);
  }

  protected override void AfterCloned()
  {
    base.AfterCloned();
    this.StateChanged = (Action<EventModel>) null;
    this.EnteringEventCombat = (Action) null;
    this._currentOptions = (List<EventOption>) null;
  }

  public virtual void CalculateVars()
  {
  }

  protected LocString L10NLookup(string entryName) => new LocString(this.LocTable, entryName);

  public virtual IEnumerable<string> GetAssetPaths(IRunState runState)
  {
    if (TestMode.IsOn)
      return (IEnumerable<string>) Array.Empty<string>();
    int capacity = 1;
    List<string> stringList = new List<string>(capacity);
    CollectionsMarshal.SetCount<string>(stringList, capacity);
    CollectionsMarshal.AsSpan<string>(stringList)[0] = this.LayoutScenePath;
    List<string> assetPaths = stringList;
    switch (this.LayoutType)
    {
      case EventLayoutType.Default:
        assetPaths.Add(this.InitialPortraitPath);
        if (this.HasPhobiaModePortrait)
          assetPaths.Add(this.InitialPhobiaModePortraitPath);
        if (this.HasVfx)
        {
          assetPaths.Add(this.VfxPath);
          break;
        }
        break;
      case EventLayoutType.Combat:
        assetPaths.AddRange(NCombatRoom.AssetPaths);
        if (this._combatSynchronizer?.MutableEncounterForLayout != null)
        {
          assetPaths.AddRange(this._combatSynchronizer.MutableEncounterForLayout.GetAssetPaths(runState));
          break;
        }
        break;
      case EventLayoutType.Ancient:
        assetPaths.Add(this.BackgroundScenePath);
        break;
    }
    return (IEnumerable<string>) assetPaths;
  }

  public virtual void OnRoomEnter()
  {
  }

  public virtual Task Resume(AbstractRoom exitedRoom) => Task.CompletedTask;

  protected void SetEventFinished(LocString description)
  {
    this.SetEventState(description, (IEnumerable<EventOption>) Array.Empty<EventOption>());
    this.IsFinished = true;
    this.EnsureCleanup();
  }

  protected virtual Task BeforeEventStarted(bool isPreFinished) => Task.CompletedTask;

  public virtual Task AfterEventStarted() => Task.CompletedTask;

  protected virtual void OnEventFinished()
  {
  }

  public void EnsureCleanup()
  {
    if (this._cleanupCalled)
      return;
    this._cleanupCalled = true;
    this.OnEventFinished();
  }

  protected virtual void SetEventState(LocString description, IEnumerable<EventOption> eventOptions)
  {
    this.AssertMutable();
    if (this._currentOptions == null)
      this._currentOptions = new List<EventOption>();
    this._currentOptions.Clear();
    this._currentOptions.AddRange(eventOptions);
    this.Description = description;
    if (this._currentOptions.Count == 0)
      this._isFinished = !this._isFinished ? true : throw new InvalidOperationException("Tried to set event options after event was finished!");
    Action<EventModel> stateChanged = this.StateChanged;
    if (stateChanged == null)
      return;
    stateChanged(this);
  }

  protected void EnterCombatWithoutExitingEvent<T>(
    IReadOnlyList<Reward> extraRewards,
    bool shouldResumeAfterCombat)
    where T : EncounterModel
  {
    this.EnterCombatWithoutExitingEvent((EncounterModel) ModelDb.Encounter<T>(), extraRewards, shouldResumeAfterCombat);
  }

  protected void EnterCombatWithoutExitingEvent(
    EncounterModel canonicalEncounter,
    IReadOnlyList<Reward> extraRewards,
    bool shouldResumeAfterCombat)
  {
    if (!this.IsShared)
      throw new InvalidOperationException($"Tried to enter combat in non-shared event {this}!");
    if (shouldResumeAfterCombat && this.LayoutType == EventLayoutType.Combat)
      throw new InvalidOperationException($"Cannot resume event {this.Id} after combat because it has a Combat layout — " + "there is no event layout to return to.");
    Action enteringEventCombat = this.EnteringEventCombat;
    if (enteringEventCombat != null)
      enteringEventCombat();
    if (LocalContext.IsMe(this.Owner))
      this.Node = (Control) null;
    this._combatSynchronizer.ReadyToEnterCombat(canonicalEncounter, this.Owner, extraRewards, shouldResumeAfterCombat);
  }

  protected EventOption RelicOption<T>(Func<Task>? onChosen, string pageName = "INITIAL") where T : RelicModel
  {
    return this.RelicOption(ModelDb.Relic<T>().ToMutable(), onChosen, pageName);
  }

  protected EventOption RelicOption(RelicModel relic, Func<Task>? onChosen, string pageName = "INITIAL")
  {
    relic.AssertMutable();
    relic.Owner = this.Owner;
    string textKey = this.OptionKey(pageName, relic.Id.Entry);
    return EventOption.FromRelic(relic, this, onChosen, textKey);
  }

  protected string InitialOptionKey(string optionName) => this.OptionKey("INITIAL", optionName);

  private string OptionKey(string pageName, string optionName)
  {
    return $"{StringHelper.Slugify(this.GetType().Name)}.pages.{pageName}.options.{optionName}";
  }

  protected async Task SelectCardsToAddToDeckFromGrid(
    List<CardCreationResult> cards,
    CardSelectorPrefs prefs)
  {
    IEnumerable<CardModel> selectedCards = await CardSelectCmd.FromSimpleGridForRewards((PlayerChoiceContext) new BlockingPlayerChoiceContext(), cards, this.Owner, prefs);
    IEnumerable<CardCreationResult> unselectedCards = cards.Where<CardCreationResult>((Func<CardCreationResult, bool>) (c => !selectedCards.Contains<CardModel>(c.Card)));
    foreach (CardModel card in selectedCards)
      CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(card, PileType.Deck));
    foreach (CardCreationResult cardCreationResult in unselectedCards)
      this.Owner.RunState.CurrentMapPointHistoryEntry.GetEntry(this.Owner.NetId).CardChoices.Add(new CardChoiceHistoryEntry(cardCreationResult.Card, false));
    unselectedCards = (IEnumerable<CardCreationResult>) null;
  }
}
