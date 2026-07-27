// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.UnsettlingLamp
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class UnsettlingLamp : RelicModel
{
  private CardModel? _triggeringCard;
  private List<PowerModel>? _doubledPowers;
  private bool _isFinishedTriggering;

  public override RelicRarity Rarity => RelicRarity.Rare;

  private CardModel? TriggeringCard
  {
    get => this._triggeringCard;
    set
    {
      this.AssertMutable();
      this._triggeringCard = value;
    }
  }

  private List<PowerModel> DoubledPowers
  {
    get
    {
      this.AssertMutable();
      if (this._doubledPowers == null)
        this._doubledPowers = new List<PowerModel>();
      return this._doubledPowers;
    }
  }

  private bool IsFinishedTriggering
  {
    get => this._isFinishedTriggering;
    set
    {
      this.AssertMutable();
      this._isFinishedTriggering = value;
    }
  }

  public override Task BeforeCombatStart()
  {
    this.TriggeringCard = (CardModel) null;
    this.DoubledPowers.Clear();
    this.IsFinishedTriggering = false;
    this.Status = RelicStatus.Active;
    return Task.CompletedTask;
  }

  public override Task BeforePowerAmountChanged(
    PowerModel power,
    Decimal amount,
    Creature target,
    Creature? applier,
    CardModel? cardSource)
  {
    if (this.TriggeringCard != null || this.IsFinishedTriggering || cardSource == null || applier != this.Owner.Creature || target.Side == this.Owner.Creature.Side || !power.IsVisible || power.GetTypeForAmount(amount) != PowerType.Debuff || target.HasPower<ArtifactPower>())
      return Task.CompletedTask;
    this.TriggeringCard = cardSource;
    this.DoubledPowers.Add(power);
    return Task.CompletedTask;
  }

  public override Decimal ModifyPowerAmountGivenMultiplicative(
    PowerModel power,
    Creature giver,
    Decimal amount,
    Creature? target,
    CardModel? cardSource)
  {
    return this.TriggeringCard == null || cardSource != this.TriggeringCard || this.IsFinishedTriggering || this.HasDoubledTemporaryPowerSource(power) || power.GetTypeForAmount(amount) != PowerType.Debuff ? 1M : 2M;
  }

  public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    if (cardPlay.Card != this.TriggeringCard || this.IsFinishedTriggering)
      return Task.CompletedTask;
    this.Flash();
    this.IsFinishedTriggering = true;
    this.Status = RelicStatus.Normal;
    return Task.CompletedTask;
  }

  public override Task AfterCombatEnd(CombatRoom room)
  {
    this.TriggeringCard = (CardModel) null;
    this.DoubledPowers.Clear();
    this.IsFinishedTriggering = false;
    this.Status = RelicStatus.Normal;
    return Task.CompletedTask;
  }

  private bool HasDoubledTemporaryPowerSource(PowerModel power)
  {
    return this.DoubledPowers.OfType<ITemporaryPower>().Any<ITemporaryPower>((Func<ITemporaryPower, bool>) (p => p.InternallyAppliedPower.GetType() == power.GetType()));
  }
}
