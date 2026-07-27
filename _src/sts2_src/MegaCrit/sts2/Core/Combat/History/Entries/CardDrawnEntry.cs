// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Combat.History.Entries.CardDrawnEntry
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Combat.History.Entries;

public class CardDrawnEntry : CombatHistoryEntry
{
  public CardModel Card { get; }

  public bool FromHandDraw { get; }

  public override string Description
  {
    get => $"{this.Actor.Player.Character.Id.Entry} discarded {this.Card.Id.Entry}";
  }

  public CardDrawnEntry(
    CardModel card,
    int roundNumber,
    CombatSide currentSide,
    bool fromHandDraw,
    CombatHistory history,
    IEnumerable<Player> players)
    : base(card.Owner.Creature, roundNumber, currentSide, history, players)
  {
    this.Card = card;
    this.FromHandDraw = fromHandDraw;
  }
}
