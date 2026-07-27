// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Settings.NAmbienceSlider
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

[ScriptPath("res://src/Core/Nodes/Screens/Settings/NAmbienceSlider.cs")]
public class NAmbienceSlider : NSettingsSlider
{
  public override void _Ready()
  {
    this.ConnectSignals();
    this._slider.SetValueWithoutAnimation((double) SaveManager.Instance.SettingsSave.VolumeAmbience * 100.0);
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    ((GodotObject) this._slider).Connect(Range.SignalName.ValueChanged, Callable.From<double>(NAmbienceSlider.\u003C\u003EO.\u003C0\u003E__OnValueChanged ?? (NAmbienceSlider.\u003C\u003EO.\u003C0\u003E__OnValueChanged = new Action<double>(NAmbienceSlider.OnValueChanged))), 0U);
  }

  private static void OnValueChanged(double value)
  {
    float num = (float) value / 100f;
    SaveManager.Instance.SettingsSave.VolumeAmbience = num;
    NAudioManager.Instance?.SetAmbienceVol(num);
    NDebugAudioManager.Instance?.SetSfxAudioVolume(num);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NAmbienceSlider.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NAmbienceSlider.MethodName.OnValueChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, new List<PropertyInfo>()
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
    if (StringName.op_Equality(ref method, NAmbienceSlider.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NAmbienceSlider.MethodName.OnValueChanged) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    NAmbienceSlider.OnValueChanged(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NAmbienceSlider.MethodName.OnValueChanged) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NAmbienceSlider.OnValueChanged(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NAmbienceSlider.MethodName._Ready) || StringName.op_Equality(ref method, NAmbienceSlider.MethodName.OnValueChanged) || base.HasGodotClassMethod(in method);
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
