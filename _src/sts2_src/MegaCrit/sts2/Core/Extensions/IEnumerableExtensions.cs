// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Extensions.IEnumerableExtensions
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Random;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Extensions;

public static class IEnumerableExtensions
{
  public static IEnumerable<T> TakeRandom<T>(this IEnumerable<T> collection, int count, Rng rng)
  {
    return collection.ToList<T>().UnstableShuffle<T>(rng).Take<T>(count);
  }
}
