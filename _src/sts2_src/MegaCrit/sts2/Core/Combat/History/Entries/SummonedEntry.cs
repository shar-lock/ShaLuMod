// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Combat.History.Entries.SummonedEntry
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Combat.History.Entries;

public class SummonedEntry(
  int amount,
  Player player,
  int roundNumber,
  CombatSide currentSide,
  CombatHistory history,
  IEnumerable<Player> players) : CombatHistoryEntry(player.Creature, roundNumber, currentSide, history, players)
{
  public int Amount { get; } = amount;

  public override string Description
  {
    get => $"{this.Actor.Player.Character.Id.Entry} summoned {this.Amount}";
  }
}
