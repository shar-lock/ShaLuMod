// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.RupturePower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class RupturePower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>());
    }
  }

  protected override object InitInternalData() => (object) new RupturePower.Data();

  public override Task BeforeCardPlayed(CardPlay cardPlay)
  {
    if (cardPlay.Card.Owner.Creature != this.Owner || this.CombatState.CurrentSide != this.Owner.Side)
      return Task.CompletedTask;
    this.GetInternalData<RupturePower.Data>().playedCards.Add(cardPlay.Card, 0);
    return Task.CompletedTask;
  }

  public override async Task AfterDamageReceived(
    PlayerChoiceContext choiceContext,
    Creature target,
    DamageResult result,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource)
  {
    if (target != this.Owner || result.UnblockedDamage <= 0 || this.CombatState.CurrentSide != this.Owner.Side)
      return;
    if (cardSource == null || !this.GetInternalData<RupturePower.Data>().playedCards.ContainsKey(cardSource))
    {
      StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>(choiceContext, this.Owner, (Decimal) this.Amount, this.Owner, (CardModel) null);
    }
    else
      this.GetInternalData<RupturePower.Data>().playedCards[cardSource] += this.Amount;
  }

  public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    int amount;
    if (cardPlay.Card.Owner.Creature != this.Owner || !this.GetInternalData<RupturePower.Data>().playedCards.Remove(cardPlay.Card, ref amount))
      return;
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>(choiceContext, this.Owner, (Decimal) amount, this.Owner, (CardModel) null);
  }

  private class Data
  {
    public readonly Dictionary<CardModel, int> playedCards = new Dictionary<CardModel, int>();
  }
}
