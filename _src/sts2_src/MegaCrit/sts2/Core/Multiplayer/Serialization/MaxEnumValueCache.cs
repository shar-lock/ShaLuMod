// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Serialization.MaxEnumValueCache
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Serialization;

public static class MaxEnumValueCache
{
  private static readonly Dictionary<Type, int> _maxEnumValues = new Dictionary<Type, int>();

  public static int Get<T>() where T : struct, Enum
  {
    int num;
    if (!MaxEnumValueCache._maxEnumValues.TryGetValue(typeof (T), out num))
    {
      if (!typeof (int).IsAssignableFrom(Enum.GetUnderlyingType(typeof (T))))
        throw new InvalidOperationException($"Trying to get max value for enum type {typeof (T)} that is not assignable to int!");
      num = ((IEnumerable<T>) Enum.GetValues<T>()).Select<T, int>((Func<T, int>) (v => Convert.ToInt32((object) v))).Max();
      MaxEnumValueCache._maxEnumValues[typeof (T)] = num;
    }
    return num;
  }
}
