// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Commands.CardCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Cards;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Commands;

public static class CardCmd
{
  public static async Task AutoPlay(
    PlayerChoiceContext choiceContext,
    CardModel card,
    Creature? target,
    AutoPlayType type = AutoPlayType.Default,
    bool skipXCapture = false,
    bool skipCardPileVisuals = false)
  {
    ICombatState combatState;
    AbstractModel preventer;
    if (CombatManager.Instance.IsOverOrEnding)
    {
      combatState = (ICombatState) null;
      preventer = (AbstractModel) null;
    }
    else if (card.Owner.Creature.IsDead)
    {
      combatState = (ICombatState) null;
      preventer = (AbstractModel) null;
    }
    else
    {
      combatState = card.CombatState ?? card.Owner.Creature.CombatState;
      if (card.Keywords.Contains(CardKeyword.Unplayable))
      {
        await CardCmd.MoveToResultPileWithoutPlaying(choiceContext, card);
        combatState = (ICombatState) null;
        preventer = (AbstractModel) null;
      }
      else if (!Hook.ShouldPlay(combatState, card, out preventer, type))
      {
        await CardCmd.MoveToResultPileWithoutPlaying(choiceContext, card);
        LocString playerDialogueLine = UnplayableReason.BlockedByHook.GetPlayerDialogueLine(preventer);
        if (playerDialogueLine == null)
        {
          combatState = (ICombatState) null;
          preventer = (AbstractModel) null;
        }
        else
        {
          Control vfxContainer = card.Owner.Creature.GetVfxContainer();
          if (vfxContainer == null)
          {
            combatState = (ICombatState) null;
            preventer = (AbstractModel) null;
          }
          else
          {
            ((Node) vfxContainer).AddChildSafely((Node) NThoughtBubbleVfx.Create(playerDialogueLine.GetFormattedText(), card.Owner.Creature, new double?(1.0)));
            combatState = (ICombatState) null;
            preventer = (AbstractModel) null;
          }
        }
      }
      else
      {
        if (card.TargetType == TargetType.AnyEnemy)
        {
          if (target == null)
            target = card.Owner.RunState.Rng.CombatTargets.NextItem<Creature>((IEnumerable<Creature>) combatState.HittableEnemies);
          if (target == null)
          {
            await CardCmd.MoveToResultPileWithoutPlaying(choiceContext, card);
            combatState = (ICombatState) null;
            preventer = (AbstractModel) null;
            return;
          }
        }
        if (card.TargetType == TargetType.AnyAlly)
        {
          IEnumerable<Creature> items = combatState.Allies.Where<Creature>((Func<Creature, bool>) (c => c != null && c.IsAlive && c.IsPlayer && c != card.Owner.Creature));
          if (target == null)
            target = card.Owner.RunState.Rng.CombatTargets.NextItem<Creature>(items);
          if (target == null)
          {
            await CardCmd.MoveToResultPileWithoutPlaying(choiceContext, card);
            combatState = (ICombatState) null;
            preventer = (AbstractModel) null;
            return;
          }
        }
        PlayerCombatState playerCombatState = card.Owner.PlayerCombatState;
        if (card.EnergyCost.CostsX && !skipXCapture)
          card.EnergyCost.CapturedXValue = playerCombatState.Energy;
        if (!skipXCapture)
          card.LastStarsSpent = !card.HasStarCostX ? Math.Max(0, card.GetStarCostWithModifiers()) : playerCombatState.Stars;
        if (card.Pile == null)
        {
          CardPileAddResult cardPileAddResult = await CardPileCmd.Add(card, PileType.Play);
        }
        if (!skipCardPileVisuals)
          TaskHelper.RunSafely(card.OnEnqueuePlayVfx(target));
        await Hook.BeforeCardAutoPlayed(combatState, card, target, type);
        await card.OnPlayWrapper(choiceContext, target, true, new ResourceInfo()
        {
          EnergySpent = 0,
          EnergyValue = card.EnergyCost.GetAmountToSpend(),
          StarsSpent = 0,
          StarValue = Math.Max(0, card.GetStarCostWithModifiers())
        }, skipCardPileVisuals);
        combatState = (ICombatState) null;
        preventer = (AbstractModel) null;
      }
    }
  }

  private static async Task MoveToResultPileWithoutPlaying(
    PlayerChoiceContext choiceContext,
    CardModel card)
  {
    CardPileAddResult cardPileAddResult = await CardPileCmd.Add(card, PileType.Play);
    await card.MoveToResultPileWithoutPlaying(choiceContext);
  }

  public static async Task Discard(PlayerChoiceContext choiceContext, CardModel card)
  {
    // ISSUE: object of a compiler-generated type is created
    await CardCmd.Discard(choiceContext, (IEnumerable<CardModel>) new \u003C\u003Ez__ReadOnlySingleElementList<CardModel>(card));
  }

  public static async Task Discard(PlayerChoiceContext choiceContext, IEnumerable<CardModel> cards)
  {
    await CardCmd.DiscardAndDraw(choiceContext, cards, 0);
  }

  public static async Task DiscardAndDraw(
    PlayerChoiceContext choiceContext,
    IEnumerable<CardModel> cardsToDiscard,
    int cardsToDraw)
  {
    List<CardModel> discardCards;
    ICombatState combatState;
    List<CardModel> slyCards;
    CardPile discardPile;
    if (CombatManager.Instance.IsOverOrEnding)
    {
      discardCards = (List<CardModel>) null;
      combatState = (ICombatState) null;
      slyCards = (List<CardModel>) null;
      discardPile = (CardPile) null;
    }
    else
    {
      discardCards = cardsToDiscard.ToList<CardModel>();
      if (discardCards.Count == 0)
      {
        discardCards = (List<CardModel>) null;
        combatState = (ICombatState) null;
        slyCards = (List<CardModel>) null;
        discardPile = (CardPile) null;
      }
      else
      {
        combatState = discardCards[0].CombatState ?? discardCards[0].Owner.Creature.CombatState;
        slyCards = new List<CardModel>();
        discardPile = PileType.Discard.GetPile(discardCards[0].Owner);
        foreach (CardModel card in discardCards)
        {
          if (card.IsSlyThisTurn)
            slyCards.Add(card);
          CardPileAddResult cardPileAddResult = await CardPileCmd.Add(card, discardPile);
          CombatManager.Instance.History.CardDiscarded(combatState, card);
          await Hook.AfterCardDiscarded(combatState, choiceContext, card);
        }
        discardPile.InvokeContentsChanged();
        if (cardsToDraw > 0)
        {
          IEnumerable<CardModel> cardModels = await CardPileCmd.Draw(choiceContext, (Decimal) cardsToDraw, discardCards[0].Owner);
        }
        foreach (CardModel card in slyCards)
          await CardCmd.AutoPlay(choiceContext, card, (Creature) null, AutoPlayType.SlyDiscard);
        discardCards = (List<CardModel>) null;
        combatState = (ICombatState) null;
        slyCards = (List<CardModel>) null;
        discardPile = (CardPile) null;
      }
    }
  }

  public static void Downgrade(CardModel card)
  {
    if (CombatManager.Instance.IsEnding)
      return;
    CardPile pile = card.Pile;
    if (pile != null && pile.Type == PileType.Deck)
      card.Owner.RunState.CurrentMapPointHistoryEntry?.GetEntry(card.Owner.NetId).DowngradedCards.Add(card.Id);
    card.DowngradeInternal();
  }

  public static async Task Exhaust(
    PlayerChoiceContext choiceContext,
    CardModel card,
    bool causedByEthereal = false,
    bool skipVisuals = false)
  {
    ICombatState combatState;
    if (CombatManager.Instance.IsOverOrEnding)
    {
      combatState = (ICombatState) null;
    }
    else
    {
      combatState = card.CombatState ?? card.Owner.Creature.CombatState;
      CardPileAddResult cardPileAddResult = await CardPileCmd.Add(card, PileType.Exhaust, skipVisuals: skipVisuals);
      CombatManager.Instance.History.CardExhausted(combatState, card);
      await Hook.AfterCardExhausted(combatState, choiceContext, card, causedByEthereal);
      combatState = (ICombatState) null;
    }
  }

  public static void Upgrade(CardModel card, CardPreviewStyle style = CardPreviewStyle.HorizontalLayout)
  {
    // ISSUE: object of a compiler-generated type is created
    CardCmd.Upgrade((IEnumerable<CardModel>) new \u003C\u003Ez__ReadOnlySingleElementList<CardModel>(card), style);
  }

  public static void Upgrade(IEnumerable<CardModel> cards, CardPreviewStyle style)
  {
    if (CombatManager.Instance.IsEnding)
      return;
    foreach (CardModel card in cards)
    {
      if (card.IsUpgradable)
      {
        CardPile pile1 = card.Pile;
        if (pile1 != null && pile1.Type == PileType.Deck)
          card.Owner.RunState.CurrentMapPointHistoryEntry?.GetEntry(card.Owner.NetId).UpgradedCards.Add(card.Id);
        card.UpgradeInternal();
        card.FinalizeUpgradeInternal();
        if (LocalContext.IsMine(card))
        {
          CardPile pile2 = card.Pile;
          if (pile2 != null && pile2.Type == PileType.Deck)
          {
            Control previewContainer;
            switch (style)
            {
              case CardPreviewStyle.None:
                continue;
              case CardPreviewStyle.HorizontalLayout:
                previewContainer = NRun.Instance?.GlobalUi.CardPreviewContainer;
                break;
              case CardPreviewStyle.MessyLayout:
                previewContainer = (Control) NRun.Instance?.GlobalUi.MessyCardPreviewContainer;
                break;
              case CardPreviewStyle.EventLayout:
                previewContainer = NRun.Instance?.GlobalUi.EventCardPreviewContainer;
                break;
              case CardPreviewStyle.GridLayout:
                previewContainer = (Control) NRun.Instance?.GlobalUi.GridCardPreviewContainer;
                break;
              default:
                throw new ArgumentOutOfRangeException(nameof (style), $"Unexpected {"CardPreviewStyle"} {style}!");
            }
            Control parent = previewContainer;
            if (parent != null)
              ((Node) parent).AddChildSafely((Node) NCardUpgradeVfx.Create(card));
          }
        }
      }
    }
  }

  public static async Task<CardPileAddResult> TransformToRandom(
    CardModel original,
    Rng rng,
    CardPreviewStyle style = CardPreviewStyle.HorizontalLayout)
  {
    return (await CardCmd.Transform(new CardTransformation(original).Yield(), rng, style)).First<CardPileAddResult>();
  }

  public static async Task<CardPileAddResult?> TransformTo<T>(
    CardModel original,
    CardPreviewStyle style = CardPreviewStyle.HorizontalLayout)
    where T : CardModel
  {
    return await CardCmd.Transform(original, (CardModel) original.CardScope.CreateCard<T>(original.Owner), style);
  }

  public static async Task<CardPileAddResult?> Transform(
    CardModel original,
    CardModel replacement,
    CardPreviewStyle style = CardPreviewStyle.HorizontalLayout)
  {
    return new CardPileAddResult?((await CardCmd.Transform(new CardTransformation(original, replacement).Yield(), (Rng) null, style)).FirstOrDefault<CardPileAddResult>());
  }

  private static int PileIndexSort(
    (CardTransformation, CardPile, int, CardModel) value1,
    (CardTransformation, CardPile, int, CardModel) value2)
  {
    return value1.Item2.Type != value2.Item2.Type ? value1.Item2.Type.CompareTo((object) value2.Item2.Type) : value1.Item3.CompareTo(value2.Item3);
  }

  public static async Task<IEnumerable<CardPileAddResult>> Transform(
    IEnumerable<CardTransformation> transformations,
    Rng? rng,
    CardPreviewStyle style = CardPreviewStyle.HorizontalLayout)
  {
    if (CombatManager.Instance.IsEnding)
      return (IEnumerable<CardPileAddResult>) Array.Empty<CardPileAddResult>();
    CardTransformation[] transformationsArr = transformations.ToArray<CardTransformation>();
    if (transformationsArr.Length == 0)
      return (IEnumerable<CardPileAddResult>) Array.Empty<CardPileAddResult>();
    ICombatState combatState = transformationsArr[0].Original.CombatState;
    List<(CardTransformation, CardPile, int, CardModel)> transformationsWithOriginalData = new List<(CardTransformation, CardPile, int, CardModel)>();
    foreach (CardTransformation cardTransformation in transformationsArr)
    {
      cardTransformation.Original.AssertMutable();
      if (!cardTransformation.Original.IsTransformable)
        throw new InvalidOperationException($"Can't transform {cardTransformation.Original.Id.Entry} because it's un-transformable.");
      CardPile pile = cardTransformation.Original.Pile;
      if (pile == null)
        throw new InvalidOperationException($"Can't transform {cardTransformation.Original.Id.Entry} because it has no pile.");
      int num = pile.Cards.IndexOf<CardModel>(cardTransformation.Original);
      CardModel replacement = cardTransformation.GetReplacement(rng);
      if (replacement == null)
        throw new InvalidOperationException($"Attempting to transform un-transformable card {cardTransformation.Original}!");
      cardTransformation.Original.RemoveFromCurrentPile();
      transformationsWithOriginalData.Add((cardTransformation, pile, num, replacement));
    }
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    transformationsWithOriginalData.Sort(CardCmd.\u003C\u003EO.\u003C0\u003E__PileIndexSort ?? (CardCmd.\u003C\u003EO.\u003C0\u003E__PileIndexSort = new Comparison<(CardTransformation, CardPile, int, CardModel)>(CardCmd.PileIndexSort)));
    List<CardPileAddResult> results = new List<CardPileAddResult>();
    foreach ((CardTransformation cardTransformation, CardPile cardPile, int index, CardModel cardModel) in transformationsWithOriginalData)
    {
      CardModel original = cardTransformation.Original;
      IRunState runState = original.Owner.RunState;
      CardModel replacement = cardModel;
      replacement.AssertMutable();
      CardPileAddResult result = new CardPileAddResult()
      {
        success = true,
        cardAdded = replacement,
        modifyingModels = (List<AbstractModel>) null
      };
      if (replacement.Owner != original.Owner)
        throw new InvalidOperationException($"Attempting to transform card {original} to {replacement}, but the replacement has a different owner!");
      if (cardPile.Type == PileType.Deck)
      {
        List<AbstractModel> modifyingModels;
        replacement = Hook.ModifyCardBeingAddedToDeck(runState, replacement, out modifyingModels);
        result.cardAdded = replacement;
        result.modifyingModels = modifyingModels;
        replacement.FloorAddedToDeck = new int?(runState.TotalFloor);
        runState.CurrentMapPointHistoryEntry?.GetEntry(original.Owner.NetId).CardsTransformed.Add(new CardTransformationHistoryEntry(original, replacement));
      }
      if (cardPile.Type == PileType.Deck)
      {
        cardPile.AddInternal(replacement);
      }
      else
      {
        cardPile.AddInternal(replacement, index);
        CombatManager.Instance.History.CardGenerated(combatState, replacement, replacement.Owner);
        await Hook.AfterCardEnteredCombat(combatState, replacement);
      }
      await Hook.AfterCardChangedPiles(runState, combatState, replacement, cardPile.Type, (AbstractModel) null);
      cardPile.InvokeCardAddFinished();
      original.AfterTransformedFrom();
      replacement.AfterTransformedTo();
      results.Add(result);
      original = (CardModel) null;
      runState = (IRunState) null;
      replacement = (CardModel) null;
      result = new CardPileAddResult();
      cardPile = (CardPile) null;
    }
    List<Task> vfxTasks = new List<Task>();
    int i;
    for (i = 0; i < results.Count; ++i)
    {
      CardModel original = transformationsWithOriginalData[i].Item1.Original;
      CardModel cardAdded = results[i].cardAdded;
      if (LocalContext.IsMine(cardAdded))
      {
        if (cardAdded.Pile.Type == PileType.Hand)
        {
          if (!TestMode.IsOn)
          {
            NCardPlayQueue playQueue = NCombatRoom.Instance.Ui.PlayQueue;
            NPlayerHand hand = NCombatRoom.Instance.Ui.Hand;
            NCard onTable = NCard.FindOnTable(original, new PileType?(PileType.Hand));
            if (onTable == null)
              throw new InvalidOperationException($"Couldn't get hand node for original card {transformationsArr[i].Original}!");
            if (((Node) playQueue).IsAncestorOf((Node) onTable))
              playQueue.RemoveCardFromQueueForCancellation(onTable, true);
            hand.TryCancelCardPlay(original);
            NCardTransformShineVfx transformShineVfx = NCardTransformShineVfx.Create(onTable, cardAdded, (IEnumerable<RelicModel>) Array.Empty<RelicModel>());
            if (transformShineVfx != null)
            {
              vfxTasks.Add(transformShineVfx.PlayAnimationWithoutWaitingForEnd());
              await Cmd.CustomScaledWait(0.1f, 0.25f);
            }
          }
        }
        else if (style != CardPreviewStyle.None && TestMode.IsOff)
        {
          Control control1;
          switch (style)
          {
            case CardPreviewStyle.HorizontalLayout:
              control1 = NCombatRoom.Instance?.Ui.CardPreviewContainer ?? NRun.Instance?.GlobalUi.CardPreviewContainer;
              break;
            case CardPreviewStyle.MessyLayout:
              control1 = (Control) (NCombatRoom.Instance?.Ui.MessyCardPreviewContainer ?? NRun.Instance?.GlobalUi.MessyCardPreviewContainer);
              break;
            case CardPreviewStyle.EventLayout:
              control1 = NRun.Instance?.GlobalUi.EventCardPreviewContainer;
              break;
            case CardPreviewStyle.GridLayout:
              control1 = (Control) NRun.Instance?.GlobalUi.GridCardPreviewContainer;
              break;
            default:
              throw new ArgumentOutOfRangeException(nameof (style), $"Unexpected {"CardPreviewStyle"} {style}!");
          }
          Control control2 = control1;
          if (control2 != null)
          {
            Control parent = control2;
            CardModel startCard = original;
            CardModel endCard = cardAdded;
            List<AbstractModel> modifyingModels = results[i].modifyingModels;
            IEnumerable<RelicModel> relicsToFlash = modifyingModels != null ? modifyingModels.OfType<RelicModel>() : (IEnumerable<RelicModel>) null;
            NCardTransformVfx child = NCardTransformVfx.Create(startCard, endCard, relicsToFlash);
            ((Node) parent).AddChildSafely((Node) child);
          }
        }
      }
    }
    await Task.WhenAll((IEnumerable<Task>) vfxTasks);
    for (i = 0; i < results.Count; ++i)
    {
      CardPileAddResult cardPileAddResult = results[i];
      if (cardPileAddResult.success && cardPileAddResult.cardAdded.Pile.Type.IsCombatPile())
        await Hook.AfterCardGeneratedForCombat(cardPileAddResult.cardAdded.CombatState, cardPileAddResult.cardAdded, cardPileAddResult.cardAdded.Owner);
      transformationsWithOriginalData[i].Item1.Original.RemoveFromState();
    }
    return (IEnumerable<CardPileAddResult>) results;
  }

  public static T? Enchant<T>(CardModel card, Decimal amount) where T : EnchantmentModel
  {
    return CardCmd.Enchant(ModelDb.Enchantment<T>().ToMutable(), card, amount) as T;
  }

  public static EnchantmentModel? Enchant(
    EnchantmentModel enchantment,
    CardModel card,
    Decimal amount)
  {
    enchantment.AssertMutable();
    if (!enchantment.CanEnchant(card))
      throw new InvalidOperationException($"Cannot enchant {card.Id} with {enchantment.Id}.");
    if (card.Enchantment == null)
    {
      card.EnchantInternal(enchantment, amount);
      enchantment.ModifyCard();
    }
    else if (card.Enchantment.GetType() == enchantment.GetType())
      card.Enchantment.Amount += (int) amount;
    else
      throw new InvalidOperationException($"Cannot enchant {card.Id} with {enchantment.Id} because it already has enchantment {card.Enchantment.Id}.");
    card.FinalizeUpgradeInternal();
    CardPile pile = card.Pile;
    if (pile != null && pile.Type == PileType.Deck)
      card.Owner.RunState.CurrentMapPointHistoryEntry?.GetEntry(card.Owner.NetId).CardsEnchanted.Add(new CardEnchantmentHistoryEntry(card, enchantment.Id));
    return card.Enchantment;
  }

  public static void ClearEnchantment(CardModel card) => card.ClearEnchantmentInternal();

  public static async Task<IEnumerable<T>> AfflictAndPreview<T>(
    IEnumerable<CardModel> cards,
    Decimal amount,
    CardPreviewStyle style = CardPreviewStyle.HorizontalLayout)
    where T : AfflictionModel
  {
    List<T> afflictions = new List<T>();
    List<CardModel> cardList = new List<CardModel>();
    foreach (CardModel card in cards)
    {
      T obj = await CardCmd.Afflict<T>(card, amount);
      if ((object) obj != null)
      {
        afflictions.Add(obj);
        cardList.Add(card);
      }
    }
    if (cardList.Count > 0 && style != CardPreviewStyle.None)
    {
      if (cardList.Any<CardModel>((Func<CardModel, bool>) (c => c.Owner != cardList[0].Owner)))
        throw new InvalidOperationException("All cards passed to AfflictAndPreview must have the same owner!");
      if (LocalContext.IsMine(cardList[0]))
      {
        CardCmd.Preview((IReadOnlyList<CardModel>) cardList, style: style);
        await Cmd.Wait(1.25f);
      }
    }
    IEnumerable<T> objs = (IEnumerable<T>) afflictions;
    afflictions = (List<T>) null;
    return objs;
  }

  public static async Task<T?> Afflict<T>(CardModel card, Decimal amount) where T : AfflictionModel
  {
    return await CardCmd.Afflict(ModelDb.Affliction<T>().ToMutable(), card, amount) as T;
  }

  public static Task<AfflictionModel?> Afflict(
    AfflictionModel affliction,
    CardModel card,
    Decimal amount)
  {
    if (CombatManager.Instance.IsOverOrEnding)
    {
      CardPile pile = card.Pile;
      if (pile != null && pile.IsCombatPile)
        return Task.FromResult<AfflictionModel>((AfflictionModel) null);
    }
    affliction.AssertMutable();
    ICombatState combatState = card.CombatState ?? card.Owner.Creature.CombatState;
    if (combatState == null || !Hook.ShouldAfflict(combatState, card, affliction))
      return Task.FromResult<AfflictionModel>((AfflictionModel) null);
    if (!affliction.CanAfflict(card))
      return Task.FromResult<AfflictionModel>((AfflictionModel) null);
    if (card.Affliction == null)
    {
      card.AfflictInternal(affliction, amount);
      affliction.AfterApplied();
    }
    else if (card.Affliction.GetType() == affliction.GetType())
      card.Affliction.Amount += (int) amount;
    else
      throw new InvalidOperationException($"Cannot afflict {card.Id} with {affliction.Id} because it already has affliction {card.Affliction.Id}.");
    CombatManager.Instance.History.CardAfflicted(combatState, card, affliction);
    return Task.FromResult<AfflictionModel>(card.Affliction);
  }

  public static void ClearAffliction(CardModel card) => card.ClearAfflictionInternal();

  public static void ApplyKeyword(CardModel card, params CardKeyword[] keywords)
  {
    foreach (CardKeyword keyword in keywords)
      card.AddKeyword(keyword);
    NCard.FindOnTable(card)?.UpdateVisuals(card.Pile.Type, CardPreviewMode.Normal);
  }

  public static void RemoveKeyword(CardModel card, params CardKeyword[] keywords)
  {
    foreach (CardKeyword keyword in keywords)
      card.RemoveKeyword(keyword);
    NCard.FindOnTable(card)?.UpdateVisuals(card.Pile.Type, CardPreviewMode.Normal);
  }

  public static void ApplySingleTurnSly(CardModel card)
  {
    card.GiveSingleTurnSly();
    NCard.FindOnTable(card)?.UpdateVisuals(card.Pile.Type, CardPreviewMode.Normal);
  }

  public static void ApplySingleTurnRetain(CardModel card)
  {
    card.GiveSingleTurnRetain();
    NCard.FindOnTable(card)?.UpdateVisuals(card.Pile.Type, CardPreviewMode.Normal);
  }

  public static TaskCompletionSource? Preview(CardModel card, float time = 1.2f, CardPreviewStyle style = CardPreviewStyle.HorizontalLayout)
  {
    return CardCmd.PreviewInternal(card, false, time: time, style: style);
  }

  public static void Preview(IReadOnlyList<CardModel> cards, float time = 1.2f, CardPreviewStyle style = CardPreviewStyle.HorizontalLayout)
  {
    if (TestMode.IsOn || CombatManager.Instance.IsEnding)
      return;
    foreach (CardModel card in (IEnumerable<CardModel>) cards)
      CardCmd.PreviewInternal(card, false, time: time, style: style);
  }

  public static void PreviewCardPileAdd(
    CardPileAddResult result,
    float time = 1.2f,
    CardPreviewStyle style = CardPreviewStyle.HorizontalLayout)
  {
    if (TestMode.IsOn || CombatManager.Instance.IsEnding || !result.success || !LocalContext.IsMine(result.cardAdded))
      return;
    CardModel cardAdded = result.cardAdded;
    List<AbstractModel> modifyingModels = result.modifyingModels;
    IEnumerable<RelicModel> relicsToFlash = (modifyingModels != null ? modifyingModels.OfType<RelicModel>() : (IEnumerable<RelicModel>) null) ?? (IEnumerable<RelicModel>) null;
    double time1 = (double) time;
    int style1 = (int) style;
    CardCmd.PreviewInternal(cardAdded, true, relicsToFlash, (float) time1, (CardPreviewStyle) style1);
  }

  public static void PreviewCardPileAdd(
    IReadOnlyList<CardPileAddResult> results,
    float time = 1.2f,
    CardPreviewStyle style = CardPreviewStyle.HorizontalLayout)
  {
    if (TestMode.IsOn || CombatManager.Instance.IsEnding)
      return;
    if (results.Count > 5 && style == CardPreviewStyle.HorizontalLayout)
      Log.Warn("Horizontal layout is being used with more than five cards! They will go offscreen");
    foreach (CardPileAddResult result in (IEnumerable<CardPileAddResult>) results)
    {
      if (result.success && LocalContext.IsMine(result.cardAdded))
      {
        CardModel cardAdded = result.cardAdded;
        List<AbstractModel> modifyingModels = result.modifyingModels;
        IEnumerable<RelicModel> relicsToFlash = (modifyingModels != null ? modifyingModels.OfType<RelicModel>() : (IEnumerable<RelicModel>) null) ?? (IEnumerable<RelicModel>) null;
        double time1 = (double) time;
        int style1 = (int) style;
        CardCmd.PreviewInternal(cardAdded, true, relicsToFlash, (float) time1, (CardPreviewStyle) style1);
      }
    }
  }

  private static TaskCompletionSource? PreviewInternal(
    CardModel card,
    bool isAddingCardsToPile,
    IEnumerable<RelicModel>? relicsToFlash = null,
    float time = 1.2f,
    CardPreviewStyle style = CardPreviewStyle.HorizontalLayout)
  {
    if (card.Pile == null)
      return (TaskCompletionSource) null;
    if (TestMode.IsOn)
      return (TaskCompletionSource) null;
    if (CombatManager.Instance.IsEnding)
      return (TaskCompletionSource) null;
    if (!LocalContext.IsMine(card))
      return (TaskCompletionSource) null;
    PileType pileType = card.Pile.Type;
    if (CardCmd.GetTotalCardsBeingPreviewed() > 50)
      return (TaskCompletionSource) null;
    Control control;
    switch (style)
    {
      case CardPreviewStyle.HorizontalLayout:
        control = pileType.IsCombatPile() ? NCombatRoom.Instance.Ui.CardPreviewContainer : NRun.Instance?.GlobalUi.CardPreviewContainer;
        break;
      case CardPreviewStyle.MessyLayout:
        control = pileType.IsCombatPile() ? (Control) NCombatRoom.Instance.Ui.MessyCardPreviewContainer : (Control) NRun.Instance?.GlobalUi.MessyCardPreviewContainer;
        break;
      case CardPreviewStyle.EventLayout:
        if (pileType.IsCombatPile())
          throw new InvalidOperationException();
        control = NRun.Instance?.GlobalUi.EventCardPreviewContainer;
        break;
      case CardPreviewStyle.GridLayout:
        if (pileType.IsCombatPile())
          throw new InvalidOperationException();
        control = (Control) NRun.Instance?.GlobalUi.GridCardPreviewContainer;
        break;
      default:
        throw new ArgumentOutOfRangeException(nameof (style), $"Unexpected {"CardPreviewStyle"} {style}!");
    }
    Control parent1 = control;
    if (parent1 == null)
      return (TaskCompletionSource) null;
    if (style == CardPreviewStyle.HorizontalLayout && ((Node) parent1).GetChildCount(false) > 5)
      parent1 = pileType.IsCombatPile() ? (Control) NCombatRoom.Instance.Ui.MessyCardPreviewContainer : (Control) NRun.Instance?.GlobalUi.MessyCardPreviewContainer;
    NCard node = NCard.Create(card);
    if (parent1 != null)
      ((Node) parent1).AddChildSafely((Node) node);
    node.UpdateVisuals(pileType, CardPreviewMode.Normal);
    TaskCompletionSource source = new TaskCompletionSource();
    Tween tween = ((Node) node).CreateTween();
    tween.TweenProperty((GodotObject) node, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.25).From(Variant.op_Implicit(Vector2.Zero)).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    tween.TweenCallback(Callable.From((Action) (() => TaskHelper.RunSafely(CardCmd.FlashRelics(node, relicsToFlash)))));
    tween.TweenCallback(Callable.From((Action) (() =>
    {
      NCardFlyVfx child = (NCardFlyVfx) null;
      Node parent2 = pileType != PileType.Deck ? (Node) card.Owner.Creature.GetVfxContainer() : NRun.Instance?.GlobalUi.TopBar.TrailContainer;
      if (parent2 != null)
        child = NCardFlyVfx.Create(node, card.Pile != null ? card.Pile.Type : pileType, isAddingCardsToPile, card.Owner.Character.TrailPath);
      if (child != null && parent2 != null)
      {
        parent2.AddChildSafely((Node) child);
        TaskHelper.RunSafely(child.SwooshAwayCompletion.Task.ContinueWith((Action<Task>) (_ => source.SetResult())));
      }
      else
      {
        ((Node) node).QueueFreeSafely();
        source.SetResult();
      }
    }))).SetDelay((double) time);
    return source;
  }

  private static int GetTotalCardsBeingPreviewed()
  {
    int cardsBeingPreviewed = 0;
    if (NCombatRoom.Instance != null)
      cardsBeingPreviewed = ((Node) NCombatRoom.Instance.Ui.CardPreviewContainer).GetChildCount(false) + ((Node) NCombatRoom.Instance.Ui.MessyCardPreviewContainer).GetChildCount(false);
    if (NRun.Instance != null)
      cardsBeingPreviewed = cardsBeingPreviewed + ((Node) NRun.Instance.GlobalUi.CardPreviewContainer).GetChildCount(false) + ((Node) NRun.Instance.GlobalUi.MessyCardPreviewContainer).GetChildCount(false) + ((Node) NRun.Instance.GlobalUi.EventCardPreviewContainer).GetChildCount(false) + ((Node) NRun.Instance.GlobalUi.GridCardPreviewContainer).GetChildCount(false);
    return cardsBeingPreviewed;
  }

  private static Task FlashRelics(NCard node, IEnumerable<RelicModel>? relicsToFlash)
  {
    if (relicsToFlash == null)
      return Task.CompletedTask;
    foreach (RelicModel relic in relicsToFlash)
    {
      relic.Flash();
      node.FlashRelicOnCard(relic);
    }
    return Task.CompletedTask;
  }
}
