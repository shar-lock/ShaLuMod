// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.MonologuePower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class MonologuePower : PowerModel
{
  public const string strengthAppliedKey = "StrengthApplied";

  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType
  {
    get
    {
      return this.DynamicVars["StrengthApplied"].IntValue != 0 ? PowerStackType.Counter : PowerStackType.None;
    }
  }

  public override int DisplayAmount => this.DynamicVars["StrengthApplied"].IntValue;

  public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new PowerVar<StrengthPower>(1M),
        new DynamicVar("StrengthApplied", 0M)
      });
    }
  }

  protected override object InitInternalData() => (object) new MonologuePower.Data();

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>());
    }
  }

  public override Task BeforeCardPlayed(CardPlay cardPlay)
  {
    if (cardPlay.Card.Owner.Creature != this.Owner)
      return Task.CompletedTask;
    this.GetInternalData<MonologuePower.Data>().amountsForPlayedCards.Add(cardPlay.Card, this.DynamicVars.Strength.IntValue);
    return Task.CompletedTask;
  }

  public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    int amount;
    if (cardPlay.Card.Owner != this.Owner.Player || !this.GetInternalData<MonologuePower.Data>().amountsForPlayedCards.Remove(cardPlay.Card, ref amount))
      return;
    this.Flash();
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>(choiceContext, this.Owner, (Decimal) amount, this.Owner, (CardModel) null, true);
    this.DynamicVars["StrengthApplied"].BaseValue += (Decimal) this.DynamicVars.Strength.IntValue;
    this.InvokeDisplayAmountChanged();
  }

  public override async Task AfterSideTurnEnd(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IEnumerable<Creature> participants)
  {
    if (!participants.Contains<Creature>(this.Owner))
      return;
    await PowerCmd.Remove((PowerModel) this);
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>(choiceContext, this.Owner, -this.DynamicVars["StrengthApplied"].BaseValue, this.Owner, (CardModel) null, true);
  }

  private class Data
  {
    public readonly Dictionary<CardModel, int> amountsForPlayedCards = new Dictionary<CardModel, int>();
  }
}
