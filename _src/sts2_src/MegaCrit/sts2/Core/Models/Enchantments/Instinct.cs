// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Enchantments.Instinct
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using System;

#nullable disable
namespace MegaCrit.Sts2.Core.Models.Enchantments;

public sealed class Instinct : EnchantmentModel
{
  public override bool CanEnchantCardType(CardType cardType) => cardType == CardType.Attack;

  public override Decimal EnchantDamageMultiplicative(Decimal originalDamage, ValueProp props)
  {
    return !props.IsPoweredAttack() ? 1M : 2M;
  }
}
