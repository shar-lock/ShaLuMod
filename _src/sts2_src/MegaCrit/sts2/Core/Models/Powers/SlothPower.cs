// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.SlothPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class SlothPower : PowerModel
{
  private int _cardsPlayedThisTurn;

  public override PowerType Type => PowerType.Debuff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override int DisplayAmount => this._cardsPlayedThisTurn;

  public override bool ShouldPlay(CardModel card, AutoPlayType _)
  {
    return card.Owner.Creature != this.Owner || this._cardsPlayedThisTurn < this.Amount;
  }

  public override Task BeforeCardPlayed(CardPlay cardPlay)
  {
    if (cardPlay.Card.Owner != this.Owner.Player)
      return Task.CompletedTask;
    ++this._cardsPlayedThisTurn;
    this.InvokeDisplayAmountChanged();
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
    this._cardsPlayedThisTurn = 0;
    this.InvokeDisplayAmountChanged();
    return Task.CompletedTask;
  }
}
