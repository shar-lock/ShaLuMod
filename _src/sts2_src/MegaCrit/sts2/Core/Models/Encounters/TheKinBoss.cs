// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Encounters.TheKinBoss
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Encounters;

public sealed class TheKinBoss : EncounterModel
{
  public override RoomType RoomType => RoomType.Boss;

  public override bool HasScene => true;

  protected override bool HasCustomBackground => true;

  public override float GetCameraScaling() => 0.85f;

  public override Vector2 GetCameraOffset() => Vector2.op_Multiply(Vector2.Down, 50f);

  public override string CustomBgm => "event:/music/act1_boss_the_kin";

  public override string BossNodePath
  {
    get => $"res://images/map/placeholder/{this.Id.Entry.ToLowerInvariant()}_icon";
  }

  public override MegaSkeletonDataResource? BossNodeSpineResource
  {
    get => (MegaSkeletonDataResource) null;
  }

  public override IReadOnlyList<string> Slots
  {
    get
    {
      return (IReadOnlyList<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[3]
      {
        "slot1",
        "slot2",
        "leaderSlot"
      });
    }
  }

  public override IEnumerable<MonsterModel> AllPossibleMonsters
  {
    get
    {
      return (IEnumerable<MonsterModel>) new \u003C\u003Ez__ReadOnlyArray<MonsterModel>(new MonsterModel[2]
      {
        (MonsterModel) ModelDb.Monster<KinFollower>(),
        (MonsterModel) ModelDb.Monster<KinPriest>()
      });
    }
  }

  protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters()
  {
    KinFollower mutable = (KinFollower) ModelDb.Monster<KinFollower>().ToMutable();
    mutable.StartsWithDance = true;
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<(MonsterModel, string)>) new \u003C\u003Ez__ReadOnlyArray<(MonsterModel, string)>(new (MonsterModel, string)[3]
    {
      ((MonsterModel) mutable, "slot1"),
      (ModelDb.Monster<KinFollower>().ToMutable(), "slot2"),
      (ModelDb.Monster<KinPriest>().ToMutable(), "leaderSlot")
    });
  }
}
