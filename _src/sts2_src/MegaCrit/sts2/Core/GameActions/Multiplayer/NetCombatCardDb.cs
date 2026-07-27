// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.GameActions.Multiplayer.NetCombatCardDb
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.GameActions.Multiplayer;

public class NetCombatCardDb
{
  private uint _nextId;
  private readonly Dictionary<uint, CardModel> _idToCard = new Dictionary<uint, CardModel>();
  private readonly Dictionary<CardModel, uint> _cardToId = new Dictionary<CardModel, uint>();
  private readonly List<NetCombatCardDb.Subscription> _subscriptions = new List<NetCombatCardDb.Subscription>();

  public static NetCombatCardDb Instance { get; } = new NetCombatCardDb();

  public void StartCombat(IReadOnlyList<Player> players)
  {
    this._nextId = 0U;
    this._idToCard.Clear();
    this._cardToId.Clear();
    foreach (Player player in (IEnumerable<Player>) players)
    {
      if (player.PlayerCombatState != null || !TestMode.IsOn)
      {
        foreach (CardModel card in player.PlayerCombatState.AllPiles.SelectMany<CardPile, CardModel>((Func<CardPile, IEnumerable<CardModel>>) (p => (IEnumerable<CardModel>) p.Cards)))
          this.IdCardIfNecessary(card);
        foreach (CardPile allPile in (IEnumerable<CardPile>) player.PlayerCombatState.AllPiles)
        {
          NetCombatCardDb.Subscription subscription = new NetCombatCardDb.Subscription()
          {
            pile = allPile
          };
          subscription.action = (Action) (() => this.OnPileContentsChanged(subscription.pile));
          this._subscriptions.Add(subscription);
          allPile.ContentsChanged += subscription.action;
        }
      }
    }
    CombatManager.Instance.CombatEnded += new Action<CombatRoom>(this.OnCombatEnded);
  }

  private void OnCombatEnded(CombatRoom _)
  {
    foreach (NetCombatCardDb.Subscription subscription in this._subscriptions)
      subscription.pile.ContentsChanged -= subscription.action;
    this._subscriptions.Clear();
    CombatManager.Instance.CombatEnded -= new Action<CombatRoom>(this.OnCombatEnded);
  }

  public uint GetCardId(CardModel card)
  {
    uint id;
    if (!this.TryGetCardId(card, out id))
      throw new InvalidOperationException($"Card {card} could not be found in combat ID database!");
    return id;
  }

  public CardModel GetCard(uint id)
  {
    CardModel card;
    if (!this.TryGetCard(id, out card))
      throw new InvalidOperationException($"Could not map ID {id} to any card!");
    return card;
  }

  public bool TryGetCardId(CardModel card, out uint id)
  {
    return card.IsMutable ? this._cardToId.TryGetValue(card, out id) : throw new InvalidOperationException($"Tried to get ID for canonical card {card}! Use the ModelId instead");
  }

  public bool TryGetCard(uint id, out CardModel? card) => this._idToCard.TryGetValue(id, out card);

  private void OnPileContentsChanged(CardPile pile)
  {
    foreach (CardModel card in (IEnumerable<CardModel>) pile.Cards)
      this.IdCardIfNecessary(card);
  }

  private void IdCardIfNecessary(CardModel card)
  {
    if (this._cardToId.ContainsKey(card))
      return;
    if (card.Owner == null)
    {
      Log.Error($"Tried to ID combat card {card} without an owner! This is not allowed");
    }
    else
    {
      Log.LogMessage(LogLevel.Debug, LogType.Network, $"ID card {card} owned by {card.Owner.NetId} in pile {card.Pile?.Type} with id: {this._nextId}");
      this._cardToId[card] = this._nextId;
      this._idToCard[this._nextId] = card;
      ++this._nextId;
    }
  }

  public uint IdCardForTesting(CardModel card)
  {
    this.IdCardIfNecessary(card);
    return this._cardToId[card];
  }

  public void ClearCardsForTesting()
  {
    this._nextId = 0U;
    this._idToCard.Clear();
    this._cardToId.Clear();
  }

  private struct Subscription
  {
    public CardPile pile;
    public Action action;
  }
}
