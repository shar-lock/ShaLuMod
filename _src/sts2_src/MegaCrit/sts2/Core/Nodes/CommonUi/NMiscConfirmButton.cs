// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.CommonUi.NMiscConfirmButton
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.CommonUi;

[ScriptPath("res://src/Core/Nodes/CommonUi/NMiscConfirmButton.cs")]
public class NMiscConfirmButton : NButton
{
  private Control _buttonImage;
  private Color _downColor = Colors.Gray;
  private static readonly Vector2 _hoverScale = new Vector2(1.05f, 1.05f);
  private static readonly Vector2 _downScale = new Vector2(0.95f, 0.95f);
  private const float _pressDownDur = 0.25f;
  private const float _unhoverAnimDur = 0.5f;
  private const float _animInOutDur = 0.35f;
  private Vector2 _showPos;
  private Vector2 _hidePos;
  private Tween? _moveTween;
  private CancellationTokenSource? _pressDownCancelToken;
  private CancellationTokenSource? _unhoverAnimCancelToken;

  public override void _Ready()
  {
    this.ConnectSignals();
    this._isEnabled = false;
    this._buttonImage = ((Node) this).GetNode<Control>(NodePath.op_Implicit("Image"));
    ((GodotObject) ((Node) this).GetTree().Root).Connect(Viewport.SignalName.SizeChanged, Callable.From(new Action(this.OnWindowChange)), 0U);
    this.OnWindowChange();
  }

  public override void _ExitTree()
  {
    base._ExitTree();
    this._pressDownCancelToken?.Cancel();
    this._unhoverAnimCancelToken?.Cancel();
  }

  private void OnWindowChange()
  {
    this._showPos = this.Position;
    this._hidePos = Vector2.op_Addition(this.Position, new Vector2(0.0f, 64f));
  }

  protected override void OnEnable()
  {
    ((CanvasItem) this._buttonImage).Modulate = Colors.White;
    this._moveTween?.Kill();
    this._moveTween = ((Node) this).CreateTween();
    this._moveTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("position"), Variant.op_Implicit(this._showPos), 0.34999999403953552).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L).From(Variant.op_Implicit(this._hidePos));
  }

  protected override void OnDisable()
  {
    this._moveTween?.Kill();
    this._moveTween = ((Node) this).CreateTween();
    this._moveTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("position"), Variant.op_Implicit(this._hidePos), 0.34999999403953552).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L).From(Variant.op_Implicit(this._showPos));
  }

  protected override void OnFocus()
  {
    base.OnFocus();
    this._unhoverAnimCancelToken?.Cancel();
    this.Scale = NMiscConfirmButton._hoverScale;
    ((CanvasItem) this._buttonImage).Modulate = Colors.White;
  }

  protected override void OnUnfocus()
  {
    this._pressDownCancelToken?.Cancel();
    this._unhoverAnimCancelToken = new CancellationTokenSource();
    TaskHelper.RunSafely(this.AnimUnhover(this._unhoverAnimCancelToken));
  }

  private async Task AnimUnhover(CancellationTokenSource cancelToken)
  {
    float num1 = 0.0f;
    Vector2 startScale = this.Scale;
    Color startButtonColor = ((CanvasItem) this._buttonImage).Modulate;
    float num;
    for (; (double) num1 < 0.5; num1 = num + await ((Node) this).AwaitProcessFrame())
    {
      if (cancelToken.IsCancellationRequested)
        return;
      this.Scale = ((Vector2) ref startScale).Lerp(Vector2.One, Ease.ExpoOut(num1 / 0.5f));
      ((CanvasItem) this._buttonImage).Modulate = ((Color) ref startButtonColor).Lerp(Colors.White, Ease.ExpoOut(num1 / 0.5f));
      num = num1;
    }
    this.Scale = Vector2.One;
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
    this.Scale = NMiscConfirmButton._hoverScale;
    float num;
    for (; (double) num1 < 0.25; num1 = num + await ((Node) this).AwaitProcessFrame())
    {
      if (cancelToken.IsCancellationRequested)
        return;
      this.Scale = ((Vector2) ref NMiscConfirmButton._hoverScale).Lerp(NMiscConfirmButton._downScale, Ease.CubicOut(num1 / 0.25f));
      Control buttonImage = this._buttonImage;
      Color white = Colors.White;
      Color color = ((Color) ref white).Lerp(this._downColor, Ease.CubicOut(num1 / 0.25f));
      ((CanvasItem) buttonImage).Modulate = color;
      num = num1;
    }
    this.Scale = NMiscConfirmButton._downScale;
    ((CanvasItem) this._buttonImage).Modulate = this._downColor;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(8)
    {
      new MethodInfo(NMiscConfirmButton.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMiscConfirmButton.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMiscConfirmButton.MethodName.OnWindowChange, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMiscConfirmButton.MethodName.OnEnable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMiscConfirmButton.MethodName.OnDisable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMiscConfirmButton.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMiscConfirmButton.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMiscConfirmButton.MethodName.OnPress, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMiscConfirmButton.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMiscConfirmButton.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMiscConfirmButton.MethodName.OnWindowChange) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnWindowChange();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMiscConfirmButton.MethodName.OnEnable) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnEnable();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMiscConfirmButton.MethodName.OnDisable) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnDisable();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMiscConfirmButton.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMiscConfirmButton.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NMiscConfirmButton.MethodName.OnPress) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnPress();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMiscConfirmButton.MethodName._Ready) || StringName.op_Equality(ref method, NMiscConfirmButton.MethodName._ExitTree) || StringName.op_Equality(ref method, NMiscConfirmButton.MethodName.OnWindowChange) || StringName.op_Equality(ref method, NMiscConfirmButton.MethodName.OnEnable) || StringName.op_Equality(ref method, NMiscConfirmButton.MethodName.OnDisable) || StringName.op_Equality(ref method, NMiscConfirmButton.MethodName.OnFocus) || StringName.op_Equality(ref method, NMiscConfirmButton.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NMiscConfirmButton.MethodName.OnPress) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMiscConfirmButton.PropertyName._buttonImage))
    {
      this._buttonImage = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMiscConfirmButton.PropertyName._downColor))
    {
      this._downColor = VariantUtils.ConvertTo<Color>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMiscConfirmButton.PropertyName._showPos))
    {
      this._showPos = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMiscConfirmButton.PropertyName._hidePos))
    {
      this._hidePos = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMiscConfirmButton.PropertyName._moveTween))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._moveTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMiscConfirmButton.PropertyName._buttonImage))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._buttonImage);
      return true;
    }
    if (StringName.op_Equality(ref name, NMiscConfirmButton.PropertyName._downColor))
    {
      value = VariantUtils.CreateFrom<Color>(ref this._downColor);
      return true;
    }
    if (StringName.op_Equality(ref name, NMiscConfirmButton.PropertyName._showPos))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._showPos);
      return true;
    }
    if (StringName.op_Equality(ref name, NMiscConfirmButton.PropertyName._hidePos))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._hidePos);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMiscConfirmButton.PropertyName._moveTween))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Tween>(ref this._moveTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NMiscConfirmButton.PropertyName._buttonImage, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 20L, NMiscConfirmButton.PropertyName._downColor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NMiscConfirmButton.PropertyName._showPos, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NMiscConfirmButton.PropertyName._hidePos, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMiscConfirmButton.PropertyName._moveTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NMiscConfirmButton.PropertyName._buttonImage, Variant.From<Control>(ref this._buttonImage));
    info.AddProperty(NMiscConfirmButton.PropertyName._downColor, Variant.From<Color>(ref this._downColor));
    info.AddProperty(NMiscConfirmButton.PropertyName._showPos, Variant.From<Vector2>(ref this._showPos));
    info.AddProperty(NMiscConfirmButton.PropertyName._hidePos, Variant.From<Vector2>(ref this._hidePos));
    info.AddProperty(NMiscConfirmButton.PropertyName._moveTween, Variant.From<Tween>(ref this._moveTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NMiscConfirmButton.PropertyName._buttonImage, ref variant1))
      this._buttonImage = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NMiscConfirmButton.PropertyName._downColor, ref variant2))
      this._downColor = ((Variant) ref variant2).As<Color>();
    Variant variant3;
    if (info.TryGetProperty(NMiscConfirmButton.PropertyName._showPos, ref variant3))
      this._showPos = ((Variant) ref variant3).As<Vector2>();
    Variant variant4;
    if (info.TryGetProperty(NMiscConfirmButton.PropertyName._hidePos, ref variant4))
      this._hidePos = ((Variant) ref variant4).As<Vector2>();
    Variant variant5;
    if (!info.TryGetProperty(NMiscConfirmButton.PropertyName._moveTween, ref variant5))
      return;
    this._moveTween = ((Variant) ref variant5).As<Tween>();
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
  }

  public new class PropertyName : NButton.PropertyName
  {
    public static readonly StringName _buttonImage = StringName.op_Implicit(nameof (_buttonImage));
    public static readonly StringName _downColor = StringName.op_Implicit(nameof (_downColor));
    public static readonly StringName _showPos = StringName.op_Implicit(nameof (_showPos));
    public static readonly StringName _hidePos = StringName.op_Implicit(nameof (_hidePos));
    public static readonly StringName _moveTween = StringName.op_Implicit(nameof (_moveTween));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
