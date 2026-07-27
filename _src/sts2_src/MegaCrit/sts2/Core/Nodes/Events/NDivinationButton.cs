// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Events.NDivinationButton
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Events;

[ScriptPath("res://src/Core/Nodes/Events/NDivinationButton.cs")]
public class NDivinationButton : NButton
{
  private static readonly StringName _v = new StringName("v");
  private static readonly StringName _s = new StringName("s");
  private ShaderMaterial _hsv;
  private Tween? _tween;
  private Tween? _outlineTween;
  private MegaLabel _label;
  private Control _outline;
  private string[] _customHotkeys = Array.Empty<string>();

  public override void _Ready()
  {
    this.ConnectSignals();
    this._hsv = (ShaderMaterial) ((CanvasItem) ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("%ButtonImage"))).Material;
    this._outline = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Outline"));
    this._label = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%Label"));
  }

  protected override string[] Hotkeys => this._customHotkeys;

  public void SetLabel(LocString loc) => this._label.SetTextAutoSize(loc.GetFormattedText());

  protected override void OnFocus()
  {
    if (((CanvasItem) this._outline).Visible)
      return;
    base.OnFocus();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NDivinationButton._s), Variant.op_Implicit(1f), 0.05).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NDivinationButton._v), Variant.op_Implicit(1f), 0.05).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  protected override void OnUnfocus()
  {
    if (((CanvasItem) this._outline).Visible)
      return;
    base.OnUnfocus();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NDivinationButton._s), Variant.op_Implicit(0.9f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NDivinationButton._v), Variant.op_Implicit(0.8f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  private void UpdateShaderS(float value)
  {
    this._hsv.SetShaderParameter(NDivinationButton._s, Variant.op_Implicit(value));
  }

  private void UpdateShaderV(float value)
  {
    this._hsv.SetShaderParameter(NDivinationButton._v, Variant.op_Implicit(value));
  }

  public void SetActive(bool isActive) => ((CanvasItem) this._outline).Visible = isActive;

  public void SetHotkeys(string[] hotkeys)
  {
    this.UnregisterHotkeys();
    this._customHotkeys = hotkeys;
    this.RegisterHotkeys();
    this.UpdateControllerButton();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(7)
    {
      new MethodInfo(NDivinationButton.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDivinationButton.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDivinationButton.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDivinationButton.MethodName.UpdateShaderS, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("value"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NDivinationButton.MethodName.UpdateShaderV, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("value"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NDivinationButton.MethodName.SetActive, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("isActive"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NDivinationButton.MethodName.SetHotkeys, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
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
    if (StringName.op_Equality(ref method, NDivinationButton.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDivinationButton.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDivinationButton.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDivinationButton.MethodName.UpdateShaderS) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateShaderS(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDivinationButton.MethodName.UpdateShaderV) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateShaderV(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDivinationButton.MethodName.SetActive) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetActive(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NDivinationButton.MethodName.SetHotkeys) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.SetHotkeys(VariantUtils.ConvertTo<string[]>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NDivinationButton.MethodName._Ready) || StringName.op_Equality(ref method, NDivinationButton.MethodName.OnFocus) || StringName.op_Equality(ref method, NDivinationButton.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NDivinationButton.MethodName.UpdateShaderS) || StringName.op_Equality(ref method, NDivinationButton.MethodName.UpdateShaderV) || StringName.op_Equality(ref method, NDivinationButton.MethodName.SetActive) || StringName.op_Equality(ref method, NDivinationButton.MethodName.SetHotkeys) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDivinationButton.PropertyName._hsv))
    {
      this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDivinationButton.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDivinationButton.PropertyName._outlineTween))
    {
      this._outlineTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDivinationButton.PropertyName._label))
    {
      this._label = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDivinationButton.PropertyName._outline))
    {
      this._outline = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDivinationButton.PropertyName._customHotkeys))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._customHotkeys = VariantUtils.ConvertTo<string[]>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDivinationButton.PropertyName.Hotkeys))
    {
      ref godot_variant local = ref value;
      string[] hotkeys = this.Hotkeys;
      godot_variant from = VariantUtils.CreateFrom<string[]>(ref hotkeys);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NDivinationButton.PropertyName._hsv))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
      return true;
    }
    if (StringName.op_Equality(ref name, NDivinationButton.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (StringName.op_Equality(ref name, NDivinationButton.PropertyName._outlineTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._outlineTween);
      return true;
    }
    if (StringName.op_Equality(ref name, NDivinationButton.PropertyName._label))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._label);
      return true;
    }
    if (StringName.op_Equality(ref name, NDivinationButton.PropertyName._outline))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._outline);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDivinationButton.PropertyName._customHotkeys))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<string[]>(ref this._customHotkeys);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NDivinationButton.PropertyName._hsv, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDivinationButton.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDivinationButton.PropertyName._outlineTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDivinationButton.PropertyName._label, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDivinationButton.PropertyName._outline, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 34L, NDivinationButton.PropertyName._customHotkeys, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 34L, NDivinationButton.PropertyName.Hotkeys, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NDivinationButton.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
    info.AddProperty(NDivinationButton.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NDivinationButton.PropertyName._outlineTween, Variant.From<Tween>(ref this._outlineTween));
    info.AddProperty(NDivinationButton.PropertyName._label, Variant.From<MegaLabel>(ref this._label));
    info.AddProperty(NDivinationButton.PropertyName._outline, Variant.From<Control>(ref this._outline));
    info.AddProperty(NDivinationButton.PropertyName._customHotkeys, Variant.From<string[]>(ref this._customHotkeys));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NDivinationButton.PropertyName._hsv, ref variant1))
      this._hsv = ((Variant) ref variant1).As<ShaderMaterial>();
    Variant variant2;
    if (info.TryGetProperty(NDivinationButton.PropertyName._tween, ref variant2))
      this._tween = ((Variant) ref variant2).As<Tween>();
    Variant variant3;
    if (info.TryGetProperty(NDivinationButton.PropertyName._outlineTween, ref variant3))
      this._outlineTween = ((Variant) ref variant3).As<Tween>();
    Variant variant4;
    if (info.TryGetProperty(NDivinationButton.PropertyName._label, ref variant4))
      this._label = ((Variant) ref variant4).As<MegaLabel>();
    Variant variant5;
    if (info.TryGetProperty(NDivinationButton.PropertyName._outline, ref variant5))
      this._outline = ((Variant) ref variant5).As<Control>();
    Variant variant6;
    if (!info.TryGetProperty(NDivinationButton.PropertyName._customHotkeys, ref variant6))
      return;
    this._customHotkeys = ((Variant) ref variant6).As<string[]>();
  }

  public new class MethodName : NButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public static readonly StringName UpdateShaderS = StringName.op_Implicit(nameof (UpdateShaderS));
    public static readonly StringName UpdateShaderV = StringName.op_Implicit(nameof (UpdateShaderV));
    public static readonly StringName SetActive = StringName.op_Implicit(nameof (SetActive));
    public static readonly StringName SetHotkeys = StringName.op_Implicit(nameof (SetHotkeys));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public new static readonly StringName Hotkeys = StringName.op_Implicit(nameof (Hotkeys));
    public static readonly StringName _hsv = StringName.op_Implicit(nameof (_hsv));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _outlineTween = StringName.op_Implicit(nameof (_outlineTween));
    public static readonly StringName _label = StringName.op_Implicit(nameof (_label));
    public static readonly StringName _outline = StringName.op_Implicit(nameof (_outline));
    public static readonly StringName _customHotkeys = StringName.op_Implicit(nameof (_customHotkeys));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
