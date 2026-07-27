// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.StranglePower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class StranglePower : PowerModel
{
  public override PowerType Type => PowerType.Debuff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override PowerInstanceType InstanceType => PowerInstanceType.InstancedPerApplier;

  protected override object InitInternalData() => (object) new StranglePower.Data();

  public override Task BeforeCardPlayed(CardPlay cardPlay)
  {
    if (this.Applier?.Player == null || cardPlay.Card.Owner != this.Applier.Player)
      return Task.CompletedTask;
    this.GetInternalData<StranglePower.Data>().amountsForPlayedCards.Add(cardPlay.Card, this.Amount);
    return Task.CompletedTask;
  }

  public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    int amount;
    if (!this.GetInternalData<StranglePower.Data>().amountsForPlayedCards.Remove(cardPlay.Card, ref amount))
      return;
    this.Flash();
    IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(choiceContext, this.Owner, (Decimal) amount, ValueProp.Unblockable | ValueProp.Unpowered, (CardModel) null, (CardPlay) null);
  }

  public override async Task AfterSideTurnEnd(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IEnumerable<Creature> participants)
  {
    if (!participants.Contains<Creature>(this.Owner))
      return;
    await PowerCmd.Remove((PowerModel) this);
  }

  private class Data
  {
    public readonly Dictionary<CardModel, int> amountsForPlayedCards = new Dictionary<CardModel, int>();
  }
}
