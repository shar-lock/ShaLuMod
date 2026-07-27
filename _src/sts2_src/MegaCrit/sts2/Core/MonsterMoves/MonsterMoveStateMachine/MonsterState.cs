// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterState
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Random;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;

public abstract class MonsterState
{
  public abstract string Id { get; }

  public abstract string GetNextState(Creature owner, Rng rng);

  public abstract void RegisterStates(Dictionary<string, MonsterState> monsterStates);

  public virtual void OnEnterState()
  {
  }

  public virtual void OnExitState()
  {
  }

  public virtual bool ShouldAppearInLogs => true;

  public virtual bool CanTransitionAway => true;

  public virtual bool IsMove => this is MoveState;
}
