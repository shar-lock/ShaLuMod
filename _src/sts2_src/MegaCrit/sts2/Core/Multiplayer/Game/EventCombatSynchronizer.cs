// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Game.EventCombatSynchronizer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Game;

public class EventCombatSynchronizer
{
  private readonly IRunState _runState;
  private readonly List<EventCombatSynchronizer.EventCombatState?> _states = new List<EventCombatSynchronizer.EventCombatState>();
  private readonly Logger _logger = new Logger(nameof (EventCombatSynchronizer), LogType.GameSync);
  private EventModel? _canonicalEvent;

  public EncounterModel? MutableEncounterForLayout { get; private set; }

  public CombatState? CombatStateForLayout { get; private set; }

  public EventCombatSynchronizer(IPlayerCollection playerCollection, IRunState runState)
  {
    this._runState = runState;
    foreach (Player player in (IEnumerable<Player>) playerCollection.Players)
      this._states.Add((EventCombatSynchronizer.EventCombatState) null);
  }

  public void InitializeForEvent(EventModel localEvent)
  {
    this._canonicalEvent = localEvent.CanonicalInstance;
    if (localEvent.LayoutType != EventLayoutType.Combat)
      return;
    this.MutableEncounterForLayout = localEvent.CanonicalEncounter != null ? localEvent.CanonicalEncounter.ToMutable() : throw new InvalidOperationException($"Canonical encounter is not set for event {localEvent.Id} with combat layout!");
    this.MutableEncounterForLayout.GenerateMonstersWithSlots(this._runState);
    this.CombatStateForLayout = this.CreateCombatState(this.MutableEncounterForLayout);
    foreach (Player player in (IEnumerable<Player>) this._runState.Players)
      this.CombatStateForLayout.AddPlayer(player);
    foreach ((MonsterModel, string) monstersWithSlot in (IEnumerable<(MonsterModel, string)>) this.CombatStateForLayout.Encounter.MonstersWithSlots)
      this.CombatStateForLayout.AddCreature(this.CombatStateForLayout.CreateCreature(monstersWithSlot.Item1, CombatSide.Enemy, monstersWithSlot.Item2));
  }

  public void ReadyToEnterCombat(
    EncounterModel canonicalEncounter,
    Player player,
    IReadOnlyList<Reward> extraRewards,
    bool shouldResumeAfterCombat)
  {
    this._logger.Debug($"Player {player.NetId} is ready to enter combat {canonicalEncounter.Id} (extra rewards: {extraRewards.Count}, should resume after: {shouldResumeAfterCombat})");
    int playerSlotIndex = this._runState.GetPlayerSlotIndex(player);
    if (this._states[playerSlotIndex] != null)
      throw new InvalidOperationException($"Player {player.NetId} became ready to enter combat {canonicalEncounter.Id},but they are already set to ready for {this._states[playerSlotIndex].canonicalEncounter.Id}!");
    this._states[playerSlotIndex] = new EventCombatSynchronizer.EventCombatState()
    {
      canonicalEncounter = canonicalEncounter,
      extraRewards = extraRewards,
      shouldResumeAfterCombat = shouldResumeAfterCombat
    };
    if (!this._states.All<EventCombatSynchronizer.EventCombatState>((Func<EventCombatSynchronizer.EventCombatState, bool>) (s => s != null)))
      return;
    this.EnterCombat();
  }

  private void EnterCombat()
  {
    if (this._canonicalEvent == null)
      throw new InvalidOperationException("GenerateInternalCombatState must be called before EnterCombat!");
    EncounterModel canonicalEncounter = this._states[0].canonicalEncounter;
    bool resumeAfterCombat = this._states[0].shouldResumeAfterCombat;
    for (int index = 0; index < this._states.Count; ++index)
    {
      if (this._states[index].shouldResumeAfterCombat != resumeAfterCombat)
        throw new InvalidOperationException($"Event for player {this._runState.Players[index].NetId} tried to start event combatwith shouldResumeAfterCombat set to {this._states[index].shouldResumeAfterCombat}, but the host says it should be {resumeAfterCombat}!");
      if (this._states[index].canonicalEncounter != canonicalEncounter)
        throw new InvalidOperationException($"Event for player {this._runState.Players[index].NetId} tried to start event combatwith encounter {this._states[index].canonicalEncounter.Id}, but the host says it should be {canonicalEncounter.Id}!");
    }
    this._logger.Debug($"Entering combat {canonicalEncounter.Id} from event");
    CombatRoom room = new CombatRoom(this.CombatStateForLayout ?? this.CreateCombatState(canonicalEncounter.ToMutable()))
    {
      ShouldCreateCombat = this._canonicalEvent.LayoutType != EventLayoutType.Combat,
      ShouldResumeParentEventAfterCombat = resumeAfterCombat,
      ParentEventId = this._canonicalEvent.Id
    };
    foreach (EventCombatSynchronizer.EventCombatState state in this._states)
    {
      foreach (Reward extraReward in (IEnumerable<Reward>) state.extraRewards)
        room.AddExtraReward(extraReward.Player, extraReward);
    }
    TaskHelper.RunSafely(RunManager.Instance.EnterRoomWithoutExitingCurrentRoom((AbstractRoom) room, this._canonicalEvent.LayoutType != EventLayoutType.Combat));
  }

  public void ResetState()
  {
    CombatState combatStateForLayout = this.CombatStateForLayout;
    // ISSUE: explicit non-virtual call
    foreach (Creature creature in (IEnumerable<Creature>) ((combatStateForLayout != null ? (object) __nonvirtual (combatStateForLayout.Creatures) : (object) null) ?? (object) Array.Empty<Creature>()))
      this.CombatStateForLayout?.RemoveCreature(creature, true);
    this._canonicalEvent = (EventModel) null;
    this.MutableEncounterForLayout = (EncounterModel) null;
    this.CombatStateForLayout = (CombatState) null;
    for (int index = 0; index < this._states.Count; ++index)
      this._states[index] = (EventCombatSynchronizer.EventCombatState) null;
  }

  private CombatState CreateCombatState(EncounterModel mutableEncounter)
  {
    return new CombatState(mutableEncounter, this._runState, this._runState.Modifiers, this._runState.BadgeModels, this._runState.MultiplayerScalingModel);
  }

  private class EventCombatState
  {
    public required EncounterModel canonicalEncounter;
    public required IReadOnlyList<Reward> extraRewards;
    public bool shouldResumeAfterCombat;
  }
}
