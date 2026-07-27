// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Runs.IRunState
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Singleton;
using MegaCrit.Sts2.Core.Odds;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.Unlocks;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Runs;

public interface IRunState : ICardScope, IPlayerCollection
{
  IReadOnlyList<ActModel> Acts { get; }

  int CurrentActIndex { get; set; }

  ActModel Act { get; }

  ActMap Map { get; set; }

  MapCoord? CurrentMapCoord { get; }

  GameMode GameMode { get; }

  MapPoint? CurrentMapPoint { get; }

  RunLocation RunLocation { get; }

  MapLocation MapLocation { get; }

  int ActFloor { get; set; }

  int TotalFloor { get; }

  int CurrentRoomCount { get; }

  AbstractRoom? CurrentRoom { get; }

  AbstractRoom? BaseRoom { get; }

  bool IsGameOver { get; }

  int AscensionLevel { get; }

  RunRngSet Rng { get; }

  RunOddsSet Odds { get; }

  RelicGrabBag SharedRelicGrabBag { get; }

  UnlockState UnlockState { get; }

  IReadOnlyList<ModifierModel> Modifiers { get; }

  IReadOnlyList<BadgeModel> BadgeModels { get; }

  MultiplayerScalingModel? MultiplayerScalingModel { get; }

  IReadOnlyList<IReadOnlyList<MapPointHistoryEntry>> MapPointHistory { get; }

  MapPointHistoryEntry? CurrentMapPointHistoryEntry { get; }

  ExtraRunFields ExtraFields { get; }

  CardMultiplayerConstraint CardMultiplayerConstraint
  {
    get
    {
      return this.Players.Count <= 1 ? CardMultiplayerConstraint.SingleplayerOnly : CardMultiplayerConstraint.MultiplayerOnly;
    }
  }

  bool ContainsCard(CardModel card);

  CardModel LoadCard(SerializableCard serializableCard, Player owner);

  void AppendToMapPointHistory(
    MapPointType mapPointType,
    RoomType initialRoomType,
    ModelId? modelId);

  MapPointHistoryEntry? GetHistoryEntryFor(MapLocation location);

  IEnumerable<AbstractModel> IterateHookListeners(ICombatState? childCombatState);

  int GetAndIncrementNextRoomId();

  static IRunState GetFrom(IEnumerable<Creature> creatures)
  {
    Creature creature1 = creatures.FirstOrDefault<Creature>((Func<Creature, bool>) (c => c.IsPlayer));
    if (creature1 != null)
      return creature1.Player.RunState;
    Creature creature2 = creatures.FirstOrDefault<Creature>((Func<Creature, bool>) (c => c.PetOwner != null));
    if (creature2 != null)
      return creature2.PetOwner.RunState;
    Creature creature3 = creatures.FirstOrDefault<Creature>((Func<Creature, bool>) (c => c.CombatState != null));
    if (creature3 != null)
      return creature3.CombatState.RunState;
    Log.Warn("Unable to extract RunState from creatures list! If you're in a test, this is okay, but it's probably a bug outside of tests. Falling back to null run state.");
    return (IRunState) NullRunState.Instance;
  }
}
