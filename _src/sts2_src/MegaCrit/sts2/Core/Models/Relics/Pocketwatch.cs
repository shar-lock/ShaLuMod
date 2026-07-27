// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.Pocketwatch
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
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

public sealed class Pocketwatch : RelicModel
{
  private const string _cardThresholdKey = "CardThreshold";
  private int _cardsPlayedThisTurn;
  private int _cardsPlayedLastTurn;

  public override RelicRarity Rarity => RelicRarity.Rare;

  public override bool ShowCounter => CombatManager.Instance.IsInProgress;

  public override int DisplayAmount => this._cardsPlayedThisTurn;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        new DynamicVar("CardThreshold", 3M),
        (DynamicVar) new CardsVar(3)
      });
    }
  }

  public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    if (cardPlay.Card.Owner != this.Owner || !CombatManager.Instance.IsInProgress)
      return Task.CompletedTask;
    ++this._cardsPlayedThisTurn;
    this.RefreshCounter();
    return Task.CompletedTask;
  }

  public override Decimal ModifyHandDraw(Player player, Decimal count)
  {
    return player != this.Owner || this.Owner.PlayerCombatState.TurnNumber == 1 || (Decimal) this._cardsPlayedLastTurn > this.DynamicVars["CardThreshold"].BaseValue ? count : count + this.DynamicVars.Cards.BaseValue;
  }

  public override Task AfterModifyingHandDraw()
  {
    this.Flash();
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
    this._cardsPlayedLastTurn = this._cardsPlayedThisTurn;
    this._cardsPlayedThisTurn = 0;
    return Task.CompletedTask;
  }

  public override Task AfterSideTurnStart(
    CombatSide side,
    IReadOnlyList<Creature> participants,
    ICombatState combatState)
  {
    if (!participants.Contains<Creature>(this.Owner.Creature))
      return Task.CompletedTask;
    this.RefreshCounter();
    return Task.CompletedTask;
  }

  private void RefreshCounter()
  {
    this.Status = (Decimal) this._cardsPlayedThisTurn <= this.DynamicVars["CardThreshold"].BaseValue ? RelicStatus.Active : RelicStatus.Normal;
    this.InvokeDisplayAmountChanged();
  }

  public override Task AfterCombatEnd(CombatRoom _)
  {
    this._cardsPlayedThisTurn = 0;
    this._cardsPlayedLastTurn = 0;
    this.Status = RelicStatus.Normal;
    this.InvokeDisplayAmountChanged();
    return Task.CompletedTask;
  }
}
