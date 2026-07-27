// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Enchantments.Vigorous
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Enchantments;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Enchantments;

public sealed class Vigorous : EnchantmentModel
{
  public override bool CanEnchantCardType(CardType cardType) => cardType == CardType.Attack;

  public override bool ShowAmount => true;

  public override Decimal EnchantDamageAdditive(Decimal originalDamage, ValueProp props)
  {
    return this.Status != EnchantmentStatus.Normal || !props.IsPoweredAttack() ? 0M : (Decimal) this.Amount;
  }

  public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    if (cardPlay.Card != this.Card)
      return Task.CompletedTask;
    this.Status = EnchantmentStatus.Disabled;
    return Task.CompletedTask;
  }
}
