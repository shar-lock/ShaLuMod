// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Encounters.LivingFogNormal
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models.Afflictions;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Encounters;

public sealed class LivingFogNormal : EncounterModel
{
  private const string _livingFogSlot = "livingFog";
  private const string _bombSlotPrefix = "bomb";

  public override RoomType RoomType => RoomType.Monster;

  public override bool HasScene => true;

  public override IReadOnlyList<string> Slots
  {
    get
    {
      return (IReadOnlyList<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[6]
      {
        "bomb1",
        "bomb2",
        "bomb3",
        "bomb4",
        "bomb5",
        "livingFog"
      });
    }
  }

  public override float GetCameraScaling() => 0.9f;

  public override IEnumerable<MonsterModel> AllPossibleMonsters
  {
    get
    {
      return (IEnumerable<MonsterModel>) new \u003C\u003Ez__ReadOnlyArray<MonsterModel>(new MonsterModel[2]
      {
        (MonsterModel) ModelDb.Monster<LivingFog>(),
        (MonsterModel) ModelDb.Monster<GasBomb>()
      });
    }
  }

  public override IEnumerable<string> ExtraAssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(ModelDb.Affliction<Smog>().OverlayPath);
    }
  }

  protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<(MonsterModel, string)>) new \u003C\u003Ez__ReadOnlySingleElementList<(MonsterModel, string)>((ModelDb.Monster<LivingFog>().ToMutable(), "livingFog"));
  }
}
