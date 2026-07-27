// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Rooms.CombatEventVisuals
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Rooms;

public class CombatEventVisuals : ICombatRoomVisuals
{
  public EncounterModel Encounter { get; }

  public IEnumerable<Creature> Allies { get; }

  public IEnumerable<Creature> Enemies
  {
    get
    {
      return this.Encounter.MonstersWithSlots.Select<(MonsterModel, string), Creature>((Func<(MonsterModel, string), Creature>) (m => m.Item1.Creature));
    }
  }

  public ActModel Act { get; }

  public CombatEventVisuals(EncounterModel encounter, IEnumerable<Player> players, ActModel act)
  {
    this.Encounter = encounter;
    this.Allies = players.Select<Player, Creature>((Func<Player, Creature>) (p => p.Creature));
    this.Act = act;
  }
}
