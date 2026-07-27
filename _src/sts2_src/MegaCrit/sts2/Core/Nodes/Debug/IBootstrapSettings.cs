// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Debug.IBootstrapSettings
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.SourceGeneration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Debug;

[GenerateSubtypes]
public interface IBootstrapSettings
{
  CharacterModel Character { get; }

  RoomType RoomType { get; }

  EncounterModel Encounter { get; }

  EventModel Event { get; }

  ActModel Act { get; }

  int Ascension { get; }

  bool SaveRunHistory { get; }

  string? Seed { get; }

  bool DoPreloading { get; }

  bool BootstrapInMultiplayer { get; }

  List<ModifierModel> Modifiers { get; }

  string? Language => (string) null;

  int? ReplayPlayerIndex => new int?();

  Task Setup(Player localPlayer);

  MapPointType MapPointType
  {
    get
    {
      RoomType roomType = this.RoomType;
      MapPointType mapPointType;
      switch (roomType)
      {
        case RoomType.Unassigned:
          throw new ArgumentOutOfRangeException();
        case RoomType.Monster:
          mapPointType = MapPointType.Monster;
          break;
        case RoomType.Elite:
          mapPointType = MapPointType.Elite;
          break;
        case RoomType.Boss:
          mapPointType = MapPointType.Boss;
          break;
        case RoomType.Treasure:
          mapPointType = MapPointType.Treasure;
          break;
        case RoomType.Shop:
          mapPointType = MapPointType.Shop;
          break;
        case RoomType.Event:
          mapPointType = MapPointType.Unknown;
          break;
        case RoomType.RestSite:
          mapPointType = MapPointType.RestSite;
          break;
        case RoomType.Map:
          mapPointType = MapPointType.Unknown;
          break;
        default:
          \u003CPrivateImplementationDetails\u003E.ThrowSwitchExpressionException((object) roomType);
          break;
      }
      return mapPointType;
    }
  }
}
