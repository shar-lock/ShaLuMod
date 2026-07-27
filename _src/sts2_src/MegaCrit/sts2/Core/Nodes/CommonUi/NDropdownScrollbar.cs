// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.CommonUi.NDropdownScrollbar
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

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.CommonUi;

[ScriptPath("res://src/Core/Nodes/CommonUi/NDropdownScrollbar.cs")]
public class NDropdownScrollbar : NButton
{
  private NDropdownContainer _dropdownContainer;
  private Control _train;
  public bool hasControl;
  private Vector2 _startDragPos;
  private Vector2 _targetDragPos;
  private float _scrollLimitTop;
  private float _scrollLimitBottom;

  public override void _Ready()
  {
    this.ConnectSignals();
    this._dropdownContainer = ((Node) this).GetParent<NDropdownContainer>();
    this._train = ((Node) this).GetNode<Control>(NodePath.op_Implicit("Train"));
    ((GodotObject) this).Connect(CanvasItem.SignalName.VisibilityChanged, Callable.From(new Action(this.OnShow)), 0U);
  }

  public void RefreshTrainBounds()
  {
    this._scrollLimitTop = (float) (600.0 - (double) this._train.Size.Y - 9.0);
    this._scrollLimitBottom = 9f;
  }

  protected override void OnFocus() => ((CanvasItem) this._train).Modulate = StsColors.gold;

  protected override void OnUnfocus()
  {
    if (this.hasControl)
      return;
    ((CanvasItem) this._train).Modulate = StsColors.quarterTransparentWhite;
  }

  private void OnShow() => ((CanvasItem) this._train).Modulate = StsColors.quarterTransparentWhite;

  protected override void OnPress()
  {
    this.hasControl = true;
    ((CanvasItem) this._train).Modulate = StsColors.gold;
    Input.MouseMode = (Input.MouseModeEnum) 1L;
  }

  public override void _Input(InputEvent inputEvent)
  {
    base._Input(inputEvent);
    if (!this.hasControl || !(inputEvent is InputEventMouseButton eventMouseButton) || eventMouseButton.ButtonIndex != 1L || !((InputEvent) eventMouseButton).IsReleased())
      return;
    this.hasControl = false;
    Input.MouseMode = (Input.MouseModeEnum) 0L;
    ((CanvasItem) this._train).Modulate = StsColors.quarterTransparentWhite;
  }

  public override void _GuiInput(InputEvent inputEvent)
  {
    base._GuiInput(inputEvent);
    if (!this.hasControl || !(inputEvent is InputEventMouseMotion eventMouseMotion))
      return;
    Control train = this._train;
    train.Position = Vector2.op_Addition(train.Position, new Vector2(0.0f, eventMouseMotion.Relative.Y));
    this.ClampTrain();
    this._dropdownContainer.UpdatePositionBasedOnTrain((float) (1.0 - ((double) this._train.Position.Y - (double) this._scrollLimitBottom) / ((double) this._scrollLimitTop - (double) this._scrollLimitBottom)));
  }

  private void ClampTrain()
  {
    this._train.Position = new Vector2(this._train.Position.X, Mathf.Clamp(this._train.Position.Y, this._scrollLimitBottom, this._scrollLimitTop));
  }

  public void SetTrainPositionFromPercentage(float percentage)
  {
    this._train.Position = new Vector2(this._train.Position.X, this._scrollLimitBottom + percentage * (this._scrollLimitTop - this._scrollLimitBottom));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(10)
    {
      new MethodInfo(NDropdownScrollbar.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDropdownScrollbar.MethodName.RefreshTrainBounds, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDropdownScrollbar.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDropdownScrollbar.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDropdownScrollbar.MethodName.OnShow, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDropdownScrollbar.MethodName.OnPress, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDropdownScrollbar.MethodName._Input, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NDropdownScrollbar.MethodName._GuiInput, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NDropdownScrollbar.MethodName.ClampTrain, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDropdownScrollbar.MethodName.SetTrainPositionFromPercentage, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("percentage"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NDropdownScrollbar.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDropdownScrollbar.MethodName.RefreshTrainBounds) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshTrainBounds();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDropdownScrollbar.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDropdownScrollbar.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDropdownScrollbar.MethodName.OnShow) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnShow();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDropdownScrollbar.MethodName.OnPress) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnPress();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDropdownScrollbar.MethodName._Input) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Input(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDropdownScrollbar.MethodName._GuiInput) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      base._GuiInput(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDropdownScrollbar.MethodName.ClampTrain) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ClampTrain();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NDropdownScrollbar.MethodName.SetTrainPositionFromPercentage) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.SetTrainPositionFromPercentage(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NDropdownScrollbar.MethodName._Ready) || StringName.op_Equality(ref method, NDropdownScrollbar.MethodName.RefreshTrainBounds) || StringName.op_Equality(ref method, NDropdownScrollbar.MethodName.OnFocus) || StringName.op_Equality(ref method, NDropdownScrollbar.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NDropdownScrollbar.MethodName.OnShow) || StringName.op_Equality(ref method, NDropdownScrollbar.MethodName.OnPress) || StringName.op_Equality(ref method, NDropdownScrollbar.MethodName._Input) || StringName.op_Equality(ref method, NDropdownScrollbar.MethodName._GuiInput) || StringName.op_Equality(ref method, NDropdownScrollbar.MethodName.ClampTrain) || StringName.op_Equality(ref method, NDropdownScrollbar.MethodName.SetTrainPositionFromPercentage) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDropdownScrollbar.PropertyName._dropdownContainer))
    {
      this._dropdownContainer = VariantUtils.ConvertTo<NDropdownContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDropdownScrollbar.PropertyName._train))
    {
      this._train = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDropdownScrollbar.PropertyName.hasControl))
    {
      this.hasControl = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDropdownScrollbar.PropertyName._startDragPos))
    {
      this._startDragPos = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDropdownScrollbar.PropertyName._targetDragPos))
    {
      this._targetDragPos = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDropdownScrollbar.PropertyName._scrollLimitTop))
    {
      this._scrollLimitTop = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDropdownScrollbar.PropertyName._scrollLimitBottom))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._scrollLimitBottom = VariantUtils.ConvertTo<float>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDropdownScrollbar.PropertyName._dropdownContainer))
    {
      value = VariantUtils.CreateFrom<NDropdownContainer>(ref this._dropdownContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NDropdownScrollbar.PropertyName._train))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._train);
      return true;
    }
    if (StringName.op_Equality(ref name, NDropdownScrollbar.PropertyName.hasControl))
    {
      value = VariantUtils.CreateFrom<bool>(ref this.hasControl);
      return true;
    }
    if (StringName.op_Equality(ref name, NDropdownScrollbar.PropertyName._startDragPos))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._startDragPos);
      return true;
    }
    if (StringName.op_Equality(ref name, NDropdownScrollbar.PropertyName._targetDragPos))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._targetDragPos);
      return true;
    }
    if (StringName.op_Equality(ref name, NDropdownScrollbar.PropertyName._scrollLimitTop))
    {
      value = VariantUtils.CreateFrom<float>(ref this._scrollLimitTop);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDropdownScrollbar.PropertyName._scrollLimitBottom))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<float>(ref this._scrollLimitBottom);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NDropdownScrollbar.PropertyName._dropdownContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDropdownScrollbar.PropertyName._train, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NDropdownScrollbar.PropertyName.hasControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NDropdownScrollbar.PropertyName._startDragPos, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NDropdownScrollbar.PropertyName._targetDragPos, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NDropdownScrollbar.PropertyName._scrollLimitTop, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NDropdownScrollbar.PropertyName._scrollLimitBottom, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NDropdownScrollbar.PropertyName._dropdownContainer, Variant.From<NDropdownContainer>(ref this._dropdownContainer));
    info.AddProperty(NDropdownScrollbar.PropertyName._train, Variant.From<Control>(ref this._train));
    info.AddProperty(NDropdownScrollbar.PropertyName.hasControl, Variant.From<bool>(ref this.hasControl));
    info.AddProperty(NDropdownScrollbar.PropertyName._startDragPos, Variant.From<Vector2>(ref this._startDragPos));
    info.AddProperty(NDropdownScrollbar.PropertyName._targetDragPos, Variant.From<Vector2>(ref this._targetDragPos));
    info.AddProperty(NDropdownScrollbar.PropertyName._scrollLimitTop, Variant.From<float>(ref this._scrollLimitTop));
    info.AddProperty(NDropdownScrollbar.PropertyName._scrollLimitBottom, Variant.From<float>(ref this._scrollLimitBottom));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NDropdownScrollbar.PropertyName._dropdownContainer, ref variant1))
      this._dropdownContainer = ((Variant) ref variant1).As<NDropdownContainer>();
    Variant variant2;
    if (info.TryGetProperty(NDropdownScrollbar.PropertyName._train, ref variant2))
      this._train = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NDropdownScrollbar.PropertyName.hasControl, ref variant3))
      this.hasControl = ((Variant) ref variant3).As<bool>();
    Variant variant4;
    if (info.TryGetProperty(NDropdownScrollbar.PropertyName._startDragPos, ref variant4))
      this._startDragPos = ((Variant) ref variant4).As<Vector2>();
    Variant variant5;
    if (info.TryGetProperty(NDropdownScrollbar.PropertyName._targetDragPos, ref variant5))
      this._targetDragPos = ((Variant) ref variant5).As<Vector2>();
    Variant variant6;
    if (info.TryGetProperty(NDropdownScrollbar.PropertyName._scrollLimitTop, ref variant6))
      this._scrollLimitTop = ((Variant) ref variant6).As<float>();
    Variant variant7;
    if (!info.TryGetProperty(NDropdownScrollbar.PropertyName._scrollLimitBottom, ref variant7))
      return;
    this._scrollLimitBottom = ((Variant) ref variant7).As<float>();
  }

  public new class MethodName : NButton.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName RefreshTrainBounds = StringName.op_Implicit(nameof (RefreshTrainBounds));
    public new static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public new static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public static readonly StringName OnShow = StringName.op_Implicit(nameof (OnShow));
    public new static readonly StringName OnPress = StringName.op_Implicit(nameof (OnPress));
    public new static readonly StringName _Input = StringName.op_Implicit(nameof (_Input));
    public new static readonly StringName _GuiInput = StringName.op_Implicit(nameof (_GuiInput));
    public static readonly StringName ClampTrain = StringName.op_Implicit(nameof (ClampTrain));
    public static readonly StringName SetTrainPositionFromPercentage = StringName.op_Implicit(nameof (SetTrainPositionFromPercentage));
  }

  public new class PropertyName : NButton.PropertyName
  {
    public static readonly StringName _dropdownContainer = StringName.op_Implicit(nameof (_dropdownContainer));
    public static readonly StringName _train = StringName.op_Implicit(nameof (_train));
    public static readonly StringName hasControl = StringName.op_Implicit(nameof (hasControl));
    public static readonly StringName _startDragPos = StringName.op_Implicit(nameof (_startDragPos));
    public static readonly StringName _targetDragPos = StringName.op_Implicit(nameof (_targetDragPos));
    public static readonly StringName _scrollLimitTop = StringName.op_Implicit(nameof (_scrollLimitTop));
    public static readonly StringName _scrollLimitBottom = StringName.op_Implicit(nameof (_scrollLimitBottom));
  }

  public new class SignalName : NButton.SignalName
  {
  }
}
