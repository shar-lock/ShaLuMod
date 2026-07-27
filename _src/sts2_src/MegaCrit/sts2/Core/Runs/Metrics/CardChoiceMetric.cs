// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Runs.Metrics.CardChoiceMetric
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Runs.History;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Runs.Metrics;

public struct CardChoiceMetric
{
  public readonly List<string> picked;
  public readonly List<string> skipped;

  public CardChoiceMetric(List<CardChoiceHistoryEntry> choices)
  {
    this.picked = new List<string>();
    this.skipped = new List<string>();
    foreach (CardChoiceHistoryEntry choice in choices)
    {
      if (choice.wasPicked)
        this.picked.Add(choice.Card.Id.Entry);
      else
        this.skipped.Add(choice.Card.Id.Entry);
    }
  }
}
