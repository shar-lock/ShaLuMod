// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Animation.AnimState
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Animation;

public class AnimState
{
  public const string attackAnim = "attack";
  public const string castAnim = "cast";
  public const string dieAnim = "die";
  public const string hurtAnim = "hurt";
  public const string idleAnim = "idle_loop";
  public const string reviveAnim = "revive";
  public const string stunAnim = "stun";
  private readonly Dictionary<string, List<AnimState.Branch>> _branchedStates;

  public string Id { get; }

  public bool IsLooping { get; }

  public bool HasLooped { get; private set; }

  public AnimState? NextState { get; set; }

  public string? BoundsContainer { get; init; }

  public AnimState(string id, bool isLooping = false)
  {
    this.Id = id;
    this.IsLooping = isLooping;
    this._branchedStates = new Dictionary<string, List<AnimState.Branch>>();
  }

  public void AddBranch(string trigger, AnimState state, Func<bool>? condition = null)
  {
    AnimState.Branch branch = new AnimState.Branch()
    {
      state = state,
      condition = condition
    };
    List<AnimState.Branch> branchList;
    if (!this._branchedStates.TryGetValue(trigger, out branchList))
    {
      branchList = new List<AnimState.Branch>();
      this._branchedStates[trigger] = branchList;
    }
    branchList.Add(branch);
  }

  public AnimState? CallTrigger(string trigger)
  {
    List<AnimState.Branch> branchList;
    if (this._branchedStates.TryGetValue(trigger, out branchList))
    {
      foreach (AnimState.Branch branch in branchList)
      {
        Func<bool> condition = branch.condition;
        if ((condition != null ? (condition() ? 1 : 0) : 1) != 0)
          return branch.state;
      }
    }
    return (AnimState) null;
  }

  public bool HasTrigger(string trigger) => this._branchedStates.ContainsKey(trigger);

  public void MarkHasLooped() => this.HasLooped = true;

  private struct Branch
  {
    public AnimState state;
    public Func<bool>? condition;
  }
}
