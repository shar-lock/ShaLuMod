// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Game.ChecksumTracker
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Checksums;
using MegaCrit.Sts2.Core.Multiplayer.Replay;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;
using Sentry;
using System;
using System.Collections.Generic;
using System.IO.Hashing;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Game;

public class ChecksumTracker : IDisposable
{
  private const int _checksumsToSave = 20;
  private readonly List<ChecksumTracker.TrackedChecksum> _checksums = new List<ChecksumTracker.TrackedChecksum>();
  private readonly List<ChecksumTracker.QueuedRemoteChecksum> _queuedRemoteChecksums = new List<ChecksumTracker.QueuedRemoteChecksum>();
  private readonly INetGameService _netService;
  private readonly IRunState _runState;
  private readonly PacketWriter _packetWriter = new PacketWriter();
  private readonly Logger _logger = new Logger(nameof (ChecksumTracker), LogType.GameSync);
  private List<ReplayChecksumData>? _replayChecksums;

  public uint NextId { get; private set; }

  public bool IsEnabled { get; set; } = true;

  public event Action<NetFullCombatState>? StateDiverged;

  public event Action<NetChecksumData, string, NetFullCombatState>? ChecksumGenerated;

  public ChecksumTracker(INetGameService netService, IRunState runState)
  {
    this._netService = netService;
    this._runState = runState;
    this._netService.RegisterMessageHandler<ChecksumDataMessage>(new MessageHandlerDelegate<ChecksumDataMessage>(this.OnReceivedChecksumDataMessage));
    this._netService.RegisterMessageHandler<StateDivergenceMessage>(new MessageHandlerDelegate<StateDivergenceMessage>(this.OnReceivedStateDivergenceMessage));
  }

  public void Dispose()
  {
    this._netService.UnregisterMessageHandler<ChecksumDataMessage>(new MessageHandlerDelegate<ChecksumDataMessage>(this.OnReceivedChecksumDataMessage));
    this._netService.UnregisterMessageHandler<StateDivergenceMessage>(new MessageHandlerDelegate<StateDivergenceMessage>(this.OnReceivedStateDivergenceMessage));
  }

  public NetChecksumData GenerateChecksum(string context, GameAction? action)
  {
    if (!this.IsEnabled)
      return new NetChecksumData();
    this._logger.Debug($"Generating checksum for context: {context} action: {action} id: {this.NextId}");
    NetChecksumData andTrackChecksum = this.ObtainAndTrackChecksum(context, action);
    if (this._netService.Type == NetGameType.Client)
      this._netService.SendMessage<ChecksumDataMessage>(new ChecksumDataMessage()
      {
        checksumData = andTrackChecksum
      });
    this.CheckAgainstReplayChecksum(andTrackChecksum, context);
    return andTrackChecksum;
  }

  private void OnReceivedChecksumDataMessage(ChecksumDataMessage message, ulong senderId)
  {
    if (this._netService.Type != NetGameType.Host)
      throw new InvalidOperationException("Received ChecksumDataMessage as non-host player!");
    NetChecksumData remoteChecksumData = message.checksumData;
    int index = this._checksums.FindIndex((Predicate<ChecksumTracker.TrackedChecksum>) (c => (int) c.data.id == (int) remoteChecksumData.id));
    if (index < 0)
      this._queuedRemoteChecksums.Add(new ChecksumTracker.QueuedRemoteChecksum()
      {
        data = remoteChecksumData,
        senderId = senderId
      });
    else
      this.CompareChecksums(this._checksums[index], remoteChecksumData, senderId);
  }

  private void OnReceivedStateDivergenceMessage(StateDivergenceMessage message, ulong senderId)
  {
    NetChecksumData remoteChecksumData = message.senderChecksum;
    int index = this._checksums.FindIndex((Predicate<ChecksumTracker.TrackedChecksum>) (c => (int) c.data.id == (int) remoteChecksumData.id));
    if (index < 0)
      Log.Error($"Received state divergence message for checksum ID {remoteChecksumData.id} that has fallen out of our list! (Our next ID: {this.NextId})");
    else
      this.LogStateDivergence(this._checksums[index], message, senderId, index);
    if (this._netService.Type != NetGameType.Host)
      return;
    if (this._netService is NetHostGameService netService)
    {
      // ISSUE: explicit non-virtual call
      __nonvirtual (netService.DisconnectClient(senderId, NetError.StateDivergence, false));
    }
    NErrorPopup modalToCreate = NErrorPopup.Create(new NetErrorInfo(NetError.StateDivergence, true));
    if (modalToCreate == null)
      return;
    NModalContainer.Instance?.Add((Node) modalToCreate);
  }

  private NetChecksumData ObtainAndTrackChecksum(string context, GameAction? action)
  {
    NetFullCombatState state = NetFullCombatState.FromRun(this._runState, action);
    uint checksum = this.GenerateChecksum(state);
    NetChecksumData andTrackChecksum = new NetChecksumData()
    {
      id = this.NextId,
      checksum = checksum
    };
    ++this.NextId;
    ChecksumTracker.TrackedChecksum localChecksum = new ChecksumTracker.TrackedChecksum()
    {
      data = andTrackChecksum,
      context = context,
      fingerprintContext = action != null ? action.GetType().Name : context,
      fullState = state
    };
    Action<NetChecksumData, string, NetFullCombatState> checksumGenerated = this.ChecksumGenerated;
    if (checksumGenerated != null)
      checksumGenerated(andTrackChecksum, context, state);
    this._checksums.Add(localChecksum);
    if (this._checksums.Count > 20)
      this._checksums.RemoveAt(0);
    for (int index = 0; index < this._queuedRemoteChecksums.Count; ++index)
    {
      if ((int) this._queuedRemoteChecksums[index].data.id == (int) localChecksum.data.id)
      {
        this.CompareChecksums(localChecksum, this._queuedRemoteChecksums[index].data, this._queuedRemoteChecksums[index].senderId);
        this._queuedRemoteChecksums.RemoveAt(index);
        --index;
      }
    }
    return andTrackChecksum;
  }

  private void CompareChecksums(
    ChecksumTracker.TrackedChecksum localChecksum,
    NetChecksumData remoteChecksum,
    ulong remoteId)
  {
    if ((int) localChecksum.data.id != (int) remoteChecksum.id)
      throw new InvalidOperationException("Trying to compare two checksums with different IDs!");
    if (this._netService.Type != NetGameType.Host)
      throw new InvalidOperationException("CompareChecksums should only be called on the host!");
    if ((int) localChecksum.data.checksum == (int) remoteChecksum.checksum)
      return;
    if (!TestMode.IsOn)
      Log.Error($"State divergence detected! Checksum with ID {localChecksum.data.id} for client {remoteId} doesn't match host's!\nContext: {localChecksum.context}. Local: {localChecksum.data.checksum}. Remote: {remoteChecksum.checksum}.");
    this._netService.SendMessage<StateDivergenceMessage>(new StateDivergenceMessage()
    {
      senderChecksum = localChecksum.data,
      senderCombatState = localChecksum.fullState
    }, remoteId);
  }

  private void LogStateDivergence(
    ChecksumTracker.TrackedChecksum localChecksum,
    StateDivergenceMessage message,
    ulong remoteId,
    int checksumIndex)
  {
    if ((int) localChecksum.data.id != (int) message.senderChecksum.id)
      throw new InvalidOperationException("Trying to compare two checksums with different IDs!");
    if (!TestMode.IsOn)
    {
      Log.Error($"State divergence message received for player {remoteId} checksum ID {localChecksum.data.id}! (We are {this._netService.Type} {this._netService.NetId})\nContext: {localChecksum.context}. Local: {localChecksum.data.checksum}. Remote: {message.senderChecksum.checksum}.\nLOCAL STATE DUMP\n{localChecksum.fullState}\nREMOTE STATE DUMP\n{message.senderCombatState}\n");
      this.ReportDivergenceToSentry(localChecksum, message, remoteId, checksumIndex);
    }
    if (this._netService.Type != NetGameType.Client)
      return;
    this._netService.SendMessage<StateDivergenceMessage>(new StateDivergenceMessage()
    {
      senderChecksum = localChecksum.data,
      senderCombatState = localChecksum.fullState
    });
    Action<NetFullCombatState> stateDiverged = this.StateDiverged;
    if (stateDiverged == null)
      return;
    stateDiverged(message.senderCombatState);
  }

  private void ReportDivergenceToSentry(
    ChecksumTracker.TrackedChecksum localChecksum,
    StateDivergenceMessage message,
    ulong remoteId,
    int checksumIndex)
  {
    string role = this._netService.Type.ToString();
    string message1 = "Multiplayer state divergence: " + localChecksum.fingerprintContext;
    string localState = localChecksum.fullState.ToString();
    string remoteState = message.senderCombatState.ToString();
    SentryService.CaptureException((Exception) new StateDivergenceException(message1), (Action<Scope>) (scope =>
    {
      EventLikeExtensions.SetFingerprint((IEventLike) scope, new string[2]
      {
        "StateDivergence",
        localChecksum.fingerprintContext
      });
      scope.SetTag("net.role", role);
      scope.SetTag("divergence.type", localChecksum.fingerprintContext);
      scope.SetExtra("divergence.context", (object) localChecksum.context);
      scope.SetExtra("divergence.checksum_id", (object) localChecksum.data.id);
      scope.SetExtra("divergence.local_checksum", (object) localChecksum.data.checksum);
      scope.SetExtra("divergence.remote_checksum", (object) message.senderChecksum.checksum);
      scope.SetExtra("divergence.local_net_id", (object) this._netService.NetId);
      scope.SetExtra("divergence.remote_net_id", (object) remoteId);
      scope.SetExtra("divergence.lobby_id", (object) (this._netService.GetRawLobbyIdentifier() ?? "unknown"));
      scope.AddCompressedAttachment(localState, "local_state.txt.gz");
      scope.AddCompressedAttachment(remoteState, "remote_state.txt.gz");
      if (checksumIndex <= 0)
        return;
      string text = this._checksums[checksumIndex - 1].fullState.ToString();
      scope.AddCompressedAttachment(text, "previous_state.txt.gz");
    }));
  }

  public void LoadReplayChecksums(List<ReplayChecksumData> replayChecksums, uint nextId)
  {
    this.NextId = nextId;
    this._replayChecksums = replayChecksums;
  }

  private void CheckAgainstReplayChecksum(NetChecksumData localData, string context)
  {
    if (this._replayChecksums == null)
      return;
    int index = this._replayChecksums.FindIndex((Predicate<ReplayChecksumData>) (c => (int) c.checksumData.id == (int) localData.id));
    if (index == -1)
    {
      Log.Error($"Replay state diverged! Generated checksum for {context} with id {localData.id} that doesn't exist in the replay data");
    }
    else
    {
      ReplayChecksumData replayChecksum = this._replayChecksums[index];
      uint checksum = this.GenerateChecksum(replayChecksum.fullState);
      if ((int) checksum == (int) localData.checksum)
        return;
      Log.Error($"Replay state divergence! Checksum ID {localData.id}!\nLocal context: {context}. Replay context: {replayChecksum.context}. Local: {localData.checksum}. Replay: {checksum}.\nLOCAL STATE DUMP\n{this._checksums.Find((Predicate<ChecksumTracker.TrackedChecksum>) (c => (int) c.data.id == (int) localData.id)).fullState}\nREPLAY STATE DUMP\n{replayChecksum.fullState}\n");
    }
  }

  public uint GenerateChecksum(NetFullCombatState state)
  {
    this._packetWriter.Reset();
    this._packetWriter.Write<NetFullCombatState>(state);
    this._packetWriter.ZeroByteRemainder();
    return XxHash32.HashToUInt32(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(this._packetWriter.Buffer).Slice(0, this._packetWriter.BytePosition)), 0);
  }

  private struct TrackedChecksum
  {
    public NetChecksumData data;
    public string context;
    public string fingerprintContext;
    public NetFullCombatState fullState;
  }

  private struct QueuedRemoteChecksum
  {
    public NetChecksumData data;
    public ulong senderId;
  }
}
