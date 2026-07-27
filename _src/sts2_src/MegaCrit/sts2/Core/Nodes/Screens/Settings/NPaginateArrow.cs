// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Settings.NPaginateArrow
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
namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings;

[ScriptPath("res://src/Core/Nodes/Screens/Settings/NPaginateArrow.cs")]
public class NPaginateArrow : NButton
{
  private static readonly StringName _v = new StringName("v");
  [Export]
  private bool _pageLeft;
  private TextureRect _image;
  private ShaderMaterial _hsv;
  private NPaginator? _paginator;
  private Tween? _tween;
  private Vector2 _baseScale;

  public override void _Ready()
  {
    this.ConnectSignals();
    this._paginator = ((Node) this).GetParent<NPaginator>();
    this._image = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("Image"));
    this._baseScale = ((Control) this._image).Scale;
    this._hsv = (ShaderMaterial) ((CanvasItem) this._image).Material;
  }

  protected override void OnRelease()
  {
    if (this._pageLeft)
      this._paginator?.PageLeft();
    else
      this._paginator?.PageRight();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._image, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(this._baseScale, 1.1f)), 0.05);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NPaginateArrow._v), Variant.op_Implicit(1.5f), 0.05);
  }

  protected override void OnFocus()
  {
    base.OnFocus();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._image, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(this._baseScale, 1.1f)), 0.05);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NPaginateArrow._v), Variant.op_Implicit(1.5f), 0.05);
  }

  protected override void OnUnfocus()
  {
    base.OnUnfocus();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._image, NodePath.op_Implicit("scale"), Variant.op_Implicit(this._baseScale), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NPaginateArrow._v), Variant.op_Implicit(0.9f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  protected override void OnPress()
  {
    base.OnPress();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._image, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(this._baseScale, 0.9f)), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NPaginateArrow._v), Variant.op_Implicit(0.8f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  private void UpdateShaderV(float value)
  {
    this._hsv.SetShaderParameter(NPaginateArrow._v, Variant.op_Implicit(value));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(6)
    {
      new MethodInfo(NPaginateArrow.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPaginateArrow.MethodName.OnRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPaginateArrow.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPaginateArrow.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPaginateArrow.MethodName.OnPress, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPaginateArrow.MethodName.UpdateShaderV, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
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
    if (StringName.op_Equality(ref method, NPaginateArrow.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPaginateArrow.MethodName.OnRelease) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnRelease();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPaginateArrow.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPaginateArrow.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPaginateArrow.MethodName.OnPress) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPress();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NPaginateArrow.MethodName.UpdateShaderV) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.UpdateShaderV(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NPaginateArrow.MethodName._Ready) || StringName.op_Equality(ref method, NPaginateArrow.MethodName.OnRelease) || StringName.op_Equality(ref method, NPaginateArrow.MethodName.OnFocus) || StringName.op_Equality(ref method, NPaginateArrow.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NPaginateArrow.MethodName.OnPress) || StringName.op_Equality(ref method, NPaginateArrow.MethodName.UpdateShaderV) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NPaginateArrow.PropertyName._pageLeft))
    {
      this._pageLeft = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPaginateArrow.PropertyName._image))
    {
      this._image = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPaginateArrow.PropertyName._hsv))
    {
      this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPaginateArrow.PropertyName._paginator))
    {
      this._paginator = VariantUtils.ConvertTo<NPaginator>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPaginateArrow.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NPaginateArrow.PropertyName._baseScale))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._baseScale = VariantUtils.ConvertTo<Vector2>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NPaginateArrow.PropertyName._pageLeft))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._pageLeft);
      return true;
    }
    if (StringName.op_Equality(ref name, NPaginateArrow.PropertyName._image))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._image);
      return true;
    }
    if (StringName.op_Equality(ref name, NPaginateArrow.PropertyName._hsv))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
      return true;
    }
    if (StringName.op_Equality(ref name, NPaginateArrow.PropertyName._paginator))
    {
      value = VariantUtils.CreateFrom<NPaginator>(ref this._paginator);
      return true;
    }
    if (StringName.op_Equality(ref name, NPaginateArrow.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NPaginateArrow.PropertyName._baseScale))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Vector2>(ref this._baseScale);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 1L, NPaginateArrow.PropertyName._pageLeft, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NPaginateArrow.PropertyName._image, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPaginateArrow.PropertyName._hsv, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPaginateArrow.PropertyName._paginator, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPaginateArrow.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NPaginateArrow.PropertyName._baseScale, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NPaginateArrow.PropertyName._pageLeft, Variant.From<bool>(ref this._pageLeft));
    info.AddProperty(NPaginateArrow.PropertyName._image, Variant.From<TextureRect>(ref this._image));
    info.AddProperty(NPaginateArrow.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
    info.AddProperty(NPaginateArrow.PropertyName._paginator, Variant.From<NPaginator>(ref this._paginator));
    info.AddProperty(NPaginateArrow.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NPaginateArrow.PropertyName._baseScale, Variant.From<Vector2>(ref this._baseScale));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NPaginateArrow.PropertyName._pageLeft, ref variant1))
      this._pageLeft = ((Variant) ref variant1).As<bool>();
    Variant variant2;
    if (info.TryGetProperty(NPaginateArrow.PropertyName._image, ref variant2))
      this._image = ((Variant) ref variant2).As<TextureRect>();
    Variant variant3;
    if (info.TryGetProperty(NPaginateArrow.PropertyName._hsv, ref variant3))
      this._hsv = ((Variant) ref variant3).As<ShaderMaterial>();
    Variant variant4;
    if (info.TryGetProperty(NPaginateArrow.PropertyName._paginator, ref variant4))
      this._paginator = ((Variant) ref variant4).As<NPaginator>();
    Variant variant5;
    if (info.TryGetProperty(NPaginateArrow.PropertyName._tween, ref variant5))
      this._tween = ((Variant) ref variant5).As<Tween>();
    Variant variant6;
    if (!info.TryGetProperty(NPaginateArrow.PropertyName._baseScale, ref variant6))
      return;
    this._baseScale = ((Variant) ref variant6).As<Vector2>();
  }

  public new class MethodName : NButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName OnRelease = StringName.op_Implicit(nameof (OnRelease));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public new static readonly StringName OnPress = StringName.op_Implicit(nameof (OnPress));
    public static readonly StringName UpdateShaderV = StringName.op_Implicit(nameof (UpdateShaderV));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public static readonly StringName _pageLeft = StringName.op_Implicit(nameof (_pageLeft));
    public static readonly StringName _image = StringName.op_Implicit(nameof (_image));
    public static readonly StringName _hsv = StringName.op_Implicit(nameof (_hsv));
    public static readonly StringName _paginator = StringName.op_Implicit(nameof (_paginator));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _baseScale = StringName.op_Implicit(nameof (_baseScale));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
