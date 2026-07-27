// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Settings.NSettingsPanel
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings;

[ScriptPath("res://src/Core/Nodes/Screens/Settings/NSettingsPanel.cs")]
public class NSettingsPanel : Control
{
  private float _minPadding = 50f;
  protected Control? _firstControl;
  private Tween? _tween;

  public VBoxContainer Content { get; private set; }

  public Control? DefaultFocusedControl => this._firstControl;

  public override void _Ready()
  {
    this.Content = ((Node) this).GetNode<VBoxContainer>(NodePath.op_Implicit("VBoxContainer"));
    ((GodotObject) this).Connect(CanvasItem.SignalName.VisibilityChanged, Callable.From(new Action(this.OnVisibilityChange)), 0U);
    ((GodotObject) ((Node) this).GetViewport()).Connect(Viewport.SignalName.SizeChanged, Callable.From(new Action(this.RefreshSize)), 0U);
    this.RefreshSize();
    List<Control> ancestors = new List<Control>();
    this.GetSettingsOptionsRecursive((Control) this.Content, ancestors, true);
    foreach (GodotObject godotObject in ancestors)
      godotObject.Connect(CanvasItem.SignalName.VisibilityChanged, Callable.From(new Action(this.UpdateNavigation)), 0U);
    this.UpdateNavigation();
  }

  private void RefreshSize()
  {
    Vector2 size = ((Node) this).GetParent<Control>().Size;
    Vector2 minimumSize = ((Control) this.Content).GetMinimumSize();
    if ((double) minimumSize.Y + (double) this._minPadding >= (double) size.Y)
      this.Size = new Vector2(((Control) this.Content).Size.X, minimumSize.Y + size.Y * 0.4f);
    else
      this.Size = new Vector2(((Control) this.Content).Size.X, minimumSize.Y);
  }

  protected virtual void OnVisibilityChange()
  {
    if (!((CanvasItem) this).Visible)
      return;
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.5).From(Variant.op_Implicit(StsColors.transparentBlack)).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
  }

  protected virtual void UpdateNavigation()
  {
    List<Control> controlList = new List<Control>();
    this.GetSettingsOptionsRecursive((Control) this.Content, controlList);
    for (int index = 0; index < controlList.Count; ++index)
    {
      controlList[index].FocusNeighborLeft = ((Node) controlList[index]).GetPath();
      controlList[index].FocusNeighborRight = ((Node) controlList[index]).GetPath();
      controlList[index].FocusNeighborTop = index > 0 ? ((Node) controlList[index - 1]).GetPath() : ((Node) controlList[index]).GetPath();
      controlList[index].FocusNeighborBottom = index < controlList.Count - 1 ? ((Node) controlList[index + 1]).GetPath() : ((Node) controlList[index]).GetPath();
    }
    this._firstControl = controlList.FirstOrDefault<Control>();
  }

  private void GetSettingsOptionsRecursive(
    Control parent,
    List<Control> ancestors,
    bool includeInvisibleSettings = false)
  {
    foreach (Control control in ((IEnumerable) ((Node) parent).GetChildren(false)).OfType<Control>())
    {
      if (!this.IsSettingsOption(control))
        this.GetSettingsOptionsRecursive(control, ancestors, includeInvisibleSettings);
      else if (((CanvasItem) ((Node) control).GetParent<Control>()).IsVisible() | includeInvisibleSettings && control.FocusMode == 2L)
        ancestors.Add(control);
    }
  }

  private bool IsSettingsOption(Control c)
  {
    bool flag;
    switch (c)
    {
      case NButton nbutton:
        return nbutton.IsEnabled;
      case NPaginator _:
      case NTickbox _:
      case NButton _:
      case NDropdownPositioner _:
      case NSettingsSlider _:
        flag = true;
        break;
      default:
        flag = false;
        break;
    }
    return flag;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NSettingsPanel.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSettingsPanel.MethodName.RefreshSize, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSettingsPanel.MethodName.OnVisibilityChange, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSettingsPanel.MethodName.UpdateNavigation, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSettingsPanel.MethodName.IsSettingsOption, new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("c"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NSettingsPanel.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSettingsPanel.MethodName.RefreshSize) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RefreshSize();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSettingsPanel.MethodName.OnVisibilityChange) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnVisibilityChange();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSettingsPanel.MethodName.UpdateNavigation) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateNavigation();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NSettingsPanel.MethodName.IsSettingsOption) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    bool flag = this.IsSettingsOption(VariantUtils.ConvertTo<Control>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = VariantUtils.CreateFrom<bool>(ref flag);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NSettingsPanel.MethodName._Ready) || StringName.op_Equality(ref method, NSettingsPanel.MethodName.RefreshSize) || StringName.op_Equality(ref method, NSettingsPanel.MethodName.OnVisibilityChange) || StringName.op_Equality(ref method, NSettingsPanel.MethodName.UpdateNavigation) || StringName.op_Equality(ref method, NSettingsPanel.MethodName.IsSettingsOption) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSettingsPanel.PropertyName.Content))
    {
      this.Content = VariantUtils.ConvertTo<VBoxContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSettingsPanel.PropertyName._minPadding))
    {
      this._minPadding = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSettingsPanel.PropertyName._firstControl))
    {
      this._firstControl = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSettingsPanel.PropertyName._tween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._tween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSettingsPanel.PropertyName.Content))
    {
      ref godot_variant local = ref value;
      VBoxContainer content = this.Content;
      godot_variant from = VariantUtils.CreateFrom<VBoxContainer>(ref content);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NSettingsPanel.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NSettingsPanel.PropertyName._minPadding))
    {
      value = VariantUtils.CreateFrom<float>(ref this._minPadding);
      return true;
    }
    if (StringName.op_Equality(ref name, NSettingsPanel.PropertyName._firstControl))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._firstControl);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSettingsPanel.PropertyName._tween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._tween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 3L, NSettingsPanel.PropertyName._minPadding, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSettingsPanel.PropertyName._firstControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSettingsPanel.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSettingsPanel.PropertyName.Content, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSettingsPanel.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName content1 = NSettingsPanel.PropertyName.Content;
    VBoxContainer content2 = this.Content;
    Variant variant = Variant.From<VBoxContainer>(ref content2);
    serializationInfo.AddProperty(content1, variant);
    info.AddProperty(NSettingsPanel.PropertyName._minPadding, Variant.From<float>(ref this._minPadding));
    info.AddProperty(NSettingsPanel.PropertyName._firstControl, Variant.From<Control>(ref this._firstControl));
    info.AddProperty(NSettingsPanel.PropertyName._tween, Variant.From<Tween>(ref this._tween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NSettingsPanel.PropertyName.Content, ref variant1))
      this.Content = ((Variant) ref variant1).As<VBoxContainer>();
    Variant variant2;
    if (info.TryGetProperty(NSettingsPanel.PropertyName._minPadding, ref variant2))
      this._minPadding = ((Variant) ref variant2).As<float>();
    Variant variant3;
    if (info.TryGetProperty(NSettingsPanel.PropertyName._firstControl, ref variant3))
      this._firstControl = ((Variant) ref variant3).As<Control>();
    Variant variant4;
    if (!info.TryGetProperty(NSettingsPanel.PropertyName._tween, ref variant4))
      return;
    this._tween = ((Variant) ref variant4).As<Tween>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName RefreshSize = StringName.op_Implicit(nameof (RefreshSize));
    public static readonly StringName OnVisibilityChange = StringName.op_Implicit(nameof (OnVisibilityChange));
    public static readonly StringName UpdateNavigation = StringName.op_Implicit(nameof (UpdateNavigation));
    public static readonly StringName IsSettingsOption = StringName.op_Implicit(nameof (IsSettingsOption));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName Content = StringName.op_Implicit(nameof (Content));
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _minPadding = StringName.op_Implicit(nameof (_minPadding));
    public static readonly StringName _firstControl = StringName.op_Implicit(nameof (_firstControl));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
  }

  public class SignalName : Control.SignalName
  {
  }
}
