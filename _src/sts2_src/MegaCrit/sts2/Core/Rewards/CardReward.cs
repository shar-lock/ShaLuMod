// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Rewards.CardReward
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.CardRewardAlternatives;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Rewards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Rewards;

public class CardReward : Reward
{
  private readonly PlayerChoiceSynchronizer _synchronizer;
  private readonly List<CardCreationResult> _cards = new List<CardCreationResult>();
  private bool _cardsWereManuallySet;
  private bool _hasBeenRerolled;
  private NCardRewardSelectionScreen? _currentlyShownScreen;

  private static string RareRewardIcon
  {
    get => ImageHelper.GetImagePath("ui/reward_screen/reward_icon_rare.png");
  }

  private static string UncommonRewardIcon
  {
    get => ImageHelper.GetImagePath("ui/reward_screen/reward_icon_uncommon.png");
  }

  private static string RewardIcon
  {
    get => ImageHelper.GetImagePath("ui/reward_screen/reward_icon_card.png");
  }

  protected override RewardType RewardType => RewardType.Card;

  public override int RewardsSetIndex => 5;

  protected override string IconPath
  {
    get
    {
      CardCreationOptions options = this.Options;
      if ((object) options != null && options.Source == CardCreationSource.Encounter && options.RarityOdds == CardRarityOddsType.BossEncounter)
        return CardReward.RareRewardIcon;
      if (!this.Options.TryGetSingleRarityInPool().HasValue)
        return CardReward.RewardIcon;
      string iconPath;
      switch (this._cards[0].Card.Rarity)
      {
        case CardRarity.Uncommon:
          iconPath = CardReward.UncommonRewardIcon;
          break;
        case CardRarity.Rare:
          iconPath = CardReward.RareRewardIcon;
          break;
        default:
          iconPath = CardReward.RewardIcon;
          break;
      }
      return iconPath;
    }
  }

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[3]
      {
        CardReward.RareRewardIcon,
        CardReward.UncommonRewardIcon,
        CardReward.RewardIcon
      });
    }
  }

  public override LocString Description => new LocString("gameplay_ui", "COMBAT_REWARD_ADD_CARD");

  public IEnumerable<CardModel> Cards
  {
    get
    {
      return this._cards.Select<CardCreationResult, CardModel>((Func<CardCreationResult, CardModel>) (e => e.Card));
    }
  }

  public event Action? AfterGenerated;

  private int OptionCount { get; }

  private CardCreationOptions Options { get; }

  private CardCreationOptions RerollOptions { get; }

  public bool CanReroll { get; set; }

  public bool CanSkip { get; init; } = true;

  public CardReward(
    CardCreationOptions options,
    int cardCount,
    Player player,
    PlayerChoiceSynchronizer? synchronizer = null)
    : base(player)
  {
    this.OptionCount = cardCount;
    this.Options = options.WithFlags(CardCreationFlags.IsCardReward);
    this.RerollOptions = options.WithFlags(CardCreationFlags.IsCardReward);
    this._synchronizer = synchronizer ?? RunManager.Instance.PlayerChoiceSynchronizer;
    player.RelicObtained += new Action<RelicModel>(this.OnRelicObtained);
  }

  public CardReward(
    IEnumerable<CardModel> cardsToOffer,
    CardCreationSource source,
    Player player,
    CardCreationOptions rerollOptions,
    PlayerChoiceSynchronizer? synchronizer = null)
    : base(player)
  {
    this.Options = new CardCreationOptions((IEnumerable<CardPoolModel>) Array.Empty<CardPoolModel>(), source, CardRarityOddsType.Uniform).WithFlags(CardCreationFlags.NoCardPoolModifications | CardCreationFlags.NoCardModelModifications | CardCreationFlags.IsCardReward);
    this.RerollOptions = rerollOptions;
    this._cardsWereManuallySet = true;
    this._cards = cardsToOffer.Select<CardModel, CardCreationResult>((Func<CardModel, CardCreationResult>) (c => new CardCreationResult(c))).ToList<CardCreationResult>();
    this.OptionCount = this._cards.Count;
    this._synchronizer = synchronizer ?? RunManager.Instance.PlayerChoiceSynchronizer;
  }

  public override bool IsPopulated => this._cards.Count > 0;

  public override void Populate()
  {
    CardCreationOptions cardCreationOptions = this._hasBeenRerolled ? this.RerollOptions : this.Options;
    if (this._cardsWereManuallySet && !this._hasBeenRerolled)
    {
      List<AbstractModel> modifiers;
      if (!Hook.TryModifyCardRewardOptions(this.Player.RunState, this.Player, this._cards, cardCreationOptions, out modifiers))
        return;
      TaskHelper.RunSafely(Hook.AfterModifyingCardRewardOptions(this.Player.RunState, (IEnumerable<AbstractModel>) modifiers));
    }
    else
    {
      if (this._cards.Count > 0)
        return;
      IEnumerable<CardCreationResult> forReward = CardFactory.CreateForReward(this.Player, this.OptionCount, cardCreationOptions);
      this._cards.Clear();
      this._cards.AddRange(forReward);
      IReadOnlyList<CardRewardAlternative> extraOptions = CardRewardAlternative.Generate(this);
      Action afterGenerated = this.AfterGenerated;
      if (afterGenerated != null)
        afterGenerated();
      this._currentlyShownScreen?.RefreshOptions((IReadOnlyList<CardCreationResult>) this._cards, extraOptions);
    }
  }

  private void OnRelicObtained(RelicModel relic)
  {
    if (this._cards == null)
      throw new InvalidOperationException("cards must be set first before you can update them");
    if (relic.TryModifyCardRewardOptions(this.Player, this._cards, this.Options))
      TaskHelper.RunSafely(relic.AfterModifyingRewards());
    if (!relic.TryModifyCardRewardOptionsLate(this.Player, this._cards, this.Options))
      return;
    TaskHelper.RunSafely(relic.AfterModifyingRewards());
  }

  protected override async Task<bool> OnSelect()
  {
    Log.Info($"Player {this.Player.NetId} selected card reward");
    bool rewardComplete = false;
    bool endSelection = false;
    List<CardModel> chosenCardIds = new List<CardModel>();
    IReadOnlyList<CardRewardAlternative> cardRewardOption = CardRewardAlternative.Generate(this);
    if (LocalContext.IsMe(this.Player))
      this._currentlyShownScreen = NCardRewardSelectionScreen.ShowScreen((IReadOnlyList<CardCreationResult>) this._cards, cardRewardOption);
    while (!endSelection)
    {
      uint choiceId = this._synchronizer.ReserveChoiceId(this.Player);
      int? index;
      int? nullable1;
      CardModel obtainedCard;
      if (LocalContext.IsMe(this.Player))
      {
        if (this._currentlyShownScreen != null)
        {
          index = await this._currentlyShownScreen.OptionSelected();
        }
        else
        {
          CardRewardSelection? selection = CardSelectCmd.Selector?.GetSelectedCardReward((IReadOnlyList<CardCreationResult>) this._cards, cardRewardOption);
          if (!selection.HasValue)
            throw new InvalidOperationException("Card selector unset during test!");
          if (selection.Value.alternative != null)
          {
            index = new int?(this._cards.Count + cardRewardOption.FirstIndex<CardRewardAlternative>((Predicate<CardRewardAlternative>) (r => r == selection.Value.alternative)));
          }
          else
          {
            obtainedCard = selection.Value.card;
            int? nullable2;
            if (obtainedCard == null)
            {
              nullable1 = new int?();
              nullable2 = nullable1;
            }
            else
              nullable2 = new int?(this._cards.FirstIndex<CardCreationResult>((Predicate<CardCreationResult>) (c => c.Card == selection.Value.card)));
            index = nullable2;
          }
        }
        PlayerChoiceResult result = PlayerChoiceResult.FromIndex(index);
        this._synchronizer.SyncLocalChoice(this.Player, choiceId, result);
      }
      else
        index = (await this._synchronizer.WaitForRemoteChoice(this.Player, choiceId)).AsIndexOrNull();
      NCardHolder cardHolder;
      CardRewardAlternative rewardAlternative;
      if (index.HasValue)
      {
        nullable1 = index;
        int count = this._cards.Count;
        if (nullable1.GetValueOrDefault() < count & nullable1.HasValue)
        {
          obtainedCard = this._cards[index.Value].Card;
          rewardComplete = true;
          endSelection = !Hook.ShouldAllowSelectingMoreCardRewards(this.Player.RunState, this.Player, this);
          cardHolder = this._currentlyShownScreen?.GetCardHolder(obtainedCard);
          rewardAlternative = (CardRewardAlternative) null;
        }
        else
        {
          nullable1 = index;
          int num = this._cards.Count + cardRewardOption.Count;
          if (nullable1.GetValueOrDefault() < num & nullable1.HasValue)
          {
            rewardAlternative = cardRewardOption[index.Value - this._cards.Count];
            rewardComplete = rewardAlternative.AfterSelected == PostAlternateCardRewardAction.EndSelectionAndCompleteReward;
            bool flag;
            switch (rewardAlternative.AfterSelected)
            {
              case PostAlternateCardRewardAction.EndSelectionAndDoNotCompleteReward:
              case PostAlternateCardRewardAction.EndSelectionAndCompleteReward:
                flag = true;
                break;
              default:
                flag = false;
                break;
            }
            endSelection = flag;
            cardHolder = (NCardHolder) null;
            obtainedCard = (CardModel) null;
          }
          else
          {
            Log.Error($"Received bad player choice index {index} for a card reward with {this._cards.Count} cards and {cardRewardOption.Count} alternatives!");
            continue;
          }
        }
      }
      else
      {
        rewardComplete = false;
        endSelection = true;
        cardHolder = (NCardHolder) null;
        obtainedCard = (CardModel) null;
        rewardAlternative = (CardRewardAlternative) null;
      }
      if (((obtainedCard != null ? 1 : (rewardAlternative != null ? 1 : 0)) | (rewardComplete ? 1 : 0)) != 0)
      {
        if (obtainedCard != null)
        {
          CardPileAddResult cardPileAddResult = await CardPileCmd.Add(obtainedCard, PileType.Deck);
          if (cardPileAddResult.success)
          {
            obtainedCard = cardPileAddResult.cardAdded;
            chosenCardIds.Add(obtainedCard);
            this._cards.RemoveAll((Predicate<CardCreationResult>) (c => c.Card == obtainedCard));
            if (cardHolder != null)
            {
              NCard cardNode = cardHolder.CardNode;
              NRun.Instance.GlobalUi.ReparentCard(cardNode);
              ((Node) cardHolder).QueueFreeSafely();
              NRun.Instance.GlobalUi.TopBar.TrailContainer.AddChildSafely((Node) NCardFlyVfx.Create(cardNode, PileType.Deck, true, obtainedCard.Owner.Character.TrailPath));
            }
            Log.Info($"Player {this.Player.NetId} obtained {obtainedCard.Id} from card reward");
          }
        }
        else if (rewardAlternative != null)
          await rewardAlternative.OnSelect();
      }
      cardHolder = (NCardHolder) null;
    }
    this.Player.RelicObtained -= new Action<RelicModel>(this.OnRelicObtained);
    foreach (CardModel card in chosenCardIds)
      this.Player.RunState.CurrentMapPointHistoryEntry.GetEntry(this.Player.NetId).CardChoices.Add(new CardChoiceHistoryEntry(card, true));
    if (rewardComplete)
    {
      foreach (CardCreationResult card in this._cards)
        this.Player.RunState.CurrentMapPointHistoryEntry.GetEntry(this.Player.NetId).CardChoices.Add(new CardChoiceHistoryEntry(card.Card, false));
    }
    if (this._currentlyShownScreen != null)
    {
      NOverlayStack.Instance?.Remove((IOverlayScreen) this._currentlyShownScreen);
      this._currentlyShownScreen = (NCardRewardSelectionScreen) null;
    }
    bool flag1 = rewardComplete;
    chosenCardIds = (List<CardModel>) null;
    cardRewardOption = (IReadOnlyList<CardRewardAlternative>) null;
    return flag1;
  }

  public override void OnSkipped()
  {
    foreach (CardCreationResult card in this._cards)
      this.Player.RunState.CurrentMapPointHistoryEntry.GetEntry(this.Player.NetId).CardChoices.Add(new CardChoiceHistoryEntry(card.Card, false));
    this.Player.RelicObtained -= new Action<RelicModel>(this.OnRelicObtained);
  }

  public void Reroll()
  {
    this.CanReroll = false;
    foreach (CardCreationResult card in this._cards)
      this.Player.RunState.CurrentMapPointHistoryEntry.GetEntry(this.Player.NetId).CardChoices.Add(new CardChoiceHistoryEntry(card.Card, false));
    this._hasBeenRerolled = true;
    this._cards.Clear();
    this.Populate();
  }

  public override SerializableReward ToSerializable()
  {
    if (this.Options.CardPools.Count <= 0)
      throw new NotImplementedException("Tried to serialize a CardReward without any card pools! This is not currently supported.");
    if (this.Options.CardPoolFilter != null)
      throw new NotImplementedException("Tried to serialize a CardReward with a card pool filter! This is not currently supported.");
    CardCreationFlags cardCreationFlags = this.Options.Flags & ~CardCreationFlags.IsCardReward;
    if (cardCreationFlags != ~CardCreationFlags.NoModifications)
      throw new NotImplementedException("Tried to serialize a CardReward with card creation flags! " + $"This is not currently supported. Flags: {cardCreationFlags}");
    return new SerializableReward()
    {
      RewardType = RewardType.Card,
      Source = this.Options.Source,
      RarityOdds = this.Options.RarityOdds,
      CardPoolIds = this.Options.CardPools.Select<CardPoolModel, ModelId>((Func<CardPoolModel, ModelId>) (p => p.Id)).ToList<ModelId>(),
      OptionCount = this.OptionCount
    };
  }

  public override void MarkContentAsSeen()
  {
  }
}
