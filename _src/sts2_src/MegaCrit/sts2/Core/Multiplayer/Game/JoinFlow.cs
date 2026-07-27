// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Game.JoinFlow
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Multiplayer.Connection;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Unlocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Game;

public class JoinFlow
{
  private TaskCompletionSource<InitialGameInfoMessage>? _connectCompletion;
  private TaskCompletionSource<ClientRejoinResponseMessage>? _rejoinCompletion;
  private TaskCompletionSource<ClientLoadJoinResponseMessage>? _loadJoinCompletion;
  private TaskCompletionSource<ClientLobbyJoinResponseMessage>? _joinCompletion;
  private readonly Logger _logger = new Logger(nameof (JoinFlow), LogType.Network);
  private readonly JoinFlow.MockInfo? _mockInfo;

  public INetClientGameService NetService { get; }

  public CancellationTokenSource CancelToken { get; } = new CancellationTokenSource();

  public JoinFlow(INetClientGameService netService, JoinFlow.MockInfo? mockInfo = null)
  {
    this._mockInfo = mockInfo;
    this.NetService = netService;
  }

  public async Task<JoinResult> Begin(IClientConnectionInitializer initializer, SceneTree? sceneTree)
  {
    Logger.logLevelTypeMap[LogType.Network] = LogLevel.Debug;
    Logger.logLevelTypeMap[LogType.Actions] = LogLevel.VeryDebug;
    Logger.logLevelTypeMap[LogType.GameSync] = LogLevel.VeryDebug;
    if (this._connectCompletion != null)
      throw new InvalidOperationException("JoinFlow object can only be used once!");
    this._logger.Info($"Beginning join with initializer {initializer}");
    this.CancelToken.Token.Register(new Action(this.Cancel));
    CancellationTokenSource updateLoopCancelSource = new CancellationTokenSource();
    if (sceneTree != null)
      TaskHelper.RunSafely(this.NetServiceUpdateLoop(updateLoopCancelSource, sceneTree));
    object obj = (object) null;
    int num = 0;
    JoinResult joinResult;
    try
    {
      try
      {
        this.NetService.RegisterMessageHandler<InitialGameInfoMessage>(new MessageHandlerDelegate<InitialGameInfoMessage>(this.HandleInitialGameInfoMessage));
        this.NetService.RegisterMessageHandler<ClientLobbyJoinResponseMessage>(new MessageHandlerDelegate<ClientLobbyJoinResponseMessage>(this.HandleJoinResponseMessage));
        this.NetService.RegisterMessageHandler<ClientLoadJoinResponseMessage>(new MessageHandlerDelegate<ClientLoadJoinResponseMessage>(this.HandleLoadJoinResponseMessage));
        this.NetService.RegisterMessageHandler<ClientRejoinResponseMessage>(new MessageHandlerDelegate<ClientRejoinResponseMessage>(this.HandleRejoinResponseMessage));
        this.NetService.Disconnected += new Action<NetErrorInfo>(this.OnDisconnected);
        this._connectCompletion = new TaskCompletionSource<InitialGameInfoMessage>();
        NetErrorInfo? nullable = await initializer.Connect(this.NetService, this.CancelToken.Token);
        if (nullable.HasValue)
        {
          this._logger.Info($"Connection failed: {nullable}");
          throw new ClientConnectionFailedException("Could not connect", nullable.Value);
        }
        this._logger.Info("Initializer connection completed, awaiting initial game info message");
        InitialGameInfoMessage initialMessage = await this._connectCompletion.Task;
        if (initialMessage.connectionFailureReason.HasValue)
        {
          this._logger.Info($"Received initial join message with failure: {initialMessage.connectionFailureReason}");
          throw new ClientConnectionFailedException("Got connection failure from host", new NetErrorInfo(initialMessage.connectionFailureReason.Value));
        }
        RunSessionState state = initialMessage.sessionState;
        this._logger.Info($"Got initial game info message. Version: {initialMessage.version} Hash: {initialMessage.idDatabaseHash} Type: {initialMessage.gameMode} State: {state}");
        ConnectionFailureExtraInfo failureExtraInfo = new ConnectionFailureExtraInfo();
        failureExtraInfo.hostBranch = new PlatformBranch?(initialMessage.branch);
        failureExtraInfo.hostVersion = initialMessage.version;
        failureExtraInfo.hostHash = new ulong?((ulong) initialMessage.idDatabaseHash);
        ref readonly JoinFlow.MockInfo? local1 = ref this._mockInfo;
        failureExtraInfo.localVersion = (local1.HasValue ? local1.GetValueOrDefault().version : (string) null) ?? NGame.GetGameVersion();
        ref readonly JoinFlow.MockInfo? local2 = ref this._mockInfo;
        failureExtraInfo.localBranch = new PlatformBranch?(local2.HasValue ? local2.GetValueOrDefault().branch : PlatformUtil.GetPlatformBranch());
        ref readonly JoinFlow.MockInfo? local3 = ref this._mockInfo;
        failureExtraInfo.localHash = new ulong?(local3.HasValue ? (ulong) local3.GetValueOrDefault().hash : (ulong) ModelIdSerializationCache.Hash);
        ConnectionFailureExtraInfo extraInfo = failureExtraInfo;
        if (initialMessage.version != extraInfo.localVersion)
        {
          NetErrorInfo info = new NetErrorInfo(ConnectionFailureReason.VersionMismatch, extraInfo);
          throw new ClientConnectionFailedException($"Version mismatch. Host: {initialMessage.version} Ours: {extraInfo.localVersion} Host branch: {initialMessage.branch}", info);
        }
        List<string> stringList1 = (!this._mockInfo.HasValue ? ModManager.GetGameplayRelevantModNameList() : this._mockInfo.Value.gameplayAffectingMods) ?? new List<string>();
        List<string> stringList2 = initialMessage.gameplayAffectingMods ?? new List<string>();
        List<string> list1 = stringList2.Except<string>((IEnumerable<string>) stringList1).ToList<string>();
        List<string> list2 = stringList1.Except<string>((IEnumerable<string>) stringList2).ToList<string>();
        extraInfo.missingModsOnHost = list2;
        extraInfo.missingModsOnLocal = list1;
        if (list1.Count > 0 || list2.Count > 0)
        {
          this._logger.Warn($"Mismatch in gameplay-relevant mods with the host!\nMods that host has that we don't: {string.Join(",", (IEnumerable<string>) list1)}.\nMods that we have that host doesn't: {string.Join(",", (IEnumerable<string>) list2)}.");
          throw new ClientConnectionFailedException($"Mod mismatch. Host mods: {string.Join(",", (IEnumerable<string>) stringList2)} Local mods: {string.Join(",", (IEnumerable<string>) stringList1)}", new NetErrorInfo(ConnectionFailureReason.ModMismatch, extraInfo));
        }
        long idDatabaseHash = (long) initialMessage.idDatabaseHash;
        ulong? localHash = extraInfo.localHash;
        long valueOrDefault = (long) localHash.GetValueOrDefault();
        if (!(idDatabaseHash == valueOrDefault & localHash.HasValue))
        {
          this._logger.Warn($"Our version {extraInfo.localVersion} matches the host's, but our Model ID hash does not! Disconnecting");
          throw new ClientConnectionFailedException($"ModelDb hash mismatch. Host: {initialMessage.idDatabaseHash} Ours: {ModelIdSerializationCache.Hash}", new NetErrorInfo(ConnectionFailureReason.VersionMismatch, extraInfo));
        }
        List<string> stringList3 = (!this._mockInfo.HasValue ? ModManager.GetNonGameplayRelevantModNameList() : this._mockInfo.Value.nonGameplayAffectingMods) ?? new List<string>();
        List<string> stringList4 = initialMessage.otherMods ?? new List<string>();
        List<string> list3 = stringList4.Except<string>((IEnumerable<string>) stringList3).ToList<string>();
        List<string> list4 = stringList3.Except<string>((IEnumerable<string>) stringList4).ToList<string>();
        if (list4.Count > 0 || list3.Count > 0)
          this._logger.Warn($"Mismatch in non-gameplay relevant mods. This is allowed, but it's up to the mod authors to guarantee that it doesn't break anything.\nNon-gameplay relevant mods that host has that we don't: {string.Join(",", (IEnumerable<string>) list3)}.\nNon-gameplay relevant mods that we have that host doesn't: {string.Join(",", (IEnumerable<string>) list4)}.");
        switch (state)
        {
          case RunSessionState.InLobby:
            ClientLobbyJoinResponseMessage joinResponseMessage1 = await this.AttemptJoin();
            joinResult = new JoinResult()
            {
              gameMode = initialMessage.gameMode,
              sessionState = new RunSessionState?(state),
              joinResponse = new ClientLobbyJoinResponseMessage?(joinResponseMessage1)
            };
            break;
          case RunSessionState.InLoadedLobby:
            ClientLoadJoinResponseMessage joinResponseMessage2 = await this.AttemptLoadJoin();
            joinResult = new JoinResult()
            {
              gameMode = initialMessage.gameMode,
              sessionState = new RunSessionState?(state),
              loadJoinResponse = new ClientLoadJoinResponseMessage?(joinResponseMessage2)
            };
            break;
          case RunSessionState.Running:
            ClientRejoinResponseMessage rejoinResponseMessage = await this.AttemptRejoin();
            joinResult = new JoinResult()
            {
              gameMode = initialMessage.gameMode,
              sessionState = new RunSessionState?(state),
              rejoinResponse = new ClientRejoinResponseMessage?(rejoinResponseMessage)
            };
            break;
          default:
            this.NetService.Disconnect(NetError.InternalError, true);
            throw new InvalidOperationException($"Received invalid state {state} from connection!");
        }
      }
      catch (Exception ex)
      {
        if (this.NetService.IsConnected)
          this.NetService.Disconnect(ex is OperationCanceledException ? NetError.CancelledJoin : NetError.InternalError);
        Logger.logLevelTypeMap[LogType.Network] = LogLevel.Info;
        Logger.logLevelTypeMap[LogType.Actions] = LogLevel.Info;
        Logger.logLevelTypeMap[LogType.GameSync] = LogLevel.Info;
        throw;
      }
      num = 1;
    }
    catch (object ex)
    {
      obj = ex;
    }
    await updateLoopCancelSource.CancelAsync();
    this.NetService.UnregisterMessageHandler<InitialGameInfoMessage>(new MessageHandlerDelegate<InitialGameInfoMessage>(this.HandleInitialGameInfoMessage));
    this.NetService.UnregisterMessageHandler<ClientLobbyJoinResponseMessage>(new MessageHandlerDelegate<ClientLobbyJoinResponseMessage>(this.HandleJoinResponseMessage));
    this.NetService.UnregisterMessageHandler<ClientLoadJoinResponseMessage>(new MessageHandlerDelegate<ClientLoadJoinResponseMessage>(this.HandleLoadJoinResponseMessage));
    this.NetService.UnregisterMessageHandler<ClientRejoinResponseMessage>(new MessageHandlerDelegate<ClientRejoinResponseMessage>(this.HandleRejoinResponseMessage));
    this.NetService.Disconnected -= new Action<NetErrorInfo>(this.OnDisconnected);
    object obj1 = obj;
    if (obj1 != null)
    {
      if (!(obj1 is Exception source))
        throw obj1;
      ExceptionDispatchInfo.Capture(source).Throw();
    }
    if (num == 1)
      return joinResult;
    obj = (object) null;
    joinResult = new JoinResult();
    updateLoopCancelSource = (CancellationTokenSource) null;
    JoinResult joinResult1;
    return joinResult1;
  }

  private async Task NetServiceUpdateLoop(CancellationTokenSource token, SceneTree sceneTree)
  {
    while (!token.IsCancellationRequested)
    {
      try
      {
        this.NetService.Update();
      }
      catch (Exception ex)
      {
        Log.Error(ex.ToString());
      }
      Variant[] signal = await ((GodotObject) sceneTree).ToSignal((GodotObject) sceneTree, SceneTree.SignalName.ProcessFrame);
    }
  }

  private async Task<ClientLobbyJoinResponseMessage> AttemptJoin()
  {
    this._joinCompletion = new TaskCompletionSource<ClientLobbyJoinResponseMessage>();
    this._logger.Info("Sending ClientLobbyJoinRequestMessage and waiting for response message");
    UnlockState stateFromProgress = SaveManager.Instance.GenerateUnlockStateFromProgress();
    this.NetService.SendMessage<ClientLobbyJoinRequestMessage>(new ClientLobbyJoinRequestMessage()
    {
      maxAscensionUnlocked = SaveManager.Instance.Progress.MaxMultiplayerAscension,
      unlockState = stateFromProgress.ToSerializable()
    });
    ClientLobbyJoinResponseMessage task = await this._joinCompletion.Task;
    this._logger.Info($"Received {"ClientLobbyJoinResponseMessage"}: {task}");
    return task;
  }

  private async Task<ClientLoadJoinResponseMessage> AttemptLoadJoin()
  {
    this._loadJoinCompletion = new TaskCompletionSource<ClientLoadJoinResponseMessage>();
    this._logger.Info("Sending ClientLoadJoinRequestMessage and waiting for rejoin response message");
    this.NetService.SendMessage<ClientLoadJoinRequestMessage>(new ClientLoadJoinRequestMessage());
    ClientLoadJoinResponseMessage task = await this._loadJoinCompletion.Task;
    this._logger.Info($"Received ClientLoadJoinResponseMessage: {task}");
    return task;
  }

  private async Task<ClientRejoinResponseMessage> AttemptRejoin()
  {
    this._rejoinCompletion = new TaskCompletionSource<ClientRejoinResponseMessage>();
    this._logger.Info("Sending ClientRequestRejoinMessage and waiting for rejoin response message");
    this.NetService.SendMessage<ClientRejoinRequestMessage>(new ClientRejoinRequestMessage());
    ClientRejoinResponseMessage task = await this._rejoinCompletion.Task;
    this._logger.Info($"Received ClientRejoinResponseMessage: {task}");
    return task;
  }

  private void HandleInitialGameInfoMessage(InitialGameInfoMessage message, ulong _)
  {
    if (this._connectCompletion == null || ((Task) this._connectCompletion.Task).IsCompleted)
      this._logger.Warn($"Received {"InitialGameInfoMessage"} when we weren't expecting it! Completion status: {this._connectCompletion}");
    else
      this._connectCompletion.SetResult(message);
  }

  private void HandleRejoinResponseMessage(ClientRejoinResponseMessage message, ulong senderId)
  {
    if (this._rejoinCompletion == null || ((Task) this._rejoinCompletion.Task).IsCompleted)
      this._logger.Warn($"Received {"ClientRejoinResponseMessage"} when we weren't expecting it! Completion status: {this._connectCompletion}");
    else
      this._rejoinCompletion.SetResult(message);
  }

  private void HandleLoadJoinResponseMessage(ClientLoadJoinResponseMessage message, ulong senderId)
  {
    if (this._loadJoinCompletion == null || ((Task) this._loadJoinCompletion.Task).IsCompleted)
      this._logger.Warn($"Received {"ClientLoadJoinResponseMessage"} when we weren't expecting it! Completion status: {this._connectCompletion}");
    else
      this._loadJoinCompletion.SetResult(message);
  }

  private void HandleJoinResponseMessage(ClientLobbyJoinResponseMessage message, ulong senderId)
  {
    if (this._joinCompletion == null || ((Task) this._joinCompletion.Task).IsCompleted)
      this._logger.Warn($"Received {"ClientLobbyJoinResponseMessage"} when we weren't expecting it! Completion status: {this._connectCompletion}");
    else
      this._joinCompletion.SetResult(message);
  }

  private void OnDisconnected(NetErrorInfo info)
  {
    this._logger.Info($"Disconnected during join flow, reason: {info.GetReason()}. Failing with an exception");
    ClientConnectionFailedException connectionFailedException = new ClientConnectionFailedException($"Unexpectedly disconnected from host while joining. Reason: {info.GetReason()}", info);
    TaskCompletionSource<InitialGameInfoMessage> connectCompletion = this._connectCompletion;
    if (connectCompletion != null)
    {
      Task<InitialGameInfoMessage> task = connectCompletion.Task;
      if (task != null && !((Task) task).IsCompleted)
        this._connectCompletion.SetException((Exception) connectionFailedException);
    }
    TaskCompletionSource<ClientLobbyJoinResponseMessage> joinCompletion = this._joinCompletion;
    if (joinCompletion != null)
    {
      Task<ClientLobbyJoinResponseMessage> task = joinCompletion.Task;
      if (task != null && !((Task) task).IsCompleted)
        this._joinCompletion?.SetException((Exception) connectionFailedException);
    }
    TaskCompletionSource<ClientLoadJoinResponseMessage> loadJoinCompletion = this._loadJoinCompletion;
    if (loadJoinCompletion != null)
    {
      Task<ClientLoadJoinResponseMessage> task = loadJoinCompletion.Task;
      if (task != null && !((Task) task).IsCompleted)
        this._loadJoinCompletion?.SetException((Exception) connectionFailedException);
    }
    TaskCompletionSource<ClientRejoinResponseMessage> rejoinCompletion = this._rejoinCompletion;
    if (rejoinCompletion == null)
      return;
    Task<ClientRejoinResponseMessage> task1 = rejoinCompletion.Task;
    if (task1 == null || ((Task) task1).IsCompleted)
      return;
    this._rejoinCompletion?.SetException((Exception) connectionFailedException);
  }

  private void Cancel()
  {
    TaskCompletionSource<InitialGameInfoMessage> connectCompletion = this._connectCompletion;
    if (connectCompletion != null)
    {
      Task<InitialGameInfoMessage> task = connectCompletion.Task;
      if (task != null && !((Task) task).IsCompleted)
        this._connectCompletion.SetCanceled();
    }
    TaskCompletionSource<ClientLobbyJoinResponseMessage> joinCompletion = this._joinCompletion;
    if (joinCompletion != null)
    {
      Task<ClientLobbyJoinResponseMessage> task = joinCompletion.Task;
      if (task != null && !((Task) task).IsCompleted)
        this._joinCompletion?.SetCanceled();
    }
    TaskCompletionSource<ClientLoadJoinResponseMessage> loadJoinCompletion = this._loadJoinCompletion;
    if (loadJoinCompletion != null)
    {
      Task<ClientLoadJoinResponseMessage> task = loadJoinCompletion.Task;
      if (task != null && !((Task) task).IsCompleted)
        this._loadJoinCompletion?.SetCanceled();
    }
    TaskCompletionSource<ClientRejoinResponseMessage> rejoinCompletion = this._rejoinCompletion;
    if (rejoinCompletion == null)
      return;
    Task<ClientRejoinResponseMessage> task1 = rejoinCompletion.Task;
    if (task1 == null || ((Task) task1).IsCompleted)
      return;
    this._rejoinCompletion?.SetCanceled();
  }

  public struct MockInfo
  {
    public string version;
    public uint hash;
    public PlatformBranch branch;
    public List<string>? gameplayAffectingMods;
    public List<string>? nonGameplayAffectingMods;
  }
}
