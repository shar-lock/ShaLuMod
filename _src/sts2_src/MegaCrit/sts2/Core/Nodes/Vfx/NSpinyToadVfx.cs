// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NSpinyToadVfx
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
[ScriptPath("res://src/Core/Nodes/Vfx/NSpinyToadVfx.cs")]
public class NSpinyToadVfx : Node
{
  private GpuParticles2D _spineParticles;
  private GpuParticles2D _spineSubParticles;
  private Node2D _parent;
  private MegaSprite _animController;

  public override void _Ready()
  {
    this._parent = this.GetParent<Node2D>();
    this._animController = new MegaSprite(Variant.op_Implicit((GodotObject) this._parent));
    this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
    this._spineParticles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("BurstParticles"));
    this._spineSubParticles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("BurstParticlesSub"));
    this._spineParticles.Emitting = false;
    this._spineParticles.OneShot = true;
    this._spineSubParticles.Emitting = false;
    this._spineSubParticles.OneShot = true;
    this.RunWhenSpineReady(this._animController, (Action<MegaAnimationState>) (animState => animState.SetAnimation("explode")));
  }

  private void OnAnimationEvent(
    GodotObject _,
    GodotObject __,
    GodotObject ___,
    GodotObject spineEvent)
  {
    if (!(new MegaEvent(Variant.op_Implicit(spineEvent)).GetData().GetEventName() == "explode"))
      return;
    this.StartExplosion();
  }

  private void StartExplosion() => this._spineParticles.Restart();

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NSpinyToadVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSpinyToadVfx.MethodName.OnAnimationEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("__"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("___"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("spineEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false)
      }, (List<Variant>) null),
      new MethodInfo(NSpinyToadVfx.MethodName.StartExplosion, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NSpinyToadVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSpinyToadVfx.MethodName.OnAnimationEvent) && ((NativeVariantPtrArgs) ref args).Count == 4)
    {
      this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[3]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NSpinyToadVfx.MethodName.StartExplosion) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.StartExplosion();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NSpinyToadVfx.MethodName._Ready) || StringName.op_Equality(ref method, NSpinyToadVfx.MethodName.OnAnimationEvent) || StringName.op_Equality(ref method, NSpinyToadVfx.MethodName.StartExplosion) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSpinyToadVfx.PropertyName._spineParticles))
    {
      this._spineParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpinyToadVfx.PropertyName._spineSubParticles))
    {
      this._spineSubParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSpinyToadVfx.PropertyName._parent))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSpinyToadVfx.PropertyName._spineParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._spineParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpinyToadVfx.PropertyName._spineSubParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._spineSubParticles);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSpinyToadVfx.PropertyName._parent))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NSpinyToadVfx.PropertyName._spineParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSpinyToadVfx.PropertyName._spineSubParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSpinyToadVfx.PropertyName._parent, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NSpinyToadVfx.PropertyName._spineParticles, Variant.From<GpuParticles2D>(ref this._spineParticles));
    info.AddProperty(NSpinyToadVfx.PropertyName._spineSubParticles, Variant.From<GpuParticles2D>(ref this._spineSubParticles));
    info.AddProperty(NSpinyToadVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NSpinyToadVfx.PropertyName._spineParticles, ref variant1))
      this._spineParticles = ((Variant) ref variant1).As<GpuParticles2D>();
    Variant variant2;
    if (info.TryGetProperty(NSpinyToadVfx.PropertyName._spineSubParticles, ref variant2))
      this._spineSubParticles = ((Variant) ref variant2).As<GpuParticles2D>();
    Variant variant3;
    if (!info.TryGetProperty(NSpinyToadVfx.PropertyName._parent, ref variant3))
      return;
    this._parent = ((Variant) ref variant3).As<Node2D>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnAnimationEvent = StringName.op_Implicit(nameof (OnAnimationEvent));
    public static readonly StringName StartExplosion = StringName.op_Implicit(nameof (StartExplosion));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName _spineParticles = StringName.op_Implicit(nameof (_spineParticles));
    public static readonly StringName _spineSubParticles = StringName.op_Implicit(nameof (_spineSubParticles));
    public static readonly StringName _parent = StringName.op_Implicit(nameof (_parent));
  }

  public class SignalName : Node.SignalName
  {
  }
}
