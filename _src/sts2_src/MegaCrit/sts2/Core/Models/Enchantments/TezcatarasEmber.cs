// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Enchantments.TezcatarasEmber
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Enchantments;

public sealed class TezcatarasEmber : EnchantmentModel
{
  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Eternal));
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new DamageVar(3M, ValueProp.Move));
    }
  }

  protected override void OnEnchant()
  {
    this.Card.EnergyCost.UpgradeBy(-this.Card.EnergyCost.GetWithModifiers(CostModifiers.None));
    this.Card.AddKeyword(CardKeyword.Eternal);
  }

  public override Decimal EnchantDamageAdditive(Decimal originalDamage, ValueProp props)
  {
    return !props.IsPoweredAttack() ? 0M : this.DynamicVars.Damage.BaseValue;
  }
}
