// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Thrash
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Thrash : CardModel
{
  private Decimal _extraDamage;

  public Thrash()
    : base(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
  {
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Exhaust));
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new DamageVar(4M, ValueProp.Move));
    }
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

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
    AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).WithHitCount(2).FromCard((CardModel) this, cardPlay).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_thrash").Execute(choiceContext);
    CardModel cardModel = this.Owner.RunState.Rng.CombatCardSelection.NextItem<CardModel>(PileType.Hand.GetPile(this.Owner).Cards.Where<CardModel>((Func<CardModel, bool>) (c => c.Type == CardType.Attack)));
    if (cardModel == null)
      return;
    Decimal damage1 = 0M;
    if (cardModel.DynamicVars.ContainsKey("CalculatedDamage"))
      damage1 = cardModel.DynamicVars.CalculatedDamage.Calculate((Creature) null);
    else if (cardModel.DynamicVars.ContainsKey("Damage"))
      damage1 = cardModel.DynamicVars.Damage.BaseValue;
    else if (cardModel.DynamicVars.ContainsKey("OstyDamage"))
      damage1 = cardModel.DynamicVars.OstyDamage.BaseValue;
    else
      Log.Warn($"{this.Id.Entry} exhausted attack card {cardModel.Id.Entry} that did not have an appropriate damage var!");
    Decimal num = Hook.ModifyDamage(this.Owner.RunState, this.Owner.Creature.CombatState, (Creature) null, this.Owner.Creature, damage1, ValueProp.Move, cardModel, (CardPlay) null, ModifyDamageHookType.All, CardPreviewMode.None, out IEnumerable<AbstractModel> _);
    DamageVar damage2 = this.DynamicVars.Damage;
    damage2.BaseValue = damage2.BaseValue + num;
    this.ExtraDamage += num;
    await CardCmd.Exhaust(choiceContext, cardModel);
  }

  protected override void AfterDowngraded()
  {
    base.AfterDowngraded();
    DamageVar damage = this.DynamicVars.Damage;
    damage.BaseValue = damage.BaseValue + this.ExtraDamage;
  }

  protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(2M);
}
