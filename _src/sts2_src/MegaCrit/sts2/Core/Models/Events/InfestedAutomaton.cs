// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.InfestedAutomaton
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class InfestedAutomaton : EventModel
{
  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
    {
      new EventOption((EventModel) this, new Func<Task>(this.Study), "INFESTED_AUTOMATON.pages.INITIAL.options.STUDY", Array.Empty<IHoverTip>()),
      new EventOption((EventModel) this, new Func<Task>(this.TouchCore), "INFESTED_AUTOMATON.pages.INITIAL.options.TOUCH_CORE", Array.Empty<IHoverTip>())
    });
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get => (IEnumerable<DynamicVar>) Array.Empty<DynamicVar>();
  }

  private async Task Study()
  {
    // ISSUE: object of a compiler-generated type is created
    CardModel card = CardFactory.CreateForReward(this.Owner, 1, CardCreationOptions.ForNonCombatWithDefaultOdds((IEnumerable<CardPoolModel>) new \u003C\u003Ez__ReadOnlySingleElementList<CardPoolModel>(this.Owner.Character.CardPool), (Func<CardModel, bool>) (c => c.Type == CardType.Power))).FirstOrDefault<CardCreationResult>()?.Card;
    if (card != null)
      CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(card, PileType.Deck), style: CardPreviewStyle.EventLayout);
    this.SetEventFinished(this.L10NLookup("INFESTED_AUTOMATON.pages.STUDY.description"));
  }

  private async Task TouchCore()
  {
    // ISSUE: object of a compiler-generated type is created
    CardModel card = CardFactory.CreateForReward(this.Owner, 1, CardCreationOptions.ForNonCombatWithDefaultOdds((IEnumerable<CardPoolModel>) new \u003C\u003Ez__ReadOnlySingleElementList<CardPoolModel>(this.Owner.Character.CardPool), (Func<CardModel, bool>) (c =>
    {
      CardEnergyCost energyCost = c.EnergyCost;
      return energyCost != null && energyCost.Canonical == 0 && !energyCost.CostsX;
    })).WithFlags(CardCreationFlags.NoCardPoolModifications)).FirstOrDefault<CardCreationResult>()?.Card;
    if (card != null)
      CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(card, PileType.Deck), style: CardPreviewStyle.EventLayout);
    this.SetEventFinished(this.L10NLookup("INFESTED_AUTOMATON.pages.TOUCH_CORE.description"));
  }
}
