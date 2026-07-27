// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Cards.LocalCostModifier
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.Cards;

public class LocalCostModifier
{
  public int Amount { get; set; }

  public LocalCostType Type { get; }

  public LocalCostModifierExpiration Expiration { get; }

  public bool IsReduceOnly { get; }

  public LocalCostModifier(
    int amount,
    LocalCostType type,
    LocalCostModifierExpiration expiration,
    bool reduceOnly)
  {
    this.Amount = amount;
    this.Type = type;
    this.Expiration = expiration;
    this.IsReduceOnly = reduceOnly;
  }

  public int Modify(int currentCost)
  {
    switch (this.Type)
    {
      case LocalCostType.Absolute:
        return this.IsReduceOnly ? Math.Min(currentCost, this.Amount) : this.Amount;
      case LocalCostType.Relative:
        return this.IsReduceOnly ? Math.Min(currentCost, currentCost + this.Amount) : currentCost + this.Amount;
      default:
        throw new ArgumentOutOfRangeException("Type", (object) this.Type, (string) null);
    }
  }

  public LocalCostModifier Clone()
  {
    return new LocalCostModifier(this.Amount, this.Type, this.Expiration, this.IsReduceOnly);
  }
}
