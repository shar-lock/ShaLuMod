// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.Vambrace
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class Vambrace : RelicModel
{
  private CardModel? _triggeringCard;
  private bool _blockGainedThisCombat;

  public override RelicRarity Rarity => RelicRarity.Uncommon;

  private CardModel? TriggeringCard
  {
    get => this._triggeringCard;
    set
    {
      this.AssertMutable();
      this._triggeringCard = value;
    }
  }

  private bool BlockGainedThisCombat
  {
    get => this._blockGainedThisCombat;
    set
    {
      this.AssertMutable();
      this._blockGainedThisCombat = value;
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.Static(StaticHoverTip.Block));
    }
  }

  public override Task BeforeCombatStart()
  {
    this.TriggeringCard = (CardModel) null;
    this.BlockGainedThisCombat = false;
    this.Status = RelicStatus.Active;
    return Task.CompletedTask;
  }

  public override Decimal ModifyBlockMultiplicative(
    Creature target,
    Decimal block,
    ValueProp props,
    CardModel? cardSource,
    CardPlay? cardPlay)
  {
    return !props.IsCardOrMonsterMove() || cardSource == null || this.TriggeringCard != null && this.TriggeringCard != cardSource || cardSource.Owner != this.Owner || this.BlockGainedThisCombat ? 1M : 2M;
  }

  public override Task AfterModifyingBlockAmount(
    Decimal modifiedAmount,
    CardModel? cardSource,
    CardPlay? cardPlay)
  {
    if (modifiedAmount <= 0M || cardSource == null)
      return Task.CompletedTask;
    this.Flash();
    this.Status = RelicStatus.Normal;
    this.TriggeringCard = cardSource;
    return Task.CompletedTask;
  }

  public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    if (cardPlay.Card.Owner != this.Owner || cardPlay.Card != this.TriggeringCard || this.BlockGainedThisCombat)
      return Task.CompletedTask;
    this.BlockGainedThisCombat = true;
    return Task.CompletedTask;
  }

  public override Task AfterCombatEnd(CombatRoom room)
  {
    this.TriggeringCard = (CardModel) null;
    this.BlockGainedThisCombat = false;
    this.Status = RelicStatus.Normal;
    return Task.CompletedTask;
  }
}
