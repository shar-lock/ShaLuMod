// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.KinglyPunch
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class KinglyPunch : CardModel
{
  private const string _increaseKey = "Increase";
  private Decimal _extraDamage;

  public KinglyPunch()
    : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
  {
  }

  private Decimal ExtraDamage
  {
    get => this._extraDamage;
    set
    {
      this.AssertMutable();
      this._extraDamage = value;
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new DamageVar(8M, ValueProp.Move),
        new DynamicVar("Increase", 4M)
      });
    }
  }

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
    AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).FromCard((CardModel) this, cardPlay).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
  }

  public override Task AfterCardDrawn(
    PlayerChoiceContext choiceContext,
    CardModel card,
    bool fromHandDraw)
  {
    if (card != this)
      return Task.CompletedTask;
    Decimal baseValue = this.DynamicVars["Increase"].BaseValue;
    DamageVar damage = this.DynamicVars.Damage;
    damage.BaseValue = damage.BaseValue + baseValue;
    this.ExtraDamage += baseValue;
    return Task.CompletedTask;
  }

  protected override void OnUpgrade()
  {
    this.DynamicVars.Damage.UpgradeValueBy(2M);
    this.DynamicVars["Increase"].UpgradeValueBy(2M);
  }

  protected override void AfterDowngraded()
  {
    base.AfterDowngraded();
    DamageVar damage = this.DynamicVars.Damage;
    damage.BaseValue = damage.BaseValue + this.ExtraDamage;
  }
}
