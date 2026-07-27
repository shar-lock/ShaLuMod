// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.DailyRun.NDailyRunLeaderboardRow
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Leaderboard;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.DailyRun;

[ScriptPath("res://src/Core/Nodes/Screens/DailyRun/NDailyRunLeaderboardRow.cs")]
public class NDailyRunLeaderboardRow : Control
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("screens/daily_run/daily_run_leaderboard_row");
  private MegaLabel _rank;
  private MegaLabel _name;
  private MegaLabel _floor;
  private MegaLabel _badges;
  private MegaLabel _time;
  private LeaderboardEntry _entry;
  private bool _isYou;

  public static NDailyRunLeaderboardRow? Create(LeaderboardEntry entry, bool isYou)
  {
    if (TestMode.IsOn)
      return (NDailyRunLeaderboardRow) null;
    NDailyRunLeaderboardRow runLeaderboardRow = PreloadManager.Cache.GetScene(NDailyRunLeaderboardRow._scenePath).Instantiate<NDailyRunLeaderboardRow>((PackedScene.GenEditState) 0L);
    runLeaderboardRow._entry = entry;
    runLeaderboardRow._isYou = isYou;
    return runLeaderboardRow;
  }

  public override void _Ready()
  {
    this._rank = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%Rank"));
    this._floor = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%Floor"));
    this._name = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%Name"));
    this._badges = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%Badges"));
    this._time = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%Time"));
    IEnumerable<string> values = this._entry.userIds.Select<ulong, string>((Func<ulong, string>) (id => PlatformUtil.GetPlayerNameRaw(LeaderboardManager.CurrentPlatform, id)));
    DecodedDailyScore decodedDailyScore = ScoreUtility.DecodeDailyScore(this._entry.score);
    if (!decodedDailyScore.isValid)
    {
      ((Node) this).QueueFreeSafely();
    }
    else
    {
      this._rank.SetTextAutoSize($"{this._entry.rank + 1} ");
      this._name.SetTextAutoSize(string.Join(",", values));
      if (this._isYou)
        ((CanvasItem) this._name).Modulate = StsColors.blue;
      this._floor.SetTextAutoSize($"{decodedDailyScore.floors}");
      if (decodedDailyScore.victory == 2)
        ((CanvasItem) ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Tick"))).Visible = true;
      this._badges.SetTextAutoSize($"{decodedDailyScore.badges}");
      this._time.SetTextAutoSize(NDailyRunLeaderboardRow.FormatHoursAndMinutes(decodedDailyScore.runTime));
    }
  }

  private static string FormatHoursAndMinutes(int value)
  {
    if (value >= 9999)
      return "--:--";
    return $"{value / 60}:{value % 60:D2}";
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NDailyRunLeaderboardRow.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDailyRunLeaderboardRow.MethodName.FormatHoursAndMinutes, new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("value"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NDailyRunLeaderboardRow.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NDailyRunLeaderboardRow.MethodName.FormatHoursAndMinutes) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    string str = NDailyRunLeaderboardRow.FormatHoursAndMinutes(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = VariantUtils.CreateFrom<string>(ref str);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NDailyRunLeaderboardRow.MethodName.FormatHoursAndMinutes) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      string str = NDailyRunLeaderboardRow.FormatHoursAndMinutes(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<string>(ref str);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NDailyRunLeaderboardRow.MethodName._Ready) || StringName.op_Equality(ref method, NDailyRunLeaderboardRow.MethodName.FormatHoursAndMinutes) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDailyRunLeaderboardRow.PropertyName._rank))
    {
      this._rank = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunLeaderboardRow.PropertyName._name))
    {
      this._name = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunLeaderboardRow.PropertyName._floor))
    {
      this._floor = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunLeaderboardRow.PropertyName._badges))
    {
      this._badges = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunLeaderboardRow.PropertyName._time))
    {
      this._time = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDailyRunLeaderboardRow.PropertyName._isYou))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._isYou = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDailyRunLeaderboardRow.PropertyName._rank))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._rank);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunLeaderboardRow.PropertyName._name))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._name);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunLeaderboardRow.PropertyName._floor))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._floor);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunLeaderboardRow.PropertyName._badges))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._badges);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunLeaderboardRow.PropertyName._time))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._time);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDailyRunLeaderboardRow.PropertyName._isYou))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<bool>(ref this._isYou);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NDailyRunLeaderboardRow.PropertyName._rank, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDailyRunLeaderboardRow.PropertyName._name, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDailyRunLeaderboardRow.PropertyName._floor, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDailyRunLeaderboardRow.PropertyName._badges, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDailyRunLeaderboardRow.PropertyName._time, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NDailyRunLeaderboardRow.PropertyName._isYou, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NDailyRunLeaderboardRow.PropertyName._rank, Variant.From<MegaLabel>(ref this._rank));
    info.AddProperty(NDailyRunLeaderboardRow.PropertyName._name, Variant.From<MegaLabel>(ref this._name));
    info.AddProperty(NDailyRunLeaderboardRow.PropertyName._floor, Variant.From<MegaLabel>(ref this._floor));
    info.AddProperty(NDailyRunLeaderboardRow.PropertyName._badges, Variant.From<MegaLabel>(ref this._badges));
    info.AddProperty(NDailyRunLeaderboardRow.PropertyName._time, Variant.From<MegaLabel>(ref this._time));
    info.AddProperty(NDailyRunLeaderboardRow.PropertyName._isYou, Variant.From<bool>(ref this._isYou));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NDailyRunLeaderboardRow.PropertyName._rank, ref variant1))
      this._rank = ((Variant) ref variant1).As<MegaLabel>();
    Variant variant2;
    if (info.TryGetProperty(NDailyRunLeaderboardRow.PropertyName._name, ref variant2))
      this._name = ((Variant) ref variant2).As<MegaLabel>();
    Variant variant3;
    if (info.TryGetProperty(NDailyRunLeaderboardRow.PropertyName._floor, ref variant3))
      this._floor = ((Variant) ref variant3).As<MegaLabel>();
    Variant variant4;
    if (info.TryGetProperty(NDailyRunLeaderboardRow.PropertyName._badges, ref variant4))
      this._badges = ((Variant) ref variant4).As<MegaLabel>();
    Variant variant5;
    if (info.TryGetProperty(NDailyRunLeaderboardRow.PropertyName._time, ref variant5))
      this._time = ((Variant) ref variant5).As<MegaLabel>();
    Variant variant6;
    if (!info.TryGetProperty(NDailyRunLeaderboardRow.PropertyName._isYou, ref variant6))
      return;
    this._isYou = ((Variant) ref variant6).As<bool>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName FormatHoursAndMinutes = StringName.op_Implicit(nameof (FormatHoursAndMinutes));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _rank = StringName.op_Implicit(nameof (_rank));
    public static readonly StringName _name = StringName.op_Implicit(nameof (_name));
    public static readonly StringName _floor = StringName.op_Implicit(nameof (_floor));
    public static readonly StringName _badges = StringName.op_Implicit(nameof (_badges));
    public static readonly StringName _time = StringName.op_Implicit(nameof (_time));
    public static readonly StringName _isYou = StringName.op_Implicit(nameof (_isYou));
  }

  public class SignalName : Control.SignalName
  {
  }
}
