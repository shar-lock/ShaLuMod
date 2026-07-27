// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Combat.History.Entries.CreatureAttackedEntry
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Combat.History.Entries;

public class CreatureAttackedEntry : CombatHistoryEntry
{
  public IReadOnlyList<DamageResult> DamageResults { get; }

  public override string Description => this.Actor.Name + " attacked";

  public CreatureAttackedEntry(
    Creature attacker,
    IReadOnlyList<DamageResult> damageResults,
    int roundNumber,
    CombatSide currentSide,
    CombatHistory history,
    IEnumerable<Player> players)
    : base(attacker, roundNumber, currentSide, history, players)
  {
    this.DamageResults = damageResults;
  }
}
