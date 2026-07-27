// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.BookOfFiveRings
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class BookOfFiveRings : RelicModel
{
  private bool _isActivating;
  private int _cardsAdded;

  public override RelicRarity Rarity => RelicRarity.Common;

  public override bool IsAllowed(IRunState runState)
  {
    return RelicModel.IsBeforeAct3TreasureChest(runState);
  }

  public override bool ShowCounter => true;

  public override int DisplayAmount
  {
    get => !this.IsActivating ? this.CardsAddedSinceLastTrigger : this.DynamicVars.Cards.IntValue;
  }

  private bool IsActivating
  {
    get => this._isActivating;
    set
    {
      this.AssertMutable();
      this._isActivating = value;
      this.InvokeDisplayAmountChanged();
    }
  }

  [SavedProperty]
  public int CardsAdded
  {
    get => this._cardsAdded;
    set
    {
      this.AssertMutable();
      this._cardsAdded = value;
      this.InvokeDisplayAmountChanged();
    }
  }

  private int CardsAddedSinceLastTrigger => this.CardsAdded % this.DynamicVars.Cards.IntValue;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new CardsVar(5),
        (DynamicVar) new HealVar(20M)
      });
    }
  }

  public override async Task AfterCardChangedPiles(
    CardModel card,
    PileType oldPileType,
    AbstractModel? clonedBy)
  {
    if (this.Owner.Creature.IsDead || card.Owner != this.Owner)
      return;
    CardPile pile = card.Pile;
    if ((pile != null ? (pile.Type != PileType.Deck ? 1 : 0) : 1) != 0)
      return;
    this.CardsAdded++;
    if (this.CardsAddedSinceLastTrigger != 0)
      return;
    TaskHelper.RunSafely(this.DoActivateVisuals());
    await CreatureCmd.Heal(this.Owner.Creature, this.DynamicVars.Heal.BaseValue);
  }

  private async Task DoActivateVisuals()
  {
    this.IsActivating = true;
    this.Flash();
    await Cmd.Wait(1f);
    this.IsActivating = false;
  }
}
