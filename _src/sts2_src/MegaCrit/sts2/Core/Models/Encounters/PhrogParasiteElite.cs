// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Encounters.PhrogParasiteElite
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Encounters;

public sealed class PhrogParasiteElite : EncounterModel
{
  private const string _wrigglerSlotPrefix = "wriggler";
  private const string _phrogSlot = "phrog";

  public override RoomType RoomType => RoomType.Elite;

  public override IReadOnlyList<string> Slots
  {
    get
    {
      return (IReadOnlyList<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[5]
      {
        "phrog",
        "wriggler1",
        "wriggler2",
        "wriggler3",
        "wriggler4"
      });
    }
  }

  public override float GetCameraScaling() => 0.95f;

  public override bool HasScene => true;

  public static string GetWrigglerSlotName(int index) => $"{"wriggler"}{index + 1}";

  public override IEnumerable<MonsterModel> AllPossibleMonsters
  {
    get
    {
      return (IEnumerable<MonsterModel>) new \u003C\u003Ez__ReadOnlyArray<MonsterModel>(new MonsterModel[2]
      {
        (MonsterModel) ModelDb.Monster<PhrogParasite>(),
        (MonsterModel) ModelDb.Monster<Wriggler>()
      });
    }
  }

  public override IEnumerable<string> ExtraAssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(ModelDb.Card<Infection>().OverlayPath);
    }
  }

  protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<(MonsterModel, string)>) new \u003C\u003Ez__ReadOnlySingleElementList<(MonsterModel, string)>((ModelDb.Monster<PhrogParasite>().ToMutable(), "phrog"));
  }
}
