// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.GodotExtensions.NDropdown
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Debug;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.GodotExtensions;

[ScriptPath("res://src/Core/Nodes/GodotExtensions/NDropdown.cs")]
public class NDropdown : NClickableControl
{
  private Control _dropdownContainer;
  protected Control _dropdownItems;
  private NButton _dismisser;
  protected MegaLabel _currentOptionLabel;
  protected Control _currentOptionHighlight;
  private bool _isHovered;
  private bool _isOpen;

  public override void _Ready()
  {
    if (((object) this).GetType() != typeof (NDropdown))
      throw new InvalidOperationException("Don't call base._Ready(). Use ConnectSignals() instead");
    this.ConnectSignals();
  }

  protected override void ConnectSignals()
  {
    base.ConnectSignals();
    this._currentOptionHighlight = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Highlight"));
    this._currentOptionLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%Label"));
    this._dropdownContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%DropdownContainer"));
    this._dropdownItems = ((Node) this._dropdownContainer).GetNode<Control>(NodePath.op_Implicit("VBoxContainer"));
    this._dismisser = ((Node) this).GetNode<NButton>(NodePath.op_Implicit("%Dismisser"));
    ((GodotObject) this._dismisser).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.OnDismisserClicked)), 0U);
    ((GodotObject) this).Connect(CanvasItem.SignalName.VisibilityChanged, Callable.From(new Action(this.OnVisibilityChange)), 0U);
  }

  private void OnVisibilityChange()
  {
    if (((CanvasItem) this).IsVisibleInTree() || !this._isOpen)
      return;
    this.CloseDropdown();
  }

  protected void ClearDropdownItems()
  {
    foreach (Node child in ((Node) this._dropdownItems).GetChildren(false))
    {
      ((Node) this._dropdownItems).RemoveChildSafely(child);
      child.QueueFreeSafely();
    }
  }

  public override void _Input(InputEvent inputEvent)
  {
    if (!((CanvasItem) this).IsVisibleInTree() || !this._isEnabled || NDevConsole.IsConsoleVisible)
      return;
    Viewport viewport = ((Node) this).GetViewport();
    if (viewport == null)
      return;
    bool flag;
    switch (viewport.GuiGetFocusOwner())
    {
      case TextEdit _:
      case LineEdit _:
        flag = true;
        break;
      default:
        flag = false;
        break;
    }
    if (flag || !inputEvent.IsActionPressed(MegaInput.cancel, false, false) || !this._isOpen)
      return;
    this.CloseDropdown();
    viewport.SetInputAsHandled();
  }

  private void OnDismisserClicked(NButton obj) => this.CloseDropdown();

  protected override void OnRelease()
  {
    base.OnRelease();
    if (this._isOpen)
    {
      Log.Info("Closing dropdown because you clicked on the main dropdown button.");
      this.CloseDropdown();
    }
    else
      this.OpenDropdown();
  }

  private void OpenDropdown()
  {
    ((CanvasItem) this._dropdownContainer).Visible = true;
    ((CanvasItem) this._dismisser).Visible = true;
    this._isOpen = true;
    ((Node) this).GetParent().MoveChildSafely((Node) this, ((Node) this).GetParent().GetChildCount(false));
    List<NDropdownItem> list = ((IEnumerable) ((Node) this._dropdownItems).GetChildren(false)).OfType<NDropdownItem>().ToList<NDropdownItem>();
    for (int index = 0; index < list.Count; ++index)
    {
      list[index].UnhoverSelection();
      list[index].FocusNeighborLeft = ((Node) list[index]).GetPath();
      list[index].FocusNeighborRight = ((Node) list[index]).GetPath();
      list[index].FocusNeighborTop = index > 0 ? ((Node) list[index - 1]).GetPath() : ((Node) list[index]).GetPath();
      list[index].FocusNeighborBottom = index < list.Count - 1 ? ((Node) list[index + 1]).GetPath() : ((Node) list[index]).GetPath();
      list[index].FocusMode = (Control.FocusModeEnum) 2L;
    }
    NDropdownItem control = list.FirstOrDefault<NDropdownItem>();
    if (control == null)
      return;
    control.TryGrabFocus();
  }

  protected void CloseDropdown()
  {
    ((CanvasItem) this._dismisser).Visible = false;
    ((CanvasItem) this._dropdownContainer).Visible = false;
    this._isOpen = false;
    this.TryGrabFocus();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(9)
    {
      new MethodInfo(NDropdown.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDropdown.MethodName.ConnectSignals, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDropdown.MethodName.OnVisibilityChange, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDropdown.MethodName.ClearDropdownItems, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDropdown.MethodName._Input, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NDropdown.MethodName.OnDismisserClicked, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("obj"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NDropdown.MethodName.OnRelease, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDropdown.MethodName.OpenDropdown, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDropdown.MethodName.CloseDropdown, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NDropdown.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDropdown.MethodName.ConnectSignals) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ConnectSignals();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDropdown.MethodName.OnVisibilityChange) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnVisibilityChange();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDropdown.MethodName.ClearDropdownItems) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ClearDropdownItems();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDropdown.MethodName._Input) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Input(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDropdown.MethodName.OnDismisserClicked) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnDismisserClicked(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDropdown.MethodName.OnRelease) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnRelease();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDropdown.MethodName.OpenDropdown) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OpenDropdown();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NDropdown.MethodName.CloseDropdown) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.CloseDropdown();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NDropdown.MethodName._Ready) || StringName.op_Equality(ref method, NDropdown.MethodName.ConnectSignals) || StringName.op_Equality(ref method, NDropdown.MethodName.OnVisibilityChange) || StringName.op_Equality(ref method, NDropdown.MethodName.ClearDropdownItems) || StringName.op_Equality(ref method, NDropdown.MethodName._Input) || StringName.op_Equality(ref method, NDropdown.MethodName.OnDismisserClicked) || StringName.op_Equality(ref method, NDropdown.MethodName.OnRelease) || StringName.op_Equality(ref method, NDropdown.MethodName.OpenDropdown) || StringName.op_Equality(ref method, NDropdown.MethodName.CloseDropdown) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDropdown.PropertyName._dropdownContainer))
    {
      this._dropdownContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDropdown.PropertyName._dropdownItems))
    {
      this._dropdownItems = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDropdown.PropertyName._dismisser))
    {
      this._dismisser = VariantUtils.ConvertTo<NButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDropdown.PropertyName._currentOptionLabel))
    {
      this._currentOptionLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDropdown.PropertyName._currentOptionHighlight))
    {
      this._currentOptionHighlight = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDropdown.PropertyName._isHovered))
    {
      this._isHovered = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDropdown.PropertyName._isOpen))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._isOpen = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDropdown.PropertyName._dropdownContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._dropdownContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NDropdown.PropertyName._dropdownItems))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._dropdownItems);
      return true;
    }
    if (StringName.op_Equality(ref name, NDropdown.PropertyName._dismisser))
    {
      value = VariantUtils.CreateFrom<NButton>(ref this._dismisser);
      return true;
    }
    if (StringName.op_Equality(ref name, NDropdown.PropertyName._currentOptionLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._currentOptionLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NDropdown.PropertyName._currentOptionHighlight))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._currentOptionHighlight);
      return true;
    }
    if (StringName.op_Equality(ref name, NDropdown.PropertyName._isHovered))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isHovered);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDropdown.PropertyName._isOpen))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<bool>(ref this._isOpen);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NDropdown.PropertyName._dropdownContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDropdown.PropertyName._dropdownItems, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDropdown.PropertyName._dismisser, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDropdown.PropertyName._currentOptionLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDropdown.PropertyName._currentOptionHighlight, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NDropdown.PropertyName._isHovered, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NDropdown.PropertyName._isOpen, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NDropdown.PropertyName._dropdownContainer, Variant.From<Control>(ref this._dropdownContainer));
    info.AddProperty(NDropdown.PropertyName._dropdownItems, Variant.From<Control>(ref this._dropdownItems));
    info.AddProperty(NDropdown.PropertyName._dismisser, Variant.From<NButton>(ref this._dismisser));
    info.AddProperty(NDropdown.PropertyName._currentOptionLabel, Variant.From<MegaLabel>(ref this._currentOptionLabel));
    info.AddProperty(NDropdown.PropertyName._currentOptionHighlight, Variant.From<Control>(ref this._currentOptionHighlight));
    info.AddProperty(NDropdown.PropertyName._isHovered, Variant.From<bool>(ref this._isHovered));
    info.AddProperty(NDropdown.PropertyName._isOpen, Variant.From<bool>(ref this._isOpen));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NDropdown.PropertyName._dropdownContainer, ref variant1))
      this._dropdownContainer = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NDropdown.PropertyName._dropdownItems, ref variant2))
      this._dropdownItems = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NDropdown.PropertyName._dismisser, ref variant3))
      this._dismisser = ((Variant) ref variant3).As<NButton>();
    Variant variant4;
    if (info.TryGetProperty(NDropdown.PropertyName._currentOptionLabel, ref variant4))
      this._currentOptionLabel = ((Variant) ref variant4).As<MegaLabel>();
    Variant variant5;
    if (info.TryGetProperty(NDropdown.PropertyName._currentOptionHighlight, ref variant5))
      this._currentOptionHighlight = ((Variant) ref variant5).As<Control>();
    Variant variant6;
    if (info.TryGetProperty(NDropdown.PropertyName._isHovered, ref variant6))
      this._isHovered = ((Variant) ref variant6).As<bool>();
    Variant variant7;
    if (!info.TryGetProperty(NDropdown.PropertyName._isOpen, ref variant7))
      return;
    this._isOpen = ((Variant) ref variant7).As<bool>();
  }

  public new class MethodName : NClickableControl.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName ConnectSignals = StringName.op_Implicit(nameof (ConnectSignals));
    public static readonly StringName OnVisibilityChange = StringName.op_Implicit(nameof (OnVisibilityChange));
    public static readonly StringName ClearDropdownItems = StringName.op_Implicit(nameof (ClearDropdownItems));
    public static readonly StringName _Input = StringName.op_Implicit(nameof (_Input));
    public static readonly StringName OnDismisserClicked = StringName.op_Implicit(nameof (OnDismisserClicked));
    public new static readonly StringName OnRelease = StringName.op_Implicit(nameof (OnRelease));
    public static readonly StringName OpenDropdown = StringName.op_Implicit(nameof (OpenDropdown));
    public static readonly StringName CloseDropdown = StringName.op_Implicit(nameof (CloseDropdown));
  }

  public new class PropertyName : NClickableControl.PropertyName
  {
    public static readonly StringName _dropdownContainer = StringName.op_Implicit(nameof (_dropdownContainer));
    public static readonly StringName _dropdownItems = StringName.op_Implicit(nameof (_dropdownItems));
    public static readonly StringName _dismisser = StringName.op_Implicit(nameof (_dismisser));
    public static readonly StringName _currentOptionLabel = StringName.op_Implicit(nameof (_currentOptionLabel));
    public static readonly StringName _currentOptionHighlight = StringName.op_Implicit(nameof (_currentOptionHighlight));
    public new static readonly StringName _isHovered = StringName.op_Implicit(nameof (_isHovered));
    public static readonly StringName _isOpen = StringName.op_Implicit(nameof (_isOpen));
  }

  public new class SignalName : NClickableControl.SignalName
  {
  }
}
