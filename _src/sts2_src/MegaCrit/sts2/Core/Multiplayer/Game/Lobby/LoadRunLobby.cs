// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Game.Lobby.LoadRunLobby
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Game.Lobby;

public class LoadRunLobby
{
  private readonly Logger _logger;
  private readonly List<LoadRunLobby.ConnectingPlayer> _connectingPlayers = new List<LoadRunLobby.ConnectingPlayer>();
  private bool _isBeginningRun;
  private readonly HashSet<ulong> _readyPlayers = new HashSet<ulong>();

  public INetGameService NetService { get; }

  public ILoadRunLobbyListener LobbyListener { get; }

  public PeerInputSynchronizer InputSynchronizer { get; }

  public SerializableRun Run { get; }

  public HashSet<ulong> ConnectedPlayerIds { get; } = new HashSet<ulong>();

  public GameMode GameMode => this.Run.GameMode;

  public int HandshakeTimeout { get; set; } = 10000;

  public LoadRunLobby(
    INetGameService netService,
    ILoadRunLobbyListener lobbyListener,
    SerializableRun runSave)
  {
    this.Run = runSave;
    this.NetService = netService;
    this.LobbyListener = lobbyListener;
    this.InputSynchronizer = new PeerInputSynchronizer(this.NetService);
    this._logger = new Logger(nameof (LoadRunLobby), LogType.Network);
    this.NetService.RegisterMessageHandler<ClientLoadJoinRequestMessage>(new MessageHandlerDelegate<ClientLoadJoinRequestMessage>(this.HandleClientLoadJoinRequestMessage));
    this.NetService.RegisterMessageHandler<ClientLobbyJoinRequestMessage>(new MessageHandlerDelegate<ClientLobbyJoinRequestMessage>(this.HandleClientLobbyJoinRequestMessage));
    this.NetService.RegisterMessageHandler<ClientRejoinRequestMessage>(new MessageHandlerDelegate<ClientRejoinRequestMessage>(this.HandleClientRejoinRequestMessage));
    this.NetService.RegisterMessageHandler<PlayerReconnectedMessage>(new MessageHandlerDelegate<PlayerReconnectedMessage>(this.HandlePlayerReconnectedMessage));
    this.NetService.RegisterMessageHandler<PlayerLeftMessage>(new MessageHandlerDelegate<PlayerLeftMessage>(this.HandlePlayerLeftMessage));
    this.NetService.RegisterMessageHandler<LobbyPlayerSetReadyMessage>(new MessageHandlerDelegate<LobbyPlayerSetReadyMessage>(this.HandlePlayerReadyMessage));
    this.NetService.RegisterMessageHandler<LobbyBeginLoadedRunMessage>(new MessageHandlerDelegate<LobbyBeginLoadedRunMessage>(this.HandleLobbyBeginRunMessage));
    this.NetService.Disconnected += new Action<NetErrorInfo>(this.OnDisconnected);
    if (this.NetService.Type != NetGameType.Host)
      return;
    INetHostGameService netHostGameService = (INetHostGameService) netService;
    netHostGameService.ClientConnected += new Action<ulong>(this.OnConnectedToClientAsHost);
    netHostGameService.ClientDisconnected += new Action<ulong, NetErrorInfo>(this.OnDisconnectedFromClientAsHost);
  }

  public LoadRunLobby(
    INetGameService netService,
    ILoadRunLobbyListener lobbyListener,
    ClientLoadJoinResponseMessage message)
    : this(netService, lobbyListener, message.serializableRun)
  {
    foreach (ulong num in message.playersAlreadyConnected)
      this.ConnectedPlayerIds.Add(num);
  }

  public void CleanUp(bool disconnectSession, NetError error = NetError.Quit)
  {
    this.NetService.UnregisterMessageHandler<ClientLoadJoinRequestMessage>(new MessageHandlerDelegate<ClientLoadJoinRequestMessage>(this.HandleClientLoadJoinRequestMessage));
    this.NetService.UnregisterMessageHandler<ClientLobbyJoinRequestMessage>(new MessageHandlerDelegate<ClientLobbyJoinRequestMessage>(this.HandleClientLobbyJoinRequestMessage));
    this.NetService.UnregisterMessageHandler<ClientRejoinRequestMessage>(new MessageHandlerDelegate<ClientRejoinRequestMessage>(this.HandleClientRejoinRequestMessage));
    this.NetService.UnregisterMessageHandler<PlayerReconnectedMessage>(new MessageHandlerDelegate<PlayerReconnectedMessage>(this.HandlePlayerReconnectedMessage));
    this.NetService.UnregisterMessageHandler<PlayerLeftMessage>(new MessageHandlerDelegate<PlayerLeftMessage>(this.HandlePlayerLeftMessage));
    this.NetService.UnregisterMessageHandler<LobbyPlayerSetReadyMessage>(new MessageHandlerDelegate<LobbyPlayerSetReadyMessage>(this.HandlePlayerReadyMessage));
    this.NetService.UnregisterMessageHandler<LobbyBeginLoadedRunMessage>(new MessageHandlerDelegate<LobbyBeginLoadedRunMessage>(this.HandleLobbyBeginRunMessage));
    if (disconnectSession)
    {
      if (this.NetService.IsConnected)
        this.NetService.Disconnect(error);
      this.InputSynchronizer.Dispose();
    }
    this.NetService.Disconnected -= new Action<NetErrorInfo>(this.OnDisconnected);
    if (this.NetService.Type != NetGameType.Host)
      return;
    INetHostGameService netService = (INetHostGameService) this.NetService;
    netService.ClientConnected -= new Action<ulong>(this.OnConnectedToClientAsHost);
    netService.ClientDisconnected -= new Action<ulong, NetErrorInfo>(this.OnDisconnectedFromClientAsHost);
  }

  public void AddLocalHostPlayer()
  {
    if (this.NetService.Type == NetGameType.Client)
      throw new InvalidOperationException("Tried to add local host player as client!");
    this._logger.Context = $"Lobby ({this.NetService.NetId})";
    this.ConnectedPlayerIds.Add(this.NetService.NetId);
    this.LobbyListener.PlayerConnected(this.NetService.NetId);
  }

  private void HandleClientLoadJoinRequestMessage(
    ClientLoadJoinRequestMessage message,
    ulong senderId)
  {
    INetHostGameService netHostGameService = this.NetService.Type == NetGameType.Host ? (INetHostGameService) this.NetService : throw new InvalidOperationException("Received ClientLoadJoinRequestMessage as non-host!");
    try
    {
      if (this.Run.Players.FindIndex((Predicate<SerializablePlayer>) (p => (long) p.NetId == (long) senderId)) < 0)
      {
        this._logger.Warn($"Client {senderId} sent ClientLoadJoinRequestMessage but they are not in the loaded run!");
        netHostGameService.DisconnectClient(senderId, NetError.NotInSaveGame);
      }
      else
      {
        this._logger.Info($"Received ClientLoadJoinRequestMessage for {senderId}");
        this.ConnectedPlayerIds.Add(senderId);
        this.LobbyListener.PlayerConnected(senderId);
        ClientLoadJoinResponseMessage message1 = new ClientLoadJoinResponseMessage();
        message1.serializableRun = this.Run;
        message1.playersAlreadyConnected = this.ConnectedPlayerIds.ToList<ulong>();
        this._logger.Debug($"Sending ClientLoadJoinResponseMessage to {senderId}");
        netHostGameService.SendMessage<ClientLoadJoinResponseMessage>(message1, senderId);
        netHostGameService.SetPeerReadyForBroadcasting(senderId);
        PlayerReconnectedMessage message2 = new PlayerReconnectedMessage();
        message2.playerId = senderId;
        foreach (ulong connectedPlayerId in this.ConnectedPlayerIds)
        {
          if ((long) connectedPlayerId != (long) senderId && (long) connectedPlayerId != (long) this.NetService.NetId)
          {
            this._logger.Debug($"Sending PlayerReconnectedMessage to {connectedPlayerId}");
            netHostGameService.SendMessage<PlayerReconnectedMessage>(message2, connectedPlayerId);
          }
        }
        this.RemoveConnectingPlayer(senderId);
      }
    }
    catch
    {
      netHostGameService.DisconnectClient(senderId, NetError.InternalError);
      throw;
    }
  }

  private void HandleClientLobbyJoinRequestMessage(ClientLobbyJoinRequestMessage _, ulong senderId)
  {
    if (this.NetService.Type != NetGameType.Host)
      throw new InvalidOperationException("Received ClientLobbyJoinRequestMessage as non-host!");
    this._logger.Info($"Received invalid ClientLobbyJoinRequestMessage for {senderId}");
    ((INetHostGameService) this.NetService).DisconnectClient(senderId, NetError.InvalidJoin);
  }

  private void HandleClientRejoinRequestMessage(ClientRejoinRequestMessage _, ulong senderId)
  {
    if (this.NetService.Type != NetGameType.Host)
      throw new InvalidOperationException("Received ClientRejoinRequestMessage as non-host!");
    this._logger.Info($"Received invalid ClientRejoinRequestMessage for {senderId}");
    ((INetHostGameService) this.NetService).DisconnectClient(senderId, NetError.InvalidJoin);
  }

  private void HandlePlayerReconnectedMessage(PlayerReconnectedMessage message, ulong _)
  {
    this._logger.Debug($"Received PlayerReconnectedMessage with player ID {message.playerId}");
    this.ConnectedPlayerIds.Add(message.playerId);
    this.LobbyListener.PlayerConnected(message.playerId);
  }

  private void HandlePlayerLeftMessage(PlayerLeftMessage message, ulong senderId)
  {
    this._logger.Debug($"Received PlayerLeftMessage for {message.playerId}");
    if (!this.ConnectedPlayerIds.Contains(message.playerId))
      return;
    this.ConnectedPlayerIds.Remove(message.playerId);
    this.InputSynchronizer.OnPlayerDisconnected(message.playerId);
    this.LobbyListener.RemotePlayerDisconnected(message.playerId);
  }

  private void HandlePlayerReadyMessage(LobbyPlayerSetReadyMessage message, ulong senderId)
  {
    this._logger.Debug($"Received {"LobbyPlayerSetReadyMessage"} for player {senderId} with value {message.ready}");
    if (message.ready)
    {
      if (this._readyPlayers.Add(senderId))
        this.LobbyListener.PlayerReadyChanged(senderId);
    }
    else if (this._readyPlayers.Remove(senderId))
      this.LobbyListener.PlayerReadyChanged(senderId);
    this.BeginRunForAllPlayersIfAllReady();
  }

  private void HandleLobbyBeginRunMessage(LobbyBeginLoadedRunMessage message, ulong senderId)
  {
    this._logger.Debug("Received LobbyBeginLoadedRunMessage");
    this._isBeginningRun = true;
    this.BeginRunLocally();
  }

  private async Task TryBeginRunForAllPlayers()
  {
    if (this.NetService.Type == NetGameType.Client)
      throw new InvalidOperationException("Can only begin run for all peers as host!");
    if (this._isBeginningRun)
    {
      this._logger.Warn("Tried to begin run twice, ignoring second one!");
    }
    else
    {
      this._isBeginningRun = true;
      if (!await this.LobbyListener.ShouldAllowRunToBegin())
      {
        this.SetReady(false);
        this._isBeginningRun = false;
      }
      else
      {
        this.NetService.SendMessage<LobbyBeginLoadedRunMessage>(new LobbyBeginLoadedRunMessage());
        this.BeginRunLocally();
        if (this.NetService.Type != NetGameType.Host)
          return;
        ((INetHostGameService) this.NetService).NetHost?.SetHostIsClosed(true);
      }
    }
  }

  private void BeginRunLocally()
  {
    this.NetService.SetBufferMessages(true);
    this.LobbyListener.BeginRun();
  }

  public void SetReady(bool ready)
  {
    if (ready)
      this._readyPlayers.Add(this.NetService.NetId);
    else
      this._readyPlayers.Remove(this.NetService.NetId);
    this.NetService.SendMessage<LobbyPlayerSetReadyMessage>(new LobbyPlayerSetReadyMessage()
    {
      ready = ready
    });
    this.LobbyListener.PlayerReadyChanged(this.NetService.NetId);
    this._logger.Info($"Local player {this.NetService.NetId} is ready");
    this.BeginRunForAllPlayersIfAllReady();
  }

  public bool IsPlayerReady(ulong playerId) => this._readyPlayers.Contains(playerId);

  public bool IsAboutToBeginGame()
  {
    return this._connectingPlayers.Count <= 0 && (!this.NetService.Type.IsMultiplayer() || this.ConnectedPlayerIds.Count != 1) && !this.ConnectedPlayerIds.Except<ulong>((IEnumerable<ulong>) this._readyPlayers).Any<ulong>();
  }

  private void BeginRunForAllPlayersIfAllReady()
  {
    if (this.NetService.Type != NetGameType.Host && this.NetService.Type != NetGameType.Singleplayer || !this.IsAboutToBeginGame())
      return;
    TaskHelper.RunSafely(this.TryBeginRunForAllPlayers());
  }

  private void OnConnectedToClientAsHost(ulong playerId)
  {
    this._logger.Info($"Client {playerId} connected. Sending initial game info message");
    InitialGameInfoMessage message = InitialGameInfoMessage.Basic() with
    {
      sessionState = RunSessionState.InLoadedLobby,
      gameMode = this.GameMode
    };
    if (this._isBeginningRun)
    {
      message.connectionFailureReason = new ConnectionFailureReason?(ConnectionFailureReason.RunInProgress);
      this.NetService.SendMessage<InitialGameInfoMessage>(message, playerId);
      this._logger.Warn($"Client {playerId} connected but we are already beginning the run!");
      ((INetHostGameService) this.NetService).DisconnectClient(playerId, NetError.RunInProgress);
    }
    else if (this.Run.Players.FindIndex((Predicate<SerializablePlayer>) (p => (long) p.NetId == (long) playerId)) < 0)
    {
      message.connectionFailureReason = new ConnectionFailureReason?(ConnectionFailureReason.NotInSaveGame);
      this.NetService.SendMessage<InitialGameInfoMessage>(message, playerId);
      this._logger.Warn($"Client {playerId} connected but they were not in the loaded game!");
      ((INetHostGameService) this.NetService).DisconnectClient(playerId, NetError.NotInSaveGame);
    }
    else
    {
      LoadRunLobby.ConnectingPlayer connectingPlayer = new LoadRunLobby.ConnectingPlayer()
      {
        id = playerId,
        timeoutCancelToken = new CancellationTokenSource()
      };
      this._connectingPlayers.Add(connectingPlayer);
      this.NetService.SendMessage<InitialGameInfoMessage>(message, playerId);
      TaskHelper.RunSafely(this.BeginHandshakeTimeout(connectingPlayer));
    }
  }

  private async Task BeginHandshakeTimeout(LoadRunLobby.ConnectingPlayer connectingPlayer)
  {
    await Task.Delay(this.HandshakeTimeout, connectingPlayer.timeoutCancelToken.Token);
    if (connectingPlayer.timeoutCancelToken.IsCancellationRequested || this._connectingPlayers.IndexOf(connectingPlayer) < 0)
      return;
    this._logger.Info($"Disconnecting player {connectingPlayer.id} because they did not respond to the initial game join handshake within {this.HandshakeTimeout}ms");
    ((INetHostGameService) this.NetService).DisconnectClient(connectingPlayer.id, NetError.HandshakeTimeout);
  }

  private void OnDisconnectedFromClientAsHost(ulong playerId, NetErrorInfo info)
  {
    if (!this.ConnectedPlayerIds.Contains(playerId))
      return;
    this._logger.Info($"Client {playerId} disconnected, reason: {info.GetReason()}");
    PlayerLeftMessage message = new PlayerLeftMessage()
    {
      playerId = playerId
    };
    this.NetService.SendMessage<PlayerLeftMessage>(message);
    this.ConnectedPlayerIds.Remove(playerId);
    this._readyPlayers.Remove(playerId);
    this.RemoveConnectingPlayer(playerId);
    this.InputSynchronizer.OnPlayerDisconnected(message.playerId);
    this.LobbyListener.RemotePlayerDisconnected(playerId);
    this.BeginRunForAllPlayersIfAllReady();
  }

  private void RemoveConnectingPlayer(ulong playerId)
  {
    for (int index = 0; index < this._connectingPlayers.Count; ++index)
    {
      if ((long) this._connectingPlayers[index].id == (long) playerId)
      {
        this._connectingPlayers[index].timeoutCancelToken.Cancel();
        this._connectingPlayers.RemoveAt(index);
        --index;
      }
    }
  }

  private void OnDisconnected(NetErrorInfo info)
  {
    this._logger.Info($"Disconnected from host, reason: {info.GetReason()}");
    this.ConnectedPlayerIds.Clear();
    this.LobbyListener.LocalPlayerDisconnected(info);
  }

  private struct ConnectingPlayer : IEquatable<LoadRunLobby.ConnectingPlayer>
  {
    public ulong id;
    public CancellationTokenSource timeoutCancelToken;

    public bool Equals(LoadRunLobby.ConnectingPlayer other)
    {
      return (long) this.id == (long) other.id && this.timeoutCancelToken.Equals((object) other.timeoutCancelToken);
    }

    public override bool Equals(object? obj)
    {
      return obj is LoadRunLobby.ConnectingPlayer other && this.Equals(other);
    }

    public override int GetHashCode()
    {
      return HashCode.Combine<ulong, CancellationTokenSource>(this.id, this.timeoutCancelToken);
    }
  }
}
