// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.PenNib
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class PenNib : RelicModel
{
  private const int _attacksThreshold = 10;
  private bool _isActivating;
  private int _attacksPlayed;
  private CardModel? _attackToDouble;

  public override RelicRarity Rarity => RelicRarity.Uncommon;

  public override bool ShowCounter => true;

  public override int DisplayAmount => !this.IsActivating ? this.AttacksPlayed % 10 : 10;

  private bool IsActivating
  {
    get => this._isActivating;
    set
    {
      this.AssertMutable();
      this._isActivating = value;
      this.UpdateDisplay();
    }
  }

  [SavedProperty]
  public int AttacksPlayed
  {
    get => this._attacksPlayed;
    private set
    {
      this.AssertMutable();
      this._attacksPlayed = value % 10;
      this.UpdateDisplay();
    }
  }

  private CardModel? AttackToDouble
  {
    get => this._attackToDouble;
    set
    {
      this.AssertMutable();
      this._attackToDouble = value;
    }
  }

  private void UpdateDisplay()
  {
    if (this.IsActivating)
      this.Status = RelicStatus.Normal;
    else
      this.Status = this.AttacksPlayed == 9 ? RelicStatus.Active : RelicStatus.Normal;
    this.InvokeDisplayAmountChanged();
  }

  public void NotifyAttackPlayed()
  {
    ++this.AttacksPlayed;
    if (this.AttacksPlayed != 0)
      return;
    TaskHelper.RunSafely(this.DoActivateVisuals());
  }

  public override Decimal ModifyDamageMultiplicative(
    Creature? target,
    Decimal amount,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource,
    CardPlay? cardPlay)
  {
    if (!props.IsPoweredAttack() || cardSource == null || dealer != this.Owner.Creature && dealer != this.Owner.Osty)
      return 1M;
    if (this.AttackToDouble == null)
    {
      CardPile pile = cardSource.Pile;
      return (pile != null ? (pile.Type != PileType.Play ? 1 : 0) : 1) != 0 && this.AttacksPlayed == 9 ? 2M : 1M;
    }
    return cardSource == this.AttackToDouble ? 2M : 1M;
  }

  public override Task BeforeCardPlayed(CardPlay cardPlay)
  {
    if (cardPlay.Card.Type != CardType.Attack || cardPlay.Card.Owner != this.Owner)
      return Task.CompletedTask;
    this.NotifyAttackPlayed();
    if (this.AttacksPlayed == 0)
      this.AttackToDouble = cardPlay.Card;
    return Task.CompletedTask;
  }

  public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    if (this.AttackToDouble == null || cardPlay.Card != this.AttackToDouble)
      return Task.CompletedTask;
    this.AttackToDouble = (CardModel) null;
    return Task.CompletedTask;
  }

  private async Task DoActivateVisuals()
  {
    this.IsActivating = true;
    this.Flash();
    await Cmd.Wait(1f);
    this.IsActivating = false;
  }
}
