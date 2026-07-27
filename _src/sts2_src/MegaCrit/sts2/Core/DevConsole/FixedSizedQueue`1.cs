// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.DevConsole.FixedSizedQueue`1
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.DevConsole;

public class FixedSizedQueue<T> : List<T>
{
  private readonly int _limit;

  public FixedSizedQueue(int limit) => this._limit = limit;

  public void Enqueue(T obj)
  {
    if (this.Count >= this._limit)
      this.RemoveAt(this.Count - 1);
    this.Insert(0, obj);
  }
}
