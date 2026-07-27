// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Localization.DynamicVars.OstyDamageVar
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

public class OstyDamageVar : DynamicVar
{
  public const string defaultName = "OstyDamage";

  public ValueProp Props { get; set; }

  public OstyDamageVar(Decimal damage, ValueProp props)
    : base("OstyDamage", damage)
  {
    this.Props = props;
  }

  public OstyDamageVar(string name, Decimal damage, ValueProp props)
    : base(name, damage)
  {
    this.Props = props;
  }

  public override void UpdateCardPreview(
    CardModel card,
    CardPreviewMode previewMode,
    Creature? target,
    bool runGlobalHooks)
  {
    Decimal originalDamage1 = this.BaseValue;
    EnchantmentModel enchantment = card.Enchantment;
    if (enchantment != null)
    {
      Decimal originalDamage2 = originalDamage1 + enchantment.EnchantDamageAdditive(originalDamage1, this.Props);
      originalDamage1 = originalDamage2 * enchantment.EnchantDamageMultiplicative(originalDamage2, this.Props);
      if (!card.IsEnchantmentPreview)
        this.EnchantedValue = originalDamage1;
    }
    if (runGlobalHooks)
    {
      ICombatState combatState = card.CombatState ?? card.Owner.Creature.CombatState;
      originalDamage1 = Hook.ModifyDamage(card.Owner.RunState, combatState, target, card.Owner.Osty, this.BaseValue, this.Props, card, (CardPlay) null, ModifyDamageHookType.All, previewMode, out IEnumerable<AbstractModel> _);
    }
    this.PreviewValue = originalDamage1;
  }
}
