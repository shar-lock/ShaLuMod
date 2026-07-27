// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NRestSiteFireVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NRestSiteFireVfx.cs")]
public class NRestSiteFireVfx : Node2D
{
  [Export]
  private float _minFlickerScale = 0.85f;
  [Export]
  private float _maxFlickerScale = 1.05f;
  [Export]
  private float _minFlickerTime = 0.3f;
  [Export]
  private float _maxFlickerTime = 0.5f;
  [Export]
  private float _minSkew = -0.1f;
  [Export]
  private float _maxSkew = 0.1f;
  [Export]
  private float _minSkewTime = 0.8f;
  [Export]
  private float _maxSkewTime = 1.5f;
  [Export]
  private float _extinguishTime = 0.2f;
  [Export]
  private bool _enabled = true;
  [Export]
  private Array<CpuParticles2D> _cpuGlowParticles = new Array<CpuParticles2D>();
  [Export]
  private Array<GpuParticles2D> _gpuSparkParticles = new Array<GpuParticles2D>();
  private Vector2 _baseScale;
  private float _baseSkew;
  private Tween? _scaleTweenRef;

  public override void _Ready()
  {
    if (!this._enabled)
      return;
    this._baseScale = this.Scale;
    this._baseSkew = this.Skew;
    this.Flicker();
    this.Sway();
  }

  private void Flicker()
  {
    Vector2 vector2_1;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2_1).\u002Ector(this._baseScale.X, Rng.Chaotic.NextFloat(this._baseScale.Y * this._minFlickerScale, this._baseScale.Y));
    Vector2 vector2_2;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2_2).\u002Ector(this._baseScale.X, Rng.Chaotic.NextFloat(this._baseScale.Y, this._baseScale.Y * this._maxFlickerScale));
    Tween tween = ((Node) this).CreateTween();
    tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(vector2_1), (double) Rng.Chaotic.NextFloat(this._minFlickerTime, this._maxFlickerTime)).SetTrans((Tween.TransitionType) 4L).SetEase((Tween.EaseType) 2L);
    tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(vector2_2), (double) Rng.Chaotic.NextFloat(this._minFlickerTime, this._maxFlickerTime)).SetTrans((Tween.TransitionType) 4L).SetEase((Tween.EaseType) 2L);
    tween.TweenCallback(Callable.From(new Action(this.Flicker)));
    this._scaleTweenRef = tween;
  }

  private void Sway()
  {
    float num1 = Rng.Chaotic.NextFloat(this._baseSkew + this._minSkew, this._baseSkew);
    float num2 = Rng.Chaotic.NextFloat(this._baseSkew, this._baseSkew + this._maxSkew);
    Tween tween = ((Node) this).CreateTween();
    tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("skew"), Variant.op_Implicit(num1), (double) Rng.Chaotic.NextFloat(this._minSkewTime, this._maxSkewTime)).SetTrans((Tween.TransitionType) 1L).SetEase((Tween.EaseType) 2L);
    tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("skew"), Variant.op_Implicit(num2), (double) Rng.Chaotic.NextFloat(this._minSkewTime, this._maxSkewTime)).SetTrans((Tween.TransitionType) 1L).SetEase((Tween.EaseType) 2L);
    tween.TweenCallback(Callable.From(new Action(this.Sway)));
  }

  public void Extinguish()
  {
    Tween tween = ((Node) this).CreateTween().SetParallel(true);
    foreach (CpuParticles2D cpuGlowParticle in this._cpuGlowParticles)
    {
      cpuGlowParticle.Emitting = false;
      tween.TweenProperty((GodotObject) cpuGlowParticle, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.Zero), (double) this._extinguishTime).SetTrans((Tween.TransitionType) 10L).SetEase((Tween.EaseType) 0L);
    }
    foreach (GpuParticles2D gpuSparkParticle in this._gpuSparkParticles)
      gpuSparkParticle.Emitting = false;
    this._scaleTweenRef?.Kill();
    ((Node) this).CreateTween().TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.Zero), (double) this._extinguishTime).SetTrans((Tween.TransitionType) 10L).SetEase((Tween.EaseType) 0L);
  }

  public override void _ExitTree() => this._scaleTweenRef?.Kill();

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NRestSiteFireVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRestSiteFireVfx.MethodName.Flicker, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRestSiteFireVfx.MethodName.Sway, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRestSiteFireVfx.MethodName.Extinguish, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRestSiteFireVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NRestSiteFireVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRestSiteFireVfx.MethodName.Flicker) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Flicker();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRestSiteFireVfx.MethodName.Sway) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Sway();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRestSiteFireVfx.MethodName.Extinguish) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Extinguish();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NRestSiteFireVfx.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._ExitTree();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NRestSiteFireVfx.MethodName._Ready) || StringName.op_Equality(ref method, NRestSiteFireVfx.MethodName.Flicker) || StringName.op_Equality(ref method, NRestSiteFireVfx.MethodName.Sway) || StringName.op_Equality(ref method, NRestSiteFireVfx.MethodName.Extinguish) || StringName.op_Equality(ref method, NRestSiteFireVfx.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRestSiteFireVfx.PropertyName._minFlickerScale))
    {
      this._minFlickerScale = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteFireVfx.PropertyName._maxFlickerScale))
    {
      this._maxFlickerScale = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteFireVfx.PropertyName._minFlickerTime))
    {
      this._minFlickerTime = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteFireVfx.PropertyName._maxFlickerTime))
    {
      this._maxFlickerTime = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteFireVfx.PropertyName._minSkew))
    {
      this._minSkew = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteFireVfx.PropertyName._maxSkew))
    {
      this._maxSkew = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteFireVfx.PropertyName._minSkewTime))
    {
      this._minSkewTime = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteFireVfx.PropertyName._maxSkewTime))
    {
      this._maxSkewTime = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteFireVfx.PropertyName._extinguishTime))
    {
      this._extinguishTime = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteFireVfx.PropertyName._enabled))
    {
      this._enabled = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteFireVfx.PropertyName._cpuGlowParticles))
    {
      this._cpuGlowParticles = VariantUtils.ConvertToArray<CpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteFireVfx.PropertyName._gpuSparkParticles))
    {
      this._gpuSparkParticles = VariantUtils.ConvertToArray<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteFireVfx.PropertyName._baseScale))
    {
      this._baseScale = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteFireVfx.PropertyName._baseSkew))
    {
      this._baseSkew = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRestSiteFireVfx.PropertyName._scaleTweenRef))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._scaleTweenRef = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRestSiteFireVfx.PropertyName._minFlickerScale))
    {
      value = VariantUtils.CreateFrom<float>(ref this._minFlickerScale);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteFireVfx.PropertyName._maxFlickerScale))
    {
      value = VariantUtils.CreateFrom<float>(ref this._maxFlickerScale);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteFireVfx.PropertyName._minFlickerTime))
    {
      value = VariantUtils.CreateFrom<float>(ref this._minFlickerTime);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteFireVfx.PropertyName._maxFlickerTime))
    {
      value = VariantUtils.CreateFrom<float>(ref this._maxFlickerTime);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteFireVfx.PropertyName._minSkew))
    {
      value = VariantUtils.CreateFrom<float>(ref this._minSkew);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteFireVfx.PropertyName._maxSkew))
    {
      value = VariantUtils.CreateFrom<float>(ref this._maxSkew);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteFireVfx.PropertyName._minSkewTime))
    {
      value = VariantUtils.CreateFrom<float>(ref this._minSkewTime);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteFireVfx.PropertyName._maxSkewTime))
    {
      value = VariantUtils.CreateFrom<float>(ref this._maxSkewTime);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteFireVfx.PropertyName._extinguishTime))
    {
      value = VariantUtils.CreateFrom<float>(ref this._extinguishTime);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteFireVfx.PropertyName._enabled))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._enabled);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteFireVfx.PropertyName._cpuGlowParticles))
    {
      value = VariantUtils.CreateFromArray<CpuParticles2D>(this._cpuGlowParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteFireVfx.PropertyName._gpuSparkParticles))
    {
      value = VariantUtils.CreateFromArray<GpuParticles2D>(this._gpuSparkParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteFireVfx.PropertyName._baseScale))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._baseScale);
      return true;
    }
    if (StringName.op_Equality(ref name, NRestSiteFireVfx.PropertyName._baseSkew))
    {
      value = VariantUtils.CreateFrom<float>(ref this._baseSkew);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRestSiteFireVfx.PropertyName._scaleTweenRef))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._scaleTweenRef);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 3L, NRestSiteFireVfx.PropertyName._minFlickerScale, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 3L, NRestSiteFireVfx.PropertyName._maxFlickerScale, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 3L, NRestSiteFireVfx.PropertyName._minFlickerTime, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 3L, NRestSiteFireVfx.PropertyName._maxFlickerTime, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 3L, NRestSiteFireVfx.PropertyName._minSkew, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 3L, NRestSiteFireVfx.PropertyName._maxSkew, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 3L, NRestSiteFireVfx.PropertyName._minSkewTime, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 3L, NRestSiteFireVfx.PropertyName._maxSkewTime, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 3L, NRestSiteFireVfx.PropertyName._extinguishTime, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 1L, NRestSiteFireVfx.PropertyName._enabled, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 28L, NRestSiteFireVfx.PropertyName._cpuGlowParticles, (PropertyHint) 23L, "24/34:CPUParticles2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 28L, NRestSiteFireVfx.PropertyName._gpuSparkParticles, (PropertyHint) 23L, "24/34:GPUParticles2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 5L, NRestSiteFireVfx.PropertyName._baseScale, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NRestSiteFireVfx.PropertyName._baseSkew, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRestSiteFireVfx.PropertyName._scaleTweenRef, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NRestSiteFireVfx.PropertyName._minFlickerScale, Variant.From<float>(ref this._minFlickerScale));
    info.AddProperty(NRestSiteFireVfx.PropertyName._maxFlickerScale, Variant.From<float>(ref this._maxFlickerScale));
    info.AddProperty(NRestSiteFireVfx.PropertyName._minFlickerTime, Variant.From<float>(ref this._minFlickerTime));
    info.AddProperty(NRestSiteFireVfx.PropertyName._maxFlickerTime, Variant.From<float>(ref this._maxFlickerTime));
    info.AddProperty(NRestSiteFireVfx.PropertyName._minSkew, Variant.From<float>(ref this._minSkew));
    info.AddProperty(NRestSiteFireVfx.PropertyName._maxSkew, Variant.From<float>(ref this._maxSkew));
    info.AddProperty(NRestSiteFireVfx.PropertyName._minSkewTime, Variant.From<float>(ref this._minSkewTime));
    info.AddProperty(NRestSiteFireVfx.PropertyName._maxSkewTime, Variant.From<float>(ref this._maxSkewTime));
    info.AddProperty(NRestSiteFireVfx.PropertyName._extinguishTime, Variant.From<float>(ref this._extinguishTime));
    info.AddProperty(NRestSiteFireVfx.PropertyName._enabled, Variant.From<bool>(ref this._enabled));
    info.AddProperty(NRestSiteFireVfx.PropertyName._cpuGlowParticles, Variant.CreateFrom<CpuParticles2D>(this._cpuGlowParticles));
    info.AddProperty(NRestSiteFireVfx.PropertyName._gpuSparkParticles, Variant.CreateFrom<GpuParticles2D>(this._gpuSparkParticles));
    info.AddProperty(NRestSiteFireVfx.PropertyName._baseScale, Variant.From<Vector2>(ref this._baseScale));
    info.AddProperty(NRestSiteFireVfx.PropertyName._baseSkew, Variant.From<float>(ref this._baseSkew));
    info.AddProperty(NRestSiteFireVfx.PropertyName._scaleTweenRef, Variant.From<Tween>(ref this._scaleTweenRef));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NRestSiteFireVfx.PropertyName._minFlickerScale, ref variant1))
      this._minFlickerScale = ((Variant) ref variant1).As<float>();
    Variant variant2;
    if (info.TryGetProperty(NRestSiteFireVfx.PropertyName._maxFlickerScale, ref variant2))
      this._maxFlickerScale = ((Variant) ref variant2).As<float>();
    Variant variant3;
    if (info.TryGetProperty(NRestSiteFireVfx.PropertyName._minFlickerTime, ref variant3))
      this._minFlickerTime = ((Variant) ref variant3).As<float>();
    Variant variant4;
    if (info.TryGetProperty(NRestSiteFireVfx.PropertyName._maxFlickerTime, ref variant4))
      this._maxFlickerTime = ((Variant) ref variant4).As<float>();
    Variant variant5;
    if (info.TryGetProperty(NRestSiteFireVfx.PropertyName._minSkew, ref variant5))
      this._minSkew = ((Variant) ref variant5).As<float>();
    Variant variant6;
    if (info.TryGetProperty(NRestSiteFireVfx.PropertyName._maxSkew, ref variant6))
      this._maxSkew = ((Variant) ref variant6).As<float>();
    Variant variant7;
    if (info.TryGetProperty(NRestSiteFireVfx.PropertyName._minSkewTime, ref variant7))
      this._minSkewTime = ((Variant) ref variant7).As<float>();
    Variant variant8;
    if (info.TryGetProperty(NRestSiteFireVfx.PropertyName._maxSkewTime, ref variant8))
      this._maxSkewTime = ((Variant) ref variant8).As<float>();
    Variant variant9;
    if (info.TryGetProperty(NRestSiteFireVfx.PropertyName._extinguishTime, ref variant9))
      this._extinguishTime = ((Variant) ref variant9).As<float>();
    Variant variant10;
    if (info.TryGetProperty(NRestSiteFireVfx.PropertyName._enabled, ref variant10))
      this._enabled = ((Variant) ref variant10).As<bool>();
    Variant variant11;
    if (info.TryGetProperty(NRestSiteFireVfx.PropertyName._cpuGlowParticles, ref variant11))
      this._cpuGlowParticles = ((Variant) ref variant11).AsGodotArray<CpuParticles2D>();
    Variant variant12;
    if (info.TryGetProperty(NRestSiteFireVfx.PropertyName._gpuSparkParticles, ref variant12))
      this._gpuSparkParticles = ((Variant) ref variant12).AsGodotArray<GpuParticles2D>();
    Variant variant13;
    if (info.TryGetProperty(NRestSiteFireVfx.PropertyName._baseScale, ref variant13))
      this._baseScale = ((Variant) ref variant13).As<Vector2>();
    Variant variant14;
    if (info.TryGetProperty(NRestSiteFireVfx.PropertyName._baseSkew, ref variant14))
      this._baseSkew = ((Variant) ref variant14).As<float>();
    Variant variant15;
    if (!info.TryGetProperty(NRestSiteFireVfx.PropertyName._scaleTweenRef, ref variant15))
      return;
    this._scaleTweenRef = ((Variant) ref variant15).As<Tween>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName Flicker = StringName.op_Implicit(nameof (Flicker));
    public static readonly StringName Sway = StringName.op_Implicit(nameof (Sway));
    public static readonly StringName Extinguish = StringName.op_Implicit(nameof (Extinguish));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _minFlickerScale = StringName.op_Implicit(nameof (_minFlickerScale));
    public static readonly StringName _maxFlickerScale = StringName.op_Implicit(nameof (_maxFlickerScale));
    public static readonly StringName _minFlickerTime = StringName.op_Implicit(nameof (_minFlickerTime));
    public static readonly StringName _maxFlickerTime = StringName.op_Implicit(nameof (_maxFlickerTime));
    public static readonly StringName _minSkew = StringName.op_Implicit(nameof (_minSkew));
    public static readonly StringName _maxSkew = StringName.op_Implicit(nameof (_maxSkew));
    public static readonly StringName _minSkewTime = StringName.op_Implicit(nameof (_minSkewTime));
    public static readonly StringName _maxSkewTime = StringName.op_Implicit(nameof (_maxSkewTime));
    public static readonly StringName _extinguishTime = StringName.op_Implicit(nameof (_extinguishTime));
    public static readonly StringName _enabled = StringName.op_Implicit(nameof (_enabled));
    public static readonly StringName _cpuGlowParticles = StringName.op_Implicit(nameof (_cpuGlowParticles));
    public static readonly StringName _gpuSparkParticles = StringName.op_Implicit(nameof (_gpuSparkParticles));
    public static readonly StringName _baseScale = StringName.op_Implicit(nameof (_baseScale));
    public static readonly StringName _baseSkew = StringName.op_Implicit(nameof (_baseSkew));
    public static readonly StringName _scaleTweenRef = StringName.op_Implicit(nameof (_scaleTweenRef));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
