// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Badges.CccComboModel
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Badges;

public class CccComboModel : BadgeModel
{
  private int _cardsPlayedThisTurn;

  public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    if (!LocalContext.IsMine(cardPlay.Card))
      return Task.CompletedTask;
    ++this._cardsPlayedThisTurn;
    if (this._cardsPlayedThisTurn >= 20)
    {
      Player owner = cardPlay.Card.Owner;
      if (!owner.ExtraFields.CccomboBadgeUnlocked)
        Log.Info("Player played 20 cards in a single turn, ccccombo unlocked");
      owner.ExtraFields.CccomboBadgeUnlocked = true;
    }
    return Task.CompletedTask;
  }

  public override Task AfterSideTurnStart(
    CombatSide side,
    IReadOnlyList<Creature> participants,
    ICombatState combatState)
  {
    if (side != CombatSide.Player)
      return Task.CompletedTask;
    this._cardsPlayedThisTurn = 0;
    return Task.CompletedTask;
  }
}
