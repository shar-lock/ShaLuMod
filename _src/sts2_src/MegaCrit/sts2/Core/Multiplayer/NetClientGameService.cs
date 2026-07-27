// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.NetClientGameService
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Quality;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Platform;
using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer;

public class NetClientGameService : 
  INetClientHandler,
  INetHandler,
  INetClientGameService,
  INetGameService
{
  private readonly NetMessageBus _messageBus = new NetMessageBus();
  private readonly NetQualityTracker _qualityTracker;

  public event Action? ConnectedToHost;

  public event Action<NetErrorInfo>? Disconnected;

  public NetClient? NetClient { get; private set; }

  public bool IsConnected
  {
    get
    {
      NetClient netClient = this.NetClient;
      return netClient != null && netClient.IsConnected;
    }
  }

  public ulong NetId
  {
    get
    {
      return (this.NetClient ?? throw new InvalidOperationException("Tried to get NetId while not connected!")).NetId;
    }
  }

  public ulong HostNetId
  {
    get
    {
      return (this.NetClient ?? throw new InvalidOperationException("Tried to get HostNetId while not connected!")).HostNetId;
    }
  }

  public bool IsGameLoading => this._qualityTracker.IsGameLoading;

  public PlatformType Platform { get; private set; }

  public NetGameType Type => NetGameType.Client;

  public NetClientGameService()
  {
    this._qualityTracker = new NetQualityTracker((INetGameService) this);
  }

  public void Initialize(NetClient client, PlatformType platform)
  {
    this.NetClient = client;
    this.Platform = platform;
  }

  public void Update()
  {
    NetClient netClient = this.NetClient;
    if ((netClient != null ? (netClient.IsConnected ? 1 : 0) : 0) == 0)
      return;
    this.NetClient.Update();
    this._qualityTracker.Update();
  }

  public void SendMessage<T>(T message, ulong playerId) where T : INetMessage
  {
    long num = (long) playerId;
    ulong? hostNetId = this.NetClient?.HostNetId;
    long valueOrDefault = (long) hostNetId.GetValueOrDefault();
    if (!(num == valueOrDefault & hostNetId.HasValue))
      throw new NotImplementedException("Cannot send messages to non-host players as client!");
    this.SendMessage<T>(message);
  }

  public void SendMessage<T>(T message) where T : INetMessage
  {
    if (!this.IsConnected)
    {
      Log.Error($"Attempted to send message {message} while {this} is not connected!");
    }
    else
    {
      int length;
      this.NetClient.SendMessageToHost(this._messageBus.SerializeMessage<T>(this.NetClient.NetId, message, out length), length, message.Mode, message.Mode.ToChannelId());
    }
  }

  public void RegisterMessageHandler<T>(MessageHandlerDelegate<T> handler) where T : INetMessage
  {
    this._messageBus.RegisterMessageHandler<T>(handler);
  }

  public void UnregisterMessageHandler<T>(MessageHandlerDelegate<T> handler) where T : INetMessage
  {
    this._messageBus.UnregisterMessageHandler<T>(handler);
  }

  public void OnPacketReceived(
    ulong senderId,
    byte[] packetBytes,
    NetTransferMode mode,
    int channel)
  {
    INetMessage message;
    ulong? overrideSenderId;
    if (!this._messageBus.TryDeserializeMessage(packetBytes, out message, out overrideSenderId))
      return;
    senderId = overrideSenderId ?? senderId;
    this._messageBus.SendMessageToAllHandlers(message, senderId);
  }

  public void Disconnect(NetError reason, bool now = false)
  {
    NetClient netClient = this.NetClient;
    if ((netClient != null ? (netClient.IsConnected ? 1 : 0) : 0) == 0)
      return;
    this.NetClient.DisconnectFromHost(reason, now);
  }

  public void OnConnectedToHost()
  {
    this._qualityTracker.OnPeerConnected(this.NetClient.HostNetId);
    Action connectedToHost = this.ConnectedToHost;
    if (connectedToHost == null)
      return;
    connectedToHost();
  }

  public void OnDisconnectedFromHost(ulong hostNetId, NetErrorInfo info)
  {
    this._qualityTracker.OnPeerDisconnected(hostNetId);
    this._qualityTracker.Dispose();
    Action<NetErrorInfo> disconnected = this.Disconnected;
    if (disconnected == null)
      return;
    disconnected(info);
  }

  public ConnectionStats? GetStatsForPeer(ulong peerId)
  {
    return this._qualityTracker.GetStatsForPeer(peerId);
  }

  public void SetGameLoading(bool isLoading) => this._qualityTracker.SetIsLoading(isLoading);

  public void SetBufferMessages(bool bufferMessages)
  {
    this._messageBus.SetBufferMessages(bufferMessages);
  }

  public string? GetRawLobbyIdentifier() => this.NetClient?.GetRawLobbyIdentifier();
}
