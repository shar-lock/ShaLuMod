// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceSynchronizer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Game;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.GameActions.Multiplayer;

public class PlayerChoiceSynchronizer : IDisposable
{
  private readonly List<uint> _choiceIds = new List<uint>();
  private readonly List<PlayerChoiceSynchronizer.ReceivedChoice> _receivedChoices = new List<PlayerChoiceSynchronizer.ReceivedChoice>();
  private readonly Logger _logger = new Logger(nameof (PlayerChoiceSynchronizer), LogType.Actions);
  private readonly INetGameService _netService;
  private readonly IPlayerCollection _players;

  public IReadOnlyList<uint> ChoiceIds => (IReadOnlyList<uint>) this._choiceIds;

  public event Action<Player, uint, NetPlayerChoiceResult>? PlayerChoiceReceived;

  public PlayerChoiceSynchronizer(INetGameService netService, IPlayerCollection players)
  {
    this._players = players;
    this._netService = netService;
    netService.RegisterMessageHandler<PlayerChoiceMessage>(new MessageHandlerDelegate<PlayerChoiceMessage>(this.OnPlayerChoiceMessageReceived));
  }

  public void Dispose()
  {
    this._netService.UnregisterMessageHandler<PlayerChoiceMessage>(new MessageHandlerDelegate<PlayerChoiceMessage>(this.OnPlayerChoiceMessageReceived));
  }

  public uint ReserveChoiceId(Player player)
  {
    int playerSlotIndex = this._players.GetPlayerSlotIndex(player);
    while (this._choiceIds.Count <= playerSlotIndex)
      this._choiceIds.Add(0U);
    uint num = this._choiceIds[playerSlotIndex]++;
    this._logger.VeryDebug($"Reserved choice id {num} for player {player.NetId}, next is {this._choiceIds[playerSlotIndex]}");
    return num;
  }

  public void SyncLocalChoice(Player player, uint choiceId, PlayerChoiceResult result)
  {
    PlayerChoiceMessage message = this.ValidateChoiceId(player, choiceId) ? new PlayerChoiceMessage()
    {
      result = result.ToNetData(),
      choiceId = choiceId
    } : throw new InvalidOperationException($"Tried to sync local choice with ID {choiceId} for player {player.NetId}, but player's choice ID is {this.GetChoiceId(player)}!");
    this._logger.Debug($"Sending player choice id {choiceId} for player {player.NetId}, result {result}");
    Action<Player, uint, NetPlayerChoiceResult> playerChoiceReceived = this.PlayerChoiceReceived;
    if (playerChoiceReceived != null)
      playerChoiceReceived(player, choiceId, message.result);
    this._netService.SendMessage<PlayerChoiceMessage>(message);
  }

  public async Task<PlayerChoiceResult> WaitForRemoteChoice(Player player, uint choiceId)
  {
    if (this._netService.Type == NetGameType.Singleplayer)
      throw new InvalidOperationException("Cannot wait for remote choice in singleplayer!");
    if (!this.ValidateChoiceId(player, choiceId))
      throw new InvalidOperationException($"Tried to wait for remote choice with ID {choiceId} for player {player.NetId}, but player's choice ID is {this.GetChoiceId(player)}!");
    int index = this._receivedChoices.FindIndex((Predicate<PlayerChoiceSynchronizer.ReceivedChoice>) (c => (int) c.choiceId == (int) choiceId && (long) c.senderId == (long) player.NetId));
    PlayerChoiceSynchronizer.ReceivedChoice receivedChoice;
    if (index >= 0)
    {
      this._logger.Debug($"Was going to wait for remote choice {choiceId} for player {player.NetId} but we've already received it");
      receivedChoice = this._receivedChoices[index];
      this._receivedChoices.RemoveAt(index);
    }
    else
    {
      receivedChoice = new PlayerChoiceSynchronizer.ReceivedChoice()
      {
        choiceId = choiceId,
        senderId = player.NetId,
        completionSource = new TaskCompletionSource<NetPlayerChoiceResult>()
      };
      this._logger.Debug($"Awaiting remote choice {choiceId} for player {player.NetId}");
      this._receivedChoices.Add(receivedChoice);
    }
    NetPlayerChoiceResult task = await receivedChoice.completionSource.Task;
    PlayerChoiceResult playerChoiceResult = PlayerChoiceResult.FromNetData(player, this._players, task);
    this._logger.Debug($"Finished waiting for remote choice {choiceId} for player {player.NetId}: {playerChoiceResult}");
    return playerChoiceResult;
  }

  private void OnPlayerChoiceMessageReceived(PlayerChoiceMessage message, ulong senderId)
  {
    this._logger.Debug($"Received choice from {senderId} for choice ID {message.choiceId}: {message.result}");
    this.OnReceivePlayerChoice(this._players.GetPlayer(senderId), message.choiceId, message.result);
  }

  public void FastForwardChoiceIds(List<uint> choiceIds)
  {
    this._logger.Debug("Fast-forwarded choice IDs to: " + string.Join<uint>(",", (IEnumerable<uint>) choiceIds));
    this._choiceIds.Clear();
    this._choiceIds.AddRange((IEnumerable<uint>) choiceIds);
  }

  public void ReceiveReplayChoice(Player player, uint choiceId, NetPlayerChoiceResult result)
  {
    this.OnReceivePlayerChoice(player, choiceId, result);
  }

  private void OnReceivePlayerChoice(Player player, uint choiceId, NetPlayerChoiceResult result)
  {
    Action<Player, uint, NetPlayerChoiceResult> playerChoiceReceived = this.PlayerChoiceReceived;
    if (playerChoiceReceived != null)
      playerChoiceReceived(player, choiceId, result);
    int index = this._receivedChoices.FindIndex((Predicate<PlayerChoiceSynchronizer.ReceivedChoice>) (c => (int) c.choiceId == (int) choiceId && (long) c.senderId == (long) player.NetId));
    PlayerChoiceSynchronizer.ReceivedChoice receivedChoice;
    if (index >= 0)
    {
      this._logger.Debug("We are already waiting for the choice, fulfilling the task");
      receivedChoice = this._receivedChoices[index];
      this._receivedChoices.RemoveAt(index);
    }
    else
    {
      receivedChoice = new PlayerChoiceSynchronizer.ReceivedChoice()
      {
        choiceId = choiceId,
        senderId = player.NetId,
        completionSource = new TaskCompletionSource<NetPlayerChoiceResult>()
      };
      this._logger.Debug("We are not yet waiting for the choice, creating a new received choice");
      this._receivedChoices.Add(receivedChoice);
    }
    receivedChoice.completionSource.SetResult(result);
  }

  private bool ValidateChoiceId(Player player, uint choiceId)
  {
    return choiceId < this.GetChoiceId(player);
  }

  private uint GetChoiceId(Player player)
  {
    int playerSlotIndex = this._players.GetPlayerSlotIndex(player);
    return playerSlotIndex >= this._choiceIds.Count ? 0U : this._choiceIds[playerSlotIndex];
  }

  private struct ReceivedChoice
  {
    public ulong senderId;
    public uint choiceId;
    public TaskCompletionSource<NetPlayerChoiceResult> completionSource;
  }
}
