// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Encounters.FakeMerchantEventEncounter
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Encounters;

public sealed class FakeMerchantEventEncounter : EncounterModel
{
  private const string _merchantSlot = "merchant";

  public override RoomType RoomType => RoomType.Monster;

  public override bool HasScene => true;

  protected override bool HasCustomBackground => true;

  public override IReadOnlyList<string> Slots
  {
    get => (IReadOnlyList<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>("merchant");
  }

  public override IEnumerable<MonsterModel> AllPossibleMonsters
  {
    get
    {
      return (IEnumerable<MonsterModel>) new \u003C\u003Ez__ReadOnlySingleElementList<MonsterModel>((MonsterModel) ModelDb.Monster<FakeMerchantMonster>());
    }
  }

  public override int MinGoldReward => 300;

  public override int MaxGoldReward => 300;

  protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<(MonsterModel, string)>) new \u003C\u003Ez__ReadOnlySingleElementList<(MonsterModel, string)>((ModelDb.Monster<FakeMerchantMonster>().ToMutable(), "merchant"));
  }
}
