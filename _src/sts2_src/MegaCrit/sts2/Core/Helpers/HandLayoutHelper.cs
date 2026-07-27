// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Helpers.HandLayoutHelper
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Helpers;

public static class HandLayoutHelper
{
  public static int GetInsertIndex<T>(
    IReadOnlyList<T> pileOrder,
    IEnumerable<T> presentCards,
    T target)
  {
    EqualityComparer<T> comparer = EqualityComparer<T>.Default;
    int num1 = HandLayoutHelper.IndexOf<T>(pileOrder, target, comparer);
    if (num1 < 0)
      return -1;
    int insertIndex = 0;
    foreach (T presentCard in presentCards)
    {
      if (!comparer.Equals(presentCard, target))
      {
        int num2 = HandLayoutHelper.IndexOf<T>(pileOrder, presentCard, comparer);
        if (num2 >= 0 && num2 < num1)
          ++insertIndex;
      }
    }
    return insertIndex;
  }

  private static int IndexOf<T>(IReadOnlyList<T> list, T item, EqualityComparer<T> comparer)
  {
    for (int index = 0; index < list.Count; ++index)
    {
      if (comparer.Equals(list[index], item))
        return index;
    }
    return -1;
  }
}
