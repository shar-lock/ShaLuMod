// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.TheCourier
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Merchant;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class TheCourier : RelicModel
{
  private const string _discountKey = "Discount";

  public override RelicRarity Rarity => RelicRarity.Rare;

  public override bool IsAllowedInShops => false;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Discount", 20M));
    }
  }

  public override Decimal ModifyMerchantPrice(
    Player player,
    MerchantEntry entry,
    Decimal originalPrice)
  {
    return player != this.Owner ? originalPrice : originalPrice * (1M - this.DynamicVars["Discount"].BaseValue / 100M);
  }

  public override bool ShouldRefillMerchantEntry(MerchantEntry entry, Player player)
  {
    return player == this.Owner;
  }
}
