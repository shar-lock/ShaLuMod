// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.LethalityPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class LethalityPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override Decimal ModifyDamageMultiplicative(
    Creature? target,
    Decimal amount,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource,
    CardPlay? cardPlay)
  {
    if (!props.IsPoweredAttack() || cardSource == null || cardSource.Owner.Creature != this.Owner)
      return 1M;
    if (cardSource != null)
    {
      CardPile pile = cardSource.Pile;
      if (pile != null && pile.Type == PileType.Play && cardSource.CurrentPlayIndex > 0)
        return 1M;
    }
    int num1 = CombatManager.Instance.History.CardPlaysStarted.Count<CardPlayStartedEntry>((Func<CardPlayStartedEntry, bool>) (e => e.HappenedThisTurn(this.CombatState) && e.CardPlay.Card.Type == CardType.Attack && e.CardPlay.Player == this.Owner.Player));
    CardPile pile1 = cardSource.Pile;
    int num2 = pile1 != null && pile1.Type == PileType.Play ? 1 : 0;
    return num1 > num2 ? 1M : 1M + (Decimal) this.Amount / 100M;
  }
}
