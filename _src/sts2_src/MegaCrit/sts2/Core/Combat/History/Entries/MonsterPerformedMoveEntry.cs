// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Combat.History.Entries.MonsterPerformedMoveEntry
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#nullable enable
namespace MegaCrit.Sts2.Core.Combat.History.Entries;

public class MonsterPerformedMoveEntry : CombatHistoryEntry
{
  public MonsterModel Monster { get; }

  public MoveState Move { get; }

  public IEnumerable<Creature>? Targets { get; }

  public override string Description
  {
    get
    {
      StringBuilder stringBuilder1 = new StringBuilder($"{this.Monster.Id.Entry} performed {this.Move.Id}");
      if (this.Targets != null)
      {
        StringBuilder stringBuilder2 = stringBuilder1;
        StringBuilder stringBuilder3 = stringBuilder2;
        StringBuilder.AppendInterpolatedStringHandler interpolatedStringHandler;
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(11, 1, stringBuilder2);
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(" targeting ");
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(string.Join(",", this.Targets.Select<Creature, string>(MonsterPerformedMoveEntry.\u003C\u003EO.\u003C0\u003E__GetTargetName ?? (MonsterPerformedMoveEntry.\u003C\u003EO.\u003C0\u003E__GetTargetName = new Func<Creature, string>(MonsterPerformedMoveEntry.GetTargetName)))));
        ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
        stringBuilder3.Append(ref local);
      }
      return stringBuilder1.ToString();
    }
  }

  public MonsterPerformedMoveEntry(
    MonsterModel monster,
    MoveState move,
    IEnumerable<Creature>? targets,
    int roundNumber,
    CombatSide currentSide,
    CombatHistory history,
    IEnumerable<Player> players)
    : base(monster.Creature, roundNumber, currentSide, history, players)
  {
    this.Monster = monster;
    this.Move = move;
    this.Targets = targets;
  }

  private static string GetTargetName(Creature creature)
  {
    return !creature.IsPlayer ? creature.Monster.Id.Entry : creature.Player.Character.Id.Entry;
  }
}
