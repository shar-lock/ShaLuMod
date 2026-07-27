// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Enchantments.Slither
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Enchantments;

public sealed class Slither : EnchantmentModel
{
  private int _testEnergyCostOverride = -1;

  public int TestEnergyCostOverride
  {
    get => this._testEnergyCostOverride;
    set
    {
      if (TestMode.IsOff)
        throw new InvalidOperationException("Only set this value in test mode.");
      this.AssertMutable();
      this._testEnergyCostOverride = value;
    }
  }

  public override bool CanEnchant(CardModel card)
  {
    return base.CanEnchant(card) && !card.Keywords.Contains(CardKeyword.Unplayable) && !card.EnergyCost.CostsX;
  }

  public override Task AfterCardDrawn(
    PlayerChoiceContext choiceContext,
    CardModel card,
    bool fromHandDraw)
  {
    if (card != this.Card || this.Card.Pile.Type != PileType.Hand)
      return Task.CompletedTask;
    this.Card.EnergyCost.SetThisCombat(this.NextEnergyCost());
    NCard.FindOnTable(card)?.PlayRandomizeCostAnim();
    return Task.CompletedTask;
  }

  private int NextEnergyCost()
  {
    return this.TestEnergyCostOverride >= 0 ? this.TestEnergyCostOverride : this.Card.Owner.RunState.Rng.CombatEnergyCosts.NextInt(4);
  }
}
