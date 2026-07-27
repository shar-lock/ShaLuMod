// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Potions.PotionRarityExtensions
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Localization;
using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.Potions;

public static class PotionRarityExtensions
{
  public static LocString ToLocString(this PotionRarity potionRarity)
  {
    LocString locString;
    switch (potionRarity)
    {
      case PotionRarity.None:
        throw new ArgumentOutOfRangeException(nameof (potionRarity), (object) potionRarity, (string) null);
      case PotionRarity.Common:
        locString = new LocString("gameplay_ui", "POTION_RARITY.COMMON");
        break;
      case PotionRarity.Uncommon:
        locString = new LocString("gameplay_ui", "POTION_RARITY.UNCOMMON");
        break;
      case PotionRarity.Rare:
        locString = new LocString("gameplay_ui", "POTION_RARITY.RARE");
        break;
      case PotionRarity.Event:
        locString = new LocString("gameplay_ui", "POTION_RARITY.EVENT");
        break;
      case PotionRarity.Token:
        locString = new LocString("gameplay_ui", "POTION_RARITY.TOKEN");
        break;
      default:
        // ISSUE: reference to a compiler-generated method
        \u003CPrivateImplementationDetails\u003E.ThrowSwitchExpressionException((object) potionRarity);
        break;
    }
    return locString;
  }
}
