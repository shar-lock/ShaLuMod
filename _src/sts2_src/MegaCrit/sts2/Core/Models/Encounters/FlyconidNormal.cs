// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Encounters.FlyconidNormal
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Encounters;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Encounters;

public sealed class FlyconidNormal : EncounterModel
{
  private static readonly MonsterModel[] _mediumSlimes = new MonsterModel[2]
  {
    (MonsterModel) ModelDb.Monster<LeafSlimeM>(),
    (MonsterModel) ModelDb.Monster<TwigSlimeM>()
  };

  public override RoomType RoomType => RoomType.Monster;

  public override IEnumerable<EncounterTag> Tags
  {
    get
    {
      return (IEnumerable<EncounterTag>) new \u003C\u003Ez__ReadOnlyArray<EncounterTag>(new EncounterTag[2]
      {
        EncounterTag.Mushroom,
        EncounterTag.Slimes
      });
    }
  }

  public override IEnumerable<MonsterModel> AllPossibleMonsters
  {
    get
    {
      MonsterModel monsterModel = (MonsterModel) ModelDb.Monster<Flyconid>();
      MonsterModel[] mediumSlimes = FlyconidNormal._mediumSlimes;
      int index = 0;
      MonsterModel[] items = new MonsterModel[1 + mediumSlimes.Length];
      items[index] = monsterModel;
      int num1 = index + 1;
      ReadOnlySpan<MonsterModel> readOnlySpan = new ReadOnlySpan<MonsterModel>(mediumSlimes);
      readOnlySpan.CopyTo(new Span<MonsterModel>(items).Slice(num1, readOnlySpan.Length));
      int num2 = num1 + readOnlySpan.Length;
      return (IEnumerable<MonsterModel>) new \u003C\u003Ez__ReadOnlyArray<MonsterModel>(items);
    }
  }

  protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<(MonsterModel, string)>) new \u003C\u003Ez__ReadOnlyArray<(MonsterModel, string)>(new (MonsterModel, string)[2]
    {
      (this.Rng.NextItem<MonsterModel>((IEnumerable<MonsterModel>) FlyconidNormal._mediumSlimes).ToMutable(), (string) null),
      (ModelDb.Monster<Flyconid>().ToMutable(), (string) null)
    });
  }
}
