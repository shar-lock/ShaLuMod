// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.RingingPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Afflictions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class RingingPower : PowerModel
{
  public override PowerType Type => PowerType.Debuff;

  public override PowerStackType StackType => PowerStackType.Single;

  public override async Task AfterApplied(Creature? applier, CardModel? cardSource)
  {
    foreach (CardModel allCard in this.Owner.Player.PlayerCombatState.AllCards)
    {
      if (allCard.Affliction == null)
      {
        Ringing ringing = await CardCmd.Afflict<Ringing>(allCard, 1M);
      }
    }
  }

  public override async Task AfterCardEnteredCombat(CardModel card)
  {
    if (card.Owner != this.Owner.Player || card.Affliction != null)
      return;
    Ringing ringing = await CardCmd.Afflict<Ringing>(card, 1M);
  }

  public override async Task AfterSideTurnEnd(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IEnumerable<Creature> participants)
  {
    if (!participants.Contains<Creature>(this.Owner))
      return;
    this.Flash();
    await PowerCmd.Remove((PowerModel) this);
  }

  public override Task AfterRemoved(Creature oldOwner)
  {
    Player player = oldOwner.Player;
    object obj;
    if (player == null)
    {
      obj = (object) null;
    }
    else
    {
      PlayerCombatState playerCombatState = player.PlayerCombatState;
      obj = playerCombatState != null ? (object) playerCombatState.AllCards : (object) null;
    }
    if (obj == null)
      obj = (object) Array.Empty<CardModel>();
    foreach (CardModel card in (IEnumerable<CardModel>) obj)
    {
      if (card.Affliction is Ringing)
        CardCmd.ClearAffliction(card);
    }
    return Task.CompletedTask;
  }

  public override bool ShouldPlay(CardModel card, AutoPlayType _)
  {
    return card.Owner.Creature != this.Owner || !(card.Affliction is Ringing) || !CombatManager.Instance.History.CardPlaysStarted.Any<CardPlayStartedEntry>((Func<CardPlayStartedEntry, bool>) (e => e.HappenedThisTurn(this.CombatState) && e.CardPlay.Player == this.Owner.Player));
  }
}
