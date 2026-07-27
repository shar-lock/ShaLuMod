// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.StormPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Orbs;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class StormPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  protected override object InitInternalData() => (object) new StormPower.Data();

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlyArray<IHoverTip>(new IHoverTip[2]
      {
        HoverTipFactory.Static(StaticHoverTip.Channeling),
        HoverTipFactory.FromOrb<LightningOrb>()
      });
    }
  }

  public override Task BeforeCardPlayed(CardPlay cardPlay)
  {
    if (cardPlay.Card.Owner != this.Owner.Player || cardPlay.Card.Type != CardType.Power)
      return Task.CompletedTask;
    this.GetInternalData<StormPower.Data>().amountsForPlayedCards.Add(cardPlay.Card, this.Amount);
    return Task.CompletedTask;
  }

  public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    int lightning;
    if (cardPlay.Card.Owner != this.Owner.Player || !this.GetInternalData<StormPower.Data>().amountsForPlayedCards.Remove(cardPlay.Card, ref lightning) || lightning <= 0)
      return;
    this.Flash();
    for (int i = 0; i < lightning; ++i)
      await OrbCmd.Channel<LightningOrb>(choiceContext, this.Owner.Player);
  }

  private class Data
  {
    public readonly Dictionary<CardModel, int> amountsForPlayedCards = new Dictionary<CardModel, int>();
  }
}
