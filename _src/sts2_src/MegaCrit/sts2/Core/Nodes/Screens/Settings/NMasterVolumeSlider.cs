// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Settings.NMasterVolumeSlider
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Saves;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings;

[ScriptPath("res://src/Core/Nodes/Screens/Settings/NMasterVolumeSlider.cs")]
public class NMasterVolumeSlider : NSettingsSlider
{
  public override void _Ready()
  {
    this.ConnectSignals();
    this._slider.SetValueWithoutAnimation((double) SaveManager.Instance.SettingsSave.VolumeMaster * 100.0);
    ((GodotObject) this._slider).Connect(Range.SignalName.ValueChanged, Callable.From<double>(new Action<double>(this.OnValueChanged)), 0U);
  }

  private void OnValueChanged(double value)
  {
    float num = (float) value * 0.01f;
    SaveManager.Instance.SettingsSave.VolumeMaster = num;
    NAudioManager.Instance?.SetMasterVol(num);
    NDebugAudioManager.Instance?.SetMasterAudioVolume(num);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NMasterVolumeSlider.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMasterVolumeSlider.MethodName.OnValueChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("value"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMasterVolumeSlider.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NMasterVolumeSlider.MethodName.OnValueChanged) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnValueChanged(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMasterVolumeSlider.MethodName._Ready) || StringName.op_Equality(ref method, NMasterVolumeSlider.MethodName.OnValueChanged) || base.HasGodotClassMethod(in method);
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

  public new class MethodName : NSettingsSlider.MethodName
  {
    public new static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public new static readonly StringName OnValueChanged = StringName.op_Implicit(nameof (OnValueChanged));
  }

  public new class PropertyName : NSettingsSlider.PropertyName
  {
  }

  public new class SignalName : NSettingsSlider.SignalName
  {
  }
}
