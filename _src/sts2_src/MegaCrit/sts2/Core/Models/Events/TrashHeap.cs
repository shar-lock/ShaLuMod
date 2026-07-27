// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.TrashHeap
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class TrashHeap : EventModel
{
  private static RelicModel[] Relics
  {
    get
    {
      return new RelicModel[5]
      {
        (RelicModel) ModelDb.Relic<DarkstonePeriapt>(),
        (RelicModel) ModelDb.Relic<DreamCatcher>(),
        (RelicModel) ModelDb.Relic<HandDrill>(),
        (RelicModel) ModelDb.Relic<MawBank>(),
        (RelicModel) ModelDb.Relic<TheBoot>()
      };
    }
  }

  private static CardModel[] Cards
  {
    get
    {
      return new CardModel[10]
      {
        (CardModel) ModelDb.Card<Caltrops>(),
        (CardModel) ModelDb.Card<Clash>(),
        (CardModel) ModelDb.Card<Distraction>(),
        (CardModel) ModelDb.Card<DualWield>(),
        (CardModel) ModelDb.Card<Entrench>(),
        (CardModel) ModelDb.Card<HelloWorld>(),
        (CardModel) ModelDb.Card<Outmaneuver>(),
        (CardModel) ModelDb.Card<Rebound>(),
        (CardModel) ModelDb.Card<RipAndTear>(),
        (CardModel) ModelDb.Card<Stack>()
      };
    }
  }

  public override bool IsAllowed(IRunState runState)
  {
    return runState.Players.All<Player>((Func<Player, bool>) (player => player.Creature.CurrentHp > 5));
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new HpLossVar(8M),
        (DynamicVar) new GoldVar(100)
      });
    }
  }

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
    {
      new EventOption((EventModel) this, new Func<Task>(this.DiveIn), "TRASH_HEAP.pages.INITIAL.options.DIVE_IN", Array.Empty<IHoverTip>()).ThatDoesDamage((Decimal) this.DynamicVars.HpLoss.IntValue),
      new EventOption((EventModel) this, new Func<Task>(this.Grab), "TRASH_HEAP.pages.INITIAL.options.GRAB", Array.Empty<IHoverTip>())
    });
  }

  private async Task DiveIn()
  {
    IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Owner.Creature, (Decimal) this.DynamicVars.HpLoss.IntValue, ValueProp.Unblockable | ValueProp.Unpowered, (Creature) null, (CardModel) null, (CardPlay) null);
    RelicModel relicModel = await RelicCmd.Obtain(this.Rng.NextItem<RelicModel>((IEnumerable<RelicModel>) TrashHeap.Relics).ToMutable(), this.Owner);
    this.SetEventFinished(this.L10NLookup("TRASH_HEAP.pages.DIVE_IN.description"));
  }

  private async Task Grab()
  {
    await PlayerCmd.GainGold(this.DynamicVars.Gold.BaseValue, this.Owner);
    CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(this.Owner.RunState.CreateCard(this.Rng.NextItem<CardModel>((IEnumerable<CardModel>) TrashHeap.Cards), this.Owner), PileType.Deck), 2f);
    this.SetEventFinished(this.L10NLookup("TRASH_HEAP.pages.GRAB.description"));
  }
}
