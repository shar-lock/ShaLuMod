// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary.NCardPoolFilter
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary;

[ScriptPath("res://src/Core/Nodes/Screens/CardLibrary/NCardPoolFilter.cs")]
public class NCardPoolFilter : NButton
{
  private static readonly StringName _v = new StringName("v");
  private static readonly StringName _s = new StringName("s");
  private bool _isSelected;
  private Control _image;
  private ShaderMaterial _hsv;
  private NSelectionReticle _controllerSelectionReticle;
  private Tween? _tween;
  private const float _focusedMultiplier = 1.2f;
  private const float _pressDownMultiplier = 0.8f;
  private static readonly Vector2 _enabledScale = Vector2.op_Multiply(Vector2.One, 1.1f);
  private static readonly Vector2 _disabledScale = Vector2.op_Multiply(Vector2.One, 0.95f);
  private 
  #nullable disable
  NCardPoolFilter.ToggledEventHandler backing_Toggled;

  public 
  #nullable enable
  LocString? Loc { get; set; }

  public bool IsSelected
  {
    get => this._isSelected;
    set
    {
      this._isSelected = value;
      this.OnToggle();
    }
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this._image = ((Node) this).GetNode<Control>(NodePath.op_Implicit("Image"));
    this._controllerSelectionReticle = ((Node) this).GetNode<NSelectionReticle>(NodePath.op_Implicit("%SelectionReticle"));
    this._hsv = (ShaderMaterial) ((CanvasItem) this._image).GetMaterial();
  }

  private void OnToggle()
  {
    this._tween?.Kill();
    this._hsv.SetShaderParameter(NCardPoolFilter._s, Variant.op_Implicit(this._isSelected ? 1f : 0.3f));
    this._hsv.SetShaderParameter(NCardPoolFilter._v, Variant.op_Implicit(this._isSelected ? 1f : 0.55f));
    if (!this._isSelected)
    {
      this._tween = ((Node) this).CreateTween().SetParallel(true);
      this._tween.TweenProperty((GodotObject) this._image, NodePath.op_Implicit("scale"), Variant.op_Implicit(NCardPoolFilter._disabledScale), 0.3).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
    }
    else
    {
      this._tween = ((Node) this).CreateTween().SetParallel(true);
      this._tween.TweenProperty((GodotObject) this._image, NodePath.op_Implicit("scale"), Variant.op_Implicit(NCardPoolFilter._enabledScale), 0.2).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 10L);
    }
  }

  protected override void OnRelease()
  {
    base.OnRelease();
    this.IsSelected = !this.IsSelected;
    ((GodotObject) this).EmitSignal(NCardPoolFilter.SignalName.Toggled, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) this)
    });
  }

  protected override void OnFocus()
  {
    base.OnFocus();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._image, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(this._isSelected ? NCardPoolFilter._enabledScale : NCardPoolFilter._disabledScale, 1.2f)), 0.05);
    if (NControllerManager.Instance.IsUsingController)
      this._controllerSelectionReticle.OnSelect();
    if (this.Loc == null)
      return;
    NHoverTipSet.CreateAndShow((Control) this, (IHoverTip) new HoverTip(this.Loc))?.SetGlobalPosition(new Vector2(310f, this.GlobalPosition.Y), false);
  }

  protected override void OnUnfocus()
  {
    base.OnUnfocus();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._image, NodePath.op_Implicit("scale"), Variant.op_Implicit(this._isSelected ? NCardPoolFilter._enabledScale : NCardPoolFilter._disabledScale), 0.3);
    this._controllerSelectionReticle.OnDeselect();
    if (this.Loc == null)
      return;
    NHoverTipSet.Remove((Control) this);
  }

  protected override void OnPress()
  {
    if (this._isSelected)
      return;
    base.OnPress();
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._image, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(this._isSelected ? NCardPoolFilter._enabledScale : NCardPoolFilter._disabledScale, 0.8f)), 0.3).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(6)
    {
      new MethodInfo(NCardPoolFilter.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardPoolFilter.MethodName.OnToggle, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardPoolFilter.MethodName.OnRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardPoolFilter.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardPoolFilter.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardPoolFilter.MethodName.OnPress, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCardPoolFilter.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardPoolFilter.MethodName.OnToggle) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnToggle();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardPoolFilter.MethodName.OnRelease) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnRelease();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardPoolFilter.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardPoolFilter.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCardPoolFilter.MethodName.OnPress) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnPress();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCardPoolFilter.MethodName._Ready) || StringName.op_Equality(ref method, NCardPoolFilter.MethodName.OnToggle) || StringName.op_Equality(ref method, NCardPoolFilter.MethodName.OnRelease) || StringName.op_Equality(ref method, NCardPoolFilter.MethodName.OnFocus) || StringName.op_Equality(ref method, NCardPoolFilter.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NCardPoolFilter.MethodName.OnPress) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardPoolFilter.PropertyName.IsSelected))
    {
      this.IsSelected = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardPoolFilter.PropertyName._isSelected))
    {
      this._isSelected = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardPoolFilter.PropertyName._image))
    {
      this._image = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardPoolFilter.PropertyName._hsv))
    {
      this._hsv = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardPoolFilter.PropertyName._controllerSelectionReticle))
    {
      this._controllerSelectionReticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardPoolFilter.PropertyName._tween))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._tween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardPoolFilter.PropertyName.IsSelected))
    {
      ref godot_variant local = ref value;
      bool isSelected = this.IsSelected;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref isSelected);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCardPoolFilter.PropertyName._isSelected))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isSelected);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardPoolFilter.PropertyName._image))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._image);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardPoolFilter.PropertyName._hsv))
    {
      value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._hsv);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardPoolFilter.PropertyName._controllerSelectionReticle))
    {
      value = VariantUtils.CreateFrom<NSelectionReticle>(ref this._controllerSelectionReticle);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardPoolFilter.PropertyName._tween))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<Tween>(ref this._tween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 1L, NCardPoolFilter.PropertyName._isSelected, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardPoolFilter.PropertyName._image, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardPoolFilter.PropertyName._hsv, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardPoolFilter.PropertyName._controllerSelectionReticle, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardPoolFilter.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCardPoolFilter.PropertyName.IsSelected, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName isSelected1 = NCardPoolFilter.PropertyName.IsSelected;
    bool isSelected2 = this.IsSelected;
    Variant variant = Variant.From<bool>(ref isSelected2);
    serializationInfo.AddProperty(isSelected1, variant);
    info.AddProperty(NCardPoolFilter.PropertyName._isSelected, Variant.From<bool>(ref this._isSelected));
    info.AddProperty(NCardPoolFilter.PropertyName._image, Variant.From<Control>(ref this._image));
    info.AddProperty(NCardPoolFilter.PropertyName._hsv, Variant.From<ShaderMaterial>(ref this._hsv));
    info.AddProperty(NCardPoolFilter.PropertyName._controllerSelectionReticle, Variant.From<NSelectionReticle>(ref this._controllerSelectionReticle));
    info.AddProperty(NCardPoolFilter.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddSignalEventDelegate(NCardPoolFilter.SignalName.Toggled, (Delegate) this.backing_Toggled);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCardPoolFilter.PropertyName.IsSelected, ref variant1))
      this.IsSelected = ((Variant) ref variant1).As<bool>();
    Variant variant2;
    if (info.TryGetProperty(NCardPoolFilter.PropertyName._isSelected, ref variant2))
      this._isSelected = ((Variant) ref variant2).As<bool>();
    Variant variant3;
    if (info.TryGetProperty(NCardPoolFilter.PropertyName._image, ref variant3))
      this._image = ((Variant) ref variant3).As<Control>();
    Variant variant4;
    if (info.TryGetProperty(NCardPoolFilter.PropertyName._hsv, ref variant4))
      this._hsv = ((Variant) ref variant4).As<ShaderMaterial>();
    Variant variant5;
    if (info.TryGetProperty(NCardPoolFilter.PropertyName._controllerSelectionReticle, ref variant5))
      this._controllerSelectionReticle = ((Variant) ref variant5).As<NSelectionReticle>();
    Variant variant6;
    if (info.TryGetProperty(NCardPoolFilter.PropertyName._tween, ref variant6))
      this._tween = ((Variant) ref variant6).As<Tween>();
    NCardPoolFilter.ToggledEventHandler toggledEventHandler;
    if (!info.TryGetSignalEventDelegate<NCardPoolFilter.ToggledEventHandler>(NCardPoolFilter.SignalName.Toggled, ref toggledEventHandler))
      return;
    this.backing_Toggled = toggledEventHandler;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<MethodInfo> GetGodotSignalList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NCardPoolFilter.SignalName.Toggled, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("filter"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null)
    };
  }

  public event NCardPoolFilter.ToggledEventHandler Toggled
  {
    add => this.backing_Toggled += value;
    remove => this.backing_Toggled -= value;
  }

  protected void EmitSignalToggled(NCardPoolFilter filter)
  {
    ((GodotObject) this).EmitSignal(NCardPoolFilter.SignalName.Toggled, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) filter)
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RaiseGodotClassSignalCallbacks(
    in godot_string_name signal,
    NativeVariantPtrArgs args)
  {
    if (StringName.op_Equality(ref signal, NCardPoolFilter.SignalName.Toggled) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NCardPoolFilter.ToggledEventHandler backingToggled = this.backing_Toggled;
      if (backingToggled == null)
        return;
      backingToggled(VariantUtils.ConvertTo<NCardPoolFilter>(ref ((NativeVariantPtrArgs) ref args)[0]));
    }
    else
      base.RaiseGodotClassSignalCallbacks(in signal, args);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassSignal(in godot_string_name signal)
  {
    return StringName.op_Equality(ref signal, NCardPoolFilter.SignalName.Toggled) || base.HasGodotClassSignal(in signal);
  }

  [Signal]
  public delegate void ToggledEventHandler(
  #nullable enable
  NCardPoolFilter filter);

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
  }

  public new class PropertyName : NButton.PropertyName
  {
    public static readonly StringName IsSelected = StringName.op_Implicit(nameof (IsSelected));
    public static readonly StringName _isSelected = StringName.op_Implicit(nameof (_isSelected));
    public static readonly StringName _image = StringName.op_Implicit(nameof (_image));
    public static readonly StringName _hsv = StringName.op_Implicit(nameof (_hsv));
    public static readonly StringName _controllerSelectionReticle = StringName.op_Implicit(nameof (_controllerSelectionReticle));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
  }

  public new class SignalName : NButton.SignalName
  {
    public static readonly StringName Toggled = StringName.op_Implicit(nameof (Toggled));
  }
}
