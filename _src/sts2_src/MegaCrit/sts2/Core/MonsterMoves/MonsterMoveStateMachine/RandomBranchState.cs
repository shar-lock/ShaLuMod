// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.RandomBranchState
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;

public class RandomBranchState : MonsterState
{
  public string StateId { get; }

  public List<RandomBranchState.StateWeight> States { get; set; } = new List<RandomBranchState.StateWeight>();

  public override bool ShouldAppearInLogs => false;

  public RandomBranchState(string id) => this.StateId = id;

  public void AddBranch(
    MonsterState state,
    int cooldown,
    MoveRepeatType repeatType,
    Func<float> weight)
  {
    if (repeatType.Equals((object) MoveRepeatType.CanRepeatXTimes))
      throw new ArgumentException("Use other constructor to specify number of repeats");
    this.States.Add(new RandomBranchState.StateWeight()
    {
      repeatType = repeatType,
      stateId = state.Id,
      weightLambda = weight,
      cooldown = cooldown
    });
  }

  public void AddBranch(MonsterState state, int cooldown, int maxRepeats, Func<float> weight)
  {
    this.States.Add(new RandomBranchState.StateWeight()
    {
      maxTimes = maxRepeats,
      repeatType = MoveRepeatType.CanRepeatXTimes,
      stateId = state.Id,
      weightLambda = weight,
      cooldown = cooldown
    });
  }

  public void AddBranch(MonsterState state, int maxRepeats, Func<float> weight)
  {
    this.AddBranch(state, 0, maxRepeats, weight);
  }

  public void AddBranch(MonsterState state, int cooldown, MoveRepeatType repeatType, float weight)
  {
    this.AddBranch(state, cooldown, repeatType, (Func<float>) (() => weight));
  }

  public void AddBranch(MonsterState state, MoveRepeatType repeatType, float weight)
  {
    this.AddBranch(state, repeatType, (Func<float>) (() => weight));
  }

  public void AddBranch(MonsterState state, MoveRepeatType repeatType, Func<float> weight)
  {
    this.AddBranch(state, 0, repeatType, weight);
  }

  public void AddBranch(MonsterState state, int maxRepeats, float weight)
  {
    this.AddBranch(state, maxRepeats, (Func<float>) (() => weight));
  }

  public void AddBranch(MonsterState state, int cooldown, MoveRepeatType repeatType)
  {
    this.AddBranch(state, cooldown, repeatType, 1f);
  }

  public void AddBranch(MonsterState state, int maxRepeats)
  {
    this.AddBranch(state, maxRepeats, 1f);
  }

  public void AddBranch(MonsterState state, MoveRepeatType repeatType)
  {
    this.AddBranch(state, repeatType, 1f);
  }

  public override string GetNextState(Creature owner, Rng rng)
  {
    float max = this.States.Sum<RandomBranchState.StateWeight>((Func<RandomBranchState.StateWeight, float>) (x => RandomBranchState.GetStateWeight(x, owner)));
    float num = rng.NextFloat(max);
    foreach (RandomBranchState.StateWeight state in this.States)
    {
      num -= RandomBranchState.GetStateWeight(state, owner);
      if ((double) num <= 0.0)
        return state.stateId;
    }
    throw new InvalidOperationException($"No valid state found in RandomBranchState {this.Id}!");
  }

  private static float GetStateWeight(RandomBranchState.StateWeight stateWeight, Creature owner)
  {
    MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine moveStateMachine = owner.Monster.MoveStateMachine;
    float num1 = 1f;
    if (stateWeight.repeatType.Equals((object) MoveRepeatType.UseOnlyOnce))
    {
      MonsterState state = moveStateMachine.States[stateWeight.stateId];
      if (moveStateMachine.StateLog.Contains(state))
        num1 = 0.0f;
    }
    else if (!stateWeight.repeatType.Equals((object) MoveRepeatType.CanRepeatForever))
    {
      float num2 = stateWeight.repeatType.Equals((object) MoveRepeatType.CannotRepeat) ? 1f : (float) stateWeight.maxTimes;
      num1 = (float) ((double) moveStateMachine.StateLog.Count < (double) num2 ? 1 : 0);
      for (int index = 0; (double) moveStateMachine.StateLog.Count >= (double) num2 && (double) index < (double) num2 && moveStateMachine.StateLog.Count - index > 0; ++index)
      {
        MonsterState state = moveStateMachine.States[stateWeight.stateId];
        if (moveStateMachine.StateLog[moveStateMachine.StateLog.Count - 1 - index] != state)
        {
          num1 = 1f;
          break;
        }
      }
    }
    return stateWeight.cooldown > 0 && moveStateMachine.StateLog.Where<MonsterState>((Func<MonsterState, bool>) (state => state.IsMove)).Reverse<MonsterState>().Take<MonsterState>(stateWeight.cooldown).Any<MonsterState>((Func<MonsterState, bool>) (move => move.Id == stateWeight.stateId)) ? 0.0f : num1 * stateWeight.GetWeight();
  }

  public override void RegisterStates(Dictionary<string, MonsterState> monsterStates)
  {
    monsterStates.Add(this.Id, (MonsterState) this);
  }

  public override string Id => this.StateId;

  public struct StateWeight
  {
    public string stateId;
    public MoveRepeatType repeatType;
    public int maxTimes;
    public Func<float> weightLambda;
    public int cooldown;

    public float GetWeight()
    {
      if (this.weightLambda != null)
        return this.weightLambda();
      throw new InvalidOperationException(this.stateId + " doesn't have a weight");
    }
  }
}
