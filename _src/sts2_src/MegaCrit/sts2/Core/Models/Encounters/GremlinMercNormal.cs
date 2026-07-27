// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Encounters.GremlinMercNormal
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Encounters;

public sealed class GremlinMercNormal : EncounterModel
{
  public const string mercSlot = "merc";
  public const string sneakySlot = "sneaky";
  public const string fatSlot = "fat";
  private bool _goldWasStolen;

  public override RoomType RoomType => RoomType.Monster;

  public override bool HasScene => true;

  public override IEnumerable<MonsterModel> AllPossibleMonsters
  {
    get
    {
      return (IEnumerable<MonsterModel>) new \u003C\u003Ez__ReadOnlyArray<MonsterModel>(new MonsterModel[3]
      {
        (MonsterModel) ModelDb.Monster<GremlinMerc>(),
        (MonsterModel) ModelDb.Monster<FatGremlin>(),
        (MonsterModel) ModelDb.Monster<SneakyGremlin>()
      });
    }
  }

  protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<(MonsterModel, string)>) new \u003C\u003Ez__ReadOnlySingleElementList<(MonsterModel, string)>((ModelDb.Monster<GremlinMerc>().ToMutable(), "merc"));
  }

  private bool GoldWasStolen
  {
    get => this._goldWasStolen;
    set
    {
      this.AssertMutable();
      this._goldWasStolen = value;
    }
  }

  public void MarkGoldStolen() => this.GoldWasStolen = true;

  public override float CalculateGoldProportion(CombatState combatState)
  {
    if (!combatState.EscapedCreatures.Any<Creature>((Func<Creature, bool>) (c => c.Monster is FatGremlin)))
      return 1f;
    return !this.GoldWasStolen ? 0.5f : 0.0f;
  }
}
