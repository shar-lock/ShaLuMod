// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.NetHostGameService
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Quality;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Multiplayer.Transport.ENet;
using MegaCrit.Sts2.Core.Multiplayer.Transport.Steam;
using MegaCrit.Sts2.Core.Platform;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer;

public class NetHostGameService : INetHostHandler, INetHandler, INetHostGameService, INetGameService
{
  private NetHost? _netHost;
  private readonly NetMessageBus _messageBus = new NetMessageBus();
  private readonly NetQualityTracker _qualityTracker;
  private readonly List<NetClientData> _connectedPeers = new List<NetClientData>();

  public event Action<NetErrorInfo>? Disconnected;

  public event Action<ulong>? ClientConnected;

  public event Action<ulong, NetErrorInfo>? ClientDisconnected;

  public bool IsConnected
  {
    get
    {
      NetHost netHost = this._netHost;
      return netHost != null && netHost.IsConnected;
    }
  }

  public IReadOnlyList<NetClientData> ConnectedPeers
  {
    get => (IReadOnlyList<NetClientData>) this._connectedPeers;
  }

  public ulong NetId
  {
    get
    {
      return (this._netHost ?? throw new InvalidOperationException("Tried to get NetId while not connected!")).NetId;
    }
  }

  public bool IsGameLoading => this._qualityTracker.IsGameLoading;

  public PlatformType Platform { get; private set; }

  public NetHost? NetHost => this._netHost;

  public NetGameType Type => NetGameType.Host;

  public NetHostGameService()
  {
    this._qualityTracker = new NetQualityTracker((INetGameService) this);
  }

  public NetErrorInfo? StartENetHost(ushort port, int maxClients)
  {
    ENetHost enetHost = new ENetHost((INetHostHandler) this);
    this._netHost = (NetHost) enetHost;
    return enetHost.StartHost(port, maxClients);
  }

  public Task<NetErrorInfo?> StartSteamHost(int maxClients)
  {
    SteamHost steamHost = new SteamHost((INetHostHandler) this);
    this._netHost = (NetHost) steamHost;
    this.Platform = PlatformType.Steam;
    return steamHost.StartHost(maxClients);
  }

  public void Update()
  {
    NetHost netHost = this._netHost;
    if ((netHost != null ? (netHost.IsConnected ? 1 : 0) : 0) == 0)
      return;
    this._netHost.Update();
    this._qualityTracker.Update();
  }

  public void SendMessage<T>(T message, ulong peerId) where T : INetMessage
  {
    this.SendMessageToClientInternal<T>(message, peerId, message.Mode.ToChannelId(), new ulong?());
  }

  private void SendMessageToClientInternal<T>(
    T message,
    ulong peerId,
    int channel,
    ulong? overrideSenderId)
    where T : INetMessage
  {
    if (!this.IsConnected)
    {
      Log.Error($"Attempted to send message {message} while {this} is not connected!");
    }
    else
    {
      int length;
      byte[] bytes = this._messageBus.SerializeMessage<T>((ulong) ((long) overrideSenderId ?? (long) this._netHost.NetId), message, out length);
      this._netHost.SendMessageToClient(peerId, bytes, length, message.Mode, channel);
    }
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
      byte[] bytes = this._messageBus.SerializeMessage<T>(this._netHost.NetId, message, out length);
      foreach (NetClientData connectedPeer in this._connectedPeers)
      {
        if (connectedPeer.readyForBroadcasting)
          this._netHost.SendMessageToClient(connectedPeer.peerId, bytes, length, message.Mode, message.Mode.ToChannelId());
      }
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
    if (message.ShouldBroadcast)
      this.BroadcastMessage<INetMessage>(message, senderId, channel, overrideSenderId.Value);
    senderId = overrideSenderId ?? senderId;
    this._messageBus.SendMessageToAllHandlers(message, senderId);
  }

  private void BroadcastMessage<T>(
    T message,
    ulong excludePeerId,
    int channel,
    ulong overrideSenderId)
    where T : INetMessage
  {
    foreach (NetClientData connectedPeer in this._connectedPeers)
    {
      if (connectedPeer.readyForBroadcasting && (long) connectedPeer.peerId != (long) excludePeerId)
        this.SendMessageToClientInternal<T>(message, connectedPeer.peerId, channel, new ulong?(overrideSenderId));
    }
  }

  public void SetPeerReadyForBroadcasting(ulong peerId)
  {
    for (int index = 0; index < this._connectedPeers.Count; ++index)
    {
      if ((long) this._connectedPeers[index].peerId == (long) peerId)
      {
        NetClientData connectedPeer = this._connectedPeers[index] with
        {
          readyForBroadcasting = true
        };
        this._connectedPeers[index] = connectedPeer;
      }
    }
  }

  public void DisconnectClient(ulong peerId, NetError reason, bool now = false)
  {
    this._netHost.DisconnectClient(peerId, reason, now);
  }

  public void Disconnect(NetError reason, bool now = false)
  {
    NetHost netHost = this._netHost;
    if ((netHost != null ? (netHost.IsConnected ? 1 : 0) : 0) == 0)
      return;
    this._netHost.StopHost(reason, now);
    this._qualityTracker.Dispose();
  }

  public void OnDisconnected(NetErrorInfo info)
  {
    Action<NetErrorInfo> disconnected = this.Disconnected;
    if (disconnected == null)
      return;
    disconnected(info);
  }

  public void OnPeerConnected(ulong peerId)
  {
    this._connectedPeers.Add(new NetClientData()
    {
      peerId = peerId,
      readyForBroadcasting = false
    });
    this._qualityTracker.OnPeerConnected(peerId);
    Action<ulong> clientConnected = this.ClientConnected;
    if (clientConnected == null)
      return;
    clientConnected(peerId);
  }

  public void OnPeerDisconnected(ulong peerId, NetErrorInfo info)
  {
    this._connectedPeers.RemoveAll((Predicate<NetClientData>) (p => (long) p.peerId == (long) peerId));
    this._qualityTracker.OnPeerDisconnected(peerId);
    Action<ulong, NetErrorInfo> clientDisconnected = this.ClientDisconnected;
    if (clientDisconnected == null)
      return;
    clientDisconnected(peerId, info);
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

  public string? GetRawLobbyIdentifier() => this._netHost?.GetRawLobbyIdentifier();
}
