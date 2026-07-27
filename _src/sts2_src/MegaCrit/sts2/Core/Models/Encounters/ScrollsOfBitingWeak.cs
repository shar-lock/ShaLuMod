// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Encounters.ScrollsOfBitingWeak
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Encounters;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Encounters;

public sealed class ScrollsOfBitingWeak : EncounterModel
{
  public override RoomType RoomType => RoomType.Monster;

  public override bool IsWeak => true;

  public override IEnumerable<EncounterTag> Tags
  {
    get
    {
      return (IEnumerable<EncounterTag>) new \u003C\u003Ez__ReadOnlySingleElementList<EncounterTag>(EncounterTag.Scrolls);
    }
  }

  public override IEnumerable<MonsterModel> AllPossibleMonsters
  {
    get
    {
      return (IEnumerable<MonsterModel>) new \u003C\u003Ez__ReadOnlySingleElementList<MonsterModel>((MonsterModel) ModelDb.Monster<ScrollOfBiting>());
    }
  }

  protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters()
  {
    ScrollOfBiting mutable1 = (ScrollOfBiting) ModelDb.Monster<ScrollOfBiting>().ToMutable();
    ScrollOfBiting mutable2 = (ScrollOfBiting) ModelDb.Monster<ScrollOfBiting>().ToMutable();
    ScrollOfBiting mutable3 = (ScrollOfBiting) ModelDb.Monster<ScrollOfBiting>().ToMutable();
    int num = this.Rng.NextInt(3);
    mutable1.StarterMoveIdx = num;
    mutable2.StarterMoveIdx = (num + 1) % 3;
    mutable3.StarterMoveIdx = (num + 2) % 3;
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<(MonsterModel, string)>) new \u003C\u003Ez__ReadOnlyArray<(MonsterModel, string)>(new (MonsterModel, string)[3]
    {
      ((MonsterModel) mutable1, (string) null),
      ((MonsterModel) mutable2, (string) null),
      ((MonsterModel) mutable3, (string) null)
    });
  }
}
