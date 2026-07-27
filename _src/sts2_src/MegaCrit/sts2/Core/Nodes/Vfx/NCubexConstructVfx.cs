// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NCubexConstructVfx
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
[ScriptPath("res://src/Core/Nodes/Vfx/NCubexConstructVfx.cs")]
public class NCubexConstructVfx : Node
{
  private NLaserVfx _laser;
  private Node2D _rings;
  private Node2D _parent;
  private MegaSprite _animController;

  public override void _Ready()
  {
    this._parent = this.GetParent<Node2D>();
    this._animController = new MegaSprite(Variant.op_Implicit((GodotObject) this._parent));
    this._laser = ((Node) this._parent).GetNode<NLaserVfx>(NodePath.op_Implicit("LaserBone/Laser"));
    this._rings = ((Node) this._parent).GetNode<Node2D>(NodePath.op_Implicit("LaserBone/Rings"));
    ((CanvasItem) this._rings).Visible = false;
    this._laser.Scale = Vector2.op_Division(Vector2.One, this._parent.Scale);
    ((CanvasItem) this._laser).Visible = false;
    this._laser.ResetLaser();
    this.RunWhenSpineReady(this._animController, (Action<MegaAnimationState>) (animState =>
    {
      MegaSkeleton skeleton = this._animController.GetSkeleton();
      this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
      MegaSkin skin = this._animController.NewSkin("newSkin");
      skin.AddSkin(skeleton.GetData().FindSkin("moss3"));
      skeleton.SetSkin(skin);
      animState.SetAnimation("idle_loop");
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
      case "laser_start":
        this.StartLaser();
        break;
      case "laser_end":
        this.EndLaser();
        break;
    }
  }

  private void StartLaser()
  {
    ((CanvasItem) this._laser).Visible = true;
    this._laser.ExtendLaser(new Vector2(this._laser.Position.X - 3000f, this._parent.Position.Y));
    Vector2 scale = this._rings.Scale;
    this._rings.Scale = Vector2.Zero;
    ((CanvasItem) this._rings).Visible = true;
    this.CreateTween().TweenProperty((GodotObject) this._rings, NodePath.op_Implicit("scale"), Variant.op_Implicit(scale), 0.40000000596046448).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  private void EndLaser()
  {
    this._laser.RetractLaser();
    ((CanvasItem) this._rings).Visible = false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NCubexConstructVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCubexConstructVfx.MethodName.OnAnimationEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("__"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("___"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("spineEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCubexConstructVfx.MethodName.StartLaser, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCubexConstructVfx.MethodName.EndLaser, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCubexConstructVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCubexConstructVfx.MethodName.OnAnimationEvent) && ((NativeVariantPtrArgs) ref args).Count == 4)
    {
      this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[3]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCubexConstructVfx.MethodName.StartLaser) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.StartLaser();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCubexConstructVfx.MethodName.EndLaser) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.EndLaser();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCubexConstructVfx.MethodName._Ready) || StringName.op_Equality(ref method, NCubexConstructVfx.MethodName.OnAnimationEvent) || StringName.op_Equality(ref method, NCubexConstructVfx.MethodName.StartLaser) || StringName.op_Equality(ref method, NCubexConstructVfx.MethodName.EndLaser) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCubexConstructVfx.PropertyName._laser))
    {
      this._laser = VariantUtils.ConvertTo<NLaserVfx>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCubexConstructVfx.PropertyName._rings))
    {
      this._rings = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCubexConstructVfx.PropertyName._parent))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCubexConstructVfx.PropertyName._laser))
    {
      value = VariantUtils.CreateFrom<NLaserVfx>(ref this._laser);
      return true;
    }
    if (StringName.op_Equality(ref name, NCubexConstructVfx.PropertyName._rings))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._rings);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCubexConstructVfx.PropertyName._parent))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCubexConstructVfx.PropertyName._laser, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCubexConstructVfx.PropertyName._rings, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCubexConstructVfx.PropertyName._parent, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NCubexConstructVfx.PropertyName._laser, Variant.From<NLaserVfx>(ref this._laser));
    info.AddProperty(NCubexConstructVfx.PropertyName._rings, Variant.From<Node2D>(ref this._rings));
    info.AddProperty(NCubexConstructVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCubexConstructVfx.PropertyName._laser, ref variant1))
      this._laser = ((Variant) ref variant1).As<NLaserVfx>();
    Variant variant2;
    if (info.TryGetProperty(NCubexConstructVfx.PropertyName._rings, ref variant2))
      this._rings = ((Variant) ref variant2).As<Node2D>();
    Variant variant3;
    if (!info.TryGetProperty(NCubexConstructVfx.PropertyName._parent, ref variant3))
      return;
    this._parent = ((Variant) ref variant3).As<Node2D>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnAnimationEvent = StringName.op_Implicit(nameof (OnAnimationEvent));
    public static readonly StringName StartLaser = StringName.op_Implicit(nameof (StartLaser));
    public static readonly StringName EndLaser = StringName.op_Implicit(nameof (EndLaser));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName _laser = StringName.op_Implicit(nameof (_laser));
    public static readonly StringName _rings = StringName.op_Implicit(nameof (_rings));
    public static readonly StringName _parent = StringName.op_Implicit(nameof (_parent));
  }

  public class SignalName : Node.SignalName
  {
  }
}
