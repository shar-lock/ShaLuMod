// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.CalamityPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class CalamityPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  protected override object InitInternalData() => (object) new CalamityPower.Data();

  public override Task BeforeCardPlayed(CardPlay cardPlay)
  {
    if (cardPlay.Card.Owner.Creature != this.Owner || cardPlay.Card.Type != CardType.Attack)
      return Task.CompletedTask;
    this.GetInternalData<CalamityPower.Data>().amountsForPlayedCards.Add(cardPlay.Card, this.Amount);
    return Task.CompletedTask;
  }

  public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    int num;
    if (!this.GetInternalData<CalamityPower.Data>().amountsForPlayedCards.Remove(cardPlay.Card, ref num))
      return;
    foreach (CardModel card in CardFactory.GetForCombat(this.Owner.Player, this.Owner.Player.Character.CardPool.GetUnlockedCards(this.Owner.Player.UnlockState, this.Owner.Player.RunState.CardMultiplayerConstraint).Where<CardModel>((Func<CardModel, bool>) (c => c.Type == CardType.Attack)), this.Amount, this.Owner.Player.RunState.Rng.CombatCardGeneration).ToList<CardModel>())
    {
      CardPileAddResult combat = await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, this.Owner.Player);
    }
  }

  private class Data
  {
    public readonly Dictionary<CardModel, int> amountsForPlayedCards = new Dictionary<CardModel, int>();
  }
}
