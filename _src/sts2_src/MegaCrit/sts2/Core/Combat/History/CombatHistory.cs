// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Combat.History.CombatHistory
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Combat.History;

public class CombatHistory
{
  private readonly List<CombatHistoryEntry> _entries = new List<CombatHistoryEntry>();

  public event Action? Changed;

  public IEnumerable<CombatHistoryEntry> Entries => (IEnumerable<CombatHistoryEntry>) this._entries;

  public IEnumerable<CardPlayStartedEntry> CardPlaysStarted
  {
    get => this.Entries.OfType<CardPlayStartedEntry>();
  }

  public IEnumerable<CardPlayFinishedEntry> CardPlaysFinished
  {
    get => this.Entries.OfType<CardPlayFinishedEntry>();
  }

  public void Clear()
  {
    this._entries.Clear();
    Action changed = this.Changed;
    if (changed == null)
      return;
    changed();
  }

  public void CardPlayStarted(ICombatState combatState, CardPlay cardPlay)
  {
    this.Add(combatState, (CombatHistoryEntry) new CardPlayStartedEntry(cardPlay, combatState.RoundNumber, combatState.CurrentSide, this, (IEnumerable<Player>) combatState.Players));
  }

  public void CardPlayFinished(ICombatState combatState, CardPlay cardPlay)
  {
    this.Add(combatState, (CombatHistoryEntry) new CardPlayFinishedEntry(cardPlay, combatState.RoundNumber, combatState.CurrentSide, this, (IEnumerable<Player>) combatState.Players));
  }

  public void CardAfflicted(ICombatState combatState, CardModel card, AfflictionModel affliction)
  {
    this.Add(combatState, (CombatHistoryEntry) new CardAfflictedEntry(card, affliction, combatState.RoundNumber, combatState.CurrentSide, this, (IEnumerable<Player>) combatState.Players));
  }

  public void CardDiscarded(ICombatState combatState, CardModel card)
  {
    this.Add(combatState, (CombatHistoryEntry) new CardDiscardedEntry(card, combatState.RoundNumber, combatState.CurrentSide, this, (IEnumerable<Player>) combatState.Players));
  }

  public void CardDrawn(ICombatState combatState, CardModel card, bool fromHandDraw)
  {
    this.Add(combatState, (CombatHistoryEntry) new CardDrawnEntry(card, combatState.RoundNumber, combatState.CurrentSide, fromHandDraw, this, (IEnumerable<Player>) combatState.Players));
  }

  public void CardExhausted(ICombatState combatState, CardModel card)
  {
    this.Add(combatState, (CombatHistoryEntry) new CardExhaustedEntry(card, combatState.RoundNumber, combatState.CurrentSide, this, (IEnumerable<Player>) combatState.Players));
  }

  public void CardGenerated(ICombatState combatState, CardModel card, Player? creator)
  {
    this.Add(combatState, (CombatHistoryEntry) new CardGeneratedEntry(card, creator, combatState.RoundNumber, combatState.CurrentSide, this, (IEnumerable<Player>) combatState.Players));
  }

  public void CreatureAttacked(
    ICombatState combatState,
    Creature attacker,
    IReadOnlyList<DamageResult> damageResults)
  {
    this.Add(combatState, (CombatHistoryEntry) new CreatureAttackedEntry(attacker, damageResults, combatState.RoundNumber, combatState.CurrentSide, this, (IEnumerable<Player>) combatState.Players));
  }

  public void DamageReceived(
    ICombatState combatState,
    Creature receiver,
    Creature? dealer,
    DamageResult result,
    CardModel? cardSource)
  {
    this.Add(combatState, (CombatHistoryEntry) new DamageReceivedEntry(result, receiver, dealer, cardSource, combatState.RoundNumber, combatState.CurrentSide, this, (IEnumerable<Player>) combatState.Players));
  }

  public void BlockGained(
    ICombatState combatState,
    Creature receiver,
    int amount,
    ValueProp props,
    CardPlay? cardPlay)
  {
    this.Add(combatState, (CombatHistoryEntry) new BlockGainedEntry(amount, props, cardPlay, receiver, combatState.RoundNumber, combatState.CurrentSide, this, (IEnumerable<Player>) combatState.Players));
  }

  public void EnergySpent(ICombatState combatState, int amount, Player player)
  {
    this.Add(combatState, (CombatHistoryEntry) new EnergySpentEntry(amount, player, combatState.RoundNumber, combatState.CurrentSide, this, (IEnumerable<Player>) combatState.Players));
  }

  public void MonsterPerformedMove(
    ICombatState combatState,
    MonsterModel monster,
    MoveState move,
    IEnumerable<Creature>? targets)
  {
    this.Add(combatState, (CombatHistoryEntry) new MonsterPerformedMoveEntry(monster, move, targets, combatState.RoundNumber, combatState.CurrentSide, this, (IEnumerable<Player>) combatState.Players));
  }

  public void OrbChanneled(ICombatState combatState, OrbModel orb)
  {
    this.Add(combatState, (CombatHistoryEntry) new OrbChanneledEntry(orb, combatState.RoundNumber, combatState.CurrentSide, this, (IEnumerable<Player>) combatState.Players));
  }

  public void PotionUsed(ICombatState combatState, PotionModel potion, Creature? target)
  {
    this.Add(combatState, (CombatHistoryEntry) new PotionUsedEntry(potion, target, combatState.RoundNumber, combatState.CurrentSide, this, (IEnumerable<Player>) combatState.Players));
  }

  public void PowerReceived(
    ICombatState combatState,
    PowerModel power,
    Decimal amount,
    Creature? applier)
  {
    this.Add(combatState, (CombatHistoryEntry) new PowerReceivedEntry(power, amount, applier, combatState.RoundNumber, combatState.CurrentSide, this, (IEnumerable<Player>) combatState.Players));
  }

  public void StarsModified(ICombatState combatState, int amount, Player player)
  {
    this.Add(combatState, (CombatHistoryEntry) new StarsModifiedEntry(amount, player, combatState.RoundNumber, combatState.CurrentSide, this, (IEnumerable<Player>) combatState.Players));
  }

  public void Summoned(ICombatState combatState, int amount, Player player)
  {
    this.Add(combatState, (CombatHistoryEntry) new SummonedEntry(amount, player, combatState.RoundNumber, combatState.CurrentSide, this, (IEnumerable<Player>) combatState.Players));
  }

  private void Add(ICombatState combatState, CombatHistoryEntry entry)
  {
    if (!combatState.IsLiveCombat())
      return;
    this._entries.Add(entry);
    Action changed = this.Changed;
    if (changed == null)
      return;
    changed();
  }
}
