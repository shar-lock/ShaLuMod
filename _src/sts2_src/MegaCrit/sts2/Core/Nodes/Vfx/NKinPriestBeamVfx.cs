// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NKinPriestBeamVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NKinPriestBeamVfx.cs")]
public class NKinPriestBeamVfx : Node2D
{
  private const float _beamMaxLengthScale = 4f;
  private const float _startRotation = 1f;
  private const float _endRotation = -1f;
  private Sprite2D _beam;
  private Node2D _beamHolder;
  private GpuParticles2D _staticParticles;
  private Vector2 _baseBeamScale;
  private Tween? _lengthTween;
  private Tween? _rotationTween;

  public override void _Ready()
  {
    this._beam = ((Node) this).GetNode<Sprite2D>(NodePath.op_Implicit("BeamHolder/Beam"));
    this._staticParticles = ((Node) this).GetNode<GpuParticles2D>(NodePath.op_Implicit("BeamHolder/StaticParticles"));
    this._beamHolder = ((Node) this).GetNode<Node2D>(NodePath.op_Implicit("BeamHolder"));
    this._baseBeamScale = ((Node2D) this._beam).Scale;
    this._staticParticles.Emitting = false;
    ((CanvasItem) this._staticParticles).Visible = false;
    ((CanvasItem) this._beamHolder).Visible = false;
  }

  public override void _Process(double delta)
  {
    Vector2 vector2;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2).\u002Ector(Rng.Chaotic.NextFloat(-0.05f, 0.05f), Rng.Chaotic.NextFloat(-0.7f, 0.7f));
    ((Node2D) this._beam).Scale = Vector2.op_Addition(this._baseBeamScale, vector2);
    Color modulate = ((CanvasItem) this).Modulate;
    modulate.A = Rng.Chaotic.NextFloat(0.8f, 1f);
    ((CanvasItem) this).Modulate = modulate;
  }

  public void Fire()
  {
    this._staticParticles.Restart();
    ((CanvasItem) this._staticParticles).Visible = true;
    ((CanvasItem) this._beamHolder).Visible = true;
    this._rotationTween = ((Node) this).CreateTween();
    this.RotationDegrees = 1f;
    this._rotationTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("rotation_degrees"), Variant.op_Implicit(-1f), 0.800000011920929).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    this._beamHolder.Scale = Vector2.One;
    this._lengthTween = ((Node) this).CreateTween();
    this._lengthTween.TweenProperty((GodotObject) this._beamHolder, NodePath.op_Implicit("scale:x"), Variant.op_Implicit(4f), 0.37999999523162842).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._lengthTween.Chain().TweenProperty((GodotObject) this._beamHolder, NodePath.op_Implicit("scale:x"), Variant.op_Implicit(0.5), 0.60000002384185791).SetEase((Tween.EaseType) 0L).SetTrans((Tween.TransitionType) 5L);
    this._lengthTween.TweenCallback(Callable.From(new Action(this.OnTweenComplete)));
  }

  private void OnTweenComplete()
  {
    this._rotationTween.Kill();
    this._lengthTween.Kill();
    this._staticParticles.Emitting = false;
    ((CanvasItem) this._staticParticles).Visible = false;
    ((CanvasItem) this._beamHolder).Visible = false;
  }

  public override void _ExitTree()
  {
    this._lengthTween?.Kill();
    this._rotationTween?.Kill();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NKinPriestBeamVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NKinPriestBeamVfx.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NKinPriestBeamVfx.MethodName.Fire, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NKinPriestBeamVfx.MethodName.OnTweenComplete, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NKinPriestBeamVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NKinPriestBeamVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NKinPriestBeamVfx.MethodName._Process) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NKinPriestBeamVfx.MethodName.Fire) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Fire();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NKinPriestBeamVfx.MethodName.OnTweenComplete) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnTweenComplete();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NKinPriestBeamVfx.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._ExitTree();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NKinPriestBeamVfx.MethodName._Ready) || StringName.op_Equality(ref method, NKinPriestBeamVfx.MethodName._Process) || StringName.op_Equality(ref method, NKinPriestBeamVfx.MethodName.Fire) || StringName.op_Equality(ref method, NKinPriestBeamVfx.MethodName.OnTweenComplete) || StringName.op_Equality(ref method, NKinPriestBeamVfx.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NKinPriestBeamVfx.PropertyName._beam))
    {
      this._beam = VariantUtils.ConvertTo<Sprite2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NKinPriestBeamVfx.PropertyName._beamHolder))
    {
      this._beamHolder = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NKinPriestBeamVfx.PropertyName._staticParticles))
    {
      this._staticParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NKinPriestBeamVfx.PropertyName._baseBeamScale))
    {
      this._baseBeamScale = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NKinPriestBeamVfx.PropertyName._lengthTween))
    {
      this._lengthTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NKinPriestBeamVfx.PropertyName._rotationTween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._rotationTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NKinPriestBeamVfx.PropertyName._beam))
    {
      value = VariantUtils.CreateFrom<Sprite2D>(ref this._beam);
      return true;
    }
    if (StringName.op_Equality(ref name, NKinPriestBeamVfx.PropertyName._beamHolder))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._beamHolder);
      return true;
    }
    if (StringName.op_Equality(ref name, NKinPriestBeamVfx.PropertyName._staticParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._staticParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NKinPriestBeamVfx.PropertyName._baseBeamScale))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._baseBeamScale);
      return true;
    }
    if (StringName.op_Equality(ref name, NKinPriestBeamVfx.PropertyName._lengthTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._lengthTween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NKinPriestBeamVfx.PropertyName._rotationTween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._rotationTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NKinPriestBeamVfx.PropertyName._beam, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NKinPriestBeamVfx.PropertyName._beamHolder, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NKinPriestBeamVfx.PropertyName._staticParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NKinPriestBeamVfx.PropertyName._baseBeamScale, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NKinPriestBeamVfx.PropertyName._lengthTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NKinPriestBeamVfx.PropertyName._rotationTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NKinPriestBeamVfx.PropertyName._beam, Variant.From<Sprite2D>(ref this._beam));
    info.AddProperty(NKinPriestBeamVfx.PropertyName._beamHolder, Variant.From<Node2D>(ref this._beamHolder));
    info.AddProperty(NKinPriestBeamVfx.PropertyName._staticParticles, Variant.From<GpuParticles2D>(ref this._staticParticles));
    info.AddProperty(NKinPriestBeamVfx.PropertyName._baseBeamScale, Variant.From<Vector2>(ref this._baseBeamScale));
    info.AddProperty(NKinPriestBeamVfx.PropertyName._lengthTween, Variant.From<Tween>(ref this._lengthTween));
    info.AddProperty(NKinPriestBeamVfx.PropertyName._rotationTween, Variant.From<Tween>(ref this._rotationTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NKinPriestBeamVfx.PropertyName._beam, ref variant1))
      this._beam = ((Variant) ref variant1).As<Sprite2D>();
    Variant variant2;
    if (info.TryGetProperty(NKinPriestBeamVfx.PropertyName._beamHolder, ref variant2))
      this._beamHolder = ((Variant) ref variant2).As<Node2D>();
    Variant variant3;
    if (info.TryGetProperty(NKinPriestBeamVfx.PropertyName._staticParticles, ref variant3))
      this._staticParticles = ((Variant) ref variant3).As<GpuParticles2D>();
    Variant variant4;
    if (info.TryGetProperty(NKinPriestBeamVfx.PropertyName._baseBeamScale, ref variant4))
      this._baseBeamScale = ((Variant) ref variant4).As<Vector2>();
    Variant variant5;
    if (info.TryGetProperty(NKinPriestBeamVfx.PropertyName._lengthTween, ref variant5))
      this._lengthTween = ((Variant) ref variant5).As<Tween>();
    Variant variant6;
    if (!info.TryGetProperty(NKinPriestBeamVfx.PropertyName._rotationTween, ref variant6))
      return;
    this._rotationTween = ((Variant) ref variant6).As<Tween>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
    public static readonly StringName Fire = StringName.op_Implicit(nameof (Fire));
    public static readonly StringName OnTweenComplete = StringName.op_Implicit(nameof (OnTweenComplete));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _beam = StringName.op_Implicit(nameof (_beam));
    public static readonly StringName _beamHolder = StringName.op_Implicit(nameof (_beamHolder));
    public static readonly StringName _staticParticles = StringName.op_Implicit(nameof (_staticParticles));
    public static readonly StringName _baseBeamScale = StringName.op_Implicit(nameof (_baseBeamScale));
    public static readonly StringName _lengthTween = StringName.op_Implicit(nameof (_lengthTween));
    public static readonly StringName _rotationTween = StringName.op_Implicit(nameof (_rotationTween));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
