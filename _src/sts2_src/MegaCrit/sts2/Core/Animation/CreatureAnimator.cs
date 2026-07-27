// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Animation.CreatureAnimator
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Random;
using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Animation;

public class CreatureAnimator
{
  public const string idleTrigger = "Idle";
  public const string attackTrigger = "Attack";
  public const string powerUpTrigger = "PowerUp";
  public const string castTrigger = "Cast";
  public const string deathTrigger = "Dead";
  public const string hitTrigger = "Hit";
  public const string reviveTrigger = "Revive";
  private const float _animVariance = 0.1f;
  private readonly MegaSprite _spineController;
  private AnimState _currentState;
  private readonly AnimState _anyState;

  public event Action<string>? BoundsUpdated;

  public CreatureAnimator(AnimState initialState, MegaSprite spineController)
  {
    this._anyState = new AnimState("anyState");
    this._spineController = spineController;
    this._currentState = initialState;
    this._spineController.ConnectAnimationStarted(Callable.From<GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject>(this.OnAnimationStarted)));
    this._spineController.ConnectAnimationCompleted(Callable.From<GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject>(this.OnAnimationCompleted)));
    this._spineController.ConnectAnimationInterrupted(Callable.From<GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject>(this.OnAnimationInterrupted)));
    this.SetNextState(initialState);
    if (!(initialState.Id == "idle_loop"))
      return;
    MegaAnimationState animationState = this._spineController.GetAnimationState();
    using (MegaTrackEntry current = animationState.GetCurrent(0))
    {
      if (current == null)
        return;
      current.SetTrackTime(Rng.Chaotic.NextFloat(current.GetAnimationEnd()));
      animationState.Update(0.0f);
      MegaSkeleton skeleton = this._spineController.GetSkeleton();
      if (skeleton == null)
        return;
      animationState.Apply(skeleton);
    }
  }

  public void AddAnyState(string trigger, AnimState state, Func<bool>? condition = null)
  {
    this._anyState.AddBranch(trigger, state, condition);
  }

  public void SetTrigger(string trigger)
  {
    AnimState state = this._anyState.CallTrigger(trigger) ?? this._currentState.CallTrigger(trigger);
    if (state == null)
      return;
    this.SetNextState(state);
  }

  public bool HasTrigger(string trigger) => this._anyState.HasTrigger(trigger);

  private void SetNextState(AnimState state)
  {
    if (this._currentState.BoundsContainer != null)
    {
      Action<string> boundsUpdated = this.BoundsUpdated;
      if (boundsUpdated != null)
        boundsUpdated(this._currentState.BoundsContainer);
    }
    this._currentState = state;
    if (!this._spineController.HasAnimation(this._currentState.Id))
    {
      Log.Warn($"could not find '{this._currentState.Id}' animation on '{(this._spineController.BoundObject is Node boundObject ? boundObject.Name.ToString() : (string) null) ?? "unknown"}'");
    }
    else
    {
      MegaAnimationState animationState = this._spineController.GetAnimationState();
      animationState.SetAnimation(this._currentState.Id, this._currentState.IsLooping);
      if (this._currentState.IsLooping)
      {
        using (MegaTrackEntry current = animationState.GetCurrent(0))
        {
          if (current != null)
            this.OffsetLoopingAnimation(current);
        }
      }
      if (state.BoundsContainer != null)
      {
        Action<string> boundsUpdated = this.BoundsUpdated;
        if (boundsUpdated != null)
          boundsUpdated(state.BoundsContainer);
      }
      if (state.NextState == null)
        return;
      this.AddNextState(state.NextState);
    }
  }

  private void AddNextState(AnimState state)
  {
    if (!this._spineController.HasAnimation(state.Id))
    {
      string str = (this._spineController.BoundObject is Node boundObject ? boundObject.Name.ToString() : (string) null) ?? "unknown";
      Log.Warn($"could not find '{state.Id}' animation (queued) on '{str}'");
    }
    else
    {
      MegaAnimationState animationState = this._spineController.GetAnimationState();
      if (state.IsLooping)
      {
        using (MegaTrackEntry track = animationState.AddAnimationTracked(state.Id, loop: state.IsLooping))
          this.OffsetLoopingAnimation(track);
      }
      else
        animationState.AddAnimation(state.Id, loop: state.IsLooping);
      if (state.NextState == null)
        return;
      this.AddNextState(state.NextState);
    }
  }

  private void OnAnimationStarted(GodotObject _, GodotObject __, GodotObject ___)
  {
    AnimState currentState = this._currentState;
    if (currentState == null || currentState.HasLooped || currentState.BoundsContainer == null)
      return;
    Action<string> boundsUpdated = this.BoundsUpdated;
    if (boundsUpdated == null)
      return;
    boundsUpdated(this._currentState.BoundsContainer);
  }

  private void OnAnimationCompleted(GodotObject _, GodotObject __, GodotObject ___)
  {
    AnimState currentState1 = this._currentState;
    if (currentState1 != null && !currentState1.HasLooped && currentState1.BoundsContainer != null)
    {
      Action<string> boundsUpdated = this.BoundsUpdated;
      if (boundsUpdated != null)
        boundsUpdated(this._currentState.BoundsContainer);
    }
    AnimState currentState2 = this._currentState;
    if (currentState2 != null && currentState2.IsLooping && !currentState2.HasLooped)
      this._currentState.MarkHasLooped();
    if (this._currentState.NextState == null)
      return;
    this._currentState = this._currentState.NextState;
  }

  private void OnAnimationInterrupted(GodotObject _, GodotObject __, GodotObject ___)
  {
    if (this._currentState.BoundsContainer == null)
      return;
    Action<string> boundsUpdated = this.BoundsUpdated;
    if (boundsUpdated == null)
      return;
    boundsUpdated(this._currentState.BoundsContainer);
  }

  private void OffsetLoopingAnimation(MegaTrackEntry track)
  {
    track.SetTimeScale(Rng.Chaotic.NextFloat(0.9f, 1.1f));
    float animationEnd = track.GetAnimationEnd();
    track.SetTrackTime((animationEnd + Rng.Chaotic.NextFloat(-0.1f, 0.1f)) % animationEnd);
  }
}
