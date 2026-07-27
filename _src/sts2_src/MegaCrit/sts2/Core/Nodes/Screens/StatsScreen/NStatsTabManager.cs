// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.StatsScreen.NStatsTabManager
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Debug;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.StatsScreen;

[ScriptPath("res://src/Core/Nodes/Screens/StatsScreen/NStatsTabManager.cs")]
public class NStatsTabManager : Control
{
  private static readonly StringName _tabLeftHotkey = MegaInput.viewDeckAndTabLeft;
  private static readonly StringName _tabRightHotkey = MegaInput.viewExhaustPileAndTabRight;
  private Control _leftTriggerIcon;
  private Control _rightTriggerIcon;
  private Control _tabContainer;
  private List<NSettingsTab> _tabs;
  private NSettingsTab? _currentTab;

  public override void _Ready()
  {
    this._leftTriggerIcon = ((Node) this).GetNode<Control>(NodePath.op_Implicit("LeftTriggerIcon"));
    this._rightTriggerIcon = ((Node) this).GetNode<Control>(NodePath.op_Implicit("RightTriggerIcon"));
    this._tabContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("TabContainer"));
    this._tabs = ((IEnumerable) ((Node) this._tabContainer).GetChildren(false)).OfType<NSettingsTab>().ToList<NSettingsTab>();
    ((GodotObject) NControllerManager.Instance).Connect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.UpdateControllerButton)), 0U);
    ((GodotObject) NControllerManager.Instance).Connect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.UpdateControllerButton)), 0U);
    ((GodotObject) NInputManager.Instance).Connect(NInputManager.SignalName.InputRebound, Callable.From(new Action(this.UpdateControllerButton)), 0U);
    foreach (NSettingsTab tab in this._tabs)
    {
      NSettingsTab nSettingsTab = tab;
      ((GodotObject) nSettingsTab).Connect(NClickableControl.SignalName.Released, Callable.From<NClickableControl>((Action<NClickableControl>) (_ => this.SwitchToTab(nSettingsTab))), 0U);
    }
    this.UpdateControllerButton();
  }

  public void ResetTabs()
  {
    this.SwitchToTab(((Node) this._tabContainer).GetChild<NSettingsTab>(0, false));
  }

  public override void _Input(InputEvent inputEvent)
  {
    if (!((CanvasItem) this).IsVisibleInTree() || NDevConsole.IsConsoleVisible)
      return;
    bool flag;
    switch (((Node) this).GetViewport().GuiGetFocusOwner())
    {
      case TextEdit _:
      case LineEdit _:
        flag = true;
        break;
      default:
        flag = false;
        break;
    }
    if (flag)
      return;
    if (inputEvent.IsActionPressed(NStatsTabManager._tabLeftHotkey, false, false))
    {
      int index = this._tabs.IndexOf(this._currentTab) - 1;
      if (index >= 0)
      {
        this._tabs[index].ForceTabPressed();
        this.SwitchToTab(this._tabs[index]);
      }
    }
    if (!inputEvent.IsActionPressed(NStatsTabManager._tabRightHotkey, false, false))
      return;
    int index1 = Math.Min(this._tabs.Count - 1, this._tabs.IndexOf(this._currentTab) + 1);
    if (index1 >= this._tabs.Count)
      return;
    this._tabs[index1].ForceTabPressed();
  }

  private void SwitchToTab(NSettingsTab tab)
  {
    this._currentTab = tab;
    foreach (NSettingsTab tab1 in this._tabs)
    {
      if (tab1 != this._currentTab)
        tab1.Deselect();
      else
        tab1.Select();
    }
  }

  private void UpdateControllerButton()
  {
    ((CanvasItem) this._leftTriggerIcon).Visible = NControllerManager.Instance.IsUsingController;
    ((CanvasItem) this._rightTriggerIcon).Visible = NControllerManager.Instance.IsUsingController;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NStatsTabManager.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NStatsTabManager.MethodName.ResetTabs, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NStatsTabManager.MethodName._Input, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NStatsTabManager.MethodName.SwitchToTab, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("tab"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NStatsTabManager.MethodName.UpdateControllerButton, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NStatsTabManager.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NStatsTabManager.MethodName.ResetTabs) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ResetTabs();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NStatsTabManager.MethodName._Input) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Input(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NStatsTabManager.MethodName.SwitchToTab) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SwitchToTab(VariantUtils.ConvertTo<NSettingsTab>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NStatsTabManager.MethodName.UpdateControllerButton) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.UpdateControllerButton();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NStatsTabManager.MethodName._Ready) || StringName.op_Equality(ref method, NStatsTabManager.MethodName.ResetTabs) || StringName.op_Equality(ref method, NStatsTabManager.MethodName._Input) || StringName.op_Equality(ref method, NStatsTabManager.MethodName.SwitchToTab) || StringName.op_Equality(ref method, NStatsTabManager.MethodName.UpdateControllerButton) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NStatsTabManager.PropertyName._leftTriggerIcon))
    {
      this._leftTriggerIcon = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NStatsTabManager.PropertyName._rightTriggerIcon))
    {
      this._rightTriggerIcon = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NStatsTabManager.PropertyName._tabContainer))
    {
      this._tabContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NStatsTabManager.PropertyName._currentTab))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._currentTab = VariantUtils.ConvertTo<NSettingsTab>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NStatsTabManager.PropertyName._leftTriggerIcon))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._leftTriggerIcon);
      return true;
    }
    if (StringName.op_Equality(ref name, NStatsTabManager.PropertyName._rightTriggerIcon))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._rightTriggerIcon);
      return true;
    }
    if (StringName.op_Equality(ref name, NStatsTabManager.PropertyName._tabContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._tabContainer);
      return true;
    }
    if (!StringName.op_Equality(ref name, NStatsTabManager.PropertyName._currentTab))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<NSettingsTab>(ref this._currentTab);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NStatsTabManager.PropertyName._leftTriggerIcon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NStatsTabManager.PropertyName._rightTriggerIcon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NStatsTabManager.PropertyName._tabContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NStatsTabManager.PropertyName._currentTab, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NStatsTabManager.PropertyName._leftTriggerIcon, Variant.From<Control>(ref this._leftTriggerIcon));
    info.AddProperty(NStatsTabManager.PropertyName._rightTriggerIcon, Variant.From<Control>(ref this._rightTriggerIcon));
    info.AddProperty(NStatsTabManager.PropertyName._tabContainer, Variant.From<Control>(ref this._tabContainer));
    info.AddProperty(NStatsTabManager.PropertyName._currentTab, Variant.From<NSettingsTab>(ref this._currentTab));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NStatsTabManager.PropertyName._leftTriggerIcon, ref variant1))
      this._leftTriggerIcon = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NStatsTabManager.PropertyName._rightTriggerIcon, ref variant2))
      this._rightTriggerIcon = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NStatsTabManager.PropertyName._tabContainer, ref variant3))
      this._tabContainer = ((Variant) ref variant3).As<Control>();
    Variant variant4;
    if (!info.TryGetProperty(NStatsTabManager.PropertyName._currentTab, ref variant4))
      return;
    this._currentTab = ((Variant) ref variant4).As<NSettingsTab>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName ResetTabs = StringName.op_Implicit(nameof (ResetTabs));
    public static readonly StringName _Input = StringName.op_Implicit(nameof (_Input));
    public static readonly StringName SwitchToTab = StringName.op_Implicit(nameof (SwitchToTab));
    public static readonly StringName UpdateControllerButton = StringName.op_Implicit(nameof (UpdateControllerButton));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _leftTriggerIcon = StringName.op_Implicit(nameof (_leftTriggerIcon));
    public static readonly StringName _rightTriggerIcon = StringName.op_Implicit(nameof (_rightTriggerIcon));
    public static readonly StringName _tabContainer = StringName.op_Implicit(nameof (_tabContainer));
    public static readonly StringName _currentTab = StringName.op_Implicit(nameof (_currentTab));
  }

  public class SignalName : Control.SignalName
  {
  }
}
