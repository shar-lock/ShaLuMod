// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Settings.NSettingsSlider
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings;

[ScriptPath("res://src/Core/Nodes/Screens/Settings/NSettingsSlider.cs")]
public abstract class NSettingsSlider : Control
{
  protected NSlider _slider;
  private MegaLabel _valueLabel;
  private NSelectionReticle _selectionReticle;

  public override void _Ready()
  {
    if (((object) this).GetType() != typeof (NSettingsSlider))
    {
      Log.Error($"{((object) this).GetType()}");
      throw new InvalidOperationException("Don't call base._Ready()! Call ConnectSignals() instead.");
    }
    this.ConnectSignals();
  }

  protected virtual void ConnectSignals()
  {
    this._slider = ((Node) this).GetNode<NSlider>(NodePath.op_Implicit("Slider"));
    this._valueLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("SliderValue"));
    this._selectionReticle = ((Node) this).GetNode<NSelectionReticle>(NodePath.op_Implicit("SelectionReticle"));
    ((GodotObject) this._slider).Connect(Range.SignalName.ValueChanged, Callable.From<double>(new Action<double>(this.OnValueChanged)), 0U);
    this._valueLabel.SetTextAutoSize($"{this._slider.Value}%");
    ((GodotObject) this).Connect(Control.SignalName.FocusEntered, Callable.From(new Action(this.OnFocus)), 0U);
    ((GodotObject) this).Connect(Control.SignalName.FocusExited, Callable.From(new Action(this.OnUnfocus)), 0U);
  }

  public override void _GuiInput(InputEvent input)
  {
    base._GuiInput(input);
    if (input.IsActionPressed(MegaInput.left, false, false))
      this._slider.Value -= 5.0;
    if (!input.IsActionPressed(MegaInput.right, false, false))
      return;
    this._slider.Value += 5.0;
  }

  private void OnValueChanged(double value) => this._valueLabel.SetTextAutoSize($"{value}%");

  private void OnFocus()
  {
    if (!NControllerManager.Instance.IsUsingController)
      return;
    this._selectionReticle.OnSelect();
  }

  private void OnUnfocus() => this._selectionReticle.OnDeselect();

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(6)
    {
      new MethodInfo(NSettingsSlider.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSettingsSlider.MethodName.ConnectSignals, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSettingsSlider.MethodName._GuiInput, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("input"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NSettingsSlider.MethodName.OnValueChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("value"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NSettingsSlider.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSettingsSlider.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NSettingsSlider.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSettingsSlider.MethodName.ConnectSignals) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ConnectSignals();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSettingsSlider.MethodName._GuiInput) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      base._GuiInput(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSettingsSlider.MethodName.OnValueChanged) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnValueChanged(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSettingsSlider.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NSettingsSlider.MethodName.OnUnfocus) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.OnUnfocus();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NSettingsSlider.MethodName._Ready) || StringName.op_Equality(ref method, NSettingsSlider.MethodName.ConnectSignals) || StringName.op_Equality(ref method, NSettingsSlider.MethodName._GuiInput) || StringName.op_Equality(ref method, NSettingsSlider.MethodName.OnValueChanged) || StringName.op_Equality(ref method, NSettingsSlider.MethodName.OnFocus) || StringName.op_Equality(ref method, NSettingsSlider.MethodName.OnUnfocus) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSettingsSlider.PropertyName._slider))
    {
      this._slider = VariantUtils.ConvertTo<NSlider>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSettingsSlider.PropertyName._valueLabel))
    {
      this._valueLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSettingsSlider.PropertyName._selectionReticle))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._selectionReticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSettingsSlider.PropertyName._slider))
    {
      value = VariantUtils.CreateFrom<NSlider>(ref this._slider);
      return true;
    }
    if (StringName.op_Equality(ref name, NSettingsSlider.PropertyName._valueLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._valueLabel);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSettingsSlider.PropertyName._selectionReticle))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<NSelectionReticle>(ref this._selectionReticle);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NSettingsSlider.PropertyName._slider, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSettingsSlider.PropertyName._valueLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSettingsSlider.PropertyName._selectionReticle, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NSettingsSlider.PropertyName._slider, Variant.From<NSlider>(ref this._slider));
    info.AddProperty(NSettingsSlider.PropertyName._valueLabel, Variant.From<MegaLabel>(ref this._valueLabel));
    info.AddProperty(NSettingsSlider.PropertyName._selectionReticle, Variant.From<NSelectionReticle>(ref this._selectionReticle));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NSettingsSlider.PropertyName._slider, ref variant1))
      this._slider = ((Variant) ref variant1).As<NSlider>();
    Variant variant2;
    if (info.TryGetProperty(NSettingsSlider.PropertyName._valueLabel, ref variant2))
      this._valueLabel = ((Variant) ref variant2).As<MegaLabel>();
    Variant variant3;
    if (!info.TryGetProperty(NSettingsSlider.PropertyName._selectionReticle, ref variant3))
      return;
    this._selectionReticle = ((Variant) ref variant3).As<NSelectionReticle>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName ConnectSignals = StringName.op_Implicit(nameof (ConnectSignals));
    public static readonly StringName _GuiInput = StringName.op_Implicit(nameof (_GuiInput));
    public static readonly StringName OnValueChanged = StringName.op_Implicit(nameof (OnValueChanged));
    public static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _slider = StringName.op_Implicit(nameof (_slider));
    public static readonly StringName _valueLabel = StringName.op_Implicit(nameof (_valueLabel));
    public static readonly StringName _selectionReticle = StringName.op_Implicit(nameof (_selectionReticle));
  }

  public class SignalName : Control.SignalName
  {
  }
}
