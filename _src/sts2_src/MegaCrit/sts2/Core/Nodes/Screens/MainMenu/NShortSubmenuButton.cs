// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NShortSubmenuButton
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.Fonts;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;

[ScriptPath("res://src/Core/Nodes/Screens/MainMenu/NShortSubmenuButton.cs")]
public class NShortSubmenuButton : NButton
{
  private Control _bgPanel;
  private TextureRect _icon;
  private MegaLabel _title;
  private MegaRichTextLabel _description;
  private string? _locKeyPrefix;
  private ShaderMaterial _hsv;
  private static readonly StringName _v = new StringName("v");
  private static readonly StringName _s = new StringName("s");
  private float _defaultV;
  private float _focusV;
  private float _pressV;
  private Tween? _tween;

  public static string? GetImagePath(string key)
  {
    string imagePath = ImageHelper.GetImagePath($"packed/main_menu/submenu_icon_{key.ToLowerInvariant()}.png");
    return ResourceLoader.Exists(imagePath, "") ? imagePath : (string) null;
  }

  public override void _Ready()
  {
    if (((object) this).GetType() != typeof (NShortSubmenuButton))
      throw new InvalidOperationException("Don't call base._Ready(). Use ConnectSignals() instead.");
    this.ConnectSignals();
  }

  protected override void ConnectSignals()
  {
    base.ConnectSignals();
    this._bgPanel = ((Node) this).GetNode<Control>(NodePath.op_Implicit("BgPanel"));
    this._hsv = (ShaderMaterial) ((CanvasItem) this._bgPanel).Material;
    this._icon = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("Icon"));
    this._title = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%Title"));
    this._description = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%Description"));
    this._defaultV = Variant.op_Explicit(this._hsv.GetShaderParameter(NShortSubmenuButton._v));
    this._focusV = this._defaultV + 0.2f;
    this._pressV = this._defaultV - 0.2f;
  }

  public void SetIconAndLocalization(string locKeyPrefix)
  {
    this._locKeyPrefix = locKeyPrefix;
    this.RefreshLabels();
    string imagePath = NShortSubmenuButton.GetImagePath(locKeyPrefix);
    if (imagePath == null)
      return;
    this._icon.Texture = (Texture2D) PreloadManager.Cache.GetCompressedTexture2D(imagePath);
  }

  public override void _Notification(int what)
  {
    if (what != 2010 || this._locKeyPrefix == null || !((Node) this).IsNodeReady())
      return;
    this.RefreshLabels();
  }

  private void RefreshLabels()
  {
    this._title.SetTextAutoSize(new LocString("main_menu_ui", this._locKeyPrefix + ".title").GetFormattedText());
    ((Control) this._title).ApplyLocaleFontSubstitution(FontType.Regular, ThemeConstants.Label.Font);
    LocString locString;
    if (this.IsEnabled)
    {
      locString = new LocString("main_menu_ui", this._locKeyPrefix + ".description");
    }
    else
    {
      locString = new LocString("main_menu_ui", this._locKeyPrefix + ".LOCKED.description");
      if (!locString.Exists())
      {
        Log.Warn($"Short submenu button {((Node) this).Name} tried to find locked description for {this._locKeyPrefix} but couldn't");
        locString = new LocString("main_menu_ui", this._locKeyPrefix + ".description");
      }
    }
    this._description.Text = locString.GetFormattedText();
    ((Control) this._description).ApplyLocaleFontSubstitution(FontType.Regular, ThemeConstants.RichTextLabel.NormalFont);
    ((Control) this._description).ApplyLocaleFontSubstitution(FontType.Bold, ThemeConstants.RichTextLabel.BoldFont);
    ((Control) this._description).ApplyLocaleFontSubstitution(FontType.Italic, ThemeConstants.RichTextLabel.ItalicsFont);
  }

  protected override void OnEnable()
  {
    ((CanvasItem) this).Modulate = Colors.White;
    ((CanvasItem) ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Lock"))).Visible = false;
    this._hsv.SetShaderParameter(NShortSubmenuButton._s, Variant.op_Implicit(1f));
    ((CanvasItem) this._icon).Modulate = Colors.White;
  }

  protected override void OnDisable()
  {
    ((CanvasItem) this).Modulate = Colors.DarkGray;
    ((CanvasItem) ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Lock"))).Visible = true;
    this._hsv.SetShaderParameter(NShortSubmenuButton._s, Variant.op_Implicit(0.0f));
    ((CanvasItem) this._icon).Modulate = new Color(0.5f, 0.5f, 0.5f, 0.5f);
  }

  protected override void OnFocus()
  {
    base.OnFocus();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 1.02f)), 0.05);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderParam)), this._hsv.GetShaderParameter(NShortSubmenuButton._v), Variant.op_Implicit(this._focusV), 0.05);
  }

  protected override void OnUnfocus()
  {
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.3).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderParam)), this._hsv.GetShaderParameter(NShortSubmenuButton._v), Variant.op_Implicit(this._defaultV), 0.3);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.3);
  }

  protected override void OnPress()
  {
    base.OnPress();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 0.98f)), 0.2).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate"), Variant.op_Implicit(StsColors.lightGray), 0.2);
  }

  protected override void OnRelease() => this.OnUnfocus();

  private void UpdateShaderParam(float newV)
  {
    this._hsv.SetShaderParameter(NShortSubmenuButton._v, Variant.op_Implicit(newV));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(13)
    {
      new MethodInfo(NShortSubmenuButton.MethodName.GetImagePath, new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("key"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NShortSubmenuButton.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NShortSubmenuButton.MethodName.ConnectSignals, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NShortSubmenuButton.MethodName.SetIconAndLocalization, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("locKeyPrefix"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NShortSubmenuButton.MethodName._Notification, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("what"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NShortSubmenuButton.MethodName.RefreshLabels, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NShortSubmenuButton.MethodName.OnEnable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NShortSubmenuButton.MethodName.OnDisable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NShortSubmenuButton.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NShortSubmenuButton.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NShortSubmenuButton.MethodName.OnPress, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NShortSubmenuButton.MethodName.OnRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NShortSubmenuButton.MethodName.UpdateShaderParam, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
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
    if (StringName.op_Equality(ref method, NShortSubmenuButton.MethodName.GetImagePath) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      string imagePath = NShortSubmenuButton.GetImagePath(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<string>(ref imagePath);
      return true;
    }
    if (StringName.op_Equality(ref method, NShortSubmenuButton.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NShortSubmenuButton.MethodName.ConnectSignals) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ConnectSignals();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NShortSubmenuButton.MethodName.SetIconAndLocalization) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetIconAndLocalization(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NShortSubmenuButton.MethodName._Notification) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((GodotObject) this)._Notification(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NShortSubmenuButton.MethodName.RefreshLabels) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshLabels();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NShortSubmenuButton.MethodName.OnEnable) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnEnable();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NShortSubmenuButton.MethodName.OnDisable) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnDisable();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NShortSubmenuButton.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NShortSubmenuButton.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NShortSubmenuButton.MethodName.OnPress) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPress();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NShortSubmenuButton.MethodName.OnRelease) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnRelease();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NShortSubmenuButton.MethodName.UpdateShaderParam) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.UpdateShaderParam(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NShortSubmenuButton.MethodName.GetImagePath) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      string imagePath = NShortSubmenuButton.GetImagePath(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<string>(ref imagePath);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NShortSubmenuButton.MethodName.GetImagePath) || StringName.op_Equality(ref method, NShortSubmenuButton.MethodName._Ready) || StringName.op_Equality(ref method, NShortSubmenuButton.MethodName.ConnectSignals) || StringName.op_Equality(ref method, NShortSubmenuButton.MethodName.SetIconAndLocalization) || StringName.op_Equality(ref method, NShortSubmenuButton.MethodName._Notification) || StringName.op_Equality(ref method, NShortSubmenuButton.MethodName.RefreshLabels) || StringName.op_Equality(ref method, NShortSubmenuButton.MethodName.OnEnable) || StringName.op_Equality(ref method, NShortSubmenuButton.MethodName.OnDisable) || StringName.op_Equality(ref method, NShortSubmenuButton.MethodName.OnFocus) || StringName.op_Equality(ref method, NShortSubmenuButton.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NShortSubmenuButton.MethodName.OnPress) || StringName.op_Equality(ref method, NShortSubmenuButton.MethodName.OnRelease) || StringName.op_Equality(ref method, NShortSubmenuButton.MethodName.UpdateShaderParam) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NShortSubmenuButton.PropertyName._bgPanel))
    {
      this._bgPanel = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NShortSubmenuButton.PropertyName._icon))
    {
      this._icon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NShortSubmenuButton.PropertyName._title))
    {
      this._title = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NShortSubmenuButton.PropertyName._description))
    {
      this._description = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NShortSubmenuButton.PropertyName._locKeyPrefix))
    {
      this._locKeyPrefix = VariantUtils.ConvertTo<string>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NShortSubmenuButton.PropertyName._hsv))
    {
      this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NShortSubmenuButton.PropertyName._defaultV))
    {
      this._defaultV = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NShortSubmenuButton.PropertyName._focusV))
    {
      this._focusV = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NShortSubmenuButton.PropertyName._pressV))
    {
      this._pressV = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NShortSubmenuButton.PropertyName._tween))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._tween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NShortSubmenuButton.PropertyName._bgPanel))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._bgPanel);
      return true;
    }
    if (StringName.op_Equality(ref name, NShortSubmenuButton.PropertyName._icon))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._icon);
      return true;
    }
    if (StringName.op_Equality(ref name, NShortSubmenuButton.PropertyName._title))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._title);
      return true;
    }
    if (StringName.op_Equality(ref name, NShortSubmenuButton.PropertyName._description))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._description);
      return true;
    }
    if (StringName.op_Equality(ref name, NShortSubmenuButton.PropertyName._locKeyPrefix))
    {
      value = VariantUtils.CreateFrom<string>(ref this._locKeyPrefix);
      return true;
    }
    if (StringName.op_Equality(ref name, NShortSubmenuButton.PropertyName._hsv))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
      return true;
    }
    if (StringName.op_Equality(ref name, NShortSubmenuButton.PropertyName._defaultV))
    {
      value = VariantUtils.CreateFrom<float>(ref this._defaultV);
      return true;
    }
    if (StringName.op_Equality(ref name, NShortSubmenuButton.PropertyName._focusV))
    {
      value = VariantUtils.CreateFrom<float>(ref this._focusV);
      return true;
    }
    if (StringName.op_Equality(ref name, NShortSubmenuButton.PropertyName._pressV))
    {
      value = VariantUtils.CreateFrom<float>(ref this._pressV);
      return true;
    }
    if (!StringName.op_Equality(ref name, NShortSubmenuButton.PropertyName._tween))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Tween>(ref this._tween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NShortSubmenuButton.PropertyName._bgPanel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NShortSubmenuButton.PropertyName._icon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NShortSubmenuButton.PropertyName._title, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NShortSubmenuButton.PropertyName._description, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 4L, NShortSubmenuButton.PropertyName._locKeyPrefix, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NShortSubmenuButton.PropertyName._hsv, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NShortSubmenuButton.PropertyName._defaultV, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NShortSubmenuButton.PropertyName._focusV, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NShortSubmenuButton.PropertyName._pressV, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NShortSubmenuButton.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NShortSubmenuButton.PropertyName._bgPanel, Variant.From<Control>(ref this._bgPanel));
    info.AddProperty(NShortSubmenuButton.PropertyName._icon, Variant.From<TextureRect>(ref this._icon));
    info.AddProperty(NShortSubmenuButton.PropertyName._title, Variant.From<MegaLabel>(ref this._title));
    info.AddProperty(NShortSubmenuButton.PropertyName._description, Variant.From<MegaRichTextLabel>(ref this._description));
    info.AddProperty(NShortSubmenuButton.PropertyName._locKeyPrefix, Variant.From<string>(ref this._locKeyPrefix));
    info.AddProperty(NShortSubmenuButton.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
    info.AddProperty(NShortSubmenuButton.PropertyName._defaultV, Variant.From<float>(ref this._defaultV));
    info.AddProperty(NShortSubmenuButton.PropertyName._focusV, Variant.From<float>(ref this._focusV));
    info.AddProperty(NShortSubmenuButton.PropertyName._pressV, Variant.From<float>(ref this._pressV));
    info.AddProperty(NShortSubmenuButton.PropertyName._tween, Variant.From<Tween>(ref this._tween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NShortSubmenuButton.PropertyName._bgPanel, ref variant1))
      this._bgPanel = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NShortSubmenuButton.PropertyName._icon, ref variant2))
      this._icon = ((Variant) ref variant2).As<TextureRect>();
    Variant variant3;
    if (info.TryGetProperty(NShortSubmenuButton.PropertyName._title, ref variant3))
      this._title = ((Variant) ref variant3).As<MegaLabel>();
    Variant variant4;
    if (info.TryGetProperty(NShortSubmenuButton.PropertyName._description, ref variant4))
      this._description = ((Variant) ref variant4).As<MegaRichTextLabel>();
    Variant variant5;
    if (info.TryGetProperty(NShortSubmenuButton.PropertyName._locKeyPrefix, ref variant5))
      this._locKeyPrefix = ((Variant) ref variant5).As<string>();
    Variant variant6;
    if (info.TryGetProperty(NShortSubmenuButton.PropertyName._hsv, ref variant6))
      this._hsv = ((Variant) ref variant6).As<ShaderMaterial>();
    Variant variant7;
    if (info.TryGetProperty(NShortSubmenuButton.PropertyName._defaultV, ref variant7))
      this._defaultV = ((Variant) ref variant7).As<float>();
    Variant variant8;
    if (info.TryGetProperty(NShortSubmenuButton.PropertyName._focusV, ref variant8))
      this._focusV = ((Variant) ref variant8).As<float>();
    Variant variant9;
    if (info.TryGetProperty(NShortSubmenuButton.PropertyName._pressV, ref variant9))
      this._pressV = ((Variant) ref variant9).As<float>();
    Variant variant10;
    if (!info.TryGetProperty(NShortSubmenuButton.PropertyName._tween, ref variant10))
      return;
    this._tween = ((Variant) ref variant10).As<Tween>();
  }

  public new class MethodName : NButton.MethodName
  {
    public static readonly StringName GetImagePath = StringName.op_Implicit(nameof (GetImagePath));
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName ConnectSignals = StringName.op_Implicit(nameof (ConnectSignals));
    public static readonly StringName SetIconAndLocalization = StringName.op_Implicit(nameof (SetIconAndLocalization));
    public static readonly StringName _Notification = StringName.op_Implicit(nameof (_Notification));
    public static readonly StringName RefreshLabels = StringName.op_Implicit(nameof (RefreshLabels));
    public new static readonly StringName OnEnable = StringName.op_Implicit(nameof (OnEnable));
    public new static readonly StringName OnDisable = StringName.op_Implicit(nameof (OnDisable));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public new static readonly StringName OnPress = StringName.op_Implicit(nameof (OnPress));
    public new static readonly StringName OnRelease = StringName.op_Implicit(nameof (OnRelease));
    public static readonly StringName UpdateShaderParam = StringName.op_Implicit(nameof (UpdateShaderParam));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public static readonly StringName _bgPanel = StringName.op_Implicit(nameof (_bgPanel));
    public static readonly StringName _icon = StringName.op_Implicit(nameof (_icon));
    public static readonly StringName _title = StringName.op_Implicit(nameof (_title));
    public static readonly StringName _description = StringName.op_Implicit(nameof (_description));
    public static readonly StringName _locKeyPrefix = StringName.op_Implicit(nameof (_locKeyPrefix));
    public static readonly StringName _hsv = StringName.op_Implicit(nameof (_hsv));
    public static readonly StringName _defaultV = StringName.op_Implicit(nameof (_defaultV));
    public static readonly StringName _focusV = StringName.op_Implicit(nameof (_focusV));
    public static readonly StringName _pressV = StringName.op_Implicit(nameof (_pressV));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
