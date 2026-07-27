// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Modifiers.Hoarder
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Modifiers;

public class Hoarder : ModifierModel
{
  private readonly HashSet<CardModel> _cardsToSkip = new HashSet<CardModel>();

  public override async Task AfterCardChangedPiles(
    CardModel card,
    PileType oldPileType,
    AbstractModel? clonedBy)
  {
    if (oldPileType != PileType.None)
      return;
    CardPile pile = card.Pile;
    if ((pile != null ? (pile.Type != PileType.Deck ? 1 : 0) : 1) != 0 || clonedBy != null || this._cardsToSkip.Remove(card))
      return;
    for (int i = 0; i < 2; ++i)
    {
      CardModel card1 = card.Owner.RunState.CloneCard(card);
      this._cardsToSkip.Add(card1);
      CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(card1, PileType.Deck, clonedBy: (AbstractModel) this));
    }
  }

  public override bool ShouldAllowMerchantCardRemoval(Player player) => false;
}
