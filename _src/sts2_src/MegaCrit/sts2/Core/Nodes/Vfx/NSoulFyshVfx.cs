// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NSoulFyshVfx
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
[ScriptPath("res://src/Core/Nodes/Vfx/NSoulFyshVfx.cs")]
public class NSoulFyshVfx : Node
{
  private static readonly StringName _amount = new StringName("amount");
  private ShaderMaterial? _soundShaderMat;
  private MegaSlotNode _soundSlotNode;
  private ShaderMaterial? _beckonShaderMat;
  private MegaSlotNode _beckonSlotNode;
  private Node2D _parent;
  private MegaSprite _animController;

  public override void _Ready()
  {
    this._parent = this.GetParent<Node2D>();
    this._animController = new MegaSprite(Variant.op_Implicit((GodotObject) this._parent));
    this._animController.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
    this._soundSlotNode = new MegaSlotNode(Variant.op_Implicit((GodotObject) ((Node) this._parent).GetNode(NodePath.op_Implicit("Soundwave"))));
    this._soundShaderMat = this._soundSlotNode.GetNormalMaterial() as ShaderMaterial;
    this._soundShaderMat?.SetShaderParameter(NSoulFyshVfx._amount, Variant.op_Implicit(0.3f));
    this._beckonSlotNode = new MegaSlotNode(Variant.op_Implicit((GodotObject) ((Node) this._parent).GetNode(NodePath.op_Implicit("Beckonwave"))));
    this._beckonShaderMat = this._beckonSlotNode.GetNormalMaterial() as ShaderMaterial;
    this._beckonShaderMat?.SetShaderParameter(NSoulFyshVfx._amount, Variant.op_Implicit(0.3f));
    this.RunWhenSpineReady(this._animController, (Action<MegaAnimationState>) (animState => animState.SetAnimation("attack_debuff")));
  }

  private void OnAnimationEvent(
    GodotObject _,
    GodotObject __,
    GodotObject ___,
    GodotObject spineEvent)
  {
    switch (new MegaEvent(Variant.op_Implicit(spineEvent)).GetData().GetEventName())
    {
      case "soundwave_start":
        this.StartSoundwave();
        break;
      case "soundwave_end":
        this.EndSoundwave();
        break;
      case "beckon_start":
        this.StartBeckon();
        break;
      case "beckon_end":
        this.EndBeckon();
        break;
    }
  }

  private void StartSoundwave()
  {
    Tween tween = this.CreateTween();
    tween.SetEase((Tween.EaseType) 1L);
    tween.SetTrans((Tween.TransitionType) 4L);
    tween.TweenProperty((GodotObject) this._soundShaderMat, NodePath.op_Implicit("shader_parameter/amount"), Variant.op_Implicit(1f), 0.44999998807907104);
  }

  private void EndSoundwave()
  {
    Tween tween = this.CreateTween();
    tween.SetEase((Tween.EaseType) 0L);
    tween.SetTrans((Tween.TransitionType) 4L);
    tween.TweenProperty((GodotObject) this._soundShaderMat, NodePath.op_Implicit("shader_parameter/amount"), Variant.op_Implicit(0.3f), 0.5);
  }

  private void StartBeckon()
  {
    Tween tween = this.CreateTween();
    tween.SetEase((Tween.EaseType) 1L);
    tween.SetTrans((Tween.TransitionType) 4L);
    tween.TweenProperty((GodotObject) this._beckonShaderMat, NodePath.op_Implicit("shader_parameter/amount"), Variant.op_Implicit(1f), 0.25);
  }

  private void EndBeckon()
  {
    Tween tween = this.CreateTween();
    tween.SetEase((Tween.EaseType) 0L);
    tween.SetTrans((Tween.TransitionType) 4L);
    tween.TweenProperty((GodotObject) this._beckonShaderMat, NodePath.op_Implicit("shader_parameter/amount"), Variant.op_Implicit(0.3f), 0.5);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(6)
    {
      new MethodInfo(NSoulFyshVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSoulFyshVfx.MethodName.OnAnimationEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("__"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("___"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("spineEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false)
      }, (List<Variant>) null),
      new MethodInfo(NSoulFyshVfx.MethodName.StartSoundwave, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSoulFyshVfx.MethodName.EndSoundwave, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSoulFyshVfx.MethodName.StartBeckon, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSoulFyshVfx.MethodName.EndBeckon, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NSoulFyshVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSoulFyshVfx.MethodName.OnAnimationEvent) && ((NativeVariantPtrArgs) ref args).Count == 4)
    {
      this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[3]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSoulFyshVfx.MethodName.StartSoundwave) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.StartSoundwave();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSoulFyshVfx.MethodName.EndSoundwave) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.EndSoundwave();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSoulFyshVfx.MethodName.StartBeckon) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.StartBeckon();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NSoulFyshVfx.MethodName.EndBeckon) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.EndBeckon();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NSoulFyshVfx.MethodName._Ready) || StringName.op_Equality(ref method, NSoulFyshVfx.MethodName.OnAnimationEvent) || StringName.op_Equality(ref method, NSoulFyshVfx.MethodName.StartSoundwave) || StringName.op_Equality(ref method, NSoulFyshVfx.MethodName.EndSoundwave) || StringName.op_Equality(ref method, NSoulFyshVfx.MethodName.StartBeckon) || StringName.op_Equality(ref method, NSoulFyshVfx.MethodName.EndBeckon) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSoulFyshVfx.PropertyName._soundShaderMat))
    {
      this._soundShaderMat = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSoulFyshVfx.PropertyName._beckonShaderMat))
    {
      this._beckonShaderMat = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSoulFyshVfx.PropertyName._parent))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSoulFyshVfx.PropertyName._soundShaderMat))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._soundShaderMat);
      return true;
    }
    if (StringName.op_Equality(ref name, NSoulFyshVfx.PropertyName._beckonShaderMat))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._beckonShaderMat);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSoulFyshVfx.PropertyName._parent))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NSoulFyshVfx.PropertyName._soundShaderMat, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSoulFyshVfx.PropertyName._beckonShaderMat, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSoulFyshVfx.PropertyName._parent, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NSoulFyshVfx.PropertyName._soundShaderMat, Variant.From<ShaderMaterial>(ref this._soundShaderMat));
    info.AddProperty(NSoulFyshVfx.PropertyName._beckonShaderMat, Variant.From<ShaderMaterial>(ref this._beckonShaderMat));
    info.AddProperty(NSoulFyshVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NSoulFyshVfx.PropertyName._soundShaderMat, ref variant1))
      this._soundShaderMat = ((Variant) ref variant1).As<ShaderMaterial>();
    Variant variant2;
    if (info.TryGetProperty(NSoulFyshVfx.PropertyName._beckonShaderMat, ref variant2))
      this._beckonShaderMat = ((Variant) ref variant2).As<ShaderMaterial>();
    Variant variant3;
    if (!info.TryGetProperty(NSoulFyshVfx.PropertyName._parent, ref variant3))
      return;
    this._parent = ((Variant) ref variant3).As<Node2D>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnAnimationEvent = StringName.op_Implicit(nameof (OnAnimationEvent));
    public static readonly StringName StartSoundwave = StringName.op_Implicit(nameof (StartSoundwave));
    public static readonly StringName EndSoundwave = StringName.op_Implicit(nameof (EndSoundwave));
    public static readonly StringName StartBeckon = StringName.op_Implicit(nameof (StartBeckon));
    public static readonly StringName EndBeckon = StringName.op_Implicit(nameof (EndBeckon));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName _soundShaderMat = StringName.op_Implicit(nameof (_soundShaderMat));
    public static readonly StringName _beckonShaderMat = StringName.op_Implicit(nameof (_beckonShaderMat));
    public static readonly StringName _parent = StringName.op_Implicit(nameof (_parent));
  }

  public class SignalName : Node.SignalName
  {
  }
}
