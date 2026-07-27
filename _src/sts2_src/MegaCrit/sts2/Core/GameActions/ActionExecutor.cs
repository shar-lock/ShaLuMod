// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.GameActions.ActionExecutor
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Actions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.GameActions;

public class ActionExecutor
{
  private readonly ActionQueueSet _actionQueueSet;
  private bool _isPaused;
  private CancellationTokenSource? _actionCancelToken;
  private TaskCompletionSource<bool>? _queueTaskCompletionSource;
  private readonly Logger _logger;

  public bool IsPaused => this._isPaused;

  public bool IsRunning
  {
    get
    {
      TaskCompletionSource<bool> completionSource = this._queueTaskCompletionSource;
      if (completionSource != null)
      {
        Task<bool> task = completionSource.Task;
        if (task != null)
          return !((Task) task).IsCompleted;
      }
      return false;
    }
  }

  public GameAction? CurrentlyRunningAction { get; private set; }

  public event Action<GameAction>? BeforeActionExecuted;

  public event Action<GameAction>? JustBeforeActionFinishedExecuting;

  public event Action<GameAction>? AfterActionExecuted;

  public ActionExecutor(ActionQueueSet actionQueueSet)
  {
    actionQueueSet.ActionQueueChanged += new Action(this.ActionQueueChanged);
    this._actionQueueSet = actionQueueSet;
    this._logger = new Logger(nameof (ActionExecutor), LogType.Actions);
  }

  public void Pause()
  {
    if (NonInteractiveMode.AutoSlayerCheck())
      return;
    this._logger.Debug("Pausing queue");
    this._isPaused = true;
  }

  public void Unpause()
  {
    this._logger.Debug("Un-pausing queue");
    this._isPaused = false;
  }

  public Task FinishedExecutingActions()
  {
    return this._queueTaskCompletionSource == null ? Task.CompletedTask : (Task) this._queueTaskCompletionSource.Task;
  }

  public void Cancel()
  {
    this._logger.Debug("Cancelling queue");
    this._actionCancelToken?.Cancel();
  }

  private void ActionQueueChanged()
  {
    if (this.IsRunning)
      return;
    this._logger.Debug("Action queue changed, beginning ExecuteActions");
    TaskHelper.RunSafely(this.ExecuteActions());
  }

  private async Task ExecuteActions()
  {
    this._queueTaskCompletionSource = new TaskCompletionSource<bool>();
    this._actionCancelToken = new CancellationTokenSource();
    try
    {
      GameAction readyAction = this._actionQueueSet.GetReadyAction();
      while (readyAction != null)
      {
        await this.WaitForUnpause();
        if (readyAction.State == GameActionState.Canceled)
        {
          readyAction = this._actionQueueSet.GetReadyAction();
        }
        else
        {
          this._logger.Debug($"Executing action: {readyAction}");
          Action<GameAction> beforeActionExecuted = this.BeforeActionExecuted;
          if (beforeActionExecuted != null)
            beforeActionExecuted(readyAction);
          if (NonInteractiveMode.IsActive)
          {
            this.CurrentlyRunningAction = readyAction;
            await readyAction.Execute();
            this.AfterActionFinished(readyAction);
          }
          else
          {
            this.CurrentlyRunningAction = readyAction;
            readyAction.JustBeforeFinished += new Action<GameAction>(this.JustBeforeActionFinished);
            readyAction.AfterFinished += new Action<GameAction>(this.AfterActionFinished);
            Task actionTask = readyAction.Execute();
            while (!actionTask.IsCompleted && !this._actionCancelToken.IsCancellationRequested)
            {
              Variant[] signal = await ((GodotObject) Engine.GetMainLoop()).ToSignal((GodotObject) Engine.GetMainLoop(), SceneTree.SignalName.ProcessFrame);
            }
            if (actionTask.IsFaulted)
              Log.Error($"GameAction {readyAction} completed with exception: {actionTask.Exception}");
            actionTask = (Task) null;
          }
          bool flag = CombatManager.Instance.IsInProgress;
          if (flag)
            flag = !(readyAction is EndPlayerTurnAction) && !(readyAction is ReadyToBeginEnemyTurnAction);
          if (flag)
          {
            int num = await CombatManager.Instance.CheckWinCondition() ? 1 : 0;
          }
          if (readyAction.State == GameActionState.Finished)
            this._logger.Debug($"Completed execution of action {readyAction}, attempting to find new action");
          else
            this._logger.Debug($"Paused execution of action {readyAction} (state is {readyAction.State}), attempting to find new action");
          readyAction.JustBeforeFinished -= new Action<GameAction>(this.JustBeforeActionFinished);
          readyAction.AfterFinished -= new Action<GameAction>(this.AfterActionFinished);
          readyAction = this._actionQueueSet.GetReadyAction();
        }
      }
      this._queueTaskCompletionSource?.SetResult(true);
      readyAction = (GameAction) null;
    }
    catch (OperationCanceledException ex)
    {
      InvalidOperationException operationException = new InvalidOperationException("ActionExecutor.ExecuteActions should never be canceled!", (Exception) ex);
      this._queueTaskCompletionSource?.SetException((Exception) operationException);
      throw operationException;
    }
    catch (Exception ex)
    {
      this._queueTaskCompletionSource?.SetException(ex);
      throw;
    }
  }

  private void JustBeforeActionFinished(GameAction action)
  {
    if (this.CurrentlyRunningAction != action)
    {
      Log.Error($"Currently running action {this.CurrentlyRunningAction} did not match finishing action {action}!");
    }
    else
    {
      if (action.State != GameActionState.Finished)
        return;
      Action<GameAction> finishedExecuting = this.JustBeforeActionFinishedExecuting;
      if (finishedExecuting == null)
        return;
      finishedExecuting(action);
    }
  }

  private void AfterActionFinished(GameAction action)
  {
    if (this.CurrentlyRunningAction != action)
    {
      Log.Error($"Currently running action {this.CurrentlyRunningAction} did not match recently finished action {action}!");
    }
    else
    {
      if (action.State == GameActionState.Finished)
      {
        Action<GameAction> afterActionExecuted = this.AfterActionExecuted;
        if (afterActionExecuted != null)
          afterActionExecuted(action);
      }
      this.CurrentlyRunningAction = (GameAction) null;
    }
  }

  private async Task WaitForUnpause()
  {
    if (NonInteractiveMode.AutoSlayerCheck())
      return;
    while (this._isPaused)
    {
      if (TestMode.IsOn)
      {
        await Task.Delay(50);
      }
      else
      {
        double num = (double) await ((Node) NGame.Instance).AwaitProcessFrame();
      }
    }
  }
}
