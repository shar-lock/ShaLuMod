// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Encounters.OvicopterNormal
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Encounters;

public sealed class OvicopterNormal : EncounterModel
{
  private const string _ovicopterSlot = "ovicopter";
  private const string _eggSlotPrefix = "egg";

  public override bool HasScene => true;

  public override IReadOnlyList<string> Slots
  {
    get
    {
      return (IReadOnlyList<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[6]
      {
        "egg1",
        "egg2",
        "egg3",
        "egg4",
        "egg5",
        "ovicopter"
      });
    }
  }

  public override float GetCameraScaling() => 0.8f;

  public override Vector2 GetCameraOffset()
  {
    return Vector2.op_Addition(Vector2.op_Multiply(Vector2.Down, 50f), Vector2.op_Multiply(Vector2.Left, 100f));
  }

  public override RoomType RoomType => RoomType.Monster;

  public override IEnumerable<MonsterModel> AllPossibleMonsters
  {
    get
    {
      return (IEnumerable<MonsterModel>) new \u003C\u003Ez__ReadOnlyArray<MonsterModel>(new MonsterModel[2]
      {
        (MonsterModel) ModelDb.Monster<Ovicopter>(),
        (MonsterModel) ModelDb.Monster<ToughEgg>()
      });
    }
  }

  protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters()
  {
    return (IReadOnlyList<(MonsterModel, string)>) new List<(MonsterModel, string)>()
    {
      (ModelDb.Monster<Ovicopter>().ToMutable(), "ovicopter")
    };
  }
}
