// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Runs.Metrics;

public struct EncounterMetric(string id, int damage, int turns)
{
  public readonly string id = id;
  public readonly int damage = Math.Clamp(damage, 0, 100);
  public readonly int turns = turns;
}
