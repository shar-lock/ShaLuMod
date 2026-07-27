// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Bestiary.BestiaryEntry
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Rooms;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Bestiary;

public class BestiaryEntry
{
  public MonsterModel? monsterModel;
  public required EncounterModel encounterModel;
  public required RoomType roomType;

  public static BestiaryEntry FromMonster(
    MonsterModel monster,
    EncounterModel encounter,
    RoomType type)
  {
    return new BestiaryEntry()
    {
      monsterModel = monster,
      encounterModel = encounter,
      roomType = type
    };
  }

  public static BestiaryEntry FromEncounter(EncounterModel encounter, RoomType type)
  {
    return new BestiaryEntry()
    {
      encounterModel = encounter,
      roomType = type
    };
  }

  public string GetEncounterTitle() => this.encounterModel.Title.GetFormattedText();

  public string GetEntryTitle()
  {
    return this.monsterModel != null ? this.monsterModel.Title.GetFormattedText() : this.GetEncounterTitle();
  }

  public bool CanReuseLayout(NBestiaryLayout? layout)
  {
    if (this.encounterModel is KaiserCrabBoss)
      return layout is NBestiaryLayoutKaiserCrab;
    return this.encounterModel is DecimillipedeElite ? layout is NBestiaryLayoutDecimillipede : layout is NBestiaryLayoutDefault;
  }

  public NBestiaryLayout? CreateLayoutNode(NBestiary bestiary)
  {
    if (this.encounterModel is KaiserCrabBoss)
      return (NBestiaryLayout) NBestiaryLayoutKaiserCrab.Create();
    return this.encounterModel is DecimillipedeElite ? (NBestiaryLayout) NBestiaryLayoutDecimillipede.Create(bestiary) : (NBestiaryLayout) NBestiaryLayoutDefault.Create();
  }

  public bool IsDiscovered(
    HashSet<ModelId> discoveredMonsterIds,
    HashSet<ModelId> discoveredEncounterIds)
  {
    return this.monsterModel == null ? discoveredEncounterIds.Contains(this.encounterModel.Id) : discoveredMonsterIds.Contains(this.monsterModel.Id);
  }
}
