// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Ftue.NFtueConfirmButton
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Ftue;

[ScriptPath("res://src/Core/Nodes/Ftue/NFtueConfirmButton.cs")]
public class NFtueConfirmButton : NButton
{
  private static readonly StringName _v = new StringName("v");
  private TextureRect _outline;
  private Tween? _outlineTween;
  private Tween? _scaleTween;
  private ShaderMaterial _hsv;
  private MegaLabel _label;

  protected override string[] Hotkeys
  {
    get
    {
      return new string[1]
      {
        StringName.op_Implicit(MegaInput.select)
      };
    }
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._outline = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("Outline"));
    this._hsv = (ShaderMaterial) ((CanvasItem) this).Material;
    this._label = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%Label"));
    this._label.SetTextAutoSize(new LocString("ftues", "CONFIRM_BUTTON").GetRawText());
    this._outlineTween = ((Node) this).CreateTween().SetLoops(0);
    this._outlineTween.TweenProperty((GodotObject) this._outline, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.6);
    this._outlineTween.TweenProperty((GodotObject) this._outline, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.25f), 0.6);
  }

  protected override void OnFocus()
  {
    base.OnFocus();
    this._outlineTween?.Kill();
    ((CanvasItem) this._outline).Modulate = StsColors.gold;
    this._scaleTween?.Kill();
    this._scaleTween = ((Node) this).CreateTween();
    this._scaleTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 1.05f)), 0.05).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._scaleTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NFtueConfirmButton._v), Variant.op_Implicit(1.4f), 0.05).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  protected override void OnUnfocus()
  {
    base.OnUnfocus();
    this._scaleTween?.Kill();
    this._scaleTween = ((Node) this).CreateTween();
    this._scaleTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._scaleTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NFtueConfirmButton._v), Variant.op_Implicit(1f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._outlineTween?.Kill();
    this._outlineTween = ((Node) this).CreateTween().SetLoops(0);
    this._outlineTween.TweenProperty((GodotObject) this._outline, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.25f), 0.6);
    this._outlineTween.TweenProperty((GodotObject) this._outline, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.6).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
  }

  private void UpdateShaderV(float value)
  {
    this._hsv.SetShaderParameter(NFtueConfirmButton._v, Variant.op_Implicit(value));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NFtueConfirmButton.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NFtueConfirmButton.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NFtueConfirmButton.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NFtueConfirmButton.MethodName.UpdateShaderV, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("value"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NFtueConfirmButton.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NFtueConfirmButton.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NFtueConfirmButton.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NFtueConfirmButton.MethodName.UpdateShaderV) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.UpdateShaderV(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NFtueConfirmButton.MethodName._Ready) || StringName.op_Equality(ref method, NFtueConfirmButton.MethodName.OnFocus) || StringName.op_Equality(ref method, NFtueConfirmButton.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NFtueConfirmButton.MethodName.UpdateShaderV) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NFtueConfirmButton.PropertyName._outline))
    {
      this._outline = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NFtueConfirmButton.PropertyName._outlineTween))
    {
      this._outlineTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NFtueConfirmButton.PropertyName._scaleTween))
    {
      this._scaleTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NFtueConfirmButton.PropertyName._hsv))
    {
      this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NFtueConfirmButton.PropertyName._label))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NFtueConfirmButton.PropertyName.Hotkeys))
    {
      ref godot_variant local = ref value;
      string[] hotkeys = this.Hotkeys;
      godot_variant from = VariantUtils.CreateFrom<string[]>(ref hotkeys);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NFtueConfirmButton.PropertyName._outline))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._outline);
      return true;
    }
    if (StringName.op_Equality(ref name, NFtueConfirmButton.PropertyName._outlineTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._outlineTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NFtueConfirmButton.PropertyName._scaleTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._scaleTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NFtueConfirmButton.PropertyName._hsv))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
      return true;
    }
    if (!StringName.op_Equality(ref name, NFtueConfirmButton.PropertyName._label))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 34L, NFtueConfirmButton.PropertyName.Hotkeys, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NFtueConfirmButton.PropertyName._outline, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NFtueConfirmButton.PropertyName._outlineTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NFtueConfirmButton.PropertyName._scaleTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NFtueConfirmButton.PropertyName._hsv, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NFtueConfirmButton.PropertyName._label, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NFtueConfirmButton.PropertyName._outline, Variant.From<TextureRect>(ref this._outline));
    info.AddProperty(NFtueConfirmButton.PropertyName._outlineTween, Variant.From<Tween>(ref this._outlineTween));
    info.AddProperty(NFtueConfirmButton.PropertyName._scaleTween, Variant.From<Tween>(ref this._scaleTween));
    info.AddProperty(NFtueConfirmButton.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
    info.AddProperty(NFtueConfirmButton.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NFtueConfirmButton.PropertyName._outline, ref variant1))
      this._outline = ((Variant) ref variant1).As<TextureRect>();
    Variant variant2;
    if (info.TryGetProperty(NFtueConfirmButton.PropertyName._outlineTween, ref variant2))
      this._outlineTween = ((Variant) ref variant2).As<Tween>();
    Variant variant3;
    if (info.TryGetProperty(NFtueConfirmButton.PropertyName._scaleTween, ref variant3))
      this._scaleTween = ((Variant) ref variant3).As<Tween>();
    Variant variant4;
    if (info.TryGetProperty(NFtueConfirmButton.PropertyName._hsv, ref variant4))
      this._hsv = ((Variant) ref variant4).As<ShaderMaterial>();
    Variant variant5;
    if (!info.TryGetProperty(NFtueConfirmButton.PropertyName._label, ref variant5))
      return;
    this._label = ((Variant) ref variant5).As<MegaLabel>();
  }

  public new class MethodName : NButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public static readonly StringName UpdateShaderV = StringName.op_Implicit(nameof (UpdateShaderV));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public new static readonly StringName Hotkeys = StringName.op_Implicit(nameof (Hotkeys));
    public static readonly StringName _outline = StringName.op_Implicit(nameof (_outline));
    public static readonly StringName _outlineTween = StringName.op_Implicit(nameof (_outlineTween));
    public static readonly StringName _scaleTween = StringName.op_Implicit(nameof (_scaleTween));
    public static readonly StringName _hsv = StringName.op_Implicit(nameof (_hsv));
    public static readonly StringName _label = StringName.op_Implicit(nameof (_label));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
