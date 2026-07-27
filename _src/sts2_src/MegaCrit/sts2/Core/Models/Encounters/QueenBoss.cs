// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Encounters.QueenBoss
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Models.Afflictions;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Encounters;

public sealed class QueenBoss : EncounterModel
{
  private const string _queenSlot = "queen";
  private const string _amalgamSlot = "amalgam";

  public override RoomType RoomType => RoomType.Boss;

  public override IReadOnlyList<string> Slots
  {
    get
    {
      return (IReadOnlyList<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[2]
      {
        "amalgam",
        "queen"
      });
    }
  }

  public override string CustomBgm => "event:/music/act3_boss_queen";

  public override float GetCameraScaling() => 0.9f;

  public override Vector2 GetCameraOffset() => Vector2.op_Multiply(Vector2.Down, 60f);

  protected override bool HasCustomBackground => true;

  public override bool HasScene => true;

  public override IEnumerable<MonsterModel> AllPossibleMonsters
  {
    get
    {
      return (IEnumerable<MonsterModel>) new \u003C\u003Ez__ReadOnlyArray<MonsterModel>(new MonsterModel[2]
      {
        (MonsterModel) ModelDb.Monster<Queen>(),
        (MonsterModel) ModelDb.Monster<TorchHeadAmalgam>()
      });
    }
  }

  public override IEnumerable<string> ExtraAssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(ModelDb.Affliction<Bound>().OverlayPath);
    }
  }

  protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<(MonsterModel, string)>) new \u003C\u003Ez__ReadOnlyArray<(MonsterModel, string)>(new (MonsterModel, string)[2]
    {
      (ModelDb.Monster<TorchHeadAmalgam>().ToMutable(), "amalgam"),
      (ModelDb.Monster<Queen>().ToMutable(), "queen")
    });
  }
}
