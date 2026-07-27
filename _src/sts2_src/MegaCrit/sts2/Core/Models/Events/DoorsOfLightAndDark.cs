// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.DoorsOfLightAndDark
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class DoorsOfLightAndDark : EventModel
{
  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new CardsVar(2));
    }
  }

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
    {
      new EventOption((EventModel) this, new Func<Task>(this.Light), "DOORS_OF_LIGHT_AND_DARK.pages.INITIAL.options.LIGHT", Array.Empty<IHoverTip>()),
      new EventOption((EventModel) this, new Func<Task>(this.Dark), "DOORS_OF_LIGHT_AND_DARK.pages.INITIAL.options.DARK", Array.Empty<IHoverTip>())
    });
  }

  private Task Light()
  {
    foreach (CardModel card in PileType.Deck.GetPile(this.Owner).Cards.Where<CardModel>((Func<CardModel, bool>) (c => c != null && c.IsUpgradable)).ToList<CardModel>().StableShuffle<CardModel>(this.Rng).Take<CardModel>(this.DynamicVars.Cards.IntValue))
      CardCmd.Upgrade(card);
    this.SetEventFinished(this.L10NLookup("DOORS_OF_LIGHT_AND_DARK.pages.LIGHT.description"));
    return Task.CompletedTask;
  }

  private async Task Dark()
  {
    await CardPileCmd.RemoveFromDeck((IReadOnlyList<CardModel>) (await CardSelectCmd.FromDeckForRemoval(this.Owner, new CardSelectorPrefs(CardSelectorPrefs.RemoveSelectionPrompt, 1))).ToList<CardModel>());
    this.SetEventFinished(this.L10NLookup("DOORS_OF_LIGHT_AND_DARK.pages.DARK.description"));
  }
}
