// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Encounters.TwoTailedRatsNormal
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Encounters;

public sealed class TwoTailedRatsNormal : EncounterModel
{
  public override bool HasScene => true;

  public override IReadOnlyList<string> Slots
  {
    get
    {
      return (IReadOnlyList<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[5]
      {
        "first",
        "second",
        "third",
        "fourth",
        "fifth"
      });
    }
  }

  public override RoomType RoomType => RoomType.Monster;

  public override float GetCameraScaling() => 0.85f;

  public override Vector2 GetCameraOffset() => Vector2.op_Multiply(Vector2.Down, 25f);

  public override IEnumerable<MonsterModel> AllPossibleMonsters
  {
    get
    {
      return (IEnumerable<MonsterModel>) new \u003C\u003Ez__ReadOnlySingleElementList<MonsterModel>((MonsterModel) ModelDb.Monster<TwoTailedRat>());
    }
  }

  protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters()
  {
    TwoTailedRat mutable1 = (TwoTailedRat) ModelDb.Monster<TwoTailedRat>().ToMutable();
    TwoTailedRat mutable2 = (TwoTailedRat) ModelDb.Monster<TwoTailedRat>().ToMutable();
    TwoTailedRat mutable3 = (TwoTailedRat) ModelDb.Monster<TwoTailedRat>().ToMutable();
    int num = this.Rng.NextInt(3);
    mutable1.StarterMoveIndex = num;
    mutable2.StarterMoveIndex = (num + 1) % 3;
    mutable3.StarterMoveIndex = (num + 2) % 3;
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<(MonsterModel, string)>) new \u003C\u003Ez__ReadOnlyArray<(MonsterModel, string)>(new (MonsterModel, string)[3]
    {
      ((MonsterModel) mutable1, this.Slots[2]),
      ((MonsterModel) mutable2, this.Slots[3]),
      ((MonsterModel) mutable3, this.Slots[4])
    });
  }
}
