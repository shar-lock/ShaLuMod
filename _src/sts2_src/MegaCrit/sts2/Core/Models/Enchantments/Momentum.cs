// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Enchantments.Momentum
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Enchantments;

public sealed class Momentum : EnchantmentModel
{
  private int _extraDamage;

  public override bool CanEnchantCardType(CardType cardType) => cardType == CardType.Attack;

  public override bool HasExtraCardText => true;

  public override bool ShowAmount => true;

  private int ExtraDamage
  {
    get => this._extraDamage;
    set
    {
      this.AssertMutable();
      this._extraDamage = value;
    }
  }

  public override Task OnPlay(PlayerChoiceContext choiceContext, CardPlay? cardPlay)
  {
    this.ExtraDamage += this.Amount;
    return Task.CompletedTask;
  }

  public override Decimal EnchantDamageAdditive(Decimal originalDamage, ValueProp props)
  {
    return !props.IsPoweredAttack() ? 0M : (Decimal) this.ExtraDamage;
  }
}
