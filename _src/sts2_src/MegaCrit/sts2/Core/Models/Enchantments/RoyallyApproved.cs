// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Enchantments.RoyallyApproved
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Enchantments;

public sealed class RoyallyApproved : EnchantmentModel
{
  public override bool CanEnchantCardType(CardType cardType)
  {
    bool flag;
    switch (cardType)
    {
      case CardType.Attack:
      case CardType.Skill:
        flag = true;
        break;
      default:
        flag = false;
        break;
    }
    return flag;
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlyArray<IHoverTip>(new IHoverTip[2]
      {
        HoverTipFactory.FromKeyword(CardKeyword.Innate),
        HoverTipFactory.FromKeyword(CardKeyword.Retain)
      });
    }
  }

  protected override void OnEnchant()
  {
    this.Card.AddKeyword(CardKeyword.Innate);
    this.Card.AddKeyword(CardKeyword.Retain);
  }
}
