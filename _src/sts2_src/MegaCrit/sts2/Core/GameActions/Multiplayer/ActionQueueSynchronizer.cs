// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.GameActions.Multiplayer.ActionQueueSynchronizer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Game;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.GameActions.Multiplayer;

public class ActionQueueSynchronizer
{
  private readonly ActionQueueSet _actionQueueSet;
  private readonly INetGameService _netService;
  private readonly Logger _logger;
  private readonly IPlayerCollection _playerCollection;
  private readonly RunLocationTargetedMessageBuffer _messageBuffer;
  private readonly List<GenericHookGameAction> _hookActions = new List<GenericHookGameAction>();
  private uint _nextHookId;
  private readonly List<GameAction> _requestedActionsWaitingForPlayerTurn = new List<GameAction>();

  public uint NextHookId => this._nextHookId;

  public ActionSynchronizerCombatState CombatState { get; private set; }

  public ActionQueueSynchronizer(
    IPlayerCollection players,
    ActionQueueSet actionQueueSet,
    RunLocationTargetedMessageBuffer messageBuffer,
    INetGameService netService)
  {
    this._playerCollection = players;
    this._actionQueueSet = actionQueueSet;
    this._netService = netService;
    this._messageBuffer = messageBuffer;
    this._messageBuffer.RegisterMessageHandler<RequestEnqueueActionMessage>(new MessageHandlerDelegate<RequestEnqueueActionMessage>(this.HandleRequestEnqueueActionMessage));
    this._messageBuffer.RegisterMessageHandler<ActionEnqueuedMessage>(new MessageHandlerDelegate<ActionEnqueuedMessage>(this.HandleActionEnqueuedMessage));
    this._messageBuffer.RegisterMessageHandler<RequestEnqueueHookActionMessage>(new MessageHandlerDelegate<RequestEnqueueHookActionMessage>(this.HandleRequestEnqueueHookActionMessage));
    this._messageBuffer.RegisterMessageHandler<HookActionEnqueuedMessage>(new MessageHandlerDelegate<HookActionEnqueuedMessage>(this.HandleHookActionEnqueuedMessage));
    this._messageBuffer.RegisterMessageHandler<RequestResumeActionAfterPlayerChoiceMessage>(new MessageHandlerDelegate<RequestResumeActionAfterPlayerChoiceMessage>(this.HandleRequestResumeActionAfterPlayerChoiceMessage));
    this._messageBuffer.RegisterMessageHandler<ResumeActionAfterPlayerChoiceMessage>(new MessageHandlerDelegate<ResumeActionAfterPlayerChoiceMessage>(this.HandleResumeActionAfterPlayerChoiceMessage));
    this._logger = new Logger(nameof (ActionQueueSynchronizer), LogType.Actions);
  }

  public void Dispose()
  {
    this._messageBuffer.UnregisterMessageHandler<RequestEnqueueActionMessage>(new MessageHandlerDelegate<RequestEnqueueActionMessage>(this.HandleRequestEnqueueActionMessage));
    this._messageBuffer.UnregisterMessageHandler<ActionEnqueuedMessage>(new MessageHandlerDelegate<ActionEnqueuedMessage>(this.HandleActionEnqueuedMessage));
    this._messageBuffer.UnregisterMessageHandler<RequestEnqueueHookActionMessage>(new MessageHandlerDelegate<RequestEnqueueHookActionMessage>(this.HandleRequestEnqueueHookActionMessage));
    this._messageBuffer.UnregisterMessageHandler<HookActionEnqueuedMessage>(new MessageHandlerDelegate<HookActionEnqueuedMessage>(this.HandleHookActionEnqueuedMessage));
    this._messageBuffer.UnregisterMessageHandler<RequestResumeActionAfterPlayerChoiceMessage>(new MessageHandlerDelegate<RequestResumeActionAfterPlayerChoiceMessage>(this.HandleRequestResumeActionAfterPlayerChoiceMessage));
    this._messageBuffer.UnregisterMessageHandler<ResumeActionAfterPlayerChoiceMessage>(new MessageHandlerDelegate<ResumeActionAfterPlayerChoiceMessage>(this.HandleResumeActionAfterPlayerChoiceMessage));
  }

  public void SetCombatState(ActionSynchronizerCombatState combatState)
  {
    if (this.CombatState == combatState)
      return;
    ActionSynchronizerCombatState combatState1 = this.CombatState;
    this.CombatState = combatState;
    bool flag1;
    switch (combatState1)
    {
      case ActionSynchronizerCombatState.NotInCombat:
      case ActionSynchronizerCombatState.PreCombatSetup:
        flag1 = true;
        break;
      default:
        flag1 = false;
        break;
    }
    bool flag2 = flag1;
    if (flag2)
    {
      bool flag3;
      switch (combatState)
      {
        case ActionSynchronizerCombatState.PlayPhase:
        case ActionSynchronizerCombatState.EndTurnPhaseOne:
        case ActionSynchronizerCombatState.NotPlayPhase:
          flag3 = true;
          break;
        default:
          flag3 = false;
          break;
      }
      flag2 = flag3;
    }
    if (flag2)
    {
      this._logger.Debug($"Combat becomes {combatState} from {combatState1}. Signaling queues that combat has begun");
      this._actionQueueSet.CombatStarted();
    }
    switch (combatState)
    {
      case ActionSynchronizerCombatState.NotInCombat:
        this._logger.Debug($"Combat state becomes {combatState}. Cancelling deferred actions and cancelling all remaining combat actions");
        this._actionQueueSet.CombatEnded();
        this._actionQueueSet.UnpauseAllPlayerQueues();
        foreach (GameAction gameAction in this._requestedActionsWaitingForPlayerTurn)
          gameAction.Cancel();
        this._requestedActionsWaitingForPlayerTurn.Clear();
        break;
      case ActionSynchronizerCombatState.PreCombatSetup:
        if (combatState1 != ActionSynchronizerCombatState.NotInCombat)
          break;
        this._logger.Debug($"Combat state becomes {combatState}. Allowing non-combat actions to execute, but not combat actions.");
        this._actionQueueSet.SetUpForCombat();
        break;
      case ActionSynchronizerCombatState.PlayPhase:
        this._logger.Debug($"Combat state becomes {combatState}. Requesting {this._requestedActionsWaitingForPlayerTurn.Count} actions and unpausing action queues");
        foreach (GameAction action in this._requestedActionsWaitingForPlayerTurn)
          this.RequestEnqueue(action);
        this._requestedActionsWaitingForPlayerTurn.Clear();
        this._actionQueueSet.UnpauseAllPlayerQueues();
        break;
      case ActionSynchronizerCombatState.EndTurnPhaseOne:
        this._logger.Debug($"Combat state becomes {combatState} (from {combatState1}). Starting to cancel all player-driven actions");
        this._actionQueueSet.StartCancellingAllPlayerDrivenCombatActions();
        break;
      case ActionSynchronizerCombatState.NotPlayPhase:
        this._logger.Debug($"Combat state becomes {combatState}. Pausing all player queues");
        this._actionQueueSet.PauseAllPlayerQueues();
        break;
    }
  }

  public void RequestEnqueue(GameAction action)
  {
    if (action.ActionType == GameActionType.CombatPlayPhaseOnly && this.CombatState == ActionSynchronizerCombatState.NotPlayPhase)
    {
      this._logger.Debug($"Attempted to request enqueue of action {action} during the enemy turn. Deferring the action until the player turn");
      this._requestedActionsWaitingForPlayerTurn.Add(action);
    }
    else
    {
      switch (this._netService.Type)
      {
        case NetGameType.Singleplayer:
        case NetGameType.Host:
          this.EnqueueAction(action, this._netService.NetId);
          break;
        case NetGameType.Client:
          this._logger.Debug($"Sending message to host requesting to enqueue action {action} at {Log.Timestamp}");
          this._netService.SendMessage<RequestEnqueueActionMessage>(new RequestEnqueueActionMessage()
          {
            action = action.ToNetAction(),
            location = this._messageBuffer.CurrentLocation
          });
          break;
      }
    }
  }

  public GenericHookGameAction GenerateHookAction(ulong ownerId, GameActionType gameActionType)
  {
    GenericHookGameAction hookActionForId = this.GetHookActionForId(this._nextHookId, ownerId, gameActionType);
    ++this._nextHookId;
    return hookActionForId;
  }

  public void RequestEnqueueHookAction(GenericHookGameAction action)
  {
    switch (this._netService.Type)
    {
      case NetGameType.Singleplayer:
      case NetGameType.Host:
        this.EnqueueHookAction(action);
        break;
      case NetGameType.Client:
        this._logger.Debug($"Sending message to host requesting to enqueue HOOK action with id {action.HookId} at {Log.Timestamp}");
        this._netService.SendMessage<RequestEnqueueHookActionMessage>(new RequestEnqueueHookActionMessage()
        {
          hookActionId = action.HookId,
          location = this._messageBuffer.CurrentLocation,
          gameActionType = action.ActionType
        });
        break;
    }
  }

  public void RequestResumeActionAfterPlayerChoice(GameAction action)
  {
    switch (this._netService.Type)
    {
      case NetGameType.Singleplayer:
      case NetGameType.Host:
        this.ResumeActionAfterPlayerChoice(action.Id.Value);
        break;
      case NetGameType.Client:
        this._logger.Debug($"Sending message to host requesting resumption of action {action}, id {action.Id}, at {Log.Timestamp}");
        this._netService.SendMessage<RequestResumeActionAfterPlayerChoiceMessage>(new RequestResumeActionAfterPlayerChoiceMessage()
        {
          actionId = action.Id.Value,
          location = this._messageBuffer.CurrentLocation
        });
        break;
    }
  }

  private void EnqueueAction(GameAction action, ulong actionOwnerId)
  {
    if (this._netService.Type == NetGameType.Host)
    {
      this._logger.Debug($"Sending message to clients to enqueue action {action} at {Log.Timestamp}");
      this._netService.SendMessage<ActionEnqueuedMessage>(new ActionEnqueuedMessage()
      {
        playerId = actionOwnerId,
        location = this._messageBuffer.CurrentLocation,
        action = action.ToNetAction()
      });
    }
    this._logger.Debug($"Enqueueing action {action} from owner {actionOwnerId}");
    this._actionQueueSet.EnqueueWithoutSynchronizing(action);
  }

  private void EnqueueHookAction(GenericHookGameAction gameAction)
  {
    if (this._netService.Type == NetGameType.Host)
    {
      this._logger.Debug($"Sending message to clients to enqueue hook action {gameAction} at {Log.Timestamp}");
      this._netService.SendMessage<HookActionEnqueuedMessage>(new HookActionEnqueuedMessage()
      {
        hookActionId = gameAction.HookId,
        ownerId = gameAction.OwnerId,
        location = this._messageBuffer.CurrentLocation,
        gameActionType = gameAction.ActionType
      });
    }
    this._logger.Debug($"Enqueueing HOOK action with id {gameAction.HookId}");
    this._actionQueueSet.EnqueueWithoutSynchronizing((GameAction) gameAction);
  }

  private void ResumeActionAfterPlayerChoice(uint id)
  {
    if (this._netService.Type == NetGameType.Host)
    {
      this._logger.Debug($"Sending message to clients to resume action id {id} at {Log.Timestamp}");
      this._netService.SendMessage<ResumeActionAfterPlayerChoiceMessage>(new ResumeActionAfterPlayerChoiceMessage()
      {
        actionId = id,
        location = this._messageBuffer.CurrentLocation
      });
    }
    this._logger.Debug($"Resuming action with ID {id}");
    this._actionQueueSet.ResumeActionWithoutSynchronizing(id);
  }

  private void HandleRequestEnqueueActionMessage(
    RequestEnqueueActionMessage message,
    ulong senderId)
  {
    if (this._netService.Type != NetGameType.Host)
      throw new InvalidOperationException("Received action enqueue request while not host!");
    this._logger.Debug($"Received request enqueue action for action {message.action} ({senderId}) at {Log.Timestamp}");
    this.EnqueueAction(this.NetActionToGameAction(message.action, senderId), senderId);
  }

  private void HandleActionEnqueuedMessage(ActionEnqueuedMessage message, ulong _)
  {
    if (this._netService.Type != NetGameType.Client)
      throw new InvalidOperationException("Received action enqueued message while not client!");
    this._logger.Debug($"Received handle action enqueued message {message} at {Log.Timestamp}");
    this.EnqueueAction(this.NetActionToGameAction(message.action, message.playerId), message.playerId);
  }

  private void HandleRequestEnqueueHookActionMessage(
    RequestEnqueueHookActionMessage message,
    ulong senderId)
  {
    if (this._netService.Type != NetGameType.Host)
      throw new InvalidOperationException("Received hook action enqueue request while not host!");
    this._logger.Debug($"Received HOOK request enqueue action from {senderId}, hook id: {message.hookActionId}, at {Log.Timestamp}");
    this.EnqueueHookAction(this.GetHookActionForId(message.hookActionId, senderId, message.gameActionType));
  }

  private void HandleHookActionEnqueuedMessage(HookActionEnqueuedMessage message, ulong _)
  {
    if (this._netService.Type != NetGameType.Client)
      throw new InvalidOperationException("Received hook action enqueued message while not host!");
    this._logger.Debug($"Received HOOK request enqueue action, hook id: {message.hookActionId}, at {Log.Timestamp}");
    this.EnqueueHookAction(this.GetHookActionForId(message.hookActionId, message.ownerId, message.gameActionType));
  }

  private void HandleRequestResumeActionAfterPlayerChoiceMessage(
    RequestResumeActionAfterPlayerChoiceMessage afterPlayerChoiceMessage,
    ulong senderId)
  {
    if (this._netService.Type != NetGameType.Host)
      throw new InvalidOperationException("Received action ready request message while not host!");
    this._logger.Debug($"Received action ready request message for action {afterPlayerChoiceMessage.actionId}, sender {senderId}, at {Log.Timestamp}");
    this.ResumeActionAfterPlayerChoice(afterPlayerChoiceMessage.actionId);
  }

  private void HandleResumeActionAfterPlayerChoiceMessage(
    ResumeActionAfterPlayerChoiceMessage afterPlayerChoiceMessage,
    ulong _)
  {
    if (this._netService.Type != NetGameType.Client)
      throw new InvalidOperationException("Received action ready message while not client!");
    this._logger.Debug($"Received action ready message for action {afterPlayerChoiceMessage.actionId} at {Log.Timestamp}");
    this.ResumeActionAfterPlayerChoice(afterPlayerChoiceMessage.actionId);
  }

  private GameAction NetActionToGameAction(INetAction action, ulong actionOwnerId)
  {
    return action.ToGameAction(this._playerCollection.GetPlayer(actionOwnerId) ?? throw new InvalidOperationException($"Action owner ID {actionOwnerId} for action {action} could not be mapped to a Player!"));
  }

  public GenericHookGameAction GetHookActionForId(
    uint id,
    ulong ownerId,
    GameActionType gameActionType)
  {
    GenericHookGameAction action = this._hookActions.Find((Predicate<GenericHookGameAction>) (a => (int) a.HookId == (int) id));
    if (action == null)
    {
      action = new GenericHookGameAction(id, ownerId, gameActionType);
      TaskHelper.RunSafely(action.ExecutionStartedTask.ContinueWith((Action<Task>) (_ => this.HookActionStarted(action))));
      this._hookActions.Add(action);
    }
    else
    {
      if ((long) action.OwnerId != (long) ownerId)
        throw new InvalidOperationException($"Attempted to get hook for owner {ownerId} with hook ID {id}, but the hook already existed and had owner {action.OwnerId}!");
      if (action.ActionType != gameActionType)
        throw new InvalidOperationException($"Generating GenericHookGameAction with type {gameActionType}. Found one already enqueued, but with a mismatching game action type {action.ActionType}!");
    }
    return action;
  }

  private void HookActionStarted(GenericHookGameAction action) => this._hookActions.Remove(action);

  public void FastForwardHookId(uint hookId) => this._nextHookId = hookId;
}
