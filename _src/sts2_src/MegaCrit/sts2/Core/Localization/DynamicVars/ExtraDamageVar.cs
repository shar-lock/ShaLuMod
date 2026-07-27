// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Localization.DynamicVars.ExtraDamageVar
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Localization.DynamicVars;

public class ExtraDamageVar(Decimal damage) : DynamicVar("ExtraDamage", damage)
{
  public const string defaultName = "ExtraDamage";

  public bool IsFromOsty { get; private set; }

  public ExtraDamageVar FromOsty()
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
    Decimal baseValue = this.BaseValue;
    EnchantmentModel enchantment = card.Enchantment;
    if (enchantment != null)
    {
      baseValue *= enchantment.EnchantDamageMultiplicative(baseValue, ValueProp.Move);
      if (!card.IsEnchantmentPreview)
        this.EnchantedValue = baseValue;
    }
    this.PreviewValue = baseValue;
  }
}
