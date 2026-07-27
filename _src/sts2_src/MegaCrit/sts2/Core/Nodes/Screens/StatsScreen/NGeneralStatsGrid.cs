// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.StatsScreen.NGeneralStatsGrid
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Timeline;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.StatsScreen;

[ScriptPath("res://src/Core/Nodes/Screens/StatsScreen/NGeneralStatsGrid.cs")]
public class NGeneralStatsGrid : Control
{
  private static readonly string _achievementsIconPath = ImageHelper.GetImagePath("atlases/stats_screen_atlas.sprites/stats_achievements.tres");
  private static readonly string _playtimeIconPath = ImageHelper.GetImagePath("atlases/stats_screen_atlas.sprites/stats_clock.tres");
  private static readonly string _cardsIconPath = ImageHelper.GetImagePath("atlases/stats_screen_atlas.sprites/stats_cards.tres");
  private static readonly string _winLossIconPath = ImageHelper.GetImagePath("atlases/stats_screen_atlas.sprites/stats_swords.tres");
  private static readonly string _monsterIconPath = ImageHelper.GetImagePath("atlases/stats_screen_atlas.sprites/stats_monsters.tres");
  private static readonly string _ancientsIconPath = ImageHelper.GetImagePath("atlases/stats_screen_atlas.sprites/stats_ancients.tres");
  private static readonly string _relicIconPath = ImageHelper.GetImagePath("atlases/stats_screen_atlas.sprites/stats_chest.tres");
  private static readonly string _potionIconPath = ImageHelper.GetImagePath("atlases/stats_screen_atlas.sprites/stats_potions_seen.tres");
  private static readonly string _eventsIconPath = ImageHelper.GetImagePath("atlases/stats_screen_atlas.sprites/stats_questionmark.tres");
  private static readonly string _streakIconPath = ImageHelper.GetImagePath("atlases/stats_screen_atlas.sprites/stats_chain.tres");
  private Node _gridContainer;
  private NStatEntry _achievementsEntry;
  private NStatEntry _playtimeEntry;
  private NStatEntry _cardsEntry;
  private NStatEntry _winLossEntry;
  private NStatEntry _monsterEntry;
  private NStatEntry _relicEntry;
  private NStatEntry _potionEntry;
  private NStatEntry _eventsEntry;
  private NStatEntry _streakEntry;
  private Control _characterStatContainer;
  private Tween? _screenTween;

  public static string[] AssetPaths
  {
    get
    {
      List<string> stringList = new List<string>();
      stringList.Add(NGeneralStatsGrid._achievementsIconPath);
      stringList.Add(NGeneralStatsGrid._playtimeIconPath);
      stringList.Add(NGeneralStatsGrid._cardsIconPath);
      stringList.Add(NGeneralStatsGrid._winLossIconPath);
      stringList.Add(NGeneralStatsGrid._monsterIconPath);
      stringList.Add(NGeneralStatsGrid._ancientsIconPath);
      stringList.Add(NGeneralStatsGrid._relicIconPath);
      stringList.Add(NGeneralStatsGrid._potionIconPath);
      stringList.Add(NGeneralStatsGrid._eventsIconPath);
      stringList.Add(NGeneralStatsGrid._streakIconPath);
      stringList.AddRange((IEnumerable<string>) NCharacterStats.AssetPaths);
      return stringList.ToArray();
    }
  }

  private static HoverTip PlaytimeTip
  {
    get
    {
      return new HoverTip(new LocString("stats_screen", "TIP_PLAYTIME.header"), new LocString("stats_screen", "TIP_PLAYTIME.description"));
    }
  }

  private static HoverTip WinsLossesTip
  {
    get
    {
      return new HoverTip(new LocString("stats_screen", "TIP_WIN_LOSS.header"), new LocString("stats_screen", "TIP_WIN_LOSS.description"));
    }
  }

  public override void _Ready()
  {
    this._gridContainer = (Node) ((Node) this).GetNode<Control>(NodePath.op_Implicit("%GridContainer"));
    this._characterStatContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%CharacterStatsContainer"));
    this._achievementsEntry = this.CreateSection(NGeneralStatsGrid._achievementsIconPath);
    this._playtimeEntry = this.CreateSection(NGeneralStatsGrid._playtimeIconPath);
    this._cardsEntry = this.CreateSection(NGeneralStatsGrid._cardsIconPath);
    this._winLossEntry = this.CreateSection(NGeneralStatsGrid._winLossIconPath);
    this._monsterEntry = this.CreateSection(NGeneralStatsGrid._monsterIconPath);
    this._relicEntry = this.CreateSection(NGeneralStatsGrid._relicIconPath);
    this._potionEntry = this.CreateSection(NGeneralStatsGrid._potionIconPath);
    this._eventsEntry = this.CreateSection(NGeneralStatsGrid._eventsIconPath);
    this._streakEntry = this.CreateSection(NGeneralStatsGrid._streakIconPath);
    this.SetupHoverTips();
  }

  private NStatEntry CreateSection(string imgUrl)
  {
    NStatEntry child = NStatEntry.Create(imgUrl);
    this._gridContainer.AddChildSafely((Node) child);
    return child;
  }

  public void LoadStats()
  {
    ProgressState progressSave = SaveManager.Instance.Progress;
    SaveManager instance = SaveManager.Instance;
    LocString locString1 = new LocString("stats_screen", "ENTRY_ACHIEVEMENTS.top");
    int denominator = AchievementsUtil.TotalAchievementCount();
    int numerator1 = AchievementsUtil.UnlockedAchievementCount();
    locString1.Add("Amount", StringHelper.RatioFormat(numerator1, denominator));
    this._achievementsEntry.SetTopText(locString1.GetFormattedText() ?? "");
    LocString locString2 = new LocString("stats_screen", "ENTRY_ACHIEVEMENTS.bottom");
    int numerator2 = progressSave.Epochs.Count<SerializableEpoch>((Func<SerializableEpoch, bool>) (epoch => epoch.State >= EpochState.Revealed));
    if (EpochModel.AllEpochIds.All<string>((Func<string, bool>) (id => progressSave.Epochs.Any<SerializableEpoch>((Func<SerializableEpoch, bool>) (epoch => epoch.Id == id)))))
    {
      int count = progressSave.Epochs.Count;
      locString2.Add("Amount", StringHelper.RatioFormat(numerator2, count));
    }
    else
      locString2.Add("Amount", StringHelper.RatioFormat(numerator2.ToString(), "??"));
    this._achievementsEntry.SetBottomText(locString2.GetFormattedText() ?? "");
    LocString locString3 = new LocString("stats_screen", "ENTRY_PLAYTIME.top");
    locString3.Add("Playtime", TimeFormatting.Format((float) progressSave.TotalPlaytime));
    this._playtimeEntry.SetTopText(locString3.GetFormattedText());
    if (progressSave.Wins > 0)
    {
      LocString locString4 = new LocString("stats_screen", "ENTRY_PLAYTIME.bottom");
      locString4.Add("FastestWin", TimeFormatting.Format((float) progressSave.FastestVictory));
      this._playtimeEntry.SetBottomText(locString4.GetFormattedText());
    }
    LocString locString5 = new LocString("stats_screen", "ENTRY_CARDS.top");
    locString5.Add("Amount", StringHelper.RatioFormat(instance.GetTotalUnlockedCards(), SaveManager.GetUnlockableCardCount()));
    this._cardsEntry.SetTopText(locString5.GetFormattedText() ?? "");
    LocString locString6 = new LocString("stats_screen", "ENTRY_CARDS.bottom");
    locString6.Add("Amount", StringHelper.RatioFormat(((IReadOnlyCollection<ModelId>) progressSave.DiscoveredCards).Count, ModelDb.AllCards.Count<CardModel>()));
    this._cardsEntry.SetBottomText(locString6.GetFormattedText());
    int ascensionProgress = instance.GetAggregateAscensionProgress();
    if (ascensionProgress > 0)
    {
      LocString locString7 = new LocString("stats_screen", "ENTRY_WIN_LOSS.top");
      locString7.Add("Amount", StringHelper.RatioFormat(ascensionProgress, SaveManager.GetAggregateAscensionCount()));
      this._winLossEntry.SetTopText(locString7.GetFormattedText() ?? "");
    }
    LocString locString8 = new LocString("stats_screen", "ENTRY_WIN_LOSS.bottom");
    locString8.Add("Wins", (Decimal) progressSave.Wins);
    locString8.Add("Losses", (Decimal) progressSave.Losses);
    this._winLossEntry.SetBottomText(locString8.GetFormattedText());
    LocString locString9 = new LocString("stats_screen", "ENTRY_MONSTER.top");
    locString9.Add("Amount", StringHelper.Radix(instance.GetTotalKills()));
    this._monsterEntry.SetTopText(locString9.GetFormattedText() ?? "");
    LocString locString10 = new LocString("stats_screen", "ENTRY_MONSTER.bottom");
    locString10.Add("Amount", StringHelper.RatioFormat(instance.Progress.EnemyStats.Count, ModelDb.Monsters.Count<MonsterModel>()));
    this._monsterEntry.SetBottomText(locString10.GetFormattedText() ?? "");
    HashSet<ModelId> hashSet = ModelDb.AllEvents.Select<EventModel, ModelId>((Func<EventModel, ModelId>) (e => e.Id)).ToHashSet<ModelId>();
    int numerator3 = ((IEnumerable<ModelId>) progressSave.DiscoveredEvents).Intersect<ModelId>((IEnumerable<ModelId>) hashSet).Count<ModelId>();
    ((IEnumerable<ModelId>) progressSave.DiscoveredEvents).Except<ModelId>((IEnumerable<ModelId>) hashSet).Count<ModelId>();
    LocString locString11 = new LocString("stats_screen", "ENTRY_RELIC.top");
    locString11.Add("Amount", StringHelper.RatioFormat(instance.GetTotalUnlockedRelics(), SaveManager.GetUnlockableRelicCount()));
    this._relicEntry.SetTopText(locString11.GetFormattedText() ?? "");
    LocString locString12 = new LocString("stats_screen", "ENTRY_RELIC.bottom");
    locString12.Add("Amount", StringHelper.RatioFormat(((IReadOnlyCollection<ModelId>) progressSave.DiscoveredRelics).Count, ModelDb.AllRelics.Count<RelicModel>()));
    this._relicEntry.SetBottomText(locString12.GetFormattedText());
    LocString locString13 = new LocString("stats_screen", "ENTRY_POTION.top");
    locString13.Add("Amount", StringHelper.RatioFormat(instance.GetTotalUnlockedPotions(), SaveManager.GetUnlockablePotionCount()));
    this._potionEntry.SetTopText(locString13.GetFormattedText() ?? "");
    LocString locString14 = new LocString("stats_screen", "ENTRY_POTION.bottom");
    locString14.Add("Amount", (Decimal) ModelDb.AllPotions.Count<PotionModel>());
    this._potionEntry.SetBottomText(locString14.GetFormattedText());
    LocString locString15 = new LocString("stats_screen", "ENTRY_EVENTS.top");
    locString15.Add("Amount", "N/A");
    this._eventsEntry.SetTopText(locString15.GetFormattedText());
    LocString locString16 = new LocString("stats_screen", "ENTRY_EVENTS.bottom");
    locString16.Add("Amount", StringHelper.RatioFormat(numerator3, hashSet.Count));
    this._eventsEntry.SetBottomText(locString16.GetFormattedText());
    LocString locString17 = new LocString("stats_screen", "ENTRY_STREAK.top");
    locString17.Add("Amount", (Decimal) progressSave.BestWinStreak);
    this._streakEntry.SetTopText(locString17.GetFormattedText());
    if (ascensionProgress > 999999999)
    {
      LocString locString18 = new LocString("stats_screen", "ENTRY_STREAK.bottom");
      locString18.Add("Amount", 5M);
      this._streakEntry.SetBottomText($"[red]{locString18.GetFormattedText()}[/red]");
    }
    ((Node) this._characterStatContainer).FreeChildren();
    this.CreateCharacterSection(progressSave, ModelDb.Character<Ironclad>().Id);
    this.CreateCharacterSection(progressSave, ModelDb.Character<Silent>().Id);
    this.CreateCharacterSection(progressSave, ModelDb.Character<Regent>().Id);
    this.CreateCharacterSection(progressSave, ModelDb.Character<Necrobinder>().Id);
    this.CreateCharacterSection(progressSave, ModelDb.Character<Defect>().Id);
  }

  private void CreateCharacterSection(ProgressState progressSave, ModelId id)
  {
    CharacterStats statsForCharacter = progressSave.GetStatsForCharacter(id);
    if (statsForCharacter == null)
      return;
    ((Node) this._characterStatContainer).AddChildSafely((Node) NCharacterStats.Create(statsForCharacter));
  }

  private void SetupHoverTips()
  {
    this._playtimeEntry.SetHoverTip(NGeneralStatsGrid.PlaytimeTip);
    if (SaveManager.Instance.GetAggregateAscensionProgress() <= 0)
      return;
    this._winLossEntry.SetHoverTip(NGeneralStatsGrid.WinsLossesTip);
  }

  public Control DefaultFocusedControl => (Control) this._achievementsEntry;

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NGeneralStatsGrid.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGeneralStatsGrid.MethodName.CreateSection, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("imgUrl"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NGeneralStatsGrid.MethodName.LoadStats, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGeneralStatsGrid.MethodName.SetupHoverTips, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NGeneralStatsGrid.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGeneralStatsGrid.MethodName.CreateSection) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NStatEntry section = this.CreateSection(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<NStatEntry>(ref section);
      return true;
    }
    if (StringName.op_Equality(ref method, NGeneralStatsGrid.MethodName.LoadStats) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.LoadStats();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NGeneralStatsGrid.MethodName.SetupHoverTips) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.SetupHoverTips();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NGeneralStatsGrid.MethodName._Ready) || StringName.op_Equality(ref method, NGeneralStatsGrid.MethodName.CreateSection) || StringName.op_Equality(ref method, NGeneralStatsGrid.MethodName.LoadStats) || StringName.op_Equality(ref method, NGeneralStatsGrid.MethodName.SetupHoverTips) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NGeneralStatsGrid.PropertyName._gridContainer))
    {
      this._gridContainer = VariantUtils.ConvertTo<Node>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGeneralStatsGrid.PropertyName._achievementsEntry))
    {
      this._achievementsEntry = VariantUtils.ConvertTo<NStatEntry>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGeneralStatsGrid.PropertyName._playtimeEntry))
    {
      this._playtimeEntry = VariantUtils.ConvertTo<NStatEntry>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGeneralStatsGrid.PropertyName._cardsEntry))
    {
      this._cardsEntry = VariantUtils.ConvertTo<NStatEntry>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGeneralStatsGrid.PropertyName._winLossEntry))
    {
      this._winLossEntry = VariantUtils.ConvertTo<NStatEntry>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGeneralStatsGrid.PropertyName._monsterEntry))
    {
      this._monsterEntry = VariantUtils.ConvertTo<NStatEntry>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGeneralStatsGrid.PropertyName._relicEntry))
    {
      this._relicEntry = VariantUtils.ConvertTo<NStatEntry>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGeneralStatsGrid.PropertyName._potionEntry))
    {
      this._potionEntry = VariantUtils.ConvertTo<NStatEntry>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGeneralStatsGrid.PropertyName._eventsEntry))
    {
      this._eventsEntry = VariantUtils.ConvertTo<NStatEntry>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGeneralStatsGrid.PropertyName._streakEntry))
    {
      this._streakEntry = VariantUtils.ConvertTo<NStatEntry>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGeneralStatsGrid.PropertyName._characterStatContainer))
    {
      this._characterStatContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NGeneralStatsGrid.PropertyName._screenTween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._screenTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NGeneralStatsGrid.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NGeneralStatsGrid.PropertyName._gridContainer))
    {
      value = VariantUtils.CreateFrom<Node>(ref this._gridContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NGeneralStatsGrid.PropertyName._achievementsEntry))
    {
      value = VariantUtils.CreateFrom<NStatEntry>(ref this._achievementsEntry);
      return true;
    }
    if (StringName.op_Equality(ref name, NGeneralStatsGrid.PropertyName._playtimeEntry))
    {
      value = VariantUtils.CreateFrom<NStatEntry>(ref this._playtimeEntry);
      return true;
    }
    if (StringName.op_Equality(ref name, NGeneralStatsGrid.PropertyName._cardsEntry))
    {
      value = VariantUtils.CreateFrom<NStatEntry>(ref this._cardsEntry);
      return true;
    }
    if (StringName.op_Equality(ref name, NGeneralStatsGrid.PropertyName._winLossEntry))
    {
      value = VariantUtils.CreateFrom<NStatEntry>(ref this._winLossEntry);
      return true;
    }
    if (StringName.op_Equality(ref name, NGeneralStatsGrid.PropertyName._monsterEntry))
    {
      value = VariantUtils.CreateFrom<NStatEntry>(ref this._monsterEntry);
      return true;
    }
    if (StringName.op_Equality(ref name, NGeneralStatsGrid.PropertyName._relicEntry))
    {
      value = VariantUtils.CreateFrom<NStatEntry>(ref this._relicEntry);
      return true;
    }
    if (StringName.op_Equality(ref name, NGeneralStatsGrid.PropertyName._potionEntry))
    {
      value = VariantUtils.CreateFrom<NStatEntry>(ref this._potionEntry);
      return true;
    }
    if (StringName.op_Equality(ref name, NGeneralStatsGrid.PropertyName._eventsEntry))
    {
      value = VariantUtils.CreateFrom<NStatEntry>(ref this._eventsEntry);
      return true;
    }
    if (StringName.op_Equality(ref name, NGeneralStatsGrid.PropertyName._streakEntry))
    {
      value = VariantUtils.CreateFrom<NStatEntry>(ref this._streakEntry);
      return true;
    }
    if (StringName.op_Equality(ref name, NGeneralStatsGrid.PropertyName._characterStatContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._characterStatContainer);
      return true;
    }
    if (!StringName.op_Equality(ref name, NGeneralStatsGrid.PropertyName._screenTween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._screenTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NGeneralStatsGrid.PropertyName._gridContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGeneralStatsGrid.PropertyName._achievementsEntry, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGeneralStatsGrid.PropertyName._playtimeEntry, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGeneralStatsGrid.PropertyName._cardsEntry, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGeneralStatsGrid.PropertyName._winLossEntry, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGeneralStatsGrid.PropertyName._monsterEntry, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGeneralStatsGrid.PropertyName._relicEntry, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGeneralStatsGrid.PropertyName._potionEntry, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGeneralStatsGrid.PropertyName._eventsEntry, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGeneralStatsGrid.PropertyName._streakEntry, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGeneralStatsGrid.PropertyName._characterStatContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGeneralStatsGrid.PropertyName._screenTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGeneralStatsGrid.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NGeneralStatsGrid.PropertyName._gridContainer, Variant.From<Node>(ref this._gridContainer));
    info.AddProperty(NGeneralStatsGrid.PropertyName._achievementsEntry, Variant.From<NStatEntry>(ref this._achievementsEntry));
    info.AddProperty(NGeneralStatsGrid.PropertyName._playtimeEntry, Variant.From<NStatEntry>(ref this._playtimeEntry));
    info.AddProperty(NGeneralStatsGrid.PropertyName._cardsEntry, Variant.From<NStatEntry>(ref this._cardsEntry));
    info.AddProperty(NGeneralStatsGrid.PropertyName._winLossEntry, Variant.From<NStatEntry>(ref this._winLossEntry));
    info.AddProperty(NGeneralStatsGrid.PropertyName._monsterEntry, Variant.From<NStatEntry>(ref this._monsterEntry));
    info.AddProperty(NGeneralStatsGrid.PropertyName._relicEntry, Variant.From<NStatEntry>(ref this._relicEntry));
    info.AddProperty(NGeneralStatsGrid.PropertyName._potionEntry, Variant.From<NStatEntry>(ref this._potionEntry));
    info.AddProperty(NGeneralStatsGrid.PropertyName._eventsEntry, Variant.From<NStatEntry>(ref this._eventsEntry));
    info.AddProperty(NGeneralStatsGrid.PropertyName._streakEntry, Variant.From<NStatEntry>(ref this._streakEntry));
    info.AddProperty(NGeneralStatsGrid.PropertyName._characterStatContainer, Variant.From<Control>(ref this._characterStatContainer));
    info.AddProperty(NGeneralStatsGrid.PropertyName._screenTween, Variant.From<Tween>(ref this._screenTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NGeneralStatsGrid.PropertyName._gridContainer, ref variant1))
      this._gridContainer = ((Variant) ref variant1).As<Node>();
    Variant variant2;
    if (info.TryGetProperty(NGeneralStatsGrid.PropertyName._achievementsEntry, ref variant2))
      this._achievementsEntry = ((Variant) ref variant2).As<NStatEntry>();
    Variant variant3;
    if (info.TryGetProperty(NGeneralStatsGrid.PropertyName._playtimeEntry, ref variant3))
      this._playtimeEntry = ((Variant) ref variant3).As<NStatEntry>();
    Variant variant4;
    if (info.TryGetProperty(NGeneralStatsGrid.PropertyName._cardsEntry, ref variant4))
      this._cardsEntry = ((Variant) ref variant4).As<NStatEntry>();
    Variant variant5;
    if (info.TryGetProperty(NGeneralStatsGrid.PropertyName._winLossEntry, ref variant5))
      this._winLossEntry = ((Variant) ref variant5).As<NStatEntry>();
    Variant variant6;
    if (info.TryGetProperty(NGeneralStatsGrid.PropertyName._monsterEntry, ref variant6))
      this._monsterEntry = ((Variant) ref variant6).As<NStatEntry>();
    Variant variant7;
    if (info.TryGetProperty(NGeneralStatsGrid.PropertyName._relicEntry, ref variant7))
      this._relicEntry = ((Variant) ref variant7).As<NStatEntry>();
    Variant variant8;
    if (info.TryGetProperty(NGeneralStatsGrid.PropertyName._potionEntry, ref variant8))
      this._potionEntry = ((Variant) ref variant8).As<NStatEntry>();
    Variant variant9;
    if (info.TryGetProperty(NGeneralStatsGrid.PropertyName._eventsEntry, ref variant9))
      this._eventsEntry = ((Variant) ref variant9).As<NStatEntry>();
    Variant variant10;
    if (info.TryGetProperty(NGeneralStatsGrid.PropertyName._streakEntry, ref variant10))
      this._streakEntry = ((Variant) ref variant10).As<NStatEntry>();
    Variant variant11;
    if (info.TryGetProperty(NGeneralStatsGrid.PropertyName._characterStatContainer, ref variant11))
      this._characterStatContainer = ((Variant) ref variant11).As<Control>();
    Variant variant12;
    if (!info.TryGetProperty(NGeneralStatsGrid.PropertyName._screenTween, ref variant12))
      return;
    this._screenTween = ((Variant) ref variant12).As<Tween>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName CreateSection = StringName.op_Implicit(nameof (CreateSection));
    public static readonly StringName LoadStats = StringName.op_Implicit(nameof (LoadStats));
    public static readonly StringName SetupHoverTips = StringName.op_Implicit(nameof (SetupHoverTips));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _gridContainer = StringName.op_Implicit(nameof (_gridContainer));
    public static readonly StringName _achievementsEntry = StringName.op_Implicit(nameof (_achievementsEntry));
    public static readonly StringName _playtimeEntry = StringName.op_Implicit(nameof (_playtimeEntry));
    public static readonly StringName _cardsEntry = StringName.op_Implicit(nameof (_cardsEntry));
    public static readonly StringName _winLossEntry = StringName.op_Implicit(nameof (_winLossEntry));
    public static readonly StringName _monsterEntry = StringName.op_Implicit(nameof (_monsterEntry));
    public static readonly StringName _relicEntry = StringName.op_Implicit(nameof (_relicEntry));
    public static readonly StringName _potionEntry = StringName.op_Implicit(nameof (_potionEntry));
    public static readonly StringName _eventsEntry = StringName.op_Implicit(nameof (_eventsEntry));
    public static readonly StringName _streakEntry = StringName.op_Implicit(nameof (_streakEntry));
    public static readonly StringName _characterStatContainer = StringName.op_Implicit(nameof (_characterStatContainer));
    public static readonly StringName _screenTween = StringName.op_Implicit(nameof (_screenTween));
  }

  public class SignalName : Control.SignalName
  {
  }
}
