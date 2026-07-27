// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Cards.CardEnergyCost
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.Cards;

public sealed class CardEnergyCost
{
  private readonly CardModel _card;
  private int _base;
  private int _capturedXValue;
  private List<LocalCostModifier> _localModifiers = new List<LocalCostModifier>();

  public int Canonical { get; }

  public bool CostsX { get; }

  public bool WasJustUpgraded { get; private set; }

  public bool HasLocalModifiers => this._localModifiers.Count > 0;

  public CardEnergyCost(CardModel card, int canonicalCost, bool costsX)
  {
    this._card = card;
    this.CostsX = costsX;
    this.Canonical = this.CostsX ? 0 : canonicalCost;
    this._base = this.Canonical;
  }

  public int GetWithModifiers(CostModifiers modifiers)
  {
    int withModifiers = this._base;
    if (this._card.IsCanonical || this._base < 0 || this.CostsX)
      return withModifiers;
    if (modifiers.HasFlag((Enum) CostModifiers.Local))
    {
      foreach (LocalCostModifier localModifier in this._localModifiers)
        withModifiers = localModifier.Modify(withModifiers);
    }
    if (modifiers.HasFlag((Enum) CostModifiers.Global) && this._card.CombatState != null)
      withModifiers = (int) Hook.ModifyEnergyCostInCombat(this._card.CombatState, this._card, (Decimal) withModifiers);
    return Math.Max(0, withModifiers);
  }

  public int CapturedXValue
  {
    get
    {
      if (!this.CostsX)
        throw new InvalidOperationException("Only X-cost cards have a captured value.");
      return this._capturedXValue;
    }
    set
    {
      this._card.AssertMutable();
      if (!this.CostsX)
        throw new InvalidOperationException("Only X-cost cards have a captured value.");
      this._capturedXValue = value;
    }
  }

  public int GetAmountToSpend()
  {
    if (!this.CostsX)
      return Math.Max(0, this.GetWithModifiers(CostModifiers.All));
    PlayerCombatState playerCombatState = this._card.Owner.PlayerCombatState;
    return playerCombatState == null ? 0 : playerCombatState.Energy;
  }

  public int GetResolved()
  {
    return this.CostsX ? this.CapturedXValue : Math.Max(0, this.GetWithModifiers(CostModifiers.All));
  }

  public void SetUntilPlayed(int cost, bool reduceOnly = false)
  {
    if (cost == 0 && this.Canonical < 0)
      return;
    this._localModifiers.Add(new LocalCostModifier(cost, LocalCostType.Absolute, LocalCostModifierExpiration.WhenPlayed, reduceOnly));
  }

  public void SetThisTurnOrUntilPlayed(int cost, bool reduceOnly = false)
  {
    if (cost == 0 && this.Canonical < 0)
      return;
    this._localModifiers.Add(new LocalCostModifier(cost, LocalCostType.Absolute, LocalCostModifierExpiration.EndOfTurn | LocalCostModifierExpiration.WhenPlayed, reduceOnly));
  }

  public void SetThisTurn(int cost, bool reduceOnly = false)
  {
    if (cost == 0 && this.Canonical < 0)
      return;
    this._localModifiers.Add(new LocalCostModifier(cost, LocalCostType.Absolute, LocalCostModifierExpiration.EndOfTurn, reduceOnly));
  }

  public void SetThisCombat(int cost, bool reduceOnly = false)
  {
    if (cost == 0 && this.Canonical < 0)
      return;
    this._localModifiers.Add(new LocalCostModifier(cost, LocalCostType.Absolute, LocalCostModifierExpiration.EndOfCombat, reduceOnly));
  }

  public void AddUntilPlayed(int amount, bool reduceOnly = false)
  {
    if (amount == 0)
      return;
    this._localModifiers.Add(new LocalCostModifier(amount, LocalCostType.Relative, LocalCostModifierExpiration.WhenPlayed, reduceOnly));
  }

  public void AddThisTurnOrUntilPlayed(int amount, bool reduceOnly = false)
  {
    if (amount == 0)
      return;
    this._localModifiers.Add(new LocalCostModifier(amount, LocalCostType.Relative, LocalCostModifierExpiration.EndOfTurn | LocalCostModifierExpiration.WhenPlayed, reduceOnly));
  }

  public void AddThisTurn(int amount, bool reduceOnly = false)
  {
    if (amount == 0)
      return;
    this._localModifiers.Add(new LocalCostModifier(amount, LocalCostType.Relative, LocalCostModifierExpiration.EndOfTurn, reduceOnly));
  }

  public void AddThisCombat(int amount, bool reduceOnly = false)
  {
    if (amount == 0)
      return;
    this._localModifiers.Add(new LocalCostModifier(amount, LocalCostType.Relative, LocalCostModifierExpiration.EndOfCombat, reduceOnly));
  }

  public bool EndOfTurnCleanup()
  {
    this._card.AssertMutable();
    return this._localModifiers.RemoveAll((Predicate<LocalCostModifier>) (m => m.Expiration.HasFlag((Enum) LocalCostModifierExpiration.EndOfTurn))) > 0;
  }

  public bool AfterCardPlayedCleanup()
  {
    this._card.AssertMutable();
    return this._localModifiers.RemoveAll((Predicate<LocalCostModifier>) (m => m.Expiration.HasFlag((Enum) LocalCostModifierExpiration.WhenPlayed))) > 0;
  }

  public void UpgradeBy(int addend)
  {
    this._card.AssertMutable();
    if (this.CostsX || addend == 0)
      return;
    int num = this._base;
    int newBaseCost = Math.Max(this._base + addend, 0);
    this.WasJustUpgraded = true;
    if (newBaseCost < num)
    {
      foreach (LocalCostModifier localModifier in this._localModifiers)
      {
        if (localModifier.Type == LocalCostType.Absolute && localModifier.Amount > newBaseCost)
          localModifier.Amount = newBaseCost;
      }
    }
    this.SetCustomBaseCost(newBaseCost);
  }

  public void FinalizeUpgrade()
  {
    this._card.AssertMutable();
    this.WasJustUpgraded = false;
  }

  public void ResetForDowngrade()
  {
    this._card.AssertMutable();
    this._base = this.Canonical;
    this._card.InvokeEnergyCostChanged();
  }

  public void SetCustomBaseCost(int newBaseCost)
  {
    this._card.AssertMutable();
    this._base = newBaseCost;
    this._card.InvokeEnergyCostChanged();
  }

  public CardEnergyCost Clone(CardModel newCard)
  {
    List<LocalCostModifier> list = this._localModifiers.Select<LocalCostModifier, LocalCostModifier>((Func<LocalCostModifier, LocalCostModifier>) (m => m.Clone())).ToList<LocalCostModifier>();
    return new CardEnergyCost(newCard, newCard.EnergyCost.Canonical, newCard.EnergyCost.CostsX)
    {
      _base = this._base,
      _capturedXValue = this._capturedXValue,
      WasJustUpgraded = this.WasJustUpgraded,
      _localModifiers = list
    };
  }
}
