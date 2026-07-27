// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NArchitectVfx
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
[ScriptPath("res://src/Core/Nodes/Vfx/NArchitectVfx.cs")]
public class NArchitectVfx : Node
{
  private Node2D _parent;
  private MegaSprite _animController;
  private NBasicTrail _innerTrail;
  private NBasicTrail _outerTrail;

  public override void _Ready()
  {
    this._parent = this.GetParent<Node2D>();
    this._animController = new MegaSprite(Variant.op_Implicit((GodotObject) this._parent));
    this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
    this._innerTrail = ((Node) this._parent).GetNode<NBasicTrail>(NodePath.op_Implicit("TrailSlot/TrailInner"));
    this._outerTrail = ((Node) this._parent).GetNode<NBasicTrail>(NodePath.op_Implicit("TrailSlot/TrailOuter"));
    this.RunWhenSpineReady(this._animController, (Action<MegaAnimationState>) (animState =>
    {
      animState.SetAnimation("idle_loop");
      animState.SetAnimation("_tracks/head_normal", trackId: 1);
    }));
  }

  private void OnAnimationEvent(
    GodotObject _,
    GodotObject __,
    GodotObject ___,
    GodotObject spineEvent)
  {
    switch (new MegaEvent(Variant.op_Implicit(spineEvent)).GetData().GetEventName())
    {
      case "trail_start":
        this.StartTrail();
        break;
      case "trail_end":
        this.EndTrail();
        break;
    }
  }

  private void StartTrail()
  {
    ((CanvasItem) this._innerTrail).Visible = true;
    ((CanvasItem) this._outerTrail).Visible = true;
    this._innerTrail.ClearPoints();
    this._outerTrail.ClearPoints();
  }

  private void EndTrail()
  {
    ((CanvasItem) this._innerTrail).Visible = false;
    ((CanvasItem) this._outerTrail).Visible = false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NArchitectVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NArchitectVfx.MethodName.OnAnimationEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("__"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("___"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("spineEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false)
      }, (List<Variant>) null),
      new MethodInfo(NArchitectVfx.MethodName.StartTrail, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NArchitectVfx.MethodName.EndTrail, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NArchitectVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NArchitectVfx.MethodName.OnAnimationEvent) && ((NativeVariantPtrArgs) ref args).Count == 4)
    {
      this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[3]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NArchitectVfx.MethodName.StartTrail) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.StartTrail();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NArchitectVfx.MethodName.EndTrail) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.EndTrail();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NArchitectVfx.MethodName._Ready) || StringName.op_Equality(ref method, NArchitectVfx.MethodName.OnAnimationEvent) || StringName.op_Equality(ref method, NArchitectVfx.MethodName.StartTrail) || StringName.op_Equality(ref method, NArchitectVfx.MethodName.EndTrail) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NArchitectVfx.PropertyName._parent))
    {
      this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NArchitectVfx.PropertyName._innerTrail))
    {
      this._innerTrail = VariantUtils.ConvertTo<NBasicTrail>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NArchitectVfx.PropertyName._outerTrail))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._outerTrail = VariantUtils.ConvertTo<NBasicTrail>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NArchitectVfx.PropertyName._parent))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
      return true;
    }
    if (StringName.op_Equality(ref name, NArchitectVfx.PropertyName._innerTrail))
    {
      value = VariantUtils.CreateFrom<NBasicTrail>(ref this._innerTrail);
      return true;
    }
    if (!StringName.op_Equality(ref name, NArchitectVfx.PropertyName._outerTrail))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<NBasicTrail>(ref this._outerTrail);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NArchitectVfx.PropertyName._parent, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NArchitectVfx.PropertyName._innerTrail, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NArchitectVfx.PropertyName._outerTrail, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NArchitectVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
    info.AddProperty(NArchitectVfx.PropertyName._innerTrail, Variant.From<NBasicTrail>(ref this._innerTrail));
    info.AddProperty(NArchitectVfx.PropertyName._outerTrail, Variant.From<NBasicTrail>(ref this._outerTrail));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NArchitectVfx.PropertyName._parent, ref variant1))
      this._parent = ((Variant) ref variant1).As<Node2D>();
    Variant variant2;
    if (info.TryGetProperty(NArchitectVfx.PropertyName._innerTrail, ref variant2))
      this._innerTrail = ((Variant) ref variant2).As<NBasicTrail>();
    Variant variant3;
    if (!info.TryGetProperty(NArchitectVfx.PropertyName._outerTrail, ref variant3))
      return;
    this._outerTrail = ((Variant) ref variant3).As<NBasicTrail>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnAnimationEvent = StringName.op_Implicit(nameof (OnAnimationEvent));
    public static readonly StringName StartTrail = StringName.op_Implicit(nameof (StartTrail));
    public static readonly StringName EndTrail = StringName.op_Implicit(nameof (EndTrail));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName _parent = StringName.op_Implicit(nameof (_parent));
    public static readonly StringName _innerTrail = StringName.op_Implicit(nameof (_innerTrail));
    public static readonly StringName _outerTrail = StringName.op_Implicit(nameof (_outerTrail));
  }

  public class SignalName : Node.SignalName
  {
  }
}
