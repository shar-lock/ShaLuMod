// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Helpers.Ease
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using System;

#nullable disable
namespace MegaCrit.Sts2.Core.Helpers;

public static class Ease
{
  private const float _tolerance = 0.001f;
  private const float _pi = 3.14159274f;
  private const float _halfPi = 1.57079637f;

  public static float Interpolate(float p, Ease.Functions function)
  {
    float num;
    switch (function)
    {
      case Ease.Functions.QuadIn:
        num = Ease.QuadIn(p);
        break;
      case Ease.Functions.QuadOut:
        num = Ease.QuadOut(p);
        break;
      case Ease.Functions.QuadInOut:
        num = Ease.QuadInOut(p);
        break;
      case Ease.Functions.CubicIn:
        num = Ease.CubicIn(p);
        break;
      case Ease.Functions.CubicOut:
        num = Ease.CubicOut(p);
        break;
      case Ease.Functions.CubicInOut:
        num = Ease.CubicInOut(p);
        break;
      case Ease.Functions.QuartIn:
        num = Ease.QuartIn(p);
        break;
      case Ease.Functions.QuartOut:
        num = Ease.QuartOut(p);
        break;
      case Ease.Functions.QuartInOut:
        num = Ease.QuartInOut(p);
        break;
      case Ease.Functions.QuintIn:
        num = Ease.QuintIn(p);
        break;
      case Ease.Functions.QuinOut:
        num = Ease.QuintOut(p);
        break;
      case Ease.Functions.QuinInOut:
        num = Ease.QuintInOut(p);
        break;
      case Ease.Functions.SineIn:
        num = Ease.SineIn(p);
        break;
      case Ease.Functions.SineOut:
        num = Ease.SineOut(p);
        break;
      case Ease.Functions.SineInOut:
        num = Ease.SineInOut(p);
        break;
      case Ease.Functions.CircIn:
        num = Ease.CircIn(p);
        break;
      case Ease.Functions.CircOut:
        num = Ease.CircOut(p);
        break;
      case Ease.Functions.CircInOut:
        num = Ease.CircInOut(p);
        break;
      case Ease.Functions.ExpoIn:
        num = Ease.ExpoIn(p);
        break;
      case Ease.Functions.ExpoOut:
        num = Ease.ExpoOut(p);
        break;
      case Ease.Functions.ExpoInOut:
        num = Ease.ExpoInOut(p);
        break;
      case Ease.Functions.ElasticIn:
        num = Ease.ElasticIn(p);
        break;
      case Ease.Functions.ElasticOut:
        num = Ease.ElasticOut(p);
        break;
      case Ease.Functions.ElasticInOut:
        num = Ease.ElasticInOut(p);
        break;
      case Ease.Functions.BackIn:
        num = Ease.BackIn(p);
        break;
      case Ease.Functions.BackOut:
        num = Ease.BackOut(p);
        break;
      case Ease.Functions.BackInOut:
        num = Ease.BackInOut(p);
        break;
      case Ease.Functions.BounceIn:
        num = Ease.BounceIn(p);
        break;
      case Ease.Functions.BounceOut:
        num = Ease.BounceOut(p);
        break;
      case Ease.Functions.BounceInOut:
        num = Ease.BounceInOut(p);
        break;
      default:
        num = Ease.Linear(p);
        break;
    }
    return num;
  }

  public static float Linear(float p) => p;

  public static float QuadIn(float p) => p * p;

  public static float QuadOut(float p) => (float) -((double) p * ((double) p - 2.0));

  public static float QuadInOut(float p)
  {
    return (double) p < 0.5 ? 2f * p * p : (float) (-2.0 * (double) p * (double) p + 4.0 * (double) p - 1.0);
  }

  public static float CubicIn(float p) => p * p * p;

  public static float CubicOut(float p)
  {
    float num = p - 1f;
    return (float) ((double) num * (double) num * (double) num + 1.0);
  }

  public static float CubicInOut(float p)
  {
    if ((double) p < 0.5)
      return 4f * p * p * p;
    float num = (float) (2.0 * (double) p - 2.0);
    return (float) (0.5 * (double) num * (double) num * (double) num + 1.0);
  }

  public static float QuartIn(float p) => p * p * p * p;

  public static float QuartOut(float p)
  {
    float num = p - 1f;
    return (float) ((double) num * (double) num * (double) num * (1.0 - (double) p) + 1.0);
  }

  public static float QuartInOut(float p)
  {
    if ((double) p < 0.5)
      return 8f * p * p * p * p;
    float num = p - 1f;
    return (float) (-8.0 * (double) num * (double) num * (double) num * (double) num + 1.0);
  }

  public static float QuintIn(float p) => p * p * p * p * p;

  public static float QuintOut(float p)
  {
    float num = p - 1f;
    return (float) ((double) num * (double) num * (double) num * (double) num * (double) num + 1.0);
  }

  public static float QuintInOut(float p)
  {
    if ((double) p < 0.5)
      return 16f * p * p * p * p * p;
    float num = (float) (2.0 * (double) p - 2.0);
    return (float) (0.5 * (double) num * (double) num * (double) num * (double) num * (double) num + 1.0);
  }

  public static float SineIn(float p)
  {
    return Mathf.Sin((float) (((double) p - 1.0) * 1.5707963705062866)) + 1f;
  }

  public static float SineOut(float p) => Mathf.Sin(p * 1.57079637f);

  public static float SineInOut(float p)
  {
    return (float) (0.5 * (1.0 - (double) Mathf.Cos(p * 3.14159274f)));
  }

  public static float CircIn(float p) => 1f - Mathf.Sqrt((float) (1.0 - (double) p * (double) p));

  public static float CircOut(float p) => Mathf.Sqrt((2f - p) * p);

  public static float CircInOut(float p)
  {
    return (double) p < 0.5 ? (float) (0.5 * (1.0 - (double) Mathf.Sqrt((float) (1.0 - 4.0 * ((double) p * (double) p))))) : (float) (0.5 * ((double) Mathf.Sqrt((float) (-(2.0 * (double) p - 3.0) * (2.0 * (double) p - 1.0))) + 1.0));
  }

  public static float ExpoIn(float p)
  {
    return (double) p != 0.0 ? Mathf.Pow(2f, (float) (10.0 * ((double) p - 1.0))) : p;
  }

  public static float ExpoOut(float p)
  {
    return (double) Math.Abs(p - 1f) >= 1.0 / 1000.0 ? 1f - Mathf.Pow(2f, -10f * p) : p;
  }

  public static float ExpoInOut(float p)
  {
    if ((double) p == 0.0 || (double) Math.Abs(p - 1f) < 1.0 / 1000.0)
      return p;
    return (double) p < 0.5 ? 0.5f * Mathf.Pow(2f, (float) (20.0 * (double) p - 10.0)) : (float) (-0.5 * (double) Mathf.Pow(2f, (float) (-20.0 * (double) p + 10.0)) + 1.0);
  }

  public static float ElasticIn(float p)
  {
    return Mathf.Sin(20.4203529f * p) * Mathf.Pow(2f, (float) (10.0 * ((double) p - 1.0)));
  }

  public static float ElasticOut(float p)
  {
    return (float) ((double) Mathf.Sin((float) (-20.420352935791016 * ((double) p + 1.0))) * (double) Mathf.Pow(2f, -10f * p) + 1.0);
  }

  public static float ElasticInOut(float p)
  {
    return (double) p < 0.5 ? 0.5f * Mathf.Sin((float) (20.420352935791016 * (2.0 * (double) p))) * Mathf.Pow(2f, (float) (10.0 * (2.0 * (double) p - 1.0))) : (float) (0.5 * ((double) Mathf.Sin((float) (-20.420352935791016 * (2.0 * (double) p - 1.0 + 1.0))) * (double) Mathf.Pow(2f, (float) (-10.0 * (2.0 * (double) p - 1.0))) + 2.0));
  }

  public static float BackIn(float p, float strength = 1f)
  {
    float num = strength * 1.70158f;
    return (float) ((double) p * (double) p * (((double) num + 1.0) * (double) p - (double) num));
  }

  public static float BackOut(float p, float strength = 1f)
  {
    float num = strength * 1.70158f;
    return (float) (1.0 + (double) num * ((double) p - 1.0) * ((double) p - 1.0) * (((double) num + 1.0) * ((double) p - 1.0) + (double) num));
  }

  public static float BackInOut(float p, float strength = 1f)
  {
    float num1 = strength * 1.70158f;
    p *= 2f;
    float num2;
    float num3;
    return (double) p < 1.0 ? (float) (0.5 * ((double) p * (double) p * (((double) (num2 = num1 * 1.525f) + 1.0) * (double) p - (double) num2))) : (float) (0.5 * ((double) (p -= 2f) * (double) p * (((double) (num3 = num1 * 1.525f) + 1.0) * (double) p + (double) num3) + 2.0));
  }

  public static float BounceIn(float p) => 1f - Ease.BounceOut(1f - p);

  public static float BounceOut(float p)
  {
    if ((double) p < 0.36363637447357178)
      return (float) (121.0 * (double) p * (double) p / 16.0);
    if ((double) p < 0.72727274894714355)
      return (float) (9.0749998092651367 * (double) p * (double) p - 9.8999996185302734 * (double) p + 3.4000000953674316);
    return (double) p < 0.89999997615814209 ? (float) (12.066481590270996 * (double) p * (double) p - 19.635457992553711 * (double) p + 8.89806079864502) : (float) (10.800000190734863 * (double) p * (double) p - 20.520000457763672 * (double) p + 10.720000267028809);
  }

  public static float BounceInOut(float p)
  {
    return (double) p < 0.5 ? 0.5f * Ease.BounceIn(p * 2f) : (float) (0.5 * (double) Ease.BounceOut((float) ((double) p * 2.0 - 1.0)) + 0.5);
  }

  public enum Functions
  {
    QuadIn,
    QuadOut,
    QuadInOut,
    CubicIn,
    CubicOut,
    CubicInOut,
    QuartIn,
    QuartOut,
    QuartInOut,
    QuintIn,
    QuinOut,
    QuinInOut,
    SineIn,
    SineOut,
    SineInOut,
    CircIn,
    CircOut,
    CircInOut,
    ExpoIn,
    ExpoOut,
    ExpoInOut,
    ElasticIn,
    ElasticOut,
    ElasticInOut,
    BackIn,
    BackOut,
    BackInOut,
    BounceIn,
    BounceOut,
    BounceInOut,
  }
}
