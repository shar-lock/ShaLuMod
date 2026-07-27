// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NMainMenuTextButton
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.Fonts;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;

[ScriptPath("res://src/Core/Nodes/Screens/MainMenu/NMainMenuTextButton.cs")]
public class NMainMenuTextButton : NButton
{
  public MegaLabel? label;
  private Color _defaultColor = StsColors.cream;
  private Color _hoveredColor = StsColors.gold;
  private Color _downColor = StsColors.halfTransparentWhite;
  private static readonly Vector2 _hoverScale = new Vector2(1.05f, 1.05f);
  private static readonly Vector2 _downScale = new Vector2(0.95f, 0.95f);
  private static readonly StyleBoxEmpty _emptyStyleBox = new StyleBoxEmpty();
  private const double _pressDownDur = 0.2;
  private const double _unhoverAnimDur = 0.5;
  private Tween? _tween;
  private LocString? _locString;

  public override void _Ready()
  {
    if (((object) this).GetType() != typeof (NMainMenuTextButton))
      throw new InvalidOperationException("Don't call base._Ready()!");
    this.ConnectSignals();
  }

  protected override void ConnectSignals()
  {
    base.ConnectSignals();
    this.label = ((Node) this).GetChild<MegaLabel>(0, false);
    ((Control) this.label).AddThemeStyleboxOverride(ThemeConstants.Control.Focus, (StyleBox) NMainMenuTextButton._emptyStyleBox);
    ((Control) this.label).FocusMode = (Control.FocusModeEnum) 0L;
  }

  public void SetLocalization(string locKey)
  {
    this._locString = new LocString("main_menu_ui", locKey);
    this.RefreshLabel();
  }

  public override void _Notification(int what)
  {
    if (what != 2010)
      return;
    this.RefreshLabel();
  }

  private void RefreshLabel()
  {
    if (this.label == null || this._locString == null)
      return;
    this.label.Text = this._locString.GetFormattedText();
    ((Control) this.label).ApplyLocaleFontSubstitution(FontType.Regular, ThemeConstants.Label.Font);
    TaskHelper.RunSafely(this.UpdatePivotOffset());
  }

  private async Task UpdatePivotOffset()
  {
    double num = (double) await ((Node) this).AwaitProcessFrame();
    if (this.label == null)
      return;
    ((Control) this.label).PivotOffset = Vector2.op_Multiply(((Control) this.label).Size, 0.5f);
  }

  protected override void OnPress()
  {
    base.OnPress();
    this.AnimPressDown();
  }

  protected override void OnRelease() => this.AnimRelease();

  protected override void OnFocus()
  {
    base.OnFocus();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this.label, NodePath.op_Implicit("scale"), Variant.op_Implicit(NMainMenuTextButton._hoverScale), 0.05);
    this._tween.TweenProperty((GodotObject) this.label, NodePath.op_Implicit("self_modulate"), Variant.op_Implicit(this._hoveredColor), 0.05);
  }

  protected override void OnUnfocus()
  {
    base.OnUnfocus();
    this.AnimUnhover();
  }

  private void AnimUnhover()
  {
    this._tween?.Kill();
    if (this.label == null)
      return;
    ((CanvasItem) this.label).SelfModulate = this._defaultColor;
    this._tween = ((Node) this).CreateTween();
    this._tween.TweenProperty((GodotObject) this.label, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  private void AnimPressDown()
  {
    this._tween?.Kill();
    if (this.label == null)
      return;
    ((CanvasItem) this.label).SelfModulate = this._hoveredColor;
    ((Control) this.label).Scale = NMainMenuTextButton._hoverScale;
    this._tween = ((Node) this).CreateTween();
    this._tween.TweenProperty((GodotObject) this.label, NodePath.op_Implicit("scale"), Variant.op_Implicit(NMainMenuTextButton._downScale), 0.2).SetTrans((Tween.TransitionType) 7L).SetEase((Tween.EaseType) 1L);
    this._tween.TweenProperty((GodotObject) this.label, NodePath.op_Implicit("self_modulate"), Variant.op_Implicit(this._downColor), 0.2).SetTrans((Tween.TransitionType) 7L).SetEase((Tween.EaseType) 1L);
  }

  private void AnimRelease()
  {
    this._tween?.Kill();
    if (this.label == null)
      return;
    this._tween = ((Node) this).CreateTween();
    this._tween.TweenProperty((GodotObject) this.label, NodePath.op_Implicit("scale"), Variant.op_Implicit(this.IsFocused ? NMainMenuTextButton._hoverScale : Vector2.One), 0.2).SetTrans((Tween.TransitionType) 7L).SetEase((Tween.EaseType) 1L);
    this._tween.TweenProperty((GodotObject) this.label, NodePath.op_Implicit("self_modulate"), Variant.op_Implicit(this._defaultColor), 0.2).SetTrans((Tween.TransitionType) 7L).SetEase((Tween.EaseType) 1L);
  }

  protected override void OnDisable()
  {
    if (this.label == null)
      return;
    ((CanvasItem) this.label).Modulate = StsColors.quarterTransparentWhite;
  }

  protected override void OnEnable()
  {
    if (this.label == null)
      return;
    ((CanvasItem) this.label).Modulate = Colors.White;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(14)
    {
      new MethodInfo(NMainMenuTextButton.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMainMenuTextButton.MethodName.ConnectSignals, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMainMenuTextButton.MethodName.SetLocalization, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("locKey"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMainMenuTextButton.MethodName._Notification, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("what"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMainMenuTextButton.MethodName.RefreshLabel, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMainMenuTextButton.MethodName.OnPress, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMainMenuTextButton.MethodName.OnRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMainMenuTextButton.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMainMenuTextButton.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMainMenuTextButton.MethodName.AnimUnhover, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMainMenuTextButton.MethodName.AnimPressDown, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMainMenuTextButton.MethodName.AnimRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMainMenuTextButton.MethodName.OnDisable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMainMenuTextButton.MethodName.OnEnable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMainMenuTextButton.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMainMenuTextButton.MethodName.ConnectSignals) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ConnectSignals();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMainMenuTextButton.MethodName.SetLocalization) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetLocalization(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMainMenuTextButton.MethodName._Notification) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((GodotObject) this)._Notification(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMainMenuTextButton.MethodName.RefreshLabel) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshLabel();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMainMenuTextButton.MethodName.OnPress) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPress();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMainMenuTextButton.MethodName.OnRelease) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnRelease();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMainMenuTextButton.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMainMenuTextButton.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMainMenuTextButton.MethodName.AnimUnhover) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimUnhover();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMainMenuTextButton.MethodName.AnimPressDown) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimPressDown();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMainMenuTextButton.MethodName.AnimRelease) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimRelease();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMainMenuTextButton.MethodName.OnDisable) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnDisable();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NMainMenuTextButton.MethodName.OnEnable) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnEnable();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMainMenuTextButton.MethodName._Ready) || StringName.op_Equality(ref method, NMainMenuTextButton.MethodName.ConnectSignals) || StringName.op_Equality(ref method, NMainMenuTextButton.MethodName.SetLocalization) || StringName.op_Equality(ref method, NMainMenuTextButton.MethodName._Notification) || StringName.op_Equality(ref method, NMainMenuTextButton.MethodName.RefreshLabel) || StringName.op_Equality(ref method, NMainMenuTextButton.MethodName.OnPress) || StringName.op_Equality(ref method, NMainMenuTextButton.MethodName.OnRelease) || StringName.op_Equality(ref method, NMainMenuTextButton.MethodName.OnFocus) || StringName.op_Equality(ref method, NMainMenuTextButton.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NMainMenuTextButton.MethodName.AnimUnhover) || StringName.op_Equality(ref method, NMainMenuTextButton.MethodName.AnimPressDown) || StringName.op_Equality(ref method, NMainMenuTextButton.MethodName.AnimRelease) || StringName.op_Equality(ref method, NMainMenuTextButton.MethodName.OnDisable) || StringName.op_Equality(ref method, NMainMenuTextButton.MethodName.OnEnable) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMainMenuTextButton.PropertyName.label))
    {
      this.label = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuTextButton.PropertyName._defaultColor))
    {
      this._defaultColor = VariantUtils.ConvertTo<Color>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuTextButton.PropertyName._hoveredColor))
    {
      this._hoveredColor = VariantUtils.ConvertTo<Color>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuTextButton.PropertyName._downColor))
    {
      this._downColor = VariantUtils.ConvertTo<Color>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMainMenuTextButton.PropertyName._tween))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._tween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMainMenuTextButton.PropertyName.label))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this.label);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuTextButton.PropertyName._defaultColor))
    {
      value = VariantUtils.CreateFrom<Color>(ref this._defaultColor);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuTextButton.PropertyName._hoveredColor))
    {
      value = VariantUtils.CreateFrom<Color>(ref this._hoveredColor);
      return true;
    }
    if (StringName.op_Equality(ref name, NMainMenuTextButton.PropertyName._downColor))
    {
      value = VariantUtils.CreateFrom<Color>(ref this._downColor);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMainMenuTextButton.PropertyName._tween))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Tween>(ref this._tween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NMainMenuTextButton.PropertyName.label, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 20L, NMainMenuTextButton.PropertyName._defaultColor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 20L, NMainMenuTextButton.PropertyName._hoveredColor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 20L, NMainMenuTextButton.PropertyName._downColor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMainMenuTextButton.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NMainMenuTextButton.PropertyName.label, Variant.From<MegaLabel>(ref this.label));
    info.AddProperty(NMainMenuTextButton.PropertyName._defaultColor, Variant.From<Color>(ref this._defaultColor));
    info.AddProperty(NMainMenuTextButton.PropertyName._hoveredColor, Variant.From<Color>(ref this._hoveredColor));
    info.AddProperty(NMainMenuTextButton.PropertyName._downColor, Variant.From<Color>(ref this._downColor));
    info.AddProperty(NMainMenuTextButton.PropertyName._tween, Variant.From<Tween>(ref this._tween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NMainMenuTextButton.PropertyName.label, ref variant1))
      this.label = ((Variant) ref variant1).As<MegaLabel>();
    Variant variant2;
    if (info.TryGetProperty(NMainMenuTextButton.PropertyName._defaultColor, ref variant2))
      this._defaultColor = ((Variant) ref variant2).As<Color>();
    Variant variant3;
    if (info.TryGetProperty(NMainMenuTextButton.PropertyName._hoveredColor, ref variant3))
      this._hoveredColor = ((Variant) ref variant3).As<Color>();
    Variant variant4;
    if (info.TryGetProperty(NMainMenuTextButton.PropertyName._downColor, ref variant4))
      this._downColor = ((Variant) ref variant4).As<Color>();
    Variant variant5;
    if (!info.TryGetProperty(NMainMenuTextButton.PropertyName._tween, ref variant5))
      return;
    this._tween = ((Variant) ref variant5).As<Tween>();
  }

  public new class MethodName : NButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName ConnectSignals = StringName.op_Implicit(nameof (ConnectSignals));
    public static readonly StringName SetLocalization = StringName.op_Implicit(nameof (SetLocalization));
    public static readonly StringName _Notification = StringName.op_Implicit(nameof (_Notification));
    public static readonly StringName RefreshLabel = StringName.op_Implicit(nameof (RefreshLabel));
    public new static readonly StringName OnPress = StringName.op_Implicit(nameof (OnPress));
    public new static readonly StringName OnRelease = StringName.op_Implicit(nameof (OnRelease));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public static readonly StringName AnimUnhover = StringName.op_Implicit(nameof (AnimUnhover));
    public static readonly StringName AnimPressDown = StringName.op_Implicit(nameof (AnimPressDown));
    public static readonly StringName AnimRelease = StringName.op_Implicit(nameof (AnimRelease));
    public new static readonly StringName OnDisable = StringName.op_Implicit(nameof (OnDisable));
    public new static readonly StringName OnEnable = StringName.op_Implicit(nameof (OnEnable));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public static readonly StringName label = StringName.op_Implicit(nameof (label));
    public static readonly StringName _defaultColor = StringName.op_Implicit(nameof (_defaultColor));
    public static readonly StringName _hoveredColor = StringName.op_Implicit(nameof (_hoveredColor));
    public static readonly StringName _downColor = StringName.op_Implicit(nameof (_downColor));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
