// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Encounters.SlimesWeak
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Encounters;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Encounters;

public sealed class SlimesWeak : EncounterModel
{
  private static readonly MonsterModel[] _smallSlimes = new MonsterModel[2]
  {
    (MonsterModel) ModelDb.Monster<LeafSlimeS>(),
    (MonsterModel) ModelDb.Monster<TwigSlimeS>()
  };
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
      return (IEnumerable<EncounterTag>) new \u003C\u003Ez__ReadOnlySingleElementList<EncounterTag>(EncounterTag.Slimes);
    }
  }

  public override bool IsWeak => true;

  public override IEnumerable<MonsterModel> AllPossibleMonsters
  {
    get
    {
      MonsterModel[] smallSlimes = SlimesWeak._smallSlimes;
      MonsterModel[] mediumSlimes = SlimesWeak._mediumSlimes;
      int num1 = 0;
      MonsterModel[] items = new MonsterModel[smallSlimes.Length + mediumSlimes.Length];
      ReadOnlySpan<MonsterModel> readOnlySpan1 = new ReadOnlySpan<MonsterModel>(smallSlimes);
      readOnlySpan1.CopyTo(new Span<MonsterModel>(items).Slice(num1, readOnlySpan1.Length));
      int num2 = num1 + readOnlySpan1.Length;
      ReadOnlySpan<MonsterModel> readOnlySpan2 = new ReadOnlySpan<MonsterModel>(mediumSlimes);
      readOnlySpan2.CopyTo(new Span<MonsterModel>(items).Slice(num2, readOnlySpan2.Length));
      int num3 = num2 + readOnlySpan2.Length;
      return (IEnumerable<MonsterModel>) new \u003C\u003Ez__ReadOnlyArray<MonsterModel>(items);
    }
  }

  protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters()
  {
    List<(MonsterModel, string)> monsters = new List<(MonsterModel, string)>();
    List<MonsterModel> list = ((IEnumerable<MonsterModel>) SlimesWeak._smallSlimes).ToList<MonsterModel>();
    MonsterModel monsterModel1 = this.Rng.NextItem<MonsterModel>((IEnumerable<MonsterModel>) list);
    list.Remove(monsterModel1);
    MonsterModel monsterModel2 = this.Rng.NextItem<MonsterModel>((IEnumerable<MonsterModel>) list);
    monsters.Add((monsterModel1.ToMutable(), (string) null));
    monsters.Add((this.Rng.NextItem<MonsterModel>((IEnumerable<MonsterModel>) SlimesWeak._mediumSlimes).ToMutable(), (string) null));
    monsters.Add((monsterModel2.ToMutable(), (string) null));
    return (IReadOnlyList<(MonsterModel, string)>) monsters;
  }
}
