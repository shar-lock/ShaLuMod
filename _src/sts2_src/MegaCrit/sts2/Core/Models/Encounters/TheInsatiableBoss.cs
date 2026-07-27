// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Encounters.TheInsatiableBoss
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Encounters;

public sealed class TheInsatiableBoss : EncounterModel
{
  public override string CustomBgm => "event:/music/act2_boss_the_insatiable";

  public override RoomType RoomType => RoomType.Boss;

  public override float GetCameraScaling() => 0.9f;

  protected override bool HasCustomBackground => true;

  public override string AmbientSfx => "event:/sfx/ambience/act2_ambience_the_insatiable";

  public override IEnumerable<MonsterModel> AllPossibleMonsters
  {
    get
    {
      return (IEnumerable<MonsterModel>) new \u003C\u003Ez__ReadOnlySingleElementList<MonsterModel>((MonsterModel) ModelDb.Monster<TheInsatiable>());
    }
  }

  protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<(MonsterModel, string)>) new \u003C\u003Ez__ReadOnlySingleElementList<(MonsterModel, string)>((ModelDb.Monster<TheInsatiable>().ToMutable(), (string) null));
  }
}
