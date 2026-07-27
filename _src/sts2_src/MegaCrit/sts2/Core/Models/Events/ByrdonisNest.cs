// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.ByrdonisNest
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
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

public sealed class ByrdonisNest : EventModel
{
  private const string _cardKey = "Card";

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
    {
      new EventOption((EventModel) this, new Func<Task>(this.Eat), "BYRDONIS_NEST.pages.INITIAL.options.EAT", Array.Empty<IHoverTip>()),
      new EventOption((EventModel) this, new Func<Task>(this.Take), "BYRDONIS_NEST.pages.INITIAL.options.TAKE", HoverTipFactory.FromCardWithCardHoverTips<ByrdonisEgg>())
    });
  }

  public override bool IsAllowed(IRunState runState)
  {
    return runState.Players.All<Player>((Func<Player, bool>) (p => !p.HasEventPet()));
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new MaxHpVar(7M),
        (DynamicVar) new StringVar("Card", ModelDb.Card<ByrdonisEgg>().Title)
      });
    }
  }

  private async Task Eat()
  {
    await CreatureCmd.GainMaxHp(this.Owner.Creature, this.DynamicVars.MaxHp.BaseValue);
    this.SetEventFinished(this.L10NLookup("BYRDONIS_NEST.pages.EAT.description"));
  }

  private async Task Take()
  {
    CardCmd.PreviewCardPileAdd(await CardPileCmd.Add((CardModel) this.Owner.RunState.CreateCard<ByrdonisEgg>(this.Owner), PileType.Deck), 2f);
    this.SetEventFinished(this.L10NLookup("BYRDONIS_NEST.pages.TAKE.description"));
  }
}
