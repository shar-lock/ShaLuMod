// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.GodotExtensions.NMegaTextEdit
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Localization.Fonts;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Platform;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.GodotExtensions;

[ScriptPath("res://src/Core/Nodes/GodotExtensions/NMegaTextEdit.cs")]
public class NMegaTextEdit : TextEdit
{
  private NSelectionReticle? _selectionReticle;
  private bool _isEditing;

  public bool IsEditing() => this._isEditing;

  public override void _Ready()
  {
    this.RefreshFont();
    ((GodotObject) this).Connect(Control.SignalName.FocusExited, Callable.From(new Action(this.StopEditing)), 0U);
    if (((Node) this).HasNode(NodePath.op_Implicit("SelectionReticle")))
      this._selectionReticle = ((Node) this).GetNode<NSelectionReticle>(NodePath.op_Implicit("SelectionReticle"));
    ((GodotObject) this).Connect(Control.SignalName.FocusEntered, Callable.From(new Action(this.OnFocus)), 0U);
    ((GodotObject) this).Connect(Control.SignalName.FocusExited, Callable.From(new Action(this.OnUnfocus)), 0U);
    ((GodotObject) this).Connect(Control.SignalName.MouseEntered, Callable.From(new Action(this.OnFocus)), 0U);
    ((GodotObject) this).Connect(Control.SignalName.MouseExited, Callable.From(new Action(this.OnUnfocus)), 0U);
  }

  public void RefreshFont()
  {
    ((Control) this).ApplyLocaleFontSubstitution(FontType.Regular, ThemeConstants.TextEdit.Font);
  }

  private void OnFocus()
  {
    if (!NControllerManager.Instance.IsUsingController)
      return;
    this._selectionReticle?.OnSelect();
  }

  private void OnUnfocus() => this._selectionReticle?.OnDeselect();

  public override void _GuiInput(InputEvent inputEvent)
  {
    ((Control) this)._GuiInput(inputEvent);
    if (inputEvent is InputEventMouseButton eventMouseButton && eventMouseButton.ButtonIndex == 1L && ((InputEvent) eventMouseButton).IsPressed())
      this.OpenKeyboard();
    if (inputEvent.IsActionPressed(MegaInput.select, false, false))
      this.OpenKeyboard();
    if (!inputEvent.IsActionPressed(MegaInput.cancel, false, false) || !this.IsEditing())
      return;
    this.StopEditing();
    ((Node) this).GetViewport()?.SetInputAsHandled();
  }

  private void OpenKeyboard()
  {
    ((Control) this).TryGrabFocus();
    this._isEditing = true;
    PlatformUtil.OpenVirtualKeyboard();
  }

  private void StopEditing()
  {
    this._isEditing = false;
    PlatformUtil.CloseVirtualKeyboard();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(8)
    {
      new MethodInfo(NMegaTextEdit.MethodName.IsEditing, new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMegaTextEdit.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMegaTextEdit.MethodName.RefreshFont, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMegaTextEdit.MethodName.OnFocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMegaTextEdit.MethodName.OnUnfocus, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMegaTextEdit.MethodName._GuiInput, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMegaTextEdit.MethodName.OpenKeyboard, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMegaTextEdit.MethodName.StopEditing, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMegaTextEdit.MethodName.IsEditing) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      bool flag = this.IsEditing();
      ret = VariantUtils.CreateFrom<bool>(ref flag);
      return true;
    }
    if (StringName.op_Equality(ref method, NMegaTextEdit.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMegaTextEdit.MethodName.RefreshFont) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshFont();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMegaTextEdit.MethodName.OnFocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnFocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMegaTextEdit.MethodName.OnUnfocus) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnUnfocus();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMegaTextEdit.MethodName._GuiInput) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Control) this)._GuiInput(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMegaTextEdit.MethodName.OpenKeyboard) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OpenKeyboard();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NMegaTextEdit.MethodName.StopEditing) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.StopEditing();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMegaTextEdit.MethodName.IsEditing) || StringName.op_Equality(ref method, NMegaTextEdit.MethodName._Ready) || StringName.op_Equality(ref method, NMegaTextEdit.MethodName.RefreshFont) || StringName.op_Equality(ref method, NMegaTextEdit.MethodName.OnFocus) || StringName.op_Equality(ref method, NMegaTextEdit.MethodName.OnUnfocus) || StringName.op_Equality(ref method, NMegaTextEdit.MethodName._GuiInput) || StringName.op_Equality(ref method, NMegaTextEdit.MethodName.OpenKeyboard) || StringName.op_Equality(ref method, NMegaTextEdit.MethodName.StopEditing) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMegaTextEdit.PropertyName._selectionReticle))
    {
      this._selectionReticle = VariantUtils.ConvertTo<NSelectionReticle>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMegaTextEdit.PropertyName._isEditing))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._isEditing = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMegaTextEdit.PropertyName._selectionReticle))
    {
      value = VariantUtils.CreateFrom<NSelectionReticle>(ref this._selectionReticle);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMegaTextEdit.PropertyName._isEditing))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<bool>(ref this._isEditing);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NMegaTextEdit.PropertyName._selectionReticle, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NMegaTextEdit.PropertyName._isEditing, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NMegaTextEdit.PropertyName._selectionReticle, Variant.From<NSelectionReticle>(ref this._selectionReticle));
    info.AddProperty(NMegaTextEdit.PropertyName._isEditing, Variant.From<bool>(ref this._isEditing));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NMegaTextEdit.PropertyName._selectionReticle, ref variant1))
      this._selectionReticle = ((Variant) ref variant1).As<NSelectionReticle>();
    Variant variant2;
    if (!info.TryGetProperty(NMegaTextEdit.PropertyName._isEditing, ref variant2))
      return;
    this._isEditing = ((Variant) ref variant2).As<bool>();
  }

  public class MethodName : TextEdit.MethodName
  {
    public static readonly StringName IsEditing = StringName.op_Implicit(nameof (IsEditing));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName RefreshFont = StringName.op_Implicit(nameof (RefreshFont));
    public static readonly StringName OnFocus = StringName.op_Implicit(nameof (OnFocus));
    public static readonly StringName OnUnfocus = StringName.op_Implicit(nameof (OnUnfocus));
    public static readonly StringName _GuiInput = StringName.op_Implicit(nameof (_GuiInput));
    public static readonly StringName OpenKeyboard = StringName.op_Implicit(nameof (OpenKeyboard));
    public static readonly StringName StopEditing = StringName.op_Implicit(nameof (StopEditing));
  }

  public class PropertyName : TextEdit.PropertyName
  {
    public static readonly StringName _selectionReticle = StringName.op_Implicit(nameof (_selectionReticle));
    public static readonly StringName _isEditing = StringName.op_Implicit(nameof (_isEditing));
  }

  public class SignalName : TextEdit.SignalName
  {
  }
}
