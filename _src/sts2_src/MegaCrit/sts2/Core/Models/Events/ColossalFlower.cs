// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.ColossalFlower
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
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class ColossalFlower : EventModel
{
  private static readonly string[] _prizeKeys = new string[3]
  {
    "Prize1",
    "Prize2",
    "Prize3"
  };
  private static readonly int[] _prizeCosts = new int[3]
  {
    35,
    75,
    135
  };
  private static readonly int[] _prizeDamage = new int[3]
  {
    5,
    6,
    7
  };
  private int _numberOfDigs;

  private int NumberOfDigs
  {
    get => this._numberOfDigs;
    set
    {
      this.AssertMutable();
      this._numberOfDigs = value;
    }
  }

  public override bool IsAllowed(IRunState runState)
  {
    return runState.Players.All<Player>((Func<Player, bool>) (p => p.Creature.CurrentHp >= 19));
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[3]
      {
        (DynamicVar) new GoldVar(ColossalFlower._prizeKeys[0], ColossalFlower._prizeCosts[0]),
        (DynamicVar) new GoldVar(ColossalFlower._prizeKeys[1], ColossalFlower._prizeCosts[1]),
        (DynamicVar) new GoldVar(ColossalFlower._prizeKeys[2], ColossalFlower._prizeCosts[2])
      });
    }
  }

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
    {
      new EventOption((EventModel) this, new Func<Task>(this.ExtractCurrentPrize), $"COLOSSAL_FLOWER.pages.INITIAL.options.EXTRACT_CURRENT_PRIZE_{this.NumberOfDigs + 1}", Array.Empty<IHoverTip>()),
      new EventOption((EventModel) this, new Func<Task>(this.ReachDeeper), $"COLOSSAL_FLOWER.pages.INITIAL.options.REACH_DEEPER_{this.NumberOfDigs + 1}", Array.Empty<IHoverTip>()).ThatDoesDamage((Decimal) ColossalFlower._prizeDamage[this.NumberOfDigs])
    });
  }

  private async Task ReachDeeper()
  {
    await this.DealReachDeeperDamage();
    this.NumberOfDigs++;
    if (this.NumberOfDigs < 2)
    {
      // ISSUE: object of a compiler-generated type is created
      this.SetEventState(this.L10NLookup($"COLOSSAL_FLOWER.pages.REACH_DEEPER_{this.NumberOfDigs}.description"), (IEnumerable<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
      {
        new EventOption((EventModel) this, new Func<Task>(this.ExtractCurrentPrize), $"COLOSSAL_FLOWER.pages.REACH_DEEPER_{this.NumberOfDigs}.options.EXTRACT_CURRENT_PRIZE_{this.NumberOfDigs + 1}", Array.Empty<IHoverTip>()),
        new EventOption((EventModel) this, new Func<Task>(this.ReachDeeper), $"COLOSSAL_FLOWER.pages.REACH_DEEPER_{this.NumberOfDigs}.options.REACH_DEEPER_{this.NumberOfDigs + 1}", Array.Empty<IHoverTip>()).ThatDoesDamage((Decimal) ColossalFlower._prizeDamage[this.NumberOfDigs])
      }));
    }
    else
    {
      // ISSUE: object of a compiler-generated type is created
      this.SetEventState(this.L10NLookup("COLOSSAL_FLOWER.pages.REACH_DEEPER_2.description"), (IEnumerable<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
      {
        new EventOption((EventModel) this, new Func<Task>(this.ExtractInstead), "COLOSSAL_FLOWER.pages.REACH_DEEPER_2.options.EXTRACT_INSTEAD", Array.Empty<IHoverTip>()),
        new EventOption((EventModel) this, new Func<Task>(this.ObtainPollinousCore), "COLOSSAL_FLOWER.pages.REACH_DEEPER_2.options.POLLINOUS_CORE", HoverTipFactory.FromRelic<PollinousCore>()).ThatDoesDamage((Decimal) ColossalFlower._prizeDamage[this.NumberOfDigs])
      }));
    }
  }

  private async Task ExtractCurrentPrize()
  {
    await PlayerCmd.GainGold((Decimal) ColossalFlower._prizeCosts[this.NumberOfDigs], this.Owner);
    this.SetEventFinished(this.L10NLookup("COLOSSAL_FLOWER.pages.EXTRACT_CURRENT_PRIZE.description"));
  }

  private async Task ExtractInstead()
  {
    await PlayerCmd.GainGold((Decimal) ColossalFlower._prizeCosts[this.NumberOfDigs], this.Owner);
    this.SetEventFinished(this.L10NLookup("COLOSSAL_FLOWER.pages.EXTRACT_INSTEAD.description"));
  }

  private async Task ObtainPollinousCore()
  {
    await this.DealReachDeeperDamage();
    PollinousCore pollinousCore = await RelicCmd.Obtain<PollinousCore>(this.Owner);
    this.SetEventFinished(this.L10NLookup("COLOSSAL_FLOWER.pages.POLLINOUS_CORE.description"));
  }

  private async Task DealReachDeeperDamage()
  {
    IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Owner.Creature, (Decimal) ColossalFlower._prizeDamage[this.NumberOfDigs], ValueProp.Unblockable | ValueProp.Unpowered, (CardModel) null, (CardPlay) null);
  }
}
