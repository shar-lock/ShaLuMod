// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.VelvetChoker
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class VelvetChoker : RelicModel
{
  private int _cardsPlayedThisTurn;

  public override RelicRarity Rarity => RelicRarity.Ancient;

  public override bool ShowCounter => CombatManager.Instance.IsInProgress;

  public override int DisplayAmount => !this.IsCanonical ? this._cardsPlayedThisTurn : 0;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new CardsVar(6),
        (DynamicVar) new EnergyVar(1)
      });
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.ForEnergy((RelicModel) this));
    }
  }

  private bool ShouldPreventCardPlay
  {
    get => this._cardsPlayedThisTurn >= this.DynamicVars.Cards.IntValue;
  }

  public override Decimal ModifyMaxEnergy(Player player, Decimal amount)
  {
    return player != this.Owner ? amount : amount + this.DynamicVars.Energy.BaseValue;
  }

  public override bool ShouldPlay(CardModel card, AutoPlayType _)
  {
    return card.Owner != this.Owner || !this.ShouldPreventCardPlay;
  }

  public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    if (cardPlay.Card.Owner == this.Owner)
      ++this._cardsPlayedThisTurn;
    this.InvokeDisplayAmountChanged();
    return Task.CompletedTask;
  }

  public override Task AfterRoomEntered(AbstractRoom room)
  {
    if (!(room is CombatRoom))
      return Task.CompletedTask;
    this._cardsPlayedThisTurn = 0;
    this.InvokeDisplayAmountChanged();
    return Task.CompletedTask;
  }

  public override Task AfterCombatEnd(CombatRoom room)
  {
    this._cardsPlayedThisTurn = 0;
    this.InvokeDisplayAmountChanged();
    return Task.CompletedTask;
  }

  public override Task BeforeSideTurnStart(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IReadOnlyList<Creature> participants,
    ICombatState combatState)
  {
    if (!participants.Contains<Creature>(this.Owner.Creature))
      return Task.CompletedTask;
    this._cardsPlayedThisTurn = 0;
    this.InvokeDisplayAmountChanged();
    return Task.CompletedTask;
  }
}
