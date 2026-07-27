// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NIroncladVfx
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

[ScriptPath("res://src/Core/Nodes/Vfx/NIroncladVfx.cs")]
public class NIroncladVfx : Node
{
  private static readonly StringName _step = new StringName("step");
  private Vector2 _slashStepBase;
  private ShaderMaterial? _slashShaderMat;
  private Tween? _tween;
  private Node2D _parent;
  private MegaSprite _megaSprite;

  public override void _Ready()
  {
    this._parent = this.GetParent<Node2D>();
    this._slashShaderMat = new MegaSlotNode(Variant.op_Implicit((GodotObject) ((Node) this._parent).GetNode(NodePath.op_Implicit("SlashVfxSlot")))).GetNormalMaterial() as ShaderMaterial;
    this._slashStepBase = Variant.op_Explicit(this._slashShaderMat.GetShaderParameter(NIroncladVfx._step));
    this._megaSprite = new MegaSprite(Variant.op_Implicit((GodotObject) this._parent));
    this._megaSprite.ConnectAnimationEvent(Callable.From<GodotObject, GodotObject, GodotObject, GodotObject>(new Action<GodotObject, GodotObject, GodotObject, GodotObject>(this.OnAnimationEvent)));
  }

  private void OnAnimationEvent(
    GodotObject _,
    GodotObject __,
    GodotObject ___,
    GodotObject spineEvent)
  {
    switch (new MegaEvent(Variant.op_Implicit(spineEvent)).GetData().GetEventName())
    {
      case "heavy_slash_start":
        this.OnHeavySlash();
        break;
      case "attack_slash_start":
        this.OnAttackSlash();
        break;
    }
  }

  private void OnHeavySlash()
  {
    this._slashShaderMat?.SetShaderParameter(NIroncladVfx._step, Variant.op_Implicit(this._slashStepBase));
    this._tween?.Kill();
    this._tween = this.CreateTween().SetEase((Tween.EaseType) 0L).SetTrans((Tween.TransitionType) 7L);
    Vector2 vector2;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2).\u002Ector(1f, 1.02f);
    this._tween.TweenProperty((GodotObject) this._slashShaderMat, NodePath.op_Implicit("shader_parameter/step"), Variant.op_Implicit(vector2), 0.34999999403953552);
  }

  private void OnAttackSlash()
  {
    this._slashShaderMat?.SetShaderParameter(NIroncladVfx._step, Variant.op_Implicit(this._slashStepBase));
    this._tween?.Kill();
    this._tween = this.CreateTween().SetEase((Tween.EaseType) 0L).SetTrans((Tween.TransitionType) 4L);
    Vector2 vector2;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2).\u002Ector(1f, 1.02f);
    this._tween.TweenInterval(0.15000000596046448);
    this._tween.TweenProperty((GodotObject) this._slashShaderMat, NodePath.op_Implicit("shader_parameter/step"), Variant.op_Implicit(vector2), 0.20000000298023224);
  }

  public override void _ExitTree() => this._tween?.Kill();

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NIroncladVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NIroncladVfx.MethodName.OnAnimationEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("__"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("___"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false),
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("spineEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Object"), false)
      }, (List<Variant>) null),
      new MethodInfo(NIroncladVfx.MethodName.OnHeavySlash, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NIroncladVfx.MethodName.OnAttackSlash, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NIroncladVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NIroncladVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NIroncladVfx.MethodName.OnAnimationEvent) && ((NativeVariantPtrArgs) ref args).Count == 4)
    {
      this.OnAnimationEvent(VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[1]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[2]), VariantUtils.ConvertTo<GodotObject>(ref ((NativeVariantPtrArgs) ref args)[3]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NIroncladVfx.MethodName.OnHeavySlash) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnHeavySlash();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NIroncladVfx.MethodName.OnAttackSlash) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnAttackSlash();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NIroncladVfx.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    base._ExitTree();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NIroncladVfx.MethodName._Ready) || StringName.op_Equality(ref method, NIroncladVfx.MethodName.OnAnimationEvent) || StringName.op_Equality(ref method, NIroncladVfx.MethodName.OnHeavySlash) || StringName.op_Equality(ref method, NIroncladVfx.MethodName.OnAttackSlash) || StringName.op_Equality(ref method, NIroncladVfx.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NIroncladVfx.PropertyName._slashStepBase))
    {
      this._slashStepBase = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NIroncladVfx.PropertyName._slashShaderMat))
    {
      this._slashShaderMat = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NIroncladVfx.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NIroncladVfx.PropertyName._parent))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._parent = VariantUtils.ConvertTo<Node2D>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NIroncladVfx.PropertyName._slashStepBase))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._slashStepBase);
      return true;
    }
    if (StringName.op_Equality(ref name, NIroncladVfx.PropertyName._slashShaderMat))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._slashShaderMat);
      return true;
    }
    if (StringName.op_Equality(ref name, NIroncladVfx.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NIroncladVfx.PropertyName._parent))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Node2D>(ref this._parent);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 5L, NIroncladVfx.PropertyName._slashStepBase, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NIroncladVfx.PropertyName._slashShaderMat, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NIroncladVfx.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NIroncladVfx.PropertyName._parent, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NIroncladVfx.PropertyName._slashStepBase, Variant.From<Vector2>(ref this._slashStepBase));
    info.AddProperty(NIroncladVfx.PropertyName._slashShaderMat, Variant.From<ShaderMaterial>(ref this._slashShaderMat));
    info.AddProperty(NIroncladVfx.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NIroncladVfx.PropertyName._parent, Variant.From<Node2D>(ref this._parent));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NIroncladVfx.PropertyName._slashStepBase, ref variant1))
      this._slashStepBase = ((Variant) ref variant1).As<Vector2>();
    Variant variant2;
    if (info.TryGetProperty(NIroncladVfx.PropertyName._slashShaderMat, ref variant2))
      this._slashShaderMat = ((Variant) ref variant2).As<ShaderMaterial>();
    Variant variant3;
    if (info.TryGetProperty(NIroncladVfx.PropertyName._tween, ref variant3))
      this._tween = ((Variant) ref variant3).As<Tween>();
    Variant variant4;
    if (!info.TryGetProperty(NIroncladVfx.PropertyName._parent, ref variant4))
      return;
    this._parent = ((Variant) ref variant4).As<Node2D>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnAnimationEvent = StringName.op_Implicit(nameof (OnAnimationEvent));
    public static readonly StringName OnHeavySlash = StringName.op_Implicit(nameof (OnHeavySlash));
    public static readonly StringName OnAttackSlash = StringName.op_Implicit(nameof (OnAttackSlash));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName _slashStepBase = StringName.op_Implicit(nameof (_slashStepBase));
    public static readonly StringName _slashShaderMat = StringName.op_Implicit(nameof (_slashShaderMat));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _parent = StringName.op_Implicit(nameof (_parent));
  }

  public class SignalName : Node.SignalName
  {
  }
}
