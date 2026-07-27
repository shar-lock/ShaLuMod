// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Enchantments.EnchantmentOption
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.Enchantments;

public struct EnchantmentOption(EnchantmentModel enchantment, int minAmount, int maxAmount)
{
  public readonly EnchantmentModel enchantment = enchantment;
  public readonly int minAmount = minAmount;
  public readonly int maxAmount = maxAmount;
}
