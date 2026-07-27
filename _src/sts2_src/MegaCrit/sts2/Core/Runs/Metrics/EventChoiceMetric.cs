// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Runs.Metrics;

public struct EventChoiceMetric
{
  public readonly string id;
  public readonly string act;
  public readonly string picked;

  public EventChoiceMetric(
    MapPointHistoryEntry entry,
    ulong playerId,
    SerializableActModel actModel)
  {
    this.id = entry.Rooms.First<MapPointRoomHistoryEntry>().ModelId.Entry;
    this.act = actModel.Id.Entry;
    string[] strArray = entry.GetEntry(playerId).EventChoices.Last<EventOptionHistoryEntry>().Title.LocEntryKey.Split(".", StringSplitOptions.None);
    this.picked = strArray[strArray.Length - 2];
  }
}
