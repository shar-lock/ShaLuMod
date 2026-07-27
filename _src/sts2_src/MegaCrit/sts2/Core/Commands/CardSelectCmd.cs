// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Commands.CardSelectCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Exceptions;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Commands;

public static class CardSelectCmd
{
  private static readonly Stack<MegaCrit.Sts2.Core.TestSupport.ICardSelector> _selectorStack = new Stack<MegaCrit.Sts2.Core.TestSupport.ICardSelector>();
  private static readonly Stack<MegaCrit.Sts2.Core.TestSupport.ICardSelector> _localSelectorStack = new Stack<MegaCrit.Sts2.Core.TestSupport.ICardSelector>();

  public static void Reset()
  {
    if (TestMode.IsOn || CardSelectCmd._selectorStack.Count <= 0 && CardSelectCmd._localSelectorStack.Count <= 0)
      return;
    Log.Warn($"CardSelectCmd.Reset: clearing {CardSelectCmd._selectorStack.Count}/{CardSelectCmd._localSelectorStack.Count} leaked selector(s) from the stack.");
    CardSelectCmd._selectorStack.Clear();
    CardSelectCmd._localSelectorStack.Clear();
  }

  public static MegaCrit.Sts2.Core.TestSupport.ICardSelector? Selector
  {
    get
    {
      return CardSelectCmd._selectorStack.Count <= 0 ? (MegaCrit.Sts2.Core.TestSupport.ICardSelector) null : CardSelectCmd._selectorStack.Peek();
    }
  }

  public static MegaCrit.Sts2.Core.TestSupport.ICardSelector? LocalSelector
  {
    get
    {
      return CardSelectCmd._localSelectorStack.Count <= 0 ? (MegaCrit.Sts2.Core.TestSupport.ICardSelector) null : CardSelectCmd._localSelectorStack.Peek();
    }
  }

  public static IDisposable UseSelector(MegaCrit.Sts2.Core.TestSupport.ICardSelector selector, bool localOnly = false)
  {
    Stack<MegaCrit.Sts2.Core.TestSupport.ICardSelector> stack = localOnly ? CardSelectCmd._localSelectorStack : CardSelectCmd._selectorStack;
    if (stack.Count > 0)
      throw new InvalidOperationException("A card selector is already active.");
    stack.Push(selector);
    return (IDisposable) new CardSelectCmd.SelectorScope(stack);
  }

  public static IDisposable PushSelector(MegaCrit.Sts2.Core.TestSupport.ICardSelector selector, bool localOnly = false)
  {
    Stack<MegaCrit.Sts2.Core.TestSupport.ICardSelector> stack = localOnly ? CardSelectCmd._localSelectorStack : CardSelectCmd._selectorStack;
    stack.Push(selector);
    return (IDisposable) new CardSelectCmd.StackedSelectorScope(stack, selector);
  }

  public static IDisposable SuspendSelectorForTest(bool localOnly = false)
  {
    Stack<MegaCrit.Sts2.Core.TestSupport.ICardSelector> stack = localOnly ? CardSelectCmd._localSelectorStack : CardSelectCmd._selectorStack;
    if (stack.Count == 0)
      return (IDisposable) new CardSelectCmd.NoOpScope();
    MegaCrit.Sts2.Core.TestSupport.ICardSelector saved = stack.Pop();
    return (IDisposable) new CardSelectCmd.RestoreSelectorScope(stack, saved);
  }

  private static bool ShouldSelectLocalCard(Player player)
  {
    return LocalContext.IsMe(player) && RunManager.Instance.NetService.Type != NetGameType.Replay;
  }

  private static void ReportSoftlock()
  {
    string str = "A selection screen was about to be shown with 0 options. Returning empty to prevent softlock.";
    Log.Error(str);
    SentryService.CaptureException((Exception) new SoftlockException(str));
  }

  public static async Task<CardModel?> FromChooseACardScreen(
    PlayerChoiceContext context,
    IReadOnlyList<CardModel> cards,
    Player player,
    bool canSkip = false)
  {
    if (cards.Count > 3)
      throw new ArgumentException("Only works with less than 3 cards", nameof (cards));
    if (cards.Count == 0)
    {
      CardSelectCmd.ReportSoftlock();
      return (CardModel) null;
    }
    CardModel result;
    if (CardSelectCmd.Selector != null)
    {
      result = (await CardSelectCmd.Selector.GetSelectedCards((IEnumerable<CardModel>) cards, 0, 1)).FirstOrDefault<CardModel>();
    }
    else
    {
      uint choiceId = RunManager.Instance.PlayerChoiceSynchronizer.ReserveChoiceId(player);
      await context.SignalPlayerChoiceBegun(player, PlayerChoiceOptions.None);
      if (CardSelectCmd.ShouldSelectLocalCard(player))
      {
        if (CardSelectCmd.LocalSelector != null)
        {
          result = (await CardSelectCmd.LocalSelector.GetSelectedCards((IEnumerable<CardModel>) cards, 0, 1)).FirstOrDefault<CardModel>();
        }
        else
        {
          NPlayerHand.Instance?.CancelAllCardPlay();
          NChooseACardSelectionScreen acardSelectionScreen = NChooseACardSelectionScreen.ShowScreen(cards, canSkip);
          if (LocalContext.IsMe(player))
          {
            foreach (CardModel card in (IEnumerable<CardModel>) cards)
              SaveManager.Instance.MarkCardAsSeen(card);
          }
          result = (await acardSelectionScreen.CardsSelected()).FirstOrDefault<CardModel>();
          RunManager.Instance.PlayerChoiceSynchronizer.SyncLocalChoice(player, choiceId, PlayerChoiceResult.FromIndex(new int?(cards.IndexOf<CardModel>(result))));
        }
      }
      else
      {
        int index = (await RunManager.Instance.PlayerChoiceSynchronizer.WaitForRemoteChoice(player, choiceId)).AsIndex();
        result = index < 0 ? (CardModel) null : cards[index];
      }
      await context.SignalPlayerChoiceEnded();
    }
    // ISSUE: object of a compiler-generated type is created
    CardSelectCmd.LogChoice(player, (IEnumerable<CardModel>) new \u003C\u003Ez__ReadOnlySingleElementList<CardModel>(result));
    return result;
  }

  public static async Task<IEnumerable<CardModel>> FromSimpleGridForRewards(
    PlayerChoiceContext context,
    List<CardCreationResult> cards,
    Player player,
    CardSelectorPrefs prefs)
  {
    if (CombatManager.Instance.IsEnding)
      return (IEnumerable<CardModel>) Array.Empty<CardModel>();
    if (cards.Count == 0)
    {
      CardSelectCmd.ReportSoftlock();
      return (IEnumerable<CardModel>) Array.Empty<CardModel>();
    }
    List<CardModel> result;
    if (!prefs.RequireManualConfirmation && cards.Count <= prefs.MinSelect)
      result = cards.Select<CardCreationResult, CardModel>((Func<CardCreationResult, CardModel>) (c => c.Card)).ToList<CardModel>();
    else if (CardSelectCmd.Selector != null)
    {
      result = (await CardSelectCmd.Selector.GetSelectedCards(cards.Select<CardCreationResult, CardModel>((Func<CardCreationResult, CardModel>) (c => c.Card)), prefs.MinSelect, prefs.MaxSelect)).ToList<CardModel>();
    }
    else
    {
      uint choiceId = RunManager.Instance.PlayerChoiceSynchronizer.ReserveChoiceId(player);
      await context.SignalPlayerChoiceBegun(player, PlayerChoiceOptions.None);
      if (CardSelectCmd.ShouldSelectLocalCard(player))
      {
        if (CardSelectCmd.LocalSelector != null)
        {
          result = (await CardSelectCmd.LocalSelector.GetSelectedCards(cards.Select<CardCreationResult, CardModel>((Func<CardCreationResult, CardModel>) (c => c.Card)), prefs.MinSelect, prefs.MaxSelect)).ToList<CardModel>();
        }
        else
        {
          NSimpleCardSelectScreen screen = NSimpleCardSelectScreen.Create((IReadOnlyList<CardCreationResult>) cards, prefs);
          NOverlayStack.Instance.Push((IOverlayScreen) screen);
          result = (await screen.CardsSelected()).ToList<CardModel>();
          RunManager.Instance.PlayerChoiceSynchronizer.SyncLocalChoice(player, choiceId, PlayerChoiceResult.FromIndexes(result.Select<CardModel, int>((Func<CardModel, int>) (c => cards.FindIndex((Predicate<CardCreationResult>) (r => r.Card == c)))).ToList<int>()));
        }
      }
      else
        result = (await RunManager.Instance.PlayerChoiceSynchronizer.WaitForRemoteChoice(player, choiceId)).AsIndexes().Select<int, CardModel>((Func<int, CardModel>) (i => cards[i].Card)).ToList<CardModel>();
      await context.SignalPlayerChoiceEnded();
    }
    CardSelectCmd.LogChoice(player, (IEnumerable<CardModel>) result);
    return (IEnumerable<CardModel>) result;
  }

  public static async Task<IEnumerable<CardModel>> FromSimpleGrid(
    PlayerChoiceContext context,
    IReadOnlyList<CardModel> cardsIn,
    Player player,
    CardSelectorPrefs prefs)
  {
    if (CombatManager.Instance.IsEnding)
      return (IEnumerable<CardModel>) Array.Empty<CardModel>();
    List<CardModel> cards = cardsIn.ToList<CardModel>();
    if (cards.Count == 0)
      return (IEnumerable<CardModel>) Array.Empty<CardModel>();
    List<CardModel> result;
    if (!prefs.RequireManualConfirmation && cards.Count <= prefs.MinSelect)
      result = cards.ToList<CardModel>();
    else if (CardSelectCmd.Selector != null)
    {
      result = (await CardSelectCmd.Selector.GetSelectedCards((IEnumerable<CardModel>) cards, prefs.MinSelect, prefs.MaxSelect)).ToList<CardModel>();
    }
    else
    {
      uint choiceId = RunManager.Instance.PlayerChoiceSynchronizer.ReserveChoiceId(player);
      await context.SignalPlayerChoiceBegun(player, PlayerChoiceOptions.None);
      if (CardSelectCmd.ShouldSelectLocalCard(player))
      {
        if (CardSelectCmd.LocalSelector != null)
        {
          result = (await CardSelectCmd.LocalSelector.GetSelectedCards((IEnumerable<CardModel>) cards, prefs.MinSelect, prefs.MaxSelect)).ToList<CardModel>();
        }
        else
        {
          NPlayerHand.Instance?.CancelAllCardPlay();
          NSimpleCardSelectScreen screen = NSimpleCardSelectScreen.Create((IReadOnlyList<CardModel>) cards, prefs);
          NOverlayStack.Instance.Push((IOverlayScreen) screen);
          result = (await screen.CardsSelected()).ToList<CardModel>();
          RunManager.Instance.PlayerChoiceSynchronizer.SyncLocalChoice(player, choiceId, PlayerChoiceResult.FromIndexes(result.Select<CardModel, int>((Func<CardModel, int>) (c => cards.IndexOf(c))).ToList<int>()));
        }
      }
      else
        result = (await RunManager.Instance.PlayerChoiceSynchronizer.WaitForRemoteChoice(player, choiceId)).AsIndexes().Select<int, CardModel>((Func<int, CardModel>) (i => cards[i])).ToList<CardModel>();
      await context.SignalPlayerChoiceEnded();
    }
    CardSelectCmd.LogChoice(player, (IEnumerable<CardModel>) result);
    return (IEnumerable<CardModel>) result;
  }

  public static async Task<IEnumerable<CardModel>> FromCombatPile(
    PlayerChoiceContext context,
    CardPile pile,
    Player player,
    CardSelectorPrefs prefs)
  {
    return await CardSelectCmd.FromCombatPile(context, pile, player, prefs, (Func<CardModel, bool>) (_ => true));
  }

  public static async Task<IEnumerable<CardModel>> FromCombatPile(
    PlayerChoiceContext context,
    CardPile pile,
    Player player,
    CardSelectorPrefs prefs,
    Func<CardModel, bool>? filter)
  {
    if (CombatManager.Instance.IsEnding)
      return (IEnumerable<CardModel>) Array.Empty<CardModel>();
    if (!pile.IsCombatPile)
      throw new InvalidOperationException("Cannot perform on a non combat pile");
    IEnumerable<CardModel> filtered = filter != null ? (IEnumerable<CardModel>) pile.Cards.Where<CardModel>(filter).ToList<CardModel>() : (IEnumerable<CardModel>) pile.Cards;
    int num = filtered.Count<CardModel>();
    if (num == 0)
      return (IEnumerable<CardModel>) Array.Empty<CardModel>();
    IEnumerable<CardModel> result;
    if (!prefs.RequireManualConfirmation && num <= prefs.MinSelect)
      result = filtered;
    else if (CardSelectCmd.Selector != null)
    {
      if (pile.Type == PileType.Draw)
        filtered = (IEnumerable<CardModel>) filtered.OrderBy<CardModel, CardRarity>((Func<CardModel, CardRarity>) (c => c.Rarity)).ThenBy<CardModel, ModelId>((Func<CardModel, ModelId>) (c => c.Id));
      result = (IEnumerable<CardModel>) (await CardSelectCmd.Selector.GetSelectedCards(filtered, prefs.MinSelect, prefs.MaxSelect)).ToList<CardModel>();
    }
    else
    {
      uint choiceId = RunManager.Instance.PlayerChoiceSynchronizer.ReserveChoiceId(player);
      await context.SignalPlayerChoiceBegun(player, PlayerChoiceOptions.None);
      if (CardSelectCmd.ShouldSelectLocalCard(player))
      {
        if (CardSelectCmd.LocalSelector != null)
        {
          if (pile.Type == PileType.Draw)
            filtered = (IEnumerable<CardModel>) filtered.OrderBy<CardModel, CardRarity>((Func<CardModel, CardRarity>) (c => c.Rarity)).ThenBy<CardModel, ModelId>((Func<CardModel, ModelId>) (c => c.Id));
          result = (IEnumerable<CardModel>) (await CardSelectCmd.LocalSelector.GetSelectedCards(filtered, prefs.MinSelect, prefs.MaxSelect)).ToList<CardModel>();
        }
        else
        {
          NPlayerHand.Instance?.CancelAllCardPlay();
          NCombatPileCardSelectScreen screen = NCombatPileCardSelectScreen.Create(pile, prefs, filter);
          NOverlayStack.Instance.Push((IOverlayScreen) screen);
          result = (IEnumerable<CardModel>) (await screen.CardsSelected()).ToList<CardModel>();
          RunManager.Instance.PlayerChoiceSynchronizer.SyncLocalChoice(player, choiceId, PlayerChoiceResult.FromMutableCombatCards(result));
        }
      }
      else
        result = (await RunManager.Instance.PlayerChoiceSynchronizer.WaitForRemoteChoice(player, choiceId)).AsCombatCards();
      await context.SignalPlayerChoiceEnded();
    }
    CardSelectCmd.LogChoice(player, result);
    return result;
  }

  public static async Task<IEnumerable<CardModel>> FromDeckForUpgrade(
    Player player,
    CardSelectorPrefs prefs)
  {
    List<CardModel> list = PileType.Deck.GetPile(player).Cards.Where<CardModel>((Func<CardModel, bool>) (c => c.IsUpgradable)).ToList<CardModel>();
    if (list.Count == 0)
      return (IEnumerable<CardModel>) Array.Empty<CardModel>();
    IEnumerable<CardModel> cardModels;
    if (list.Count <= prefs.MinSelect && !prefs.RequireManualConfirmation)
      cardModels = (IEnumerable<CardModel>) list;
    else if (CardSelectCmd.Selector != null)
    {
      cardModels = await CardSelectCmd.Selector.GetSelectedCards((IEnumerable<CardModel>) list, prefs.MinSelect, prefs.MaxSelect);
    }
    else
    {
      uint choiceId = RunManager.Instance.PlayerChoiceSynchronizer.ReserveChoiceId(player);
      if (CardSelectCmd.ShouldSelectLocalCard(player))
      {
        if (CardSelectCmd.LocalSelector != null)
        {
          cardModels = await CardSelectCmd.LocalSelector.GetSelectedCards((IEnumerable<CardModel>) list, prefs.MinSelect, prefs.MaxSelect);
        }
        else
        {
          cardModels = await NDeckUpgradeSelectScreen.ShowScreen((IReadOnlyList<CardModel>) list, prefs, player.RunState).CardsSelected();
          RunManager.Instance.PlayerChoiceSynchronizer.SyncLocalChoice(player, choiceId, PlayerChoiceResult.FromMutableDeckCards(cardModels));
        }
      }
      else
        cardModels = (await RunManager.Instance.PlayerChoiceSynchronizer.WaitForRemoteChoice(player, choiceId)).AsDeckCards();
    }
    CardSelectCmd.LogChoice(player, cardModels);
    return cardModels;
  }

  public static async Task<IEnumerable<CardModel>> FromDeckForTransformation(
    Player player,
    CardSelectorPrefs prefs,
    Func<CardModel, CardTransformation>? cardToTransformation = null)
  {
    List<CardModel> list = PileType.Deck.GetPile(player).Cards.Where<CardModel>((Func<CardModel, bool>) (c => c.Type != CardType.Quest && c.IsTransformable)).ToList<CardModel>();
    if (list.Count == 0)
      return (IEnumerable<CardModel>) Array.Empty<CardModel>();
    IEnumerable<CardModel> cardModels;
    if (list.Count <= prefs.MinSelect && !prefs.RequireManualConfirmation)
      cardModels = (IEnumerable<CardModel>) list;
    else if (CardSelectCmd.Selector != null)
    {
      cardModels = await CardSelectCmd.Selector.GetSelectedCards((IEnumerable<CardModel>) list, prefs.MinSelect, prefs.MaxSelect);
    }
    else
    {
      uint choiceId = RunManager.Instance.PlayerChoiceSynchronizer.ReserveChoiceId(player);
      if (CardSelectCmd.ShouldSelectLocalCard(player))
      {
        if (CardSelectCmd.LocalSelector != null)
        {
          cardModels = await CardSelectCmd.LocalSelector.GetSelectedCards((IEnumerable<CardModel>) list, prefs.MinSelect, prefs.MaxSelect);
        }
        else
        {
          if (cardToTransformation == null)
            cardToTransformation = (Func<CardModel, CardTransformation>) (c => new CardTransformation(c));
          cardModels = await NDeckTransformSelectScreen.ShowScreen((IReadOnlyList<CardModel>) list, cardToTransformation, prefs).CardsSelected();
          RunManager.Instance.PlayerChoiceSynchronizer.SyncLocalChoice(player, choiceId, PlayerChoiceResult.FromMutableDeckCards(cardModels));
        }
      }
      else
        cardModels = (await RunManager.Instance.PlayerChoiceSynchronizer.WaitForRemoteChoice(player, choiceId)).AsDeckCards();
    }
    CardSelectCmd.LogChoice(player, cardModels);
    return cardModels;
  }

  public static async Task<IEnumerable<CardModel>> FromDeckForEnchantment(
    Player player,
    EnchantmentModel enchantment,
    int amount,
    CardSelectorPrefs prefs)
  {
    return await CardSelectCmd.FromDeckForEnchantment(player, enchantment, amount, (Func<CardModel, bool>) null, prefs);
  }

  public static async Task<IEnumerable<CardModel>> FromDeckForEnchantment(
    Player player,
    EnchantmentModel enchantment,
    int amount,
    Func<CardModel?, bool>? additionalFilter,
    CardSelectorPrefs prefs)
  {
    return await CardSelectCmd.FromDeckForEnchantment((IReadOnlyList<CardModel>) PileType.Deck.GetPile(player).Cards.Where<CardModel>((Func<CardModel, bool>) (c =>
    {
      if (!enchantment.CanEnchant(c))
        return false;
      Func<CardModel, bool> func = additionalFilter;
      return func == null || func(c);
    })).ToList<CardModel>(), enchantment, amount, prefs);
  }

  public static async Task<IEnumerable<CardModel>> FromDeckForEnchantment(
    IReadOnlyList<CardModel> cards,
    EnchantmentModel enchantment,
    int amount,
    CardSelectorPrefs prefs)
  {
    if (cards.Any<CardModel>((Func<CardModel, bool>) (c => c.Pile.Type != PileType.Deck || !enchantment.CanEnchant(c))))
      throw new ArgumentException("All cards must be in the player's deck and enchantable.");
    List<CardModel> cardModelList = new List<CardModel>();
    if (cards.Count > 0)
    {
      Dictionary<CardModel, int> indexMap = PileType.Deck.GetPile(cards[0].Owner).Cards.Select((card, index) => new
      {
        card = card,
        index = index
      }).ToDictionary(x => x.card, x => x.index);
      cardModelList = cards.OrderBy<CardModel, int>((Func<CardModel, int>) (c => indexMap[c])).ToList<CardModel>();
    }
    IEnumerable<CardModel> cardModels;
    if (cards.Count <= prefs.MinSelect)
      cardModels = (IEnumerable<CardModel>) cards;
    else if (CardSelectCmd.Selector != null)
    {
      cardModels = await CardSelectCmd.Selector.GetSelectedCards((IEnumerable<CardModel>) cardModelList, prefs.MinSelect, prefs.MaxSelect);
    }
    else
    {
      Player player = cards[0].Owner;
      if (player.Creature.IsDead)
        return (IEnumerable<CardModel>) Array.Empty<CardModel>();
      uint choiceId = RunManager.Instance.PlayerChoiceSynchronizer.ReserveChoiceId(player);
      if (CardSelectCmd.ShouldSelectLocalCard(player))
      {
        if (CardSelectCmd.LocalSelector != null)
        {
          cardModels = await CardSelectCmd.LocalSelector.GetSelectedCards((IEnumerable<CardModel>) cardModelList, prefs.MinSelect, prefs.MaxSelect);
        }
        else
        {
          cardModels = await NDeckEnchantSelectScreen.ShowScreen((IReadOnlyList<CardModel>) cardModelList, enchantment, amount, prefs).CardsSelected();
          RunManager.Instance.PlayerChoiceSynchronizer.SyncLocalChoice(player, choiceId, PlayerChoiceResult.FromMutableDeckCards(cardModels));
        }
      }
      else
        cardModels = (await RunManager.Instance.PlayerChoiceSynchronizer.WaitForRemoteChoice(player, choiceId)).AsDeckCards();
      player = (Player) null;
    }
    if (cards.Count > 0)
      CardSelectCmd.LogChoice(cards[0].Owner, cardModels);
    return cardModels;
  }

  public static Task<IEnumerable<CardModel>> FromDeckForRemoval(
    Player player,
    CardSelectorPrefs prefs,
    Func<CardModel, bool>? filter = null)
  {
    List<CardModel> deck = PileType.Deck.GetPile(player).Cards.ToList<CardModel>();
    return CardSelectCmd.FromDeckGeneric(player, prefs, (Func<CardModel, bool>) (c =>
    {
      if (!c.IsRemovable)
        return false;
      return filter == null || filter(c);
    }), (Func<CardModel, int>) (c => c.Type != CardType.Curse ? deck.IndexOf(c) : -999999999));
  }

  public static async Task<IEnumerable<CardModel>> FromDeckGeneric(
    Player player,
    CardSelectorPrefs prefs,
    Func<CardModel, bool>? filter = null,
    Func<CardModel, int>? sortingOrder = null)
  {
    List<CardModel> list = PileType.Deck.GetPile(player).Cards.ToList<CardModel>();
    List<CardModel> cardModelList = filter == null ? list.ToList<CardModel>() : list.Where<CardModel>(filter).ToList<CardModel>();
    if (player.Creature.IsDead)
      return (IEnumerable<CardModel>) Array.Empty<CardModel>();
    if (sortingOrder != null)
      cardModelList = cardModelList.OrderBy<CardModel, int>(sortingOrder).ToList<CardModel>();
    if (cardModelList.Count == 0)
      return (IEnumerable<CardModel>) Array.Empty<CardModel>();
    IEnumerable<CardModel> cardModels;
    if (!prefs.RequireManualConfirmation && cardModelList.Count <= prefs.MinSelect)
      cardModels = (IEnumerable<CardModel>) cardModelList;
    else if (CardSelectCmd.Selector != null)
    {
      cardModels = await CardSelectCmd.Selector.GetSelectedCards((IEnumerable<CardModel>) cardModelList, prefs.MinSelect, prefs.MaxSelect);
    }
    else
    {
      uint choiceId = RunManager.Instance.PlayerChoiceSynchronizer.ReserveChoiceId(player);
      if (CardSelectCmd.ShouldSelectLocalCard(player))
      {
        if (CardSelectCmd.LocalSelector != null)
        {
          cardModels = await CardSelectCmd.LocalSelector.GetSelectedCards((IEnumerable<CardModel>) cardModelList, prefs.MinSelect, prefs.MaxSelect);
        }
        else
        {
          NDeckCardSelectScreen screen = NDeckCardSelectScreen.Create((IReadOnlyList<CardModel>) cardModelList, prefs);
          NOverlayStack.Instance.Push((IOverlayScreen) screen);
          cardModels = await screen.CardsSelected();
          RunManager.Instance.PlayerChoiceSynchronizer.SyncLocalChoice(player, choiceId, PlayerChoiceResult.FromMutableDeckCards(cardModels));
        }
      }
      else
        cardModels = (await RunManager.Instance.PlayerChoiceSynchronizer.WaitForRemoteChoice(player, choiceId)).AsDeckCards();
    }
    CardSelectCmd.LogChoice(player, cardModels);
    return cardModels;
  }

  public static async Task<IEnumerable<CardModel>> FromHand(
    PlayerChoiceContext context,
    Player player,
    CardSelectorPrefs prefs,
    Func<CardModel, bool>? filter,
    AbstractModel source)
  {
    if (CombatManager.Instance.IsOverOrEnding)
      return (IEnumerable<CardModel>) Array.Empty<CardModel>();
    if (CardSelectCmd.ShouldSelectLocalCard(player))
      NPlayerHand.Instance?.CancelAllCardPlay();
    List<CardModel> cards = PileType.Hand.GetPile(player).Cards.Where<CardModel>(filter ?? (Func<CardModel, bool>) (_ => true)).ToList<CardModel>();
    IEnumerable<CardModel> result;
    if (cards.Count == 0)
      result = (IEnumerable<CardModel>) cards;
    else if (!prefs.RequireManualConfirmation && cards.Count <= prefs.MinSelect)
      result = (IEnumerable<CardModel>) cards;
    else if (CardSelectCmd.Selector != null)
    {
      result = await CardSelectCmd.Selector.GetSelectedCards((IEnumerable<CardModel>) cards, prefs.MinSelect, prefs.MaxSelect);
    }
    else
    {
      uint choiceId = RunManager.Instance.PlayerChoiceSynchronizer.ReserveChoiceId(player);
      await context.SignalPlayerChoiceBegun(player, PlayerChoiceOptions.CancelPlayCardActions);
      if (CardSelectCmd.ShouldSelectLocalCard(player))
      {
        if (CardSelectCmd.LocalSelector != null)
        {
          result = await CardSelectCmd.LocalSelector.GetSelectedCards((IEnumerable<CardModel>) cards, prefs.MinSelect, prefs.MaxSelect);
        }
        else
        {
          result = await NCombatRoom.Instance.Ui.Hand.SelectCards(prefs, filter, source);
          RunManager.Instance.PlayerChoiceSynchronizer.SyncLocalChoice(player, choiceId, PlayerChoiceResult.FromMutableCombatCards(result));
        }
      }
      else
        result = (await RunManager.Instance.PlayerChoiceSynchronizer.WaitForRemoteChoice(player, choiceId)).AsCombatCards();
      await context.SignalPlayerChoiceEnded();
    }
    CardSelectCmd.LogChoice(player, result);
    return result;
  }

  public static async Task<IEnumerable<CardModel>> FromHandForDiscard(
    PlayerChoiceContext context,
    Player player,
    CardSelectorPrefs prefs,
    Func<CardModel, bool>? filter,
    AbstractModel source)
  {
    prefs.ShouldGlowGold = (Func<CardModel, bool>) (c =>
    {
      if (!c.IsSlyThisTurn)
        return false;
      UnplayableReason reason;
      return c.CanPlay(out reason, out AbstractModel _) || reason.HasResourceCostReason();
    });
    return await CardSelectCmd.FromHand(context, player, prefs, filter, source);
  }

  public static async Task<CardModel?> FromHandForUpgrade(
    PlayerChoiceContext context,
    Player player,
    AbstractModel source)
  {
    if (CombatManager.Instance.IsOverOrEnding)
      return (CardModel) null;
    if (CardSelectCmd.ShouldSelectLocalCard(player))
      NPlayerHand.Instance?.CancelAllCardPlay();
    List<CardModel> cards = PileType.Hand.GetPile(player).Cards.Where<CardModel>((Func<CardModel, bool>) (c => c.IsUpgradable)).ToList<CardModel>();
    CardModel result;
    if (cards.Count <= 1)
      result = cards.FirstOrDefault<CardModel>();
    else if (CardSelectCmd.Selector != null)
    {
      result = (await CardSelectCmd.Selector.GetSelectedCards((IEnumerable<CardModel>) cards, 1, 1)).FirstOrDefault<CardModel>();
    }
    else
    {
      uint choiceId = RunManager.Instance.PlayerChoiceSynchronizer.ReserveChoiceId(player);
      await context.SignalPlayerChoiceBegun(player, PlayerChoiceOptions.CancelPlayCardActions);
      if (CardSelectCmd.ShouldSelectLocalCard(player))
      {
        if (CardSelectCmd.LocalSelector != null)
        {
          result = (await CardSelectCmd.LocalSelector.GetSelectedCards((IEnumerable<CardModel>) cards, 1, 1)).FirstOrDefault<CardModel>();
        }
        else
        {
          result = (await NCombatRoom.Instance.Ui.Hand.SelectCards(new CardSelectorPrefs(new LocString("gameplay_ui", "CHOOSE_CARD_UPGRADE_HEADER"), 1), (Func<CardModel, bool>) (c => c.IsUpgradable), source, NPlayerHand.Mode.UpgradeSelect)).FirstOrDefault<CardModel>();
          RunManager.Instance.PlayerChoiceSynchronizer.SyncLocalChoice(player, choiceId, PlayerChoiceResult.FromMutableCombatCard(result));
        }
      }
      else
        result = (await RunManager.Instance.PlayerChoiceSynchronizer.WaitForRemoteChoice(player, choiceId)).AsCombatCards().FirstOrDefault<CardModel>();
      await context.SignalPlayerChoiceEnded();
    }
    // ISSUE: object of a compiler-generated type is created
    CardSelectCmd.LogChoice(player, (IEnumerable<CardModel>) new \u003C\u003Ez__ReadOnlySingleElementList<CardModel>(result));
    return result;
  }

  public static async Task<IEnumerable<CardModel>> FromChooseABundleScreen(
    Player player,
    IReadOnlyList<IReadOnlyList<CardModel>> bundles)
  {
    if (CombatManager.Instance.IsEnding)
      return (IEnumerable<CardModel>) Array.Empty<CardModel>();
    if (bundles.Count == 0)
    {
      CardSelectCmd.ReportSoftlock();
      return (IEnumerable<CardModel>) Array.Empty<CardModel>();
    }
    uint choiceId = RunManager.Instance.PlayerChoiceSynchronizer.ReserveChoiceId(player);
    IReadOnlyList<CardModel> cards;
    if (TestMode.IsOn)
      cards = bundles[0];
    else if (CardSelectCmd.ShouldSelectLocalCard(player))
    {
      cards = (IReadOnlyList<CardModel>) ((object) (await NChooseABundleSelectionScreen.ShowScreen(bundles).CardsSelected()).FirstOrDefault<IReadOnlyList<CardModel>>() ?? (object) Array.Empty<CardModel>());
      RunManager.Instance.PlayerChoiceSynchronizer.SyncLocalChoice(player, choiceId, PlayerChoiceResult.FromIndex(new int?(bundles.IndexOf<IReadOnlyList<CardModel>>(cards))));
    }
    else
    {
      int index = (await RunManager.Instance.PlayerChoiceSynchronizer.WaitForRemoteChoice(player, choiceId)).AsIndex();
      cards = index < 0 ? (IReadOnlyList<CardModel>) Array.Empty<CardModel>() : bundles[index];
    }
    CardSelectCmd.LogChoice(player, (IEnumerable<CardModel>) cards);
    return (IEnumerable<CardModel>) cards;
  }

  private static void LogChoice(Player player, IEnumerable<CardModel?> cards)
  {
    string str = string.Join(",", cards.OfType<CardModel>().Select<CardModel, string>((Func<CardModel, string>) (c => c.Id.Entry)));
    Log.Info($"Player {player.NetId} chose cards [{str}]");
  }

  private sealed class StackedSelectorScope : IDisposable
  {
    private readonly MegaCrit.Sts2.Core.TestSupport.ICardSelector _selector;
    private readonly Stack<MegaCrit.Sts2.Core.TestSupport.ICardSelector> _stack;
    private bool _disposed;

    public StackedSelectorScope(Stack<MegaCrit.Sts2.Core.TestSupport.ICardSelector> stack, MegaCrit.Sts2.Core.TestSupport.ICardSelector selector)
    {
      this._stack = stack;
      this._selector = selector;
    }

    public void Dispose()
    {
      if (this._disposed)
        return;
      this._disposed = true;
      if (this._stack.Count <= 0 || this._stack.Peek() != this._selector)
        return;
      this._stack.Pop();
    }
  }

  private sealed class NoOpScope : IDisposable
  {
    public void Dispose()
    {
    }
  }

  private sealed class RestoreSelectorScope : IDisposable
  {
    private readonly Stack<MegaCrit.Sts2.Core.TestSupport.ICardSelector> _stack;
    private readonly MegaCrit.Sts2.Core.TestSupport.ICardSelector _saved;
    private bool _disposed;

    public RestoreSelectorScope(Stack<MegaCrit.Sts2.Core.TestSupport.ICardSelector> stack, MegaCrit.Sts2.Core.TestSupport.ICardSelector saved)
    {
      this._stack = stack;
      this._saved = saved;
    }

    public void Dispose()
    {
      if (this._disposed)
        return;
      this._disposed = true;
      this._stack.Push(this._saved);
    }
  }

  private sealed class SelectorScope : IDisposable
  {
    private readonly Stack<MegaCrit.Sts2.Core.TestSupport.ICardSelector> _stack;
    private bool _disposed;

    public SelectorScope(Stack<MegaCrit.Sts2.Core.TestSupport.ICardSelector> stack)
    {
      this._stack = stack;
    }

    public void Dispose()
    {
      if (this._disposed)
        return;
      this._disposed = true;
      this._stack.Clear();
    }
  }
}
