// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Cards.NCardHighlight
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Cards;

[ScriptPath("res://src/Core/Nodes/Cards/NCardHighlight.cs")]
public class NCardHighlight : TextureRect
{
  private static readonly StringName _shaderParameterWidth = new StringName("width");
  public static readonly Color playableColor = new Color(0.0f, 0.957f, 0.988f, 0.98f);
  public static readonly Color gold = new Color(1f, 0.784f, 0.0f, 0.98f);
  public static readonly Color red = new Color(0.83f, 0.0f, 0.33f, 0.98f);
  private Tween? _curTween;
  private ShaderMaterial _shaderMaterial;

  public override void _Ready()
  {
    this._shaderMaterial = (ShaderMaterial) ((CanvasItem) this).Material;
  }

  public void AnimShow()
  {
    this._curTween?.Kill();
    this._curTween = ((Node) this).CreateTween();
    this._curTween.TweenMethod(Callable.From<float>(new Action<float>(this.SetShaderParameter)), Variant.op_Implicit(this.GetShaderParameter()), Variant.op_Implicit(0.075f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
  }

  public void AnimHide()
  {
    this._curTween?.Kill();
    this._curTween = ((Node) this).CreateTween();
    this._curTween.TweenMethod(Callable.From<float>(new Action<float>(this.SetShaderParameter)), Variant.op_Implicit(this.GetShaderParameter()), Variant.op_Implicit(0.0), 0.5);
  }

  public void AnimHideInstantly()
  {
    this._curTween?.Kill();
    this.SetShaderParameter(0.0f);
  }

  public void AnimFlash()
  {
    this._curTween?.Kill();
    this._curTween = ((Node) this).CreateTween();
    this._curTween.TweenMethod(Callable.From<float>(new Action<float>(this.SetShaderParameter)), Variant.op_Implicit(this.GetShaderParameter()), Variant.op_Implicit(0.15f), 0.1);
    this._curTween.TweenMethod(Callable.From<float>(new Action<float>(this.SetShaderParameter)), Variant.op_Implicit(0.15f), Variant.op_Implicit(0.075f), 0.35).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
  }

  private float GetShaderParameter()
  {
    Variant shaderParameter = this._shaderMaterial.GetShaderParameter(NCardHighlight._shaderParameterWidth);
    return ((Variant) ref shaderParameter).AsSingle();
  }

  private void SetShaderParameter(float val)
  {
    this._shaderMaterial.SetShaderParameter(NCardHighlight._shaderParameterWidth, Variant.op_Implicit(val));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(7)
    {
      new MethodInfo(NCardHighlight.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardHighlight.MethodName.AnimShow, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardHighlight.MethodName.AnimHide, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardHighlight.MethodName.AnimHideInstantly, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardHighlight.MethodName.AnimFlash, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardHighlight.MethodName.GetShaderParameter, new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardHighlight.MethodName.SetShaderParameter, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("val"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCardHighlight.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardHighlight.MethodName.AnimShow) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimShow();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardHighlight.MethodName.AnimHide) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimHide();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardHighlight.MethodName.AnimHideInstantly) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimHideInstantly();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardHighlight.MethodName.AnimFlash) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimFlash();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardHighlight.MethodName.GetShaderParameter) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      float shaderParameter = this.GetShaderParameter();
      ret = VariantUtils.CreateFrom<float>(ref shaderParameter);
      return true;
    }
    if (!StringName.op_Equality(ref method, NCardHighlight.MethodName.SetShaderParameter) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.SetShaderParameter(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCardHighlight.MethodName._Ready) || StringName.op_Equality(ref method, NCardHighlight.MethodName.AnimShow) || StringName.op_Equality(ref method, NCardHighlight.MethodName.AnimHide) || StringName.op_Equality(ref method, NCardHighlight.MethodName.AnimHideInstantly) || StringName.op_Equality(ref method, NCardHighlight.MethodName.AnimFlash) || StringName.op_Equality(ref method, NCardHighlight.MethodName.GetShaderParameter) || StringName.op_Equality(ref method, NCardHighlight.MethodName.SetShaderParameter) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardHighlight.PropertyName._curTween))
    {
      this._curTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardHighlight.PropertyName._shaderMaterial))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._shaderMaterial = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardHighlight.PropertyName._curTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._curTween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardHighlight.PropertyName._shaderMaterial))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._shaderMaterial);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCardHighlight.PropertyName._curTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardHighlight.PropertyName._shaderMaterial, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NCardHighlight.PropertyName._curTween, Variant.From<Tween>(ref this._curTween));
    info.AddProperty(NCardHighlight.PropertyName._shaderMaterial, Variant.From<ShaderMaterial>(ref this._shaderMaterial));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCardHighlight.PropertyName._curTween, ref variant1))
      this._curTween = ((Variant) ref variant1).As<Tween>();
    Variant variant2;
    if (!info.TryGetProperty(NCardHighlight.PropertyName._shaderMaterial, ref variant2))
      return;
    this._shaderMaterial = ((Variant) ref variant2).As<ShaderMaterial>();
  }

  public class MethodName : TextureRect.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName AnimShow = StringName.op_Implicit(nameof (AnimShow));
    public static readonly StringName AnimHide = StringName.op_Implicit(nameof (AnimHide));
    public static readonly StringName AnimHideInstantly = StringName.op_Implicit(nameof (AnimHideInstantly));
    public static readonly StringName AnimFlash = StringName.op_Implicit(nameof (AnimFlash));
    public static readonly StringName GetShaderParameter = StringName.op_Implicit(nameof (GetShaderParameter));
    public static readonly StringName SetShaderParameter = StringName.op_Implicit(nameof (SetShaderParameter));
  }

  public class PropertyName : TextureRect.PropertyName
  {
    public static readonly StringName _curTween = StringName.op_Implicit(nameof (_curTween));
    public static readonly StringName _shaderMaterial = StringName.op_Implicit(nameof (_shaderMaterial));
  }

  public class SignalName : TextureRect.SignalName
  {
  }
}
