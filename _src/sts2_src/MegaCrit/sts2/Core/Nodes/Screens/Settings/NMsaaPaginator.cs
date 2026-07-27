// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Settings.NMsaaPaginator
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Saves;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Settings;

[ScriptPath("res://src/Core/Nodes/Screens/Settings/NMsaaPaginator.cs")]
public class NMsaaPaginator : NPaginator, IResettableSettingNode
{
  public override void _Ready()
  {
    this.ConnectSignals();
    this._options.Add("0");
    this._options.Add("2");
    this._options.Add("4");
    this._options.Add("8");
    this.SetFromSettings();
  }

  public void SetFromSettings()
  {
    int num = this._options.IndexOf(SaveManager.Instance.SettingsSave.Msaa.ToString());
    this._currentIndex = num != -1 ? num : 3;
    this._label.SetTextAutoSize(this.GetMsaaLabel(int.Parse(this._options[this._currentIndex])));
  }

  protected override void OnIndexChanged(int index)
  {
    this._currentIndex = index;
    this._label.SetTextAutoSize(this.GetMsaaLabel(int.Parse(this._options[index])));
    SaveManager.Instance.SettingsSave.Msaa = int.Parse(this._options[index]);
    Log.Info("MSAA: " + this._label.Text);
    RenderingServer.ViewportSetMsaa2D(((Node) this).GetViewport().GetViewportRid(), this.GetMsaa(SaveManager.Instance.SettingsSave.Msaa));
  }

  private string GetMsaaLabel(int msaaAmount)
  {
    string msaaLabel;
    switch (msaaAmount)
    {
      case 2:
        msaaLabel = "2x";
        break;
      case 4:
        msaaLabel = "4x";
        break;
      case 8:
        msaaLabel = "8x";
        break;
      default:
        msaaLabel = new LocString("settings_ui", "MSAA_NONE").GetFormattedText();
        break;
    }
    return msaaLabel;
  }

  private RenderingServer.ViewportMsaa GetMsaa(int index)
  {
    RenderingServer.ViewportMsaa msaa;
    switch (index)
    {
      case 2:
        msaa = (RenderingServer.ViewportMsaa) 1L;
        break;
      case 4:
        msaa = (RenderingServer.ViewportMsaa) 2L;
        break;
      case 8:
        msaa = (RenderingServer.ViewportMsaa) 3L;
        break;
      default:
        msaa = (RenderingServer.ViewportMsaa) 0L;
        break;
    }
    return msaa;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal new static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(5)
    {
      new MethodInfo(NMsaaPaginator.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMsaaPaginator.MethodName.SetFromSettings, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMsaaPaginator.MethodName.OnIndexChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("index"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMsaaPaginator.MethodName.GetMsaaLabel, new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("msaaAmount"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NMsaaPaginator.MethodName.GetMsaa, new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
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
    if (StringName.op_Equality(ref method, NMsaaPaginator.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMsaaPaginator.MethodName.SetFromSettings) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.SetFromSettings();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMsaaPaginator.MethodName.OnIndexChanged) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnIndexChanged(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMsaaPaginator.MethodName.GetMsaaLabel) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      string msaaLabel = this.GetMsaaLabel(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<string>(ref msaaLabel);
      return true;
    }
    if (!StringName.op_Equality(ref method, NMsaaPaginator.MethodName.GetMsaa) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(in method, args, out ret);
    RenderingServer.ViewportMsaa msaa = this.GetMsaa(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = VariantUtils.CreateFrom<RenderingServer.ViewportMsaa>(ref msaa);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMsaaPaginator.MethodName._Ready) || StringName.op_Equality(ref method, NMsaaPaginator.MethodName.SetFromSettings) || StringName.op_Equality(ref method, NMsaaPaginator.MethodName.OnIndexChanged) || StringName.op_Equality(ref method, NMsaaPaginator.MethodName.GetMsaaLabel) || StringName.op_Equality(ref method, NMsaaPaginator.MethodName.GetMsaa) || base.HasGodotClassMethod(in method);
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
    public static readonly StringName GetMsaaLabel = StringName.op_Implicit(nameof (GetMsaaLabel));
    public static readonly StringName GetMsaa = StringName.op_Implicit(nameof (GetMsaa));
  }

  public new class PropertyName : NPaginator.PropertyName
  {
  }

  public new class SignalName : NPaginator.SignalName
  {
  }
}
