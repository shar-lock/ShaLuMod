// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NKnowledgeDemonVfx
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
[ScriptPath("res://src/Core/Nodes/Vfx/NKnowledgeDemonVfx.cs")]
public class NKnowledgeDemonVfx : Node
{
  private Node2D _fireNode1;
  private Node2D _fireNode2;
  private Node2D _fireNode3;
  private Node2D _fireNode4;
  private GpuParticles2D _explosionParticles;
  private GpuParticles2D _damageParticles;
  private GpuParticles2D _emberParticles;
  private GpuParticles2D _thinEmberParticles;
  private Node2D _parent;
  private MegaSprite _animController;

  public override void _Ready()
  {
    this._parent = this.GetParent<Node2D>();
    this._animController = new MegaSprite(Variant.op_Implicit((GodotObject) this._parent));
    this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
    this._animController.ConnectAnimationStarted(Callable.From<GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject>(this.OnAnimationStart)));
    this._fireNode1 = ((Node) this._parent).GetNode<Node2D>(NodePath.op_Implicit("FireSlot1/FireHolder1"));
    this._fireNode2 = ((Node) this._parent).GetNode<Node2D>(NodePath.op_Implicit("FireSlot2/FireHolder2"));
    this._fireNode3 = ((Node) this._parent).GetNode<Node2D>(NodePath.op_Implicit("FireSlot3/FireHolder3"));
    this._fireNode4 = ((Node) this._parent).GetNode<Node2D>(NodePath.op_Implicit("FireSlot4/FireHolder4"));
    this._explosionParticles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("ExplosionParticles"));
    this._damageParticles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("DamageParticles"));
    this._emberParticles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("EmberParticles"));
    this._thinEmberParticles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("ThinEmberParticles"));
    ((CanvasItem) this._fireNode1).Visible = false;
    ((CanvasItem) this._fireNode2).Visible = false;
    ((CanvasItem) this._fireNode3).Visible = false;
    ((CanvasItem) this._fireNode4).Visible = false;
    this._explosionParticles.Emitting = false;
    this._explosionParticles.OneShot = true;
    this._damageParticles.Emitting = false;
    this._damageParticles.OneShot = true;
    this._emberParticles.Emitting = false;
    this._thinEmberParticles.Emitting = false;
    this.OnBurningEnd();
    this.RunWhenSpineReady(this._animController, (Action<MegaAnimationState>) (animState => animState.SetAnimation("idle_loop")));
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
      case 7:
        if (!(eventName == "explode"))
          break;
        this.OnExplode();
        break;
      case 10:
        if (!(eventName == "embers_end"))
          break;
        this.OnEmbersEnd();
        break;
      case 11:
        switch (eventName[0])
        {
          case 'b':
            if (!(eventName == "burning_end"))
              return;
            this.OnBurningEnd();
            return;
          case 't':
            if (!(eventName == "take_damage"))
              return;
            this.OnTakeDamage();
            return;
          default:
            return;
        }
      case 12:
        if (!(eventName == "embers_start"))
          break;
        this.OnEmbersStart();
        break;
      case 13:
        if (!(eventName == "burning_start"))
          break;
        this.OnBurningStart();
        break;
      case 15:
        if (!(eventName == "thin_embers_end"))
          break;
        this.OnThinEmbersEnd();
        break;
      case 17:
        if (!(eventName == "thin_embers_start"))
          break;
        this.OnThinEmbersStart();
        break;
    }
  }

  private void OnAnimationStart(
    GodotObject spineSprite,
    GodotObject animationState,
    GodotObject trackEntry)
  {
    this.OnBurningEnd();
    this.OnEmbersEnd();
    this.OnThinEmbersEnd();
  }

  private void OnExplode() => this._explosionParticles.Restart();

  private void OnTakeDamage() => this._damageParticles.Restart();

  private void OnBurningStart()
  {
    ((CanvasItem) this._fireNode1).Visible = true;
    ((CanvasItem) this._fireNode2).Visible = true;
    ((CanvasItem) this._fireNode3).Visible = true;
    ((CanvasItem) this._fireNode4).Visible = true;
  }

  private void OnEmbersStart() => this._emberParticles.Restart();

  private void OnThinEmbersStart() => this._thinEmberParticles.Restart();

  private void OnBurningEnd()
  {
    ((CanvasItem) this._fireNode1).Visible = false;
    ((CanvasItem) this._fireNode2).Visible = false;
    ((CanvasItem) this._fireNode3).Visible = false;
    ((CanvasItem) this._fireNode4).Visible = false;
  }

  private void OnEmbersEnd() => this._emberParticles.Emitting = false;

  private void OnThinEmbersEnd() => this._thinEmberParticles.Emitting = false;

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(11)
    {
      new MethodInfo(NKnowledgeDemonVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NKnowledgeDemonVfx.MethodName.OnAnimationEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("__"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("___"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("spineEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false)
      }, (List<Variant>) null),
      new MethodInfo(NKnowledgeDemonVfx.MethodName.OnAnimationStart, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("spineSprite"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("animationState"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("trackEntry"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false)
      }, (List<Variant>) null),
      new MethodInfo(NKnowledgeDemonVfx.MethodName.OnExplode, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NKnowledgeDemonVfx.MethodName.OnTakeDamage, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NKnowledgeDemonVfx.MethodName.OnBurningStart, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NKnowledgeDemonVfx.MethodName.OnEmbersStart, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NKnowledgeDemonVfx.MethodName.OnThinEmbersStart, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NKnowledgeDemonVfx.MethodName.OnBurningEnd, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NKnowledgeDemonVfx.MethodName.OnEmbersEnd, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NKnowledgeDemonVfx.MethodName.OnThinEmbersEnd, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NKnowledgeDemonVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NKnowledgeDemonVfx.MethodName.OnAnimationEvent) && ((NativeVariantPtrArgs) ref args).Count == 4)
    {
      this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[3]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NKnowledgeDemonVfx.MethodName.OnAnimationStart) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      this.OnAnimationStart(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NKnowledgeDemonVfx.MethodName.OnExplode) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnExplode();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NKnowledgeDemonVfx.MethodName.OnTakeDamage) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnTakeDamage();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NKnowledgeDemonVfx.MethodName.OnBurningStart) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnBurningStart();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NKnowledgeDemonVfx.MethodName.OnEmbersStart) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnEmbersStart();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NKnowledgeDemonVfx.MethodName.OnThinEmbersStart) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnThinEmbersStart();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NKnowledgeDemonVfx.MethodName.OnBurningEnd) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnBurningEnd();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NKnowledgeDemonVfx.MethodName.OnEmbersEnd) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnEmbersEnd();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NKnowledgeDemonVfx.MethodName.OnThinEmbersEnd) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnThinEmbersEnd();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NKnowledgeDemonVfx.MethodName._Ready) || StringName.op_Equality(ref method, NKnowledgeDemonVfx.MethodName.OnAnimationEvent) || StringName.op_Equality(ref method, NKnowledgeDemonVfx.MethodName.OnAnimationStart) || StringName.op_Equality(ref method, NKnowledgeDemonVfx.MethodName.OnExplode) || StringName.op_Equality(ref method, NKnowledgeDemonVfx.MethodName.OnTakeDamage) || StringName.op_Equality(ref method, NKnowledgeDemonVfx.MethodName.OnBurningStart) || StringName.op_Equality(ref method, NKnowledgeDemonVfx.MethodName.OnEmbersStart) || StringName.op_Equality(ref method, NKnowledgeDemonVfx.MethodName.OnThinEmbersStart) || StringName.op_Equality(ref method, NKnowledgeDemonVfx.MethodName.OnBurningEnd) || StringName.op_Equality(ref method, NKnowledgeDemonVfx.MethodName.OnEmbersEnd) || StringName.op_Equality(ref method, NKnowledgeDemonVfx.MethodName.OnThinEmbersEnd) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NKnowledgeDemonVfx.PropertyName._fireNode1))
    {
      this._fireNode1 = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NKnowledgeDemonVfx.PropertyName._fireNode2))
    {
      this._fireNode2 = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NKnowledgeDemonVfx.PropertyName._fireNode3))
    {
      this._fireNode3 = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NKnowledgeDemonVfx.PropertyName._fireNode4))
    {
      this._fireNode4 = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NKnowledgeDemonVfx.PropertyName._explosionParticles))
    {
      this._explosionParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NKnowledgeDemonVfx.PropertyName._damageParticles))
    {
      this._damageParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NKnowledgeDemonVfx.PropertyName._emberParticles))
    {
      this._emberParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NKnowledgeDemonVfx.PropertyName._thinEmberParticles))
    {
      this._thinEmberParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NKnowledgeDemonVfx.PropertyName._parent))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NKnowledgeDemonVfx.PropertyName._fireNode1))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._fireNode1);
      return true;
    }
    if (StringName.op_Equality(ref name, NKnowledgeDemonVfx.PropertyName._fireNode2))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._fireNode2);
      return true;
    }
    if (StringName.op_Equality(ref name, NKnowledgeDemonVfx.PropertyName._fireNode3))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._fireNode3);
      return true;
    }
    if (StringName.op_Equality(ref name, NKnowledgeDemonVfx.PropertyName._fireNode4))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._fireNode4);
      return true;
    }
    if (StringName.op_Equality(ref name, NKnowledgeDemonVfx.PropertyName._explosionParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._explosionParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NKnowledgeDemonVfx.PropertyName._damageParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._damageParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NKnowledgeDemonVfx.PropertyName._emberParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._emberParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NKnowledgeDemonVfx.PropertyName._thinEmberParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._thinEmberParticles);
      return true;
    }
    if (!StringName.op_Equality(ref name, NKnowledgeDemonVfx.PropertyName._parent))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NKnowledgeDemonVfx.PropertyName._fireNode1, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NKnowledgeDemonVfx.PropertyName._fireNode2, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NKnowledgeDemonVfx.PropertyName._fireNode3, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NKnowledgeDemonVfx.PropertyName._fireNode4, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NKnowledgeDemonVfx.PropertyName._explosionParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NKnowledgeDemonVfx.PropertyName._damageParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NKnowledgeDemonVfx.PropertyName._emberParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NKnowledgeDemonVfx.PropertyName._thinEmberParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NKnowledgeDemonVfx.PropertyName._parent, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NKnowledgeDemonVfx.PropertyName._fireNode1, Variant.From<Node2D>(ref this._fireNode1));
    info.AddProperty(NKnowledgeDemonVfx.PropertyName._fireNode2, Variant.From<Node2D>(ref this._fireNode2));
    info.AddProperty(NKnowledgeDemonVfx.PropertyName._fireNode3, Variant.From<Node2D>(ref this._fireNode3));
    info.AddProperty(NKnowledgeDemonVfx.PropertyName._fireNode4, Variant.From<Node2D>(ref this._fireNode4));
    info.AddProperty(NKnowledgeDemonVfx.PropertyName._explosionParticles, Variant.From<GpuParticles2D>(ref this._explosionParticles));
    info.AddProperty(NKnowledgeDemonVfx.PropertyName._damageParticles, Variant.From<GpuParticles2D>(ref this._damageParticles));
    info.AddProperty(NKnowledgeDemonVfx.PropertyName._emberParticles, Variant.From<GpuParticles2D>(ref this._emberParticles));
    info.AddProperty(NKnowledgeDemonVfx.PropertyName._thinEmberParticles, Variant.From<GpuParticles2D>(ref this._thinEmberParticles));
    info.AddProperty(NKnowledgeDemonVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NKnowledgeDemonVfx.PropertyName._fireNode1, ref variant1))
      this._fireNode1 = ((Variant) ref variant1).As<Node2D>();
    Variant variant2;
    if (info.TryGetProperty(NKnowledgeDemonVfx.PropertyName._fireNode2, ref variant2))
      this._fireNode2 = ((Variant) ref variant2).As<Node2D>();
    Variant variant3;
    if (info.TryGetProperty(NKnowledgeDemonVfx.PropertyName._fireNode3, ref variant3))
      this._fireNode3 = ((Variant) ref variant3).As<Node2D>();
    Variant variant4;
    if (info.TryGetProperty(NKnowledgeDemonVfx.PropertyName._fireNode4, ref variant4))
      this._fireNode4 = ((Variant) ref variant4).As<Node2D>();
    Variant variant5;
    if (info.TryGetProperty(NKnowledgeDemonVfx.PropertyName._explosionParticles, ref variant5))
      this._explosionParticles = ((Variant) ref variant5).As<GpuParticles2D>();
    Variant variant6;
    if (info.TryGetProperty(NKnowledgeDemonVfx.PropertyName._damageParticles, ref variant6))
      this._damageParticles = ((Variant) ref variant6).As<GpuParticles2D>();
    Variant variant7;
    if (info.TryGetProperty(NKnowledgeDemonVfx.PropertyName._emberParticles, ref variant7))
      this._emberParticles = ((Variant) ref variant7).As<GpuParticles2D>();
    Variant variant8;
    if (info.TryGetProperty(NKnowledgeDemonVfx.PropertyName._thinEmberParticles, ref variant8))
      this._thinEmberParticles = ((Variant) ref variant8).As<GpuParticles2D>();
    Variant variant9;
    if (!info.TryGetProperty(NKnowledgeDemonVfx.PropertyName._parent, ref variant9))
      return;
    this._parent = ((Variant) ref variant9).As<Node2D>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnAnimationEvent = StringName.op_Implicit(nameof (OnAnimationEvent));
    public static readonly StringName OnAnimationStart = StringName.op_Implicit(nameof (OnAnimationStart));
    public static readonly StringName OnExplode = StringName.op_Implicit(nameof (OnExplode));
    public static readonly StringName OnTakeDamage = StringName.op_Implicit(nameof (OnTakeDamage));
    public static readonly StringName OnBurningStart = StringName.op_Implicit(nameof (OnBurningStart));
    public static readonly StringName OnEmbersStart = StringName.op_Implicit(nameof (OnEmbersStart));
    public static readonly StringName OnThinEmbersStart = StringName.op_Implicit(nameof (OnThinEmbersStart));
    public static readonly StringName OnBurningEnd = StringName.op_Implicit(nameof (OnBurningEnd));
    public static readonly StringName OnEmbersEnd = StringName.op_Implicit(nameof (OnEmbersEnd));
    public static readonly StringName OnThinEmbersEnd = StringName.op_Implicit(nameof (OnThinEmbersEnd));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName _fireNode1 = StringName.op_Implicit(nameof (_fireNode1));
    public static readonly StringName _fireNode2 = StringName.op_Implicit(nameof (_fireNode2));
    public static readonly StringName _fireNode3 = StringName.op_Implicit(nameof (_fireNode3));
    public static readonly StringName _fireNode4 = StringName.op_Implicit(nameof (_fireNode4));
    public static readonly StringName _explosionParticles = StringName.op_Implicit(nameof (_explosionParticles));
    public static readonly StringName _damageParticles = StringName.op_Implicit(nameof (_damageParticles));
    public static readonly StringName _emberParticles = StringName.op_Implicit(nameof (_emberParticles));
    public static readonly StringName _thinEmberParticles = StringName.op_Implicit(nameof (_thinEmberParticles));
    public static readonly StringName _parent = StringName.op_Implicit(nameof (_parent));
  }

  public class SignalName : Node.SignalName
  {
  }
}
