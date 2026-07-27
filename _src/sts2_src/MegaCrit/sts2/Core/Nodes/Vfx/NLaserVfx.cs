// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NLaserVfx
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

[ScriptPath("res://src/Core/Nodes/Vfx/NLaserVfx.cs")]
public class NLaserVfx : Node2D
{
  private static readonly StringName _color = new StringName("Color");
  private Node2D _animNode;
  private MegaSprite _animController;
  private Node2D _targetingBone;

  public override void _Ready()
  {
    this._animNode = ((Node) this).GetNode<Node2D>(NodePath.op_Implicit("SpineSprite"));
    this._animController = new MegaSprite(Variant.op_Implicit((GodotObject) this._animNode));
    this._targetingBone = ((Node) this).GetNode<Node2D>(NodePath.op_Implicit("SpineSprite/TargetingBone"));
    ((Node) this).RunWhenSpineReady(this._animController, (Action<MegaAnimationState>) (animState => animState.SetAnimation("animation")));
    ((CanvasItem) this._animNode).Visible = false;
  }

  public void ExtendLaser(Vector2 targetPos)
  {
    ((CanvasItem) this._animNode).Visible = true;
    this._animController.GetAnimationState().SetAnimation("animation");
    this._targetingBone.GlobalPosition = this.GlobalPosition;
    Tween tween = ((Node) this).CreateTween();
    tween.TweenProperty((GodotObject) this._targetingBone, NodePath.op_Implicit("position"), Variant.op_Implicit(targetPos), 0.15000000596046448).SetTrans((Tween.TransitionType) 5L).SetEase((Tween.EaseType) 1L);
    tween.Chain().TweenProperty((GodotObject) this._animNode, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.Red), 0.20000000298023224);
  }

  public void RetractLaser()
  {
    Tween tween = ((Node) this).CreateTween();
    tween.TweenProperty((GodotObject) this._targetingBone, NodePath.op_Implicit("position"), Variant.op_Implicit(this.Position), 0.15000000596046448).SetTrans((Tween.TransitionType) 5L).SetEase((Tween.EaseType) 0L);
    tween.Chain().TweenProperty((GodotObject) this._animNode, NodePath.op_Implicit("visible"), Variant.op_Implicit(false), 0.0);
  }

  public void ResetLaser() => this._targetingBone.Position = this.Position;

  private void SetLaserColor(Color color)
  {
    ((ShaderMaterial) this._animController.GetAdditiveMaterial()).SetShaderParameter(NLaserVfx._color, Variant.op_Implicit(color));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NLaserVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NLaserVfx.MethodName.ExtendLaser, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 5L, StringName.op_Implicit("targetPos"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NLaserVfx.MethodName.RetractLaser, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NLaserVfx.MethodName.ResetLaser, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NLaserVfx.MethodName.SetLaserColor, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 20L, StringName.op_Implicit("color"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NLaserVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NLaserVfx.MethodName.ExtendLaser) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ExtendLaser(VariantUtils.ConvertTo<Vector2>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NLaserVfx.MethodName.RetractLaser) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RetractLaser();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NLaserVfx.MethodName.ResetLaser) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ResetLaser();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NLaserVfx.MethodName.SetLaserColor) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.SetLaserColor(VariantUtils.ConvertTo<Color>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NLaserVfx.MethodName._Ready) || StringName.op_Equality(ref method, NLaserVfx.MethodName.ExtendLaser) || StringName.op_Equality(ref method, NLaserVfx.MethodName.RetractLaser) || StringName.op_Equality(ref method, NLaserVfx.MethodName.ResetLaser) || StringName.op_Equality(ref method, NLaserVfx.MethodName.SetLaserColor) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NLaserVfx.PropertyName._animNode))
    {
      this._animNode = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NLaserVfx.PropertyName._targetingBone))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._targetingBone = VariantUtils.ConvertTo<Node2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NLaserVfx.PropertyName._animNode))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._animNode);
      return true;
    }
    if (!StringName.op_Equality(ref name, NLaserVfx.PropertyName._targetingBone))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Node2D>(ref this._targetingBone);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NLaserVfx.PropertyName._animNode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NLaserVfx.PropertyName._targetingBone, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NLaserVfx.PropertyName._animNode, Variant.From<Node2D>(ref this._animNode));
    info.AddProperty(NLaserVfx.PropertyName._targetingBone, Variant.From<Node2D>(ref this._targetingBone));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NLaserVfx.PropertyName._animNode, ref variant1))
      this._animNode = ((Variant) ref variant1).As<Node2D>();
    Variant variant2;
    if (!info.TryGetProperty(NLaserVfx.PropertyName._targetingBone, ref variant2))
      return;
    this._targetingBone = ((Variant) ref variant2).As<Node2D>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName ExtendLaser = StringName.op_Implicit(nameof (ExtendLaser));
    public static readonly StringName RetractLaser = StringName.op_Implicit(nameof (RetractLaser));
    public static readonly StringName ResetLaser = StringName.op_Implicit(nameof (ResetLaser));
    public static readonly StringName SetLaserColor = StringName.op_Implicit(nameof (SetLaserColor));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName _animNode = StringName.op_Implicit(nameof (_animNode));
    public static readonly StringName _targetingBone = StringName.op_Implicit(nameof (_targetingBone));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
