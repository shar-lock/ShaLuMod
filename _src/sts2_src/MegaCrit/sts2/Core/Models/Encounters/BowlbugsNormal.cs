// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Encounters.BowlbugsNormal
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Encounters;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Encounters;

public sealed class BowlbugsNormal : EncounterModel
{
  private static readonly Dictionary<MonsterModel, int> _workerValidCounts = new Dictionary<MonsterModel, int>()
  {
    {
      (MonsterModel) ModelDb.Monster<BowlbugEgg>(),
      1
    },
    {
      (MonsterModel) ModelDb.Monster<BowlbugSilk>(),
      1
    },
    {
      (MonsterModel) ModelDb.Monster<BowlbugNectar>(),
      1
    }
  };
  private static readonly string[] _slotNames = new string[3]
  {
    "first",
    "middle",
    "last"
  };

  public override IEnumerable<EncounterTag> Tags
  {
    get
    {
      return (IEnumerable<EncounterTag>) new \u003C\u003Ez__ReadOnlySingleElementList<EncounterTag>(EncounterTag.Workers);
    }
  }

  public override RoomType RoomType => RoomType.Monster;

  public override bool HasScene => true;

  public override IEnumerable<MonsterModel> AllPossibleMonsters
  {
    get
    {
      Dictionary<MonsterModel, int>.KeyCollection keys = BowlbugsNormal._workerValidCounts.Keys;
      int index = 0;
      MonsterModel[] items = new MonsterModel[1 + keys.Count];
      foreach (MonsterModel monsterModel in keys)
      {
        items[index] = monsterModel;
        ++index;
      }
      items[index] = (MonsterModel) ModelDb.Monster<BowlbugRock>();
      return (IEnumerable<MonsterModel>) new \u003C\u003Ez__ReadOnlyArray<MonsterModel>(items);
    }
  }

  protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters()
  {
    List<MonsterModel> currentWorkers = new List<MonsterModel>();
    int capacity = 1;
    List<(MonsterModel, string)> valueTupleList = new List<(MonsterModel, string)>(capacity);
    CollectionsMarshal.SetCount<(MonsterModel, string)>(valueTupleList, capacity);
    CollectionsMarshal.AsSpan<(MonsterModel, string)>(valueTupleList)[0] = (ModelDb.Monster<BowlbugRock>().ToMutable(), BowlbugsNormal._slotNames[0]);
    List<(MonsterModel, string)> monsters = valueTupleList;
    for (int index = 0; index < 2; ++index)
    {
      MonsterModel monsterModel = this.Rng.NextItem<MonsterModel>((IEnumerable<MonsterModel>) BowlbugsNormal._workerValidCounts.Keys.Where<MonsterModel>((Func<MonsterModel, bool>) (r => currentWorkers.Count<MonsterModel>((Func<MonsterModel, bool>) (c => c == r)) < BowlbugsNormal._workerValidCounts[r])).ToList<MonsterModel>());
      currentWorkers.Add(monsterModel);
      monsters.Add((monsterModel.ToMutable(), BowlbugsNormal._slotNames[index + 1]));
    }
    return (IReadOnlyList<(MonsterModel, string)>) monsters;
  }
}
