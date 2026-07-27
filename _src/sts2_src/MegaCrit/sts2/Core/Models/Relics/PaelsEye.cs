// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.PaelsEye
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class PaelsEye : RelicModel
{
  private bool _usedThisCombat;
  private bool _wasOwnerPartOfLastPlayerTurn = true;

  public override RelicRarity Rarity => RelicRarity.Ancient;

  private bool UsedThisCombat
  {
    get => this._usedThisCombat;
    set
    {
      this.AssertMutable();
      this._usedThisCombat = value;
    }
  }

  private bool WasOwnerPartOfLastPlayerTurn
  {
    get => this._wasOwnerPartOfLastPlayerTurn;
    set
    {
      this.AssertMutable();
      this._wasOwnerPartOfLastPlayerTurn = value;
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromKeyword(CardKeyword.Exhaust));
    }
  }

  public override Task AfterObtained()
  {
    this.WasOwnerPartOfLastPlayerTurn = CombatManager.Instance.IsPartOfPlayerTurn(this.Owner);
    return Task.CompletedTask;
  }

  public override Task BeforeCardPlayed(CardPlay cardPlay)
  {
    if (!CombatManager.Instance.IsInProgress || this.UsedThisCombat || cardPlay.IsAutoPlay || cardPlay.Card.Owner != this.Owner)
      return Task.CompletedTask;
    this.Status = RelicStatus.Normal;
    return Task.CompletedTask;
  }

  public override Task AfterSideTurnStart(
    CombatSide side,
    IReadOnlyList<Creature> participants,
    ICombatState combatState)
  {
    if (side != this.Owner.Creature.Side || this.UsedThisCombat)
      return Task.CompletedTask;
    if (participants.Contains<Creature>(this.Owner.Creature))
    {
      this.Status = RelicStatus.Active;
      this.WasOwnerPartOfLastPlayerTurn = true;
    }
    else
    {
      this.Status = RelicStatus.Normal;
      this.WasOwnerPartOfLastPlayerTurn = false;
    }
    return Task.CompletedTask;
  }

  public override bool ShouldTakeExtraTurn(Player player)
  {
    return !this.UsedThisCombat && !this.AnyCardsPlayedThisTurn() && this.WasOwnerPartOfLastPlayerTurn && player == this.Owner;
  }

  public override async Task BeforeSideTurnEndEarly(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IEnumerable<Creature> participants)
  {
    if (!participants.Contains<Creature>(this.Owner.Creature) || this.UsedThisCombat || this.AnyCardsPlayedThisTurn() || !this.WasOwnerPartOfLastPlayerTurn)
      return;
    Player owner = this.Owner;
    PileType[] pileTypeArray = new PileType[1]
    {
      PileType.Hand
    };
    foreach (CardModel card in CardPile.GetCards(owner, pileTypeArray).ToList<CardModel>())
      await CardCmd.Exhaust(choiceContext, card);
  }

  public override Task AfterTakingExtraTurn(Player player)
  {
    if (player != this.Owner)
      return Task.CompletedTask;
    this.Flash();
    this.Status = RelicStatus.Normal;
    this.UsedThisCombat = true;
    return Task.CompletedTask;
  }

  public override Task AfterCombatEnd(CombatRoom _)
  {
    this.Status = RelicStatus.Normal;
    this.UsedThisCombat = false;
    return Task.CompletedTask;
  }

  private bool AnyCardsPlayedThisTurn()
  {
    PlayerCombatState playerCombatState = this.Owner.PlayerCombatState;
    return (playerCombatState != null ? (playerCombatState.TurnNumber == 1 ? 1 : 0) : 0) != 0 && this.Owner.Relics.Any<RelicModel>((Func<RelicModel, bool>) (r => r is WhisperingEarring)) || CombatManager.Instance.History.CardPlaysFinished.Any<CardPlayFinishedEntry>((Func<CardPlayFinishedEntry, bool>) (e => e.Actor == this.Owner.Creature && e.HappenedThisTurn(this.Owner.Creature.CombatState) && !e.CardPlay.IsAutoPlay));
  }
}
