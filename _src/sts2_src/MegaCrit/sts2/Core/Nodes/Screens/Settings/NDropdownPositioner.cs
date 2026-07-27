// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Settings.NDropdownPositioner
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings;

[ScriptPath("res://src/Core/Nodes/Screens/Settings/NDropdownPositioner.cs")]
public class NDropdownPositioner : Control
{
  [Export]
  private Control _dropdownNode;

  public override void _Ready()
  {
    ((GodotObject) this).Connect(Control.SignalName.FocusEntered, Callable.From((Action) (() => this._dropdownNode.TryGrabFocus())), 0U);
    ((GodotObject) this).Connect(CanvasItem.SignalName.VisibilityChanged, Callable.From(new Action(this.OnVisibilityChange)), 0U);
    this.OnVisibilityChange();
  }

  private void OnVisibilityChange()
  {
    if (!((CanvasItem) this).Visible)
      return;
    this._dropdownNode.FocusNeighborBottom = this.FocusNeighborBottom;
    this._dropdownNode.FocusNeighborTop = this.FocusNeighborTop;
    this._dropdownNode.FocusNeighborLeft = this.FocusNeighborLeft;
    this._dropdownNode.FocusNeighborRight = this.FocusNeighborRight;
  }

  public override void _Process(double delta)
  {
    this._dropdownNode.GlobalPosition = this.GlobalPosition;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NDropdownPositioner.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDropdownPositioner.MethodName.OnVisibilityChange, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDropdownPositioner.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NDropdownPositioner.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDropdownPositioner.MethodName.OnVisibilityChange) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnVisibilityChange();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NDropdownPositioner.MethodName._Process) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NDropdownPositioner.MethodName._Ready) || StringName.op_Equality(ref method, NDropdownPositioner.MethodName.OnVisibilityChange) || StringName.op_Equality(ref method, NDropdownPositioner.MethodName._Process) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NDropdownPositioner.PropertyName._dropdownNode))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._dropdownNode = VariantUtils.ConvertTo<Control>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NDropdownPositioner.PropertyName._dropdownNode))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Control>(ref this._dropdownNode);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NDropdownPositioner.PropertyName._dropdownNode, (PropertyHint) 34L, "Control", (PropertyUsageFlags) 4102L, true)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NDropdownPositioner.PropertyName._dropdownNode, Variant.From<Control>(ref this._dropdownNode));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant;
    if (!info.TryGetProperty(NDropdownPositioner.PropertyName._dropdownNode, ref variant))
      return;
    this._dropdownNode = ((Variant) ref variant).As<Control>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnVisibilityChange = StringName.op_Implicit(nameof (OnVisibilityChange));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _dropdownNode = StringName.op_Implicit(nameof (_dropdownNode));
  }

  public class SignalName : Control.SignalName
  {
  }
}
