// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Enchantments.Corrupted
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Enchantments;

public sealed class Corrupted : EnchantmentModel
{
  private const Decimal _damageAmount = 2M;

  public override bool HasExtraCardText => true;

  public override bool CanEnchantCardType(CardType cardType) => cardType == CardType.Attack;

  public override Decimal EnchantDamageMultiplicative(Decimal originalDamage, ValueProp props)
  {
    return !props.IsPoweredAttack() ? 1M : 1.5M;
  }

  public override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay? cardPlay)
  {
    IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(choiceContext, this.Card.Owner.Creature, 2M, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this.Card, cardPlay);
  }
}
