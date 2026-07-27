// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NKinPriestVfx
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
[ScriptPath("res://src/Core/Nodes/Vfx/NKinPriestVfx.cs")]
public class NKinPriestVfx : Node
{
  private GpuParticles2D _sparkParticles;
  private NKinPriestBeamVfx _beamVfx;
  private Node2D _parent;
  private MegaSprite _animController;

  public override void _Ready()
  {
    this._parent = this.GetParent<Node2D>();
    this._animController = new MegaSprite(Variant.op_Implicit((GodotObject) this._parent));
    this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
    this._sparkParticles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("TorchFireBone/SparkParticles"));
    this._sparkParticles.Emitting = false;
    this._beamVfx = ((Node) this._parent).GetNode<NKinPriestBeamVfx>(NodePath.op_Implicit("Beam"));
    this.RunWhenSpineReady(this._animController, (Action<MegaAnimationState>) (animState => animState.SetAnimation("attack_laser")));
  }

  private void OnAnimationEvent(
    GodotObject _,
    GodotObject __,
    GodotObject ___,
    GodotObject spineEvent)
  {
    switch (new MegaEvent(Variant.op_Implicit(spineEvent)).GetData().GetEventName())
    {
      case "sparks_start":
        this.StartSparks();
        break;
      case "sparks_end":
        this.EndSparks();
        break;
      case "laser_fire":
        this.FireLaser();
        break;
    }
  }

  private void StartSparks() => this._sparkParticles.Emitting = true;

  private void EndSparks() => this._sparkParticles.Emitting = false;

  private void FireLaser() => this._beamVfx.Fire();

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NKinPriestVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NKinPriestVfx.MethodName.OnAnimationEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("__"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("___"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("spineEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false)
      }, (List<Variant>) null),
      new MethodInfo(NKinPriestVfx.MethodName.StartSparks, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NKinPriestVfx.MethodName.EndSparks, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NKinPriestVfx.MethodName.FireLaser, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NKinPriestVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NKinPriestVfx.MethodName.OnAnimationEvent) && ((NativeVariantPtrArgs) ref args).Count == 4)
    {
      this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[3]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NKinPriestVfx.MethodName.StartSparks) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.StartSparks();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NKinPriestVfx.MethodName.EndSparks) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.EndSparks();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NKinPriestVfx.MethodName.FireLaser) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.FireLaser();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NKinPriestVfx.MethodName._Ready) || StringName.op_Equality(ref method, NKinPriestVfx.MethodName.OnAnimationEvent) || StringName.op_Equality(ref method, NKinPriestVfx.MethodName.StartSparks) || StringName.op_Equality(ref method, NKinPriestVfx.MethodName.EndSparks) || StringName.op_Equality(ref method, NKinPriestVfx.MethodName.FireLaser) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NKinPriestVfx.PropertyName._sparkParticles))
    {
      this._sparkParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NKinPriestVfx.PropertyName._beamVfx))
    {
      this._beamVfx = VariantUtils.ConvertTo<NKinPriestBeamVfx>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NKinPriestVfx.PropertyName._parent))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NKinPriestVfx.PropertyName._sparkParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._sparkParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NKinPriestVfx.PropertyName._beamVfx))
    {
      value = VariantUtils.CreateFrom<NKinPriestBeamVfx>(ref this._beamVfx);
      return true;
    }
    if (!StringName.op_Equality(ref name, NKinPriestVfx.PropertyName._parent))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NKinPriestVfx.PropertyName._sparkParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NKinPriestVfx.PropertyName._beamVfx, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NKinPriestVfx.PropertyName._parent, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NKinPriestVfx.PropertyName._sparkParticles, Variant.From<GpuParticles2D>(ref this._sparkParticles));
    info.AddProperty(NKinPriestVfx.PropertyName._beamVfx, Variant.From<NKinPriestBeamVfx>(ref this._beamVfx));
    info.AddProperty(NKinPriestVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NKinPriestVfx.PropertyName._sparkParticles, ref variant1))
      this._sparkParticles = ((Variant) ref variant1).As<GpuParticles2D>();
    Variant variant2;
    if (info.TryGetProperty(NKinPriestVfx.PropertyName._beamVfx, ref variant2))
      this._beamVfx = ((Variant) ref variant2).As<NKinPriestBeamVfx>();
    Variant variant3;
    if (!info.TryGetProperty(NKinPriestVfx.PropertyName._parent, ref variant3))
      return;
    this._parent = ((Variant) ref variant3).As<Node2D>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnAnimationEvent = StringName.op_Implicit(nameof (OnAnimationEvent));
    public static readonly StringName StartSparks = StringName.op_Implicit(nameof (StartSparks));
    public static readonly StringName EndSparks = StringName.op_Implicit(nameof (EndSparks));
    public static readonly StringName FireLaser = StringName.op_Implicit(nameof (FireLaser));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName _sparkParticles = StringName.op_Implicit(nameof (_sparkParticles));
    public static readonly StringName _beamVfx = StringName.op_Implicit(nameof (_beamVfx));
    public static readonly StringName _parent = StringName.op_Implicit(nameof (_parent));
  }

  public class SignalName : Node.SignalName
  {
  }
}
