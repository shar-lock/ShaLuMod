// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NAmalgamVfx
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
[ScriptPath("res://src/Core/Nodes/Vfx/NAmalgamVfx.cs")]
public class NAmalgamVfx : Node
{
  [Export]
  private GpuParticles2D _hitFxParticles;
  [Export]
  private Node2D _hitBoneNode;
  private CpuParticles2D _deathBodyParticles;
  private GpuParticles2D _laserBaseParticles;
  private GpuParticles2D _hitParticles1;
  private GpuParticles2D _hitParticles2;
  private GpuParticles2D _hitParticles3;
  private GpuParticles2D _constantSparks1;
  private GpuParticles2D _constantSparks2;
  private GpuParticles2D _constantSparks3;
  private Node2D _torch1Node;
  private Node2D _torch2Node;
  private Node2D _torch3Node;
  private Node _parent;
  private MegaSprite _animController;

  public override void _Ready()
  {
    this._parent = this.GetParent();
    this._animController = new MegaSprite(Variant.op_Implicit((GodotObject) this._parent));
    this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
    this._deathBodyParticles = this._parent.GetNode<CpuParticles2D>(NodePath.op_Implicit("CPUDeathParticles"));
    this._deathBodyParticles.Emitting = false;
    this._deathBodyParticles.OneShot = true;
    this._laserBaseParticles = this._parent.GetNode<GpuParticles2D>(NodePath.op_Implicit("laserBaseBone/laserBaseParticles"));
    this._laserBaseParticles.Emitting = false;
    this._torch1Node = this._parent.GetNode<Node2D>(NodePath.op_Implicit("torch1Slot/fire1_small_green"));
    ((CanvasItem) this._torch1Node).Visible = true;
    this._torch2Node = this._parent.GetNode<Node2D>(NodePath.op_Implicit("torch2Slot/fire2_small_green"));
    ((CanvasItem) this._torch2Node).Visible = true;
    this._torch3Node = this._parent.GetNode<Node2D>(NodePath.op_Implicit("torch3Slot/fire3_small_green"));
    ((CanvasItem) this._torch3Node).Visible = true;
    this._hitParticles1 = this._parent.GetNode<GpuParticles2D>(NodePath.op_Implicit("torch1UnscaledBone/hitParticles"));
    this._hitParticles1.Emitting = false;
    this._hitParticles1.OneShot = true;
    this._hitParticles2 = this._parent.GetNode<GpuParticles2D>(NodePath.op_Implicit("torch2UnscaledBone/hitParticles"));
    this._hitParticles2.Emitting = false;
    this._hitParticles2.OneShot = true;
    this._hitParticles3 = this._parent.GetNode<GpuParticles2D>(NodePath.op_Implicit("torch3UnscaledBone/hitParticles"));
    this._hitParticles3.Emitting = false;
    this._hitParticles3.OneShot = true;
    this._constantSparks1 = this._parent.GetNode<GpuParticles2D>(NodePath.op_Implicit("torch1Slot/constantParticles"));
    this._constantSparks2 = this._parent.GetNode<GpuParticles2D>(NodePath.op_Implicit("torch2Slot/constantParticles"));
    this._constantSparks3 = this._parent.GetNode<GpuParticles2D>(NodePath.op_Implicit("torch3Slot/constantParticles"));
    ((CanvasItem) this._hitFxParticles).Visible = false;
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
      case 4:
        switch (eventName[3])
        {
          case '1':
            if (!(eventName == "hit1"))
              return;
            this.PlayHit1();
            return;
          case '2':
            if (!(eventName == "hit2"))
              return;
            this.PlayHit2();
            return;
          case '3':
            if (!(eventName == "hit3"))
              return;
            this.PlayHit3();
            return;
          default:
            return;
        }
      case 7:
        if (!(eventName == "go_poof"))
          break;
        this.PoofToDeath();
        break;
      case 10:
        if (!(eventName == "torches_on"))
          break;
        this.RestartTorches();
        break;
      case 11:
        if (!(eventName == "torches_out"))
          break;
        this.KillTorches();
        break;
      case 13:
        if (!(eventName == "laser_hit_off"))
          break;
        this.PlayLaserHit(false);
        break;
      case 14:
        switch (eventName[6])
        {
          case 'b':
            if (!(eventName == "laser_base_off"))
              return;
            this.PlayLaserBase(false);
            return;
          case 'h':
            if (!(eventName == "laser_hit_fire"))
              return;
            this.PlayLaserHit(true);
            return;
          default:
            return;
        }
      case 15:
        if (!(eventName == "laser_base_fire"))
          break;
        this.PlayLaserBase(true);
        break;
    }
  }

  private void PoofToDeath() => this._deathBodyParticles.Restart();

  private void RestartTorches()
  {
    ((CanvasItem) this._torch1Node).Visible = true;
    ((CanvasItem) this._torch2Node).Visible = true;
    ((CanvasItem) this._torch3Node).Visible = true;
    this._constantSparks1.Emitting = true;
    this._constantSparks2.Emitting = true;
    this._constantSparks3.Emitting = true;
  }

  private void KillTorches()
  {
    ((CanvasItem) this._torch1Node).Visible = false;
    ((CanvasItem) this._torch2Node).Visible = false;
    ((CanvasItem) this._torch3Node).Visible = false;
    this._constantSparks1.Emitting = false;
    this._constantSparks2.Emitting = false;
    this._constantSparks3.Emitting = false;
  }

  private void PlayHit1() => this._hitParticles1.Restart();

  private void PlayHit2() => this._hitParticles2.Restart();

  private void PlayHit3() => this._hitParticles3.Restart();

  private void PlayLaserBase(bool starting)
  {
    if (starting)
    {
      ((CanvasItem) this._laserBaseParticles).Visible = true;
      this._laserBaseParticles.Restart();
    }
    else
    {
      this._laserBaseParticles.Emitting = false;
      ((CanvasItem) this._laserBaseParticles).Visible = false;
    }
  }

  private void PlayLaserHit(bool starting)
  {
    if (starting)
    {
      ((Node2D) this._hitFxParticles).GlobalPosition = this._hitBoneNode.GlobalPosition;
      ((CanvasItem) this._hitFxParticles).Visible = true;
      this._hitFxParticles.Restart();
    }
    else
    {
      this._hitFxParticles.Emitting = false;
      ((CanvasItem) this._hitFxParticles).Visible = false;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(10)
    {
      new MethodInfo(NAmalgamVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAmalgamVfx.MethodName.OnAnimationEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("__"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("___"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("spineEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false)
      }, (List<Variant>) null),
      new MethodInfo(NAmalgamVfx.MethodName.PoofToDeath, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAmalgamVfx.MethodName.RestartTorches, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAmalgamVfx.MethodName.KillTorches, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAmalgamVfx.MethodName.PlayHit1, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAmalgamVfx.MethodName.PlayHit2, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAmalgamVfx.MethodName.PlayHit3, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAmalgamVfx.MethodName.PlayLaserBase, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("starting"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NAmalgamVfx.MethodName.PlayLaserHit, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("starting"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NAmalgamVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAmalgamVfx.MethodName.OnAnimationEvent) && ((NativeVariantPtrArgs) ref args).Count == 4)
    {
      this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[3]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAmalgamVfx.MethodName.PoofToDeath) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.PoofToDeath();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAmalgamVfx.MethodName.RestartTorches) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RestartTorches();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAmalgamVfx.MethodName.KillTorches) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.KillTorches();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAmalgamVfx.MethodName.PlayHit1) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.PlayHit1();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAmalgamVfx.MethodName.PlayHit2) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.PlayHit2();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAmalgamVfx.MethodName.PlayHit3) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.PlayHit3();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAmalgamVfx.MethodName.PlayLaserBase) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.PlayLaserBase(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NAmalgamVfx.MethodName.PlayLaserHit) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.PlayLaserHit(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NAmalgamVfx.MethodName._Ready) || StringName.op_Equality(ref method, NAmalgamVfx.MethodName.OnAnimationEvent) || StringName.op_Equality(ref method, NAmalgamVfx.MethodName.PoofToDeath) || StringName.op_Equality(ref method, NAmalgamVfx.MethodName.RestartTorches) || StringName.op_Equality(ref method, NAmalgamVfx.MethodName.KillTorches) || StringName.op_Equality(ref method, NAmalgamVfx.MethodName.PlayHit1) || StringName.op_Equality(ref method, NAmalgamVfx.MethodName.PlayHit2) || StringName.op_Equality(ref method, NAmalgamVfx.MethodName.PlayHit3) || StringName.op_Equality(ref method, NAmalgamVfx.MethodName.PlayLaserBase) || StringName.op_Equality(ref method, NAmalgamVfx.MethodName.PlayLaserHit) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NAmalgamVfx.PropertyName._hitFxParticles))
    {
      this._hitFxParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAmalgamVfx.PropertyName._hitBoneNode))
    {
      this._hitBoneNode = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAmalgamVfx.PropertyName._deathBodyParticles))
    {
      this._deathBodyParticles = VariantUtils.ConvertTo<CpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAmalgamVfx.PropertyName._laserBaseParticles))
    {
      this._laserBaseParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAmalgamVfx.PropertyName._hitParticles1))
    {
      this._hitParticles1 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAmalgamVfx.PropertyName._hitParticles2))
    {
      this._hitParticles2 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAmalgamVfx.PropertyName._hitParticles3))
    {
      this._hitParticles3 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAmalgamVfx.PropertyName._constantSparks1))
    {
      this._constantSparks1 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAmalgamVfx.PropertyName._constantSparks2))
    {
      this._constantSparks2 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAmalgamVfx.PropertyName._constantSparks3))
    {
      this._constantSparks3 = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAmalgamVfx.PropertyName._torch1Node))
    {
      this._torch1Node = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAmalgamVfx.PropertyName._torch2Node))
    {
      this._torch2Node = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NAmalgamVfx.PropertyName._torch3Node))
    {
      this._torch3Node = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NAmalgamVfx.PropertyName._parent))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._parent = VariantUtils.ConvertTo<Node>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NAmalgamVfx.PropertyName._hitFxParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._hitFxParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NAmalgamVfx.PropertyName._hitBoneNode))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._hitBoneNode);
      return true;
    }
    if (StringName.op_Equality(ref name, NAmalgamVfx.PropertyName._deathBodyParticles))
    {
      value = VariantUtils.CreateFrom<CpuParticles2D>(ref this._deathBodyParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NAmalgamVfx.PropertyName._laserBaseParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._laserBaseParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NAmalgamVfx.PropertyName._hitParticles1))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._hitParticles1);
      return true;
    }
    if (StringName.op_Equality(ref name, NAmalgamVfx.PropertyName._hitParticles2))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._hitParticles2);
      return true;
    }
    if (StringName.op_Equality(ref name, NAmalgamVfx.PropertyName._hitParticles3))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._hitParticles3);
      return true;
    }
    if (StringName.op_Equality(ref name, NAmalgamVfx.PropertyName._constantSparks1))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._constantSparks1);
      return true;
    }
    if (StringName.op_Equality(ref name, NAmalgamVfx.PropertyName._constantSparks2))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._constantSparks2);
      return true;
    }
    if (StringName.op_Equality(ref name, NAmalgamVfx.PropertyName._constantSparks3))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._constantSparks3);
      return true;
    }
    if (StringName.op_Equality(ref name, NAmalgamVfx.PropertyName._torch1Node))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._torch1Node);
      return true;
    }
    if (StringName.op_Equality(ref name, NAmalgamVfx.PropertyName._torch2Node))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._torch2Node);
      return true;
    }
    if (StringName.op_Equality(ref name, NAmalgamVfx.PropertyName._torch3Node))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._torch3Node);
      return true;
    }
    if (!StringName.op_Equality(ref name, NAmalgamVfx.PropertyName._parent))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Node>(ref this._parent);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NAmalgamVfx.PropertyName._hitFxParticles, (PropertyHint) 34L, "GPUParticles2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NAmalgamVfx.PropertyName._hitBoneNode, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NAmalgamVfx.PropertyName._deathBodyParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAmalgamVfx.PropertyName._laserBaseParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAmalgamVfx.PropertyName._hitParticles1, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAmalgamVfx.PropertyName._hitParticles2, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAmalgamVfx.PropertyName._hitParticles3, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAmalgamVfx.PropertyName._constantSparks1, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAmalgamVfx.PropertyName._constantSparks2, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAmalgamVfx.PropertyName._constantSparks3, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAmalgamVfx.PropertyName._torch1Node, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAmalgamVfx.PropertyName._torch2Node, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAmalgamVfx.PropertyName._torch3Node, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NAmalgamVfx.PropertyName._parent, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NAmalgamVfx.PropertyName._hitFxParticles, Variant.From<GpuParticles2D>(ref this._hitFxParticles));
    info.AddProperty(NAmalgamVfx.PropertyName._hitBoneNode, Variant.From<Node2D>(ref this._hitBoneNode));
    info.AddProperty(NAmalgamVfx.PropertyName._deathBodyParticles, Variant.From<CpuParticles2D>(ref this._deathBodyParticles));
    info.AddProperty(NAmalgamVfx.PropertyName._laserBaseParticles, Variant.From<GpuParticles2D>(ref this._laserBaseParticles));
    info.AddProperty(NAmalgamVfx.PropertyName._hitParticles1, Variant.From<GpuParticles2D>(ref this._hitParticles1));
    info.AddProperty(NAmalgamVfx.PropertyName._hitParticles2, Variant.From<GpuParticles2D>(ref this._hitParticles2));
    info.AddProperty(NAmalgamVfx.PropertyName._hitParticles3, Variant.From<GpuParticles2D>(ref this._hitParticles3));
    info.AddProperty(NAmalgamVfx.PropertyName._constantSparks1, Variant.From<GpuParticles2D>(ref this._constantSparks1));
    info.AddProperty(NAmalgamVfx.PropertyName._constantSparks2, Variant.From<GpuParticles2D>(ref this._constantSparks2));
    info.AddProperty(NAmalgamVfx.PropertyName._constantSparks3, Variant.From<GpuParticles2D>(ref this._constantSparks3));
    info.AddProperty(NAmalgamVfx.PropertyName._torch1Node, Variant.From<Node2D>(ref this._torch1Node));
    info.AddProperty(NAmalgamVfx.PropertyName._torch2Node, Variant.From<Node2D>(ref this._torch2Node));
    info.AddProperty(NAmalgamVfx.PropertyName._torch3Node, Variant.From<Node2D>(ref this._torch3Node));
    info.AddProperty(NAmalgamVfx.PropertyName._parent, Variant.From<Node>(ref this._parent));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NAmalgamVfx.PropertyName._hitFxParticles, ref variant1))
      this._hitFxParticles = ((Variant) ref variant1).As<GpuParticles2D>();
    Variant variant2;
    if (info.TryGetProperty(NAmalgamVfx.PropertyName._hitBoneNode, ref variant2))
      this._hitBoneNode = ((Variant) ref variant2).As<Node2D>();
    Variant variant3;
    if (info.TryGetProperty(NAmalgamVfx.PropertyName._deathBodyParticles, ref variant3))
      this._deathBodyParticles = ((Variant) ref variant3).As<CpuParticles2D>();
    Variant variant4;
    if (info.TryGetProperty(NAmalgamVfx.PropertyName._laserBaseParticles, ref variant4))
      this._laserBaseParticles = ((Variant) ref variant4).As<GpuParticles2D>();
    Variant variant5;
    if (info.TryGetProperty(NAmalgamVfx.PropertyName._hitParticles1, ref variant5))
      this._hitParticles1 = ((Variant) ref variant5).As<GpuParticles2D>();
    Variant variant6;
    if (info.TryGetProperty(NAmalgamVfx.PropertyName._hitParticles2, ref variant6))
      this._hitParticles2 = ((Variant) ref variant6).As<GpuParticles2D>();
    Variant variant7;
    if (info.TryGetProperty(NAmalgamVfx.PropertyName._hitParticles3, ref variant7))
      this._hitParticles3 = ((Variant) ref variant7).As<GpuParticles2D>();
    Variant variant8;
    if (info.TryGetProperty(NAmalgamVfx.PropertyName._constantSparks1, ref variant8))
      this._constantSparks1 = ((Variant) ref variant8).As<GpuParticles2D>();
    Variant variant9;
    if (info.TryGetProperty(NAmalgamVfx.PropertyName._constantSparks2, ref variant9))
      this._constantSparks2 = ((Variant) ref variant9).As<GpuParticles2D>();
    Variant variant10;
    if (info.TryGetProperty(NAmalgamVfx.PropertyName._constantSparks3, ref variant10))
      this._constantSparks3 = ((Variant) ref variant10).As<GpuParticles2D>();
    Variant variant11;
    if (info.TryGetProperty(NAmalgamVfx.PropertyName._torch1Node, ref variant11))
      this._torch1Node = ((Variant) ref variant11).As<Node2D>();
    Variant variant12;
    if (info.TryGetProperty(NAmalgamVfx.PropertyName._torch2Node, ref variant12))
      this._torch2Node = ((Variant) ref variant12).As<Node2D>();
    Variant variant13;
    if (info.TryGetProperty(NAmalgamVfx.PropertyName._torch3Node, ref variant13))
      this._torch3Node = ((Variant) ref variant13).As<Node2D>();
    Variant variant14;
    if (!info.TryGetProperty(NAmalgamVfx.PropertyName._parent, ref variant14))
      return;
    this._parent = ((Variant) ref variant14).As<Node>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnAnimationEvent = StringName.op_Implicit(nameof (OnAnimationEvent));
    public static readonly StringName PoofToDeath = StringName.op_Implicit(nameof (PoofToDeath));
    public static readonly StringName RestartTorches = StringName.op_Implicit(nameof (RestartTorches));
    public static readonly StringName KillTorches = StringName.op_Implicit(nameof (KillTorches));
    public static readonly StringName PlayHit1 = StringName.op_Implicit(nameof (PlayHit1));
    public static readonly StringName PlayHit2 = StringName.op_Implicit(nameof (PlayHit2));
    public static readonly StringName PlayHit3 = StringName.op_Implicit(nameof (PlayHit3));
    public static readonly StringName PlayLaserBase = StringName.op_Implicit(nameof (PlayLaserBase));
    public static readonly StringName PlayLaserHit = StringName.op_Implicit(nameof (PlayLaserHit));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName _hitFxParticles = StringName.op_Implicit(nameof (_hitFxParticles));
    public static readonly StringName _hitBoneNode = StringName.op_Implicit(nameof (_hitBoneNode));
    public static readonly StringName _deathBodyParticles = StringName.op_Implicit(nameof (_deathBodyParticles));
    public static readonly StringName _laserBaseParticles = StringName.op_Implicit(nameof (_laserBaseParticles));
    public static readonly StringName _hitParticles1 = StringName.op_Implicit(nameof (_hitParticles1));
    public static readonly StringName _hitParticles2 = StringName.op_Implicit(nameof (_hitParticles2));
    public static readonly StringName _hitParticles3 = StringName.op_Implicit(nameof (_hitParticles3));
    public static readonly StringName _constantSparks1 = StringName.op_Implicit(nameof (_constantSparks1));
    public static readonly StringName _constantSparks2 = StringName.op_Implicit(nameof (_constantSparks2));
    public static readonly StringName _constantSparks3 = StringName.op_Implicit(nameof (_constantSparks3));
    public static readonly StringName _torch1Node = StringName.op_Implicit(nameof (_torch1Node));
    public static readonly StringName _torch2Node = StringName.op_Implicit(nameof (_torch2Node));
    public static readonly StringName _torch3Node = StringName.op_Implicit(nameof (_torch3Node));
    public static readonly StringName _parent = StringName.op_Implicit(nameof (_parent));
  }

  public class SignalName : Node.SignalName
  {
  }
}
