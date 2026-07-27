// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NSpectralKnightVfx
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
[ScriptPath("res://src/Core/Nodes/Vfx/NSpectralKnightVfx.cs")]
public class NSpectralKnightVfx : Node
{
  private GpuParticles2D _flameParticlesFlat;
  private GpuParticles2D _flameParticlesAdd;
  private GpuParticles2D _cinderParticles;
  private Node2D _parent;
  private MegaSprite _animController;

  public override void _Ready()
  {
    this._parent = this.GetParent<Node2D>();
    this._animController = new MegaSprite(Variant.op_Implicit((GodotObject) this._parent));
    this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
    this._animController.ConnectAnimationStarted(Callable.From<GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject>(this.OnAnimationStart)));
    this._flameParticlesAdd = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("FlameParticlesAdd"));
    this._flameParticlesFlat = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("FlameParticlesFlat"));
    this._cinderParticles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("CinderParticles"));
    this.TurnOffFire();
  }

  private void OnAnimationEvent(
    GodotObject _,
    GodotObject __,
    GodotObject ___,
    GodotObject spineEvent)
  {
    switch (new MegaEvent(Variant.op_Implicit(spineEvent)).GetData().GetEventName())
    {
      case "flame_start":
        this.TurnOnFire();
        break;
      case "flame_end":
        this.TurnOffFire();
        break;
    }
  }

  private void OnAnimationStart(
    GodotObject spineSprite,
    GodotObject animationState,
    GodotObject trackEntry)
  {
    if (!(new MegaAnimationState(Variant.op_Implicit(animationState)).GetCurrentAnimationName() != "attack"))
      return;
    this.TurnOffFire();
  }

  private void TurnOnFire()
  {
    this._flameParticlesAdd.Restart();
    this._flameParticlesFlat.Restart();
    this._cinderParticles.Restart();
  }

  private void TurnOffFire()
  {
    this._flameParticlesAdd.Emitting = false;
    this._flameParticlesFlat.Emitting = false;
    this._cinderParticles.Emitting = false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NSpectralKnightVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSpectralKnightVfx.MethodName.OnAnimationEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("__"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("___"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("spineEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false)
      }, (List<Variant>) null),
      new MethodInfo(NSpectralKnightVfx.MethodName.OnAnimationStart, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("spineSprite"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("animationState"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("trackEntry"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false)
      }, (List<Variant>) null),
      new MethodInfo(NSpectralKnightVfx.MethodName.TurnOnFire, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSpectralKnightVfx.MethodName.TurnOffFire, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NSpectralKnightVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSpectralKnightVfx.MethodName.OnAnimationEvent) && ((NativeVariantPtrArgs) ref args).Count == 4)
    {
      this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[3]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSpectralKnightVfx.MethodName.OnAnimationStart) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      this.OnAnimationStart(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSpectralKnightVfx.MethodName.TurnOnFire) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.TurnOnFire();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NSpectralKnightVfx.MethodName.TurnOffFire) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.TurnOffFire();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NSpectralKnightVfx.MethodName._Ready) || StringName.op_Equality(ref method, NSpectralKnightVfx.MethodName.OnAnimationEvent) || StringName.op_Equality(ref method, NSpectralKnightVfx.MethodName.OnAnimationStart) || StringName.op_Equality(ref method, NSpectralKnightVfx.MethodName.TurnOnFire) || StringName.op_Equality(ref method, NSpectralKnightVfx.MethodName.TurnOffFire) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSpectralKnightVfx.PropertyName._flameParticlesFlat))
    {
      this._flameParticlesFlat = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpectralKnightVfx.PropertyName._flameParticlesAdd))
    {
      this._flameParticlesAdd = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpectralKnightVfx.PropertyName._cinderParticles))
    {
      this._cinderParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSpectralKnightVfx.PropertyName._parent))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSpectralKnightVfx.PropertyName._flameParticlesFlat))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._flameParticlesFlat);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpectralKnightVfx.PropertyName._flameParticlesAdd))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._flameParticlesAdd);
      return true;
    }
    if (StringName.op_Equality(ref name, NSpectralKnightVfx.PropertyName._cinderParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._cinderParticles);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSpectralKnightVfx.PropertyName._parent))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NSpectralKnightVfx.PropertyName._flameParticlesFlat, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSpectralKnightVfx.PropertyName._flameParticlesAdd, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSpectralKnightVfx.PropertyName._cinderParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSpectralKnightVfx.PropertyName._parent, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NSpectralKnightVfx.PropertyName._flameParticlesFlat, Variant.From<GpuParticles2D>(ref this._flameParticlesFlat));
    info.AddProperty(NSpectralKnightVfx.PropertyName._flameParticlesAdd, Variant.From<GpuParticles2D>(ref this._flameParticlesAdd));
    info.AddProperty(NSpectralKnightVfx.PropertyName._cinderParticles, Variant.From<GpuParticles2D>(ref this._cinderParticles));
    info.AddProperty(NSpectralKnightVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NSpectralKnightVfx.PropertyName._flameParticlesFlat, ref variant1))
      this._flameParticlesFlat = ((Variant) ref variant1).As<GpuParticles2D>();
    Variant variant2;
    if (info.TryGetProperty(NSpectralKnightVfx.PropertyName._flameParticlesAdd, ref variant2))
      this._flameParticlesAdd = ((Variant) ref variant2).As<GpuParticles2D>();
    Variant variant3;
    if (info.TryGetProperty(NSpectralKnightVfx.PropertyName._cinderParticles, ref variant3))
      this._cinderParticles = ((Variant) ref variant3).As<GpuParticles2D>();
    Variant variant4;
    if (!info.TryGetProperty(NSpectralKnightVfx.PropertyName._parent, ref variant4))
      return;
    this._parent = ((Variant) ref variant4).As<Node2D>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnAnimationEvent = StringName.op_Implicit(nameof (OnAnimationEvent));
    public static readonly StringName OnAnimationStart = StringName.op_Implicit(nameof (OnAnimationStart));
    public static readonly StringName TurnOnFire = StringName.op_Implicit(nameof (TurnOnFire));
    public static readonly StringName TurnOffFire = StringName.op_Implicit(nameof (TurnOffFire));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName _flameParticlesFlat = StringName.op_Implicit(nameof (_flameParticlesFlat));
    public static readonly StringName _flameParticlesAdd = StringName.op_Implicit(nameof (_flameParticlesAdd));
    public static readonly StringName _cinderParticles = StringName.op_Implicit(nameof (_cinderParticles));
    public static readonly StringName _parent = StringName.op_Implicit(nameof (_parent));
  }

  public class SignalName : Node.SignalName
  {
  }
}
