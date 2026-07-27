// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Claw
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Claw : CardModel
{
  private const string _increaseKey = "Increase";
  private Decimal _extraDamageFromClawPlays;

  public Claw()
    : base(0, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
  {
  }

  private Decimal ExtraDamageFromClawPlays
  {
    get => this._extraDamageFromClawPlays;
    set
    {
      this.AssertMutable();
      this._extraDamageFromClawPlays = value;
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new DamageVar(3M, ValueProp.Move),
        new DynamicVar("Increase", 2M)
      });
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
    AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard((CardModel) this, cardPlay).Targeting(cardPlay.Target).WithHitVfxNode((Func<Creature, Node2D>) (t => (Node2D) NScratchVfx.Create(t, true))).Execute(choiceContext);
    IEnumerable<Claw> claws = this.Owner.PlayerCombatState.AllCards.OfType<Claw>();
    Decimal baseValue = this.DynamicVars["Increase"].BaseValue;
    foreach (Claw claw in claws)
      claw.BuffFromClawPlay(baseValue);
  }

  protected override void OnUpgrade()
  {
    this.DynamicVars.Damage.UpgradeValueBy(1M);
    this.DynamicVars["Increase"].UpgradeValueBy(1M);
  }

  protected override void AfterDowngraded()
  {
    base.AfterDowngraded();
    DamageVar damage = this.DynamicVars.Damage;
    damage.BaseValue = damage.BaseValue + this.ExtraDamageFromClawPlays;
  }

  private void BuffFromClawPlay(Decimal extraDamage)
  {
    DamageVar damage = this.DynamicVars.Damage;
    damage.BaseValue = damage.BaseValue + extraDamage;
    this.ExtraDamageFromClawPlays += extraDamage;
  }
}
