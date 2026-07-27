// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.BeatingRemnant
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class BeatingRemnant : RelicModel
{
  private const string _maxHpLossKey = "MaxHpLoss";
  private Decimal _damageReceivedThisTurn;

  public override RelicRarity Rarity => RelicRarity.Rare;

  private Decimal DamageReceivedThisTurn
  {
    get => this._damageReceivedThisTurn;
    set
    {
      this.AssertMutable();
      this._damageReceivedThisTurn = value;
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("MaxHpLoss", 20M));
    }
  }

  public override Decimal ModifyHpLostAfterOsty(
    Creature target,
    Decimal amount,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource)
  {
    return !CombatManager.Instance.IsInProgress || target != this.Owner.Creature ? amount : Math.Min(amount, this.DynamicVars["MaxHpLoss"].BaseValue - this.DamageReceivedThisTurn);
  }

  public override Task AfterModifyingHpLostAfterOsty()
  {
    this.Flash();
    return Task.CompletedTask;
  }

  public override Task AfterDamageReceived(
    PlayerChoiceContext choiceContext,
    Creature target,
    DamageResult result,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource)
  {
    if (!CombatManager.Instance.IsInProgress || target != this.Owner.Creature)
      return Task.CompletedTask;
    this.DamageReceivedThisTurn += (Decimal) result.UnblockedDamage;
    return Task.CompletedTask;
  }

  public override Task BeforeSideTurnStart(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IReadOnlyList<Creature> participants,
    ICombatState combatState)
  {
    if (!participants.Contains<Creature>(this.Owner.Creature))
      return Task.CompletedTask;
    this.DamageReceivedThisTurn = 0M;
    return Task.CompletedTask;
  }
}
