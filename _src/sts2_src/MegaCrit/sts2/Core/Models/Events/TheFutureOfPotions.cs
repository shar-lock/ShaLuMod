// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.TheFutureOfPotions
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class TheFutureOfPotions : EventModel
{
  private const string _choiceKey = "THE_FUTURE_OF_POTIONS.pages.INITIAL.options.POTION";
  private const string _potionKey = "Potion";
  private const string _rarityKey = "Rarity";
  private const string _typeKey = "Type";
  private Dictionary<PotionModel, CardType>? _cardTypes;

  private LocString ChoiceTitle
  {
    get => new LocString("events", "THE_FUTURE_OF_POTIONS.pages.INITIAL.options.POTION.title");
  }

  private LocString ChoiceDescription
  {
    get
    {
      return new LocString("events", "THE_FUTURE_OF_POTIONS.pages.INITIAL.options.POTION.description");
    }
  }

  public override bool IsAllowed(IRunState runState)
  {
    return runState.Players.All<Player>((Func<Player, bool>) (p => p.Potions.Count<PotionModel>() >= 2));
  }

  private Dictionary<PotionModel, CardType> PotionToCardType
  {
    get
    {
      this.AssertMutable();
      if (this._cardTypes == null)
      {
        this._cardTypes = new Dictionary<PotionModel, CardType>();
        foreach (PotionModel potion in this.Owner.Potions)
        {
          int capacity = 3;
          List<CardType> cardTypeList = new List<CardType>(capacity);
          CollectionsMarshal.SetCount<CardType>(cardTypeList, capacity);
          Span<CardType> span = CollectionsMarshal.AsSpan<CardType>(cardTypeList);
          int num1 = 0;
          span[num1] = CardType.Attack;
          int num2 = num1 + 1;
          span[num2] = CardType.Skill;
          int num3 = num2 + 1;
          span[num3] = CardType.Power;
          List<CardType> items = cardTypeList;
          if (potion.Rarity == PotionRarity.Common || potion.Rarity == PotionRarity.Token)
            items.Remove(CardType.Power);
          this._cardTypes.Add(potion, this.Rng.NextItem<CardType>((IEnumerable<CardType>) items));
        }
      }
      return this._cardTypes;
    }
  }

  protected override Task BeforeEventStarted(bool isPreFinished)
  {
    this.Owner.CanUseOrRemovePotions = false;
    return Task.CompletedTask;
  }

  protected override void OnEventFinished() => this.Owner.CanUseOrRemovePotions = true;

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    List<EventOption> initialOptions = new List<EventOption>();
    List<PotionModel> list = this.Owner.Potions.ToList<PotionModel>();
    int num = Mathf.Min(3, list.Count);
    for (int index = 0; index < num; ++index)
    {
      PotionModel potion = list[index];
      LocString choiceTitle = this.ChoiceTitle;
      choiceTitle.Add("Rarity", potion.Rarity.ToLocString().GetFormattedText());
      LocString choiceDescription = this.ChoiceDescription;
      choiceDescription.Add("Potion", potion.Title.GetFormattedText());
      choiceDescription.Add("Rarity", this.GetCardRarity(potion).ToLocString().GetFormattedText());
      choiceDescription.Add("Type", this.PotionToCardType[potion].ToLocString().GetFormattedText());
      initialOptions.Add(new EventOption((EventModel) this, (Func<Task>) (async () => await this.Trade(potion)), choiceTitle, choiceDescription, "THE_FUTURE_OF_POTIONS.pages.INITIAL.options.POTION", potion.HoverTips).ThatHasDynamicTitle());
    }
    return (IReadOnlyList<EventOption>) initialOptions;
  }

  public override IEnumerable<LocString> GameInfoOptions
  {
    get
    {
      List<LocString> list = base.GameInfoOptions.ToList<LocString>();
      if (list.Count != 2)
        throw new InvalidOperationException("TheFutureOfPotions must've changed loc format, please update its\nGameInfoOptions method.");
      list.First<LocString>((Func<LocString, bool>) (o => o.LocEntryKey.EndsWith(".title"))).Add("Rarity", "[rarity]");
      LocString locString = list.First<LocString>((Func<LocString, bool>) (o => o.LocEntryKey.EndsWith(".description")));
      locString.Add("Potion", "[potion]");
      locString.Add("Rarity", "[same-rarity]");
      locString.Add("Type", "[card of random type]");
      return (IEnumerable<LocString>) list;
    }
  }

  private async Task Trade(PotionModel potion)
  {
    CardRarity targetRarity = this.GetCardRarity(potion);
    await PotionCmd.Discard(potion);
    // ISSUE: object of a compiler-generated type is created
    CardReward reward = new CardReward(CardCreationOptions.ForNonCombatWithUniformOdds((IEnumerable<CardPoolModel>) new \u003C\u003Ez__ReadOnlySingleElementList<CardPoolModel>(this.Owner.Character.CardPool), (Func<CardModel, bool>) (c => c.Rarity == targetRarity && c.Type == this.PotionToCardType[potion])).WithFlags(CardCreationFlags.NoRarityModification | CardCreationFlags.NoCardPoolModifications), 3, this.Owner);
    reward.AfterGenerated += new Action(UpgradeCardsInReward);
    await RewardsCmd.OfferCustom(this.Owner, new List<Reward>(1)
    {
      (Reward) reward
    });
    await this.Done();

    void UpgradeCardsInReward()
    {
      foreach (CardModel card in reward.Cards)
        CardCmd.Upgrade(card);
    }
  }

  private Task Done()
  {
    this.SetEventFinished(this.L10NLookup("THE_FUTURE_OF_POTIONS.pages.DONE.description"));
    return Task.CompletedTask;
  }

  private CardRarity GetCardRarity(PotionModel potion)
  {
    switch (potion.Rarity)
    {
      case PotionRarity.Common:
      case PotionRarity.Token:
        return CardRarity.Common;
      case PotionRarity.Uncommon:
        return CardRarity.Uncommon;
      case PotionRarity.Rare:
      case PotionRarity.Event:
        return CardRarity.Rare;
      default:
        throw new InvalidOperationException($"Potion {potion.Id.Entry} has invalid rarity {potion.Rarity}");
    }
  }
}
