// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.HistoryCourse
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
using System;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class HistoryCourse : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Event;

  public override async Task AfterAutoPrePlayPhaseEntered(
    PlayerChoiceContext choiceContext,
    Player player)
  {
    if (player != this.Owner || this.Owner.PlayerCombatState.TurnNumber == 1)
      return;
    CardModel card = CombatManager.Instance.History.CardPlaysFinished.LastOrDefault<CardPlayFinishedEntry>((Func<CardPlayFinishedEntry, bool>) (e => e.CardPlay.Player == this.Owner && e.HappenedLastPlayerTurn(this.Owner) && e.CardPlay.Card.Type == CardType.Attack && !e.CardPlay.Card.IsDupe))?.CardPlay.Card;
    if (card == null)
      return;
    this.Flash();
    await CardCmd.AutoPlay(choiceContext, card.CreateDupe(player), (Creature) null);
  }
}
