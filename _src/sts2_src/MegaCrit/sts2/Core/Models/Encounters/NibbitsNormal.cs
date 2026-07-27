// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Encounters.NibbitsNormal
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Encounters;

public sealed class NibbitsNormal : EncounterModel
{
  private const string _backSlot = "back";
  private const string _frontSlot = "front";

  public override IReadOnlyList<string> Slots
  {
    get
    {
      return (IReadOnlyList<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[2]
      {
        "front",
        "back"
      });
    }
  }

  public override RoomType RoomType => RoomType.Monster;

  public override bool HasScene => true;

  public override IEnumerable<MonsterModel> AllPossibleMonsters
  {
    get
    {
      return (IEnumerable<MonsterModel>) new \u003C\u003Ez__ReadOnlySingleElementList<MonsterModel>((MonsterModel) ModelDb.Monster<Nibbit>());
    }
  }

  protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters()
  {
    Nibbit mutable = (Nibbit) ModelDb.Monster<Nibbit>().ToMutable();
    mutable.IsFront = true;
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<(MonsterModel, string)>) new \u003C\u003Ez__ReadOnlyArray<(MonsterModel, string)>(new (MonsterModel, string)[2]
    {
      ((MonsterModel) mutable, "front"),
      (ModelDb.Monster<Nibbit>().ToMutable(), "back")
    });
  }
}
