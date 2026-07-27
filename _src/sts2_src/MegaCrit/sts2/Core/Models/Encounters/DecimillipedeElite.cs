// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Encounters.DecimillipedeElite
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Encounters;

public sealed class DecimillipedeElite : EncounterModel
{
  private const string _segmentSlot = "segment";

  public override bool HasScene => true;

  public override IReadOnlyList<string> Slots
  {
    get
    {
      return (IReadOnlyList<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[3]
      {
        "segment1",
        "segment2",
        "segment3"
      });
    }
  }

  public override float GetCameraScaling() => 0.87f;

  public override Vector2 GetCameraOffset() => Vector2.op_Multiply(Vector2.Down, 50f);

  public override RoomType RoomType => RoomType.Elite;

  public override IEnumerable<MonsterModel> AllPossibleMonsters
  {
    get
    {
      return (IEnumerable<MonsterModel>) new \u003C\u003Ez__ReadOnlyArray<MonsterModel>(new MonsterModel[3]
      {
        (MonsterModel) ModelDb.Monster<DecimillipedeSegmentFront>(),
        (MonsterModel) ModelDb.Monster<DecimillipedeSegmentMiddle>(),
        (MonsterModel) ModelDb.Monster<DecimillipedeSegmentBack>()
      });
    }
  }

  protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters()
  {
    DecimillipedeSegment mutable1 = (DecimillipedeSegment) ModelDb.Monster<DecimillipedeSegmentFront>().ToMutable();
    DecimillipedeSegment mutable2 = (DecimillipedeSegment) ModelDb.Monster<DecimillipedeSegmentMiddle>().ToMutable();
    DecimillipedeSegment mutable3 = (DecimillipedeSegment) ModelDb.Monster<DecimillipedeSegmentBack>().ToMutable();
    int num = this.Rng.NextInt(3);
    mutable1.StarterMoveIdx = num;
    mutable2.StarterMoveIdx = (num + 1) % 3;
    mutable3.StarterMoveIdx = (num + 2) % 3;
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<(MonsterModel, string)>) new \u003C\u003Ez__ReadOnlyArray<(MonsterModel, string)>(new (MonsterModel, string)[3]
    {
      ((MonsterModel) mutable1, "segment1"),
      ((MonsterModel) mutable2, "segment2"),
      ((MonsterModel) mutable3, "segment3")
    });
  }
}
