// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Game.Lobby.StartRunLobby
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Daily;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Acts;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Ftue;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.Timeline.Epochs;
using MegaCrit.Sts2.Core.Unlocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Game.Lobby;

public class StartRunLobby
{
  private readonly Logger _logger;
  private readonly List<StartRunLobby.ConnectingPlayer> _connectingPlayers = new List<StartRunLobby.ConnectingPlayer>();
  private bool _isBeginningRun;
  private readonly List<ModifierModel> _modifiers = new List<ModifierModel>();

  public INetGameService NetService { get; }

  public IStartRunLobbyListener LobbyListener { get; }

  public PeerInputSynchronizer InputSynchronizer { get; }

  public int MaxPlayers { get; private set; }

  public int Ascension { get; private set; }

  public int MaxAscension { get; private set; }

  public string? Seed { get; private set; }

  public TimeServerResult? DailyTime { get; private set; }

  public GameMode GameMode { get; private set; }

  public IReadOnlyList<ModifierModel> Modifiers => (IReadOnlyList<ModifierModel>) this._modifiers;

  public int HandshakeTimeout { get; set; } = 10000;

  public string Act1 { get; set; } = "random";

  public List<LobbyPlayer> Players { get; } = new List<LobbyPlayer>();

  public LobbyPlayer LocalPlayer
  {
    get
    {
      return this.Players.Find((Predicate<LobbyPlayer>) (p => (long) p.id == (long) this.NetService.NetId));
    }
  }

  public event Action<LobbyPlayer>? PlayerConnected;

  public event Action<LobbyPlayer>? PlayerDisconnected;

  public StartRunLobby(
    GameMode gameMode,
    INetGameService netService,
    IStartRunLobbyListener lobbyListener,
    int maxPlayers)
  {
    this.GameMode = gameMode;
    this.NetService = netService;
    this.LobbyListener = lobbyListener;
    this.MaxPlayers = maxPlayers;
    this.InputSynchronizer = new PeerInputSynchronizer(netService);
    this._logger = new Logger(nameof (StartRunLobby), LogType.Network);
    this.NetService.RegisterMessageHandler<ClientLobbyJoinRequestMessage>(new MessageHandlerDelegate<ClientLobbyJoinRequestMessage>(this.HandleClientLobbyJoinRequestMessage));
    this.NetService.RegisterMessageHandler<ClientLoadJoinRequestMessage>(new MessageHandlerDelegate<ClientLoadJoinRequestMessage>(this.HandleClientLoadJoinRequestMessage));
    this.NetService.RegisterMessageHandler<ClientRejoinRequestMessage>(new MessageHandlerDelegate<ClientRejoinRequestMessage>(this.HandleClientRejoinRequestMessage));
    this.NetService.RegisterMessageHandler<PlayerJoinedMessage>(new MessageHandlerDelegate<PlayerJoinedMessage>(this.HandlePlayerJoinedMessage));
    this.NetService.RegisterMessageHandler<PlayerLeftMessage>(new MessageHandlerDelegate<PlayerLeftMessage>(this.HandlePlayerLeftMessage));
    this.NetService.RegisterMessageHandler<LobbyPlayerChangedCharacterMessage>(new MessageHandlerDelegate<LobbyPlayerChangedCharacterMessage>(this.HandleLobbyPlayerChangedCharacterMessage));
    this.NetService.RegisterMessageHandler<LobbyAscensionChangedMessage>(new MessageHandlerDelegate<LobbyAscensionChangedMessage>(this.HandleAscensionChangedMessage));
    this.NetService.RegisterMessageHandler<LobbySeedChangedMessage>(new MessageHandlerDelegate<LobbySeedChangedMessage>(this.HandleSeedChangedMessage));
    this.NetService.RegisterMessageHandler<LobbyModifiersChangedMessage>(new MessageHandlerDelegate<LobbyModifiersChangedMessage>(this.HandleModifiersChangedMessage));
    this.NetService.RegisterMessageHandler<LobbyPlayerSetReadyMessage>(new MessageHandlerDelegate<LobbyPlayerSetReadyMessage>(this.HandlePlayerReadyMessage));
    this.NetService.RegisterMessageHandler<LobbyBeginRunMessage>(new MessageHandlerDelegate<LobbyBeginRunMessage>(this.HandleLobbyBeginRunMessage));
    this.NetService.Disconnected += new Action<NetErrorInfo>(this.OnDisconnected);
    if (this.NetService.Type != NetGameType.Host)
      return;
    INetHostGameService netHostGameService = (INetHostGameService) netService;
    netHostGameService.ClientConnected += new Action<ulong>(this.OnConnectedToClientAsHost);
    netHostGameService.ClientDisconnected += new Action<ulong, NetErrorInfo>(this.OnDisconnectedFromClientAsHost);
    foreach (NetClientData connectedPeer in (IEnumerable<NetClientData>) netHostGameService.ConnectedPeers)
      this.OnConnectedToClientAsHost(connectedPeer.peerId);
  }

  public StartRunLobby(
    GameMode gameMode,
    INetGameService netService,
    IStartRunLobbyListener lobbyListener,
    TimeServerResult timeServerResult,
    int maxPlayers)
    : this(gameMode, netService, lobbyListener, maxPlayers)
  {
    this.DailyTime = new TimeServerResult?(timeServerResult);
  }

  public void InitializeFromMessage(ClientLobbyJoinResponseMessage message)
  {
    foreach (LobbyPlayer lobbyPlayer in message.playersInLobby)
      this.Players.Add(lobbyPlayer);
    this._modifiers.Clear();
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    this._modifiers.AddRange(message.modifiers.Select<SerializableModifier, ModifierModel>(StartRunLobby.\u003C\u003EO.\u003C0\u003E__FromSerializable ?? (StartRunLobby.\u003C\u003EO.\u003C0\u003E__FromSerializable = new Func<SerializableModifier, ModifierModel>(ModifierModel.FromSerializable))));
    this.Ascension = message.ascension;
    this.UpdateMaxMultiplayerAscension();
    this.Seed = message.seed;
    this.LobbyListener.PlayerConnected(this.LocalPlayer);
    Action<LobbyPlayer> playerConnected = this.PlayerConnected;
    if (playerConnected == null)
      return;
    playerConnected(this.LocalPlayer);
  }

  public void CleanUp(bool disconnectSession, NetError error = NetError.Quit)
  {
    this.NetService.UnregisterMessageHandler<ClientLobbyJoinRequestMessage>(new MessageHandlerDelegate<ClientLobbyJoinRequestMessage>(this.HandleClientLobbyJoinRequestMessage));
    this.NetService.UnregisterMessageHandler<ClientLoadJoinRequestMessage>(new MessageHandlerDelegate<ClientLoadJoinRequestMessage>(this.HandleClientLoadJoinRequestMessage));
    this.NetService.UnregisterMessageHandler<ClientRejoinRequestMessage>(new MessageHandlerDelegate<ClientRejoinRequestMessage>(this.HandleClientRejoinRequestMessage));
    this.NetService.UnregisterMessageHandler<PlayerJoinedMessage>(new MessageHandlerDelegate<PlayerJoinedMessage>(this.HandlePlayerJoinedMessage));
    this.NetService.UnregisterMessageHandler<PlayerLeftMessage>(new MessageHandlerDelegate<PlayerLeftMessage>(this.HandlePlayerLeftMessage));
    this.NetService.UnregisterMessageHandler<LobbyPlayerChangedCharacterMessage>(new MessageHandlerDelegate<LobbyPlayerChangedCharacterMessage>(this.HandleLobbyPlayerChangedCharacterMessage));
    this.NetService.UnregisterMessageHandler<LobbyAscensionChangedMessage>(new MessageHandlerDelegate<LobbyAscensionChangedMessage>(this.HandleAscensionChangedMessage));
    this.NetService.UnregisterMessageHandler<LobbySeedChangedMessage>(new MessageHandlerDelegate<LobbySeedChangedMessage>(this.HandleSeedChangedMessage));
    this.NetService.UnregisterMessageHandler<LobbyModifiersChangedMessage>(new MessageHandlerDelegate<LobbyModifiersChangedMessage>(this.HandleModifiersChangedMessage));
    this.NetService.UnregisterMessageHandler<LobbyPlayerSetReadyMessage>(new MessageHandlerDelegate<LobbyPlayerSetReadyMessage>(this.HandlePlayerReadyMessage));
    this.NetService.UnregisterMessageHandler<LobbyBeginRunMessage>(new MessageHandlerDelegate<LobbyBeginRunMessage>(this.HandleLobbyBeginRunMessage));
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

  public LobbyPlayer? AddLocalHostPlayer(UnlockState unlocks, int maxMultiplayerAscension)
  {
    if (this.NetService.Type == NetGameType.Client)
      throw new InvalidOperationException("Tried to add local host player as client!");
    this._logger.Context = $"{nameof (StartRunLobby)} ({this.NetService.NetId})";
    return this.AddLocalHostPlayerInternal(unlocks.ToSerializable(), maxMultiplayerAscension);
  }

  public LobbyPlayer? AddLocalHostPlayerInternal(
    SerializableUnlockState unlockState,
    int maxMultiplayerAscension)
  {
    LobbyPlayer? nullable = this.TryAddPlayerInFirstAvailableSlot(unlockState, maxMultiplayerAscension, this.NetService.NetId);
    if (nullable.HasValue)
    {
      this.LobbyListener.PlayerConnected(nullable.Value);
      Action<LobbyPlayer> playerConnected = this.PlayerConnected;
      if (playerConnected != null)
        playerConnected(nullable.Value);
    }
    this.UpdateMaxMultiplayerAscension();
    return nullable;
  }

  private void HandleClientLobbyJoinRequestMessage(
    ClientLobbyJoinRequestMessage message,
    ulong senderId)
  {
    INetHostGameService netHostGameService = this.NetService.Type == NetGameType.Host ? (INetHostGameService) this.NetService : throw new InvalidOperationException("Received ClientLobbyJoinRequestMessage as non-host!");
    try
    {
      if (this.Players.Count >= this.MaxPlayers)
      {
        this._logger.Warn($"Client {senderId} sent ClientLobbyJoinRequestMessage but we are at maximum players!");
        netHostGameService.DisconnectClient(senderId, NetError.LobbyFull);
      }
      else
      {
        this._logger.Info($"Received ClientLobbyJoinRequestMessage for {senderId}");
        LobbyPlayer? nullable = this.TryAddPlayerInFirstAvailableSlot(message.unlockState, message.maxAscensionUnlocked, senderId);
        if (!nullable.HasValue)
          return;
        this.UpdateMaxMultiplayerAscension();
        ClientLobbyJoinResponseMessage message1 = new ClientLobbyJoinResponseMessage();
        message1.playersInLobby = this.Players;
        message1.ascension = this.Ascension;
        message1.dailyTime = this.DailyTime;
        message1.seed = this.Seed;
        message1.modifiers = this.Modifiers.Select<ModifierModel, SerializableModifier>((Func<ModifierModel, SerializableModifier>) (m => m.ToSerializable())).ToList<SerializableModifier>();
        this._logger.Debug($"Sending ClientLobbyJoinResponseMessage length ({message1.playersInLobby.Count}) to ({nullable.Value.id})");
        netHostGameService.SendMessage<ClientLobbyJoinResponseMessage>(message1, senderId);
        netHostGameService.SetPeerReadyForBroadcasting(senderId);
        PlayerJoinedMessage message2 = new PlayerJoinedMessage();
        message2.lobbyPlayer = nullable.Value;
        foreach (LobbyPlayer player in this.Players)
        {
          if ((long) player.id != (long) this.NetService.NetId && (long) player.id != (long) nullable.Value.id)
            this.NetService.SendMessage<PlayerJoinedMessage>(message2, player.id);
        }
        this.RemoveConnectingPlayer(nullable.Value.id);
        this.LobbyListener.PlayerConnected(nullable.Value);
        Action<LobbyPlayer> playerConnected = this.PlayerConnected;
        if (playerConnected == null)
          return;
        playerConnected(nullable.Value);
      }
    }
    catch
    {
      netHostGameService.DisconnectClient(senderId, NetError.InternalError);
      throw;
    }
  }

  private void UpdateMaxMultiplayerAscension()
  {
    int num = this.Players.Min<LobbyPlayer>((Func<LobbyPlayer, int>) (p => p.maxMultiplayerAscensionUnlocked));
    if (num == this.MaxAscension)
      return;
    this.MaxAscension = num;
    this.LobbyListener.MaxAscensionChanged();
    if (this.Ascension <= this.MaxAscension || this.NetService.Type != NetGameType.Host)
      return;
    this.SyncAscensionChange(this.MaxAscension);
  }

  private void HandleClientLoadJoinRequestMessage(ClientLoadJoinRequestMessage _, ulong senderId)
  {
    if (this.NetService.Type != NetGameType.Host)
      throw new InvalidOperationException("Received ClientLoadJoinRequestMessage as non-host!");
    this._logger.Info($"Received invalid ClientLoadJoinRequestMessage for {senderId}");
    ((NetHostGameService) this.NetService).DisconnectClient(senderId, NetError.InvalidJoin, false);
  }

  private void HandleClientRejoinRequestMessage(ClientRejoinRequestMessage _, ulong senderId)
  {
    if (this.NetService.Type != NetGameType.Host)
      throw new InvalidOperationException("Received ClientRejoinRequestMessage as non-host!");
    this._logger.Info($"Received invalid ClientRejoinRequestMessage for {senderId}");
    ((NetHostGameService) this.NetService).DisconnectClient(senderId, NetError.InvalidJoin, false);
  }

  private void HandlePlayerJoinedMessage(PlayerJoinedMessage message, ulong senderId)
  {
    this._logger.Debug($"Received PlayerJoinedMessage with ({message.lobbyPlayer})");
    this.Players.Add(message.lobbyPlayer);
    this.LobbyListener.PlayerConnected(message.lobbyPlayer);
    Action<LobbyPlayer> playerConnected = this.PlayerConnected;
    if (playerConnected != null)
      playerConnected(message.lobbyPlayer);
    this.UpdateMaxMultiplayerAscension();
  }

  private void HandlePlayerLeftMessage(PlayerLeftMessage message, ulong senderId)
  {
    this._logger.Debug($"Received PlayerLeftMessage for {message.playerId}");
    int index = this.Players.FindIndex((Predicate<LobbyPlayer>) (p => (long) p.id == (long) message.playerId));
    if (index < 0)
      return;
    LobbyPlayer player = this.Players[index];
    this.Players.RemoveAt(index);
    this.InputSynchronizer.OnPlayerDisconnected(player.id);
    this.LobbyListener.RemotePlayerDisconnected(player);
    Action<LobbyPlayer> playerDisconnected = this.PlayerDisconnected;
    if (playerDisconnected == null)
      return;
    playerDisconnected(player);
  }

  private void HandleLobbyPlayerChangedCharacterMessage(
    LobbyPlayerChangedCharacterMessage message,
    ulong senderId)
  {
    this._logger.Debug($"Received LobbyPlayerChangedCharacterMessage for {senderId} {message.character}");
    this.ChangeCharacter(senderId, message.character);
  }

  private void HandleAscensionChangedMessage(LobbyAscensionChangedMessage message, ulong _)
  {
    if (this._isBeginningRun)
    {
      Log.Warn($"Received AscensionChangedMessage with ascension {message.ascension} while run was already starting! Ignoring");
    }
    else
    {
      this._logger.Debug($"Received AscensionChangedMessage, new ascension: {message.ascension}");
      this.Ascension = message.ascension;
      this.LobbyListener.AscensionChanged();
    }
  }

  private void HandleSeedChangedMessage(LobbySeedChangedMessage message, ulong _)
  {
    if (this._isBeginningRun)
    {
      Log.Warn($"Received SeedChangedMessage with seed {message.seed} while run was already starting! Ignoring");
    }
    else
    {
      this._logger.Debug("Received SeedChangedMessage, new seed: " + message.seed);
      this.Seed = message.seed;
      this.LobbyListener.SeedChanged();
    }
  }

  private void HandleModifiersChangedMessage(LobbyModifiersChangedMessage message, ulong _)
  {
    this._logger.Debug("Received ModifiersChangedMessage, new modifiers: " + string.Join<ModelId>(",", message.modifiers.Select<SerializableModifier, ModelId>((Func<SerializableModifier, ModelId>) (m => m.Id))));
    if (this._isBeginningRun)
    {
      Log.Warn($"Received ModifiersChangedMessage with {message.modifiers.Count} while run was already starting! Ignoring");
    }
    else
    {
      this._modifiers.Clear();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this._modifiers.AddRange(message.modifiers.Select<SerializableModifier, ModifierModel>(StartRunLobby.\u003C\u003EO.\u003C0\u003E__FromSerializable ?? (StartRunLobby.\u003C\u003EO.\u003C0\u003E__FromSerializable = new Func<SerializableModifier, ModifierModel>(ModifierModel.FromSerializable))));
      this.LobbyListener.ModifiersChanged();
    }
  }

  private void HandlePlayerReadyMessage(LobbyPlayerSetReadyMessage message, ulong senderId)
  {
    this._logger.Debug($"Received LobbyPlayerSetReadyMessage for player {senderId} with value {message.ready}");
    int index = this.Players.FindIndex((Predicate<LobbyPlayer>) (p => (long) p.id == (long) senderId));
    if (index < 0)
      return;
    LobbyPlayer player = this.Players[index] with
    {
      isReady = message.ready
    };
    this.Players[index] = player;
    this.LobbyListener.PlayerChanged(player, false);
    this.BeginRunForAllPlayersIfAllReady();
  }

  private void HandleLobbyBeginRunMessage(LobbyBeginRunMessage message, ulong senderId)
  {
    this._logger.Debug("Received LobbyBeginRunMessage");
    this.Players.Clear();
    this.Players.AddRange((IEnumerable<LobbyPlayer>) message.playersInLobby);
    this.Act1 = message.act1;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    this.BeginRunLocally(message.seed, message.modifiers.Select<SerializableModifier, ModifierModel>(StartRunLobby.\u003C\u003EO.\u003C0\u003E__FromSerializable ?? (StartRunLobby.\u003C\u003EO.\u003C0\u003E__FromSerializable = new Func<SerializableModifier, ModifierModel>(ModifierModel.FromSerializable))).ToList<ModifierModel>());
  }

  private void ChangeCharacter(
    ulong playerId,
    CharacterModel character,
    bool isRandomCharacterResolution = false)
  {
    if (this._isBeginningRun)
    {
      Log.Warn($"Player {playerId} tried to change character while run was already starting! Ignoring");
    }
    else
    {
      int index = this.Players.FindIndex((Predicate<LobbyPlayer>) (p => (long) p.id == (long) playerId));
      if (index < 0)
        return;
      LobbyPlayer player = this.Players[index] with
      {
        character = character
      };
      this.Players[index] = player;
      this.LobbyListener.PlayerChanged(player, isRandomCharacterResolution);
    }
  }

  private void BeginRunForAllPlayers(string seed, List<ModifierModel> modifiers)
  {
    if (this.NetService.Type == NetGameType.Client)
      throw new InvalidOperationException("Can only begin run as host!");
    if (this._isBeginningRun)
    {
      this._logger.Warn("Tried to begin run twice, ignoring second one!");
    }
    else
    {
      this.UpdatePreferredAscension();
      this.NetService.SendMessage<LobbyBeginRunMessage>(new LobbyBeginRunMessage()
      {
        playersInLobby = this.Players,
        seed = seed,
        modifiers = modifiers.Select<ModifierModel, SerializableModifier>((Func<ModifierModel, SerializableModifier>) (m => m.ToSerializable())).ToList<SerializableModifier>(),
        act1 = this.Act1
      });
      this.BeginRunLocally(seed, modifiers);
      if (this.NetService.Type != NetGameType.Host)
        return;
      ((INetHostGameService) this.NetService).NetHost?.SetHostIsClosed(true);
    }
  }

  private void BeginRunLocally(string seed, List<ModifierModel> modifiers)
  {
    Rng rng = new Rng(StringHelper.GetDeterministicHashCode(seed), "act_selection");
    List<ActModel> list = ActModel.GetRandomList(rng, this.GetUnlockState(), this.NetService.Type.IsMultiplayer()).ToList<ActModel>();
    list[0] = StartRunLobby.GetAct(this.Act1) ?? list[0];
    for (int index = 0; index < this.Players.Count; ++index)
    {
      LobbyPlayer player = this.Players[index];
      if (player.character is RandomCharacter)
      {
        CharacterModel character = rng.NextItem<CharacterModel>(ModelDb.AllCharacters);
        this.ChangeCharacter(player.id, character, true);
      }
    }
    if (this.NetService.Type == NetGameType.Singleplayer)
    {
      CharacterStats characterStats = SaveManager.Instance.Progress.GetOrCreateCharacterStats(this.Players[0].character.Id);
      int num = Math.Min(this.Ascension, characterStats.MaxAscension);
      if (this.Ascension != num)
      {
        this.Ascension = num;
        this.LobbyListener.AscensionChanged();
      }
      if (this.MaxAscension != characterStats.MaxAscension)
      {
        this.MaxAscension = characterStats.MaxAscension;
        this.LobbyListener.MaxAscensionChanged();
      }
    }
    this.NetService.SetBufferMessages(true);
    this._isBeginningRun = true;
    this.LobbyListener.BeginRun(seed, list, (IReadOnlyList<ModifierModel>) modifiers);
  }

  private static ActModel? GetAct(string act1Key)
  {
    ActModel act;
    switch (act1Key)
    {
      case "overgrowth":
        act = (ActModel) ModelDb.Act<Overgrowth>();
        break;
      case "underdocks":
        act = (ActModel) ModelDb.Act<Underdocks>();
        break;
      default:
        act = (ActModel) null;
        break;
    }
    return act;
  }

  private void SetSingleplayerAscensionAfterCharacterChanged(ModelId characterId)
  {
    if (this.NetService.Type.IsMultiplayer())
      return;
    CharacterStats characterStats = SaveManager.Instance.Progress.GetOrCreateCharacterStats(characterId);
    bool flag = this.IsAscensionEpochRevealed(characterId);
    if (characterId == ModelDb.GetId<RandomCharacter>())
    {
      this.MaxAscension = this.GetMaxAscensionAcrossAllCharacters();
      this.SyncAscensionChange(Math.Min(characterStats.PreferredAscension, this.MaxAscension));
      this.LobbyListener.MaxAscensionChanged();
      this._logger.Info($"{characterId} ascension set to Max: {this.Ascension}");
    }
    else if (characterStats == null || characterStats.MaxAscension <= 0 || !flag)
    {
      this.MaxAscension = 0;
      this.SyncAscensionChange(0);
      this.LobbyListener.MaxAscensionChanged();
      if (!flag)
        this._logger.Info($"{characterId} has not revealed the Ascension Epoch, disabling Ascension.");
      else
        this._logger.Info($"{characterId} has no progress, disabling Ascension.");
    }
    else
    {
      this.MaxAscension = characterStats.MaxAscension;
      this.SyncAscensionChange(Math.Min(characterStats.PreferredAscension, characterStats.MaxAscension));
      this.LobbyListener.MaxAscensionChanged();
      this._logger.Info($"{characterId} ascension set to preferred: {this.Ascension}");
    }
    if (this.GameMode != GameMode.Standard || this.GetMaxAscensionAcrossAllCharacters() <= 0 || SaveManager.Instance.SeenPopup("ascension_singleplayer_ftue"))
      return;
    NAscensionSingleplayerFtue modalToCreate = NAscensionSingleplayerFtue.Create();
    if (modalToCreate == null)
      return;
    NModalContainer.Instance.Add((Node) modalToCreate);
  }

  private int GetMaxAscensionAcrossAllCharacters()
  {
    int acrossAllCharacters = 0;
    foreach (CharacterStats characterStats in SaveManager.Instance.Progress.CharacterStats.Values)
      acrossAllCharacters = Math.Max(acrossAllCharacters, characterStats.MaxAscension);
    return acrossAllCharacters;
  }

  private bool IsAscensionEpochRevealed(ModelId characterId)
  {
    if (characterId == ModelDb.GetId<Ironclad>())
      return SaveManager.Instance.IsEpochRevealed<Ironclad4Epoch>();
    if (characterId == ModelDb.GetId<Silent>())
      return SaveManager.Instance.IsEpochRevealed<Silent4Epoch>();
    if (characterId == ModelDb.GetId<Regent>())
      return SaveManager.Instance.IsEpochRevealed<Regent4Epoch>();
    if (characterId == ModelDb.GetId<Defect>())
      return SaveManager.Instance.IsEpochRevealed<Defect4Epoch>();
    return !(characterId == ModelDb.GetId<Necrobinder>()) || SaveManager.Instance.IsEpochRevealed<Necrobinder4Epoch>();
  }

  private void UpdatePreferredAscension()
  {
    if (this.GameMode == GameMode.Daily)
      return;
    if (this.NetService.Type == NetGameType.Singleplayer)
    {
      if (this.Players.Count == 0)
        return;
      CharacterStats characterStats = SaveManager.Instance.Progress.GetOrCreateCharacterStats(this.LocalPlayer.character.Id);
      if (characterStats.MaxAscension == 0 && characterStats.Id != ModelDb.Character<RandomCharacter>().Id || characterStats.PreferredAscension == this.Ascension)
        return;
      this._logger.Info($"Setting preferred Ascension for {this.LocalPlayer.character.Id} to {this.Ascension}");
      characterStats.PreferredAscension = this.Ascension;
      SaveManager.Instance.SaveProgressFile();
    }
    else
    {
      if (this.NetService.Type != NetGameType.Host)
        return;
      ProgressState progress = SaveManager.Instance.Progress;
      if (progress.PreferredMultiplayerAscension == this.Ascension)
        return;
      this._logger.Info($"Setting preferred multiplayer ascension to {this.Ascension}");
      progress.PreferredMultiplayerAscension = this.Ascension;
      SaveManager.Instance.SaveProgressFile();
    }
  }

  public void SetLocalCharacter(CharacterModel character)
  {
    this.ChangeCharacter(this.NetService.NetId, character);
    this.NetService.SendMessage<LobbyPlayerChangedCharacterMessage>(new LobbyPlayerChangedCharacterMessage()
    {
      character = character
    });
    this.SetSingleplayerAscensionAfterCharacterChanged(character.Id);
  }

  public void SetSeed(string? seed)
  {
    bool flag;
    switch (this.NetService.Type)
    {
      case NetGameType.Singleplayer:
      case NetGameType.Host:
        flag = true;
        break;
      default:
        flag = false;
        break;
    }
    if (!flag)
      throw new InvalidOperationException("Can only be called on host or singleplayer");
    this.Seed = seed;
    this.NetService.SendMessage<LobbySeedChangedMessage>(new LobbySeedChangedMessage()
    {
      seed = seed
    });
    this.LobbyListener.SeedChanged();
  }

  public void SetModifiers(IReadOnlyCollection<ModifierModel> modifiers)
  {
    bool flag;
    switch (this.NetService.Type)
    {
      case NetGameType.Singleplayer:
      case NetGameType.Host:
        flag = true;
        break;
      default:
        flag = false;
        break;
    }
    if (!flag)
      throw new InvalidOperationException("Can only be called on host or singleplayer");
    if (this._isBeginningRun)
    {
      Log.Warn("Tried to change modifiers while run was already starting! Ignoring");
    }
    else
    {
      this._modifiers.Clear();
      this._modifiers.AddRange((IEnumerable<ModifierModel>) modifiers);
      this.NetService.SendMessage<LobbyModifiersChangedMessage>(new LobbyModifiersChangedMessage()
      {
        modifiers = modifiers.Select<ModifierModel, SerializableModifier>((Func<ModifierModel, SerializableModifier>) (m => m.ToSerializable())).ToList<SerializableModifier>()
      });
      this.LobbyListener.ModifiersChanged();
    }
  }

  public void SetReady(bool ready)
  {
    int index = this.Players.FindIndex((Predicate<LobbyPlayer>) (p => (long) p.id == (long) this.NetService.NetId));
    if (index < 0)
      throw new InvalidOperationException("Tried to set local player ready, but they are not in the list of players in the lobby!");
    if (this._isBeginningRun)
    {
      Log.Warn("Tried to set ready while run was already starting! Ignoring");
    }
    else
    {
      LobbyPlayer player = this.Players[index] with
      {
        isReady = ready
      };
      this.Players[index] = player;
      this.NetService.SendMessage<LobbyPlayerSetReadyMessage>(new LobbyPlayerSetReadyMessage()
      {
        ready = ready
      });
      this.LobbyListener.PlayerChanged(this.LocalPlayer, false);
      this._logger.Info($"Local player {this.LocalPlayer.id} is ready");
      this.BeginRunForAllPlayersIfAllReady();
    }
  }

  private void BeginRunForAllPlayersIfAllReady()
  {
    if (!this.IsAboutToBeginGame())
      return;
    bool flag;
    switch (this.NetService.Type)
    {
      case NetGameType.Singleplayer:
      case NetGameType.Host:
        flag = true;
        break;
      default:
        flag = false;
        break;
    }
    if (!flag)
      return;
    this.BeginRunForAllPlayers(NGame.Instance?.DebugSeedOverride == null ? (this.Seed == null ? SeedHelper.GetRandomSeed() : SeedHelper.CanonicalizeSeed(this.Seed)) : NGame.Instance.DebugSeedOverride, this._modifiers);
  }

  public bool IsAboutToBeginGame()
  {
    return this._connectingPlayers.Count <= 0 && (!this.NetService.Type.IsMultiplayer() || this.Players.Count != 1) && this.Players.All<LobbyPlayer>((Func<LobbyPlayer, bool>) (p => p.isReady));
  }

  public void SyncAscensionChange(int ascension)
  {
    if (this.NetService.Type == NetGameType.Client)
      throw new InvalidOperationException("Client attempted to change ascension level!");
    if (this.Ascension == ascension)
      return;
    if (this._isBeginningRun)
    {
      Log.Warn($"Tried to set ascension to {ascension} while run was already starting! Ignoring");
    }
    else
    {
      this.Ascension = ascension;
      this.NetService.SendMessage<LobbyAscensionChangedMessage>(new LobbyAscensionChangedMessage()
      {
        ascension = ascension
      });
      this.UpdatePreferredAscension();
      this.LobbyListener.AscensionChanged();
    }
  }

  private LobbyPlayer? TryAddPlayerInFirstAvailableSlot(
    SerializableUnlockState unlockState,
    int maxAscensionUnlocked,
    ulong playerId)
  {
    int num = -1;
    for (int i = 0; i < this.MaxPlayers; i++)
    {
      if (this.Players.FindIndex((Predicate<LobbyPlayer>) (p => p.slotId == i)) < 0)
      {
        num = i;
        break;
      }
    }
    if (num < 0)
      return new LobbyPlayer?();
    LobbyPlayer lobbyPlayer = new LobbyPlayer()
    {
      character = (CharacterModel) ModelDb.Character<Ironclad>(),
      id = playerId,
      slotId = num,
      maxMultiplayerAscensionUnlocked = maxAscensionUnlocked,
      unlockState = unlockState
    };
    this.Players.Add(lobbyPlayer);
    return new LobbyPlayer?(lobbyPlayer);
  }

  private void OnConnectedToClientAsHost(ulong playerId)
  {
    this._logger.Info($"Client {playerId} connected. Sending initial game info message");
    InitialGameInfoMessage message = InitialGameInfoMessage.Basic() with
    {
      sessionState = RunSessionState.InLobby,
      gameMode = this.GameMode
    };
    if (this._isBeginningRun)
    {
      message.connectionFailureReason = new ConnectionFailureReason?(ConnectionFailureReason.RunInProgress);
      this.NetService.SendMessage<InitialGameInfoMessage>(message, playerId);
      this._logger.Warn($"Client {playerId} connected but we are already beginning the run!");
      ((NetHostGameService) this.NetService).DisconnectClient(playerId, NetError.RunInProgress, false);
    }
    else if (this.Players.Count >= this.MaxPlayers)
    {
      message.connectionFailureReason = new ConnectionFailureReason?(ConnectionFailureReason.LobbyFull);
      this.NetService.SendMessage<InitialGameInfoMessage>(message, playerId);
      this._logger.Warn($"Client {playerId} connected but we are at maximum players!");
      ((NetHostGameService) this.NetService).DisconnectClient(playerId, NetError.LobbyFull, false);
    }
    else
    {
      StartRunLobby.ConnectingPlayer connectingPlayer = new StartRunLobby.ConnectingPlayer()
      {
        id = playerId,
        timeoutCancelToken = new CancellationTokenSource()
      };
      this._connectingPlayers.Add(connectingPlayer);
      this.NetService.SendMessage<InitialGameInfoMessage>(message, playerId);
      TaskHelper.RunSafely(this.BeginHandshakeTimeout(connectingPlayer));
    }
  }

  private async Task BeginHandshakeTimeout(StartRunLobby.ConnectingPlayer connectingPlayer)
  {
    await Task.Delay(this.HandshakeTimeout, connectingPlayer.timeoutCancelToken.Token);
    if (connectingPlayer.timeoutCancelToken.IsCancellationRequested || this._connectingPlayers.IndexOf(connectingPlayer) < 0)
      return;
    this._logger.Info($"Disconnecting player {connectingPlayer.id} because they did not respond to the initial game join handshake within {this.HandshakeTimeout}ms");
    ((INetHostGameService) this.NetService).DisconnectClient(connectingPlayer.id, NetError.HandshakeTimeout);
  }

  private void OnDisconnectedFromClientAsHost(ulong playerId, NetErrorInfo info)
  {
    this._logger.Info($"Client {playerId} disconnected, reason: {info.GetReason()}");
    this.RemoveConnectingPlayer(playerId);
    int index = this.Players.FindIndex((Predicate<LobbyPlayer>) (p => (long) p.id == (long) playerId));
    if (index < 0)
    {
      this._logger.Info($"Player {playerId} not found in players list. Assuming they disconnected during the handshake");
    }
    else
    {
      LobbyPlayer player = this.Players[index];
      this.NetService.SendMessage<PlayerLeftMessage>(new PlayerLeftMessage()
      {
        playerId = playerId
      });
      this.Players.RemoveAt(index);
      this.InputSynchronizer.OnPlayerDisconnected(player.id);
      this.LobbyListener.RemotePlayerDisconnected(player);
      Action<LobbyPlayer> playerDisconnected = this.PlayerDisconnected;
      if (playerDisconnected != null)
        playerDisconnected(player);
      this.UpdateMaxMultiplayerAscension();
      this.BeginRunForAllPlayersIfAllReady();
    }
  }

  private UnlockState GetUnlockState()
  {
    return this.GameMode == GameMode.Daily ? UnlockState.all : new UnlockState(this.Players.Select<LobbyPlayer, UnlockState>((Func<LobbyPlayer, UnlockState>) (p => UnlockState.FromSerializable(p.unlockState))));
  }

  private void RemoveConnectingPlayer(ulong playerId)
  {
    for (int index = 0; index < this._connectingPlayers.Count; ++index)
    {
      if ((long) this._connectingPlayers[index].id == (long) playerId)
      {
        this._connectingPlayers[index].timeoutCancelToken.Cancel();
        this._logger.Info($"Cancel handshake timeout for {playerId}");
        this._connectingPlayers.RemoveAt(index);
        --index;
      }
    }
  }

  private void OnDisconnected(NetErrorInfo info)
  {
    this._logger.Info($"Disconnected from host, reason: {info.GetReason()}");
    this.LobbyListener.LocalPlayerDisconnected(info);
  }

  private struct ConnectingPlayer : IEquatable<StartRunLobby.ConnectingPlayer>
  {
    public ulong id;
    public CancellationTokenSource timeoutCancelToken;

    public bool Equals(StartRunLobby.ConnectingPlayer other)
    {
      return (long) this.id == (long) other.id && this.timeoutCancelToken.Equals((object) other.timeoutCancelToken);
    }

    public override bool Equals(object? obj)
    {
      return obj is StartRunLobby.ConnectingPlayer other && this.Equals(other);
    }

    public override int GetHashCode()
    {
      return HashCode.Combine<ulong, CancellationTokenSource>(this.id, this.timeoutCancelToken);
    }
  }
}
