// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NDecimillipedeSegmentVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[GlobalClass]
[ScriptPath("res://src/Core/Nodes/Vfx/NDecimillipedeSegmentVfx.cs")]
public class NDecimillipedeSegmentVfx : Node
{
  private static readonly StringName _opacity = new StringName("opacity");
  private static readonly StringName _direction = new StringName("direction");
  [Export]
  private CpuParticles2D[] _damageParticleNodes;
  private readonly Vector2 _particleGravity = new Vector2(0.0f, 300f);
  private float _particleSpeedScale = 2f;
  private Vector2 _particleVelocityMinMax = new Vector2(400f, 600f);
  [Export]
  private Node2D[] _sprayNodes;
  private Node2D _parent;
  private MegaSprite _animController;
  private readonly List<Vector2> _sprayNodeScales = new List<Vector2>();

  public override void _Ready()
  {
    this._parent = this.GetParent<Node2D>();
    this._animController = new MegaSprite(Variant.op_Implicit((GodotObject) this._parent));
    this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
    foreach (Node2D sprayNode in this._sprayNodes)
    {
      ((CanvasItem) sprayNode).Visible = false;
      this._sprayNodeScales.Add(sprayNode.Scale);
    }
  }

  public void Regenerate()
  {
    for (int index = 0; index < this._sprayNodes.Length; ++index)
    {
      Node2D sprayNode = this._sprayNodes[index];
      ((CanvasItem) sprayNode).Visible = true;
      ShaderMaterial material = (ShaderMaterial) ((CanvasItem) sprayNode).Material;
      material.SetShaderParameter(NDecimillipedeSegmentVfx._direction, Variant.op_Implicit(-1));
      Tween tween = this.CreateTween().SetParallel(true);
      float num = Rng.Chaotic.NextFloat(0.5f);
      material.SetShaderParameter(NDecimillipedeSegmentVfx._opacity, Variant.op_Implicit(0.5f));
      tween.TweenProperty((GodotObject) sprayNode, NodePath.op_Implicit("scale"), Variant.op_Implicit(this._sprayNodeScales[index]), (double) num + 0.5).SetTrans((Tween.TransitionType) 5L).SetEase((Tween.EaseType) 1L);
      tween.TweenProperty((GodotObject) ((CanvasItem) sprayNode).Material, NodePath.op_Implicit("shader_parameter/opacity"), Variant.op_Implicit(0.9f), (double) num).SetTrans((Tween.TransitionType) 5L).SetEase((Tween.EaseType) 1L);
    }
  }

  private void EndRegenerate()
  {
    foreach (Node2D sprayNode in this._sprayNodes)
    {
      Vector2 scale = sprayNode.Scale;
      Tween tween = this.CreateTween();
      float num = Rng.Chaotic.NextFloat(0.9f, 1.25f);
      tween.TweenProperty((GodotObject) sprayNode, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.Zero), (double) num).SetTrans((Tween.TransitionType) 4L).SetEase((Tween.EaseType) 0L);
      tween.TweenProperty((GodotObject) sprayNode, NodePath.op_Implicit("visible"), Variant.op_Implicit(false), 0.0);
      tween.TweenProperty((GodotObject) sprayNode, NodePath.op_Implicit("scale"), Variant.op_Implicit(scale), 0.0);
    }
  }

  private void Wither()
  {
    foreach (CpuParticles2D damageParticleNode in this._damageParticleNodes)
    {
      damageParticleNode.Gravity = this._particleGravity;
      damageParticleNode.SpeedScale = (double) this._particleSpeedScale;
      damageParticleNode.InitialVelocityMin = this._particleVelocityMinMax.X;
      damageParticleNode.InitialVelocityMax = this._particleVelocityMinMax.Y;
      damageParticleNode.Restart();
    }
  }

  private void OnAnimationEvent(
    GodotObject _,
    GodotObject __,
    GodotObject ___,
    GodotObject animEvent)
  {
    switch (new MegaEvent(Variant.op_Implicit(animEvent)).GetData().GetEventName())
    {
      case "suck_complete":
        this.EndRegenerate();
        break;
      case "explode":
        this.Wither();
        break;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NDecimillipedeSegmentVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDecimillipedeSegmentVfx.MethodName.Regenerate, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDecimillipedeSegmentVfx.MethodName.EndRegenerate, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDecimillipedeSegmentVfx.MethodName.Wither, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDecimillipedeSegmentVfx.MethodName.OnAnimationEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("__"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("___"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("animEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NDecimillipedeSegmentVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDecimillipedeSegmentVfx.MethodName.Regenerate) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Regenerate();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDecimillipedeSegmentVfx.MethodName.EndRegenerate) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.EndRegenerate();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDecimillipedeSegmentVfx.MethodName.Wither) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Wither();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NDecimillipedeSegmentVfx.MethodName.OnAnimationEvent) || ((NativeVariantPtrArgs) ref args).Count != 4)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[3]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NDecimillipedeSegmentVfx.MethodName._Ready) || StringName.op_Equality(ref method, NDecimillipedeSegmentVfx.MethodName.Regenerate) || StringName.op_Equality(ref method, NDecimillipedeSegmentVfx.MethodName.EndRegenerate) || StringName.op_Equality(ref method, NDecimillipedeSegmentVfx.MethodName.Wither) || StringName.op_Equality(ref method, NDecimillipedeSegmentVfx.MethodName.OnAnimationEvent) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDecimillipedeSegmentVfx.PropertyName._damageParticleNodes))
    {
      this._damageParticleNodes = VariantUtils.ConvertToSystemArrayOfGodotObject<CpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDecimillipedeSegmentVfx.PropertyName._particleSpeedScale))
    {
      this._particleSpeedScale = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDecimillipedeSegmentVfx.PropertyName._particleVelocityMinMax))
    {
      this._particleVelocityMinMax = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDecimillipedeSegmentVfx.PropertyName._sprayNodes))
    {
      this._sprayNodes = VariantUtils.ConvertToSystemArrayOfGodotObject<Node2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDecimillipedeSegmentVfx.PropertyName._parent))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDecimillipedeSegmentVfx.PropertyName._damageParticleNodes))
    {
      value = VariantUtils.CreateFromSystemArrayOfGodotObject((GodotObject[]) this._damageParticleNodes);
      return true;
    }
    if (StringName.op_Equality(ref name, NDecimillipedeSegmentVfx.PropertyName._particleGravity))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._particleGravity);
      return true;
    }
    if (StringName.op_Equality(ref name, NDecimillipedeSegmentVfx.PropertyName._particleSpeedScale))
    {
      value = VariantUtils.CreateFrom<float>(ref this._particleSpeedScale);
      return true;
    }
    if (StringName.op_Equality(ref name, NDecimillipedeSegmentVfx.PropertyName._particleVelocityMinMax))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._particleVelocityMinMax);
      return true;
    }
    if (StringName.op_Equality(ref name, NDecimillipedeSegmentVfx.PropertyName._sprayNodes))
    {
      value = VariantUtils.CreateFromSystemArrayOfGodotObject((GodotObject[]) this._sprayNodes);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDecimillipedeSegmentVfx.PropertyName._parent))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 28L, NDecimillipedeSegmentVfx.PropertyName._damageParticleNodes, (PropertyHint) 23L, "24/34:CPUParticles2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 5L, NDecimillipedeSegmentVfx.PropertyName._particleGravity, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NDecimillipedeSegmentVfx.PropertyName._particleSpeedScale, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NDecimillipedeSegmentVfx.PropertyName._particleVelocityMinMax, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 28L, NDecimillipedeSegmentVfx.PropertyName._sprayNodes, (PropertyHint) 23L, "24/34:Node2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NDecimillipedeSegmentVfx.PropertyName._parent, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NDecimillipedeSegmentVfx.PropertyName._damageParticleNodes, Variant.CreateFrom((GodotObject[]) this._damageParticleNodes));
    info.AddProperty(NDecimillipedeSegmentVfx.PropertyName._particleSpeedScale, Variant.From<float>(ref this._particleSpeedScale));
    info.AddProperty(NDecimillipedeSegmentVfx.PropertyName._particleVelocityMinMax, Variant.From<Vector2>(ref this._particleVelocityMinMax));
    info.AddProperty(NDecimillipedeSegmentVfx.PropertyName._sprayNodes, Variant.CreateFrom((GodotObject[]) this._sprayNodes));
    info.AddProperty(NDecimillipedeSegmentVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NDecimillipedeSegmentVfx.PropertyName._damageParticleNodes, ref variant1))
      this._damageParticleNodes = ((Variant) ref variant1).AsGodotObjectArray<CpuParticles2D>();
    Variant variant2;
    if (info.TryGetProperty(NDecimillipedeSegmentVfx.PropertyName._particleSpeedScale, ref variant2))
      this._particleSpeedScale = ((Variant) ref variant2).As<float>();
    Variant variant3;
    if (info.TryGetProperty(NDecimillipedeSegmentVfx.PropertyName._particleVelocityMinMax, ref variant3))
      this._particleVelocityMinMax = ((Variant) ref variant3).As<Vector2>();
    Variant variant4;
    if (info.TryGetProperty(NDecimillipedeSegmentVfx.PropertyName._sprayNodes, ref variant4))
      this._sprayNodes = ((Variant) ref variant4).AsGodotObjectArray<Node2D>();
    Variant variant5;
    if (!info.TryGetProperty(NDecimillipedeSegmentVfx.PropertyName._parent, ref variant5))
      return;
    this._parent = ((Variant) ref variant5).As<Node2D>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName Regenerate = StringName.op_Implicit(nameof (Regenerate));
    public static readonly StringName EndRegenerate = StringName.op_Implicit(nameof (EndRegenerate));
    public static readonly StringName Wither = StringName.op_Implicit(nameof (Wither));
    public static readonly StringName OnAnimationEvent = StringName.op_Implicit(nameof (OnAnimationEvent));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName _damageParticleNodes = StringName.op_Implicit(nameof (_damageParticleNodes));
    public static readonly StringName _particleGravity = StringName.op_Implicit(nameof (_particleGravity));
    public static readonly StringName _particleSpeedScale = StringName.op_Implicit(nameof (_particleSpeedScale));
    public static readonly StringName _particleVelocityMinMax = StringName.op_Implicit(nameof (_particleVelocityMinMax));
    public static readonly StringName _sprayNodes = StringName.op_Implicit(nameof (_sprayNodes));
    public static readonly StringName _parent = StringName.op_Implicit(nameof (_parent));
  }

  public class SignalName : Node.SignalName
  {
  }
}
