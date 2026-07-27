// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.UnrestSite
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
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

public sealed class UnrestSite : EventModel
{
  private const string _maxHpLossKey = "MaxHpLoss";

  public override bool IsAllowed(IRunState runState)
  {
    return runState.Players.All<Player>((Func<Player, bool>) (p => (Decimal) p.Creature.CurrentHp <= (Decimal) p.Creature.MaxHp * 0.70M));
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new HealVar(0M),
        new DynamicVar("MaxHpLoss", 8M)
      });
    }
  }

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
    {
      new EventOption((EventModel) this, new Func<Task>(this.Rest), "UNREST_SITE.pages.INITIAL.options.REST", HoverTipFactory.FromCardWithCardHoverTips<PoorSleep>()),
      new EventOption((EventModel) this, new Func<Task>(this.Kill), "UNREST_SITE.pages.INITIAL.options.KILL", Array.Empty<IHoverTip>()).ThatDecreasesMaxHp(this.DynamicVars["MaxHpLoss"].BaseValue)
    });
  }

  public override void CalculateVars()
  {
    this.DynamicVars.Heal.BaseValue = (Decimal) (this.Owner.Creature.MaxHp - this.Owner.Creature.CurrentHp);
  }

  private async Task Rest()
  {
    await CreatureCmd.Heal(this.Owner.Creature, this.DynamicVars.Heal.BaseValue);
    // ISSUE: object of a compiler-generated type is created
    IEnumerable<CardPileAddResult> deck = await CardPileCmd.AddCursesToDeck((IEnumerable<CardModel>) new \u003C\u003Ez__ReadOnlySingleElementList<CardModel>((CardModel) ModelDb.Card<PoorSleep>()), this.Owner);
    this.SetEventFinished(this.L10NLookup("UNREST_SITE.pages.REST.description"));
  }

  private async Task Kill()
  {
    await CreatureCmd.LoseMaxHp((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Owner.Creature, this.DynamicVars["MaxHpLoss"].BaseValue, false);
    RelicModel relicModel = await RelicCmd.Obtain(RelicFactory.PullNextRelicFromFront(this.Owner).ToMutable(), this.Owner);
    this.SetEventFinished(this.L10NLookup("UNREST_SITE.pages.KILL.description"));
  }
}
