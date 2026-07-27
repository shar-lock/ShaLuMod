// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.CardSelection.NChoiceSelectionSkipButton
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.CardSelection;

[ScriptPath("res://src/Core/Nodes/Screens/CardSelection/NChoiceSelectionSkipButton.cs")]
public class NChoiceSelectionSkipButton : NButton
{
  private static readonly StringName _v = new StringName("v");
  private TextureRect _image;
  private MegaLabel _label;
  private string? _optionName;
  private Tween? _animInTween;
  private Vector2 _showPosition;
  private static readonly Vector2 _animOffsetPosition = new Vector2(0.0f, -50f);
  private Tween? _currentTween;
  private ShaderMaterial _hsv;
  private Variant _hsvDefault = Variant.op_Implicit(0.9);
  private Variant _hsvHover = Variant.op_Implicit(1.1);
  private Variant _hsvDown = Variant.op_Implicit(0.7);
  private static readonly Vector2 _defaultScale = Vector2.One;
  private static readonly Vector2 _hoverScale = Vector2.op_Multiply(Vector2.One, 1.05f);
  private static readonly Vector2 _downScale = Vector2.op_Multiply(Vector2.One, 0.95f);

  protected override string[] Hotkeys
  {
    get
    {
      return new string[1]
      {
        StringName.op_Implicit(MegaInput.cancel)
      };
    }
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._image = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("Image"));
    this._label = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("Label"));
    this._hsv = (ShaderMaterial) ((CanvasItem) this._image).Material;
    this._showPosition = this.Position;
    if (this._optionName != null)
      this._label.SetTextAutoSize(this._optionName);
    this._controllerHotkeyIcon.Texture = NInputManager.Instance.GetHotkeyIcon(((IEnumerable<string>) this.Hotkeys).First<string>());
  }

  public void AnimateIn()
  {
    this._animInTween?.Kill();
    this._animInTween = ((Node) this).CreateTween().SetParallel(true);
    this._animInTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.4).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L).From(Variant.op_Implicit(0.0f));
    this._animInTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("position"), Variant.op_Implicit(this._showPosition), 0.4).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L).From(Variant.op_Implicit(Vector2.op_Addition(this._showPosition, NChoiceSelectionSkipButton._animOffsetPosition)));
  }

  protected override void OnPress()
  {
    this._currentTween?.Kill();
    this._currentTween = ((Node) this).CreateTween().SetParallel(true);
    this._currentTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderParam)), this._hsvHover, this._hsvDown, 0.20000000298023224).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._currentTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(NChoiceSelectionSkipButton._downScale), 0.20000000298023224).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  protected override void OnFocus()
  {
    this._currentTween?.Kill();
    this.Scale = NChoiceSelectionSkipButton._hoverScale;
    this._hsv.SetShaderParameter(NChoiceSelectionSkipButton._v, this._hsvHover);
  }

  protected override void OnUnfocus()
  {
    this._currentTween?.Kill();
    this._currentTween = ((Node) this).CreateTween().SetParallel(true);
    this._currentTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderParam)), this._hsv.GetShaderParameter(NChoiceSelectionSkipButton._v), this._hsvDefault, 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._currentTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(NChoiceSelectionSkipButton._defaultScale), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  private void UpdateShaderParam(float value)
  {
    this._hsv.SetShaderParameter(NChoiceSelectionSkipButton._v, Variant.op_Implicit(value));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(6)
    {
      new MethodInfo(NChoiceSelectionSkipButton.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NChoiceSelectionSkipButton.MethodName.AnimateIn, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NChoiceSelectionSkipButton.MethodName.OnPress, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NChoiceSelectionSkipButton.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NChoiceSelectionSkipButton.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NChoiceSelectionSkipButton.MethodName.UpdateShaderParam, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
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
    if (StringName.op_Equality(ref method, NChoiceSelectionSkipButton.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NChoiceSelectionSkipButton.MethodName.AnimateIn) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimateIn();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NChoiceSelectionSkipButton.MethodName.OnPress) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPress();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NChoiceSelectionSkipButton.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NChoiceSelectionSkipButton.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NChoiceSelectionSkipButton.MethodName.UpdateShaderParam) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.UpdateShaderParam(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NChoiceSelectionSkipButton.MethodName._Ready) || StringName.op_Equality(ref method, NChoiceSelectionSkipButton.MethodName.AnimateIn) || StringName.op_Equality(ref method, NChoiceSelectionSkipButton.MethodName.OnPress) || StringName.op_Equality(ref method, NChoiceSelectionSkipButton.MethodName.OnFocus) || StringName.op_Equality(ref method, NChoiceSelectionSkipButton.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NChoiceSelectionSkipButton.MethodName.UpdateShaderParam) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NChoiceSelectionSkipButton.PropertyName._image))
    {
      this._image = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NChoiceSelectionSkipButton.PropertyName._label))
    {
      this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NChoiceSelectionSkipButton.PropertyName._optionName))
    {
      this._optionName = VariantUtils.ConvertTo<string>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NChoiceSelectionSkipButton.PropertyName._animInTween))
    {
      this._animInTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NChoiceSelectionSkipButton.PropertyName._showPosition))
    {
      this._showPosition = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NChoiceSelectionSkipButton.PropertyName._currentTween))
    {
      this._currentTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NChoiceSelectionSkipButton.PropertyName._hsv))
    {
      this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NChoiceSelectionSkipButton.PropertyName._hsvDefault))
    {
      this._hsvDefault = VariantUtils.ConvertTo<Variant>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NChoiceSelectionSkipButton.PropertyName._hsvHover))
    {
      this._hsvHover = VariantUtils.ConvertTo<Variant>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NChoiceSelectionSkipButton.PropertyName._hsvDown))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._hsvDown = VariantUtils.ConvertTo<Variant>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NChoiceSelectionSkipButton.PropertyName.Hotkeys))
    {
      ref godot_variant local = ref value;
      string[] hotkeys = this.Hotkeys;
      godot_variant from = VariantUtils.CreateFrom<string[]>(ref hotkeys);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NChoiceSelectionSkipButton.PropertyName._image))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._image);
      return true;
    }
    if (StringName.op_Equality(ref name, NChoiceSelectionSkipButton.PropertyName._label))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
      return true;
    }
    if (StringName.op_Equality(ref name, NChoiceSelectionSkipButton.PropertyName._optionName))
    {
      value = VariantUtils.CreateFrom<string>(ref this._optionName);
      return true;
    }
    if (StringName.op_Equality(ref name, NChoiceSelectionSkipButton.PropertyName._animInTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._animInTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NChoiceSelectionSkipButton.PropertyName._showPosition))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._showPosition);
      return true;
    }
    if (StringName.op_Equality(ref name, NChoiceSelectionSkipButton.PropertyName._currentTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._currentTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NChoiceSelectionSkipButton.PropertyName._hsv))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
      return true;
    }
    if (StringName.op_Equality(ref name, NChoiceSelectionSkipButton.PropertyName._hsvDefault))
    {
      value = VariantUtils.CreateFrom<Variant>(ref this._hsvDefault);
      return true;
    }
    if (StringName.op_Equality(ref name, NChoiceSelectionSkipButton.PropertyName._hsvHover))
    {
      value = VariantUtils.CreateFrom<Variant>(ref this._hsvHover);
      return true;
    }
    if (!StringName.op_Equality(ref name, NChoiceSelectionSkipButton.PropertyName._hsvDown))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Variant>(ref this._hsvDown);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NChoiceSelectionSkipButton.PropertyName._image, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NChoiceSelectionSkipButton.PropertyName._label, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 4L, NChoiceSelectionSkipButton.PropertyName._optionName, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NChoiceSelectionSkipButton.PropertyName._animInTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NChoiceSelectionSkipButton.PropertyName._showPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NChoiceSelectionSkipButton.PropertyName._currentTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NChoiceSelectionSkipButton.PropertyName._hsv, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 0L, NChoiceSelectionSkipButton.PropertyName._hsvDefault, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 0L, NChoiceSelectionSkipButton.PropertyName._hsvHover, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 0L, NChoiceSelectionSkipButton.PropertyName._hsvDown, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 34L, NChoiceSelectionSkipButton.PropertyName.Hotkeys, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NChoiceSelectionSkipButton.PropertyName._image, Variant.From<TextureRect>(ref this._image));
    info.AddProperty(NChoiceSelectionSkipButton.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
    info.AddProperty(NChoiceSelectionSkipButton.PropertyName._optionName, Variant.From<string>(ref this._optionName));
    info.AddProperty(NChoiceSelectionSkipButton.PropertyName._animInTween, Variant.From<Tween>(ref this._animInTween));
    info.AddProperty(NChoiceSelectionSkipButton.PropertyName._showPosition, Variant.From<Vector2>(ref this._showPosition));
    info.AddProperty(NChoiceSelectionSkipButton.PropertyName._currentTween, Variant.From<Tween>(ref this._currentTween));
    info.AddProperty(NChoiceSelectionSkipButton.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
    info.AddProperty(NChoiceSelectionSkipButton.PropertyName._hsvDefault, Variant.From<Variant>(ref this._hsvDefault));
    info.AddProperty(NChoiceSelectionSkipButton.PropertyName._hsvHover, Variant.From<Variant>(ref this._hsvHover));
    info.AddProperty(NChoiceSelectionSkipButton.PropertyName._hsvDown, Variant.From<Variant>(ref this._hsvDown));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NChoiceSelectionSkipButton.PropertyName._image, ref variant1))
      this._image = ((Variant) ref variant1).As<TextureRect>();
    Variant variant2;
    if (info.TryGetProperty(NChoiceSelectionSkipButton.PropertyName._label, ref variant2))
      this._label = ((Variant) ref variant2).As<MegaLabel>();
    Variant variant3;
    if (info.TryGetProperty(NChoiceSelectionSkipButton.PropertyName._optionName, ref variant3))
      this._optionName = ((Variant) ref variant3).As<string>();
    Variant variant4;
    if (info.TryGetProperty(NChoiceSelectionSkipButton.PropertyName._animInTween, ref variant4))
      this._animInTween = ((Variant) ref variant4).As<Tween>();
    Variant variant5;
    if (info.TryGetProperty(NChoiceSelectionSkipButton.PropertyName._showPosition, ref variant5))
      this._showPosition = ((Variant) ref variant5).As<Vector2>();
    Variant variant6;
    if (info.TryGetProperty(NChoiceSelectionSkipButton.PropertyName._currentTween, ref variant6))
      this._currentTween = ((Variant) ref variant6).As<Tween>();
    Variant variant7;
    if (info.TryGetProperty(NChoiceSelectionSkipButton.PropertyName._hsv, ref variant7))
      this._hsv = ((Variant) ref variant7).As<ShaderMaterial>();
    Variant variant8;
    if (info.TryGetProperty(NChoiceSelectionSkipButton.PropertyName._hsvDefault, ref variant8))
      this._hsvDefault = ((Variant) ref variant8).As<Variant>();
    Variant variant9;
    if (info.TryGetProperty(NChoiceSelectionSkipButton.PropertyName._hsvHover, ref variant9))
      this._hsvHover = ((Variant) ref variant9).As<Variant>();
    Variant variant10;
    if (!info.TryGetProperty(NChoiceSelectionSkipButton.PropertyName._hsvDown, ref variant10))
      return;
    this._hsvDown = ((Variant) ref variant10).As<Variant>();
  }

  public new class MethodName : NButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName AnimateIn = StringName.op_Implicit(nameof (AnimateIn));
    public new static readonly StringName OnPress = StringName.op_Implicit(nameof (OnPress));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public static readonly StringName UpdateShaderParam = StringName.op_Implicit(nameof (UpdateShaderParam));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public new static readonly StringName Hotkeys = StringName.op_Implicit(nameof (Hotkeys));
    public static readonly StringName _image = StringName.op_Implicit(nameof (_image));
    public static readonly StringName _label = StringName.op_Implicit(nameof (_label));
    public static readonly StringName _optionName = StringName.op_Implicit(nameof (_optionName));
    public static readonly StringName _animInTween = StringName.op_Implicit(nameof (_animInTween));
    public static readonly StringName _showPosition = StringName.op_Implicit(nameof (_showPosition));
    public static readonly StringName _currentTween = StringName.op_Implicit(nameof (_currentTween));
    public static readonly StringName _hsv = StringName.op_Implicit(nameof (_hsv));
    public static readonly StringName _hsvDefault = StringName.op_Implicit(nameof (_hsvDefault));
    public static readonly StringName _hsvHover = StringName.op_Implicit(nameof (_hsvHover));
    public static readonly StringName _hsvDown = StringName.op_Implicit(nameof (_hsvDown));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
