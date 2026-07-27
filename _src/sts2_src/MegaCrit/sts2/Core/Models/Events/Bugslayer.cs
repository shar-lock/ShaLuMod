// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.Bugslayer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class Bugslayer : EventModel
{
  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
    {
      new EventOption((EventModel) this, new Func<Task>(this.Extermination), "BUGSLAYER.pages.INITIAL.options.EXTERMINATION", HoverTipFactory.FromCardWithCardHoverTips<Exterminate>()),
      new EventOption((EventModel) this, new Func<Task>(this.Squash), "BUGSLAYER.pages.INITIAL.options.SQUASH", HoverTipFactory.FromCardWithCardHoverTips<MegaCrit.Sts2.Core.Models.Cards.Squash>())
    });
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new StringVar("Card1", ModelDb.Card<Exterminate>().Title),
        (DynamicVar) new StringVar("Card2", ModelDb.Card<MegaCrit.Sts2.Core.Models.Cards.Squash>().Title)
      });
    }
  }

  private async Task Extermination()
  {
    await this.AddAndPreview<Exterminate>(this.L10NLookup("BUGSLAYER.pages.EXTERMINATION.description"));
  }

  private async Task Squash()
  {
    await this.AddAndPreview<MegaCrit.Sts2.Core.Models.Cards.Squash>(this.L10NLookup("BUGSLAYER.pages.SQUASH.description"));
  }

  private async Task AddAndPreview<T>(LocString loc) where T : CardModel
  {
    CardCmd.PreviewCardPileAdd(await CardPileCmd.Add((CardModel) this.Owner.RunState.CreateCard<T>(this.Owner), PileType.Deck), 2f);
    this.SetEventFinished(loc);
  }
}
