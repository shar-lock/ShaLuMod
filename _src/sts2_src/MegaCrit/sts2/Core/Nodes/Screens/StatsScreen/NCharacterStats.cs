// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.StatsScreen.NCharacterStats
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
namespace MegaCrit.Sts2.Core.Nodes.Screens.StatsScreen;

[ScriptPath("res://src/Core/Nodes/Screens/StatsScreen/NCharacterStats.cs")]
public class NCharacterStats : Node
{
  private static readonly string _playtimeIconPath = ImageHelper.GetImagePath("atlases/stats_screen_atlas.sprites/stats_clock.tres");
  private static readonly string _winLossIconPath = ImageHelper.GetImagePath("atlases/stats_screen_atlas.sprites/stats_swords.tres");
  private static readonly string _chainIconPath = ImageHelper.GetImagePath("atlases/stats_screen_atlas.sprites/stats_chain.tres");
  private CharacterStats _characterStats;
  private Control _characterIcon;
  private Node _statsContainer;
  private MegaLabel _nameLabel;
  private MegaLabel _unlocksLabel;
  private NStatEntry _playtimeEntry;
  private NStatEntry _winLossEntry;
  private NStatEntry _streakEntry;

  public static string[] AssetPaths
  {
    get
    {
      return new string[4]
      {
        NCharacterStats.ScenePath,
        NCharacterStats._playtimeIconPath,
        NCharacterStats._winLossIconPath,
        NCharacterStats._chainIconPath
      };
    }
  }

  private static string ScenePath
  {
    get => SceneHelper.GetScenePath("screens/stats_screen/character_stats");
  }

  public static NCharacterStats Create(CharacterStats characterStats)
  {
    NCharacterStats ncharacterStats = PreloadManager.Cache.GetScene(NCharacterStats.ScenePath).Instantiate<NCharacterStats>((PackedScene.GenEditState) 0L);
    ncharacterStats._characterStats = characterStats;
    return ncharacterStats;
  }

  public override void _Ready()
  {
    CharacterModel byId = ModelDb.GetById<CharacterModel>(this._characterStats.Id);
    this._characterIcon = this.GetNode<Control>(NodePath.op_Implicit("%CharacterIcon"));
    ((Node) this._characterIcon).AddChildSafely((Node) byId.Icon);
    this._statsContainer = this.GetNode<Node>(NodePath.op_Implicit("%StatsContainer"));
    this._playtimeEntry = this.CreateSection(NCharacterStats._playtimeIconPath);
    this._winLossEntry = this.CreateSection(NCharacterStats._winLossIconPath);
    this._streakEntry = this.CreateSection(NCharacterStats._chainIconPath);
    this._nameLabel = this.GetNode<MegaLabel>(NodePath.op_Implicit("%NameLabel"));
    this._unlocksLabel = this.GetNode<MegaLabel>(NodePath.op_Implicit("%UnlocksLabel"));
    this._nameLabel.SetTextAutoSize(byId.Title.GetRawText());
    ((Control) this._nameLabel).AddThemeColorOverride(ThemeConstants.Label.FontColor, byId.NameColor);
    this.LoadStats();
  }

  private void LoadStats()
  {
    ((CanvasItem) this._unlocksLabel).Visible = false;
    LocString locString1 = new LocString("stats_screen", "ENTRY_CHAR_PLAYTIME.top");
    locString1.Add("Playtime", TimeFormatting.Format((float) this._characterStats.Playtime));
    this._playtimeEntry.SetTopText(locString1.GetFormattedText());
    if (this._characterStats.FastestWinTime >= 0L)
    {
      LocString locString2 = new LocString("stats_screen", "ENTRY_CHAR_PLAYTIME.bottom");
      locString2.Add("FastestWin", TimeFormatting.Format((float) this._characterStats.FastestWinTime));
      this._playtimeEntry.SetBottomText(locString2.GetFormattedText());
    }
    LocString locString3 = new LocString("stats_screen", "ENTRY_CHAR_WIN_LOSS.top");
    if (this._characterStats.MaxAscension > 0)
    {
      locString3.Add("Amount", (Decimal) this._characterStats.MaxAscension);
      this._winLossEntry.SetTopText($"[red]{locString3.GetFormattedText()}[/red]");
    }
    LocString locString4 = new LocString("stats_screen", "ENTRY_CHAR_WIN_LOSS.bottom");
    locString4.Add("Wins", (Decimal) this._characterStats.TotalWins);
    locString4.Add("Losses", (Decimal) this._characterStats.TotalLosses);
    this._winLossEntry.SetBottomText(locString4.GetFormattedText());
    LocString locString5 = new LocString("stats_screen", "ENTRY_CHAR_STREAK.top");
    locString5.Add("Amount", (Decimal) this._characterStats.CurrentWinStreak);
    this._streakEntry.SetTopText(locString5.GetFormattedText());
    LocString locString6 = new LocString("stats_screen", "ENTRY_CHAR_STREAK.bottom");
    locString6.Add("Amount", (Decimal) this._characterStats.BestWinStreak);
    this._streakEntry.SetBottomText(locString6.GetFormattedText());
  }

  private NStatEntry CreateSection(string imgUrl)
  {
    NStatEntry child = NStatEntry.Create(imgUrl);
    this._statsContainer.AddChildSafely((Node) child);
    return child;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NCharacterStats.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCharacterStats.MethodName.LoadStats, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCharacterStats.MethodName.CreateSection, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("imgUrl"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCharacterStats.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      base._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCharacterStats.MethodName.LoadStats) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.LoadStats();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCharacterStats.MethodName.CreateSection) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    NStatEntry section = this.CreateSection(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = VariantUtils.CreateFrom<NStatEntry>(ref section);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCharacterStats.MethodName._Ready) || StringName.op_Equality(ref method, NCharacterStats.MethodName.LoadStats) || StringName.op_Equality(ref method, NCharacterStats.MethodName.CreateSection) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCharacterStats.PropertyName._characterIcon))
    {
      this._characterIcon = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterStats.PropertyName._statsContainer))
    {
      this._statsContainer = VariantUtils.ConvertTo<Node>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterStats.PropertyName._nameLabel))
    {
      this._nameLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterStats.PropertyName._unlocksLabel))
    {
      this._unlocksLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterStats.PropertyName._playtimeEntry))
    {
      this._playtimeEntry = VariantUtils.ConvertTo<NStatEntry>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterStats.PropertyName._winLossEntry))
    {
      this._winLossEntry = VariantUtils.ConvertTo<NStatEntry>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCharacterStats.PropertyName._streakEntry))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._streakEntry = VariantUtils.ConvertTo<NStatEntry>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCharacterStats.PropertyName._characterIcon))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._characterIcon);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterStats.PropertyName._statsContainer))
    {
      value = VariantUtils.CreateFrom<Node>(ref this._statsContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterStats.PropertyName._nameLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._nameLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterStats.PropertyName._unlocksLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._unlocksLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterStats.PropertyName._playtimeEntry))
    {
      value = VariantUtils.CreateFrom<NStatEntry>(ref this._playtimeEntry);
      return true;
    }
    if (StringName.op_Equality(ref name, NCharacterStats.PropertyName._winLossEntry))
    {
      value = VariantUtils.CreateFrom<NStatEntry>(ref this._winLossEntry);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCharacterStats.PropertyName._streakEntry))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<NStatEntry>(ref this._streakEntry);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCharacterStats.PropertyName._characterIcon, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCharacterStats.PropertyName._statsContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCharacterStats.PropertyName._nameLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCharacterStats.PropertyName._unlocksLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCharacterStats.PropertyName._playtimeEntry, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCharacterStats.PropertyName._winLossEntry, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCharacterStats.PropertyName._streakEntry, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NCharacterStats.PropertyName._characterIcon, Variant.From<Control>(ref this._characterIcon));
    info.AddProperty(NCharacterStats.PropertyName._statsContainer, Variant.From<Node>(ref this._statsContainer));
    info.AddProperty(NCharacterStats.PropertyName._nameLabel, Variant.From<MegaLabel>(ref this._nameLabel));
    info.AddProperty(NCharacterStats.PropertyName._unlocksLabel, Variant.From<MegaLabel>(ref this._unlocksLabel));
    info.AddProperty(NCharacterStats.PropertyName._playtimeEntry, Variant.From<NStatEntry>(ref this._playtimeEntry));
    info.AddProperty(NCharacterStats.PropertyName._winLossEntry, Variant.From<NStatEntry>(ref this._winLossEntry));
    info.AddProperty(NCharacterStats.PropertyName._streakEntry, Variant.From<NStatEntry>(ref this._streakEntry));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCharacterStats.PropertyName._characterIcon, ref variant1))
      this._characterIcon = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NCharacterStats.PropertyName._statsContainer, ref variant2))
      this._statsContainer = ((Variant) ref variant2).As<Node>();
    Variant variant3;
    if (info.TryGetProperty(NCharacterStats.PropertyName._nameLabel, ref variant3))
      this._nameLabel = ((Variant) ref variant3).As<MegaLabel>();
    Variant variant4;
    if (info.TryGetProperty(NCharacterStats.PropertyName._unlocksLabel, ref variant4))
      this._unlocksLabel = ((Variant) ref variant4).As<MegaLabel>();
    Variant variant5;
    if (info.TryGetProperty(NCharacterStats.PropertyName._playtimeEntry, ref variant5))
      this._playtimeEntry = ((Variant) ref variant5).As<NStatEntry>();
    Variant variant6;
    if (info.TryGetProperty(NCharacterStats.PropertyName._winLossEntry, ref variant6))
      this._winLossEntry = ((Variant) ref variant6).As<NStatEntry>();
    Variant variant7;
    if (!info.TryGetProperty(NCharacterStats.PropertyName._streakEntry, ref variant7))
      return;
    this._streakEntry = ((Variant) ref variant7).As<NStatEntry>();
  }

  public class MethodName : Node.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName LoadStats = StringName.op_Implicit(nameof (LoadStats));
    public static readonly StringName CreateSection = StringName.op_Implicit(nameof (CreateSection));
  }

  public class PropertyName : Node.PropertyName
  {
    public static readonly StringName _characterIcon = StringName.op_Implicit(nameof (_characterIcon));
    public static readonly StringName _statsContainer = StringName.op_Implicit(nameof (_statsContainer));
    public static readonly StringName _nameLabel = StringName.op_Implicit(nameof (_nameLabel));
    public static readonly StringName _unlocksLabel = StringName.op_Implicit(nameof (_unlocksLabel));
    public static readonly StringName _playtimeEntry = StringName.op_Implicit(nameof (_playtimeEntry));
    public static readonly StringName _winLossEntry = StringName.op_Implicit(nameof (_winLossEntry));
    public static readonly StringName _streakEntry = StringName.op_Implicit(nameof (_streakEntry));
  }

  public class SignalName : Node.SignalName
  {
  }
}
