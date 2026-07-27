// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.VitruvianMinion
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class VitruvianMinion : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Shop;

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block));
    }
  }

  public override Decimal ModifyDamageMultiplicative(
    Creature? target,
    Decimal amount,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource,
    CardPlay? cardPlay)
  {
    return cardSource == null || cardSource.Owner != this.Owner || !cardSource.Tags.Contains<CardTag>(CardTag.Minion) ? 1M : 2M;
  }

  public override Decimal ModifyBlockMultiplicative(
    Creature target,
    Decimal block,
    ValueProp props,
    CardModel? cardSource,
    CardPlay? cardPlay)
  {
    return cardSource == null || cardSource.Owner != this.Owner || !cardSource.Tags.Contains<CardTag>(CardTag.Minion) ? 1M : 2M;
  }
}
