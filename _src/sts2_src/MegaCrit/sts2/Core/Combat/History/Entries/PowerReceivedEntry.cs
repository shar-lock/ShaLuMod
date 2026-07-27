// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Combat.History.Entries.PowerReceivedEntry
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Combat.History.Entries;

public class PowerReceivedEntry : CombatHistoryEntry
{
  public PowerModel Power { get; }

  public Decimal Amount { get; }

  public Creature? Applier { get; }

  public override string Description
  {
    get
    {
      if (this.Applier != null)
        return $"{this.Applier.ModelId.Entry} applied {this.Amount} {this.Power.Id.Entry} to {this.Actor.ModelId.Entry}";
      return $"{this.Actor.ModelId.Entry} received {this.Amount} {this.Power.Id.Entry}";
    }
  }

  public PowerReceivedEntry(
    PowerModel power,
    Decimal amount,
    Creature? applier,
    int roundNumber,
    CombatSide currentSide,
    CombatHistory history,
    IEnumerable<Player> players)
    : base(power.Owner, roundNumber, currentSide, history, players)
  {
    this.Power = power;
    this.Amount = amount;
    this.Applier = applier;
  }
}
