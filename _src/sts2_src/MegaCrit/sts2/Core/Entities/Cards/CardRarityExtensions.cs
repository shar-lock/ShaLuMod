// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Cards.CardRarityExtensions
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Localization;
using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.Cards;

public static class CardRarityExtensions
{
  public static CardRarity GetNextHighestRarityWithWrapping(this CardRarity cardRarity)
  {
    switch (cardRarity)
    {
      case CardRarity.None:
        return CardRarity.None;
      case CardRarity.Basic:
        return CardRarity.Common;
      case CardRarity.Common:
        return CardRarity.Uncommon;
      case CardRarity.Uncommon:
        return CardRarity.Rare;
      case CardRarity.Rare:
        return CardRarity.Common;
      case CardRarity.Ancient:
        return CardRarity.None;
      case CardRarity.Event:
        return CardRarity.None;
      case CardRarity.Token:
        return CardRarity.None;
      case CardRarity.Status:
        return CardRarity.None;
      case CardRarity.Curse:
        return CardRarity.None;
      case CardRarity.Quest:
        return CardRarity.None;
      default:
        throw new ArgumentOutOfRangeException(nameof (cardRarity), (object) cardRarity, (string) null);
    }
  }

  public static LocString ToLocString(this CardRarity cardRarity)
  {
    switch (cardRarity)
    {
      case CardRarity.Basic:
        return new LocString("gameplay_ui", "CARD_RARITY.BASIC");
      case CardRarity.Common:
        return new LocString("gameplay_ui", "CARD_RARITY.COMMON");
      case CardRarity.Uncommon:
        return new LocString("gameplay_ui", "CARD_RARITY.UNCOMMON");
      case CardRarity.Rare:
        return new LocString("gameplay_ui", "CARD_RARITY.RARE");
      case CardRarity.Ancient:
        return new LocString("gameplay_ui", "CARD_RARITY.ANCIENT");
      case CardRarity.Event:
        return new LocString("gameplay_ui", "CARD_RARITY.EVENT");
      case CardRarity.Token:
        return new LocString("gameplay_ui", "CARD_RARITY.TOKEN");
      case CardRarity.Status:
        return new LocString("gameplay_ui", "CARD_RARITY.STATUS");
      case CardRarity.Curse:
        return new LocString("gameplay_ui", "CARD_RARITY.CURSE");
      case CardRarity.Quest:
        return new LocString("gameplay_ui", "CARD_RARITY.QUEST");
      default:
        throw new ArgumentOutOfRangeException(nameof (cardRarity), (object) cardRarity, (string) null);
    }
  }
}
