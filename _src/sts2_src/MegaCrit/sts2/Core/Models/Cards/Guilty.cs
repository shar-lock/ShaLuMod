// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Guilty
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Guilty : CardModel
{
  public const int maxCombats = 5;
  private const string _combatsKey = "Combats";
  private int _combatsSeen;

  public Guilty()
    : base(-1, CardType.Curse, CardRarity.Curse, TargetType.None)
  {
  }

  public override int MaxUpgradeLevel => 0;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Combats", 5M));
    }
  }

  public override IEnumerable<CardKeyword> CanonicalKeywords
  {
    get
    {
      return (IEnumerable<CardKeyword>) new \u003C\u003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Unplayable);
    }
  }

  [SavedProperty]
  public int CombatsSeen
  {
    get => this._combatsSeen;
    set
    {
      this.AssertMutable();
      this._combatsSeen = value;
      this.DynamicVars["Combats"].BaseValue = (Decimal) (5 - this.CombatsSeen);
    }
  }

  public override async Task AfterCombatEnd(CombatRoom _)
  {
    CardPile pile = this.Pile;
    if ((pile != null ? (pile.Type != PileType.Deck ? 1 : 0) : 1) != 0)
      return;
    this.CombatsSeen++;
    if (this.CombatsSeen < 5 || this.Pile.Type != PileType.Deck)
      return;
    await CardPileCmd.RemoveFromDeck((CardModel) this);
  }
}
