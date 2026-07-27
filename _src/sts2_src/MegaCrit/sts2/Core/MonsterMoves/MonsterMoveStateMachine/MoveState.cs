// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MoveState
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;

public class MoveState : MonsterState
{
  private bool _performedAtLeastOnce;
  private readonly Func<IReadOnlyList<Creature>, Task> _onPerform;

  public IReadOnlyList<AbstractIntent> Intents { get; private set; }

  public string StateId { get; }

  public bool MustPerformOnceBeforeTransitioning { get; set; }

  public string? FollowUpStateId { get; init; }

  public MonsterState? FollowUpState { get; set; }

  public override bool CanTransitionAway
  {
    get => !this.MustPerformOnceBeforeTransitioning || this._performedAtLeastOnce;
  }

  public MoveState()
    : this("UNSET_MOVE", MoveState.\u003C\u003EO.\u003C0\u003E__UnsetMove ?? (MoveState.\u003C\u003EO.\u003C0\u003E__UnsetMove = new Func<IReadOnlyList<Creature>, Task>(MoveState.UnsetMove)))
  {
    // ISSUE: reference to a compiler-generated field (out of statement scope)
    // ISSUE: reference to a compiler-generated field (out of statement scope)
  }

  public MoveState(
    string stateId,
    Func<IReadOnlyList<Creature>, Task> onPerform,
    params AbstractIntent[] intents)
  {
    this._onPerform = onPerform;
    this.Intents = (IReadOnlyList<AbstractIntent>) intents;
    this.StateId = stateId;
  }

  public async Task PerformMove(IEnumerable<Creature> targets)
  {
    this._performedAtLeastOnce = true;
    if (!(targets is Creature[] creatureArray))
      creatureArray = targets.ToArray<Creature>();
    await this._onPerform((IReadOnlyList<Creature>) creatureArray);
  }

  public override void OnExitState() => this._performedAtLeastOnce = false;

  public override string GetNextState(Creature owner, Rng rng)
  {
    return (this.FollowUpState?.Id ?? this.FollowUpStateId) ?? throw new InvalidOperationException("No valid followup state.");
  }

  public override void RegisterStates(Dictionary<string, MonsterState> monsterStates)
  {
    monsterStates.Add(this.Id, (MonsterState) this);
  }

  public override bool IsMove => true;

  public override string Id => this.StateId;

  private static Task UnsetMove(IEnumerable<Creature> c)
  {
    throw new InvalidOperationException("No move has been set for the monster");
  }
}
