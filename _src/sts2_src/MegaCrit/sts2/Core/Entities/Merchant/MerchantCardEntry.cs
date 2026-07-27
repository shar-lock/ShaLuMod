// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Merchant.MerchantCardEntry
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.Merchant;

public sealed class MerchantCardEntry : MerchantEntry
{
  private readonly MerchantInventory? _inventory;
  private readonly IEnumerable<CardModel> _cardPool;
  private readonly CardType? _cardType;
  private readonly CardRarity? _cardRarity;

  private static int GetCost(CardModel card)
  {
    int num;
    switch (card.Rarity)
    {
      case CardRarity.Uncommon:
        num = 75;
        break;
      case CardRarity.Rare:
        num = 150;
        break;
      default:
        num = 50;
        break;
    }
    int cost = num;
    if (card.Pool is ColorlessCardPool)
      cost = Mathf.RoundToInt((float) cost * 1.15f);
    return cost;
  }

  public CardCreationResult? CreationResult { get; private set; }

  public bool IsOnSale { get; private set; }

  public MerchantCardEntry(
    Player player,
    MerchantInventory? inventory,
    IEnumerable<CardModel> cardPool,
    CardType cardType)
    : base(player)
  {
    this._inventory = inventory;
    this._cardPool = cardPool;
    this._cardType = new CardType?(cardType);
  }

  public MerchantCardEntry(
    Player player,
    MerchantInventory? inventory,
    IEnumerable<CardModel> cardPool,
    CardRarity cardRarity)
    : base(player)
  {
    this._inventory = inventory;
    this._cardPool = cardPool;
    this._cardRarity = new CardRarity?(cardRarity);
  }

  public void Populate()
  {
    MerchantInventory inventory = this._inventory;
    HashSet<CardModel> second = (inventory != null ? inventory.CardEntries.Select<MerchantCardEntry, CardModel>((Func<MerchantCardEntry, CardModel>) (e => e.CreationResult?.Card.CanonicalInstance)).OfType<CardModel>().ToHashSet<CardModel>() : (HashSet<CardModel>) null) ?? new HashSet<CardModel>();
    if (this._cardType.HasValue)
    {
      this.CreationResult = CardFactory.CreateForMerchant(this._player, this._cardPool.Except<CardModel>((IEnumerable<CardModel>) second), this._cardType.Value);
    }
    else
    {
      if (!this._cardRarity.HasValue)
        throw new InvalidOperationException();
      this.CreationResult = CardFactory.CreateForMerchant(this._player, this._cardPool.Except<CardModel>((IEnumerable<CardModel>) second), this._cardRarity.Value);
    }
    IRunState runState = this._player.RunState;
    Player player = this._player;
    int capacity = 1;
    List<CardCreationResult> cards = new List<CardCreationResult>(capacity);
    CollectionsMarshal.SetCount<CardCreationResult>(cards, capacity);
    CollectionsMarshal.AsSpan<CardCreationResult>(cards)[0] = this.CreationResult;
    Hook.ModifyMerchantCardCreationResults(runState, player, cards);
    this.CalcCost();
  }

  protected override void UpdateEntry()
  {
    if (this.CreationResult == null)
      return;
    IRunState runState = this._player.RunState;
    Player player = this._player;
    int capacity = 1;
    List<CardCreationResult> cards = new List<CardCreationResult>(capacity);
    CollectionsMarshal.SetCount<CardCreationResult>(cards, capacity);
    CollectionsMarshal.AsSpan<CardCreationResult>(cards)[0] = this.CreationResult;
    Hook.ModifyMerchantCardCreationResults(runState, player, cards);
  }

  public void SetOnSale()
  {
    this.IsOnSale = true;
    this.CalcCost();
  }

  public override bool IsStocked => this.CreationResult != null;

  public override void CalcCost()
  {
    if (this.CreationResult == null)
      throw new InvalidOperationException("There is no item to purchase.");
    this._cost = Mathf.RoundToInt((float) MerchantCardEntry.GetCost(this.CreationResult.Card) * this._player.PlayerRng.Shops.NextFloat(0.95f, 1.05f));
    if (!this.IsOnSale)
      return;
    this._cost /= 2;
  }

  protected override async Task<(bool, int)> OnTryPurchase(
    MerchantInventory? inventory,
    bool ignoreCost)
  {
    if (!(await CardPileCmd.Add(this.CreationResult.Card, PileType.Deck)).success)
    {
      this.InvokePurchaseFailed(PurchaseStatus.FailureSpace);
      return (false, 0);
    }
    if (!ignoreCost)
      await PlayerCmd.LoseGold((Decimal) this.Cost, this._player, GoldLossType.Spent);
    RunManager.Instance.RewardSynchronizer.SyncLocalGoldLost(this.Cost);
    RunManager.Instance.RewardSynchronizer.SyncLocalObtainedCard(this.CreationResult.Card);
    if (this.CreationResult.Card.Pool is ColorlessCardPool)
      this._player.RunState.CurrentMapPointHistoryEntry?.GetEntry(this._player.NetId).BoughtColorless.Add(this.CreationResult.Card.Id);
    return (true, ignoreCost ? 0 : this.Cost);
  }

  protected override void ClearAfterPurchase() => this.CreationResult = (CardCreationResult) null;

  protected override void RestockAfterPurchase(MerchantInventory? inventory)
  {
    this.IsOnSale = false;
    this.Populate();
  }
}
