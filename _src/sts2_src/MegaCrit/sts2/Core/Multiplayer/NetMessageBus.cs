// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.NetMessageBus
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer;

public class NetMessageBus
{
  private readonly PacketReader _reader = new PacketReader();
  private readonly PacketWriter _writer = new PacketWriter();
  private readonly Logger _logger = new Logger(nameof (NetMessageBus), LogType.Network);
  private readonly Dictionary<Type, List<NetMessageBus.CallbackPair>> _messageHandlers = new Dictionary<Type, List<NetMessageBus.CallbackPair>>();
  private readonly List<NetMessageBus.CallbackPair> _cachedPairList = new List<NetMessageBus.CallbackPair>();
  private bool _isBufferingMessages;
  private readonly HashSet<byte> _warnedMessageTypes = new HashSet<byte>();
  private readonly List<(INetMessage, ulong)> _bufferedMessages = new List<(INetMessage, ulong)>();

  public byte[] SerializeMessage<T>(ulong senderId, T message, out int length) where T : INetMessage
  {
    this._writer.Reset();
    this._writer.WriteByte((byte) message.ToId());
    this._writer.WriteULong(senderId);
    message.Serialize(this._writer);
    length = (int) Math.Ceiling((double) this._writer.BitPosition / 8.0);
    return this._writer.Buffer;
  }

  public bool TryDeserializeMessage(
    byte[] packetBytes,
    out INetMessage? message,
    out ulong? overrideSenderId)
  {
    overrideSenderId = new ulong?();
    message = (INetMessage) null;
    this._reader.Reset(packetBytes);
    byte id = this._reader.ReadByte();
    Type type;
    if (!MessageTypes.TryGetMessageType((int) id, out type))
    {
      if (ModManager.IsRunningModded() && (int) id >= MessageTypes.Count && !this._warnedMessageTypes.Contains(id))
      {
        Log.Warn($"Received message with length {packetBytes.Length} and first byte {id} that is outside the bounds of our known messages ({MessageTypes.Count}). Since we are modded, we are assuming this is a message that does not affect gameplay and will not warn about this again.");
        this._warnedMessageTypes.Add(id);
      }
      else
        Log.Error($"Received message with length {packetBytes.Length} and first byte {id} that is not a valid message ID!");
      return false;
    }
    overrideSenderId = new ulong?(this._reader.ReadULong());
    message = (INetMessage) Activator.CreateInstance(type);
    message.Deserialize(this._reader);
    return true;
  }

  public void SendMessageToAllHandlers(INetMessage message, ulong senderId)
  {
    if (this._isBufferingMessages && message.ShouldBuffer)
    {
      this._logger.Debug($"Received message of type {message.GetType()} but we are currently buffering messages.");
      this._bufferedMessages.Add((message, senderId));
    }
    else
    {
      List<NetMessageBus.CallbackPair> collection;
      if (!this._messageHandlers.TryGetValue(message.GetType(), out collection) || collection.Count == 0)
      {
        Log.Error($"Received message of type {message.GetType()}, but no message handlers are registered for that type!");
      }
      else
      {
        this._cachedPairList.Clear();
        this._cachedPairList.AddRange((IEnumerable<NetMessageBus.CallbackPair>) collection);
        this._logger.LogMessage(message.LogLevel, $"Received message {message}, sending to {this._cachedPairList.Count} handlers", 0);
        foreach (NetMessageBus.CallbackPair cachedPair in this._cachedPairList)
        {
          try
          {
            cachedPair.handler(message, senderId);
          }
          catch (Exception ex)
          {
            this._logger.Error($"Exception encountered while processing message {message}: {ex}");
          }
        }
      }
    }
  }

  public void SetBufferMessages(bool bufferMessages)
  {
    if (this._isBufferingMessages == bufferMessages)
      return;
    this._isBufferingMessages = bufferMessages;
    if (bufferMessages)
    {
      this._logger.Debug("NetMessageBus is starting to buffer messages.");
    }
    else
    {
      this._logger.Debug($"NetMessageBus is releasing {this._bufferedMessages.Count} buffered messages.");
      foreach ((INetMessage, ulong) bufferedMessage in this._bufferedMessages)
        this.SendMessageToAllHandlers(bufferedMessage.Item1, bufferedMessage.Item2);
      this._bufferedMessages.Clear();
    }
  }

  public void RegisterMessageHandler<T>(MessageHandlerDelegate<T> handler) where T : INetMessage
  {
    if (typeof (T) == typeof (INetMessage))
      throw new InvalidOperationException("RegisterMessageHandler must be called with a concrete implementation of INetMessage!");
    List<NetMessageBus.CallbackPair> callbackPairList;
    if (!this._messageHandlers.TryGetValue(typeof (T), out callbackPairList))
    {
      callbackPairList = new List<NetMessageBus.CallbackPair>();
      this._messageHandlers[typeof (T)] = callbackPairList;
    }
    callbackPairList.Add(new NetMessageBus.CallbackPair()
    {
      handler = (NetMessageBus.AnonymizedMessageHandlerDelegate) ((message, senderId) => handler((T) message, senderId)),
      originalHandler = (object) handler
    });
  }

  public void UnregisterMessageHandler<T>(MessageHandlerDelegate<T> handler) where T : INetMessage
  {
    if (typeof (T) == typeof (INetMessage))
      throw new InvalidOperationException("UnregisterMessageHandler must be called with a concrete implementation of INetMessage!");
    List<NetMessageBus.CallbackPair> callbackPairList;
    if (!this._messageHandlers.TryGetValue(typeof (T), out callbackPairList))
      return;
    callbackPairList.RemoveAll((Predicate<NetMessageBus.CallbackPair>) (p => (Delegate) p.originalHandler == (Delegate) handler));
  }

  private delegate void AnonymizedMessageHandlerDelegate(INetMessage message, ulong senderId);

  private struct CallbackPair
  {
    public NetMessageBus.AnonymizedMessageHandlerDelegate handler;
    public object originalHandler;
  }
}
