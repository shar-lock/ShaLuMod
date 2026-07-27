// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NFogmogVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NFogmogVfx.cs")]
public class NFogmogVfx : Node
{
  private GpuParticles2D _thrustParicles;
  private GpuParticles2D _dustLeftParticles;
  private GpuParticles2D _dustRightParticles;
  private MegaSprite _megaSprite;

  public override void _Ready()
  {
    this._thrustParicles = this.GetNode<GpuParticles2D>(NodePath.op_Implicit("../ThrustSlotNode/ThrustParticles"));
    this._dustLeftParticles = this.GetNode<GpuParticles2D>(NodePath.op_Implicit("../DustSlotNode/DustLeftParticles"));
    this._dustRightParticles = this.GetNode<GpuParticles2D>(NodePath.op_Implicit("../DustSlotNode/DustRightParticles"));
    this._thrustParicles.Emitting = false;
    this._dustLeftParticles.Emitting = false;
    this._dustRightParticles.Emitting = false;
    this._megaSprite = new MegaSprite(Variant.op_Implicit((GodotObject) this.GetParent<Node2D>()));
    this._megaSprite.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
  }

  private void OnAnimationEvent(
    GodotObject _,
    GodotObject __,
    GodotObject ___,
    GodotObject spineEvent)
  {
    switch (new MegaEvent(Variant.op_Implicit(spineEvent)).GetData().GetEventName())
    {
      case "thrust_start":
        this.StartThrust();
        break;
      case "thrust_end":
        this.EndThrust();
        break;
    }
  }

  private void StartThrust()
  {
    this._thrustParicles.Restart();
    this._dustRightParticles.Restart();
    this._dustLeftParticles.Restart();
  }

  private void EndThrust()
  {
    this._thrustParicles.Emitting = false;
    this._dustLeftParticles.Emitting = false;
    this._dustRightParticles.Emitting = false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NFogmogVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NFogmogVfx.MethodName.OnAnimationEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("__"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("___"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("spineEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false)
      }, (List<Variant>) null),
      new MethodInfo(NFogmogVfx.MethodName.StartThrust, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NFogmogVfx.MethodName.EndThrust, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NFogmogVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NFogmogVfx.MethodName.OnAnimationEvent) && ((NativeVariantPtrArgs) ref args).Count == 4)
    {
      this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[3]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NFogmogVfx.MethodName.StartThrust) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.StartThrust();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NFogmogVfx.MethodName.EndThrust) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.EndThrust();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NFogmogVfx.MethodName._Ready) || StringName.op_Equality(ref method, NFogmogVfx.MethodName.OnAnimationEvent) || StringName.op_Equality(ref method, NFogmogVfx.MethodName.StartThrust) || StringName.op_Equality(ref method, NFogmogVfx.MethodName.EndThrust) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NFogmogVfx.PropertyName._thrustParicles))
    {
      this._thrustParicles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NFogmogVfx.PropertyName._dustLeftParticles))
    {
      this._dustLeftParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NFogmogVfx.PropertyName._dustRightParticles))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._dustRightParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NFogmogVfx.PropertyName._thrustParicles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._thrustParicles);
      return true;
    }
    if (StringName.op_Equality(ref name, NFogmogVfx.PropertyName._dustLeftParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._dustLeftParticles);
      return true;
    }
    if (!StringName.op_Equality(ref name, NFogmogVfx.PropertyName._dustRightParticles))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._dustRightParticles);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NFogmogVfx.PropertyName._thrustParicles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NFogmogVfx.PropertyName._dustLeftParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NFogmogVfx.PropertyName._dustRightParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NFogmogVfx.PropertyName._thrustParicles, Variant.From<GpuParticles2D>(ref this._thrustParicles));
    info.AddProperty(NFogmogVfx.PropertyName._dustLeftParticles, Variant.From<GpuParticles2D>(ref this._dustLeftParticles));
    info.AddProperty(NFogmogVfx.PropertyName._dustRightParticles, Variant.From<GpuParticles2D>(ref this._dustRightParticles));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NFogmogVfx.PropertyName._thrustParicles, ref variant1))
      this._thrustParicles = ((Variant) ref variant1).As<GpuParticles2D>();
    Variant variant2;
    if (info.TryGetProperty(NFogmogVfx.PropertyName._dustLeftParticles, ref variant2))
      this._dustLeftParticles = ((Variant) ref variant2).As<GpuParticles2D>();
    Variant variant3;
    if (!info.TryGetProperty(NFogmogVfx.PropertyName._dustRightParticles, ref variant3))
      return;
    this._dustRightParticles = ((Variant) ref variant3).As<GpuParticles2D>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnAnimationEvent = StringName.op_Implicit(nameof (OnAnimationEvent));
    public static readonly StringName StartThrust = StringName.op_Implicit(nameof (StartThrust));
    public static readonly StringName EndThrust = StringName.op_Implicit(nameof (EndThrust));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName _thrustParicles = StringName.op_Implicit(nameof (_thrustParicles));
    public static readonly StringName _dustLeftParticles = StringName.op_Implicit(nameof (_dustLeftParticles));
    public static readonly StringName _dustRightParticles = StringName.op_Implicit(nameof (_dustRightParticles));
  }

  public class SignalName : Node.SignalName
  {
  }
}
