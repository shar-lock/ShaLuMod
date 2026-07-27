// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Localization.DynamicVars.BlockVar
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

public class BlockVar : DynamicVar
{
  public const string defaultName = "Block";

  public ValueProp Props { get; }

  public BlockVar(Decimal block, ValueProp props)
    : base("Block", block)
  {
    this.Props = props;
  }

  public BlockVar(string name, Decimal block, ValueProp props)
    : base(name, block)
  {
    this.Props = props;
  }

  public override void UpdateCardPreview(
    CardModel card,
    CardPreviewMode previewMode,
    Creature? target,
    bool runGlobalHooks)
  {
    Decimal originalBlock1 = this.BaseValue;
    EnchantmentModel enchantment = card.Enchantment;
    if (enchantment != null)
    {
      Decimal originalBlock2 = originalBlock1 + enchantment.EnchantBlockAdditive(originalBlock1);
      originalBlock1 = originalBlock2 * enchantment.EnchantBlockMultiplicative(originalBlock2);
      if (!card.IsEnchantmentPreview)
        this.EnchantedValue = originalBlock1;
    }
    if (runGlobalHooks)
      originalBlock1 = Hook.ModifyBlock(card.CombatState, card.Owner.Creature, this.BaseValue, this.Props, card, (CardPlay) null, out IEnumerable<AbstractModel> _);
    this.PreviewValue = originalBlock1;
  }
}
