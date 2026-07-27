// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.BrilliantScarf
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class BrilliantScarf : RelicModel
{
  private int _cardsPlayedThisTurn;

  public override RelicRarity Rarity => RelicRarity.Ancient;

  public override bool ShowCounter
  {
    get
    {
      return CombatManager.Instance.IsInProgress && this.CardsPlayedThisTurn < this.DynamicVars.Cards.IntValue;
    }
  }

  public override int DisplayAmount => this.CardsPlayedThisTurn;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new CardsVar(5));
    }
  }

  private int CardsPlayedThisTurn
  {
    get => this._cardsPlayedThisTurn;
    set
    {
      this.AssertMutable();
      this._cardsPlayedThisTurn = value;
      this.UpdateDisplay();
    }
  }

  private void UpdateDisplay()
  {
    this.Status = this.CardsPlayedThisTurn == this.DynamicVars.Cards.IntValue - 1 ? RelicStatus.Active : RelicStatus.Normal;
    this.InvokeDisplayAmountChanged();
  }

  public override bool TryModifyEnergyCostInCombatLate(
    CardModel card,
    Decimal originalCost,
    out Decimal modifiedCost)
  {
    modifiedCost = originalCost;
    if (!this.ShouldModifyCost(card))
      return false;
    modifiedCost = 0M;
    return true;
  }

  public override bool TryModifyStarCost(
    CardModel card,
    Decimal originalCost,
    out Decimal modifiedCost)
  {
    modifiedCost = originalCost;
    if (!this.ShouldModifyCost(card))
      return false;
    modifiedCost = 0M;
    return true;
  }

  public override Task BeforeSideTurnStart(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IReadOnlyList<Creature> participants,
    ICombatState combatState)
  {
    if (!participants.Contains<Creature>(this.Owner.Creature))
      return Task.CompletedTask;
    this.CardsPlayedThisTurn = 0;
    return Task.CompletedTask;
  }

  public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    if (!CombatManager.Instance.IsInProgress || cardPlay.IsAutoPlay || cardPlay.Card.Owner != this.Owner)
      return Task.CompletedTask;
    ++this.CardsPlayedThisTurn;
    return Task.CompletedTask;
  }

  public override Task AfterCombatEnd(CombatRoom _)
  {
    this.CardsPlayedThisTurn = 0;
    return Task.CompletedTask;
  }

  private bool ShouldModifyCost(CardModel card)
  {
    if (!CombatManager.Instance.IsInProgress || card.Owner.Creature != this.Owner.Creature || (Decimal) this.CardsPlayedThisTurn != this.DynamicVars.Cards.BaseValue - 1M)
      return false;
    PileType? type = card.Pile?.Type;
    bool flag;
    if (type.HasValue)
    {
      switch (type.GetValueOrDefault())
      {
        case PileType.Hand:
        case PileType.Play:
          flag = true;
          goto label_6;
      }
    }
    flag = false;
label_6:
    return flag;
  }
}
