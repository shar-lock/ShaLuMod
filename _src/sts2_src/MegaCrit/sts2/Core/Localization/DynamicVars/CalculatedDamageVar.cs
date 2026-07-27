// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Localization.DynamicVars.CalculatedDamageVar
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Localization.DynamicVars;

public class CalculatedDamageVar : CalculatedVar
{
  public const string defaultName = "CalculatedDamage";

  public ValueProp Props { get; }

  public bool IsFromOsty { get; private set; }

  public CalculatedDamageVar(ValueProp props)
    : base("CalculatedDamage")
  {
    this.Props = props;
  }

  public CalculatedDamageVar FromOsty()
  {
    this.IsFromOsty = true;
    return this;
  }

  public override void UpdateCardPreview(
    CardModel card,
    CardPreviewMode previewMode,
    Creature? target,
    bool runGlobalHooks)
  {
    EnchantmentModel enchantment = card.Enchantment;
    if (enchantment != null)
    {
      Decimal baseValue = this.GetBaseVar().BaseValue;
      Decimal originalDamage = baseValue + enchantment.EnchantDamageAdditive(baseValue, this.Props);
      Decimal num = Math.Max(originalDamage * enchantment.EnchantDamageMultiplicative(originalDamage, this.Props), 0M);
      if (card.IsEnchantmentPreview)
        this.PreviewValue = num;
      else
        this.EnchantedValue = num;
    }
    Decimal num1 = this.Calculate(target);
    if (runGlobalHooks)
    {
      ICombatState combatState = card.CombatState ?? card.Owner.Creature.CombatState;
      this.PreviewValue = Hook.ModifyDamage(card.Owner.RunState, combatState, target, this.IsFromOsty ? card.Owner.Osty : card.Owner.Creature, num1, this.Props, card, (CardPlay) null, ModifyDamageHookType.All, previewMode, out IEnumerable<AbstractModel> _);
    }
    else if (!card.IsEnchantmentPreview)
    {
      if (enchantment != null)
      {
        Decimal originalDamage = num1 + enchantment.EnchantDamageAdditive(num1, this.Props);
        num1 = originalDamage * enchantment.EnchantDamageMultiplicative(originalDamage, this.Props);
      }
      this.PreviewValue = num1;
    }
    this.PreviewValue = Math.Max(this.PreviewValue, 0M);
  }

  protected override DynamicVar GetExtraVar()
  {
    return (DynamicVar) ((CardModel) this._owner).DynamicVars.ExtraDamage;
  }
}
