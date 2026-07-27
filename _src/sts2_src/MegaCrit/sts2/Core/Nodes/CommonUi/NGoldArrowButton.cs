// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.CommonUi.NGoldArrowButton
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.CommonUi;

[ScriptPath("res://src/Core/Nodes/CommonUi/NGoldArrowButton.cs")]
public class NGoldArrowButton : NButton
{
  private static readonly StringName _v = new StringName("v");
  protected TextureRect _icon;
  private ShaderMaterial _hsv;
  private Tween? _animTween;
  private float _valueDefault = 0.9f;
  private float _valueHovered = 1.2f;
  private Vector2 _hoverScale = Vector2.op_Multiply(Vector2.One, 1.1f);

  public override void _Ready()
  {
    this.ConnectSignals();
    this._icon = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("TextureRect"));
    this._hsv = (ShaderMaterial) ((CanvasItem) this._icon).Material;
    this._hsv.SetShaderParameter(NGoldArrowButton._v, Variant.op_Implicit(this._valueDefault));
  }

  protected override void OnFocus()
  {
    base.OnFocus();
    this._animTween?.Kill();
    this._hsv.SetShaderParameter(NGoldArrowButton._v, Variant.op_Implicit(this._valueHovered));
    ((Control) this._icon).Scale = this._hoverScale;
  }

  protected override void OnUnfocus()
  {
    base.OnUnfocus();
    this._animTween?.Kill();
    this._animTween = ((Node) this).CreateTween().SetParallel(true);
    this._animTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderParam)), Variant.op_Implicit(this._valueHovered), Variant.op_Implicit(this._valueDefault), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._animTween.TweenProperty((GodotObject) this._icon, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  protected override void OnPress()
  {
    base.OnPress();
    this._animTween?.Kill();
    this._animTween = ((Node) this).CreateTween();
    this._hsv.SetShaderParameter(NGoldArrowButton._v, Variant.op_Implicit(0.7f));
    this._animTween.TweenProperty((GodotObject) this._icon, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  protected override void OnRelease()
  {
    if (!this.IsFocused)
      return;
    this._animTween?.Kill();
    this._hsv.SetShaderParameter(NGoldArrowButton._v, Variant.op_Implicit(this._valueHovered));
    ((Control) this._icon).Scale = this._hoverScale;
  }

  private void UpdateShaderParam(float newV)
  {
    this._hsv.SetShaderParameter(NGoldArrowButton._v, Variant.op_Implicit(newV));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(6)
    {
      new MethodInfo(NGoldArrowButton.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGoldArrowButton.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGoldArrowButton.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGoldArrowButton.MethodName.OnPress, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGoldArrowButton.MethodName.OnRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGoldArrowButton.MethodName.UpdateShaderParam, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("newV"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NGoldArrowButton.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGoldArrowButton.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGoldArrowButton.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGoldArrowButton.MethodName.OnPress) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPress();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGoldArrowButton.MethodName.OnRelease) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnRelease();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NGoldArrowButton.MethodName.UpdateShaderParam) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.UpdateShaderParam(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NGoldArrowButton.MethodName._Ready) || StringName.op_Equality(ref method, NGoldArrowButton.MethodName.OnFocus) || StringName.op_Equality(ref method, NGoldArrowButton.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NGoldArrowButton.MethodName.OnPress) || StringName.op_Equality(ref method, NGoldArrowButton.MethodName.OnRelease) || StringName.op_Equality(ref method, NGoldArrowButton.MethodName.UpdateShaderParam) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NGoldArrowButton.PropertyName._icon))
    {
      this._icon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGoldArrowButton.PropertyName._hsv))
    {
      this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGoldArrowButton.PropertyName._animTween))
    {
      this._animTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGoldArrowButton.PropertyName._valueDefault))
    {
      this._valueDefault = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGoldArrowButton.PropertyName._valueHovered))
    {
      this._valueHovered = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NGoldArrowButton.PropertyName._hoverScale))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._hoverScale = VariantUtils.ConvertTo<Vector2>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NGoldArrowButton.PropertyName._icon))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._icon);
      return true;
    }
    if (StringName.op_Equality(ref name, NGoldArrowButton.PropertyName._hsv))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
      return true;
    }
    if (StringName.op_Equality(ref name, NGoldArrowButton.PropertyName._animTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._animTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NGoldArrowButton.PropertyName._valueDefault))
    {
      value = VariantUtils.CreateFrom<float>(ref this._valueDefault);
      return true;
    }
    if (StringName.op_Equality(ref name, NGoldArrowButton.PropertyName._valueHovered))
    {
      value = VariantUtils.CreateFrom<float>(ref this._valueHovered);
      return true;
    }
    if (!StringName.op_Equality(ref name, NGoldArrowButton.PropertyName._hoverScale))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Vector2>(ref this._hoverScale);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NGoldArrowButton.PropertyName._icon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGoldArrowButton.PropertyName._hsv, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGoldArrowButton.PropertyName._animTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NGoldArrowButton.PropertyName._valueDefault, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NGoldArrowButton.PropertyName._valueHovered, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NGoldArrowButton.PropertyName._hoverScale, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NGoldArrowButton.PropertyName._icon, Variant.From<TextureRect>(ref this._icon));
    info.AddProperty(NGoldArrowButton.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
    info.AddProperty(NGoldArrowButton.PropertyName._animTween, Variant.From<Tween>(ref this._animTween));
    info.AddProperty(NGoldArrowButton.PropertyName._valueDefault, Variant.From<float>(ref this._valueDefault));
    info.AddProperty(NGoldArrowButton.PropertyName._valueHovered, Variant.From<float>(ref this._valueHovered));
    info.AddProperty(NGoldArrowButton.PropertyName._hoverScale, Variant.From<Vector2>(ref this._hoverScale));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NGoldArrowButton.PropertyName._icon, ref variant1))
      this._icon = ((Variant) ref variant1).As<TextureRect>();
    Variant variant2;
    if (info.TryGetProperty(NGoldArrowButton.PropertyName._hsv, ref variant2))
      this._hsv = ((Variant) ref variant2).As<ShaderMaterial>();
    Variant variant3;
    if (info.TryGetProperty(NGoldArrowButton.PropertyName._animTween, ref variant3))
      this._animTween = ((Variant) ref variant3).As<Tween>();
    Variant variant4;
    if (info.TryGetProperty(NGoldArrowButton.PropertyName._valueDefault, ref variant4))
      this._valueDefault = ((Variant) ref variant4).As<float>();
    Variant variant5;
    if (info.TryGetProperty(NGoldArrowButton.PropertyName._valueHovered, ref variant5))
      this._valueHovered = ((Variant) ref variant5).As<float>();
    Variant variant6;
    if (!info.TryGetProperty(NGoldArrowButton.PropertyName._hoverScale, ref variant6))
      return;
    this._hoverScale = ((Variant) ref variant6).As<Vector2>();
  }

  public new class MethodName : NButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public new static readonly StringName OnPress = StringName.op_Implicit(nameof (OnPress));
    public new static readonly StringName OnRelease = StringName.op_Implicit(nameof (OnRelease));
    public static readonly StringName UpdateShaderParam = StringName.op_Implicit(nameof (UpdateShaderParam));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public static readonly StringName _icon = StringName.op_Implicit(nameof (_icon));
    public static readonly StringName _hsv = StringName.op_Implicit(nameof (_hsv));
    public static readonly StringName _animTween = StringName.op_Implicit(nameof (_animTween));
    public static readonly StringName _valueDefault = StringName.op_Implicit(nameof (_valueDefault));
    public static readonly StringName _valueHovered = StringName.op_Implicit(nameof (_valueHovered));
    public static readonly StringName _hoverScale = StringName.op_Implicit(nameof (_hoverScale));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
