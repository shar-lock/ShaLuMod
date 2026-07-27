// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Runs.RunState
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Singleton;
using MegaCrit.Sts2.Core.Odds;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.Unlocks;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Runs;

public class RunState : IRunState, ICardScope, IPlayerCollection
{
  private readonly List<Player> _players = new List<Player>();
  private int _currentActIndex;
  private readonly List<MapCoord> _visitedMapCoords = new List<MapCoord>();
  private readonly List<List<MapPointHistoryEntry>> _mapPointHistory = new List<List<MapPointHistoryEntry>>();
  private readonly List<AbstractRoom> _currentRooms = new List<AbstractRoom>();
  private readonly HashSet<ModelId> _visitedEventIds = new HashSet<ModelId>();
  private readonly List<CardModel> _allCards = new List<CardModel>();

  public IReadOnlyList<Player> Players => (IReadOnlyList<Player>) this._players;

  public IReadOnlyList<ActModel> Acts { get; private set; }

  public int CurrentActIndex
  {
    get => this._currentActIndex;
    set
    {
      if (this._currentActIndex == value)
        return;
      this._visitedMapCoords.Clear();
      this.ActFloor = 0;
      this.NextRoomId = 0;
      this._currentActIndex = value;
    }
  }

  public int NextRoomId { get; private set; }

  public ActModel Act => this.Acts[this.CurrentActIndex];

  public ActMap Map { get; set; } = (ActMap) NullActMap.Instance;

  public IReadOnlyList<MapCoord> VisitedMapCoords
  {
    get => (IReadOnlyList<MapCoord>) this._visitedMapCoords;
  }

  public MapCoord? CurrentMapCoord
  {
    get
    {
      return this._visitedMapCoords.Count != 0 ? new MapCoord?(this._visitedMapCoords.Last<MapCoord>()) : new MapCoord?();
    }
  }

  public MapPoint? CurrentMapPoint
  {
    get
    {
      return !this.CurrentMapCoord.HasValue ? (MapPoint) null : this.Map.GetPoint(this.CurrentMapCoord.Value);
    }
  }

  public RunLocation RunLocation => new RunLocation(this.MapLocation, (int?) this.CurrentRoom?.Id);

  public MapLocation MapLocation => new MapLocation(this.CurrentMapCoord, this.CurrentActIndex);

  public int ActFloor { get; set; }

  public int TotalFloor
  {
    get
    {
      return this.MapPointHistory.Sum<IReadOnlyList<MapPointHistoryEntry>>((Func<IReadOnlyList<MapPointHistoryEntry>, int>) (c => c.Count));
    }
  }

  public IReadOnlyList<IReadOnlyList<MapPointHistoryEntry>> MapPointHistory
  {
    get => (IReadOnlyList<IReadOnlyList<MapPointHistoryEntry>>) this._mapPointHistory;
  }

  public MapPointHistoryEntry? CurrentMapPointHistoryEntry
  {
    get
    {
      IReadOnlyList<MapPointHistoryEntry> source = this.MapPointHistory.LastOrDefault<IReadOnlyList<MapPointHistoryEntry>>();
      return source == null ? (MapPointHistoryEntry) null : source.LastOrDefault<MapPointHistoryEntry>();
    }
  }

  public int CurrentRoomCount => this._currentRooms.Count;

  public AbstractRoom? CurrentRoom => this._currentRooms.LastOrDefault<AbstractRoom>();

  public AbstractRoom? BaseRoom => this._currentRooms.FirstOrDefault<AbstractRoom>();

  public bool IsGameOver
  {
    get
    {
      return this.Players.Count > 0 && this.Players.All<Player>((Func<Player, bool>) (p => p.Creature.IsDead));
    }
  }

  public GameMode GameMode { get; init; }

  public int AscensionLevel { get; init; }

  public RunRngSet Rng { get; init; }

  public RunOddsSet Odds { get; init; }

  public RelicGrabBag SharedRelicGrabBag { get; init; }

  public UnlockState UnlockState { get; init; }

  public IReadOnlySet<ModelId> VisitedEventIds => (IReadOnlySet<ModelId>) this._visitedEventIds;

  public IReadOnlyList<ModifierModel> Modifiers { get; private set; }

  public IReadOnlyList<BadgeModel> BadgeModels { get; private set; }

  public ExtraRunFields ExtraFields { get; private set; } = new ExtraRunFields();

  public MultiplayerScalingModel MultiplayerScalingModel { get; private set; }

  public static RunState CreateForNewRun(
    IReadOnlyList<Player> players,
    IReadOnlyList<ActModel> acts,
    IReadOnlyList<ModifierModel> modifiers,
    GameMode gameMode,
    int ascensionLevel,
    string seed)
  {
    RunRngSet rng = new RunRngSet(seed);
    RunOddsSet odds = new RunOddsSet(rng.UnknownMapPoint);
    RunState shared = RunState.CreateShared(players, acts, modifiers, gameMode, 0, rng, odds, new RelicGrabBag(true), ascensionLevel);
    foreach (Player player in (IEnumerable<Player>) players)
    {
      player.InitializeSeed(seed);
      foreach (CardModel card in (IEnumerable<CardModel>) player.Deck.Cards)
        card.AfterCreated();
    }
    return shared;
  }

  public static RunState FromSerializable(SerializableRun save)
  {
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    List<Player> list = save.Players.Select<SerializablePlayer, Player>(RunState.\u003C\u003EO.\u003C0\u003E__FromSerializable ?? (RunState.\u003C\u003EO.\u003C0\u003E__FromSerializable = new Func<SerializablePlayer, Player>(Player.FromSerializable))).ToList<Player>();
    RunRngSet rng = RunRngSet.FromSave(save.SerializableRng);
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    RunState shared = RunState.CreateShared((IReadOnlyList<Player>) list, (IReadOnlyList<ActModel>) save.Acts.Select<SerializableActModel, ActModel>(RunState.\u003C\u003EO.\u003C1\u003E__FromSave ?? (RunState.\u003C\u003EO.\u003C1\u003E__FromSave = new Func<SerializableActModel, ActModel>(ActModel.FromSave))).ToList<ActModel>(), (IReadOnlyList<ModifierModel>) save.Modifiers.Select<SerializableModifier, ModifierModel>(RunState.\u003C\u003EO.\u003C2\u003E__FromSerializable ?? (RunState.\u003C\u003EO.\u003C2\u003E__FromSerializable = new Func<SerializableModifier, ModifierModel>(ModifierModel.FromSerializable))).ToList<ModifierModel>(), save.GameMode, save.CurrentActIndex, rng, RunOddsSet.FromSerializable(save.SerializableOdds, rng.UnknownMapPoint), RelicGrabBag.FromSerializable(save.SerializableSharedRelicGrabBag), save.Ascension);
    shared._visitedMapCoords.AddRange((IEnumerable<MapCoord>) save.VisitedMapCoords);
    shared._visitedEventIds.UnionWith((IEnumerable<ModelId>) save.EventsSeen);
    // ISSUE: object of a compiler-generated type is created
    shared._mapPointHistory.AddRange((IEnumerable<List<MapPointHistoryEntry>>) new \u003C\u003Ez__ReadOnlyArray<List<MapPointHistoryEntry>>(save.MapPointHistory.ToArray()));
    shared.ExtraFields = ExtraRunFields.FromSerializable(save.ExtraFields);
    return shared;
  }

  public static RunState CreateForTest(
    IReadOnlyList<Player>? players = null,
    IReadOnlyList<ActModel>? acts = null,
    IReadOnlyList<ModifierModel>? modifiers = null,
    GameMode gameMode = GameMode.Standard,
    int ascensionLevel = 0,
    string? seed = null)
  {
    if (seed == null)
      seed = SeedHelper.GetRandomSeed();
    RunRngSet rng = new RunRngSet(seed);
    // ISSUE: object of a compiler-generated type is created
    RunState shared = RunState.CreateShared(players ?? (IReadOnlyList<Player>) new \u003C\u003Ez__ReadOnlySingleElementList<Player>(Player.CreateForNewRun<Deprived>(UnlockState.all, 1UL)), (IReadOnlyList<ActModel>) (acts ?? ActModel.GetDefaultList()).Select<ActModel, ActModel>((Func<ActModel, ActModel>) (a => a.ToMutable())).ToList<ActModel>(), (IReadOnlyList<ModifierModel>) ((object) modifiers ?? (object) Array.Empty<ModifierModel>()), gameMode, 0, rng, new RunOddsSet(rng.UnknownMapPoint), new RelicGrabBag(true), ascensionLevel);
    foreach (Player player in (IEnumerable<Player>) shared.Players)
      player.InitializeSeed(seed);
    return shared;
  }

  private static RunState CreateShared(
    IReadOnlyList<Player> players,
    IReadOnlyList<ActModel> acts,
    IReadOnlyList<ModifierModel> modifiers,
    GameMode gameMode,
    int currentActIndex,
    RunRngSet rng,
    RunOddsSet odds,
    RelicGrabBag sharedRelicGrabBag,
    int ascensionLevel)
  {
    RunState state = new RunState(players, acts, modifiers, gameMode, currentActIndex, rng, odds, sharedRelicGrabBag, ascensionLevel);
    foreach (Player player in (IEnumerable<Player>) players)
    {
      player.RunState = (IRunState) state;
      foreach (CardModel card in (IEnumerable<CardModel>) player.Deck.Cards)
        state.AddCard(card, player);
    }
    state.MultiplayerScalingModel = (MultiplayerScalingModel) ModelDb.Singleton<MultiplayerScalingModel>().MutableClone();
    state.MultiplayerScalingModel.Initialize(state);
    state.BadgeModels = (IReadOnlyList<BadgeModel>) ModelDb.BadgeModels.Select<BadgeModel, BadgeModel>((Func<BadgeModel, BadgeModel>) (m => (BadgeModel) m.MutableClone())).ToList<BadgeModel>();
    return state;
  }

  private RunState(
    IReadOnlyList<Player> players,
    IReadOnlyList<ActModel> acts,
    IReadOnlyList<ModifierModel> modifiers,
    GameMode gameMode,
    int currentActIndex,
    RunRngSet rng,
    RunOddsSet odds,
    RelicGrabBag sharedRelicGrabBag,
    int ascensionLevel)
  {
    foreach (AbstractModel act in (IEnumerable<ActModel>) acts)
      act.AssertMutable();
    this._players.AddRange((IEnumerable<Player>) players);
    this.Acts = acts;
    this.Modifiers = modifiers;
    this.GameMode = gameMode;
    this.CurrentActIndex = currentActIndex;
    this.Rng = rng;
    this.Odds = odds;
    this.SharedRelicGrabBag = sharedRelicGrabBag;
    this.UnlockState = new UnlockState(players.Select<Player, UnlockState>((Func<Player, UnlockState>) (p => p.UnlockState)));
    this.AscensionLevel = ascensionLevel;
  }

  public int GetPlayerSlotIndex(Player player) => this.Players.IndexOf<Player>(player);

  public int GetPlayerSlotIndex(ulong netId)
  {
    return this.Players.FirstIndex<Player>((Predicate<Player>) (p => (long) p.NetId == (long) netId));
  }

  public Player? GetPlayer(ulong netId)
  {
    return this.Players.FirstOrDefault<Player>((Func<Player, bool>) (p => (long) p.NetId == (long) netId));
  }

  public T CreateCard<T>(Player owner) where T : CardModel
  {
    return (T) this.CreateCard((CardModel) ModelDb.Card<T>(), owner);
  }

  public CardModel CreateCard(CardModel canonicalCard, Player owner)
  {
    CardModel mutable = canonicalCard.ToMutable();
    this.AddCard(mutable, owner);
    mutable.AfterCreated();
    return mutable;
  }

  public CardModel CloneCard(CardModel mutableCard)
  {
    CardModel card = (CardModel) mutableCard.ClonePreservingMutability();
    this.AddCard(card);
    return card;
  }

  public void AddCard(CardModel card, Player owner)
  {
    if (!card.HasBeenRemovedFromState)
      card.Owner = owner;
    this.AddCard(card);
  }

  public void RemoveCard(CardModel card)
  {
    this._allCards.Remove(card);
    card.Owner = (Player) null;
  }

  public bool ContainsCard(CardModel card) => this._allCards.Contains(card);

  public CardModel LoadCard(SerializableCard serializableCard, Player owner)
  {
    CardModel card = CardModel.FromSerializable(serializableCard);
    this.AddCard(card, owner);
    return card;
  }

  private void AddCard(CardModel card)
  {
    card.AssertMutable();
    if (card.HasBeenRemovedFromState)
    {
      if (!this.ContainsCard(card))
        throw new InvalidOperationException($"Tried to add card {card} to RunState that has HasBeenRemovedFromState set as true, but it does not belong to this state!");
      card.HasBeenRemovedFromState = false;
    }
    else
      this._allCards.Add(card);
  }

  public bool AddVisitedMapCoord(MapCoord coord)
  {
    if (this._visitedMapCoords.Contains(coord))
      return false;
    this._visitedMapCoords.Add(coord);
    this.NextRoomId = 0;
    return true;
  }

  public AbstractRoom PopCurrentRoom()
  {
    AbstractRoom abstractRoom = this._currentRooms.Count != 0 ? this._currentRooms.Last<AbstractRoom>() : throw new InvalidOperationException("Not in any rooms.");
    this._currentRooms.RemoveAt(this._currentRooms.Count - 1);
    return abstractRoom;
  }

  public void PushRoom(AbstractRoom room)
  {
    if (this._currentRooms.Contains(room))
      throw new InvalidOperationException("Already in this room.");
    this._currentRooms.Add(room);
  }

  public void AddVisitedEvent(EventModel eventModel) => this._visitedEventIds.Add(eventModel.Id);

  public void AppendToMapPointHistory(
    MapPointType mapPointType,
    RoomType initialRoomType,
    ModelId? roomModelId)
  {
    if (this._mapPointHistory.Count <= this.CurrentActIndex)
    {
      int num = this.CurrentActIndex + 1 - this._mapPointHistory.Count;
      for (int index = 0; index < num; ++index)
        this._mapPointHistory.Add(new List<MapPointHistoryEntry>());
    }
    this._mapPointHistory[this.CurrentActIndex].Add(new MapPointHistoryEntry(mapPointType, (IPlayerCollection) this)
    {
      Rooms = {
        new MapPointRoomHistoryEntry()
        {
          RoomType = initialRoomType,
          ModelId = roomModelId
        }
      }
    });
  }

  public MapPointHistoryEntry? GetHistoryEntryFor(MapLocation location)
  {
    if (location.actIndex < this._mapPointHistory.Count && location.coord.HasValue)
    {
      ref MapCoord? local = ref location.coord;
      int? nullable = local.HasValue ? new int?(local.GetValueOrDefault().row) : new int?();
      int count = this._mapPointHistory[location.actIndex].Count;
      if (!(nullable.GetValueOrDefault() >= count & nullable.HasValue))
        return this._mapPointHistory[location.actIndex][location.coord.Value.row];
    }
    return (MapPointHistoryEntry) null;
  }

  public IEnumerable<AbstractModel> IterateHookListeners(ICombatState? childCombatState)
  {
    List<AbstractModel> abstractModelList = new List<AbstractModel>(this.Players.Count * 50);
    foreach (Player player in (IEnumerable<Player>) this.Players)
    {
      if (player.IsActiveForHooks)
      {
        foreach (CardModel card in (IEnumerable<CardModel>) player.Deck.Cards)
        {
          abstractModelList.Add((AbstractModel) card);
          if (card.Enchantment != null)
            abstractModelList.Add((AbstractModel) card.Enchantment);
        }
      }
    }
    if (childCombatState == null)
    {
      foreach (Player player in (IEnumerable<Player>) this.Players)
      {
        if (player.IsActiveForHooks)
        {
          abstractModelList.AddRange((IEnumerable<AbstractModel>) player.Relics.Where<RelicModel>((Func<RelicModel, bool>) (r => !r.IsMelted)));
          abstractModelList.AddRange((IEnumerable<AbstractModel>) player.Potions);
        }
      }
      abstractModelList.AddRange((IEnumerable<AbstractModel>) this.Modifiers);
      abstractModelList.AddRange((IEnumerable<AbstractModel>) this.BadgeModels);
      abstractModelList.Add((AbstractModel) this.MultiplayerScalingModel);
    }
    foreach (AbstractModel model in abstractModelList)
    {
      if (RunState.Contains(model))
        yield return model;
    }
    foreach (AbstractModel runStateSubscriber in ModHelper.IterateAllRunStateSubscribers(this))
      yield return runStateSubscriber;
    if (childCombatState != null)
    {
      foreach (AbstractModel iterateHookListener in childCombatState.IterateHookListeners())
        yield return iterateHookListener;
    }
  }

  public void AddPlayerDebug(Player player, int index)
  {
    if (index >= 0)
      this._players.Insert(index, player);
    else
      this._players.Add(player);
    player.RunState = (IRunState) this;
    player.InitializeSeed(this.Rng.StringSeed);
    foreach (CardModel card in (IEnumerable<CardModel>) player.Deck.Cards)
      card.AfterCreated();
    foreach (CardModel card in (IEnumerable<CardModel>) player.Deck.Cards)
      this.AddCard(card, player);
    MapPointHistoryEntry pointHistoryEntry = this.CurrentMapPointHistoryEntry;
    if (pointHistoryEntry != null)
      pointHistoryEntry.PlayerStats.Add(new PlayerMapPointHistoryEntry()
      {
        PlayerId = player.NetId
      });
    if (!RunManager.Instance.IsInProgress)
      return;
    player.PopulateRelicGrabBagIfNecessary(this.Rng.UpFront);
    RunManager.Instance.ApplyAscensionEffects(player);
  }

  public void SetActDebug(ActModel act)
  {
    act.AssertMutable();
    List<ActModel> list = this.Acts.ToList<ActModel>();
    list[this.CurrentActIndex] = act;
    this.Acts = (IReadOnlyList<ActModel>) list;
  }

  public void RemoveStaleVisitedMapCoords(ActMap map)
  {
    int num = this._visitedMapCoords.RemoveAll((Predicate<MapCoord>) (coord => !map.HasPoint(coord)));
    if (num <= 0)
      return;
    Log.Error($"Removed {num} stale visited map coord(s) that don't exist in the current map");
  }

  public void ClearVisitedMapCoordsDebug()
  {
    this._visitedMapCoords.Clear();
    this.ActFloor = 0;
  }

  public void AddModifierDebug(ModifierModel modifier)
  {
    IReadOnlyList<ModifierModel> modifiers = this.Modifiers;
    int index = 0;
    ModifierModel[] items = new ModifierModel[1 + modifiers.Count];
    foreach (ModifierModel modifierModel in (IEnumerable<ModifierModel>) modifiers)
    {
      items[index] = modifierModel;
      ++index;
    }
    items[index] = modifier;
    // ISSUE: object of a compiler-generated type is created
    this.Modifiers = (IReadOnlyList<ModifierModel>) new \u003C\u003Ez__ReadOnlyArray<ModifierModel>(items);
  }

  public int GetAndIncrementNextRoomId()
  {
    int nextRoomId = this.NextRoomId;
    ++this.NextRoomId;
    return nextRoomId;
  }

  private static bool Contains(AbstractModel model)
  {
    switch (model)
    {
      case RelicModel relicModel:
        return !relicModel.HasBeenRemovedFromState && relicModel.Owner.IsActiveForHooks;
      case PotionModel potionModel:
        return !potionModel.HasBeenRemovedFromState && potionModel.Owner.IsActiveForHooks;
      case CardModel cardModel:
        return !cardModel.HasBeenRemovedFromState && cardModel.Owner.IsActiveForHooks;
      case EnchantmentModel enchantmentModel:
        return enchantmentModel.HasCard && !enchantmentModel.Card.HasBeenRemovedFromState && enchantmentModel.Card.Owner.IsActiveForHooks;
      case AchievementModel _:
        return true;
      case BadgeModel _:
        return true;
      case ModifierModel _:
        return true;
      case MultiplayerScalingModel _:
        return true;
      default:
        throw new ArgumentOutOfRangeException(nameof (model), (object) model, $"Invalid model type {model.GetType()} ({model})");
    }
  }
}
