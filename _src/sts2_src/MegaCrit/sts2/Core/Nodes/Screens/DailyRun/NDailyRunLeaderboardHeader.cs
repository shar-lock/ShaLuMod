// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.DailyRun.NDailyRunLeaderboardHeader
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
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.DailyRun;

[ScriptPath("res://src/Core/Nodes/Screens/DailyRun/NDailyRunLeaderboardHeader.cs")]
public class NDailyRunLeaderboardHeader : Control
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("screens/daily_run/daily_run_leaderboard_header");
  private MegaLabel _rank;
  private MegaLabel _name;
  private MegaLabel _score;

  public static NDailyRunLeaderboardHeader? Create()
  {
    return TestMode.IsOn ? (NDailyRunLeaderboardHeader) null : PreloadManager.Cache.GetScene(NDailyRunLeaderboardHeader._scenePath).Instantiate<NDailyRunLeaderboardHeader>((PackedScene.GenEditState) 0L);
  }

  public override void _Ready()
  {
    this._name = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%Name"));
    this._name.SetTextAutoSize(new LocString("main_menu_ui", "LEADERBOARDS.nameHeader").GetRawText());
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(2)
    {
      new MethodInfo(NDailyRunLeaderboardHeader.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDailyRunLeaderboardHeader.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NDailyRunLeaderboardHeader.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NDailyRunLeaderboardHeader leaderboardHeader = NDailyRunLeaderboardHeader.Create();
      ret = VariantUtils.CreateFrom<NDailyRunLeaderboardHeader>(ref leaderboardHeader);
      return true;
    }
    if (!StringName.op_Equality(ref method, NDailyRunLeaderboardHeader.MethodName._Ready) || ((NativeVariantPtrArgs) ref args).Count != 0)
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
    if (StringName.op_Equality(ref method, NDailyRunLeaderboardHeader.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NDailyRunLeaderboardHeader leaderboardHeader = NDailyRunLeaderboardHeader.Create();
      ret = VariantUtils.CreateFrom<NDailyRunLeaderboardHeader>(ref leaderboardHeader);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NDailyRunLeaderboardHeader.MethodName.Create) || StringName.op_Equality(ref method, NDailyRunLeaderboardHeader.MethodName._Ready) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDailyRunLeaderboardHeader.PropertyName._rank))
    {
      this._rank = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunLeaderboardHeader.PropertyName._name))
    {
      this._name = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDailyRunLeaderboardHeader.PropertyName._score))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._score = VariantUtils.ConvertTo<MegaLabel>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDailyRunLeaderboardHeader.PropertyName._rank))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._rank);
      return true;
    }
    if (StringName.op_Equality(ref name, NDailyRunLeaderboardHeader.PropertyName._name))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._name);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDailyRunLeaderboardHeader.PropertyName._score))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<MegaLabel>(ref this._score);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NDailyRunLeaderboardHeader.PropertyName._rank, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDailyRunLeaderboardHeader.PropertyName._name, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDailyRunLeaderboardHeader.PropertyName._score, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NDailyRunLeaderboardHeader.PropertyName._rank, Variant.From<MegaLabel>(ref this._rank));
    info.AddProperty(NDailyRunLeaderboardHeader.PropertyName._name, Variant.From<MegaLabel>(ref this._name));
    info.AddProperty(NDailyRunLeaderboardHeader.PropertyName._score, Variant.From<MegaLabel>(ref this._score));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NDailyRunLeaderboardHeader.PropertyName._rank, ref variant1))
      this._rank = ((Variant) ref variant1).As<MegaLabel>();
    Variant variant2;
    if (info.TryGetProperty(NDailyRunLeaderboardHeader.PropertyName._name, ref variant2))
      this._name = ((Variant) ref variant2).As<MegaLabel>();
    Variant variant3;
    if (!info.TryGetProperty(NDailyRunLeaderboardHeader.PropertyName._score, ref variant3))
      return;
    this._score = ((Variant) ref variant3).As<MegaLabel>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _rank = StringName.op_Implicit(nameof (_rank));
    public static readonly StringName _name = StringName.op_Implicit(nameof (_name));
    public static readonly StringName _score = StringName.op_Implicit(nameof (_score));
  }

  public class SignalName : Control.SignalName
  {
  }
}
