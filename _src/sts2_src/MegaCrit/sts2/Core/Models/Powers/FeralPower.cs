// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.FeralPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class FeralPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override int DisplayAmount
  {
    get => Math.Max(0, this.Amount - this.GetInternalData<FeralPower.Data>().zeroCostAttacksPlayed);
  }

  protected override object InitInternalData() => (object) new FeralPower.Data();

  public override Task AfterApplied(Creature? applier, CardModel? cardSource)
  {
    this.SetZeroCostAttacksPlayed(CombatManager.Instance.History.Entries.OfType<CardPlayStartedEntry>().Count<CardPlayStartedEntry>((Func<CardPlayStartedEntry, bool>) (e => e.CardPlay.Card.Type == CardType.Attack && e.CardPlay.Player == this.Owner.Player && e.CardPlay.Resources.EnergyValue == 0 && e.HappenedThisTurn(this.CombatState))));
    return Task.CompletedTask;
  }

  public override CardLocation ModifyCardPlayResultLocation(
    CardModel card,
    bool isAutoPlay,
    ResourceInfo resources,
    CardLocation location)
  {
    if (card.Owner.Creature != this.Owner || card.Type != CardType.Attack || resources.EnergyValue > 0 || card.IsDupe || this.GetInternalData<FeralPower.Data>().zeroCostAttacksPlayed >= this.Amount)
      return location;
    location.pileType = PileType.Hand;
    location.position = CardPilePosition.Top;
    return location;
  }

  public override Task AfterModifyingCardPlayResultLocation(CardModel card, CardLocation location)
  {
    this.Flash();
    this.SetZeroCostAttacksPlayed(this.GetInternalData<FeralPower.Data>().zeroCostAttacksPlayed + 1);
    this.InvokeDisplayAmountChanged();
    return Task.CompletedTask;
  }

  public override Task AfterSideTurnStart(
    CombatSide side,
    IReadOnlyList<Creature> participants,
    ICombatState combatState)
  {
    if (!participants.Contains<Creature>(this.Owner))
      return Task.CompletedTask;
    this.SetZeroCostAttacksPlayed(0);
    this.InvokeDisplayAmountChanged();
    return Task.CompletedTask;
  }

  private void SetZeroCostAttacksPlayed(int value)
  {
    this.GetInternalData<FeralPower.Data>().zeroCostAttacksPlayed = value;
    this.InvokeDisplayAmountChanged();
  }

  private class Data
  {
    public int zeroCostAttacksPlayed;
  }
}
