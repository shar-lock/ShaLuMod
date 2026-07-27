// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NSlimedBerserkerVfx
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

[GlobalClass]
[ScriptPath("res://src/Core/Nodes/Vfx/NSlimedBerserkerVfx.cs")]
public class NSlimedBerserkerVfx : Node
{
  private GpuParticles2D _gooParticlesR;
  private GpuParticles2D _gooParticlesL;
  private GpuParticles2D _gooParticlesVomit;
  private Node2D _parent;
  private MegaSprite _animController;

  public override void _Ready()
  {
    this._parent = this.GetParent<Node2D>();
    this._animController = new MegaSprite(Variant.op_Implicit((GodotObject) this._parent));
    this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
    this._gooParticlesR = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("ParticleSlotNodeR/GooParticles"));
    this._gooParticlesL = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("ParticleSlotNodeL/GooParticles"));
    this._gooParticlesVomit = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("ParticleSlotNodeVomit/GooParticles"));
    this.StopGooParticles();
    this.StopVomitParticles();
  }

  private void OnAnimationEvent(
    GodotObject _,
    GodotObject __,
    GodotObject ___,
    GodotObject spineEvent)
  {
    switch (new MegaEvent(Variant.op_Implicit(spineEvent)).GetData().GetEventName())
    {
      case "goo_start":
        this.StartGooParticles();
        break;
      case "goo_stop":
        this.StopGooParticles();
        break;
      case "vomit_start":
        this.StartVomitParticles();
        break;
      case "vomit_stop":
        this.StopVomitParticles();
        break;
    }
  }

  private void StartGooParticles()
  {
    this._gooParticlesR.Emitting = true;
    this._gooParticlesL.Emitting = true;
  }

  private void StopGooParticles()
  {
    this._gooParticlesR.Emitting = false;
    this._gooParticlesL.Emitting = false;
  }

  private void StopVomitParticles() => this._gooParticlesVomit.Emitting = false;

  private void StartVomitParticles() => this._gooParticlesVomit.Emitting = true;

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(6)
    {
      new MethodInfo(NSlimedBerserkerVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSlimedBerserkerVfx.MethodName.OnAnimationEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("__"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("___"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("spineEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false)
      }, (List<Variant>) null),
      new MethodInfo(NSlimedBerserkerVfx.MethodName.StartGooParticles, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSlimedBerserkerVfx.MethodName.StopGooParticles, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSlimedBerserkerVfx.MethodName.StopVomitParticles, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSlimedBerserkerVfx.MethodName.StartVomitParticles, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NSlimedBerserkerVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSlimedBerserkerVfx.MethodName.OnAnimationEvent) && ((NativeVariantPtrArgs) ref args).Count == 4)
    {
      this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[3]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSlimedBerserkerVfx.MethodName.StartGooParticles) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.StartGooParticles();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSlimedBerserkerVfx.MethodName.StopGooParticles) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.StopGooParticles();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSlimedBerserkerVfx.MethodName.StopVomitParticles) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.StopVomitParticles();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NSlimedBerserkerVfx.MethodName.StartVomitParticles) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.StartVomitParticles();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NSlimedBerserkerVfx.MethodName._Ready) || StringName.op_Equality(ref method, NSlimedBerserkerVfx.MethodName.OnAnimationEvent) || StringName.op_Equality(ref method, NSlimedBerserkerVfx.MethodName.StartGooParticles) || StringName.op_Equality(ref method, NSlimedBerserkerVfx.MethodName.StopGooParticles) || StringName.op_Equality(ref method, NSlimedBerserkerVfx.MethodName.StopVomitParticles) || StringName.op_Equality(ref method, NSlimedBerserkerVfx.MethodName.StartVomitParticles) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSlimedBerserkerVfx.PropertyName._gooParticlesR))
    {
      this._gooParticlesR = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSlimedBerserkerVfx.PropertyName._gooParticlesL))
    {
      this._gooParticlesL = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSlimedBerserkerVfx.PropertyName._gooParticlesVomit))
    {
      this._gooParticlesVomit = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSlimedBerserkerVfx.PropertyName._parent))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSlimedBerserkerVfx.PropertyName._gooParticlesR))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._gooParticlesR);
      return true;
    }
    if (StringName.op_Equality(ref name, NSlimedBerserkerVfx.PropertyName._gooParticlesL))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._gooParticlesL);
      return true;
    }
    if (StringName.op_Equality(ref name, NSlimedBerserkerVfx.PropertyName._gooParticlesVomit))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._gooParticlesVomit);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSlimedBerserkerVfx.PropertyName._parent))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NSlimedBerserkerVfx.PropertyName._gooParticlesR, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSlimedBerserkerVfx.PropertyName._gooParticlesL, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSlimedBerserkerVfx.PropertyName._gooParticlesVomit, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSlimedBerserkerVfx.PropertyName._parent, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NSlimedBerserkerVfx.PropertyName._gooParticlesR, Variant.From<GpuParticles2D>(ref this._gooParticlesR));
    info.AddProperty(NSlimedBerserkerVfx.PropertyName._gooParticlesL, Variant.From<GpuParticles2D>(ref this._gooParticlesL));
    info.AddProperty(NSlimedBerserkerVfx.PropertyName._gooParticlesVomit, Variant.From<GpuParticles2D>(ref this._gooParticlesVomit));
    info.AddProperty(NSlimedBerserkerVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NSlimedBerserkerVfx.PropertyName._gooParticlesR, ref variant1))
      this._gooParticlesR = ((Variant) ref variant1).As<GpuParticles2D>();
    Variant variant2;
    if (info.TryGetProperty(NSlimedBerserkerVfx.PropertyName._gooParticlesL, ref variant2))
      this._gooParticlesL = ((Variant) ref variant2).As<GpuParticles2D>();
    Variant variant3;
    if (info.TryGetProperty(NSlimedBerserkerVfx.PropertyName._gooParticlesVomit, ref variant3))
      this._gooParticlesVomit = ((Variant) ref variant3).As<GpuParticles2D>();
    Variant variant4;
    if (!info.TryGetProperty(NSlimedBerserkerVfx.PropertyName._parent, ref variant4))
      return;
    this._parent = ((Variant) ref variant4).As<Node2D>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnAnimationEvent = StringName.op_Implicit(nameof (OnAnimationEvent));
    public static readonly StringName StartGooParticles = StringName.op_Implicit(nameof (StartGooParticles));
    public static readonly StringName StopGooParticles = StringName.op_Implicit(nameof (StopGooParticles));
    public static readonly StringName StopVomitParticles = StringName.op_Implicit(nameof (StopVomitParticles));
    public static readonly StringName StartVomitParticles = StringName.op_Implicit(nameof (StartVomitParticles));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName _gooParticlesR = StringName.op_Implicit(nameof (_gooParticlesR));
    public static readonly StringName _gooParticlesL = StringName.op_Implicit(nameof (_gooParticlesL));
    public static readonly StringName _gooParticlesVomit = StringName.op_Implicit(nameof (_gooParticlesVomit));
    public static readonly StringName _parent = StringName.op_Implicit(nameof (_parent));
  }

  public class SignalName : Node.SignalName
  {
  }
}
