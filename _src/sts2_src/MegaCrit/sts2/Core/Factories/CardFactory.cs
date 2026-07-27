// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Factories.CardFactory
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

#nullable enable
namespace MegaCrit.Sts2.Core.Factories;

public static class CardFactory
{
  private static Decimal UpgradedCardOddScaling
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.Scarcity, 0.125M, 0.25M);
  }

  private static IEnumerable<CardModel> FilterForPlayerCount(
    IRunState runState,
    IEnumerable<CardModel> options)
  {
    return runState.Players.Count > 1 ? options.Where<CardModel>((Func<CardModel, bool>) (c => c.MultiplayerConstraint != CardMultiplayerConstraint.SingleplayerOnly)) : options.Where<CardModel>((Func<CardModel, bool>) (c => c.MultiplayerConstraint != CardMultiplayerConstraint.MultiplayerOnly));
  }

  public static CardCreationResult CreateForMerchant(
    Player player,
    IEnumerable<CardModel> options,
    CardType type)
  {
    if (player.Character is Deprived)
      throw new InvalidOperationException("Merchant inventory can't be generated for the test character. Update your test to use Ironclad.");
    options = Hook.ModifyMerchantCardPool(player.RunState, player, options);
    options = options.Where<CardModel>((Func<CardModel, bool>) (c => c.Rarity != CardRarity.Basic));
    options = CardFactory.FilterForPlayerCount(player.RunState, options);
    CardModel[] optionsArr = options.ToArray<CardModel>();
    CardRarity rarity = CardFactory.GetNextAllowedRarity(Hook.ModifyMerchantCardRarity(player.RunState, player, player.PlayerOdds.CardRarity.RollWithoutChangingFutureOdds(CardRarityOddsType.Shop)), (Func<CardRarity, bool>) (r => ((IEnumerable<CardModel>) optionsArr).Any<CardModel>((Func<CardModel, bool>) (c => c.Rarity == r && c.Type == type))));
    if (rarity == CardRarity.None)
      throw new InvalidOperationException($"Can't generate valid rarity for merchant card type {type} with card options: {string.Join<ModelId>(",", ((IEnumerable<CardModel>) optionsArr).Select<CardModel, ModelId>((Func<CardModel, ModelId>) (c => c.Id)))}");
    List<CardModel> list = ((IEnumerable<CardModel>) optionsArr).Where<CardModel>((Func<CardModel, bool>) (c => c.Rarity == rarity && c.Type == type)).ToList<CardModel>();
    CardModel card = player.RunState.CreateCard(player.PlayerRng.Shops.NextItem<CardModel>((IEnumerable<CardModel>) list), player);
    CardFactory.RollForUpgrade(player, card, -999999999M);
    return new CardCreationResult(card);
  }

  public static CardCreationResult CreateForMerchant(
    Player player,
    IEnumerable<CardModel> options,
    CardRarity rarity)
  {
    options = Hook.ModifyMerchantCardPool(player.RunState, player, options);
    options = options.Where<CardModel>((Func<CardModel, bool>) (c => c.Rarity != CardRarity.Basic));
    options = CardFactory.FilterForPlayerCount(player.RunState, options);
    CardModel[] array = options.ToArray<CardModel>();
    CardRarity modifiedRarity = Hook.ModifyMerchantCardRarity(player.RunState, player, rarity);
    IEnumerable<CardModel> items = ((IEnumerable<CardModel>) array).Where<CardModel>((Func<CardModel, bool>) (c => c.Rarity == modifiedRarity));
    CardModel card = player.RunState.CreateCard(player.PlayerRng.Shops.NextItem<CardModel>(items), player);
    CardFactory.RollForUpgrade(player, card, -999999999M);
    return new CardCreationResult(card);
  }

  public static IEnumerable<CardCreationResult> CreateForReward(
    Player player,
    int cardCount,
    CardCreationOptions options)
  {
    List<CardModel> blacklist = new List<CardModel>();
    List<CardCreationResult> cardRewardOptions = new List<CardCreationResult>();
    for (int index = 0; index < cardCount; ++index)
    {
      CardModel forReward = CardFactory.CreateForReward(player, (IEnumerable<CardModel>) blacklist, options);
      blacklist.Add(forReward.CanonicalInstance);
      cardRewardOptions.Add(new CardCreationResult(forReward));
      if (!options.Flags.HasFlag((Enum) CardCreationFlags.NoUpgradeRoll))
      {
        Rng rng = options.RngOverride ?? player.PlayerRng.Rewards;
        CardFactory.RollForUpgrade(player, forReward, 0M, rng);
      }
    }
    List<AbstractModel> modifiers;
    if (!options.Flags.HasFlag((Enum) CardCreationFlags.NoModifyHooks) && Hook.TryModifyCardRewardOptions(player.RunState, player, cardRewardOptions, options, out modifiers))
      TaskHelper.RunSafely(Hook.AfterModifyingCardRewardOptions(player.RunState, (IEnumerable<AbstractModel>) modifiers));
    return (IEnumerable<CardCreationResult>) cardRewardOptions;
  }

  public static IEnumerable<CardModel> GetDistinctForCombat(
    Player player,
    IEnumerable<CardModel> cards,
    int count,
    Rng rng)
  {
    List<CardModel> distinctForCombat = TestRngInjector.ConsumeCombatCardGenerationOverride();
    if (distinctForCombat != null)
      return (IEnumerable<CardModel>) distinctForCombat;
    cards = CardFactory.FilterForPlayerCount(player.RunState, cards);
    return CardFactory.FilterForCombat(cards).TakeRandom<CardModel>(count, rng).Select<CardModel, CardModel>((Func<CardModel, CardModel>) (c => player.Creature.CombatState.CreateCard(c, player)));
  }

  public static IEnumerable<CardModel> GetForCombat(
    Player player,
    IEnumerable<CardModel> cards,
    int count,
    Rng rng)
  {
    List<CardModel> list1 = CardFactory.FilterForCombat(cards).ToList<CardModel>();
    List<CardModel> list2 = CardFactory.FilterForPlayerCount(player.RunState, (IEnumerable<CardModel>) list1).ToList<CardModel>();
    List<CardModel> forCombat = new List<CardModel>();
    for (int index = 0; index < count; ++index)
    {
      CardModel canonicalCard = rng.NextItem<CardModel>((IEnumerable<CardModel>) list2);
      CardModel card = player.Creature.CombatState.CreateCard(canonicalCard, player);
      forCombat.Add(card);
    }
    return (IEnumerable<CardModel>) forCombat;
  }

  public static IEnumerable<CardModel> FilterForCombat(IEnumerable<CardModel> cards)
  {
    return cards.Where<CardModel>((Func<CardModel, bool>) (c => c.CanBeGeneratedInCombat && c.Rarity != CardRarity.Basic && c.Rarity != CardRarity.Ancient && c.Rarity != CardRarity.Event)).Distinct<CardModel>();
  }

  public static IEnumerable<CardModel> GetDefaultTransformationOptions(
    CardModel original,
    bool isInCombat)
  {
    IEnumerable<CardModel> unlockedCards = (original.Type != CardType.Quest && original.Rarity != CardRarity.Event && original.Rarity != CardRarity.Ancient && original.Rarity != CardRarity.Token ? original.Pool : (CardPoolModel) ModelDb.CardPool<ColorlessCardPool>()).GetUnlockedCards(original.Owner.UnlockState, original.RunState.CardMultiplayerConstraint);
    return (IEnumerable<CardModel>) CardFactory.GetFilteredTransformationOptions(original, unlockedCards, isInCombat);
  }

  public static CardModel CreateRandomCardForTransform(
    CardModel original,
    bool isInCombat,
    Rng rng)
  {
    IEnumerable<CardModel> transformationOptions = CardFactory.GetDefaultTransformationOptions(original, isInCombat);
    return original.CardScope.CreateCard(rng.NextItem<CardModel>(transformationOptions), original.Owner);
  }

  public static CardModel CreateRandomCardForTransform(
    CardModel original,
    IEnumerable<CardModel> options,
    bool isInCombat,
    Rng rng)
  {
    CardModel[] transformationOptions = CardFactory.GetFilteredTransformationOptions(original, options, isInCombat);
    return original.CardScope.CreateCard(rng.NextItem<CardModel>((IEnumerable<CardModel>) transformationOptions), original.Owner);
  }

  private static CardModel[] GetFilteredTransformationOptions(
    CardModel original,
    IEnumerable<CardModel> originalOptions,
    bool isInCombat)
  {
    IEnumerable<CardModel> source = originalOptions;
    bool flag;
    switch (original.Rarity)
    {
      case CardRarity.Status:
      case CardRarity.Curse:
        flag = true;
        break;
      default:
        flag = false;
        break;
    }
    if (!flag)
      source = source.Where<CardModel>((Func<CardModel, bool>) (c =>
      {
        bool transformationOptions;
        switch (c.Rarity)
        {
          case CardRarity.Common:
          case CardRarity.Uncommon:
          case CardRarity.Rare:
            transformationOptions = true;
            break;
          default:
            transformationOptions = false;
            break;
        }
        return transformationOptions;
      }));
    if (isInCombat)
      source = source.Where<CardModel>((Func<CardModel, bool>) (c => c.CanBeGeneratedInCombat));
    IEnumerable<CardModel> list = (IEnumerable<CardModel>) source.Where<CardModel>((Func<CardModel, bool>) (c => c.Id != original.Id)).ToList<CardModel>();
    CardModel[] array = CardFactory.FilterForPlayerCount(original.Owner.RunState, list).ToArray<CardModel>();
    return array.Length != 0 ? array : throw new InvalidOperationException("All transformation options provided are invalid! Original options: " + string.Join<CardModel>(",", originalOptions));
  }

  private static CardModel CreateForReward(
    Player player,
    IEnumerable<CardModel> blacklist,
    CardCreationOptions options)
  {
    options = Hook.ModifyCardRewardCreationOptions(player.RunState, player, options);
    IEnumerable<CardModel> list = (IEnumerable<CardModel>) options.GetPossibleCards(player).Except<CardModel>(blacklist).ToList<CardModel>();
    IEnumerable<CardModel> array = (IEnumerable<CardModel>) CardFactory.FilterForPlayerCount(player.RunState, list).ToArray<CardModel>();
    CardRarity? selectedRarity = new CardRarity?();
    IEnumerable<CardModel> items;
    if (options.RarityOdds == CardRarityOddsType.Uniform)
    {
      items = array.Where<CardModel>((Func<CardModel, bool>) (c => c.Rarity != CardRarity.Basic && c.Rarity != CardRarity.Ancient));
    }
    else
    {
      HashSet<CardRarity> hashSet = array.Select<CardModel, CardRarity>((Func<CardModel, CardRarity>) (c => c.Rarity)).ToHashSet<CardRarity>();
      selectedRarity = new CardRarity?(CardFactory.RollForRarity(player, options.RarityOdds, options.Source, hashSet, options.Flags.HasFlag((Enum) CardCreationFlags.ForceRarityOddsChange)));
      CardRarity? nullable1 = selectedRarity;
      CardRarity cardRarity = CardRarity.None;
      if (nullable1.GetValueOrDefault() == cardRarity & nullable1.HasValue)
        throw new InvalidOperationException($"Tried to create a card for a reward, but we couldn't generate a valid rarity! Odds: {options.RarityOdds} Card pool: {string.Join<CardModel>(",", array)}, blacklist: {string.Join<CardModel>(",", blacklist)}");
      items = array.Where<CardModel>((Func<CardModel, bool>) (card =>
      {
        int rarity = (int) card.Rarity;
        CardRarity? nullable2 = selectedRarity;
        int valueOrDefault = (int) nullable2.GetValueOrDefault();
        return rarity == valueOrDefault & nullable2.HasValue;
      }));
    }
    return player.RunState.CreateCard((options.RngOverride ?? player.PlayerRng.Rewards).NextItem<CardModel>(items) ?? throw new InvalidOperationException($"Tried to create a card for a reward, but we couldn't generate a valid card! Selected rarity: {selectedRarity}, card pool: {string.Join<CardModel>(",", array)}, blacklist: {string.Join<CardModel>(",", blacklist)}, odds: {options.RarityOdds}"), player);
  }

  private static CardRarity RollForRarity(
    Player player,
    CardRarityOddsType rollMethod,
    CardCreationSource source,
    HashSet<CardRarity> allowedRarities,
    bool forceRarityOddsChange)
  {
    bool flag1 = forceRarityOddsChange;
    if (!flag1)
    {
      bool flag2 = source == CardCreationSource.Encounter;
      if (flag2)
      {
        bool flag3;
        switch (rollMethod)
        {
          case CardRarityOddsType.RegularEncounter:
          case CardRarityOddsType.EliteEncounter:
          case CardRarityOddsType.BossEncounter:
            flag3 = true;
            break;
          default:
            flag3 = false;
            break;
        }
        flag2 = flag3;
      }
      flag1 = flag2;
    }
    return CardFactory.GetNextAllowedRarity(!flag1 ? player.PlayerOdds.CardRarity.RollWithBaseOdds(rollMethod) : player.PlayerOdds.CardRarity.Roll(rollMethod), new Func<CardRarity, bool>(allowedRarities.Contains));
  }

  private static CardRarity GetNextAllowedRarity(
    CardRarity rarity,
    Func<CardRarity, bool> isAllowed)
  {
    int capacity = 1;
    List<CardRarity> cardRarityList1 = new List<CardRarity>(capacity);
    CollectionsMarshal.SetCount<CardRarity>(cardRarityList1, capacity);
    CollectionsMarshal.AsSpan<CardRarity>(cardRarityList1)[0] = rarity;
    List<CardRarity> cardRarityList2 = cardRarityList1;
    while (!isAllowed(rarity) && rarity != CardRarity.None)
    {
      rarity = rarity.GetNextHighestRarityWithWrapping();
      if (cardRarityList2.Contains(rarity))
        return CardRarity.None;
    }
    return rarity;
  }

  private static void RollForUpgrade(Player player, CardModel card, Decimal baseChance)
  {
    CardFactory.RollForUpgrade(player, card, baseChance, player.PlayerRng.Rewards);
  }

  private static void RollForUpgrade(Player player, CardModel card, Decimal baseChance, Rng rng)
  {
    Decimal num1 = (Decimal) rng.NextFloat();
    if (!card.IsUpgradable)
      return;
    Decimal originalOdds = baseChance;
    if (card.Rarity != CardRarity.Rare)
    {
      int currentActIndex = player.RunState.CurrentActIndex;
      originalOdds += (Decimal) currentActIndex * CardFactory.UpgradedCardOddScaling;
    }
    Decimal num2 = Hook.ModifyCardRewardUpgradeOdds(player.RunState, player, card, originalOdds);
    if (!(num1 <= num2))
      return;
    CardCmd.Upgrade(card);
  }
}
