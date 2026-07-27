// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Transport.ENet.ENetClient
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Transport.ENet;

public class ENetClient(INetClientHandler handler) : NetClient(handler)
{
  private const int _handshakeTimeoutMsec = 10000;
  private const int _handshakeUpdateRateMsec = 100;
  private readonly Logger _logger = new Logger(nameof (ENetClient), LogType.Network);
  private ENetConnection? _connection;
  private ENetPacketPeer? _peer;
  private bool _isConnected;
  private ulong _netId;

  public override bool IsConnected => this._isConnected;

  public override ulong NetId => this._netId;

  public override ulong HostNetId => 1;

  public async Task<NetErrorInfo?> ConnectToHost(
    ulong netId,
    string ip,
    ushort port,
    CancellationToken cancelToken = default (CancellationToken))
  {
    this._connection = new ENetConnection();
    this._connection.CreateHost(32 /*0x20*/, 0, 0, 0);
    this._peer = this._connection.ConnectToHost(ip, (int) port, 0, 0);
    int timeoutTimer = 0;
    ENetServiceData? output;
    while (!this._connection.TryService(out output) || output.Value.type != 1L)
    {
      await Task.Delay(100, cancelToken);
      if (cancelToken.IsCancellationRequested)
      {
        this.DisconnectFromHost(NetError.CancelledJoin, false);
        this._logger.Warn("User cancelled join flow");
        return new NetErrorInfo?();
      }
      timeoutTimer += 100;
      if (timeoutTimer > 10000)
      {
        this._peer.Reset();
        this._logger.Error("Connection timed out!");
        return new NetErrorInfo?(new NetErrorInfo(NetError.Timeout, false));
      }
    }
    if (this._peer.GetState() != 5L)
    {
      this._logger.Error($"Connection to {ip}:{port} failed!");
      return new NetErrorInfo?(new NetErrorInfo(NetError.UnknownNetworkError, false));
    }
    List<ENetServiceData> bufferedPackets = new List<ENetServiceData>();
    NetErrorInfo? host = await this.SendAndWaitForNetIdAck(netId, bufferedPackets, cancelToken);
    if (host.HasValue)
    {
      this._peer.PeerDisconnect(0);
      return host;
    }
    this._netId = netId;
    this._isConnected = true;
    this._handler.OnConnectedToHost();
    foreach (ENetServiceData data in bufferedPackets)
      this.HandleMessageReceived(data);
    return new NetErrorInfo?();
  }

  private async Task<NetErrorInfo?> SendAndWaitForNetIdAck(
    ulong netId,
    List<ENetServiceData> bufferedPackets,
    CancellationToken cancelToken = default (CancellationToken))
  {
    this._logger.Info($"Sending handshake with net ID {netId}");
    this._peer.Send(0, ENetPacket.FromHandshakeRequest(new ENetHandshakeRequest()
    {
      netId = netId
    }).AllBytes, 1);
    bool receivedAck = false;
    int timeoutTimer = 0;
    while (!receivedAck)
    {
      await Task.Delay(100, cancelToken);
      if (cancelToken.IsCancellationRequested)
      {
        this._logger.Warn("User cancelled join flow");
        this.DisconnectFromHost(NetError.CancelledJoin, false);
        return new NetErrorInfo?();
      }
      ENetServiceData? output;
      if (this._connection.TryService(out output) && output.Value.type == 3L)
      {
        ENetPacket enetPacket = new ENetPacket(output.Value.packetData);
        if (enetPacket.PacketType == ENetPacketType.ApplicationMessage)
        {
          bufferedPackets.Add(output.Value);
          continue;
        }
        ENetHandshakeResponse handshakeResponse = enetPacket.AsHandshakeResponse();
        if ((long) handshakeResponse.netId != (long) netId)
        {
          this._logger.Error($"Received net ID ({handshakeResponse.netId}) during handshake that did not match ours!");
          return new NetErrorInfo?(new NetErrorInfo(NetError.InternalError, false));
        }
        if (handshakeResponse.status != ENetHandshakeStatus.Success)
        {
          this._logger.Error($"Received non-success code during handshake ({handshakeResponse.status})!");
          return new NetErrorInfo?(new NetErrorInfo(NetError.Kicked, false));
        }
        receivedAck = true;
      }
      timeoutTimer += 100;
      if (timeoutTimer > 10000)
      {
        this._logger.Error("Timed out waiting for handshake ack!");
        this.DisconnectFromHost(NetError.Timeout, false);
        return new NetErrorInfo?(new NetErrorInfo(NetError.Timeout, false));
      }
    }
    return new NetErrorInfo?();
  }

  public override void Update()
  {
    this.AssertClientStarted();
    while (true)
    {
      ENetServiceData? output;
      do
      {
        ENetPacketPeer peer = this._peer;
        if ((peer != null ? (peer.IsActive() ? 1 : 0) : 0) != 0)
        {
          ENetConnection connection = this._connection;
          if ((connection != null ? (connection.TryService(out output) ? 1 : 0) : 0) != 0)
          {
            long num = output.Value.type - -1L;
            if (num <= 4L)
            {
              switch ((uint) num)
              {
                case 0:
                  this._logger.Error($"Got error from ENetConnection! Error: {output.Value.error} TODO: Expand me");
                  continue;
                case 2:
                  this._logger.Debug("Received connect on client");
                  continue;
                case 3:
                  this._logger.Debug($"Received disconnect on client. Already disconnected: {!this._isConnected}");
                  continue;
                case 4:
                  goto label_10;
                default:
                  goto label_11;
              }
            }
            else
              goto label_11;
          }
          else
            goto label_12;
        }
        else
          goto label_5;
      }
      while (!this._isConnected);
      this._isConnected = false;
      this._handler.OnDisconnectedFromHost(this.HostNetId, new NetErrorInfo(NetError.UnknownNetworkError, false));
      continue;
label_10:
      this.HandleMessageReceived(output.Value);
    }
label_5:
    return;
label_12:
    return;
label_11:
    throw new ArgumentOutOfRangeException();
  }

  private void HandleMessageReceived(ENetServiceData data)
  {
    if (!this._isConnected)
      return;
    this._logger.VeryDebug($"Received packet of length {data.packetData.Length}");
    ENetPacket enetPacket = new ENetPacket(data.packetData);
    if (enetPacket.PacketType == ENetPacketType.ApplicationMessage)
      this._handler.OnPacketReceived(1UL, enetPacket.AsAppMessage(), data.mode, data.channel);
    else if (enetPacket.PacketType == ENetPacketType.Disconnection)
    {
      ENetDisconnection enetDisconnection = enetPacket.AsDisconnection();
      this._logger.Debug($"Received disconnection packet with reason: {enetDisconnection.reason}");
      this._handler.OnDisconnectedFromHost(this.HostNetId, new NetErrorInfo(enetDisconnection.reason, false));
      this._isConnected = false;
    }
    else
      this._logger.Error($"Got unexpected packet of type {enetPacket.PacketType} while we were connected to the host!");
  }

  public override void DisconnectFromHost(NetError reason, bool now = false)
  {
    if (now)
    {
      this._peer?.PeerDisconnectNow(0);
    }
    else
    {
      this._peer?.Send(0, ENetPacket.FromDisconnection(new ENetDisconnection()
      {
        reason = reason
      }).AllBytes, 8);
      this._peer?.PeerDisconnectLater(0);
    }
    this._connection?.Flush();
    if (!this._isConnected)
      return;
    this._isConnected = false;
    this._handler.OnDisconnectedFromHost(this.HostNetId, new NetErrorInfo(reason, true));
    this._connection?.Destroy();
  }

  public override void SendMessageToHost(
    byte[] bytes,
    int length,
    NetTransferMode mode,
    int channel = 0)
  {
    if (!this._isConnected)
      throw new InvalidOperationException("Tried to send message to host while disconnected!");
    this.AssertClientStarted();
    ENetPacket enetPacket = ENetPacket.FromAppMessage(bytes, length);
    this._logger.VeryDebug($"Sending packet of length {enetPacket.AllBytes.Length}");
    this._peer?.Send(channel, enetPacket.AllBytes, ENetUtil.FlagsFromMode(mode));
  }

  private void AssertClientStarted()
  {
    if (this._connection == null)
      throw new InvalidOperationException("Must call ConnectToHost first and wait for connection!");
  }

  public override string? GetRawLobbyIdentifier() => (string) null;
}
