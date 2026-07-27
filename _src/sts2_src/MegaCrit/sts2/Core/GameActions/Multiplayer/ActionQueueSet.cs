// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.GameActions.Multiplayer.ActionQueueSet
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Entities.Actions;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.GameActions.Multiplayer;

public class ActionQueueSet
{
  private readonly Logger _logger;
  private readonly List<ActionQueueSet.ActionQueue> _actionQueues = new List<ActionQueueSet.ActionQueue>();
  private readonly List<ActionQueueSet.ActionWaitingForResumption> _actionsWaitingForResumption = new List<ActionQueueSet.ActionWaitingForResumption>();
  private TaskCompletionSource? _queuesEmptyCompletionSource;
  private uint _nextId;
  private bool _isInCombat;
  private bool _wasReset;

  public bool IsEmpty
  {
    get
    {
      return this._queuesEmptyCompletionSource == null || this._queuesEmptyCompletionSource.Task.IsCompleted;
    }
  }

  public uint NextActionId => this._nextId;

  public event Action? ActionQueueChanged;

  public event Action<GameAction>? ActionEnqueued;

  public event Action<uint>? ActionResumed;

  public ActionQueueSet(IReadOnlyList<Player> players)
  {
    this._logger = new Logger(nameof (ActionQueueSet), LogType.Actions);
    foreach (Player player in (IEnumerable<Player>) players)
      this._actionQueues.Add(new ActionQueueSet.ActionQueue()
      {
        actions = new List<GameAction>(),
        ownerId = player.NetId
      });
  }

  public void EnqueueWithoutSynchronizing(GameAction gameAction)
  {
    if (this._queuesEmptyCompletionSource == null || this._queuesEmptyCompletionSource.Task.IsCompleted)
      this._queuesEmptyCompletionSource = new TaskCompletionSource();
    if (gameAction.Id.HasValue)
      throw new InvalidOperationException($"Attempting to enqueue GameAction {gameAction} which already has an ID {gameAction.Id}, indicating it was previously enqueued to the queue!");
    gameAction.OnEnqueued(new Action<GameAction>(this.PopAction), this.GetAndIncrementActionId());
    ActionQueueSet.ActionQueue queue = this.GetQueue(gameAction.OwnerId);
    try
    {
      Action<GameAction> actionEnqueued = this.ActionEnqueued;
      if (actionEnqueued != null)
        actionEnqueued(gameAction);
    }
    catch (Exception ex)
    {
      Log.Error($"Exception encountered in ActionEnqueued for action {gameAction}: {ex}");
      SentryService.CaptureException(ex);
    }
    if (queue.isCancellingPlayCardActions && gameAction is PlayCardAction)
    {
      this._logger.Debug($"Attempted to enqueue PlayCardAction {gameAction} to player queue owned by {gameAction.OwnerId}, but it's currently cancelling all play card actions due to player choice");
      gameAction.Cancel();
    }
    else if (queue.isCancellingPlayerDrivenCombatActions && ActionQueueSet.IsGameActionPlayerDriven(gameAction) && gameAction.ActionType != GameActionType.NonCombat && gameAction.ActionType != GameActionType.Any)
    {
      this._logger.Debug($"Attempted to enqueue GameAction {gameAction} to player queue owned by {gameAction.OwnerId}, but it's currently cancelling all non-hook actions due to end of turn");
      gameAction.Cancel();
    }
    else
    {
      bool flag1 = queue.isCancellingCombatActions;
      if (flag1)
      {
        bool flag2;
        switch (gameAction.ActionType)
        {
          case GameActionType.Combat:
          case GameActionType.CombatPlayPhaseOnly:
            flag2 = true;
            break;
          default:
            flag2 = false;
            break;
        }
        flag1 = flag2;
      }
      if (flag1)
      {
        this._logger.Debug($"Attempted to enqueue GameAction {gameAction} to player queue owned by {gameAction.OwnerId}, but it's currently cancelling all combat actions");
        gameAction.Cancel();
      }
      else
      {
        this._logger.Debug($"Enqueueing action {gameAction} to player queue owned by {gameAction.OwnerId}");
        queue.actions.Add(gameAction);
        Action actionQueueChanged = this.ActionQueueChanged;
        if (actionQueueChanged == null)
          return;
        actionQueueChanged();
      }
    }
  }

  public static bool IsGameActionPlayerDriven(GameAction gameAction)
  {
    return !(gameAction is GenericHookGameAction) && !(gameAction is ReadyToBeginEnemyTurnAction);
  }

  public GameAction? GetReadyAction()
  {
    GameAction readyAction = (GameAction) null;
    this._logger.VeryDebug("Attempting to find ready action");
    foreach (ActionQueueSet.ActionQueue actionQueue in this._actionQueues)
    {
      if (actionQueue.actions.Count <= 0)
      {
        this._logger.VeryDebug($"Queue for player {actionQueue.ownerId} is empty");
      }
      else
      {
        while (actionQueue.actions.Count > 0 && actionQueue.actions[0].State == GameActionState.Canceled)
        {
          this._logger.Warn($"Removing canceled action {actionQueue.actions[0]} from front of player queue {actionQueue.ownerId}");
          actionQueue.actions.RemoveAt(0);
        }
        if (actionQueue.actions.Count > 0)
        {
          GameAction action = actionQueue.actions[0];
          if (this._isInCombat && action.ActionType == GameActionType.NonCombat)
          {
            this._logger.VeryDebug($"We are currently in combat and candidate action {action} has type {action.ActionType}");
          }
          else
          {
            bool flag1 = !this._isInCombat;
            if (flag1)
            {
              bool flag2;
              switch (action.ActionType)
              {
                case GameActionType.Combat:
                case GameActionType.CombatPlayPhaseOnly:
                  flag2 = true;
                  break;
                default:
                  flag2 = false;
                  break;
              }
              flag1 = flag2;
            }
            if (flag1)
              this._logger.VeryDebug($"We are currently not in combat and candidate action {action} has type {action.ActionType}");
            else if (actionQueue.isPaused && action.ActionType == GameActionType.CombatPlayPhaseOnly)
              this._logger.VeryDebug($"Queue for player {actionQueue.ownerId} is paused and candidate action {action} has type {action.ActionType}");
            else if (action.State == GameActionState.GatheringPlayerChoice)
            {
              this._logger.VeryDebug($"Action {action} at front of player queue {actionQueue.ownerId} is waiting for player choice");
            }
            else
            {
              if (action.State != GameActionState.WaitingForExecution && action.State != GameActionState.ReadyToResumeExecuting)
                throw new InvalidOperationException($"GameAction {action} at the front of player action queue {actionQueue.ownerId} is in invalid state {action.State}!");
              DefaultInterpolatedStringHandler interpolatedStringHandler;
              uint? id1;
              if (readyAction != null)
              {
                uint? id2 = action.Id;
                id1 = readyAction.Id;
                if (!(id2.GetValueOrDefault() < id1.GetValueOrDefault() & id2.HasValue & id1.HasValue))
                {
                  Logger logger = this._logger;
                  interpolatedStringHandler = new DefaultInterpolatedStringHandler(59, 4);
                  interpolatedStringHandler.AppendLiteral("Action ");
                  interpolatedStringHandler.AppendFormatted<GameAction>(action);
                  interpolatedStringHandler.AppendLiteral(" has id ");
                  ref DefaultInterpolatedStringHandler local1 = ref interpolatedStringHandler;
                  id1 = action.Id;
                  int num1 = (int) id1.Value;
                  local1.AppendFormatted<uint>((uint) num1);
                  interpolatedStringHandler.AppendLiteral(" greater than current ready action ");
                  interpolatedStringHandler.AppendFormatted<GameAction>(readyAction);
                  interpolatedStringHandler.AppendLiteral(" with ID ");
                  ref DefaultInterpolatedStringHandler local2 = ref interpolatedStringHandler;
                  id1 = readyAction.Id;
                  int num2 = (int) id1.Value;
                  local2.AppendFormatted<uint>((uint) num2);
                  string stringAndClear = interpolatedStringHandler.ToStringAndClear();
                  logger.VeryDebug(stringAndClear);
                  continue;
                }
              }
              Logger logger1 = this._logger;
              interpolatedStringHandler = new DefaultInterpolatedStringHandler(55, 3);
              interpolatedStringHandler.AppendLiteral("Action ");
              interpolatedStringHandler.AppendFormatted<GameAction>(action);
              interpolatedStringHandler.AppendLiteral(" with id ");
              ref DefaultInterpolatedStringHandler local = ref interpolatedStringHandler;
              id1 = action.Id;
              int num = (int) id1.Value;
              local.AppendFormatted<uint>((uint) num);
              interpolatedStringHandler.AppendLiteral(" belonging to ");
              interpolatedStringHandler.AppendFormatted<ulong>(actionQueue.ownerId);
              interpolatedStringHandler.AppendLiteral(" becomes new ready action");
              string stringAndClear1 = interpolatedStringHandler.ToStringAndClear();
              logger1.VeryDebug(stringAndClear1);
              readyAction = action;
            }
          }
        }
      }
    }
    if (readyAction != null)
      this._logger.VeryDebug($"Got ready action {readyAction} ({readyAction.Id})");
    else
      this._logger.VeryDebug("No action is ready");
    return readyAction;
  }

  public void PauseActionForPlayerChoice(GameAction action, PlayerChoiceOptions options)
  {
    ActionQueueSet.ActionQueue queue = this.GetQueue(action.OwnerId);
    if (action != queue.actions[0])
      throw new InvalidOperationException($"Attempting to pause action {action} that is not at the front of the owner {action.Id}'s queue!");
    this._logger.Debug($"Pausing action {action} for player choice");
    action.PauseForPlayerChoice();
    ActionQueueSet.ActionWaitingForResumption? nullable = new ActionQueueSet.ActionWaitingForResumption?();
    for (int index = 0; index < this._actionsWaitingForResumption.Count; ++index)
    {
      int oldId = (int) this._actionsWaitingForResumption[index].oldId;
      uint? id = action.Id;
      int valueOrDefault = (int) id.GetValueOrDefault();
      if (oldId == valueOrDefault & id.HasValue)
      {
        nullable = new ActionQueueSet.ActionWaitingForResumption?(this._actionsWaitingForResumption[index]);
        this._actionsWaitingForResumption.RemoveAt(index);
        break;
      }
    }
    if (options.HasFlag((Enum) PlayerChoiceOptions.CancelPlayCardActions))
    {
      this.CancelNonExecutingActionsOfType<PlayCardAction>(action.OwnerId, nullable?.newId);
      queue.isCancellingPlayCardActions = true;
    }
    Action actionQueueChanged1 = this.ActionQueueChanged;
    if (actionQueueChanged1 != null)
      actionQueueChanged1();
    if (!nullable.HasValue)
      return;
    this._logger.Debug($"Immediately resuming action {action} - already had resumption waiting");
    action.ResumeAfterGatheringPlayerChoice(nullable.Value.newId);
    queue.isCancellingPlayCardActions = false;
    Action actionQueueChanged2 = this.ActionQueueChanged;
    if (actionQueueChanged2 == null)
      return;
    actionQueueChanged2();
  }

  public Task BecameEmpty()
  {
    return this._queuesEmptyCompletionSource == null ? Task.CompletedTask : this._queuesEmptyCompletionSource.Task;
  }

  public void PauseAllPlayerQueues()
  {
    this._logger.Debug("Pausing all player queues");
    foreach (ActionQueueSet.ActionQueue actionQueue in this._actionQueues)
    {
      actionQueue.isPaused = true;
      actionQueue.isCancellingPlayerDrivenCombatActions = false;
    }
    Action actionQueueChanged = this.ActionQueueChanged;
    if (actionQueueChanged == null)
      return;
    actionQueueChanged();
  }

  public void StartCancellingAllPlayerDrivenCombatActions()
  {
    this._logger.Debug("Setting all player queues to cancel all non-hook actions");
    foreach (ActionQueueSet.ActionQueue actionQueue in this._actionQueues)
    {
      actionQueue.isCancellingPlayerDrivenCombatActions = true;
      for (int index = 0; index < actionQueue.actions.Count; ++index)
      {
        GameAction action = actionQueue.actions[index];
        if (ActionQueueSet.IsGameActionPlayerDriven(action) && action.ActionType != GameActionType.NonCombat && action.ActionType != GameActionType.Any && action.State == GameActionState.WaitingForExecution)
        {
          this._logger.VeryDebug($"Cancelling non-hook action {actionQueue.actions[index]}");
          action.Cancel();
          actionQueue.actions.RemoveAt(index);
          --index;
        }
      }
    }
  }

  public bool ActionQueueIsPaused(ulong playerId) => this.GetQueue(playerId).isPaused;

  public void UnpauseAllPlayerQueues()
  {
    this._logger.Debug("Unpausing all player queues");
    foreach (ActionQueueSet.ActionQueue actionQueue in this._actionQueues)
    {
      actionQueue.isPaused = false;
      actionQueue.isCancellingPlayerDrivenCombatActions = false;
    }
    Action actionQueueChanged = this.ActionQueueChanged;
    if (actionQueueChanged == null)
      return;
    actionQueueChanged();
  }

  public void CombatEnded()
  {
    this._logger.Debug("Combat ended. Cancelling all non-executing combat actions in all queues");
    this._isInCombat = false;
    foreach (ActionQueueSet.ActionQueue actionQueue in this._actionQueues)
    {
      for (int index = 0; index < actionQueue.actions.Count; ++index)
      {
        GameAction action = actionQueue.actions[index];
        bool flag;
        switch (action.ActionType)
        {
          case GameActionType.Combat:
          case GameActionType.CombatPlayPhaseOnly:
            flag = true;
            break;
          default:
            flag = false;
            break;
        }
        if (flag && action.State != GameActionState.Executing)
        {
          this._logger.VeryDebug($"Cancelling action {action}");
          action.Cancel();
          actionQueue.actions.RemoveAt(index);
          --index;
        }
        else
          this._logger.VeryDebug($"Not cancelling action {action}, type: {action.ActionType}, state: {action.State}");
      }
      actionQueue.isCancellingPlayCardActions = false;
      actionQueue.isCancellingPlayerDrivenCombatActions = false;
      actionQueue.isCancellingCombatActions = true;
    }
    this.CheckIfQueuesEmpty();
    Action actionQueueChanged = this.ActionQueueChanged;
    if (actionQueueChanged == null)
      return;
    actionQueueChanged();
  }

  public void SetUpForCombat()
  {
    this._logger.Debug("Setting up for combat.");
    this._wasReset = false;
    this._isInCombat = false;
    foreach (ActionQueueSet.ActionQueue actionQueue in this._actionQueues)
      actionQueue.isCancellingCombatActions = false;
  }

  public void CombatStarted()
  {
    this._logger.Debug("Combat started.");
    this._isInCombat = true;
    this._wasReset = false;
    foreach (ActionQueueSet.ActionQueue actionQueue in this._actionQueues)
      actionQueue.isCancellingCombatActions = false;
  }

  public void Reset()
  {
    this._wasReset = true;
    this._actionQueues.Clear();
    this.CheckIfQueuesEmpty();
    Action actionQueueChanged = this.ActionQueueChanged;
    if (actionQueueChanged == null)
      return;
    actionQueueChanged();
  }

  public void CancelNonExecutingActionsForPlayer(ulong playerId)
  {
    this._logger.Debug($"Cancelling all non-executing actions owned by {playerId}");
    ActionQueueSet.ActionQueue queue = this.GetQueue(playerId);
    for (int index = 0; index < queue.actions.Count; ++index)
    {
      if (queue.actions[index].State == GameActionState.WaitingForExecution)
      {
        this._logger.VeryDebug($"Cancelling action {queue.actions[index]}");
        queue.actions[index].Cancel();
        queue.actions.RemoveAt(index);
        --index;
      }
    }
    this.CheckIfQueuesEmpty();
  }

  private void CancelNonExecutingActionsOfType<T>(ulong ownerId, uint? maxActionId) where T : GameAction
  {
    this._logger.Debug($"Cancelling non-executing actions of type {typeof (T)} owned by {ownerId}");
    foreach (ActionQueueSet.ActionQueue actionQueue in this._actionQueues)
    {
      if ((long) actionQueue.ownerId == (long) ownerId)
      {
        for (int index = 0; index < actionQueue.actions.Count; ++index)
        {
          GameAction action = actionQueue.actions[index];
          if (action is T && action.State == GameActionState.WaitingForExecution)
          {
            if (maxActionId.HasValue)
            {
              int num = (int) action.Id.Value;
              uint? nullable = maxActionId;
              int valueOrDefault = (int) nullable.GetValueOrDefault();
              if ((uint) num >= (uint) valueOrDefault & nullable.HasValue)
                continue;
            }
            this._logger.VeryDebug($"Cancelling action {actionQueue.actions[index]}");
            action.Cancel();
            actionQueue.actions.RemoveAt(index);
            --index;
          }
        }
      }
    }
    this.CheckIfQueuesEmpty();
  }

  public void ResumeActionWithoutSynchronizing(uint id)
  {
    Action<uint> actionResumed = this.ActionResumed;
    if (actionResumed != null)
      actionResumed(id);
    uint incrementActionId = this.GetAndIncrementActionId();
    GameAction gameAction;
    ActionQueueSet.ActionQueue queue;
    if (this.TryGetAction(id, out gameAction, out queue) && gameAction.State == GameActionState.GatheringPlayerChoice)
    {
      this._logger.Debug($"Resuming action {gameAction} after player choice");
      queue.isCancellingPlayCardActions = false;
      gameAction.ResumeAfterGatheringPlayerChoice(incrementActionId);
      Action actionQueueChanged = this.ActionQueueChanged;
      if (actionQueueChanged == null)
        return;
      actionQueueChanged();
    }
    else
    {
      this._logger.Debug($"Action with id {id} is not ready to resume, enqueueing resumption");
      this._actionsWaitingForResumption.Add(new ActionQueueSet.ActionWaitingForResumption()
      {
        oldId = id,
        newId = incrementActionId
      });
    }
  }

  private bool TryGetAction(
    uint id,
    out GameAction? gameAction,
    out ActionQueueSet.ActionQueue? queue)
  {
    foreach (ActionQueueSet.ActionQueue actionQueue in this._actionQueues)
    {
      foreach (GameAction action in actionQueue.actions)
      {
        uint? id1 = action.Id;
        uint num = id;
        if ((int) id1.GetValueOrDefault() == (int) num & id1.HasValue)
        {
          queue = actionQueue;
          gameAction = action;
          return true;
        }
      }
    }
    queue = (ActionQueueSet.ActionQueue) null;
    gameAction = (GameAction) null;
    return false;
  }

  private ActionQueueSet.ActionQueue GetQueue(ulong playerId)
  {
    return this._actionQueues.FirstOrDefault<ActionQueueSet.ActionQueue>((Func<ActionQueueSet.ActionQueue, bool>) (q => (long) q.ownerId == (long) playerId)) ?? throw new InvalidOperationException($"Tried to get local action queue for nonexistent player with ID {playerId}!");
  }

  private void PopAction(GameAction action)
  {
    if (this._wasReset)
      return;
    bool flag = false;
    foreach (ActionQueueSet.ActionQueue actionQueue in this._actionQueues)
    {
      if (actionQueue.actions.Count != 0)
      {
        if (actionQueue.actions[0] == action)
        {
          flag = true;
          actionQueue.actions.RemoveAt(0);
        }
        else
        {
          foreach (GameAction action1 in actionQueue.actions)
          {
            if (action1 == action)
              throw new InvalidOperationException($"Tried to pop action {action}, but it is not the top-most action for player {actionQueue.ownerId}!");
          }
        }
      }
    }
    if (flag)
    {
      Action actionQueueChanged = this.ActionQueueChanged;
      if (actionQueueChanged != null)
        actionQueueChanged();
      if (action.Exception != null)
        this._queuesEmptyCompletionSource?.SetException(action.Exception);
      else
        this.CheckIfQueuesEmpty();
    }
    else
      throw new InvalidOperationException($"Tried to pop action {action}, but we didn't find it in any queue!");
  }

  public void FastForwardNextActionId(uint nextId) => this._nextId = nextId;

  private void CheckIfQueuesEmpty()
  {
    if (!this._actionQueues.All<ActionQueueSet.ActionQueue>((Func<ActionQueueSet.ActionQueue, bool>) (q => q.actions.Count == 0)))
      return;
    TaskCompletionSource completionSource = this._queuesEmptyCompletionSource;
    if (completionSource == null)
      return;
    Task task = completionSource.Task;
    if (task == null || task.IsCompleted)
      return;
    this._queuesEmptyCompletionSource?.SetResult();
  }

  private uint GetAndIncrementActionId()
  {
    uint nextId = this._nextId;
    ++this._nextId;
    return nextId;
  }

  private class ActionQueue
  {
    public List<GameAction> actions = new List<GameAction>();
    public ulong ownerId;
    public bool isCancellingPlayCardActions;
    public bool isCancellingPlayerDrivenCombatActions;
    public bool isCancellingCombatActions = true;
    public bool isPaused;
  }

  private struct ActionWaitingForResumption
  {
    public uint oldId;
    public uint newId;
  }
}
