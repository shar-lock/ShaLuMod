// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.Utilities.NHitStop
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;

[ScriptPath("res://src/Core/Nodes/Vfx/Utilities/NHitStop.cs")]
public class NHitStop : Node
{
  private const float _minTimeScale = 0.1f;
  private CancellationTokenSource? _cancelToken;

  public void DoHitStop(ShakeStrength strength, ShakeDuration duration)
  {
    this._cancelToken?.Cancel();
    this._cancelToken = new CancellationTokenSource();
    TaskHelper.RunSafely(this.HitStopTask(this.EaseForStrength(strength), this.SecondsForDuration(duration)));
  }

  private async Task HitStopTask(Ease.Functions easing, float seconds)
  {
    this.SetTimeScale(0.1f);
    ulong lastTicks = Time.GetTicksMsec();
    float timer = 0.0f;
    while ((double) timer <= (double) seconds)
    {
      double num = (double) await this.AwaitProcessFrame();
      CancellationTokenSource cancelToken = this._cancelToken;
      if ((cancelToken != null ? (cancelToken.IsCancellationRequested ? 1 : 0) : 0) != 0)
        return;
      timer += (float) (Time.GetTicksMsec() - lastTicks) / 1000f;
      this.SetTimeScale(Mathf.Min((float) (0.10000000149011612 + (double) Ease.Interpolate(timer / seconds, easing) * 0.89999997615814209), 1f));
      lastTicks = Time.GetTicksMsec();
    }
    this.SetTimeScale(1f);
  }

  private void SetTimeScale(float timeScale) => Engine.SetTimeScale((double) timeScale);

  private Ease.Functions EaseForStrength(ShakeStrength strength)
  {
    switch (strength)
    {
      case ShakeStrength.VeryWeak:
        return Ease.Functions.CircIn;
      case ShakeStrength.Weak:
        return Ease.Functions.SineIn;
      case ShakeStrength.Medium:
        return Ease.Functions.QuadIn;
      case ShakeStrength.Strong:
        return Ease.Functions.QuartIn;
      case ShakeStrength.TooMuch:
        return Ease.Functions.ExpoIn;
      default:
        throw new ArgumentOutOfRangeException(nameof (strength), (object) strength, (string) null);
    }
  }

  private float SecondsForDuration(ShakeDuration duration)
  {
    switch (duration)
    {
      case ShakeDuration.Short:
        return 0.15f;
      case ShakeDuration.Normal:
        return 0.3f;
      case ShakeDuration.Long:
        return 0.6f;
      case ShakeDuration.Forever:
        return 2f;
      default:
        throw new ArgumentOutOfRangeException(nameof (duration), (object) duration, (string) null);
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NHitStop.MethodName.DoHitStop, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("strength"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("duration"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NHitStop.MethodName.SetTimeScale, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("timeScale"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NHitStop.MethodName.EaseForStrength, new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("strength"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NHitStop.MethodName.SecondsForDuration, new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("duration"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NHitStop.MethodName.DoHitStop) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.DoHitStop(VariantUtils.ConvertTo<ShakeStrength>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<ShakeDuration>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHitStop.MethodName.SetTimeScale) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetTimeScale(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NHitStop.MethodName.EaseForStrength) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      Ease.Functions functions = this.EaseForStrength(VariantUtils.ConvertTo<ShakeStrength>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<Ease.Functions>(ref functions);
      return true;
    }
    if (!StringName.op_Equality(ref method, NHitStop.MethodName.SecondsForDuration) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    float num = this.SecondsForDuration(VariantUtils.ConvertTo<ShakeDuration>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = VariantUtils.CreateFrom<float>(ref num);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NHitStop.MethodName.DoHitStop) || StringName.op_Equality(ref method, NHitStop.MethodName.SetTimeScale) || StringName.op_Equality(ref method, NHitStop.MethodName.EaseForStrength) || StringName.op_Equality(ref method, NHitStop.MethodName.SecondsForDuration) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName DoHitStop = StringName.op_Implicit(nameof (DoHitStop));
    public static readonly StringName SetTimeScale = StringName.op_Implicit(nameof (SetTimeScale));
    public static readonly StringName EaseForStrength = StringName.op_Implicit(nameof (EaseForStrength));
    public static readonly StringName SecondsForDuration = StringName.op_Implicit(nameof (SecondsForDuration));
  }

  public class PropertyName : Node.PropertyName
  {
  }

  public class SignalName : Node.SignalName
  {
  }
}
