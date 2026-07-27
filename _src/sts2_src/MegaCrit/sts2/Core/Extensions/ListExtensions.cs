// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Extensions.ListExtensions
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Extensions;

public static class ListExtensions
{
  public static List<T> StableShuffle<T>(this List<T> list, Rng rng) where T : IComparable<T>
  {
    List<T> list1 = list.ToList<T>();
    list1.Sort();
    for (int index = 0; index < list.Count; ++index)
      list[index] = list1[index];
    return list.UnstableShuffle<T>(rng);
  }

  public static List<T> UnstableShuffle<T>(this List<T> list, Rng rng)
  {
    int count = list.Count;
    while (count > 1)
    {
      --count;
      int index1 = rng.NextInt(count + 1);
      List<T> objList1 = list;
      int index2 = index1;
      List<T> objList2 = list;
      int num = count;
      T obj1 = list[count];
      T obj2 = list[index1];
      objList1[index2] = obj1;
      int index3 = num;
      T obj3 = obj2;
      objList2[index3] = obj3;
    }
    return list;
  }

  public static int IndexOf<T>(this IReadOnlyList<T> readOnlyList, T item)
  {
    if (readOnlyList is IList<T> objList)
      return objList.IndexOf(item);
    for (int index = 0; index < readOnlyList.Count; ++index)
    {
      if (EqualityComparer<T>.Default.Equals(readOnlyList[index], item))
        return index;
    }
    return -1;
  }

  public static int FirstIndex<T>(this IReadOnlyList<T> readOnlyList, Predicate<T> match)
  {
    for (int index = 0; index < readOnlyList.Count; ++index)
    {
      if (match(readOnlyList[index]))
        return index;
    }
    return -1;
  }
}
