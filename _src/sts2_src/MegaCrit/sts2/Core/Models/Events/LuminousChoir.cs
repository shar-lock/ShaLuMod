// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.LuminousChoir
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class LuminousChoir : EventModel
{
  public override bool IsAllowed(IRunState runState)
  {
    return runState.Players.All<Player>((Func<Player, bool>) (p => (Decimal) p.Gold >= this.DynamicVars.Gold.BaseValue && p.RelicGrabBag.HasAvailableRelics(runState)));
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new GoldVar(149));
    }
  }

  public override void CalculateVars()
  {
    GoldVar gold = this.DynamicVars.Gold;
    gold.BaseValue = gold.BaseValue - (Decimal) this.Rng.NextInt(0, 50);
  }

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    int capacity = 1;
    List<EventOption> eventOptionList = new List<EventOption>(capacity);
    CollectionsMarshal.SetCount<EventOption>(eventOptionList, capacity);
    CollectionsMarshal.AsSpan<EventOption>(eventOptionList)[0] = new EventOption((EventModel) this, new Func<Task>(this.ReachIntoTheFlesh), "LUMINOUS_CHOIR.pages.INITIAL.options.REACH_INTO_THE_FLESH", HoverTipFactory.FromCardWithCardHoverTips<SporeMind>());
    List<EventOption> initialOptions = eventOptionList;
    if (this.Owner.Gold >= this.DynamicVars.Gold.IntValue)
      initialOptions.Add(new EventOption((EventModel) this, new Func<Task>(this.OfferTribute), "LUMINOUS_CHOIR.pages.INITIAL.options.OFFER_TRIBUTE", Array.Empty<IHoverTip>()));
    else
      initialOptions.Add(new EventOption((EventModel) this, (Func<Task>) null, "LUMINOUS_CHOIR.pages.INITIAL.options.OFFER_TRIBUTE_LOCKED", Array.Empty<IHoverTip>()));
    return (IReadOnlyList<EventOption>) initialOptions;
  }

  private async Task ReachIntoTheFlesh()
  {
    await CardPileCmd.RemoveFromDeck((IReadOnlyList<CardModel>) (await CardSelectCmd.FromDeckForRemoval(this.Owner, new CardSelectorPrefs(CardSelectorPrefs.RemoveSelectionPrompt, 2))).ToList<CardModel>());
    CardModel deck = await CardPileCmd.AddCurseToDeck<SporeMind>(this.Owner);
    this.SetEventFinished(this.L10NLookup("LUMINOUS_CHOIR.pages.REACH_INTO_THE_FLESH.description"));
  }

  private async Task OfferTribute()
  {
    await PlayerCmd.LoseGold((Decimal) this.DynamicVars.Gold.IntValue, this.Owner, GoldLossType.Spent);
    RelicModel relicModel = await RelicCmd.Obtain(RelicFactory.PullNextRelicFromFront(this.Owner).ToMutable(), this.Owner);
    this.SetEventFinished(this.L10NLookup("LUMINOUS_CHOIR.pages.OFFER_TRIBUTE.description"));
  }
}
