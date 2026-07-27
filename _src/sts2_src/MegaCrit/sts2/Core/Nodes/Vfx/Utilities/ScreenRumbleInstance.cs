// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.Utilities.ScreenRumbleInstance
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Random;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;

public class ScreenRumbleInstance : ShakeInstance
{
  private readonly FastNoiseLite _noise = new FastNoiseLite();
  private readonly float _randomOffset = Rng.Chaotic.NextFloat(99999f);
  private readonly float _speed = 10f;
  private Vector2 _targetOffset = Vector2.Zero;
  private readonly RumbleStyle _style;

  public ScreenRumbleInstance(
    float strength,
    double duration,
    float speedMultiplier,
    RumbleStyle style)
  {
    this._noise.Frequency = style == RumbleStyle.Rumble ? 6f : 0.1f;
    this._noise.NoiseType = (FastNoiseLite.NoiseTypeEnum) 3L;
    this._strength = strength * (style == RumbleStyle.Rumble ? 0.5f : 5f);
    this._speed *= speedMultiplier;
    this._startDuration = duration;
    this._duration = duration;
    this._style = style;
  }

  public override Vector2 Update(double delta)
  {
    if (this.IsDone)
      return Vector2.Zero;
    this._duration -= delta;
    float duration = (float) this._duration;
    float noise1D1 = ((Noise) this._noise).GetNoise1D(duration + this._randomOffset);
    float noise1D2 = ((Noise) this._noise).GetNoise1D(duration + this._randomOffset + this._randomOffset);
    this._ease = Ease.CubicOut((float) (this._duration / this._startDuration));
    Vector2 vector2;
    if (this._style == RumbleStyle.Drunk)
    {
      this._targetOffset = ((Vector2) ref this._targetOffset).Lerp(new Vector2(noise1D1, noise1D2), Mathf.Clamp((float) delta * this._speed, 0.0f, 1f));
      vector2 = Vector2.op_Multiply(Vector2.op_Multiply(this._targetOffset, this._strength), this._ease);
    }
    else
      vector2 = Vector2.op_Multiply(Vector2.op_Multiply(new Vector2(noise1D1, noise1D2), this._strength), this._ease);
    if (this._duration < 0.0)
      this.IsDone = true;
    return vector2;
  }
}
