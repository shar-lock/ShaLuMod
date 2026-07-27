// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Combat.History.Entries.CardGeneratedEntry
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Combat.History.Entries;

public class CardGeneratedEntry : CombatHistoryEntry
{
  public CardModel Card { get; }

  public Player? Creator { get; }

  public override string Description
  {
    get => $"{this.Actor.Player.Character.Id.Entry} generated {this.Card.Id.Entry} during combat";
  }

  public CardGeneratedEntry(
    CardModel card,
    Player? creator,
    int roundNumber,
    CombatSide currentSide,
    CombatHistory history,
    IEnumerable<Player> players)
    : base(card.Owner.Creature, roundNumber, currentSide, history, players)
  {
    this.Card = card;
    this.Creator = creator;
  }
}
