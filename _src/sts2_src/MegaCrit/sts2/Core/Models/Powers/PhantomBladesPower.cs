// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.PhantomBladesPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class PhantomBladesPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlyArray<IHoverTip>(new IHoverTip[2]
      {
        HoverTipFactory.FromCard<Shiv>(),
        HoverTipFactory.FromKeyword(CardKeyword.Retain)
      });
    }
  }

  public override Task AfterCardEnteredCombat(CardModel card)
  {
    if (!card.Tags.Contains<CardTag>(CardTag.Shiv) || card.Owner != this.Owner.Player)
      return Task.CompletedTask;
    CardCmd.ApplyKeyword(card, CardKeyword.Retain);
    return Task.CompletedTask;
  }

  public override Task AfterApplied(Creature? applier, CardModel? cardSource)
  {
    foreach (CardModel card in this.Owner.Player.PlayerCombatState.AllCards.Where<CardModel>((Func<CardModel, bool>) (c => c.Tags.Contains<CardTag>(CardTag.Shiv))))
      CardCmd.ApplyKeyword(card, CardKeyword.Retain);
    return Task.CompletedTask;
  }

  public override Decimal ModifyDamageAdditive(
    Creature? target,
    Decimal amount,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource,
    CardPlay? cardPlay)
  {
    return !props.IsPoweredAttack() || cardSource == null || !cardSource.Tags.Contains<CardTag>(CardTag.Shiv) || dealer != this.Owner || CombatManager.Instance.History.CardPlaysFinished.Count<CardPlayFinishedEntry>((Func<CardPlayFinishedEntry, bool>) (e => e.HappenedThisTurn(this.CombatState) && e.CardPlay.Card.Tags.Contains<CardTag>(CardTag.Shiv) && e.CardPlay.Player == this.Owner.Player)) > 0 ? 0M : (Decimal) this.Amount;
  }
}
