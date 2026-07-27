// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Encounters.ToadpolesWeak
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Encounters;

public sealed class ToadpolesWeak : EncounterModel
{
  public override RoomType RoomType => RoomType.Monster;

  public override bool IsWeak => true;

  public override IEnumerable<MonsterModel> AllPossibleMonsters
  {
    get
    {
      return (IEnumerable<MonsterModel>) new \u003C\u003Ez__ReadOnlySingleElementList<MonsterModel>((MonsterModel) ModelDb.Monster<Toadpole>());
    }
  }

  protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters()
  {
    Toadpole mutable1 = (Toadpole) ModelDb.Monster<Toadpole>().ToMutable();
    mutable1.IsFront = true;
    Toadpole mutable2 = (Toadpole) ModelDb.Monster<Toadpole>().ToMutable();
    mutable2.IsFront = false;
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<(MonsterModel, string)>) new \u003C\u003Ez__ReadOnlyArray<(MonsterModel, string)>(new (MonsterModel, string)[2]
    {
      ((MonsterModel) mutable1, (string) null),
      ((MonsterModel) mutable2, (string) null)
    });
  }
}
