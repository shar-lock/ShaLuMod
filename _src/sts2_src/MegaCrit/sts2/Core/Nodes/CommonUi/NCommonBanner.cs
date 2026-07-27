// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.CommonUi.NCommonBanner
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.CommonUi;

[ScriptPath("res://src/Core/Nodes/CommonUi/NCommonBanner.cs")]
public class NCommonBanner : Control
{
  public MegaLabel label;
  private Tween? _labelTween;
  private Tween? _tween;
  private Vector2 _showPos;
  private Vector2 _hidePos;
  private static readonly Vector2 _hideOffset = new Vector2(0.0f, 50f);
  private Vector2 _imgOffset;

  public override void _Ready()
  {
    this.label = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%Label"));
    ((CanvasItem) this).Modulate = StsColors.transparentWhite;
    Rect2 viewportRect = ((CanvasItem) this).GetViewportRect();
    double num1 = (double) ((Rect2) ref viewportRect).Size.X * 0.5;
    viewportRect = ((CanvasItem) this).GetViewportRect();
    double num2 = (double) ((Rect2) ref viewportRect).Size.Y * 0.5;
    this._imgOffset = Vector2.op_Subtraction(new Vector2((float) num1, (float) num2), this.GlobalPosition);
    ((GodotObject) ((Node) this).GetTree().Root).Connect(Viewport.SignalName.SizeChanged, Callable.From(new Action(this.OnWindowChange)), 0U);
    this.OnWindowChange();
  }

  private void OnWindowChange()
  {
    Rect2 viewportRect = ((CanvasItem) this).GetViewportRect();
    double num1 = (double) ((Rect2) ref viewportRect).Size.X * 0.5;
    viewportRect = ((CanvasItem) this).GetViewportRect();
    double num2 = (double) ((Rect2) ref viewportRect).Size.Y * 0.5;
    this._showPos = Vector2.op_Subtraction(new Vector2((float) num1, (float) num2), this._imgOffset);
    this._hidePos = Vector2.op_Addition(this._showPos, NCommonBanner._hideOffset);
    this.Position = this._showPos;
  }

  public void AnimateIn()
  {
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.4).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("global_position"), Variant.op_Implicit(this._showPos), 0.4).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L).From(Variant.op_Implicit(this._hidePos));
  }

  public void AnimateOut()
  {
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.4).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  public void ChangeText(string text)
  {
    this.label.SetTextAutoSize(text);
    ((CanvasItem) this.label).Modulate = StsColors.transparentWhite;
    this._labelTween?.Kill();
    this._labelTween = ((Node) this).CreateTween();
    this._labelTween.TweenProperty((GodotObject) this.label, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.25);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NCommonBanner.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCommonBanner.MethodName.OnWindowChange, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCommonBanner.MethodName.AnimateIn, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCommonBanner.MethodName.AnimateOut, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCommonBanner.MethodName.ChangeText, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("text"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCommonBanner.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCommonBanner.MethodName.OnWindowChange) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnWindowChange();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCommonBanner.MethodName.AnimateIn) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimateIn();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCommonBanner.MethodName.AnimateOut) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AnimateOut();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCommonBanner.MethodName.ChangeText) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.ChangeText(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCommonBanner.MethodName._Ready) || StringName.op_Equality(ref method, NCommonBanner.MethodName.OnWindowChange) || StringName.op_Equality(ref method, NCommonBanner.MethodName.AnimateIn) || StringName.op_Equality(ref method, NCommonBanner.MethodName.AnimateOut) || StringName.op_Equality(ref method, NCommonBanner.MethodName.ChangeText) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCommonBanner.PropertyName.label))
    {
      this.label = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCommonBanner.PropertyName._labelTween))
    {
      this._labelTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCommonBanner.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCommonBanner.PropertyName._showPos))
    {
      this._showPos = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCommonBanner.PropertyName._hidePos))
    {
      this._hidePos = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCommonBanner.PropertyName._imgOffset))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._imgOffset = VariantUtils.ConvertTo<Vector2>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCommonBanner.PropertyName.label))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this.label);
      return true;
    }
    if (StringName.op_Equality(ref name, NCommonBanner.PropertyName._labelTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._labelTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NCommonBanner.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (StringName.op_Equality(ref name, NCommonBanner.PropertyName._showPos))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._showPos);
      return true;
    }
    if (StringName.op_Equality(ref name, NCommonBanner.PropertyName._hidePos))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._hidePos);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCommonBanner.PropertyName._imgOffset))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Vector2>(ref this._imgOffset);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCommonBanner.PropertyName.label, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCommonBanner.PropertyName._labelTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCommonBanner.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NCommonBanner.PropertyName._showPos, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NCommonBanner.PropertyName._hidePos, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NCommonBanner.PropertyName._imgOffset, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NCommonBanner.PropertyName.label, Variant.From<MegaLabel>(ref this.label));
    info.AddProperty(NCommonBanner.PropertyName._labelTween, Variant.From<Tween>(ref this._labelTween));
    info.AddProperty(NCommonBanner.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NCommonBanner.PropertyName._showPos, Variant.From<Vector2>(ref this._showPos));
    info.AddProperty(NCommonBanner.PropertyName._hidePos, Variant.From<Vector2>(ref this._hidePos));
    info.AddProperty(NCommonBanner.PropertyName._imgOffset, Variant.From<Vector2>(ref this._imgOffset));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCommonBanner.PropertyName.label, ref variant1))
      this.label = ((Variant) ref variant1).As<MegaLabel>();
    Variant variant2;
    if (info.TryGetProperty(NCommonBanner.PropertyName._labelTween, ref variant2))
      this._labelTween = ((Variant) ref variant2).As<Tween>();
    Variant variant3;
    if (info.TryGetProperty(NCommonBanner.PropertyName._tween, ref variant3))
      this._tween = ((Variant) ref variant3).As<Tween>();
    Variant variant4;
    if (info.TryGetProperty(NCommonBanner.PropertyName._showPos, ref variant4))
      this._showPos = ((Variant) ref variant4).As<Vector2>();
    Variant variant5;
    if (info.TryGetProperty(NCommonBanner.PropertyName._hidePos, ref variant5))
      this._hidePos = ((Variant) ref variant5).As<Vector2>();
    Variant variant6;
    if (!info.TryGetProperty(NCommonBanner.PropertyName._imgOffset, ref variant6))
      return;
    this._imgOffset = ((Variant) ref variant6).As<Vector2>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnWindowChange = StringName.op_Implicit(nameof (OnWindowChange));
    public static readonly StringName AnimateIn = StringName.op_Implicit(nameof (AnimateIn));
    public static readonly StringName AnimateOut = StringName.op_Implicit(nameof (AnimateOut));
    public static readonly StringName ChangeText = StringName.op_Implicit(nameof (ChangeText));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName label = StringName.op_Implicit(nameof (label));
    public static readonly StringName _labelTween = StringName.op_Implicit(nameof (_labelTween));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _showPos = StringName.op_Implicit(nameof (_showPos));
    public static readonly StringName _hidePos = StringName.op_Implicit(nameof (_hidePos));
    public static readonly StringName _imgOffset = StringName.op_Implicit(nameof (_imgOffset));
  }

  public class SignalName : Control.SignalName
  {
  }
}
