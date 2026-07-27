// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Game.RunLocationTargetedMessageBuffer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Game;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Game;

public class RunLocationTargetedMessageBuffer
{
  private readonly INetGameService _gameService;
  private readonly List<RunLocationTargetedMessageBuffer.BlockedMessage> _messagesWaitingOnLocationChange = new List<RunLocationTargetedMessageBuffer.BlockedMessage>();
  private readonly List<RunLocationTargetedMessageBuffer.TypeAndMessageHandlers> _messageHandlers = new List<RunLocationTargetedMessageBuffer.TypeAndMessageHandlers>();
  private readonly HashSet<RunLocation> _visitedLocations = new HashSet<RunLocation>();
  private readonly Logger _logger = new Logger(nameof (RunLocationTargetedMessageBuffer), LogType.GameSync);

  public RunLocation CurrentLocation { get; private set; }

  public RunLocationTargetedMessageBuffer(INetGameService gameService)
  {
    this._gameService = gameService;
    this._visitedLocations.Add(this.CurrentLocation);
  }

  public void OnLocationChanged(RunLocation location)
  {
    this._logger.Debug($"Run location changed to {location} (previously at: {this.CurrentLocation}), checking if we have enqueued messages");
    this.CurrentLocation = location;
    this._visitedLocations.Add(this.CurrentLocation);
    for (int index = 0; index < this._messagesWaitingOnLocationChange.Count; ++index)
    {
      RunLocationTargetedMessageBuffer.BlockedMessage blockedMessage = this._messagesWaitingOnLocationChange[index];
      if (this._visitedLocations.Contains(blockedMessage.location))
      {
        this._logger.Debug($"Handling enqueued message {blockedMessage.message} of type {blockedMessage.messageType} from {blockedMessage.senderId}");
        this.CallHandlersOfType(blockedMessage.messageType, blockedMessage.message, blockedMessage.senderId);
        this._messagesWaitingOnLocationChange.RemoveAt(index);
        --index;
      }
    }
    if (this._messagesWaitingOnLocationChange.Count <= 0)
      return;
    this._logger.Error($"After transitioning to {location}, there are still {this._messagesWaitingOnLocationChange.Count} messages for other locations. This is likely indicates a bug. Messages:\n{string.Join<RunLocationTargetedMessageBuffer.BlockedMessage>("\n", (IEnumerable<RunLocationTargetedMessageBuffer.BlockedMessage>) this._messagesWaitingOnLocationChange)}");
  }

  private void CallHandlersOfType(Type type, INetMessage message, ulong senderId)
  {
    foreach (RunLocationTargetedMessageBuffer.TypeAndMessageHandlers messageHandler in this._messageHandlers)
    {
      if (messageHandler.messageType == type)
      {
        foreach (RunLocationTargetedMessageBuffer.MessageHandler handler in messageHandler.handlers)
          handler.anonymizedHandler(message, senderId);
      }
    }
  }

  public void RegisterMessageHandler<T>(MessageHandlerDelegate<T> handler) where T : INetMessage, IRunLocationTargetedMessage
  {
    this._logger.VeryDebug($"Register message handler {handler} for {typeof (T)}");
    RunLocationTargetedMessageBuffer.TypeAndMessageHandlers? nullable = new RunLocationTargetedMessageBuffer.TypeAndMessageHandlers?();
    foreach (RunLocationTargetedMessageBuffer.TypeAndMessageHandlers messageHandler in this._messageHandlers)
    {
      if (messageHandler.messageType == typeof (T))
        nullable = new RunLocationTargetedMessageBuffer.TypeAndMessageHandlers?(messageHandler);
    }
    if (!nullable.HasValue)
    {
      MessageHandlerDelegate<T> messageHandlerDelegate = new MessageHandlerDelegate<T>(this.HandleMessage<T>);
      nullable = new RunLocationTargetedMessageBuffer.TypeAndMessageHandlers?(new RunLocationTargetedMessageBuffer.TypeAndMessageHandlers()
      {
        messageType = typeof (T),
        netServiceHandler = (object) messageHandlerDelegate,
        handlers = new List<RunLocationTargetedMessageBuffer.MessageHandler>()
      });
      this._gameService.RegisterMessageHandler<T>(messageHandlerDelegate);
      this._messageHandlers.Add(nullable.Value);
    }
    nullable.Value.handlers.Add(new RunLocationTargetedMessageBuffer.MessageHandler()
    {
      anonymizedHandler = new RunLocationTargetedMessageBuffer.AnonymizedMessageHandlerDelegate(AnonymousDelegate),
      originalHandler = (object) handler
    });

    void AnonymousDelegate(INetMessage message, ulong senderId) => handler((T) message, senderId);
  }

  public void UnregisterMessageHandler<T>(MessageHandlerDelegate<T> handler) where T : INetMessage, IRunLocationTargetedMessage
  {
    for (int index1 = 0; index1 < this._messageHandlers.Count; ++index1)
    {
      RunLocationTargetedMessageBuffer.TypeAndMessageHandlers messageHandler = this._messageHandlers[index1];
      if (!(messageHandler.messageType != typeof (T)))
      {
        for (int index2 = 0; index2 < messageHandler.handlers.Count; ++index2)
        {
          if (messageHandler.handlers[index2].originalHandler is MessageHandlerDelegate<T> originalHandler && originalHandler == handler)
          {
            messageHandler.handlers.RemoveAt(index2);
            --index2;
          }
        }
        if (messageHandler.handlers.Count <= 0)
        {
          this._gameService.UnregisterMessageHandler<T>((MessageHandlerDelegate<T>) messageHandler.netServiceHandler);
          this._messageHandlers.RemoveAt(index1);
          --index1;
        }
      }
    }
  }

  private void HandleMessage<T>(T message, ulong senderId) where T : INetMessage, IRunLocationTargetedMessage
  {
    this._logger.VeryDebug($"Handling map-targeted message {message} from {senderId} for location {message.Location}");
    if (this._visitedLocations.Contains(message.Location))
    {
      this.CallHandlersOfType(typeof (T), (INetMessage) message, senderId);
    }
    else
    {
      this._logger.Debug($"Message {message} from {senderId} is for location {message.Location}, enqueueing it because we are currently at location {this.CurrentLocation}");
      this._messagesWaitingOnLocationChange.Add(new RunLocationTargetedMessageBuffer.BlockedMessage()
      {
        location = message.Location,
        message = (INetMessage) message,
        messageType = message.GetType(),
        senderId = senderId
      });
    }
  }

  private delegate void AnonymizedMessageHandlerDelegate(INetMessage message, ulong senderId);

  private struct TypeAndMessageHandlers
  {
    public Type messageType;
    public object netServiceHandler;
    public List<RunLocationTargetedMessageBuffer.MessageHandler> handlers;
  }

  private struct MessageHandler
  {
    public object originalHandler;
    public RunLocationTargetedMessageBuffer.AnonymizedMessageHandlerDelegate anonymizedHandler;
  }

  private struct BlockedMessage
  {
    public RunLocation location;
    public INetMessage message;
    public ulong senderId;
    public Type messageType;
  }
}
