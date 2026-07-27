// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NAxebotVfx
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
[ScriptPath("res://src/Core/Nodes/Vfx/NAxebotVfx.cs")]
public class NAxebotVfx : Node
{
  private GpuParticles2D _hurtParticles1;
  private GpuParticles2D _hurtParticles2;
  private GpuParticles2D _smokeParticlesLeft;
  private GpuParticles2D _smokeParticlesRight;
  private Node2D _parent;
  private MegaSprite _animController;
  private int _currentWeapon = 1;

  public override void _Ready()
  {
    this._parent = this.GetParent<Node2D>();
    this._animController = new MegaSprite(Variant.op_Implicit((GodotObject) this._parent));
    this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
    this._hurtParticles1 = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("SparksBoneNode/HurtParticles1"));
    this._hurtParticles2 = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("SparksBoneNode/HurtParticles2"));
    this._smokeParticlesLeft = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("SmokeNodeLeft/SmokeParticles"));
    this._smokeParticlesRight = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("SmokeNodeRight/SmokeParticles"));
    this._hurtParticles1.OneShot = true;
    this._hurtParticles2.OneShot = true;
    this._hurtParticles1.Emitting = false;
    this._hurtParticles2.Emitting = false;
    this._smokeParticlesLeft.Emitting = false;
    this._smokeParticlesRight.Emitting = false;
  }

  private void OnAnimationEvent(
    GodotObject _,
    GodotObject __,
    GodotObject ___,
    GodotObject spineEvent)
  {
    switch (new MegaEvent(Variant.op_Implicit(spineEvent)).GetData().GetEventName())
    {
      case "start_hurt_sparks":
        this.TurnOnHurt();
        break;
      case "start_death_sparks1":
        this.TurnOnDeath1();
        break;
      case "start_death_sparks2":
        this.TurnOnDeath2();
        break;
      case "landing_smoke_start":
        this.TurnOnLandingSmoke();
        break;
      case "landing_smoke_end":
        this.TurnOffLandingSmoke();
        break;
    }
  }

  private void TurnOnDeath1() => this._hurtParticles1.Restart();

  private void TurnOnDeath2() => this._hurtParticles2.Restart();

  private void TurnOnHurt()
  {
    if (this._currentWeapon == 1)
    {
      this._hurtParticles1.Restart();
      this._currentWeapon = 2;
    }
    else
    {
      this._hurtParticles2.Restart();
      this._currentWeapon = 1;
    }
  }

  private void TurnOnLandingSmoke()
  {
    this._smokeParticlesLeft.Restart();
    this._smokeParticlesRight.Restart();
  }

  private void TurnOffLandingSmoke()
  {
    this._smokeParticlesLeft.Emitting = false;
    this._smokeParticlesRight.Emitting = false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(7)
    {
      new MethodInfo(NAxebotVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAxebotVfx.MethodName.OnAnimationEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("__"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("___"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("spineEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false)
      }, (List<Variant>) null),
      new MethodInfo(NAxebotVfx.MethodName.TurnOnDeath1, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAxebotVfx.MethodName.TurnOnDeath2, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAxebotVfx.MethodName.TurnOnHurt, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAxebotVfx.MethodName.TurnOnLandingSmoke, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAxebotVfx.MethodName.TurnOffLandingSmoke, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NAxebotVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAxebotVfx.MethodName.OnAnimationEvent) && ((NativeVariantPtrArgs) ref args).Count == 4)
    {
      this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[3]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAxebotVfx.MethodName.TurnOnDeath1) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.TurnOnDeath1();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAxebotVfx.MethodName.TurnOnDeath2) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.TurnOnDeath2();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAxebotVfx.MethodName.TurnOnHurt) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.TurnOnHurt();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAxebotVfx.MethodName.TurnOnLandingSmoke) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.TurnOnLandingSmoke();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NAxebotVfx.MethodName.TurnOffLandingSmoke) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.TurnOffLandingSmoke();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NAxebotVfx.MethodName._Ready) || StringName.op_Equality(ref method, NAxebotVfx.MethodName.OnAnimationEvent) || StringName.op_Equality(ref method, NAxebotVfx.MethodName.TurnOnDeath1) || StringName.op_Equality(ref method, NAxebotVfx.MethodName.TurnOnDeath2) || StringName.op_Equality(ref method, NAxebotVfx.MethodName.TurnOnHurt) || StringName.op_Equality(ref method, NAxebotVfx.MethodName.TurnOnLandingSmoke) || StringName.op_Equality(ref method, NAxebotVfx.MethodName.TurnOffLandingSmoke) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NAxebotVfx.PropertyName._hurtParticles1))
    {
      this._hurtParticles1 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAxebotVfx.PropertyName._hurtParticles2))
    {
      this._hurtParticles2 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAxebotVfx.PropertyName._smokeParticlesLeft))
    {
      this._smokeParticlesLeft = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAxebotVfx.PropertyName._smokeParticlesRight))
    {
      this._smokeParticlesRight = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAxebotVfx.PropertyName._parent))
    {
      this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NAxebotVfx.PropertyName._currentWeapon))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._currentWeapon = VariantUtils.ConvertTo<int>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NAxebotVfx.PropertyName._hurtParticles1))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._hurtParticles1);
      return true;
    }
    if (StringName.op_Equality(ref name, NAxebotVfx.PropertyName._hurtParticles2))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._hurtParticles2);
      return true;
    }
    if (StringName.op_Equality(ref name, NAxebotVfx.PropertyName._smokeParticlesLeft))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._smokeParticlesLeft);
      return true;
    }
    if (StringName.op_Equality(ref name, NAxebotVfx.PropertyName._smokeParticlesRight))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._smokeParticlesRight);
      return true;
    }
    if (StringName.op_Equality(ref name, NAxebotVfx.PropertyName._parent))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
      return true;
    }
    if (!StringName.op_Equality(ref name, NAxebotVfx.PropertyName._currentWeapon))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<int>(ref this._currentWeapon);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NAxebotVfx.PropertyName._hurtParticles1, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAxebotVfx.PropertyName._hurtParticles2, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAxebotVfx.PropertyName._smokeParticlesLeft, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAxebotVfx.PropertyName._smokeParticlesRight, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAxebotVfx.PropertyName._parent, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NAxebotVfx.PropertyName._currentWeapon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NAxebotVfx.PropertyName._hurtParticles1, Variant.From<GpuParticles2D>(ref this._hurtParticles1));
    info.AddProperty(NAxebotVfx.PropertyName._hurtParticles2, Variant.From<GpuParticles2D>(ref this._hurtParticles2));
    info.AddProperty(NAxebotVfx.PropertyName._smokeParticlesLeft, Variant.From<GpuParticles2D>(ref this._smokeParticlesLeft));
    info.AddProperty(NAxebotVfx.PropertyName._smokeParticlesRight, Variant.From<GpuParticles2D>(ref this._smokeParticlesRight));
    info.AddProperty(NAxebotVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
    info.AddProperty(NAxebotVfx.PropertyName._currentWeapon, Variant.From<int>(ref this._currentWeapon));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NAxebotVfx.PropertyName._hurtParticles1, ref variant1))
      this._hurtParticles1 = ((Variant) ref variant1).As<GpuParticles2D>();
    Variant variant2;
    if (info.TryGetProperty(NAxebotVfx.PropertyName._hurtParticles2, ref variant2))
      this._hurtParticles2 = ((Variant) ref variant2).As<GpuParticles2D>();
    Variant variant3;
    if (info.TryGetProperty(NAxebotVfx.PropertyName._smokeParticlesLeft, ref variant3))
      this._smokeParticlesLeft = ((Variant) ref variant3).As<GpuParticles2D>();
    Variant variant4;
    if (info.TryGetProperty(NAxebotVfx.PropertyName._smokeParticlesRight, ref variant4))
      this._smokeParticlesRight = ((Variant) ref variant4).As<GpuParticles2D>();
    Variant variant5;
    if (info.TryGetProperty(NAxebotVfx.PropertyName._parent, ref variant5))
      this._parent = ((Variant) ref variant5).As<Node2D>();
    Variant variant6;
    if (!info.TryGetProperty(NAxebotVfx.PropertyName._currentWeapon, ref variant6))
      return;
    this._currentWeapon = ((Variant) ref variant6).As<int>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnAnimationEvent = StringName.op_Implicit(nameof (OnAnimationEvent));
    public static readonly StringName TurnOnDeath1 = StringName.op_Implicit(nameof (TurnOnDeath1));
    public static readonly StringName TurnOnDeath2 = StringName.op_Implicit(nameof (TurnOnDeath2));
    public static readonly StringName TurnOnHurt = StringName.op_Implicit(nameof (TurnOnHurt));
    public static readonly StringName TurnOnLandingSmoke = StringName.op_Implicit(nameof (TurnOnLandingSmoke));
    public static readonly StringName TurnOffLandingSmoke = StringName.op_Implicit(nameof (TurnOffLandingSmoke));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName _hurtParticles1 = StringName.op_Implicit(nameof (_hurtParticles1));
    public static readonly StringName _hurtParticles2 = StringName.op_Implicit(nameof (_hurtParticles2));
    public static readonly StringName _smokeParticlesLeft = StringName.op_Implicit(nameof (_smokeParticlesLeft));
    public static readonly StringName _smokeParticlesRight = StringName.op_Implicit(nameof (_smokeParticlesRight));
    public static readonly StringName _parent = StringName.op_Implicit(nameof (_parent));
    public static readonly StringName _currentWeapon = StringName.op_Implicit(nameof (_currentWeapon));
  }

  public class SignalName : Node.SignalName
  {
  }
}
