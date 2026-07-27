// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.ConditionalBranchState
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;

public class ConditionalBranchState : MonsterState
{
  public string BranchId { get; }

  public override string Id => this.BranchId;

  private List<ConditionalBranchState.ConditionalBranch> States { get; } = new List<ConditionalBranchState.ConditionalBranch>();

  public ConditionalBranchState(string stateId) => this.BranchId = stateId;

  public void AddState(MonsterState move, Func<bool> condition)
  {
    this.States.Add(new ConditionalBranchState.ConditionalBranch(move, condition));
  }

  public override string GetNextState(Creature _, Rng __)
  {
    foreach (ConditionalBranchState.ConditionalBranch state in this.States)
    {
      if ((double) state.Evaluate() > 0.0)
        return state.id;
    }
    throw new InvalidOperationException("No valid next state found.");
  }

  public override void RegisterStates(Dictionary<string, MonsterState> monsterStates)
  {
    monsterStates.Add(this.Id, (MonsterState) this);
  }

  public override bool ShouldAppearInLogs => false;

  private readonly struct ConditionalBranch
  {
    public readonly string id;
    private readonly Func<bool> _conditionalLambda;

    public ConditionalBranch(MonsterState state, Func<bool> condition)
    {
      this.id = state.Id;
      this._conditionalLambda = condition;
    }

    public float Evaluate()
    {
      return this._conditionalLambda != null ? (float) (this._conditionalLambda() ? 1 : 0) : 1f;
    }
  }
}
