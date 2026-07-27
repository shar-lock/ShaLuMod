// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Transport.ENet.ENetHost
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Transport.ENet;

public class ENetHost(INetHostHandler handler) : NetHost(handler)
{
  private const int _handshakeTimeoutMsec = 10000;
  private const int _handshakeUpdateRateMsec = 100;
  private readonly List<ENetHost.ClientConnection> _connectedPeers = new List<ENetHost.ClientConnection>();
  private ENetConnection? _connection;
  private bool _isConnected;
  private readonly List<ENetHost.HandshakeAwaitingResponse> _receivedHandshakes = new List<ENetHost.HandshakeAwaitingResponse>();
  private readonly Logger _logger = new Logger(nameof (ENetHost), LogType.Network);

  public override bool IsConnected => this._isConnected;

  public override IEnumerable<ulong> ConnectedPeerIds
  {
    get
    {
      return this._connectedPeers.Select<ENetHost.ClientConnection, ulong>((Func<ENetHost.ClientConnection, ulong>) (c => c.netId));
    }
  }

  public override ulong NetId => 1;

  public NetErrorInfo? StartHost(ushort port, int maxClients)
  {
    this._connection = new ENetConnection();
    Error hostBound = this._connection.CreateHostBound("0.0.0.0", (int) port, maxClients, 0, 0, 0);
    if (hostBound != null)
    {
      this._logger.Error($"Failed to create host! {hostBound}");
      return new NetErrorInfo?(new NetErrorInfo(hostBound));
    }
    this._isConnected = true;
    return new NetErrorInfo?();
  }

  private async Task DoClientHandshake(ENetPacketPeer peer)
  {
    peer.SetTimeout(24, 20000, 20000);
    int timeoutTimer = 0;
    ENetHost.HandshakeAwaitingResponse? handshake = new ENetHost.HandshakeAwaitingResponse?();
    while (!handshake.HasValue)
    {
      foreach (ENetHost.HandshakeAwaitingResponse receivedHandshake in this._receivedHandshakes)
      {
        if (receivedHandshake.conn.peer == peer)
        {
          handshake = new ENetHost.HandshakeAwaitingResponse?(receivedHandshake);
          break;
        }
      }
      if (!handshake.HasValue)
      {
        await Task.Delay(100);
        timeoutTimer += 100;
        if (timeoutTimer >= 10000)
        {
          this._logger.Error("Timed out waiting for handshake!");
          peer.Reset();
          handshake = new ENetHost.HandshakeAwaitingResponse?();
          return;
        }
      }
    }
    if (this.GetConnectionById(handshake.Value.conn.netId).HasValue)
    {
      this._logger.Info($"Second client attempted to connect with peer ID {handshake.Value.conn.netId}, disconnecting them");
      handshake.Value.conn.peer.Send(0, ENetPacket.FromHandshakeResponse(new ENetHandshakeResponse()
      {
        netId = handshake.Value.conn.netId,
        status = ENetHandshakeStatus.IdCollision
      }).AllBytes, 1);
      handshake.Value.conn.peer.PeerDisconnect(0);
      handshake = new ENetHost.HandshakeAwaitingResponse?();
    }
    else
    {
      this._logger.Debug($"Acknowledging handshake for peer with ID {handshake.Value.conn.netId}");
      handshake.Value.conn.peer.Send(0, ENetPacket.FromHandshakeResponse(new ENetHandshakeResponse()
      {
        netId = handshake.Value.conn.netId,
        status = ENetHandshakeStatus.Success
      }).AllBytes, 1);
      this._connectedPeers.Add(handshake.Value.conn);
      this._handler.OnPeerConnected(handshake.Value.conn.netId);
      handshake = new ENetHost.HandshakeAwaitingResponse?();
    }
  }

  private void HandleClientDisconnection(
    ENetHost.ClientConnection conn,
    NetError reason,
    bool notifyHandler = true)
  {
    this._logger.Debug($"Peer ID {conn.netId} disconnected, reason: {reason}");
    this._connectedPeers.Remove(conn);
    if (!notifyHandler)
      return;
    this._handler.OnPeerDisconnected(conn.netId, new NetErrorInfo(reason, false));
  }

  private ENetHost.ClientConnection? GetConnectionByPeer(ENetPacketPeer peer)
  {
    foreach (ENetHost.ClientConnection connectedPeer in this._connectedPeers)
    {
      if (connectedPeer.peer == peer)
        return new ENetHost.ClientConnection?(connectedPeer);
    }
    return new ENetHost.ClientConnection?();
  }

  private ENetHost.ClientConnection? GetConnectionById(ulong id)
  {
    foreach (ENetHost.ClientConnection connectedPeer in this._connectedPeers)
    {
      if ((long) connectedPeer.netId == (long) id)
        return new ENetHost.ClientConnection?(connectedPeer);
    }
    return new ENetHost.ClientConnection?();
  }

  public override void Update()
  {
    this.AssertHostStarted();
    ENetServiceData? output;
    while (this._connection.TryService(out output))
    {
      long num = output.Value.type - -1L;
      if (num <= 4L)
      {
        switch ((uint) num)
        {
          case 0:
            this._logger.Error("Got error from ENetConnection! TODO: Expand me");
            continue;
          case 2:
            TaskHelper.RunSafely(this.DoClientHandshake(output.Value.peer));
            continue;
          case 3:
            continue;
          case 4:
            this.HandlePacketReceived(output.Value);
            continue;
        }
      }
      throw new ArgumentOutOfRangeException();
    }
  }

  private void HandlePacketReceived(ENetServiceData data)
  {
    byte[] packetData = data.packetData;
    this._logger.VeryDebug($"Received packet of length {packetData.Length}");
    ENetPacketPeer peer = data.peer;
    ENetPacket enetPacket = new ENetPacket(packetData);
    if (enetPacket.PacketType == ENetPacketType.HandshakeRequest)
    {
      ENetHandshakeRequest handshakeRequest = enetPacket.AsHandshakeRequest();
      ENetHost.ClientConnection clientConnection = new ENetHost.ClientConnection();
      clientConnection.peer = peer;
      clientConnection.netId = handshakeRequest.netId;
      ENetHost.HandshakeAwaitingResponse awaitingResponse = new ENetHost.HandshakeAwaitingResponse();
      awaitingResponse.conn = clientConnection;
      awaitingResponse.receivedMsec = Time.GetTicksMsec();
      this._logger.Debug($"Received handshake packet containing peer ID {clientConnection.netId}");
      this._receivedHandshakes.Add(awaitingResponse);
    }
    else if (enetPacket.PacketType == ENetPacketType.Disconnection)
    {
      ENetDisconnection enetDisconnection = enetPacket.AsDisconnection();
      ENetHost.ClientConnection? connectionByPeer = this.GetConnectionByPeer(peer);
      if (!connectionByPeer.HasValue)
        return;
      this.HandleClientDisconnection(connectionByPeer.Value, enetDisconnection.reason);
    }
    else
    {
      ENetHost.ClientConnection? connectionByPeer = this.GetConnectionByPeer(peer);
      if (!connectionByPeer.HasValue)
        this._logger.Error($"Received a non-handshake packet length {packetData.Length} for a peer, but no connection exists!");
      else
        this._handler.OnPacketReceived(connectionByPeer.Value.netId, enetPacket.AsAppMessage(), data.mode, data.channel);
    }
  }

  public override void SendMessageToClient(
    ulong peerId,
    byte[] bytes,
    int length,
    NetTransferMode mode,
    int channel = 0)
  {
    this.AssertHostStarted();
    ENetHost.ClientConnection? connectionById = this.GetConnectionById(peerId);
    if (!connectionById.HasValue)
    {
      this._logger.Error($"Tried to send message to client with ID {peerId}, but no client with that ID is connected!");
    }
    else
    {
      ENetPacket enetPacket = ENetPacket.FromAppMessage(bytes, length);
      this._logger.VeryDebug($"Sending packet of length {enetPacket.AllBytes.Length}");
      connectionById.Value.peer.Send(channel, enetPacket.AllBytes, ENetUtil.FlagsFromMode(mode));
    }
  }

  public override void DisconnectClient(ulong peerId, NetError reason, bool now = false)
  {
    this.DisconnectClientInternal(peerId, reason, now);
  }

  private void DisconnectClientInternal(
    ulong peerId,
    NetError reason,
    bool now = false,
    bool notifyHandler = true)
  {
    this._logger.Info($"Disconnecting client {peerId}, reason: {reason}. Now: {now}");
    this.AssertHostStarted();
    ENetHost.ClientConnection? connectionById = this.GetConnectionById(peerId);
    if (!connectionById.HasValue)
      return;
    if (now)
    {
      connectionById.Value.peer.PeerDisconnectNow(0);
    }
    else
    {
      ENetPacket enetPacket = ENetPacket.FromDisconnection(new ENetDisconnection()
      {
        reason = reason
      });
      connectionById.Value.peer.Send(0, enetPacket.AllBytes, 1);
      connectionById.Value.peer.PeerDisconnectLater(0);
    }
    this.HandleClientDisconnection(connectionById.Value, reason, notifyHandler);
  }

  public override void SendMessageToAll(
    byte[] bytes,
    int length,
    NetTransferMode mode,
    int channel = 0)
  {
    this.AssertHostStarted();
    foreach (ENetHost.ClientConnection connectedPeer in this._connectedPeers)
      this.SendMessageToClient(connectedPeer.netId, bytes, length, mode, channel);
  }

  public override void SetHostIsClosed(bool isClosed)
  {
  }

  public override void StopHost(NetError reason, bool now = false)
  {
    Log.Info($"Stopping host. Reason: {reason}. Now: {now}");
    foreach (ENetHost.ClientConnection clientConnection in this._connectedPeers.ToList<ENetHost.ClientConnection>())
      this.DisconnectClientInternal(clientConnection.netId, reason, now, false);
    this._connectedPeers.Clear();
    this._connection.Flush();
    this._connection.Destroy();
    this._isConnected = false;
    this._handler.OnDisconnected(new NetErrorInfo(reason, true));
  }

  private void AssertHostStarted()
  {
    if (this._connection == null)
      throw new InvalidOperationException("Must call StartHost first!");
  }

  public override string? GetRawLobbyIdentifier() => (string) null;

  private record struct ClientConnection
  {
    public ulong netId;
    public ENetPacketPeer peer;

    [CompilerGenerated]
    public override readonly int GetHashCode()
    {
      return EqualityComparer<ulong>.Default.GetHashCode(this.netId) * -1521134295 + EqualityComparer<ENetPacketPeer>.Default.GetHashCode(this.peer);
    }

    [CompilerGenerated]
    public readonly bool Equals(ENetHost.ClientConnection other)
    {
      return EqualityComparer<ulong>.Default.Equals(this.netId, other.netId) && EqualityComparer<ENetPacketPeer>.Default.Equals(this.peer, other.peer);
    }
  }

  private struct HandshakeAwaitingResponse
  {
    public ulong receivedMsec;
    public ENetHost.ClientConnection conn;
  }
}
