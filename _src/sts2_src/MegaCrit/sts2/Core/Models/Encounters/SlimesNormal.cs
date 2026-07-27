// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Encounters.SlimesNormal
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

public sealed class SlimesNormal : EncounterModel
{
  public override float GetCameraScaling() => 0.9f;

  public override Vector2 GetCameraOffset() => Vector2.op_Multiply(Vector2.Down, 50f);

  public override RoomType RoomType => RoomType.Monster;

  public override IEnumerable<EncounterTag> Tags
  {
    get
    {
      return (IEnumerable<EncounterTag>) new \u003C\u003Ez__ReadOnlySingleElementList<EncounterTag>(EncounterTag.Slimes);
    }
  }

  public override IEnumerable<MonsterModel> AllPossibleMonsters
  {
    get
    {
      return (IEnumerable<MonsterModel>) new \u003C\u003Ez__ReadOnlyArray<MonsterModel>(new MonsterModel[4]
      {
        (MonsterModel) ModelDb.Monster<LeafSlimeS>(),
        (MonsterModel) ModelDb.Monster<TwigSlimeS>(),
        (MonsterModel) ModelDb.Monster<TwigSlimeM>(),
        (MonsterModel) ModelDb.Monster<LeafSlimeM>()
      });
    }
  }

  protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters()
  {
    bool flag = this.Rng.NextBool();
    MonsterModel monsterModel1 = flag ? (MonsterModel) ModelDb.Monster<LeafSlimeS>() : (MonsterModel) ModelDb.Monster<TwigSlimeS>();
    MonsterModel monsterModel2 = flag ? (MonsterModel) ModelDb.Monster<TwigSlimeS>() : (MonsterModel) ModelDb.Monster<LeafSlimeS>();
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<(MonsterModel, string)>) new \u003C\u003Ez__ReadOnlyArray<(MonsterModel, string)>(new (MonsterModel, string)[4]
    {
      (ModelDb.Monster<TwigSlimeM>().ToMutable(), (string) null),
      (ModelDb.Monster<LeafSlimeM>().ToMutable(), (string) null),
      (monsterModel1.ToMutable(), (string) null),
      (monsterModel2.ToMutable(), (string) null)
    });
  }
}
