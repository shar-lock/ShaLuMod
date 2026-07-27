// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Helpers.MathHelper
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;

#nullable disable
namespace MegaCrit.Sts2.Core.Helpers;

public static class MathHelper
{
  public const float degToRad = 0.0174533f;

  public static float Remap(float value, float from1, float to1, float from2, float to2)
  {
    return (float) (((double) value - (double) from1) / ((double) to1 - (double) from1) * ((double) to2 - (double) from2)) + from2;
  }

  public static Vector2 BezierCurve(Vector2 v0, Vector2 v1, Vector2 c0, float t)
  {
    return Vector2.op_Addition(Vector2.op_Addition(Vector2.op_Multiply(Mathf.Pow(1f - t, 2f), v0), Vector2.op_Multiply((float) (2.0 * (1.0 - (double) t)) * t, c0)), Vector2.op_Multiply(Mathf.Pow(t, 2f), v1));
  }

  public static float GetAngle(Vector2 vector) => Mathf.Atan2(vector.Y, vector.X);

  public static Vector2 Clamp(Vector2 input, float min, float max)
  {
    float num = Mathf.Clamp(input.X, min, max);
    return new Vector2(num, num);
  }

  public static float SmoothDamp(
    float current,
    float target,
    ref float currentVelocity,
    float smoothTime,
    float deltaTime,
    float maxSpeed = float.PositiveInfinity)
  {
    smoothTime = Mathf.Max(0.0001f, smoothTime);
    float num1 = 2f / smoothTime;
    float num2 = num1 * deltaTime;
    float num3 = (float) (1.0 / (1.0 + (double) num2 + 0.47999998927116394 * (double) num2 * (double) num2 + 0.23499999940395355 * (double) num2 * (double) num2 * (double) num2));
    float num4 = current - target;
    float num5 = target;
    float num6 = maxSpeed * smoothTime;
    float num7 = Mathf.Clamp(num4, -num6, num6);
    target = current - num7;
    float num8 = (currentVelocity + num1 * num7) * deltaTime;
    currentVelocity = (currentVelocity - num1 * num8) * num3;
    float num9 = target + (num7 + num8) * num3;
    if ((double) num5 - (double) current > 0.0 == (double) num9 > (double) num5)
    {
      num9 = num5;
      currentVelocity = (num9 - num5) / deltaTime;
    }
    return num9;
  }

  public static Vector2 SmoothDamp(
    Vector2 current,
    Vector2 target,
    ref Vector2 currentVelocity,
    float smoothTime,
    float deltaTime,
    float maxSpeed = float.PositiveInfinity)
  {
    smoothTime = Mathf.Max(0.0001f, smoothTime);
    float num1 = 2f / smoothTime;
    float num2 = num1 * deltaTime;
    float num3 = (float) (1.0 / (1.0 + (double) num2 + 0.47999998927116394 * (double) num2 * (double) num2 + 0.23499999940395355 * (double) num2 * (double) num2 * (double) num2));
    float num4 = current.X - target.X;
    float num5 = current.Y - target.Y;
    Vector2 vector2 = target;
    float num6 = maxSpeed * smoothTime;
    float num7 = num6 * num6;
    float num8 = (float) ((double) num4 * (double) num4 + (double) num5 * (double) num5);
    if ((double) num8 > (double) num7)
    {
      float num9 = Mathf.Sqrt(num8);
      num4 = num4 / num9 * num6;
      num5 = num5 / num9 * num6;
    }
    target.X = current.X - num4;
    target.Y = current.Y - num5;
    float num10 = (currentVelocity.X + num1 * num4) * deltaTime;
    float num11 = (currentVelocity.Y + num1 * num5) * deltaTime;
    currentVelocity.X = (currentVelocity.X - num1 * num10) * num3;
    currentVelocity.Y = (currentVelocity.Y - num1 * num11) * num3;
    float num12 = target.X + (num4 + num10) * num3;
    float num13 = target.Y + (num5 + num11) * num3;
    float num14 = vector2.X - current.X;
    float num15 = vector2.Y - current.Y;
    float num16 = num12 - vector2.X;
    float num17 = num13 - vector2.Y;
    if ((double) num14 * (double) num16 + (double) num15 * (double) num17 > 0.0)
    {
      num12 = vector2.X;
      num13 = vector2.Y;
      currentVelocity.X = (num12 - vector2.X) / deltaTime;
      currentVelocity.Y = (num13 - vector2.Y) / deltaTime;
    }
    return new Vector2(num12, num13);
  }
}
