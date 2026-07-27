// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.DailyRun.NLeaderboardPageArrow
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
namespace MegaCrit.Sts2.Core.Nodes.Screens.DailyRun;

[ScriptPath("res://src/Core/Nodes/Screens/DailyRun/NLeaderboardPageArrow.cs")]
public class NLeaderboardPageArrow : NButton
{
  [Export]
  private bool _isLeftArrow = true;
  private static readonly StringName _v = new StringName("v");
  private TextureRect _image;
  private ShaderMaterial _hsv;
  private Tween? _tween;
  private Vector2 _baseScale;
  private Action? _onRelease;
  private string[] _hotkeys = Array.Empty<string>();

  protected override string[] Hotkeys => this._hotkeys;

  public override void _Ready()
  {
    string[] strArray;
    if (!this._isLeftArrow)
      strArray = new string[1]
      {
        StringName.op_Implicit(MegaInput.viewDiscardPile)
      };
    else
      strArray = new string[1]
      {
        StringName.op_Implicit(MegaInput.viewDrawPile)
      };
    this._hotkeys = strArray;
    this.ConnectSignals();
    this._image = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("Image"));
    this._baseScale = ((Control) this._image).Scale;
    this._hsv = (ShaderMaterial) ((CanvasItem) this._image).Material;
  }

  protected override void GetControllerIconNode()
  {
    this._controllerHotkeyIcon = ((Node) this).GetNodeOrNull<TextureRect>(NodePath.op_Implicit("ControllerIcon"));
  }

  public void Connect(Action onRelease) => this._onRelease = onRelease;

  protected override void OnDisable() => ((CanvasItem) this).Modulate = StsColors.exhaustGray;

  protected override void OnEnable() => ((CanvasItem) this).Modulate = Colors.White;

  protected override void OnRelease()
  {
    Action onRelease = this._onRelease;
    if (onRelease != null)
      onRelease();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._image, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(this._baseScale, 1.1f)), 0.05);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NLeaderboardPageArrow._v), Variant.op_Implicit(1.5f), 0.05);
  }

  protected override void OnFocus()
  {
    base.OnFocus();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._image, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(this._baseScale, 1.1f)), 0.05);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NLeaderboardPageArrow._v), Variant.op_Implicit(1.5f), 0.05);
  }

  protected override void OnUnfocus()
  {
    base.OnUnfocus();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._image, NodePath.op_Implicit("scale"), Variant.op_Implicit(this._baseScale), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NLeaderboardPageArrow._v), Variant.op_Implicit(0.9f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  protected override void OnPress()
  {
    base.OnPress();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._image, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(this._baseScale, 0.9f)), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NLeaderboardPageArrow._v), Variant.op_Implicit(0.8f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  private void UpdateShaderV(float value)
  {
    this._hsv.SetShaderParameter(NLeaderboardPageArrow._v, Variant.op_Implicit(value));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(9)
    {
      new MethodInfo(NLeaderboardPageArrow.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NLeaderboardPageArrow.MethodName.GetControllerIconNode, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NLeaderboardPageArrow.MethodName.OnDisable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NLeaderboardPageArrow.MethodName.OnEnable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NLeaderboardPageArrow.MethodName.OnRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NLeaderboardPageArrow.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NLeaderboardPageArrow.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NLeaderboardPageArrow.MethodName.OnPress, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NLeaderboardPageArrow.MethodName.UpdateShaderV, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
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
    if (StringName.op_Equality(ref method, NLeaderboardPageArrow.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NLeaderboardPageArrow.MethodName.GetControllerIconNode) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.GetControllerIconNode();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NLeaderboardPageArrow.MethodName.OnDisable) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnDisable();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NLeaderboardPageArrow.MethodName.OnEnable) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnEnable();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NLeaderboardPageArrow.MethodName.OnRelease) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnRelease();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NLeaderboardPageArrow.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NLeaderboardPageArrow.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NLeaderboardPageArrow.MethodName.OnPress) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPress();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NLeaderboardPageArrow.MethodName.UpdateShaderV) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.UpdateShaderV(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NLeaderboardPageArrow.MethodName._Ready) || StringName.op_Equality(ref method, NLeaderboardPageArrow.MethodName.GetControllerIconNode) || StringName.op_Equality(ref method, NLeaderboardPageArrow.MethodName.OnDisable) || StringName.op_Equality(ref method, NLeaderboardPageArrow.MethodName.OnEnable) || StringName.op_Equality(ref method, NLeaderboardPageArrow.MethodName.OnRelease) || StringName.op_Equality(ref method, NLeaderboardPageArrow.MethodName.OnFocus) || StringName.op_Equality(ref method, NLeaderboardPageArrow.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NLeaderboardPageArrow.MethodName.OnPress) || StringName.op_Equality(ref method, NLeaderboardPageArrow.MethodName.UpdateShaderV) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NLeaderboardPageArrow.PropertyName._isLeftArrow))
    {
      this._isLeftArrow = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NLeaderboardPageArrow.PropertyName._image))
    {
      this._image = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NLeaderboardPageArrow.PropertyName._hsv))
    {
      this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NLeaderboardPageArrow.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NLeaderboardPageArrow.PropertyName._baseScale))
    {
      this._baseScale = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NLeaderboardPageArrow.PropertyName._hotkeys))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._hotkeys = VariantUtils.ConvertTo<string[]>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NLeaderboardPageArrow.PropertyName.Hotkeys))
    {
      ref godot_variant local = ref value;
      string[] hotkeys = this.Hotkeys;
      godot_variant from = VariantUtils.CreateFrom<string[]>(ref hotkeys);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NLeaderboardPageArrow.PropertyName._isLeftArrow))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isLeftArrow);
      return true;
    }
    if (StringName.op_Equality(ref name, NLeaderboardPageArrow.PropertyName._image))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._image);
      return true;
    }
    if (StringName.op_Equality(ref name, NLeaderboardPageArrow.PropertyName._hsv))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
      return true;
    }
    if (StringName.op_Equality(ref name, NLeaderboardPageArrow.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (StringName.op_Equality(ref name, NLeaderboardPageArrow.PropertyName._baseScale))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._baseScale);
      return true;
    }
    if (!StringName.op_Equality(ref name, NLeaderboardPageArrow.PropertyName._hotkeys))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<string[]>(ref this._hotkeys);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 1L, NLeaderboardPageArrow.PropertyName._isLeftArrow, (PropertyHint) 0L, "", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NLeaderboardPageArrow.PropertyName._image, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NLeaderboardPageArrow.PropertyName._hsv, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NLeaderboardPageArrow.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NLeaderboardPageArrow.PropertyName._baseScale, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 34L, NLeaderboardPageArrow.PropertyName._hotkeys, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 34L, NLeaderboardPageArrow.PropertyName.Hotkeys, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NLeaderboardPageArrow.PropertyName._isLeftArrow, Variant.From<bool>(ref this._isLeftArrow));
    info.AddProperty(NLeaderboardPageArrow.PropertyName._image, Variant.From<TextureRect>(ref this._image));
    info.AddProperty(NLeaderboardPageArrow.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
    info.AddProperty(NLeaderboardPageArrow.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NLeaderboardPageArrow.PropertyName._baseScale, Variant.From<Vector2>(ref this._baseScale));
    info.AddProperty(NLeaderboardPageArrow.PropertyName._hotkeys, Variant.From<string[]>(ref this._hotkeys));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NLeaderboardPageArrow.PropertyName._isLeftArrow, ref variant1))
      this._isLeftArrow = ((Variant) ref variant1).As<bool>();
    Variant variant2;
    if (info.TryGetProperty(NLeaderboardPageArrow.PropertyName._image, ref variant2))
      this._image = ((Variant) ref variant2).As<TextureRect>();
    Variant variant3;
    if (info.TryGetProperty(NLeaderboardPageArrow.PropertyName._hsv, ref variant3))
      this._hsv = ((Variant) ref variant3).As<ShaderMaterial>();
    Variant variant4;
    if (info.TryGetProperty(NLeaderboardPageArrow.PropertyName._tween, ref variant4))
      this._tween = ((Variant) ref variant4).As<Tween>();
    Variant variant5;
    if (info.TryGetProperty(NLeaderboardPageArrow.PropertyName._baseScale, ref variant5))
      this._baseScale = ((Variant) ref variant5).As<Vector2>();
    Variant variant6;
    if (!info.TryGetProperty(NLeaderboardPageArrow.PropertyName._hotkeys, ref variant6))
      return;
    this._hotkeys = ((Variant) ref variant6).As<string[]>();
  }

  public new class MethodName : NButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName GetControllerIconNode = StringName.op_Implicit(nameof (GetControllerIconNode));
    public new static readonly StringName OnDisable = StringName.op_Implicit(nameof (OnDisable));
    public new static readonly StringName OnEnable = StringName.op_Implicit(nameof (OnEnable));
    public new static readonly StringName OnRelease = StringName.op_Implicit(nameof (OnRelease));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public new static readonly StringName OnPress = StringName.op_Implicit(nameof (OnPress));
    public static readonly StringName UpdateShaderV = StringName.op_Implicit(nameof (UpdateShaderV));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public new static readonly StringName Hotkeys = StringName.op_Implicit(nameof (Hotkeys));
    public static readonly StringName _isLeftArrow = StringName.op_Implicit(nameof (_isLeftArrow));
    public static readonly StringName _image = StringName.op_Implicit(nameof (_image));
    public static readonly StringName _hsv = StringName.op_Implicit(nameof (_hsv));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _baseScale = StringName.op_Implicit(nameof (_baseScale));
    public static readonly StringName _hotkeys = StringName.op_Implicit(nameof (_hotkeys));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
