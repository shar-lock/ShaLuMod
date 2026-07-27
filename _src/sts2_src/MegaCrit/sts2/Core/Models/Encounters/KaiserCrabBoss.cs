// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Encounters.KaiserCrabBoss
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

public sealed class KaiserCrabBoss : EncounterModel
{
  public const string kaiserCrabCustomTrackName = "kaiser_crab_progress";
  private const string _crusherSlot = "crusher";
  private const string _rocketSlot = "rocket";

  public override string BossNodePath
  {
    get => $"res://images/map/placeholder/{this.Id.Entry.ToLowerInvariant()}_icon";
  }

  public override string CustomBgm => "event:/music/act2_boss_kaiser_crab";

  public override float GetCameraScaling() => 0.75f;

  public override Vector2 GetCameraOffset() => Vector2.op_Multiply(Vector2.Down, 35f);

  protected override bool HasCustomBackground => true;

  public override MegaSkeletonDataResource? BossNodeSpineResource
  {
    get => (MegaSkeletonDataResource) null;
  }

  public override RoomType RoomType => RoomType.Boss;

  public override bool FullyCenterPlayers => true;

  public override IReadOnlyList<string> Slots
  {
    get
    {
      return (IReadOnlyList<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[2]
      {
        "crusher",
        "rocket"
      });
    }
  }

  public override bool HasScene => true;

  public override IEnumerable<MonsterModel> AllPossibleMonsters
  {
    get
    {
      return (IEnumerable<MonsterModel>) new \u003C\u003Ez__ReadOnlyArray<MonsterModel>(new MonsterModel[2]
      {
        (MonsterModel) ModelDb.Monster<Crusher>(),
        (MonsterModel) ModelDb.Monster<Rocket>()
      });
    }
  }

  protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<(MonsterModel, string)>) new \u003C\u003Ez__ReadOnlyArray<(MonsterModel, string)>(new (MonsterModel, string)[2]
    {
      (ModelDb.Monster<Crusher>().ToMutable(), "crusher"),
      (ModelDb.Monster<Rocket>().ToMutable(), "rocket")
    });
  }
}
