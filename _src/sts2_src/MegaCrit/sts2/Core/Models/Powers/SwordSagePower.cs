// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.SwordSagePower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Cards;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class SwordSagePower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      List<IHoverTip> items = new List<IHoverTip>();
      items.AddRange(HoverTipFactory.FromCardWithCardHoverTips<SovereignBlade>());
      items.Add(HoverTipFactory.Static(StaticHoverTip.ReplayStatic));
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlyList<IHoverTip>(items);
    }
  }

  public override Task AfterPowerAmountChanged(
    PlayerChoiceContext choiceContext,
    PowerModel power,
    Decimal amount,
    Creature? applier,
    CardModel? cardSource)
  {
    if (!(power is SwordSagePower) || power.Owner != this.Owner)
      return Task.CompletedTask;
    Player player = this.Owner.Player;
    object obj;
    if (player == null)
    {
      obj = (object) null;
    }
    else
    {
      PlayerCombatState playerCombatState = player.PlayerCombatState;
      obj = playerCombatState != null ? (object) playerCombatState.AllCards : (object) null;
    }
    if (obj == null)
      obj = (object) Array.Empty<CardModel>();
    foreach (CardModel card in (IEnumerable<CardModel>) obj)
      this.TryAddReplays(card, (int) amount);
    return Task.CompletedTask;
  }

  public override Task AfterCardEnteredCombat(CardModel card)
  {
    if (card.IsClone)
      return Task.CompletedTask;
    this.TryAddReplays(card, this.Amount);
    return Task.CompletedTask;
  }

  public override Task AfterRemoved(Creature oldOwner)
  {
    Player player = oldOwner.Player;
    object obj;
    if (player == null)
    {
      obj = (object) null;
    }
    else
    {
      PlayerCombatState playerCombatState = player.PlayerCombatState;
      obj = playerCombatState != null ? (object) playerCombatState.AllCards : (object) null;
    }
    if (obj == null)
      obj = (object) Array.Empty<CardModel>();
    foreach (CardModel cardModel in (IEnumerable<CardModel>) obj)
    {
      if (cardModel is SovereignBlade sovereignBlade)
        sovereignBlade.BaseReplayCount -= this.Amount;
    }
    return Task.CompletedTask;
  }

  private void TryAddReplays(CardModel card, int amount)
  {
    if (card.Owner != this.Owner.Player || !(card is SovereignBlade sovereignBlade))
      return;
    sovereignBlade.BaseReplayCount += amount;
  }
}
