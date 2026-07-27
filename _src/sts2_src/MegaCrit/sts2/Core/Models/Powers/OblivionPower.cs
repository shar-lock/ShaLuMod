// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.OblivionPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class OblivionPower : PowerModel
{
  public override PowerType Type => PowerType.Debuff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override PowerInstanceType InstanceType => PowerInstanceType.InstancedPerApplier;

  protected override object InitInternalData() => (object) new OblivionPower.Data();

  public override Task BeforeCardPlayed(CardPlay cardPlay)
  {
    if (this.Applier?.Player == null || cardPlay.Card.Owner != this.Applier.Player)
      return Task.CompletedTask;
    this.GetInternalData<OblivionPower.Data>().amountsForPlayedCards.Add(cardPlay.Card, this.Amount);
    return Task.CompletedTask;
  }

  public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    int amount;
    if (!this.GetInternalData<OblivionPower.Data>().amountsForPlayedCards.Remove(cardPlay.Card, ref amount))
      return;
    this.Flash();
    DoomPower doomPower = await PowerCmd.Apply<DoomPower>(choiceContext, this.Owner, (Decimal) amount, this.Applier, (CardModel) null);
  }

  public override async Task AfterSideTurnEnd(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IEnumerable<Creature> participants)
  {
    if (side != CombatSide.Player)
      return;
    await PowerCmd.Remove((PowerModel) this);
  }

  private class Data
  {
    public readonly Dictionary<CardModel, int> amountsForPlayedCards = new Dictionary<CardModel, int>();
  }
}
