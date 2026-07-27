// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.RunHistoryScreen.NMapPointHistory
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Relics;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.RunHistoryScreen;

[ScriptPath("res://src/Core/Nodes/Screens/RunHistoryScreen/NMapPointHistory.cs")]
public class NMapPointHistory : Control
{
  private Control _actContainer;
  private readonly List<NActHistoryEntry> _actHistories = new List<NActHistoryEntry>();

  private List<NMapPointHistoryEntry> MapHistories
  {
    get
    {
      return this._actHistories.SelectMany<NActHistoryEntry, NMapPointHistoryEntry>((Func<NActHistoryEntry, IEnumerable<NMapPointHistoryEntry>>) (a => (IEnumerable<NMapPointHistoryEntry>) a.Entries)).ToList<NMapPointHistoryEntry>();
    }
  }

  public Control? DefaultFocusedControl
  {
    get => (Control) this.MapHistories.FirstOrDefault<NMapPointHistoryEntry>();
  }

  public override void _Ready()
  {
    this._actContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Acts"));
  }

  public void LoadHistory(RunHistory history)
  {
    foreach (Node child in ((Node) this._actContainer).GetChildren(false))
      child.QueueFreeSafely();
    this._actHistories.Clear();
    int baseFloorNum = 1;
    for (int index = 0; index < history.MapPointHistory.Count; ++index)
    {
      NActHistoryEntry child = NActHistoryEntry.Create(SaveUtil.ActOrDeprecated(history.Acts[index]).Title, history, (IReadOnlyList<MapPointHistoryEntry>) history.MapPointHistory[index], baseFloorNum);
      ((Node) this._actContainer).AddChildSafely((Node) child);
      this._actHistories.Add(child);
      baseFloorNum += history.MapPointHistory[index].Count;
    }
  }

  public void SetPlayer(RunHistoryPlayer player)
  {
    foreach (NActHistoryEntry actHistory in this._actHistories)
      actHistory.SetPlayer(player);
  }

  public void SetDeckHistory(NDeckHistory deckHistory)
  {
    ((GodotObject) deckHistory).Connect(NDeckHistory.SignalName.Hovered, Callable.From<NDeckHistoryEntry>(new Action<NDeckHistoryEntry>(this.HighlightRelevantEntries)), 0U);
    ((GodotObject) deckHistory).Connect(NDeckHistory.SignalName.Unhovered, Callable.From<NDeckHistoryEntry>(new Action<NDeckHistoryEntry>(this.UnHighlightEntries)), 0U);
  }

  public void SetRelicHistory(NRelicHistory relicHistory)
  {
    ((GodotObject) relicHistory).Connect(NRelicHistory.SignalName.Hovered, Callable.From<NRelicBasicHolder>(new Action<NRelicBasicHolder>(this.HighlightRelevantEntries)), 0U);
    ((GodotObject) relicHistory).Connect(NRelicHistory.SignalName.Unhovered, Callable.From<NRelicBasicHolder>(new Action<NRelicBasicHolder>(this.UnHighlightEntries)), 0U);
  }

  private void HighlightRelevantEntries(NDeckHistoryEntry historyEntry)
  {
    foreach (int num in historyEntry.FloorsAddedToDeck)
    {
      int floorNumber = num;
      this.MapHistories.FirstOrDefault<NMapPointHistoryEntry>((Func<NMapPointHistoryEntry, bool>) (e => e.FloorNum == floorNumber))?.Highlight();
    }
  }

  private void HighlightRelevantEntries(NRelicBasicHolder holder)
  {
    if (holder.Relic.Model.FloorAddedToDeck <= 0)
      return;
    this.MapHistories.FirstOrDefault<NMapPointHistoryEntry>((Func<NMapPointHistoryEntry, bool>) (e => e.FloorNum == holder.Relic.Model.FloorAddedToDeck))?.Highlight();
  }

  private void UnHighlightEntries(NRelicBasicHolder _) => this.UnHighlightEntries();

  private void UnHighlightEntries(NDeckHistoryEntry _) => this.UnHighlightEntries();

  private void UnHighlightEntries()
  {
    foreach (NMapPointHistoryEntry mapHistory in this.MapHistories)
      mapHistory.Unhighlight();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(6)
    {
      new MethodInfo(NMapPointHistory.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NMapPointHistory.MethodName.SetDeckHistory, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("deckHistory"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("VBoxContainer"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMapPointHistory.MethodName.SetRelicHistory, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("relicHistory"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("VBoxContainer"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMapPointHistory.MethodName.HighlightRelevantEntries, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("historyEntry"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMapPointHistory.MethodName.UnHighlightEntries, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NMapPointHistory.MethodName.UnHighlightEntries, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NMapPointHistory.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapPointHistory.MethodName.SetDeckHistory) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetDeckHistory(VariantUtils.ConvertTo<NDeckHistory>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapPointHistory.MethodName.SetRelicHistory) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetRelicHistory(VariantUtils.ConvertTo<NRelicHistory>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapPointHistory.MethodName.HighlightRelevantEntries) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.HighlightRelevantEntries(VariantUtils.ConvertTo<NDeckHistoryEntry>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NMapPointHistory.MethodName.UnHighlightEntries) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UnHighlightEntries(VariantUtils.ConvertTo<NRelicBasicHolder>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NMapPointHistory.MethodName.UnHighlightEntries) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.UnHighlightEntries();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMapPointHistory.MethodName._Ready) || StringName.op_Equality(ref method, NMapPointHistory.MethodName.SetDeckHistory) || StringName.op_Equality(ref method, NMapPointHistory.MethodName.SetRelicHistory) || StringName.op_Equality(ref method, NMapPointHistory.MethodName.HighlightRelevantEntries) || StringName.op_Equality(ref method, NMapPointHistory.MethodName.UnHighlightEntries) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NMapPointHistory.PropertyName._actContainer))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._actContainer = VariantUtils.ConvertTo<Control>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMapPointHistory.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (!StringName.op_Equality(ref name, NMapPointHistory.PropertyName._actContainer))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Control>(ref this._actContainer);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NMapPointHistory.PropertyName._actContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapPointHistory.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NMapPointHistory.PropertyName._actContainer, Variant.From<Control>(ref this._actContainer));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant;
    if (!info.TryGetProperty(NMapPointHistory.PropertyName._actContainer, ref variant))
      return;
    this._actContainer = ((Variant) ref variant).As<Control>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName SetDeckHistory = StringName.op_Implicit(nameof (SetDeckHistory));
    public static readonly StringName SetRelicHistory = StringName.op_Implicit(nameof (SetRelicHistory));
    public static readonly StringName HighlightRelevantEntries = StringName.op_Implicit(nameof (HighlightRelevantEntries));
    public static readonly StringName UnHighlightEntries = StringName.op_Implicit(nameof (UnHighlightEntries));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _actContainer = StringName.op_Implicit(nameof (_actContainer));
  }

  public class SignalName : Control.SignalName
  {
  }
}
