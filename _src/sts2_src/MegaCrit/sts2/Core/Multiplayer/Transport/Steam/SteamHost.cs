// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Transport.Steam.SteamHost
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Platform.Steam;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Transport.Steam;

public class SteamHost(INetHostHandler handler) : NetHost(handler)
{
  private static readonly List<SteamHost.ClientConnection> _connectionsCache = new List<SteamHost.ClientConnection>();
  private readonly Logger _logger = new Logger(nameof (SteamHost), LogType.Network);
  private Callback<SteamNetConnectionStatusChangedCallback_t>? _netStatusChangedCallback;
  private CSteamID? _lobbyId;
  private HSteamListenSocket _socket;
  private readonly List<SteamHost.ClientConnection> _connections = new List<SteamHost.ClientConnection>();
  private bool _isConnected;

  public override bool IsConnected => this._isConnected;

  public override ulong NetId => SteamUser.GetSteamID().m_SteamID;

  public override IEnumerable<ulong> ConnectedPeerIds
  {
    get
    {
      return this._connections.Select<SteamHost.ClientConnection, ulong>((Func<SteamHost.ClientConnection, ulong>) (c => ((SteamNetworkingIdentity) ref c.netId).GetSteamID64()));
    }
  }

  public CSteamID? LobbyId => this._lobbyId;

  public async Task<NetErrorInfo?> StartHost(int maxPlayers)
  {
    this._logger.Info($"Initializing Steam host. Our player id: {this.NetId}");
    using (SteamCallResult<LobbyCreated_t> callResult = new SteamCallResult<LobbyCreated_t>(SteamMatchmaking.CreateLobby((ELobbyType) 1, maxPlayers), SteamInitializer.DisconnectToken))
    {
      LobbyCreated_t task = await callResult.Task;
      if (task.m_eResult != 1)
      {
        this._logger.Error($"Error creating steam lobby! {task.m_eResult}");
        return new NetErrorInfo?(new NetErrorInfo(task.m_eResult));
      }
      this._lobbyId = new CSteamID?(new CSteamID(task.m_ulSteamIDLobby));
      this._socket = SteamNetworkingSockets.CreateListenSocketP2P(0, 0, new SteamNetworkingConfigValue_t[2]
      {
        new SteamNetworkingConfigValue_t()
        {
          m_eValue = (ESteamNetworkingConfigValue) 24,
          m_eDataType = (ESteamNetworkingConfigDataType) 1,
          m_val = new SteamNetworkingConfigValue_t.OptionValue()
          {
            m_int32 = 20000
          }
        },
        new SteamNetworkingConfigValue_t()
        {
          m_eValue = (ESteamNetworkingConfigValue) 25,
          m_eDataType = (ESteamNetworkingConfigDataType) 1,
          m_val = new SteamNetworkingConfigValue_t.OptionValue()
          {
            m_int32 = 20000
          }
        }
      });
      // ISSUE: method pointer
      this._netStatusChangedCallback = new Callback<SteamNetConnectionStatusChangedCallback_t>(new Callback<SteamNetConnectionStatusChangedCallback_t>.DispatchDelegate((object) this, __methodptr(OnNetStatusChanged)), false);
      this._isConnected = true;
      return new NetErrorInfo?();
    }
  }

  public override void Update()
  {
    SteamHost._connectionsCache.Clear();
    SteamHost._connectionsCache.AddRange((IEnumerable<SteamHost.ClientConnection>) this._connections);
    foreach (SteamHost.ClientConnection clientConnection in SteamHost._connectionsCache)
      SteamUtil.ProcessMessages(clientConnection.conn, (INetHandler) this._handler, this._logger);
  }

  public override void SetHostIsClosed(bool isClosed)
  {
    SteamMatchmaking.SetLobbyType(this._lobbyId.Value, isClosed ? (ELobbyType) 0 : (ELobbyType) 1);
  }

  private void OnNetStatusChanged(SteamNetConnectionStatusChangedCallback_t data)
  {
    this._logger.Debug($"Connection status changed: ({((SteamNetworkingIdentity) ref data.m_info.m_identityRemote).GetSteamID64()}: {data.m_eOldState} -> {data.m_info.m_eState}");
    if (data.m_info.m_eState == 1)
    {
      if (!this.IsInLobby(data.m_info.m_identityRemote))
      {
        this._logger.Warn($"Player with steam id {((SteamNetworkingIdentity) ref data.m_info.m_identityRemote).GetSteamID64()} attempted to join the game, but they are not in the lobby (id {this._lobbyId.Value})");
        this.CloseConnectionAndRemove(data.m_hConn, NetError.TryAgainLater.ToSteam(), "Player is not in the lobby!", true, false);
      }
      else
      {
        this._logger.Info($"Accepting new connection with user {((SteamNetworkingIdentity) ref data.m_info.m_identityRemote).GetSteamID64()}");
        EResult eresult = SteamNetworkingSockets.AcceptConnection(data.m_hConn);
        if (eresult == 1)
          return;
        this._logger.Error($"Tried to accept connection with user {((SteamNetworkingIdentity) ref data.m_info.m_identityRemote).GetSteamID64()} but it returned result {eresult}!");
        this.CloseConnectionAndRemove(data.m_hConn, SteamDisconnectionReason.AppInternalError, $"Connection accept failure: {eresult}", true, false);
      }
    }
    else if (data.m_info.m_eState == 3)
    {
      this._connections.Add(new SteamHost.ClientConnection()
      {
        conn = data.m_hConn,
        netId = data.m_info.m_identityRemote
      });
      this._handler.OnPeerConnected(((SteamNetworkingIdentity) ref data.m_info.m_identityRemote).GetSteamID64());
    }
    else if (data.m_info.m_eState == 5)
    {
      this._logger.Info($"Steam connection closed because of problem. Reason: {data.m_info.m_eEndReason}, {((SteamNetConnectionInfo_t) ref data.m_info).m_szEndDebug}");
      this.CloseConnectionAndRemove(data.m_hConn, (SteamDisconnectionReason) data.m_info.m_eEndReason, ((SteamNetConnectionInfo_t) ref data.m_info).m_szEndDebug, true, false);
    }
    else
    {
      if (data.m_info.m_eState != 4)
        return;
      this._logger.Info($"Steam connection closed by peer. Reason: {data.m_info.m_eEndReason}, {((SteamNetConnectionInfo_t) ref data.m_info).m_szEndDebug}");
      this.CloseConnectionAndRemove(data.m_hConn, (SteamDisconnectionReason) data.m_info.m_eEndReason, ((SteamNetConnectionInfo_t) ref data.m_info).m_szEndDebug, true, false);
    }
  }

  public override void SendMessageToClient(
    ulong peerId,
    byte[] bytes,
    int length,
    NetTransferMode mode,
    int channel = 0)
  {
    this._logger.VeryDebug($"Sending {length} bytes to client {peerId}");
    SteamNetworkingIdentity networkingIdentity = new SteamNetworkingIdentity();
    ((SteamNetworkingIdentity) ref networkingIdentity).SetSteamID64(peerId);
    SteamHost.ClientConnection? connectionForNetId = this.GetConnectionForNetId(peerId);
    if (!connectionForNetId.HasValue)
      throw new InvalidOperationException($"Could not find connection for peer {peerId}!");
    GCHandle gcHandle = GCHandle.Alloc((object) bytes, (GCHandleType) 3);
    try
    {
      long num;
      EResult connection = SteamNetworkingSockets.SendMessageToConnection(connectionForNetId.Value.conn, gcHandle.AddrOfPinnedObject(), (uint) length, SteamUtil.FlagsFromMode(mode), ref num);
      if (connection == 1)
        return;
      this._logger.Warn($"Failed to send message length {length} to peer {peerId}: {connection}");
    }
    finally
    {
      gcHandle.Free();
    }
  }

  public override void SendMessageToAll(
    byte[] bytes,
    int length,
    NetTransferMode mode,
    int channel = 0)
  {
    foreach (SteamHost.ClientConnection connection in this._connections)
    {
      SteamNetworkingIdentity netId = connection.netId;
      this.SendMessageToClient(((SteamNetworkingIdentity) ref netId).GetSteamID64(), bytes, length, mode, channel);
    }
  }

  private SteamHost.ClientConnection? GetConnectionForNetId(ulong peerId)
  {
    foreach (SteamHost.ClientConnection connection in this._connections)
    {
      SteamNetworkingIdentity netId = connection.netId;
      if ((long) ((SteamNetworkingIdentity) ref netId).GetSteamID64() == (long) peerId)
        return new SteamHost.ClientConnection?(connection);
    }
    return new SteamHost.ClientConnection?();
  }

  public override void DisconnectClient(ulong peerId, NetError reason, bool now = false)
  {
    HSteamNetConnection? nullable = new HSteamNetConnection?();
    this._logger.Debug($"Disconnecting peer {peerId}, reason: {reason}");
    foreach (SteamHost.ClientConnection connection in this._connections)
    {
      SteamNetworkingIdentity netId = connection.netId;
      if ((long) ((SteamNetworkingIdentity) ref netId).GetSteamID64() == (long) peerId)
      {
        nullable = new HSteamNetConnection?(connection.conn);
        break;
      }
    }
    if (!nullable.HasValue)
      return;
    this.CloseConnectionAndRemove(nullable.Value, reason.ToSteam(), (string) null, now, true);
  }

  private void CloseConnectionAndRemove(
    HSteamNetConnection conn,
    SteamDisconnectionReason reason,
    string? debugReason,
    bool now,
    bool selfInitiated)
  {
    SteamNetworkingSockets.CloseConnection(conn, (int) reason, debugReason, !now);
    int index = this._connections.FindIndex((Predicate<SteamHost.ClientConnection>) (c => HSteamNetConnection.op_Equality(c.conn, conn)));
    if (index < 0)
      return;
    SteamHost.ClientConnection connection = this._connections[index];
    this._connections.RemoveAt(index);
    this._handler.OnPeerDisconnected(((SteamNetworkingIdentity) ref connection.netId).GetSteamID64(), new NetErrorInfo(reason, debugReason, selfInitiated));
  }

  public override void StopHost(NetError reason, bool now = false)
  {
    this._logger.Debug("Stopping host");
    foreach (SteamHost.ClientConnection connection in this._connections)
      SteamNetworkingSockets.CloseConnection(connection.conn, (int) reason.ToSteam(), (string) null, !now);
    this._connections.Clear();
    SteamMatchmaking.LeaveLobby(this._lobbyId.Value);
    if (now)
      SteamNetworkingSockets.CloseListenSocket(this._socket);
    else
      TaskHelper.RunSafely(this.CloseSocketAfterDelay(this._socket));
    this._lobbyId = new CSteamID?();
    this._isConnected = false;
    this._netStatusChangedCallback?.Dispose();
    this._netStatusChangedCallback = (Callback<SteamNetConnectionStatusChangedCallback_t>) null;
    this._handler.OnDisconnected(new NetErrorInfo(reason, true));
  }

  private async Task CloseSocketAfterDelay(HSteamListenSocket socket)
  {
    await Task.Delay(1000);
    this._logger.Debug("Closing socket after delay.");
    SteamNetworkingSockets.CloseListenSocket(socket);
  }

  private bool IsInLobby(SteamNetworkingIdentity id)
  {
    CSteamID steamId = ((SteamNetworkingIdentity) ref id).GetSteamID();
    if (CSteamID.op_Equality(steamId, CSteamID.Nil))
      return false;
    int numLobbyMembers = SteamMatchmaking.GetNumLobbyMembers(this._lobbyId.Value);
    for (int index = 0; index < numLobbyMembers; ++index)
    {
      CSteamID lobbyMemberByIndex = SteamMatchmaking.GetLobbyMemberByIndex(this._lobbyId.Value, index);
      if (CSteamID.op_Equality(steamId, lobbyMemberByIndex))
        return true;
    }
    return false;
  }

  public override string? GetRawLobbyIdentifier()
  {
    ref CSteamID? local = ref this._lobbyId;
    return !local.HasValue ? (string) null : local.GetValueOrDefault().m_SteamID.ToString();
  }

  private struct ClientConnection
  {
    public HSteamNetConnection conn;
    public SteamNetworkingIdentity netId;
  }
}
