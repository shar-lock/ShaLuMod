// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.JugglingPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class JugglingPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  protected override object InitInternalData() => (object) new JugglingPower.Data();

  public override Task AfterApplied(Creature? applier, CardModel? cardSource)
  {
    this.GetInternalData<JugglingPower.Data>().attacksPlayedThisTurn = CombatManager.Instance.History.CardPlaysStarted.Count<CardPlayStartedEntry>((Func<CardPlayStartedEntry, bool>) (e => e.CardPlay.Card.Type == CardType.Attack && e.CardPlay.Player == this.Owner.Player && e.HappenedThisTurn(this.CombatState)));
    return Task.CompletedTask;
  }

  public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    if (cardPlay.Card.Owner != this.Owner.Player || cardPlay.Card.Type != CardType.Attack)
      return;
    ++this.GetInternalData<JugglingPower.Data>().attacksPlayedThisTurn;
    if (this.GetInternalData<JugglingPower.Data>().attacksPlayedThisTurn != 3)
      return;
    this.Flash();
    for (int i = 0; i < this.Amount; ++i)
    {
      CardPileAddResult combat = await CardPileCmd.AddGeneratedCardToCombat(cardPlay.Card.CreateClone(), PileType.Hand, this.Owner.Player);
    }
  }

  public override Task AfterSideTurnEnd(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IEnumerable<Creature> participants)
  {
    if (!participants.Contains<Creature>(this.Owner))
      return Task.CompletedTask;
    this.GetInternalData<JugglingPower.Data>().attacksPlayedThisTurn = 0;
    return Task.CompletedTask;
  }

  private class Data
  {
    public int attacksPlayedThisTurn;
  }
}
