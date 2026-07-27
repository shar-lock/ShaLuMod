// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Runs.NullRunState
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.Models.Singleton;
using MegaCrit.Sts2.Core.Odds;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.Unlocks;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Runs;

public class NullRunState : IRunState, ICardScope, IPlayerCollection
{
  public static NullRunState Instance { get; } = new NullRunState();

  public IReadOnlyList<Player> Players => (IReadOnlyList<Player>) Array.Empty<Player>();

  public IReadOnlyList<ActModel> Acts => ActModel.GetDefaultList();

  public int CurrentActIndex
  {
    get => 0;
    set => throw new InvalidOperationException("Cannot set act index in a null run.");
  }

  public ActModel Act => ModelDb.Act<Overgrowth>().ToMutable();

  public ActMap Map
  {
    get => (ActMap) NullActMap.Instance;
    set => throw new InvalidOperationException("Cannot set map in a null run.");
  }

  public MapCoord? CurrentMapCoord => new MapCoord?();

  public MapPoint? CurrentMapPoint => (MapPoint) null;

  public MapLocation MapLocation => new MapLocation(new MapCoord?(), 0);

  public RunLocation RunLocation => new RunLocation(this.MapLocation, new int?());

  public GameMode GameMode => GameMode.Standard;

  public int ActFloor
  {
    get => 0;
    set => throw new InvalidOperationException("Cannot set act floor in a null run.");
  }

  public int TotalFloor => 0;

  public IReadOnlyList<ModifierModel> Modifiers
  {
    get => (IReadOnlyList<ModifierModel>) Array.Empty<ModifierModel>();
  }

  public IReadOnlyList<BadgeModel> BadgeModels
  {
    get => (IReadOnlyList<BadgeModel>) Array.Empty<BadgeModel>();
  }

  public MultiplayerScalingModel? MultiplayerScalingModel => (MultiplayerScalingModel) null;

  public IReadOnlyList<IReadOnlyList<MapPointHistoryEntry>> MapPointHistory
  {
    get
    {
      return (IReadOnlyList<IReadOnlyList<MapPointHistoryEntry>>) Array.Empty<IReadOnlyList<MapPointHistoryEntry>>();
    }
  }

  public MapPointHistoryEntry? CurrentMapPointHistoryEntry => (MapPointHistoryEntry) null;

  public int CurrentRoomCount => 0;

  public AbstractRoom? CurrentRoom => (AbstractRoom) null;

  public AbstractRoom? BaseRoom => (AbstractRoom) null;

  public bool IsGameOver => false;

  public int AscensionLevel => 0;

  public RunRngSet Rng => new RunRngSet(string.Empty);

  public RunOddsSet Odds => new RunOddsSet(new MegaCrit.Sts2.Core.Random.Rng());

  public RelicGrabBag SharedRelicGrabBag => new RelicGrabBag(false);

  public UnlockState UnlockState => UnlockState.all;

  public ExtraRunFields ExtraFields => new ExtraRunFields();

  private NullRunState()
  {
  }

  public Player? GetPlayer(ulong netId) => (Player) null;

  public int GetPlayerSlotIndex(Player player) => -1;

  public T CreateCard<T>(Player owner) where T : CardModel
  {
    throw new InvalidOperationException("Cannot create cards in a null run.");
  }

  public CardModel CreateCard(CardModel canonicalCard, Player owner)
  {
    throw new InvalidOperationException("Cannot create cards in a null run.");
  }

  public CardModel CloneCard(CardModel mutableCard)
  {
    throw new InvalidOperationException("Cannot clone cards in a null run.");
  }

  public bool ContainsCard(CardModel card) => false;

  public void AddCard(CardModel mutableCard, Player owner)
  {
    throw new InvalidOperationException("Cannot add cards in a null run.");
  }

  public void RemoveCard(CardModel card)
  {
    throw new InvalidOperationException("Cannot remove cards in a null run.");
  }

  public CardModel LoadCard(SerializableCard serializableCard, Player owner)
  {
    throw new InvalidOperationException("Cannot load cards in a null run.");
  }

  public void AppendToMapPointHistory(
    MapPointType mapPointType,
    RoomType initialRoomType,
    ModelId? modelId)
  {
  }

  public MapPointHistoryEntry? GetHistoryEntryFor(MapLocation location)
  {
    return (MapPointHistoryEntry) null;
  }

  public int GetAndIncrementNextRoomId() => 0;

  public IEnumerable<AbstractModel> IterateHookListeners(ICombatState? childCombatState)
  {
    return childCombatState?.IterateHookListeners() ?? (IEnumerable<AbstractModel>) Array.Empty<AbstractModel>();
  }
}
