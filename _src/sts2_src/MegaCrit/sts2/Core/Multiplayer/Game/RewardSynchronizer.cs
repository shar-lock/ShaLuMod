// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Game.RewardSynchronizer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Game;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Sync;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Game;

public class RewardSynchronizer : IDisposable
{
  private readonly RunLocationTargetedMessageBuffer _messageBuffer;
  private readonly INetGameService _gameService;
  private readonly IPlayerCollection _playerCollection;
  private readonly ulong _localPlayerId;
  private readonly List<RewardSynchronizer.BufferedMessage> _bufferedMessages = new List<RewardSynchronizer.BufferedMessage>();

  private Player LocalPlayer => this._playerCollection.GetPlayer(this._localPlayerId);

  public RewardSynchronizer(
    RunLocationTargetedMessageBuffer messageBuffer,
    INetGameService gameService,
    IPlayerCollection playerCollection,
    ulong localPlayerId)
  {
    this._gameService = gameService;
    this._playerCollection = playerCollection;
    this._localPlayerId = localPlayerId;
    this._messageBuffer = messageBuffer;
    this._messageBuffer.RegisterMessageHandler<RewardObtainedMessage>(new MessageHandlerDelegate<RewardObtainedMessage>(this.HandleRewardObtainedMessage));
    this._messageBuffer.RegisterMessageHandler<GoldLostMessage>(new MessageHandlerDelegate<GoldLostMessage>(this.HandleGoldLostMessage));
    this._messageBuffer.RegisterMessageHandler<CardRemovedMessage>(new MessageHandlerDelegate<CardRemovedMessage>(this.HandleCardRemovedMessage));
    CombatManager.Instance.CombatEnded += new Action<CombatRoom>(this.OnCombatEnded);
  }

  public void Dispose()
  {
    this._messageBuffer.UnregisterMessageHandler<RewardObtainedMessage>(new MessageHandlerDelegate<RewardObtainedMessage>(this.HandleRewardObtainedMessage));
    this._messageBuffer.UnregisterMessageHandler<GoldLostMessage>(new MessageHandlerDelegate<GoldLostMessage>(this.HandleGoldLostMessage));
    this._messageBuffer.UnregisterMessageHandler<CardRemovedMessage>(new MessageHandlerDelegate<CardRemovedMessage>(this.HandleCardRemovedMessage));
    CombatManager.Instance.CombatEnded -= new Action<CombatRoom>(this.OnCombatEnded);
  }

  public void SyncLocalObtainedCard(CardModel card) => this.SyncLocalCardEvent(card, false);

  public void SyncLocalSkippedCard(CardModel card) => this.SyncLocalCardEvent(card, true);

  private void SyncLocalCardEvent(CardModel card, bool skipped)
  {
    if (!RunManager.Instance.IsSingleplayerOrFakeMultiplayer && CombatManager.Instance.IsInProgress)
      throw new InvalidOperationException($"Tried to sync card event {card} during combat! This is not allowed");
    this._gameService.SendMessage<RewardObtainedMessage>(new RewardObtainedMessage()
    {
      rewardType = RewardType.Card,
      cardModel = card,
      wasSkipped = skipped,
      location = this._messageBuffer.CurrentLocation
    });
  }

  public void SyncLocalObtainedRelic(RelicModel relic) => this.SyncLocalRelicEvent(relic, false);

  public void SyncLocalSkippedRelic(RelicModel relic) => this.SyncLocalRelicEvent(relic, true);

  private void SyncLocalRelicEvent(RelicModel relic, bool skipped)
  {
    if (!RunManager.Instance.IsSingleplayerOrFakeMultiplayer && CombatManager.Instance.IsInProgress)
      throw new InvalidOperationException($"Tried to sync relic event {relic} during combat! This is not allowed");
    this._gameService.SendMessage<RewardObtainedMessage>(new RewardObtainedMessage()
    {
      rewardType = RewardType.Relic,
      relicModel = relic,
      wasSkipped = skipped,
      location = this._messageBuffer.CurrentLocation
    });
  }

  public void SyncLocalObtainedPotion(PotionModel potion)
  {
    this.SyncLocalPotionEvent(potion, false);
  }

  public void SyncLocalSkippedPotion(PotionModel potion) => this.SyncLocalPotionEvent(potion, true);

  private void SyncLocalPotionEvent(PotionModel potion, bool skipped)
  {
    if (!RunManager.Instance.IsSingleplayerOrFakeMultiplayer && CombatManager.Instance.IsInProgress)
      throw new InvalidOperationException($"Tried to sync potion event {potion} during combat! This is not allowed");
    this._gameService.SendMessage<RewardObtainedMessage>(new RewardObtainedMessage()
    {
      rewardType = RewardType.Potion,
      potionModel = potion,
      wasSkipped = skipped,
      location = this._messageBuffer.CurrentLocation
    });
  }

  public void SyncLocalObtainedGold(int goldAmount)
  {
    if (!RunManager.Instance.IsSingleplayerOrFakeMultiplayer && CombatManager.Instance.IsInProgress)
      throw new InvalidOperationException($"Tried to sync obtaining {goldAmount} gold during combat! This is not allowed");
    this._gameService.SendMessage<RewardObtainedMessage>(new RewardObtainedMessage()
    {
      rewardType = RewardType.Gold,
      goldAmount = new int?(goldAmount),
      wasSkipped = false,
      location = this._messageBuffer.CurrentLocation
    });
  }

  public void SyncLocalGoldLost(int goldLost)
  {
    if (!RunManager.Instance.IsSingleplayerOrFakeMultiplayer && CombatManager.Instance.IsInProgress)
      throw new InvalidOperationException($"Tried to sync losing {goldLost} gold during combat! This is not allowed");
    this._gameService.SendMessage<GoldLostMessage>(new GoldLostMessage()
    {
      goldLost = goldLost,
      location = this._messageBuffer.CurrentLocation
    });
  }

  public async Task<bool> DoLocalCardRemoval()
  {
    this._gameService.SendMessage<CardRemovedMessage>(new CardRemovedMessage()
    {
      Location = this._messageBuffer.CurrentLocation
    });
    return await this.DoUnsyncedCardRemoval(this.LocalPlayer);
  }

  private void HandleRewardObtainedMessage(RewardObtainedMessage message, ulong senderId)
  {
    if (CombatManager.Instance.IsInProgress)
    {
      this._bufferedMessages.Add(new RewardSynchronizer.BufferedMessage()
      {
        senderId = senderId,
        rewardMessage = new RewardObtainedMessage?(message)
      });
    }
    else
    {
      Player player = this._playerCollection.GetPlayer(senderId);
      MapPointHistoryEntry historyEntryFor = player.RunState.GetHistoryEntryFor(message.location.mapLocation);
      PlayerMapPointHistoryEntry pointHistoryEntry = (PlayerMapPointHistoryEntry) null;
      if (historyEntryFor != null)
        pointHistoryEntry = historyEntryFor.GetEntry(player.NetId);
      switch (message.rewardType)
      {
        case RewardType.Card:
          CardModel cardModel = message.cardModel;
          if (!message.wasSkipped)
          {
            player.RunState.AddCard(cardModel, player);
            TaskHelper.RunSafely((Task) CardPileCmd.Add(cardModel, PileType.Deck));
            Log.Debug($"Player {player.NetId} obtained {cardModel.Id} from card reward");
          }
          else
            Log.Debug($"Player {player.NetId} skipped {cardModel.Id} from card reward");
          pointHistoryEntry?.CardChoices.Add(new CardChoiceHistoryEntry(message.cardModel, !message.wasSkipped));
          break;
        case RewardType.Gold:
          if (message.wasSkipped)
            throw new NotImplementedException("Cannot handle skip gold reward message!");
          TaskHelper.RunSafely(PlayerCmd.GainGold((Decimal) message.goldAmount.Value, player));
          Log.Debug($"Player {player.NetId} obtained {message.goldAmount} gold from gold reward");
          break;
        case RewardType.Potion:
          if (!message.wasSkipped)
          {
            TaskHelper.RunSafely((Task) PotionCmd.TryToProcure(message.potionModel.ToMutable(), player));
            Log.Debug($"Player {player.NetId} obtained {message.potionModel?.Id} from potion reward");
            break;
          }
          Log.Debug($"Player {player.NetId} skipped {message.potionModel?.Id} from potion reward");
          pointHistoryEntry?.PotionChoices.Add(new ModelChoiceHistoryEntry(message.potionModel.Id, !message.wasSkipped));
          break;
        case RewardType.Relic:
          if (!message.wasSkipped)
          {
            TaskHelper.RunSafely((Task) RelicCmd.Obtain(message.relicModel, player));
            Log.Debug($"Player {player.NetId} obtained {message.relicModel?.Id} from relic reward");
            break;
          }
          Log.Debug($"Player {player.NetId} skipped {message.relicModel?.Id} from relic reward");
          pointHistoryEntry?.RelicChoices.Add(new ModelChoiceHistoryEntry(message.relicModel.Id, !message.wasSkipped));
          break;
        default:
          throw new ArgumentOutOfRangeException("rewardType", (object) message.rewardType, (string) null);
      }
    }
  }

  private void HandleGoldLostMessage(GoldLostMessage message, ulong senderId)
  {
    if (CombatManager.Instance.IsInProgress)
    {
      this._bufferedMessages.Add(new RewardSynchronizer.BufferedMessage()
      {
        senderId = senderId,
        goldLostMessage = new GoldLostMessage?(message)
      });
    }
    else
    {
      Player player = this._playerCollection.GetPlayer(senderId);
      TaskHelper.RunSafely(PlayerCmd.LoseGold((Decimal) message.goldLost, player));
      Log.Debug($"Player {player.NetId} lost {message.goldLost} gold");
    }
  }

  private void HandleCardRemovedMessage(CardRemovedMessage message, ulong senderId)
  {
    if (CombatManager.Instance.IsInProgress)
    {
      this._bufferedMessages.Add(new RewardSynchronizer.BufferedMessage()
      {
        senderId = senderId,
        cardRemovedMessage = message
      });
    }
    else
    {
      Player player = this._playerCollection.GetPlayer(senderId);
      if (player == this.LocalPlayer)
        throw new InvalidOperationException("CardRemovedMessage should not be sent to the player removing the card!");
      TaskHelper.RunSafely((Task) this.DoUnsyncedCardRemoval(player));
    }
  }

  private void OnCombatEnded(CombatRoom _)
  {
    foreach (RewardSynchronizer.BufferedMessage bufferedMessage in this._bufferedMessages)
    {
      if (bufferedMessage.rewardMessage.HasValue)
        this.HandleRewardObtainedMessage(bufferedMessage.rewardMessage.Value, bufferedMessage.senderId);
      else if (bufferedMessage.goldLostMessage.HasValue)
        this.HandleGoldLostMessage(bufferedMessage.goldLostMessage.Value, bufferedMessage.senderId);
      else if (bufferedMessage.cardRemovedMessage != null)
        this.HandleCardRemovedMessage(bufferedMessage.cardRemovedMessage, bufferedMessage.senderId);
    }
    this._bufferedMessages.Clear();
  }

  public async Task<bool> DoUnsyncedCardRemoval(Player player)
  {
    CardModel card = (await CardSelectCmd.FromDeckForRemoval(player, new CardSelectorPrefs(new LocString("gameplay_ui", "COMBAT_REWARD_CARD_REMOVAL.selectionScreenPrompt"), 1)
    {
      Cancelable = true,
      RequireManualConfirmation = true
    })).FirstOrDefault<CardModel>();
    if (card == null)
      return false;
    await CardPileCmd.RemoveFromDeck(card);
    Log.Debug($"Player {player.NetId} removed {card.Id} from deck");
    return true;
  }

  private struct BufferedMessage
  {
    public ulong senderId;
    public RewardObtainedMessage? rewardMessage;
    public GoldLostMessage? goldLostMessage;
    public CardRemovedMessage? cardRemovedMessage;
  }
}
