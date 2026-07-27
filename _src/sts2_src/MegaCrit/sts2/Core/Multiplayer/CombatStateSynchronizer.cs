// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.CombatStateSynchronizer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Game.Lobby;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Game;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer;

public class CombatStateSynchronizer : IDisposable
{
  private readonly INetGameService _netService;
  private readonly RunState _runState;
  private readonly RunLobby? _runLobby;
  private readonly Dictionary<ulong, SerializablePlayer> _syncData = new Dictionary<ulong, SerializablePlayer>();
  private SerializableRunRngSet? _rngSet;
  private SerializableRelicGrabBag? _sharedRelicGrabBag;
  private readonly Logger _logger = new Logger(nameof (CombatStateSynchronizer), LogType.GameSync);
  private TaskCompletionSource? _syncCompletionSource;

  public bool IsDisabled { get; set; }

  public CombatStateSynchronizer(INetGameService netService, RunLobby? runLobby, RunState runState)
  {
    this._netService = netService;
    this._runState = runState;
    this._runLobby = runLobby;
    this._netService.RegisterMessageHandler<SyncPlayerDataMessage>(new MessageHandlerDelegate<SyncPlayerDataMessage>(this.OnSyncPlayerMessageReceived));
    this._netService.RegisterMessageHandler<SyncRngMessage>(new MessageHandlerDelegate<SyncRngMessage>(this.OnSyncRngMessageReceived));
    if (this._runLobby == null)
      return;
    this._runLobby.RemotePlayerDisconnected += new Action<ulong>(this.OnPeerDisconnected);
  }

  public void Dispose()
  {
    this._netService.UnregisterMessageHandler<SyncPlayerDataMessage>(new MessageHandlerDelegate<SyncPlayerDataMessage>(this.OnSyncPlayerMessageReceived));
    this._netService.UnregisterMessageHandler<SyncRngMessage>(new MessageHandlerDelegate<SyncRngMessage>(this.OnSyncRngMessageReceived));
    if (this._runLobby == null)
      return;
    this._runLobby.RemotePlayerDisconnected -= new Action<ulong>(this.OnPeerDisconnected);
  }

  private void OnSyncPlayerMessageReceived(SyncPlayerDataMessage syncMessage, ulong senderId)
  {
    this._logger.Debug($"Received sync player message from {senderId}");
    if (this._syncData.ContainsKey(senderId))
    {
      this._logger.Error($"Received two player sync messages from {senderId}! Ignoring the second one");
    }
    else
    {
      this._syncData[senderId] = syncMessage.player;
      this.CheckSyncCompleted();
    }
  }

  private void OnSyncRngMessageReceived(SyncRngMessage syncMessage, ulong senderId)
  {
    this._logger.Debug($"Received sync RNG message from {senderId}");
    if (this._rngSet != null)
    {
      this._logger.Error($"Received two RNG sync messages from {senderId}! Ignoring the second one");
    }
    else
    {
      this._rngSet = syncMessage.rng;
      this._sharedRelicGrabBag = syncMessage.sharedRelicGrabBag;
      this.CheckSyncCompleted();
    }
  }

  private void OnPeerDisconnected(ulong peerId)
  {
    TaskCompletionSource completionSource = this._syncCompletionSource;
    if ((completionSource != null ? (completionSource.Task.IsCompleted ? 1 : 0) : 1) != 0)
      return;
    this._logger.Debug($"Peer {peerId} disconnected, checking if sync is complete");
    this.CheckSyncCompleted();
  }

  public void StartSync()
  {
    this._logger.Debug("Broadcasting combat sync message to all peers");
    if (this._netService.Type == NetGameType.Singleplayer || this.IsDisabled)
      return;
    if (this._syncCompletionSource != null)
      throw new InvalidOperationException("StartSync called twice before WaitForSync!");
    SyncPlayerDataMessage message = new SyncPlayerDataMessage();
    Player me = LocalContext.GetMe((IPlayerCollection) this._runState);
    message.player = me.ToSerializable();
    this._netService.SendMessage<SyncPlayerDataMessage>(message);
    this._syncData[message.player.NetId] = message.player;
    this._syncCompletionSource = new TaskCompletionSource();
    if (this._netService.Type == NetGameType.Host)
    {
      this._rngSet = me.RunState.Rng.ToSerializable();
      this._sharedRelicGrabBag = me.RunState.SharedRelicGrabBag.ToSerializable();
      this._netService.SendMessage<SyncRngMessage>(new SyncRngMessage()
      {
        rng = this._rngSet,
        sharedRelicGrabBag = this._sharedRelicGrabBag
      });
    }
    this.CheckSyncCompleted();
  }

  public async Task WaitForSync()
  {
    this._logger.Debug("Waiting to receive all sync messages from all clients");
    if (this._netService.Type == NetGameType.Singleplayer || this.IsDisabled)
      return;
    if (this._syncCompletionSource == null)
      throw new InvalidOperationException("StartSync must be called before WaitForSync!");
    await this._syncCompletionSource.Task;
    foreach (KeyValuePair<ulong, SerializablePlayer> keyValuePair in this._syncData)
    {
      if (this._runLobby != null && !this._runLobby.ConnectedPlayerIds.Contains<ulong>(keyValuePair.Key))
      {
        this._logger.Debug($"Skipping sync for disconnected player {keyValuePair.Key}");
      }
      else
      {
        Player player = this._runState.GetPlayer(keyValuePair.Key);
        if (!LocalContext.IsMe(player))
          player.SyncWithSerializedPlayer(keyValuePair.Value);
      }
    }
    if (this._netService.Type != NetGameType.Host)
    {
      if (this._rngSet != null)
        this._runState.Rng.LoadFromSerializable(this._rngSet);
      else if (this._runState.Players.Count > 1)
        this._logger.Error("There are two or more players and we are a client, but we never received the RNG set!");
      if (this._sharedRelicGrabBag != null)
        this._runState.SharedRelicGrabBag.LoadFromSerializable(this._sharedRelicGrabBag);
      else if (this._runState.Players.Count > 1)
        this._logger.Error("There are two or more players and we are a client, but we never received the shared relic grab bag!");
    }
    this._syncData.Clear();
    this._rngSet = (SerializableRunRngSet) null;
    this._sharedRelicGrabBag = (SerializableRelicGrabBag) null;
    this._syncCompletionSource = (TaskCompletionSource) null;
  }

  private void CheckSyncCompleted()
  {
    if (this._syncCompletionSource == null)
      return;
    if (this._runLobby == null)
      throw new InvalidOperationException("Tried to combat sync without a valid run lobby!");
    if (this._netService.Type == NetGameType.Singleplayer || this.IsDisabled)
    {
      this._syncCompletionSource.SetResult();
    }
    else
    {
      foreach (ulong connectedPlayerId in (IEnumerable<ulong>) this._runLobby.ConnectedPlayerIds)
      {
        if (!this._syncData.ContainsKey(connectedPlayerId))
          return;
      }
      if (this._rngSet == null)
        return;
      this._syncCompletionSource.SetResult();
    }
  }
}
