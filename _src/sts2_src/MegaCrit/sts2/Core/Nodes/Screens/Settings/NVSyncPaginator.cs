// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Settings.NVSyncPaginator
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings;

[ScriptPath("res://src/Core/Nodes/Screens/Settings/NVSyncPaginator.cs")]
public class NVSyncPaginator : NPaginator, IResettableSettingNode
{
  public override void _Ready()
  {
    this.ConnectSignals();
    this._options.Add(new LocString("settings_ui", "VSYNC_OFF").GetFormattedText());
    this._options.Add(new LocString("settings_ui", "VSYNC_ON").GetFormattedText());
    this._options.Add(new LocString("settings_ui", "VSYNC_ADAPTIVE").GetFormattedText());
    this.SetFromSettings();
  }

  public void SetFromSettings()
  {
    int num = this._options.IndexOf(NVSyncPaginator.GetVSyncString(SaveManager.Instance.SettingsSave.VSync));
    if (num != -1)
      this._currentIndex = num;
    else
      this._currentIndex = 2;
    this._label.SetTextAutoSize(this._options[this._currentIndex]);
  }

  private static string GetVSyncString(VSyncType vsyncType)
  {
    return new LocString("settings_ui", NVSyncPaginator.GetVSyncLabelKey(vsyncType)).GetFormattedText();
  }

  public static string GetVSyncLabelKey(VSyncType vsyncType)
  {
    switch (vsyncType)
    {
      case VSyncType.Off:
        return "VSYNC_OFF";
      case VSyncType.On:
        return "VSYNC_ON";
      case VSyncType.Adaptive:
        return "VSYNC_ADAPTIVE";
      default:
        Log.Error("Invalid VSync type: " + vsyncType.ToString());
        throw new ArgumentOutOfRangeException(nameof (vsyncType), (object) vsyncType, (string) null);
    }
  }

  protected override void OnIndexChanged(int index)
  {
    this._currentIndex = index;
    this._label.SetTextAutoSize(this._options[index]);
    switch (index)
    {
      case 0:
        SaveManager.Instance.SettingsSave.VSync = VSyncType.Off;
        break;
      case 1:
        SaveManager.Instance.SettingsSave.VSync = VSyncType.On;
        break;
      case 2:
        SaveManager.Instance.SettingsSave.VSync = VSyncType.Adaptive;
        break;
      default:
        Log.Error($"Invalid VSync index: {index}");
        break;
    }
    NGame.ApplySyncSetting();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NVSyncPaginator.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NVSyncPaginator.MethodName.SetFromSettings, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NVSyncPaginator.MethodName.GetVSyncString, new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("vsyncType"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NVSyncPaginator.MethodName.GetVSyncLabelKey, new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("vsyncType"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NVSyncPaginator.MethodName.OnIndexChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("index"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NVSyncPaginator.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NVSyncPaginator.MethodName.SetFromSettings) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SetFromSettings();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NVSyncPaginator.MethodName.GetVSyncString) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      string vsyncString = NVSyncPaginator.GetVSyncString(VariantUtils.ConvertTo<VSyncType>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<string>(ref vsyncString);
      return true;
    }
    if (StringName.op_Equality(ref method, NVSyncPaginator.MethodName.GetVSyncLabelKey) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      string vsyncLabelKey = NVSyncPaginator.GetVSyncLabelKey(VariantUtils.ConvertTo<VSyncType>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<string>(ref vsyncLabelKey);
      return true;
    }
    if (!StringName.op_Equality(ref method, NVSyncPaginator.MethodName.OnIndexChanged) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnIndexChanged(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NVSyncPaginator.MethodName.GetVSyncString) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      string vsyncString = NVSyncPaginator.GetVSyncString(VariantUtils.ConvertTo<VSyncType>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<string>(ref vsyncString);
      return true;
    }
    if (StringName.op_Equality(ref method, NVSyncPaginator.MethodName.GetVSyncLabelKey) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      string vsyncLabelKey = NVSyncPaginator.GetVSyncLabelKey(VariantUtils.ConvertTo<VSyncType>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<string>(ref vsyncLabelKey);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NVSyncPaginator.MethodName._Ready) || StringName.op_Equality(ref method, NVSyncPaginator.MethodName.SetFromSettings) || StringName.op_Equality(ref method, NVSyncPaginator.MethodName.GetVSyncString) || StringName.op_Equality(ref method, NVSyncPaginator.MethodName.GetVSyncLabelKey) || StringName.op_Equality(ref method, NVSyncPaginator.MethodName.OnIndexChanged) || base.HasGodotClassMethod(in method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    base.SaveGodotObjectData(info);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    base.RestoreGodotObjectData(info);
  }

  public new class MethodName : NPaginator.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName SetFromSettings = StringName.op_Implicit(nameof (SetFromSettings));
    public static readonly StringName GetVSyncString = StringName.op_Implicit(nameof (GetVSyncString));
    public static readonly StringName GetVSyncLabelKey = StringName.op_Implicit(nameof (GetVSyncLabelKey));
    public new static readonly StringName OnIndexChanged = StringName.op_Implicit(nameof (OnIndexChanged));
  }

  public new class PropertyName : NPaginator.PropertyName
  {
  }

  public new class SignalName : NPaginator.SignalName
  {
  }
}
