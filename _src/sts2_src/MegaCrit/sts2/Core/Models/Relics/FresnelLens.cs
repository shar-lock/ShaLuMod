// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.FresnelLens
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Enchantments;
using MegaCrit.Sts2.Core.Runs;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class FresnelLens : RelicModel
{
  private const string _nimbleAmountKey = "NimbleAmount";

  public override RelicRarity Rarity => RelicRarity.Event;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("NimbleAmount", 2M));
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get => HoverTipFactory.FromEnchantment<Nimble>(this.DynamicVars["NimbleAmount"].IntValue);
  }

  public override bool TryModifyCardRewardOptionsLate(
    Player player,
    List<CardCreationResult> cardRewards,
    CardCreationOptions options)
  {
    if (player != this.Owner)
      return false;
    this.EnchantValidCards(cardRewards);
    return true;
  }

  public override void ModifyMerchantCardCreationResults(
    Player player,
    List<CardCreationResult> cards)
  {
    if (player != this.Owner)
      return;
    this.EnchantValidCards(cards);
  }

  public override bool TryModifyCardBeingAddedToDeck(CardModel card, out CardModel? newCard)
  {
    newCard = (CardModel) null;
    if (card.Owner != this.Owner || !ModelDb.Enchantment<Nimble>().CanEnchant(card))
      return false;
    newCard = this.EnchantCard(card);
    return true;
  }

  private void EnchantValidCards(List<CardCreationResult> options)
  {
    Nimble nimble = ModelDb.Enchantment<Nimble>();
    foreach (CardCreationResult option in options)
    {
      CardModel card = option.Card;
      if (nimble.CanEnchant(card))
        option.ModifyCard(this.EnchantCard(card), (RelicModel) this);
    }
  }

  private CardModel EnchantCard(CardModel card)
  {
    CardModel card1 = this.Owner.RunState.CloneCard(card);
    CardCmd.Enchant<Nimble>(card1, this.DynamicVars["NimbleAmount"].BaseValue);
    return card1;
  }
}
