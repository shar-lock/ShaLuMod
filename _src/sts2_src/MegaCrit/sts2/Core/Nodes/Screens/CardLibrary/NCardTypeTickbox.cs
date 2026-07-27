// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary.NCardTypeTickbox
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary;

[ScriptPath("res://src/Core/Nodes/Screens/CardLibrary/NCardTypeTickbox.cs")]
public class NCardTypeTickbox : NButton
{
  private static readonly StringName _v = new StringName("v");
  private static readonly StringName _s = new StringName("s");
  private bool _isTicked = true;
  private float _baseS = 1f;
  private float _baseV = 1.2f;
  private Control _outline;
  private Control _image;
  private ShaderMaterial _hsv;
  private Tween? _tween;
  private Vector2 _baseScale;
  private float _hoverScale = 1.2f;
  private float _pressDownScale = 0.9f;
  private 
  #nullable disable
  NCardTypeTickbox.ToggledEventHandler backing_Toggled;

  public bool IsTicked
  {
    get => this._isTicked;
    set
    {
      this._isTicked = value;
      ((CanvasItem) this._outline).Visible = this._isTicked;
      this.OnToggle();
    }
  }

  public 
  #nullable enable
  LocString Loc { get; set; }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._baseScale = this.Scale;
    this._image = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Image"));
    this._outline = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Outline"));
    this._hsv = (ShaderMaterial) ((CanvasItem) this._image).Material;
  }

  private void OnToggle()
  {
    this._baseS = this._isTicked ? 1f : 0.65f;
    this._baseV = this._isTicked ? 1.2f : 0.7f;
    this.UpdateShaderS(this._baseS);
    this.UpdateShaderV(this._baseV);
  }

  protected override void OnRelease()
  {
    base.OnRelease();
    this.IsTicked = !this.IsTicked;
    ((GodotObject) this).EmitSignal(NCardTypeTickbox.SignalName.Toggled, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) this)
    });
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._image, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(this._baseScale, this._hoverScale)), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NCardTypeTickbox._s), Variant.op_Implicit(this._baseS), 0.05);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NCardTypeTickbox._v), Variant.op_Implicit(this._baseV), 0.05);
  }

  protected override void OnFocus()
  {
    base.OnFocus();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._image, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(this._baseScale, this._hoverScale)), 0.05);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NCardTypeTickbox._s), Variant.op_Implicit(this._isTicked ? 1.4f : 1f), 0.05);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NCardTypeTickbox._v), Variant.op_Implicit(this._isTicked ? 1.4f : 1f), 0.05);
    NHoverTipSet.CreateAndShow((Control) this, (IHoverTip) new HoverTip(this.Loc))?.SetGlobalPosition(new Vector2(310f, this.GlobalPosition.Y), false);
  }

  protected override void OnUnfocus()
  {
    base.OnUnfocus();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._image, NodePath.op_Implicit("scale"), Variant.op_Implicit(this._baseScale), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NCardTypeTickbox._s), Variant.op_Implicit(this._baseS), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NCardTypeTickbox._v), Variant.op_Implicit(this._baseV), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    NHoverTipSet.Remove((Control) this);
  }

  protected override void OnPress()
  {
    base.OnPress();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._image, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(this._baseScale, this._pressDownScale)), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderS)), this._hsv.GetShaderParameter(NCardTypeTickbox._s), Variant.op_Implicit(this._baseS), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NCardTypeTickbox._v), Variant.op_Implicit(this._baseV), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  private void UpdateShaderS(float value)
  {
    this._hsv.SetShaderParameter(NCardTypeTickbox._s, Variant.op_Implicit(value));
  }

  private void UpdateShaderV(float value)
  {
    this._hsv.SetShaderParameter(NCardTypeTickbox._v, Variant.op_Implicit(value));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(8)
    {
      new MethodInfo(NCardTypeTickbox.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardTypeTickbox.MethodName.OnToggle, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardTypeTickbox.MethodName.OnRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardTypeTickbox.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardTypeTickbox.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardTypeTickbox.MethodName.OnPress, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardTypeTickbox.MethodName.UpdateShaderS, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("value"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCardTypeTickbox.MethodName.UpdateShaderV, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
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
    if (StringName.op_Equality(ref method, NCardTypeTickbox.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardTypeTickbox.MethodName.OnToggle) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnToggle();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardTypeTickbox.MethodName.OnRelease) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnRelease();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardTypeTickbox.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardTypeTickbox.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardTypeTickbox.MethodName.OnPress) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPress();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardTypeTickbox.MethodName.UpdateShaderS) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateShaderS(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCardTypeTickbox.MethodName.UpdateShaderV) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.UpdateShaderV(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCardTypeTickbox.MethodName._Ready) || StringName.op_Equality(ref method, NCardTypeTickbox.MethodName.OnToggle) || StringName.op_Equality(ref method, NCardTypeTickbox.MethodName.OnRelease) || StringName.op_Equality(ref method, NCardTypeTickbox.MethodName.OnFocus) || StringName.op_Equality(ref method, NCardTypeTickbox.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NCardTypeTickbox.MethodName.OnPress) || StringName.op_Equality(ref method, NCardTypeTickbox.MethodName.UpdateShaderS) || StringName.op_Equality(ref method, NCardTypeTickbox.MethodName.UpdateShaderV) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardTypeTickbox.PropertyName.IsTicked))
    {
      this.IsTicked = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTypeTickbox.PropertyName._isTicked))
    {
      this._isTicked = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTypeTickbox.PropertyName._baseS))
    {
      this._baseS = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTypeTickbox.PropertyName._baseV))
    {
      this._baseV = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTypeTickbox.PropertyName._outline))
    {
      this._outline = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTypeTickbox.PropertyName._image))
    {
      this._image = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTypeTickbox.PropertyName._hsv))
    {
      this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTypeTickbox.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTypeTickbox.PropertyName._baseScale))
    {
      this._baseScale = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTypeTickbox.PropertyName._hoverScale))
    {
      this._hoverScale = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardTypeTickbox.PropertyName._pressDownScale))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._pressDownScale = VariantUtils.ConvertTo<float>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardTypeTickbox.PropertyName.IsTicked))
    {
      ref godot_variant local = ref value;
      bool isTicked = this.IsTicked;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref isTicked);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTypeTickbox.PropertyName._isTicked))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isTicked);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTypeTickbox.PropertyName._baseS))
    {
      value = VariantUtils.CreateFrom<float>(ref this._baseS);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTypeTickbox.PropertyName._baseV))
    {
      value = VariantUtils.CreateFrom<float>(ref this._baseV);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTypeTickbox.PropertyName._outline))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._outline);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTypeTickbox.PropertyName._image))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._image);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTypeTickbox.PropertyName._hsv))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTypeTickbox.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTypeTickbox.PropertyName._baseScale))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._baseScale);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardTypeTickbox.PropertyName._hoverScale))
    {
      value = VariantUtils.CreateFrom<float>(ref this._hoverScale);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardTypeTickbox.PropertyName._pressDownScale))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<float>(ref this._pressDownScale);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 1L, NCardTypeTickbox.PropertyName._isTicked, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NCardTypeTickbox.PropertyName._baseS, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NCardTypeTickbox.PropertyName._baseV, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCardTypeTickbox.PropertyName.IsTicked, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardTypeTickbox.PropertyName._outline, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardTypeTickbox.PropertyName._image, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardTypeTickbox.PropertyName._hsv, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardTypeTickbox.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NCardTypeTickbox.PropertyName._baseScale, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NCardTypeTickbox.PropertyName._hoverScale, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NCardTypeTickbox.PropertyName._pressDownScale, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName isTicked1 = NCardTypeTickbox.PropertyName.IsTicked;
    bool isTicked2 = this.IsTicked;
    Variant variant = Variant.From<bool>(ref isTicked2);
    serializationInfo.AddProperty(isTicked1, variant);
    info.AddProperty(NCardTypeTickbox.PropertyName._isTicked, Variant.From<bool>(ref this._isTicked));
    info.AddProperty(NCardTypeTickbox.PropertyName._baseS, Variant.From<float>(ref this._baseS));
    info.AddProperty(NCardTypeTickbox.PropertyName._baseV, Variant.From<float>(ref this._baseV));
    info.AddProperty(NCardTypeTickbox.PropertyName._outline, Variant.From<Control>(ref this._outline));
    info.AddProperty(NCardTypeTickbox.PropertyName._image, Variant.From<Control>(ref this._image));
    info.AddProperty(NCardTypeTickbox.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
    info.AddProperty(NCardTypeTickbox.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NCardTypeTickbox.PropertyName._baseScale, Variant.From<Vector2>(ref this._baseScale));
    info.AddProperty(NCardTypeTickbox.PropertyName._hoverScale, Variant.From<float>(ref this._hoverScale));
    info.AddProperty(NCardTypeTickbox.PropertyName._pressDownScale, Variant.From<float>(ref this._pressDownScale));
    info.AddSignalEventDelegate(NCardTypeTickbox.SignalName.Toggled, (Delegate) this.backing_Toggled);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCardTypeTickbox.PropertyName.IsTicked, ref variant1))
      this.IsTicked = ((Variant) ref variant1).As<bool>();
    Variant variant2;
    if (info.TryGetProperty(NCardTypeTickbox.PropertyName._isTicked, ref variant2))
      this._isTicked = ((Variant) ref variant2).As<bool>();
    Variant variant3;
    if (info.TryGetProperty(NCardTypeTickbox.PropertyName._baseS, ref variant3))
      this._baseS = ((Variant) ref variant3).As<float>();
    Variant variant4;
    if (info.TryGetProperty(NCardTypeTickbox.PropertyName._baseV, ref variant4))
      this._baseV = ((Variant) ref variant4).As<float>();
    Variant variant5;
    if (info.TryGetProperty(NCardTypeTickbox.PropertyName._outline, ref variant5))
      this._outline = ((Variant) ref variant5).As<Control>();
    Variant variant6;
    if (info.TryGetProperty(NCardTypeTickbox.PropertyName._image, ref variant6))
      this._image = ((Variant) ref variant6).As<Control>();
    Variant variant7;
    if (info.TryGetProperty(NCardTypeTickbox.PropertyName._hsv, ref variant7))
      this._hsv = ((Variant) ref variant7).As<ShaderMaterial>();
    Variant variant8;
    if (info.TryGetProperty(NCardTypeTickbox.PropertyName._tween, ref variant8))
      this._tween = ((Variant) ref variant8).As<Tween>();
    Variant variant9;
    if (info.TryGetProperty(NCardTypeTickbox.PropertyName._baseScale, ref variant9))
      this._baseScale = ((Variant) ref variant9).As<Vector2>();
    Variant variant10;
    if (info.TryGetProperty(NCardTypeTickbox.PropertyName._hoverScale, ref variant10))
      this._hoverScale = ((Variant) ref variant10).As<float>();
    Variant variant11;
    if (info.TryGetProperty(NCardTypeTickbox.PropertyName._pressDownScale, ref variant11))
      this._pressDownScale = ((Variant) ref variant11).As<float>();
    NCardTypeTickbox.ToggledEventHandler toggledEventHandler;
    if (!info.TryGetSignalEventDelegate<NCardTypeTickbox.ToggledEventHandler>(NCardTypeTickbox.SignalName.Toggled, ref toggledEventHandler))
      return;
    this.backing_Toggled = toggledEventHandler;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<MethodInfo> GetGodotSignalList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NCardTypeTickbox.SignalName.Toggled, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("tickbox"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null)
    };
  }

  public event NCardTypeTickbox.ToggledEventHandler Toggled
  {
    add => this.backing_Toggled += value;
    remove => this.backing_Toggled -= value;
  }

  protected void EmitSignalToggled(NCardTypeTickbox tickbox)
  {
    ((GodotObject) this).EmitSignal(NCardTypeTickbox.SignalName.Toggled, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) tickbox)
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RaiseGodotClassSignalCallbacks(
    in godot_string_name signal,
    NativeVariantPtrArgs args)
  {
    if (StringName.op_Equality(ref signal, NCardTypeTickbox.SignalName.Toggled) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NCardTypeTickbox.ToggledEventHandler backingToggled = this.backing_Toggled;
      if (backingToggled == null)
        return;
      backingToggled(VariantUtils.ConvertTo<NCardTypeTickbox>(ref ((NativeVariantPtrArgs) ref args)[0]));
    }
    else
      base.RaiseGodotClassSignalCallbacks(in signal, args);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassSignal(in godot_string_name signal)
  {
    return StringName.op_Equality(ref signal, NCardTypeTickbox.SignalName.Toggled) || base.HasGodotClassSignal(in signal);
  }

  [Signal]
  public delegate void ToggledEventHandler(
  #nullable enable
  NCardTypeTickbox tickbox);

  public new class MethodName : NButton.MethodName
  {
    public new static readonly 
    #nullable disable
    StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnToggle = StringName.op_Implicit(nameof (OnToggle));
    public new static readonly StringName OnRelease = StringName.op_Implicit(nameof (OnRelease));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public new static readonly StringName OnPress = StringName.op_Implicit(nameof (OnPress));
    public static readonly StringName UpdateShaderS = StringName.op_Implicit(nameof (UpdateShaderS));
    public static readonly StringName UpdateShaderV = StringName.op_Implicit(nameof (UpdateShaderV));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public static readonly StringName IsTicked = StringName.op_Implicit(nameof (IsTicked));
    public static readonly StringName _isTicked = StringName.op_Implicit(nameof (_isTicked));
    public static readonly StringName _baseS = StringName.op_Implicit(nameof (_baseS));
    public static readonly StringName _baseV = StringName.op_Implicit(nameof (_baseV));
    public static readonly StringName _outline = StringName.op_Implicit(nameof (_outline));
    public static readonly StringName _image = StringName.op_Implicit(nameof (_image));
    public static readonly StringName _hsv = StringName.op_Implicit(nameof (_hsv));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _baseScale = StringName.op_Implicit(nameof (_baseScale));
    public static readonly StringName _hoverScale = StringName.op_Implicit(nameof (_hoverScale));
    public static readonly StringName _pressDownScale = StringName.op_Implicit(nameof (_pressDownScale));
  }

  public new class SignalName : NButton.SignalName
  {
    public static readonly StringName Toggled = StringName.op_Implicit(nameof (Toggled));
  }
}
