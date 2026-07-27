// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NSkulkingColonyVfx
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

[ScriptPath("res://src/Core/Nodes/Vfx/NSkulkingColonyVfx.cs")]
public class NSkulkingColonyVfx : Node2D
{
  private MegaSprite _megaSprite;
  private GpuParticles2D _particles1;
  private GpuParticles2D _wideParticles;
  private GpuParticles2D _poofParticles;

  public override void _Ready()
  {
    this._particles1 = ((Node) this).GetNode<GpuParticles2D>(NodePath.op_Implicit("../ParticleSlot1/Particles"));
    this._wideParticles = ((Node) this).GetNode<GpuParticles2D>(NodePath.op_Implicit("../ParticleSlot2/ParticlesWide"));
    this._poofParticles = ((Node) this).GetNode<GpuParticles2D>(NodePath.op_Implicit("../ParticleSlot3/Particles"));
    this._particles1.Emitting = false;
    this._poofParticles.Emitting = false;
    this._wideParticles.Emitting = false;
    this._particles1.OneShot = true;
    this._poofParticles.OneShot = true;
    this._wideParticles.OneShot = true;
    this._megaSprite = new MegaSprite(Variant.op_Implicit((GodotObject) ((Node) this).GetParent<Node2D>()));
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
      case "take_damage":
        this.DamageHandler();
        break;
      case "take_fatal_damage":
        this.DeathHandler();
        break;
      case "final_poof":
        this.PoofHandler();
        break;
    }
  }

  private void DamageHandler() => this._particles1.Restart();

  private void DeathHandler() => this._wideParticles.Restart();

  private void PoofHandler() => this._poofParticles.Restart();

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NSkulkingColonyVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSkulkingColonyVfx.MethodName.OnAnimationEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("__"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("___"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("spineEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false)
      }, (List<Variant>) null),
      new MethodInfo(NSkulkingColonyVfx.MethodName.DamageHandler, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSkulkingColonyVfx.MethodName.DeathHandler, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSkulkingColonyVfx.MethodName.PoofHandler, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NSkulkingColonyVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSkulkingColonyVfx.MethodName.OnAnimationEvent) && ((NativeVariantPtrArgs) ref args).Count == 4)
    {
      this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[3]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSkulkingColonyVfx.MethodName.DamageHandler) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.DamageHandler();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSkulkingColonyVfx.MethodName.DeathHandler) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.DeathHandler();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NSkulkingColonyVfx.MethodName.PoofHandler) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.PoofHandler();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NSkulkingColonyVfx.MethodName._Ready) || StringName.op_Equality(ref method, NSkulkingColonyVfx.MethodName.OnAnimationEvent) || StringName.op_Equality(ref method, NSkulkingColonyVfx.MethodName.DamageHandler) || StringName.op_Equality(ref method, NSkulkingColonyVfx.MethodName.DeathHandler) || StringName.op_Equality(ref method, NSkulkingColonyVfx.MethodName.PoofHandler) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSkulkingColonyVfx.PropertyName._particles1))
    {
      this._particles1 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSkulkingColonyVfx.PropertyName._wideParticles))
    {
      this._wideParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSkulkingColonyVfx.PropertyName._poofParticles))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._poofParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSkulkingColonyVfx.PropertyName._particles1))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._particles1);
      return true;
    }
    if (StringName.op_Equality(ref name, NSkulkingColonyVfx.PropertyName._wideParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._wideParticles);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSkulkingColonyVfx.PropertyName._poofParticles))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._poofParticles);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NSkulkingColonyVfx.PropertyName._particles1, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSkulkingColonyVfx.PropertyName._wideParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSkulkingColonyVfx.PropertyName._poofParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NSkulkingColonyVfx.PropertyName._particles1, Variant.From<GpuParticles2D>(ref this._particles1));
    info.AddProperty(NSkulkingColonyVfx.PropertyName._wideParticles, Variant.From<GpuParticles2D>(ref this._wideParticles));
    info.AddProperty(NSkulkingColonyVfx.PropertyName._poofParticles, Variant.From<GpuParticles2D>(ref this._poofParticles));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NSkulkingColonyVfx.PropertyName._particles1, ref variant1))
      this._particles1 = ((Variant) ref variant1).As<GpuParticles2D>();
    Variant variant2;
    if (info.TryGetProperty(NSkulkingColonyVfx.PropertyName._wideParticles, ref variant2))
      this._wideParticles = ((Variant) ref variant2).As<GpuParticles2D>();
    Variant variant3;
    if (!info.TryGetProperty(NSkulkingColonyVfx.PropertyName._poofParticles, ref variant3))
      return;
    this._poofParticles = ((Variant) ref variant3).As<GpuParticles2D>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnAnimationEvent = StringName.op_Implicit(nameof (OnAnimationEvent));
    public static readonly StringName DamageHandler = StringName.op_Implicit(nameof (DamageHandler));
    public static readonly StringName DeathHandler = StringName.op_Implicit(nameof (DeathHandler));
    public static readonly StringName PoofHandler = StringName.op_Implicit(nameof (PoofHandler));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _particles1 = StringName.op_Implicit(nameof (_particles1));
    public static readonly StringName _wideParticles = StringName.op_Implicit(nameof (_wideParticles));
    public static readonly StringName _poofParticles = StringName.op_Implicit(nameof (_poofParticles));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
