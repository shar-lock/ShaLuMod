// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.CommonUi.NTickbox
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.CommonUi;

[ScriptPath("res://src/Core/Nodes/CommonUi/NTickbox.cs")]
public class NTickbox : NButton
{
  private static readonly StringName _v = new StringName("v");
  private bool _isTicked = true;
  private Control _imageContainer;
  private Control _tickedImage;
  private Control _notTickedImage;
  private ShaderMaterial _hsv;
  private Tween? _tween;
  private Vector2 _baseScale;
  private float _hoverScale = 1.05f;
  private float _pressDownScale = 0.95f;
  private float _hoverV = 1.2f;
  private 
  #nullable disable
  NTickbox.ToggledEventHandler backing_Toggled;

  public bool IsTicked
  {
    get => this._isTicked;
    set
    {
      this._isTicked = value;
      ((CanvasItem) this._tickedImage).Visible = this._isTicked;
      ((CanvasItem) this._notTickedImage).Visible = !this._isTicked;
    }
  }

  public override void _Ready()
  {
    if (((object) this).GetType() != typeof (NTickbox))
    {
      Log.Error($"{((object) this).GetType()}");
      throw new InvalidOperationException("Don't call base._Ready()! Call ConnectSignals() instead.");
    }
    this.ConnectSignals();
  }

  protected override void ConnectSignals()
  {
    base.ConnectSignals();
    this._imageContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%TickboxVisuals"));
    this._baseScale = this._imageContainer.Scale;
    this._hsv = (ShaderMaterial) ((CanvasItem) this._imageContainer).Material;
    this._tickedImage = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%TickboxVisuals/Ticked"));
    this._notTickedImage = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%TickboxVisuals/NotTicked"));
  }

  protected override void OnRelease()
  {
    base.OnRelease();
    this.ForceToggleTick();
  }

  public void ForceToggleTick()
  {
    this.IsTicked = !this.IsTicked;
    if (this.IsTicked)
    {
      SfxCmd.Play("event:/sfx/ui/clicks/ui_checkbox_on");
      this.OnTick();
    }
    else
    {
      SfxCmd.Play("event:/sfx/ui/clicks/ui_checkbox_off");
      this.OnUntick();
    }
    ((GodotObject) this).EmitSignal(NTickbox.SignalName.Toggled, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) this)
    });
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._imageContainer, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(this._baseScale, this._hoverScale)), 0.05);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NTickbox._v), Variant.op_Implicit(this._hoverV), 0.05);
  }

  protected virtual void OnUntick()
  {
  }

  protected virtual void OnTick()
  {
  }

  protected override void OnFocus()
  {
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._imageContainer, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(this._baseScale, this._hoverScale)), 0.05);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NTickbox._v), Variant.op_Implicit(this._hoverV), 0.05);
  }

  protected override void OnUnfocus()
  {
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._imageContainer, NodePath.op_Implicit("scale"), Variant.op_Implicit(this._baseScale), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NTickbox._v), Variant.op_Implicit(1f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  protected override void OnPress()
  {
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._imageContainer, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(this._baseScale, this._pressDownScale)), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    this._tween.TweenMethod(Callable.From<float>(new Action<float>(this.UpdateShaderV)), this._hsv.GetShaderParameter(NTickbox._v), Variant.op_Implicit(0.8f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  protected override void OnDisable()
  {
    base.OnDisable();
    ((CanvasItem) this).Modulate = StsColors.gray;
  }

  protected override void OnEnable()
  {
    base.OnEnable();
    ((CanvasItem) this).Modulate = Colors.White;
  }

  private void UpdateShaderV(float value)
  {
    this._hsv.SetShaderParameter(NTickbox._v, Variant.op_Implicit(value));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(12)
    {
      new MethodInfo(NTickbox.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTickbox.MethodName.ConnectSignals, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTickbox.MethodName.OnRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTickbox.MethodName.ForceToggleTick, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTickbox.MethodName.OnUntick, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTickbox.MethodName.OnTick, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTickbox.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTickbox.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTickbox.MethodName.OnPress, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTickbox.MethodName.OnDisable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTickbox.MethodName.OnEnable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NTickbox.MethodName.UpdateShaderV, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
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
    if (StringName.op_Equality(ref method, NTickbox.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTickbox.MethodName.ConnectSignals) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ConnectSignals();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTickbox.MethodName.OnRelease) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnRelease();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTickbox.MethodName.ForceToggleTick) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ForceToggleTick();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTickbox.MethodName.OnUntick) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUntick();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTickbox.MethodName.OnTick) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnTick();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTickbox.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTickbox.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTickbox.MethodName.OnPress) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPress();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTickbox.MethodName.OnDisable) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnDisable();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NTickbox.MethodName.OnEnable) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnEnable();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NTickbox.MethodName.UpdateShaderV) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.UpdateShaderV(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NTickbox.MethodName._Ready) || StringName.op_Equality(ref method, NTickbox.MethodName.ConnectSignals) || StringName.op_Equality(ref method, NTickbox.MethodName.OnRelease) || StringName.op_Equality(ref method, NTickbox.MethodName.ForceToggleTick) || StringName.op_Equality(ref method, NTickbox.MethodName.OnUntick) || StringName.op_Equality(ref method, NTickbox.MethodName.OnTick) || StringName.op_Equality(ref method, NTickbox.MethodName.OnFocus) || StringName.op_Equality(ref method, NTickbox.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NTickbox.MethodName.OnPress) || StringName.op_Equality(ref method, NTickbox.MethodName.OnDisable) || StringName.op_Equality(ref method, NTickbox.MethodName.OnEnable) || StringName.op_Equality(ref method, NTickbox.MethodName.UpdateShaderV) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NTickbox.PropertyName.IsTicked))
    {
      this.IsTicked = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTickbox.PropertyName._isTicked))
    {
      this._isTicked = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTickbox.PropertyName._imageContainer))
    {
      this._imageContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTickbox.PropertyName._tickedImage))
    {
      this._tickedImage = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTickbox.PropertyName._notTickedImage))
    {
      this._notTickedImage = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTickbox.PropertyName._hsv))
    {
      this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTickbox.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTickbox.PropertyName._baseScale))
    {
      this._baseScale = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTickbox.PropertyName._hoverScale))
    {
      this._hoverScale = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NTickbox.PropertyName._pressDownScale))
    {
      this._pressDownScale = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NTickbox.PropertyName._hoverV))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._hoverV = VariantUtils.ConvertTo<float>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NTickbox.PropertyName.IsTicked))
    {
      ref godot_variant local = ref value;
      bool isTicked = this.IsTicked;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref isTicked);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NTickbox.PropertyName._isTicked))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isTicked);
      return true;
    }
    if (StringName.op_Equality(ref name, NTickbox.PropertyName._imageContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._imageContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NTickbox.PropertyName._tickedImage))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._tickedImage);
      return true;
    }
    if (StringName.op_Equality(ref name, NTickbox.PropertyName._notTickedImage))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._notTickedImage);
      return true;
    }
    if (StringName.op_Equality(ref name, NTickbox.PropertyName._hsv))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
      return true;
    }
    if (StringName.op_Equality(ref name, NTickbox.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (StringName.op_Equality(ref name, NTickbox.PropertyName._baseScale))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._baseScale);
      return true;
    }
    if (StringName.op_Equality(ref name, NTickbox.PropertyName._hoverScale))
    {
      value = VariantUtils.CreateFrom<float>(ref this._hoverScale);
      return true;
    }
    if (StringName.op_Equality(ref name, NTickbox.PropertyName._pressDownScale))
    {
      value = VariantUtils.CreateFrom<float>(ref this._pressDownScale);
      return true;
    }
    if (!StringName.op_Equality(ref name, NTickbox.PropertyName._hoverV))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<float>(ref this._hoverV);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 1L, NTickbox.PropertyName._isTicked, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NTickbox.PropertyName.IsTicked, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTickbox.PropertyName._imageContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTickbox.PropertyName._tickedImage, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTickbox.PropertyName._notTickedImage, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTickbox.PropertyName._hsv, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NTickbox.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NTickbox.PropertyName._baseScale, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NTickbox.PropertyName._hoverScale, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NTickbox.PropertyName._pressDownScale, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NTickbox.PropertyName._hoverV, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName isTicked1 = NTickbox.PropertyName.IsTicked;
    bool isTicked2 = this.IsTicked;
    Variant variant = Variant.From<bool>(ref isTicked2);
    serializationInfo.AddProperty(isTicked1, variant);
    info.AddProperty(NTickbox.PropertyName._isTicked, Variant.From<bool>(ref this._isTicked));
    info.AddProperty(NTickbox.PropertyName._imageContainer, Variant.From<Control>(ref this._imageContainer));
    info.AddProperty(NTickbox.PropertyName._tickedImage, Variant.From<Control>(ref this._tickedImage));
    info.AddProperty(NTickbox.PropertyName._notTickedImage, Variant.From<Control>(ref this._notTickedImage));
    info.AddProperty(NTickbox.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
    info.AddProperty(NTickbox.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NTickbox.PropertyName._baseScale, Variant.From<Vector2>(ref this._baseScale));
    info.AddProperty(NTickbox.PropertyName._hoverScale, Variant.From<float>(ref this._hoverScale));
    info.AddProperty(NTickbox.PropertyName._pressDownScale, Variant.From<float>(ref this._pressDownScale));
    info.AddProperty(NTickbox.PropertyName._hoverV, Variant.From<float>(ref this._hoverV));
    info.AddSignalEventDelegate(NTickbox.SignalName.Toggled, (Delegate) this.backing_Toggled);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NTickbox.PropertyName.IsTicked, ref variant1))
      this.IsTicked = ((Variant) ref variant1).As<bool>();
    Variant variant2;
    if (info.TryGetProperty(NTickbox.PropertyName._isTicked, ref variant2))
      this._isTicked = ((Variant) ref variant2).As<bool>();
    Variant variant3;
    if (info.TryGetProperty(NTickbox.PropertyName._imageContainer, ref variant3))
      this._imageContainer = ((Variant) ref variant3).As<Control>();
    Variant variant4;
    if (info.TryGetProperty(NTickbox.PropertyName._tickedImage, ref variant4))
      this._tickedImage = ((Variant) ref variant4).As<Control>();
    Variant variant5;
    if (info.TryGetProperty(NTickbox.PropertyName._notTickedImage, ref variant5))
      this._notTickedImage = ((Variant) ref variant5).As<Control>();
    Variant variant6;
    if (info.TryGetProperty(NTickbox.PropertyName._hsv, ref variant6))
      this._hsv = ((Variant) ref variant6).As<ShaderMaterial>();
    Variant variant7;
    if (info.TryGetProperty(NTickbox.PropertyName._tween, ref variant7))
      this._tween = ((Variant) ref variant7).As<Tween>();
    Variant variant8;
    if (info.TryGetProperty(NTickbox.PropertyName._baseScale, ref variant8))
      this._baseScale = ((Variant) ref variant8).As<Vector2>();
    Variant variant9;
    if (info.TryGetProperty(NTickbox.PropertyName._hoverScale, ref variant9))
      this._hoverScale = ((Variant) ref variant9).As<float>();
    Variant variant10;
    if (info.TryGetProperty(NTickbox.PropertyName._pressDownScale, ref variant10))
      this._pressDownScale = ((Variant) ref variant10).As<float>();
    Variant variant11;
    if (info.TryGetProperty(NTickbox.PropertyName._hoverV, ref variant11))
      this._hoverV = ((Variant) ref variant11).As<float>();
    NTickbox.ToggledEventHandler toggledEventHandler;
    if (!info.TryGetSignalEventDelegate<NTickbox.ToggledEventHandler>(NTickbox.SignalName.Toggled, ref toggledEventHandler))
      return;
    this.backing_Toggled = toggledEventHandler;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<MethodInfo> GetGodotSignalList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NTickbox.SignalName.Toggled, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("tickbox"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null)
    };
  }

  public event NTickbox.ToggledEventHandler Toggled
  {
    add => this.backing_Toggled += value;
    remove => this.backing_Toggled -= value;
  }

  protected void EmitSignalToggled(NTickbox tickbox)
  {
    ((GodotObject) this).EmitSignal(NTickbox.SignalName.Toggled, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) tickbox)
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RaiseGodotClassSignalCallbacks(
    in godot_string_name signal,
    NativeVariantPtrArgs args)
  {
    if (StringName.op_Equality(ref signal, NTickbox.SignalName.Toggled) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NTickbox.ToggledEventHandler backingToggled = this.backing_Toggled;
      if (backingToggled == null)
        return;
      backingToggled(VariantUtils.ConvertTo<NTickbox>(ref ((NativeVariantPtrArgs) ref args)[0]));
    }
    else
      base.RaiseGodotClassSignalCallbacks(in signal, args);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassSignal(in godot_string_name signal)
  {
    return StringName.op_Equality(ref signal, NTickbox.SignalName.Toggled) || base.HasGodotClassSignal(in signal);
  }

  [Signal]
  public delegate void ToggledEventHandler(
  #nullable enable
  NTickbox tickbox);

  public new class MethodName : NButton.MethodName
  {
    public new static readonly 
    #nullable disable
    StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName ConnectSignals = StringName.op_Implicit(nameof (ConnectSignals));
    public new static readonly StringName OnRelease = StringName.op_Implicit(nameof (OnRelease));
    public static readonly StringName ForceToggleTick = StringName.op_Implicit(nameof (ForceToggleTick));
    public static readonly StringName OnUntick = StringName.op_Implicit(nameof (OnUntick));
    public static readonly StringName OnTick = StringName.op_Implicit(nameof (OnTick));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public new static readonly StringName OnPress = StringName.op_Implicit(nameof (OnPress));
    public new static readonly StringName OnDisable = StringName.op_Implicit(nameof (OnDisable));
    public new static readonly StringName OnEnable = StringName.op_Implicit(nameof (OnEnable));
    public static readonly StringName UpdateShaderV = StringName.op_Implicit(nameof (UpdateShaderV));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public static readonly StringName IsTicked = StringName.op_Implicit(nameof (IsTicked));
    public static readonly StringName _isTicked = StringName.op_Implicit(nameof (_isTicked));
    public static readonly StringName _imageContainer = StringName.op_Implicit(nameof (_imageContainer));
    public static readonly StringName _tickedImage = StringName.op_Implicit(nameof (_tickedImage));
    public static readonly StringName _notTickedImage = StringName.op_Implicit(nameof (_notTickedImage));
    public static readonly StringName _hsv = StringName.op_Implicit(nameof (_hsv));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _baseScale = StringName.op_Implicit(nameof (_baseScale));
    public static readonly StringName _hoverScale = StringName.op_Implicit(nameof (_hoverScale));
    public static readonly StringName _pressDownScale = StringName.op_Implicit(nameof (_pressDownScale));
    public static readonly StringName _hoverV = StringName.op_Implicit(nameof (_hoverV));
  }

  public new class SignalName : NButton.SignalName
  {
    public static readonly StringName Toggled = StringName.op_Implicit(nameof (Toggled));
  }
}
