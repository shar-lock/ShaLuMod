// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.Utilities.ScreenTraumaRumble
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Random;
using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;

public class ScreenTraumaRumble
{
  private const float _rumbleAmount = 200f;
  private const float _speed = 1000f;
  private const float _decayRate = 2f;
  private const float _maxShake = 50f;
  private readonly FastNoiseLite _noise = new FastNoiseLite();
  private float _trauma;
  private double _duration;
  private float _multiplier;

  public ScreenTraumaRumble()
  {
    this._noise.Frequency = 0.01f;
    this._noise.Seed = Rng.Chaotic.NextInt(999999);
    this._noise.NoiseType = (FastNoiseLite.NoiseTypeEnum) 3L;
  }

  public void AddTrauma(ShakeStrength amount)
  {
    float trauma = this._trauma;
    float num;
    switch (amount)
    {
      case ShakeStrength.VeryWeak:
        num = 0.07f;
        break;
      case ShakeStrength.Weak:
        num = 0.2f;
        break;
      case ShakeStrength.Medium:
        num = 0.4f;
        break;
      case ShakeStrength.Strong:
        num = 0.6f;
        break;
      case ShakeStrength.TooMuch:
        num = 1f;
        break;
      default:
        throw new ArgumentOutOfRangeException(nameof (amount), (object) amount, (string) null);
    }
    this._trauma = trauma + num;
    this._trauma = Mathf.Min(this._trauma, 1f);
  }

  public Vector2 Update(double delta)
  {
    if ((double) this._trauma <= 0.0)
      return Vector2.Zero;
    this._duration += delta * 1000.0;
    float num = Mathf.Pow(this._trauma, 1.5f) * 200f * this._multiplier;
    Vector2 vector2 = Vector2.op_Multiply(new Vector2(((Noise) this._noise).GetNoise1D((float) this._duration + 100f), ((Noise) this._noise).GetNoise1D((float) this._duration + 300f)), num);
    vector2 = ((Vector2) ref vector2).LimitLength(50f);
    this._trauma = Mathf.Max(this._trauma - (float) (2.0 * delta), 0.0f);
    return vector2;
  }

  public void SetMultiplier(float multiplier) => this._multiplier = multiplier;
}
