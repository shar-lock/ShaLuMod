// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Enchantments.Spiral
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Enchantments;

public sealed class Spiral : EnchantmentModel
{
  private const string _timesKey = "Times";

  public override bool CanEnchant(CardModel c)
  {
    if (!base.CanEnchant(c) || c.Rarity != CardRarity.Basic)
      return false;
    return c.Tags.Contains<CardTag>(CardTag.Strike) || c.Tags.Contains<CardTag>(CardTag.Defend);
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new IntVar("Times", 1M));
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.ReplayDynamic, this.DynamicVars["Times"]));
    }
  }

  public override int EnchantPlayCount(int originalPlayCount)
  {
    return originalPlayCount + this.DynamicVars["Times"].IntValue;
  }
}
