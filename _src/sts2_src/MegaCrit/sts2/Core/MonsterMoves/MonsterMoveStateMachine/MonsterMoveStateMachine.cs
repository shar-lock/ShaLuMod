// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;

public class MonsterMoveStateMachine
{
  private MonsterState _currentState;
  private readonly MonsterState _initialState;
  private bool _performedFirstMove;

  public Dictionary<string, MonsterState> States { get; } = new Dictionary<string, MonsterState>();

  public List<MonsterState> StateLog { get; } = new List<MonsterState>();

  public MonsterMoveStateMachine(IEnumerable<MonsterState> states, MonsterState initialState)
  {
    foreach (MonsterState state in states)
      state.RegisterStates(this.States);
    this._initialState = initialState;
    this._currentState = this._initialState;
    if (!this._currentState.ShouldAppearInLogs)
      return;
    this.StateLog.Add(this._currentState);
  }

  public MoveState RollMove(IEnumerable<Creature> targets, Creature owner, Rng rng)
  {
    this.FindNextMoveState(targets, owner, rng, true);
    return this._currentState.IsMove ? (MoveState) this._currentState : throw new InvalidOperationException(this._currentState.Id + " is not a valid move state");
  }

  public void ForceCurrentState(MonsterState state) => this.SetCurrentState(state);

  public void OnMovePerformed(MoveState _) => this._performedFirstMove = true;

  private void FindNextMoveState(
    IEnumerable<Creature> targets,
    Creature owner,
    Rng rng,
    bool logMove)
  {
    if (this._currentState == null)
      throw new InvalidOperationException("Cannot find next move state when current state is null.");
    if (!this._currentState.CanTransitionAway || !this._performedFirstMove && this._currentState.IsMove)
      return;
    MonsterState monsterState = (MonsterState) null;
    do
    {
      string nextState = this._currentState.GetNextState(owner, rng);
      if (!string.IsNullOrEmpty(nextState) && !this.States.ContainsKey(nextState))
        throw new InvalidOperationException("no valid state found: " + nextState);
      this.SetCurrentState(string.IsNullOrEmpty(nextState) ? this._initialState : this.States[nextState]);
      monsterState = monsterState != null || !this._currentState.ShouldAppearInLogs ? monsterState : this._currentState;
    }
    while (!this._currentState.IsMove);
    if (!logMove || monsterState == null)
      return;
    this.StateLog.Add(monsterState);
  }

  private void SetCurrentState(MonsterState state)
  {
    this._currentState.OnExitState();
    this._currentState = state;
    this._currentState.OnEnterState();
  }
}
