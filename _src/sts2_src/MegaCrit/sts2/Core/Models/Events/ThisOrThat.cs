// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Events.ThisOrThat
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Events;

public sealed class ThisOrThat : EventModel
{
  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[3]
      {
        (DynamicVar) new HpLossVar(6M),
        (DynamicVar) new GoldVar(0),
        (DynamicVar) new StringVar("Curse", ModelDb.Card<Clumsy>().Title)
      });
    }
  }

  public override void CalculateVars()
  {
    this.DynamicVars.Gold.BaseValue = (Decimal) this.Rng.NextInt(41, 69);
  }

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<EventOption>) new \u003C\u003Ez__ReadOnlyArray<EventOption>(new EventOption[2]
    {
      new EventOption((EventModel) this, new Func<Task>(this.Plain), "THIS_OR_THAT.pages.INITIAL.options.PLAIN", Array.Empty<IHoverTip>()).ThatDoesDamage((Decimal) this.DynamicVars.HpLoss.IntValue),
      new EventOption((EventModel) this, new Func<Task>(this.Ornate), "THIS_OR_THAT.pages.INITIAL.options.ORNATE", HoverTipFactory.FromCardWithCardHoverTips<Clumsy>())
    });
  }

  private async Task Plain()
  {
    IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Owner.Creature, (Decimal) this.DynamicVars.HpLoss.IntValue, ValueProp.Unblockable | ValueProp.Unpowered, (Creature) null, (CardModel) null, (CardPlay) null);
    await PlayerCmd.GainGold((Decimal) this.DynamicVars.Gold.IntValue, this.Owner);
    this.SetEventFinished(this.L10NLookup("THIS_OR_THAT.pages.PLAIN.description"));
  }

  private async Task Ornate()
  {
    RelicModel relicModel = await RelicCmd.Obtain(RelicFactory.PullNextRelicFromFront(this.Owner).ToMutable(), this.Owner);
    CardModel deck = await CardPileCmd.AddCurseToDeck<Clumsy>(this.Owner);
    this.SetEventFinished(this.L10NLookup("THIS_OR_THAT.pages.ORNATE.description"));
  }
}
