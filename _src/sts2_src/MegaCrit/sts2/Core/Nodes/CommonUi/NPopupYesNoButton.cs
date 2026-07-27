// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.CommonUi.NPopupYesNoButton
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.CommonUi;

[ScriptPath("res://src/Core/Nodes/CommonUi/NPopupYesNoButton.cs")]
public class NPopupYesNoButton : NButton
{
  private static readonly StringName _v = new StringName("v");
  private static readonly StringName _s = new StringName("s");
  private Control _visuals;
  private Control _outline;
  private Control _image;
  private MegaLabel _label;
  private Tween? _tween;
  private float _baseS;
  private float _baseV;
  private ShaderMaterial _hsv;
  private CanvasItemMaterial _outlineMaterial;
  private bool _isFocused;
  private static readonly Color _goldOutline = new Color("f0b400");
  private bool _isYes;

  public bool IsYes
  {
    get => this._isYes;
    set
    {
      this.DisconnectHotkeys();
      this._isYes = value;
      Callable callable = Callable.From(new Action(((NButton) this).RegisterHotkeys));
      ((Callable) ref callable).CallDeferred(Array.Empty<Variant>());
      this.UpdateControllerButton();
    }
  }

  protected override string[] Hotkeys
  {
    get
    {
      return new string[1]
      {
        StringName.op_Implicit(this._isYes ? MegaInput.select : MegaInput.cancel)
      };
    }
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._visuals = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Visuals"));
    this._outline = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Outline"));
    this._image = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Image"));
    this._label = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%Label"));
    this._hsv = (ShaderMaterial) ((CanvasItem) this._image).GetMaterial();
    this._outlineMaterial = (CanvasItemMaterial) ((CanvasItem) this._outline).GetMaterial();
    this._baseS = Variant.op_Explicit(this._hsv.GetShaderParameter(NPopupYesNoButton._s));
    this._baseV = Variant.op_Explicit(this._hsv.GetShaderParameter(NPopupYesNoButton._v));
  }

  public override void _ExitTree()
  {
    base._ExitTree();
    this.DisconnectHotkeys();
  }

  public void DisconnectHotkeys()
  {
    foreach (string hotkey in this.Hotkeys)
    {
      NHotkeyManager.Instance.RemoveHotkeyPressedBinding(hotkey, new Action(((NClickableControl) this).OnPressHandler));
      NHotkeyManager.Instance.RemoveHotkeyReleasedBinding(hotkey, new Action(((NClickableControl) this).OnReleaseHandler));
    }
  }

  public void SetText(string text) => this._label.SetTextAutoSize(text);

  protected override void OnFocus()
  {
    base.OnFocus();
    this._isFocused = true;
    this._outlineMaterial.BlendMode = (CanvasItemMaterial.BlendModeEnum) 1L;
    ((CanvasItem) this._outline).Modulate = Colors.White;
    ((CanvasItem) this._outline).SelfModulate = NPopupYesNoButton._goldOutline;
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._visuals, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 1.025f)), 0.05);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NPopupYesNoButton._s), Variant.op_Implicit(this._baseS + 0.25f), 0.05).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NPopupYesNoButton._v), Variant.op_Implicit(this._baseV + 0.25f), 0.05).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  protected override void OnUnfocus()
  {
    base.OnUnfocus();
    this._isFocused = false;
    this._outlineMaterial.BlendMode = (CanvasItemMaterial.BlendModeEnum) 0L;
    ((CanvasItem) this._outline).Modulate = StsColors.halfTransparentWhite;
    ((CanvasItem) this._outline).SelfModulate = Colors.Black;
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._visuals, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NPopupYesNoButton._s), Variant.op_Implicit(this._baseS), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NPopupYesNoButton._v), Variant.op_Implicit(this._baseV), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  protected override void OnPress()
  {
    base.OnPress();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._visuals, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 0.975f)), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NPopupYesNoButton._s), Variant.op_Implicit(this._baseS - 0.1f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NPopupYesNoButton._v), Variant.op_Implicit(this._baseV - 0.1f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  protected override void OnRelease()
  {
    this._isFocused = false;
    this._outlineMaterial.BlendMode = (CanvasItemMaterial.BlendModeEnum) 0L;
    ((CanvasItem) this._outline).Modulate = StsColors.halfTransparentWhite;
    ((CanvasItem) this._outline).SelfModulate = Colors.Black;
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._visuals, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.05);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NPopupYesNoButton._s), Variant.op_Implicit(this._baseS), 0.05);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NPopupYesNoButton._v), Variant.op_Implicit(this._baseV), 0.05);
  }

  private void UpdateShaderS(float value)
  {
    this._hsv.SetShaderParameter(NPopupYesNoButton._s, Variant.op_Implicit(value));
  }

  private void UpdateShaderV(float value)
  {
    this._hsv.SetShaderParameter(NPopupYesNoButton._v, Variant.op_Implicit(value));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(10)
    {
      new MethodInfo(NPopupYesNoButton.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPopupYesNoButton.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPopupYesNoButton.MethodName.DisconnectHotkeys, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPopupYesNoButton.MethodName.SetText, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("text"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NPopupYesNoButton.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPopupYesNoButton.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPopupYesNoButton.MethodName.OnPress, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPopupYesNoButton.MethodName.OnRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPopupYesNoButton.MethodName.UpdateShaderS, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("value"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NPopupYesNoButton.MethodName.UpdateShaderV, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
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
    if (StringName.op_Equality(ref method, NPopupYesNoButton.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPopupYesNoButton.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPopupYesNoButton.MethodName.DisconnectHotkeys) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.DisconnectHotkeys();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPopupYesNoButton.MethodName.SetText) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetText(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPopupYesNoButton.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPopupYesNoButton.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPopupYesNoButton.MethodName.OnPress) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPress();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPopupYesNoButton.MethodName.OnRelease) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnRelease();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPopupYesNoButton.MethodName.UpdateShaderS) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateShaderS(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NPopupYesNoButton.MethodName.UpdateShaderV) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.UpdateShaderV(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NPopupYesNoButton.MethodName._Ready) || StringName.op_Equality(ref method, NPopupYesNoButton.MethodName._ExitTree) || StringName.op_Equality(ref method, NPopupYesNoButton.MethodName.DisconnectHotkeys) || StringName.op_Equality(ref method, NPopupYesNoButton.MethodName.SetText) || StringName.op_Equality(ref method, NPopupYesNoButton.MethodName.OnFocus) || StringName.op_Equality(ref method, NPopupYesNoButton.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NPopupYesNoButton.MethodName.OnPress) || StringName.op_Equality(ref method, NPopupYesNoButton.MethodName.OnRelease) || StringName.op_Equality(ref method, NPopupYesNoButton.MethodName.UpdateShaderS) || StringName.op_Equality(ref method, NPopupYesNoButton.MethodName.UpdateShaderV) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NPopupYesNoButton.PropertyName.IsYes))
    {
      this.IsYes = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPopupYesNoButton.PropertyName._visuals))
    {
      this._visuals = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPopupYesNoButton.PropertyName._outline))
    {
      this._outline = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPopupYesNoButton.PropertyName._image))
    {
      this._image = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPopupYesNoButton.PropertyName._label))
    {
      this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPopupYesNoButton.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPopupYesNoButton.PropertyName._baseS))
    {
      this._baseS = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPopupYesNoButton.PropertyName._baseV))
    {
      this._baseV = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPopupYesNoButton.PropertyName._hsv))
    {
      this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPopupYesNoButton.PropertyName._outlineMaterial))
    {
      this._outlineMaterial = VariantUtils.ConvertTo<CanvasItemMaterial>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPopupYesNoButton.PropertyName._isFocused))
    {
      this._isFocused = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NPopupYesNoButton.PropertyName._isYes))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._isYes = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NPopupYesNoButton.PropertyName.IsYes))
    {
      ref godot_variant local = ref value;
      bool isYes = this.IsYes;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref isYes);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NPopupYesNoButton.PropertyName.Hotkeys))
    {
      ref godot_variant local = ref value;
      string[] hotkeys = this.Hotkeys;
      godot_variant from = VariantUtils.CreateFrom<string[]>(ref hotkeys);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NPopupYesNoButton.PropertyName._visuals))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._visuals);
      return true;
    }
    if (StringName.op_Equality(ref name, NPopupYesNoButton.PropertyName._outline))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._outline);
      return true;
    }
    if (StringName.op_Equality(ref name, NPopupYesNoButton.PropertyName._image))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._image);
      return true;
    }
    if (StringName.op_Equality(ref name, NPopupYesNoButton.PropertyName._label))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
      return true;
    }
    if (StringName.op_Equality(ref name, NPopupYesNoButton.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (StringName.op_Equality(ref name, NPopupYesNoButton.PropertyName._baseS))
    {
      value = VariantUtils.CreateFrom<float>(ref this._baseS);
      return true;
    }
    if (StringName.op_Equality(ref name, NPopupYesNoButton.PropertyName._baseV))
    {
      value = VariantUtils.CreateFrom<float>(ref this._baseV);
      return true;
    }
    if (StringName.op_Equality(ref name, NPopupYesNoButton.PropertyName._hsv))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
      return true;
    }
    if (StringName.op_Equality(ref name, NPopupYesNoButton.PropertyName._outlineMaterial))
    {
      value = VariantUtils.CreateFrom<CanvasItemMaterial>(ref this._outlineMaterial);
      return true;
    }
    if (StringName.op_Equality(ref name, NPopupYesNoButton.PropertyName._isFocused))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isFocused);
      return true;
    }
    if (!StringName.op_Equality(ref name, NPopupYesNoButton.PropertyName._isYes))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<bool>(ref this._isYes);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NPopupYesNoButton.PropertyName._visuals, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPopupYesNoButton.PropertyName._outline, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPopupYesNoButton.PropertyName._image, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPopupYesNoButton.PropertyName._label, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPopupYesNoButton.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NPopupYesNoButton.PropertyName._baseS, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NPopupYesNoButton.PropertyName._baseV, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPopupYesNoButton.PropertyName._hsv, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPopupYesNoButton.PropertyName._outlineMaterial, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NPopupYesNoButton.PropertyName._isFocused, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NPopupYesNoButton.PropertyName._isYes, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NPopupYesNoButton.PropertyName.IsYes, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 34L, NPopupYesNoButton.PropertyName.Hotkeys, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName isYes1 = NPopupYesNoButton.PropertyName.IsYes;
    bool isYes2 = this.IsYes;
    Variant variant = Variant.From<bool>(ref isYes2);
    serializationInfo.AddProperty(isYes1, variant);
    info.AddProperty(NPopupYesNoButton.PropertyName._visuals, Variant.From<Control>(ref this._visuals));
    info.AddProperty(NPopupYesNoButton.PropertyName._outline, Variant.From<Control>(ref this._outline));
    info.AddProperty(NPopupYesNoButton.PropertyName._image, Variant.From<Control>(ref this._image));
    info.AddProperty(NPopupYesNoButton.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
    info.AddProperty(NPopupYesNoButton.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NPopupYesNoButton.PropertyName._baseS, Variant.From<float>(ref this._baseS));
    info.AddProperty(NPopupYesNoButton.PropertyName._baseV, Variant.From<float>(ref this._baseV));
    info.AddProperty(NPopupYesNoButton.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
    info.AddProperty(NPopupYesNoButton.PropertyName._outlineMaterial, Variant.From<CanvasItemMaterial>(ref this._outlineMaterial));
    info.AddProperty(NPopupYesNoButton.PropertyName._isFocused, Variant.From<bool>(ref this._isFocused));
    info.AddProperty(NPopupYesNoButton.PropertyName._isYes, Variant.From<bool>(ref this._isYes));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NPopupYesNoButton.PropertyName.IsYes, ref variant1))
      this.IsYes = ((Variant) ref variant1).As<bool>();
    Variant variant2;
    if (info.TryGetProperty(NPopupYesNoButton.PropertyName._visuals, ref variant2))
      this._visuals = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NPopupYesNoButton.PropertyName._outline, ref variant3))
      this._outline = ((Variant) ref variant3).As<Control>();
    Variant variant4;
    if (info.TryGetProperty(NPopupYesNoButton.PropertyName._image, ref variant4))
      this._image = ((Variant) ref variant4).As<Control>();
    Variant variant5;
    if (info.TryGetProperty(NPopupYesNoButton.PropertyName._label, ref variant5))
      this._label = ((Variant) ref variant5).As<MegaLabel>();
    Variant variant6;
    if (info.TryGetProperty(NPopupYesNoButton.PropertyName._tween, ref variant6))
      this._tween = ((Variant) ref variant6).As<Tween>();
    Variant variant7;
    if (info.TryGetProperty(NPopupYesNoButton.PropertyName._baseS, ref variant7))
      this._baseS = ((Variant) ref variant7).As<float>();
    Variant variant8;
    if (info.TryGetProperty(NPopupYesNoButton.PropertyName._baseV, ref variant8))
      this._baseV = ((Variant) ref variant8).As<float>();
    Variant variant9;
    if (info.TryGetProperty(NPopupYesNoButton.PropertyName._hsv, ref variant9))
      this._hsv = ((Variant) ref variant9).As<ShaderMaterial>();
    Variant variant10;
    if (info.TryGetProperty(NPopupYesNoButton.PropertyName._outlineMaterial, ref variant10))
      this._outlineMaterial = ((Variant) ref variant10).As<CanvasItemMaterial>();
    Variant variant11;
    if (info.TryGetProperty(NPopupYesNoButton.PropertyName._isFocused, ref variant11))
      this._isFocused = ((Variant) ref variant11).As<bool>();
    Variant variant12;
    if (!info.TryGetProperty(NPopupYesNoButton.PropertyName._isYes, ref variant12))
      return;
    this._isYes = ((Variant) ref variant12).As<bool>();
  }

  public new class MethodName : NButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName DisconnectHotkeys = StringName.op_Implicit(nameof (DisconnectHotkeys));
    public static readonly StringName SetText = StringName.op_Implicit(nameof (SetText));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public new static readonly StringName OnPress = StringName.op_Implicit(nameof (OnPress));
    public new static readonly StringName OnRelease = StringName.op_Implicit(nameof (OnRelease));
    public static readonly StringName UpdateShaderS = StringName.op_Implicit(nameof (UpdateShaderS));
    public static readonly StringName UpdateShaderV = StringName.op_Implicit(nameof (UpdateShaderV));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public static readonly StringName IsYes = StringName.op_Implicit(nameof (IsYes));
    public new static readonly StringName Hotkeys = StringName.op_Implicit(nameof (Hotkeys));
    public static readonly StringName _visuals = StringName.op_Implicit(nameof (_visuals));
    public static readonly StringName _outline = StringName.op_Implicit(nameof (_outline));
    public static readonly StringName _image = StringName.op_Implicit(nameof (_image));
    public static readonly StringName _label = StringName.op_Implicit(nameof (_label));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _baseS = StringName.op_Implicit(nameof (_baseS));
    public static readonly StringName _baseV = StringName.op_Implicit(nameof (_baseV));
    public static readonly StringName _hsv = StringName.op_Implicit(nameof (_hsv));
    public static readonly StringName _outlineMaterial = StringName.op_Implicit(nameof (_outlineMaterial));
    public static readonly StringName _isFocused = StringName.op_Implicit(nameof (_isFocused));
    public static readonly StringName _isYes = StringName.op_Implicit(nameof (_isYes));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
