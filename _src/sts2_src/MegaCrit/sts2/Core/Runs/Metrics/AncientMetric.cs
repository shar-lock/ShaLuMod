// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Runs.Metrics.AncientMetric
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Runs.History;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Runs.Metrics;

public struct AncientMetric
{
  public readonly string picked;
  public readonly List<string> skipped;

  public AncientMetric(MapPointHistoryEntry entry, PlayerMapPointHistoryEntry playerEntry)
  {
    this.picked = (playerEntry.AncientChoices.FirstOrDefault<AncientChoiceHistoryEntry>((Func<AncientChoiceHistoryEntry, bool>) (o => o.WasChosen)) ?? throw new InvalidOperationException($"Failed to find chosen ancient choice! {entry.Rooms.First<MapPointRoomHistoryEntry>().ModelId} {playerEntry.PlayerId}")).TextKey;
    this.skipped = playerEntry.AncientChoices.Where<AncientChoiceHistoryEntry>((Func<AncientChoiceHistoryEntry, bool>) (o => !o.WasChosen)).Select<AncientChoiceHistoryEntry, string>((Func<AncientChoiceHistoryEntry, string>) (o => o.TextKey)).ToList<string>();
  }
}
