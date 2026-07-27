// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Mocks.MockAttackCard
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards.Mocks;

public sealed class MockAttackCard : MockCardModel
{
  private int _hitCount = 1;
  private bool _fromOsty;
  private TargetType _targetingType = TargetType.AnyEnemy;

  public override CardType Type => CardType.Attack;

  public override TargetType TargetType => this._targetingType;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[3]
      {
        (DynamicVar) new DamageVar(6M, ValueProp.Move),
        (DynamicVar) new OstyDamageVar(6M, ValueProp.Move),
        (DynamicVar) new BlockVar(0M, ValueProp.Move)
      });
    }
  }

  [PreserveBaseOverrides]
  MockAttackCard MockCardModel.MockBlock(int block)
  {
    this.AssertMutable();
    this.DynamicVars.Block.BaseValue = (Decimal) block;
    return this;
  }

  public MockAttackCard MockDamage(Decimal damage)
  {
    this.AssertMutable();
    this.DynamicVars.Damage.BaseValue = damage;
    return this;
  }

  public MockAttackCard MockOstyDamage(Decimal damage)
  {
    this.AssertMutable();
    this.DynamicVars.OstyDamage.BaseValue = damage;
    return this;
  }

  public MockAttackCard MockHitCount(int hitCount)
  {
    this.AssertMutable();
    this._hitCount = hitCount;
    return this;
  }

  public MockAttackCard MockFromOsty()
  {
    this.AssertMutable();
    this._fromOsty = true;
    this.MockTag(CardTag.OstyAttack);
    return this;
  }

  public MockAttackCard MockTargetingType(TargetType targetingType)
  {
    this.AssertMutable();
    this._targetingType = targetingType;
    return this;
  }

  public MockAttackCard MockUnpoweredDamage()
  {
    this.AssertMutable();
    this.DynamicVars.Damage.Props = ValueProp.Unpowered;
    this.DynamicVars.OstyDamage.Props = ValueProp.Unpowered;
    return this;
  }

  protected override int GetBaseBlock() => this.DynamicVars.Block.IntValue;

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    if (this._mockSelfHpLoss > 0)
    {
      IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(choiceContext, this.Owner.Creature, (Decimal) this._mockSelfHpLoss, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, (CardModel) this, cardPlay);
    }
    if (this.DynamicVars.Block.BaseValue > 0M)
    {
      Decimal num = await CreatureCmd.GainBlock(this.Owner.Creature, this.DynamicVars.Block, cardPlay);
    }
    int hitCount = !this.EnergyCost.CostsX ? (!this.HasStarCostX ? this._hitCount : this.ResolveStarXValue()) : this.ResolveEnergyXValue();
    AttackCommand attackCommand1 = DamageCmd.Attack(this._fromOsty ? this.DynamicVars.OstyDamage.BaseValue : this.DynamicVars.Damage.BaseValue).WithHitCount(hitCount);
    AttackCommand attackCommand2;
    if (this._fromOsty)
    {
      if (this.Owner.Osty == null)
        throw new InvalidOperationException("Must summon Osty before using osty attack!");
      attackCommand2 = attackCommand1.FromOsty(this.Owner.Osty, (CardModel) this, cardPlay);
    }
    else
      attackCommand2 = attackCommand1.FromCard((CardModel) this, cardPlay);
    AttackCommand attackCommand3;
    switch (this._targetingType)
    {
      case TargetType.AnyEnemy:
        ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
        attackCommand3 = attackCommand2.Targeting(cardPlay.Target);
        break;
      case TargetType.AllEnemies:
        attackCommand3 = attackCommand2.TargetingAllOpponents(this.CombatState);
        break;
      case TargetType.RandomEnemy:
        attackCommand3 = attackCommand2.TargetingRandomOpponents(this.CombatState);
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
    if (this.DynamicVars.Damage.Props.HasFlag((Enum) ValueProp.Unpowered))
      attackCommand3 = attackCommand3.Unpowered();
    AttackCommand attackCommand4 = await attackCommand3.Execute(choiceContext);
    if (this._mockExtraLogic == null)
      return;
    await this._mockExtraLogic((CardModel) this);
  }

  protected override void OnUpgrade()
  {
    if (this._mockUpgradeLogic != null)
      this._mockUpgradeLogic((CardModel) this);
    else
      this.DynamicVars.Damage.UpgradeValueBy(3M);
  }
}
