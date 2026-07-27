// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.MusicBox
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Rooms;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class MusicBox : RelicModel
{
  private bool _wasUsedThisTurn;
  private CardModel? _cardBeingPlayed;

  public override RelicRarity Rarity => RelicRarity.Ancient;

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Ethereal));
    }
  }

  private bool WasUsedThisTurn
  {
    get => this._wasUsedThisTurn;
    set
    {
      this.AssertMutable();
      this._wasUsedThisTurn = value;
    }
  }

  private CardModel? CardBeingPlayed
  {
    get => this._cardBeingPlayed;
    set
    {
      this.AssertMutable();
      this._cardBeingPlayed = value;
    }
  }

  public override Task BeforeCardPlayed(CardPlay cardPlay)
  {
    if (this.CardBeingPlayed != null || cardPlay.Card.Owner != this.Owner || this.WasUsedThisTurn || cardPlay.Card.Type != CardType.Attack)
      return Task.CompletedTask;
    this.CardBeingPlayed = cardPlay.Card;
    return Task.CompletedTask;
  }

  public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    if (cardPlay.Card != this.CardBeingPlayed)
      return;
    this.Flash();
    CardModel clone = cardPlay.Card.CreateClone();
    CardCmd.ApplyKeyword(clone, CardKeyword.Ethereal);
    CardPileAddResult combat = await CardPileCmd.AddGeneratedCardToCombat(clone, PileType.Hand, this.Owner);
    this.WasUsedThisTurn = true;
    this.CardBeingPlayed = (CardModel) null;
  }

  public override Task BeforeSideTurnStart(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IReadOnlyList<Creature> participants,
    ICombatState combatState)
  {
    if (!participants.Contains<Creature>(this.Owner.Creature))
      return Task.CompletedTask;
    this.WasUsedThisTurn = false;
    this.CardBeingPlayed = (CardModel) null;
    return Task.CompletedTask;
  }

  public override Task AfterCombatEnd(CombatRoom _)
  {
    this.WasUsedThisTurn = false;
    this.CardBeingPlayed = (CardModel) null;
    return Task.CompletedTask;
  }
}
