// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Cards.CardPile
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.Cards;

public class CardPile(PileType type)
{
  private readonly List<CardModel> _cards;

  public static int MaxCardsInHand => 10;

  public event Action? ContentsChanged;

  public event Action<CardModel>? CardAdded;

  public event Action<CardModel>? CardRemoved;

  public event Action? CardAddFinished;

  public event Action? CardRemoveFinished;

  public PileType Type { get; } = type;

  public IReadOnlyList<CardModel> Cards => (IReadOnlyList<CardModel>) this._cards;

  public bool IsEmpty => !this.Cards.Any<CardModel>();

  public bool IsCombatPile => this.Type.IsCombatPile();

  public int UpgradableCardCount
  {
    get => this._cards.Count<CardModel>((Func<CardModel, bool>) (card => card.IsUpgradable));
  }

  public static CardPile? Get(PileType type, Player player)
  {
    switch (type)
    {
      case PileType.None:
        return (CardPile) null;
      case PileType.Draw:
        return player.PlayerCombatState?.DrawPile;
      case PileType.Hand:
        return player.PlayerCombatState?.Hand;
      case PileType.Discard:
        return player.PlayerCombatState?.DiscardPile;
      case PileType.Exhaust:
        return player.PlayerCombatState?.ExhaustPile;
      case PileType.Play:
        return player.PlayerCombatState?.PlayPile;
      case PileType.Deck:
        return player.Deck;
      default:
        throw new ArgumentOutOfRangeException(nameof (type), (object) type, (string) null);
    }
  }

  public static IEnumerable<CardModel> GetCards(Player player, params PileType[] piles)
  {
    return ((IEnumerable<PileType>) piles).SelectMany<PileType, CardModel>((Func<PileType, IEnumerable<CardModel>>) (p => (IEnumerable<CardModel>) p.GetPile(player).Cards));
  }

  public void RandomizeOrderInternal(Player player, Rng rng, CombatState state)
  {
    this._cards.UnstableShuffle<CardModel>(rng);
    Action<List<CardModel>> action = TestRngInjector.ConsumeInitialShuffleOverride();
    if (action != null)
      action(this._cards);
    Hook.ModifyShuffleOrder((ICombatState) state, player, this._cards, true);
  }

  public void AddInternal(CardModel card, int index = -1, bool silent = false)
  {
    card.AssertMutable();
    if (this.Cards.Contains<CardModel>(card))
      throw new InvalidOperationException($"Card pile already contains {card}.");
    if (index >= 0)
      this._cards.Insert(index, card);
    else
      this._cards.Add(card);
    if (this.IsCombatPile && CombatManager.Instance.IsInProgress)
      CombatManager.Instance.StateTracker.Subscribe(card);
    if (silent)
      return;
    Action<CardModel> cardAdded = this.CardAdded;
    if (cardAdded != null)
      cardAdded(card);
    this.InvokeContentsChanged();
  }

  public void RemoveInternal(CardModel card, bool silent = false)
  {
    if (!this.Cards.Contains<CardModel>(card))
      throw new InvalidOperationException($"Card pile does not contain {card}.");
    this._cards.Remove(card);
    if (this.IsCombatPile)
      CombatManager.Instance.StateTracker.Unsubscribe(card);
    if (silent)
      return;
    Action<CardModel> cardRemoved = this.CardRemoved;
    if (cardRemoved != null)
      cardRemoved(card);
    this.InvokeContentsChanged();
    this.InvokeCardRemoveFinished();
  }

  public void MoveToBottomInternal(CardModel card)
  {
    if (!this.Cards.Contains<CardModel>(card))
      throw new InvalidOperationException($"Card pile does not contain {card}.");
    this._cards.Remove(card);
    this._cards.Add(card);
  }

  public void MoveToTopInternal(CardModel card)
  {
    if (!this.Cards.Contains<CardModel>(card))
      throw new InvalidOperationException($"Card pile does not contain {card}.");
    this._cards.Remove(card);
    this._cards.Insert(0, card);
  }

  public void Clear(bool silent = false)
  {
    foreach (CardModel card in this.Cards.ToList<CardModel>())
      this.RemoveInternal(card, silent);
    this._cards.Clear();
  }

  public void InvokeCardAddFinished()
  {
    Action cardAddFinished = this.CardAddFinished;
    if (cardAddFinished == null)
      return;
    cardAddFinished();
  }

  public void InvokeCardRemoveFinished()
  {
    Action cardRemoveFinished = this.CardRemoveFinished;
    if (cardRemoveFinished == null)
      return;
    cardRemoveFinished();
  }

  public void InvokeContentsChanged()
  {
    Action contentsChanged = this.ContentsChanged;
    if (contentsChanged == null)
      return;
    contentsChanged();
  }
}
