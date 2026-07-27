// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Players.PlayerCombatState
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Orbs;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.Players;

public class PlayerCombatState
{
  private readonly Player _player;
  private readonly List<Creature> _pets = new List<Creature>();
  private CardPile[]? _piles;
  private int _energy;
  private int _stars;
  private PlayerTurnPhase _phase;

  public IReadOnlyList<Creature> Pets => (IReadOnlyList<Creature>) this._pets;

  public int TurnNumber { get; private set; } = 1;

  public PlayerTurnPhase Phase
  {
    get => this._phase;
    set
    {
      if (value == this._phase)
        return;
      this._phase = value;
      Action turnPhaseChanged = this.PlayerTurnPhaseChanged;
      if (turnPhaseChanged == null)
        return;
      turnPhaseChanged();
    }
  }

  public event Action? PlayerTurnPhaseChanged;

  public event Action<int, int>? EnergyChanged;

  public event Action<int, int>? StarsChanged;

  public CardPile Hand { get; } = new CardPile(PileType.Hand);

  public CardPile DrawPile { get; } = new CardPile(PileType.Draw);

  public CardPile DiscardPile { get; } = new CardPile(PileType.Discard);

  public CardPile ExhaustPile { get; } = new CardPile(PileType.Exhaust);

  public CardPile PlayPile { get; } = new CardPile(PileType.Play);

  public IReadOnlyList<CardPile> AllPiles
  {
    get
    {
      if (this._piles == null)
        this._piles = new CardPile[5]
        {
          this.Hand,
          this.DrawPile,
          this.DiscardPile,
          this.ExhaustPile,
          this.PlayPile
        };
      return (IReadOnlyList<CardPile>) this._piles;
    }
  }

  public IEnumerable<CardModel> AllCards
  {
    get
    {
      return this.AllPiles.SelectMany<CardPile, CardModel>((Func<CardPile, IEnumerable<CardModel>>) (p => (IEnumerable<CardModel>) p.Cards));
    }
  }

  public int Energy
  {
    get => this._energy;
    set
    {
      if (this._energy == value)
        return;
      int energy = this._energy;
      this._energy = value;
      Action<int, int> energyChanged = this.EnergyChanged;
      if (energyChanged == null)
        return;
      energyChanged(energy, this._energy);
    }
  }

  public int MaxEnergy
  {
    get
    {
      return (int) Hook.ModifyMaxEnergy(this._player.Creature.CombatState, this._player, (Decimal) this._player.MaxEnergy);
    }
  }

  public int Stars
  {
    get => this._stars;
    set
    {
      if (this._stars == value)
        return;
      int stars = this._stars;
      this._stars = value;
      CombatManager.Instance.History.StarsModified(this._player.Creature.CombatState, this._stars - stars, this._player);
      Action<int, int> starsChanged = this.StarsChanged;
      if (starsChanged == null)
        return;
      starsChanged(stars, this._stars);
    }
  }

  public OrbQueue OrbQueue { get; }

  public PlayerCombatState(Player player)
  {
    this._player = player;
    CombatManager.Instance.StateTracker.Subscribe(this);
    foreach (CardPile allPile in (IEnumerable<CardPile>) this.AllPiles)
      CombatManager.Instance.StateTracker.Subscribe(allPile);
    this.OrbQueue = new OrbQueue(player);
    this.OrbQueue.Clear();
    this.OrbQueue.AddCapacity(player.BaseOrbSlotCount);
  }

  public void AfterCombatEnd()
  {
    CombatManager.Instance.StateTracker.Unsubscribe(this);
    foreach (CardPile allPile in (IEnumerable<CardPile>) this.AllPiles)
    {
      allPile.Clear();
      CombatManager.Instance.StateTracker.Unsubscribe(allPile);
    }
    this._pets.Clear();
  }

  public void IncrementTurnNumber() => ++this.TurnNumber;

  public void ResetEnergy() => this.Energy = this.MaxEnergy;

  public void AddMaxEnergyToCurrent() => this.Energy += this.MaxEnergy;

  public void LoseEnergy(Decimal amount)
  {
    this.Energy = !(amount < 0M) ? (int) Math.Clamp((Decimal) this.Energy - amount, 0M, 999999999M) : throw new ArgumentException("Must not be negative.", nameof (amount));
  }

  public void GainEnergy(Decimal amount)
  {
    this.Energy = !(amount < 0M) ? (int) Math.Clamp((Decimal) this.Energy + amount, 0M, 999999999M) : throw new ArgumentException("Must not be negative.", nameof (amount));
  }

  public bool HasEnoughResourcesFor(CardModel card, out UnplayableReason reason)
  {
    int num1 = Math.Max(0, card.EnergyCost.GetWithModifiers(CostModifiers.All));
    int num2 = Math.Max(0, card.GetStarCostWithModifiers());
    if (num1 > this.Energy && card.CombatState != null && Hook.ShouldPayExcessEnergyCostWithStars(card.CombatState, this._player))
    {
      num2 += (num1 - this.Energy) * 2;
      num1 = this.Energy;
    }
    reason = UnplayableReason.None;
    if (num1 > this.Energy)
      reason |= UnplayableReason.EnergyCostTooHigh;
    if (num2 > this.Stars)
      reason |= UnplayableReason.StarCostTooHigh;
    return reason == UnplayableReason.None;
  }

  public void LoseStars(Decimal amount)
  {
    this.Stars = !(amount < 0M) ? (int) Math.Max((Decimal) this.Stars - amount, 0M) : throw new ArgumentException("Must not be negative.", nameof (amount));
  }

  public void GainStars(Decimal amount)
  {
    this.Stars = !(amount < 0M) ? (int) Math.Max((Decimal) this.Stars + amount, 0M) : throw new ArgumentException("Must not be negative.", nameof (amount));
  }

  public void AddPetInternal(Creature pet)
  {
    pet.Monster.AssertMutable();
    if (this._pets.Contains(pet))
      return;
    if (pet.PetOwner != this._player)
      pet.PetOwner = this._player;
    pet.Died += new Action<Creature>(this.OnPetDied);
    this._pets.Add(pet);
  }

  public Creature? GetPet<T>() where T : MonsterModel
  {
    return this.Pets.FirstOrDefault<Creature>((Func<Creature, bool>) (p => p.Monster is T));
  }

  public void RecalculateCardValues()
  {
    foreach (CardModel allCard in this.AllCards)
      allCard.Enchantment?.RecalculateValues();
  }

  public void EndOfTurnCleanup()
  {
    foreach (CardModel allCard in this.AllCards)
      allCard.EndOfTurnCleanup();
  }

  public bool HasCardsToPlay()
  {
    return this.Hand.Cards.Any<CardModel>((Func<CardModel, bool>) (c => c.CanPlay()));
  }

  private void OnPetDied(Creature pet)
  {
    if (!this._pets.Contains(pet))
      throw new InvalidOperationException("Player does not have pet " + pet.LogName);
    if (!Hook.ShouldCreatureBeRemovedFromCombatAfterDeath(pet.CombatState, pet))
      return;
    pet.Died -= new Action<Creature>(this.OnPetDied);
    this._pets.Remove(pet);
  }
}
