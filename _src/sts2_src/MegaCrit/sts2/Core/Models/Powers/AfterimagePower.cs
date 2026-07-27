// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.AfterimagePower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class AfterimagePower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block));
    }
  }

  protected override object InitInternalData() => (object) new AfterimagePower.Data();

  public override Task BeforeCardPlayed(CardPlay cardPlay)
  {
    if (cardPlay.Card.Owner.Creature != this.Owner)
      return Task.CompletedTask;
    this.GetInternalData<AfterimagePower.Data>().amountsForPlayedCards.Add(cardPlay.Card, this.Amount);
    return Task.CompletedTask;
  }

  public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    int amount;
    if (cardPlay.Card.Owner.Creature != this.Owner || !this.GetInternalData<AfterimagePower.Data>().amountsForPlayedCards.Remove(cardPlay.Card, ref amount) || amount <= 0)
      return;
    Decimal num = await CreatureCmd.GainBlock(this.Owner, (Decimal) amount, ValueProp.Unpowered, (CardPlay) null, true);
  }

  private class Data
  {
    public readonly Dictionary<CardModel, int> amountsForPlayedCards = new Dictionary<CardModel, int>();
  }
}
