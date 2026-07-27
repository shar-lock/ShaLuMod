// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NTestSubjectVfx
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
[ScriptPath("res://src/Core/Nodes/Vfx/NTestSubjectVfx.cs")]
public class NTestSubjectVfx : Node
{
  private GpuParticles2D _neckParticles;
  private GpuParticles2D _dizzyParticles;
  private GpuParticles2D _emberParticles;
  private GpuParticles2D _flameParticles;
  private GpuParticles2D _burnParticles;
  private GpuParticles2D _targetedBurnParticle;
  private GpuParticles2D _burnParticleFountain;
  private GpuParticles2D _ceilingParticles;
  private Node2D _parent;
  private MegaSprite _animController;
  private MegaSprite _frontBurnVfxController;
  private MegaSprite _backBurnVfxController;
  private bool _keyDown;
  private bool _doingThing;

  public override void _Ready()
  {
    this._parent = this.GetParent<Node2D>();
    this._animController = new MegaSprite(Variant.op_Implicit((GodotObject) this._parent));
    this._frontBurnVfxController = new MegaSprite(Variant.op_Implicit((GodotObject) this.GetNode(NodePath.op_Implicit("../FrontBurnVfxSlot/FrontBurnVfx"))));
    this._backBurnVfxController = new MegaSprite(Variant.op_Implicit((GodotObject) this.GetNode(NodePath.op_Implicit("../BackBurnVfxSlot/BackBurnVfx"))));
    this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
    this._neckParticles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("NeckParticlesSlot/NeckParticles"));
    this._dizzyParticles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("NeckParticlesSlot/DizzyPaticles"));
    this._emberParticles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("../../EmberParticles"));
    this._flameParticles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("../../FlameParticles"));
    this._burnParticles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("../../BurnParticles"));
    this._targetedBurnParticle = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("../../TargetedBurnParticle"));
    this._burnParticleFountain = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("../../BurnParticleFountain"));
    this._ceilingParticles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("../../CeilingSparks"));
    this._neckParticles.OneShot = true;
    this._neckParticles.Emitting = false;
    this._dizzyParticles.Emitting = false;
    this._emberParticles.OneShot = true;
    this._emberParticles.Emitting = false;
    this._flameParticles.Emitting = false;
    this._burnParticles.Emitting = false;
    this._targetedBurnParticle.Emitting = false;
    this._burnParticleFountain.Emitting = false;
    this._ceilingParticles.OneShot = true;
    this._ceilingParticles.Emitting = false;
    this.RunWhenSpineReady(this._animController, (Action<MegaAnimationState>) (animState => animState.SetAnimation("idle_loop3")));
    this.RunWhenSpineReady(this._frontBurnVfxController, (Action<MegaAnimationState>) (animState => animState.SetAnimation("empty")));
    this.RunWhenSpineReady(this._backBurnVfxController, (Action<MegaAnimationState>) (animState => animState.SetAnimation("empty")));
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
      case 10:
        if (!(eventName == "end_flames"))
          break;
        this.EndFlames();
        break;
      case 11:
        if (!(eventName == "end_dizzies"))
          break;
        this.EndDizzies();
        break;
      case 12:
        switch (eventName[6])
        {
          case 'e':
            if (!(eventName == "start_embers"))
              return;
            this.StartEmbers();
            return;
          case 'f':
            if (!(eventName == "start_flames"))
              return;
            this.StartFlames();
            return;
          case 'r':
            if (!(eventName == "end_burn_vfx"))
              return;
            this.EndBurnVfx();
            return;
          case 'x':
            if (!(eventName == "neck_explode"))
              return;
            this.SquirtNeck();
            return;
          default:
            return;
        }
      case 13:
        if (!(eventName == "start_dizzies"))
          break;
        this.StartDizzies();
        break;
      case 14:
        if (!(eventName == "start_burn_vfx"))
          break;
        this.StartBurnVfx();
        break;
      case 20:
        if (!(eventName == "start_ceiling_sparks"))
          break;
        this.StartCeilingSparks();
        break;
    }
  }

  private void PlayAnim1()
  {
    this._animController.GetAnimationState().SetAnimation("die3", false);
    this._animController.GetAnimationState().AddAnimation("idle_loop3");
  }

  private void SquirtNeck() => this._neckParticles.Restart();

  private void StartDizzies()
  {
    if (this._dizzyParticles.Emitting)
      return;
    this._dizzyParticles.Emitting = true;
  }

  private void EndDizzies() => this._dizzyParticles.Emitting = false;

  private void StartEmbers() => this._emberParticles.Restart();

  private void StartFlames() => this._flameParticles.Emitting = true;

  private void EndFlames() => this._flameParticles.Emitting = false;

  private void StartBurnVfx()
  {
    this._frontBurnVfxController.GetAnimationState().SetAnimation("burn", false);
    this._backBurnVfxController.GetAnimationState().SetAnimation("burn", false);
    this._burnParticles.Restart();
    this._targetedBurnParticle.Emitting = true;
    this._burnParticleFountain.Restart();
  }

  private void EndBurnVfx()
  {
    this._burnParticles.Emitting = false;
    this._targetedBurnParticle.Emitting = false;
    this._burnParticleFountain.Emitting = false;
  }

  private void StartCeilingSparks() => this._ceilingParticles.Restart();

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(12)
    {
      new MethodInfo(NTestSubjectVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTestSubjectVfx.MethodName.OnAnimationEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("__"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("___"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("spineEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false)
      }, (List<Variant>) null),
      new MethodInfo(NTestSubjectVfx.MethodName.PlayAnim1, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTestSubjectVfx.MethodName.SquirtNeck, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTestSubjectVfx.MethodName.StartDizzies, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTestSubjectVfx.MethodName.EndDizzies, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTestSubjectVfx.MethodName.StartEmbers, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTestSubjectVfx.MethodName.StartFlames, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTestSubjectVfx.MethodName.EndFlames, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTestSubjectVfx.MethodName.StartBurnVfx, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTestSubjectVfx.MethodName.EndBurnVfx, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTestSubjectVfx.MethodName.StartCeilingSparks, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NTestSubjectVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTestSubjectVfx.MethodName.OnAnimationEvent) && ((NativeVariantPtrArgs) ref args).Count == 4)
    {
      this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[3]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTestSubjectVfx.MethodName.PlayAnim1) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.PlayAnim1();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTestSubjectVfx.MethodName.SquirtNeck) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SquirtNeck();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTestSubjectVfx.MethodName.StartDizzies) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.StartDizzies();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTestSubjectVfx.MethodName.EndDizzies) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.EndDizzies();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTestSubjectVfx.MethodName.StartEmbers) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.StartEmbers();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTestSubjectVfx.MethodName.StartFlames) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.StartFlames();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTestSubjectVfx.MethodName.EndFlames) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.EndFlames();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTestSubjectVfx.MethodName.StartBurnVfx) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.StartBurnVfx();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTestSubjectVfx.MethodName.EndBurnVfx) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.EndBurnVfx();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NTestSubjectVfx.MethodName.StartCeilingSparks) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.StartCeilingSparks();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NTestSubjectVfx.MethodName._Ready) || StringName.op_Equality(ref method, NTestSubjectVfx.MethodName.OnAnimationEvent) || StringName.op_Equality(ref method, NTestSubjectVfx.MethodName.PlayAnim1) || StringName.op_Equality(ref method, NTestSubjectVfx.MethodName.SquirtNeck) || StringName.op_Equality(ref method, NTestSubjectVfx.MethodName.StartDizzies) || StringName.op_Equality(ref method, NTestSubjectVfx.MethodName.EndDizzies) || StringName.op_Equality(ref method, NTestSubjectVfx.MethodName.StartEmbers) || StringName.op_Equality(ref method, NTestSubjectVfx.MethodName.StartFlames) || StringName.op_Equality(ref method, NTestSubjectVfx.MethodName.EndFlames) || StringName.op_Equality(ref method, NTestSubjectVfx.MethodName.StartBurnVfx) || StringName.op_Equality(ref method, NTestSubjectVfx.MethodName.EndBurnVfx) || StringName.op_Equality(ref method, NTestSubjectVfx.MethodName.StartCeilingSparks) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NTestSubjectVfx.PropertyName._neckParticles))
    {
      this._neckParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTestSubjectVfx.PropertyName._dizzyParticles))
    {
      this._dizzyParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTestSubjectVfx.PropertyName._emberParticles))
    {
      this._emberParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTestSubjectVfx.PropertyName._flameParticles))
    {
      this._flameParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTestSubjectVfx.PropertyName._burnParticles))
    {
      this._burnParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTestSubjectVfx.PropertyName._targetedBurnParticle))
    {
      this._targetedBurnParticle = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTestSubjectVfx.PropertyName._burnParticleFountain))
    {
      this._burnParticleFountain = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTestSubjectVfx.PropertyName._ceilingParticles))
    {
      this._ceilingParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTestSubjectVfx.PropertyName._parent))
    {
      this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTestSubjectVfx.PropertyName._keyDown))
    {
      this._keyDown = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NTestSubjectVfx.PropertyName._doingThing))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._doingThing = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NTestSubjectVfx.PropertyName._neckParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._neckParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NTestSubjectVfx.PropertyName._dizzyParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._dizzyParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NTestSubjectVfx.PropertyName._emberParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._emberParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NTestSubjectVfx.PropertyName._flameParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._flameParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NTestSubjectVfx.PropertyName._burnParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._burnParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NTestSubjectVfx.PropertyName._targetedBurnParticle))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._targetedBurnParticle);
      return true;
    }
    if (StringName.op_Equality(ref name, NTestSubjectVfx.PropertyName._burnParticleFountain))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._burnParticleFountain);
      return true;
    }
    if (StringName.op_Equality(ref name, NTestSubjectVfx.PropertyName._ceilingParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._ceilingParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NTestSubjectVfx.PropertyName._parent))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
      return true;
    }
    if (StringName.op_Equality(ref name, NTestSubjectVfx.PropertyName._keyDown))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._keyDown);
      return true;
    }
    if (!StringName.op_Equality(ref name, NTestSubjectVfx.PropertyName._doingThing))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<bool>(ref this._doingThing);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NTestSubjectVfx.PropertyName._neckParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTestSubjectVfx.PropertyName._dizzyParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTestSubjectVfx.PropertyName._emberParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTestSubjectVfx.PropertyName._flameParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTestSubjectVfx.PropertyName._burnParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTestSubjectVfx.PropertyName._targetedBurnParticle, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTestSubjectVfx.PropertyName._burnParticleFountain, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTestSubjectVfx.PropertyName._ceilingParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTestSubjectVfx.PropertyName._parent, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NTestSubjectVfx.PropertyName._keyDown, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NTestSubjectVfx.PropertyName._doingThing, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NTestSubjectVfx.PropertyName._neckParticles, Variant.From<GpuParticles2D>(ref this._neckParticles));
    info.AddProperty(NTestSubjectVfx.PropertyName._dizzyParticles, Variant.From<GpuParticles2D>(ref this._dizzyParticles));
    info.AddProperty(NTestSubjectVfx.PropertyName._emberParticles, Variant.From<GpuParticles2D>(ref this._emberParticles));
    info.AddProperty(NTestSubjectVfx.PropertyName._flameParticles, Variant.From<GpuParticles2D>(ref this._flameParticles));
    info.AddProperty(NTestSubjectVfx.PropertyName._burnParticles, Variant.From<GpuParticles2D>(ref this._burnParticles));
    info.AddProperty(NTestSubjectVfx.PropertyName._targetedBurnParticle, Variant.From<GpuParticles2D>(ref this._targetedBurnParticle));
    info.AddProperty(NTestSubjectVfx.PropertyName._burnParticleFountain, Variant.From<GpuParticles2D>(ref this._burnParticleFountain));
    info.AddProperty(NTestSubjectVfx.PropertyName._ceilingParticles, Variant.From<GpuParticles2D>(ref this._ceilingParticles));
    info.AddProperty(NTestSubjectVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
    info.AddProperty(NTestSubjectVfx.PropertyName._keyDown, Variant.From<bool>(ref this._keyDown));
    info.AddProperty(NTestSubjectVfx.PropertyName._doingThing, Variant.From<bool>(ref this._doingThing));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NTestSubjectVfx.PropertyName._neckParticles, ref variant1))
      this._neckParticles = ((Variant) ref variant1).As<GpuParticles2D>();
    Variant variant2;
    if (info.TryGetProperty(NTestSubjectVfx.PropertyName._dizzyParticles, ref variant2))
      this._dizzyParticles = ((Variant) ref variant2).As<GpuParticles2D>();
    Variant variant3;
    if (info.TryGetProperty(NTestSubjectVfx.PropertyName._emberParticles, ref variant3))
      this._emberParticles = ((Variant) ref variant3).As<GpuParticles2D>();
    Variant variant4;
    if (info.TryGetProperty(NTestSubjectVfx.PropertyName._flameParticles, ref variant4))
      this._flameParticles = ((Variant) ref variant4).As<GpuParticles2D>();
    Variant variant5;
    if (info.TryGetProperty(NTestSubjectVfx.PropertyName._burnParticles, ref variant5))
      this._burnParticles = ((Variant) ref variant5).As<GpuParticles2D>();
    Variant variant6;
    if (info.TryGetProperty(NTestSubjectVfx.PropertyName._targetedBurnParticle, ref variant6))
      this._targetedBurnParticle = ((Variant) ref variant6).As<GpuParticles2D>();
    Variant variant7;
    if (info.TryGetProperty(NTestSubjectVfx.PropertyName._burnParticleFountain, ref variant7))
      this._burnParticleFountain = ((Variant) ref variant7).As<GpuParticles2D>();
    Variant variant8;
    if (info.TryGetProperty(NTestSubjectVfx.PropertyName._ceilingParticles, ref variant8))
      this._ceilingParticles = ((Variant) ref variant8).As<GpuParticles2D>();
    Variant variant9;
    if (info.TryGetProperty(NTestSubjectVfx.PropertyName._parent, ref variant9))
      this._parent = ((Variant) ref variant9).As<Node2D>();
    Variant variant10;
    if (info.TryGetProperty(NTestSubjectVfx.PropertyName._keyDown, ref variant10))
      this._keyDown = ((Variant) ref variant10).As<bool>();
    Variant variant11;
    if (!info.TryGetProperty(NTestSubjectVfx.PropertyName._doingThing, ref variant11))
      return;
    this._doingThing = ((Variant) ref variant11).As<bool>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnAnimationEvent = StringName.op_Implicit(nameof (OnAnimationEvent));
    public static readonly StringName PlayAnim1 = StringName.op_Implicit(nameof (PlayAnim1));
    public static readonly StringName SquirtNeck = StringName.op_Implicit(nameof (SquirtNeck));
    public static readonly StringName StartDizzies = StringName.op_Implicit(nameof (StartDizzies));
    public static readonly StringName EndDizzies = StringName.op_Implicit(nameof (EndDizzies));
    public static readonly StringName StartEmbers = StringName.op_Implicit(nameof (StartEmbers));
    public static readonly StringName StartFlames = StringName.op_Implicit(nameof (StartFlames));
    public static readonly StringName EndFlames = StringName.op_Implicit(nameof (EndFlames));
    public static readonly StringName StartBurnVfx = StringName.op_Implicit(nameof (StartBurnVfx));
    public static readonly StringName EndBurnVfx = StringName.op_Implicit(nameof (EndBurnVfx));
    public static readonly StringName StartCeilingSparks = StringName.op_Implicit(nameof (StartCeilingSparks));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName _neckParticles = StringName.op_Implicit(nameof (_neckParticles));
    public static readonly StringName _dizzyParticles = StringName.op_Implicit(nameof (_dizzyParticles));
    public static readonly StringName _emberParticles = StringName.op_Implicit(nameof (_emberParticles));
    public static readonly StringName _flameParticles = StringName.op_Implicit(nameof (_flameParticles));
    public static readonly StringName _burnParticles = StringName.op_Implicit(nameof (_burnParticles));
    public static readonly StringName _targetedBurnParticle = StringName.op_Implicit(nameof (_targetedBurnParticle));
    public static readonly StringName _burnParticleFountain = StringName.op_Implicit(nameof (_burnParticleFountain));
    public static readonly StringName _ceilingParticles = StringName.op_Implicit(nameof (_ceilingParticles));
    public static readonly StringName _parent = StringName.op_Implicit(nameof (_parent));
    public static readonly StringName _keyDown = StringName.op_Implicit(nameof (_keyDown));
    public static readonly StringName _doingThing = StringName.op_Implicit(nameof (_doingThing));
  }

  public class SignalName : Node.SignalName
  {
  }
}
