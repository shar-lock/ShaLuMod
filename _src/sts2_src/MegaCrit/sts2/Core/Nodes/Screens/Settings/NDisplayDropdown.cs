// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Settings.NDisplayDropdown
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings;

[ScriptPath("res://src/Core/Nodes/Screens/Settings/NDisplayDropdown.cs")]
public class NDisplayDropdown : NSettingsDropdown
{
  [Export]
  private PackedScene _dropdownItemScene;
  private static readonly LocString _optionString = new LocString("settings_ui", "DISPLAY_DROPDOWN_OPTION");
  private int _currentDisplayIndex = -1;

  public override void _Ready()
  {
    this.ConnectSignals();
    ((GodotObject) NGame.Instance).Connect(NGame.SignalName.WindowChange, Callable.From<bool>(new Action<bool>(this.OnWindowChange)), 0U);
    this.OnWindowChange(SaveManager.Instance.SettingsSave.AspectRatioSetting == AspectRatioSetting.Auto);
    this.ClearDropdownItems();
    for (int setIndex = 0; setIndex < DisplayServer.GetScreenCount(); ++setIndex)
    {
      NDisplayDropdownItem child = this._dropdownItemScene.Instantiate<NDisplayDropdownItem>((PackedScene.GenEditState) 0L);
      ((Node) this._dropdownItems).AddChildSafely((Node) child);
      ((GodotObject) child).Connect(NDropdownItem.SignalName.Selected, Callable.From<NDropdownItem>(new Action<NDropdownItem>(this.OnDropdownItemSelected)), 0U);
      child.Init(setIndex);
    }
    ((Node) this._dropdownItems).GetParent<NDropdownContainer>().RefreshLayout();
  }

  public override void _Notification(int what)
  {
    if (what != 1012 || !((Node) this).IsNodeReady())
      return;
    this.OnWindowChange(SaveManager.Instance.SettingsSave.AspectRatioSetting == AspectRatioSetting.Auto);
  }

  private void OnWindowChange(bool _)
  {
    long currentScreen = (long) ((Node) this).GetWindow().CurrentScreen;
    if (currentScreen == (long) this._currentDisplayIndex)
      return;
    this._currentDisplayIndex = (int) currentScreen;
    NDisplayDropdown._optionString.Add("MonitorIndex", (Decimal) this._currentDisplayIndex);
    this._currentOptionLabel.SetTextAutoSize(NDisplayDropdown._optionString.GetFormattedText());
    SaveManager.Instance.SettingsSave.TargetDisplay = this._currentDisplayIndex;
    NResolutionDropdown instance = NResolutionDropdown.Instance;
    if ((instance != null ? (((Node) instance).IsNodeReady() ? 1 : 0) : 0) == 0)
      return;
    NResolutionDropdown.Instance.RefreshCurrentlySelectedResolution();
    NResolutionDropdown.Instance.PopulateDropdownItems();
  }

  private void OnDropdownItemSelected(NDropdownItem nDropdownItem)
  {
    SettingsSave settingsSave = SaveManager.Instance.SettingsSave;
    NDisplayDropdownItem ndisplayDropdownItem = (NDisplayDropdownItem) nDropdownItem;
    if (ndisplayDropdownItem.displayIndex == this._currentDisplayIndex)
      return;
    this.CloseDropdown();
    settingsSave.TargetDisplay = ndisplayDropdownItem.displayIndex;
    if (!settingsSave.Fullscreen && !PlatformUtil.GetSupportedWindowMode().ShouldForceFullscreen())
    {
      Vector2I size = DisplayServer.ScreenGetSize(ndisplayDropdownItem.displayIndex);
      settingsSave.WindowPosition = Vector2I.op_Division(size, 8);
      settingsSave.WindowSize = new Vector2I((int) ((double) size.X * 0.75), (int) ((double) size.Y * 0.75));
    }
    NResolutionDropdown.Instance.RefreshCurrentlySelectedResolution();
    NResolutionDropdown.Instance.PopulateDropdownItems();
    NGame.Instance.ApplyDisplaySettings();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NDisplayDropdown.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDisplayDropdown.MethodName._Notification, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("what"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NDisplayDropdown.MethodName.OnWindowChange, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NDisplayDropdown.MethodName.OnDropdownItemSelected, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("nDropdownItem"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NDisplayDropdown.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDisplayDropdown.MethodName._Notification) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((GodotObject) this)._Notification(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDisplayDropdown.MethodName.OnWindowChange) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnWindowChange(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NDisplayDropdown.MethodName.OnDropdownItemSelected) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnDropdownItemSelected(VariantUtils.ConvertTo<NDropdownItem>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NDisplayDropdown.MethodName._Ready) || StringName.op_Equality(ref method, NDisplayDropdown.MethodName._Notification) || StringName.op_Equality(ref method, NDisplayDropdown.MethodName.OnWindowChange) || StringName.op_Equality(ref method, NDisplayDropdown.MethodName.OnDropdownItemSelected) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDisplayDropdown.PropertyName._dropdownItemScene))
    {
      this._dropdownItemScene = VariantUtils.ConvertTo<PackedScene>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDisplayDropdown.PropertyName._currentDisplayIndex))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._currentDisplayIndex = VariantUtils.ConvertTo<int>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDisplayDropdown.PropertyName._dropdownItemScene))
    {
      value = VariantUtils.CreateFrom<PackedScene>(ref this._dropdownItemScene);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDisplayDropdown.PropertyName._currentDisplayIndex))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<int>(ref this._currentDisplayIndex);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NDisplayDropdown.PropertyName._dropdownItemScene, (PropertyHint) 17L, "PackedScene", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 2L, NDisplayDropdown.PropertyName._currentDisplayIndex, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NDisplayDropdown.PropertyName._dropdownItemScene, Variant.From<PackedScene>(ref this._dropdownItemScene));
    info.AddProperty(NDisplayDropdown.PropertyName._currentDisplayIndex, Variant.From<int>(ref this._currentDisplayIndex));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NDisplayDropdown.PropertyName._dropdownItemScene, ref variant1))
      this._dropdownItemScene = ((Variant) ref variant1).As<PackedScene>();
    Variant variant2;
    if (!info.TryGetProperty(NDisplayDropdown.PropertyName._currentDisplayIndex, ref variant2))
      return;
    this._currentDisplayIndex = ((Variant) ref variant2).As<int>();
  }

  public new class MethodName : NSettingsDropdown.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _Notification = StringName.op_Implicit(nameof (_Notification));
    public static readonly StringName OnWindowChange = StringName.op_Implicit(nameof (OnWindowChange));
    public static readonly StringName OnDropdownItemSelected = StringName.op_Implicit(nameof (OnDropdownItemSelected));
  }

  public new class PropertyName : NSettingsDropdown.PropertyName
  {
    public static readonly StringName _dropdownItemScene = StringName.op_Implicit(nameof (_dropdownItemScene));
    public static readonly StringName _currentDisplayIndex = StringName.op_Implicit(nameof (_currentDisplayIndex));
  }

  public new class SignalName : NSettingsDropdown.SignalName
  {
  }
}
