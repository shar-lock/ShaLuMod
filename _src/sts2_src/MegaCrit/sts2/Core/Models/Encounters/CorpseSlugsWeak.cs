// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Encounters.CorpseSlugsWeak
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

public sealed class CorpseSlugsWeak : EncounterModel
{
  public override IEnumerable<EncounterTag> Tags
  {
    get
    {
      return (IEnumerable<EncounterTag>) new \u003C\u003Ez__ReadOnlySingleElementList<EncounterTag>(EncounterTag.Slugs);
    }
  }

  public override RoomType RoomType => RoomType.Monster;

  public override bool IsWeak => true;

  public override IEnumerable<MonsterModel> AllPossibleMonsters
  {
    get
    {
      return (IEnumerable<MonsterModel>) new \u003C\u003Ez__ReadOnlySingleElementList<MonsterModel>((MonsterModel) ModelDb.Monster<CorpseSlug>());
    }
  }

  protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters()
  {
    int capacity = 2;
    List<(MonsterModel, string)> valueTupleList = new List<(MonsterModel, string)>(capacity);
    CollectionsMarshal.SetCount<(MonsterModel, string)>(valueTupleList, capacity);
    Span<(MonsterModel, string)> span = CollectionsMarshal.AsSpan<(MonsterModel, string)>(valueTupleList);
    int num1 = 0;
    span[num1] = (ModelDb.Monster<CorpseSlug>().ToMutable(), (string) null);
    int num2 = num1 + 1;
    span[num2] = (ModelDb.Monster<CorpseSlug>().ToMutable(), (string) null);
    List<(MonsterModel, string)> source = valueTupleList;
    CorpseSlug.EnsureCorpseSlugsStartWithDifferentMoves(source.Select<(MonsterModel, string), MonsterModel>((Func<(MonsterModel, string), MonsterModel>) (kvp => kvp.Item1)), this.Rng);
    return (IReadOnlyList<(MonsterModel, string)>) source;
  }
}
