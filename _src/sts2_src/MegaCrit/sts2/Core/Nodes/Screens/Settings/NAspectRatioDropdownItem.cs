// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Settings.NAspectRatioDropdownItem
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings;

[ScriptPath("res://src/Core/Nodes/Screens/Settings/NAspectRatioDropdownItem.cs")]
public class NAspectRatioDropdownItem : NDropdownItem
{
  public AspectRatioSetting aspectRatioSetting;

  public void Init(AspectRatioSetting setAspectRatioSetting)
  {
    this.aspectRatioSetting = setAspectRatioSetting;
    switch (this.aspectRatioSetting)
    {
      case AspectRatioSetting.FourByThree:
        this._label.SetTextAutoSize(new LocString("settings_ui", "ASPECT_RATIO_FOUR_BY_THREE").GetFormattedText());
        break;
      case AspectRatioSetting.SixteenByTen:
        this._label.SetTextAutoSize(new LocString("settings_ui", "ASPECT_RATIO_SIXTEEN_BY_TEN").GetFormattedText());
        break;
      case AspectRatioSetting.SixteenByNine:
        this._label.SetTextAutoSize(new LocString("settings_ui", "ASPECT_RATIO_SIXTEEN_BY_NINE").GetFormattedText());
        break;
      case AspectRatioSetting.TwentyOneByNine:
        this._label.SetTextAutoSize(new LocString("settings_ui", "ASPECT_RATIO_TWENTY_ONE_BY_NINE").GetFormattedText());
        break;
      case AspectRatioSetting.Auto:
        this._label.SetTextAutoSize(new LocString("settings_ui", "ASPECT_RATIO_AUTO").GetFormattedText());
        break;
      default:
        throw new ArgumentOutOfRangeException($"Invalid Aspect Ratio: {this.aspectRatioSetting}");
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NAspectRatioDropdownItem.MethodName.Init, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("setAspectRatioSetting"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (!StringName.op_Equality(ref method, NAspectRatioDropdownItem.MethodName.Init) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.Init(VariantUtils.ConvertTo<AspectRatioSetting>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NAspectRatioDropdownItem.MethodName.Init) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NAspectRatioDropdownItem.PropertyName.aspectRatioSetting))
      return base.SetGodotClassPropertyValue(in name, in value);
    this.aspectRatioSetting = VariantUtils.ConvertTo<AspectRatioSetting>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NAspectRatioDropdownItem.PropertyName.aspectRatioSetting))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<AspectRatioSetting>(ref this.aspectRatioSetting);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 2L, NAspectRatioDropdownItem.PropertyName.aspectRatioSetting, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NAspectRatioDropdownItem.PropertyName.aspectRatioSetting, Variant.From<AspectRatioSetting>(ref this.aspectRatioSetting));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant;
    if (!info.TryGetProperty(NAspectRatioDropdownItem.PropertyName.aspectRatioSetting, ref variant))
      return;
    this.aspectRatioSetting = ((Variant) ref variant).As<AspectRatioSetting>();
  }

  public new class MethodName : NDropdownItem.MethodName
  {
    public static readonly StringName Init = StringName.op_Implicit(nameof (Init));
  }

  public new class PropertyName : NDropdownItem.PropertyName
  {
    public static readonly StringName aspectRatioSetting = StringName.op_Implicit(nameof (aspectRatioSetting));
  }

  public new class SignalName : NDropdownItem.SignalName
  {
  }
}
