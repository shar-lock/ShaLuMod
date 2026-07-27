// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Settings.NSettingsTabManager
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings;

[ScriptPath("res://src/Core/Nodes/Screens/Settings/NSettingsTabManager.cs")]
public class NSettingsTabManager : Control
{
  private const float _scrollPaddingTop = 20f;
  private const float _scrollPaddingBottom = 30f;
  private static readonly StringName _tabLeftHotkey = MegaInput.viewDeckAndTabLeft;
  private static readonly StringName _tabRightHotkey = MegaInput.viewExhaustPileAndTabRight;
  private NSettingsTab? _currentTab;
  private NScrollableContainer _scrollContainer;
  private readonly Dictionary<NSettingsTab, NSettingsPanel> _tabs = new Dictionary<NSettingsTab, NSettingsPanel>();
  private TextureRect _leftTriggerIcon;
  private TextureRect _rightTriggerIcon;
  private Tween? _scrollbarTween;
  private 
  #nullable disable
  NSettingsTabManager.TabChangedEventHandler backing_TabChanged;

  private 
  #nullable enable
  NSettingsPanel CurrentlyDisplayedPanel => this._tabs[this._currentTab];

  public Control? DefaultFocusedControl
  {
    get
    {
      return this._currentTab == null ? (Control) null : this._tabs[this._currentTab].DefaultFocusedControl;
    }
  }

  public override void _Ready()
  {
    this._leftTriggerIcon = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("LeftTriggerIcon"));
    this._rightTriggerIcon = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("RightTriggerIcon"));
    this._scrollContainer = ((Node) this).GetNode<NScrollableContainer>(NodePath.op_Implicit("%ScrollContainer"));
    this._scrollContainer.DisableScrollingIfContentFits();
    NSettingsTab node1 = ((Node) this).GetNode<NSettingsTab>(NodePath.op_Implicit("General"));
    node1.SetLabel(new LocString("settings_ui", "TAB_GENERAL").GetFormattedText());
    this._tabs.Add(node1, ((Node) this).GetNode<NSettingsPanel>(NodePath.op_Implicit("%GeneralSettings")));
    NSettingsTab node2 = ((Node) this).GetNode<NSettingsTab>(NodePath.op_Implicit("Graphics"));
    node2.SetLabel(new LocString("settings_ui", "TAB_GRAPHICS").GetFormattedText());
    this._tabs.Add(node2, ((Node) this).GetNode<NSettingsPanel>(NodePath.op_Implicit("%GraphicsSettings")));
    NSettingsTab node3 = ((Node) this).GetNode<NSettingsTab>(NodePath.op_Implicit("Sound"));
    node3.SetLabel(new LocString("settings_ui", "TAB_SOUND").GetFormattedText());
    this._tabs.Add(node3, ((Node) this).GetNode<NSettingsPanel>(NodePath.op_Implicit("%SoundSettings")));
    NSettingsTab node4 = ((Node) this).GetNode<NSettingsTab>(NodePath.op_Implicit("Input"));
    node4.SetLabel(new LocString("settings_ui", "TAB_INPUT").GetFormattedText());
    this._tabs.Add(node4, ((Node) this).GetNode<NSettingsPanel>(NodePath.op_Implicit("%InputSettings")));
    foreach (NSettingsTab key in this._tabs.Keys)
    {
      NSettingsTab tab = key;
      ((GodotObject) tab).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>((Action<NButton>) (_ => this.SwitchTabTo(tab))), 0U);
    }
    ((GodotObject) NControllerManager.Instance).Connect(NControllerManager.SignalName.MouseDetected, Callable.From(new Action(this.UpdateControllerButton)), 0U);
    ((GodotObject) NControllerManager.Instance).Connect(NControllerManager.SignalName.ControllerDetected, Callable.From(new Action(this.UpdateControllerButton)), 0U);
    ((GodotObject) NInputManager.Instance).Connect(NInputManager.SignalName.InputRebound, Callable.From(new Action(this.UpdateControllerButton)), 0U);
    this.UpdateControllerButton();
  }

  public void ResetTabs()
  {
    this._tabs.First<KeyValuePair<NSettingsTab, NSettingsPanel>>().Key.Select();
    this.SwitchTabTo(this._tabs.First<KeyValuePair<NSettingsTab, NSettingsPanel>>().Key);
  }

  public void Enable()
  {
    NHotkeyManager.Instance.PushHotkeyPressedBinding(StringName.op_Implicit(NSettingsTabManager._tabLeftHotkey), new Action(this.TabLeft));
    NHotkeyManager.Instance.PushHotkeyPressedBinding(StringName.op_Implicit(NSettingsTabManager._tabRightHotkey), new Action(this.TabRight));
  }

  public void Disable()
  {
    NHotkeyManager.Instance.RemoveHotkeyPressedBinding(StringName.op_Implicit(NSettingsTabManager._tabLeftHotkey), new Action(this.TabLeft));
    NHotkeyManager.Instance.RemoveHotkeyPressedBinding(StringName.op_Implicit(NSettingsTabManager._tabRightHotkey), new Action(this.TabRight));
  }

  private void TabLeft()
  {
    List<NSettingsTab> list = this._tabs.Keys.ToList<NSettingsTab>();
    int index = list.IndexOf(this._currentTab) - 1;
    if (index < 0)
      return;
    this.SwitchTabTo(list[index]);
  }

  private void TabRight()
  {
    List<NSettingsTab> list = this._tabs.Keys.ToList<NSettingsTab>();
    int index = Math.Min(list.Count - 1, list.IndexOf(this._currentTab) + 1);
    if (index >= list.Count)
      return;
    this.SwitchTabTo(list[index]);
  }

  private void SwitchTabTo(NSettingsTab selectedTab)
  {
    if (selectedTab != this._currentTab)
    {
      foreach (NSettingsTab key in this._tabs.Keys)
      {
        key.Deselect();
        ((CanvasItem) this._tabs[key]).Visible = false;
      }
      selectedTab.Select();
      ((CanvasItem) this._tabs[selectedTab]).Visible = true;
      this._currentTab = selectedTab;
      this._scrollContainer.SetContent((Control) this.CurrentlyDisplayedPanel, 20f, 30f);
      this._scrollContainer.InstantlyScrollToTop();
      this._scrollbarTween?.Kill();
      this._scrollbarTween = ((Node) this).CreateTween().SetParallel(true);
      this._scrollbarTween.TweenProperty((GodotObject) this._scrollContainer.Scrollbar, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.5).From(Variant.op_Implicit(StsColors.transparentBlack)).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 7L);
    }
    ActiveScreenContext.Instance.Update();
  }

  private void UpdateControllerButton()
  {
    ((CanvasItem) this._leftTriggerIcon).Visible = NControllerManager.Instance.IsUsingController;
    ((CanvasItem) this._rightTriggerIcon).Visible = NControllerManager.Instance.IsUsingController;
    this._leftTriggerIcon.Texture = NInputManager.Instance.GetHotkeyIcon(StringName.op_Implicit(MegaInput.viewDeckAndTabLeft));
    this._rightTriggerIcon.Texture = NInputManager.Instance.GetHotkeyIcon(StringName.op_Implicit(MegaInput.viewExhaustPileAndTabRight));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(8)
    {
      new MethodInfo(NSettingsTabManager.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSettingsTabManager.MethodName.ResetTabs, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSettingsTabManager.MethodName.Enable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSettingsTabManager.MethodName.Disable, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSettingsTabManager.MethodName.TabLeft, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSettingsTabManager.MethodName.TabRight, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSettingsTabManager.MethodName.SwitchTabTo, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("selectedTab"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NSettingsTabManager.MethodName.UpdateControllerButton, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NSettingsTabManager.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSettingsTabManager.MethodName.ResetTabs) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ResetTabs();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSettingsTabManager.MethodName.Enable) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Enable();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSettingsTabManager.MethodName.Disable) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Disable();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSettingsTabManager.MethodName.TabLeft) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.TabLeft();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSettingsTabManager.MethodName.TabRight) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.TabRight();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSettingsTabManager.MethodName.SwitchTabTo) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SwitchTabTo(VariantUtils.ConvertTo<NSettingsTab>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NSettingsTabManager.MethodName.UpdateControllerButton) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.UpdateControllerButton();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NSettingsTabManager.MethodName._Ready) || StringName.op_Equality(ref method, NSettingsTabManager.MethodName.ResetTabs) || StringName.op_Equality(ref method, NSettingsTabManager.MethodName.Enable) || StringName.op_Equality(ref method, NSettingsTabManager.MethodName.Disable) || StringName.op_Equality(ref method, NSettingsTabManager.MethodName.TabLeft) || StringName.op_Equality(ref method, NSettingsTabManager.MethodName.TabRight) || StringName.op_Equality(ref method, NSettingsTabManager.MethodName.SwitchTabTo) || StringName.op_Equality(ref method, NSettingsTabManager.MethodName.UpdateControllerButton) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSettingsTabManager.PropertyName._currentTab))
    {
      this._currentTab = VariantUtils.ConvertTo<NSettingsTab>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSettingsTabManager.PropertyName._scrollContainer))
    {
      this._scrollContainer = VariantUtils.ConvertTo<NScrollableContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSettingsTabManager.PropertyName._leftTriggerIcon))
    {
      this._leftTriggerIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSettingsTabManager.PropertyName._rightTriggerIcon))
    {
      this._rightTriggerIcon = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSettingsTabManager.PropertyName._scrollbarTween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._scrollbarTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSettingsTabManager.PropertyName.CurrentlyDisplayedPanel))
    {
      ref godot_variant local = ref value;
      NSettingsPanel currentlyDisplayedPanel = this.CurrentlyDisplayedPanel;
      godot_variant from = VariantUtils.CreateFrom<NSettingsPanel>(ref currentlyDisplayedPanel);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NSettingsTabManager.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NSettingsTabManager.PropertyName._currentTab))
    {
      value = VariantUtils.CreateFrom<NSettingsTab>(ref this._currentTab);
      return true;
    }
    if (StringName.op_Equality(ref name, NSettingsTabManager.PropertyName._scrollContainer))
    {
      value = VariantUtils.CreateFrom<NScrollableContainer>(ref this._scrollContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NSettingsTabManager.PropertyName._leftTriggerIcon))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._leftTriggerIcon);
      return true;
    }
    if (StringName.op_Equality(ref name, NSettingsTabManager.PropertyName._rightTriggerIcon))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._rightTriggerIcon);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSettingsTabManager.PropertyName._scrollbarTween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._scrollbarTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NSettingsTabManager.PropertyName._currentTab, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSettingsTabManager.PropertyName._scrollContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSettingsTabManager.PropertyName.CurrentlyDisplayedPanel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSettingsTabManager.PropertyName._leftTriggerIcon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSettingsTabManager.PropertyName._rightTriggerIcon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSettingsTabManager.PropertyName._scrollbarTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSettingsTabManager.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NSettingsTabManager.PropertyName._currentTab, Variant.From<NSettingsTab>(ref this._currentTab));
    info.AddProperty(NSettingsTabManager.PropertyName._scrollContainer, Variant.From<NScrollableContainer>(ref this._scrollContainer));
    info.AddProperty(NSettingsTabManager.PropertyName._leftTriggerIcon, Variant.From<TextureRect>(ref this._leftTriggerIcon));
    info.AddProperty(NSettingsTabManager.PropertyName._rightTriggerIcon, Variant.From<TextureRect>(ref this._rightTriggerIcon));
    info.AddProperty(NSettingsTabManager.PropertyName._scrollbarTween, Variant.From<Tween>(ref this._scrollbarTween));
    info.AddSignalEventDelegate(NSettingsTabManager.SignalName.TabChanged, (Delegate) this.backing_TabChanged);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NSettingsTabManager.PropertyName._currentTab, ref variant1))
      this._currentTab = ((Variant) ref variant1).As<NSettingsTab>();
    Variant variant2;
    if (info.TryGetProperty(NSettingsTabManager.PropertyName._scrollContainer, ref variant2))
      this._scrollContainer = ((Variant) ref variant2).As<NScrollableContainer>();
    Variant variant3;
    if (info.TryGetProperty(NSettingsTabManager.PropertyName._leftTriggerIcon, ref variant3))
      this._leftTriggerIcon = ((Variant) ref variant3).As<TextureRect>();
    Variant variant4;
    if (info.TryGetProperty(NSettingsTabManager.PropertyName._rightTriggerIcon, ref variant4))
      this._rightTriggerIcon = ((Variant) ref variant4).As<TextureRect>();
    Variant variant5;
    if (info.TryGetProperty(NSettingsTabManager.PropertyName._scrollbarTween, ref variant5))
      this._scrollbarTween = ((Variant) ref variant5).As<Tween>();
    NSettingsTabManager.TabChangedEventHandler changedEventHandler;
    if (!info.TryGetSignalEventDelegate<NSettingsTabManager.TabChangedEventHandler>(NSettingsTabManager.SignalName.TabChanged, ref changedEventHandler))
      return;
    this.backing_TabChanged = changedEventHandler;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<MethodInfo> GetGodotSignalList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NSettingsTabManager.SignalName.TabChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  public event NSettingsTabManager.TabChangedEventHandler TabChanged
  {
    add => this.backing_TabChanged += value;
    remove => this.backing_TabChanged -= value;
  }

  protected void EmitSignalTabChanged()
  {
    ((GodotObject) this).EmitSignal(NSettingsTabManager.SignalName.TabChanged, Array.Empty<Variant>());
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RaiseGodotClassSignalCallbacks(
    in godot_string_name signal,
    NativeVariantPtrArgs args)
  {
    if (StringName.op_Equality(ref signal, NSettingsTabManager.SignalName.TabChanged) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NSettingsTabManager.TabChangedEventHandler backingTabChanged = this.backing_TabChanged;
      if (backingTabChanged == null)
        return;
      backingTabChanged();
    }
    else
      ((GodotObject) this).RaiseGodotClassSignalCallbacks(ref signal, args);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassSignal(in godot_string_name signal)
  {
    return StringName.op_Equality(ref signal, NSettingsTabManager.SignalName.TabChanged) || base.HasGodotClassSignal(ref signal);
  }

  [Signal]
  public delegate void TabChangedEventHandler();

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName ResetTabs = StringName.op_Implicit(nameof (ResetTabs));
    public static readonly StringName Enable = StringName.op_Implicit(nameof (Enable));
    public static readonly StringName Disable = StringName.op_Implicit(nameof (Disable));
    public static readonly StringName TabLeft = StringName.op_Implicit(nameof (TabLeft));
    public static readonly StringName TabRight = StringName.op_Implicit(nameof (TabRight));
    public static readonly StringName SwitchTabTo = StringName.op_Implicit(nameof (SwitchTabTo));
    public static readonly StringName UpdateControllerButton = StringName.op_Implicit(nameof (UpdateControllerButton));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName CurrentlyDisplayedPanel = StringName.op_Implicit(nameof (CurrentlyDisplayedPanel));
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _currentTab = StringName.op_Implicit(nameof (_currentTab));
    public static readonly StringName _scrollContainer = StringName.op_Implicit(nameof (_scrollContainer));
    public static readonly StringName _leftTriggerIcon = StringName.op_Implicit(nameof (_leftTriggerIcon));
    public static readonly StringName _rightTriggerIcon = StringName.op_Implicit(nameof (_rightTriggerIcon));
    public static readonly StringName _scrollbarTween = StringName.op_Implicit(nameof (_scrollbarTween));
  }

  public class SignalName : Control.SignalName
  {
    public static readonly StringName TabChanged = StringName.op_Implicit(nameof (TabChanged));
  }
}
