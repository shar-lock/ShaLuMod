// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Enchantments.Nimble
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Enchantments;

public sealed class Nimble : EnchantmentModel
{
  public override bool ShowAmount => true;

  public override bool CanEnchant(CardModel card) => base.CanEnchant(card) && card.GainsBlock;

  public override Decimal EnchantBlockAdditive(Decimal originalBlock) => (Decimal) this.Amount;
}
