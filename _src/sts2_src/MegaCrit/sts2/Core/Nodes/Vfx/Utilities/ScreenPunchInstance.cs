// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.Utilities.ScreenPunchInstance
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Helpers;
using System;

#nullable disable
namespace MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;

public class ScreenPunchInstance : ShakeInstance
{
  public ScreenPunchInstance(float strength, double duration, float degAngle)
  {
    this._strength = strength;
    this._startDuration = duration;
    this._duration = duration;
    this._angle = Mathf.DegToRad(degAngle);
  }

  public override Vector2 Update(double delta)
  {
    if (this.IsDone)
      return Vector2.Zero;
    this._duration -= delta;
    float num = (float) Math.Cos(this._duration * (double) ShakeInstance.WiggleSpeed);
    this._ease = Ease.CubicOut((float) (this._duration / this._startDuration));
    Vector2 vector2_1 = new Vector2(num, 0.0f);
    Vector2 vector2_2 = Vector2.op_Multiply(Vector2.op_Multiply(((Vector2) ref vector2_1).Rotated(this._angle), this._strength), this._ease);
    if (this._duration < 0.0)
      this.IsDone = true;
    return vector2_2;
  }

  public void Cancel()
  {
    this._duration = 0.0;
    this.IsDone = true;
  }
}
