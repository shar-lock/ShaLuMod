// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.LostWisp
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class LostWisp : EventModel
{
  private const string _relicKey = "Relic";
  private const string _curseKey = "Curse";
  private const int _baseGold = 60;
  private const int _goldVariance = 15;

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    EventOption[] items = new EventOption[2];
    Func<Task> onChosen = new Func<Task>(this.Claim);
    List<IHoverTip> hoverTipList = new List<IHoverTip>();
    hoverTipList.AddRange(HoverTipFactory.FromRelic<MegaCrit.Sts2.Core.Models.Relics.LostWisp>());
    hoverTipList.AddRange(HoverTipFactory.FromCardWithCardHoverTips<Decay>());
    IHoverTip[] array = hoverTipList.ToArray();
    items[0] = new EventOption((EventModel) this, onChosen, "LOST_WISP.pages.INITIAL.options.CLAIM", array);
    items[1] = new EventOption((EventModel) this, new Func<Task>(this.Search), "LOST_WISP.pages.INITIAL.options.SEARCH", Array.Empty<IHoverTip>());
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(items);
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[3]
      {
        (DynamicVar) new GoldVar(60),
        (DynamicVar) new StringVar("Relic", ModelDb.Relic<MegaCrit.Sts2.Core.Models.Relics.LostWisp>().Title.GetFormattedText()),
        (DynamicVar) new StringVar("Curse", ModelDb.Card<Decay>().Title)
      });
    }
  }

  public override void CalculateVars()
  {
    GoldVar gold = this.DynamicVars.Gold;
    gold.BaseValue = gold.BaseValue + (Decimal) this.Rng.NextInt(-15, 16 /*0x10*/);
  }

  private async Task Claim()
  {
    // ISSUE: object of a compiler-generated type is created
    IEnumerable<CardPileAddResult> deck = await CardPileCmd.AddCursesToDeck((IEnumerable<CardModel>) new \u003C\u003Ez__ReadOnlySingleElementList<CardModel>((CardModel) ModelDb.Card<Decay>()), this.Owner);
    MegaCrit.Sts2.Core.Models.Relics.LostWisp lostWisp = await RelicCmd.Obtain<MegaCrit.Sts2.Core.Models.Relics.LostWisp>(this.Owner);
    this.SetEventFinished(this.L10NLookup("LOST_WISP.pages.CLAIM.description"));
  }

  private async Task Search()
  {
    await PlayerCmd.GainGold((Decimal) this.DynamicVars.Gold.IntValue, this.Owner);
    this.SetEventFinished(this.L10NLookup("LOST_WISP.pages.SEARCH.description"));
  }
}
