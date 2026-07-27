// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.GameActions.GameAction
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Entities.Actions;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using System;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.GameActions;

public abstract class GameAction
{
  private static readonly Logger _logger = new Logger(nameof (GameAction), LogType.Actions);
  private TaskCompletionSource? _pauseForPlayerChoiceTaskSource;
  private TaskCompletionSource? _executeAfterResumptionTaskSource;
  private TaskCompletionSource _completionSource = new TaskCompletionSource();
  private Task? _executionTask;

  public GameActionState State { get; private set; }

  public abstract ulong OwnerId { get; }

  public abstract GameActionType ActionType { get; }

  public uint? Id { get; private set; }

  public event Action<GameAction>? JustBeforeFinished;

  public event Action<GameAction>? AfterFinished;

  public event Action<GameAction>? BeforeExecuted;

  public event Action<GameAction>? BeforeCancelled;

  public event Action<GameAction>? BeforePausedForPlayerChoice;

  public event Action<GameAction>? BeforeReadyToResumeAfterPlayerChoice;

  public event Action<GameAction>? BeforeResumedAfterPlayerChoice;

  public Task CompletionTask => this._completionSource.Task;

  public Exception? Exception
  {
    get
    {
      Task executionTask = this._executionTask;
      return executionTask == null ? (Exception) null : (Exception) executionTask.Exception;
    }
  }

  public virtual bool RecordableToReplay => true;

  public void OnEnqueued(Action<GameAction> afterFinished, uint id)
  {
    if (this.State != GameActionState.None)
      throw new InvalidOperationException($"GameAction {this} was enqueued to the queue twice!");
    Log.VeryDebug($"Action {this} enqueued with id {id}");
    this.Id = new uint?(id);
    this.AfterFinished += afterFinished;
    this.State = GameActionState.WaitingForExecution;
  }

  public async Task Execute()
  {
    this._pauseForPlayerChoiceTaskSource = new TaskCompletionSource();
    switch (this.State)
    {
      case GameActionState.WaitingForExecution:
        GameAction._logger.VeryDebug($"Action {this} began executing");
        this.State = GameActionState.Executing;
        Action<GameAction> beforeExecuted = this.BeforeExecuted;
        if (beforeExecuted != null)
          beforeExecuted(this);
        this._executionTask = TaskHelper.RunSafely(this.ExecuteAction());
        break;
      case GameActionState.ReadyToResumeExecuting:
        GameAction._logger.VeryDebug($"Action {this} resumed execution");
        this.State = GameActionState.Executing;
        this._executeAfterResumptionTaskSource.SetResult();
        break;
      default:
        throw new InvalidOperationException($"Attempted to execute GameAction {this} from invalid state {this.State}! Expected WaitingForExecution or ReadyToResumeExecuting");
    }
    try
    {
      await TaskHelper.WhenAny(this._executionTask, this._pauseForPlayerChoiceTaskSource.Task);
    }
    finally
    {
      if (this._executionTask.IsCompleted)
      {
        GameAction._logger.VeryDebug($"Action {this} finished execution");
        this.State = GameActionState.Finished;
        Action<GameAction> justBeforeFinished = this.JustBeforeFinished;
        if (justBeforeFinished != null)
          justBeforeFinished(this);
        this._completionSource.TrySetResult();
        Action<GameAction> afterFinished = this.AfterFinished;
        if (afterFinished != null)
          afterFinished(this);
      }
      else
        GameAction._logger.VeryDebug($"Action {this} paused execution");
    }
  }

  public void ResumeAfterGatheringPlayerChoice(uint newId)
  {
    if (this.State != GameActionState.GatheringPlayerChoice)
      throw new InvalidOperationException($"Tried setting GameAction {this} ready from invalid state {this.State}! Expected GatheringPlayerChoice");
    GameAction._logger.VeryDebug($"Action {this} finished gathering player choice, and is assigned new id {newId}");
    this.Id = new uint?(newId);
    Action<GameAction> afterPlayerChoice = this.BeforeReadyToResumeAfterPlayerChoice;
    if (afterPlayerChoice != null)
      afterPlayerChoice(this);
    this.State = GameActionState.ReadyToResumeExecuting;
  }

  public async Task WaitForActionToResumeExecutingAfterPlayerChoice()
  {
    GameAction._logger.VeryDebug($"Action {this} waiting to resume execution after player choice");
    await this._executeAfterResumptionTaskSource.Task;
    this._executeAfterResumptionTaskSource = (TaskCompletionSource) null;
    Action<GameAction> afterPlayerChoice = this.BeforeResumedAfterPlayerChoice;
    if (afterPlayerChoice == null)
      return;
    afterPlayerChoice(this);
  }

  public void PauseForPlayerChoice()
  {
    if (this.State != GameActionState.Executing)
      throw new InvalidOperationException($"Tried to pause GameAction {this} from invalid state {this.State}! Expected Executing");
    GameAction._logger.VeryDebug($"Action {this} gathering player choice");
    this._executeAfterResumptionTaskSource = new TaskCompletionSource();
    Action<GameAction> pausedForPlayerChoice = this.BeforePausedForPlayerChoice;
    if (pausedForPlayerChoice != null)
      pausedForPlayerChoice(this);
    this.State = GameActionState.GatheringPlayerChoice;
    this._pauseForPlayerChoiceTaskSource.SetResult();
  }

  protected abstract Task ExecuteAction();

  public void Cancel()
  {
    this.State = GameActionState.Canceled;
    Action<GameAction> beforeCancelled = this.BeforeCancelled;
    if (beforeCancelled != null)
      beforeCancelled(this);
    this.CancelAction();
    Callable callable = Callable.From<bool>((Func<bool>) (() => this._completionSource.TrySetCanceled()));
    ((Callable) ref callable).CallDeferred(Array.Empty<Variant>());
  }

  protected virtual void CancelAction()
  {
  }

  public abstract INetAction ToNetAction();
}
