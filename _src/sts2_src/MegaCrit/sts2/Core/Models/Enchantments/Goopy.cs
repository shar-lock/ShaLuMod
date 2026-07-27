// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Enchantments.Goopy
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Enchantments;

public sealed class Goopy : EnchantmentModel
{
  public override bool HasExtraCardText => true;

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Exhaust));
    }
  }

  public override bool CanEnchant(CardModel card)
  {
    return base.CanEnchant(card) && card.Tags.Contains<CardTag>(CardTag.Defend);
  }

  protected override void OnEnchant() => this.Card.AddKeyword(CardKeyword.Exhaust);

  public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    if (cardPlay.Card != this.Card)
      return Task.CompletedTask;
    ++this.Amount;
    if (this.Card.DeckVersion != null)
      ++this.Card.DeckVersion.Enchantment.Amount;
    return Task.CompletedTask;
  }

  public override Decimal EnchantBlockAdditive(Decimal originalBlock)
  {
    return (Decimal) (this.Amount - 1);
  }
}
