// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.RestSite.CookRestSiteOption
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.RestSite;

public sealed class CookRestSiteOption(Player owner) : RestSiteOption(owner)
{
  private const int _cardsToRemove = 2;
  private const int _maxHpGain = 5;

  public override string OptionId => "COOK";

  public override LocString Description
  {
    get
    {
      if (!this.IsEnabled)
        return new LocString("rest_site_ui", $"OPTION_{this.OptionId}.descriptionDisabled");
      LocString description = new LocString("rest_site_ui", $"OPTION_{this.OptionId}.description");
      description.Add("Cards", 2M);
      description.Add("MaxHp", 5M);
      return description;
    }
  }

  public override bool IsEnabled => CookRestSiteOption.GetRemovableCardCount(this.Owner) >= 2;

  public override async Task<bool> OnSelect()
  {
    IEnumerable<CardModel> source = await CardSelectCmd.FromDeckForRemoval(this.Owner, new CardSelectorPrefs(CardSelectorPrefs.RemoveSelectionPrompt, 2)
    {
      Cancelable = true,
      RequireManualConfirmation = true
    });
    if (!source.Any<CardModel>())
      return false;
    foreach (CardModel card in source)
      await CardPileCmd.RemoveFromDeck(card);
    await CreatureCmd.GainMaxHp(this.Owner.Creature, 5M);
    return true;
  }

  private static int GetRemovableCardCount(Player player)
  {
    return PileType.Deck.GetPile(player).Cards.Count<CardModel>((Func<CardModel, bool>) (c => c.IsRemovable));
  }
}
