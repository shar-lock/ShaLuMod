// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Transport.Steam.SteamClient
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Platform.Steam;
using Steamworks;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Transport.Steam;

public class SteamClient(INetClientHandler handler) : NetClient(handler)
{
  private CSteamID? _lobbyId;
  private CSteamID _hostNetId;
  private HSteamNetConnection? _conn;
  private bool _isConnected;
  private TaskCompletionSource<SteamClient.ConnectionResult>? _connectingTaskCompletionSource;
  private Callback<SteamNetConnectionStatusChangedCallback_t>? _netStatusChangedCallback;
  private readonly Logger _logger = new Logger(nameof (SteamClient), LogType.Network);

  public override bool IsConnected => this._isConnected;

  public override ulong NetId => SteamUser.GetSteamID().m_SteamID;

  public override ulong HostNetId => this._hostNetId.m_SteamID;

  public CSteamID? LobbyId => this._lobbyId;

  public Task<NetErrorInfo?> ConnectToLobbyOwnedByFriend(
    ulong steamPlayerId,
    CancellationToken cancelToken = default (CancellationToken))
  {
    this._logger.Info($"Attempting to connect to lobby. Our player id: {this.NetId}. Other player's id: {steamPlayerId}.");
    FriendGameInfo_t friendGameInfoT;
    if (!SteamFriends.GetFriendGamePlayed(new CSteamID(steamPlayerId), ref friendGameInfoT))
    {
      Log.Warn("Tried to join a friend's Steam game, but they're not playing any game. Likely a stale invite");
      return Task.FromResult<NetErrorInfo?>(new NetErrorInfo?(new NetErrorInfo(NetError.InvalidJoin, false)));
    }
    if (!CGameID.op_Inequality(friendGameInfoT.m_gameID, new CGameID(2868840UL)) && friendGameInfoT.m_steamIDLobby.m_SteamID != 0UL)
      return this.ConnectToLobby(friendGameInfoT.m_steamIDLobby.m_SteamID, cancelToken);
    Log.Warn("Tried to join a friend's Steam game, but they are not playing STS2 or they are not in a lobby. Likely a stale invite");
    return Task.FromResult<NetErrorInfo?>(new NetErrorInfo?(new NetErrorInfo(NetError.InvalidJoin, false)));
  }

  public async Task<NetErrorInfo?> ConnectToLobby(ulong lobbyId, CancellationToken cancelToken = default (CancellationToken))
  {
    SteamCallResult<LobbyEnter_t> callResult;
    await using (cancelToken.Register(new Action(this.CancelConnection)))
    {
      callResult = new SteamCallResult<LobbyEnter_t>(SteamMatchmaking.JoinLobby(new CSteamID(lobbyId)), cancelToken);
      try
      {
        this._logger.Debug($"Attempting to enter lobby {lobbyId}");
        LobbyEnter_t task1 = await callResult.Task;
        if ((long) task1.m_ulSteamIDLobby != (long) lobbyId)
        {
          this._logger.Error("Joined incorrect lobby?");
          return new NetErrorInfo?(new NetErrorInfo(NetError.InternalError, false));
        }
        EChatRoomEnterResponse roomEnterResponse = (EChatRoomEnterResponse) (int) task1.m_EChatRoomEnterResponse;
        if (roomEnterResponse != 1)
        {
          this._logger.Error($"Failed to enter lobby, response: {roomEnterResponse}");
          return new NetErrorInfo?(new NetErrorInfo(roomEnterResponse));
        }
        this._lobbyId = new CSteamID?(new CSteamID(task1.m_ulSteamIDLobby));
        CSteamID lobbyOwnerId = SteamMatchmaking.GetLobbyOwner(this._lobbyId.Value);
        SteamNetworkingIdentity netId = lobbyOwnerId.ToNetId();
        // ISSUE: method pointer
        this._netStatusChangedCallback = new Callback<SteamNetConnectionStatusChangedCallback_t>(new Callback<SteamNetConnectionStatusChangedCallback_t>.DispatchDelegate((object) this, __methodptr(OnNetStatusChanged)), false);
        this._connectingTaskCompletionSource = new TaskCompletionSource<SteamClient.ConnectionResult>();
        this._conn = new HSteamNetConnection?(SteamNetworkingSockets.ConnectP2P(ref netId, 0, 0, (SteamNetworkingConfigValue_t[]) null));
        this._logger.Debug($"Connecting to user {lobbyOwnerId.m_SteamID}");
        SteamClient.ConnectionResult task2 = await this._connectingTaskCompletionSource.Task;
        if (task2.disconnectionReason.HasValue)
        {
          this.CleanupConnection((int) task2.disconnectionReason.Value, task2.debugReason);
          return new NetErrorInfo?(new NetErrorInfo(task2.disconnectionReason.Value, task2.debugReason, false));
        }
        if ((int) this._conn.Value.m_HSteamNetConnection != (int) task2.connection.Value.m_HSteamNetConnection)
        {
          this._logger.Error("Got different connection back from OnNetStatusChanged than we expected!");
          SteamNetworkingSockets.CloseConnection(task2.connection.Value, 0, (string) null, false);
          this.CleanupConnection(1017, "Invalid OnNetStatusChanged hConn");
          return new NetErrorInfo?(new NetErrorInfo(NetError.InternalError, false));
        }
        this._connectingTaskCompletionSource = (TaskCompletionSource<SteamClient.ConnectionResult>) null;
        this._isConnected = true;
        this._hostNetId = lobbyOwnerId;
        this._handler.OnConnectedToHost();
        this._logger.Debug($"Successfully connected to host {lobbyOwnerId.m_SteamID}");
        return new NetErrorInfo?();
      }
      finally
      {
        callResult?.Dispose();
      }
    }
    callResult = (SteamCallResult<LobbyEnter_t>) null;
    NetErrorInfo? lobby;
    return lobby;
  }

  private void OnNetStatusChanged(SteamNetConnectionStatusChangedCallback_t data)
  {
    this._logger.Debug($"Connection status changed: {data.m_eOldState} -> {data.m_info.m_eState}");
    if (data.m_info.m_eState == 3)
    {
      this._logger.Debug("Steam connection accepted.");
      if (this._connectingTaskCompletionSource == null)
      {
        this._logger.Error("Connection was accepted while we were not waiting for it!");
        this.DisconnectFromHostInternal(SteamDisconnectionReason.InternalError, "Not Connecting", true, false);
      }
      else
        this._connectingTaskCompletionSource.SetResult(new SteamClient.ConnectionResult()
        {
          connection = new HSteamNetConnection?(data.m_hConn)
        });
    }
    else if (data.m_info.m_eState == 5)
    {
      this._logger.Info($"Steam connection closed because of problem. Reason: {data.m_info.m_eEndReason}, {((SteamNetConnectionInfo_t) ref data.m_info).m_szEndDebug}");
      this.HandleDisconnection((SteamDisconnectionReason) data.m_info.m_eEndReason, ((SteamNetConnectionInfo_t) ref data.m_info).m_szEndDebug);
    }
    else
    {
      if (data.m_info.m_eState != 4)
        return;
      this._logger.Info($"Steam connection closed by host. Reason: {data.m_info.m_eEndReason}, {((SteamNetConnectionInfo_t) ref data.m_info).m_szEndDebug}");
      this.HandleDisconnection((SteamDisconnectionReason) data.m_info.m_eEndReason, ((SteamNetConnectionInfo_t) ref data.m_info).m_szEndDebug);
    }
  }

  private void HandleDisconnection(SteamDisconnectionReason reason, string debugReason)
  {
    if (this._connectingTaskCompletionSource != null)
      this._connectingTaskCompletionSource.SetResult(new SteamClient.ConnectionResult()
      {
        disconnectionReason = new SteamDisconnectionReason?(reason),
        debugReason = debugReason
      });
    else
      this.DisconnectFromHostInternal(reason, debugReason, true, false);
  }

  public override void Update()
  {
    SteamUtil.ProcessMessages(this._conn.Value, (INetHandler) this._handler, this._logger);
  }

  public override void SendMessageToHost(
    byte[] bytes,
    int length,
    NetTransferMode mode,
    int channel = 0)
  {
    this._logger.VeryDebug($"Sending {length} bytes to host");
    GCHandle gcHandle = GCHandle.Alloc((object) bytes, (GCHandleType) 3);
    try
    {
      long num;
      EResult connection = SteamNetworkingSockets.SendMessageToConnection(this._conn.Value, gcHandle.AddrOfPinnedObject(), (uint) length, SteamUtil.FlagsFromMode(mode), ref num);
      if (connection == 1)
        return;
      this._logger.Warn($"Failed to send message length {length}: {connection}");
    }
    finally
    {
      gcHandle.Free();
    }
  }

  public override void DisconnectFromHost(NetError reason, bool now = false)
  {
    this.DisconnectFromHostInternal(reason.ToSteam(), string.Empty, now, true);
  }

  private void CleanupLobby()
  {
    SteamMatchmaking.LeaveLobby(this._lobbyId.Value);
    this._lobbyId = new CSteamID?();
    this._netStatusChangedCallback?.Dispose();
    this._netStatusChangedCallback = (Callback<SteamNetConnectionStatusChangedCallback_t>) null;
  }

  private void CleanupConnection(int closeReason, string? debugReason)
  {
    SteamNetworkingSockets.CloseConnection(this._conn.Value, closeReason, debugReason, false);
    this._conn = new HSteamNetConnection?();
    this._connectingTaskCompletionSource = (TaskCompletionSource<SteamClient.ConnectionResult>) null;
    this.CleanupLobby();
  }

  private void DisconnectFromHostInternal(
    SteamDisconnectionReason reason,
    string? debugReason,
    bool now,
    bool selfInitiated)
  {
    this._logger.Debug($"Disconnecting from host (now: {now} reason: {reason} debug: {debugReason})");
    SteamNetworkingSockets.CloseConnection(this._conn.Value, (int) reason, debugReason, !now);
    SteamMatchmaking.LeaveLobby(this._lobbyId.Value);
    ulong steamId = this._hostNetId.m_SteamID;
    this._conn = new HSteamNetConnection?();
    this._lobbyId = new CSteamID?();
    this._connectingTaskCompletionSource = (TaskCompletionSource<SteamClient.ConnectionResult>) null;
    this._netStatusChangedCallback?.Dispose();
    this._netStatusChangedCallback = (Callback<SteamNetConnectionStatusChangedCallback_t>) null;
    this._isConnected = false;
    this._hostNetId = CSteamID.Nil;
    this._handler.OnDisconnectedFromHost(steamId, new NetErrorInfo(reason, debugReason, selfInitiated));
  }

  private void CancelConnection()
  {
    this._connectingTaskCompletionSource?.SetCanceled();
    if (this._conn.HasValue)
    {
      this.DisconnectFromHost(NetError.Quit, false);
    }
    else
    {
      if (!this._lobbyId.HasValue)
        return;
      this.CleanupLobby();
    }
  }

  public override string? GetRawLobbyIdentifier()
  {
    ref CSteamID? local = ref this._lobbyId;
    return !local.HasValue ? (string) null : local.GetValueOrDefault().m_SteamID.ToString();
  }

  private struct ConnectionResult
  {
    public HSteamNetConnection? connection;
    public SteamDisconnectionReason? disconnectionReason;
    public string? debugReason;
  }
}
