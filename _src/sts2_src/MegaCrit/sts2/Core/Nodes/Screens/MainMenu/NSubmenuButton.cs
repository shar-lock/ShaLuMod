// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NSubmenuButton
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.Fonts;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;

[ScriptPath("res://src/Core/Nodes/Screens/MainMenu/NSubmenuButton.cs")]
public class NSubmenuButton : NButton
{
  private static readonly StringName _v = new StringName("v");
  private static readonly StringName _s = new StringName("s");
  private ShaderMaterial _hsv;
  private Control _bgPanel;
  private TextureRect _icon;
  private MegaLabel _title;
  private MegaRichTextLabel _description;
  private string? _locKeyPrefix;
  private float _defaultV;
  private const float _hoverV = 1f;
  private Tween? _scaleTween;
  private static readonly Vector2 _hoverScale = Vector2.op_Multiply(Vector2.One, 1.025f);

  public override void _Ready()
  {
    if (((object) this).GetType() != typeof (NSubmenuButton))
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
    this._defaultV = Variant.op_Explicit(this._hsv.GetShaderParameter(NSubmenuButton._v));
  }

  public void SetIconAndLocalization(string locKeyPrefix)
  {
    this._locKeyPrefix = locKeyPrefix;
    this.RefreshLabels();
  }

  public override void _Notification(int what)
  {
    if (what != 2010 || this._locKeyPrefix == null || !((Node) this).IsNodeReady())
      return;
    this.RefreshLabels();
  }

  public void RefreshLabels()
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
        Log.Warn($"Submenu button {((Node) this).Name} tried to find locked description for {this._locKeyPrefix} but couldn't");
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
    ((CanvasItem) ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%Lock"))).Visible = false;
    this._hsv.SetShaderParameter(NSubmenuButton._s, Variant.op_Implicit(1f));
    ((CanvasItem) this._icon).Modulate = Colors.White;
  }

  protected override void OnDisable()
  {
    ((CanvasItem) this).Modulate = Colors.DarkGray;
    ((CanvasItem) ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%Lock"))).Visible = true;
    this._hsv.SetShaderParameter(NSubmenuButton._s, Variant.op_Implicit(0.0f));
    ((CanvasItem) this._icon).Modulate = new Color(0.5f, 0.5f, 0.5f, 0.5f);
  }

  protected override void OnFocus()
  {
    base.OnFocus();
    this._scaleTween?.Kill();
    this.Scale = NSubmenuButton._hoverScale;
    this._hsv.SetShaderParameter(NSubmenuButton._v, Variant.op_Implicit(1f));
  }

  protected override void OnUnfocus()
  {
    this._scaleTween?.Kill();
    this._scaleTween = ((Node) this).CreateTween().SetParallel(true);
    this._scaleTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    this._scaleTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderParam)), Variant.op_Implicit(1f), Variant.op_Implicit(this._defaultV), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  private void UpdateShaderParam(float newV)
  {
    this._hsv.SetShaderParameter(NSubmenuButton._v, Variant.op_Implicit(newV));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(10)
    {
      new MethodInfo(NSubmenuButton.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSubmenuButton.MethodName.ConnectSignals, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSubmenuButton.MethodName.SetIconAndLocalization, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("locKeyPrefix"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NSubmenuButton.MethodName._Notification, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("what"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NSubmenuButton.MethodName.RefreshLabels, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSubmenuButton.MethodName.OnEnable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSubmenuButton.MethodName.OnDisable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSubmenuButton.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSubmenuButton.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSubmenuButton.MethodName.UpdateShaderParam, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
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
    if (StringName.op_Equality(ref method, NSubmenuButton.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSubmenuButton.MethodName.ConnectSignals) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ConnectSignals();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSubmenuButton.MethodName.SetIconAndLocalization) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetIconAndLocalization(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSubmenuButton.MethodName._Notification) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((GodotObject) this)._Notification(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSubmenuButton.MethodName.RefreshLabels) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshLabels();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSubmenuButton.MethodName.OnEnable) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnEnable();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSubmenuButton.MethodName.OnDisable) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnDisable();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSubmenuButton.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSubmenuButton.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NSubmenuButton.MethodName.UpdateShaderParam) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.UpdateShaderParam(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NSubmenuButton.MethodName._Ready) || StringName.op_Equality(ref method, NSubmenuButton.MethodName.ConnectSignals) || StringName.op_Equality(ref method, NSubmenuButton.MethodName.SetIconAndLocalization) || StringName.op_Equality(ref method, NSubmenuButton.MethodName._Notification) || StringName.op_Equality(ref method, NSubmenuButton.MethodName.RefreshLabels) || StringName.op_Equality(ref method, NSubmenuButton.MethodName.OnEnable) || StringName.op_Equality(ref method, NSubmenuButton.MethodName.OnDisable) || StringName.op_Equality(ref method, NSubmenuButton.MethodName.OnFocus) || StringName.op_Equality(ref method, NSubmenuButton.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NSubmenuButton.MethodName.UpdateShaderParam) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSubmenuButton.PropertyName._hsv))
    {
      this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSubmenuButton.PropertyName._bgPanel))
    {
      this._bgPanel = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSubmenuButton.PropertyName._icon))
    {
      this._icon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSubmenuButton.PropertyName._title))
    {
      this._title = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSubmenuButton.PropertyName._description))
    {
      this._description = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSubmenuButton.PropertyName._locKeyPrefix))
    {
      this._locKeyPrefix = VariantUtils.ConvertTo<string>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSubmenuButton.PropertyName._defaultV))
    {
      this._defaultV = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSubmenuButton.PropertyName._scaleTween))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._scaleTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSubmenuButton.PropertyName._hsv))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
      return true;
    }
    if (StringName.op_Equality(ref name, NSubmenuButton.PropertyName._bgPanel))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._bgPanel);
      return true;
    }
    if (StringName.op_Equality(ref name, NSubmenuButton.PropertyName._icon))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._icon);
      return true;
    }
    if (StringName.op_Equality(ref name, NSubmenuButton.PropertyName._title))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._title);
      return true;
    }
    if (StringName.op_Equality(ref name, NSubmenuButton.PropertyName._description))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._description);
      return true;
    }
    if (StringName.op_Equality(ref name, NSubmenuButton.PropertyName._locKeyPrefix))
    {
      value = VariantUtils.CreateFrom<string>(ref this._locKeyPrefix);
      return true;
    }
    if (StringName.op_Equality(ref name, NSubmenuButton.PropertyName._defaultV))
    {
      value = VariantUtils.CreateFrom<float>(ref this._defaultV);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSubmenuButton.PropertyName._scaleTween))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Tween>(ref this._scaleTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NSubmenuButton.PropertyName._hsv, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSubmenuButton.PropertyName._bgPanel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSubmenuButton.PropertyName._icon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSubmenuButton.PropertyName._title, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSubmenuButton.PropertyName._description, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 4L, NSubmenuButton.PropertyName._locKeyPrefix, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NSubmenuButton.PropertyName._defaultV, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSubmenuButton.PropertyName._scaleTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NSubmenuButton.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
    info.AddProperty(NSubmenuButton.PropertyName._bgPanel, Variant.From<Control>(ref this._bgPanel));
    info.AddProperty(NSubmenuButton.PropertyName._icon, Variant.From<TextureRect>(ref this._icon));
    info.AddProperty(NSubmenuButton.PropertyName._title, Variant.From<MegaLabel>(ref this._title));
    info.AddProperty(NSubmenuButton.PropertyName._description, Variant.From<MegaRichTextLabel>(ref this._description));
    info.AddProperty(NSubmenuButton.PropertyName._locKeyPrefix, Variant.From<string>(ref this._locKeyPrefix));
    info.AddProperty(NSubmenuButton.PropertyName._defaultV, Variant.From<float>(ref this._defaultV));
    info.AddProperty(NSubmenuButton.PropertyName._scaleTween, Variant.From<Tween>(ref this._scaleTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NSubmenuButton.PropertyName._hsv, ref variant1))
      this._hsv = ((Variant) ref variant1).As<ShaderMaterial>();
    Variant variant2;
    if (info.TryGetProperty(NSubmenuButton.PropertyName._bgPanel, ref variant2))
      this._bgPanel = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NSubmenuButton.PropertyName._icon, ref variant3))
      this._icon = ((Variant) ref variant3).As<TextureRect>();
    Variant variant4;
    if (info.TryGetProperty(NSubmenuButton.PropertyName._title, ref variant4))
      this._title = ((Variant) ref variant4).As<MegaLabel>();
    Variant variant5;
    if (info.TryGetProperty(NSubmenuButton.PropertyName._description, ref variant5))
      this._description = ((Variant) ref variant5).As<MegaRichTextLabel>();
    Variant variant6;
    if (info.TryGetProperty(NSubmenuButton.PropertyName._locKeyPrefix, ref variant6))
      this._locKeyPrefix = ((Variant) ref variant6).As<string>();
    Variant variant7;
    if (info.TryGetProperty(NSubmenuButton.PropertyName._defaultV, ref variant7))
      this._defaultV = ((Variant) ref variant7).As<float>();
    Variant variant8;
    if (!info.TryGetProperty(NSubmenuButton.PropertyName._scaleTween, ref variant8))
      return;
    this._scaleTween = ((Variant) ref variant8).As<Tween>();
  }

  public new class MethodName : NButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName ConnectSignals = StringName.op_Implicit(nameof (ConnectSignals));
    public static readonly StringName SetIconAndLocalization = StringName.op_Implicit(nameof (SetIconAndLocalization));
    public static readonly StringName _Notification = StringName.op_Implicit(nameof (_Notification));
    public static readonly StringName RefreshLabels = StringName.op_Implicit(nameof (RefreshLabels));
    public new static readonly StringName OnEnable = StringName.op_Implicit(nameof (OnEnable));
    public new static readonly StringName OnDisable = StringName.op_Implicit(nameof (OnDisable));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public static readonly StringName UpdateShaderParam = StringName.op_Implicit(nameof (UpdateShaderParam));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public static readonly StringName _hsv = StringName.op_Implicit(nameof (_hsv));
    public static readonly StringName _bgPanel = StringName.op_Implicit(nameof (_bgPanel));
    public static readonly StringName _icon = StringName.op_Implicit(nameof (_icon));
    public static readonly StringName _title = StringName.op_Implicit(nameof (_title));
    public static readonly StringName _description = StringName.op_Implicit(nameof (_description));
    public static readonly StringName _locKeyPrefix = StringName.op_Implicit(nameof (_locKeyPrefix));
    public static readonly StringName _defaultV = StringName.op_Implicit(nameof (_defaultV));
    public static readonly StringName _scaleTween = StringName.op_Implicit(nameof (_scaleTween));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
