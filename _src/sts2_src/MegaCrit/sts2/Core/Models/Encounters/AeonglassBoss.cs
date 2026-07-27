// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Encounters.AeonglassBoss
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

public sealed class AeonglassBoss : EncounterModel
{
  public override RoomType RoomType => RoomType.Boss;

  public override MegaSkeletonDataResource? BossNodeSpineResource
  {
    get => (MegaSkeletonDataResource) null;
  }

  public override string BossNodePath
  {
    get => $"res://images/map/placeholder/{this.Id.Entry.ToLowerInvariant()}_icon";
  }

  public override string CustomBgm => "event:/music/act3_boss_queen";

  public override float GetCameraScaling() => 0.9f;

  public override Vector2 GetCameraOffset() => Vector2.op_Multiply(Vector2.Down, 60f);

  protected override bool HasCustomBackground => false;

  public override IEnumerable<MonsterModel> AllPossibleMonsters
  {
    get
    {
      return (IEnumerable<MonsterModel>) new \u003C\u003Ez__ReadOnlySingleElementList<MonsterModel>((MonsterModel) ModelDb.Monster<Aeonglass>());
    }
  }

  protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<(MonsterModel, string)>) new \u003C\u003Ez__ReadOnlySingleElementList<(MonsterModel, string)>((ModelDb.Monster<Aeonglass>().ToMutable(), (string) null));
  }
}
