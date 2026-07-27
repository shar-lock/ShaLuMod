// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.GameActions.Multiplayer.HookPlayerChoiceContext
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.GameActions.Multiplayer;

public class HookPlayerChoiceContext : PlayerChoiceContext
{
  private readonly ulong _localPlayerId;
  private GenericHookGameAction? _gameAction;
  private readonly TaskCompletionSource _taskAssignedCompletionSource = new TaskCompletionSource();
  private readonly TaskCompletionSource _pausedBeforeTaskAssignedCompletionSource = new TaskCompletionSource();
  private readonly TaskCompletionSource _pausedCompletionSource = new TaskCompletionSource();
  private ActionQueueSynchronizer? _actionQueueSynchronizer;
  private ActionQueueSet? _actionQueueSet;
  private ActionExecutor? _actionExecutor;
  private GameActionType _gameActionType;

  private ActionQueueSynchronizer ActionQueueSynchronizer
  {
    get => this._actionQueueSynchronizer ?? RunManager.Instance.ActionQueueSynchronizer;
  }

  private ActionQueueSet ActionQueueSet
  {
    get => this._actionQueueSet ?? RunManager.Instance.ActionQueueSet;
  }

  private ActionExecutor ActionExecutor
  {
    get => this._actionExecutor ?? RunManager.Instance.ActionExecutor;
  }

  public AbstractModel? Source { get; }

  public Task? Task { get; private set; }

  public Player? Owner { get; }

  public override ulong? OwnerId => this.Owner?.NetId;

  public GenericHookGameAction? GameAction => this._gameAction;

  public HookPlayerChoiceContext(Player owner, ulong localPlayerId, GameActionType gameActionType)
  {
    this._gameActionType = gameActionType;
    this._localPlayerId = localPlayerId;
    this.Owner = owner;
  }

  public HookPlayerChoiceContext(
    AbstractModel source,
    ulong localPlayerId,
    ICombatState? combatState,
    GameActionType gameActionType)
  {
    this._localPlayerId = localPlayerId;
    this.Source = source;
    this.Owner = HookPlayerChoiceContext.GetOwner(source, combatState);
    this.PushModel(this.Source);
    this._gameActionType = gameActionType;
  }

  public static Player? GetOwner(AbstractModel source, ICombatState? combatState)
  {
    Player player;
    switch (source)
    {
      case CardModel cardModel:
        player = cardModel.Owner;
        break;
      case RelicModel relicModel:
        player = relicModel.Owner;
        break;
      case PotionModel potionModel:
        player = potionModel.Owner;
        break;
      case AfflictionModel afflictionModel:
        player = afflictionModel.Card.Owner;
        break;
      case EnchantmentModel enchantmentModel:
        player = enchantmentModel.Card.Owner;
        break;
      case PowerModel powerModel:
        player = powerModel.Owner.Player;
        break;
      default:
        player = (Player) null;
        break;
    }
    return player ?? combatState?.Players[0];
  }

  public void MockDependenciesForTest(
    ActionQueueSynchronizer? actionQueueSynchronizer,
    ActionQueueSet? actionQueueSet,
    ActionExecutor? actionExecutor)
  {
    this._actionQueueSet = actionQueueSet;
    this._actionQueueSynchronizer = actionQueueSynchronizer;
    this._actionExecutor = actionExecutor;
  }

  public async Task<bool> AssignTaskAndWaitForPauseOrCompletion(Task task)
  {
    this.Task = this.Source == null ? task : this.ExecuteTaskThenInvokeExecutionFinished(task);
    this._taskAssignedCompletionSource.SetResult();
    await TaskHelper.WhenAny(task, this._pausedCompletionSource.Task);
    return task.IsCompleted;
  }

  public async Task<bool> WaitForPauseOrCompletionWithoutAssigningTask(Task task)
  {
    await TaskHelper.WhenAny(task, this._pausedBeforeTaskAssignedCompletionSource.Task);
    return task.IsCompleted;
  }

  private async Task ExecuteTaskThenInvokeExecutionFinished(Task task)
  {
    await task;
    this.Source?.InvokeExecutionFinished();
  }

  public override async Task SignalPlayerChoiceBegun(Player chooser, PlayerChoiceOptions options)
  {
    long netId = (long) chooser.NetId;
    ulong? ownerId = this.OwnerId;
    long valueOrDefault = (long) ownerId.GetValueOrDefault();
    if (!(netId == valueOrDefault & ownerId.HasValue))
      Log.Warn($"{nameof (HookPlayerChoiceContext)} is executing player choice owned by {chooser.NetId}, but the player choice began with owner {this.OwnerId}! This will work, but will likely result in some weird-looking user experience. See {"BlockingPlayerChoiceContext"} for a resolution.");
    if (this._gameAction != null)
    {
      if (this.ActionExecutor != null && this.ActionExecutor.CurrentlyRunningAction != this._gameAction)
      {
        Log.Error($"Tried to interrupt action {this._gameAction} but the currently running action is {this.ActionExecutor.CurrentlyRunningAction}!");
        return;
      }
    }
    else
    {
      if (this.Owner == null)
        throw new InvalidOperationException($"HookPlayerChoiceContext is assigned a model {this.Source} with no owner, but the model has requested a player choice! This is not supported");
      this._gameAction = this.ActionQueueSynchronizer.GenerateHookAction(chooser.NetId, this._gameActionType);
      this._pausedBeforeTaskAssignedCompletionSource.SetResult();
      if (this.Task == null)
      {
        await this._taskAssignedCompletionSource.Task;
        if (this.Task == null)
          throw new InvalidOperationException("HookPlayerChoiceContext was never passed a task to await!");
      }
      this._gameAction.SetChoiceContext(this);
      if ((long) this._gameAction.OwnerId == (long) this._localPlayerId)
        this.ActionQueueSynchronizer.RequestEnqueueHookAction(this._gameAction);
      this._pausedCompletionSource.SetResult();
      await this._gameAction.ExecutionStartedTask;
    }
    this.ActionQueueSet.PauseActionForPlayerChoice((MegaCrit.Sts2.Core.GameActions.GameAction) this._gameAction, options);
  }

  public override async Task SignalPlayerChoiceEnded()
  {
    if ((long) this._gameAction.OwnerId == (long) this._localPlayerId)
      this.ActionQueueSynchronizer.RequestResumeActionAfterPlayerChoice((MegaCrit.Sts2.Core.GameActions.GameAction) this._gameAction);
    await this._gameAction.WaitForActionToResumeExecutingAfterPlayerChoice();
  }

  public async Task WaitForCompletion()
  {
    if (this.Task == null)
      throw new InvalidOperationException("Tried to call WaitForCompletion before AssignTaskAndWaitForPauseOrCompletion was called!");
    await this.Task;
    if (this.GameAction == null)
      return;
    await this.GameAction.CompletionTask;
  }
}
