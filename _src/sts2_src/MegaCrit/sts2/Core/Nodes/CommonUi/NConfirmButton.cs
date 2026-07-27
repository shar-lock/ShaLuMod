// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.CommonUi.NConfirmButton
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
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.CommonUi;

[ScriptPath("res://src/Core/Nodes/CommonUi/NConfirmButton.cs")]
public class NConfirmButton : NButton
{
  private Control _outline;
  private Control _buttonImage;
  private Color _defaultOutlineColor = StsColors.cream;
  private Color _hoveredOutlineColor = StsColors.gold;
  private Color _downColor = Colors.Gray;
  private Color _outlineColor = new Color("F0B400");
  private Color _outlineTransparentColor = new Color("00FFFF00");
  private Viewport _viewport;
  private string[] _hotkeys = new string[1]
  {
    StringName.op_Implicit(MegaInput.accept)
  };
  private static readonly Vector2 _hoverScale = new Vector2(1.05f, 1.05f);
  private static readonly Vector2 _downScale = new Vector2(0.95f, 0.95f);
  private const float _pressDownDur = 0.25f;
  private const float _unhoverAnimDur = 0.5f;
  private const float _animInOutDur = 0.35f;
  private Vector2 _posOffset;
  private Vector2 _showPos;
  private Vector2 _hidePos;
  private static readonly Vector2 _hideOffset = new Vector2(180f, 0.0f);
  private Tween? _moveTween;
  private CancellationTokenSource? _pressDownCancelToken;
  private CancellationTokenSource? _unhoverAnimCancelToken;

  protected override string[] Hotkeys => this._hotkeys;

  public override void _Ready()
  {
    this.ConnectSignals();
    this._isEnabled = false;
    this._outline = ((Node) this).GetNode<Control>(NodePath.op_Implicit("Outline"));
    this._buttonImage = ((Node) this).GetNode<Control>(NodePath.op_Implicit("Image"));
    this._viewport = ((Node) this).GetViewport();
    this._posOffset = new Vector2(this.OffsetRight + 120f, (float) (-(double) this.OffsetBottom + 110.0));
    ((GodotObject) ((Node) this).GetTree().Root).Connect(Viewport.SignalName.SizeChanged, Callable.From(new Action(this.OnWindowChange)), 0U);
    this.OnWindowChange();
    this.OnDisable();
  }

  public override void _ExitTree()
  {
    base._ExitTree();
    this._pressDownCancelToken?.Cancel();
    this._unhoverAnimCancelToken?.Cancel();
  }

  private void OnWindowChange()
  {
    this._showPos = Vector2.op_Subtraction(NGame.Instance.Size, this._posOffset);
    this._hidePos = Vector2.op_Addition(this._showPos, NConfirmButton._hideOffset);
    this.Position = this._isEnabled ? this._showPos : this._hidePos;
  }

  protected override void OnEnable()
  {
    base.OnEnable();
    this._isEnabled = true;
    ((CanvasItem) this._outline).Modulate = Colors.Transparent;
    ((CanvasItem) this._buttonImage).Modulate = Colors.White;
    this._moveTween?.Kill();
    this._moveTween = ((Node) this).CreateTween();
    this._moveTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("position"), Variant.op_Implicit(this._showPos), 0.34999999403953552).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L).FromCurrent();
  }

  protected override void OnDisable()
  {
    base.OnDisable();
    this._isEnabled = false;
    this._moveTween?.Kill();
    this._moveTween = ((Node) this).CreateTween();
    this._moveTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("position"), Variant.op_Implicit(this._hidePos), 0.34999999403953552).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L).FromCurrent();
  }

  protected override void OnFocus()
  {
    base.OnFocus();
    this._unhoverAnimCancelToken?.Cancel();
    this.Scale = NConfirmButton._hoverScale;
    ((CanvasItem) this._outline).Modulate = this._outlineColor;
    ((CanvasItem) this._buttonImage).Modulate = Colors.White;
  }

  protected override void OnUnfocus()
  {
    base.OnUnfocus();
    this._pressDownCancelToken?.Cancel();
    this._unhoverAnimCancelToken = new CancellationTokenSource();
    TaskHelper.RunSafely(this.AnimUnhover(this._unhoverAnimCancelToken));
  }

  private async Task AnimUnhover(CancellationTokenSource cancelToken)
  {
    float num1 = 0.0f;
    Vector2 startScale = this.Scale;
    Color startButtonColor = ((CanvasItem) this._buttonImage).Modulate;
    Color startColor = ((CanvasItem) this._outline).Modulate;
    float num;
    for (; (double) num1 < 0.5; num1 = num + await ((Node) this).AwaitProcessFrame())
    {
      if (cancelToken.IsCancellationRequested)
        return;
      this.Scale = ((Vector2) ref startScale).Lerp(Vector2.One, Ease.ExpoOut(num1 / 0.5f));
      ((CanvasItem) this._outline).Modulate = ((Color) ref startColor).Lerp(this._outlineTransparentColor, Ease.ExpoOut(num1 / 0.5f));
      ((CanvasItem) this._buttonImage).Modulate = ((Color) ref startButtonColor).Lerp(Colors.White, Ease.ExpoOut(num1 / 0.5f));
      num = num1;
    }
    this.Scale = Vector2.One;
    ((CanvasItem) this._outline).Modulate = this._outlineTransparentColor;
    ((CanvasItem) this._buttonImage).Modulate = Colors.White;
  }

  protected override void OnPress()
  {
    base.OnPress();
    this._pressDownCancelToken = new CancellationTokenSource();
    TaskHelper.RunSafely(this.AnimPressDown(this._pressDownCancelToken));
  }

  private async Task AnimPressDown(CancellationTokenSource cancelToken)
  {
    float num1 = 0.0f;
    ((CanvasItem) this._buttonImage).Modulate = Colors.White;
    ((CanvasItem) this._outline).Modulate = this._outlineColor;
    this.Scale = NConfirmButton._hoverScale;
    float num;
    for (; (double) num1 < 0.25; num1 = num + await ((Node) this).AwaitProcessFrame())
    {
      if (cancelToken.IsCancellationRequested)
        return;
      this.Scale = ((Vector2) ref NConfirmButton._hoverScale).Lerp(NConfirmButton._downScale, Ease.CubicOut(num1 / 0.25f));
      Control buttonImage = this._buttonImage;
      Color white = Colors.White;
      Color color = ((Color) ref white).Lerp(this._downColor, Ease.CubicOut(num1 / 0.25f));
      ((CanvasItem) buttonImage).Modulate = color;
      ((CanvasItem) this._outline).Modulate = ((Color) ref this._outlineColor).Lerp(this._outlineTransparentColor, Ease.CubicOut(num1 / 0.25f));
      num = num1;
    }
    this.Scale = NConfirmButton._downScale;
    ((CanvasItem) this._buttonImage).Modulate = this._downColor;
    ((CanvasItem) this._outline).Modulate = this._outlineTransparentColor;
  }

  public void OverrideHotkeys(string[] hotkeys) => this._hotkeys = hotkeys;

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(9)
    {
      new MethodInfo(NConfirmButton.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NConfirmButton.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NConfirmButton.MethodName.OnWindowChange, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NConfirmButton.MethodName.OnEnable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NConfirmButton.MethodName.OnDisable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NConfirmButton.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NConfirmButton.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NConfirmButton.MethodName.OnPress, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NConfirmButton.MethodName.OverrideHotkeys, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 34L, StringName.op_Implicit("hotkeys"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NConfirmButton.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NConfirmButton.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NConfirmButton.MethodName.OnWindowChange) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnWindowChange();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NConfirmButton.MethodName.OnEnable) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnEnable();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NConfirmButton.MethodName.OnDisable) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnDisable();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NConfirmButton.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NConfirmButton.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NConfirmButton.MethodName.OnPress) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPress();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NConfirmButton.MethodName.OverrideHotkeys) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OverrideHotkeys(VariantUtils.ConvertTo<string[]>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NConfirmButton.MethodName._Ready) || StringName.op_Equality(ref method, NConfirmButton.MethodName._ExitTree) || StringName.op_Equality(ref method, NConfirmButton.MethodName.OnWindowChange) || StringName.op_Equality(ref method, NConfirmButton.MethodName.OnEnable) || StringName.op_Equality(ref method, NConfirmButton.MethodName.OnDisable) || StringName.op_Equality(ref method, NConfirmButton.MethodName.OnFocus) || StringName.op_Equality(ref method, NConfirmButton.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NConfirmButton.MethodName.OnPress) || StringName.op_Equality(ref method, NConfirmButton.MethodName.OverrideHotkeys) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NConfirmButton.PropertyName._outline))
    {
      this._outline = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NConfirmButton.PropertyName._buttonImage))
    {
      this._buttonImage = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NConfirmButton.PropertyName._defaultOutlineColor))
    {
      this._defaultOutlineColor = VariantUtils.ConvertTo<Color>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NConfirmButton.PropertyName._hoveredOutlineColor))
    {
      this._hoveredOutlineColor = VariantUtils.ConvertTo<Color>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NConfirmButton.PropertyName._downColor))
    {
      this._downColor = VariantUtils.ConvertTo<Color>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NConfirmButton.PropertyName._outlineColor))
    {
      this._outlineColor = VariantUtils.ConvertTo<Color>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NConfirmButton.PropertyName._outlineTransparentColor))
    {
      this._outlineTransparentColor = VariantUtils.ConvertTo<Color>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NConfirmButton.PropertyName._viewport))
    {
      this._viewport = VariantUtils.ConvertTo<Viewport>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NConfirmButton.PropertyName._hotkeys))
    {
      this._hotkeys = VariantUtils.ConvertTo<string[]>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NConfirmButton.PropertyName._posOffset))
    {
      this._posOffset = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NConfirmButton.PropertyName._showPos))
    {
      this._showPos = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NConfirmButton.PropertyName._hidePos))
    {
      this._hidePos = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NConfirmButton.PropertyName._moveTween))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._moveTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NConfirmButton.PropertyName.Hotkeys))
    {
      ref godot_variant local = ref value;
      string[] hotkeys = this.Hotkeys;
      godot_variant from = VariantUtils.CreateFrom<string[]>(ref hotkeys);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NConfirmButton.PropertyName._outline))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._outline);
      return true;
    }
    if (StringName.op_Equality(ref name, NConfirmButton.PropertyName._buttonImage))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._buttonImage);
      return true;
    }
    if (StringName.op_Equality(ref name, NConfirmButton.PropertyName._defaultOutlineColor))
    {
      value = VariantUtils.CreateFrom<Color>(ref this._defaultOutlineColor);
      return true;
    }
    if (StringName.op_Equality(ref name, NConfirmButton.PropertyName._hoveredOutlineColor))
    {
      value = VariantUtils.CreateFrom<Color>(ref this._hoveredOutlineColor);
      return true;
    }
    if (StringName.op_Equality(ref name, NConfirmButton.PropertyName._downColor))
    {
      value = VariantUtils.CreateFrom<Color>(ref this._downColor);
      return true;
    }
    if (StringName.op_Equality(ref name, NConfirmButton.PropertyName._outlineColor))
    {
      value = VariantUtils.CreateFrom<Color>(ref this._outlineColor);
      return true;
    }
    if (StringName.op_Equality(ref name, NConfirmButton.PropertyName._outlineTransparentColor))
    {
      value = VariantUtils.CreateFrom<Color>(ref this._outlineTransparentColor);
      return true;
    }
    if (StringName.op_Equality(ref name, NConfirmButton.PropertyName._viewport))
    {
      value = VariantUtils.CreateFrom<Viewport>(ref this._viewport);
      return true;
    }
    if (StringName.op_Equality(ref name, NConfirmButton.PropertyName._hotkeys))
    {
      value = VariantUtils.CreateFrom<string[]>(ref this._hotkeys);
      return true;
    }
    if (StringName.op_Equality(ref name, NConfirmButton.PropertyName._posOffset))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._posOffset);
      return true;
    }
    if (StringName.op_Equality(ref name, NConfirmButton.PropertyName._showPos))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._showPos);
      return true;
    }
    if (StringName.op_Equality(ref name, NConfirmButton.PropertyName._hidePos))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._hidePos);
      return true;
    }
    if (!StringName.op_Equality(ref name, NConfirmButton.PropertyName._moveTween))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Tween>(ref this._moveTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NConfirmButton.PropertyName._outline, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NConfirmButton.PropertyName._buttonImage, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 20L, NConfirmButton.PropertyName._defaultOutlineColor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 20L, NConfirmButton.PropertyName._hoveredOutlineColor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 20L, NConfirmButton.PropertyName._downColor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 20L, NConfirmButton.PropertyName._outlineColor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 20L, NConfirmButton.PropertyName._outlineTransparentColor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NConfirmButton.PropertyName._viewport, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 34L, NConfirmButton.PropertyName._hotkeys, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 34L, NConfirmButton.PropertyName.Hotkeys, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NConfirmButton.PropertyName._posOffset, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NConfirmButton.PropertyName._showPos, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NConfirmButton.PropertyName._hidePos, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NConfirmButton.PropertyName._moveTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NConfirmButton.PropertyName._outline, Variant.From<Control>(ref this._outline));
    info.AddProperty(NConfirmButton.PropertyName._buttonImage, Variant.From<Control>(ref this._buttonImage));
    info.AddProperty(NConfirmButton.PropertyName._defaultOutlineColor, Variant.From<Color>(ref this._defaultOutlineColor));
    info.AddProperty(NConfirmButton.PropertyName._hoveredOutlineColor, Variant.From<Color>(ref this._hoveredOutlineColor));
    info.AddProperty(NConfirmButton.PropertyName._downColor, Variant.From<Color>(ref this._downColor));
    info.AddProperty(NConfirmButton.PropertyName._outlineColor, Variant.From<Color>(ref this._outlineColor));
    info.AddProperty(NConfirmButton.PropertyName._outlineTransparentColor, Variant.From<Color>(ref this._outlineTransparentColor));
    info.AddProperty(NConfirmButton.PropertyName._viewport, Variant.From<Viewport>(ref this._viewport));
    info.AddProperty(NConfirmButton.PropertyName._hotkeys, Variant.From<string[]>(ref this._hotkeys));
    info.AddProperty(NConfirmButton.PropertyName._posOffset, Variant.From<Vector2>(ref this._posOffset));
    info.AddProperty(NConfirmButton.PropertyName._showPos, Variant.From<Vector2>(ref this._showPos));
    info.AddProperty(NConfirmButton.PropertyName._hidePos, Variant.From<Vector2>(ref this._hidePos));
    info.AddProperty(NConfirmButton.PropertyName._moveTween, Variant.From<Tween>(ref this._moveTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NConfirmButton.PropertyName._outline, ref variant1))
      this._outline = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NConfirmButton.PropertyName._buttonImage, ref variant2))
      this._buttonImage = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NConfirmButton.PropertyName._defaultOutlineColor, ref variant3))
      this._defaultOutlineColor = ((Variant) ref variant3).As<Color>();
    Variant variant4;
    if (info.TryGetProperty(NConfirmButton.PropertyName._hoveredOutlineColor, ref variant4))
      this._hoveredOutlineColor = ((Variant) ref variant4).As<Color>();
    Variant variant5;
    if (info.TryGetProperty(NConfirmButton.PropertyName._downColor, ref variant5))
      this._downColor = ((Variant) ref variant5).As<Color>();
    Variant variant6;
    if (info.TryGetProperty(NConfirmButton.PropertyName._outlineColor, ref variant6))
      this._outlineColor = ((Variant) ref variant6).As<Color>();
    Variant variant7;
    if (info.TryGetProperty(NConfirmButton.PropertyName._outlineTransparentColor, ref variant7))
      this._outlineTransparentColor = ((Variant) ref variant7).As<Color>();
    Variant variant8;
    if (info.TryGetProperty(NConfirmButton.PropertyName._viewport, ref variant8))
      this._viewport = ((Variant) ref variant8).As<Viewport>();
    Variant variant9;
    if (info.TryGetProperty(NConfirmButton.PropertyName._hotkeys, ref variant9))
      this._hotkeys = ((Variant) ref variant9).As<string[]>();
    Variant variant10;
    if (info.TryGetProperty(NConfirmButton.PropertyName._posOffset, ref variant10))
      this._posOffset = ((Variant) ref variant10).As<Vector2>();
    Variant variant11;
    if (info.TryGetProperty(NConfirmButton.PropertyName._showPos, ref variant11))
      this._showPos = ((Variant) ref variant11).As<Vector2>();
    Variant variant12;
    if (info.TryGetProperty(NConfirmButton.PropertyName._hidePos, ref variant12))
      this._hidePos = ((Variant) ref variant12).As<Vector2>();
    Variant variant13;
    if (!info.TryGetProperty(NConfirmButton.PropertyName._moveTween, ref variant13))
      return;
    this._moveTween = ((Variant) ref variant13).As<Tween>();
  }

  public new class MethodName : NButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName OnWindowChange = StringName.op_Implicit(nameof (OnWindowChange));
    public new static readonly StringName OnEnable = StringName.op_Implicit(nameof (OnEnable));
    public new static readonly StringName OnDisable = StringName.op_Implicit(nameof (OnDisable));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public new static readonly StringName OnPress = StringName.op_Implicit(nameof (OnPress));
    public static readonly StringName OverrideHotkeys = StringName.op_Implicit(nameof (OverrideHotkeys));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public new static readonly StringName Hotkeys = StringName.op_Implicit(nameof (Hotkeys));
    public static readonly StringName _outline = StringName.op_Implicit(nameof (_outline));
    public static readonly StringName _buttonImage = StringName.op_Implicit(nameof (_buttonImage));
    public static readonly StringName _defaultOutlineColor = StringName.op_Implicit(nameof (_defaultOutlineColor));
    public static readonly StringName _hoveredOutlineColor = StringName.op_Implicit(nameof (_hoveredOutlineColor));
    public static readonly StringName _downColor = StringName.op_Implicit(nameof (_downColor));
    public static readonly StringName _outlineColor = StringName.op_Implicit(nameof (_outlineColor));
    public static readonly StringName _outlineTransparentColor = StringName.op_Implicit(nameof (_outlineTransparentColor));
    public static readonly StringName _viewport = StringName.op_Implicit(nameof (_viewport));
    public static readonly StringName _hotkeys = StringName.op_Implicit(nameof (_hotkeys));
    public static readonly StringName _posOffset = StringName.op_Implicit(nameof (_posOffset));
    public static readonly StringName _showPos = StringName.op_Implicit(nameof (_showPos));
    public static readonly StringName _hidePos = StringName.op_Implicit(nameof (_hidePos));
    public static readonly StringName _moveTween = StringName.op_Implicit(nameof (_moveTween));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
