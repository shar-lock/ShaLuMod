// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Encounters.SlumberingBeetleNormal
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Entities.Encounters;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Encounters;

public sealed class SlumberingBeetleNormal : EncounterModel
{
  public override IEnumerable<EncounterTag> Tags
  {
    get
    {
      return (IEnumerable<EncounterTag>) new \u003C\u003Ez__ReadOnlySingleElementList<EncounterTag>(EncounterTag.Workers);
    }
  }

  public override RoomType RoomType => RoomType.Monster;

  public override IEnumerable<MonsterModel> AllPossibleMonsters
  {
    get
    {
      return (IEnumerable<MonsterModel>) new \u003C\u003Ez__ReadOnlyArray<MonsterModel>(new MonsterModel[3]
      {
        (MonsterModel) ModelDb.Monster<BowlbugRock>(),
        (MonsterModel) ModelDb.Monster<SlumberingBeetle>(),
        (MonsterModel) ModelDb.Monster<BowlbugSilk>()
      });
    }
  }

  protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<(MonsterModel, string)>) new \u003C\u003Ez__ReadOnlyArray<(MonsterModel, string)>(new (MonsterModel, string)[3]
    {
      (ModelDb.Monster<BowlbugRock>().ToMutable(), "first"),
      (ModelDb.Monster<BowlbugSilk>().ToMutable(), "second"),
      (ModelDb.Monster<SlumberingBeetle>().ToMutable(), "third")
    });
  }

  public override float GetCameraScaling() => 0.85f;

  public override Vector2 GetCameraOffset() => new Vector2(0.0f, 50f);

  public override bool HasScene => true;
}
