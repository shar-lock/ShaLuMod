// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.SubroutinePower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class SubroutinePower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  protected override object InitInternalData() => (object) new SubroutinePower.Data();

  public override Task BeforeCardPlayed(CardPlay cardPlay)
  {
    if (cardPlay.Card.Owner != this.Owner.Player || cardPlay.Card.Type != CardType.Power)
      return Task.CompletedTask;
    this.GetInternalData<SubroutinePower.Data>().amountsForPlayedCards.Add(cardPlay.Card, this.Amount);
    return Task.CompletedTask;
  }

  public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    int energy;
    if (cardPlay.Card.Owner != this.Owner.Player || !this.GetInternalData<SubroutinePower.Data>().amountsForPlayedCards.Remove(cardPlay.Card, ref energy) || energy <= 0)
      return;
    this.Flash();
    for (int i = 0; i < energy; ++i)
      await PlayerCmd.GainEnergy(1M, this.Owner.Player);
  }

  private class Data
  {
    public readonly Dictionary<CardModel, int> amountsForPlayedCards = new Dictionary<CardModel, int>();
  }
}
