// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.HoverTips.NMapPointHistoryHoverTip
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.HoverTips;

[ScriptPath("res://src/Core/Nodes/HoverTips/NMapPointHistoryHoverTip.cs")]
public class NMapPointHistoryHoverTip : MarginContainer
{
  private readonly LocString _mapPointRoomStatsLoc = new LocString("run_history", "MAP_POINT_HISTORY.room_stats");
  private readonly LocString _mapPointPlayerStatsLoc = new LocString("run_history", "MAP_POINT_HISTORY.player_stats");
  private readonly LocString _combatStats = new LocString("run_history", "MAP_POINT_HISTORY.combatStats");
  private readonly LocString _floorTitle = new LocString("run_history", "MAP_POINT_HISTORY.header");
  private readonly LocString _chose = new LocString("run_history", "MAP_POINT_HISTORY.chose");
  private readonly LocString _skipped = new LocString("run_history", "MAP_POINT_HISTORY.skipped");
  private readonly LocString _rewardsHeaderLoc = new LocString("run_history", "HISTORY_ENTRY.rewardsHeader");
  private readonly LocString _skippedHeaderLoc = new LocString("run_history", "HISTORY_ENTRY.skippedHeader");
  private readonly LocString _enchanted = new LocString("run_history", "HISTORY_ENTRY.enchanted");
  private readonly LocString _obtained = new LocString("run_history", "HISTORY_ENTRY.obtained");
  private readonly LocString _goldGained = new LocString("run_history", "HISTORY_ENTRY.goldGained");
  private readonly LocString _goldSpent = new LocString("run_history", "HISTORY_ENTRY.goldSpent");
  private readonly LocString _goldLost = new LocString("run_history", "HISTORY_ENTRY.goldLost");
  private readonly LocString _goldStolen = new LocString("run_history", "HISTORY_ENTRY.goldStolen");
  private readonly LocString _used = new LocString("run_history", "HISTORY_ENTRY.used");
  private readonly LocString _removed = new LocString("run_history", "HISTORY_ENTRY.removed");
  private readonly LocString _transformed = new LocString("run_history", "HISTORY_ENTRY.transformed");
  private readonly LocString _upgraded = new LocString("run_history", "HISTORY_ENTRY.upgraded");
  private readonly LocString _downgraded = new LocString("run_history", "HISTORY_ENTRY.downgraded");
  private readonly LocString _damaged = new LocString("run_history", "MAP_POINT_HISTORY.damageTaken");
  private readonly LocString _healed = new LocString("run_history", "MAP_POINT_HISTORY.healed");
  private readonly LocString _maxHpGained = new LocString("run_history", "MAP_POINT_HISTORY.maxHpGained");
  private readonly LocString _maxHpLost = new LocString("run_history", "MAP_POINT_HISTORY.maxHpLost");
  private readonly LocString _turns = new LocString("run_history", "MAP_POINT_HISTORY.turnsTaken");
  private readonly LocString _quests = new LocString("run_history", "MAP_POINT_HISTORY.questCompleted");
  private const string _goldIconPath = "res://images/packed/sprite_fonts/gold_icon.png";
  private const string _cardIconPath = "res://images/packed/sprite_fonts/card_icon.png";
  private const string _chestIconPath = "res://images/packed/sprite_fonts/chest_icon.png";
  private const string _potionIconPath = "res://images/packed/sprite_fonts/potion_icon.png";
  private const string _roomHistoryTipScenePath = "res://scenes/ui/map_point_history_hover_tip.tscn";
  private MapPointHistoryEntry _entry;
  private int _floorNum;
  private ulong _playerId;
  private MegaLabel _titleLabel;
  private RichTextLabel _playerStats;
  private RichTextLabel _roomStats;
  private Control _rewardStatsContainer;
  private Control _skippedStatsContainer;
  private List<RichTextLabel> _rewardRows;
  private List<RichTextLabel> _skippedRows;
  private RichTextLabel _actionStats;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>("res://scenes/ui/map_point_history_hover_tip.tscn");
    }
  }

  public static NMapPointHistoryHoverTip Create(
    int floorNum,
    ulong playerId,
    MapPointHistoryEntry historyEntry)
  {
    NMapPointHistoryHoverTip pointHistoryHoverTip = PreloadManager.Cache.GetScene("res://scenes/ui/map_point_history_hover_tip.tscn").Instantiate<NMapPointHistoryHoverTip>((PackedScene.GenEditState) 0L);
    pointHistoryHoverTip._entry = historyEntry;
    pointHistoryHoverTip._floorNum = floorNum;
    pointHistoryHoverTip._playerId = playerId;
    return pointHistoryHoverTip;
  }

  public override void _Ready()
  {
    this._titleLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%Title"));
    this._playerStats = ((Node) this).GetNode<RichTextLabel>(NodePath.op_Implicit("%PlayerStats"));
    this._roomStats = ((Node) this).GetNode<RichTextLabel>(NodePath.op_Implicit("%RoomStats"));
    this._actionStats = ((Node) this).GetNode<RichTextLabel>(NodePath.op_Implicit("%CardStats"));
    this._rewardStatsContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%RewardStats"));
    this._skippedStatsContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%SkippedStats"));
    this._rewardRows = ((IEnumerable) ((Node) ((Node) this).GetNode<Control>(NodePath.op_Implicit("%RewardRows"))).GetChildren(false)).OfType<RichTextLabel>().ToList<RichTextLabel>();
    this._skippedRows = ((IEnumerable) ((Node) ((Node) this).GetNode<Control>(NodePath.op_Implicit("%SkippedRows"))).GetChildren(false)).OfType<RichTextLabel>().ToList<RichTextLabel>();
    ((Node) this._rewardStatsContainer).GetNode<MegaLabel>(NodePath.op_Implicit("Header")).SetTextAutoSize(this._rewardsHeaderLoc.GetFormattedText());
    ((Node) this._skippedStatsContainer).GetNode<MegaLabel>(NodePath.op_Implicit("Header")).SetTextAutoSize(this._skippedHeaderLoc.GetFormattedText());
    this._floorTitle.Add("FloorNum", (Decimal) this._floorNum);
    this._titleLabel.SetTextAutoSize(this._floorTitle.GetFormattedText());
    string str1;
    switch (this._entry.MapPointType)
    {
      case MapPointType.Shop:
        str1 = "ROOM_MERCHANT";
        break;
      case MapPointType.Treasure:
        str1 = "ROOM_TREASURE";
        break;
      case MapPointType.RestSite:
        str1 = "ROOM_REST";
        break;
      case MapPointType.Monster:
        str1 = "ROOM_ENEMY";
        break;
      case MapPointType.Elite:
        str1 = "ROOM_ELITE";
        break;
      case MapPointType.Boss:
        str1 = "ROOM_BOSS";
        break;
      case MapPointType.Ancient:
        str1 = "ROOM_ANCIENT";
        break;
      default:
        str1 = (string) null;
        break;
    }
    string str2 = str1;
    if (this._entry.MapPointType == MapPointType.Unknown)
    {
      string str3;
      switch (this._entry.Rooms.First<MapPointRoomHistoryEntry>().RoomType)
      {
        case RoomType.Monster:
          str3 = "ROOM_UNKNOWN_ENEMY";
          break;
        case RoomType.Elite:
          str3 = "ROOM_UNKNOWN_ELITE";
          break;
        case RoomType.Treasure:
          str3 = "ROOM_UNKNOWN_TREASURE";
          break;
        case RoomType.Shop:
          str3 = "ROOM_UNKNOWN_MERCHANT";
          break;
        case RoomType.Event:
          str3 = "ROOM_EVENT";
          break;
        default:
          str3 = (string) null;
          break;
      }
      str2 = str3;
    }
    this._mapPointRoomStatsLoc.Add("MapPointType", new LocString("static_hover_tips", str2 + ".title"));
    switch (this._entry.Rooms.First<MapPointRoomHistoryEntry>().RoomType)
    {
      case RoomType.Treasure:
      case RoomType.Shop:
      case RoomType.RestSite:
        this._mapPointRoomStatsLoc.Add("ModelTitle", "");
        break;
      case RoomType.Event:
        this._mapPointRoomStatsLoc.Add("ModelTitle", SaveUtil.EventOrDeprecated(this._entry.Rooms.First<MapPointRoomHistoryEntry>().ModelId).Title);
        break;
      default:
        this._mapPointRoomStatsLoc.Add("ModelTitle", SaveUtil.EncounterOrDeprecated(this._entry.Rooms.First<MapPointRoomHistoryEntry>().ModelId).Title);
        break;
    }
    this._roomStats.Text = this._mapPointRoomStatsLoc.GetFormattedText();
    PlayerMapPointHistoryEntry playerEntry = this._entry.PlayerStats.FirstOrDefault<PlayerMapPointHistoryEntry>((Func<PlayerMapPointHistoryEntry, bool>) (e => (long) e.PlayerId == (long) this._playerId));
    if (playerEntry == null)
      throw new InvalidOperationException($"Player with ID {this._playerId} not found in player stats for this run history!");
    this._mapPointPlayerStatsLoc.Add("HP", (Decimal) playerEntry.CurrentHp);
    this._mapPointPlayerStatsLoc.Add("MaxHP", (Decimal) playerEntry.MaxHp);
    this._mapPointPlayerStatsLoc.Add("Gold", (Decimal) playerEntry.CurrentGold);
    this._playerStats.Text = this._mapPointPlayerStatsLoc.GetFormattedText();
    this.PopulateActionStats(playerEntry);
    this.PopulateRewardAndSkippedEntries(playerEntry);
  }

  private void PopulateActionStats(PlayerMapPointHistoryEntry playerEntry)
  {
    StringBuilder stringBuilder1 = new StringBuilder();
    StringBuilder.AppendInterpolatedStringHandler interpolatedStringHandler;
    if (this._entry.MapPointType == MapPointType.Ancient)
    {
      LocString ancientPickedChoiceLoc = playerEntry.GetAncientPickedChoiceLoc();
      if (ancientPickedChoiceLoc != null)
      {
        this._chose.Add("Choice", ancientPickedChoiceLoc);
        StringBuilder stringBuilder2 = stringBuilder1;
        StringBuilder stringBuilder3 = stringBuilder2;
        // ISSUE: explicit constructor call
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(2, 1, stringBuilder2);
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("\t");
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(this._chose.GetFormattedText());
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("\n");
        ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
        stringBuilder3.Append(ref local);
      }
      foreach (LocString variable in playerEntry.GetAncientSkippedChoiceLoc())
      {
        this._skipped.Add("Choice", variable);
        StringBuilder stringBuilder4 = stringBuilder1;
        StringBuilder stringBuilder5 = stringBuilder4;
        interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(2, 1, stringBuilder4);
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("\t");
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(this._skipped.GetFormattedText());
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("\n");
        ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
        stringBuilder5.Append(ref local);
      }
    }
    else
    {
      ModelId modelId = this._entry.FirstRoomOfType(RoomType.Event)?.ModelId;
      EventModel eventModel = modelId != (ModelId) null ? SaveUtil.EventOrDeprecated(modelId) : (EventModel) null;
      if (eventModel != null)
      {
        foreach (EventOptionHistoryEntry eventChoice in playerEntry.EventChoices)
        {
          eventModel.DynamicVars.AddTo(eventChoice.Title);
          if (eventChoice.Variables != null)
          {
            foreach (KeyValuePair<string, object> variable in eventChoice.Variables)
              eventChoice.Title.AddObj(variable.Key, variable.Value);
          }
          this._chose.Add("Choice", eventChoice.Title.GetFormattedText());
          StringBuilder stringBuilder6 = stringBuilder1;
          StringBuilder stringBuilder7 = stringBuilder6;
          interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(2, 1, stringBuilder6);
          ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("\t");
          ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(this._chose.GetFormattedText());
          ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("\n");
          ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
          stringBuilder7.Append(ref local);
        }
      }
    }
    foreach (string restSiteChoice in playerEntry.RestSiteChoices)
    {
      this._chose.Add("Choice", new LocString("rest_site_ui", $"OPTION_{restSiteChoice}.name").GetFormattedText() ?? "");
      StringBuilder stringBuilder8 = stringBuilder1;
      StringBuilder stringBuilder9 = stringBuilder8;
      interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(2, 1, stringBuilder8);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("\t");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(this._chose.GetFormattedText());
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("\n");
      ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
      stringBuilder9.Append(ref local);
    }
    MapPointRoomHistoryEntry roomHistoryEntry = this._entry.Rooms.FirstOrDefault<MapPointRoomHistoryEntry>((Func<MapPointRoomHistoryEntry, bool>) (r => r.RoomType.IsCombatRoom()));
    if (playerEntry.IsAffectedByFurCoat)
    {
      StringBuilder stringBuilder10 = stringBuilder1;
      StringBuilder stringBuilder11 = stringBuilder10;
      // ISSUE: explicit constructor call
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(2, 1, stringBuilder10);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("\t");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(FurCoat.HistoryEntry.GetFormattedText());
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("\n");
      ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
      stringBuilder11.Append(ref local);
    }
    if (playerEntry.MaxHpLost > 0)
    {
      this._maxHpLost.Add("HP", (Decimal) playerEntry.MaxHpLost);
      StringBuilder stringBuilder12 = stringBuilder1;
      StringBuilder stringBuilder13 = stringBuilder12;
      // ISSUE: explicit constructor call
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(2, 1, stringBuilder12);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("\t");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(this._maxHpLost.GetFormattedText());
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("\n");
      ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
      stringBuilder13.Append(ref local);
    }
    if (playerEntry.DamageTaken > 0 || roomHistoryEntry != null)
    {
      this._damaged.Add("Damage", (Decimal) playerEntry.DamageTaken);
      StringBuilder stringBuilder14 = stringBuilder1;
      StringBuilder stringBuilder15 = stringBuilder14;
      // ISSUE: explicit constructor call
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(2, 1, stringBuilder14);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("\t");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(this._damaged.GetFormattedText());
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("\n");
      ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
      stringBuilder15.Append(ref local);
    }
    if (playerEntry.MaxHpGained > 0)
    {
      this._maxHpGained.Add("HP", (Decimal) playerEntry.MaxHpGained);
      StringBuilder stringBuilder16 = stringBuilder1;
      StringBuilder stringBuilder17 = stringBuilder16;
      // ISSUE: explicit constructor call
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(2, 1, stringBuilder16);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("\t");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(this._maxHpGained.GetFormattedText());
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("\n");
      ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
      stringBuilder17.Append(ref local);
    }
    if (playerEntry.HpHealed > 0)
    {
      this._healed.Add("HP", (Decimal) playerEntry.HpHealed);
      StringBuilder stringBuilder18 = stringBuilder1;
      StringBuilder stringBuilder19 = stringBuilder18;
      // ISSUE: explicit constructor call
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(2, 1, stringBuilder18);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("\t");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(this._healed.GetFormattedText());
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("\n");
      ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
      stringBuilder19.Append(ref local);
    }
    if (roomHistoryEntry != null)
    {
      this._turns.Add("Turns", (Decimal) roomHistoryEntry.TurnsTaken);
      StringBuilder stringBuilder20 = stringBuilder1;
      StringBuilder stringBuilder21 = stringBuilder20;
      // ISSUE: explicit constructor call
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(2, 1, stringBuilder20);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("\t");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(this._turns.GetFormattedText());
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("\n");
      ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
      stringBuilder21.Append(ref local);
    }
    foreach (ModelId completedQuest in playerEntry.CompletedQuests)
    {
      this._quests.Add("Quest", SaveUtil.CardOrDeprecated(completedQuest).Title);
      StringBuilder stringBuilder22 = stringBuilder1;
      StringBuilder stringBuilder23 = stringBuilder22;
      interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(2, 1, stringBuilder22);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("\t");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(this._quests.GetFormattedText());
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("\n");
      ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
      stringBuilder23.Append(ref local);
    }
    foreach (ModelId id in playerEntry.PotionUsed)
    {
      this._used.Add("Icon", "[img=top]res://images/packed/sprite_fonts/potion_icon.png[/img]");
      this._used.Add("Title", SaveUtil.PotionOrDeprecated(id).Title);
      StringBuilder stringBuilder24 = stringBuilder1;
      StringBuilder stringBuilder25 = stringBuilder24;
      interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(1, 1, stringBuilder24);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(this._used.GetFormattedText());
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("\n");
      ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
      stringBuilder25.Append(ref local);
    }
    foreach (ModelId id in playerEntry.PotionDiscarded)
    {
      this._removed.Add("Icon", "[img=top]res://images/packed/sprite_fonts/potion_icon.png[/img]");
      this._removed.Add("Title", SaveUtil.PotionOrDeprecated(id).Title);
      StringBuilder stringBuilder26 = stringBuilder1;
      StringBuilder stringBuilder27 = stringBuilder26;
      interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(1, 1, stringBuilder26);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(this._removed.GetFormattedText());
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("\n");
      ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
      stringBuilder27.Append(ref local);
    }
    if (playerEntry.GoldSpent > 0)
    {
      this._goldSpent.Add("Amount", (Decimal) playerEntry.GoldSpent);
      StringBuilder stringBuilder28 = stringBuilder1;
      StringBuilder stringBuilder29 = stringBuilder28;
      // ISSUE: explicit constructor call
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(1, 1, stringBuilder28);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(this._goldSpent.GetFormattedText());
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("\n");
      ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
      stringBuilder29.Append(ref local);
    }
    if (playerEntry.GoldLost > 0)
    {
      this._goldLost.Add("Amount", (Decimal) playerEntry.GoldLost);
      StringBuilder stringBuilder30 = stringBuilder1;
      StringBuilder stringBuilder31 = stringBuilder30;
      // ISSUE: explicit constructor call
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(1, 1, stringBuilder30);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(this._goldLost.GetFormattedText());
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("\n");
      ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
      stringBuilder31.Append(ref local);
    }
    if (playerEntry.GoldStolen > 0)
    {
      this._goldStolen.Add("Icon", "[img=top]res://images/packed/sprite_fonts/gold_icon.png[/img]");
      this._goldStolen.Add("Amount", (Decimal) playerEntry.GoldStolen);
      StringBuilder stringBuilder32 = stringBuilder1;
      StringBuilder stringBuilder33 = stringBuilder32;
      // ISSUE: explicit constructor call
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(1, 1, stringBuilder32);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(this._goldStolen.GetFormattedText());
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("\n");
      ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
      stringBuilder33.Append(ref local);
    }
    this._actionStats.Text = stringBuilder1.ToString().Trim('\n');
  }

  private void PopulateRewardAndSkippedEntries(PlayerMapPointHistoryEntry playerEntry)
  {
    List<string> stringList1 = new List<string>();
    List<string> stringList2 = new List<string>();
    if (playerEntry.GoldGained > 0)
    {
      this._goldGained.Add("Icon", "[img=top]res://images/packed/sprite_fonts/gold_icon.png[/img]");
      this._goldGained.Add("Amount", (Decimal) playerEntry.GoldGained);
      stringList1.Add(this._goldGained.GetFormattedText());
    }
    foreach (SerializableCard save in playerEntry.CardsGained)
    {
      this._obtained.Add("Icon", "[img=top]res://images/packed/sprite_fonts/card_icon.png[/img]");
      this._obtained.Add("Title", CardModel.FromSerializable(save).Title);
      stringList1.Add(this._obtained.GetFormattedText());
    }
    foreach (CardChoiceHistoryEntry cardChoice in playerEntry.CardChoices)
    {
      if (!cardChoice.wasPicked)
      {
        this._obtained.Add("Icon", "[img=top]res://images/packed/sprite_fonts/card_icon.png[/img]");
        this._obtained.Add("Title", CardModel.FromSerializable(cardChoice.Card).Title);
        stringList2.Add(this._obtained.GetFormattedText());
      }
    }
    foreach (ModelChoiceHistoryEntry relicChoice in playerEntry.RelicChoices)
    {
      this._obtained.Add("Icon", "[img=top]res://images/packed/sprite_fonts/chest_icon.png[/img]");
      this._obtained.Add("Title", SaveUtil.RelicOrDeprecated(relicChoice.choice).Title);
      if (relicChoice.wasPicked)
        stringList1.Add(this._obtained.GetFormattedText());
      else
        stringList2.Add(this._obtained.GetFormattedText());
    }
    foreach (ModelChoiceHistoryEntry potionChoice in playerEntry.PotionChoices)
    {
      this._obtained.Add("Icon", "[img=top]res://images/packed/sprite_fonts/potion_icon.png[/img]");
      this._obtained.Add("Title", SaveUtil.PotionOrDeprecated(potionChoice.choice).Title);
      if (potionChoice.wasPicked)
        stringList1.Add(this._obtained.GetFormattedText());
      else
        stringList2.Add(this._obtained.GetFormattedText());
    }
    foreach (SerializableCard save in playerEntry.CardsRemoved)
    {
      this._removed.Add("Icon", "[img=top]res://images/packed/sprite_fonts/card_icon.png[/img]");
      this._removed.Add("Title", CardModel.FromSerializable(save).Title);
      stringList1.Add(this._removed.GetFormattedText() ?? "");
    }
    foreach (ModelId id in playerEntry.RelicsRemoved)
    {
      this._removed.Add("Icon", "[img=top]res://images/packed/sprite_fonts/chest_icon.png[/img]");
      this._removed.Add("Title", SaveUtil.RelicOrDeprecated(id).Title);
      stringList1.Add(this._removed.GetFormattedText() ?? "");
    }
    foreach (ModelId upgradedCard in playerEntry.UpgradedCards)
    {
      this._upgraded.Add("Icon", "[img=top]res://images/packed/sprite_fonts/card_icon.png[/img]");
      this._upgraded.Add("Title", SaveUtil.CardOrDeprecated(upgradedCard).Title);
      stringList1.Add(this._upgraded.GetFormattedText() ?? "");
    }
    foreach (ModelId downgradedCard in playerEntry.DowngradedCards)
    {
      this._downgraded.Add("Icon", "[img=top]res://images/packed/sprite_fonts/card_icon.png[/img]");
      this._downgraded.Add("Title", SaveUtil.CardOrDeprecated(downgradedCard).Title);
      stringList1.Add(this._downgraded.GetFormattedText() ?? "");
    }
    foreach (CardEnchantmentHistoryEntry enchantmentHistoryEntry in playerEntry.CardsEnchanted)
    {
      this._enchanted.Add("Icon", "[img=top]res://images/packed/sprite_fonts/card_icon.png[/img]");
      this._enchanted.Add("Title1", CardModel.FromSerializable(enchantmentHistoryEntry.Card).Title);
      this._enchanted.Add("Title2", SaveUtil.EnchantmentOrDeprecated(enchantmentHistoryEntry.Enchantment).Title);
      stringList1.Add(this._enchanted.GetFormattedText() ?? "");
    }
    foreach (CardTransformationHistoryEntry transformationHistoryEntry in playerEntry.CardsTransformed)
    {
      this._transformed.Add("Icon", "[img=top]res://images/packed/sprite_fonts/card_icon.png[/img]");
      this._transformed.Add("Title1", CardModel.FromSerializable(transformationHistoryEntry.OriginalCard).Title);
      this._transformed.Add("Title2", CardModel.FromSerializable(transformationHistoryEntry.FinalCard).Title);
      stringList1.Add(this._transformed.GetFormattedText() ?? "");
    }
    int num1 = Mathf.Max(5, Mathf.CeilToInt((float) stringList1.Count / 2f));
    ((CanvasItem) this._rewardRows[1]).Visible = stringList1.Count > 5;
    ((CanvasItem) this._rewardStatsContainer).Visible = stringList1.Count > 0;
    for (int index = 0; index < stringList1.Count; ++index)
    {
      RichTextLabel rewardRow = this._rewardRows[index / num1];
      rewardRow.Text = $"{rewardRow.Text}\t{stringList1[index]}\n";
    }
    int num2 = Mathf.Max(5, Mathf.CeilToInt((float) stringList2.Count / 2f));
    ((CanvasItem) this._skippedRows[1]).Visible = stringList2.Count > 5;
    ((CanvasItem) this._skippedStatsContainer).Visible = stringList2.Count > 0;
    for (int index = 0; index < stringList2.Count; ++index)
    {
      RichTextLabel skippedRow = this._skippedRows[index / num2];
      skippedRow.Text = $"{skippedRow.Text}\t{stringList2[index]}\n";
    }
    foreach (RichTextLabel rewardRow in this._rewardRows)
      rewardRow.Text = rewardRow.Text.Trim('\n');
    foreach (RichTextLabel skippedRow in this._skippedRows)
      skippedRow.Text = skippedRow.Text.Trim('\n');
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NMapPointHistoryHoverTip.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (!StringName.op_Equality(ref method, NMapPointHistoryHoverTip.MethodName._Ready) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._Ready();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMapPointHistoryHoverTip.MethodName._Ready) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMapPointHistoryHoverTip.PropertyName._floorNum))
    {
      this._floorNum = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapPointHistoryHoverTip.PropertyName._playerId))
    {
      this._playerId = VariantUtils.ConvertTo<ulong>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapPointHistoryHoverTip.PropertyName._titleLabel))
    {
      this._titleLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapPointHistoryHoverTip.PropertyName._playerStats))
    {
      this._playerStats = VariantUtils.ConvertTo<RichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapPointHistoryHoverTip.PropertyName._roomStats))
    {
      this._roomStats = VariantUtils.ConvertTo<RichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapPointHistoryHoverTip.PropertyName._rewardStatsContainer))
    {
      this._rewardStatsContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapPointHistoryHoverTip.PropertyName._skippedStatsContainer))
    {
      this._skippedStatsContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMapPointHistoryHoverTip.PropertyName._actionStats))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._actionStats = VariantUtils.ConvertTo<RichTextLabel>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMapPointHistoryHoverTip.PropertyName._floorNum))
    {
      value = VariantUtils.CreateFrom<int>(ref this._floorNum);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapPointHistoryHoverTip.PropertyName._playerId))
    {
      value = VariantUtils.CreateFrom<ulong>(ref this._playerId);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapPointHistoryHoverTip.PropertyName._titleLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._titleLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapPointHistoryHoverTip.PropertyName._playerStats))
    {
      value = VariantUtils.CreateFrom<RichTextLabel>(ref this._playerStats);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapPointHistoryHoverTip.PropertyName._roomStats))
    {
      value = VariantUtils.CreateFrom<RichTextLabel>(ref this._roomStats);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapPointHistoryHoverTip.PropertyName._rewardStatsContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._rewardStatsContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NMapPointHistoryHoverTip.PropertyName._skippedStatsContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._skippedStatsContainer);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMapPointHistoryHoverTip.PropertyName._actionStats))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<RichTextLabel>(ref this._actionStats);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 2L, NMapPointHistoryHoverTip.PropertyName._floorNum, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NMapPointHistoryHoverTip.PropertyName._playerId, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapPointHistoryHoverTip.PropertyName._titleLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapPointHistoryHoverTip.PropertyName._playerStats, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapPointHistoryHoverTip.PropertyName._roomStats, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapPointHistoryHoverTip.PropertyName._rewardStatsContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapPointHistoryHoverTip.PropertyName._skippedStatsContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NMapPointHistoryHoverTip.PropertyName._actionStats, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NMapPointHistoryHoverTip.PropertyName._floorNum, Variant.From<int>(ref this._floorNum));
    info.AddProperty(NMapPointHistoryHoverTip.PropertyName._playerId, Variant.From<ulong>(ref this._playerId));
    info.AddProperty(NMapPointHistoryHoverTip.PropertyName._titleLabel, Variant.From<MegaLabel>(ref this._titleLabel));
    info.AddProperty(NMapPointHistoryHoverTip.PropertyName._playerStats, Variant.From<RichTextLabel>(ref this._playerStats));
    info.AddProperty(NMapPointHistoryHoverTip.PropertyName._roomStats, Variant.From<RichTextLabel>(ref this._roomStats));
    info.AddProperty(NMapPointHistoryHoverTip.PropertyName._rewardStatsContainer, Variant.From<Control>(ref this._rewardStatsContainer));
    info.AddProperty(NMapPointHistoryHoverTip.PropertyName._skippedStatsContainer, Variant.From<Control>(ref this._skippedStatsContainer));
    info.AddProperty(NMapPointHistoryHoverTip.PropertyName._actionStats, Variant.From<RichTextLabel>(ref this._actionStats));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NMapPointHistoryHoverTip.PropertyName._floorNum, ref variant1))
      this._floorNum = ((Variant) ref variant1).As<int>();
    Variant variant2;
    if (info.TryGetProperty(NMapPointHistoryHoverTip.PropertyName._playerId, ref variant2))
      this._playerId = ((Variant) ref variant2).As<ulong>();
    Variant variant3;
    if (info.TryGetProperty(NMapPointHistoryHoverTip.PropertyName._titleLabel, ref variant3))
      this._titleLabel = ((Variant) ref variant3).As<MegaLabel>();
    Variant variant4;
    if (info.TryGetProperty(NMapPointHistoryHoverTip.PropertyName._playerStats, ref variant4))
      this._playerStats = ((Variant) ref variant4).As<RichTextLabel>();
    Variant variant5;
    if (info.TryGetProperty(NMapPointHistoryHoverTip.PropertyName._roomStats, ref variant5))
      this._roomStats = ((Variant) ref variant5).As<RichTextLabel>();
    Variant variant6;
    if (info.TryGetProperty(NMapPointHistoryHoverTip.PropertyName._rewardStatsContainer, ref variant6))
      this._rewardStatsContainer = ((Variant) ref variant6).As<Control>();
    Variant variant7;
    if (info.TryGetProperty(NMapPointHistoryHoverTip.PropertyName._skippedStatsContainer, ref variant7))
      this._skippedStatsContainer = ((Variant) ref variant7).As<Control>();
    Variant variant8;
    if (!info.TryGetProperty(NMapPointHistoryHoverTip.PropertyName._actionStats, ref variant8))
      return;
    this._actionStats = ((Variant) ref variant8).As<RichTextLabel>();
  }

  public class MethodName : MarginContainer.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
  }

  public class PropertyName : MarginContainer.PropertyName
  {
    public static readonly StringName _floorNum = StringName.op_Implicit(nameof (_floorNum));
    public static readonly StringName _playerId = StringName.op_Implicit(nameof (_playerId));
    public static readonly StringName _titleLabel = StringName.op_Implicit(nameof (_titleLabel));
    public static readonly StringName _playerStats = StringName.op_Implicit(nameof (_playerStats));
    public static readonly StringName _roomStats = StringName.op_Implicit(nameof (_roomStats));
    public static readonly StringName _rewardStatsContainer = StringName.op_Implicit(nameof (_rewardStatsContainer));
    public static readonly StringName _skippedStatsContainer = StringName.op_Implicit(nameof (_skippedStatsContainer));
    public static readonly StringName _actionStats = StringName.op_Implicit(nameof (_actionStats));
  }

  public class SignalName : MarginContainer.SignalName
  {
  }
}
