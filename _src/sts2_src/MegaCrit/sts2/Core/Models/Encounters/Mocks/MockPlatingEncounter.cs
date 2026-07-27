// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Encounters.Mocks.MockPlatingEncounter
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models.Monsters.Mocks;
using MegaCrit.Sts2.Core.Rooms;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Encounters.Mocks;

public sealed class MockPlatingEncounter : EncounterModel
{
  private int _platingAmount = 1;

  public override bool IsMock => true;

  public override RoomType RoomType => RoomType.Monster;

  public int PlatingAmount
  {
    get => this._platingAmount;
    set
    {
      this.AssertMutable();
      this._platingAmount = value;
    }
  }

  public override IEnumerable<MonsterModel> AllPossibleMonsters
  {
    get
    {
      return (IEnumerable<MonsterModel>) new \u003C\u003Ez__ReadOnlySingleElementList<MonsterModel>((MonsterModel) ModelDb.Monster<MockPlatingMonster>());
    }
  }

  protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters()
  {
    MonsterModel mutable = ModelDb.Monster<MockPlatingMonster>().ToMutable();
    ((MockPlatingMonster) mutable).PlatingAmount = this.PlatingAmount;
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<(MonsterModel, string)>) new \u003C\u003Ez__ReadOnlySingleElementList<(MonsterModel, string)>((mutable, (string) null));
  }
}
