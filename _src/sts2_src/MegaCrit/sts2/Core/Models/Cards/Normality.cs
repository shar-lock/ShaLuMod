// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Normality
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Normality : CardModel
{
  private const int _numOfCardsPerTurn = 3;
  private const string _calculatedCardsKey = "CalculatedCards";

  public Normality()
    : base(-1, CardType.Curse, CardRarity.Curse, TargetType.None)
  {
  }

  protected override bool ShouldGlowRedInternal => this.ShouldPreventCardPlay;

  private bool ShouldPreventCardPlay => this.CardsPlayedThisTurn >= 3;

  public override int MaxUpgradeLevel => 0;

  public override IEnumerable<CardKeyword> CanonicalKeywords
  {
    get
    {
      return (IEnumerable<CardKeyword>) new \u003C\u003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Unplayable);
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[3]
      {
        (DynamicVar) new CalculationBaseVar(3M),
        (DynamicVar) new CalculationExtraVar(-1M),
        (DynamicVar) new CalculatedVar("CalculatedCards").WithMultiplier((Func<CardModel, Creature, Decimal>) ((card, _) => (Decimal) Math.Min(3, ((Normality) card).CardsPlayedThisTurn)))
      });
    }
  }

  public override bool ShouldPlay(CardModel card, AutoPlayType _)
  {
    if (card.Owner != this.Owner)
      return true;
    CardPile pile = this.Pile;
    return (pile != null ? (pile.Type != PileType.Hand ? 1 : 0) : 1) != 0 || !this.ShouldPreventCardPlay;
  }

  private int CardsPlayedThisTurn
  {
    get
    {
      return CombatManager.Instance.History.CardPlaysStarted.Count<CardPlayStartedEntry>((Func<CardPlayStartedEntry, bool>) (e => e.HappenedThisTurn(this.CombatState) && e.CardPlay.Player == this.Owner));
    }
  }
}
