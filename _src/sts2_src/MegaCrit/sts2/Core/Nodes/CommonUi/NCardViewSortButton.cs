// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.CommonUi.NCardViewSortButton
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.CommonUi;

[ScriptPath("res://src/Core/Nodes/CommonUi/NCardViewSortButton.cs")]
public class NCardViewSortButton : NButton
{
  private static readonly StringName _v = new StringName("v");
  private static readonly StringName _s = new StringName("s");
  private static readonly StringName _h = new StringName("h");
  private bool _isDescending;
  private Control _button;
  private ShaderMaterial _hsv;
  private MegaLabel _label;
  private TextureRect _sortIcon;
  private NSelectionReticle _selectionReticle;
  private Tween? _tween;

  public bool IsDescending
  {
    get => this._isDescending;
    set
    {
      this._isDescending = value;
      this.OnToggle();
    }
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._button = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%ButtonImage"));
    this._hsv = (ShaderMaterial) ((CanvasItem) this._button).GetMaterial();
    this._label = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%Label"));
    this._sortIcon = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%Image"));
    this._selectionReticle = ((Node) this).GetNode<NSelectionReticle>(NodePath.op_Implicit("SelectionReticle"));
  }

  public void SetHue(ShaderMaterial mat)
  {
    this._hsv.SetShaderParameter(NCardViewSortButton._h, mat.GetShaderParameter(NCardViewSortButton._h));
  }

  private void OnToggle() => this._sortIcon.SetFlipV(!this._isDescending);

  protected override void OnRelease()
  {
    base.OnRelease();
    this.IsDescending = !this.IsDescending;
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._button, NodePath.op_Implicit("scale"), Variant.op_Implicit(new Vector2(1.05f, 1.05f)), 0.05);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NCardViewSortButton._s), Variant.op_Implicit(1f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NCardViewSortButton._v), Variant.op_Implicit(1f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  protected override void OnFocus()
  {
    base.OnFocus();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._button, NodePath.op_Implicit("scale"), Variant.op_Implicit(new Vector2(1.05f, 1.05f)), 0.05);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NCardViewSortButton._s), Variant.op_Implicit(1f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NCardViewSortButton._v), Variant.op_Implicit(1f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    if (!NControllerManager.Instance.IsUsingController)
      return;
    this._selectionReticle.OnSelect();
  }

  protected override void OnUnfocus()
  {
    base.OnUnfocus();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._button, NodePath.op_Implicit("scale"), Variant.op_Implicit(new Vector2(1.05f, 1f)), 0.5);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NCardViewSortButton._s), Variant.op_Implicit(0.8f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NCardViewSortButton._v), Variant.op_Implicit(0.8f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    if (!NControllerManager.Instance.IsUsingController)
      return;
    this._selectionReticle.OnDeselect();
  }

  protected override void OnPress()
  {
    base.OnPress();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._button, NodePath.op_Implicit("scale"), Variant.op_Implicit(new Vector2(1f, 0.95f)), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NCardViewSortButton._s), Variant.op_Implicit(0.8f), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NCardViewSortButton._v), Variant.op_Implicit(0.8f), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  public void SetLabel(string text) => this._label.SetTextAutoSize(text);

  private void UpdateShaderS(float value)
  {
    this._hsv.SetShaderParameter(NCardViewSortButton._s, Variant.op_Implicit(value));
  }

  private void UpdateShaderV(float value)
  {
    this._hsv.SetShaderParameter(NCardViewSortButton._v, Variant.op_Implicit(value));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(10)
    {
      new MethodInfo(NCardViewSortButton.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardViewSortButton.MethodName.SetHue, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("mat"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("ShaderMaterial"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCardViewSortButton.MethodName.OnToggle, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardViewSortButton.MethodName.OnRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardViewSortButton.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardViewSortButton.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardViewSortButton.MethodName.OnPress, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardViewSortButton.MethodName.SetLabel, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("text"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCardViewSortButton.MethodName.UpdateShaderS, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("value"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCardViewSortButton.MethodName.UpdateShaderV, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
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
    if (StringName.op_Equality(ref method, NCardViewSortButton.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardViewSortButton.MethodName.SetHue) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetHue(VariantUtils.ConvertTo<ShaderMaterial>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardViewSortButton.MethodName.OnToggle) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnToggle();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardViewSortButton.MethodName.OnRelease) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnRelease();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardViewSortButton.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardViewSortButton.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardViewSortButton.MethodName.OnPress) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPress();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardViewSortButton.MethodName.SetLabel) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetLabel(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardViewSortButton.MethodName.UpdateShaderS) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateShaderS(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCardViewSortButton.MethodName.UpdateShaderV) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.UpdateShaderV(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCardViewSortButton.MethodName._Ready) || StringName.op_Equality(ref method, NCardViewSortButton.MethodName.SetHue) || StringName.op_Equality(ref method, NCardViewSortButton.MethodName.OnToggle) || StringName.op_Equality(ref method, NCardViewSortButton.MethodName.OnRelease) || StringName.op_Equality(ref method, NCardViewSortButton.MethodName.OnFocus) || StringName.op_Equality(ref method, NCardViewSortButton.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NCardViewSortButton.MethodName.OnPress) || StringName.op_Equality(ref method, NCardViewSortButton.MethodName.SetLabel) || StringName.op_Equality(ref method, NCardViewSortButton.MethodName.UpdateShaderS) || StringName.op_Equality(ref method, NCardViewSortButton.MethodName.UpdateShaderV) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardViewSortButton.PropertyName.IsDescending))
    {
      this.IsDescending = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardViewSortButton.PropertyName._isDescending))
    {
      this._isDescending = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardViewSortButton.PropertyName._button))
    {
      this._button = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardViewSortButton.PropertyName._hsv))
    {
      this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardViewSortButton.PropertyName._label))
    {
      this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardViewSortButton.PropertyName._sortIcon))
    {
      this._sortIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardViewSortButton.PropertyName._selectionReticle))
    {
      this._selectionReticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardViewSortButton.PropertyName._tween))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._tween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardViewSortButton.PropertyName.IsDescending))
    {
      ref godot_variant local = ref value;
      bool isDescending = this.IsDescending;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref isDescending);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCardViewSortButton.PropertyName._isDescending))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isDescending);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardViewSortButton.PropertyName._button))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._button);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardViewSortButton.PropertyName._hsv))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardViewSortButton.PropertyName._label))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardViewSortButton.PropertyName._sortIcon))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._sortIcon);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardViewSortButton.PropertyName._selectionReticle))
    {
      value = VariantUtils.CreateFrom<NSelectionReticle>(ref this._selectionReticle);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardViewSortButton.PropertyName._tween))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Tween>(ref this._tween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 1L, NCardViewSortButton.PropertyName._isDescending, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardViewSortButton.PropertyName._button, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardViewSortButton.PropertyName._hsv, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardViewSortButton.PropertyName._label, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardViewSortButton.PropertyName._sortIcon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardViewSortButton.PropertyName._selectionReticle, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardViewSortButton.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCardViewSortButton.PropertyName.IsDescending, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName isDescending1 = NCardViewSortButton.PropertyName.IsDescending;
    bool isDescending2 = this.IsDescending;
    Variant variant = Variant.From<bool>(ref isDescending2);
    serializationInfo.AddProperty(isDescending1, variant);
    info.AddProperty(NCardViewSortButton.PropertyName._isDescending, Variant.From<bool>(ref this._isDescending));
    info.AddProperty(NCardViewSortButton.PropertyName._button, Variant.From<Control>(ref this._button));
    info.AddProperty(NCardViewSortButton.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
    info.AddProperty(NCardViewSortButton.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
    info.AddProperty(NCardViewSortButton.PropertyName._sortIcon, Variant.From<TextureRect>(ref this._sortIcon));
    info.AddProperty(NCardViewSortButton.PropertyName._selectionReticle, Variant.From<NSelectionReticle>(ref this._selectionReticle));
    info.AddProperty(NCardViewSortButton.PropertyName._tween, Variant.From<Tween>(ref this._tween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCardViewSortButton.PropertyName.IsDescending, ref variant1))
      this.IsDescending = ((Variant) ref variant1).As<bool>();
    Variant variant2;
    if (info.TryGetProperty(NCardViewSortButton.PropertyName._isDescending, ref variant2))
      this._isDescending = ((Variant) ref variant2).As<bool>();
    Variant variant3;
    if (info.TryGetProperty(NCardViewSortButton.PropertyName._button, ref variant3))
      this._button = ((Variant) ref variant3).As<Control>();
    Variant variant4;
    if (info.TryGetProperty(NCardViewSortButton.PropertyName._hsv, ref variant4))
      this._hsv = ((Variant) ref variant4).As<ShaderMaterial>();
    Variant variant5;
    if (info.TryGetProperty(NCardViewSortButton.PropertyName._label, ref variant5))
      this._label = ((Variant) ref variant5).As<MegaLabel>();
    Variant variant6;
    if (info.TryGetProperty(NCardViewSortButton.PropertyName._sortIcon, ref variant6))
      this._sortIcon = ((Variant) ref variant6).As<TextureRect>();
    Variant variant7;
    if (info.TryGetProperty(NCardViewSortButton.PropertyName._selectionReticle, ref variant7))
      this._selectionReticle = ((Variant) ref variant7).As<NSelectionReticle>();
    Variant variant8;
    if (!info.TryGetProperty(NCardViewSortButton.PropertyName._tween, ref variant8))
      return;
    this._tween = ((Variant) ref variant8).As<Tween>();
  }

  public new class MethodName : NButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName SetHue = StringName.op_Implicit(nameof (SetHue));
    public static readonly StringName OnToggle = StringName.op_Implicit(nameof (OnToggle));
    public new static readonly StringName OnRelease = StringName.op_Implicit(nameof (OnRelease));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public new static readonly StringName OnPress = StringName.op_Implicit(nameof (OnPress));
    public static readonly StringName SetLabel = StringName.op_Implicit(nameof (SetLabel));
    public static readonly StringName UpdateShaderS = StringName.op_Implicit(nameof (UpdateShaderS));
    public static readonly StringName UpdateShaderV = StringName.op_Implicit(nameof (UpdateShaderV));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public static readonly StringName IsDescending = StringName.op_Implicit(nameof (IsDescending));
    public static readonly StringName _isDescending = StringName.op_Implicit(nameof (_isDescending));
    public static readonly StringName _button = StringName.op_Implicit(nameof (_button));
    public static readonly StringName _hsv = StringName.op_Implicit(nameof (_hsv));
    public static readonly StringName _label = StringName.op_Implicit(nameof (_label));
    public static readonly StringName _sortIcon = StringName.op_Implicit(nameof (_sortIcon));
    public static readonly StringName _selectionReticle = StringName.op_Implicit(nameof (_selectionReticle));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
