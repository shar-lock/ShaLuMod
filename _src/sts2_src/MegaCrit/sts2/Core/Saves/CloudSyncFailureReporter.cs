// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.CloudSyncFailureReporter
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves;

public class CloudSyncFailureReporter
{
  public const int maxReportsPerSessionPerType = 5;
  private readonly Dictionary<Type, int> _reportedCountsByType = new Dictionary<Type, int>();

  public bool ShouldReport(Exception exception)
  {
    Type type = exception.GetType();
    int valueOrDefault = CollectionExtensions.GetValueOrDefault<Type, int>((IReadOnlyDictionary<Type, int>) this._reportedCountsByType, type);
    if (valueOrDefault >= 5)
      return false;
    this._reportedCountsByType[type] = valueOrDefault + 1;
    return true;
  }
}
