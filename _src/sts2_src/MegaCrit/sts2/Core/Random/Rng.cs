// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Random.Rng
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Saves;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Random;

public class Rng
{
  private int _counter;
  private readonly MegaRandom _random;

  public static Rng Chaotic { get; } = new Rng((ulong) DateTimeOffset.Now.ToUnixTimeSeconds());

  public Rng(ulong seed = 0) => this._random = new MegaRandom(seed);

  public Rng(SerializableRng serializable)
  {
    this._counter = serializable.counter;
    this._random = new MegaRandom(serializable);
  }

  public Rng(Player player, ModelId id, ulong mixin = 0)
    : this(player.RunState.Rng.Seed + (ulong) player.RunState.GetPlayerSlotIndex(player) + StringHelper.GetDeterministicHashCode(id.Entry) + mixin)
  {
  }

  public Rng(ulong seed, string name)
    : this(seed + StringHelper.GetDeterministicHashCode(name))
  {
  }

  public void LoadFromSerializable(SerializableRng serializable)
  {
    this._counter = serializable.counter;
    this._random.Reinitialise(serializable);
  }

  public bool NextBool()
  {
    ++this._counter;
    return this._random.Next(2) == 0;
  }

  public int NextInt(int maxExclusive = 2147483647 /*0x7FFFFFFF*/)
  {
    ++this._counter;
    return this._random.Next(maxExclusive);
  }

  public int NextInt(int minInclusive, int maxExclusive)
  {
    if (minInclusive >= maxExclusive)
      throw new ArgumentOutOfRangeException(nameof (minInclusive), "Minimum must be lower than maximum.");
    ++this._counter;
    return this._random.Next(minInclusive, maxExclusive);
  }

  public uint NextUnsignedInt(uint maxExclusive = 4294967295 /*0xFFFFFFFF*/)
  {
    return this.NextUnsignedInt(0U, maxExclusive);
  }

  public uint NextUnsignedInt(uint minInclusive, uint maxExclusive)
  {
    if (minInclusive >= maxExclusive)
      throw new ArgumentOutOfRangeException(nameof (minInclusive), "Minimum must be lower than maximum.");
    ++this._counter;
    uint num = (uint) (this._random.NextDouble() * (double) (maxExclusive - minInclusive));
    return minInclusive + num;
  }

  public ulong NextUnsignedLong()
  {
    ++this._counter;
    return this._random.NextULong();
  }

  public ulong NextUnsignedLong(ulong maxExclusive = 18446744073709551615 /*0xFFFFFFFFFFFFFFFF*/)
  {
    return maxExclusive == ulong.MaxValue ? this.NextUnsignedLong() : this.NextUnsignedLong(0UL, maxExclusive);
  }

  public ulong NextUnsignedLong(ulong minInclusive, ulong maxExclusive)
  {
    if (minInclusive >= maxExclusive)
      throw new ArgumentOutOfRangeException(nameof (minInclusive), "Minimum must be lower than maximum.");
    ++this._counter;
    ulong num = (ulong) (this._random.NextDouble() * (double) (maxExclusive - minInclusive));
    return minInclusive + num;
  }

  public float NextFloat(float max = 1f) => this.NextFloat(0.0f, max);

  public float NextFloat(float min, float max)
  {
    if ((double) min > (double) max)
      throw new ArgumentOutOfRangeException(nameof (min), "Minimum must not be higher than maximum.");
    ++this._counter;
    return (float) (this._random.NextDouble() * ((double) max - (double) min)) + min;
  }

  public double NextDouble()
  {
    ++this._counter;
    return this._random.NextDouble();
  }

  public double NextDouble(double min, double max)
  {
    if (min > max)
      throw new ArgumentOutOfRangeException(nameof (min), "Minimum must not be higher than maximum.");
    ++this._counter;
    return this._random.NextDouble() * (max - min) + min;
  }

  public float NextGaussianFloat(float mean = 0.0f, float stdDev = 1f, float min = 0.0f, float max = 1f)
  {
    return (float) this.NextGaussianDouble((double) mean, (double) stdDev, (double) min, (double) max);
  }

  public double NextGaussianDouble(double mean = 0.0, double stdDev = 1.0, double min = 0.0, double max = 1.0)
  {
    if (min > max)
      throw new ArgumentOutOfRangeException(nameof (min), "Minimum must not be higher than maximum.");
    if (mean < 0.0 || mean > 1.0)
      throw new ArgumentOutOfRangeException(nameof (mean), (object) mean, "Mean must be within [0, 1].");
    double num1;
    do
    {
      double num2 = 1.0 - this.NextDouble();
      double num3 = 1.0 - this.NextDouble();
      double num4 = Math.Sqrt(-2.0 * Math.Log(num2)) * Math.Cos(2.0 * Math.PI * num3);
      num1 = mean + num4 * stdDev;
    }
    while (num1 < 0.0 || num1 > 1.0);
    return num1 * (max - min) + min;
  }

  public int NextGaussianInt(int mean, int stdDev, int min, int max)
  {
    if (min > max)
      throw new ArgumentOutOfRangeException(nameof (min), "Minimum must not be higher than maximum.");
    if (mean < min || mean > max)
      throw new ArgumentOutOfRangeException(nameof (mean), (object) mean, "Mean must be within [min, max].");
    int num1;
    do
    {
      double num2 = 1.0 - this.NextDouble();
      double num3 = 1.0 - this.NextDouble();
      double num4 = Math.Sqrt(-2.0 * Math.Log(num2)) * Math.Sin(2.0 * Math.PI * num3);
      num1 = (int) Math.Round((double) mean + (double) stdDev * num4);
    }
    while (num1 < min || num1 > max);
    return num1;
  }

  public T? NextItem<T>(IEnumerable<T> items)
  {
    if (!(items is T[] objArray))
      objArray = items.ToArray<T>();
    IEnumerable<T> source = (IEnumerable<T>) objArray;
    int maxExclusive = source.Count<T>();
    if (maxExclusive == 0)
      return default (T);
    int index = this.NextInt(0, maxExclusive);
    return source.ElementAt<T>(index);
  }

  public T? WeightedNextItem<T>(IEnumerable<T> items, Func<T?, float> weightFetcher)
  {
    return Rng.WeightedNextItem<T>(this.NextFloat(), items, weightFetcher, default (T));
  }

  public static T WeightedNextItem<T>(
    float randInput,
    IEnumerable<T> items,
    Func<T, float> weightFetcher,
    T fallback)
  {
    float num1 = items.Sum<T>(weightFetcher);
    float num2 = randInput * num1;
    foreach (T obj in items)
    {
      num2 -= weightFetcher(obj);
      if ((double) num2 <= 0.0)
        return obj;
    }
    return fallback;
  }

  public void Shuffle<T>(IList<T> list)
  {
    for (int index1 = list.Count - 1; index1 > 0; --index1)
    {
      int index2 = this.NextInt(index1 + 1);
      IList<T> objList1 = list;
      int index3 = index1;
      IList<T> objList2 = list;
      int num = index2;
      T obj1 = list[index2];
      T obj2 = list[index1];
      objList1[index3] = obj1;
      int index4 = num;
      T obj3 = obj2;
      objList2[index4] = obj3;
    }
  }

  public SerializableRng ToSerializable()
  {
    SerializableRng rng = new SerializableRng()
    {
      counter = this._counter
    };
    this._random.FillSerializableState(rng);
    return rng;
  }
}
