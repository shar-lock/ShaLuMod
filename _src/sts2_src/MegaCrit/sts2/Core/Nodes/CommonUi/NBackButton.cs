// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.CommonUi.NBackButton
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.CommonUi;

[ScriptPath("res://src/Core/Nodes/CommonUi/NBackButton.cs")]
public class NBackButton : NButton
{
  private Control _outline;
  private Control _buttonImage;
  private Color _defaultOutlineColor = StsColors.cream;
  private Color _hoveredOutlineColor = StsColors.gold;
  private Color _downColor = Colors.Gray;
  private Color _outlineColor = new Color("F0B400");
  private Color _outlineTransparentColor = new Color("FF000000");
  private static readonly Vector2 _hoverScale = Vector2.op_Multiply(Vector2.One, 1.05f);
  private static readonly Vector2 _downScale = Vector2.One;
  private const double _animInOutDur = 0.35;
  private Vector2 _posOffset;
  private Vector2 _showPos;
  private Vector2 _hidePos;
  private static readonly Vector2 _hideOffset = new Vector2(-180f, 0.0f);
  private Tween? _hoverTween;
  private Tween? _moveTween;

  protected override string ClickedSfx => "event:/sfx/ui/clicks/ui_back";

  protected override string[] Hotkeys
  {
    get
    {
      return new string[3]
      {
        StringName.op_Implicit(MegaInput.cancel),
        StringName.op_Implicit(MegaInput.pauseAndBack),
        StringName.op_Implicit(MegaInput.back)
      };
    }
  }

  protected override string ControllerIconHotkey => StringName.op_Implicit(MegaInput.cancel);

  public override void _Ready()
  {
    this.ConnectSignals();
    this._outline = ((Node) this).GetNode<Control>(NodePath.op_Implicit("Outline"));
    this._buttonImage = ((Node) this).GetNode<Control>(NodePath.op_Implicit("Image"));
    this._isEnabled = false;
    this._posOffset = new Vector2(this.OffsetLeft + 80f, (float) (-(double) this.OffsetBottom + 110.0));
    ((GodotObject) ((Node) this).GetTree().Root).Connect(Viewport.SignalName.SizeChanged, Callable.From(new Action(this.OnWindowChange)), 0U);
    this.OnWindowChange();
    this.OnDisable();
  }

  private void OnWindowChange()
  {
    this._showPos = Vector2.op_Subtraction(new Vector2(0.0f, (float) ((Node) this).GetWindow().ContentScaleSize.Y), this._posOffset);
    this._hidePos = Vector2.op_Addition(this._showPos, NBackButton._hideOffset);
    this.Position = this._isEnabled ? this._showPos : this._hidePos;
  }

  public void MoveToHidePosition() => this.GlobalPosition = this._hidePos;

  protected override void OnEnable()
  {
    base.OnEnable();
    this._isEnabled = true;
    ((CanvasItem) this._outline).Modulate = Colors.Transparent;
    ((CanvasItem) this._buttonImage).Modulate = Colors.White;
    this.Scale = Vector2.One;
    this._moveTween?.Kill();
    this._moveTween = ((Node) this).CreateTween();
    this._moveTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("global_position"), Variant.op_Implicit(this._showPos), 0.35).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L);
  }

  protected override void OnDisable()
  {
    base.OnDisable();
    this._isEnabled = false;
    this._moveTween?.Kill();
    this._moveTween = ((Node) this).CreateTween();
    this._moveTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("global_position"), Variant.op_Implicit(this._hidePos), 0.35).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  protected override void OnFocus()
  {
    base.OnFocus();
    this._hoverTween?.Kill();
    this._hoverTween = ((Node) this).CreateTween().SetParallel(true);
    this._hoverTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(NBackButton._hoverScale), 0.05);
    this._hoverTween.TweenProperty((GodotObject) this._outline, NodePath.op_Implicit("modulate"), Variant.op_Implicit(this._outlineColor), 0.05);
  }

  protected override void OnUnfocus()
  {
    this._hoverTween?.Kill();
    this._hoverTween = ((Node) this).CreateTween().SetParallel(true);
    this._hoverTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(NBackButton._hoverScale), 0.5).SetTrans((Tween.TransitionType) 5L).SetEase((Tween.EaseType) 1L);
    this._hoverTween.TweenProperty((GodotObject) this._outline, NodePath.op_Implicit("modulate"), Variant.op_Implicit(this._outlineTransparentColor), 0.5);
  }

  protected override void OnPress()
  {
    base.OnPress();
    this._hoverTween?.Kill();
    this._hoverTween = ((Node) this).CreateTween().SetParallel(true);
    this._hoverTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(NBackButton._downScale), 0.25).SetTrans((Tween.TransitionType) 5L).SetEase((Tween.EaseType) 1L);
    this._hoverTween.TweenProperty((GodotObject) this._buttonImage, NodePath.op_Implicit("modulate"), Variant.op_Implicit(this._downColor), 0.25).SetTrans((Tween.TransitionType) 7L).SetEase((Tween.EaseType) 1L);
    this._hoverTween.TweenProperty((GodotObject) this._outline, NodePath.op_Implicit("modulate"), Variant.op_Implicit(this._outlineTransparentColor), 0.25).SetTrans((Tween.TransitionType) 7L).SetEase((Tween.EaseType) 1L);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(8)
    {
      new MethodInfo(NBackButton.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBackButton.MethodName.OnWindowChange, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBackButton.MethodName.MoveToHidePosition, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBackButton.MethodName.OnEnable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBackButton.MethodName.OnDisable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBackButton.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBackButton.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBackButton.MethodName.OnPress, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NBackButton.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBackButton.MethodName.OnWindowChange) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnWindowChange();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBackButton.MethodName.MoveToHidePosition) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.MoveToHidePosition();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBackButton.MethodName.OnEnable) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnEnable();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBackButton.MethodName.OnDisable) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnDisable();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBackButton.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBackButton.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NBackButton.MethodName.OnPress) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnPress();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NBackButton.MethodName._Ready) || StringName.op_Equality(ref method, NBackButton.MethodName.OnWindowChange) || StringName.op_Equality(ref method, NBackButton.MethodName.MoveToHidePosition) || StringName.op_Equality(ref method, NBackButton.MethodName.OnEnable) || StringName.op_Equality(ref method, NBackButton.MethodName.OnDisable) || StringName.op_Equality(ref method, NBackButton.MethodName.OnFocus) || StringName.op_Equality(ref method, NBackButton.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NBackButton.MethodName.OnPress) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NBackButton.PropertyName._outline))
    {
      this._outline = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBackButton.PropertyName._buttonImage))
    {
      this._buttonImage = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBackButton.PropertyName._defaultOutlineColor))
    {
      this._defaultOutlineColor = VariantUtils.ConvertTo<Color>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBackButton.PropertyName._hoveredOutlineColor))
    {
      this._hoveredOutlineColor = VariantUtils.ConvertTo<Color>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBackButton.PropertyName._downColor))
    {
      this._downColor = VariantUtils.ConvertTo<Color>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBackButton.PropertyName._outlineColor))
    {
      this._outlineColor = VariantUtils.ConvertTo<Color>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBackButton.PropertyName._outlineTransparentColor))
    {
      this._outlineTransparentColor = VariantUtils.ConvertTo<Color>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBackButton.PropertyName._posOffset))
    {
      this._posOffset = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBackButton.PropertyName._showPos))
    {
      this._showPos = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBackButton.PropertyName._hidePos))
    {
      this._hidePos = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NBackButton.PropertyName._hoverTween))
    {
      this._hoverTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NBackButton.PropertyName._moveTween))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._moveTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NBackButton.PropertyName.ClickedSfx))
    {
      ref godot_variant local = ref value;
      string clickedSfx = this.ClickedSfx;
      godot_variant from = VariantUtils.CreateFrom<string>(ref clickedSfx);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NBackButton.PropertyName.Hotkeys))
    {
      ref godot_variant local = ref value;
      string[] hotkeys = this.Hotkeys;
      godot_variant from = VariantUtils.CreateFrom<string[]>(ref hotkeys);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NBackButton.PropertyName.ControllerIconHotkey))
    {
      ref godot_variant local = ref value;
      string controllerIconHotkey = this.ControllerIconHotkey;
      godot_variant from = VariantUtils.CreateFrom<string>(ref controllerIconHotkey);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NBackButton.PropertyName._outline))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._outline);
      return true;
    }
    if (StringName.op_Equality(ref name, NBackButton.PropertyName._buttonImage))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._buttonImage);
      return true;
    }
    if (StringName.op_Equality(ref name, NBackButton.PropertyName._defaultOutlineColor))
    {
      value = VariantUtils.CreateFrom<Color>(ref this._defaultOutlineColor);
      return true;
    }
    if (StringName.op_Equality(ref name, NBackButton.PropertyName._hoveredOutlineColor))
    {
      value = VariantUtils.CreateFrom<Color>(ref this._hoveredOutlineColor);
      return true;
    }
    if (StringName.op_Equality(ref name, NBackButton.PropertyName._downColor))
    {
      value = VariantUtils.CreateFrom<Color>(ref this._downColor);
      return true;
    }
    if (StringName.op_Equality(ref name, NBackButton.PropertyName._outlineColor))
    {
      value = VariantUtils.CreateFrom<Color>(ref this._outlineColor);
      return true;
    }
    if (StringName.op_Equality(ref name, NBackButton.PropertyName._outlineTransparentColor))
    {
      value = VariantUtils.CreateFrom<Color>(ref this._outlineTransparentColor);
      return true;
    }
    if (StringName.op_Equality(ref name, NBackButton.PropertyName._posOffset))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._posOffset);
      return true;
    }
    if (StringName.op_Equality(ref name, NBackButton.PropertyName._showPos))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._showPos);
      return true;
    }
    if (StringName.op_Equality(ref name, NBackButton.PropertyName._hidePos))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._hidePos);
      return true;
    }
    if (StringName.op_Equality(ref name, NBackButton.PropertyName._hoverTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._hoverTween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NBackButton.PropertyName._moveTween))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Tween>(ref this._moveTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 4L, NBackButton.PropertyName.ClickedSfx, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 34L, NBackButton.PropertyName.Hotkeys, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 4L, NBackButton.PropertyName.ControllerIconHotkey, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBackButton.PropertyName._outline, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBackButton.PropertyName._buttonImage, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 20L, NBackButton.PropertyName._defaultOutlineColor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 20L, NBackButton.PropertyName._hoveredOutlineColor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 20L, NBackButton.PropertyName._downColor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 20L, NBackButton.PropertyName._outlineColor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 20L, NBackButton.PropertyName._outlineTransparentColor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NBackButton.PropertyName._posOffset, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NBackButton.PropertyName._showPos, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NBackButton.PropertyName._hidePos, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBackButton.PropertyName._hoverTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NBackButton.PropertyName._moveTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NBackButton.PropertyName._outline, Variant.From<Control>(ref this._outline));
    info.AddProperty(NBackButton.PropertyName._buttonImage, Variant.From<Control>(ref this._buttonImage));
    info.AddProperty(NBackButton.PropertyName._defaultOutlineColor, Variant.From<Color>(ref this._defaultOutlineColor));
    info.AddProperty(NBackButton.PropertyName._hoveredOutlineColor, Variant.From<Color>(ref this._hoveredOutlineColor));
    info.AddProperty(NBackButton.PropertyName._downColor, Variant.From<Color>(ref this._downColor));
    info.AddProperty(NBackButton.PropertyName._outlineColor, Variant.From<Color>(ref this._outlineColor));
    info.AddProperty(NBackButton.PropertyName._outlineTransparentColor, Variant.From<Color>(ref this._outlineTransparentColor));
    info.AddProperty(NBackButton.PropertyName._posOffset, Variant.From<Vector2>(ref this._posOffset));
    info.AddProperty(NBackButton.PropertyName._showPos, Variant.From<Vector2>(ref this._showPos));
    info.AddProperty(NBackButton.PropertyName._hidePos, Variant.From<Vector2>(ref this._hidePos));
    info.AddProperty(NBackButton.PropertyName._hoverTween, Variant.From<Tween>(ref this._hoverTween));
    info.AddProperty(NBackButton.PropertyName._moveTween, Variant.From<Tween>(ref this._moveTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NBackButton.PropertyName._outline, ref variant1))
      this._outline = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NBackButton.PropertyName._buttonImage, ref variant2))
      this._buttonImage = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NBackButton.PropertyName._defaultOutlineColor, ref variant3))
      this._defaultOutlineColor = ((Variant) ref variant3).As<Color>();
    Variant variant4;
    if (info.TryGetProperty(NBackButton.PropertyName._hoveredOutlineColor, ref variant4))
      this._hoveredOutlineColor = ((Variant) ref variant4).As<Color>();
    Variant variant5;
    if (info.TryGetProperty(NBackButton.PropertyName._downColor, ref variant5))
      this._downColor = ((Variant) ref variant5).As<Color>();
    Variant variant6;
    if (info.TryGetProperty(NBackButton.PropertyName._outlineColor, ref variant6))
      this._outlineColor = ((Variant) ref variant6).As<Color>();
    Variant variant7;
    if (info.TryGetProperty(NBackButton.PropertyName._outlineTransparentColor, ref variant7))
      this._outlineTransparentColor = ((Variant) ref variant7).As<Color>();
    Variant variant8;
    if (info.TryGetProperty(NBackButton.PropertyName._posOffset, ref variant8))
      this._posOffset = ((Variant) ref variant8).As<Vector2>();
    Variant variant9;
    if (info.TryGetProperty(NBackButton.PropertyName._showPos, ref variant9))
      this._showPos = ((Variant) ref variant9).As<Vector2>();
    Variant variant10;
    if (info.TryGetProperty(NBackButton.PropertyName._hidePos, ref variant10))
      this._hidePos = ((Variant) ref variant10).As<Vector2>();
    Variant variant11;
    if (info.TryGetProperty(NBackButton.PropertyName._hoverTween, ref variant11))
      this._hoverTween = ((Variant) ref variant11).As<Tween>();
    Variant variant12;
    if (!info.TryGetProperty(NBackButton.PropertyName._moveTween, ref variant12))
      return;
    this._moveTween = ((Variant) ref variant12).As<Tween>();
  }

  public new class MethodName : NButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnWindowChange = StringName.op_Implicit(nameof (OnWindowChange));
    public static readonly StringName MoveToHidePosition = StringName.op_Implicit(nameof (MoveToHidePosition));
    public new static readonly StringName OnEnable = StringName.op_Implicit(nameof (OnEnable));
    public new static readonly StringName OnDisable = StringName.op_Implicit(nameof (OnDisable));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public new static readonly StringName OnPress = StringName.op_Implicit(nameof (OnPress));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public new static readonly StringName ClickedSfx = StringName.op_Implicit(nameof (ClickedSfx));
    public new static readonly StringName Hotkeys = StringName.op_Implicit(nameof (Hotkeys));
    public new static readonly StringName ControllerIconHotkey = StringName.op_Implicit(nameof (ControllerIconHotkey));
    public static readonly StringName _outline = StringName.op_Implicit(nameof (_outline));
    public static readonly StringName _buttonImage = StringName.op_Implicit(nameof (_buttonImage));
    public static readonly StringName _defaultOutlineColor = StringName.op_Implicit(nameof (_defaultOutlineColor));
    public static readonly StringName _hoveredOutlineColor = StringName.op_Implicit(nameof (_hoveredOutlineColor));
    public static readonly StringName _downColor = StringName.op_Implicit(nameof (_downColor));
    public static readonly StringName _outlineColor = StringName.op_Implicit(nameof (_outlineColor));
    public static readonly StringName _outlineTransparentColor = StringName.op_Implicit(nameof (_outlineTransparentColor));
    public static readonly StringName _posOffset = StringName.op_Implicit(nameof (_posOffset));
    public static readonly StringName _showPos = StringName.op_Implicit(nameof (_showPos));
    public static readonly StringName _hidePos = StringName.op_Implicit(nameof (_hidePos));
    public static readonly StringName _hoverTween = StringName.op_Implicit(nameof (_hoverTween));
    public static readonly StringName _moveTween = StringName.op_Implicit(nameof (_moveTween));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
