// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Encounters.RubyRaidersNormal
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Encounters;

public sealed class RubyRaidersNormal : EncounterModel
{
  private static readonly Dictionary<MonsterModel, int> _raiderValidCounts = new Dictionary<MonsterModel, int>()
  {
    {
      (MonsterModel) ModelDb.Monster<AxeRubyRaider>(),
      1
    },
    {
      (MonsterModel) ModelDb.Monster<AssassinRubyRaider>(),
      1
    },
    {
      (MonsterModel) ModelDb.Monster<BruteRubyRaider>(),
      1
    },
    {
      (MonsterModel) ModelDb.Monster<CrossbowRubyRaider>(),
      1
    },
    {
      (MonsterModel) ModelDb.Monster<TrackerRubyRaider>(),
      1
    }
  };

  public override RoomType RoomType => RoomType.Monster;

  public override IEnumerable<MonsterModel> AllPossibleMonsters
  {
    get => (IEnumerable<MonsterModel>) RubyRaidersNormal._raiderValidCounts.Keys;
  }

  protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters()
  {
    List<MonsterModel> currentRaiders = new List<MonsterModel>();
    List<(MonsterModel, string)> monsters = new List<(MonsterModel, string)>();
    for (int index = 0; index < 3; ++index)
    {
      MonsterModel monsterModel = this.Rng.NextItem<MonsterModel>((IEnumerable<MonsterModel>) RubyRaidersNormal._raiderValidCounts.Keys.Where<MonsterModel>((Func<MonsterModel, bool>) (r => currentRaiders.Count<MonsterModel>((Func<MonsterModel, bool>) (c => c == r)) < RubyRaidersNormal._raiderValidCounts[r])).ToList<MonsterModel>());
      currentRaiders.Add(monsterModel);
      monsters.Add((monsterModel.ToMutable(), (string) null));
    }
    return (IReadOnlyList<(MonsterModel, string)>) monsters;
  }
}
