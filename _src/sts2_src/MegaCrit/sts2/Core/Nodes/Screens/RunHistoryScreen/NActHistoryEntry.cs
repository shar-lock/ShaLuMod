// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.RunHistoryScreen.NActHistoryEntry
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.RunHistoryScreen;

[ScriptPath("res://src/Core/Nodes/Screens/RunHistoryScreen/NActHistoryEntry.cs")]
public class NActHistoryEntry : HBoxContainer
{
  private MegaLabel _actLabel;
  private LocString _actName;
  private RunHistory _runHistory;
  private IReadOnlyList<MapPointHistoryEntry> _entries;
  private int _baseFloorNum;

  private static string ScenePath
  {
    get => SceneHelper.GetScenePath("screens/run_history_screen/act_history_entry");
  }

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NActHistoryEntry.ScenePath);
    }
  }

  public List<NMapPointHistoryEntry> Entries { get; private set; } = new List<NMapPointHistoryEntry>();

  public override void _Ready()
  {
    this._actLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%Title"));
    this._actLabel.SetTextAutoSize(this._actName.GetFormattedText());
    for (int index = 0; index < this._entries.Count; ++index)
    {
      NMapPointHistoryEntry child = NMapPointHistoryEntry.Create(this._runHistory, this._entries[index], index + this._baseFloorNum);
      ((Node) this).AddChildSafely((Node) child);
      this.Entries.Add(child);
    }
  }

  public void SetPlayer(RunHistoryPlayer player)
  {
    foreach (NMapPointHistoryEntry entry in this.Entries)
      entry.SetPlayer(player);
  }

  public static NActHistoryEntry? Create(
    LocString actName,
    RunHistory runHistory,
    IReadOnlyList<MapPointHistoryEntry> logs,
    int baseFloorNum)
  {
    if (TestMode.IsOn)
      return (NActHistoryEntry) null;
    NActHistoryEntry nactHistoryEntry = PreloadManager.Cache.GetScene(NActHistoryEntry.ScenePath).Instantiate<NActHistoryEntry>((PackedScene.GenEditState) 0L);
    nactHistoryEntry._actName = actName;
    nactHistoryEntry._runHistory = runHistory;
    nactHistoryEntry._entries = logs;
    nactHistoryEntry._baseFloorNum = baseFloorNum;
    return nactHistoryEntry;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NActHistoryEntry.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (!StringName.op_Equality(ref method, NActHistoryEntry.MethodName._Ready) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._Ready();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NActHistoryEntry.MethodName._Ready) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NActHistoryEntry.PropertyName._actLabel))
    {
      this._actLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NActHistoryEntry.PropertyName._baseFloorNum))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._baseFloorNum = VariantUtils.ConvertTo<int>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NActHistoryEntry.PropertyName._actLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._actLabel);
      return true;
    }
    if (!StringName.op_Equality(ref name, NActHistoryEntry.PropertyName._baseFloorNum))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<int>(ref this._baseFloorNum);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NActHistoryEntry.PropertyName._actLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NActHistoryEntry.PropertyName._baseFloorNum, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NActHistoryEntry.PropertyName._actLabel, Variant.From<MegaLabel>(ref this._actLabel));
    info.AddProperty(NActHistoryEntry.PropertyName._baseFloorNum, Variant.From<int>(ref this._baseFloorNum));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NActHistoryEntry.PropertyName._actLabel, ref variant1))
      this._actLabel = ((Variant) ref variant1).As<MegaLabel>();
    Variant variant2;
    if (!info.TryGetProperty(NActHistoryEntry.PropertyName._baseFloorNum, ref variant2))
      return;
    this._baseFloorNum = ((Variant) ref variant2).As<int>();
  }

  public class MethodName : HBoxContainer.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
  }

  public class PropertyName : HBoxContainer.PropertyName
  {
    public static readonly StringName _actLabel = StringName.op_Implicit(nameof (_actLabel));
    public static readonly StringName _baseFloorNum = StringName.op_Implicit(nameof (_baseFloorNum));
  }

  public class SignalName : HBoxContainer.SignalName
  {
  }
}
