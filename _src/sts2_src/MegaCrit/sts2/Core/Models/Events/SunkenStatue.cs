// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.SunkenStatue
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class SunkenStatue : EventModel
{
  private const string _relicKey = "Relic";
  private const string _hpLossKey = "HpLoss";
  private const int _goldVariance = 10;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[3]
      {
        (DynamicVar) new StringVar("Relic", ModelDb.Relic<SwordOfStone>().Title.GetFormattedText()),
        (DynamicVar) new GoldVar(111),
        new DynamicVar("HpLoss", 7M)
      });
    }
  }

  public override void CalculateVars()
  {
    GoldVar gold = this.DynamicVars.Gold;
    gold.BaseValue = gold.BaseValue + (Decimal) this.Rng.NextInt(-10, 11);
  }

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
    {
      new EventOption((EventModel) this, new Func<Task>(this.GrabSword), "SUNKEN_STATUE.pages.INITIAL.options.GRAB_SWORD", HoverTipFactory.FromRelic<SwordOfStone>()),
      new EventOption((EventModel) this, new Func<Task>(this.DiveIntoWater), "SUNKEN_STATUE.pages.INITIAL.options.DIVE_INTO_WATER", Array.Empty<IHoverTip>()).ThatDoesDamage(this.DynamicVars["HpLoss"].BaseValue)
    });
  }

  private async Task GrabSword()
  {
    SwordOfStone swordOfStone = await RelicCmd.Obtain<SwordOfStone>(this.Owner);
    this.SetEventFinished(this.L10NLookup("SUNKEN_STATUE.pages.GRAB_SWORD.description"));
  }

  private async Task DiveIntoWater()
  {
    await PlayerCmd.GainGold(this.DynamicVars.Gold.BaseValue, this.Owner);
    IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Owner.Creature, this.DynamicVars["HpLoss"].BaseValue, ValueProp.Unblockable | ValueProp.Unpowered, (Creature) null, (CardModel) null, (CardPlay) null);
    this.SetEventFinished(this.L10NLookup("SUNKEN_STATUE.pages.DIVE_INTO_WATER.description"));
  }
}
