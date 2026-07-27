// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Settings.NLanguageButton
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Saves;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings;

[ScriptPath("res://src/Core/Nodes/Screens/Settings/NLanguageButton.cs")]
public class NLanguageButton : NButton
{
  private TextureRect _image;
  private Control _outline;
  private TextureRect _flag;
  private MegaLabel _label;
  private Tween? _tween;
  public string isoCode;

  public bool IsSelected { get; private set; }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._image = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%ButtonImage"));
    this._outline = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Outline"));
    this._flag = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%FlagImage"));
    this._label = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%Label"));
  }

  public void Init(string languageIsoCode)
  {
    Log.Info("Adding Language: " + languageIsoCode);
    this.isoCode = languageIsoCode;
    this._label.SetTextAutoSize(new LocString("settings_ui", "LANGUAGE_" + languageIsoCode.ToUpperInvariant()).GetRawText());
    this._flag.Texture = ResourceLoader.Load<Texture2D>($"res://images/ui/settings_screen/flag_{languageIsoCode}.png", (string) null, (ResourceLoader.CacheMode) 1L);
    this.IsSelected = SaveManager.Instance.SettingsSave.Language == this.isoCode;
    ((CanvasItem) this._outline).Visible = this.IsSelected;
  }

  protected override void OnRelease()
  {
    base.OnRelease();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._image, NodePath.op_Implicit("self_modulate"), Variant.op_Implicit(new Color("26373d")), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenProperty((GodotObject) this._image, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.05);
    this._tween.TweenProperty((GodotObject) this._image, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("modulate"), Variant.op_Implicit(StsColors.cream), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  protected override void OnFocus()
  {
    base.OnFocus();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._image, NodePath.op_Implicit("self_modulate"), Variant.op_Implicit(new Color("235161")), 0.03);
    this._tween.TweenProperty((GodotObject) this._image, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.05);
    this._tween.TweenProperty((GodotObject) this._image, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 1.05f)), 0.03);
    this._tween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("modulate"), Variant.op_Implicit(StsColors.gold), 0.03);
  }

  protected override void OnUnfocus()
  {
    base.OnUnfocus();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._image, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.5);
    this._tween.TweenProperty((GodotObject) this._image, NodePath.op_Implicit("self_modulate"), Variant.op_Implicit(new Color("26373d")), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenProperty((GodotObject) this._image, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenProperty((GodotObject) this._label, NodePath.op_Implicit("modulate"), Variant.op_Implicit(StsColors.cream), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  protected override void OnPress()
  {
    base.OnPress();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._image, NodePath.op_Implicit("self_modulate"), Variant.op_Implicit(new Color("26373d")), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenProperty((GodotObject) this._image, NodePath.op_Implicit("modulate"), Variant.op_Implicit(StsColors.lightGray), 0.05);
    this._tween.TweenProperty((GodotObject) this._image, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 0.95f)), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  public void SetAsSelected()
  {
    this.IsSelected = true;
    ((CanvasItem) this._outline).Visible = true;
  }

  public void SetAsDeselected()
  {
    this.IsSelected = false;
    ((CanvasItem) this._outline).Visible = false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(8)
    {
      new MethodInfo(NLanguageButton.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NLanguageButton.MethodName.Init, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("languageIsoCode"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NLanguageButton.MethodName.OnRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NLanguageButton.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NLanguageButton.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NLanguageButton.MethodName.OnPress, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NLanguageButton.MethodName.SetAsSelected, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NLanguageButton.MethodName.SetAsDeselected, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NLanguageButton.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NLanguageButton.MethodName.Init) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.Init(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NLanguageButton.MethodName.OnRelease) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnRelease();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NLanguageButton.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NLanguageButton.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NLanguageButton.MethodName.OnPress) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPress();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NLanguageButton.MethodName.SetAsSelected) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SetAsSelected();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NLanguageButton.MethodName.SetAsDeselected) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.SetAsDeselected();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NLanguageButton.MethodName._Ready) || StringName.op_Equality(ref method, NLanguageButton.MethodName.Init) || StringName.op_Equality(ref method, NLanguageButton.MethodName.OnRelease) || StringName.op_Equality(ref method, NLanguageButton.MethodName.OnFocus) || StringName.op_Equality(ref method, NLanguageButton.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NLanguageButton.MethodName.OnPress) || StringName.op_Equality(ref method, NLanguageButton.MethodName.SetAsSelected) || StringName.op_Equality(ref method, NLanguageButton.MethodName.SetAsDeselected) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NLanguageButton.PropertyName.IsSelected))
    {
      this.IsSelected = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NLanguageButton.PropertyName._image))
    {
      this._image = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NLanguageButton.PropertyName._outline))
    {
      this._outline = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NLanguageButton.PropertyName._flag))
    {
      this._flag = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NLanguageButton.PropertyName._label))
    {
      this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NLanguageButton.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NLanguageButton.PropertyName.isoCode))
      return base.SetGodotClassPropertyValue(in name, in value);
    this.isoCode = VariantUtils.ConvertTo<string>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NLanguageButton.PropertyName.IsSelected))
    {
      ref godot_variant local = ref value;
      bool isSelected = this.IsSelected;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref isSelected);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NLanguageButton.PropertyName._image))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._image);
      return true;
    }
    if (StringName.op_Equality(ref name, NLanguageButton.PropertyName._outline))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._outline);
      return true;
    }
    if (StringName.op_Equality(ref name, NLanguageButton.PropertyName._flag))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._flag);
      return true;
    }
    if (StringName.op_Equality(ref name, NLanguageButton.PropertyName._label))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
      return true;
    }
    if (StringName.op_Equality(ref name, NLanguageButton.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NLanguageButton.PropertyName.isoCode))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<string>(ref this.isoCode);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NLanguageButton.PropertyName._image, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NLanguageButton.PropertyName._outline, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NLanguageButton.PropertyName._flag, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NLanguageButton.PropertyName._label, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NLanguageButton.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 4L, NLanguageButton.PropertyName.isoCode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NLanguageButton.PropertyName.IsSelected, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName isSelected1 = NLanguageButton.PropertyName.IsSelected;
    bool isSelected2 = this.IsSelected;
    Variant variant = Variant.From<bool>(ref isSelected2);
    serializationInfo.AddProperty(isSelected1, variant);
    info.AddProperty(NLanguageButton.PropertyName._image, Variant.From<TextureRect>(ref this._image));
    info.AddProperty(NLanguageButton.PropertyName._outline, Variant.From<Control>(ref this._outline));
    info.AddProperty(NLanguageButton.PropertyName._flag, Variant.From<TextureRect>(ref this._flag));
    info.AddProperty(NLanguageButton.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
    info.AddProperty(NLanguageButton.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NLanguageButton.PropertyName.isoCode, Variant.From<string>(ref this.isoCode));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NLanguageButton.PropertyName.IsSelected, ref variant1))
      this.IsSelected = ((Variant) ref variant1).As<bool>();
    Variant variant2;
    if (info.TryGetProperty(NLanguageButton.PropertyName._image, ref variant2))
      this._image = ((Variant) ref variant2).As<TextureRect>();
    Variant variant3;
    if (info.TryGetProperty(NLanguageButton.PropertyName._outline, ref variant3))
      this._outline = ((Variant) ref variant3).As<Control>();
    Variant variant4;
    if (info.TryGetProperty(NLanguageButton.PropertyName._flag, ref variant4))
      this._flag = ((Variant) ref variant4).As<TextureRect>();
    Variant variant5;
    if (info.TryGetProperty(NLanguageButton.PropertyName._label, ref variant5))
      this._label = ((Variant) ref variant5).As<MegaLabel>();
    Variant variant6;
    if (info.TryGetProperty(NLanguageButton.PropertyName._tween, ref variant6))
      this._tween = ((Variant) ref variant6).As<Tween>();
    Variant variant7;
    if (!info.TryGetProperty(NLanguageButton.PropertyName.isoCode, ref variant7))
      return;
    this.isoCode = ((Variant) ref variant7).As<string>();
  }

  public new class MethodName : NButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName Init = StringName.op_Implicit(nameof (Init));
    public new static readonly StringName OnRelease = StringName.op_Implicit(nameof (OnRelease));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public new static readonly StringName OnPress = StringName.op_Implicit(nameof (OnPress));
    public static readonly StringName SetAsSelected = StringName.op_Implicit(nameof (SetAsSelected));
    public static readonly StringName SetAsDeselected = StringName.op_Implicit(nameof (SetAsDeselected));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public static readonly StringName IsSelected = StringName.op_Implicit(nameof (IsSelected));
    public static readonly StringName _image = StringName.op_Implicit(nameof (_image));
    public static readonly StringName _outline = StringName.op_Implicit(nameof (_outline));
    public static readonly StringName _flag = StringName.op_Implicit(nameof (_flag));
    public static readonly StringName _label = StringName.op_Implicit(nameof (_label));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName isoCode = StringName.op_Implicit(nameof (isoCode));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
