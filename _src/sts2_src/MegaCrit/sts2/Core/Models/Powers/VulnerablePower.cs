// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.VulnerablePower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class VulnerablePower : PowerModel
{
  private const string _damageIncrease = "DamageIncrease";

  public override PowerType Type => PowerType.Debuff;

  public override PowerStackType StackType => PowerStackType.Counter;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("DamageIncrease", 1.5M));
    }
  }

  public override Decimal ModifyDamageMultiplicative(
    Creature? target,
    Decimal amount,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource,
    CardPlay? cardPlay)
  {
    if (target != this.Owner || !props.IsPoweredAttack())
      return 1M;
    Decimal amount1 = this.DynamicVars["DamageIncrease"].BaseValue;
    if (dealer != null)
    {
      PaperPhrog relic = dealer.Player?.GetRelic<PaperPhrog>();
      if (relic != null)
        amount1 = relic.ModifyVulnerableMultiplier(target, amount1, props, dealer, cardSource);
      CrueltyPower crueltyPower = dealer.GetPower<CrueltyPower>() ?? dealer.PetOwner?.Creature.GetPower<CrueltyPower>();
      if (crueltyPower != null)
        amount1 = crueltyPower.ModifyVulnerableMultiplier(target, amount1, props, dealer, cardSource);
    }
    DebilitatePower power = target.GetPower<DebilitatePower>();
    if (power != null)
      amount1 = power.ModifyVulnerableMultiplier(target, amount1, props, dealer, cardSource);
    return amount1;
  }

  public override async Task AfterSideTurnEnd(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IEnumerable<Creature> participants)
  {
    if (side != CombatSide.Enemy)
      return;
    await PowerCmd.TickDownDuration((PowerModel) this);
  }
}
