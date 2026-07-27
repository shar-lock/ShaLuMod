// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NVantomVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[GlobalClass]
[ScriptPath("res://src/Core/Nodes/Vfx/NVantomVfx.cs")]
public class NVantomVfx : Node
{
  private static readonly StringName _step = new StringName("step");
  private ShaderMaterial? _tailShaderMat;
  private GpuParticles2D _sprayParticles;
  private GpuParticles2D _deathSprayParticles;
  private GpuParticles2D _deathSprayParticlesBack;
  private GpuParticles2D _deathExplosionParticles;
  private Node2D _parent;
  private MegaSprite _animController;

  public override void _Ready()
  {
    this._parent = this.GetParent<Node2D>();
    this._animController = new MegaSprite(Variant.op_Implicit((GodotObject) this._parent));
    this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
    this._animController.ConnectAnimationStarted(Callable.From<GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject>(this.OnAnimationStart)));
    this._tailShaderMat = new MegaSlotNode(Variant.op_Implicit((GodotObject) ((Node) this._parent).GetNode(NodePath.op_Implicit("TailSlotNode")))).GetNormalMaterial() as ShaderMaterial;
    this._sprayParticles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("SprayBoneNode/SprayParticles"));
    this._deathSprayParticles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("DeathSpraySlotNode/DeathSprayParticles"));
    this._deathSprayParticlesBack = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("DeathSprayBackSlotNode/DeathSprayParticlesBack"));
    this._deathExplosionParticles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("DeathExplosionSlotNode/DeathExplosionParticles"));
    this._tailShaderMat?.SetShaderParameter(NVantomVfx._step, Variant.op_Implicit(-0.1f));
    this.RunWhenSpineReady(this._animController, (Action<MegaAnimationState>) (animState =>
    {
      animState.SetAnimation("idle_loop");
      animState.SetAnimation("_tracks/charged_0", trackId: 1);
    }));
    this._sprayParticles.Emitting = false;
    this._deathSprayParticles.Emitting = false;
    this._deathSprayParticlesBack.Emitting = false;
    this._deathExplosionParticles.Emitting = false;
    this._deathExplosionParticles.OneShot = true;
  }

  private void OnAnimationEvent(
    GodotObject _,
    GodotObject __,
    GodotObject ___,
    GodotObject spineEvent)
  {
    switch (new MegaEvent(Variant.op_Implicit(spineEvent)).GetData().GetEventName())
    {
      case "dissolve_tail":
        this.DissolveTail();
        break;
      case "spray_on":
        this.StartSpray();
        break;
      case "spray_off":
        this.EndSpray();
        break;
      case "death_spray_on":
        this.StartDeathSpray();
        break;
      case "death_spray_off":
        this.EndDeathSpray();
        break;
      case "death_explosion":
        this.DeathExplode();
        break;
    }
  }

  private void OnAnimationStart(
    GodotObject spineSprite,
    GodotObject animationState,
    GodotObject trackEntry)
  {
    if (!(new MegaAnimationState(Variant.op_Implicit(animationState)).GetCurrentAnimationName() != "die"))
      return;
    this._tailShaderMat?.SetShaderParameter(NVantomVfx._step, Variant.op_Implicit(0.0f));
  }

  private void DissolveTail()
  {
    if (this._tailShaderMat == null)
      return;
    Tween tween = this.CreateTween();
    tween.SetEase((Tween.EaseType) 0L);
    tween.SetTrans((Tween.TransitionType) 4L);
    tween.TweenProperty((GodotObject) this._tailShaderMat, NodePath.op_Implicit("shader_parameter/step"), Variant.op_Implicit(1f), 1.0);
    tween.TweenCallback(Callable.From((Action) (() =>
    {
      this._animController.GetAnimationState().SetAnimation("_tracks/charge_up_1", false, 1);
      this._animController.GetAnimationState().AddAnimation("_tracks/charged_1", trackId: 1);
    })));
  }

  private void StartSpray() => this._sprayParticles.Emitting = true;

  private void EndSpray() => this._sprayParticles.Emitting = false;

  private void StartDeathSpray()
  {
    this._deathSprayParticles.Emitting = true;
    this._deathSprayParticlesBack.Emitting = true;
  }

  private void EndDeathSpray()
  {
    this._deathSprayParticles.Emitting = false;
    this._deathSprayParticlesBack.Emitting = false;
  }

  private void DeathExplode() => this._deathExplosionParticles.Restart();

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(9)
    {
      new MethodInfo(NVantomVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NVantomVfx.MethodName.OnAnimationEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("__"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("___"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("spineEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false)
      }, (List<Variant>) null),
      new MethodInfo(NVantomVfx.MethodName.OnAnimationStart, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("spineSprite"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("animationState"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("trackEntry"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false)
      }, (List<Variant>) null),
      new MethodInfo(NVantomVfx.MethodName.DissolveTail, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NVantomVfx.MethodName.StartSpray, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NVantomVfx.MethodName.EndSpray, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NVantomVfx.MethodName.StartDeathSpray, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NVantomVfx.MethodName.EndDeathSpray, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NVantomVfx.MethodName.DeathExplode, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NVantomVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NVantomVfx.MethodName.OnAnimationEvent) && ((NativeVariantPtrArgs) ref args).Count == 4)
    {
      this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[3]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NVantomVfx.MethodName.OnAnimationStart) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      this.OnAnimationStart(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NVantomVfx.MethodName.DissolveTail) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.DissolveTail();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NVantomVfx.MethodName.StartSpray) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.StartSpray();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NVantomVfx.MethodName.EndSpray) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.EndSpray();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NVantomVfx.MethodName.StartDeathSpray) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.StartDeathSpray();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NVantomVfx.MethodName.EndDeathSpray) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.EndDeathSpray();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NVantomVfx.MethodName.DeathExplode) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.DeathExplode();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NVantomVfx.MethodName._Ready) || StringName.op_Equality(ref method, NVantomVfx.MethodName.OnAnimationEvent) || StringName.op_Equality(ref method, NVantomVfx.MethodName.OnAnimationStart) || StringName.op_Equality(ref method, NVantomVfx.MethodName.DissolveTail) || StringName.op_Equality(ref method, NVantomVfx.MethodName.StartSpray) || StringName.op_Equality(ref method, NVantomVfx.MethodName.EndSpray) || StringName.op_Equality(ref method, NVantomVfx.MethodName.StartDeathSpray) || StringName.op_Equality(ref method, NVantomVfx.MethodName.EndDeathSpray) || StringName.op_Equality(ref method, NVantomVfx.MethodName.DeathExplode) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NVantomVfx.PropertyName._tailShaderMat))
    {
      this._tailShaderMat = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NVantomVfx.PropertyName._sprayParticles))
    {
      this._sprayParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NVantomVfx.PropertyName._deathSprayParticles))
    {
      this._deathSprayParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NVantomVfx.PropertyName._deathSprayParticlesBack))
    {
      this._deathSprayParticlesBack = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NVantomVfx.PropertyName._deathExplosionParticles))
    {
      this._deathExplosionParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NVantomVfx.PropertyName._parent))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NVantomVfx.PropertyName._tailShaderMat))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._tailShaderMat);
      return true;
    }
    if (StringName.op_Equality(ref name, NVantomVfx.PropertyName._sprayParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._sprayParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NVantomVfx.PropertyName._deathSprayParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._deathSprayParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NVantomVfx.PropertyName._deathSprayParticlesBack))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._deathSprayParticlesBack);
      return true;
    }
    if (StringName.op_Equality(ref name, NVantomVfx.PropertyName._deathExplosionParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._deathExplosionParticles);
      return true;
    }
    if (!StringName.op_Equality(ref name, NVantomVfx.PropertyName._parent))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NVantomVfx.PropertyName._tailShaderMat, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NVantomVfx.PropertyName._sprayParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NVantomVfx.PropertyName._deathSprayParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NVantomVfx.PropertyName._deathSprayParticlesBack, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NVantomVfx.PropertyName._deathExplosionParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NVantomVfx.PropertyName._parent, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NVantomVfx.PropertyName._tailShaderMat, Variant.From<ShaderMaterial>(ref this._tailShaderMat));
    info.AddProperty(NVantomVfx.PropertyName._sprayParticles, Variant.From<GpuParticles2D>(ref this._sprayParticles));
    info.AddProperty(NVantomVfx.PropertyName._deathSprayParticles, Variant.From<GpuParticles2D>(ref this._deathSprayParticles));
    info.AddProperty(NVantomVfx.PropertyName._deathSprayParticlesBack, Variant.From<GpuParticles2D>(ref this._deathSprayParticlesBack));
    info.AddProperty(NVantomVfx.PropertyName._deathExplosionParticles, Variant.From<GpuParticles2D>(ref this._deathExplosionParticles));
    info.AddProperty(NVantomVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NVantomVfx.PropertyName._tailShaderMat, ref variant1))
      this._tailShaderMat = ((Variant) ref variant1).As<ShaderMaterial>();
    Variant variant2;
    if (info.TryGetProperty(NVantomVfx.PropertyName._sprayParticles, ref variant2))
      this._sprayParticles = ((Variant) ref variant2).As<GpuParticles2D>();
    Variant variant3;
    if (info.TryGetProperty(NVantomVfx.PropertyName._deathSprayParticles, ref variant3))
      this._deathSprayParticles = ((Variant) ref variant3).As<GpuParticles2D>();
    Variant variant4;
    if (info.TryGetProperty(NVantomVfx.PropertyName._deathSprayParticlesBack, ref variant4))
      this._deathSprayParticlesBack = ((Variant) ref variant4).As<GpuParticles2D>();
    Variant variant5;
    if (info.TryGetProperty(NVantomVfx.PropertyName._deathExplosionParticles, ref variant5))
      this._deathExplosionParticles = ((Variant) ref variant5).As<GpuParticles2D>();
    Variant variant6;
    if (!info.TryGetProperty(NVantomVfx.PropertyName._parent, ref variant6))
      return;
    this._parent = ((Variant) ref variant6).As<Node2D>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnAnimationEvent = StringName.op_Implicit(nameof (OnAnimationEvent));
    public static readonly StringName OnAnimationStart = StringName.op_Implicit(nameof (OnAnimationStart));
    public static readonly StringName DissolveTail = StringName.op_Implicit(nameof (DissolveTail));
    public static readonly StringName StartSpray = StringName.op_Implicit(nameof (StartSpray));
    public static readonly StringName EndSpray = StringName.op_Implicit(nameof (EndSpray));
    public static readonly StringName StartDeathSpray = StringName.op_Implicit(nameof (StartDeathSpray));
    public static readonly StringName EndDeathSpray = StringName.op_Implicit(nameof (EndDeathSpray));
    public static readonly StringName DeathExplode = StringName.op_Implicit(nameof (DeathExplode));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName _tailShaderMat = StringName.op_Implicit(nameof (_tailShaderMat));
    public static readonly StringName _sprayParticles = StringName.op_Implicit(nameof (_sprayParticles));
    public static readonly StringName _deathSprayParticles = StringName.op_Implicit(nameof (_deathSprayParticles));
    public static readonly StringName _deathSprayParticlesBack = StringName.op_Implicit(nameof (_deathSprayParticlesBack));
    public static readonly StringName _deathExplosionParticles = StringName.op_Implicit(nameof (_deathExplosionParticles));
    public static readonly StringName _parent = StringName.op_Implicit(nameof (_parent));
  }

  public class SignalName : Node.SignalName
  {
  }
}
