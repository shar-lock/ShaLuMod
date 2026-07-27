// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Game.OneOffSynchronizer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events.Custom.CrystalSphereEvent;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Multiplayer.Messages.Game;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Game;

public class OneOffSynchronizer : IDisposable
{
  private readonly RunLocationTargetedMessageBuffer _messageBuffer;
  private readonly INetGameService _gameService;
  private readonly IPlayerCollection _playerCollection;
  private readonly ulong _localPlayerId;

  private Player LocalPlayer => this._playerCollection.GetPlayer(this._localPlayerId);

  public OneOffSynchronizer(
    RunLocationTargetedMessageBuffer messageBuffer,
    INetGameService gameService,
    IPlayerCollection playerCollection,
    ulong localPlayerId)
  {
    this._playerCollection = playerCollection;
    this._localPlayerId = localPlayerId;
    this._gameService = gameService;
    this._messageBuffer = messageBuffer;
    messageBuffer.RegisterMessageHandler<MerchantCardRemovalMessage>(new MessageHandlerDelegate<MerchantCardRemovalMessage>(this.HandleMerchantCardRemoval));
    messageBuffer.RegisterMessageHandler<TreasureChestOpenedMessage>(new MessageHandlerDelegate<TreasureChestOpenedMessage>(this.HandleTreasureChestOpenedMessage));
    messageBuffer.RegisterMessageHandler<CrystalSphereRewardsMessage>(new MessageHandlerDelegate<CrystalSphereRewardsMessage>(this.HandleCrystalSphereRewardsMessage));
  }

  public void Dispose()
  {
    this._messageBuffer.UnregisterMessageHandler<MerchantCardRemovalMessage>(new MessageHandlerDelegate<MerchantCardRemovalMessage>(this.HandleMerchantCardRemoval));
    this._messageBuffer.UnregisterMessageHandler<TreasureChestOpenedMessage>(new MessageHandlerDelegate<TreasureChestOpenedMessage>(this.HandleTreasureChestOpenedMessage));
    this._messageBuffer.UnregisterMessageHandler<CrystalSphereRewardsMessage>(new MessageHandlerDelegate<CrystalSphereRewardsMessage>(this.HandleCrystalSphereRewardsMessage));
  }

  public Task<bool> DoLocalMerchantCardRemoval(int goldCost, bool cancelable = true)
  {
    this._gameService.SendMessage<MerchantCardRemovalMessage>(new MerchantCardRemovalMessage()
    {
      goldCost = goldCost,
      Location = this._messageBuffer.CurrentLocation
    });
    return this.DoMerchantCardRemoval(this.LocalPlayer, goldCost, cancelable);
  }

  private void HandleMerchantCardRemoval(MerchantCardRemovalMessage message, ulong senderId)
  {
    Player player = this._playerCollection.GetPlayer(senderId);
    if (player == this.LocalPlayer)
      throw new InvalidOperationException("MerchantCardRemovalMessage should not be sent to the player removing the card!");
    TaskHelper.RunSafely((Task) this.DoMerchantCardRemoval(player, message.goldCost));
  }

  private async Task<bool> DoMerchantCardRemoval(Player player, int goldCost, bool cancelable = true)
  {
    CardModel card = (await CardSelectCmd.FromDeckForRemoval(player, new CardSelectorPrefs(CardSelectorPrefs.RemoveSelectionPrompt, 1)
    {
      Cancelable = cancelable,
      RequireManualConfirmation = true
    })).FirstOrDefault<CardModel>();
    if (card != null)
    {
      await PlayerCmd.LoseGold((Decimal) goldCost, player, GoldLossType.Spent);
      await CardPileCmd.RemoveFromDeck(card);
      ++player.ExtraFields.CardShopRemovalsUsed;
    }
    bool flag = card != null;
    card = (CardModel) null;
    return flag;
  }

  public Task<int> DoLocalTreasureRoomRewards()
  {
    this._gameService.SendMessage<TreasureChestOpenedMessage>(new TreasureChestOpenedMessage()
    {
      Location = this._messageBuffer.CurrentLocation
    });
    return this.DoTreasureRoomRewards(this.LocalPlayer);
  }

  private void HandleTreasureChestOpenedMessage(TreasureChestOpenedMessage message, ulong senderId)
  {
    Player player = this._playerCollection.GetPlayer(senderId);
    if (player == this.LocalPlayer)
      throw new InvalidOperationException("TreasureChestOpenedMessage should not be sent to the player who opened the treasure chest!");
    TaskHelper.RunSafely((Task) this.DoTreasureRoomRewards(player));
  }

  private async Task<int> DoTreasureRoomRewards(Player player)
  {
    if (!Hook.ShouldGenerateTreasure(player.RunState, player))
      return 0;
    double gold = (double) player.PlayerRng.Rewards.NextInt(42, 53);
    if (AscensionHelper.HasAscension(AscensionLevel.Poverty))
      gold *= AscensionHelper.PovertyAscensionGoldMultiplier;
    await PlayerCmd.GainGold((Decimal) (int) gold, player);
    double num = gold;
    gold = num + (double) await this.TryHandleSpoilsMap(player);
    return (int) gold;
  }

  private async Task<int> TryHandleSpoilsMap(Player player)
  {
    MapCoord? currentMapCoord = player.RunState.CurrentMapCoord;
    MapPoint mapPoint1;
    if (!currentMapCoord.HasValue)
    {
      mapPoint1 = (MapPoint) null;
    }
    else
    {
      ActMap map = player.RunState.Map;
      currentMapCoord = player.RunState.CurrentMapCoord;
      MapCoord coord = currentMapCoord.Value;
      mapPoint1 = map.GetPoint(coord);
    }
    MapPoint mapPoint2 = mapPoint1;
    if (mapPoint2 == null || !mapPoint2.Quests.Any<AbstractModel>((Func<AbstractModel, bool>) (q => q is SpoilsMap)))
      return 0;
    List<SpoilsMap> list = player.Deck.Cards.OfType<SpoilsMap>().ToList<SpoilsMap>();
    int num1 = 0;
    foreach (SpoilsMap spoilsMap in list)
    {
      int num = num1;
      num1 = num + await spoilsMap.OnQuestComplete();
    }
    return num1;
  }

  public async Task DoLocalCrystalSphereRewards(
    Player owner,
    Rng rng,
    List<CrystalSphereItem> revealed)
  {
    if (owner != LocalContext.GetMe(this._playerCollection))
      throw new InvalidOperationException($"Trying to sync crystal sphere rewards for non-local player {owner.NetId}!");
    List<SerializableCrystalSphereItem> list = revealed.Select<CrystalSphereItem, SerializableCrystalSphereItem>((Func<CrystalSphereItem, SerializableCrystalSphereItem>) (r => r.ToSerializable())).ToList<SerializableCrystalSphereItem>();
    this._gameService.SendMessage<CrystalSphereRewardsMessage>(new CrystalSphereRewardsMessage()
    {
      Location = this._messageBuffer.CurrentLocation,
      rewards = list
    });
    await this.OfferCrystalSphereRewards(owner, revealed, rng);
  }

  private void HandleCrystalSphereRewardsMessage(
    CrystalSphereRewardsMessage message,
    ulong senderId)
  {
    Player player = this._playerCollection.GetPlayer(senderId);
    if (player == this.LocalPlayer)
      throw new InvalidOperationException("CrystalSphereRewardsMessage should not be sent to the player who completed the event!");
    EventModel eventForPlayer = RunManager.Instance.EventSynchronizer.GetEventForPlayer(player);
    if (!(eventForPlayer is CrystalSphere crystalSphere))
      throw new InvalidOperationException($"Received {"CrystalSphereRewardsMessage"} for player {player.NetId} while the player was in event {eventForPlayer.Id}!");
    List<CrystalSphereItem> list = message.rewards.Select<SerializableCrystalSphereItem, CrystalSphereItem>((Func<SerializableCrystalSphereItem, CrystalSphereItem>) (r => CrystalSphereItem.FromSerializable(r, player))).ToList<CrystalSphereItem>();
    TaskHelper.RunSafely(this.OfferCrystalSphereRewards(player, list, crystalSphere.Rng));
  }

  private async Task OfferCrystalSphereRewards(
    Player owner,
    List<CrystalSphereItem> revealed,
    Rng rng)
  {
    List<Reward> list = revealed.Select<CrystalSphereItem, Reward>((Func<CrystalSphereItem, Reward>) (r => r.ToReward(owner, rng))).OfType<Reward>().ToList<Reward>();
    await RewardsCmd.OfferCustom(owner, list);
  }
}
