// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Settings.NBackgroundModeTickbox
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Saves;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings;

[ScriptPath("res://src/Core/Nodes/Screens/Settings/NBackgroundModeTickbox.cs")]
public class NBackgroundModeTickbox : NSettingsTickbox, IResettableSettingNode
{
  private NSettingsScreen _settingsScreen;

  public override void _Ready()
  {
    this.ConnectSignals();
    this._settingsScreen = ((Node) this).GetAncestorOfType<NSettingsScreen>();
    this.SetFromSettings();
  }

  public void SetFromSettings()
  {
    this.IsTicked = SaveManager.Instance.SettingsSave.LimitFpsInBackground;
  }

  protected override void OnTick()
  {
    this._settingsScreen.ShowToast(new LocString("settings_ui", "TOAST_LIMIT_FPS_IN_BACKGROUND_ON"));
    SaveManager.Instance.SettingsSave.LimitFpsInBackground = true;
  }

  protected override void OnUntick()
  {
    this._settingsScreen.ShowToast(new LocString("settings_ui", "TOAST_LIMIT_FPS_IN_BACKGROUND_OFF"));
    SaveManager.Instance.SettingsSave.LimitFpsInBackground = false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NBackgroundModeTickbox.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBackgroundModeTickbox.MethodName.SetFromSettings, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBackgroundModeTickbox.MethodName.OnTick, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NBackgroundModeTickbox.MethodName.OnUntick, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NBackgroundModeTickbox.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBackgroundModeTickbox.MethodName.SetFromSettings) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SetFromSettings();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NBackgroundModeTickbox.MethodName.OnTick) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnTick();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NBackgroundModeTickbox.MethodName.OnUntick) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnUntick();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NBackgroundModeTickbox.MethodName._Ready) || StringName.op_Equality(ref method, NBackgroundModeTickbox.MethodName.SetFromSettings) || StringName.op_Equality(ref method, NBackgroundModeTickbox.MethodName.OnTick) || StringName.op_Equality(ref method, NBackgroundModeTickbox.MethodName.OnUntick) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NBackgroundModeTickbox.PropertyName._settingsScreen))
      return base.SetGodotClassPropertyValue(in name, in value);
    this._settingsScreen = VariantUtils.ConvertTo<NSettingsScreen>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NBackgroundModeTickbox.PropertyName._settingsScreen))
      return base.GetGodotClassPropertyValue(in name, out value);
    value = VariantUtils.CreateFrom<NSettingsScreen>(ref this._settingsScreen);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NBackgroundModeTickbox.PropertyName._settingsScreen, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
    info.AddProperty(NBackgroundModeTickbox.PropertyName._settingsScreen, Variant.From<NSettingsScreen>(ref this._settingsScreen));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
    Variant variant;
    if (!info.TryGetProperty(NBackgroundModeTickbox.PropertyName._settingsScreen, ref variant))
      return;
    this._settingsScreen = ((Variant) ref variant).As<NSettingsScreen>();
  }

  public new class MethodName : NSettingsTickbox.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName SetFromSettings = StringName.op_Implicit(nameof (SetFromSettings));
    public new static readonly StringName OnTick = StringName.op_Implicit(nameof (OnTick));
    public new static readonly StringName OnUntick = StringName.op_Implicit(nameof (OnUntick));
  }

  public new class PropertyName : NSettingsTickbox.PropertyName
  {
    public new static readonly StringName _settingsScreen = StringName.op_Implicit(nameof (_settingsScreen));
  }

  public new class SignalName : NSettingsTickbox.SignalName
  {
  }
}
