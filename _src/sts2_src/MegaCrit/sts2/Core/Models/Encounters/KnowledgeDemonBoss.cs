// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Encounters.KnowledgeDemonBoss
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

public sealed class KnowledgeDemonBoss : EncounterModel
{
  public override RoomType RoomType => RoomType.Boss;

  public override float GetCameraScaling() => 0.85f;

  public override Vector2 GetCameraOffset() => Vector2.op_Multiply(Vector2.Down, 70f);

  public override string CustomBgm => "event:/music/act2_boss_knowledge_demon";

  public override string BossNodePath
  {
    get => $"res://images/map/placeholder/{this.Id.Entry.ToLowerInvariant()}_icon";
  }

  public override MegaSkeletonDataResource? BossNodeSpineResource
  {
    get => (MegaSkeletonDataResource) null;
  }

  public override IEnumerable<MonsterModel> AllPossibleMonsters
  {
    get
    {
      return (IEnumerable<MonsterModel>) new \u003C\u003Ez__ReadOnlySingleElementList<MonsterModel>((MonsterModel) ModelDb.Monster<KnowledgeDemon>());
    }
  }

  protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<(MonsterModel, string)>) new \u003C\u003Ez__ReadOnlySingleElementList<(MonsterModel, string)>((ModelDb.Monster<KnowledgeDemon>().ToMutable(), (string) null));
  }

  protected override bool HasCustomBackground => true;
}
