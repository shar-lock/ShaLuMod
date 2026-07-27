// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Helpers.GrabBag`1
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Helpers;

public class GrabBag<T>
{
  private readonly List<(T, double)> _entries = new List<(T, double)>();
  private double _totalWeight;

  public void Add(T element, double weight)
  {
    this._entries.Add((element, weight));
    this._totalWeight += weight;
  }

  public T? Grab(Rng rng, Func<T, bool>? predicate = null)
  {
    int index = this.GrabIndex(rng, predicate);
    return index < 0 ? default (T) : this._entries[index].Item1;
  }

  public T? GrabAndRemove(Rng rng, Func<T, bool>? predicate = null)
  {
    int index = this.GrabIndex(rng, predicate);
    if (index < 0)
      return default (T);
    T obj = this._entries[index].Item1;
    this.Remove(index);
    return obj;
  }

  public int Count => this._entries.Count;

  public bool Any() => this._entries.Any<(T, double)>();

  private int GrabIndex(Rng rng, Func<T, bool>? predicate)
  {
    if (predicate != null && !this._entries.Any<(T, double)>((Func<(T, double), bool>) (e => predicate(e.Item1))))
      return -1;
    int index;
    do
    {
      index = this.GrabIndex(rng);
    }
    while (predicate != null && index >= 0 && !predicate(this._entries[index].Item1));
    return index;
  }

  private int GrabIndex(Rng rng)
  {
    double num1 = rng.NextDouble() * this._totalWeight;
    double num2 = 0.0;
    for (int index = 0; index < this._entries.Count; ++index)
    {
      num2 += this._entries[index].Item2;
      if (num1 < num2)
        return index;
    }
    return -1;
  }

  private void Remove(int index)
  {
    this._totalWeight -= this._entries[index].Item2;
    this._entries.RemoveAt(index);
  }
}
