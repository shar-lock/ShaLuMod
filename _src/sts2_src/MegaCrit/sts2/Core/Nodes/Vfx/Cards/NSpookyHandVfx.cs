// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.Cards.NSpookyHandVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Random;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable
namespace MegaCrit.Sts2.Core.Nodes.Vfx.Cards;

[ScriptPath("res://src/Core/Nodes/Vfx/Cards/NSpookyHandVfx.cs")]
public class NSpookyHandVfx : Control
{
  private const float _pauseDuration = 0.05f;
  private float _elapsedPauseTime;
  private int _pauseCounter;
  private bool _isPaused;
  private float _timer;
  private int _totalPauses;
  private float _canPauseTimer;
  private float _intensity;
  private float _speed;
  private float _duration;
  private float _originalRotation;
  private Vector2 _targetScale;

  public override void _Ready()
  {
    this._totalPauses = Rng.Chaotic.NextInt(2, 7);
    this._canPauseTimer = Rng.Chaotic.NextFloat(0.5f, 1.2f);
    this._speed = Rng.Chaotic.NextFloat(3f, 5f);
    this._intensity = Rng.Chaotic.NextFloat(0.1f, 0.3f);
    this._originalRotation = this.Rotation;
    this._targetScale = this.Scale;
    this.AnimateIn();
  }

  private void AnimateIn()
  {
    this.Scale = Vector2.Zero;
    Tween tween = ((Node) this).CreateTween().SetParallel(true);
    ((CanvasItem) this).Modulate = new Color(((CanvasItem) this).Modulate.R + Rng.Chaotic.NextFloat(-0.2f, 0.2f), ((CanvasItem) this).Modulate.G, ((CanvasItem) this).Modulate.B, 1f);
    tween.TweenInterval(Rng.Chaotic.NextDouble(0.0, 0.4));
    tween.Chain();
    tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(this._targetScale), Rng.Chaotic.NextDouble(0.4, 0.5)).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 11L);
    tween.Chain();
    tween.TweenInterval(Rng.Chaotic.NextDouble(0.3, 0.6));
    tween.Chain();
    double num = Rng.Chaotic.NextDouble(0.4, 0.6);
    tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(this._targetScale, 0.5f)), num).SetEase((Tween.EaseType) 0L).SetTrans((Tween.TransitionType) 10L);
    tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), num).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 4L);
  }

  public override void _Process(double delta)
  {
    float num = (float) delta;
    this._duration += num * this._speed;
    this._canPauseTimer -= num;
    if (this._isPaused)
    {
      this._elapsedPauseTime += num;
      if ((double) this._elapsedPauseTime >= 0.05000000074505806)
      {
        this._isPaused = false;
        this._elapsedPauseTime = 0.0f;
        ++this._pauseCounter;
      }
      this.Rotation = this._originalRotation + (float) ((double) Mathf.Sin(this._duration) * (double) this._intensity * 0.5);
    }
    else
      this.Rotation = this._originalRotation + Mathf.Sin(this._duration) * this._intensity;
    this._timer += num;
    if ((double) this._canPauseTimer >= 0.0 || this._pauseCounter >= this._totalPauses)
      return;
    this._isPaused = true;
    this._timer = 0.0f;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NSpookyHandVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSpookyHandVfx.MethodName.AnimateIn, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSpookyHandVfx.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NSpookyHandVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSpookyHandVfx.MethodName.AnimateIn) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimateIn();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NSpookyHandVfx.MethodName._Process) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NSpookyHandVfx.MethodName._Ready) || StringName.op_Equality(ref method, NSpookyHandVfx.MethodName.AnimateIn) || StringName.op_Equality(ref method, NSpookyHandVfx.MethodName._Process) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSpookyHandVfx.PropertyName._elapsedPauseTime))
    {
      this._elapsedPauseTime = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpookyHandVfx.PropertyName._pauseCounter))
    {
      this._pauseCounter = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpookyHandVfx.PropertyName._isPaused))
    {
      this._isPaused = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpookyHandVfx.PropertyName._timer))
    {
      this._timer = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpookyHandVfx.PropertyName._totalPauses))
    {
      this._totalPauses = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpookyHandVfx.PropertyName._canPauseTimer))
    {
      this._canPauseTimer = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpookyHandVfx.PropertyName._intensity))
    {
      this._intensity = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpookyHandVfx.PropertyName._speed))
    {
      this._speed = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpookyHandVfx.PropertyName._duration))
    {
      this._duration = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpookyHandVfx.PropertyName._originalRotation))
    {
      this._originalRotation = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSpookyHandVfx.PropertyName._targetScale))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._targetScale = VariantUtils.ConvertTo<Vector2>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSpookyHandVfx.PropertyName._elapsedPauseTime))
    {
      value = VariantUtils.CreateFrom<float>(ref this._elapsedPauseTime);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpookyHandVfx.PropertyName._pauseCounter))
    {
      value = VariantUtils.CreateFrom<int>(ref this._pauseCounter);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpookyHandVfx.PropertyName._isPaused))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isPaused);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpookyHandVfx.PropertyName._timer))
    {
      value = VariantUtils.CreateFrom<float>(ref this._timer);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpookyHandVfx.PropertyName._totalPauses))
    {
      value = VariantUtils.CreateFrom<int>(ref this._totalPauses);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpookyHandVfx.PropertyName._canPauseTimer))
    {
      value = VariantUtils.CreateFrom<float>(ref this._canPauseTimer);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpookyHandVfx.PropertyName._intensity))
    {
      value = VariantUtils.CreateFrom<float>(ref this._intensity);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpookyHandVfx.PropertyName._speed))
    {
      value = VariantUtils.CreateFrom<float>(ref this._speed);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpookyHandVfx.PropertyName._duration))
    {
      value = VariantUtils.CreateFrom<float>(ref this._duration);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpookyHandVfx.PropertyName._originalRotation))
    {
      value = VariantUtils.CreateFrom<float>(ref this._originalRotation);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSpookyHandVfx.PropertyName._targetScale))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Vector2>(ref this._targetScale);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 3L, NSpookyHandVfx.PropertyName._elapsedPauseTime, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NSpookyHandVfx.PropertyName._pauseCounter, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NSpookyHandVfx.PropertyName._isPaused, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NSpookyHandVfx.PropertyName._timer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NSpookyHandVfx.PropertyName._totalPauses, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NSpookyHandVfx.PropertyName._canPauseTimer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NSpookyHandVfx.PropertyName._intensity, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NSpookyHandVfx.PropertyName._speed, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NSpookyHandVfx.PropertyName._duration, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NSpookyHandVfx.PropertyName._originalRotation, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NSpookyHandVfx.PropertyName._targetScale, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NSpookyHandVfx.PropertyName._elapsedPauseTime, Variant.From<float>(ref this._elapsedPauseTime));
    info.AddProperty(NSpookyHandVfx.PropertyName._pauseCounter, Variant.From<int>(ref this._pauseCounter));
    info.AddProperty(NSpookyHandVfx.PropertyName._isPaused, Variant.From<bool>(ref this._isPaused));
    info.AddProperty(NSpookyHandVfx.PropertyName._timer, Variant.From<float>(ref this._timer));
    info.AddProperty(NSpookyHandVfx.PropertyName._totalPauses, Variant.From<int>(ref this._totalPauses));
    info.AddProperty(NSpookyHandVfx.PropertyName._canPauseTimer, Variant.From<float>(ref this._canPauseTimer));
    info.AddProperty(NSpookyHandVfx.PropertyName._intensity, Variant.From<float>(ref this._intensity));
    info.AddProperty(NSpookyHandVfx.PropertyName._speed, Variant.From<float>(ref this._speed));
    info.AddProperty(NSpookyHandVfx.PropertyName._duration, Variant.From<float>(ref this._duration));
    info.AddProperty(NSpookyHandVfx.PropertyName._originalRotation, Variant.From<float>(ref this._originalRotation));
    info.AddProperty(NSpookyHandVfx.PropertyName._targetScale, Variant.From<Vector2>(ref this._targetScale));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NSpookyHandVfx.PropertyName._elapsedPauseTime, ref variant1))
      this._elapsedPauseTime = ((Variant) ref variant1).As<float>();
    Variant variant2;
    if (info.TryGetProperty(NSpookyHandVfx.PropertyName._pauseCounter, ref variant2))
      this._pauseCounter = ((Variant) ref variant2).As<int>();
    Variant variant3;
    if (info.TryGetProperty(NSpookyHandVfx.PropertyName._isPaused, ref variant3))
      this._isPaused = ((Variant) ref variant3).As<bool>();
    Variant variant4;
    if (info.TryGetProperty(NSpookyHandVfx.PropertyName._timer, ref variant4))
      this._timer = ((Variant) ref variant4).As<float>();
    Variant variant5;
    if (info.TryGetProperty(NSpookyHandVfx.PropertyName._totalPauses, ref variant5))
      this._totalPauses = ((Variant) ref variant5).As<int>();
    Variant variant6;
    if (info.TryGetProperty(NSpookyHandVfx.PropertyName._canPauseTimer, ref variant6))
      this._canPauseTimer = ((Variant) ref variant6).As<float>();
    Variant variant7;
    if (info.TryGetProperty(NSpookyHandVfx.PropertyName._intensity, ref variant7))
      this._intensity = ((Variant) ref variant7).As<float>();
    Variant variant8;
    if (info.TryGetProperty(NSpookyHandVfx.PropertyName._speed, ref variant8))
      this._speed = ((Variant) ref variant8).As<float>();
    Variant variant9;
    if (info.TryGetProperty(NSpookyHandVfx.PropertyName._duration, ref variant9))
      this._duration = ((Variant) ref variant9).As<float>();
    Variant variant10;
    if (info.TryGetProperty(NSpookyHandVfx.PropertyName._originalRotation, ref variant10))
      this._originalRotation = ((Variant) ref variant10).As<float>();
    Variant variant11;
    if (!info.TryGetProperty(NSpookyHandVfx.PropertyName._targetScale, ref variant11))
      return;
    this._targetScale = ((Variant) ref variant11).As<Vector2>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName AnimateIn = StringName.op_Implicit(nameof (AnimateIn));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _elapsedPauseTime = StringName.op_Implicit(nameof (_elapsedPauseTime));
    public static readonly StringName _pauseCounter = StringName.op_Implicit(nameof (_pauseCounter));
    public static readonly StringName _isPaused = StringName.op_Implicit(nameof (_isPaused));
    public static readonly StringName _timer = StringName.op_Implicit(nameof (_timer));
    public static readonly StringName _totalPauses = StringName.op_Implicit(nameof (_totalPauses));
    public static readonly StringName _canPauseTimer = StringName.op_Implicit(nameof (_canPauseTimer));
    public static readonly StringName _intensity = StringName.op_Implicit(nameof (_intensity));
    public static readonly StringName _speed = StringName.op_Implicit(nameof (_speed));
    public static readonly StringName _duration = StringName.op_Implicit(nameof (_duration));
    public static readonly StringName _originalRotation = StringName.op_Implicit(nameof (_originalRotation));
    public static readonly StringName _targetScale = StringName.op_Implicit(nameof (_targetScale));
  }

  public class SignalName : Control.SignalName
  {
  }
}
