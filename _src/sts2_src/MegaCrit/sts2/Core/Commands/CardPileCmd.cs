// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Commands.CardPileCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Ftue;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Cards;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Commands;

public static class CardPileCmd
{
  public static async Task RemoveFromDeck(CardModel card, bool showPreview = true)
  {
    // ISSUE: object of a compiler-generated type is created
    await CardPileCmd.RemoveFromDeck((IReadOnlyList<CardModel>) new \u003C\u003Ez__ReadOnlySingleElementList<CardModel>(card), showPreview);
  }

  public static async Task RemoveFromDeck(IReadOnlyList<CardModel> cards, bool showPreview = true)
  {
    foreach (CardModel card in (IEnumerable<CardModel>) cards)
    {
      if (card.Pile.Type != PileType.Deck)
        throw new InvalidOperationException("You cannot remove a card that is not in the deck.");
      card.Owner.RunState.CurrentMapPointHistoryEntry?.GetEntry(card.Owner.NetId).CardsRemoved.Add(card.ToSerializable());
      await Hook.BeforeCardRemoved(card.Owner.RunState, card);
      card.RemoveFromCurrentPile();
      if (showPreview && LocalContext.IsMine(card))
      {
        NCard cardNode = NCard.Create(card);
        if (cardNode != null)
        {
          ((Node) NRun.Instance.GlobalUi.CardPreviewContainer).AddChildSafely((Node) cardNode);
          cardNode.UpdateVisuals(PileType.None, CardPreviewMode.Normal);
          Tween tween = ((Node) cardNode).CreateTween();
          tween.TweenProperty((GodotObject) cardNode, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 1f)), 0.25).From(Variant.op_Implicit(Vector2.Zero)).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
          if (!TestMode.IsOn)
          {
            tween.TweenInterval(0.25);
            tween.TweenCallback(Callable.From((Action) (() => ((Node) NRun.Instance.GlobalUi.AboveTopBarVfxContainer).AddChildSafely((Node) NCardRemoveVfx.Create(cardNode)))));
            tween.TweenInterval(0.40000000596046448);
          }
          tween.TweenCallback(Callable.From(new Action(((GodotTreeExtensions) cardNode).QueueFreeSafely)));
        }
      }
      card.RemoveFromState();
    }
  }

  public static async Task RemoveFromCombat(CardModel card, bool skipVisuals = false)
  {
    // ISSUE: object of a compiler-generated type is created
    await CardPileCmd.RemoveFromCombat((IEnumerable<CardModel>) new \u003C\u003Ez__ReadOnlySingleElementList<CardModel>(card), skipVisuals);
  }

  public static async Task RemoveFromCombat(IEnumerable<CardModel> cards, bool skipVisuals = false)
  {
    ICombatState combatState;
    IRunState runState;
    Dictionary<CardModel, CardPile> oldPiles;
    if (!cards.Any<CardModel>())
    {
      combatState = (ICombatState) null;
      runState = (IRunState) null;
      oldPiles = (Dictionary<CardModel, CardPile>) null;
    }
    else
    {
      combatState = cards.First<CardModel>().CombatState;
      runState = cards.First<CardModel>().Owner.RunState;
      List<NCard> ncardList = new List<NCard>();
      oldPiles = new Dictionary<CardModel, CardPile>();
      CardPile pile;
      foreach (CardModel card in cards)
      {
        pile = card.Pile;
        if (pile == null || !pile.IsCombatPile)
          throw new InvalidOperationException("Card must be in a combat pile for it to be removed");
        if ((card.Pile.Type != PileType.Play || card.Type != CardType.Power) && !skipVisuals)
        {
          NCard onTable = NCard.FindOnTable(card);
          if (onTable != null)
            ncardList.Add(onTable);
        }
        oldPiles.Add(card, card.Pile);
        card.RemoveFromCurrentPile();
      }
      if (ncardList.Count != 0)
      {
        NPlayerHand hand = NCombatRoom.Instance?.Ui.Hand;
        NCardPlayQueue playQueue = NCombatRoom.Instance?.Ui.PlayQueue;
        Tween tween = (Tween) null;
        for (int index = 0; index < ncardList.Count; ++index)
        {
          NCard node = ncardList[index];
          Vector2 globalPosition = node.GlobalPosition;
          CardModel model = node.Model;
          CardPile oldPile = oldPiles[model];
          bool isInPlayQueue = playQueue != null && ((Node) playQueue).IsAncestorOf((Node) node);
          if (isInPlayQueue)
            playQueue.RemoveCardFromQueueForCancellation(node);
          if (hand != null && ((Node) hand).IsAncestorOf((Node) node))
          {
            hand.Remove(model);
          }
          else
          {
            Node parent = ((Node) node).GetParent();
            if (parent != null)
              parent.RemoveChildSafely((Node) node);
          }
          NCombatRoom instance1 = NCombatRoom.Instance;
          if (instance1 != null)
            ((Node) instance1.Ui).AddChildSafely((Node) node);
          node.GlobalPosition = globalPosition;
          if (tween == null)
          {
            tween = ((Node) NCombatRoom.Instance)?.CreateTween();
            tween?.SetParallel(true);
          }
          model.Pile?.InvokeCardAddFinished();
          if (oldPile.Type != PileType.Hand && oldPile.Type != PileType.Play)
            CardPileCmd.AppendPileLerpTween(tween, node, PileType.Play, oldPile);
          tween?.Chain().TweenCallback(Callable.From((Action) (() =>
          {
            NCombatRoom instance2 = NCombatRoom.Instance;
            NCardExhaustVfx child = instance2 != null ? NCardExhaustVfx.Create(node) : (NCardExhaustVfx) null;
            if (child != null)
            {
              if (instance2 != null)
                ((Node) instance2.Ui).AddChildSafely((Node) child);
              NDebugAudioManager.Instance?.Play("card_exhaust.mp3");
              TaskHelper.RunSafely(child.PlayAnimation());
            }
            else
            {
              if (isInPlayQueue)
                return;
              ((Node) node).QueueFreeSafely();
            }
          })));
        }
        if (tween != null)
        {
          tween.Play();
          if (NCombatRoom.Instance != null)
          {
            bool flag = await tween.AwaitFinished((Node) NCombatRoom.Instance);
          }
        }
      }
      foreach (KeyValuePair<CardModel, CardPile> keyValuePair in oldPiles)
      {
        CardModel cardModel;
        keyValuePair.Deconstruct(ref cardModel, ref pile);
        CardModel oldCard = cardModel;
        await Hook.AfterCardChangedPiles(runState, combatState, oldCard, pile.Type, (AbstractModel) null);
        oldCard.RemoveFromState();
        oldCard = (CardModel) null;
      }
      combatState = (ICombatState) null;
      runState = (IRunState) null;
      oldPiles = (Dictionary<CardModel, CardPile>) null;
    }
  }

  public static async Task GiveToAnotherPlayer(
    CardModel card,
    Player player,
    PileType pileType,
    CardPilePosition position = CardPilePosition.Bottom,
    AbstractModel? clonedBy = null)
  {
    NCard cardNode = NCard.FindOnTable(card);
    card.RemoveFromCurrentPile(true);
    card.GiveToAnotherPlayer(player);
    bool islocalPlayerTheReceivingPlayer = LocalContext.IsMine(card);
    // ISSUE: object of a compiler-generated type is created
    IReadOnlyList<CardPileAddResult> cardPileAddResultList = await CardPileCmd.Add((IEnumerable<CardModel>) new \u003C\u003Ez__ReadOnlySingleElementList<CardModel>(card), pileType.GetPile(player), position, clonedBy, true, true);
    if (cardNode == null)
      cardNode = (NCard) null;
    else if (!((Node) cardNode).IsValid())
    {
      cardNode = (NCard) null;
    }
    else
    {
      Node vfxContainer = (Node) card.Owner.Creature.GetVfxContainer();
      ((Node) cardNode).Reparent(vfxContainer, true);
      if (islocalPlayerTheReceivingPlayer)
      {
        NCard card1 = cardNode;
        CardPile pile = card.Pile;
        int num = pile != null ? (int) pile.Type : (int) pileType;
        string trailPath = card.Owner.Character.TrailPath;
        NCardFlyVfx child = NCardFlyVfx.Create(card1, (PileType) num, true, trailPath);
        if (vfxContainer == null)
        {
          cardNode = (NCard) null;
        }
        else
        {
          vfxContainer.AddChildSafely((Node) child);
          cardNode = (NCard) null;
        }
      }
      else
      {
        NCardFlyVfx child = NCardFlyVfx.Create(cardNode, player.Creature, card.Owner.Character.TrailPath);
        if (vfxContainer == null)
        {
          cardNode = (NCard) null;
        }
        else
        {
          vfxContainer.AddChildSafely((Node) child);
          cardNode = (NCard) null;
        }
      }
    }
  }

  public static async Task<CardPileAddResult> AddGeneratedCardToCombat(
    CardModel card,
    PileType newPileType,
    Player? creator,
    CardPilePosition position = CardPilePosition.Bottom)
  {
    // ISSUE: object of a compiler-generated type is created
    return (await CardPileCmd.AddGeneratedCardsToCombat((IEnumerable<CardModel>) new \u003C\u003Ez__ReadOnlySingleElementList<CardModel>(card), newPileType, creator, position))[0];
  }

  public static async Task<IReadOnlyList<CardPileAddResult>> AddGeneratedCardsToCombat(
    IEnumerable<CardModel> cards,
    PileType newPileType,
    Player? creator,
    CardPilePosition position = CardPilePosition.Bottom)
  {
    List<CardModel> list = cards.ToList<CardModel>();
    if (list.Count == 0)
      return (IReadOnlyList<CardPileAddResult>) Array.Empty<CardPileAddResult>();
    if (!CombatManager.Instance.IsInProgress)
      return (IReadOnlyList<CardPileAddResult>) Array.Empty<CardPileAddResult>();
    if (list.Any<CardModel>((Func<CardModel, bool>) (c => c.Pile != null)))
      throw new InvalidOperationException("You are not allowed to generate cards that already have a pile");
    if (!newPileType.IsCombatPile())
      throw new InvalidOperationException("You are not allowed to added generated cards to a non combat pile");
    ICombatState combatState = list[0].Owner.Creature.CombatState;
    if (combatState == null)
      return (IReadOnlyList<CardPileAddResult>) Array.Empty<CardPileAddResult>();
    List<CardPileAddResult> results = new List<CardPileAddResult>();
    foreach (CardModel card in list)
    {
      CombatManager.Instance.History.CardGenerated(combatState, card, creator);
      List<CardPileAddResult> cardPileAddResultList = results;
      cardPileAddResultList.Add(await CardPileCmd.Add(card, newPileType.GetPile(card.Owner), position));
      cardPileAddResultList = (List<CardPileAddResult>) null;
      await Hook.AfterCardGeneratedForCombat(combatState, card, creator);
    }
    return (IReadOnlyList<CardPileAddResult>) results;
  }

  public static async Task<CardPileAddResult> Add(
    CardModel card,
    PileType newPileType,
    CardPilePosition position = CardPilePosition.Bottom,
    AbstractModel? clonedBy = null,
    bool skipVisuals = false)
  {
    if (card.Owner == null)
      throw new InvalidOperationException($"Attempted to add card {card} to pile, but it has no owner!");
    return await CardPileCmd.Add(card, newPileType.GetPile(card.Owner), position, clonedBy, skipVisuals);
  }

  public static async Task<CardPileAddResult> Add(
    CardModel card,
    CardPile newPile,
    CardPilePosition position = CardPilePosition.Bottom,
    AbstractModel? clonedBy = null,
    bool skipVisuals = false)
  {
    // ISSUE: object of a compiler-generated type is created
    return (await CardPileCmd.Add((IEnumerable<CardModel>) new \u003C\u003Ez__ReadOnlySingleElementList<CardModel>(card), newPile, position, clonedBy, skipVisuals))[0];
  }

  public static async Task<IReadOnlyList<CardPileAddResult>> Add(
    IEnumerable<CardModel> cards,
    PileType newPileType,
    CardPilePosition position = CardPilePosition.Bottom,
    AbstractModel? clonedBy = null,
    bool skipVisuals = false)
  {
    return !cards.Any<CardModel>() ? (IReadOnlyList<CardPileAddResult>) Array.Empty<CardPileAddResult>() : await CardPileCmd.Add(cards, newPileType.GetPile(cards.First<CardModel>().Owner), position, clonedBy, skipVisuals);
  }

  public static async Task<IReadOnlyList<CardPileAddResult>> Add(
    IEnumerable<CardModel> cards,
    CardPile newPile,
    CardPilePosition position = CardPilePosition.Bottom,
    AbstractModel? clonedBy = null,
    bool skipVisuals = false,
    bool isChangingOwners = false)
  {
    if (!cards.Any<CardModel>())
      return (IReadOnlyList<CardPileAddResult>) Array.Empty<CardPileAddResult>();
    if (newPile.IsCombatPile && CombatManager.Instance.IsEnding)
      return (IReadOnlyList<CardPileAddResult>) cards.Select<CardModel, CardPileAddResult>((Func<CardModel, CardPileAddResult>) (c => new CardPileAddResult()
      {
        cardAdded = c,
        success = false
      })).ToList<CardPileAddResult>();
    List<CardPileAddResult> results = new List<CardPileAddResult>();
    Player owningPlayer = (Player) null;
    foreach (CardModel card in cards)
    {
      if (card.Owner == null)
        throw new InvalidOperationException(card.Id.Entry + " has no owner.");
      Creature creature = card.Owner.Creature;
      CardPileAddResult cardPileAddResult;
      if (card.HasBeenRemovedFromState || creature.IsDead || card.IsInCombat && creature.CombatState == null)
      {
        cardPileAddResult = new CardPileAddResult();
        cardPileAddResult.success = false;
        cardPileAddResult.cardAdded = card;
        cardPileAddResult.oldPile = card.Pile;
        cardPileAddResult.modifyingModels = (List<AbstractModel>) null;
        results.Add(cardPileAddResult);
      }
      else
      {
        if (newPile.Type == PileType.Deck)
        {
          if (!card.Owner.RunState.ContainsCard(card))
          {
            if (card.Owner.RunState is NullRunState)
              throw new InvalidOperationException($"Tried to add card {card.Id.Entry} to deck for an owner with a NullRunState!");
            throw new InvalidOperationException(card.Id.Entry + " must be added to a RunState before adding it to your deck.");
          }
        }
        else if (card.IsInCombat && creature.CombatState != null && !creature.CombatState.ContainsCard(card))
          throw new InvalidOperationException(card.Id.Entry + " must be added to a CombatState before adding it to this pile.");
        if (card.UpgradePreviewType.IsPreview())
          throw new InvalidOperationException("A card preview cannot be added to a pile.");
        cardPileAddResult = new CardPileAddResult();
        cardPileAddResult.success = true;
        cardPileAddResult.cardAdded = card;
        cardPileAddResult.oldPile = card.Pile;
        cardPileAddResult.modifyingModels = (List<AbstractModel>) null;
        results.Add(cardPileAddResult);
        if (owningPlayer == null)
          owningPlayer = card.Owner;
        if (owningPlayer != card.Owner)
          throw new InvalidOperationException("Tried to add cards with different owners to the same pile!");
      }
    }
    bool owningPlayerIsLocal = LocalContext.IsMe(owningPlayer);
    int i;
    if (newPile.Type == PileType.Deck)
    {
      for (i = 0; i < results.Count; ++i)
      {
        CardPileAddResult result = results[i];
        AbstractModel preventer;
        if (Hook.ShouldAddToDeck(owningPlayer.RunState, result.cardAdded, out preventer))
        {
          IRunState runState = owningPlayer.RunState;
          runState.CurrentMapPointHistoryEntry?.GetEntry(owningPlayer.NetId).CardsGained.Add(result.cardAdded.ToSerializable());
          result.cardAdded.FloorAddedToDeck = new int?(runState.TotalFloor);
        }
        else
        {
          await preventer.AfterAddToDeckPrevented(result.cardAdded);
          result.success = false;
          results[i] = result;
        }
        result = new CardPileAddResult();
      }
    }
    if (newPile.IsCombatPile && !CombatManager.Instance.IsInProgress || !results.Any<CardPileAddResult>((Func<CardPileAddResult, bool>) (r => r.success)))
      return (IReadOnlyList<CardPileAddResult>) results;
    List<NCard> cardNodes = new List<NCard>();
    List<CardModel> cardsWithoutNodesChangingPiles = new List<CardModel>();
    for (i = 0; i < results.Count; ++i)
    {
      CardPileAddResult cardPileAddResult = results[i];
      if (cardPileAddResult.success)
      {
        NCard cardNode = (NCard) null;
        CardPile oldPile = cardPileAddResult.oldPile;
        CardModel card = cardPileAddResult.cardAdded;
        CardPile targetPile = newPile;
        bool isFullHandAdd = targetPile.Type == PileType.Hand && targetPile.Cards.Count >= CardPile.MaxCardsInHand;
        if (isFullHandAdd)
          targetPile = CardPile.Get(PileType.Discard, card.Owner);
        int num1;
        if (!owningPlayerIsLocal && targetPile.Type != PileType.Play)
        {
          CardPile cardPile = oldPile;
          num1 = cardPile != null ? (cardPile.Type == PileType.Play ? 1 : 0) : 0;
        }
        else
          num1 = 1;
        if (TestMode.IsOff & num1 != 0 && !skipVisuals)
        {
          cardNode = NCard.FindOnTable(card);
          bool flag1 = cardNode == null && targetPile.Type.IsCombatPile() && (isFullHandAdd || oldPile != null || targetPile.Type == PileType.Hand);
          bool flag2 = cardNode == null;
          if (flag2)
          {
            bool flag3;
            if (oldPile != null)
            {
              switch (oldPile.Type)
              {
                case PileType.Draw:
                case PileType.Discard:
                case PileType.Exhaust:
                case PileType.Deck:
                  flag3 = true;
                  goto label_51;
              }
            }
            flag3 = false;
label_51:
            flag2 = flag3;
          }
          bool flag4 = flag2;
          if (flag4)
          {
            bool flag5;
            switch (targetPile.Type)
            {
              case PileType.Draw:
              case PileType.Discard:
              case PileType.Deck:
                flag5 = true;
                break;
              default:
                flag5 = false;
                break;
            }
            flag4 = flag5;
          }
          if (flag4)
            cardsWithoutNodesChangingPiles.Add(card);
          else if (flag1)
            cardNode = CardPileCmd.CreateCardNodeAndUpdateVisuals(card, targetPile.Type, owningPlayerIsLocal);
          if (cardNode != null)
            cardNodes.Add(cardNode);
        }
        CardModel card1 = card;
        if (oldPile != null)
          card.RemoveFromCurrentPile(skipVisuals);
        else if (targetPile.Type == PileType.Deck)
        {
          List<AbstractModel> modifyingModels;
          CardModel deck = Hook.ModifyCardBeingAddedToDeck(card.Owner.RunState, card, out modifyingModels);
          card1 = deck;
          if (modifyingModels != null && modifyingModels.Count > 0)
          {
            cardPileAddResult.cardAdded = deck;
            cardPileAddResult.modifyingModels = modifyingModels;
            results[i] = cardPileAddResult;
          }
        }
        int num2;
        switch (position)
        {
          case CardPilePosition.Bottom:
            num2 = -1;
            break;
          case CardPilePosition.Top:
            num2 = 0;
            break;
          case CardPilePosition.Random:
            num2 = card.Owner.RunState.Rng.Shuffle.NextInt(targetPile.Cards.Count + 1);
            break;
          default:
            throw new ArgumentOutOfRangeException(nameof (position), (object) position, (string) null);
        }
        int index = num2;
        targetPile.AddInternal(card1, index);
        if (oldPile == null && targetPile.IsCombatPile && !isChangingOwners)
          await Hook.AfterCardEnteredCombat(card.CombatState, card);
        if (isFullHandAdd & owningPlayerIsLocal)
          ThinkCmd.Play(new LocString("combat_messages", "HAND_FULL"), owningPlayer.Creature, 2.0);
        CardPile cardPile1 = oldPile;
        if ((cardPile1 != null ? (cardPile1.Type != PileType.Play ? 1 : 0) : 1) != 0 || newPile.Type == PileType.Hand || card.IsDupe)
          cardNode?.UpdateVisuals(targetPile.Type, CardPreviewMode.Normal);
        cardNode = (NCard) null;
        oldPile = (CardPile) null;
        card = (CardModel) null;
        targetPile = (CardPile) null;
      }
    }
    Tween tween = (Tween) null;
    if (cardNodes.Count != 0)
    {
      NPlayerHand handNode = NCombatRoom.Instance?.Ui.Hand;
      tween = ((Node) NCombatRoom.Instance)?.CreateTween().SetParallel(true);
      foreach (NCard ncard in cardNodes)
      {
        NCard cardNode = ncard;
        CardModel card = cardNode.Model;
        CardPile oldPile = results.Find((Predicate<CardPileAddResult>) (r => r.cardAdded == card)).oldPile;
        CardPileCmd.MoveCardNodeToNewPileBeforeTween(cardNode, card.Pile.Type);
        bool flag6 = !owningPlayerIsLocal;
        if (flag6)
        {
          bool flag7;
          switch (card.Pile.Type)
          {
            case PileType.Draw:
            case PileType.Hand:
            case PileType.Discard:
            case PileType.Deck:
              flag7 = true;
              break;
            default:
              flag7 = false;
              break;
          }
          flag6 = flag7;
        }
        if (flag6)
        {
          tween?.Parallel().TweenProperty((GodotObject) cardNode, NodePath.op_Implicit(nameof (position)), Variant.op_Implicit(Vector2.op_Addition(cardNode.Position, Vector2.op_Multiply(Vector2.Down, 25f))), SaveManager.Instance.PrefsSave.FastMode == FastModeType.Fast ? 0.20000000298023224 : 0.30000001192092896);
          tween?.Parallel().TweenProperty((GodotObject) cardNode, NodePath.op_Implicit("modulate"), Variant.op_Implicit(StsColors.exhaustGray), SaveManager.Instance.PrefsSave.FastMode == FastModeType.Fast ? 0.20000000298023224 : 0.30000001192092896);
          tween?.Chain().TweenCallback(Callable.From(new Action(((GodotTreeExtensions) cardNode).QueueFreeSafely)));
        }
        else
        {
          switch (card.Pile.Type)
          {
            case PileType.Hand:
              CardPileCmd.AppendPileLerpTween(tween, cardNode, card.Pile.Type, oldPile);
              if (tween != null)
              {
                tween.Parallel().TweenCallback(Callable.From((Action) (() => handNode?.Add(cardNode))));
                continue;
              }
              continue;
            case PileType.Exhaust:
              card.Pile.InvokeCardAddFinished();
              if (oldPile != null && oldPile.Type != PileType.Hand && oldPile.Type != PileType.Play)
              {
                CardPileCmd.AppendPileLerpTween(tween, cardNode, PileType.Play, oldPile);
                float num3;
                switch (SaveManager.Instance.PrefsSave.FastMode)
                {
                  case FastModeType.Fast:
                    num3 = 0.2f;
                    break;
                  case FastModeType.Instant:
                    num3 = 0.01f;
                    break;
                  default:
                    num3 = 0.5f;
                    break;
                }
                float num4 = num3;
                tween?.Chain().TweenInterval((double) num4);
              }
              if (oldPile != null && oldPile.Type == PileType.Hand)
              {
                if (tween != null)
                {
                  tween.Chain().TweenCallback(Callable.From((Action) (() =>
                  {
                    NCardExhaustQuickVfx ncardExhaustQuickVfx = NCardExhaustQuickVfx.Create(cardNode);
                    if (ncardExhaustQuickVfx != null)
                    {
                      NDebugAudioManager.Instance?.Play("card_exhaust.mp3");
                      TaskHelper.RunSafely(ncardExhaustQuickVfx.PlayAnimation());
                    }
                    else
                      ((Node) cardNode).QueueFreeSafely();
                  })));
                  continue;
                }
                continue;
              }
              if (tween != null)
              {
                tween.Chain().TweenCallback(Callable.From((Action) (() =>
                {
                  NCombatRoom instance = NCombatRoom.Instance;
                  NCardExhaustVfx child = instance != null ? NCardExhaustVfx.Create(cardNode) : (NCardExhaustVfx) null;
                  if (child != null)
                  {
                    ((Node) instance.Ui).AddChildSafely((Node) child);
                    NDebugAudioManager.Instance?.Play("card_exhaust.mp3");
                    TaskHelper.RunSafely(child.PlayAnimation());
                  }
                  else
                    ((Node) cardNode).QueueFreeSafely();
                })));
                continue;
              }
              continue;
            case PileType.Play:
              CardPileCmd.AppendPlayPileLerpTween(tween, cardNode, oldPile);
              continue;
            default:
              if (tween != null)
              {
                tween.TweenCallback(Callable.From((Action) (() =>
                {
                  Node parent = card.Pile.Type != PileType.Deck ? (Node) (object) card.Owner.Creature.GetVfxContainer() : NRun.Instance.GlobalUi.TopBar.TrailContainer;
                  ((Node) cardNode).Reparent(parent, true);
                  NCardFlyVfx child = NCardFlyVfx.Create(cardNode, card.Pile.Type, true, card.Owner.Character.TrailPath);
                  if (parent == null)
                    return;
                  parent.AddChildSafely((Node) child);
                })));
                continue;
              }
              continue;
          }
        }
      }
    }
    if (cardsWithoutNodesChangingPiles.Count != 0)
    {
      foreach (CardModel cardModel in cardsWithoutNodesChangingPiles)
      {
        CardModel card = cardModel;
        CardPile oldPile = results.Find((Predicate<CardPileAddResult>) (r => r.cardAdded == card)).oldPile;
        Node vfxContainer = card.Pile.Type != PileType.Deck ? (Node) (object) card.Owner.Creature.GetVfxContainer() : NRun.Instance.GlobalUi.TopBar.TrailContainer;
        if (tween != null)
        {
          tween.TweenCallback(Callable.From((Action) (() =>
          {
            NCardFlyShuffleVfx child = NCardFlyShuffleVfx.Create(oldPile, card.Pile, card.Owner.Character.TrailPath);
            Node parent = vfxContainer;
            if (parent == null)
              return;
            parent.AddChildSafely((Node) child);
          })));
        }
        else
        {
          NCardFlyShuffleVfx child = NCardFlyShuffleVfx.Create(oldPile, card.Pile, card.Owner.Character.TrailPath);
          Node parent = vfxContainer;
          if (parent != null)
            parent.AddChildSafely((Node) child);
        }
      }
    }
    if (tween != null)
    {
      tween.Play();
      if (!await tween.AwaitFinished((Node) NCombatRoom.Instance))
        return (IReadOnlyList<CardPileAddResult>) results;
    }
    foreach (CardPileAddResult cardPileAddResult in results)
    {
      if (cardPileAddResult.success)
      {
        CardModel cardAdded = cardPileAddResult.cardAdded;
        IRunState runState = cardAdded.Owner.RunState;
        ICombatState combatState = cardAdded.CombatState;
        CardModel card = cardAdded;
        CardPile oldPile = cardPileAddResult.oldPile;
        int type = oldPile != null ? (int) oldPile.Type : 0;
        AbstractModel clonedBy1 = clonedBy;
        await Hook.AfterCardChangedPiles(runState, combatState, card, (PileType) type, clonedBy1);
      }
    }
    return (IReadOnlyList<CardPileAddResult>) results;
  }

  public static async Task AddDuringManualCardPlay(CardModel card)
  {
    CardPile oldPile;
    if (CombatManager.Instance.IsOverOrEnding)
    {
      oldPile = (CardPile) null;
    }
    else
    {
      ICombatState combatState1 = card.Owner.Creature.CombatState;
      bool owningPlayerIsLocal = combatState1 != null && combatState1.ContainsCard(card) ? LocalContext.IsMe(card.Owner) : throw new InvalidOperationException(card.Id.Entry + " must be added to a CombatState before playing it.");
      oldPile = card.Pile;
      NCard cardNode = (NCard) null;
      if (TestMode.IsOff)
        cardNode = NCard.FindOnTable(card) ?? CardPileCmd.CreateCardNodeAndUpdateVisuals(card, PileType.Play, owningPlayerIsLocal);
      card.RemoveFromCurrentPile();
      PileType.Play.GetPile(card.Owner).AddInternal(card);
      if (cardNode != null)
      {
        CardPileCmd.MoveCardNodeToNewPileBeforeTween(cardNode, PileType.Play);
        Tween tween = ((Node) NCombatRoom.Instance).CreateTween().SetParallel(true);
        CardPileCmd.AppendPlayPileLerpTween(tween, cardNode, oldPile);
        cardNode.PlayPileTween = tween;
        tween.Play();
        if (card.Type == CardType.Power)
        {
          if (!await tween.AwaitFinished((Node) NCombatRoom.Instance))
          {
            oldPile = (CardPile) null;
            return;
          }
        }
      }
      IRunState runState = card.Owner.RunState;
      ICombatState combatState2 = card.CombatState;
      CardModel card1 = card;
      CardPile cardPile = oldPile;
      int type = cardPile != null ? (int) cardPile.Type : 0;
      await Hook.AfterCardChangedPiles(runState, combatState2, card1, (PileType) type, (AbstractModel) null);
      oldPile = (CardPile) null;
    }
  }

  private static NCard CreateCardNodeAndUpdateVisuals(
    CardModel card,
    PileType targetPileType,
    bool owningPlayerIsLocal)
  {
    NCard andUpdateVisuals = NCard.Create(card);
    ((Node) NCombatRoom.Instance.Ui).AddChildSafely((Node) andUpdateVisuals);
    andUpdateVisuals.UpdateVisuals(targetPileType, CardPreviewMode.Normal);
    if (!owningPlayerIsLocal)
      andUpdateVisuals.Position = NCombatRoom.Instance.GetCreatureNode(card.Owner.Creature).IntentContainer.GlobalPosition;
    else if (card.Pile != null)
      andUpdateVisuals.Position = card.Pile.Type.GetTargetPosition(andUpdateVisuals);
    else
      andUpdateVisuals.Position = targetPileType.GetTargetPosition(andUpdateVisuals);
    return andUpdateVisuals;
  }

  private static void MoveCardNodeToNewPileBeforeTween(NCard cardNode, PileType newPileType)
  {
    NPlayerHand hand = NCombatRoom.Instance.Ui.Hand;
    NCardPlayQueue playQueue = NCombatRoom.Instance.Ui.PlayQueue;
    Control playContainer = NCombatRoom.Instance.Ui.PlayContainer;
    Vector2 globalPosition = cardNode.GlobalPosition;
    CardModel model = cardNode.Model;
    if (((Node) playQueue).IsAncestorOf((Node) cardNode))
      playQueue.RemoveCardFromQueueForExecution(model);
    if (((Node) hand).IsAncestorOf((Node) cardNode))
    {
      hand.Remove(model);
    }
    else
    {
      Node parent = ((Node) cardNode).GetParent();
      if (parent != null)
        parent.RemoveChildSafely((Node) cardNode);
    }
    if (newPileType == PileType.Play)
    {
      ((Node) playContainer).AddChildSafely((Node) cardNode);
      if (NCombatUi.IsDebugHidingPlayContainer)
        ((CanvasItem) cardNode).Visible = false;
    }
    else
      ((Node) NCombatRoom.Instance.Ui).AddChildSafely((Node) cardNode);
    cardNode.GlobalPosition = globalPosition;
    Tween playPileTween = cardNode.PlayPileTween;
    if (playPileTween == null)
      return;
    playPileTween.FastForwardToCompletion();
  }

  private static void AppendPlayPileLerpTween(Tween? tween, NCard cardNode, CardPile? oldPile)
  {
    CardPileCmd.AppendPileLerpTween(tween, cardNode, cardNode.Model.Pile.Type, oldPile);
    tween?.Parallel().TweenCallback(Callable.From((Action) (() => NCombatRoom.Instance.Ui.AddToPlayContainer(cardNode))));
  }

  private static void AppendPileLerpTween(
    Tween? tween,
    NCard cardNode,
    PileType typePile,
    CardPile? oldPile)
  {
    if (tween == null)
      return;
    Vector2 targetPosition = typePile.GetTargetPosition(cardNode);
    float num1;
    switch (SaveManager.Instance.PrefsSave.FastMode)
    {
      case FastModeType.Fast:
        num1 = 0.1f;
        break;
      case FastModeType.Instant:
        num1 = 0.01f;
        break;
      default:
        num1 = 0.25f;
        break;
    }
    float num2 = num1;
    if (typePile != PileType.Hand)
      tween.TweenProperty((GodotObject) cardNode, NodePath.op_Implicit("position"), Variant.op_Implicit(targetPosition), (double) num2).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    if (typePile == PileType.Play)
      tween.TweenProperty((GodotObject) cardNode, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 0.8f)), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    else if (oldPile == null)
      tween.TweenProperty((GodotObject) cardNode, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), (double) num2).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L).From(Variant.op_Implicit(Vector2.Zero));
    else
      tween.Parallel().TweenProperty((GodotObject) cardNode, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), (double) num2).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
  }

  public static async Task<CardModel?> Draw(PlayerChoiceContext choiceContext, Player player)
  {
    return (await CardPileCmd.Draw(choiceContext, 1M, player)).FirstOrDefault<CardModel>();
  }

  public static Task<IEnumerable<CardModel>> Draw(
    PlayerChoiceContext choiceContext,
    Decimal count,
    Player player,
    bool fromHandDraw = false)
  {
    return CardPileCmd.DrawInternal(choiceContext, count, player, fromHandDraw);
  }

  public static Task DrawWithoutBlockingOnOtherPlayers(
    PlayerChoiceContext choiceContext,
    Decimal count,
    Player player,
    bool fromHandDraw = false)
  {
    BranchingPlayerChoiceContext choiceContext1 = new BranchingPlayerChoiceContext(LocalContext.NetId.Value, GameActionType.Combat, choiceContext);
    Task<IEnumerable<CardModel>> task = CardPileCmd.Draw((PlayerChoiceContext) choiceContext1, count, player, fromHandDraw);
    return choiceContext1.AssignTaskAndWaitForPauseOrCompletion((Task) task);
  }

  private static async Task<IEnumerable<CardModel>> DrawInternal(
    PlayerChoiceContext choiceContext,
    Decimal count,
    Player player,
    bool fromHandDraw = false)
  {
    if (CombatManager.Instance.IsOverOrEnding)
      return (IEnumerable<CardModel>) Array.Empty<CardModel>();
    AbstractModel modifier;
    if (!Hook.ShouldDraw(player.Creature.CombatState, player, fromHandDraw, out modifier))
    {
      await Hook.AfterPreventingDraw(player.Creature.CombatState, modifier);
      return (IEnumerable<CardModel>) Array.Empty<CardModel>();
    }
    ICombatState combatState = player.Creature.CombatState;
    List<CardModel> result = new List<CardModel>();
    CardPile hand = PileType.Hand.GetPile(player);
    CardPile drawPile = PileType.Draw.GetPile(player);
    int drawsRequested = count > 0M ? (int) Math.Ceiling(count) : 0;
    if (drawsRequested == 0)
      return (IEnumerable<CardModel>) result;
    int num = Math.Max(0, CardPile.MaxCardsInHand - hand.Cards.Count);
    if (num == 0)
    {
      CardPileCmd.CheckIfDrawIsPossibleAndShowThoughtBubbleIfNot(player);
      return (IEnumerable<CardModel>) result;
    }
    for (int i = 0; i < drawsRequested && num > 0 && !CombatManager.Instance.IsOverOrEnding && CardPileCmd.CheckIfDrawIsPossibleAndShowThoughtBubbleIfNot(player); ++i)
    {
      await CardPileCmd.ShuffleIfNecessary(choiceContext, player);
      if (CardPileCmd.CheckIfDrawIsPossibleAndShowThoughtBubbleIfNot(player))
      {
        CardModel card = drawPile.Cards.FirstOrDefault<CardModel>();
        if (card != null && hand.Cards.Count < CardPile.MaxCardsInHand)
        {
          result.Add(card);
          CardPileAddResult cardPileAddResult = await CardPileCmd.Add(card, hand);
          CombatManager.Instance.History.CardDrawn(combatState, card, fromHandDraw);
          await Hook.AfterCardDrawn(combatState, choiceContext, card, fromHandDraw);
          card.InvokeDrawn();
          NDebugAudioManager.Instance?.Play("card_deal.mp3", 0.25f, PitchVariance.Small);
          num = Math.Max(0, CardPile.MaxCardsInHand - hand.Cards.Count);
          card = (CardModel) null;
        }
        else
          break;
      }
      else
        break;
    }
    return (IEnumerable<CardModel>) result;
  }

  public static async Task Shuffle(PlayerChoiceContext choiceContext, Player player)
  {
    CardPile drawPile;
    HashSet<CardModel> drawPileCards;
    if (CombatManager.Instance.IsOverOrEnding)
    {
      drawPile = (CardPile) null;
      drawPileCards = (HashSet<CardModel>) null;
    }
    else
    {
      drawPile = PileType.Draw.GetPile(player);
      List<CardModel> list = PileType.Discard.GetPile(player).Cards.ToList<CardModel>();
      float timeBetweenCardAdds = Mathf.Min(0.045f, 0.8f / (float) list.Count);
      float randomTimeBetweenCardAdds = 1.11f * timeBetweenCardAdds;
      drawPileCards = drawPile.Cards.ToHashSet<CardModel>();
      list.AddRange((IEnumerable<CardModel>) drawPileCards);
      list.StableShuffle<CardModel>(player.RunState.Rng.Shuffle);
      Hook.ModifyShuffleOrder(player.Creature.CombatState, player, list, false);
      foreach (CardModel card in drawPileCards)
        drawPile.RemoveInternal(card, true);
      if (CombatManager.Instance.DebugForcedTopCardOnNextShuffle != null)
      {
        if (!list.Remove(CombatManager.Instance.DebugForcedTopCardOnNextShuffle))
          throw new InvalidOperationException($"Could not find card {CombatManager.Instance.DebugForcedTopCardOnNextShuffle.Id.Entry} in discard pile.");
        list.Insert(0, CombatManager.Instance.DebugForcedTopCardOnNextShuffle);
        CombatManager.Instance.DebugClearForcedTopCardOnNextShuffle();
      }
      float waitTimeAccumulator = 0.0f;
      foreach (CardModel card in list)
      {
        if (!drawPileCards.Contains(card))
        {
          CardPileAddResult cardPileAddResult = await CardPileCmd.Add(card, drawPile);
          if (CombatManager.Instance.IsOverOrEnding)
          {
            drawPile = (CardPile) null;
            drawPileCards = (HashSet<CardModel>) null;
            return;
          }
          float seconds = timeBetweenCardAdds + Rng.Chaotic.NextFloat((float) (-(double) randomTimeBetweenCardAdds * 0.5), randomTimeBetweenCardAdds * 0.5f);
          waitTimeAccumulator += seconds;
          if ((double) waitTimeAccumulator >= ((Node) ((SceneTree) Engine.GetMainLoop()).Root).GetProcessDeltaTime())
          {
            await Cmd.Wait(seconds);
            waitTimeAccumulator = 0.0f;
          }
        }
        else
          drawPile.AddInternal(card, silent: true);
      }
      await Cmd.CustomScaledWait(0.2f, 0.5f);
      if (CombatManager.Instance.IsOverOrEnding)
      {
        drawPile = (CardPile) null;
        drawPileCards = (HashSet<CardModel>) null;
      }
      else
      {
        await Hook.AfterShuffle(player.Creature.CombatState, choiceContext, player);
        drawPile = (CardPile) null;
        drawPileCards = (HashSet<CardModel>) null;
      }
    }
  }

  public static async Task AutoPlayFromDrawPile(
    PlayerChoiceContext choiceContext,
    Player player,
    int count,
    CardPilePosition position,
    bool forceExhaust)
  {
    List<CardModel> cards;
    CardPile drawPile;
    if (CombatManager.Instance.IsOverOrEnding)
    {
      cards = (List<CardModel>) null;
      drawPile = (CardPile) null;
    }
    else
    {
      cards = new List<CardModel>(count);
      drawPile = PileType.Draw.GetPile(player);
      for (int i = 0; i < count; ++i)
      {
        await CardPileCmd.ShuffleIfNecessary(choiceContext, player);
        CardModel cardModel;
        switch (position)
        {
          case CardPilePosition.Bottom:
            cardModel = drawPile.Cards.LastOrDefault<CardModel>();
            break;
          case CardPilePosition.Top:
            cardModel = drawPile.Cards.FirstOrDefault<CardModel>();
            break;
          case CardPilePosition.Random:
            cardModel = player.RunState.Rng.CombatCardSelection.NextItem<CardModel>((IEnumerable<CardModel>) drawPile.Cards);
            break;
          default:
            throw new ArgumentOutOfRangeException(nameof (position), (object) position, (string) null);
        }
        CardModel card = cardModel;
        if (card != null)
        {
          cards.Add(card);
          CardPileAddResult cardPileAddResult = await CardPileCmd.Add(card, PileType.Play);
        }
        else
          break;
      }
      foreach (CardModel card in cards)
      {
        if (!card.Owner.Creature.IsDead)
        {
          card.ExhaustOnNextPlay = forceExhaust;
          await CardCmd.AutoPlay(choiceContext, card, (Creature) null);
        }
        else
          break;
      }
      cards = (List<CardModel>) null;
      drawPile = (CardPile) null;
    }
  }

  public static async Task ShuffleIfNecessary(PlayerChoiceContext choiceContext, Player player)
  {
    CardPile pile1 = PileType.Draw.GetPile(player);
    CardPile pile2 = PileType.Discard.GetPile(player);
    if (pile1.Cards.Any<CardModel>() || !pile2.Cards.Any<CardModel>())
      return;
    await CardPileCmd.ShuffleFtueCheck();
    await CardPileCmd.Shuffle(choiceContext, player);
  }

  private static async Task ShuffleFtueCheck()
  {
    if (SaveManager.Instance.SeenFtue("shuffle_ftue") || NModalContainer.Instance == null)
      return;
    NShuffleFtue modalToCreate = NShuffleFtue.Create();
    NModalContainer.Instance.Add((Node) modalToCreate);
    SaveManager.Instance.MarkFtueAsComplete("shuffle_ftue");
    await modalToCreate.WaitForPlayerToConfirm();
  }

  public static async Task AddToCombatAndPreview<T>(
    IEnumerable<Creature> targets,
    PileType pileType,
    int count,
    Player? creator,
    CardPilePosition position = CardPilePosition.Bottom)
    where T : CardModel
  {
    foreach (Creature target in targets)
      await CardPileCmd.AddToCombatAndPreview<T>(target, pileType, count, creator, position);
  }

  public static async Task AddToCombatAndPreview<T>(
    Creature target,
    PileType pileType,
    int count,
    Player? creator,
    CardPilePosition position = CardPilePosition.Bottom)
    where T : CardModel
  {
    Player player = target.Player ?? target.PetOwner;
    CardPileAddResult[] statusCards;
    if (player.Creature.IsDead)
    {
      player = (Player) null;
      statusCards = (CardPileAddResult[]) null;
    }
    else
    {
      statusCards = new CardPileAddResult[count];
      for (int i = 0; i < count; ++i)
      {
        ICombatState combatState = target.CombatState;
        CardModel card = (CardModel) (combatState != null ? combatState.CreateCard<T>(player) : default (T));
        if (card != null)
        {
          CardPileAddResult[] cardPileAddResultArray = statusCards;
          int index = i;
          cardPileAddResultArray[index] = await CardPileCmd.AddGeneratedCardToCombat(card, pileType, creator, position);
          cardPileAddResultArray = (CardPileAddResult[]) null;
        }
      }
      if (!LocalContext.IsMe(player))
      {
        player = (Player) null;
        statusCards = (CardPileAddResult[]) null;
      }
      else if (pileType == PileType.Hand)
      {
        await Cmd.Wait(0.1f);
        player = (Player) null;
        statusCards = (CardPileAddResult[]) null;
      }
      else
      {
        CardCmd.PreviewCardPileAdd((IReadOnlyList<CardPileAddResult>) statusCards, style: statusCards.Length > 5 ? CardPreviewStyle.MessyLayout : CardPreviewStyle.HorizontalLayout);
        await Cmd.Wait(1f);
        player = (Player) null;
        statusCards = (CardPileAddResult[]) null;
      }
    }
  }

  public static async Task<CardModel?> AddCurseToDeck<T>(Player owner) where T : CardModel
  {
    // ISSUE: object of a compiler-generated type is created
    return (await CardPileCmd.AddCursesToDeck((IEnumerable<CardModel>) new \u003C\u003Ez__ReadOnlySingleElementList<CardModel>((CardModel) ModelDb.Card<T>()), owner)).FirstOrDefault<CardPileAddResult>().cardAdded;
  }

  public static async Task<IEnumerable<CardPileAddResult>> AddCursesToDeck(
    IEnumerable<CardModel> curses,
    Player owner)
  {
    List<CardPileAddResult> results = new List<CardPileAddResult>();
    foreach (CardModel curse in curses)
    {
      if (curse.Type != CardType.Curse)
        throw new ArgumentException(curse.Id.Entry + " is not a curse");
      results.Add(await CardPileCmd.Add(owner.RunState.CreateCard(curse, owner), PileType.Deck));
    }
    CardCmd.PreviewCardPileAdd((IReadOnlyList<CardPileAddResult>) results, 2f);
    IEnumerable<CardPileAddResult> deck = (IEnumerable<CardPileAddResult>) results;
    results = (List<CardPileAddResult>) null;
    return deck;
  }

  private static bool CheckIfDrawIsPossibleAndShowThoughtBubbleIfNot(Player player)
  {
    if (PileType.Draw.GetPile(player).Cards.Count + PileType.Discard.GetPile(player).Cards.Count == 0)
    {
      ThinkCmd.Play(new LocString("combat_messages", "NO_DRAW"), player.Creature, 2.0);
      return false;
    }
    if (PileType.Hand.GetPile(player).Cards.Count < CardPile.MaxCardsInHand)
      return true;
    ThinkCmd.Play(new LocString("combat_messages", "HAND_FULL"), player.Creature, 2.0);
    return false;
  }
}
