// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.SlowPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class SlowPower : PowerModel
{
  private const string _slowAmountKey = "SlowAmount";

  public override PowerType Type => PowerType.Debuff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override int DisplayAmount => this.DynamicVars["SlowAmount"].IntValue * 10;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("SlowAmount", 0M));
    }
  }

  public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    ++this.DynamicVars["SlowAmount"].BaseValue;
    this.InvokeDisplayAmountChanged();
    return Task.CompletedTask;
  }

  public override Decimal ModifyDamageMultiplicative(
    Creature? target,
    Decimal amount,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource,
    CardPlay? cardPlay)
  {
    return target != this.Owner || !props.IsPoweredAttack() ? 1M : 1M + 0.1M * this.DynamicVars["SlowAmount"].BaseValue;
  }

  public override Task AfterModifyingDamageAmount(CardModel? cardSource)
  {
    this.Flash();
    return Task.CompletedTask;
  }

  public override Task AfterSideTurnStart(
    CombatSide side,
    IReadOnlyList<Creature> participants,
    ICombatState combatState)
  {
    if (!participants.Contains<Creature>(this.Owner))
      return Task.CompletedTask;
    this.DynamicVars["SlowAmount"].BaseValue = 0M;
    this.InvokeDisplayAmountChanged();
    return Task.CompletedTask;
  }
}
