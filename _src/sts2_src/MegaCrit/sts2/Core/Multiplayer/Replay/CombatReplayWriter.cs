// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Replay.CombatReplayWriter
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Saves;
using System;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Replay;

public class CombatReplayWriter : IDisposable
{
  private CombatReplay? _replay;
  private readonly PacketWriter _writer = new PacketWriter();
  private readonly ActionQueueSet _actionQueueSet;
  private readonly ActionQueueSynchronizer _actionQueueSynchronizer;
  private readonly PlayerChoiceSynchronizer _playerChoiceSynchronizer;
  private readonly RewardsSetSynchronizer _rewardsSetSynchronizer;
  private readonly ChecksumTracker _checksumTracker;

  public bool IsEnabled { get; set; } = true;

  public bool IsRecordingReplay => this._replay != null;

  public CombatReplayWriter(
    PlayerChoiceSynchronizer playerChoiceSynchronizer,
    RewardsSetSynchronizer rewardsSetSynchronizer,
    ActionQueueSet actionQueueSet,
    ActionQueueSynchronizer actionQueueSynchronizer,
    ChecksumTracker checksumTracker)
  {
    this._actionQueueSet = actionQueueSet;
    this._actionQueueSynchronizer = actionQueueSynchronizer;
    this._playerChoiceSynchronizer = playerChoiceSynchronizer;
    this._rewardsSetSynchronizer = rewardsSetSynchronizer;
    this._checksumTracker = checksumTracker;
    actionQueueSet.ActionEnqueued += new Action<GameAction>(this.RecordGameAction);
    actionQueueSet.ActionResumed += new Action<uint>(this.RecordActionResume);
    playerChoiceSynchronizer.PlayerChoiceReceived += new Action<Player, uint, NetPlayerChoiceResult>(this.RecordPlayerChoice);
    checksumTracker.ChecksumGenerated += new Action<NetChecksumData, string, NetFullCombatState>(this.RecordChecksum);
  }

  public void Dispose()
  {
    this._actionQueueSet.ActionEnqueued -= new Action<GameAction>(this.RecordGameAction);
    this._actionQueueSet.ActionResumed -= new Action<uint>(this.RecordActionResume);
    this._playerChoiceSynchronizer.PlayerChoiceReceived -= new Action<Player, uint, NetPlayerChoiceResult>(this.RecordPlayerChoice);
    this._checksumTracker.ChecksumGenerated -= new Action<NetChecksumData, string, NetFullCombatState>(this.RecordChecksum);
  }

  public void RecordInitialState(SerializableRun serializableRun)
  {
    if (!this.IsEnabled)
      return;
    this._replay = new CombatReplay()
    {
      version = ReleaseInfoManager.Instance.ReleaseInfo?.Version ?? "UNRELEASED",
      gitCommit = ReleaseInfoManager.Instance.ReleaseInfo?.Commit ?? GitHelper.ShortCommitId ?? "UNKNOWN",
      modelIdHash = ModelIdSerializationCache.Hash,
      choiceIds = this._playerChoiceSynchronizer.ChoiceIds.ToList<uint>(),
      rewardIds = this._rewardsSetSynchronizer.GetNextRewardIds().ToList<int>(),
      nextActionId = this._actionQueueSet.NextActionId,
      nextChecksumId = this._checksumTracker.NextId,
      nextHookId = this._actionQueueSynchronizer.NextHookId,
      serializableRun = serializableRun
    };
    this._replay.events.Clear();
  }

  private void RecordGameAction(GameAction gameAction)
  {
    if (!this.IsEnabled || !CombatManager.Instance.IsInProgress)
      return;
    if (this._replay == null)
      throw new InvalidOperationException("RecordInitialState must be called first");
    if (gameAction is GenericHookGameAction genericHookGameAction)
    {
      this._replay.events.Add(new CombatReplayEvent()
      {
        playerId = new ulong?(gameAction.OwnerId),
        eventType = CombatReplayEventType.HookAction,
        hookId = new uint?(genericHookGameAction.HookId),
        gameActionType = new GameActionType?(genericHookGameAction.ActionType)
      });
    }
    else
    {
      if (!gameAction.RecordableToReplay)
        throw new InvalidOperationException($"Found unrecordable game action: {gameAction}");
      this._replay.events.Add(new CombatReplayEvent()
      {
        playerId = new ulong?(gameAction.OwnerId),
        eventType = CombatReplayEventType.GameAction,
        action = gameAction.ToNetAction()
      });
    }
  }

  private void RecordActionResume(uint actionId)
  {
    if (!this.IsEnabled || !CombatManager.Instance.IsInProgress)
      return;
    if (this._replay == null)
      throw new InvalidOperationException("RecordInitialState must be called first");
    this._replay.events.Add(new CombatReplayEvent()
    {
      eventType = CombatReplayEventType.ResumeAction,
      actionId = new uint?(actionId)
    });
  }

  private void RecordPlayerChoice(Player player, uint choiceId, NetPlayerChoiceResult result)
  {
    if (!this.IsEnabled || !CombatManager.Instance.IsInProgress)
      return;
    if (this._replay == null)
      throw new InvalidOperationException("RecordInitialState must be called first");
    this._replay.events.Add(new CombatReplayEvent()
    {
      eventType = CombatReplayEventType.PlayerChoice,
      playerId = new ulong?(player.NetId),
      choiceId = new uint?(choiceId),
      playerChoiceResult = new NetPlayerChoiceResult?(result)
    });
  }

  private void RecordChecksum(
    NetChecksumData checksum,
    string context,
    NetFullCombatState fullCombatState)
  {
    if (!this.IsEnabled || !CombatManager.Instance.IsInProgress)
      return;
    if (this._replay == null)
      throw new InvalidOperationException("RecordInitialState must be called first");
    this._replay.checksumData.Add(new ReplayChecksumData()
    {
      checksumData = checksum,
      context = context,
      fullState = fullCombatState
    });
  }

  public void WriteReplay(string filePath, bool stopRecording)
  {
    if (!this.IsEnabled)
      return;
    if (this._replay == null)
      throw new InvalidOperationException("RecordInitialState must be called first");
    this._writer.Reset();
    this._writer.Write<CombatReplay>(this._replay.Anonymized());
    try
    {
      DirAccess.MakeDirRecursiveAbsolute(filePath.Substring(0, filePath.LastIndexOf('/')));
      using (FileAccessStream fileAccessStream = new FileAccessStream(filePath, (FileAccess.ModeFlags) 2L))
        fileAccessStream.Write(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(this._writer.Buffer).Slice(0, this._writer.BytePosition)));
    }
    catch (Exception ex)
    {
      Log.Warn($"Exception while writing replay: {ex}");
    }
    if (!stopRecording)
      return;
    this.StopRecording();
  }

  public void StopRecording() => this._replay = (CombatReplay) null;
}
