// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Helpers.Models.EggRelicHelper
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Helpers.Models;

public static class EggRelicHelper
{
  public static void UpgradeValidCards(
    List<CardCreationResult> cards,
    CardType cardType,
    RelicModel eggRelic)
  {
    foreach (CardCreationResult card1 in cards)
    {
      CardModel card2 = card1.Card;
      if (card2.Type == cardType && card2.IsUpgradable)
      {
        CardModel card3 = eggRelic.Owner.RunState.CloneCard(card2);
        CardCmd.Upgrade(card3);
        card1.ModifyCard(card3, eggRelic);
      }
    }
  }
}
