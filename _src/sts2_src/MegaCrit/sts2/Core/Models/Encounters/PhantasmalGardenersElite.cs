// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Encounters.PhantasmalGardenersElite
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Encounters;

public sealed class PhantasmalGardenersElite : EncounterModel
{
  public const string firstSlot = "first";
  public const string secondSlot = "second";
  public const string thirdSlot = "third";
  public const string fourthSlot = "fourth";

  public override IReadOnlyList<string> Slots
  {
    get
    {
      return (IReadOnlyList<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[4]
      {
        "first",
        "second",
        "third",
        "fourth"
      });
    }
  }

  public override RoomType RoomType => RoomType.Elite;

  public override bool HasScene => true;

  public override float GetCameraScaling() => 0.85f;

  public override Vector2 GetCameraOffset() => Vector2.op_Multiply(Vector2.Down, 40f);

  public override IEnumerable<MonsterModel> AllPossibleMonsters
  {
    get
    {
      return (IEnumerable<MonsterModel>) new \u003C\u003Ez__ReadOnlySingleElementList<MonsterModel>((MonsterModel) ModelDb.Monster<PhantasmalGardener>());
    }
  }

  protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<(MonsterModel, string)>) new \u003C\u003Ez__ReadOnlyArray<(MonsterModel, string)>(new (MonsterModel, string)[4]
    {
      (ModelDb.Monster<PhantasmalGardener>().ToMutable(), "first"),
      (ModelDb.Monster<PhantasmalGardener>().ToMutable(), "second"),
      (ModelDb.Monster<PhantasmalGardener>().ToMutable(), "third"),
      (ModelDb.Monster<PhantasmalGardener>().ToMutable(), "fourth")
    });
  }
}
