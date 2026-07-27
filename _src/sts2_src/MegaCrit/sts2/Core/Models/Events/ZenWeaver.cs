// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.ZenWeaver
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class ZenWeaver : EventModel
{
  private const string _breathingTechniquesCostKey = "BreathingTechniquesCost";
  private const string _emotionalAwarenessCostKey = "EmotionalAwarenessCost";
  private const string _arachnidAcupunctureCostKey = "ArachnidAcupunctureCost";

  public override bool IsAllowed(IRunState runState)
  {
    return runState.Players.All<Player>((Func<Player, bool>) (p => (Decimal) p.Gold >= this.DynamicVars["EmotionalAwarenessCost"].BaseValue));
  }

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    int gold = this.Owner.Gold;
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[3]
    {
      new EventOption((EventModel) this, new Func<Task>(this.BreathingTechniques), "ZEN_WEAVER.pages.INITIAL.options.BREATHING_TECHNIQUES", HoverTipFactory.FromCardWithCardHoverTips<Enlightenment>()),
      gold < this.DynamicVars["EmotionalAwarenessCost"].IntValue ? this.CreateLockedOption() : new EventOption((EventModel) this, new Func<Task>(this.EmotionalAwareness), "ZEN_WEAVER.pages.INITIAL.options.EMOTIONAL_AWARENESS", Array.Empty<IHoverTip>()),
      gold < this.DynamicVars["ArachnidAcupunctureCost"].IntValue ? this.CreateLockedOption() : new EventOption((EventModel) this, new Func<Task>(this.ArachnidAcupuncture), "ZEN_WEAVER.pages.INITIAL.options.ARACHNID_ACUPUNCTURE", Array.Empty<IHoverTip>())
    });
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[3]
      {
        new DynamicVar("BreathingTechniquesCost", 50M),
        new DynamicVar("EmotionalAwarenessCost", 125M),
        new DynamicVar("ArachnidAcupunctureCost", 250M)
      });
    }
  }

  private async Task BreathingTechniques()
  {
    await PlayerCmd.LoseGold((Decimal) this.DynamicVars["BreathingTechniquesCost"].IntValue, this.Owner, GoldLossType.Spent);
    CardModel[] cards = new CardModel[2];
    for (int index = 0; index < cards.Length; ++index)
      cards[index] = (CardModel) this.Owner.RunState.CreateCard<Enlightenment>(this.Owner);
    CardCmd.PreviewCardPileAdd(await CardPileCmd.Add((IEnumerable<CardModel>) cards, PileType.Deck));
    this.SetEventFinished(this.L10NLookup("ZEN_WEAVER.pages.BREATHING_TECHNIQUES.description"));
  }

  private async Task EmotionalAwareness()
  {
    await this.RemoveCardsAndProceed(this.DynamicVars["EmotionalAwarenessCost"].IntValue, 1);
    this.SetEventFinished(this.L10NLookup("ZEN_WEAVER.pages.EMOTIONAL_AWARENESS.description"));
  }

  private async Task ArachnidAcupuncture()
  {
    await this.RemoveCardsAndProceed(this.DynamicVars["ArachnidAcupunctureCost"].IntValue, 2);
    this.SetEventFinished(this.L10NLookup("ZEN_WEAVER.pages.ARACHNID_ACUPUNCTURE.description"));
  }

  private async Task RemoveCardsAndProceed(int cost, int count)
  {
    await CardPileCmd.RemoveFromDeck((IReadOnlyList<CardModel>) (await CardSelectCmd.FromDeckForRemoval(this.Owner, new CardSelectorPrefs(CardSelectorPrefs.RemoveSelectionPrompt, count))).ToList<CardModel>());
    await PlayerCmd.LoseGold((Decimal) cost, this.Owner, GoldLossType.Spent);
  }

  private EventOption CreateLockedOption()
  {
    return new EventOption((EventModel) this, (Func<Task>) null, "ZEN_WEAVER.pages.INITIAL.options.LOCKED", Array.Empty<IHoverTip>());
  }
}
