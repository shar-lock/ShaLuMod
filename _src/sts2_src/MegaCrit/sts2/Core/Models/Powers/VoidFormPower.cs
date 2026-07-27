// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.VoidFormPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class VoidFormPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  protected override object InitInternalData() => (object) new VoidFormPower.Data();

  public override Task BeforePowerAmountChanged(
    PowerModel power,
    Decimal amount,
    Creature target,
    Creature? applier,
    CardModel? cardSource)
  {
    if (power != this)
      return Task.CompletedTask;
    this.HideTemporaryZeroCostVisual();
    return Task.CompletedTask;
  }

  public override Task BeforeApplied(
    Creature target,
    Decimal amount,
    Creature? applier,
    CardModel? cardSource)
  {
    this.HideTemporaryZeroCostVisual();
    return Task.CompletedTask;
  }

  public override bool TryModifyEnergyCostInCombatLate(
    CardModel card,
    Decimal originalCost,
    out Decimal modifiedCost)
  {
    modifiedCost = originalCost;
    if (this.ShouldSkip(card))
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
    if (this.ShouldSkip(card))
      return false;
    modifiedCost = 0M;
    return true;
  }

  public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    if (cardPlay.Card.Owner.Creature == this.Owner && cardPlay != null && !cardPlay.IsAutoPlay && cardPlay.IsLastInSeries)
      ++this.GetInternalData<VoidFormPower.Data>().cardsPlayedThisTurn;
    return Task.CompletedTask;
  }

  public override Task BeforeSideTurnStart(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IReadOnlyList<Creature> participants,
    ICombatState combatState)
  {
    if (!participants.Contains<Creature>(this.Owner))
      return Task.CompletedTask;
    this.GetInternalData<VoidFormPower.Data>().cardsPlayedThisTurn = 0;
    return Task.CompletedTask;
  }

  private bool ShouldSkip(CardModel card)
  {
    bool flag1 = card.Owner.Creature != this.Owner;
    if (!flag1)
    {
      PileType? type = card.Pile?.Type;
      bool flag2;
      if (type.HasValue)
      {
        switch (type.GetValueOrDefault())
        {
          case PileType.Hand:
          case PileType.Play:
            flag2 = true;
            goto label_5;
        }
      }
      flag2 = false;
label_5:
      flag1 = !flag2;
    }
    return flag1 || this.GetInternalData<VoidFormPower.Data>().cardsPlayedThisTurn >= this.Amount;
  }

  private void HideTemporaryZeroCostVisual()
  {
    this.GetInternalData<VoidFormPower.Data>().cardsPlayedThisTurn = 999999999;
  }

  private class Data
  {
    public int cardsPlayedThisTurn;
  }
}
