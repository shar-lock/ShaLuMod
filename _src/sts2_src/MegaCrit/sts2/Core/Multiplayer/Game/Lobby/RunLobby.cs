// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Game.Lobby.RunLobby
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Game;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Game.Lobby;

public class RunLobby
{
  private readonly Logger _logger;
  private readonly IPlayerCollection _playerCollection;
  private readonly IRunLobbyListener _lobbyListener;
  private readonly INetGameService _netService;
  private readonly HashSet<ulong> _connectedPlayerIds = new HashSet<ulong>();

  public IReadOnlyCollection<ulong> ConnectedPlayerIds
  {
    get => (IReadOnlyCollection<ulong>) this._connectedPlayerIds;
  }

  public GameMode GameMode { get; }

  public event Action<ulong>? PlayerRejoined;

  public event Action<ulong>? RemotePlayerDisconnected;

  public event Action? LocalPlayerDisconnected;

  public RunLobby(
    GameMode gameMode,
    INetGameService netService,
    IRunLobbyListener lobbyListener,
    IPlayerCollection playerCollection,
    IEnumerable<ulong> connectedPlayerIds)
  {
    this.GameMode = gameMode;
    this._netService = netService;
    this._lobbyListener = lobbyListener;
    this._playerCollection = playerCollection;
    this._logger = new Logger(nameof (RunLobby), LogType.Network);
    this._netService.RegisterMessageHandler<ClientLobbyJoinRequestMessage>(new MessageHandlerDelegate<ClientLobbyJoinRequestMessage>(this.HandleClientLobbyJoinRequestMessage));
    this._netService.RegisterMessageHandler<ClientLoadJoinRequestMessage>(new MessageHandlerDelegate<ClientLoadJoinRequestMessage>(this.HandleClientLoadJoinRequestMessage));
    this._netService.RegisterMessageHandler<ClientRejoinRequestMessage>(new MessageHandlerDelegate<ClientRejoinRequestMessage>(this.HandleClientRejoinRequestMessage));
    this._netService.RegisterMessageHandler<PlayerRejoinedMessage>(new MessageHandlerDelegate<PlayerRejoinedMessage>(this.HandlePlayerRejoinedMessage));
    this._netService.RegisterMessageHandler<PlayerLeftMessage>(new MessageHandlerDelegate<PlayerLeftMessage>(this.HandlePlayerLeftMessage));
    this._netService.RegisterMessageHandler<RunAbandonedMessage>(new MessageHandlerDelegate<RunAbandonedMessage>(this.HandleRunAbandonedMessage));
    foreach (ulong connectedPlayerId in connectedPlayerIds)
      this._connectedPlayerIds.Add(connectedPlayerId);
    this._netService.Disconnected += new Action<NetErrorInfo>(this.OnDisconnected);
    if (this._netService.Type != NetGameType.Host || !(netService is INetHostGameService netHostGameService))
      return;
    netHostGameService.ClientConnected += new Action<ulong>(this.OnConnectedToClientAsHost);
    netHostGameService.ClientDisconnected += new Action<ulong, NetErrorInfo>(this.OnDisconnectedFromClientAsHost);
  }

  public void Dispose()
  {
    this._netService.UnregisterMessageHandler<ClientLobbyJoinRequestMessage>(new MessageHandlerDelegate<ClientLobbyJoinRequestMessage>(this.HandleClientLobbyJoinRequestMessage));
    this._netService.UnregisterMessageHandler<ClientLoadJoinRequestMessage>(new MessageHandlerDelegate<ClientLoadJoinRequestMessage>(this.HandleClientLoadJoinRequestMessage));
    this._netService.UnregisterMessageHandler<ClientRejoinRequestMessage>(new MessageHandlerDelegate<ClientRejoinRequestMessage>(this.HandleClientRejoinRequestMessage));
    this._netService.UnregisterMessageHandler<PlayerRejoinedMessage>(new MessageHandlerDelegate<PlayerRejoinedMessage>(this.HandlePlayerRejoinedMessage));
    this._netService.UnregisterMessageHandler<PlayerLeftMessage>(new MessageHandlerDelegate<PlayerLeftMessage>(this.HandlePlayerLeftMessage));
    this._netService.UnregisterMessageHandler<RunAbandonedMessage>(new MessageHandlerDelegate<RunAbandonedMessage>(this.HandleRunAbandonedMessage));
    this._netService.Disconnected -= new Action<NetErrorInfo>(this.OnDisconnected);
    if (this._netService.Type != NetGameType.Host || !(this._netService is INetHostGameService netService))
      return;
    netService.ClientConnected -= new Action<ulong>(this.OnConnectedToClientAsHost);
    netService.ClientDisconnected -= new Action<ulong, NetErrorInfo>(this.OnDisconnectedFromClientAsHost);
  }

  private void HandleClientRejoinRequestMessage(ClientRejoinRequestMessage message, ulong senderId)
  {
    if (this._netService.Type != NetGameType.Host)
      throw new InvalidOperationException("Received ClientRejoinRequestMessage as non-host!");
    this._logger.Info($"Received ClientRejoinRequestMessage for {senderId}");
    NetHostGameService netService = (NetHostGameService) this._netService;
    if (this._playerCollection.GetPlayer(senderId) == null)
    {
      netService.DisconnectClient(senderId, NetError.RunInProgress, false);
    }
    else
    {
      ClientRejoinResponseMessage rejoinMessage = this._lobbyListener.GetRejoinMessage();
      netService.SendMessage<ClientRejoinResponseMessage>(rejoinMessage, senderId);
      netService.SetPeerReadyForBroadcasting(senderId);
      PlayerRejoinedMessage message1 = new PlayerRejoinedMessage()
      {
        playerId = senderId
      };
      foreach (NetClientData connectedPeer in (IEnumerable<NetClientData>) netService.ConnectedPeers)
      {
        if (connectedPeer.readyForBroadcasting && (long) connectedPeer.peerId != (long) senderId)
          netService.SendMessage<PlayerRejoinedMessage>(message1, connectedPeer.peerId);
      }
      this._connectedPlayerIds.Add(senderId);
      Action<ulong> playerRejoined = this.PlayerRejoined;
      if (playerRejoined == null)
        return;
      playerRejoined(senderId);
    }
  }

  private void HandleClientLobbyJoinRequestMessage(ClientLobbyJoinRequestMessage _, ulong senderId)
  {
    if (this._netService.Type != NetGameType.Host)
      throw new InvalidOperationException("Received ClientLobbyJoinRequestMessage as non-host!");
    this._logger.Info($"Received invalid ClientLobbyJoinRequestMessage for {senderId}");
    ((NetHostGameService) this._netService).DisconnectClient(senderId, NetError.InvalidJoin, false);
  }

  private void HandleClientLoadJoinRequestMessage(ClientLoadJoinRequestMessage _, ulong senderId)
  {
    if (this._netService.Type != NetGameType.Host)
      throw new InvalidOperationException("Received ClientLoadJoinRequestMessage as non-host!");
    this._logger.Info($"Received invalid ClientLoadJoinRequestMessage for {senderId}");
    ((NetHostGameService) this._netService).DisconnectClient(senderId, NetError.InvalidJoin, false);
  }

  private void HandlePlayerRejoinedMessage(PlayerRejoinedMessage message, ulong _)
  {
    this._logger.Debug($"Received PlayerRejoinedMessage for {message.playerId}");
    this._connectedPlayerIds.Add(message.playerId);
    Action<ulong> playerRejoined = this.PlayerRejoined;
    if (playerRejoined == null)
      return;
    playerRejoined(message.playerId);
  }

  private void HandlePlayerLeftMessage(PlayerLeftMessage message, ulong _)
  {
    this._logger.Debug($"Received PlayerLeftMessage for {message.playerId}");
    this._connectedPlayerIds.Remove(message.playerId);
    Action<ulong> playerDisconnected = this.RemotePlayerDisconnected;
    if (playerDisconnected == null)
      return;
    playerDisconnected(message.playerId);
  }

  public void AbandonRun()
  {
    if (this._netService.Type != NetGameType.Host)
      throw new InvalidOperationException("Abandon run can only be called as host!");
    this._logger.Info("Abandoning run as host");
    ((NetHostGameService) this._netService).SendMessage<RunAbandonedMessage>(new RunAbandonedMessage());
    this._lobbyListener.RunAbandoned();
    this._netService.Disconnect(NetError.HostAbandoned);
  }

  private void HandleRunAbandonedMessage(RunAbandonedMessage message, ulong _)
  {
    this._logger.Debug("Received RunAbandonedMessage");
    this._lobbyListener.RunAbandoned();
    this._netService.Disconnect(NetError.HostAbandoned);
  }

  private void OnConnectedToClientAsHost(ulong playerId)
  {
    this._logger.Info($"Player {playerId} connected to host.");
    InitialGameInfoMessage message = InitialGameInfoMessage.Basic() with
    {
      sessionState = RunSessionState.Running,
      gameMode = this.GameMode
    };
    if (this._playerCollection.GetPlayer(playerId) == null)
    {
      this._logger.Warn($"Client {playerId} connected but they are not in the run!");
      message.connectionFailureReason = new ConnectionFailureReason?(ConnectionFailureReason.RunInProgress);
      this._netService.SendMessage<InitialGameInfoMessage>(message, playerId);
      ((NetHostGameService) this._netService).DisconnectClient(playerId, NetError.RunInProgress, false);
    }
    else
      this._netService.SendMessage<InitialGameInfoMessage>(message, playerId);
  }

  private void OnDisconnectedFromClientAsHost(ulong playerId, NetErrorInfo info)
  {
    this._logger.Info($"Player {playerId} disconnected from host. Is in connected players: {this._connectedPlayerIds.Contains(playerId)}. Reason: {info.GetReason()}");
    if (!this._connectedPlayerIds.Contains(playerId))
      return;
    this._netService.SendMessage<PlayerLeftMessage>(new PlayerLeftMessage()
    {
      playerId = playerId
    });
    this._connectedPlayerIds.Remove(playerId);
    Action<ulong> playerDisconnected = this.RemotePlayerDisconnected;
    if (playerDisconnected == null)
      return;
    playerDisconnected(playerId);
  }

  private void OnDisconnected(NetErrorInfo info)
  {
    this._logger.Info($"Disconnected. Reason: {info.GetReason()}");
    this._connectedPlayerIds.Clear();
    this._lobbyListener.LocalPlayerDisconnected(info);
    Action playerDisconnected = this.LocalPlayerDisconnected;
    if (playerDisconnected == null)
      return;
    playerDisconnected();
  }
}
