// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NDoomSubEmitterVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.Collections;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NDoomSubEmitterVfx.cs")]
public class NDoomSubEmitterVfx : Node2D
{
  [Export]
  private Array<Node2D> _scalableLayers;
  [Export]
  private Array<TextureRect> _spears;
  [Export]
  private Node2D _verticalShrinkingLayer;
  [Export]
  private GpuParticles2D _particlesToKeepDense;
  private Array<Vector2> _baseScales;
  private Array<int> _indeces;
  private float _baseSpearRegionWidth = 140f;
  private float _dumbHackBecauseOfHowTexturerectsWork = -20f;
  private float _rotationHackForSameDumbReason = 4f;
  private int _baseParticleDensity = 300;
  private float _spearFixedHScale = 0.4f;
  private float _spearAngleIntensity = 0.15f;
  private float _minSpearSize = 1f;
  private float _maxSpearSize = 1.5f;
  private float _minSpearTime = 0.3f;
  private float _maxSpearTime = 0.6f;
  private float _outerMargin = 0.2f;
  private float _innerMargin = -0.2f;
  private double _time = 2.0;
  private bool _isOn;
  private float _curScaleX = 1f;
  private Tween? _tween;

  public float CurScaleX
  {
    get => this._curScaleX;
    set
    {
      this._curScaleX = value;
      this.UpdateWidth(this._curScaleX);
    }
  }

  public override void _Ready()
  {
    this._baseScales = new Array<Vector2>();
    this._indeces = new Array<int>();
    foreach (Node2D scalableLayer in this._scalableLayers)
    {
      this._baseScales.Add(scalableLayer.Scale);
      this._indeces.Add(((Node) scalableLayer).GetIndex(false));
    }
    this._isOn = false;
    this.SetVisibility(false);
    this.UpdateWidth(0.0f);
    this.ShowOrHide(0.0f, 0.0f);
  }

  private void FireSpear(TextureRect textureRect = null)
  {
    Vector2 position = ((Control) textureRect).Position;
    position.X = Rng.Chaotic.NextFloat(this._baseSpearRegionWidth * -0.5f, this._baseSpearRegionWidth * 0.5f) + this._dumbHackBecauseOfHowTexturerectsWork;
    ((Control) textureRect).Position = position;
    ((Control) textureRect).RotationDegrees = ((Control) textureRect).Position.X * this._spearAngleIntensity + this._rotationHackForSameDumbReason;
    ((CanvasItem) textureRect).Modulate = Colors.Transparent;
    float num1 = this._spearFixedHScale / this._curScaleX;
    ((Control) textureRect).Scale = new Vector2(num1, Rng.Chaotic.NextFloat(this._minSpearSize, this._maxSpearSize));
    Vector2 vector2;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2).\u002Ector(num1, 0.0f);
    float num2 = Rng.Chaotic.NextFloat(this._minSpearTime, this._maxSpearTime);
    Tween tween1 = ((Node) textureRect).CreateTween();
    tween1.TweenProperty((GodotObject) textureRect, NodePath.op_Implicit("scale"), Variant.op_Implicit(vector2), (double) num2).From(Variant.op_Implicit(new Vector2(num1, Rng.Chaotic.NextFloat(this._minSpearSize, this._maxSpearSize))));
    if (this._isOn)
      tween1.TweenCallback(Callable.From((Action) (() => this.FireSpear(textureRect))));
    Tween tween2 = ((Node) textureRect).CreateTween();
    float num3 = Rng.Chaotic.NextFloat(0.4f, 0.7f);
    tween2.TweenProperty((GodotObject) textureRect, NodePath.op_Implicit("modulate"), Variant.op_Implicit(new Color(1f, 1f, 1f, num3)), 0.20000000298023224).SetEase((Tween.EaseType) 0L).SetTrans((Tween.TransitionType) 4L);
  }

  private void FireAllSpears()
  {
    foreach (TextureRect spear in this._spears)
      this.FireSpear(spear);
  }

  public void ShowOrHide(float widthScale, float tweenTime)
  {
    this._isOn = (double) widthScale > 0.10000000149011612;
    float num1 = this._isOn ? 0.0f : 0.3f;
    float num2 = widthScale;
    Tween.EaseType easeType;
    if (this._isOn)
    {
      this.SetVisibility(true);
      easeType = (Tween.EaseType) 1L;
      this._particlesToKeepDense.Amount = (int) widthScale * this._baseParticleDensity > 1 ? (int) widthScale * this._baseParticleDensity : 1;
    }
    else
    {
      easeType = (Tween.EaseType) 0L;
      num2 = 0.0f;
      tweenTime = this._curScaleX * 0.15f;
    }
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween();
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("CurScaleX"), Variant.op_Implicit(num2), (double) tweenTime).SetDelay((double) num1).SetEase(easeType).SetTrans((Tween.TransitionType) 7L);
    if (this._isOn)
      return;
    this._tween.TweenCallback(Callable.From((Action) (() => this.SetVisibility(false))));
  }

  private void UpdateWidth(float width)
  {
    this._curScaleX = width;
    int num = 0;
    foreach (Node2D scalableLayer in this._scalableLayers)
    {
      Vector2 baseScale = this._baseScales[num];
      Vector2 vector2;
      if (scalableLayer == this._verticalShrinkingLayer && (double) width < 1.0)
      {
        // ISSUE: explicit constructor call
        ((Vector2) ref vector2).\u002Ector(baseScale.X * this._curScaleX, (float) ((double) baseScale.Y * (double) width / 1.0));
      }
      else
      {
        // ISSUE: explicit constructor call
        ((Vector2) ref vector2).\u002Ector(baseScale.X * this._curScaleX, baseScale.Y);
      }
      scalableLayer.Scale = vector2;
      ++num;
    }
  }

  private void SetVisibility(bool isOn)
  {
    int num = 0;
    foreach (Node2D scalableLayer in this._scalableLayers)
    {
      if (scalableLayer is GpuParticles2D gpuParticles2D)
      {
        if (isOn)
          gpuParticles2D.Restart();
        else
          gpuParticles2D.Emitting = false;
      }
      else if (isOn)
        ((Node) this).MoveChildSafely((Node) scalableLayer, this._indeces[num]);
      ++num;
    }
    if (!isOn)
      return;
    this.FireAllSpears();
  }

  public override void _ExitTree() => this._tween?.Kill();

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(7)
    {
      new MethodInfo(NDoomSubEmitterVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDoomSubEmitterVfx.MethodName.FireSpear, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("textureRect"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("TextureRect"), false)
      }, (List<Variant>) null),
      new MethodInfo(NDoomSubEmitterVfx.MethodName.FireAllSpears, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDoomSubEmitterVfx.MethodName.ShowOrHide, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("widthScale"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("tweenTime"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NDoomSubEmitterVfx.MethodName.UpdateWidth, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("width"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NDoomSubEmitterVfx.MethodName.SetVisibility, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("isOn"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NDoomSubEmitterVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NDoomSubEmitterVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDoomSubEmitterVfx.MethodName.FireSpear) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.FireSpear(VariantUtils.ConvertTo<TextureRect>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDoomSubEmitterVfx.MethodName.FireAllSpears) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.FireAllSpears();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDoomSubEmitterVfx.MethodName.ShowOrHide) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.ShowOrHide(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDoomSubEmitterVfx.MethodName.UpdateWidth) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateWidth(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDoomSubEmitterVfx.MethodName.SetVisibility) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetVisibility(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NDoomSubEmitterVfx.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._ExitTree();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NDoomSubEmitterVfx.MethodName._Ready) || StringName.op_Equality(ref method, NDoomSubEmitterVfx.MethodName.FireSpear) || StringName.op_Equality(ref method, NDoomSubEmitterVfx.MethodName.FireAllSpears) || StringName.op_Equality(ref method, NDoomSubEmitterVfx.MethodName.ShowOrHide) || StringName.op_Equality(ref method, NDoomSubEmitterVfx.MethodName.UpdateWidth) || StringName.op_Equality(ref method, NDoomSubEmitterVfx.MethodName.SetVisibility) || StringName.op_Equality(ref method, NDoomSubEmitterVfx.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDoomSubEmitterVfx.PropertyName.CurScaleX))
    {
      this.CurScaleX = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomSubEmitterVfx.PropertyName._scalableLayers))
    {
      this._scalableLayers = VariantUtils.ConvertToArray<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomSubEmitterVfx.PropertyName._spears))
    {
      this._spears = VariantUtils.ConvertToArray<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomSubEmitterVfx.PropertyName._verticalShrinkingLayer))
    {
      this._verticalShrinkingLayer = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomSubEmitterVfx.PropertyName._particlesToKeepDense))
    {
      this._particlesToKeepDense = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomSubEmitterVfx.PropertyName._baseScales))
    {
      this._baseScales = VariantUtils.ConvertToArray<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomSubEmitterVfx.PropertyName._indeces))
    {
      this._indeces = VariantUtils.ConvertToArray<int>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomSubEmitterVfx.PropertyName._baseSpearRegionWidth))
    {
      this._baseSpearRegionWidth = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomSubEmitterVfx.PropertyName._dumbHackBecauseOfHowTexturerectsWork))
    {
      this._dumbHackBecauseOfHowTexturerectsWork = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomSubEmitterVfx.PropertyName._rotationHackForSameDumbReason))
    {
      this._rotationHackForSameDumbReason = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomSubEmitterVfx.PropertyName._baseParticleDensity))
    {
      this._baseParticleDensity = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomSubEmitterVfx.PropertyName._spearFixedHScale))
    {
      this._spearFixedHScale = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomSubEmitterVfx.PropertyName._spearAngleIntensity))
    {
      this._spearAngleIntensity = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomSubEmitterVfx.PropertyName._minSpearSize))
    {
      this._minSpearSize = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomSubEmitterVfx.PropertyName._maxSpearSize))
    {
      this._maxSpearSize = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomSubEmitterVfx.PropertyName._minSpearTime))
    {
      this._minSpearTime = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomSubEmitterVfx.PropertyName._maxSpearTime))
    {
      this._maxSpearTime = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomSubEmitterVfx.PropertyName._outerMargin))
    {
      this._outerMargin = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomSubEmitterVfx.PropertyName._innerMargin))
    {
      this._innerMargin = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomSubEmitterVfx.PropertyName._time))
    {
      this._time = VariantUtils.ConvertTo<double>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomSubEmitterVfx.PropertyName._isOn))
    {
      this._isOn = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomSubEmitterVfx.PropertyName._curScaleX))
    {
      this._curScaleX = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDoomSubEmitterVfx.PropertyName._tween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._tween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDoomSubEmitterVfx.PropertyName.CurScaleX))
    {
      ref godot_variant local = ref value;
      float curScaleX = this.CurScaleX;
      godot_variant from = VariantUtils.CreateFrom<float>(ref curScaleX);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomSubEmitterVfx.PropertyName._scalableLayers))
    {
      value = VariantUtils.CreateFromArray<Node2D>(this._scalableLayers);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomSubEmitterVfx.PropertyName._spears))
    {
      value = VariantUtils.CreateFromArray<TextureRect>(this._spears);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomSubEmitterVfx.PropertyName._verticalShrinkingLayer))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._verticalShrinkingLayer);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomSubEmitterVfx.PropertyName._particlesToKeepDense))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._particlesToKeepDense);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomSubEmitterVfx.PropertyName._baseScales))
    {
      value = VariantUtils.CreateFromArray<Vector2>(this._baseScales);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomSubEmitterVfx.PropertyName._indeces))
    {
      value = VariantUtils.CreateFromArray<int>(this._indeces);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomSubEmitterVfx.PropertyName._baseSpearRegionWidth))
    {
      value = VariantUtils.CreateFrom<float>(ref this._baseSpearRegionWidth);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomSubEmitterVfx.PropertyName._dumbHackBecauseOfHowTexturerectsWork))
    {
      value = VariantUtils.CreateFrom<float>(ref this._dumbHackBecauseOfHowTexturerectsWork);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomSubEmitterVfx.PropertyName._rotationHackForSameDumbReason))
    {
      value = VariantUtils.CreateFrom<float>(ref this._rotationHackForSameDumbReason);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomSubEmitterVfx.PropertyName._baseParticleDensity))
    {
      value = VariantUtils.CreateFrom<int>(ref this._baseParticleDensity);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomSubEmitterVfx.PropertyName._spearFixedHScale))
    {
      value = VariantUtils.CreateFrom<float>(ref this._spearFixedHScale);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomSubEmitterVfx.PropertyName._spearAngleIntensity))
    {
      value = VariantUtils.CreateFrom<float>(ref this._spearAngleIntensity);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomSubEmitterVfx.PropertyName._minSpearSize))
    {
      value = VariantUtils.CreateFrom<float>(ref this._minSpearSize);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomSubEmitterVfx.PropertyName._maxSpearSize))
    {
      value = VariantUtils.CreateFrom<float>(ref this._maxSpearSize);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomSubEmitterVfx.PropertyName._minSpearTime))
    {
      value = VariantUtils.CreateFrom<float>(ref this._minSpearTime);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomSubEmitterVfx.PropertyName._maxSpearTime))
    {
      value = VariantUtils.CreateFrom<float>(ref this._maxSpearTime);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomSubEmitterVfx.PropertyName._outerMargin))
    {
      value = VariantUtils.CreateFrom<float>(ref this._outerMargin);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomSubEmitterVfx.PropertyName._innerMargin))
    {
      value = VariantUtils.CreateFrom<float>(ref this._innerMargin);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomSubEmitterVfx.PropertyName._time))
    {
      value = VariantUtils.CreateFrom<double>(ref this._time);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomSubEmitterVfx.PropertyName._isOn))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isOn);
      return true;
    }
    if (StringName.op_Equality(ref name, NDoomSubEmitterVfx.PropertyName._curScaleX))
    {
      value = VariantUtils.CreateFrom<float>(ref this._curScaleX);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDoomSubEmitterVfx.PropertyName._tween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._tween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 28L, NDoomSubEmitterVfx.PropertyName._scalableLayers, (PropertyHint) 23L, "24/34:Node2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 28L, NDoomSubEmitterVfx.PropertyName._spears, (PropertyHint) 23L, "24/34:TextureRect", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NDoomSubEmitterVfx.PropertyName._verticalShrinkingLayer, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NDoomSubEmitterVfx.PropertyName._particlesToKeepDense, (PropertyHint) 34L, "GPUParticles2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 28L, NDoomSubEmitterVfx.PropertyName._baseScales, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 28L, NDoomSubEmitterVfx.PropertyName._indeces, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NDoomSubEmitterVfx.PropertyName._baseSpearRegionWidth, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NDoomSubEmitterVfx.PropertyName._dumbHackBecauseOfHowTexturerectsWork, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NDoomSubEmitterVfx.PropertyName._rotationHackForSameDumbReason, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NDoomSubEmitterVfx.PropertyName._baseParticleDensity, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NDoomSubEmitterVfx.PropertyName._spearFixedHScale, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NDoomSubEmitterVfx.PropertyName._spearAngleIntensity, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NDoomSubEmitterVfx.PropertyName._minSpearSize, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NDoomSubEmitterVfx.PropertyName._maxSpearSize, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NDoomSubEmitterVfx.PropertyName._minSpearTime, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NDoomSubEmitterVfx.PropertyName._maxSpearTime, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NDoomSubEmitterVfx.PropertyName._outerMargin, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NDoomSubEmitterVfx.PropertyName._innerMargin, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NDoomSubEmitterVfx.PropertyName._time, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NDoomSubEmitterVfx.PropertyName._isOn, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NDoomSubEmitterVfx.PropertyName._curScaleX, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NDoomSubEmitterVfx.PropertyName.CurScaleX, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDoomSubEmitterVfx.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName curScaleX1 = NDoomSubEmitterVfx.PropertyName.CurScaleX;
    float curScaleX2 = this.CurScaleX;
    Variant variant = Variant.From<float>(ref curScaleX2);
    serializationInfo.AddProperty(curScaleX1, variant);
    info.AddProperty(NDoomSubEmitterVfx.PropertyName._scalableLayers, Variant.CreateFrom<Node2D>(this._scalableLayers));
    info.AddProperty(NDoomSubEmitterVfx.PropertyName._spears, Variant.CreateFrom<TextureRect>(this._spears));
    info.AddProperty(NDoomSubEmitterVfx.PropertyName._verticalShrinkingLayer, Variant.From<Node2D>(ref this._verticalShrinkingLayer));
    info.AddProperty(NDoomSubEmitterVfx.PropertyName._particlesToKeepDense, Variant.From<GpuParticles2D>(ref this._particlesToKeepDense));
    info.AddProperty(NDoomSubEmitterVfx.PropertyName._baseScales, Variant.CreateFrom<Vector2>(this._baseScales));
    info.AddProperty(NDoomSubEmitterVfx.PropertyName._indeces, Variant.CreateFrom<int>(this._indeces));
    info.AddProperty(NDoomSubEmitterVfx.PropertyName._baseSpearRegionWidth, Variant.From<float>(ref this._baseSpearRegionWidth));
    info.AddProperty(NDoomSubEmitterVfx.PropertyName._dumbHackBecauseOfHowTexturerectsWork, Variant.From<float>(ref this._dumbHackBecauseOfHowTexturerectsWork));
    info.AddProperty(NDoomSubEmitterVfx.PropertyName._rotationHackForSameDumbReason, Variant.From<float>(ref this._rotationHackForSameDumbReason));
    info.AddProperty(NDoomSubEmitterVfx.PropertyName._baseParticleDensity, Variant.From<int>(ref this._baseParticleDensity));
    info.AddProperty(NDoomSubEmitterVfx.PropertyName._spearFixedHScale, Variant.From<float>(ref this._spearFixedHScale));
    info.AddProperty(NDoomSubEmitterVfx.PropertyName._spearAngleIntensity, Variant.From<float>(ref this._spearAngleIntensity));
    info.AddProperty(NDoomSubEmitterVfx.PropertyName._minSpearSize, Variant.From<float>(ref this._minSpearSize));
    info.AddProperty(NDoomSubEmitterVfx.PropertyName._maxSpearSize, Variant.From<float>(ref this._maxSpearSize));
    info.AddProperty(NDoomSubEmitterVfx.PropertyName._minSpearTime, Variant.From<float>(ref this._minSpearTime));
    info.AddProperty(NDoomSubEmitterVfx.PropertyName._maxSpearTime, Variant.From<float>(ref this._maxSpearTime));
    info.AddProperty(NDoomSubEmitterVfx.PropertyName._outerMargin, Variant.From<float>(ref this._outerMargin));
    info.AddProperty(NDoomSubEmitterVfx.PropertyName._innerMargin, Variant.From<float>(ref this._innerMargin));
    info.AddProperty(NDoomSubEmitterVfx.PropertyName._time, Variant.From<double>(ref this._time));
    info.AddProperty(NDoomSubEmitterVfx.PropertyName._isOn, Variant.From<bool>(ref this._isOn));
    info.AddProperty(NDoomSubEmitterVfx.PropertyName._curScaleX, Variant.From<float>(ref this._curScaleX));
    info.AddProperty(NDoomSubEmitterVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NDoomSubEmitterVfx.PropertyName.CurScaleX, ref variant1))
      this.CurScaleX = ((Variant) ref variant1).As<float>();
    Variant variant2;
    if (info.TryGetProperty(NDoomSubEmitterVfx.PropertyName._scalableLayers, ref variant2))
      this._scalableLayers = ((Variant) ref variant2).AsGodotArray<Node2D>();
    Variant variant3;
    if (info.TryGetProperty(NDoomSubEmitterVfx.PropertyName._spears, ref variant3))
      this._spears = ((Variant) ref variant3).AsGodotArray<TextureRect>();
    Variant variant4;
    if (info.TryGetProperty(NDoomSubEmitterVfx.PropertyName._verticalShrinkingLayer, ref variant4))
      this._verticalShrinkingLayer = ((Variant) ref variant4).As<Node2D>();
    Variant variant5;
    if (info.TryGetProperty(NDoomSubEmitterVfx.PropertyName._particlesToKeepDense, ref variant5))
      this._particlesToKeepDense = ((Variant) ref variant5).As<GpuParticles2D>();
    Variant variant6;
    if (info.TryGetProperty(NDoomSubEmitterVfx.PropertyName._baseScales, ref variant6))
      this._baseScales = ((Variant) ref variant6).AsGodotArray<Vector2>();
    Variant variant7;
    if (info.TryGetProperty(NDoomSubEmitterVfx.PropertyName._indeces, ref variant7))
      this._indeces = ((Variant) ref variant7).AsGodotArray<int>();
    Variant variant8;
    if (info.TryGetProperty(NDoomSubEmitterVfx.PropertyName._baseSpearRegionWidth, ref variant8))
      this._baseSpearRegionWidth = ((Variant) ref variant8).As<float>();
    Variant variant9;
    if (info.TryGetProperty(NDoomSubEmitterVfx.PropertyName._dumbHackBecauseOfHowTexturerectsWork, ref variant9))
      this._dumbHackBecauseOfHowTexturerectsWork = ((Variant) ref variant9).As<float>();
    Variant variant10;
    if (info.TryGetProperty(NDoomSubEmitterVfx.PropertyName._rotationHackForSameDumbReason, ref variant10))
      this._rotationHackForSameDumbReason = ((Variant) ref variant10).As<float>();
    Variant variant11;
    if (info.TryGetProperty(NDoomSubEmitterVfx.PropertyName._baseParticleDensity, ref variant11))
      this._baseParticleDensity = ((Variant) ref variant11).As<int>();
    Variant variant12;
    if (info.TryGetProperty(NDoomSubEmitterVfx.PropertyName._spearFixedHScale, ref variant12))
      this._spearFixedHScale = ((Variant) ref variant12).As<float>();
    Variant variant13;
    if (info.TryGetProperty(NDoomSubEmitterVfx.PropertyName._spearAngleIntensity, ref variant13))
      this._spearAngleIntensity = ((Variant) ref variant13).As<float>();
    Variant variant14;
    if (info.TryGetProperty(NDoomSubEmitterVfx.PropertyName._minSpearSize, ref variant14))
      this._minSpearSize = ((Variant) ref variant14).As<float>();
    Variant variant15;
    if (info.TryGetProperty(NDoomSubEmitterVfx.PropertyName._maxSpearSize, ref variant15))
      this._maxSpearSize = ((Variant) ref variant15).As<float>();
    Variant variant16;
    if (info.TryGetProperty(NDoomSubEmitterVfx.PropertyName._minSpearTime, ref variant16))
      this._minSpearTime = ((Variant) ref variant16).As<float>();
    Variant variant17;
    if (info.TryGetProperty(NDoomSubEmitterVfx.PropertyName._maxSpearTime, ref variant17))
      this._maxSpearTime = ((Variant) ref variant17).As<float>();
    Variant variant18;
    if (info.TryGetProperty(NDoomSubEmitterVfx.PropertyName._outerMargin, ref variant18))
      this._outerMargin = ((Variant) ref variant18).As<float>();
    Variant variant19;
    if (info.TryGetProperty(NDoomSubEmitterVfx.PropertyName._innerMargin, ref variant19))
      this._innerMargin = ((Variant) ref variant19).As<float>();
    Variant variant20;
    if (info.TryGetProperty(NDoomSubEmitterVfx.PropertyName._time, ref variant20))
      this._time = ((Variant) ref variant20).As<double>();
    Variant variant21;
    if (info.TryGetProperty(NDoomSubEmitterVfx.PropertyName._isOn, ref variant21))
      this._isOn = ((Variant) ref variant21).As<bool>();
    Variant variant22;
    if (info.TryGetProperty(NDoomSubEmitterVfx.PropertyName._curScaleX, ref variant22))
      this._curScaleX = ((Variant) ref variant22).As<float>();
    Variant variant23;
    if (!info.TryGetProperty(NDoomSubEmitterVfx.PropertyName._tween, ref variant23))
      return;
    this._tween = ((Variant) ref variant23).As<Tween>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName FireSpear = StringName.op_Implicit(nameof (FireSpear));
    public static readonly StringName FireAllSpears = StringName.op_Implicit(nameof (FireAllSpears));
    public static readonly StringName ShowOrHide = StringName.op_Implicit(nameof (ShowOrHide));
    public static readonly StringName UpdateWidth = StringName.op_Implicit(nameof (UpdateWidth));
    public static readonly StringName SetVisibility = StringName.op_Implicit(nameof (SetVisibility));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName CurScaleX = StringName.op_Implicit(nameof (CurScaleX));
    public static readonly StringName _scalableLayers = StringName.op_Implicit(nameof (_scalableLayers));
    public static readonly StringName _spears = StringName.op_Implicit(nameof (_spears));
    public static readonly StringName _verticalShrinkingLayer = StringName.op_Implicit(nameof (_verticalShrinkingLayer));
    public static readonly StringName _particlesToKeepDense = StringName.op_Implicit(nameof (_particlesToKeepDense));
    public static readonly StringName _baseScales = StringName.op_Implicit(nameof (_baseScales));
    public static readonly StringName _indeces = StringName.op_Implicit(nameof (_indeces));
    public static readonly StringName _baseSpearRegionWidth = StringName.op_Implicit(nameof (_baseSpearRegionWidth));
    public static readonly StringName _dumbHackBecauseOfHowTexturerectsWork = StringName.op_Implicit(nameof (_dumbHackBecauseOfHowTexturerectsWork));
    public static readonly StringName _rotationHackForSameDumbReason = StringName.op_Implicit(nameof (_rotationHackForSameDumbReason));
    public static readonly StringName _baseParticleDensity = StringName.op_Implicit(nameof (_baseParticleDensity));
    public static readonly StringName _spearFixedHScale = StringName.op_Implicit(nameof (_spearFixedHScale));
    public static readonly StringName _spearAngleIntensity = StringName.op_Implicit(nameof (_spearAngleIntensity));
    public static readonly StringName _minSpearSize = StringName.op_Implicit(nameof (_minSpearSize));
    public static readonly StringName _maxSpearSize = StringName.op_Implicit(nameof (_maxSpearSize));
    public static readonly StringName _minSpearTime = StringName.op_Implicit(nameof (_minSpearTime));
    public static readonly StringName _maxSpearTime = StringName.op_Implicit(nameof (_maxSpearTime));
    public static readonly StringName _outerMargin = StringName.op_Implicit(nameof (_outerMargin));
    public static readonly StringName _innerMargin = StringName.op_Implicit(nameof (_innerMargin));
    public static readonly StringName _time = StringName.op_Implicit(nameof (_time));
    public static readonly StringName _isOn = StringName.op_Implicit(nameof (_isOn));
    public static readonly StringName _curScaleX = StringName.op_Implicit(nameof (_curScaleX));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
