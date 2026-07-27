// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Encounters.PunchOffEventEncounter
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Encounters;

public sealed class PunchOffEventEncounter : EncounterModel
{
  public override RoomType RoomType => RoomType.Monster;

  public override IEnumerable<MonsterModel> AllPossibleMonsters
  {
    get
    {
      return (IEnumerable<MonsterModel>) new \u003C\u003Ez__ReadOnlySingleElementList<MonsterModel>((MonsterModel) ModelDb.Monster<PunchConstruct>());
    }
  }

  protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters()
  {
    PunchConstruct mutable1 = (PunchConstruct) ModelDb.Monster<PunchConstruct>().ToMutable();
    mutable1.StartsWithFastPunch = true;
    mutable1.StartingHpReduction = this.Rng.NextInt(2, 10);
    PunchConstruct mutable2 = (PunchConstruct) ModelDb.Monster<PunchConstruct>().ToMutable();
    mutable2.StartingHpReduction = this.Rng.NextInt(2, 10);
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<(MonsterModel, string)>) new \u003C\u003Ez__ReadOnlyArray<(MonsterModel, string)>(new (MonsterModel, string)[2]
    {
      ((MonsterModel) mutable1, (string) null),
      ((MonsterModel) mutable2, (string) null)
    });
  }
}
