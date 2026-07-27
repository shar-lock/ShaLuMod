// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.CommonUi.NProceedButton
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.CommonUi;

[ScriptPath("res://src/Core/Nodes/CommonUi/NProceedButton.cs")]
public class NProceedButton : NButton
{
  private static readonly StringName _v = new StringName("v");
  private static readonly StringName _s = new StringName("s");
  private Control _outline;
  private Control _buttonImage;
  private MegaLabel _label;
  private ShaderMaterial _hsv;
  private Viewport _viewport;
  private Color _defaultOutlineColor = StsColors.cream;
  private Color _hoveredOutlineColor = StsColors.gold;
  private Color _downColor = Colors.Gray;
  private Color _outlineColor = new Color("FFCC00C0");
  private Color _outlineTransparentColor = new Color("FF000000");
  private Tween? _animTween;
  private Tween? _glowTween;
  private Tween? _hoverTween;
  private static readonly Vector2 _hoverScale = Vector2.op_Multiply(Vector2.One, 1.05f);
  private float _elapsedTime;
  private bool _shouldPulse = true;
  private static readonly Vector2 _showPosRatio = Vector2.op_Division(new Vector2(1583f, 764f), NGame.devResolution);
  private static readonly Vector2 _hidePosRatio = Vector2.op_Addition(NProceedButton._showPosRatio, Vector2.op_Division(new Vector2(400f, 0.0f), NGame.devResolution));

  public static LocString ProceedLoc => new LocString("gameplay_ui", "PROCEED_BUTTON");

  public static LocString SkipLoc => new LocString("gameplay_ui", "CHOOSE_CARD_SKIP_BUTTON");

  public bool IsSkip { get; private set; }

  protected override string[] Hotkeys
  {
    get
    {
      return new string[1]
      {
        StringName.op_Implicit(MegaInput.accept)
      };
    }
  }

  private Vector2 ShowPos
  {
    get
    {
      Vector2 showPosRatio = NProceedButton._showPosRatio;
      Rect2 visibleRect = this._viewport.GetVisibleRect();
      Vector2 size = ((Rect2) ref visibleRect).Size;
      return Vector2.op_Multiply(showPosRatio, size);
    }
  }

  private Vector2 HidePos
  {
    get
    {
      Vector2 hidePosRatio = NProceedButton._hidePosRatio;
      Rect2 visibleRect = this._viewport.GetVisibleRect();
      Vector2 size = ((Rect2) ref visibleRect).Size;
      return Vector2.op_Multiply(hidePosRatio, size);
    }
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._outline = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Outline"));
    this._buttonImage = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Image"));
    this._hsv = (ShaderMaterial) ((CanvasItem) this._buttonImage).GetMaterial();
    this._label = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%Label"));
    this._viewport = ((Node) this).GetViewport();
    this.Position = this.HidePos;
    this.Disable();
    NGame.Instance.DebugToggleProceedButton += new Action(this.DebugToggleVisibility);
  }

  public override void _ExitTree()
  {
    base._ExitTree();
    NGame.Instance.DebugToggleProceedButton -= new Action(this.DebugToggleVisibility);
  }

  protected override void OnEnable()
  {
    base.OnEnable();
    if (NGame.IsDebugHidingProceedButton)
      ((CanvasItem) this).Modulate = Colors.Transparent;
    this.Scale = Vector2.One;
    ((CanvasItem) this._buttonImage).SelfModulate = StsColors.gray;
    this.UpdateShaderS(1f);
    this.UpdateShaderV(1f);
    this._animTween?.Kill();
    this._animTween = ((Node) this).CreateTween().SetParallel(true);
    this._animTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("position"), Variant.op_Implicit(this.ShowPos), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._animTween.TweenProperty((GodotObject) this._buttonImage, NodePath.op_Implicit("self_modulate"), Variant.op_Implicit(Colors.White), 0.5);
    this._hoverTween?.Kill();
    this._hoverTween = ((Node) this).CreateTween().SetParallel(true);
    this._hoverTween.TweenProperty((GodotObject) this._outline, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.5);
    if (!this._shouldPulse)
      return;
    this.StartGlowTween();
  }

  protected override void OnDisable()
  {
    base.OnDisable();
    this._animTween?.Kill();
    this._animTween = ((Node) this).CreateTween().SetParallel(true);
    this._animTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("position"), Variant.op_Implicit(this.HidePos), 0.8).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._animTween.TweenProperty((GodotObject) this._buttonImage, NodePath.op_Implicit("self_modulate"), Variant.op_Implicit(StsColors.gray), 0.8);
    this._animTween.TweenProperty((GodotObject) this._outline, NodePath.op_Implicit("modulate"), Variant.op_Implicit(StsColors.transparentWhite), 0.8);
    this._glowTween?.Kill();
  }

  protected override void OnRelease()
  {
    if (!this._isEnabled)
      return;
    this._hoverTween?.Kill();
    this._hoverTween = ((Node) this).CreateTween().SetParallel(true);
    this._hoverTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._hoverTween.TweenProperty((GodotObject) this._buttonImage, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.5);
    this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NProceedButton._v), Variant.op_Implicit(1f), 0.05);
    this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NProceedButton._s), Variant.op_Implicit(1f), 0.05);
  }

  protected override void OnFocus()
  {
    this._hoverTween?.Kill();
    this._hoverTween = ((Node) this).CreateTween().SetParallel(true);
    this._hoverTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(NProceedButton._hoverScale), 0.05);
    this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NProceedButton._v), Variant.op_Implicit(1.4f), 0.05);
    this._glowTween?.Kill();
    this._glowTween = ((Node) this).CreateTween();
    this._glowTween.TweenProperty((GodotObject) this._outline, NodePath.op_Implicit("self_modulate:a"), Variant.op_Implicit(1f), 0.05);
  }

  protected override void OnUnfocus()
  {
    this._hoverTween?.Kill();
    this._hoverTween = ((Node) this).CreateTween().SetParallel(true);
    this._hoverTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.One), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._hoverTween.TweenProperty((GodotObject) this._buttonImage, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.5);
    this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NProceedButton._v), Variant.op_Implicit(1f), 0.05);
    this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NProceedButton._s), Variant.op_Implicit(1f), 0.05);
    if (this._shouldPulse)
      this.StartGlowTween();
    else
      this.StopGlowTween();
  }

  protected override void OnPress()
  {
    this._hoverTween?.Kill();
    this._hoverTween = ((Node) this).CreateTween().SetParallel(true);
    this._hoverTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 0.95f)), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._hoverTween.TweenProperty((GodotObject) this._buttonImage, NodePath.op_Implicit("modulate"), Variant.op_Implicit(StsColors.gray), 0.5);
    this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NProceedButton._v), Variant.op_Implicit(1f), 0.25);
    this._hoverTween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NProceedButton._s), Variant.op_Implicit(0.8f), 0.25);
    this.StopGlowTween();
  }

  public void UpdateText(LocString loc)
  {
    this._label.SetTextAutoSize(loc.GetFormattedText());
    this.IsSkip = loc.LocEntryKey == NProceedButton.SkipLoc.LocEntryKey;
  }

  private void DebugToggleVisibility()
  {
    ((CanvasItem) this).Modulate = NGame.IsDebugHidingProceedButton ? Colors.Transparent : Colors.White;
  }

  public void SetPulseState(bool isPulsing)
  {
    this._shouldPulse = isPulsing;
    if (isPulsing)
      this.StartGlowTween();
    else
      this.StopGlowTween();
  }

  private void StartGlowTween()
  {
    this._glowTween?.Kill();
    this._glowTween = ((Node) this).CreateTween().SetLoops(0);
    this._glowTween.TweenProperty((GodotObject) this._outline, NodePath.op_Implicit("self_modulate:a"), Variant.op_Implicit(0.25f), 0.5);
    this._glowTween.TweenProperty((GodotObject) this._outline, NodePath.op_Implicit("self_modulate:a"), Variant.op_Implicit(0.75f), 0.5);
  }

  private void StopGlowTween()
  {
    this._glowTween?.Kill();
    this._glowTween = ((Node) this).CreateTween();
    this._glowTween.TweenProperty((GodotObject) this._outline, NodePath.op_Implicit("self_modulate:a"), Variant.op_Implicit(0.0f), 0.5);
  }

  private void UpdateShaderS(float value)
  {
    this._hsv.SetShaderParameter(NProceedButton._s, Variant.op_Implicit(value));
  }

  private void UpdateShaderV(float value)
  {
    this._hsv.SetShaderParameter(NProceedButton._v, Variant.op_Implicit(value));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(14)
    {
      new MethodInfo(NProceedButton.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NProceedButton.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NProceedButton.MethodName.OnEnable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NProceedButton.MethodName.OnDisable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NProceedButton.MethodName.OnRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NProceedButton.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NProceedButton.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NProceedButton.MethodName.OnPress, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NProceedButton.MethodName.DebugToggleVisibility, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NProceedButton.MethodName.SetPulseState, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("isPulsing"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NProceedButton.MethodName.StartGlowTween, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NProceedButton.MethodName.StopGlowTween, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NProceedButton.MethodName.UpdateShaderS, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("value"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NProceedButton.MethodName.UpdateShaderV, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
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
    if (StringName.op_Equality(ref method, NProceedButton.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NProceedButton.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NProceedButton.MethodName.OnEnable) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnEnable();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NProceedButton.MethodName.OnDisable) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnDisable();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NProceedButton.MethodName.OnRelease) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnRelease();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NProceedButton.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NProceedButton.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NProceedButton.MethodName.OnPress) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPress();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NProceedButton.MethodName.DebugToggleVisibility) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.DebugToggleVisibility();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NProceedButton.MethodName.SetPulseState) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetPulseState(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NProceedButton.MethodName.StartGlowTween) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.StartGlowTween();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NProceedButton.MethodName.StopGlowTween) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.StopGlowTween();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NProceedButton.MethodName.UpdateShaderS) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateShaderS(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NProceedButton.MethodName.UpdateShaderV) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.UpdateShaderV(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NProceedButton.MethodName._Ready) || StringName.op_Equality(ref method, NProceedButton.MethodName._ExitTree) || StringName.op_Equality(ref method, NProceedButton.MethodName.OnEnable) || StringName.op_Equality(ref method, NProceedButton.MethodName.OnDisable) || StringName.op_Equality(ref method, NProceedButton.MethodName.OnRelease) || StringName.op_Equality(ref method, NProceedButton.MethodName.OnFocus) || StringName.op_Equality(ref method, NProceedButton.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NProceedButton.MethodName.OnPress) || StringName.op_Equality(ref method, NProceedButton.MethodName.DebugToggleVisibility) || StringName.op_Equality(ref method, NProceedButton.MethodName.SetPulseState) || StringName.op_Equality(ref method, NProceedButton.MethodName.StartGlowTween) || StringName.op_Equality(ref method, NProceedButton.MethodName.StopGlowTween) || StringName.op_Equality(ref method, NProceedButton.MethodName.UpdateShaderS) || StringName.op_Equality(ref method, NProceedButton.MethodName.UpdateShaderV) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NProceedButton.PropertyName.IsSkip))
    {
      this.IsSkip = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NProceedButton.PropertyName._outline))
    {
      this._outline = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NProceedButton.PropertyName._buttonImage))
    {
      this._buttonImage = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NProceedButton.PropertyName._label))
    {
      this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NProceedButton.PropertyName._hsv))
    {
      this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NProceedButton.PropertyName._viewport))
    {
      this._viewport = VariantUtils.ConvertTo<Viewport>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NProceedButton.PropertyName._defaultOutlineColor))
    {
      this._defaultOutlineColor = VariantUtils.ConvertTo<Color>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NProceedButton.PropertyName._hoveredOutlineColor))
    {
      this._hoveredOutlineColor = VariantUtils.ConvertTo<Color>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NProceedButton.PropertyName._downColor))
    {
      this._downColor = VariantUtils.ConvertTo<Color>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NProceedButton.PropertyName._outlineColor))
    {
      this._outlineColor = VariantUtils.ConvertTo<Color>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NProceedButton.PropertyName._outlineTransparentColor))
    {
      this._outlineTransparentColor = VariantUtils.ConvertTo<Color>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NProceedButton.PropertyName._animTween))
    {
      this._animTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NProceedButton.PropertyName._glowTween))
    {
      this._glowTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NProceedButton.PropertyName._hoverTween))
    {
      this._hoverTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NProceedButton.PropertyName._elapsedTime))
    {
      this._elapsedTime = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NProceedButton.PropertyName._shouldPulse))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._shouldPulse = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NProceedButton.PropertyName.IsSkip))
    {
      ref godot_variant local = ref value;
      bool isSkip = this.IsSkip;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref isSkip);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NProceedButton.PropertyName.Hotkeys))
    {
      ref godot_variant local = ref value;
      string[] hotkeys = this.Hotkeys;
      godot_variant from = VariantUtils.CreateFrom<string[]>(ref hotkeys);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NProceedButton.PropertyName.ShowPos))
    {
      ref godot_variant local = ref value;
      Vector2 showPos = this.ShowPos;
      godot_variant from = VariantUtils.CreateFrom<Vector2>(ref showPos);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NProceedButton.PropertyName.HidePos))
    {
      ref godot_variant local = ref value;
      Vector2 hidePos = this.HidePos;
      godot_variant from = VariantUtils.CreateFrom<Vector2>(ref hidePos);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NProceedButton.PropertyName._outline))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._outline);
      return true;
    }
    if (StringName.op_Equality(ref name, NProceedButton.PropertyName._buttonImage))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._buttonImage);
      return true;
    }
    if (StringName.op_Equality(ref name, NProceedButton.PropertyName._label))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
      return true;
    }
    if (StringName.op_Equality(ref name, NProceedButton.PropertyName._hsv))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
      return true;
    }
    if (StringName.op_Equality(ref name, NProceedButton.PropertyName._viewport))
    {
      value = VariantUtils.CreateFrom<Viewport>(ref this._viewport);
      return true;
    }
    if (StringName.op_Equality(ref name, NProceedButton.PropertyName._defaultOutlineColor))
    {
      value = VariantUtils.CreateFrom<Color>(ref this._defaultOutlineColor);
      return true;
    }
    if (StringName.op_Equality(ref name, NProceedButton.PropertyName._hoveredOutlineColor))
    {
      value = VariantUtils.CreateFrom<Color>(ref this._hoveredOutlineColor);
      return true;
    }
    if (StringName.op_Equality(ref name, NProceedButton.PropertyName._downColor))
    {
      value = VariantUtils.CreateFrom<Color>(ref this._downColor);
      return true;
    }
    if (StringName.op_Equality(ref name, NProceedButton.PropertyName._outlineColor))
    {
      value = VariantUtils.CreateFrom<Color>(ref this._outlineColor);
      return true;
    }
    if (StringName.op_Equality(ref name, NProceedButton.PropertyName._outlineTransparentColor))
    {
      value = VariantUtils.CreateFrom<Color>(ref this._outlineTransparentColor);
      return true;
    }
    if (StringName.op_Equality(ref name, NProceedButton.PropertyName._animTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._animTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NProceedButton.PropertyName._glowTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._glowTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NProceedButton.PropertyName._hoverTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._hoverTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NProceedButton.PropertyName._elapsedTime))
    {
      value = VariantUtils.CreateFrom<float>(ref this._elapsedTime);
      return true;
    }
    if (!StringName.op_Equality(ref name, NProceedButton.PropertyName._shouldPulse))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<bool>(ref this._shouldPulse);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 1L, NProceedButton.PropertyName.IsSkip, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 34L, NProceedButton.PropertyName.Hotkeys, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NProceedButton.PropertyName._outline, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NProceedButton.PropertyName._buttonImage, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NProceedButton.PropertyName._label, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NProceedButton.PropertyName._hsv, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NProceedButton.PropertyName._viewport, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 20L, NProceedButton.PropertyName._defaultOutlineColor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 20L, NProceedButton.PropertyName._hoveredOutlineColor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 20L, NProceedButton.PropertyName._downColor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 20L, NProceedButton.PropertyName._outlineColor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 20L, NProceedButton.PropertyName._outlineTransparentColor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NProceedButton.PropertyName._animTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NProceedButton.PropertyName._glowTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NProceedButton.PropertyName._hoverTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NProceedButton.PropertyName._elapsedTime, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NProceedButton.PropertyName._shouldPulse, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NProceedButton.PropertyName.ShowPos, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NProceedButton.PropertyName.HidePos, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName isSkip1 = NProceedButton.PropertyName.IsSkip;
    bool isSkip2 = this.IsSkip;
    Variant variant = Variant.From<bool>(ref isSkip2);
    serializationInfo.AddProperty(isSkip1, variant);
    info.AddProperty(NProceedButton.PropertyName._outline, Variant.From<Control>(ref this._outline));
    info.AddProperty(NProceedButton.PropertyName._buttonImage, Variant.From<Control>(ref this._buttonImage));
    info.AddProperty(NProceedButton.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
    info.AddProperty(NProceedButton.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
    info.AddProperty(NProceedButton.PropertyName._viewport, Variant.From<Viewport>(ref this._viewport));
    info.AddProperty(NProceedButton.PropertyName._defaultOutlineColor, Variant.From<Color>(ref this._defaultOutlineColor));
    info.AddProperty(NProceedButton.PropertyName._hoveredOutlineColor, Variant.From<Color>(ref this._hoveredOutlineColor));
    info.AddProperty(NProceedButton.PropertyName._downColor, Variant.From<Color>(ref this._downColor));
    info.AddProperty(NProceedButton.PropertyName._outlineColor, Variant.From<Color>(ref this._outlineColor));
    info.AddProperty(NProceedButton.PropertyName._outlineTransparentColor, Variant.From<Color>(ref this._outlineTransparentColor));
    info.AddProperty(NProceedButton.PropertyName._animTween, Variant.From<Tween>(ref this._animTween));
    info.AddProperty(NProceedButton.PropertyName._glowTween, Variant.From<Tween>(ref this._glowTween));
    info.AddProperty(NProceedButton.PropertyName._hoverTween, Variant.From<Tween>(ref this._hoverTween));
    info.AddProperty(NProceedButton.PropertyName._elapsedTime, Variant.From<float>(ref this._elapsedTime));
    info.AddProperty(NProceedButton.PropertyName._shouldPulse, Variant.From<bool>(ref this._shouldPulse));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NProceedButton.PropertyName.IsSkip, ref variant1))
      this.IsSkip = ((Variant) ref variant1).As<bool>();
    Variant variant2;
    if (info.TryGetProperty(NProceedButton.PropertyName._outline, ref variant2))
      this._outline = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NProceedButton.PropertyName._buttonImage, ref variant3))
      this._buttonImage = ((Variant) ref variant3).As<Control>();
    Variant variant4;
    if (info.TryGetProperty(NProceedButton.PropertyName._label, ref variant4))
      this._label = ((Variant) ref variant4).As<MegaLabel>();
    Variant variant5;
    if (info.TryGetProperty(NProceedButton.PropertyName._hsv, ref variant5))
      this._hsv = ((Variant) ref variant5).As<ShaderMaterial>();
    Variant variant6;
    if (info.TryGetProperty(NProceedButton.PropertyName._viewport, ref variant6))
      this._viewport = ((Variant) ref variant6).As<Viewport>();
    Variant variant7;
    if (info.TryGetProperty(NProceedButton.PropertyName._defaultOutlineColor, ref variant7))
      this._defaultOutlineColor = ((Variant) ref variant7).As<Color>();
    Variant variant8;
    if (info.TryGetProperty(NProceedButton.PropertyName._hoveredOutlineColor, ref variant8))
      this._hoveredOutlineColor = ((Variant) ref variant8).As<Color>();
    Variant variant9;
    if (info.TryGetProperty(NProceedButton.PropertyName._downColor, ref variant9))
      this._downColor = ((Variant) ref variant9).As<Color>();
    Variant variant10;
    if (info.TryGetProperty(NProceedButton.PropertyName._outlineColor, ref variant10))
      this._outlineColor = ((Variant) ref variant10).As<Color>();
    Variant variant11;
    if (info.TryGetProperty(NProceedButton.PropertyName._outlineTransparentColor, ref variant11))
      this._outlineTransparentColor = ((Variant) ref variant11).As<Color>();
    Variant variant12;
    if (info.TryGetProperty(NProceedButton.PropertyName._animTween, ref variant12))
      this._animTween = ((Variant) ref variant12).As<Tween>();
    Variant variant13;
    if (info.TryGetProperty(NProceedButton.PropertyName._glowTween, ref variant13))
      this._glowTween = ((Variant) ref variant13).As<Tween>();
    Variant variant14;
    if (info.TryGetProperty(NProceedButton.PropertyName._hoverTween, ref variant14))
      this._hoverTween = ((Variant) ref variant14).As<Tween>();
    Variant variant15;
    if (info.TryGetProperty(NProceedButton.PropertyName._elapsedTime, ref variant15))
      this._elapsedTime = ((Variant) ref variant15).As<float>();
    Variant variant16;
    if (!info.TryGetProperty(NProceedButton.PropertyName._shouldPulse, ref variant16))
      return;
    this._shouldPulse = ((Variant) ref variant16).As<bool>();
  }

  public new class MethodName : NButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public new static readonly StringName OnEnable = StringName.op_Implicit(nameof (OnEnable));
    public new static readonly StringName OnDisable = StringName.op_Implicit(nameof (OnDisable));
    public new static readonly StringName OnRelease = StringName.op_Implicit(nameof (OnRelease));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public new static readonly StringName OnPress = StringName.op_Implicit(nameof (OnPress));
    public static readonly StringName DebugToggleVisibility = StringName.op_Implicit(nameof (DebugToggleVisibility));
    public static readonly StringName SetPulseState = StringName.op_Implicit(nameof (SetPulseState));
    public static readonly StringName StartGlowTween = StringName.op_Implicit(nameof (StartGlowTween));
    public static readonly StringName StopGlowTween = StringName.op_Implicit(nameof (StopGlowTween));
    public static readonly StringName UpdateShaderS = StringName.op_Implicit(nameof (UpdateShaderS));
    public static readonly StringName UpdateShaderV = StringName.op_Implicit(nameof (UpdateShaderV));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public static readonly StringName IsSkip = StringName.op_Implicit(nameof (IsSkip));
    public new static readonly StringName Hotkeys = StringName.op_Implicit(nameof (Hotkeys));
    public static readonly StringName ShowPos = StringName.op_Implicit(nameof (ShowPos));
    public static readonly StringName HidePos = StringName.op_Implicit(nameof (HidePos));
    public static readonly StringName _outline = StringName.op_Implicit(nameof (_outline));
    public static readonly StringName _buttonImage = StringName.op_Implicit(nameof (_buttonImage));
    public static readonly StringName _label = StringName.op_Implicit(nameof (_label));
    public static readonly StringName _hsv = StringName.op_Implicit(nameof (_hsv));
    public static readonly StringName _viewport = StringName.op_Implicit(nameof (_viewport));
    public static readonly StringName _defaultOutlineColor = StringName.op_Implicit(nameof (_defaultOutlineColor));
    public static readonly StringName _hoveredOutlineColor = StringName.op_Implicit(nameof (_hoveredOutlineColor));
    public static readonly StringName _downColor = StringName.op_Implicit(nameof (_downColor));
    public static readonly StringName _outlineColor = StringName.op_Implicit(nameof (_outlineColor));
    public static readonly StringName _outlineTransparentColor = StringName.op_Implicit(nameof (_outlineTransparentColor));
    public static readonly StringName _animTween = StringName.op_Implicit(nameof (_animTween));
    public static readonly StringName _glowTween = StringName.op_Implicit(nameof (_glowTween));
    public static readonly StringName _hoverTween = StringName.op_Implicit(nameof (_hoverTween));
    public static readonly StringName _elapsedTime = StringName.op_Implicit(nameof (_elapsedTime));
    public static readonly StringName _shouldPulse = StringName.op_Implicit(nameof (_shouldPulse));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
