// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Regret
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Regret : CardModel
{
  private int _cardsInHand;

  public Regret()
    : base(-1, CardType.Curse, CardRarity.Curse, TargetType.None)
  {
  }

  public override int MaxUpgradeLevel => 0;

  public override IEnumerable<CardKeyword> CanonicalKeywords
  {
    get
    {
      return (IEnumerable<CardKeyword>) new \u003C\u003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Unplayable);
    }
  }

  private int CardsInHand
  {
    get => this._cardsInHand;
    set
    {
      this.AssertMutable();
      this._cardsInHand = value;
    }
  }

  public override bool HasTurnEndInHandEffect => true;

  public override Task BeforeSideTurnEnd(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IEnumerable<Creature> participants)
  {
    if (!participants.Contains<Creature>(this.Owner.Creature) || this.Pile.Type != PileType.Hand)
      return Task.CompletedTask;
    this.CardsInHand = this.Pile.Cards.Count;
    return Task.CompletedTask;
  }

  protected override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
  {
    IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(choiceContext, this.Owner.Creature, (Decimal) this.CardsInHand, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, (CardModel) this, (CardPlay) null);
    this.CardsInHand = 0;
  }
}
