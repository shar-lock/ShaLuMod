// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Rooms.RoomSet
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Rooms;

public class RoomSet
{
  public readonly List<EventModel> events = new List<EventModel>();
  public int eventsVisited;
  public readonly List<EncounterModel> normalEncounters = new List<EncounterModel>();
  public int normalEncountersVisited;
  public readonly List<EncounterModel> eliteEncounters = new List<EncounterModel>();
  public int eliteEncountersVisited;
  public int bossEncountersVisited;
  private AncientEventModel? _ancient;
  private EncounterModel? _boss;

  public bool HasAncient => this._ancient != null;

  public bool HasSecondBoss => this.SecondBoss != null;

  public AncientEventModel Ancient
  {
    get
    {
      return this._ancient ?? throw new InvalidOperationException("RoomSet.Ancient not set! You must call GenerateRooms");
    }
    set => this._ancient = value;
  }

  public EncounterModel Boss
  {
    get
    {
      return this._boss ?? throw new InvalidOperationException("RoomSet.Boss not set! You must call GenerateRooms");
    }
    set => this._boss = value;
  }

  public EncounterModel? SecondBoss { get; set; }

  public void MarkVisited(RoomType roomType)
  {
    switch (roomType)
    {
      case RoomType.Monster:
        ++this.normalEncountersVisited;
        break;
      case RoomType.Elite:
        ++this.eliteEncountersVisited;
        break;
      case RoomType.Boss:
        ++this.bossEncountersVisited;
        break;
      case RoomType.Event:
        ++this.eventsVisited;
        break;
    }
  }

  public EventModel NextEvent => this.events[this.eventsVisited % this.events.Count];

  public EncounterModel NextNormalEncounter
  {
    get => this.normalEncounters[this.normalEncountersVisited % this.normalEncounters.Count];
  }

  public EncounterModel NextEliteEncounter
  {
    get => this.eliteEncounters[this.eliteEncountersVisited % this.eliteEncounters.Count];
  }

  public EncounterModel NextBossEncounter
  {
    get => this.bossEncountersVisited != 0 && this.SecondBoss != null ? this.SecondBoss : this.Boss;
  }

  public void EnsureNextEventIsValid(RunState runState)
  {
    if (this.events.Count == 0)
      return;
    for (int index = 0; index < this.events.Count; ++index)
    {
      if (this.NextEvent.IsAllowed((IRunState) runState) && !runState.VisitedEventIds.Contains(this.NextEvent.Id))
        return;
      ++this.eventsVisited;
    }
    Log.Warn("All unique events exhausted, allowing repetition");
  }

  public SerializableRoomSet ToSave()
  {
    return new SerializableRoomSet()
    {
      EventIds = this.events.Select<EventModel, ModelId>((Func<EventModel, ModelId>) (e => e.Id)).ToList<ModelId>(),
      EventsVisited = this.eventsVisited,
      NormalEncounterIds = this.normalEncounters.Select<EncounterModel, ModelId>((Func<EncounterModel, ModelId>) (e => e.Id)).ToList<ModelId>(),
      NormalEncountersVisited = this.normalEncountersVisited,
      EliteEncounterIds = this.eliteEncounters.Select<EncounterModel, ModelId>((Func<EncounterModel, ModelId>) (e => e.Id)).ToList<ModelId>(),
      EliteEncountersVisited = this.eliteEncountersVisited,
      BossEncountersVisited = this.bossEncountersVisited,
      BossId = this._boss?.Id,
      SecondBossId = this.SecondBoss?.Id,
      AncientId = this._ancient?.Id
    };
  }

  public static RoomSet FromSave(SerializableRoomSet save)
  {
    RoomSet roomSet = new RoomSet();
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    roomSet.events.AddRange(save.EventIds.Select<ModelId, EventModel>(RoomSet.\u003C\u003EO.\u003C0\u003E__EventOrDeprecated ?? (RoomSet.\u003C\u003EO.\u003C0\u003E__EventOrDeprecated = new Func<ModelId, EventModel>(SaveUtil.EventOrDeprecated))).Where<EventModel>((Func<EventModel, bool>) (e => !(e is DeprecatedEvent))));
    roomSet.eventsVisited = save.EventsVisited;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    roomSet.normalEncounters.AddRange(save.NormalEncounterIds.Select<ModelId, EncounterModel>(RoomSet.\u003C\u003EO.\u003C1\u003E__EncounterOrDeprecated ?? (RoomSet.\u003C\u003EO.\u003C1\u003E__EncounterOrDeprecated = new Func<ModelId, EncounterModel>(SaveUtil.EncounterOrDeprecated))).Where<EncounterModel>((Func<EncounterModel, bool>) (e => !(e is DeprecatedEncounter) && e.RoomType == RoomType.Monster)));
    roomSet.normalEncountersVisited = save.NormalEncountersVisited;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    roomSet.eliteEncounters.AddRange(save.EliteEncounterIds.Select<ModelId, EncounterModel>(RoomSet.\u003C\u003EO.\u003C1\u003E__EncounterOrDeprecated ?? (RoomSet.\u003C\u003EO.\u003C1\u003E__EncounterOrDeprecated = new Func<ModelId, EncounterModel>(SaveUtil.EncounterOrDeprecated))).Where<EncounterModel>((Func<EncounterModel, bool>) (e => !(e is DeprecatedEncounter) && e.RoomType == RoomType.Elite)));
    roomSet.eliteEncountersVisited = save.EliteEncountersVisited;
    roomSet.bossEncountersVisited = save.BossEncountersVisited;
    roomSet._boss = save.BossId != (ModelId) null ? SaveUtil.EncounterOrDeprecated(save.BossId) : (EncounterModel) null;
    roomSet.SecondBoss = save.SecondBossId != (ModelId) null ? SaveUtil.EncounterOrDeprecated(save.SecondBossId) : (EncounterModel) null;
    roomSet._ancient = save.AncientId != (ModelId) null ? SaveUtil.AncientEventOrDeprecated(save.AncientId) : (AncientEventModel) null;
    return roomSet;
  }

  public static void SwapToOrCreateAtIndex<TBaseModel, TSpecificModel>(
    List<TBaseModel> list,
    int desiredIndex)
    where TBaseModel : AbstractModel
    where TSpecificModel : TBaseModel
  {
    int index1 = list.FindIndex((Predicate<TBaseModel>) (elem => (object) elem is TSpecificModel));
    if (index1 >= 0)
    {
      List<TBaseModel> baseModelList1 = list;
      int index2 = desiredIndex;
      List<TBaseModel> baseModelList2 = list;
      int num = index1;
      TBaseModel baseModel1 = list[index1];
      TBaseModel baseModel2 = list[desiredIndex];
      baseModelList1[index2] = baseModel1;
      int index3 = num;
      TBaseModel baseModel3 = baseModel2;
      baseModelList2[index3] = baseModel3;
    }
    else
      list[desiredIndex] = ModelDb.GetById<TBaseModel>(ModelDb.GetId<TSpecificModel>());
  }
}
