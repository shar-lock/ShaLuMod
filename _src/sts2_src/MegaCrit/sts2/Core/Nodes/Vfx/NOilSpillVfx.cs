// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NOilSpillVfx
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
[ScriptPath("res://src/Core/Nodes/Vfx/NOilSpillVfx.cs")]
public class NOilSpillVfx : Node
{
  private const int _slamSprayAmount = 800;
  private const int _sprayAttackAmount = 2000;
  private const int _deathSprayAmount = 500;
  private const float _slamSprayLifetime = 0.75f;
  private GpuParticles2D _droolParticles;
  private GpuParticles2D _sprayParticles;
  private GpuParticles2D _rainDropParticles;
  private Node2D _parent;
  private MegaSprite _animController;

  public override void _Ready()
  {
    this._parent = this.GetParent<Node2D>();
    this._animController = new MegaSprite(Variant.op_Implicit((GodotObject) this._parent));
    this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
    this._animController.ConnectAnimationStarted(Callable.From<GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject>(this.OnAnimationStart)));
    this._droolParticles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("MouthDribbleBoneNode/DribbleParticles"));
    this._sprayParticles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("MouthSpraySlot/SprayParticles"));
    this._rainDropParticles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("MouthSpraySlot/RainDropParticles"));
    this._rainDropParticles.OneShot = true;
    this._droolParticles.Restart();
    this._rainDropParticles.Restart();
    this._sprayParticles.Restart();
    this.TurnOffSprayAttack();
    this.TurnOffSlamSpray();
  }

  private void OnAnimationEvent(
    GodotObject _,
    GodotObject __,
    GodotObject ___,
    GodotObject spineEvent)
  {
    string eventName = new MegaEvent(Variant.op_Implicit(spineEvent)).GetData().GetEventName();
    if (eventName == null)
      return;
    switch (eventName.Length)
    {
      case 9:
        switch (eventName[0])
        {
          case 'd':
            if (!(eventName == "drool_end"))
              return;
            this.TurnOffDrool();
            return;
          case 's':
            if (!(eventName == "spray_end"))
              return;
            this.TurnOffSprayAttack();
            return;
          default:
            return;
        }
      case 11:
        switch (eventName[0])
        {
          case 'd':
            if (!(eventName == "drool_start"))
              return;
            this.TurnOnDrool();
            return;
          case 's':
            if (!(eventName == "spray_start"))
              return;
            this.TurnOnSprayAttack();
            return;
          default:
            return;
        }
      case 14:
        if (!(eventName == "slam_spray_end"))
          break;
        this.TurnOffSlamSpray();
        break;
      case 15:
        if (!(eventName == "death_spray_end"))
          break;
        this.TurnOffDeathSpray();
        break;
      case 16 /*0x10*/:
        if (!(eventName == "slam_spray_start"))
          break;
        this.TurnOnSlamSpray();
        break;
      case 17:
        if (!(eventName == "death_spray_start"))
          break;
        this.TurnOnDeathSpray();
        break;
    }
  }

  private void OnAnimationStart(
    GodotObject spineSprite,
    GodotObject animationState,
    GodotObject trackEntry)
  {
    string currentAnimationName = new MegaAnimationState(Variant.op_Implicit(animationState)).GetCurrentAnimationName();
    if (currentAnimationName != "slam")
      this.TurnOffSprayAttack();
    if (!(currentAnimationName != "spray"))
      return;
    this.TurnOffSlamSpray();
  }

  private void TurnOnSprayAttack()
  {
    this._sprayParticles.Amount = 2000;
    this._sprayParticles.Emitting = true;
  }

  private void TurnOffSprayAttack() => this._sprayParticles.Emitting = false;

  private void TurnOnSlamSpray()
  {
    this._rainDropParticles.OneShot = false;
    this._rainDropParticles.Amount = 800;
    this._rainDropParticles.Explosiveness = 0.0f;
    this._rainDropParticles.Lifetime = 0.75;
    this._rainDropParticles.Restart();
  }

  private void TurnOffSlamSpray() => this._rainDropParticles.Emitting = false;

  private void TurnOnDeathSpray()
  {
    this._sprayParticles.Amount = 500;
    this._sprayParticles.Emitting = true;
  }

  private void TurnOffDeathSpray() => this._sprayParticles.Emitting = false;

  private void TurnOnDrool() => this._droolParticles.Emitting = true;

  private void TurnOffDrool() => this._droolParticles.Emitting = false;

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(11)
    {
      new MethodInfo(NOilSpillVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NOilSpillVfx.MethodName.OnAnimationEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("__"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("___"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("spineEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false)
      }, (List<Variant>) null),
      new MethodInfo(NOilSpillVfx.MethodName.OnAnimationStart, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("spineSprite"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("animationState"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("trackEntry"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false)
      }, (List<Variant>) null),
      new MethodInfo(NOilSpillVfx.MethodName.TurnOnSprayAttack, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NOilSpillVfx.MethodName.TurnOffSprayAttack, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NOilSpillVfx.MethodName.TurnOnSlamSpray, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NOilSpillVfx.MethodName.TurnOffSlamSpray, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NOilSpillVfx.MethodName.TurnOnDeathSpray, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NOilSpillVfx.MethodName.TurnOffDeathSpray, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NOilSpillVfx.MethodName.TurnOnDrool, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NOilSpillVfx.MethodName.TurnOffDrool, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NOilSpillVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NOilSpillVfx.MethodName.OnAnimationEvent) && ((NativeVariantPtrArgs) ref args).Count == 4)
    {
      this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[3]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NOilSpillVfx.MethodName.OnAnimationStart) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      this.OnAnimationStart(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NOilSpillVfx.MethodName.TurnOnSprayAttack) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.TurnOnSprayAttack();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NOilSpillVfx.MethodName.TurnOffSprayAttack) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.TurnOffSprayAttack();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NOilSpillVfx.MethodName.TurnOnSlamSpray) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.TurnOnSlamSpray();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NOilSpillVfx.MethodName.TurnOffSlamSpray) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.TurnOffSlamSpray();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NOilSpillVfx.MethodName.TurnOnDeathSpray) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.TurnOnDeathSpray();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NOilSpillVfx.MethodName.TurnOffDeathSpray) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.TurnOffDeathSpray();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NOilSpillVfx.MethodName.TurnOnDrool) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.TurnOnDrool();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NOilSpillVfx.MethodName.TurnOffDrool) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.TurnOffDrool();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NOilSpillVfx.MethodName._Ready) || StringName.op_Equality(ref method, NOilSpillVfx.MethodName.OnAnimationEvent) || StringName.op_Equality(ref method, NOilSpillVfx.MethodName.OnAnimationStart) || StringName.op_Equality(ref method, NOilSpillVfx.MethodName.TurnOnSprayAttack) || StringName.op_Equality(ref method, NOilSpillVfx.MethodName.TurnOffSprayAttack) || StringName.op_Equality(ref method, NOilSpillVfx.MethodName.TurnOnSlamSpray) || StringName.op_Equality(ref method, NOilSpillVfx.MethodName.TurnOffSlamSpray) || StringName.op_Equality(ref method, NOilSpillVfx.MethodName.TurnOnDeathSpray) || StringName.op_Equality(ref method, NOilSpillVfx.MethodName.TurnOffDeathSpray) || StringName.op_Equality(ref method, NOilSpillVfx.MethodName.TurnOnDrool) || StringName.op_Equality(ref method, NOilSpillVfx.MethodName.TurnOffDrool) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NOilSpillVfx.PropertyName._droolParticles))
    {
      this._droolParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NOilSpillVfx.PropertyName._sprayParticles))
    {
      this._sprayParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NOilSpillVfx.PropertyName._rainDropParticles))
    {
      this._rainDropParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NOilSpillVfx.PropertyName._parent))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NOilSpillVfx.PropertyName._droolParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._droolParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NOilSpillVfx.PropertyName._sprayParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._sprayParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NOilSpillVfx.PropertyName._rainDropParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._rainDropParticles);
      return true;
    }
    if (!StringName.op_Equality(ref name, NOilSpillVfx.PropertyName._parent))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NOilSpillVfx.PropertyName._droolParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NOilSpillVfx.PropertyName._sprayParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NOilSpillVfx.PropertyName._rainDropParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NOilSpillVfx.PropertyName._parent, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NOilSpillVfx.PropertyName._droolParticles, Variant.From<GpuParticles2D>(ref this._droolParticles));
    info.AddProperty(NOilSpillVfx.PropertyName._sprayParticles, Variant.From<GpuParticles2D>(ref this._sprayParticles));
    info.AddProperty(NOilSpillVfx.PropertyName._rainDropParticles, Variant.From<GpuParticles2D>(ref this._rainDropParticles));
    info.AddProperty(NOilSpillVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NOilSpillVfx.PropertyName._droolParticles, ref variant1))
      this._droolParticles = ((Variant) ref variant1).As<GpuParticles2D>();
    Variant variant2;
    if (info.TryGetProperty(NOilSpillVfx.PropertyName._sprayParticles, ref variant2))
      this._sprayParticles = ((Variant) ref variant2).As<GpuParticles2D>();
    Variant variant3;
    if (info.TryGetProperty(NOilSpillVfx.PropertyName._rainDropParticles, ref variant3))
      this._rainDropParticles = ((Variant) ref variant3).As<GpuParticles2D>();
    Variant variant4;
    if (!info.TryGetProperty(NOilSpillVfx.PropertyName._parent, ref variant4))
      return;
    this._parent = ((Variant) ref variant4).As<Node2D>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnAnimationEvent = StringName.op_Implicit(nameof (OnAnimationEvent));
    public static readonly StringName OnAnimationStart = StringName.op_Implicit(nameof (OnAnimationStart));
    public static readonly StringName TurnOnSprayAttack = StringName.op_Implicit(nameof (TurnOnSprayAttack));
    public static readonly StringName TurnOffSprayAttack = StringName.op_Implicit(nameof (TurnOffSprayAttack));
    public static readonly StringName TurnOnSlamSpray = StringName.op_Implicit(nameof (TurnOnSlamSpray));
    public static readonly StringName TurnOffSlamSpray = StringName.op_Implicit(nameof (TurnOffSlamSpray));
    public static readonly StringName TurnOnDeathSpray = StringName.op_Implicit(nameof (TurnOnDeathSpray));
    public static readonly StringName TurnOffDeathSpray = StringName.op_Implicit(nameof (TurnOffDeathSpray));
    public static readonly StringName TurnOnDrool = StringName.op_Implicit(nameof (TurnOnDrool));
    public static readonly StringName TurnOffDrool = StringName.op_Implicit(nameof (TurnOffDrool));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName _droolParticles = StringName.op_Implicit(nameof (_droolParticles));
    public static readonly StringName _sprayParticles = StringName.op_Implicit(nameof (_sprayParticles));
    public static readonly StringName _rainDropParticles = StringName.op_Implicit(nameof (_rainDropParticles));
    public static readonly StringName _parent = StringName.op_Implicit(nameof (_parent));
  }

  public class SignalName : Node.SignalName
  {
  }
}
