// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NMyteVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[GlobalClass]
[ScriptPath("res://src/Core/Nodes/Vfx/NMyteVfx.cs")]
public class NMyteVfx : Node
{
  private const float _attackHeight = 150f;
  private Node2D _parent;
  private MegaSprite _animController;
  private Node2D _targetBone;
  private Creature? _target;

  public override void _Ready()
  {
    this._parent = this.GetParent<Node2D>();
    this._animController = new MegaSprite(Variant.op_Implicit((GodotObject) this._parent));
    this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
    this._targetBone = ((Node) this._parent).GetNode<Node2D>(NodePath.op_Implicit("TargetBone"));
    this.RunWhenSpineReady(this._animController, (Action<MegaAnimationState>) (animState => animState.SetAnimation("cast")));
  }

  public void SetTarget(Creature? target) => this._target = target;

  private void OnAnimationEvent(
    GodotObject _,
    GodotObject __,
    GodotObject ___,
    GodotObject spineEvent)
  {
    if (!(new MegaEvent(Variant.op_Implicit(spineEvent)).GetData().GetEventName() == "start_cast"))
      return;
    this.StartCast();
  }

  private void StartCast()
  {
    NCreature creatureNode = NCombatRoom.Instance?.GetCreatureNode(this._target);
    if (creatureNode == null)
      return;
    this._targetBone.GlobalPosition = new Vector2(creatureNode.GlobalPosition.X, creatureNode.GlobalPosition.Y - 150f);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NMyteVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMyteVfx.MethodName.OnAnimationEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("__"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("___"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("spineEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMyteVfx.MethodName.StartCast, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMyteVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMyteVfx.MethodName.OnAnimationEvent) && ((NativeVariantPtrArgs) ref args).Count == 4)
    {
      this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[3]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NMyteVfx.MethodName.StartCast) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.StartCast();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMyteVfx.MethodName._Ready) || StringName.op_Equality(ref method, NMyteVfx.MethodName.OnAnimationEvent) || StringName.op_Equality(ref method, NMyteVfx.MethodName.StartCast) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMyteVfx.PropertyName._parent))
    {
      this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMyteVfx.PropertyName._targetBone))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._targetBone = VariantUtils.ConvertTo<Node2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMyteVfx.PropertyName._parent))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMyteVfx.PropertyName._targetBone))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Node2D>(ref this._targetBone);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NMyteVfx.PropertyName._parent, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMyteVfx.PropertyName._targetBone, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NMyteVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
    info.AddProperty(NMyteVfx.PropertyName._targetBone, Variant.From<Node2D>(ref this._targetBone));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NMyteVfx.PropertyName._parent, ref variant1))
      this._parent = ((Variant) ref variant1).As<Node2D>();
    Variant variant2;
    if (!info.TryGetProperty(NMyteVfx.PropertyName._targetBone, ref variant2))
      return;
    this._targetBone = ((Variant) ref variant2).As<Node2D>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnAnimationEvent = StringName.op_Implicit(nameof (OnAnimationEvent));
    public static readonly StringName StartCast = StringName.op_Implicit(nameof (StartCast));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName _parent = StringName.op_Implicit(nameof (_parent));
    public static readonly StringName _targetBone = StringName.op_Implicit(nameof (_targetBone));
  }

  public class SignalName : Node.SignalName
  {
  }
}
