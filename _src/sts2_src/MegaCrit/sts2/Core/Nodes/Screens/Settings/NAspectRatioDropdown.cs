// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Settings.NAspectRatioDropdown
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings;

[ScriptPath("res://src/Core/Nodes/Screens/Settings/NAspectRatioDropdown.cs")]
public class NAspectRatioDropdown : NSettingsDropdown, IResettableSettingNode
{
  private AspectRatioSetting _currentAspectRatioSetting;

  private static string AspectRatioDropdownItemScenePath
  {
    get => SceneHelper.GetScenePath("ui/aspect_ratio_dropdown_item");
  }

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NAspectRatioDropdown.AspectRatioDropdownItemScenePath);
    }
  }

  public override void _Ready()
  {
    this.ConnectSignals();
    this.ClearDropdownItems();
    this.AddDropdownItem(AspectRatioSetting.Auto);
    this.AddDropdownItem(AspectRatioSetting.FourByThree);
    this.AddDropdownItem(AspectRatioSetting.SixteenByTen);
    this.AddDropdownItem(AspectRatioSetting.SixteenByNine);
    this.AddDropdownItem(AspectRatioSetting.TwentyOneByNine);
    ((Node) this._dropdownItems).GetParent<NDropdownContainer>().RefreshLayout();
    this.SetFromSettings();
  }

  public void SetFromSettings()
  {
    this._currentAspectRatioSetting = SaveManager.Instance.SettingsSave.AspectRatioSetting;
    this._currentOptionLabel.SetTextAutoSize(NAspectRatioDropdown.GetAspectRatioSettingString(this._currentAspectRatioSetting));
  }

  private void AddDropdownItem(AspectRatioSetting aspectRatioSetting)
  {
    NAspectRatioDropdownItem child = ResourceLoader.Load<PackedScene>(NAspectRatioDropdown.AspectRatioDropdownItemScenePath, (string) null, (ResourceLoader.CacheMode) 1L).Instantiate<NAspectRatioDropdownItem>((PackedScene.GenEditState) 0L);
    ((Node) this._dropdownItems).AddChildSafely((Node) child);
    ((GodotObject) child).Connect(NDropdownItem.SignalName.Selected, Callable.From<NDropdownItem>(new Action<NDropdownItem>(this.OnDropdownItemSelected)), 0U);
    child.Init(aspectRatioSetting);
  }

  private void OnDropdownItemSelected(NDropdownItem nDropdownItem)
  {
    NAspectRatioDropdownItem ratioDropdownItem = (NAspectRatioDropdownItem) nDropdownItem;
    if (ratioDropdownItem.aspectRatioSetting == this._currentAspectRatioSetting)
      return;
    this.CloseDropdown();
    SaveManager.Instance.SettingsSave.AspectRatioSetting = ratioDropdownItem.aspectRatioSetting;
    this.SetFromSettings();
    NGame.Instance.ApplyDisplaySettings();
  }

  private static string GetAspectRatioSettingString(AspectRatioSetting aspectRatioSettingString)
  {
    switch (aspectRatioSettingString)
    {
      case AspectRatioSetting.FourByThree:
        return new LocString("settings_ui", "ASPECT_RATIO_FOUR_BY_THREE").GetFormattedText();
      case AspectRatioSetting.SixteenByTen:
        return new LocString("settings_ui", "ASPECT_RATIO_SIXTEEN_BY_TEN").GetFormattedText();
      case AspectRatioSetting.SixteenByNine:
        return new LocString("settings_ui", "ASPECT_RATIO_SIXTEEN_BY_NINE").GetFormattedText();
      case AspectRatioSetting.TwentyOneByNine:
        return new LocString("settings_ui", "ASPECT_RATIO_TWENTY_ONE_BY_NINE").GetFormattedText();
      case AspectRatioSetting.Auto:
        return new LocString("settings_ui", "ASPECT_RATIO_AUTO").GetFormattedText();
      default:
        throw new ArgumentOutOfRangeException($"Invalid Aspect Ratio: {aspectRatioSettingString}");
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NAspectRatioDropdown.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAspectRatioDropdown.MethodName.SetFromSettings, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAspectRatioDropdown.MethodName.AddDropdownItem, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("aspectRatioSetting"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NAspectRatioDropdown.MethodName.OnDropdownItemSelected, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("nDropdownItem"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NAspectRatioDropdown.MethodName.GetAspectRatioSettingString, new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("aspectRatioSettingString"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NAspectRatioDropdown.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAspectRatioDropdown.MethodName.SetFromSettings) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SetFromSettings();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAspectRatioDropdown.MethodName.AddDropdownItem) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.AddDropdownItem(VariantUtils.ConvertTo<AspectRatioSetting>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NAspectRatioDropdown.MethodName.OnDropdownItemSelected) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnDropdownItemSelected(VariantUtils.ConvertTo<NDropdownItem>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NAspectRatioDropdown.MethodName.GetAspectRatioSettingString) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    string ratioSettingString = NAspectRatioDropdown.GetAspectRatioSettingString(VariantUtils.ConvertTo<AspectRatioSetting>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = VariantUtils.CreateFrom<string>(ref ratioSettingString);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NAspectRatioDropdown.MethodName.GetAspectRatioSettingString) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      string ratioSettingString = NAspectRatioDropdown.GetAspectRatioSettingString(VariantUtils.ConvertTo<AspectRatioSetting>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<string>(ref ratioSettingString);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NAspectRatioDropdown.MethodName._Ready) || StringName.op_Equality(ref method, NAspectRatioDropdown.MethodName.SetFromSettings) || StringName.op_Equality(ref method, NAspectRatioDropdown.MethodName.AddDropdownItem) || StringName.op_Equality(ref method, NAspectRatioDropdown.MethodName.OnDropdownItemSelected) || StringName.op_Equality(ref method, NAspectRatioDropdown.MethodName.GetAspectRatioSettingString) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NAspectRatioDropdown.PropertyName._currentAspectRatioSetting))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._currentAspectRatioSetting = VariantUtils.ConvertTo<AspectRatioSetting>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NAspectRatioDropdown.PropertyName._currentAspectRatioSetting))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<AspectRatioSetting>(ref this._currentAspectRatioSetting);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 2L, NAspectRatioDropdown.PropertyName._currentAspectRatioSetting, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NAspectRatioDropdown.PropertyName._currentAspectRatioSetting, Variant.From<AspectRatioSetting>(ref this._currentAspectRatioSetting));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant;
    if (!info.TryGetProperty(NAspectRatioDropdown.PropertyName._currentAspectRatioSetting, ref variant))
      return;
    this._currentAspectRatioSetting = ((Variant) ref variant).As<AspectRatioSetting>();
  }

  public new class MethodName : NSettingsDropdown.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName SetFromSettings = StringName.op_Implicit(nameof (SetFromSettings));
    public static readonly StringName AddDropdownItem = StringName.op_Implicit(nameof (AddDropdownItem));
    public static readonly StringName OnDropdownItemSelected = StringName.op_Implicit(nameof (OnDropdownItemSelected));
    public static readonly StringName GetAspectRatioSettingString = StringName.op_Implicit(nameof (GetAspectRatioSettingString));
  }

  public new class PropertyName : NSettingsDropdown.PropertyName
  {
    public static readonly StringName _currentAspectRatioSetting = StringName.op_Implicit(nameof (_currentAspectRatioSetting));
  }

  public new class SignalName : NSettingsDropdown.SignalName
  {
  }
}
