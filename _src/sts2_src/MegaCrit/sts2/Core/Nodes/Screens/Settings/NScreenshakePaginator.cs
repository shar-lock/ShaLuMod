// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Settings.NScreenshakePaginator
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.Saves;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings;

[ScriptPath("res://src/Core/Nodes/Screens/Settings/NScreenshakePaginator.cs")]
public class NScreenshakePaginator : NPaginator, IResettableSettingNode
{
  public override void _Ready()
  {
    this.ConnectSignals();
    this._options.Add(new LocString("settings_ui", "SCREENSHAKE_NONE").GetFormattedText());
    this._options.Add(new LocString("settings_ui", "SCREENSHAKE_SOME").GetFormattedText());
    this._options.Add(new LocString("settings_ui", "SCREENSHAKE_NORMAL").GetFormattedText());
    this._options.Add(new LocString("settings_ui", "SCREENSHAKE_LOTS").GetFormattedText());
    this._options.Add(new LocString("settings_ui", "SCREENSHAKE_CAAAW").GetFormattedText());
    this.SetFromSettings();
  }

  public void SetFromSettings()
  {
    this._currentIndex = SaveManager.Instance.PrefsSave.ScreenShakeOptionIndex;
    if (this._currentIndex >= this._options.Count)
      this._label.SetTextAutoSize(">:P");
    else
      this._label.SetTextAutoSize(this._options[this._currentIndex]);
    NGame.Instance.SetScreenshakeMultiplier(NScreenshakePaginator.GetShakeMultiplier(this._currentIndex));
  }

  protected override void OnIndexChanged(int index)
  {
    if (this._currentIndex >= this._options.Count)
      this._currentIndex = 2;
    this._currentIndex = index;
    this._label.SetTextAutoSize(this._options[index]);
    SaveManager.Instance.PrefsSave.ScreenShakeOptionIndex = this._currentIndex;
    Log.Info($"Screenshake set to: {this._currentIndex}");
    NGame.Instance.SetScreenshakeMultiplier(NScreenshakePaginator.GetShakeMultiplier(this._currentIndex));
    NGame.Instance.ScreenShakeTrauma(ShakeStrength.Medium);
  }

  public static float GetShakeMultiplier(int index)
  {
    float shakeMultiplier;
    switch (index)
    {
      case 0:
        shakeMultiplier = 0.0f;
        break;
      case 1:
        shakeMultiplier = 0.5f;
        break;
      case 2:
        shakeMultiplier = 1f;
        break;
      case 3:
        shakeMultiplier = 2f;
        break;
      case 4:
        shakeMultiplier = 4f;
        break;
      default:
        shakeMultiplier = (float) index;
        break;
    }
    return shakeMultiplier;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NScreenshakePaginator.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NScreenshakePaginator.MethodName.SetFromSettings, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NScreenshakePaginator.MethodName.OnIndexChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("index"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NScreenshakePaginator.MethodName.GetShakeMultiplier, new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, new List<PropertyInfo>()
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
    if (StringName.op_Equality(ref method, NScreenshakePaginator.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NScreenshakePaginator.MethodName.SetFromSettings) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SetFromSettings();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NScreenshakePaginator.MethodName.OnIndexChanged) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnIndexChanged(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NScreenshakePaginator.MethodName.GetShakeMultiplier) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    float shakeMultiplier = NScreenshakePaginator.GetShakeMultiplier(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = VariantUtils.CreateFrom<float>(ref shakeMultiplier);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NScreenshakePaginator.MethodName.GetShakeMultiplier) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      float shakeMultiplier = NScreenshakePaginator.GetShakeMultiplier(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<float>(ref shakeMultiplier);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NScreenshakePaginator.MethodName._Ready) || StringName.op_Equality(ref method, NScreenshakePaginator.MethodName.SetFromSettings) || StringName.op_Equality(ref method, NScreenshakePaginator.MethodName.OnIndexChanged) || StringName.op_Equality(ref method, NScreenshakePaginator.MethodName.GetShakeMultiplier) || base.HasGodotClassMethod(in method);
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
    public new static readonly StringName OnIndexChanged = StringName.op_Implicit(nameof (OnIndexChanged));
    public static readonly StringName GetShakeMultiplier = StringName.op_Implicit(nameof (GetShakeMultiplier));
  }

  public new class PropertyName : NPaginator.PropertyName
  {
  }

  public new class SignalName : NPaginator.SignalName
  {
  }
}
