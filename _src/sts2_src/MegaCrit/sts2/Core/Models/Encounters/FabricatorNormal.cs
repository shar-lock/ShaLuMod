// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Encounters.FabricatorNormal
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Rooms;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Encounters;

public sealed class FabricatorNormal : EncounterModel
{
  private const string _fabricatorSlot = "fabricator";
  private const string _botSlotPrefix = "bot";

  public override RoomType RoomType => RoomType.Monster;

  public override bool HasScene => true;

  public override IReadOnlyList<string> Slots
  {
    get
    {
      return (IReadOnlyList<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[5]
      {
        "bot1",
        "bot2",
        "fabricator",
        "bot3",
        "bot4"
      });
    }
  }

  public override IEnumerable<MonsterModel> AllPossibleMonsters
  {
    get
    {
      MonsterModel monsterModel1 = (MonsterModel) ModelDb.Monster<Fabricator>();
      HashSet<MonsterModel> defenseSpawns = Fabricator.defenseSpawns;
      HashSet<MonsterModel> aggroSpawns = Fabricator.aggroSpawns;
      int index1 = 0;
      MonsterModel[] items = new MonsterModel[1 + (defenseSpawns.Count + aggroSpawns.Count)];
      items[index1] = monsterModel1;
      int index2 = index1 + 1;
      foreach (MonsterModel monsterModel2 in defenseSpawns)
      {
        items[index2] = monsterModel2;
        ++index2;
      }
      foreach (MonsterModel monsterModel3 in aggroSpawns)
      {
        items[index2] = monsterModel3;
        ++index2;
      }
      return (IEnumerable<MonsterModel>) new \u003C\u003Ez__ReadOnlyArray<MonsterModel>(items);
    }
  }

  protected override IReadOnlyList<(MonsterModel, string?)> GenerateMonsters()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IReadOnlyList<(MonsterModel, string)>) new \u003C\u003Ez__ReadOnlySingleElementList<(MonsterModel, string)>((ModelDb.Monster<Fabricator>().ToMutable(), "fabricator"));
  }

  public override float GetCameraScaling() => 0.85f;

  public override Vector2 GetCameraOffset() => Vector2.op_Multiply(Vector2.Down, 60f);

  public static void SetBotFallPosition(NCreature creatureNode)
  {
    Node2D specialNode = creatureNode.GetSpecialNode<Node2D>("Visuals/FallControl");
    if (specialNode == null)
      return;
    float num = 125f;
    specialNode.Position = Vector2.op_Multiply(Vector2.Down, (num - creatureNode.Position.Y) / creatureNode.Visuals.GetCurrentBody().Scale.Y);
  }
}
