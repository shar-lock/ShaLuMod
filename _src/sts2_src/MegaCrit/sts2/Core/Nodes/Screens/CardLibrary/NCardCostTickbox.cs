// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary.NCardCostTickbox
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary;

[ScriptPath("res://src/Core/Nodes/Screens/CardLibrary/NCardCostTickbox.cs")]
public class NCardCostTickbox : NButton
{
  private static readonly StringName _v = new StringName("v");
  private static readonly StringName _s = new StringName("s");
  private bool _isTicked = true;
  private float _baseS = 1f;
  private float _baseV = 1.2f;
  private Control _outline;
  private Control _image;
  private ShaderMaterial _hsv;
  private Tween? _tween;
  private Vector2 _baseScale;
  private float _hoverV = 1.2f;
  private float _hoverScale = 1.2f;
  private float _pressDownScale = 0.9f;

  public LocString Loc { get; set; }

  public bool IsTicked
  {
    get => this._isTicked;
    set
    {
      this._isTicked = value;
      ((CanvasItem) this._outline).Visible = this._isTicked;
      this.OnToggle();
    }
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._baseScale = this.Scale;
    this._outline = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Outline"));
    this._image = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Image"));
    this._hsv = (ShaderMaterial) ((CanvasItem) this._image).Material;
    ((GodotObject) this).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>((Action<NButton>) (_ => this.IsTicked = !this.IsTicked)), 0U);
  }

  private void OnToggle()
  {
    this._baseS = this._isTicked ? 1f : 0.65f;
    this._baseV = this._isTicked ? 1.2f : 0.7f;
    this.UpdateShaderS(this._baseS);
    this.UpdateShaderV(this._baseV);
  }

  protected override void OnRelease()
  {
    base.OnRelease();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._image, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(this._baseScale, this._hoverScale)), 0.05);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NCardCostTickbox._s), Variant.op_Implicit(this._baseS), 0.05);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NCardCostTickbox._v), Variant.op_Implicit(this._baseV), 0.05);
  }

  protected override void OnFocus()
  {
    base.OnFocus();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._image, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(this._baseScale, this._hoverScale)), 0.05);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NCardCostTickbox._s), Variant.op_Implicit(this._isTicked ? 1.4f : 1f), 0.05);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NCardCostTickbox._v), Variant.op_Implicit(this._isTicked ? 1.4f : 1f), 0.05);
    NHoverTipSet.CreateAndShow((Control) this, (IHoverTip) new HoverTip(this.Loc))?.SetGlobalPosition(new Vector2(310f, this.GlobalPosition.Y), false);
  }

  protected override void OnUnfocus()
  {
    base.OnUnfocus();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._image, NodePath.op_Implicit("scale"), Variant.op_Implicit(this._baseScale), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NCardCostTickbox._s), Variant.op_Implicit(this._baseS), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NCardCostTickbox._v), Variant.op_Implicit(this._baseV), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    NHoverTipSet.Remove((Control) this);
  }

  protected override void OnPress()
  {
    base.OnPress();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._image, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(this._baseScale, this._pressDownScale)), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NCardCostTickbox._s), Variant.op_Implicit(this._baseS), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NCardCostTickbox._v), Variant.op_Implicit(this._baseV), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  private void UpdateShaderS(float value)
  {
    this._hsv.SetShaderParameter(NCardCostTickbox._s, Variant.op_Implicit(value));
  }

  private void UpdateShaderV(float value)
  {
    this._hsv.SetShaderParameter(NCardCostTickbox._v, Variant.op_Implicit(value));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(8)
    {
      new MethodInfo(NCardCostTickbox.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardCostTickbox.MethodName.OnToggle, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardCostTickbox.MethodName.OnRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardCostTickbox.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardCostTickbox.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardCostTickbox.MethodName.OnPress, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardCostTickbox.MethodName.UpdateShaderS, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("value"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCardCostTickbox.MethodName.UpdateShaderV, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
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
    if (StringName.op_Equality(ref method, NCardCostTickbox.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardCostTickbox.MethodName.OnToggle) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnToggle();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardCostTickbox.MethodName.OnRelease) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnRelease();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardCostTickbox.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardCostTickbox.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardCostTickbox.MethodName.OnPress) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPress();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardCostTickbox.MethodName.UpdateShaderS) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateShaderS(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCardCostTickbox.MethodName.UpdateShaderV) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.UpdateShaderV(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCardCostTickbox.MethodName._Ready) || StringName.op_Equality(ref method, NCardCostTickbox.MethodName.OnToggle) || StringName.op_Equality(ref method, NCardCostTickbox.MethodName.OnRelease) || StringName.op_Equality(ref method, NCardCostTickbox.MethodName.OnFocus) || StringName.op_Equality(ref method, NCardCostTickbox.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NCardCostTickbox.MethodName.OnPress) || StringName.op_Equality(ref method, NCardCostTickbox.MethodName.UpdateShaderS) || StringName.op_Equality(ref method, NCardCostTickbox.MethodName.UpdateShaderV) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardCostTickbox.PropertyName.IsTicked))
    {
      this.IsTicked = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardCostTickbox.PropertyName._isTicked))
    {
      this._isTicked = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardCostTickbox.PropertyName._baseS))
    {
      this._baseS = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardCostTickbox.PropertyName._baseV))
    {
      this._baseV = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardCostTickbox.PropertyName._outline))
    {
      this._outline = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardCostTickbox.PropertyName._image))
    {
      this._image = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardCostTickbox.PropertyName._hsv))
    {
      this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardCostTickbox.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardCostTickbox.PropertyName._baseScale))
    {
      this._baseScale = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardCostTickbox.PropertyName._hoverV))
    {
      this._hoverV = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardCostTickbox.PropertyName._hoverScale))
    {
      this._hoverScale = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardCostTickbox.PropertyName._pressDownScale))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._pressDownScale = VariantUtils.ConvertTo<float>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardCostTickbox.PropertyName.IsTicked))
    {
      ref godot_variant local = ref value;
      bool isTicked = this.IsTicked;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref isTicked);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCardCostTickbox.PropertyName._isTicked))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isTicked);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardCostTickbox.PropertyName._baseS))
    {
      value = VariantUtils.CreateFrom<float>(ref this._baseS);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardCostTickbox.PropertyName._baseV))
    {
      value = VariantUtils.CreateFrom<float>(ref this._baseV);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardCostTickbox.PropertyName._outline))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._outline);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardCostTickbox.PropertyName._image))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._image);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardCostTickbox.PropertyName._hsv))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardCostTickbox.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardCostTickbox.PropertyName._baseScale))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._baseScale);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardCostTickbox.PropertyName._hoverV))
    {
      value = VariantUtils.CreateFrom<float>(ref this._hoverV);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardCostTickbox.PropertyName._hoverScale))
    {
      value = VariantUtils.CreateFrom<float>(ref this._hoverScale);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardCostTickbox.PropertyName._pressDownScale))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<float>(ref this._pressDownScale);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 1L, NCardCostTickbox.PropertyName._isTicked, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NCardCostTickbox.PropertyName._baseS, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NCardCostTickbox.PropertyName._baseV, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCardCostTickbox.PropertyName.IsTicked, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardCostTickbox.PropertyName._outline, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardCostTickbox.PropertyName._image, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardCostTickbox.PropertyName._hsv, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardCostTickbox.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NCardCostTickbox.PropertyName._baseScale, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NCardCostTickbox.PropertyName._hoverV, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NCardCostTickbox.PropertyName._hoverScale, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NCardCostTickbox.PropertyName._pressDownScale, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName isTicked1 = NCardCostTickbox.PropertyName.IsTicked;
    bool isTicked2 = this.IsTicked;
    Variant variant = Variant.From<bool>(ref isTicked2);
    serializationInfo.AddProperty(isTicked1, variant);
    info.AddProperty(NCardCostTickbox.PropertyName._isTicked, Variant.From<bool>(ref this._isTicked));
    info.AddProperty(NCardCostTickbox.PropertyName._baseS, Variant.From<float>(ref this._baseS));
    info.AddProperty(NCardCostTickbox.PropertyName._baseV, Variant.From<float>(ref this._baseV));
    info.AddProperty(NCardCostTickbox.PropertyName._outline, Variant.From<Control>(ref this._outline));
    info.AddProperty(NCardCostTickbox.PropertyName._image, Variant.From<Control>(ref this._image));
    info.AddProperty(NCardCostTickbox.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
    info.AddProperty(NCardCostTickbox.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NCardCostTickbox.PropertyName._baseScale, Variant.From<Vector2>(ref this._baseScale));
    info.AddProperty(NCardCostTickbox.PropertyName._hoverV, Variant.From<float>(ref this._hoverV));
    info.AddProperty(NCardCostTickbox.PropertyName._hoverScale, Variant.From<float>(ref this._hoverScale));
    info.AddProperty(NCardCostTickbox.PropertyName._pressDownScale, Variant.From<float>(ref this._pressDownScale));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCardCostTickbox.PropertyName.IsTicked, ref variant1))
      this.IsTicked = ((Variant) ref variant1).As<bool>();
    Variant variant2;
    if (info.TryGetProperty(NCardCostTickbox.PropertyName._isTicked, ref variant2))
      this._isTicked = ((Variant) ref variant2).As<bool>();
    Variant variant3;
    if (info.TryGetProperty(NCardCostTickbox.PropertyName._baseS, ref variant3))
      this._baseS = ((Variant) ref variant3).As<float>();
    Variant variant4;
    if (info.TryGetProperty(NCardCostTickbox.PropertyName._baseV, ref variant4))
      this._baseV = ((Variant) ref variant4).As<float>();
    Variant variant5;
    if (info.TryGetProperty(NCardCostTickbox.PropertyName._outline, ref variant5))
      this._outline = ((Variant) ref variant5).As<Control>();
    Variant variant6;
    if (info.TryGetProperty(NCardCostTickbox.PropertyName._image, ref variant6))
      this._image = ((Variant) ref variant6).As<Control>();
    Variant variant7;
    if (info.TryGetProperty(NCardCostTickbox.PropertyName._hsv, ref variant7))
      this._hsv = ((Variant) ref variant7).As<ShaderMaterial>();
    Variant variant8;
    if (info.TryGetProperty(NCardCostTickbox.PropertyName._tween, ref variant8))
      this._tween = ((Variant) ref variant8).As<Tween>();
    Variant variant9;
    if (info.TryGetProperty(NCardCostTickbox.PropertyName._baseScale, ref variant9))
      this._baseScale = ((Variant) ref variant9).As<Vector2>();
    Variant variant10;
    if (info.TryGetProperty(NCardCostTickbox.PropertyName._hoverV, ref variant10))
      this._hoverV = ((Variant) ref variant10).As<float>();
    Variant variant11;
    if (info.TryGetProperty(NCardCostTickbox.PropertyName._hoverScale, ref variant11))
      this._hoverScale = ((Variant) ref variant11).As<float>();
    Variant variant12;
    if (!info.TryGetProperty(NCardCostTickbox.PropertyName._pressDownScale, ref variant12))
      return;
    this._pressDownScale = ((Variant) ref variant12).As<float>();
  }

  public new class MethodName : NButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnToggle = StringName.op_Implicit(nameof (OnToggle));
    public new static readonly StringName OnRelease = StringName.op_Implicit(nameof (OnRelease));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public new static readonly StringName OnPress = StringName.op_Implicit(nameof (OnPress));
    public static readonly StringName UpdateShaderS = StringName.op_Implicit(nameof (UpdateShaderS));
    public static readonly StringName UpdateShaderV = StringName.op_Implicit(nameof (UpdateShaderV));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public static readonly StringName IsTicked = StringName.op_Implicit(nameof (IsTicked));
    public static readonly StringName _isTicked = StringName.op_Implicit(nameof (_isTicked));
    public static readonly StringName _baseS = StringName.op_Implicit(nameof (_baseS));
    public static readonly StringName _baseV = StringName.op_Implicit(nameof (_baseV));
    public static readonly StringName _outline = StringName.op_Implicit(nameof (_outline));
    public static readonly StringName _image = StringName.op_Implicit(nameof (_image));
    public static readonly StringName _hsv = StringName.op_Implicit(nameof (_hsv));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _baseScale = StringName.op_Implicit(nameof (_baseScale));
    public static readonly StringName _hoverV = StringName.op_Implicit(nameof (_hoverV));
    public static readonly StringName _hoverScale = StringName.op_Implicit(nameof (_hoverScale));
    public static readonly StringName _pressDownScale = StringName.op_Implicit(nameof (_pressDownScale));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
