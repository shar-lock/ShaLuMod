// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Game.RewardsSetSynchronizer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Sync;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Game;

public class RewardsSetSynchronizer : IDisposable
{
  private readonly RunLocationTargetedMessageBuffer _messageBuffer;
  private readonly INetGameService _netService;
  private readonly IPlayerCollection _playerCollection;
  private readonly ulong _localPlayerId;
  private readonly Logger _logger = new Logger(nameof (RewardsSetSynchronizer), LogType.GameSync);
  private readonly List<RewardsSetSynchronizer.PlayerRewardState> _rewardStates = new List<RewardsSetSynchronizer.PlayerRewardState>();

  private Player LocalPlayer => this._playerCollection.GetPlayer(this._localPlayerId);

  public event Action? RewardsSkippedDuringRoomExit;

  public RewardsSetSynchronizer(
    RunLocationTargetedMessageBuffer messageBuffer,
    INetGameService netService,
    IPlayerCollection playerCollection,
    ulong localPlayerId)
  {
    this._netService = netService;
    this._playerCollection = playerCollection;
    this._localPlayerId = localPlayerId;
    for (int index = 0; index < playerCollection.Players.Count; ++index)
      this._rewardStates.Add(new RewardsSetSynchronizer.PlayerRewardState()
      {
        rewardsStack = new List<RewardsSetSynchronizer.RewardsSetState>(),
        bufferedMessages = new List<RewardsSetSynchronizer.BufferedMessage>()
      });
    this._messageBuffer = messageBuffer;
    this._messageBuffer.RegisterMessageHandler<RewardSelectedMessage>(new MessageHandlerDelegate<RewardSelectedMessage>(this.HandleRewardSelectedMessage));
    this._messageBuffer.RegisterMessageHandler<RewardSetSkippedMessage>(new MessageHandlerDelegate<RewardSetSkippedMessage>(this.HandleRewardSetSkippedMessage));
  }

  public void Dispose()
  {
    this._messageBuffer.UnregisterMessageHandler<RewardSelectedMessage>(new MessageHandlerDelegate<RewardSelectedMessage>(this.HandleRewardSelectedMessage));
    this._messageBuffer.UnregisterMessageHandler<RewardSetSkippedMessage>(new MessageHandlerDelegate<RewardSetSkippedMessage>(this.HandleRewardSetSkippedMessage));
  }

  private RewardsSetSynchronizer.PlayerRewardState GetRewardStateForPlayer(Player player)
  {
    for (int count = this._rewardStates.Count; count <= this._playerCollection.GetPlayerSlotIndex(player); ++count)
      this._rewardStates.Add(new RewardsSetSynchronizer.PlayerRewardState()
      {
        rewardsStack = new List<RewardsSetSynchronizer.RewardsSetState>(),
        bufferedMessages = new List<RewardsSetSynchronizer.BufferedMessage>()
      });
    return this._rewardStates[this._playerCollection.GetPlayerSlotIndex(player)];
  }

  public Task BeginRewardsSet(RewardsSet set)
  {
    RewardsSetSynchronizer.PlayerRewardState rewardStateForPlayer = this.GetRewardStateForPlayer(set.Player);
    set.Id = rewardStateForPlayer.nextId;
    ++rewardStateForPlayer.nextId;
    this._logger.Debug($"Beginning rewards set {set}");
    TaskCompletionSource completionSource = new TaskCompletionSource();
    RewardsSetSynchronizer.RewardsSetState setState = new RewardsSetSynchronizer.RewardsSetState()
    {
      set = set,
      completionSource = completionSource
    };
    rewardStateForPlayer.rewardsStack.Add(setState);
    foreach (RewardsSetSynchronizer.BufferedMessage bufferedMessage in rewardStateForPlayer.bufferedMessages.ToList<RewardsSetSynchronizer.BufferedMessage>())
    {
      if (set.Id == bufferedMessage.SetId)
      {
        if (bufferedMessage.selectedMessage != null)
        {
          this._logger.Debug("Handling buffered RewardSelectedMessage");
          this.HandleRewardSelectedMessage(bufferedMessage.selectedMessage, bufferedMessage.senderId);
        }
        else if (bufferedMessage.skippedMessage != null)
        {
          this._logger.Debug("Handling buffered RewardSetSkippedMessage");
          this.HandleRewardSetSkippedMessage(bufferedMessage.skippedMessage, bufferedMessage.senderId);
        }
        rewardStateForPlayer.bufferedMessages.Remove(bufferedMessage);
      }
    }
    this.CompleteRewardsSetIfNecessary(setState);
    return completionSource.Task;
  }

  public async Task<bool> SelectLocalReward(Reward reward)
  {
    if (reward.Player != this.LocalPlayer)
      throw new InvalidOperationException($"{nameof (SelectLocalReward)} called for reward {reward} with non-local player {reward.Player.NetId}! This is not allowed");
    RewardsSetSynchronizer.PlayerRewardState rewardStateForPlayer = this.GetRewardStateForPlayer(this.LocalPlayer);
    if (rewardStateForPlayer.rewardsStack.Count <= 0)
      throw new InvalidOperationException("Tried to sync reward for local player, but they are not currently viewing any reward set!");
    RewardsSetSynchronizer.RewardsSetState setState = rewardStateForPlayer.rewardsStack.Last<RewardsSetSynchronizer.RewardsSetState>();
    int num = setState.set.Rewards.IndexOf(reward);
    this._netService.SendMessage<RewardSelectedMessage>(new RewardSelectedMessage()
    {
      location = this._messageBuffer.CurrentLocation,
      setId = setState.set.Id,
      rewardIndex = num
    });
    return await this.SelectRewardForPlayer(setState, reward);
  }

  public void SkipLocalRewardsSet()
  {
    this._logger.Debug("Skipping local RewardsSet");
    RewardsSetSynchronizer.RewardsSetState rewardsSetState = this.SkipRewardsSetOnStackTopForPlayer(this.LocalPlayer);
    this._netService.SendMessage<RewardSetSkippedMessage>(new RewardSetSkippedMessage()
    {
      location = this._messageBuffer.CurrentLocation,
      setId = rewardsSetState.set.Id
    });
  }

  public void HandleRewardSelectedMessage(RewardSelectedMessage message, ulong senderId)
  {
    this._logger.Debug($"Received {"RewardSelectedMessage"} from player {senderId}, set id: {message.setId} reward index: {message.rewardIndex}");
    Player player = this._playerCollection.GetPlayer(senderId);
    RewardsSetSynchronizer.PlayerRewardState rewardStateForPlayer = this.GetRewardStateForPlayer(player);
    if (rewardStateForPlayer.nextId <= message.setId)
    {
      this._logger.Debug($"Buffering {"RewardSelectedMessage"} because RewardsSet id {message.setId} hasn't been created yet");
      rewardStateForPlayer.bufferedMessages.Add(new RewardsSetSynchronizer.BufferedMessage()
      {
        selectedMessage = message,
        senderId = senderId
      });
    }
    else
      TaskHelper.RunSafely(this.SelectRewardForPlayer(player, message.rewardIndex));
  }

  public void HandleRewardSetSkippedMessage(RewardSetSkippedMessage message, ulong senderId)
  {
    this._logger.Debug($"Received {"RewardSetSkippedMessage"} from player {senderId}, set id: {message.setId}");
    RewardsSetSynchronizer.PlayerRewardState rewardStateForPlayer = this.GetRewardStateForPlayer(this._playerCollection.GetPlayer(senderId));
    if (rewardStateForPlayer.nextId <= message.setId)
    {
      this._logger.Debug($"Buffering {"RewardSetSkippedMessage"} because RewardsSet id {message.setId} hasn't been created yet");
      rewardStateForPlayer.bufferedMessages.Add(new RewardsSetSynchronizer.BufferedMessage()
      {
        skippedMessage = message,
        senderId = senderId
      });
    }
    else
      this.SkipRewardsSetOnStackTopForPlayer(this._playerCollection.GetPlayer(senderId));
  }

  private async Task SelectRewardForPlayer(Player player, int rewardIndex)
  {
    RewardsSetSynchronizer.PlayerRewardState rewardStateForPlayer = this.GetRewardStateForPlayer(player);
    if (rewardStateForPlayer.rewardsStack.Count <= 0)
      throw new InvalidOperationException($"Tried to select reward for player {player.NetId}, but they are not currently viewing any reward set!");
    RewardsSetSynchronizer.RewardsSetState setState = rewardStateForPlayer.rewardsStack.Last<RewardsSetSynchronizer.RewardsSetState>();
    RewardsSet set = setState.set;
    if (rewardIndex < 0 || rewardIndex >= set.Rewards.Count)
      throw new InvalidOperationException($"Tried to select reward index {rewardIndex} for player {player.NetId}, but it is out of bounds in their current rewards set {set}!");
    Reward reward = set.Rewards[rewardIndex];
    int num = await this.SelectRewardForPlayer(setState, reward) ? 1 : 0;
  }

  private async Task<bool> SelectRewardForPlayer(
    RewardsSetSynchronizer.RewardsSetState setState,
    Reward reward)
  {
    this._logger.Debug($"Selecting reward {reward} for player {reward.Player.NetId}");
    bool flag = await reward.SelectUnsynchronized();
    this.CompleteRewardsSetIfNecessary(setState);
    return flag;
  }

  private void CompleteRewardsSetIfNecessary(RewardsSetSynchronizer.RewardsSetState setState)
  {
    if (!setState.set.AllRewardsSuccessfullySelected)
      return;
    this.CompleteRewardsSet(setState, RewardsSetSynchronizer.RewardSetCompleteState.Completed);
  }

  private RewardsSetSynchronizer.RewardsSetState SkipRewardsSetOnStackTopForPlayer(Player player)
  {
    RewardsSetSynchronizer.PlayerRewardState rewardStateForPlayer = this.GetRewardStateForPlayer(player);
    if (rewardStateForPlayer.rewardsStack.Count <= 0)
      throw new InvalidOperationException($"Tried to skip reward set for player {player.NetId}, but they are not currently viewing any reward set!");
    List<RewardsSetSynchronizer.RewardsSetState> rewardsStack = rewardStateForPlayer.rewardsStack;
    RewardsSetSynchronizer.RewardsSetState setState = rewardsStack[rewardsStack.Count - 1];
    this.SkipRewardsSet(setState);
    return setState;
  }

  private void SkipRewardsSet(RewardsSetSynchronizer.RewardsSetState setState)
  {
    foreach (Reward reward in setState.set.Rewards)
    {
      if (!reward.SuccessfullySelected)
        reward.OnSkipped();
    }
    this.CompleteRewardsSet(setState, RewardsSetSynchronizer.RewardSetCompleteState.Skipped);
  }

  private void CompleteRewardsSet(
    RewardsSetSynchronizer.RewardsSetState setState,
    RewardsSetSynchronizer.RewardSetCompleteState completeState)
  {
    RewardsSetSynchronizer.PlayerRewardState rewardStateForPlayer = this.GetRewardStateForPlayer(setState.set.Player);
    if (this.IsRewardsSetCompleted(setState.set))
    {
      this._logger.Error($"Reward set with id {setState.set} is already finished (state {rewardStateForPlayer.completedRewards[setState.set.Id]})!");
    }
    else
    {
      rewardStateForPlayer.rewardsStack.Remove(setState);
      rewardStateForPlayer.completedRewards[setState.set.Id] = completeState;
      setState.completionSource.SetResult();
      this._logger.Debug($"Reward set {setState.set} completed with state: {completeState}");
    }
  }

  public bool IsRewardsSetCompleted(RewardsSet set)
  {
    RewardsSetSynchronizer.PlayerRewardState rewardStateForPlayer = this.GetRewardStateForPlayer(set.Player);
    return set.Id >= 0 && rewardStateForPlayer.completedRewards.ContainsKey(set.Id);
  }

  public bool IsRewardsSetCompleted(Player player, int id)
  {
    return this.GetRewardStateForPlayer(player).completedRewards.ContainsKey(id);
  }

  public void BeforeLeavingRoom()
  {
    RewardsSetSynchronizer.PlayerRewardState rewardStateForPlayer = this.GetRewardStateForPlayer(this.LocalPlayer);
    if (rewardStateForPlayer.rewardsStack.Count > 0)
      this._logger.Debug($"Skipping remaining rewards for local player {this.LocalPlayer.NetId} because we're exiting the room");
    for (int index = rewardStateForPlayer.rewardsStack.Count - 1; index >= 0; --index)
    {
      RewardsSetSynchronizer.RewardsSetState rewards = rewardStateForPlayer.rewardsStack[index];
      this.SkipRewardsSet(rewardStateForPlayer.rewardsStack[index]);
      this._netService.SendMessage<RewardSetSkippedMessage>(new RewardSetSkippedMessage()
      {
        location = this._messageBuffer.CurrentLocation,
        setId = rewards.set.Id
      });
    }
    rewardStateForPlayer.rewardsStack.Clear();
    Action skippedDuringRoomExit = this.RewardsSkippedDuringRoomExit;
    if (skippedDuringRoomExit == null)
      return;
    skippedDuringRoomExit();
  }

  public IEnumerable<int> GetNextRewardIds()
  {
    foreach (RewardsSetSynchronizer.PlayerRewardState rewardState in this._rewardStates)
      yield return rewardState.nextId;
  }

  public void FastForwardRewardIds(List<int> rewardIds)
  {
    for (int index = 0; index < rewardIds.Count; ++index)
    {
      this._logger.Debug($"Fast-forwarded reward set ID for {index} to {rewardIds[index]}");
      this._rewardStates[index].nextId = rewardIds[index];
    }
  }

  private enum RewardSetCompleteState
  {
    None,
    Completed,
    Skipped,
  }

  private record struct BufferedMessage
  {
    public RewardSelectedMessage? selectedMessage;
    public RewardSetSkippedMessage? skippedMessage;
    public ulong senderId;

    public int SetId
    {
      get
      {
        RewardSelectedMessage selectedMessage = this.selectedMessage;
        if (selectedMessage != null)
          return selectedMessage.setId;
        return (this.skippedMessage ?? throw new InvalidOperationException()).setId;
      }
    }

    [CompilerGenerated]
    public override readonly int GetHashCode()
    {
      return (EqualityComparer<RewardSelectedMessage>.Default.GetHashCode(this.selectedMessage) * -1521134295 + EqualityComparer<RewardSetSkippedMessage>.Default.GetHashCode(this.skippedMessage)) * -1521134295 + EqualityComparer<ulong>.Default.GetHashCode(this.senderId);
    }

    [CompilerGenerated]
    public readonly bool Equals(RewardsSetSynchronizer.BufferedMessage other)
    {
      return EqualityComparer<RewardSelectedMessage>.Default.Equals(this.selectedMessage, other.selectedMessage) && EqualityComparer<RewardSetSkippedMessage>.Default.Equals(this.skippedMessage, other.skippedMessage) && EqualityComparer<ulong>.Default.Equals(this.senderId, other.senderId);
    }
  }

  private class PlayerRewardState
  {
    public required List<RewardsSetSynchronizer.RewardsSetState> rewardsStack;
    public required List<RewardsSetSynchronizer.BufferedMessage> bufferedMessages;
    public readonly Dictionary<int, RewardsSetSynchronizer.RewardSetCompleteState> completedRewards = new Dictionary<int, RewardsSetSynchronizer.RewardSetCompleteState>();
    public int nextId;
  }

  private class RewardsSetState
  {
    public required RewardsSet set;
    public required TaskCompletionSource completionSource;
  }
}
