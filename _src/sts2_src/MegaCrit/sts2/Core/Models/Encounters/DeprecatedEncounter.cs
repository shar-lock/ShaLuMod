// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Encounters.DeprecatedEncounter
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Rooms;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Encounters;

public sealed class DeprecatedEncounter : EncounterModel
{
  public override RoomType RoomType => RoomType.Monster;

  public override IEnumerable<MonsterModel> AllPossibleMonsters
  {
    get => (IEnumerable<MonsterModel>) Array.Empty<MonsterModel>();
  }

  protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters()
  {
    return (IReadOnlyList<(MonsterModel, string)>) Array.Empty<(MonsterModel, string)>();
  }
}
