// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Settings.NFpsPaginator
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Saves;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings;

[ScriptPath("res://src/Core/Nodes/Screens/Settings/NFpsPaginator.cs")]
public class NFpsPaginator : NPaginator, IResettableSettingNode
{
  public override void _Ready()
  {
    this.ConnectSignals();
    this._options.Add("24");
    this._options.Add("30");
    this._options.Add("59");
    this._options.Add("60");
    this._options.Add("75");
    this._options.Add("90");
    this._options.Add("120");
    this._options.Add("144");
    this._options.Add("165");
    this._options.Add("240");
    this._options.Add("360");
    this._options.Add("500");
    this.SetFromSettings();
  }

  public void SetFromSettings()
  {
    int num = this._options.IndexOf(SaveManager.Instance.SettingsSave.FpsLimit.ToString());
    this._currentIndex = num != -1 ? num : 3;
    this._label.SetTextAutoSize(this._options[this._currentIndex]);
  }

  protected override void OnIndexChanged(int index)
  {
    this._currentIndex = index;
    this._label.SetTextAutoSize(this._options[index]);
    SaveManager.Instance.SettingsSave.FpsLimit = int.Parse(this._options[index]);
    Log.Info($"FPS Limit: {SaveManager.Instance.SettingsSave.FpsLimit}");
    Engine.MaxFps = SaveManager.Instance.SettingsSave.FpsLimit;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NFpsPaginator.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NFpsPaginator.MethodName.SetFromSettings, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NFpsPaginator.MethodName.OnIndexChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
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
    if (StringName.op_Equality(ref method, NFpsPaginator.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NFpsPaginator.MethodName.SetFromSettings) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SetFromSettings();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NFpsPaginator.MethodName.OnIndexChanged) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    this.OnIndexChanged(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NFpsPaginator.MethodName._Ready) || StringName.op_Equality(ref method, NFpsPaginator.MethodName.SetFromSettings) || StringName.op_Equality(ref method, NFpsPaginator.MethodName.OnIndexChanged) || base.HasGodotClassMethod(in method);
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
  }

  public new class PropertyName : NPaginator.PropertyName
  {
  }

  public new class SignalName : NPaginator.SignalName
  {
  }
}
