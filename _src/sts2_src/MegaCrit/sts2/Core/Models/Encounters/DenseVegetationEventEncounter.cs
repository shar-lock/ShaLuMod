// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Encounters.DenseVegetationEventEncounter
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Encounters;

public sealed class DenseVegetationEventEncounter : EncounterModel
{
  private const string _wrigglerSlotPrefix = "wriggler";

  public override RoomType RoomType => RoomType.Monster;

  public override IReadOnlyList<string> Slots
  {
    get
    {
      return (IReadOnlyList<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[4]
      {
        "wriggler1",
        "wriggler2",
        "wriggler3",
        "wriggler4"
      });
    }
  }

  public override bool HasScene => true;

  public override IEnumerable<MonsterModel> AllPossibleMonsters
  {
    get
    {
      return (IEnumerable<MonsterModel>) new \u003C\u003Ez__ReadOnlySingleElementList<MonsterModel>((MonsterModel) ModelDb.Monster<Wriggler>());
    }
  }

  protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters()
  {
    List<(MonsterModel, string)> monsters = new List<(MonsterModel, string)>();
    foreach (string slot in (IEnumerable<string>) this.Slots)
    {
      Wriggler mutable = (Wriggler) ModelDb.Monster<Wriggler>().ToMutable();
      mutable.StartStunned = false;
      monsters.Add(((MonsterModel) mutable, slot));
    }
    return (IReadOnlyList<(MonsterModel, string)>) monsters;
  }
}
