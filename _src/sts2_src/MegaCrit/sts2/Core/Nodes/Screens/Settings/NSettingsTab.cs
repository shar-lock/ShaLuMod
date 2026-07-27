// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Settings.NSettingsTab
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings;

[ScriptPath("res://src/Core/Nodes/Screens/Settings/NSettingsTab.cs")]
public class NSettingsTab : NButton
{
  private static readonly StringName _v = new StringName("v");
  private bool _isSelected;
  private TextureRect _outline;
  private TextureRect _image;
  private MegaLabel _label;
  private ShaderMaterial _hsv;
  private Tween? _tween;
  private const float _defaultV = 0.9f;
  private const float _hoverV = 1.2f;
  private const float _unhoverDuration = 0.5f;

  public override void _Ready()
  {
    this.ConnectSignals();
    this._outline = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("Outline"));
    this._image = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("TabImage"));
    this._label = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("Label"));
    this._hsv = (ShaderMaterial) ((CanvasItem) this._image).Material;
    ((CanvasItem) this._label).Modulate = StsColors.halfTransparentCream;
  }

  public void ForceTabPressed() => this.EmitSignalReleased((NClickableControl) this);

  public void Select()
  {
    if (this._isSelected)
      return;
    this._isSelected = true;
    this._tween?.Kill();
    ((CanvasItem) this._label).Modulate = StsColors.cream;
    ((CanvasItem) this._image).Modulate = Colors.White;
    ((CanvasItem) this._outline).Visible = true;
  }

  public void Deselect()
  {
    if (!this._isSelected)
      return;
    this._isSelected = false;
    this._tween?.Kill();
    ((CanvasItem) this._outline).Visible = false;
    ((CanvasItem) this._label).Modulate = StsColors.halfTransparentCream;
  }

  protected override void OnFocus()
  {
    base.OnFocus();
    this._tween?.Kill();
    ((CanvasItem) this._label).Modulate = StsColors.gold;
    this._hsv.SetShaderParameter(NSettingsTab._v, Variant.op_Implicit(1.2f));
    this.Scale = Vector2.op_Multiply(Vector2.One, 1.05f);
  }

  protected override void OnUnfocus()
  {
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    Color color = this._isSelected ? StsColors.cream : StsColors.halfTransparentCream;
    this._tween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("modulate"), Variant.op_Implicit(color), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderParam)), Variant.op_Implicit(1.2f), Variant.op_Implicit(0.9f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  protected override void OnEnable()
  {
    this._tween?.Kill();
    ((CanvasItem) this).Modulate = Colors.White;
  }

  protected override void OnDisable()
  {
    this._tween?.Kill();
    ((CanvasItem) this).Modulate = Colors.DimGray;
  }

  private void UpdateShaderParam(float newV)
  {
    this._hsv.SetShaderParameter(NSettingsTab._v, Variant.op_Implicit(newV));
  }

  public void SetLabel(string text) => this._label.SetTextAutoSize(text);

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(10)
    {
      new MethodInfo(NSettingsTab.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSettingsTab.MethodName.ForceTabPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSettingsTab.MethodName.Select, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSettingsTab.MethodName.Deselect, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSettingsTab.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSettingsTab.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSettingsTab.MethodName.OnEnable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSettingsTab.MethodName.OnDisable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSettingsTab.MethodName.UpdateShaderParam, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("newV"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NSettingsTab.MethodName.SetLabel, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("text"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NSettingsTab.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSettingsTab.MethodName.ForceTabPressed) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ForceTabPressed();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSettingsTab.MethodName.Select) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Select();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSettingsTab.MethodName.Deselect) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Deselect();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSettingsTab.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSettingsTab.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSettingsTab.MethodName.OnEnable) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnEnable();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSettingsTab.MethodName.OnDisable) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnDisable();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSettingsTab.MethodName.UpdateShaderParam) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateShaderParam(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NSettingsTab.MethodName.SetLabel) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.SetLabel(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NSettingsTab.MethodName._Ready) || StringName.op_Equality(ref method, NSettingsTab.MethodName.ForceTabPressed) || StringName.op_Equality(ref method, NSettingsTab.MethodName.Select) || StringName.op_Equality(ref method, NSettingsTab.MethodName.Deselect) || StringName.op_Equality(ref method, NSettingsTab.MethodName.OnFocus) || StringName.op_Equality(ref method, NSettingsTab.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NSettingsTab.MethodName.OnEnable) || StringName.op_Equality(ref method, NSettingsTab.MethodName.OnDisable) || StringName.op_Equality(ref method, NSettingsTab.MethodName.UpdateShaderParam) || StringName.op_Equality(ref method, NSettingsTab.MethodName.SetLabel) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSettingsTab.PropertyName._isSelected))
    {
      this._isSelected = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSettingsTab.PropertyName._outline))
    {
      this._outline = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSettingsTab.PropertyName._image))
    {
      this._image = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSettingsTab.PropertyName._label))
    {
      this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSettingsTab.PropertyName._hsv))
    {
      this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSettingsTab.PropertyName._tween))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._tween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSettingsTab.PropertyName._isSelected))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isSelected);
      return true;
    }
    if (StringName.op_Equality(ref name, NSettingsTab.PropertyName._outline))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._outline);
      return true;
    }
    if (StringName.op_Equality(ref name, NSettingsTab.PropertyName._image))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._image);
      return true;
    }
    if (StringName.op_Equality(ref name, NSettingsTab.PropertyName._label))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
      return true;
    }
    if (StringName.op_Equality(ref name, NSettingsTab.PropertyName._hsv))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSettingsTab.PropertyName._tween))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Tween>(ref this._tween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 1L, NSettingsTab.PropertyName._isSelected, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSettingsTab.PropertyName._outline, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSettingsTab.PropertyName._image, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSettingsTab.PropertyName._label, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSettingsTab.PropertyName._hsv, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSettingsTab.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NSettingsTab.PropertyName._isSelected, Variant.From<bool>(ref this._isSelected));
    info.AddProperty(NSettingsTab.PropertyName._outline, Variant.From<TextureRect>(ref this._outline));
    info.AddProperty(NSettingsTab.PropertyName._image, Variant.From<TextureRect>(ref this._image));
    info.AddProperty(NSettingsTab.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
    info.AddProperty(NSettingsTab.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
    info.AddProperty(NSettingsTab.PropertyName._tween, Variant.From<Tween>(ref this._tween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NSettingsTab.PropertyName._isSelected, ref variant1))
      this._isSelected = ((Variant) ref variant1).As<bool>();
    Variant variant2;
    if (info.TryGetProperty(NSettingsTab.PropertyName._outline, ref variant2))
      this._outline = ((Variant) ref variant2).As<TextureRect>();
    Variant variant3;
    if (info.TryGetProperty(NSettingsTab.PropertyName._image, ref variant3))
      this._image = ((Variant) ref variant3).As<TextureRect>();
    Variant variant4;
    if (info.TryGetProperty(NSettingsTab.PropertyName._label, ref variant4))
      this._label = ((Variant) ref variant4).As<MegaLabel>();
    Variant variant5;
    if (info.TryGetProperty(NSettingsTab.PropertyName._hsv, ref variant5))
      this._hsv = ((Variant) ref variant5).As<ShaderMaterial>();
    Variant variant6;
    if (!info.TryGetProperty(NSettingsTab.PropertyName._tween, ref variant6))
      return;
    this._tween = ((Variant) ref variant6).As<Tween>();
  }

  public new class MethodName : NButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName ForceTabPressed = StringName.op_Implicit(nameof (ForceTabPressed));
    public static readonly StringName Select = StringName.op_Implicit(nameof (Select));
    public static readonly StringName Deselect = StringName.op_Implicit(nameof (Deselect));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public new static readonly StringName OnEnable = StringName.op_Implicit(nameof (OnEnable));
    public new static readonly StringName OnDisable = StringName.op_Implicit(nameof (OnDisable));
    public static readonly StringName UpdateShaderParam = StringName.op_Implicit(nameof (UpdateShaderParam));
    public static readonly StringName SetLabel = StringName.op_Implicit(nameof (SetLabel));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public static readonly StringName _isSelected = StringName.op_Implicit(nameof (_isSelected));
    public static readonly StringName _outline = StringName.op_Implicit(nameof (_outline));
    public static readonly StringName _image = StringName.op_Implicit(nameof (_image));
    public static readonly StringName _label = StringName.op_Implicit(nameof (_label));
    public static readonly StringName _hsv = StringName.op_Implicit(nameof (_hsv));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
