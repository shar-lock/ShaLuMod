// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Random.MegaRandom
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Saves;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable enable
namespace MegaCrit.Sts2.Core.Random;

public sealed class MegaRandom
{
  private const double _incrDouble = 1.1102230246251565E-16;
  private const float _incrFloat = 5.96046448E-08f;
  private ulong _s0;
  private ulong _s1;
  private ulong _s2;
  private ulong _s3;

  public MegaRandom(ulong seed) => this.Reinitialise(seed);

  public MegaRandom(SerializableRng serializable) => this.Reinitialise(serializable);

  public static ulong Splitmix64(ref ulong x)
  {
    ulong num1 = x += 11400714819323198485UL;
    ulong num2 = (ulong) (((long) num1 ^ (long) (num1 >> 30)) * -4658895280553007687L);
    ulong num3 = (ulong) (((long) num2 ^ (long) (num2 >> 27)) * -7723592293110705685L);
    return num3 ^ num3 >> 31 /*0x1F*/;
  }

  public void Reinitialise(ulong seed)
  {
    this._s0 = MegaRandom.Splitmix64(ref seed);
    this._s1 = MegaRandom.Splitmix64(ref seed);
    this._s2 = MegaRandom.Splitmix64(ref seed);
    this._s3 = MegaRandom.Splitmix64(ref seed);
  }

  public void Reinitialise(SerializableRng serializable)
  {
    this._s0 = serializable.state0;
    this._s1 = serializable.state1;
    this._s2 = serializable.state2;
    this._s3 = serializable.state3;
  }

  public unsafe void NextBytes(Span<byte> span)
  {
    ulong s0 = this._s0;
    ulong s1 = this._s1;
    ulong num1 = this._s2;
    ulong num2 = this._s3;
    for (; span.Length >= 8; span = span.Slice(8))
    {
      Unsafe.WriteUnaligned<ulong>(ref MemoryMarshal.GetReference<byte>(span), BitOperations.RotateLeft(s1 * 5UL, 7) * 9UL);
      ulong num3 = s1 << 17;
      ulong num4 = num1 ^ s0;
      ulong num5 = num2 ^ s1;
      s1 ^= num4;
      s0 ^= num5;
      num1 = num4 ^ num3;
      num2 = BitOperations.RotateLeft(num5, 45);
    }
    if (!span.IsEmpty)
    {
      byte* numPtr = (byte*) &(BitOperations.RotateLeft(s1 * 5UL, 7) * 9UL);
      for (int index = 0; index < span.Length; ++index)
        span[index] = numPtr[index];
      ulong num6 = s1 << 17;
      ulong num7 = num1 ^ s0;
      ulong num8 = num2 ^ s1;
      s1 ^= num7;
      s0 ^= num8;
      num1 = num7 ^ num6;
      num2 = BitOperations.RotateLeft(num8, 45);
    }
    this._s0 = s0;
    this._s1 = s1;
    this._s2 = num1;
    this._s3 = num2;
  }

  private ulong NextULongInner()
  {
    ulong s0 = this._s0;
    ulong s1 = this._s1;
    ulong s2 = this._s2;
    ulong s3 = this._s3;
    ulong num1 = BitOperations.RotateLeft(s1 * 5UL, 7) * 9UL;
    ulong num2 = s1 << 17;
    ulong num3 = s2 ^ s0;
    ulong num4 = s3 ^ s1;
    ulong num5 = s1 ^ num3;
    ulong num6 = s0 ^ num4;
    ulong num7 = num3 ^ num2;
    ulong num8 = BitOperations.RotateLeft(num4, 45);
    this._s0 = num6;
    this._s1 = num5;
    this._s2 = num7;
    this._s3 = num8;
    return num1;
  }

  public int Next(int maxValue)
  {
    return maxValue >= 1 ? this.NextInner(maxValue) : throw new ArgumentOutOfRangeException(nameof (maxValue), (object) maxValue, "maxValue must be > 0");
  }

  public int Next(int minValue, int maxValue)
  {
    if (minValue >= maxValue)
      throw new ArgumentOutOfRangeException(nameof (maxValue), (object) maxValue, "maxValue must be > minValue");
    long maxValue1 = (long) maxValue - (long) minValue;
    return maxValue1 <= (long) int.MaxValue ? this.NextInner((int) maxValue1) + minValue : (int) (this.NextInner(maxValue1) + (long) minValue);
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public double NextDouble() => (double) (this.NextULongInner() >> 11) * 1.1102230246251565E-16;

  public int NextInt() => (int) (this.NextULongInner() >> 33);

  public uint NextUInt() => (uint) this.NextULongInner();

  public ulong NextULong() => this.NextULongInner();

  public bool NextBool()
  {
    return (this.NextULongInner() & 9223372036854775808UL /*0x8000000000000000*/) > 0UL;
  }

  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public float NextFloat() => (float) (this.NextULongInner() >> 40) * 5.96046448E-08f;

  private int NextInner(int maxValue) => (int) (this.NextDouble() * (double) maxValue);

  private long NextInner(long maxValue) => (long) (this.NextDouble() * (double) maxValue);

  public void FillSerializableState(SerializableRng rng)
  {
    rng.state0 = this._s0;
    rng.state1 = this._s1;
    rng.state2 = this._s2;
    rng.state3 = this._s3;
  }
}
