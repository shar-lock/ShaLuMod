// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NSnappingJaxfruitVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[GlobalClass]
[ScriptPath("res://src/Core/Nodes/Vfx/NSnappingJaxfruitVfx.cs")]
public class NSnappingJaxfruitVfx : Node
{
  private const float _attackHeight = 130f;
  private Creature? _target;
  private Node2D _projectileBone;
  private Node2D _targetBone;
  private GpuParticles2D _glowParticles;
  private GpuParticles2D _blobParticles;
  private NBasicTrail _trail;
  private Node2D _parent;
  private MegaSprite _animController;

  public override void _Ready()
  {
    this._parent = this.GetParent<Node2D>();
    this._animController = new MegaSprite(Variant.op_Implicit((GodotObject) this._parent));
    this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
    this._animController.ConnectAnimationStarted(Callable.From<GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject>(this.OnAnimationStart)));
    this._projectileBone = ((Node) this._parent).GetNode<Node2D>(NodePath.op_Implicit("ProjectileAttachBone"));
    this._targetBone = ((Node) this._parent).GetNode<Node2D>(NodePath.op_Implicit("TargetBone"));
    this._glowParticles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("ProjectileAttachBone/GlowParticles"));
    this._blobParticles = ((Node) this._parent).GetNode<GpuParticles2D>(NodePath.op_Implicit("ProjectileAttachBone/BlobParticles"));
    this._trail = ((Node) this._parent).GetNode<NBasicTrail>(NodePath.op_Implicit("ProjectileAttachBone/Trail"));
    this.ResetCast();
  }

  public void SetTarget(Creature? target) => this._target = target;

  private void OnAnimationEvent(
    GodotObject _,
    GodotObject __,
    GodotObject ___,
    GodotObject spineEvent)
  {
    switch (new MegaEvent(Variant.op_Implicit(spineEvent)).GetData().GetEventName())
    {
      case "cast_start":
        this.StartCast();
        break;
      case "cast_end":
        this.ResetCast();
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
    this.ResetCast();
  }

  private void StartCast()
  {
    NCreature creatureNode = NCombatRoom.Instance?.GetCreatureNode(this._target);
    if (creatureNode != null)
      this._targetBone.GlobalPosition = new Vector2(creatureNode.GlobalPosition.X, creatureNode.GlobalPosition.Y - 130f);
    ((CanvasItem) this._projectileBone).Visible = true;
    this._trail.ClearPoints();
    this._blobParticles.Restart();
    this._glowParticles.Restart();
  }

  private void ResetCast()
  {
    this._blobParticles.Emitting = false;
    this._glowParticles.Emitting = false;
    ((CanvasItem) this._projectileBone).Visible = false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NSnappingJaxfruitVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSnappingJaxfruitVfx.MethodName.OnAnimationEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("__"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("___"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("spineEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false)
      }, (List<Variant>) null),
      new MethodInfo(NSnappingJaxfruitVfx.MethodName.OnAnimationStart, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("spineSprite"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("animationState"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("trackEntry"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false)
      }, (List<Variant>) null),
      new MethodInfo(NSnappingJaxfruitVfx.MethodName.StartCast, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSnappingJaxfruitVfx.MethodName.ResetCast, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NSnappingJaxfruitVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSnappingJaxfruitVfx.MethodName.OnAnimationEvent) && ((NativeVariantPtrArgs) ref args).Count == 4)
    {
      this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[3]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSnappingJaxfruitVfx.MethodName.OnAnimationStart) && ((NativeVariantPtrArgs) ref args).Count == 3)
    {
      this.OnAnimationStart(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSnappingJaxfruitVfx.MethodName.StartCast) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.StartCast();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NSnappingJaxfruitVfx.MethodName.ResetCast) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.ResetCast();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NSnappingJaxfruitVfx.MethodName._Ready) || StringName.op_Equality(ref method, NSnappingJaxfruitVfx.MethodName.OnAnimationEvent) || StringName.op_Equality(ref method, NSnappingJaxfruitVfx.MethodName.OnAnimationStart) || StringName.op_Equality(ref method, NSnappingJaxfruitVfx.MethodName.StartCast) || StringName.op_Equality(ref method, NSnappingJaxfruitVfx.MethodName.ResetCast) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSnappingJaxfruitVfx.PropertyName._projectileBone))
    {
      this._projectileBone = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSnappingJaxfruitVfx.PropertyName._targetBone))
    {
      this._targetBone = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSnappingJaxfruitVfx.PropertyName._glowParticles))
    {
      this._glowParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSnappingJaxfruitVfx.PropertyName._blobParticles))
    {
      this._blobParticles = VariantUtils.ConvertTo<GpuParticles2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSnappingJaxfruitVfx.PropertyName._trail))
    {
      this._trail = VariantUtils.ConvertTo<NBasicTrail>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSnappingJaxfruitVfx.PropertyName._parent))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSnappingJaxfruitVfx.PropertyName._projectileBone))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._projectileBone);
      return true;
    }
    if (StringName.op_Equality(ref name, NSnappingJaxfruitVfx.PropertyName._targetBone))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._targetBone);
      return true;
    }
    if (StringName.op_Equality(ref name, NSnappingJaxfruitVfx.PropertyName._glowParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._glowParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NSnappingJaxfruitVfx.PropertyName._blobParticles))
    {
      value = VariantUtils.CreateFrom<GpuParticles2D>(ref this._blobParticles);
      return true;
    }
    if (StringName.op_Equality(ref name, NSnappingJaxfruitVfx.PropertyName._trail))
    {
      value = VariantUtils.CreateFrom<NBasicTrail>(ref this._trail);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSnappingJaxfruitVfx.PropertyName._parent))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NSnappingJaxfruitVfx.PropertyName._projectileBone, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSnappingJaxfruitVfx.PropertyName._targetBone, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSnappingJaxfruitVfx.PropertyName._glowParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSnappingJaxfruitVfx.PropertyName._blobParticles, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSnappingJaxfruitVfx.PropertyName._trail, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSnappingJaxfruitVfx.PropertyName._parent, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NSnappingJaxfruitVfx.PropertyName._projectileBone, Variant.From<Node2D>(ref this._projectileBone));
    info.AddProperty(NSnappingJaxfruitVfx.PropertyName._targetBone, Variant.From<Node2D>(ref this._targetBone));
    info.AddProperty(NSnappingJaxfruitVfx.PropertyName._glowParticles, Variant.From<GpuParticles2D>(ref this._glowParticles));
    info.AddProperty(NSnappingJaxfruitVfx.PropertyName._blobParticles, Variant.From<GpuParticles2D>(ref this._blobParticles));
    info.AddProperty(NSnappingJaxfruitVfx.PropertyName._trail, Variant.From<NBasicTrail>(ref this._trail));
    info.AddProperty(NSnappingJaxfruitVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NSnappingJaxfruitVfx.PropertyName._projectileBone, ref variant1))
      this._projectileBone = ((Variant) ref variant1).As<Node2D>();
    Variant variant2;
    if (info.TryGetProperty(NSnappingJaxfruitVfx.PropertyName._targetBone, ref variant2))
      this._targetBone = ((Variant) ref variant2).As<Node2D>();
    Variant variant3;
    if (info.TryGetProperty(NSnappingJaxfruitVfx.PropertyName._glowParticles, ref variant3))
      this._glowParticles = ((Variant) ref variant3).As<GpuParticles2D>();
    Variant variant4;
    if (info.TryGetProperty(NSnappingJaxfruitVfx.PropertyName._blobParticles, ref variant4))
      this._blobParticles = ((Variant) ref variant4).As<GpuParticles2D>();
    Variant variant5;
    if (info.TryGetProperty(NSnappingJaxfruitVfx.PropertyName._trail, ref variant5))
      this._trail = ((Variant) ref variant5).As<NBasicTrail>();
    Variant variant6;
    if (!info.TryGetProperty(NSnappingJaxfruitVfx.PropertyName._parent, ref variant6))
      return;
    this._parent = ((Variant) ref variant6).As<Node2D>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnAnimationEvent = StringName.op_Implicit(nameof (OnAnimationEvent));
    public static readonly StringName OnAnimationStart = StringName.op_Implicit(nameof (OnAnimationStart));
    public static readonly StringName StartCast = StringName.op_Implicit(nameof (StartCast));
    public static readonly StringName ResetCast = StringName.op_Implicit(nameof (ResetCast));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName _projectileBone = StringName.op_Implicit(nameof (_projectileBone));
    public static readonly StringName _targetBone = StringName.op_Implicit(nameof (_targetBone));
    public static readonly StringName _glowParticles = StringName.op_Implicit(nameof (_glowParticles));
    public static readonly StringName _blobParticles = StringName.op_Implicit(nameof (_blobParticles));
    public static readonly StringName _trail = StringName.op_Implicit(nameof (_trail));
    public static readonly StringName _parent = StringName.op_Implicit(nameof (_parent));
  }

  public class SignalName : Node.SignalName
  {
  }
}
