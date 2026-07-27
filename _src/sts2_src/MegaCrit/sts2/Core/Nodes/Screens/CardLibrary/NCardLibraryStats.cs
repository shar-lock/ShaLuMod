// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary.NCardLibraryStats
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
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Saves;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.CardLibrary;

[ScriptPath("res://src/Core/Nodes/Screens/CardLibrary/NCardLibraryStats.cs")]
public class NCardLibraryStats : Control
{
  private MegaRichTextLabel _label;

  private static LocString Victories => new LocString("card_library", "VICTORIES");

  private static string ScenePath
  {
    get => SceneHelper.GetScenePath("screens/card_library/card_library_stats");
  }

  public static NCardLibraryStats Create()
  {
    return PreloadManager.Cache.GetScene(NCardLibraryStats.ScenePath).Instantiate<NCardLibraryStats>((PackedScene.GenEditState) 0L);
  }

  public override void _Ready()
  {
    this._label = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%Label"));
  }

  public void UpdateStats(CardModel card)
  {
    LocString victories = NCardLibraryStats.Victories;
    CardStats cardStats;
    long timesWon = !SaveManager.Instance.Progress.CardStats.TryGetValue(card.Id, out cardStats) ? 0L : cardStats.TimesWon;
    victories.Add("Victories", (Decimal) timesWon);
    this._label.Text = victories.GetFormattedText();
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NCardLibraryStats.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardLibraryStats.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCardLibraryStats.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NCardLibraryStats ncardLibraryStats = NCardLibraryStats.Create();
      ret = VariantUtils.CreateFrom<NCardLibraryStats>(ref ncardLibraryStats);
      return true;
    }
    if (!StringName.op_Equality(ref method, NCardLibraryStats.MethodName._Ready) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._Ready();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCardLibraryStats.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NCardLibraryStats ncardLibraryStats = NCardLibraryStats.Create();
      ret = VariantUtils.CreateFrom<NCardLibraryStats>(ref ncardLibraryStats);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCardLibraryStats.MethodName.Create) || StringName.op_Equality(ref method, NCardLibraryStats.MethodName._Ready) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NCardLibraryStats.PropertyName._label))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._label = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (!StringName.op_Equality(ref name, NCardLibraryStats.PropertyName._label))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._label);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCardLibraryStats.PropertyName._label, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NCardLibraryStats.PropertyName._label, Variant.From<MegaRichTextLabel>(ref this._label));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant;
    if (!info.TryGetProperty(NCardLibraryStats.PropertyName._label, ref variant))
      return;
    this._label = ((Variant) ref variant).As<MegaRichTextLabel>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _label = StringName.op_Implicit(nameof (_label));
  }

  public class SignalName : Control.SignalName
  {
  }
}
