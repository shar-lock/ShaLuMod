// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Localization.DynamicVars.CalculatedBlockVar
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Localization.DynamicVars;

public class CalculatedBlockVar : CalculatedVar
{
  public const string defaultName = "CalculatedBlock";

  public ValueProp Props { get; }

  public CalculatedBlockVar(ValueProp props)
    : base("CalculatedBlock")
  {
    this.Props = props;
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
      Decimal originalBlock = baseValue + enchantment.EnchantBlockAdditive(baseValue);
      Decimal num = originalBlock * enchantment.EnchantBlockMultiplicative(originalBlock);
      if (card.IsEnchantmentPreview)
        this.PreviewValue = num;
      else
        this.EnchantedValue = num;
    }
    Decimal originalBlock1 = this.Calculate(target);
    if (runGlobalHooks)
    {
      this.PreviewValue = Hook.ModifyBlock(card.CombatState, card.Owner.Creature, this.Calculate(target), this.Props, card, (CardPlay) null, out IEnumerable<AbstractModel> _);
    }
    else
    {
      if (card.IsEnchantmentPreview)
        return;
      if (enchantment != null)
      {
        Decimal originalBlock2 = originalBlock1 + enchantment.EnchantBlockAdditive(originalBlock1);
        originalBlock1 = originalBlock2 * enchantment.EnchantBlockMultiplicative(originalBlock2);
      }
      this.PreviewValue = originalBlock1;
    }
  }
}
