// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Enchantments.SoulsPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Enchantments;

public sealed class SoulsPower : EnchantmentModel
{
  public override bool CanEnchant(CardModel card)
  {
    return base.CanEnchant(card) && card.GetKeywordsWithSources(KeywordSources.Local).Contains(CardKeyword.Exhaust);
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Exhaust));
    }
  }

  protected override void OnEnchant() => CardCmd.RemoveKeyword(this.Card, CardKeyword.Exhaust);
}
