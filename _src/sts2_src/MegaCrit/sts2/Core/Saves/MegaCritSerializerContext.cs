// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.MegaCritSerializerContext
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Achievements;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Saves.MapDrawing;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves;

[JsonSourceGenerationOptions]
[JsonSerializable(typeof (MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFile))]
[JsonSerializable(typeof (MegaCrit.Sts2.Core.Saves.SerializableRun))]
[JsonSerializable(typeof (MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData))]
[JsonSerializable(typeof (MegaCrit.Sts2.Core.Saves.SettingsSave))]
[JsonSerializable(typeof (MegaCrit.Sts2.Core.Saves.PrefsSave))]
[JsonSerializable(typeof (MegaCrit.Sts2.Core.Saves.ProfileSave))]
[JsonSerializable(typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress))]
[JsonSerializable(typeof (MegaCrit.Sts2.Core.Runs.RunHistory))]
[JsonSerializable(typeof (List<MegaCrit.Sts2.Core.Saves.Migrations.MigratingData>))]
[JsonSerializable(typeof (List<List<MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry>>))]
[JsonSerializable(typeof (MegaCrit.Sts2.Core.Modding.ModManifest))]
[JsonSerializable(typeof (System.Text.Json.Nodes.JsonObject))]
[JsonSerializable(typeof (List<System.Text.Json.Nodes.JsonNode>))]
[JsonSerializable(typeof (long))]
[JsonSerializable(typeof (double))]
[JsonSerializable(typeof (List<Dictionary<string, object>>))]
[JsonSerializable(typeof (List<MegaCrit.Sts2.Core.Platform.Null.NullMultiplayerName>))]
[GeneratedCode("System.Text.Json.SourceGeneration", "9.0.12.31616")]
internal class MegaCritSerializerContext : JsonSerializerContext, IJsonTypeInfoResolver
{
  private JsonTypeInfo<bool>? _Boolean;
  private JsonTypeInfo<double>? _Double;
  private JsonTypeInfo<float>? _Single;
  private JsonTypeInfo<Godot.Vector2>? _Vector2;
  private JsonTypeInfo<Godot.Vector2I>? _Vector2I;
  private JsonTypeInfo<MegaCrit.Sts2.Core.ControllerInput.ControllerMappingType>? _ControllerMappingType;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Entities.Relics.RelicRarity>? _RelicRarity;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Entities.Rngs.PlayerRngType>? _PlayerRngType;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Entities.Rngs.RunRngType>? _RunRngType;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Localization.LocString>? _LocString;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Map.MapCoord>? _MapCoord;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Map.MapPointType>? _MapPointType;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Modding.ModDependency>? _ModDependency;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Modding.ModManifest>? _ModManifest;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Modding.ModSettings>? _ModSettings;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Modding.ModSource>? _ModSource;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Modding.SettingsSaveMod>? _SettingsSaveMod;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Models.Badges.BadgeRarity>? _BadgeRarity;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Models.ModelId>? _ModelId;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData>? _FeedbackData;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboard>? _NullLeaderboard;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFile>? _NullLeaderboardFile;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry>? _NullLeaderboardFileEntry;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Platform.Null.NullMultiplayerName>? _NullMultiplayerName;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Platform.PlatformType>? _PlatformType;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Rewards.RewardType>? _RewardType;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Rooms.RoomType>? _RoomType;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Runs.CardCreationSource>? _CardCreationSource;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Runs.CardRarityOddsType>? _CardRarityOddsType;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Runs.GameMode>? _GameMode;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry>? _AncientChoiceHistoryEntry;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Runs.History.CardChoiceHistoryEntry>? _CardChoiceHistoryEntry;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Runs.History.CardEnchantmentHistoryEntry>? _CardEnchantmentHistoryEntry;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Runs.History.CardTransformationHistoryEntry>? _CardTransformationHistoryEntry;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Runs.History.EventOptionHistoryEntry>? _EventOptionHistoryEntry;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>? _MapPointHistoryEntry;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry>? _MapPointRoomHistoryEntry;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry>? _ModelChoiceHistoryEntry;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry>? _PlayerMapPointHistoryEntry;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Runs.RunHistory>? _RunHistory;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Runs.RunHistoryPlayer>? _RunHistoryPlayer;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.AncientCharacterStats>? _AncientCharacterStats;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.AncientStats>? _AncientStats;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.BadgeStats>? _BadgeStats;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.CardStats>? _CardStats;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.CharacterStats>? _CharacterStats;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.EncounterStats>? _EncounterStats;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.EnemyStats>? _EnemyStats;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.EpochState>? _EpochState;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.FightStats>? _FightStats;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawingLine>? _SerializableMapDrawingLine;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawings>? _SerializableMapDrawings;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializablePlayerMapDrawings>? _SerializablePlayerMapDrawings;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Migrations.MigratingData>? _MigratingData;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.PrefsSave>? _PrefsSave;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.ProfileSave>? _ProfileSave;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties>? _SavedProperties;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<bool>>? _SavedPropertyBoolean;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Models.ModelId>>? _SavedPropertyModelId;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>>? _SavedPropertySerializableCardArray;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>? _SavedPropertySerializableCard;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int[]>>? _SavedPropertyInt32Array;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int>>? _SavedPropertyInt32;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<string>>? _SavedPropertyString;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap>? _SerializableActMap;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel>? _SerializableActModel;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableBadge>? _SerializableBadge;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>? _SerializableCard;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>? _SerializableCardArray;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableEnchantment>? _SerializableEnchantment;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableExtraRunFields>? _SerializableExtraRunFields;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint>? _SerializableMapPoint;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier>? _SerializableModifier;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer>? _SerializablePlayer;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayerOddsSet>? _SerializablePlayerOddsSet;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>? _SerializablePotion;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>? _SerializableRelic;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelicGrabBag>? _SerializableRelicGrabBag;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableReward>? _SerializableReward;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom>? _SerializableRoom;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet>? _SerializableRoomSet;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableRunOddsSet>? _SerializableRunOddsSet;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableRunRngSet>? _SerializableRunRngSet;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.SerializableEpoch>? _SerializableEpoch;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.SerializableExtraPlayerFields>? _SerializableExtraPlayerFields;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.SerializablePlayerRngSet>? _SerializablePlayerRngSet;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.SerializableProgress>? _SerializableProgress;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.SerializableRng>? _SerializableRng;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.SerializableRun>? _SerializableRun;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.SerializableUnlockedAchievement>? _SerializableUnlockedAchievement;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Saves.SettingsSave>? _SettingsSave;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Settings.AspectRatioSetting>? _AspectRatioSetting;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Settings.FastModeType>? _FastModeType;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Settings.VSyncType>? _VSyncType;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Unlocks.SerializableUnlockState>? _SerializableUnlockState;
  private JsonTypeInfo<Dictionary<MegaCrit.Sts2.Core.Entities.Relics.RelicRarity, List<MegaCrit.Sts2.Core.Models.ModelId>>>? _DictionaryRelicRarityListModelId;
  private JsonTypeInfo<Dictionary<MegaCrit.Sts2.Core.Entities.Rngs.PlayerRngType, MegaCrit.Sts2.Core.Saves.SerializableRng>>? _DictionaryPlayerRngTypeSerializableRng;
  private JsonTypeInfo<Dictionary<MegaCrit.Sts2.Core.Entities.Rngs.RunRngType, MegaCrit.Sts2.Core.Saves.SerializableRng>>? _DictionaryRunRngTypeSerializableRng;
  private JsonTypeInfo<Dictionary<string, object>>? _DictionaryStringObject;
  private JsonTypeInfo<Dictionary<string, string>>? _DictionaryStringString;
  private JsonTypeInfo<Dictionary<ulong, List<MegaCrit.Sts2.Core.Saves.Runs.SerializableReward>>>? _DictionaryUInt64ListSerializableReward;
  private JsonTypeInfo<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableBadge>>? _IEnumerableSerializableBadge;
  private JsonTypeInfo<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>? _IEnumerableSerializableCard;
  private JsonTypeInfo<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>>? _IEnumerableSerializablePotion;
  private JsonTypeInfo<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>>? _IEnumerableSerializableRelic;
  private JsonTypeInfo<List<Godot.Vector2>>? _ListVector2;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Map.MapCoord>>? _ListMapCoord;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Modding.ModDependency>>? _ListModDependency;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Modding.SettingsSaveMod>>? _ListSettingsSaveMod;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Models.ModelId>>? _ListModelId;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboard>>? _ListNullLeaderboard;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry>>? _ListNullLeaderboardFileEntry;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Platform.Null.NullMultiplayerName>>? _ListNullMultiplayerName;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry>>? _ListAncientChoiceHistoryEntry;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.History.CardChoiceHistoryEntry>>? _ListCardChoiceHistoryEntry;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.History.CardEnchantmentHistoryEntry>>? _ListCardEnchantmentHistoryEntry;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.History.CardTransformationHistoryEntry>>? _ListCardTransformationHistoryEntry;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.History.EventOptionHistoryEntry>>? _ListEventOptionHistoryEntry;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>>? _ListMapPointHistoryEntry;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry>>? _ListMapPointRoomHistoryEntry;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry>>? _ListModelChoiceHistoryEntry;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry>>? _ListPlayerMapPointHistoryEntry;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.RunHistoryPlayer>>? _ListRunHistoryPlayer;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.AncientCharacterStats>>? _ListAncientCharacterStats;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.AncientStats>>? _ListAncientStats;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.BadgeStats>>? _ListBadgeStats;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.CardStats>>? _ListCardStats;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.CharacterStats>>? _ListCharacterStats;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.EncounterStats>>? _ListEncounterStats;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.EnemyStats>>? _ListEnemyStats;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.FightStats>>? _ListFightStats;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawingLine>>? _ListSerializableMapDrawingLine;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializablePlayerMapDrawings>>? _ListSerializablePlayerMapDrawings;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Migrations.MigratingData>>? _ListMigratingData;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<bool>>>? _ListSavedPropertyBoolean;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Models.ModelId>>>? _ListSavedPropertyModelId;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>>>? _ListSavedPropertySerializableCardArray;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>>? _ListSavedPropertySerializableCard;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int[]>>>? _ListSavedPropertyInt32Array;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int>>>? _ListSavedPropertyInt32;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<string>>>? _ListSavedPropertyString;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel>>? _ListSerializableActModel;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>? _ListSerializableCard;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint>>? _ListSerializableMapPoint;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier>>? _ListSerializableModifier;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer>>? _ListSerializablePlayer;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>>? _ListSerializablePotion;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>>? _ListSerializableRelic;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableReward>>? _ListSerializableReward;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.SerializableEpoch>>? _ListSerializableEpoch;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.SerializableUnlockedAchievement>>? _ListSerializableUnlockedAchievement;
  private JsonTypeInfo<List<Dictionary<string, object>>>? _ListDictionaryStringObject;
  private JsonTypeInfo<List<List<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>>>? _ListListMapPointHistoryEntry;
  private JsonTypeInfo<List<List<MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry>>>? _ListListPlayerMapPointHistoryEntry;
  private JsonTypeInfo<List<System.Text.Json.Nodes.JsonNode>>? _ListJsonNode;
  private JsonTypeInfo<List<string>>? _ListString;
  private JsonTypeInfo<List<ulong>>? _ListUInt64;
  private JsonTypeInfo<System.DateTimeOffset>? _DateTimeOffset;
  private JsonTypeInfo<System.DateTimeOffset?>? _NullableDateTimeOffset;
  private JsonTypeInfo<System.Text.Json.Nodes.JsonNode>? _JsonNode;
  private JsonTypeInfo<System.Text.Json.Nodes.JsonObject>? _JsonObject;
  private JsonTypeInfo<int>? _Int32;
  private JsonTypeInfo<int?>? _NullableInt32;
  private JsonTypeInfo<int[]>? _Int32Array;
  private JsonTypeInfo<long>? _Int64;
  private JsonTypeInfo<object>? _Object;
  private JsonTypeInfo<string>? _String;
  private JsonTypeInfo<ulong>? _UInt64;
  private static readonly JsonSerializerOptions s_defaultOptions;
  private const BindingFlags InstanceMemberBindingFlags = (BindingFlags) 52;

  public static JsonSerializerOptions DefaultGeneratedSerializerOptions
  {
    get => ((JsonSerializerContext) MegaCritSerializerContext.Default).GeneratedSerializerOptions;
  }

  public 
  #nullable disable
  JsonTypeInfo<bool> Boolean
  {
    get
    {
      return this._Boolean ?? (this._Boolean = (JsonTypeInfo<bool>) this.Options.GetTypeInfo(typeof (bool)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<bool> Create_Boolean(JsonSerializerOptions options)
  {
    JsonTypeInfo<bool> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<bool>(options, out jsonTypeInfo))
      jsonTypeInfo = JsonMetadataServices.CreateValueInfo<bool>(options, (JsonConverter) JsonMetadataServices.BooleanConverter);
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<double> Double
  {
    get
    {
      return this._Double ?? (this._Double = (JsonTypeInfo<double>) this.Options.GetTypeInfo(typeof (double)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<double> Create_Double(JsonSerializerOptions options)
  {
    JsonTypeInfo<double> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<double>(options, out jsonTypeInfo))
      jsonTypeInfo = JsonMetadataServices.CreateValueInfo<double>(options, (JsonConverter) JsonMetadataServices.DoubleConverter);
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<float> Single
  {
    get
    {
      return this._Single ?? (this._Single = (JsonTypeInfo<float>) this.Options.GetTypeInfo(typeof (float)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<float> Create_Single(JsonSerializerOptions options)
  {
    JsonTypeInfo<float> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<float>(options, out jsonTypeInfo))
      jsonTypeInfo = JsonMetadataServices.CreateValueInfo<float>(options, (JsonConverter) JsonMetadataServices.SingleConverter);
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<Godot.Vector2> Vector2
  {
    get
    {
      return this._Vector2 ?? (this._Vector2 = (JsonTypeInfo<Godot.Vector2>) this.Options.GetTypeInfo(typeof (Godot.Vector2)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<Godot.Vector2> Create_Vector2(JsonSerializerOptions options)
  {
    JsonTypeInfo<Godot.Vector2> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<Godot.Vector2>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<Godot.Vector2> objectInfoValues = new JsonObjectInfoValues<Godot.Vector2>()
      {
        ObjectCreator = (Func<Godot.Vector2>) (() => new Godot.Vector2()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], Godot.Vector2>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.Vector2PropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (Godot.Vector2).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, Godot.Vector2>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<Godot.Vector2>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] Vector2PropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[2];
    JsonPropertyInfoValues<float> propertyInfoValues1 = new JsonPropertyInfoValues<float>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (Godot.Vector2),
      Converter = (JsonConverter<float>) null,
      Getter = (Func<object, float>) (obj => ((Godot.Vector2) obj).X),
      Setter = (Action<object, float>) ((obj, value) => Unsafe.Unbox<Godot.Vector2>(obj).X = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "X",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (Godot.Vector2).GetField("X", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<float>(options, propertyInfoValues1);
    JsonPropertyInfoValues<float> propertyInfoValues2 = new JsonPropertyInfoValues<float>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (Godot.Vector2),
      Converter = (JsonConverter<float>) null,
      Getter = (Func<object, float>) (obj => ((Godot.Vector2) obj).Y),
      Setter = (Action<object, float>) ((obj, value) => Unsafe.Unbox<Godot.Vector2>(obj).Y = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Y",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (Godot.Vector2).GetField("Y", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<float>(options, propertyInfoValues2);
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<Godot.Vector2I> Vector2I
  {
    get
    {
      return this._Vector2I ?? (this._Vector2I = (JsonTypeInfo<Godot.Vector2I>) this.Options.GetTypeInfo(typeof (Godot.Vector2I)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<Godot.Vector2I> Create_Vector2I(JsonSerializerOptions options)
  {
    JsonTypeInfo<Godot.Vector2I> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<Godot.Vector2I>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<Godot.Vector2I> objectInfoValues = new JsonObjectInfoValues<Godot.Vector2I>()
      {
        ObjectCreator = (Func<Godot.Vector2I>) (() => new Godot.Vector2I()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], Godot.Vector2I>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.Vector2IPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (Godot.Vector2I).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, Godot.Vector2I>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<Godot.Vector2I>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] Vector2IPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[2];
    JsonPropertyInfoValues<int> propertyInfoValues1 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (Godot.Vector2I),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((Godot.Vector2I) obj).X),
      Setter = (Action<object, int>) ((obj, value) => Unsafe.Unbox<Godot.Vector2I>(obj).X = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "X",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (Godot.Vector2I).GetField("X", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues1);
    JsonPropertyInfoValues<int> propertyInfoValues2 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (Godot.Vector2I),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((Godot.Vector2I) obj).Y),
      Setter = (Action<object, int>) ((obj, value) => Unsafe.Unbox<Godot.Vector2I>(obj).Y = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Y",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (Godot.Vector2I).GetField("Y", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues2);
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.ControllerInput.ControllerMappingType> ControllerMappingType
  {
    get
    {
      return this._ControllerMappingType ?? (this._ControllerMappingType = (JsonTypeInfo<MegaCrit.Sts2.Core.ControllerInput.ControllerMappingType>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.ControllerInput.ControllerMappingType)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.ControllerInput.ControllerMappingType> Create_ControllerMappingType(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.ControllerInput.ControllerMappingType> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.ControllerInput.ControllerMappingType>(options, out jsonTypeInfo))
      jsonTypeInfo = JsonMetadataServices.CreateValueInfo<MegaCrit.Sts2.Core.ControllerInput.ControllerMappingType>(options, (JsonConverter) JsonMetadataServices.GetEnumConverter<MegaCrit.Sts2.Core.ControllerInput.ControllerMappingType>(options));
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Entities.Relics.RelicRarity> RelicRarity
  {
    get
    {
      return this._RelicRarity ?? (this._RelicRarity = (JsonTypeInfo<MegaCrit.Sts2.Core.Entities.Relics.RelicRarity>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Entities.Relics.RelicRarity)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Entities.Relics.RelicRarity> Create_RelicRarity(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Entities.Relics.RelicRarity> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Entities.Relics.RelicRarity>(options, out jsonTypeInfo))
      jsonTypeInfo = JsonMetadataServices.CreateValueInfo<MegaCrit.Sts2.Core.Entities.Relics.RelicRarity>(options, (JsonConverter) JsonMetadataServices.GetEnumConverter<MegaCrit.Sts2.Core.Entities.Relics.RelicRarity>(options));
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Entities.Rngs.PlayerRngType> PlayerRngType
  {
    get
    {
      return this._PlayerRngType ?? (this._PlayerRngType = (JsonTypeInfo<MegaCrit.Sts2.Core.Entities.Rngs.PlayerRngType>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Entities.Rngs.PlayerRngType)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Entities.Rngs.PlayerRngType> Create_PlayerRngType(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Entities.Rngs.PlayerRngType> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Entities.Rngs.PlayerRngType>(options, out jsonTypeInfo))
      jsonTypeInfo = JsonMetadataServices.CreateValueInfo<MegaCrit.Sts2.Core.Entities.Rngs.PlayerRngType>(options, (JsonConverter) JsonMetadataServices.GetEnumConverter<MegaCrit.Sts2.Core.Entities.Rngs.PlayerRngType>(options));
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Entities.Rngs.RunRngType> RunRngType
  {
    get
    {
      return this._RunRngType ?? (this._RunRngType = (JsonTypeInfo<MegaCrit.Sts2.Core.Entities.Rngs.RunRngType>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Entities.Rngs.RunRngType)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Entities.Rngs.RunRngType> Create_RunRngType(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Entities.Rngs.RunRngType> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Entities.Rngs.RunRngType>(options, out jsonTypeInfo))
      jsonTypeInfo = JsonMetadataServices.CreateValueInfo<MegaCrit.Sts2.Core.Entities.Rngs.RunRngType>(options, (JsonConverter) JsonMetadataServices.GetEnumConverter<MegaCrit.Sts2.Core.Entities.Rngs.RunRngType>(options));
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Localization.LocString> LocString
  {
    get
    {
      return this._LocString ?? (this._LocString = (JsonTypeInfo<MegaCrit.Sts2.Core.Localization.LocString>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Localization.LocString)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Localization.LocString> Create_LocString(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Localization.LocString> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Localization.LocString>(options, out jsonTypeInfo))
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Localization.LocString> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Localization.LocString>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Localization.LocString>) null,
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Localization.LocString>) (args => new MegaCrit.Sts2.Core.Localization.LocString((string) args[0], (string) args[1])),
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.LocStringPropInit(options)),
        ConstructorParameterMetadataInitializer = MegaCritSerializerContext.\u003C\u003EO.\u003C0\u003E__LocStringCtorParamInit ?? (MegaCritSerializerContext.\u003C\u003EO.\u003C0\u003E__LocStringCtorParamInit = new Func<JsonParameterInfoValues[]>(MegaCritSerializerContext.LocStringCtorParamInit)),
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Localization.LocString).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, new Type[2]
        {
          typeof (string),
          typeof (string)
        }, (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Localization.LocString>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Localization.LocString>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] LocStringPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[4];
    JsonPropertyInfoValues<string> propertyInfoValues1 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Localization.LocString),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Localization.LocString) obj).LocTable),
      Setter = (Action<object, string>) null,
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "LocTable",
      JsonPropertyName = "table",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Localization.LocString).GetProperty("LocTable", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues1);
    jsonPropertyInfoArray[0].IsGetNullable = false;
    JsonPropertyInfoValues<string> propertyInfoValues2 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Localization.LocString),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Localization.LocString) obj).LocEntryKey),
      Setter = (Action<object, string>) null,
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "LocEntryKey",
      JsonPropertyName = "key",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Localization.LocString).GetProperty("LocEntryKey", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues2);
    jsonPropertyInfoArray[1].IsGetNullable = false;
    JsonPropertyInfoValues<bool> propertyInfoValues3 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Localization.LocString),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) null,
      Setter = (Action<object, bool>) null,
      IgnoreCondition = new JsonIgnoreCondition?((JsonIgnoreCondition) 1),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "IsEmpty",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Localization.LocString).GetProperty("IsEmpty", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (bool), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues3);
    JsonPropertyInfoValues<IReadOnlyDictionary<string, object>> propertyInfoValues4 = new JsonPropertyInfoValues<IReadOnlyDictionary<string, object>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Localization.LocString),
      Converter = (JsonConverter<IReadOnlyDictionary<string, object>>) null,
      Getter = (Func<object, IReadOnlyDictionary<string, object>>) null,
      Setter = (Action<object, IReadOnlyDictionary<string, object>>) null,
      IgnoreCondition = new JsonIgnoreCondition?((JsonIgnoreCondition) 1),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Variables",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Localization.LocString).GetProperty("Variables", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (IReadOnlyDictionary<string, object>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[3] = JsonMetadataServices.CreatePropertyInfo<IReadOnlyDictionary<string, object>>(options, propertyInfoValues4);
    jsonPropertyInfoArray[3].IsGetNullable = false;
    return jsonPropertyInfoArray;
  }

  private static JsonParameterInfoValues[] LocStringCtorParamInit()
  {
    return new JsonParameterInfoValues[2]
    {
      new JsonParameterInfoValues()
      {
        Name = "locTable",
        ParameterType = typeof (string),
        Position = 0,
        HasDefaultValue = false,
        DefaultValue = (object) null,
        IsNullable = false
      },
      new JsonParameterInfoValues()
      {
        Name = "locEntryKey",
        ParameterType = typeof (string),
        Position = 1,
        HasDefaultValue = false,
        DefaultValue = (object) null,
        IsNullable = false
      }
    };
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Map.MapCoord> MapCoord
  {
    get
    {
      return this._MapCoord ?? (this._MapCoord = (JsonTypeInfo<MegaCrit.Sts2.Core.Map.MapCoord>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Map.MapCoord)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Map.MapCoord> Create_MapCoord(JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Map.MapCoord> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Map.MapCoord>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Map.MapCoord> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Map.MapCoord>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Map.MapCoord>) (() => new MegaCrit.Sts2.Core.Map.MapCoord()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Map.MapCoord>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.MapCoordPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Map.MapCoord).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Map.MapCoord>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Map.MapCoord>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] MapCoordPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[2];
    JsonPropertyInfoValues<int> propertyInfoValues1 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Map.MapCoord),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Map.MapCoord) obj).col),
      Setter = (Action<object, int>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Map.MapCoord>(obj).col = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = true,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "col",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Map.MapCoord).GetField("col", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues1);
    JsonPropertyInfoValues<int> propertyInfoValues2 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Map.MapCoord),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Map.MapCoord) obj).row),
      Setter = (Action<object, int>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Map.MapCoord>(obj).row = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = true,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "row",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Map.MapCoord).GetField("row", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues2);
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Map.MapPointType> MapPointType
  {
    get
    {
      return this._MapPointType ?? (this._MapPointType = (JsonTypeInfo<MegaCrit.Sts2.Core.Map.MapPointType>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Map.MapPointType)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Map.MapPointType> Create_MapPointType(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Map.MapPointType> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Map.MapPointType>(options, out jsonTypeInfo))
      jsonTypeInfo = JsonMetadataServices.CreateValueInfo<MegaCrit.Sts2.Core.Map.MapPointType>(options, (JsonConverter) JsonMetadataServices.GetEnumConverter<MegaCrit.Sts2.Core.Map.MapPointType>(options));
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Modding.ModDependency> ModDependency
  {
    get
    {
      return this._ModDependency ?? (this._ModDependency = (JsonTypeInfo<MegaCrit.Sts2.Core.Modding.ModDependency>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Modding.ModDependency)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Modding.ModDependency> Create_ModDependency(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Modding.ModDependency> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Modding.ModDependency>(options, out jsonTypeInfo))
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Modding.ModDependency> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Modding.ModDependency>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Modding.ModDependency>) null,
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Modding.ModDependency>) (args => new MegaCrit.Sts2.Core.Modding.ModDependency((string) args[0], (string) args[1])),
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.ModDependencyPropInit(options)),
        ConstructorParameterMetadataInitializer = MegaCritSerializerContext.\u003C\u003EO.\u003C1\u003E__ModDependencyCtorParamInit ?? (MegaCritSerializerContext.\u003C\u003EO.\u003C1\u003E__ModDependencyCtorParamInit = new Func<JsonParameterInfoValues[]>(MegaCritSerializerContext.ModDependencyCtorParamInit)),
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Modding.ModDependency).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, new Type[2]
        {
          typeof (string),
          typeof (string)
        }, (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Modding.ModDependency>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Modding.ModDependency>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] ModDependencyPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[2];
    JsonPropertyInfoValues<string> propertyInfoValues1 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Modding.ModDependency),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Modding.ModDependency) obj).id),
      Setter = (Action<object, string>) ((obj, value) => ((MegaCrit.Sts2.Core.Modding.ModDependency) obj).id = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "id",
      JsonPropertyName = "id",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Modding.ModDependency).GetField("id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues1);
    jsonPropertyInfoArray[0].IsGetNullable = false;
    jsonPropertyInfoArray[0].IsSetNullable = false;
    JsonPropertyInfoValues<string> propertyInfoValues2 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Modding.ModDependency),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Modding.ModDependency) obj).minVersion),
      Setter = (Action<object, string>) ((obj, value) => ((MegaCrit.Sts2.Core.Modding.ModDependency) obj).minVersion = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "minVersion",
      JsonPropertyName = "min_version",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Modding.ModDependency).GetField("minVersion", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues2);
    return jsonPropertyInfoArray;
  }

  private static JsonParameterInfoValues[] ModDependencyCtorParamInit()
  {
    return new JsonParameterInfoValues[2]
    {
      new JsonParameterInfoValues()
      {
        Name = "id",
        ParameterType = typeof (string),
        Position = 0,
        HasDefaultValue = false,
        DefaultValue = (object) null,
        IsNullable = false
      },
      new JsonParameterInfoValues()
      {
        Name = "minVersion",
        ParameterType = typeof (string),
        Position = 1,
        HasDefaultValue = true,
        DefaultValue = (object) null,
        IsNullable = true
      }
    };
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Modding.ModManifest> ModManifest
  {
    get
    {
      return this._ModManifest ?? (this._ModManifest = (JsonTypeInfo<MegaCrit.Sts2.Core.Modding.ModManifest>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Modding.ModManifest)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Modding.ModManifest> Create_ModManifest(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Modding.ModManifest> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Modding.ModManifest>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Modding.ModManifest> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Modding.ModManifest>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Modding.ModManifest>) (() => new MegaCrit.Sts2.Core.Modding.ModManifest()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Modding.ModManifest>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.ModManifestPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Modding.ModManifest).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Modding.ModManifest>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Modding.ModManifest>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] ModManifestPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[10];
    JsonPropertyInfoValues<string> propertyInfoValues1 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Modding.ModManifest),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Modding.ModManifest) obj).id),
      Setter = (Action<object, string>) ((obj, value) => ((MegaCrit.Sts2.Core.Modding.ModManifest) obj).id = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "id",
      JsonPropertyName = "id",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Modding.ModManifest).GetField("id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues1);
    JsonPropertyInfoValues<string> propertyInfoValues2 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Modding.ModManifest),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Modding.ModManifest) obj).name),
      Setter = (Action<object, string>) ((obj, value) => ((MegaCrit.Sts2.Core.Modding.ModManifest) obj).name = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "name",
      JsonPropertyName = "name",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Modding.ModManifest).GetField("name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues2);
    JsonPropertyInfoValues<string> propertyInfoValues3 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Modding.ModManifest),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Modding.ModManifest) obj).author),
      Setter = (Action<object, string>) ((obj, value) => ((MegaCrit.Sts2.Core.Modding.ModManifest) obj).author = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "author",
      JsonPropertyName = "author",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Modding.ModManifest).GetField("author", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues3);
    JsonPropertyInfoValues<string> propertyInfoValues4 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Modding.ModManifest),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Modding.ModManifest) obj).description),
      Setter = (Action<object, string>) ((obj, value) => ((MegaCrit.Sts2.Core.Modding.ModManifest) obj).description = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "description",
      JsonPropertyName = "description",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Modding.ModManifest).GetField("description", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[3] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues4);
    JsonPropertyInfoValues<string> propertyInfoValues5 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Modding.ModManifest),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Modding.ModManifest) obj).version),
      Setter = (Action<object, string>) ((obj, value) => ((MegaCrit.Sts2.Core.Modding.ModManifest) obj).version = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "version",
      JsonPropertyName = "version",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Modding.ModManifest).GetField("version", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[4] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues5);
    JsonPropertyInfoValues<bool> propertyInfoValues6 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Modding.ModManifest),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) (obj => ((MegaCrit.Sts2.Core.Modding.ModManifest) obj).hasPck),
      Setter = (Action<object, bool>) ((obj, value) => ((MegaCrit.Sts2.Core.Modding.ModManifest) obj).hasPck = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "hasPck",
      JsonPropertyName = "has_pck",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Modding.ModManifest).GetField("hasPck", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[5] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues6);
    JsonPropertyInfoValues<bool> propertyInfoValues7 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Modding.ModManifest),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) (obj => ((MegaCrit.Sts2.Core.Modding.ModManifest) obj).hasDll),
      Setter = (Action<object, bool>) ((obj, value) => ((MegaCrit.Sts2.Core.Modding.ModManifest) obj).hasDll = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "hasDll",
      JsonPropertyName = "has_dll",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Modding.ModManifest).GetField("hasDll", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[6] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues7);
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Modding.ModDependency>> propertyInfoValues8 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Modding.ModDependency>>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Modding.ModManifest),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Modding.ModDependency>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Modding.ModDependency>>) (obj => ((MegaCrit.Sts2.Core.Modding.ModManifest) obj).dependencies),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Modding.ModDependency>>) ((obj, value) => ((MegaCrit.Sts2.Core.Modding.ModManifest) obj).dependencies = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "dependencies",
      JsonPropertyName = "dependencies",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Modding.ModManifest).GetField("dependencies", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[7] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Modding.ModDependency>>(options, propertyInfoValues8);
    JsonPropertyInfoValues<bool> propertyInfoValues9 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Modding.ModManifest),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) (obj => ((MegaCrit.Sts2.Core.Modding.ModManifest) obj).affectsGameplay),
      Setter = (Action<object, bool>) ((obj, value) => ((MegaCrit.Sts2.Core.Modding.ModManifest) obj).affectsGameplay = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "affectsGameplay",
      JsonPropertyName = "affects_gameplay",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Modding.ModManifest).GetField("affectsGameplay", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[8] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues9);
    JsonPropertyInfoValues<string> propertyInfoValues10 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Modding.ModManifest),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Modding.ModManifest) obj).minGameVersion),
      Setter = (Action<object, string>) ((obj, value) => ((MegaCrit.Sts2.Core.Modding.ModManifest) obj).minGameVersion = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "minGameVersion",
      JsonPropertyName = "min_game_version",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Modding.ModManifest).GetField("minGameVersion", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[9] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues10);
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Modding.ModSettings> ModSettings
  {
    get
    {
      return this._ModSettings ?? (this._ModSettings = (JsonTypeInfo<MegaCrit.Sts2.Core.Modding.ModSettings>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Modding.ModSettings)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Modding.ModSettings> Create_ModSettings(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Modding.ModSettings> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Modding.ModSettings>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Modding.ModSettings> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Modding.ModSettings>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Modding.ModSettings>) (() => new MegaCrit.Sts2.Core.Modding.ModSettings()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Modding.ModSettings>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.ModSettingsPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Modding.ModSettings).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Modding.ModSettings>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Modding.ModSettings>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] ModSettingsPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[2];
    JsonPropertyInfoValues<bool> propertyInfoValues1 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Modding.ModSettings),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) (obj => ((MegaCrit.Sts2.Core.Modding.ModSettings) obj).PlayerAgreedToModLoading),
      Setter = (Action<object, bool>) ((obj, value) => ((MegaCrit.Sts2.Core.Modding.ModSettings) obj).PlayerAgreedToModLoading = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "PlayerAgreedToModLoading",
      JsonPropertyName = "mods_enabled",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Modding.ModSettings).GetProperty("PlayerAgreedToModLoading", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (bool), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues1);
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Modding.SettingsSaveMod>> propertyInfoValues2 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Modding.SettingsSaveMod>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Modding.ModSettings),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Modding.SettingsSaveMod>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Modding.SettingsSaveMod>>) (obj => ((MegaCrit.Sts2.Core.Modding.ModSettings) obj).ModList),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Modding.SettingsSaveMod>>) ((obj, value) => ((MegaCrit.Sts2.Core.Modding.ModSettings) obj).ModList = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "ModList",
      JsonPropertyName = "mod_list",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Modding.ModSettings).GetProperty("ModList", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Modding.SettingsSaveMod>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Modding.SettingsSaveMod>>(options, propertyInfoValues2);
    jsonPropertyInfoArray[1].IsGetNullable = false;
    jsonPropertyInfoArray[1].IsSetNullable = false;
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Modding.ModSource> ModSource
  {
    get
    {
      return this._ModSource ?? (this._ModSource = (JsonTypeInfo<MegaCrit.Sts2.Core.Modding.ModSource>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Modding.ModSource)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Modding.ModSource> Create_ModSource(JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Modding.ModSource> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Modding.ModSource>(options, out jsonTypeInfo))
      jsonTypeInfo = JsonMetadataServices.CreateValueInfo<MegaCrit.Sts2.Core.Modding.ModSource>(options, (JsonConverter) JsonMetadataServices.GetEnumConverter<MegaCrit.Sts2.Core.Modding.ModSource>(options));
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Modding.SettingsSaveMod> SettingsSaveMod
  {
    get
    {
      return this._SettingsSaveMod ?? (this._SettingsSaveMod = (JsonTypeInfo<MegaCrit.Sts2.Core.Modding.SettingsSaveMod>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Modding.SettingsSaveMod)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Modding.SettingsSaveMod> Create_SettingsSaveMod(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Modding.SettingsSaveMod> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Modding.SettingsSaveMod>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Modding.SettingsSaveMod> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Modding.SettingsSaveMod>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Modding.SettingsSaveMod>) (() => new MegaCrit.Sts2.Core.Modding.SettingsSaveMod()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Modding.SettingsSaveMod>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.SettingsSaveModPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Modding.SettingsSaveMod).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Modding.SettingsSaveMod>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Modding.SettingsSaveMod>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] SettingsSaveModPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[3];
    JsonPropertyInfoValues<string> propertyInfoValues1 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Modding.SettingsSaveMod),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Modding.SettingsSaveMod) obj).Id),
      Setter = (Action<object, string>) ((obj, value) => ((MegaCrit.Sts2.Core.Modding.SettingsSaveMod) obj).Id = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Id",
      JsonPropertyName = "id",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Modding.SettingsSaveMod).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues1);
    jsonPropertyInfoArray[0].IsGetNullable = false;
    jsonPropertyInfoArray[0].IsSetNullable = false;
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Modding.ModSource> propertyInfoValues2 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Modding.ModSource>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Modding.SettingsSaveMod),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Modding.ModSource>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Modding.ModSource>) (obj => ((MegaCrit.Sts2.Core.Modding.SettingsSaveMod) obj).Source),
      Setter = (Action<object, MegaCrit.Sts2.Core.Modding.ModSource>) ((obj, value) => ((MegaCrit.Sts2.Core.Modding.SettingsSaveMod) obj).Source = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Source",
      JsonPropertyName = "source",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Modding.SettingsSaveMod).GetProperty("Source", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Modding.ModSource), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Modding.ModSource>(options, propertyInfoValues2);
    JsonPropertyInfoValues<bool> propertyInfoValues3 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Modding.SettingsSaveMod),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) (obj => ((MegaCrit.Sts2.Core.Modding.SettingsSaveMod) obj).IsEnabled),
      Setter = (Action<object, bool>) ((obj, value) => ((MegaCrit.Sts2.Core.Modding.SettingsSaveMod) obj).IsEnabled = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "IsEnabled",
      JsonPropertyName = "is_enabled",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Modding.SettingsSaveMod).GetProperty("IsEnabled", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (bool), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues3);
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Models.Badges.BadgeRarity> BadgeRarity
  {
    get
    {
      return this._BadgeRarity ?? (this._BadgeRarity = (JsonTypeInfo<MegaCrit.Sts2.Core.Models.Badges.BadgeRarity>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Models.Badges.BadgeRarity)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Models.Badges.BadgeRarity> Create_BadgeRarity(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Models.Badges.BadgeRarity> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Models.Badges.BadgeRarity>(options, out jsonTypeInfo))
      jsonTypeInfo = JsonMetadataServices.CreateValueInfo<MegaCrit.Sts2.Core.Models.Badges.BadgeRarity>(options, (JsonConverter) JsonMetadataServices.GetEnumConverter<MegaCrit.Sts2.Core.Models.Badges.BadgeRarity>(options));
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Models.ModelId> ModelId
  {
    get
    {
      return this._ModelId ?? (this._ModelId = (JsonTypeInfo<MegaCrit.Sts2.Core.Models.ModelId>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Models.ModelId)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Models.ModelId> Create_ModelId(JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Models.ModelId> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Models.ModelId>(options, out jsonTypeInfo))
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Models.ModelId> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Models.ModelId>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Models.ModelId>) null,
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Models.ModelId>) (args => new MegaCrit.Sts2.Core.Models.ModelId((string) args[0], (string) args[1])),
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.ModelIdPropInit(options)),
        ConstructorParameterMetadataInitializer = MegaCritSerializerContext.\u003C\u003EO.\u003C2\u003E__ModelIdCtorParamInit ?? (MegaCritSerializerContext.\u003C\u003EO.\u003C2\u003E__ModelIdCtorParamInit = new Func<JsonParameterInfoValues[]>(MegaCritSerializerContext.ModelIdCtorParamInit)),
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Models.ModelId).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, new Type[2]
        {
          typeof (string),
          typeof (string)
        }, (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Models.ModelId>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Models.ModelId>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] ModelIdPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[2];
    JsonPropertyInfoValues<string> propertyInfoValues1 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Models.ModelId),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Models.ModelId) obj).Category),
      Setter = (Action<object, string>) null,
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Category",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Models.ModelId).GetProperty("Category", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues1);
    jsonPropertyInfoArray[0].IsGetNullable = false;
    JsonPropertyInfoValues<string> propertyInfoValues2 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Models.ModelId),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Models.ModelId) obj).Entry),
      Setter = (Action<object, string>) null,
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Entry",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Models.ModelId).GetProperty("Entry", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues2);
    jsonPropertyInfoArray[1].IsGetNullable = false;
    return jsonPropertyInfoArray;
  }

  private static JsonParameterInfoValues[] ModelIdCtorParamInit()
  {
    return new JsonParameterInfoValues[2]
    {
      new JsonParameterInfoValues()
      {
        Name = "category",
        ParameterType = typeof (string),
        Position = 0,
        HasDefaultValue = false,
        DefaultValue = (object) null,
        IsNullable = false
      },
      new JsonParameterInfoValues()
      {
        Name = "entry",
        ParameterType = typeof (string),
        Position = 1,
        HasDefaultValue = false,
        DefaultValue = (object) null,
        IsNullable = false
      }
    };
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData> FeedbackData
  {
    get
    {
      return this._FeedbackData ?? (this._FeedbackData = (JsonTypeInfo<MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData> Create_FeedbackData(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData>) (() => new MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.FeedbackDataPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] FeedbackDataPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[10];
    JsonPropertyInfoValues<string> propertyInfoValues1 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData) obj).description),
      Setter = (Action<object, string>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData>(obj).description = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "description",
      JsonPropertyName = "description",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData).GetField("description", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues1);
    jsonPropertyInfoArray[0].IsGetNullable = false;
    jsonPropertyInfoArray[0].IsSetNullable = false;
    JsonPropertyInfoValues<string> propertyInfoValues2 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData) obj).category),
      Setter = (Action<object, string>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData>(obj).category = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "category",
      JsonPropertyName = "category",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData).GetField("category", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues2);
    jsonPropertyInfoArray[1].IsGetNullable = false;
    jsonPropertyInfoArray[1].IsSetNullable = false;
    JsonPropertyInfoValues<string> propertyInfoValues3 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData) obj).gameVersion),
      Setter = (Action<object, string>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData>(obj).gameVersion = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "gameVersion",
      JsonPropertyName = "game_version",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData).GetField("gameVersion", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues3);
    jsonPropertyInfoArray[2].IsGetNullable = false;
    jsonPropertyInfoArray[2].IsSetNullable = false;
    JsonPropertyInfoValues<string> propertyInfoValues4 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData) obj).uniqueId),
      Setter = (Action<object, string>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData>(obj).uniqueId = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "uniqueId",
      JsonPropertyName = "unique_id",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData).GetField("uniqueId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[3] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues4);
    jsonPropertyInfoArray[3].IsGetNullable = false;
    jsonPropertyInfoArray[3].IsSetNullable = false;
    JsonPropertyInfoValues<string> propertyInfoValues5 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData) obj).commit),
      Setter = (Action<object, string>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData>(obj).commit = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "commit",
      JsonPropertyName = "commit",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData).GetField("commit", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[4] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues5);
    jsonPropertyInfoArray[4].IsGetNullable = false;
    jsonPropertyInfoArray[4].IsSetNullable = false;
    JsonPropertyInfoValues<string> propertyInfoValues6 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData) obj).platformBranch),
      Setter = (Action<object, string>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData>(obj).platformBranch = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "platformBranch",
      JsonPropertyName = "platform_branch",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData).GetField("platformBranch", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[5] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues6);
    JsonPropertyInfoValues<string> propertyInfoValues7 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData) obj).sessionId),
      Setter = (Action<object, string>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData>(obj).sessionId = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "sessionId",
      JsonPropertyName = "session_id",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData).GetField("sessionId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[6] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues7);
    jsonPropertyInfoArray[6].IsGetNullable = false;
    jsonPropertyInfoArray[6].IsSetNullable = false;
    JsonPropertyInfoValues<bool> propertyInfoValues8 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) (obj => ((MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData) obj).isModded),
      Setter = (Action<object, bool>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData>(obj).isModded = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "isModded",
      JsonPropertyName = "is_modded",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData).GetField("isModded", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[7] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues8);
    JsonPropertyInfoValues<bool> propertyInfoValues9 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) (obj => ((MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData) obj).isFullConsole),
      Setter = (Action<object, bool>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData>(obj).isFullConsole = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "isFullConsole",
      JsonPropertyName = "is_full_console",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData).GetField("isFullConsole", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[8] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues9);
    JsonPropertyInfoValues<string> propertyInfoValues10 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData) obj).lang),
      Setter = (Action<object, string>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData>(obj).lang = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "lang",
      JsonPropertyName = "lang",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData).GetField("lang", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[9] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues10);
    jsonPropertyInfoArray[9].IsGetNullable = false;
    jsonPropertyInfoArray[9].IsSetNullable = false;
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboard> NullLeaderboard
  {
    get
    {
      return this._NullLeaderboard ?? (this._NullLeaderboard = (JsonTypeInfo<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboard>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Platform.Null.NullLeaderboard)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboard> Create_NullLeaderboard(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboard> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboard>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboard> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboard>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboard>) (() => new MegaCrit.Sts2.Core.Platform.Null.NullLeaderboard()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Platform.Null.NullLeaderboard>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.NullLeaderboardPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Platform.Null.NullLeaderboard).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Platform.Null.NullLeaderboard>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboard>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] NullLeaderboardPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[2];
    JsonPropertyInfoValues<string> propertyInfoValues1 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Platform.Null.NullLeaderboard),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Platform.Null.NullLeaderboard) obj).name),
      Setter = (Action<object, string>) ((obj, value) => ((MegaCrit.Sts2.Core.Platform.Null.NullLeaderboard) obj).name = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "name",
      JsonPropertyName = "name",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Platform.Null.NullLeaderboard).GetField("name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues1);
    jsonPropertyInfoArray[0].IsGetNullable = false;
    jsonPropertyInfoArray[0].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry>> propertyInfoValues2 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry>>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Platform.Null.NullLeaderboard),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry>>) (obj => ((MegaCrit.Sts2.Core.Platform.Null.NullLeaderboard) obj).entries),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry>>) ((obj, value) => ((MegaCrit.Sts2.Core.Platform.Null.NullLeaderboard) obj).entries = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "entries",
      JsonPropertyName = "entries",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Platform.Null.NullLeaderboard).GetField("entries", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry>>(options, propertyInfoValues2);
    jsonPropertyInfoArray[1].IsGetNullable = false;
    jsonPropertyInfoArray[1].IsSetNullable = false;
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFile> NullLeaderboardFile
  {
    get
    {
      return this._NullLeaderboardFile ?? (this._NullLeaderboardFile = (JsonTypeInfo<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFile>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFile)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFile> Create_NullLeaderboardFile(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFile> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFile>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFile> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFile>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFile>) (() => new MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFile()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFile>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.NullLeaderboardFilePropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFile).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFile>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFile>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] NullLeaderboardFilePropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[2];
    JsonPropertyInfoValues<int> propertyInfoValues1 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFile),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFile) obj).SchemaVersion),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFile) obj).SchemaVersion = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "SchemaVersion",
      JsonPropertyName = "version",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFile).GetProperty("SchemaVersion", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues1);
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboard>> propertyInfoValues2 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboard>>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFile),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboard>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboard>>) (obj => ((MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFile) obj).leaderboards),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboard>>) ((obj, value) => ((MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFile) obj).leaderboards = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "leaderboards",
      JsonPropertyName = "leaderboards",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFile).GetField("leaderboards", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboard>>(options, propertyInfoValues2);
    jsonPropertyInfoArray[1].IsGetNullable = false;
    jsonPropertyInfoArray[1].IsSetNullable = false;
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry> NullLeaderboardFileEntry
  {
    get
    {
      return this._NullLeaderboardFileEntry ?? (this._NullLeaderboardFileEntry = (JsonTypeInfo<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry> Create_NullLeaderboardFileEntry(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry>(options, out jsonTypeInfo))
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry>) null,
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry>) (args => new MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry()
        {
          name = (string) args[0]
        }),
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.NullLeaderboardFileEntryPropInit(options)),
        ConstructorParameterMetadataInitializer = MegaCritSerializerContext.\u003C\u003EO.\u003C3\u003E__NullLeaderboardFileEntryCtorParamInit ?? (MegaCritSerializerContext.\u003C\u003EO.\u003C3\u003E__NullLeaderboardFileEntryCtorParamInit = new Func<JsonParameterInfoValues[]>(MegaCritSerializerContext.NullLeaderboardFileEntryCtorParamInit)),
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] NullLeaderboardFileEntryPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[4];
    JsonPropertyInfoValues<string> propertyInfoValues1 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry) obj).name),
      Setter = (Action<object, string>) ((obj, value) => ((MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry) obj).name = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "name",
      JsonPropertyName = "name",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry).GetField("name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues1);
    jsonPropertyInfoArray[0].IsRequired = true;
    jsonPropertyInfoArray[0].IsGetNullable = false;
    jsonPropertyInfoArray[0].IsSetNullable = false;
    JsonPropertyInfoValues<int> propertyInfoValues2 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry) obj).score),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry) obj).score = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "score",
      JsonPropertyName = "score",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry).GetField("score", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues2);
    JsonPropertyInfoValues<ulong> propertyInfoValues3 = new JsonPropertyInfoValues<ulong>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry),
      Converter = (JsonConverter<ulong>) null,
      Getter = (Func<object, ulong>) (obj => ((MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry) obj).id),
      Setter = (Action<object, ulong>) ((obj, value) => ((MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry) obj).id = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "id",
      JsonPropertyName = "id",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry).GetField("id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<ulong>(options, propertyInfoValues3);
    JsonPropertyInfoValues<List<ulong>> propertyInfoValues4 = new JsonPropertyInfoValues<List<ulong>>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry),
      Converter = (JsonConverter<List<ulong>>) null,
      Getter = (Func<object, List<ulong>>) (obj => ((MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry) obj).userIds),
      Setter = (Action<object, List<ulong>>) ((obj, value) => ((MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry) obj).userIds = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "userIds",
      JsonPropertyName = "other_ids",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry).GetField("userIds", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[3] = JsonMetadataServices.CreatePropertyInfo<List<ulong>>(options, propertyInfoValues4);
    jsonPropertyInfoArray[3].IsGetNullable = false;
    jsonPropertyInfoArray[3].IsSetNullable = false;
    return jsonPropertyInfoArray;
  }

  private static JsonParameterInfoValues[] NullLeaderboardFileEntryCtorParamInit()
  {
    return new JsonParameterInfoValues[1]
    {
      new JsonParameterInfoValues()
      {
        Name = "name",
        ParameterType = typeof (string),
        Position = 0,
        IsNullable = false,
        IsMemberInitializer = true
      }
    };
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Platform.Null.NullMultiplayerName> NullMultiplayerName
  {
    get
    {
      return this._NullMultiplayerName ?? (this._NullMultiplayerName = (JsonTypeInfo<MegaCrit.Sts2.Core.Platform.Null.NullMultiplayerName>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Platform.Null.NullMultiplayerName)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Platform.Null.NullMultiplayerName> Create_NullMultiplayerName(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Platform.Null.NullMultiplayerName> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Platform.Null.NullMultiplayerName>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Platform.Null.NullMultiplayerName> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Platform.Null.NullMultiplayerName>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Platform.Null.NullMultiplayerName>) (() => new MegaCrit.Sts2.Core.Platform.Null.NullMultiplayerName()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Platform.Null.NullMultiplayerName>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.NullMultiplayerNamePropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Platform.Null.NullMultiplayerName).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Platform.Null.NullMultiplayerName>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Platform.Null.NullMultiplayerName>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] NullMultiplayerNamePropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[2];
    JsonPropertyInfoValues<ulong> propertyInfoValues1 = new JsonPropertyInfoValues<ulong>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Platform.Null.NullMultiplayerName),
      Converter = (JsonConverter<ulong>) null,
      Getter = (Func<object, ulong>) (obj => ((MegaCrit.Sts2.Core.Platform.Null.NullMultiplayerName) obj).netId),
      Setter = (Action<object, ulong>) ((obj, value) => ((MegaCrit.Sts2.Core.Platform.Null.NullMultiplayerName) obj).netId = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "netId",
      JsonPropertyName = "net_id",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Platform.Null.NullMultiplayerName).GetField("netId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<ulong>(options, propertyInfoValues1);
    JsonPropertyInfoValues<string> propertyInfoValues2 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Platform.Null.NullMultiplayerName),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Platform.Null.NullMultiplayerName) obj).name),
      Setter = (Action<object, string>) ((obj, value) => ((MegaCrit.Sts2.Core.Platform.Null.NullMultiplayerName) obj).name = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "name",
      JsonPropertyName = "name",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Platform.Null.NullMultiplayerName).GetField("name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues2);
    jsonPropertyInfoArray[1].IsGetNullable = false;
    jsonPropertyInfoArray[1].IsSetNullable = false;
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Platform.PlatformType> PlatformType
  {
    get
    {
      return this._PlatformType ?? (this._PlatformType = (JsonTypeInfo<MegaCrit.Sts2.Core.Platform.PlatformType>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Platform.PlatformType)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Platform.PlatformType> Create_PlatformType(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Platform.PlatformType> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Platform.PlatformType>(options, out jsonTypeInfo))
      jsonTypeInfo = JsonMetadataServices.CreateValueInfo<MegaCrit.Sts2.Core.Platform.PlatformType>(options, (JsonConverter) JsonMetadataServices.GetEnumConverter<MegaCrit.Sts2.Core.Platform.PlatformType>(options));
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Rewards.RewardType> RewardType
  {
    get
    {
      return this._RewardType ?? (this._RewardType = (JsonTypeInfo<MegaCrit.Sts2.Core.Rewards.RewardType>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Rewards.RewardType)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Rewards.RewardType> Create_RewardType(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Rewards.RewardType> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Rewards.RewardType>(options, out jsonTypeInfo))
      jsonTypeInfo = JsonMetadataServices.CreateValueInfo<MegaCrit.Sts2.Core.Rewards.RewardType>(options, (JsonConverter) JsonMetadataServices.GetEnumConverter<MegaCrit.Sts2.Core.Rewards.RewardType>(options));
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Rooms.RoomType> RoomType
  {
    get
    {
      return this._RoomType ?? (this._RoomType = (JsonTypeInfo<MegaCrit.Sts2.Core.Rooms.RoomType>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Rooms.RoomType)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Rooms.RoomType> Create_RoomType(JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Rooms.RoomType> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Rooms.RoomType>(options, out jsonTypeInfo))
      jsonTypeInfo = JsonMetadataServices.CreateValueInfo<MegaCrit.Sts2.Core.Rooms.RoomType>(options, (JsonConverter) JsonMetadataServices.GetEnumConverter<MegaCrit.Sts2.Core.Rooms.RoomType>(options));
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Runs.CardCreationSource> CardCreationSource
  {
    get
    {
      return this._CardCreationSource ?? (this._CardCreationSource = (JsonTypeInfo<MegaCrit.Sts2.Core.Runs.CardCreationSource>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Runs.CardCreationSource)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Runs.CardCreationSource> Create_CardCreationSource(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Runs.CardCreationSource> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Runs.CardCreationSource>(options, out jsonTypeInfo))
      jsonTypeInfo = JsonMetadataServices.CreateValueInfo<MegaCrit.Sts2.Core.Runs.CardCreationSource>(options, (JsonConverter) JsonMetadataServices.GetEnumConverter<MegaCrit.Sts2.Core.Runs.CardCreationSource>(options));
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Runs.CardRarityOddsType> CardRarityOddsType
  {
    get
    {
      return this._CardRarityOddsType ?? (this._CardRarityOddsType = (JsonTypeInfo<MegaCrit.Sts2.Core.Runs.CardRarityOddsType>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Runs.CardRarityOddsType)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Runs.CardRarityOddsType> Create_CardRarityOddsType(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Runs.CardRarityOddsType> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Runs.CardRarityOddsType>(options, out jsonTypeInfo))
      jsonTypeInfo = JsonMetadataServices.CreateValueInfo<MegaCrit.Sts2.Core.Runs.CardRarityOddsType>(options, (JsonConverter) JsonMetadataServices.GetEnumConverter<MegaCrit.Sts2.Core.Runs.CardRarityOddsType>(options));
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Runs.GameMode> GameMode
  {
    get
    {
      return this._GameMode ?? (this._GameMode = (JsonTypeInfo<MegaCrit.Sts2.Core.Runs.GameMode>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Runs.GameMode)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Runs.GameMode> Create_GameMode(JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Runs.GameMode> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Runs.GameMode>(options, out jsonTypeInfo))
      jsonTypeInfo = JsonMetadataServices.CreateValueInfo<MegaCrit.Sts2.Core.Runs.GameMode>(options, (JsonConverter) JsonMetadataServices.GetEnumConverter<MegaCrit.Sts2.Core.Runs.GameMode>(options));
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry> AncientChoiceHistoryEntry
  {
    get
    {
      return this._AncientChoiceHistoryEntry ?? (this._AncientChoiceHistoryEntry = (JsonTypeInfo<MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry> Create_AncientChoiceHistoryEntry(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry>) (() => new MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.AncientChoiceHistoryEntryPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] AncientChoiceHistoryEntryPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[3];
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Localization.LocString> propertyInfoValues1 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Localization.LocString>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Localization.LocString>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Localization.LocString>) (obj => ((MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry) obj).Title),
      Setter = (Action<object, MegaCrit.Sts2.Core.Localization.LocString>) ((obj, value) => ((MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry) obj).Title = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Title",
      JsonPropertyName = "title",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry).GetProperty("Title", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Localization.LocString), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Localization.LocString>(options, propertyInfoValues1);
    jsonPropertyInfoArray[0].IsGetNullable = false;
    jsonPropertyInfoArray[0].IsSetNullable = false;
    JsonPropertyInfoValues<bool> propertyInfoValues2 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) (obj => ((MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry) obj).WasChosen),
      Setter = (Action<object, bool>) ((obj, value) => ((MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry) obj).WasChosen = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "WasChosen",
      JsonPropertyName = "was_chosen",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry).GetProperty("WasChosen", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (bool), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues2);
    JsonPropertyInfoValues<string> propertyInfoValues3 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry) obj).TextKey),
      Setter = (Action<object, string>) null,
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "TextKey",
      JsonPropertyName = "TextKey",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry).GetProperty("TextKey", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues3);
    jsonPropertyInfoArray[2].IsGetNullable = false;
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Runs.History.CardChoiceHistoryEntry> CardChoiceHistoryEntry
  {
    get
    {
      return this._CardChoiceHistoryEntry ?? (this._CardChoiceHistoryEntry = (JsonTypeInfo<MegaCrit.Sts2.Core.Runs.History.CardChoiceHistoryEntry>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Runs.History.CardChoiceHistoryEntry)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Runs.History.CardChoiceHistoryEntry> Create_CardChoiceHistoryEntry(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Runs.History.CardChoiceHistoryEntry> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Runs.History.CardChoiceHistoryEntry>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Runs.History.CardChoiceHistoryEntry> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Runs.History.CardChoiceHistoryEntry>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Runs.History.CardChoiceHistoryEntry>) (() => new MegaCrit.Sts2.Core.Runs.History.CardChoiceHistoryEntry()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Runs.History.CardChoiceHistoryEntry>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.CardChoiceHistoryEntryPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.History.CardChoiceHistoryEntry).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Runs.History.CardChoiceHistoryEntry>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Runs.History.CardChoiceHistoryEntry>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] CardChoiceHistoryEntryPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[2];
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard> propertyInfoValues1 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.History.CardChoiceHistoryEntry),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>) (obj => ((MegaCrit.Sts2.Core.Runs.History.CardChoiceHistoryEntry) obj).Card),
      Setter = (Action<object, MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Runs.History.CardChoiceHistoryEntry>(obj).Card = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Card",
      JsonPropertyName = "card",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.History.CardChoiceHistoryEntry).GetProperty("Card", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableCard), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>(options, propertyInfoValues1);
    jsonPropertyInfoArray[0].IsGetNullable = false;
    jsonPropertyInfoArray[0].IsSetNullable = false;
    JsonPropertyInfoValues<bool> propertyInfoValues2 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.History.CardChoiceHistoryEntry),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) (obj => ((MegaCrit.Sts2.Core.Runs.History.CardChoiceHistoryEntry) obj).wasPicked),
      Setter = (Action<object, bool>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Runs.History.CardChoiceHistoryEntry>(obj).wasPicked = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "wasPicked",
      JsonPropertyName = "was_picked",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.History.CardChoiceHistoryEntry).GetField("wasPicked", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues2);
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Runs.History.CardEnchantmentHistoryEntry> CardEnchantmentHistoryEntry
  {
    get
    {
      return this._CardEnchantmentHistoryEntry ?? (this._CardEnchantmentHistoryEntry = (JsonTypeInfo<MegaCrit.Sts2.Core.Runs.History.CardEnchantmentHistoryEntry>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Runs.History.CardEnchantmentHistoryEntry)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Runs.History.CardEnchantmentHistoryEntry> Create_CardEnchantmentHistoryEntry(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Runs.History.CardEnchantmentHistoryEntry> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Runs.History.CardEnchantmentHistoryEntry>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Runs.History.CardEnchantmentHistoryEntry> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Runs.History.CardEnchantmentHistoryEntry>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Runs.History.CardEnchantmentHistoryEntry>) (() => new MegaCrit.Sts2.Core.Runs.History.CardEnchantmentHistoryEntry()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Runs.History.CardEnchantmentHistoryEntry>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.CardEnchantmentHistoryEntryPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.History.CardEnchantmentHistoryEntry).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Runs.History.CardEnchantmentHistoryEntry>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Runs.History.CardEnchantmentHistoryEntry>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] CardEnchantmentHistoryEntryPropInit(
    JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[2];
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard> propertyInfoValues1 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.History.CardEnchantmentHistoryEntry),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>) (obj => ((MegaCrit.Sts2.Core.Runs.History.CardEnchantmentHistoryEntry) obj).Card),
      Setter = (Action<object, MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Runs.History.CardEnchantmentHistoryEntry>(obj).Card = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Card",
      JsonPropertyName = "card",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.History.CardEnchantmentHistoryEntry).GetProperty("Card", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableCard), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>(options, propertyInfoValues1);
    jsonPropertyInfoArray[0].IsGetNullable = false;
    jsonPropertyInfoArray[0].IsSetNullable = false;
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId> propertyInfoValues2 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.History.CardEnchantmentHistoryEntry),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Models.ModelId>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Models.ModelId>) (obj => ((MegaCrit.Sts2.Core.Runs.History.CardEnchantmentHistoryEntry) obj).Enchantment),
      Setter = (Action<object, MegaCrit.Sts2.Core.Models.ModelId>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Runs.History.CardEnchantmentHistoryEntry>(obj).Enchantment = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Enchantment",
      JsonPropertyName = "enchantment",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.History.CardEnchantmentHistoryEntry).GetProperty("Enchantment", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Models.ModelId), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Models.ModelId>(options, propertyInfoValues2);
    jsonPropertyInfoArray[1].IsGetNullable = false;
    jsonPropertyInfoArray[1].IsSetNullable = false;
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Runs.History.CardTransformationHistoryEntry> CardTransformationHistoryEntry
  {
    get
    {
      return this._CardTransformationHistoryEntry ?? (this._CardTransformationHistoryEntry = (JsonTypeInfo<MegaCrit.Sts2.Core.Runs.History.CardTransformationHistoryEntry>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Runs.History.CardTransformationHistoryEntry)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Runs.History.CardTransformationHistoryEntry> Create_CardTransformationHistoryEntry(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Runs.History.CardTransformationHistoryEntry> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Runs.History.CardTransformationHistoryEntry>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Runs.History.CardTransformationHistoryEntry> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Runs.History.CardTransformationHistoryEntry>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Runs.History.CardTransformationHistoryEntry>) (() => new MegaCrit.Sts2.Core.Runs.History.CardTransformationHistoryEntry()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Runs.History.CardTransformationHistoryEntry>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.CardTransformationHistoryEntryPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.History.CardTransformationHistoryEntry).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Runs.History.CardTransformationHistoryEntry>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Runs.History.CardTransformationHistoryEntry>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] CardTransformationHistoryEntryPropInit(
    JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[2];
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard> propertyInfoValues1 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.History.CardTransformationHistoryEntry),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>) (obj => ((MegaCrit.Sts2.Core.Runs.History.CardTransformationHistoryEntry) obj).OriginalCard),
      Setter = (Action<object, MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Runs.History.CardTransformationHistoryEntry>(obj).OriginalCard = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "OriginalCard",
      JsonPropertyName = "original_card",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.History.CardTransformationHistoryEntry).GetProperty("OriginalCard", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableCard), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>(options, propertyInfoValues1);
    jsonPropertyInfoArray[0].IsGetNullable = false;
    jsonPropertyInfoArray[0].IsSetNullable = false;
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard> propertyInfoValues2 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.History.CardTransformationHistoryEntry),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>) (obj => ((MegaCrit.Sts2.Core.Runs.History.CardTransformationHistoryEntry) obj).FinalCard),
      Setter = (Action<object, MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Runs.History.CardTransformationHistoryEntry>(obj).FinalCard = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "FinalCard",
      JsonPropertyName = "final_card",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.History.CardTransformationHistoryEntry).GetProperty("FinalCard", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableCard), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>(options, propertyInfoValues2);
    jsonPropertyInfoArray[1].IsGetNullable = false;
    jsonPropertyInfoArray[1].IsSetNullable = false;
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Runs.History.EventOptionHistoryEntry> EventOptionHistoryEntry
  {
    get
    {
      return this._EventOptionHistoryEntry ?? (this._EventOptionHistoryEntry = (JsonTypeInfo<MegaCrit.Sts2.Core.Runs.History.EventOptionHistoryEntry>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Runs.History.EventOptionHistoryEntry)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Runs.History.EventOptionHistoryEntry> Create_EventOptionHistoryEntry(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Runs.History.EventOptionHistoryEntry> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Runs.History.EventOptionHistoryEntry>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Runs.History.EventOptionHistoryEntry> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Runs.History.EventOptionHistoryEntry>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Runs.History.EventOptionHistoryEntry>) (() => new MegaCrit.Sts2.Core.Runs.History.EventOptionHistoryEntry()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Runs.History.EventOptionHistoryEntry>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.EventOptionHistoryEntryPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.History.EventOptionHistoryEntry).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Runs.History.EventOptionHistoryEntry>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Runs.History.EventOptionHistoryEntry>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] EventOptionHistoryEntryPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[2];
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Localization.LocString> propertyInfoValues1 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Localization.LocString>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.History.EventOptionHistoryEntry),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Localization.LocString>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Localization.LocString>) (obj => ((MegaCrit.Sts2.Core.Runs.History.EventOptionHistoryEntry) obj).Title),
      Setter = (Action<object, MegaCrit.Sts2.Core.Localization.LocString>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Runs.History.EventOptionHistoryEntry>(obj).Title = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Title",
      JsonPropertyName = "title",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.History.EventOptionHistoryEntry).GetProperty("Title", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Localization.LocString), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Localization.LocString>(options, propertyInfoValues1);
    jsonPropertyInfoArray[0].IsGetNullable = false;
    jsonPropertyInfoArray[0].IsSetNullable = false;
    JsonPropertyInfoValues<Dictionary<string, object>> propertyInfoValues2 = new JsonPropertyInfoValues<Dictionary<string, object>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.History.EventOptionHistoryEntry),
      Converter = (JsonConverter<Dictionary<string, object>>) MegaCritSerializerContext.ExpandConverter(typeof (Dictionary<string, object>), (JsonConverter) new LocStringVariablesJsonConverter(), options),
      Getter = (Func<object, Dictionary<string, object>>) (obj => ((MegaCrit.Sts2.Core.Runs.History.EventOptionHistoryEntry) obj).Variables),
      Setter = (Action<object, Dictionary<string, object>>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Runs.History.EventOptionHistoryEntry>(obj).Variables = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = true,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Variables",
      JsonPropertyName = "variables",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.History.EventOptionHistoryEntry).GetProperty("Variables", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (Dictionary<string, object>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<Dictionary<string, object>>(options, propertyInfoValues2);
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry> MapPointHistoryEntry
  {
    get
    {
      return this._MapPointHistoryEntry ?? (this._MapPointHistoryEntry = (JsonTypeInfo<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry> Create_MapPointHistoryEntry(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>) (() => new MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.MapPointHistoryEntryPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] MapPointHistoryEntryPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[3];
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Map.MapPointType> propertyInfoValues1 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Map.MapPointType>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Map.MapPointType>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Map.MapPointType>) (obj => ((MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry) obj).MapPointType),
      Setter = (Action<object, MegaCrit.Sts2.Core.Map.MapPointType>) ((obj, value) => ((MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry) obj).MapPointType = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "MapPointType",
      JsonPropertyName = "map_point_type",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry).GetProperty("MapPointType", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Map.MapPointType), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Map.MapPointType>(options, propertyInfoValues1);
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry>> propertyInfoValues2 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry>>) (obj => ((MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry) obj).Rooms),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry>>) ((obj, value) => ((MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry) obj).Rooms = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Rooms",
      JsonPropertyName = "rooms",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry).GetProperty("Rooms", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry>>(options, propertyInfoValues2);
    jsonPropertyInfoArray[1].IsGetNullable = false;
    jsonPropertyInfoArray[1].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry>> propertyInfoValues3 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry>>) (obj => ((MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry) obj).PlayerStats),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry>>) ((obj, value) => ((MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry) obj).PlayerStats = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "PlayerStats",
      JsonPropertyName = "player_stats",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry).GetProperty("PlayerStats", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry>>(options, propertyInfoValues3);
    jsonPropertyInfoArray[2].IsGetNullable = false;
    jsonPropertyInfoArray[2].IsSetNullable = false;
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry> MapPointRoomHistoryEntry
  {
    get
    {
      return this._MapPointRoomHistoryEntry ?? (this._MapPointRoomHistoryEntry = (JsonTypeInfo<MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry> Create_MapPointRoomHistoryEntry(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry>) (() => new MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.MapPointRoomHistoryEntryPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] MapPointRoomHistoryEntryPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[4];
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Rooms.RoomType> propertyInfoValues1 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Rooms.RoomType>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Rooms.RoomType>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Rooms.RoomType>) (obj => ((MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry) obj).RoomType),
      Setter = (Action<object, MegaCrit.Sts2.Core.Rooms.RoomType>) ((obj, value) => ((MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry) obj).RoomType = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "RoomType",
      JsonPropertyName = "room_type",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry).GetProperty("RoomType", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Rooms.RoomType), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Rooms.RoomType>(options, propertyInfoValues1);
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId> propertyInfoValues2 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Models.ModelId>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Models.ModelId>) (obj => ((MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry) obj).ModelId),
      Setter = (Action<object, MegaCrit.Sts2.Core.Models.ModelId>) ((obj, value) => ((MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry) obj).ModelId = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "ModelId",
      JsonPropertyName = "model_id",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry).GetProperty("ModelId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Models.ModelId), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Models.ModelId>(options, propertyInfoValues2);
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>> propertyInfoValues3 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Models.ModelId>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) (obj => ((MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry) obj).MonsterIds),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) ((obj, value) => ((MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry) obj).MonsterIds = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "MonsterIds",
      JsonPropertyName = "monster_ids",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry).GetProperty("MonsterIds", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Models.ModelId>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Models.ModelId>>(options, propertyInfoValues3);
    jsonPropertyInfoArray[2].IsGetNullable = false;
    jsonPropertyInfoArray[2].IsSetNullable = false;
    JsonPropertyInfoValues<int> propertyInfoValues4 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry) obj).TurnsTaken),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry) obj).TurnsTaken = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "TurnsTaken",
      JsonPropertyName = "turns_taken",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry).GetProperty("TurnsTaken", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[3] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues4);
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry> ModelChoiceHistoryEntry
  {
    get
    {
      return this._ModelChoiceHistoryEntry ?? (this._ModelChoiceHistoryEntry = (JsonTypeInfo<MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry> Create_ModelChoiceHistoryEntry(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry>) (() => new MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.ModelChoiceHistoryEntryPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] ModelChoiceHistoryEntryPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[2];
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId> propertyInfoValues1 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Models.ModelId>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Models.ModelId>) (obj => ((MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry) obj).choice),
      Setter = (Action<object, MegaCrit.Sts2.Core.Models.ModelId>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry>(obj).choice = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "choice",
      JsonPropertyName = "choice",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry).GetField("choice", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Models.ModelId>(options, propertyInfoValues1);
    jsonPropertyInfoArray[0].IsGetNullable = false;
    jsonPropertyInfoArray[0].IsSetNullable = false;
    JsonPropertyInfoValues<bool> propertyInfoValues2 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) (obj => ((MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry) obj).wasPicked),
      Setter = (Action<object, bool>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry>(obj).wasPicked = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "wasPicked",
      JsonPropertyName = "was_picked",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry).GetField("wasPicked", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues2);
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry> PlayerMapPointHistoryEntry
  {
    get
    {
      return this._PlayerMapPointHistoryEntry ?? (this._PlayerMapPointHistoryEntry = (JsonTypeInfo<MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry> Create_PlayerMapPointHistoryEntry(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry>) (() => new MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.PlayerMapPointHistoryEntryPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] PlayerMapPointHistoryEntryPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[34];
    JsonPropertyInfoValues<ulong> propertyInfoValues1 = new JsonPropertyInfoValues<ulong>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry),
      Converter = (JsonConverter<ulong>) null,
      Getter = (Func<object, ulong>) (obj => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).PlayerId),
      Setter = (Action<object, ulong>) ((obj, value) => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).PlayerId = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "PlayerId",
      JsonPropertyName = "player_id",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry).GetProperty("PlayerId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (ulong), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<ulong>(options, propertyInfoValues1);
    JsonPropertyInfoValues<int> propertyInfoValues2 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).GoldGained),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).GoldGained = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "GoldGained",
      JsonPropertyName = "gold_gained",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry).GetProperty("GoldGained", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues2);
    JsonPropertyInfoValues<int> propertyInfoValues3 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).GoldSpent),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).GoldSpent = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "GoldSpent",
      JsonPropertyName = "gold_spent",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry).GetProperty("GoldSpent", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues3);
    JsonPropertyInfoValues<int> propertyInfoValues4 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).GoldLost),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).GoldLost = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "GoldLost",
      JsonPropertyName = "gold_lost",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry).GetProperty("GoldLost", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[3] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues4);
    JsonPropertyInfoValues<int> propertyInfoValues5 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).GoldStolen),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).GoldStolen = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "GoldStolen",
      JsonPropertyName = "gold_stolen",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry).GetProperty("GoldStolen", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[4] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues5);
    JsonPropertyInfoValues<int> propertyInfoValues6 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).StolenLoot),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).StolenLoot = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "StolenLoot",
      JsonPropertyName = "stolen_loot",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry).GetProperty("StolenLoot", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[5] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues6);
    JsonPropertyInfoValues<int> propertyInfoValues7 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).CurrentGold),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).CurrentGold = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "CurrentGold",
      JsonPropertyName = "current_gold",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry).GetProperty("CurrentGold", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[6] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues7);
    JsonPropertyInfoValues<int> propertyInfoValues8 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).CurrentHp),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).CurrentHp = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "CurrentHp",
      JsonPropertyName = "current_hp",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry).GetProperty("CurrentHp", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[7] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues8);
    JsonPropertyInfoValues<int> propertyInfoValues9 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).MaxHp),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).MaxHp = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "MaxHp",
      JsonPropertyName = "max_hp",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry).GetProperty("MaxHp", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[8] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues9);
    JsonPropertyInfoValues<int> propertyInfoValues10 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).DamageTaken),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).DamageTaken = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "DamageTaken",
      JsonPropertyName = "damage_taken",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry).GetProperty("DamageTaken", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[9] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues10);
    JsonPropertyInfoValues<int> propertyInfoValues11 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).HpHealed),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).HpHealed = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "HpHealed",
      JsonPropertyName = "hp_healed",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry).GetProperty("HpHealed", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[10] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues11);
    JsonPropertyInfoValues<int> propertyInfoValues12 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).MaxHpLost),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).MaxHpLost = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "MaxHpLost",
      JsonPropertyName = "max_hp_lost",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry).GetProperty("MaxHpLost", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[11] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues12);
    JsonPropertyInfoValues<int> propertyInfoValues13 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).MaxHpGained),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).MaxHpGained = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "MaxHpGained",
      JsonPropertyName = "max_hp_gained",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry).GetProperty("MaxHpGained", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[12] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues13);
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry>> propertyInfoValues14 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry>>) (obj => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).AncientChoices),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry>>) ((obj, value) => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).AncientChoices = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "AncientChoices",
      JsonPropertyName = "ancient_choice",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry).GetProperty("AncientChoices", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[13] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry>>(options, propertyInfoValues14);
    jsonPropertyInfoArray[13].IsGetNullable = false;
    jsonPropertyInfoArray[13].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>> propertyInfoValues15 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>) (obj => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).CardsGained),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>) ((obj, value) => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).CardsGained = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "CardsGained",
      JsonPropertyName = "cards_gained",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry).GetProperty("CardsGained", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[14] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>(options, propertyInfoValues15);
    jsonPropertyInfoArray[14].IsGetNullable = false;
    jsonPropertyInfoArray[14].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Runs.History.CardChoiceHistoryEntry>> propertyInfoValues16 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Runs.History.CardChoiceHistoryEntry>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Runs.History.CardChoiceHistoryEntry>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Runs.History.CardChoiceHistoryEntry>>) (obj => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).CardChoices),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Runs.History.CardChoiceHistoryEntry>>) ((obj, value) => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).CardChoices = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "CardChoices",
      JsonPropertyName = "card_choices",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry).GetProperty("CardChoices", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Runs.History.CardChoiceHistoryEntry>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[15] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Runs.History.CardChoiceHistoryEntry>>(options, propertyInfoValues16);
    jsonPropertyInfoArray[15].IsGetNullable = false;
    jsonPropertyInfoArray[15].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry>> propertyInfoValues17 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry>>) (obj => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).RelicChoices),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry>>) ((obj, value) => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).RelicChoices = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "RelicChoices",
      JsonPropertyName = "relic_choices",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry).GetProperty("RelicChoices", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[16 /*0x10*/] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry>>(options, propertyInfoValues17);
    jsonPropertyInfoArray[16 /*0x10*/].IsGetNullable = false;
    jsonPropertyInfoArray[16 /*0x10*/].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry>> propertyInfoValues18 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry>>) (obj => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).PotionChoices),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry>>) ((obj, value) => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).PotionChoices = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "PotionChoices",
      JsonPropertyName = "potion_choices",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry).GetProperty("PotionChoices", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[17] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry>>(options, propertyInfoValues18);
    jsonPropertyInfoArray[17].IsGetNullable = false;
    jsonPropertyInfoArray[17].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>> propertyInfoValues19 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Models.ModelId>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) (obj => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).PotionDiscarded),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) ((obj, value) => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).PotionDiscarded = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "PotionDiscarded",
      JsonPropertyName = "potion_discarded",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry).GetProperty("PotionDiscarded", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Models.ModelId>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[18] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Models.ModelId>>(options, propertyInfoValues19);
    jsonPropertyInfoArray[18].IsGetNullable = false;
    jsonPropertyInfoArray[18].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>> propertyInfoValues20 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Models.ModelId>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) (obj => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).PotionUsed),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) ((obj, value) => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).PotionUsed = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "PotionUsed",
      JsonPropertyName = "potion_used",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry).GetProperty("PotionUsed", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Models.ModelId>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[19] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Models.ModelId>>(options, propertyInfoValues20);
    jsonPropertyInfoArray[19].IsGetNullable = false;
    jsonPropertyInfoArray[19].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>> propertyInfoValues21 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>) (obj => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).CardsRemoved),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>) ((obj, value) => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).CardsRemoved = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "CardsRemoved",
      JsonPropertyName = "cards_removed",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry).GetProperty("CardsRemoved", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[20] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>(options, propertyInfoValues21);
    jsonPropertyInfoArray[20].IsGetNullable = false;
    jsonPropertyInfoArray[20].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>> propertyInfoValues22 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Models.ModelId>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) (obj => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).RelicsRemoved),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) ((obj, value) => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).RelicsRemoved = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "RelicsRemoved",
      JsonPropertyName = "relics_removed",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry).GetProperty("RelicsRemoved", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Models.ModelId>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[21] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Models.ModelId>>(options, propertyInfoValues22);
    jsonPropertyInfoArray[21].IsGetNullable = false;
    jsonPropertyInfoArray[21].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Runs.History.CardEnchantmentHistoryEntry>> propertyInfoValues23 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Runs.History.CardEnchantmentHistoryEntry>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Runs.History.CardEnchantmentHistoryEntry>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Runs.History.CardEnchantmentHistoryEntry>>) (obj => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).CardsEnchanted),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Runs.History.CardEnchantmentHistoryEntry>>) ((obj, value) => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).CardsEnchanted = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "CardsEnchanted",
      JsonPropertyName = "cards_enchanted",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry).GetProperty("CardsEnchanted", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Runs.History.CardEnchantmentHistoryEntry>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[22] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Runs.History.CardEnchantmentHistoryEntry>>(options, propertyInfoValues23);
    jsonPropertyInfoArray[22].IsGetNullable = false;
    jsonPropertyInfoArray[22].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Runs.History.CardTransformationHistoryEntry>> propertyInfoValues24 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Runs.History.CardTransformationHistoryEntry>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Runs.History.CardTransformationHistoryEntry>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Runs.History.CardTransformationHistoryEntry>>) (obj => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).CardsTransformed),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Runs.History.CardTransformationHistoryEntry>>) ((obj, value) => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).CardsTransformed = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "CardsTransformed",
      JsonPropertyName = "cards_transformed",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry).GetProperty("CardsTransformed", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Runs.History.CardTransformationHistoryEntry>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[23] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Runs.History.CardTransformationHistoryEntry>>(options, propertyInfoValues24);
    jsonPropertyInfoArray[23].IsGetNullable = false;
    jsonPropertyInfoArray[23].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>> propertyInfoValues25 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Models.ModelId>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) (obj => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).UpgradedCards),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) ((obj, value) => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).UpgradedCards = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "UpgradedCards",
      JsonPropertyName = "upgraded_cards",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry).GetProperty("UpgradedCards", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Models.ModelId>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[24] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Models.ModelId>>(options, propertyInfoValues25);
    jsonPropertyInfoArray[24].IsGetNullable = false;
    jsonPropertyInfoArray[24].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>> propertyInfoValues26 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Models.ModelId>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) (obj => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).DowngradedCards),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) ((obj, value) => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).DowngradedCards = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "DowngradedCards",
      JsonPropertyName = "downgraded_cards",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry).GetProperty("DowngradedCards", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Models.ModelId>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[25] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Models.ModelId>>(options, propertyInfoValues26);
    jsonPropertyInfoArray[25].IsGetNullable = false;
    jsonPropertyInfoArray[25].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Runs.History.EventOptionHistoryEntry>> propertyInfoValues27 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Runs.History.EventOptionHistoryEntry>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Runs.History.EventOptionHistoryEntry>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Runs.History.EventOptionHistoryEntry>>) (obj => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).EventChoices),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Runs.History.EventOptionHistoryEntry>>) ((obj, value) => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).EventChoices = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "EventChoices",
      JsonPropertyName = "event_choices",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry).GetProperty("EventChoices", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Runs.History.EventOptionHistoryEntry>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[26] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Runs.History.EventOptionHistoryEntry>>(options, propertyInfoValues27);
    jsonPropertyInfoArray[26].IsGetNullable = false;
    jsonPropertyInfoArray[26].IsSetNullable = false;
    JsonPropertyInfoValues<List<string>> propertyInfoValues28 = new JsonPropertyInfoValues<List<string>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry),
      Converter = (JsonConverter<List<string>>) null,
      Getter = (Func<object, List<string>>) (obj => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).RestSiteChoices),
      Setter = (Action<object, List<string>>) ((obj, value) => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).RestSiteChoices = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "RestSiteChoices",
      JsonPropertyName = "rest_site_choices",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry).GetProperty("RestSiteChoices", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<string>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[27] = JsonMetadataServices.CreatePropertyInfo<List<string>>(options, propertyInfoValues28);
    jsonPropertyInfoArray[27].IsGetNullable = false;
    jsonPropertyInfoArray[27].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>> propertyInfoValues29 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Models.ModelId>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) (obj => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).BoughtRelics),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) ((obj, value) => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).BoughtRelics = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "BoughtRelics",
      JsonPropertyName = "bought_relics",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry).GetProperty("BoughtRelics", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Models.ModelId>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[28] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Models.ModelId>>(options, propertyInfoValues29);
    jsonPropertyInfoArray[28].IsGetNullable = false;
    jsonPropertyInfoArray[28].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>> propertyInfoValues30 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Models.ModelId>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) (obj => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).BoughtPotions),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) ((obj, value) => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).BoughtPotions = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "BoughtPotions",
      JsonPropertyName = "bought_potions",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry).GetProperty("BoughtPotions", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Models.ModelId>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[29] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Models.ModelId>>(options, propertyInfoValues30);
    jsonPropertyInfoArray[29].IsGetNullable = false;
    jsonPropertyInfoArray[29].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>> propertyInfoValues31 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Models.ModelId>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) (obj => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).BoughtColorless),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) ((obj, value) => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).BoughtColorless = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "BoughtColorless",
      JsonPropertyName = "bought_colorless",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry).GetProperty("BoughtColorless", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Models.ModelId>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[30] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Models.ModelId>>(options, propertyInfoValues31);
    jsonPropertyInfoArray[30].IsGetNullable = false;
    jsonPropertyInfoArray[30].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>> propertyInfoValues32 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Models.ModelId>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) (obj => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).CompletedQuests),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) ((obj, value) => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).CompletedQuests = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "CompletedQuests",
      JsonPropertyName = "completed_quests",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry).GetProperty("CompletedQuests", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Models.ModelId>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[31 /*0x1F*/] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Models.ModelId>>(options, propertyInfoValues32);
    jsonPropertyInfoArray[31 /*0x1F*/].IsGetNullable = false;
    jsonPropertyInfoArray[31 /*0x1F*/].IsSetNullable = false;
    JsonPropertyInfoValues<bool> propertyInfoValues33 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) (obj => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).IsAffectedByFurCoat),
      Setter = (Action<object, bool>) ((obj, value) => ((MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry) obj).IsAffectedByFurCoat = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "IsAffectedByFurCoat",
      JsonPropertyName = "is_affected_by_fur_coat",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry).GetProperty("IsAffectedByFurCoat", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (bool), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[32 /*0x20*/] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues33);
    JsonPropertyInfoValues<bool> propertyInfoValues34 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) null,
      Setter = (Action<object, bool>) null,
      IgnoreCondition = new JsonIgnoreCondition?((JsonIgnoreCondition) 1),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "WasMugged",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry).GetProperty("WasMugged", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (bool), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[33] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues34);
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Runs.RunHistory> RunHistory
  {
    get
    {
      return this._RunHistory ?? (this._RunHistory = (JsonTypeInfo<MegaCrit.Sts2.Core.Runs.RunHistory>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Runs.RunHistory)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Runs.RunHistory> Create_RunHistory(JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Runs.RunHistory> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Runs.RunHistory>(options, out jsonTypeInfo))
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Runs.RunHistory> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Runs.RunHistory>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Runs.RunHistory>) null,
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Runs.RunHistory>) (args => new MegaCrit.Sts2.Core.Runs.RunHistory()
        {
          PlatformType = (MegaCrit.Sts2.Core.Platform.PlatformType) args[0],
          GameMode = (MegaCrit.Sts2.Core.Runs.GameMode) args[1],
          Win = (bool) args[2],
          Seed = (string) args[3],
          StartTime = (long) args[4],
          RunTime = (float) args[5],
          Ascension = (int) args[6],
          BuildId = (string) args[7],
          WasAbandoned = (bool) args[8],
          KilledByEncounter = (MegaCrit.Sts2.Core.Models.ModelId) args[9],
          KilledByEvent = (MegaCrit.Sts2.Core.Models.ModelId) args[10],
          Players = (List<MegaCrit.Sts2.Core.Runs.RunHistoryPlayer>) args[11],
          Acts = (List<MegaCrit.Sts2.Core.Models.ModelId>) args[12],
          Modifiers = (List<MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier>) args[13],
          MapPointHistory = (List<List<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>>) args[14]
        }),
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.RunHistoryPropInit(options)),
        ConstructorParameterMetadataInitializer = MegaCritSerializerContext.\u003C\u003EO.\u003C4\u003E__RunHistoryCtorParamInit ?? (MegaCritSerializerContext.\u003C\u003EO.\u003C4\u003E__RunHistoryCtorParamInit = new Func<JsonParameterInfoValues[]>(MegaCritSerializerContext.RunHistoryCtorParamInit)),
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.RunHistory).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Runs.RunHistory>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Runs.RunHistory>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] RunHistoryPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[16 /*0x10*/];
    JsonPropertyInfoValues<int> propertyInfoValues1 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.RunHistory),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Runs.RunHistory) obj).SchemaVersion),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Runs.RunHistory) obj).SchemaVersion = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "SchemaVersion",
      JsonPropertyName = "schema_version",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.RunHistory).GetProperty("SchemaVersion", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues1);
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Platform.PlatformType> propertyInfoValues2 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Platform.PlatformType>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.RunHistory),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Platform.PlatformType>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Platform.PlatformType>) (obj => ((MegaCrit.Sts2.Core.Runs.RunHistory) obj).PlatformType),
      Setter = (Action<object, MegaCrit.Sts2.Core.Platform.PlatformType>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "PlatformType",
      JsonPropertyName = "platform_type",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.RunHistory).GetProperty("PlatformType", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Platform.PlatformType), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Platform.PlatformType>(options, propertyInfoValues2);
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Runs.GameMode> propertyInfoValues3 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Runs.GameMode>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.RunHistory),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Runs.GameMode>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Runs.GameMode>) (obj => ((MegaCrit.Sts2.Core.Runs.RunHistory) obj).GameMode),
      Setter = (Action<object, MegaCrit.Sts2.Core.Runs.GameMode>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "GameMode",
      JsonPropertyName = "game_mode",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.RunHistory).GetProperty("GameMode", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Runs.GameMode), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Runs.GameMode>(options, propertyInfoValues3);
    JsonPropertyInfoValues<bool> propertyInfoValues4 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.RunHistory),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) (obj => ((MegaCrit.Sts2.Core.Runs.RunHistory) obj).Win),
      Setter = (Action<object, bool>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Win",
      JsonPropertyName = "win",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.RunHistory).GetProperty("Win", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (bool), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[3] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues4);
    JsonPropertyInfoValues<string> propertyInfoValues5 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.RunHistory),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Runs.RunHistory) obj).Seed),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Seed",
      JsonPropertyName = "seed",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.RunHistory).GetProperty("Seed", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[4] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues5);
    jsonPropertyInfoArray[4].IsGetNullable = false;
    jsonPropertyInfoArray[4].IsSetNullable = false;
    JsonPropertyInfoValues<long> propertyInfoValues6 = new JsonPropertyInfoValues<long>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.RunHistory),
      Converter = (JsonConverter<long>) null,
      Getter = (Func<object, long>) (obj => ((MegaCrit.Sts2.Core.Runs.RunHistory) obj).StartTime),
      Setter = (Action<object, long>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "StartTime",
      JsonPropertyName = "start_time",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.RunHistory).GetProperty("StartTime", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (long), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[5] = JsonMetadataServices.CreatePropertyInfo<long>(options, propertyInfoValues6);
    JsonPropertyInfoValues<float> propertyInfoValues7 = new JsonPropertyInfoValues<float>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.RunHistory),
      Converter = (JsonConverter<float>) null,
      Getter = (Func<object, float>) (obj => ((MegaCrit.Sts2.Core.Runs.RunHistory) obj).RunTime),
      Setter = (Action<object, float>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "RunTime",
      JsonPropertyName = "run_time",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.RunHistory).GetProperty("RunTime", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (float), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[6] = JsonMetadataServices.CreatePropertyInfo<float>(options, propertyInfoValues7);
    JsonPropertyInfoValues<int> propertyInfoValues8 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.RunHistory),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Runs.RunHistory) obj).Ascension),
      Setter = (Action<object, int>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Ascension",
      JsonPropertyName = "ascension",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.RunHistory).GetProperty("Ascension", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[7] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues8);
    JsonPropertyInfoValues<string> propertyInfoValues9 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.RunHistory),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Runs.RunHistory) obj).BuildId),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "BuildId",
      JsonPropertyName = "build_id",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.RunHistory).GetProperty("BuildId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[8] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues9);
    jsonPropertyInfoArray[8].IsGetNullable = false;
    jsonPropertyInfoArray[8].IsSetNullable = false;
    JsonPropertyInfoValues<bool> propertyInfoValues10 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.RunHistory),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) (obj => ((MegaCrit.Sts2.Core.Runs.RunHistory) obj).WasAbandoned),
      Setter = (Action<object, bool>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "WasAbandoned",
      JsonPropertyName = "was_abandoned",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.RunHistory).GetProperty("WasAbandoned", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (bool), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[9] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues10);
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId> propertyInfoValues11 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.RunHistory),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Models.ModelId>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Models.ModelId>) (obj => ((MegaCrit.Sts2.Core.Runs.RunHistory) obj).KilledByEncounter),
      Setter = (Action<object, MegaCrit.Sts2.Core.Models.ModelId>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "KilledByEncounter",
      JsonPropertyName = "killed_by_encounter",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.RunHistory).GetProperty("KilledByEncounter", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Models.ModelId), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[10] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Models.ModelId>(options, propertyInfoValues11);
    jsonPropertyInfoArray[10].IsGetNullable = false;
    jsonPropertyInfoArray[10].IsSetNullable = false;
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId> propertyInfoValues12 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.RunHistory),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Models.ModelId>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Models.ModelId>) (obj => ((MegaCrit.Sts2.Core.Runs.RunHistory) obj).KilledByEvent),
      Setter = (Action<object, MegaCrit.Sts2.Core.Models.ModelId>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "KilledByEvent",
      JsonPropertyName = "killed_by_event",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.RunHistory).GetProperty("KilledByEvent", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Models.ModelId), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[11] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Models.ModelId>(options, propertyInfoValues12);
    jsonPropertyInfoArray[11].IsGetNullable = false;
    jsonPropertyInfoArray[11].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Runs.RunHistoryPlayer>> propertyInfoValues13 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Runs.RunHistoryPlayer>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.RunHistory),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Runs.RunHistoryPlayer>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Runs.RunHistoryPlayer>>) (obj => ((MegaCrit.Sts2.Core.Runs.RunHistory) obj).Players),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Runs.RunHistoryPlayer>>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Players",
      JsonPropertyName = "players",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.RunHistory).GetProperty("Players", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Runs.RunHistoryPlayer>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[12] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Runs.RunHistoryPlayer>>(options, propertyInfoValues13);
    jsonPropertyInfoArray[12].IsGetNullable = false;
    jsonPropertyInfoArray[12].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>> propertyInfoValues14 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.RunHistory),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Models.ModelId>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) (obj => ((MegaCrit.Sts2.Core.Runs.RunHistory) obj).Acts),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Acts",
      JsonPropertyName = "acts",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.RunHistory).GetProperty("Acts", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Models.ModelId>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[13] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Models.ModelId>>(options, propertyInfoValues14);
    jsonPropertyInfoArray[13].IsGetNullable = false;
    jsonPropertyInfoArray[13].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier>> propertyInfoValues15 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.RunHistory),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier>>) (obj => ((MegaCrit.Sts2.Core.Runs.RunHistory) obj).Modifiers),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier>>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Modifiers",
      JsonPropertyName = "modifiers",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.RunHistory).GetProperty("Modifiers", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[14] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier>>(options, propertyInfoValues15);
    jsonPropertyInfoArray[14].IsGetNullable = false;
    jsonPropertyInfoArray[14].IsSetNullable = false;
    JsonPropertyInfoValues<List<List<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>>> propertyInfoValues16 = new JsonPropertyInfoValues<List<List<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.RunHistory),
      Converter = (JsonConverter<List<List<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>>>) null,
      Getter = (Func<object, List<List<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>>>) (obj => ((MegaCrit.Sts2.Core.Runs.RunHistory) obj).MapPointHistory),
      Setter = (Action<object, List<List<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>>>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "MapPointHistory",
      JsonPropertyName = "map_point_history",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.RunHistory).GetProperty("MapPointHistory", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<List<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[15] = JsonMetadataServices.CreatePropertyInfo<List<List<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>>>(options, propertyInfoValues16);
    jsonPropertyInfoArray[15].IsGetNullable = false;
    jsonPropertyInfoArray[15].IsSetNullable = false;
    return jsonPropertyInfoArray;
  }

  private static JsonParameterInfoValues[] RunHistoryCtorParamInit()
  {
    return new JsonParameterInfoValues[15]
    {
      new JsonParameterInfoValues()
      {
        Name = "PlatformType",
        ParameterType = typeof (MegaCrit.Sts2.Core.Platform.PlatformType),
        Position = 0,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "GameMode",
        ParameterType = typeof (MegaCrit.Sts2.Core.Runs.GameMode),
        Position = 1,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Win",
        ParameterType = typeof (bool),
        Position = 2,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Seed",
        ParameterType = typeof (string),
        Position = 3,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "StartTime",
        ParameterType = typeof (long),
        Position = 4,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "RunTime",
        ParameterType = typeof (float),
        Position = 5,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Ascension",
        ParameterType = typeof (int),
        Position = 6,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "BuildId",
        ParameterType = typeof (string),
        Position = 7,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "WasAbandoned",
        ParameterType = typeof (bool),
        Position = 8,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "KilledByEncounter",
        ParameterType = typeof (MegaCrit.Sts2.Core.Models.ModelId),
        Position = 9,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "KilledByEvent",
        ParameterType = typeof (MegaCrit.Sts2.Core.Models.ModelId),
        Position = 10,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Players",
        ParameterType = typeof (List<MegaCrit.Sts2.Core.Runs.RunHistoryPlayer>),
        Position = 11,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Acts",
        ParameterType = typeof (List<MegaCrit.Sts2.Core.Models.ModelId>),
        Position = 12,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Modifiers",
        ParameterType = typeof (List<MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier>),
        Position = 13,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "MapPointHistory",
        ParameterType = typeof (List<List<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>>),
        Position = 14,
        IsNullable = false,
        IsMemberInitializer = true
      }
    };
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Runs.RunHistoryPlayer> RunHistoryPlayer
  {
    get
    {
      return this._RunHistoryPlayer ?? (this._RunHistoryPlayer = (JsonTypeInfo<MegaCrit.Sts2.Core.Runs.RunHistoryPlayer>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Runs.RunHistoryPlayer)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Runs.RunHistoryPlayer> Create_RunHistoryPlayer(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Runs.RunHistoryPlayer> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Runs.RunHistoryPlayer>(options, out jsonTypeInfo))
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Runs.RunHistoryPlayer> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Runs.RunHistoryPlayer>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Runs.RunHistoryPlayer>) null,
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Runs.RunHistoryPlayer>) (args => new MegaCrit.Sts2.Core.Runs.RunHistoryPlayer()
        {
          Id = (ulong) args[0],
          Character = (MegaCrit.Sts2.Core.Models.ModelId) args[1]
        }),
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.RunHistoryPlayerPropInit(options)),
        ConstructorParameterMetadataInitializer = MegaCritSerializerContext.\u003C\u003EO.\u003C5\u003E__RunHistoryPlayerCtorParamInit ?? (MegaCritSerializerContext.\u003C\u003EO.\u003C5\u003E__RunHistoryPlayerCtorParamInit = new Func<JsonParameterInfoValues[]>(MegaCritSerializerContext.RunHistoryPlayerCtorParamInit)),
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.RunHistoryPlayer).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Runs.RunHistoryPlayer>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Runs.RunHistoryPlayer>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] RunHistoryPlayerPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[7];
    JsonPropertyInfoValues<ulong> propertyInfoValues1 = new JsonPropertyInfoValues<ulong>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.RunHistoryPlayer),
      Converter = (JsonConverter<ulong>) null,
      Getter = (Func<object, ulong>) (obj => ((MegaCrit.Sts2.Core.Runs.RunHistoryPlayer) obj).Id),
      Setter = (Action<object, ulong>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Id",
      JsonPropertyName = "id",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.RunHistoryPlayer).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (ulong), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<ulong>(options, propertyInfoValues1);
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId> propertyInfoValues2 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.RunHistoryPlayer),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Models.ModelId>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Models.ModelId>) (obj => ((MegaCrit.Sts2.Core.Runs.RunHistoryPlayer) obj).Character),
      Setter = (Action<object, MegaCrit.Sts2.Core.Models.ModelId>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Character",
      JsonPropertyName = "character",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.RunHistoryPlayer).GetProperty("Character", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Models.ModelId), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Models.ModelId>(options, propertyInfoValues2);
    jsonPropertyInfoArray[1].IsGetNullable = false;
    jsonPropertyInfoArray[1].IsSetNullable = false;
    JsonPropertyInfoValues<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>> propertyInfoValues3 = new JsonPropertyInfoValues<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.RunHistoryPlayer),
      Converter = (JsonConverter<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>) null,
      Getter = (Func<object, IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>) (obj => ((MegaCrit.Sts2.Core.Runs.RunHistoryPlayer) obj).Deck),
      Setter = (Action<object, IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>) ((obj, value) => ((MegaCrit.Sts2.Core.Runs.RunHistoryPlayer) obj).Deck = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Deck",
      JsonPropertyName = "deck",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.RunHistoryPlayer).GetProperty("Deck", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>(options, propertyInfoValues3);
    jsonPropertyInfoArray[2].IsGetNullable = false;
    jsonPropertyInfoArray[2].IsSetNullable = false;
    JsonPropertyInfoValues<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>> propertyInfoValues4 = new JsonPropertyInfoValues<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.RunHistoryPlayer),
      Converter = (JsonConverter<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>>) null,
      Getter = (Func<object, IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>>) (obj => ((MegaCrit.Sts2.Core.Runs.RunHistoryPlayer) obj).Relics),
      Setter = (Action<object, IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>>) ((obj, value) => ((MegaCrit.Sts2.Core.Runs.RunHistoryPlayer) obj).Relics = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Relics",
      JsonPropertyName = "relics",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.RunHistoryPlayer).GetProperty("Relics", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[3] = JsonMetadataServices.CreatePropertyInfo<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>>(options, propertyInfoValues4);
    jsonPropertyInfoArray[3].IsGetNullable = false;
    jsonPropertyInfoArray[3].IsSetNullable = false;
    JsonPropertyInfoValues<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>> propertyInfoValues5 = new JsonPropertyInfoValues<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.RunHistoryPlayer),
      Converter = (JsonConverter<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>>) null,
      Getter = (Func<object, IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>>) (obj => ((MegaCrit.Sts2.Core.Runs.RunHistoryPlayer) obj).Potions),
      Setter = (Action<object, IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>>) ((obj, value) => ((MegaCrit.Sts2.Core.Runs.RunHistoryPlayer) obj).Potions = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Potions",
      JsonPropertyName = "potions",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.RunHistoryPlayer).GetProperty("Potions", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[4] = JsonMetadataServices.CreatePropertyInfo<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>>(options, propertyInfoValues5);
    jsonPropertyInfoArray[4].IsGetNullable = false;
    jsonPropertyInfoArray[4].IsSetNullable = false;
    JsonPropertyInfoValues<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableBadge>> propertyInfoValues6 = new JsonPropertyInfoValues<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableBadge>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.RunHistoryPlayer),
      Converter = (JsonConverter<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableBadge>>) null,
      Getter = (Func<object, IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableBadge>>) (obj => ((MegaCrit.Sts2.Core.Runs.RunHistoryPlayer) obj).Badges),
      Setter = (Action<object, IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableBadge>>) ((obj, value) => ((MegaCrit.Sts2.Core.Runs.RunHistoryPlayer) obj).Badges = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Badges",
      JsonPropertyName = "badges",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.RunHistoryPlayer).GetProperty("Badges", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableBadge>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[5] = JsonMetadataServices.CreatePropertyInfo<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableBadge>>(options, propertyInfoValues6);
    jsonPropertyInfoArray[5].IsGetNullable = false;
    jsonPropertyInfoArray[5].IsSetNullable = false;
    JsonPropertyInfoValues<int> propertyInfoValues7 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.RunHistoryPlayer),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Runs.RunHistoryPlayer) obj).MaxPotionSlotCount),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Runs.RunHistoryPlayer) obj).MaxPotionSlotCount = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "MaxPotionSlotCount",
      JsonPropertyName = "max_potion_slot_count",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.RunHistoryPlayer).GetProperty("MaxPotionSlotCount", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[6] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues7);
    return jsonPropertyInfoArray;
  }

  private static JsonParameterInfoValues[] RunHistoryPlayerCtorParamInit()
  {
    return new JsonParameterInfoValues[2]
    {
      new JsonParameterInfoValues()
      {
        Name = "Id",
        ParameterType = typeof (ulong),
        Position = 0,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Character",
        ParameterType = typeof (MegaCrit.Sts2.Core.Models.ModelId),
        Position = 1,
        IsNullable = false,
        IsMemberInitializer = true
      }
    };
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.AncientCharacterStats> AncientCharacterStats
  {
    get
    {
      return this._AncientCharacterStats ?? (this._AncientCharacterStats = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.AncientCharacterStats>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.AncientCharacterStats)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.AncientCharacterStats> Create_AncientCharacterStats(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.AncientCharacterStats> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.AncientCharacterStats>(options, out jsonTypeInfo))
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.AncientCharacterStats> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.AncientCharacterStats>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Saves.AncientCharacterStats>) null,
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Saves.AncientCharacterStats>) (args => new MegaCrit.Sts2.Core.Saves.AncientCharacterStats()
        {
          Character = (MegaCrit.Sts2.Core.Models.ModelId) args[0]
        }),
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.AncientCharacterStatsPropInit(options)),
        ConstructorParameterMetadataInitializer = MegaCritSerializerContext.\u003C\u003EO.\u003C6\u003E__AncientCharacterStatsCtorParamInit ?? (MegaCritSerializerContext.\u003C\u003EO.\u003C6\u003E__AncientCharacterStatsCtorParamInit = new Func<JsonParameterInfoValues[]>(MegaCritSerializerContext.AncientCharacterStatsCtorParamInit)),
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.AncientCharacterStats).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Saves.AncientCharacterStats>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Saves.AncientCharacterStats>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] AncientCharacterStatsPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[4];
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId> propertyInfoValues1 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.AncientCharacterStats),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Models.ModelId>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Models.ModelId>) (obj => ((MegaCrit.Sts2.Core.Saves.AncientCharacterStats) obj).Character),
      Setter = (Action<object, MegaCrit.Sts2.Core.Models.ModelId>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Character",
      JsonPropertyName = "character",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.AncientCharacterStats).GetProperty("Character", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Models.ModelId), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Models.ModelId>(options, propertyInfoValues1);
    jsonPropertyInfoArray[0].IsRequired = true;
    jsonPropertyInfoArray[0].IsGetNullable = false;
    jsonPropertyInfoArray[0].IsSetNullable = false;
    JsonPropertyInfoValues<int> propertyInfoValues2 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.AncientCharacterStats),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.AncientCharacterStats) obj).Wins),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.AncientCharacterStats) obj).Wins = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Wins",
      JsonPropertyName = "wins",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.AncientCharacterStats).GetProperty("Wins", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues2);
    JsonPropertyInfoValues<int> propertyInfoValues3 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.AncientCharacterStats),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.AncientCharacterStats) obj).Losses),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.AncientCharacterStats) obj).Losses = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Losses",
      JsonPropertyName = "losses",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.AncientCharacterStats).GetProperty("Losses", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues3);
    JsonPropertyInfoValues<int> propertyInfoValues4 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.AncientCharacterStats),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) null,
      Setter = (Action<object, int>) null,
      IgnoreCondition = new JsonIgnoreCondition?((JsonIgnoreCondition) 1),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Visits",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.AncientCharacterStats).GetProperty("Visits", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[3] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues4);
    return jsonPropertyInfoArray;
  }

  private static JsonParameterInfoValues[] AncientCharacterStatsCtorParamInit()
  {
    return new JsonParameterInfoValues[1]
    {
      new JsonParameterInfoValues()
      {
        Name = "Character",
        ParameterType = typeof (MegaCrit.Sts2.Core.Models.ModelId),
        Position = 0,
        IsNullable = false,
        IsMemberInitializer = true
      }
    };
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.AncientStats> AncientStats
  {
    get
    {
      return this._AncientStats ?? (this._AncientStats = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.AncientStats>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.AncientStats)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.AncientStats> Create_AncientStats(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.AncientStats> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.AncientStats>(options, out jsonTypeInfo))
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.AncientStats> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.AncientStats>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Saves.AncientStats>) null,
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Saves.AncientStats>) (args => new MegaCrit.Sts2.Core.Saves.AncientStats()
        {
          Id = (MegaCrit.Sts2.Core.Models.ModelId) args[0],
          CharStats = (List<MegaCrit.Sts2.Core.Saves.AncientCharacterStats>) args[1]
        }),
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.AncientStatsPropInit(options)),
        ConstructorParameterMetadataInitializer = MegaCritSerializerContext.\u003C\u003EO.\u003C7\u003E__AncientStatsCtorParamInit ?? (MegaCritSerializerContext.\u003C\u003EO.\u003C7\u003E__AncientStatsCtorParamInit = new Func<JsonParameterInfoValues[]>(MegaCritSerializerContext.AncientStatsCtorParamInit)),
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.AncientStats).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Saves.AncientStats>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Saves.AncientStats>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] AncientStatsPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[5];
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId> propertyInfoValues1 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.AncientStats),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Models.ModelId>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Models.ModelId>) (obj => ((MegaCrit.Sts2.Core.Saves.AncientStats) obj).Id),
      Setter = (Action<object, MegaCrit.Sts2.Core.Models.ModelId>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Id",
      JsonPropertyName = "ancient_id",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.AncientStats).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Models.ModelId), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Models.ModelId>(options, propertyInfoValues1);
    jsonPropertyInfoArray[0].IsRequired = true;
    jsonPropertyInfoArray[0].IsGetNullable = false;
    jsonPropertyInfoArray[0].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.AncientCharacterStats>> propertyInfoValues2 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.AncientCharacterStats>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.AncientStats),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Saves.AncientCharacterStats>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Saves.AncientCharacterStats>>) (obj => ((MegaCrit.Sts2.Core.Saves.AncientStats) obj).CharStats),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Saves.AncientCharacterStats>>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "CharStats",
      JsonPropertyName = "character_stats",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.AncientStats).GetProperty("CharStats", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Saves.AncientCharacterStats>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Saves.AncientCharacterStats>>(options, propertyInfoValues2);
    jsonPropertyInfoArray[1].IsGetNullable = false;
    jsonPropertyInfoArray[1].IsSetNullable = false;
    JsonPropertyInfoValues<int> propertyInfoValues3 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.AncientStats),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) null,
      Setter = (Action<object, int>) null,
      IgnoreCondition = new JsonIgnoreCondition?((JsonIgnoreCondition) 1),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "TotalVisits",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.AncientStats).GetProperty("TotalVisits", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues3);
    JsonPropertyInfoValues<int> propertyInfoValues4 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.AncientStats),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) null,
      Setter = (Action<object, int>) null,
      IgnoreCondition = new JsonIgnoreCondition?((JsonIgnoreCondition) 1),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "TotalWins",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.AncientStats).GetProperty("TotalWins", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[3] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues4);
    JsonPropertyInfoValues<int> propertyInfoValues5 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.AncientStats),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) null,
      Setter = (Action<object, int>) null,
      IgnoreCondition = new JsonIgnoreCondition?((JsonIgnoreCondition) 1),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "TotalLosses",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.AncientStats).GetProperty("TotalLosses", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[4] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues5);
    return jsonPropertyInfoArray;
  }

  private static JsonParameterInfoValues[] AncientStatsCtorParamInit()
  {
    return new JsonParameterInfoValues[2]
    {
      new JsonParameterInfoValues()
      {
        Name = "Id",
        ParameterType = typeof (MegaCrit.Sts2.Core.Models.ModelId),
        Position = 0,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "CharStats",
        ParameterType = typeof (List<MegaCrit.Sts2.Core.Saves.AncientCharacterStats>),
        Position = 1,
        IsNullable = false,
        IsMemberInitializer = true
      }
    };
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.BadgeStats> BadgeStats
  {
    get
    {
      return this._BadgeStats ?? (this._BadgeStats = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.BadgeStats>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.BadgeStats)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.BadgeStats> Create_BadgeStats(JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.BadgeStats> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.BadgeStats>(options, out jsonTypeInfo))
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.BadgeStats> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.BadgeStats>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Saves.BadgeStats>) null,
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Saves.BadgeStats>) (args => new MegaCrit.Sts2.Core.Saves.BadgeStats()
        {
          Id = (string) args[0],
          Count = (int) args[1],
          Rarity = (MegaCrit.Sts2.Core.Models.Badges.BadgeRarity) args[2]
        }),
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.BadgeStatsPropInit(options)),
        ConstructorParameterMetadataInitializer = MegaCritSerializerContext.\u003C\u003EO.\u003C8\u003E__BadgeStatsCtorParamInit ?? (MegaCritSerializerContext.\u003C\u003EO.\u003C8\u003E__BadgeStatsCtorParamInit = new Func<JsonParameterInfoValues[]>(MegaCritSerializerContext.BadgeStatsCtorParamInit)),
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.BadgeStats).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Saves.BadgeStats>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Saves.BadgeStats>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] BadgeStatsPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[3];
    JsonPropertyInfoValues<string> propertyInfoValues1 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.BadgeStats),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Saves.BadgeStats) obj).Id),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Id",
      JsonPropertyName = "id",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.BadgeStats).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues1);
    jsonPropertyInfoArray[0].IsRequired = true;
    jsonPropertyInfoArray[0].IsGetNullable = false;
    jsonPropertyInfoArray[0].IsSetNullable = false;
    JsonPropertyInfoValues<int> propertyInfoValues2 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.BadgeStats),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.BadgeStats) obj).Count),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.BadgeStats) obj).Count = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Count",
      JsonPropertyName = "count",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.BadgeStats).GetProperty("Count", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues2);
    jsonPropertyInfoArray[1].IsRequired = true;
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.Badges.BadgeRarity> propertyInfoValues3 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.Badges.BadgeRarity>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.BadgeStats),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Models.Badges.BadgeRarity>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Models.Badges.BadgeRarity>) (obj => ((MegaCrit.Sts2.Core.Saves.BadgeStats) obj).Rarity),
      Setter = (Action<object, MegaCrit.Sts2.Core.Models.Badges.BadgeRarity>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.BadgeStats) obj).Rarity = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Rarity",
      JsonPropertyName = "rarity",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.BadgeStats).GetProperty("Rarity", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Models.Badges.BadgeRarity), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Models.Badges.BadgeRarity>(options, propertyInfoValues3);
    jsonPropertyInfoArray[2].IsRequired = true;
    return jsonPropertyInfoArray;
  }

  private static JsonParameterInfoValues[] BadgeStatsCtorParamInit()
  {
    return new JsonParameterInfoValues[3]
    {
      new JsonParameterInfoValues()
      {
        Name = "Id",
        ParameterType = typeof (string),
        Position = 0,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Count",
        ParameterType = typeof (int),
        Position = 1,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Rarity",
        ParameterType = typeof (MegaCrit.Sts2.Core.Models.Badges.BadgeRarity),
        Position = 2,
        IsNullable = false,
        IsMemberInitializer = true
      }
    };
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.CardStats> CardStats
  {
    get
    {
      return this._CardStats ?? (this._CardStats = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.CardStats>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.CardStats)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.CardStats> Create_CardStats(JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.CardStats> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.CardStats>(options, out jsonTypeInfo))
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.CardStats> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.CardStats>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Saves.CardStats>) null,
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Saves.CardStats>) (args => new MegaCrit.Sts2.Core.Saves.CardStats()
        {
          Id = (MegaCrit.Sts2.Core.Models.ModelId) args[0]
        }),
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.CardStatsPropInit(options)),
        ConstructorParameterMetadataInitializer = MegaCritSerializerContext.\u003C\u003EO.\u003C9\u003E__CardStatsCtorParamInit ?? (MegaCritSerializerContext.\u003C\u003EO.\u003C9\u003E__CardStatsCtorParamInit = new Func<JsonParameterInfoValues[]>(MegaCritSerializerContext.CardStatsCtorParamInit)),
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.CardStats).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Saves.CardStats>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Saves.CardStats>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] CardStatsPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[5];
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId> propertyInfoValues1 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.CardStats),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Models.ModelId>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Models.ModelId>) (obj => ((MegaCrit.Sts2.Core.Saves.CardStats) obj).Id),
      Setter = (Action<object, MegaCrit.Sts2.Core.Models.ModelId>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Id",
      JsonPropertyName = "id",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.CardStats).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Models.ModelId), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Models.ModelId>(options, propertyInfoValues1);
    JsonPropertyInfoValues<long> propertyInfoValues2 = new JsonPropertyInfoValues<long>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.CardStats),
      Converter = (JsonConverter<long>) null,
      Getter = (Func<object, long>) (obj => ((MegaCrit.Sts2.Core.Saves.CardStats) obj).TimesPicked),
      Setter = (Action<object, long>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.CardStats) obj).TimesPicked = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "TimesPicked",
      JsonPropertyName = "times_picked",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.CardStats).GetProperty("TimesPicked", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (long), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<long>(options, propertyInfoValues2);
    JsonPropertyInfoValues<long> propertyInfoValues3 = new JsonPropertyInfoValues<long>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.CardStats),
      Converter = (JsonConverter<long>) null,
      Getter = (Func<object, long>) (obj => ((MegaCrit.Sts2.Core.Saves.CardStats) obj).TimesSkipped),
      Setter = (Action<object, long>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.CardStats) obj).TimesSkipped = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "TimesSkipped",
      JsonPropertyName = "times_skipped",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.CardStats).GetProperty("TimesSkipped", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (long), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<long>(options, propertyInfoValues3);
    JsonPropertyInfoValues<long> propertyInfoValues4 = new JsonPropertyInfoValues<long>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.CardStats),
      Converter = (JsonConverter<long>) null,
      Getter = (Func<object, long>) (obj => ((MegaCrit.Sts2.Core.Saves.CardStats) obj).TimesWon),
      Setter = (Action<object, long>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.CardStats) obj).TimesWon = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "TimesWon",
      JsonPropertyName = "times_won",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.CardStats).GetProperty("TimesWon", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (long), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[3] = JsonMetadataServices.CreatePropertyInfo<long>(options, propertyInfoValues4);
    JsonPropertyInfoValues<long> propertyInfoValues5 = new JsonPropertyInfoValues<long>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.CardStats),
      Converter = (JsonConverter<long>) null,
      Getter = (Func<object, long>) (obj => ((MegaCrit.Sts2.Core.Saves.CardStats) obj).TimesLost),
      Setter = (Action<object, long>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.CardStats) obj).TimesLost = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "TimesLost",
      JsonPropertyName = "times_lost",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.CardStats).GetProperty("TimesLost", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (long), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[4] = JsonMetadataServices.CreatePropertyInfo<long>(options, propertyInfoValues5);
    return jsonPropertyInfoArray;
  }

  private static JsonParameterInfoValues[] CardStatsCtorParamInit()
  {
    return new JsonParameterInfoValues[1]
    {
      new JsonParameterInfoValues()
      {
        Name = "Id",
        ParameterType = typeof (MegaCrit.Sts2.Core.Models.ModelId),
        Position = 0,
        IsNullable = true,
        IsMemberInitializer = true
      }
    };
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.CharacterStats> CharacterStats
  {
    get
    {
      return this._CharacterStats ?? (this._CharacterStats = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.CharacterStats>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.CharacterStats)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.CharacterStats> Create_CharacterStats(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.CharacterStats> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.CharacterStats>(options, out jsonTypeInfo))
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.CharacterStats> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.CharacterStats>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Saves.CharacterStats>) null,
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Saves.CharacterStats>) (args => new MegaCrit.Sts2.Core.Saves.CharacterStats()
        {
          Id = (MegaCrit.Sts2.Core.Models.ModelId) args[0]
        }),
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.CharacterStatsPropInit(options)),
        ConstructorParameterMetadataInitializer = MegaCritSerializerContext.\u003C\u003EO.\u003C10\u003E__CharacterStatsCtorParamInit ?? (MegaCritSerializerContext.\u003C\u003EO.\u003C10\u003E__CharacterStatsCtorParamInit = new Func<JsonParameterInfoValues[]>(MegaCritSerializerContext.CharacterStatsCtorParamInit)),
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.CharacterStats).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Saves.CharacterStats>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Saves.CharacterStats>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] CharacterStatsPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[10];
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId> propertyInfoValues1 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.CharacterStats),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Models.ModelId>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Models.ModelId>) (obj => ((MegaCrit.Sts2.Core.Saves.CharacterStats) obj).Id),
      Setter = (Action<object, MegaCrit.Sts2.Core.Models.ModelId>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Id",
      JsonPropertyName = "id",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.CharacterStats).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Models.ModelId), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Models.ModelId>(options, propertyInfoValues1);
    JsonPropertyInfoValues<int> propertyInfoValues2 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.CharacterStats),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.CharacterStats) obj).MaxAscension),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.CharacterStats) obj).MaxAscension = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "MaxAscension",
      JsonPropertyName = "max_ascension",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.CharacterStats).GetProperty("MaxAscension", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues2);
    JsonPropertyInfoValues<int> propertyInfoValues3 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.CharacterStats),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.CharacterStats) obj).PreferredAscension),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.CharacterStats) obj).PreferredAscension = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "PreferredAscension",
      JsonPropertyName = "preferred_ascension",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.CharacterStats).GetProperty("PreferredAscension", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues3);
    JsonPropertyInfoValues<int> propertyInfoValues4 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.CharacterStats),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.CharacterStats) obj).TotalWins),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.CharacterStats) obj).TotalWins = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "TotalWins",
      JsonPropertyName = "total_wins",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.CharacterStats).GetProperty("TotalWins", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[3] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues4);
    JsonPropertyInfoValues<int> propertyInfoValues5 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.CharacterStats),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.CharacterStats) obj).TotalLosses),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.CharacterStats) obj).TotalLosses = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "TotalLosses",
      JsonPropertyName = "total_losses",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.CharacterStats).GetProperty("TotalLosses", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[4] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues5);
    JsonPropertyInfoValues<long> propertyInfoValues6 = new JsonPropertyInfoValues<long>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.CharacterStats),
      Converter = (JsonConverter<long>) null,
      Getter = (Func<object, long>) (obj => ((MegaCrit.Sts2.Core.Saves.CharacterStats) obj).FastestWinTime),
      Setter = (Action<object, long>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.CharacterStats) obj).FastestWinTime = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "FastestWinTime",
      JsonPropertyName = "fastest_win_time",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.CharacterStats).GetProperty("FastestWinTime", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (long), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[5] = JsonMetadataServices.CreatePropertyInfo<long>(options, propertyInfoValues6);
    JsonPropertyInfoValues<long> propertyInfoValues7 = new JsonPropertyInfoValues<long>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.CharacterStats),
      Converter = (JsonConverter<long>) null,
      Getter = (Func<object, long>) (obj => ((MegaCrit.Sts2.Core.Saves.CharacterStats) obj).BestWinStreak),
      Setter = (Action<object, long>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.CharacterStats) obj).BestWinStreak = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "BestWinStreak",
      JsonPropertyName = "best_win_streak",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.CharacterStats).GetProperty("BestWinStreak", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (long), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[6] = JsonMetadataServices.CreatePropertyInfo<long>(options, propertyInfoValues7);
    JsonPropertyInfoValues<long> propertyInfoValues8 = new JsonPropertyInfoValues<long>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.CharacterStats),
      Converter = (JsonConverter<long>) null,
      Getter = (Func<object, long>) (obj => ((MegaCrit.Sts2.Core.Saves.CharacterStats) obj).CurrentWinStreak),
      Setter = (Action<object, long>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.CharacterStats) obj).CurrentWinStreak = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "CurrentWinStreak",
      JsonPropertyName = "current_streak",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.CharacterStats).GetProperty("CurrentWinStreak", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (long), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[7] = JsonMetadataServices.CreatePropertyInfo<long>(options, propertyInfoValues8);
    JsonPropertyInfoValues<long> propertyInfoValues9 = new JsonPropertyInfoValues<long>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.CharacterStats),
      Converter = (JsonConverter<long>) null,
      Getter = (Func<object, long>) (obj => ((MegaCrit.Sts2.Core.Saves.CharacterStats) obj).Playtime),
      Setter = (Action<object, long>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.CharacterStats) obj).Playtime = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Playtime",
      JsonPropertyName = "playtime",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.CharacterStats).GetProperty("Playtime", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (long), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[8] = JsonMetadataServices.CreatePropertyInfo<long>(options, propertyInfoValues9);
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.BadgeStats>> propertyInfoValues10 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.BadgeStats>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.CharacterStats),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Saves.BadgeStats>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Saves.BadgeStats>>) (obj => ((MegaCrit.Sts2.Core.Saves.CharacterStats) obj).Badges),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Saves.BadgeStats>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.CharacterStats) obj).Badges = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Badges",
      JsonPropertyName = "badges",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.CharacterStats).GetProperty("Badges", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Saves.BadgeStats>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[9] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Saves.BadgeStats>>(options, propertyInfoValues10);
    jsonPropertyInfoArray[9].IsGetNullable = false;
    jsonPropertyInfoArray[9].IsSetNullable = false;
    return jsonPropertyInfoArray;
  }

  private static JsonParameterInfoValues[] CharacterStatsCtorParamInit()
  {
    return new JsonParameterInfoValues[1]
    {
      new JsonParameterInfoValues()
      {
        Name = "Id",
        ParameterType = typeof (MegaCrit.Sts2.Core.Models.ModelId),
        Position = 0,
        IsNullable = true,
        IsMemberInitializer = true
      }
    };
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.EncounterStats> EncounterStats
  {
    get
    {
      return this._EncounterStats ?? (this._EncounterStats = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.EncounterStats>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.EncounterStats)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.EncounterStats> Create_EncounterStats(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.EncounterStats> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.EncounterStats>(options, out jsonTypeInfo))
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.EncounterStats> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.EncounterStats>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Saves.EncounterStats>) null,
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Saves.EncounterStats>) (args => new MegaCrit.Sts2.Core.Saves.EncounterStats()
        {
          Id = (MegaCrit.Sts2.Core.Models.ModelId) args[0],
          FightStats = (List<MegaCrit.Sts2.Core.Saves.FightStats>) args[1]
        }),
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.EncounterStatsPropInit(options)),
        ConstructorParameterMetadataInitializer = MegaCritSerializerContext.\u003C\u003EO.\u003C11\u003E__EncounterStatsCtorParamInit ?? (MegaCritSerializerContext.\u003C\u003EO.\u003C11\u003E__EncounterStatsCtorParamInit = new Func<JsonParameterInfoValues[]>(MegaCritSerializerContext.EncounterStatsCtorParamInit)),
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.EncounterStats).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Saves.EncounterStats>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Saves.EncounterStats>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] EncounterStatsPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[4];
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId> propertyInfoValues1 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.EncounterStats),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Models.ModelId>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Models.ModelId>) (obj => ((MegaCrit.Sts2.Core.Saves.EncounterStats) obj).Id),
      Setter = (Action<object, MegaCrit.Sts2.Core.Models.ModelId>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Id",
      JsonPropertyName = "encounter_id",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.EncounterStats).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Models.ModelId), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Models.ModelId>(options, propertyInfoValues1);
    jsonPropertyInfoArray[0].IsRequired = true;
    jsonPropertyInfoArray[0].IsGetNullable = false;
    jsonPropertyInfoArray[0].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.FightStats>> propertyInfoValues2 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.FightStats>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.EncounterStats),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Saves.FightStats>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Saves.FightStats>>) (obj => ((MegaCrit.Sts2.Core.Saves.EncounterStats) obj).FightStats),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Saves.FightStats>>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "FightStats",
      JsonPropertyName = "fight_stats",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.EncounterStats).GetProperty("FightStats", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Saves.FightStats>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Saves.FightStats>>(options, propertyInfoValues2);
    jsonPropertyInfoArray[1].IsGetNullable = false;
    jsonPropertyInfoArray[1].IsSetNullable = false;
    JsonPropertyInfoValues<int> propertyInfoValues3 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.EncounterStats),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) null,
      Setter = (Action<object, int>) null,
      IgnoreCondition = new JsonIgnoreCondition?((JsonIgnoreCondition) 1),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "TotalWins",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.EncounterStats).GetProperty("TotalWins", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues3);
    JsonPropertyInfoValues<int> propertyInfoValues4 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.EncounterStats),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) null,
      Setter = (Action<object, int>) null,
      IgnoreCondition = new JsonIgnoreCondition?((JsonIgnoreCondition) 1),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "TotalLosses",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.EncounterStats).GetProperty("TotalLosses", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[3] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues4);
    return jsonPropertyInfoArray;
  }

  private static JsonParameterInfoValues[] EncounterStatsCtorParamInit()
  {
    return new JsonParameterInfoValues[2]
    {
      new JsonParameterInfoValues()
      {
        Name = "Id",
        ParameterType = typeof (MegaCrit.Sts2.Core.Models.ModelId),
        Position = 0,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "FightStats",
        ParameterType = typeof (List<MegaCrit.Sts2.Core.Saves.FightStats>),
        Position = 1,
        IsNullable = false,
        IsMemberInitializer = true
      }
    };
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.EnemyStats> EnemyStats
  {
    get
    {
      return this._EnemyStats ?? (this._EnemyStats = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.EnemyStats>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.EnemyStats)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.EnemyStats> Create_EnemyStats(JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.EnemyStats> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.EnemyStats>(options, out jsonTypeInfo))
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.EnemyStats> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.EnemyStats>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Saves.EnemyStats>) null,
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Saves.EnemyStats>) (args => new MegaCrit.Sts2.Core.Saves.EnemyStats()
        {
          Id = (MegaCrit.Sts2.Core.Models.ModelId) args[0],
          FightStats = (List<MegaCrit.Sts2.Core.Saves.FightStats>) args[1]
        }),
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.EnemyStatsPropInit(options)),
        ConstructorParameterMetadataInitializer = MegaCritSerializerContext.\u003C\u003EO.\u003C12\u003E__EnemyStatsCtorParamInit ?? (MegaCritSerializerContext.\u003C\u003EO.\u003C12\u003E__EnemyStatsCtorParamInit = new Func<JsonParameterInfoValues[]>(MegaCritSerializerContext.EnemyStatsCtorParamInit)),
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.EnemyStats).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Saves.EnemyStats>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Saves.EnemyStats>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] EnemyStatsPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[4];
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId> propertyInfoValues1 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.EnemyStats),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Models.ModelId>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Models.ModelId>) (obj => ((MegaCrit.Sts2.Core.Saves.EnemyStats) obj).Id),
      Setter = (Action<object, MegaCrit.Sts2.Core.Models.ModelId>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Id",
      JsonPropertyName = "enemy_id",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.EnemyStats).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Models.ModelId), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Models.ModelId>(options, propertyInfoValues1);
    jsonPropertyInfoArray[0].IsRequired = true;
    jsonPropertyInfoArray[0].IsGetNullable = false;
    jsonPropertyInfoArray[0].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.FightStats>> propertyInfoValues2 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.FightStats>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.EnemyStats),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Saves.FightStats>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Saves.FightStats>>) (obj => ((MegaCrit.Sts2.Core.Saves.EnemyStats) obj).FightStats),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Saves.FightStats>>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "FightStats",
      JsonPropertyName = "fight_stats",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.EnemyStats).GetProperty("FightStats", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Saves.FightStats>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Saves.FightStats>>(options, propertyInfoValues2);
    jsonPropertyInfoArray[1].IsGetNullable = false;
    jsonPropertyInfoArray[1].IsSetNullable = false;
    JsonPropertyInfoValues<int> propertyInfoValues3 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.EnemyStats),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) null,
      Setter = (Action<object, int>) null,
      IgnoreCondition = new JsonIgnoreCondition?((JsonIgnoreCondition) 1),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "TotalWins",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.EnemyStats).GetProperty("TotalWins", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues3);
    JsonPropertyInfoValues<int> propertyInfoValues4 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.EnemyStats),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) null,
      Setter = (Action<object, int>) null,
      IgnoreCondition = new JsonIgnoreCondition?((JsonIgnoreCondition) 1),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "TotalLosses",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.EnemyStats).GetProperty("TotalLosses", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[3] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues4);
    return jsonPropertyInfoArray;
  }

  private static JsonParameterInfoValues[] EnemyStatsCtorParamInit()
  {
    return new JsonParameterInfoValues[2]
    {
      new JsonParameterInfoValues()
      {
        Name = "Id",
        ParameterType = typeof (MegaCrit.Sts2.Core.Models.ModelId),
        Position = 0,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "FightStats",
        ParameterType = typeof (List<MegaCrit.Sts2.Core.Saves.FightStats>),
        Position = 1,
        IsNullable = false,
        IsMemberInitializer = true
      }
    };
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.EpochState> EpochState
  {
    get
    {
      return this._EpochState ?? (this._EpochState = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.EpochState>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.EpochState)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.EpochState> Create_EpochState(JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.EpochState> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.EpochState>(options, out jsonTypeInfo))
      jsonTypeInfo = JsonMetadataServices.CreateValueInfo<MegaCrit.Sts2.Core.Saves.EpochState>(options, (JsonConverter) JsonMetadataServices.GetEnumConverter<MegaCrit.Sts2.Core.Saves.EpochState>(options));
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.FightStats> FightStats
  {
    get
    {
      return this._FightStats ?? (this._FightStats = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.FightStats>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.FightStats)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.FightStats> Create_FightStats(JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.FightStats> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.FightStats>(options, out jsonTypeInfo))
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.FightStats> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.FightStats>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Saves.FightStats>) null,
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Saves.FightStats>) (args => new MegaCrit.Sts2.Core.Saves.FightStats()
        {
          Character = (MegaCrit.Sts2.Core.Models.ModelId) args[0]
        }),
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.FightStatsPropInit(options)),
        ConstructorParameterMetadataInitializer = MegaCritSerializerContext.\u003C\u003EO.\u003C13\u003E__FightStatsCtorParamInit ?? (MegaCritSerializerContext.\u003C\u003EO.\u003C13\u003E__FightStatsCtorParamInit = new Func<JsonParameterInfoValues[]>(MegaCritSerializerContext.FightStatsCtorParamInit)),
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.FightStats).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Saves.FightStats>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Saves.FightStats>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] FightStatsPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[3];
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId> propertyInfoValues1 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.FightStats),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Models.ModelId>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Models.ModelId>) (obj => ((MegaCrit.Sts2.Core.Saves.FightStats) obj).Character),
      Setter = (Action<object, MegaCrit.Sts2.Core.Models.ModelId>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Character",
      JsonPropertyName = "character",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.FightStats).GetProperty("Character", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Models.ModelId), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Models.ModelId>(options, propertyInfoValues1);
    jsonPropertyInfoArray[0].IsRequired = true;
    jsonPropertyInfoArray[0].IsGetNullable = false;
    jsonPropertyInfoArray[0].IsSetNullable = false;
    JsonPropertyInfoValues<int> propertyInfoValues2 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.FightStats),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.FightStats) obj).Wins),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.FightStats) obj).Wins = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Wins",
      JsonPropertyName = "wins",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.FightStats).GetProperty("Wins", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues2);
    JsonPropertyInfoValues<int> propertyInfoValues3 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.FightStats),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.FightStats) obj).Losses),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.FightStats) obj).Losses = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Losses",
      JsonPropertyName = "losses",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.FightStats).GetProperty("Losses", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues3);
    return jsonPropertyInfoArray;
  }

  private static JsonParameterInfoValues[] FightStatsCtorParamInit()
  {
    return new JsonParameterInfoValues[1]
    {
      new JsonParameterInfoValues()
      {
        Name = "Character",
        ParameterType = typeof (MegaCrit.Sts2.Core.Models.ModelId),
        Position = 0,
        IsNullable = false,
        IsMemberInitializer = true
      }
    };
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawingLine> SerializableMapDrawingLine
  {
    get
    {
      return this._SerializableMapDrawingLine ?? (this._SerializableMapDrawingLine = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawingLine>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawingLine)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawingLine> Create_SerializableMapDrawingLine(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawingLine> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawingLine>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawingLine> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawingLine>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawingLine>) (() => new MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawingLine()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawingLine>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.SerializableMapDrawingLinePropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawingLine).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawingLine>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawingLine>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] SerializableMapDrawingLinePropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[2];
    JsonPropertyInfoValues<bool> propertyInfoValues1 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawingLine),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) (obj => ((MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawingLine) obj).isEraser),
      Setter = (Action<object, bool>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawingLine) obj).isEraser = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "isEraser",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawingLine).GetField("isEraser", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues1);
    JsonPropertyInfoValues<List<Godot.Vector2>> propertyInfoValues2 = new JsonPropertyInfoValues<List<Godot.Vector2>>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawingLine),
      Converter = (JsonConverter<List<Godot.Vector2>>) null,
      Getter = (Func<object, List<Godot.Vector2>>) (obj => ((MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawingLine) obj).mapPoints),
      Setter = (Action<object, List<Godot.Vector2>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawingLine) obj).mapPoints = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "mapPoints",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawingLine).GetField("mapPoints", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<List<Godot.Vector2>>(options, propertyInfoValues2);
    jsonPropertyInfoArray[1].IsGetNullable = false;
    jsonPropertyInfoArray[1].IsSetNullable = false;
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawings> SerializableMapDrawings
  {
    get
    {
      return this._SerializableMapDrawings ?? (this._SerializableMapDrawings = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawings>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawings)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawings> Create_SerializableMapDrawings(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawings> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawings>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawings> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawings>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawings>) (() => new MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawings()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawings>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.SerializableMapDrawingsPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawings).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawings>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawings>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] SerializableMapDrawingsPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[1];
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializablePlayerMapDrawings>> propertyInfoValues = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializablePlayerMapDrawings>>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawings),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializablePlayerMapDrawings>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializablePlayerMapDrawings>>) (obj => ((MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawings) obj).drawings),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializablePlayerMapDrawings>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawings) obj).drawings = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "drawings",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawings).GetField("drawings", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializablePlayerMapDrawings>>(options, propertyInfoValues);
    jsonPropertyInfoArray[0].IsGetNullable = false;
    jsonPropertyInfoArray[0].IsSetNullable = false;
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializablePlayerMapDrawings> SerializablePlayerMapDrawings
  {
    get
    {
      return this._SerializablePlayerMapDrawings ?? (this._SerializablePlayerMapDrawings = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializablePlayerMapDrawings>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.MapDrawing.SerializablePlayerMapDrawings)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializablePlayerMapDrawings> Create_SerializablePlayerMapDrawings(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializablePlayerMapDrawings> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializablePlayerMapDrawings>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializablePlayerMapDrawings> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializablePlayerMapDrawings>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializablePlayerMapDrawings>) (() => new MegaCrit.Sts2.Core.Saves.MapDrawing.SerializablePlayerMapDrawings()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Saves.MapDrawing.SerializablePlayerMapDrawings>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.SerializablePlayerMapDrawingsPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.MapDrawing.SerializablePlayerMapDrawings).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Saves.MapDrawing.SerializablePlayerMapDrawings>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializablePlayerMapDrawings>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] SerializablePlayerMapDrawingsPropInit(
    JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[2];
    JsonPropertyInfoValues<ulong> propertyInfoValues1 = new JsonPropertyInfoValues<ulong>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.MapDrawing.SerializablePlayerMapDrawings),
      Converter = (JsonConverter<ulong>) null,
      Getter = (Func<object, ulong>) (obj => ((MegaCrit.Sts2.Core.Saves.MapDrawing.SerializablePlayerMapDrawings) obj).playerId),
      Setter = (Action<object, ulong>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.MapDrawing.SerializablePlayerMapDrawings) obj).playerId = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "playerId",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.MapDrawing.SerializablePlayerMapDrawings).GetField("playerId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<ulong>(options, propertyInfoValues1);
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawingLine>> propertyInfoValues2 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawingLine>>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.MapDrawing.SerializablePlayerMapDrawings),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawingLine>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawingLine>>) (obj => ((MegaCrit.Sts2.Core.Saves.MapDrawing.SerializablePlayerMapDrawings) obj).lines),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawingLine>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.MapDrawing.SerializablePlayerMapDrawings) obj).lines = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "lines",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.MapDrawing.SerializablePlayerMapDrawings).GetField("lines", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawingLine>>(options, propertyInfoValues2);
    jsonPropertyInfoArray[1].IsGetNullable = false;
    jsonPropertyInfoArray[1].IsSetNullable = false;
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Migrations.MigratingData> MigratingData
  {
    get
    {
      return this._MigratingData ?? (this._MigratingData = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Migrations.MigratingData>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.Migrations.MigratingData)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Migrations.MigratingData> Create_MigratingData(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Migrations.MigratingData> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.Migrations.MigratingData>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Migrations.MigratingData> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Migrations.MigratingData>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Saves.Migrations.MigratingData>) null,
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Saves.Migrations.MigratingData>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.MigratingDataPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) null,
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Saves.Migrations.MigratingData>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Saves.Migrations.MigratingData>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] MigratingDataPropInit(JsonSerializerOptions options)
  {
    return new JsonPropertyInfo[0];
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.PrefsSave> PrefsSave
  {
    get
    {
      return this._PrefsSave ?? (this._PrefsSave = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.PrefsSave>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.PrefsSave)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.PrefsSave> Create_PrefsSave(JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.PrefsSave> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.PrefsSave>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.PrefsSave> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.PrefsSave>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Saves.PrefsSave>) (() => new MegaCrit.Sts2.Core.Saves.PrefsSave()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Saves.PrefsSave>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.PrefsSavePropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.PrefsSave).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Saves.PrefsSave>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Saves.PrefsSave>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] PrefsSavePropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[12];
    JsonPropertyInfoValues<int> propertyInfoValues1 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.PrefsSave),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.PrefsSave) obj).SchemaVersion),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.PrefsSave) obj).SchemaVersion = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "SchemaVersion",
      JsonPropertyName = "schema_version",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.PrefsSave).GetProperty("SchemaVersion", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues1);
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Settings.FastModeType> propertyInfoValues2 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Settings.FastModeType>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.PrefsSave),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Settings.FastModeType>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Settings.FastModeType>) (obj => ((MegaCrit.Sts2.Core.Saves.PrefsSave) obj).FastMode),
      Setter = (Action<object, MegaCrit.Sts2.Core.Settings.FastModeType>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.PrefsSave) obj).FastMode = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "FastMode",
      JsonPropertyName = "fast_mode",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.PrefsSave).GetProperty("FastMode", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Settings.FastModeType), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Settings.FastModeType>(options, propertyInfoValues2);
    JsonPropertyInfoValues<bool> propertyInfoValues3 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.PrefsSave),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) (obj => ((MegaCrit.Sts2.Core.Saves.PrefsSave) obj).PhobiaMode),
      Setter = (Action<object, bool>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.PrefsSave) obj).PhobiaMode = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "PhobiaMode",
      JsonPropertyName = "phobia_mode",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.PrefsSave).GetProperty("PhobiaMode", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (bool), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues3);
    JsonPropertyInfoValues<int> propertyInfoValues4 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.PrefsSave),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.PrefsSave) obj).ScreenShakeOptionIndex),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.PrefsSave) obj).ScreenShakeOptionIndex = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "ScreenShakeOptionIndex",
      JsonPropertyName = "screenshake",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.PrefsSave).GetProperty("ScreenShakeOptionIndex", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[3] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues4);
    JsonPropertyInfoValues<bool> propertyInfoValues5 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.PrefsSave),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) (obj => ((MegaCrit.Sts2.Core.Saves.PrefsSave) obj).ShowRunTimer),
      Setter = (Action<object, bool>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.PrefsSave) obj).ShowRunTimer = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "ShowRunTimer",
      JsonPropertyName = "show_run_timer",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.PrefsSave).GetProperty("ShowRunTimer", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (bool), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[4] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues5);
    JsonPropertyInfoValues<bool> propertyInfoValues6 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.PrefsSave),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) (obj => ((MegaCrit.Sts2.Core.Saves.PrefsSave) obj).ShowCardIndices),
      Setter = (Action<object, bool>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.PrefsSave) obj).ShowCardIndices = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "ShowCardIndices",
      JsonPropertyName = "show_card_indices",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.PrefsSave).GetProperty("ShowCardIndices", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (bool), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[5] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues6);
    JsonPropertyInfoValues<bool> propertyInfoValues7 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.PrefsSave),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) (obj => ((MegaCrit.Sts2.Core.Saves.PrefsSave) obj).UploadData),
      Setter = (Action<object, bool>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.PrefsSave) obj).UploadData = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "UploadData",
      JsonPropertyName = "upload_data",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.PrefsSave).GetProperty("UploadData", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (bool), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[6] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues7);
    JsonPropertyInfoValues<bool> propertyInfoValues8 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.PrefsSave),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) (obj => ((MegaCrit.Sts2.Core.Saves.PrefsSave) obj).MuteInBackground),
      Setter = (Action<object, bool>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.PrefsSave) obj).MuteInBackground = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "MuteInBackground",
      JsonPropertyName = "mute_in_background",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.PrefsSave).GetProperty("MuteInBackground", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (bool), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[7] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues8);
    JsonPropertyInfoValues<bool> propertyInfoValues9 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.PrefsSave),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) (obj => ((MegaCrit.Sts2.Core.Saves.PrefsSave) obj).IsLongPressEnabled),
      Setter = (Action<object, bool>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.PrefsSave) obj).IsLongPressEnabled = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "IsLongPressEnabled",
      JsonPropertyName = "long_press",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.PrefsSave).GetProperty("IsLongPressEnabled", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (bool), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[8] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues9);
    JsonPropertyInfoValues<bool> propertyInfoValues10 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.PrefsSave),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) (obj => ((MegaCrit.Sts2.Core.Saves.PrefsSave) obj).TextEffectsEnabled),
      Setter = (Action<object, bool>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.PrefsSave) obj).TextEffectsEnabled = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "TextEffectsEnabled",
      JsonPropertyName = "text_effects_enabled",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.PrefsSave).GetProperty("TextEffectsEnabled", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (bool), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[9] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues10);
    JsonPropertyInfoValues<bool> propertyInfoValues11 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.PrefsSave),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) (obj => ((MegaCrit.Sts2.Core.Saves.PrefsSave) obj).ShowMultiplayerDrawings),
      Setter = (Action<object, bool>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.PrefsSave) obj).ShowMultiplayerDrawings = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "ShowMultiplayerDrawings",
      JsonPropertyName = "show_mp_drawings",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.PrefsSave).GetProperty("ShowMultiplayerDrawings", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (bool), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[10] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues11);
    JsonPropertyInfoValues<bool> propertyInfoValues12 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.PrefsSave),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) (obj => ((MegaCrit.Sts2.Core.Saves.PrefsSave) obj).IsBestiaryActionsPreferred),
      Setter = (Action<object, bool>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.PrefsSave) obj).IsBestiaryActionsPreferred = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "IsBestiaryActionsPreferred",
      JsonPropertyName = "bestiary_actions_preferred",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.PrefsSave).GetProperty("IsBestiaryActionsPreferred", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (bool), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[11] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues12);
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.ProfileSave> ProfileSave
  {
    get
    {
      return this._ProfileSave ?? (this._ProfileSave = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.ProfileSave>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.ProfileSave)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.ProfileSave> Create_ProfileSave(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.ProfileSave> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.ProfileSave>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.ProfileSave> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.ProfileSave>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Saves.ProfileSave>) (() => new MegaCrit.Sts2.Core.Saves.ProfileSave()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Saves.ProfileSave>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.ProfileSavePropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.ProfileSave).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Saves.ProfileSave>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Saves.ProfileSave>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] ProfileSavePropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[2];
    JsonPropertyInfoValues<int> propertyInfoValues1 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.ProfileSave),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.ProfileSave) obj).SchemaVersion),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.ProfileSave) obj).SchemaVersion = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "SchemaVersion",
      JsonPropertyName = "schema_version",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.ProfileSave).GetProperty("SchemaVersion", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues1);
    JsonPropertyInfoValues<int> propertyInfoValues2 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.ProfileSave),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.ProfileSave) obj).LastProfileId),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.ProfileSave) obj).LastProfileId = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "LastProfileId",
      JsonPropertyName = "last_profile_id",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.ProfileSave).GetProperty("LastProfileId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues2);
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties> SavedProperties
  {
    get
    {
      return this._SavedProperties ?? (this._SavedProperties = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties> Create_SavedProperties(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties>) (() => new MegaCrit.Sts2.Core.Saves.Runs.SavedProperties()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Saves.Runs.SavedProperties>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.SavedPropertiesPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Saves.Runs.SavedProperties>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] SavedPropertiesPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[7];
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int>>> propertyInfoValues1 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int>>>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int>>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int>>>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SavedProperties) obj).ints),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int>>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SavedProperties) obj).ints = value),
      IgnoreCondition = new JsonIgnoreCondition?((JsonIgnoreCondition) 3),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "ints",
      JsonPropertyName = "ints",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties).GetField("ints", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int>>>(options, propertyInfoValues1);
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<bool>>> propertyInfoValues2 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<bool>>>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<bool>>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<bool>>>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SavedProperties) obj).bools),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<bool>>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SavedProperties) obj).bools = value),
      IgnoreCondition = new JsonIgnoreCondition?((JsonIgnoreCondition) 3),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "bools",
      JsonPropertyName = "bools",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties).GetField("bools", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<bool>>>(options, propertyInfoValues2);
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<string>>> propertyInfoValues3 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<string>>>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<string>>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<string>>>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SavedProperties) obj).strings),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<string>>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SavedProperties) obj).strings = value),
      IgnoreCondition = new JsonIgnoreCondition?((JsonIgnoreCondition) 3),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "strings",
      JsonPropertyName = "strings",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties).GetField("strings", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<string>>>(options, propertyInfoValues3);
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int[]>>> propertyInfoValues4 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int[]>>>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int[]>>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int[]>>>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SavedProperties) obj).intArrays),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int[]>>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SavedProperties) obj).intArrays = value),
      IgnoreCondition = new JsonIgnoreCondition?((JsonIgnoreCondition) 3),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "intArrays",
      JsonPropertyName = "int_arrays",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties).GetField("intArrays", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[3] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int[]>>>(options, propertyInfoValues4);
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Models.ModelId>>> propertyInfoValues5 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Models.ModelId>>>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Models.ModelId>>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Models.ModelId>>>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SavedProperties) obj).modelIds),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Models.ModelId>>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SavedProperties) obj).modelIds = value),
      IgnoreCondition = new JsonIgnoreCondition?((JsonIgnoreCondition) 3),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "modelIds",
      JsonPropertyName = "model_ids",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties).GetField("modelIds", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[4] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Models.ModelId>>>(options, propertyInfoValues5);
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>> propertyInfoValues6 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SavedProperties) obj).cards),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SavedProperties) obj).cards = value),
      IgnoreCondition = new JsonIgnoreCondition?((JsonIgnoreCondition) 3),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "cards",
      JsonPropertyName = "cards",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties).GetField("cards", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[5] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>>(options, propertyInfoValues6);
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>>> propertyInfoValues7 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>>>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>>>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SavedProperties) obj).cardArrays),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SavedProperties) obj).cardArrays = value),
      IgnoreCondition = new JsonIgnoreCondition?((JsonIgnoreCondition) 3),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "cardArrays",
      JsonPropertyName = "card_arrays",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties).GetField("cardArrays", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[6] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>>>(options, propertyInfoValues7);
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<bool>> SavedPropertyBoolean
  {
    get
    {
      return this._SavedPropertyBoolean ?? (this._SavedPropertyBoolean = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<bool>>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<bool>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<bool>> Create_SavedPropertyBoolean(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<bool>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<bool>>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<bool>> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<bool>>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<bool>>) (() => new MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<bool>()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<bool>>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.SavedPropertyBooleanPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<bool>).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<bool>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<bool>>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] SavedPropertyBooleanPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[2];
    JsonPropertyInfoValues<string> propertyInfoValues1 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<bool>),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<bool>) obj).name),
      Setter = (Action<object, string>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<bool>>(obj).name = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "name",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<bool>).GetField("name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues1);
    jsonPropertyInfoArray[0].IsGetNullable = false;
    jsonPropertyInfoArray[0].IsSetNullable = false;
    JsonPropertyInfoValues<bool> propertyInfoValues2 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<bool>),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<bool>) obj).value),
      Setter = (Action<object, bool>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<bool>>(obj).value = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "value",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<bool>).GetField("value", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues2);
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Models.ModelId>> SavedPropertyModelId
  {
    get
    {
      return this._SavedPropertyModelId ?? (this._SavedPropertyModelId = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Models.ModelId>>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Models.ModelId>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Models.ModelId>> Create_SavedPropertyModelId(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Models.ModelId>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Models.ModelId>>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Models.ModelId>> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Models.ModelId>>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Models.ModelId>>) (() => new MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Models.ModelId>()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Models.ModelId>>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.SavedPropertyModelIdPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Models.ModelId>).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Models.ModelId>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Models.ModelId>>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] SavedPropertyModelIdPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[2];
    JsonPropertyInfoValues<string> propertyInfoValues1 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Models.ModelId>),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Models.ModelId>) obj).name),
      Setter = (Action<object, string>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Models.ModelId>>(obj).name = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "name",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Models.ModelId>).GetField("name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues1);
    jsonPropertyInfoArray[0].IsGetNullable = false;
    jsonPropertyInfoArray[0].IsSetNullable = false;
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId> propertyInfoValues2 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Models.ModelId>),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Models.ModelId>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Models.ModelId>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Models.ModelId>) obj).value),
      Setter = (Action<object, MegaCrit.Sts2.Core.Models.ModelId>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Models.ModelId>>(obj).value = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "value",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Models.ModelId>).GetField("value", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Models.ModelId>(options, propertyInfoValues2);
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>> SavedPropertySerializableCardArray
  {
    get
    {
      return this._SavedPropertySerializableCardArray ?? (this._SavedPropertySerializableCardArray = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>> Create_SavedPropertySerializableCardArray(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>>) (() => new MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.SavedPropertySerializableCardArrayPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] SavedPropertySerializableCardArrayPropInit(
    JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[2];
    JsonPropertyInfoValues<string> propertyInfoValues1 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>) obj).name),
      Setter = (Action<object, string>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>>(obj).name = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "name",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>).GetField("name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues1);
    jsonPropertyInfoArray[0].IsGetNullable = false;
    jsonPropertyInfoArray[0].IsSetNullable = false;
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]> propertyInfoValues2 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>) obj).value),
      Setter = (Action<object, MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>>(obj).value = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "value",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>).GetField("value", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>(options, propertyInfoValues2);
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>> SavedPropertySerializableCard
  {
    get
    {
      return this._SavedPropertySerializableCard ?? (this._SavedPropertySerializableCard = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>> Create_SavedPropertySerializableCard(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>) (() => new MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.SavedPropertySerializableCardPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] SavedPropertySerializableCardPropInit(
    JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[2];
    JsonPropertyInfoValues<string> propertyInfoValues1 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>) obj).name),
      Setter = (Action<object, string>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>(obj).name = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "name",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>).GetField("name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues1);
    jsonPropertyInfoArray[0].IsGetNullable = false;
    jsonPropertyInfoArray[0].IsSetNullable = false;
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard> propertyInfoValues2 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>) obj).value),
      Setter = (Action<object, MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>(obj).value = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "value",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>).GetField("value", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>(options, propertyInfoValues2);
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int[]>> SavedPropertyInt32Array
  {
    get
    {
      return this._SavedPropertyInt32Array ?? (this._SavedPropertyInt32Array = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int[]>>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int[]>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int[]>> Create_SavedPropertyInt32Array(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int[]>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int[]>>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int[]>> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int[]>>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int[]>>) (() => new MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int[]>()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int[]>>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.SavedPropertyInt32ArrayPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int[]>).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int[]>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int[]>>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] SavedPropertyInt32ArrayPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[2];
    JsonPropertyInfoValues<string> propertyInfoValues1 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int[]>),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int[]>) obj).name),
      Setter = (Action<object, string>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int[]>>(obj).name = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "name",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int[]>).GetField("name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues1);
    jsonPropertyInfoArray[0].IsGetNullable = false;
    jsonPropertyInfoArray[0].IsSetNullable = false;
    JsonPropertyInfoValues<int[]> propertyInfoValues2 = new JsonPropertyInfoValues<int[]>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int[]>),
      Converter = (JsonConverter<int[]>) null,
      Getter = (Func<object, int[]>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int[]>) obj).value),
      Setter = (Action<object, int[]>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int[]>>(obj).value = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "value",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int[]>).GetField("value", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<int[]>(options, propertyInfoValues2);
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int>> SavedPropertyInt32
  {
    get
    {
      return this._SavedPropertyInt32 ?? (this._SavedPropertyInt32 = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int>>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int>> Create_SavedPropertyInt32(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int>>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int>> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int>>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int>>) (() => new MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int>()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int>>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.SavedPropertyInt32PropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int>).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int>>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] SavedPropertyInt32PropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[2];
    JsonPropertyInfoValues<string> propertyInfoValues1 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int>),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int>) obj).name),
      Setter = (Action<object, string>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int>>(obj).name = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "name",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int>).GetField("name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues1);
    jsonPropertyInfoArray[0].IsGetNullable = false;
    jsonPropertyInfoArray[0].IsSetNullable = false;
    JsonPropertyInfoValues<int> propertyInfoValues2 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int>),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int>) obj).value),
      Setter = (Action<object, int>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int>>(obj).value = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "value",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int>).GetField("value", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues2);
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<string>> SavedPropertyString
  {
    get
    {
      return this._SavedPropertyString ?? (this._SavedPropertyString = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<string>>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<string>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<string>> Create_SavedPropertyString(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<string>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<string>>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<string>> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<string>>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<string>>) (() => new MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<string>()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<string>>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.SavedPropertyStringPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<string>).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<string>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<string>>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] SavedPropertyStringPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[2];
    JsonPropertyInfoValues<string> propertyInfoValues1 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<string>),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<string>) obj).name),
      Setter = (Action<object, string>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<string>>(obj).name = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "name",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<string>).GetField("name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues1);
    jsonPropertyInfoArray[0].IsGetNullable = false;
    jsonPropertyInfoArray[0].IsSetNullable = false;
    JsonPropertyInfoValues<string> propertyInfoValues2 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<string>),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<string>) obj).value),
      Setter = (Action<object, string>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<string>>(obj).value = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "value",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<string>).GetField("value", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues2);
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap> SerializableActMap
  {
    get
    {
      return this._SerializableActMap ?? (this._SerializableActMap = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap> Create_SerializableActMap(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap>) (() => new MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.SerializableActMapPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] SerializableActMapPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[7];
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint>> propertyInfoValues1 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint>>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap) obj).Points),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap) obj).Points = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Points",
      JsonPropertyName = "points",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap).GetProperty("Points", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint>>(options, propertyInfoValues1);
    jsonPropertyInfoArray[0].IsGetNullable = false;
    jsonPropertyInfoArray[0].IsSetNullable = false;
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint> propertyInfoValues2 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap) obj).BossPoint),
      Setter = (Action<object, MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap) obj).BossPoint = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "BossPoint",
      JsonPropertyName = "boss",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap).GetProperty("BossPoint", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint>(options, propertyInfoValues2);
    jsonPropertyInfoArray[1].IsGetNullable = false;
    jsonPropertyInfoArray[1].IsSetNullable = false;
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint> propertyInfoValues3 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap) obj).SecondBossPoint),
      Setter = (Action<object, MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap) obj).SecondBossPoint = value),
      IgnoreCondition = new JsonIgnoreCondition?((JsonIgnoreCondition) 3),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "SecondBossPoint",
      JsonPropertyName = "second_boss",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap).GetProperty("SecondBossPoint", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint>(options, propertyInfoValues3);
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint> propertyInfoValues4 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap) obj).StartingPoint),
      Setter = (Action<object, MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap) obj).StartingPoint = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "StartingPoint",
      JsonPropertyName = "start",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap).GetProperty("StartingPoint", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[3] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint>(options, propertyInfoValues4);
    jsonPropertyInfoArray[3].IsGetNullable = false;
    jsonPropertyInfoArray[3].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Map.MapCoord>> propertyInfoValues5 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Map.MapCoord>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Map.MapCoord>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Map.MapCoord>>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap) obj).StartMapPointCoords),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Map.MapCoord>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap) obj).StartMapPointCoords = value),
      IgnoreCondition = new JsonIgnoreCondition?((JsonIgnoreCondition) 3),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "StartMapPointCoords",
      JsonPropertyName = "start_coords",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap).GetProperty("StartMapPointCoords", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Map.MapCoord>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[4] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Map.MapCoord>>(options, propertyInfoValues5);
    JsonPropertyInfoValues<int> propertyInfoValues6 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap) obj).GridWidth),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap) obj).GridWidth = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "GridWidth",
      JsonPropertyName = "width",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap).GetProperty("GridWidth", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[5] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues6);
    JsonPropertyInfoValues<int> propertyInfoValues7 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap) obj).GridHeight),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap) obj).GridHeight = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "GridHeight",
      JsonPropertyName = "height",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap).GetProperty("GridHeight", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[6] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues7);
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel> SerializableActModel
  {
    get
    {
      return this._SerializableActModel ?? (this._SerializableActModel = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel> Create_SerializableActModel(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel>) (() => new MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.SerializableActModelPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] SerializableActModelPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[3];
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId> propertyInfoValues1 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Models.ModelId>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Models.ModelId>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel) obj).Id),
      Setter = (Action<object, MegaCrit.Sts2.Core.Models.ModelId>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel) obj).Id = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Id",
      JsonPropertyName = "id",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Models.ModelId), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Models.ModelId>(options, propertyInfoValues1);
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet> propertyInfoValues2 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel) obj).SerializableRooms),
      Setter = (Action<object, MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel) obj).SerializableRooms = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "SerializableRooms",
      JsonPropertyName = "rooms",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel).GetProperty("SerializableRooms", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet>(options, propertyInfoValues2);
    jsonPropertyInfoArray[1].IsGetNullable = false;
    jsonPropertyInfoArray[1].IsSetNullable = false;
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap> propertyInfoValues3 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel) obj).SavedMap),
      Setter = (Action<object, MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel) obj).SavedMap = value),
      IgnoreCondition = new JsonIgnoreCondition?((JsonIgnoreCondition) 3),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "SavedMap",
      JsonPropertyName = "saved_map",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel).GetProperty("SavedMap", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap>(options, propertyInfoValues3);
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableBadge> SerializableBadge
  {
    get
    {
      return this._SerializableBadge ?? (this._SerializableBadge = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableBadge>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableBadge)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableBadge> Create_SerializableBadge(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableBadge> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.Runs.SerializableBadge>(options, out jsonTypeInfo))
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableBadge> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableBadge>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Saves.Runs.SerializableBadge>) null,
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Saves.Runs.SerializableBadge>) (args => new MegaCrit.Sts2.Core.Saves.Runs.SerializableBadge()
        {
          Id = (string) args[0],
          Rarity = (MegaCrit.Sts2.Core.Models.Badges.BadgeRarity) args[1]
        }),
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.SerializableBadgePropInit(options)),
        ConstructorParameterMetadataInitializer = MegaCritSerializerContext.\u003C\u003EO.\u003C14\u003E__SerializableBadgeCtorParamInit ?? (MegaCritSerializerContext.\u003C\u003EO.\u003C14\u003E__SerializableBadgeCtorParamInit = new Func<JsonParameterInfoValues[]>(MegaCritSerializerContext.SerializableBadgeCtorParamInit)),
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableBadge).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Saves.Runs.SerializableBadge>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableBadge>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] SerializableBadgePropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[2];
    JsonPropertyInfoValues<string> propertyInfoValues1 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableBadge),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableBadge) obj).Id),
      Setter = (Action<object, string>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableBadge) obj).Id = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Id",
      JsonPropertyName = "id",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableBadge).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues1);
    jsonPropertyInfoArray[0].IsRequired = true;
    jsonPropertyInfoArray[0].IsGetNullable = false;
    jsonPropertyInfoArray[0].IsSetNullable = false;
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.Badges.BadgeRarity> propertyInfoValues2 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.Badges.BadgeRarity>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableBadge),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Models.Badges.BadgeRarity>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Models.Badges.BadgeRarity>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableBadge) obj).Rarity),
      Setter = (Action<object, MegaCrit.Sts2.Core.Models.Badges.BadgeRarity>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableBadge) obj).Rarity = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Rarity",
      JsonPropertyName = "rarity",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableBadge).GetProperty("Rarity", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Models.Badges.BadgeRarity), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Models.Badges.BadgeRarity>(options, propertyInfoValues2);
    jsonPropertyInfoArray[1].IsRequired = true;
    return jsonPropertyInfoArray;
  }

  private static JsonParameterInfoValues[] SerializableBadgeCtorParamInit()
  {
    return new JsonParameterInfoValues[2]
    {
      new JsonParameterInfoValues()
      {
        Name = "Id",
        ParameterType = typeof (string),
        Position = 0,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Rarity",
        ParameterType = typeof (MegaCrit.Sts2.Core.Models.Badges.BadgeRarity),
        Position = 1,
        IsNullable = false,
        IsMemberInitializer = true
      }
    };
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard> SerializableCard
  {
    get
    {
      return this._SerializableCard ?? (this._SerializableCard = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableCard)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard> Create_SerializableCard(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>) (() => new MegaCrit.Sts2.Core.Saves.Runs.SerializableCard()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.SerializableCardPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableCard).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] SerializableCardPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[5];
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId> propertyInfoValues1 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableCard),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Models.ModelId>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Models.ModelId>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableCard) obj).Id),
      Setter = (Action<object, MegaCrit.Sts2.Core.Models.ModelId>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableCard) obj).Id = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Id",
      JsonPropertyName = "id",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableCard).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Models.ModelId), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Models.ModelId>(options, propertyInfoValues1);
    JsonPropertyInfoValues<int> propertyInfoValues2 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableCard),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableCard) obj).CurrentUpgradeLevel),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableCard) obj).CurrentUpgradeLevel = value),
      IgnoreCondition = new JsonIgnoreCondition?((JsonIgnoreCondition) 2),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "CurrentUpgradeLevel",
      JsonPropertyName = "current_upgrade_level",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableCard).GetProperty("CurrentUpgradeLevel", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues2);
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableEnchantment> propertyInfoValues3 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableEnchantment>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableCard),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Saves.Runs.SerializableEnchantment>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Saves.Runs.SerializableEnchantment>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableCard) obj).Enchantment),
      Setter = (Action<object, MegaCrit.Sts2.Core.Saves.Runs.SerializableEnchantment>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableCard) obj).Enchantment = value),
      IgnoreCondition = new JsonIgnoreCondition?((JsonIgnoreCondition) 3),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Enchantment",
      JsonPropertyName = "enchantment",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableCard).GetProperty("Enchantment", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableEnchantment), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableEnchantment>(options, propertyInfoValues3);
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties> propertyInfoValues4 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableCard),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Saves.Runs.SavedProperties>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableCard) obj).Props),
      Setter = (Action<object, MegaCrit.Sts2.Core.Saves.Runs.SavedProperties>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableCard) obj).Props = value),
      IgnoreCondition = new JsonIgnoreCondition?((JsonIgnoreCondition) 3),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Props",
      JsonPropertyName = "props",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableCard).GetProperty("Props", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[3] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties>(options, propertyInfoValues4);
    JsonPropertyInfoValues<int?> propertyInfoValues5 = new JsonPropertyInfoValues<int?>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableCard),
      Converter = (JsonConverter<int?>) null,
      Getter = (Func<object, int?>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableCard) obj).FloorAddedToDeck),
      Setter = (Action<object, int?>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableCard) obj).FloorAddedToDeck = value),
      IgnoreCondition = new JsonIgnoreCondition?((JsonIgnoreCondition) 3),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "FloorAddedToDeck",
      JsonPropertyName = "floor_added_to_deck",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableCard).GetProperty("FloorAddedToDeck", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int?), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[4] = JsonMetadataServices.CreatePropertyInfo<int?>(options, propertyInfoValues5);
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]> SerializableCardArray
  {
    get
    {
      return this._SerializableCardArray ?? (this._SerializableCardArray = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[])));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]> Create_SerializableCardArray(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]> collectionInfoValues = new JsonCollectionInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>) null,
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateArrayInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableEnchantment> SerializableEnchantment
  {
    get
    {
      return this._SerializableEnchantment ?? (this._SerializableEnchantment = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableEnchantment>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableEnchantment)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableEnchantment> Create_SerializableEnchantment(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableEnchantment> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.Runs.SerializableEnchantment>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableEnchantment> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableEnchantment>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Saves.Runs.SerializableEnchantment>) (() => new MegaCrit.Sts2.Core.Saves.Runs.SerializableEnchantment()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Saves.Runs.SerializableEnchantment>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.SerializableEnchantmentPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableEnchantment).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Saves.Runs.SerializableEnchantment>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableEnchantment>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] SerializableEnchantmentPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[3];
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId> propertyInfoValues1 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableEnchantment),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Models.ModelId>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Models.ModelId>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableEnchantment) obj).Id),
      Setter = (Action<object, MegaCrit.Sts2.Core.Models.ModelId>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableEnchantment) obj).Id = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Id",
      JsonPropertyName = "id",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableEnchantment).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Models.ModelId), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Models.ModelId>(options, propertyInfoValues1);
    JsonPropertyInfoValues<int> propertyInfoValues2 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableEnchantment),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableEnchantment) obj).Amount),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableEnchantment) obj).Amount = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Amount",
      JsonPropertyName = "amount",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableEnchantment).GetProperty("Amount", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues2);
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties> propertyInfoValues3 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableEnchantment),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Saves.Runs.SavedProperties>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableEnchantment) obj).Props),
      Setter = (Action<object, MegaCrit.Sts2.Core.Saves.Runs.SavedProperties>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableEnchantment) obj).Props = value),
      IgnoreCondition = new JsonIgnoreCondition?((JsonIgnoreCondition) 3),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Props",
      JsonPropertyName = "props",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableEnchantment).GetProperty("Props", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties>(options, propertyInfoValues3);
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableExtraRunFields> SerializableExtraRunFields
  {
    get
    {
      return this._SerializableExtraRunFields ?? (this._SerializableExtraRunFields = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableExtraRunFields>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableExtraRunFields)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableExtraRunFields> Create_SerializableExtraRunFields(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableExtraRunFields> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.Runs.SerializableExtraRunFields>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableExtraRunFields> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableExtraRunFields>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Saves.Runs.SerializableExtraRunFields>) (() => new MegaCrit.Sts2.Core.Saves.Runs.SerializableExtraRunFields()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Saves.Runs.SerializableExtraRunFields>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.SerializableExtraRunFieldsPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableExtraRunFields).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Saves.Runs.SerializableExtraRunFields>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableExtraRunFields>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] SerializableExtraRunFieldsPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[3];
    JsonPropertyInfoValues<bool> propertyInfoValues1 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableExtraRunFields),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableExtraRunFields) obj).StartedWithNeow),
      Setter = (Action<object, bool>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableExtraRunFields) obj).StartedWithNeow = value),
      IgnoreCondition = new JsonIgnoreCondition?((JsonIgnoreCondition) 2),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "StartedWithNeow",
      JsonPropertyName = "started_with_neow",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableExtraRunFields).GetProperty("StartedWithNeow", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (bool), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues1);
    JsonPropertyInfoValues<int> propertyInfoValues2 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableExtraRunFields),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableExtraRunFields) obj).TestSubjectKills),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableExtraRunFields) obj).TestSubjectKills = value),
      IgnoreCondition = new JsonIgnoreCondition?((JsonIgnoreCondition) 2),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "TestSubjectKills",
      JsonPropertyName = "test_subject_kills",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableExtraRunFields).GetProperty("TestSubjectKills", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues2);
    JsonPropertyInfoValues<bool> propertyInfoValues3 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableExtraRunFields),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableExtraRunFields) obj).FreedRepy),
      Setter = (Action<object, bool>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableExtraRunFields) obj).FreedRepy = value),
      IgnoreCondition = new JsonIgnoreCondition?((JsonIgnoreCondition) 2),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "FreedRepy",
      JsonPropertyName = "freed_repy",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableExtraRunFields).GetProperty("FreedRepy", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (bool), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues3);
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint> SerializableMapPoint
  {
    get
    {
      return this._SerializableMapPoint ?? (this._SerializableMapPoint = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint> Create_SerializableMapPoint(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint>) (() => new MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.SerializableMapPointPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] SerializableMapPointPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[4];
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Map.MapCoord> propertyInfoValues1 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Map.MapCoord>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Map.MapCoord>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Map.MapCoord>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint) obj).Coord),
      Setter = (Action<object, MegaCrit.Sts2.Core.Map.MapCoord>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint) obj).Coord = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Coord",
      JsonPropertyName = "coord",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint).GetProperty("Coord", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Map.MapCoord), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Map.MapCoord>(options, propertyInfoValues1);
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Map.MapPointType> propertyInfoValues2 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Map.MapPointType>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Map.MapPointType>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Map.MapPointType>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint) obj).PointType),
      Setter = (Action<object, MegaCrit.Sts2.Core.Map.MapPointType>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint) obj).PointType = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "PointType",
      JsonPropertyName = "type",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint).GetProperty("PointType", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Map.MapPointType), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Map.MapPointType>(options, propertyInfoValues2);
    JsonPropertyInfoValues<bool> propertyInfoValues3 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint) obj).CanBeModified),
      Setter = (Action<object, bool>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint) obj).CanBeModified = value),
      IgnoreCondition = new JsonIgnoreCondition?((JsonIgnoreCondition) 2),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "CanBeModified",
      JsonPropertyName = "can_modify",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint).GetProperty("CanBeModified", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (bool), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues3);
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Map.MapCoord>> propertyInfoValues4 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Map.MapCoord>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Map.MapCoord>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Map.MapCoord>>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint) obj).ChildCoords),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Map.MapCoord>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint) obj).ChildCoords = value),
      IgnoreCondition = new JsonIgnoreCondition?((JsonIgnoreCondition) 3),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "ChildCoords",
      JsonPropertyName = "children",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint).GetProperty("ChildCoords", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Map.MapCoord>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[3] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Map.MapCoord>>(options, propertyInfoValues4);
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier> SerializableModifier
  {
    get
    {
      return this._SerializableModifier ?? (this._SerializableModifier = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier> Create_SerializableModifier(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier>) (() => new MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.SerializableModifierPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] SerializableModifierPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[2];
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId> propertyInfoValues1 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Models.ModelId>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Models.ModelId>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier) obj).Id),
      Setter = (Action<object, MegaCrit.Sts2.Core.Models.ModelId>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier) obj).Id = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Id",
      JsonPropertyName = "id",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Models.ModelId), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Models.ModelId>(options, propertyInfoValues1);
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties> propertyInfoValues2 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Saves.Runs.SavedProperties>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier) obj).Props),
      Setter = (Action<object, MegaCrit.Sts2.Core.Saves.Runs.SavedProperties>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier) obj).Props = value),
      IgnoreCondition = new JsonIgnoreCondition?((JsonIgnoreCondition) 3),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Props",
      JsonPropertyName = "props",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier).GetProperty("Props", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties>(options, propertyInfoValues2);
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer> SerializablePlayer
  {
    get
    {
      return this._SerializablePlayer ?? (this._SerializablePlayer = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer> Create_SerializablePlayer(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer>) (() => new MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.SerializablePlayerPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] SerializablePlayerPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[21];
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId> propertyInfoValues1 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Models.ModelId>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Models.ModelId>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer) obj).CharacterId),
      Setter = (Action<object, MegaCrit.Sts2.Core.Models.ModelId>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer) obj).CharacterId = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "CharacterId",
      JsonPropertyName = "character_id",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer).GetProperty("CharacterId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Models.ModelId), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Models.ModelId>(options, propertyInfoValues1);
    JsonPropertyInfoValues<int> propertyInfoValues2 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer) obj).CurrentHp),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer) obj).CurrentHp = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "CurrentHp",
      JsonPropertyName = "current_hp",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer).GetProperty("CurrentHp", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues2);
    JsonPropertyInfoValues<int> propertyInfoValues3 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer) obj).MaxHp),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer) obj).MaxHp = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "MaxHp",
      JsonPropertyName = "max_hp",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer).GetProperty("MaxHp", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues3);
    JsonPropertyInfoValues<int> propertyInfoValues4 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer) obj).MaxEnergy),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer) obj).MaxEnergy = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "MaxEnergy",
      JsonPropertyName = "max_energy",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer).GetProperty("MaxEnergy", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[3] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues4);
    JsonPropertyInfoValues<int> propertyInfoValues5 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer) obj).MaxPotionSlotCount),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer) obj).MaxPotionSlotCount = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "MaxPotionSlotCount",
      JsonPropertyName = "max_potion_slot_count",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer).GetProperty("MaxPotionSlotCount", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[4] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues5);
    JsonPropertyInfoValues<int> propertyInfoValues6 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer) obj).Gold),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer) obj).Gold = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Gold",
      JsonPropertyName = "gold",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer).GetProperty("Gold", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[5] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues6);
    JsonPropertyInfoValues<int> propertyInfoValues7 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer) obj).BaseOrbSlotCount),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer) obj).BaseOrbSlotCount = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "BaseOrbSlotCount",
      JsonPropertyName = "base_orb_slot_count",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer).GetProperty("BaseOrbSlotCount", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[6] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues7);
    JsonPropertyInfoValues<ulong> propertyInfoValues8 = new JsonPropertyInfoValues<ulong>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer),
      Converter = (JsonConverter<ulong>) null,
      Getter = (Func<object, ulong>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer) obj).NetId),
      Setter = (Action<object, ulong>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer) obj).NetId = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "NetId",
      JsonPropertyName = "net_id",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer).GetProperty("NetId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (ulong), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[7] = JsonMetadataServices.CreatePropertyInfo<ulong>(options, propertyInfoValues8);
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>> propertyInfoValues9 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer) obj).Deck),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer) obj).Deck = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Deck",
      JsonPropertyName = "deck",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer).GetProperty("Deck", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[8] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>(options, propertyInfoValues9);
    jsonPropertyInfoArray[8].IsGetNullable = false;
    jsonPropertyInfoArray[8].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>> propertyInfoValues10 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer) obj).Relics),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer) obj).Relics = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Relics",
      JsonPropertyName = "relics",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer).GetProperty("Relics", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[9] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>>(options, propertyInfoValues10);
    jsonPropertyInfoArray[9].IsGetNullable = false;
    jsonPropertyInfoArray[9].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>> propertyInfoValues11 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer) obj).Potions),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer) obj).Potions = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Potions",
      JsonPropertyName = "potions",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer).GetProperty("Potions", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[10] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>>(options, propertyInfoValues11);
    jsonPropertyInfoArray[10].IsGetNullable = false;
    jsonPropertyInfoArray[10].IsSetNullable = false;
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.SerializablePlayerRngSet> propertyInfoValues12 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.SerializablePlayerRngSet>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Saves.SerializablePlayerRngSet>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Saves.SerializablePlayerRngSet>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer) obj).Rng),
      Setter = (Action<object, MegaCrit.Sts2.Core.Saves.SerializablePlayerRngSet>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer) obj).Rng = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Rng",
      JsonPropertyName = "rng",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer).GetProperty("Rng", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Saves.SerializablePlayerRngSet), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[11] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Saves.SerializablePlayerRngSet>(options, propertyInfoValues12);
    jsonPropertyInfoArray[11].IsGetNullable = false;
    jsonPropertyInfoArray[11].IsSetNullable = false;
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayerOddsSet> propertyInfoValues13 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayerOddsSet>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayerOddsSet>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayerOddsSet>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer) obj).Odds),
      Setter = (Action<object, MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayerOddsSet>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer) obj).Odds = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Odds",
      JsonPropertyName = "odds",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer).GetProperty("Odds", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayerOddsSet), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[12] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayerOddsSet>(options, propertyInfoValues13);
    jsonPropertyInfoArray[12].IsGetNullable = false;
    jsonPropertyInfoArray[12].IsSetNullable = false;
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelicGrabBag> propertyInfoValues14 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelicGrabBag>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelicGrabBag>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Saves.Runs.SerializableRelicGrabBag>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer) obj).RelicGrabBag),
      Setter = (Action<object, MegaCrit.Sts2.Core.Saves.Runs.SerializableRelicGrabBag>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer) obj).RelicGrabBag = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "RelicGrabBag",
      JsonPropertyName = "relic_grab_bag",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer).GetProperty("RelicGrabBag", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRelicGrabBag), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[13] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelicGrabBag>(options, propertyInfoValues14);
    jsonPropertyInfoArray[13].IsGetNullable = false;
    jsonPropertyInfoArray[13].IsSetNullable = false;
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.SerializableExtraPlayerFields> propertyInfoValues15 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.SerializableExtraPlayerFields>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Saves.SerializableExtraPlayerFields>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Saves.SerializableExtraPlayerFields>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer) obj).ExtraFields),
      Setter = (Action<object, MegaCrit.Sts2.Core.Saves.SerializableExtraPlayerFields>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer) obj).ExtraFields = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "ExtraFields",
      JsonPropertyName = "extra_fields",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer).GetProperty("ExtraFields", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Saves.SerializableExtraPlayerFields), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[14] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Saves.SerializableExtraPlayerFields>(options, propertyInfoValues15);
    jsonPropertyInfoArray[14].IsGetNullable = false;
    jsonPropertyInfoArray[14].IsSetNullable = false;
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Unlocks.SerializableUnlockState> propertyInfoValues16 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Unlocks.SerializableUnlockState>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Unlocks.SerializableUnlockState>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Unlocks.SerializableUnlockState>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer) obj).UnlockState),
      Setter = (Action<object, MegaCrit.Sts2.Core.Unlocks.SerializableUnlockState>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer) obj).UnlockState = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "UnlockState",
      JsonPropertyName = "unlock_state",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer).GetProperty("UnlockState", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Unlocks.SerializableUnlockState), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[15] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Unlocks.SerializableUnlockState>(options, propertyInfoValues16);
    jsonPropertyInfoArray[15].IsGetNullable = false;
    jsonPropertyInfoArray[15].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>> propertyInfoValues17 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Models.ModelId>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer) obj).DiscoveredCards),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer) obj).DiscoveredCards = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "DiscoveredCards",
      JsonPropertyName = "discovered_cards",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer).GetProperty("DiscoveredCards", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Models.ModelId>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[16 /*0x10*/] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Models.ModelId>>(options, propertyInfoValues17);
    jsonPropertyInfoArray[16 /*0x10*/].IsGetNullable = false;
    jsonPropertyInfoArray[16 /*0x10*/].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>> propertyInfoValues18 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Models.ModelId>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer) obj).DiscoveredEnemies),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer) obj).DiscoveredEnemies = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "DiscoveredEnemies",
      JsonPropertyName = "discovered_enemies",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer).GetProperty("DiscoveredEnemies", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Models.ModelId>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[17] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Models.ModelId>>(options, propertyInfoValues18);
    jsonPropertyInfoArray[17].IsGetNullable = false;
    jsonPropertyInfoArray[17].IsSetNullable = false;
    JsonPropertyInfoValues<List<string>> propertyInfoValues19 = new JsonPropertyInfoValues<List<string>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer),
      Converter = (JsonConverter<List<string>>) MegaCritSerializerContext.ExpandConverter(typeof (List<string>), (JsonConverter) new EpochIdListConverter(), options),
      Getter = (Func<object, List<string>>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer) obj).DiscoveredEpochs),
      Setter = (Action<object, List<string>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer) obj).DiscoveredEpochs = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "DiscoveredEpochs",
      JsonPropertyName = "discovered_epochs",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer).GetProperty("DiscoveredEpochs", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<string>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[18] = JsonMetadataServices.CreatePropertyInfo<List<string>>(options, propertyInfoValues19);
    jsonPropertyInfoArray[18].IsGetNullable = false;
    jsonPropertyInfoArray[18].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>> propertyInfoValues20 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Models.ModelId>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer) obj).DiscoveredPotions),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer) obj).DiscoveredPotions = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "DiscoveredPotions",
      JsonPropertyName = "discovered_potions",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer).GetProperty("DiscoveredPotions", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Models.ModelId>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[19] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Models.ModelId>>(options, propertyInfoValues20);
    jsonPropertyInfoArray[19].IsGetNullable = false;
    jsonPropertyInfoArray[19].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>> propertyInfoValues21 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Models.ModelId>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer) obj).DiscoveredRelics),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer) obj).DiscoveredRelics = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "DiscoveredRelics",
      JsonPropertyName = "discovered_relics",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer).GetProperty("DiscoveredRelics", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Models.ModelId>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[20] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Models.ModelId>>(options, propertyInfoValues21);
    jsonPropertyInfoArray[20].IsGetNullable = false;
    jsonPropertyInfoArray[20].IsSetNullable = false;
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayerOddsSet> SerializablePlayerOddsSet
  {
    get
    {
      return this._SerializablePlayerOddsSet ?? (this._SerializablePlayerOddsSet = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayerOddsSet>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayerOddsSet)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayerOddsSet> Create_SerializablePlayerOddsSet(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayerOddsSet> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayerOddsSet>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayerOddsSet> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayerOddsSet>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayerOddsSet>) (() => new MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayerOddsSet()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayerOddsSet>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.SerializablePlayerOddsSetPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayerOddsSet).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayerOddsSet>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayerOddsSet>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] SerializablePlayerOddsSetPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[2];
    JsonPropertyInfoValues<float> propertyInfoValues1 = new JsonPropertyInfoValues<float>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayerOddsSet),
      Converter = (JsonConverter<float>) null,
      Getter = (Func<object, float>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayerOddsSet) obj).CardRarityOddsValue),
      Setter = (Action<object, float>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayerOddsSet) obj).CardRarityOddsValue = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "CardRarityOddsValue",
      JsonPropertyName = "card_rarity_odds_value",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayerOddsSet).GetProperty("CardRarityOddsValue", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (float), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<float>(options, propertyInfoValues1);
    JsonPropertyInfoValues<float> propertyInfoValues2 = new JsonPropertyInfoValues<float>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayerOddsSet),
      Converter = (JsonConverter<float>) null,
      Getter = (Func<object, float>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayerOddsSet) obj).PotionRewardOddsValue),
      Setter = (Action<object, float>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayerOddsSet) obj).PotionRewardOddsValue = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "PotionRewardOddsValue",
      JsonPropertyName = "potion_reward_odds_value",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayerOddsSet).GetProperty("PotionRewardOddsValue", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (float), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<float>(options, propertyInfoValues2);
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion> SerializablePotion
  {
    get
    {
      return this._SerializablePotion ?? (this._SerializablePotion = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion> Create_SerializablePotion(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>) (() => new MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.SerializablePotionPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] SerializablePotionPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[2];
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId> propertyInfoValues1 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Models.ModelId>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Models.ModelId>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion) obj).Id),
      Setter = (Action<object, MegaCrit.Sts2.Core.Models.ModelId>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion) obj).Id = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Id",
      JsonPropertyName = "id",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Models.ModelId), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Models.ModelId>(options, propertyInfoValues1);
    JsonPropertyInfoValues<int> propertyInfoValues2 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion) obj).SlotIndex),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion) obj).SlotIndex = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "SlotIndex",
      JsonPropertyName = "slot_index",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion).GetProperty("SlotIndex", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues2);
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic> SerializableRelic
  {
    get
    {
      return this._SerializableRelic ?? (this._SerializableRelic = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic> Create_SerializableRelic(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>) (() => new MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.SerializableRelicPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] SerializableRelicPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[3];
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId> propertyInfoValues1 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Models.ModelId>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Models.ModelId>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic) obj).Id),
      Setter = (Action<object, MegaCrit.Sts2.Core.Models.ModelId>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic) obj).Id = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Id",
      JsonPropertyName = "id",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Models.ModelId), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Models.ModelId>(options, propertyInfoValues1);
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties> propertyInfoValues2 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Saves.Runs.SavedProperties>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic) obj).Props),
      Setter = (Action<object, MegaCrit.Sts2.Core.Saves.Runs.SavedProperties>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic) obj).Props = value),
      IgnoreCondition = new JsonIgnoreCondition?((JsonIgnoreCondition) 3),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Props",
      JsonPropertyName = "props",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic).GetProperty("Props", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties>(options, propertyInfoValues2);
    JsonPropertyInfoValues<int?> propertyInfoValues3 = new JsonPropertyInfoValues<int?>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic),
      Converter = (JsonConverter<int?>) null,
      Getter = (Func<object, int?>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic) obj).FloorAddedToDeck),
      Setter = (Action<object, int?>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic) obj).FloorAddedToDeck = value),
      IgnoreCondition = new JsonIgnoreCondition?((JsonIgnoreCondition) 3),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "FloorAddedToDeck",
      JsonPropertyName = "floor_added_to_deck",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic).GetProperty("FloorAddedToDeck", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int?), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<int?>(options, propertyInfoValues3);
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelicGrabBag> SerializableRelicGrabBag
  {
    get
    {
      return this._SerializableRelicGrabBag ?? (this._SerializableRelicGrabBag = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelicGrabBag>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRelicGrabBag)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelicGrabBag> Create_SerializableRelicGrabBag(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelicGrabBag> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelicGrabBag>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelicGrabBag> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelicGrabBag>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelicGrabBag>) (() => new MegaCrit.Sts2.Core.Saves.Runs.SerializableRelicGrabBag()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Saves.Runs.SerializableRelicGrabBag>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.SerializableRelicGrabBagPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRelicGrabBag).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Saves.Runs.SerializableRelicGrabBag>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelicGrabBag>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] SerializableRelicGrabBagPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[1];
    JsonPropertyInfoValues<Dictionary<MegaCrit.Sts2.Core.Entities.Relics.RelicRarity, List<MegaCrit.Sts2.Core.Models.ModelId>>> propertyInfoValues = new JsonPropertyInfoValues<Dictionary<MegaCrit.Sts2.Core.Entities.Relics.RelicRarity, List<MegaCrit.Sts2.Core.Models.ModelId>>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRelicGrabBag),
      Converter = (JsonConverter<Dictionary<MegaCrit.Sts2.Core.Entities.Relics.RelicRarity, List<MegaCrit.Sts2.Core.Models.ModelId>>>) null,
      Getter = (Func<object, Dictionary<MegaCrit.Sts2.Core.Entities.Relics.RelicRarity, List<MegaCrit.Sts2.Core.Models.ModelId>>>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRelicGrabBag) obj).RelicIdLists),
      Setter = (Action<object, Dictionary<MegaCrit.Sts2.Core.Entities.Relics.RelicRarity, List<MegaCrit.Sts2.Core.Models.ModelId>>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRelicGrabBag) obj).RelicIdLists = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "RelicIdLists",
      JsonPropertyName = "relic_id_lists",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRelicGrabBag).GetProperty("RelicIdLists", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (Dictionary<MegaCrit.Sts2.Core.Entities.Relics.RelicRarity, List<MegaCrit.Sts2.Core.Models.ModelId>>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<Dictionary<MegaCrit.Sts2.Core.Entities.Relics.RelicRarity, List<MegaCrit.Sts2.Core.Models.ModelId>>>(options, propertyInfoValues);
    jsonPropertyInfoArray[0].IsGetNullable = false;
    jsonPropertyInfoArray[0].IsSetNullable = false;
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableReward> SerializableReward
  {
    get
    {
      return this._SerializableReward ?? (this._SerializableReward = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableReward>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableReward)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableReward> Create_SerializableReward(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableReward> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.Runs.SerializableReward>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableReward> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableReward>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Saves.Runs.SerializableReward>) (() => new MegaCrit.Sts2.Core.Saves.Runs.SerializableReward()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Saves.Runs.SerializableReward>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.SerializableRewardPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableReward).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Saves.Runs.SerializableReward>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableReward>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] SerializableRewardPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[10];
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Rewards.RewardType> propertyInfoValues1 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Rewards.RewardType>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableReward),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Rewards.RewardType>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Rewards.RewardType>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableReward) obj).RewardType),
      Setter = (Action<object, MegaCrit.Sts2.Core.Rewards.RewardType>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableReward) obj).RewardType = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "RewardType",
      JsonPropertyName = "reward_type",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableReward).GetProperty("RewardType", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Rewards.RewardType), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Rewards.RewardType>(options, propertyInfoValues1);
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId> propertyInfoValues2 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableReward),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Models.ModelId>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Models.ModelId>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableReward) obj).PredeterminedModelId),
      Setter = (Action<object, MegaCrit.Sts2.Core.Models.ModelId>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableReward) obj).PredeterminedModelId = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "PredeterminedModelId",
      JsonPropertyName = "predetermined_model_id",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableReward).GetProperty("PredeterminedModelId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Models.ModelId), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Models.ModelId>(options, propertyInfoValues2);
    jsonPropertyInfoArray[1].IsGetNullable = false;
    jsonPropertyInfoArray[1].IsSetNullable = false;
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard> propertyInfoValues3 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableReward),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableReward) obj).SpecialCard),
      Setter = (Action<object, MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableReward) obj).SpecialCard = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "SpecialCard",
      JsonPropertyName = "special_card",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableReward).GetProperty("SpecialCard", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableCard), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>(options, propertyInfoValues3);
    JsonPropertyInfoValues<int> propertyInfoValues4 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableReward),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableReward) obj).GoldAmount),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableReward) obj).GoldAmount = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "GoldAmount",
      JsonPropertyName = "gold_amount",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableReward).GetProperty("GoldAmount", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[3] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues4);
    JsonPropertyInfoValues<bool> propertyInfoValues5 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableReward),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableReward) obj).WasGoldStolenBack),
      Setter = (Action<object, bool>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableReward) obj).WasGoldStolenBack = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "WasGoldStolenBack",
      JsonPropertyName = "was_gold_stolen_back",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableReward).GetProperty("WasGoldStolenBack", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (bool), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[4] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues5);
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Runs.CardCreationSource> propertyInfoValues6 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Runs.CardCreationSource>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableReward),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Runs.CardCreationSource>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Runs.CardCreationSource>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableReward) obj).Source),
      Setter = (Action<object, MegaCrit.Sts2.Core.Runs.CardCreationSource>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableReward) obj).Source = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Source",
      JsonPropertyName = "source",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableReward).GetProperty("Source", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Runs.CardCreationSource), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[5] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Runs.CardCreationSource>(options, propertyInfoValues6);
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Runs.CardRarityOddsType> propertyInfoValues7 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Runs.CardRarityOddsType>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableReward),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Runs.CardRarityOddsType>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Runs.CardRarityOddsType>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableReward) obj).RarityOdds),
      Setter = (Action<object, MegaCrit.Sts2.Core.Runs.CardRarityOddsType>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableReward) obj).RarityOdds = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "RarityOdds",
      JsonPropertyName = "rarity_odds",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableReward).GetProperty("RarityOdds", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Runs.CardRarityOddsType), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[6] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Runs.CardRarityOddsType>(options, propertyInfoValues7);
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>> propertyInfoValues8 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableReward),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Models.ModelId>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableReward) obj).CardPoolIds),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableReward) obj).CardPoolIds = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "CardPoolIds",
      JsonPropertyName = "card_pools",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableReward).GetProperty("CardPoolIds", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Models.ModelId>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[7] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Models.ModelId>>(options, propertyInfoValues8);
    jsonPropertyInfoArray[7].IsGetNullable = false;
    jsonPropertyInfoArray[7].IsSetNullable = false;
    JsonPropertyInfoValues<int> propertyInfoValues9 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableReward),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableReward) obj).OptionCount),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableReward) obj).OptionCount = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "OptionCount",
      JsonPropertyName = "option_count",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableReward).GetProperty("OptionCount", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[8] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues9);
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId> propertyInfoValues10 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableReward),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Models.ModelId>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Models.ModelId>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableReward) obj).CustomDescriptionEncounterSourceId),
      Setter = (Action<object, MegaCrit.Sts2.Core.Models.ModelId>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableReward) obj).CustomDescriptionEncounterSourceId = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "CustomDescriptionEncounterSourceId",
      JsonPropertyName = "custom_description_encounter_source_id",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableReward).GetProperty("CustomDescriptionEncounterSourceId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Models.ModelId), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[9] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Models.ModelId>(options, propertyInfoValues10);
    jsonPropertyInfoArray[9].IsGetNullable = false;
    jsonPropertyInfoArray[9].IsSetNullable = false;
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom> SerializableRoom
  {
    get
    {
      return this._SerializableRoom ?? (this._SerializableRoom = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom> Create_SerializableRoom(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom>) (() => new MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.SerializableRoomPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] SerializableRoomPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[9];
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Rooms.RoomType> propertyInfoValues1 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Rooms.RoomType>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Rooms.RoomType>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Rooms.RoomType>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom) obj).RoomType),
      Setter = (Action<object, MegaCrit.Sts2.Core.Rooms.RoomType>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom) obj).RoomType = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "RoomType",
      JsonPropertyName = "room_type",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom).GetProperty("RoomType", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Rooms.RoomType), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Rooms.RoomType>(options, propertyInfoValues1);
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId> propertyInfoValues2 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Models.ModelId>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Models.ModelId>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom) obj).EncounterId),
      Setter = (Action<object, MegaCrit.Sts2.Core.Models.ModelId>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom) obj).EncounterId = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "EncounterId",
      JsonPropertyName = "encounter_id",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom).GetProperty("EncounterId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Models.ModelId), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Models.ModelId>(options, propertyInfoValues2);
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId> propertyInfoValues3 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Models.ModelId>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Models.ModelId>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom) obj).EventId),
      Setter = (Action<object, MegaCrit.Sts2.Core.Models.ModelId>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom) obj).EventId = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "EventId",
      JsonPropertyName = "event_id",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom).GetProperty("EventId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Models.ModelId), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Models.ModelId>(options, propertyInfoValues3);
    JsonPropertyInfoValues<bool> propertyInfoValues4 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom) obj).IsPreFinished),
      Setter = (Action<object, bool>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom) obj).IsPreFinished = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "IsPreFinished",
      JsonPropertyName = "is_pre_finished",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom).GetProperty("IsPreFinished", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (bool), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[3] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues4);
    JsonPropertyInfoValues<float> propertyInfoValues5 = new JsonPropertyInfoValues<float>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom),
      Converter = (JsonConverter<float>) null,
      Getter = (Func<object, float>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom) obj).GoldProportion),
      Setter = (Action<object, float>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom) obj).GoldProportion = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "GoldProportion",
      JsonPropertyName = "reward_proportion",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom).GetProperty("GoldProportion", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (float), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[4] = JsonMetadataServices.CreatePropertyInfo<float>(options, propertyInfoValues5);
    JsonPropertyInfoValues<Dictionary<ulong, List<MegaCrit.Sts2.Core.Saves.Runs.SerializableReward>>> propertyInfoValues6 = new JsonPropertyInfoValues<Dictionary<ulong, List<MegaCrit.Sts2.Core.Saves.Runs.SerializableReward>>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom),
      Converter = (JsonConverter<Dictionary<ulong, List<MegaCrit.Sts2.Core.Saves.Runs.SerializableReward>>>) null,
      Getter = (Func<object, Dictionary<ulong, List<MegaCrit.Sts2.Core.Saves.Runs.SerializableReward>>>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom) obj).ExtraRewards),
      Setter = (Action<object, Dictionary<ulong, List<MegaCrit.Sts2.Core.Saves.Runs.SerializableReward>>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom) obj).ExtraRewards = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "ExtraRewards",
      JsonPropertyName = "extra_rewards",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom).GetProperty("ExtraRewards", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (Dictionary<ulong, List<MegaCrit.Sts2.Core.Saves.Runs.SerializableReward>>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[5] = JsonMetadataServices.CreatePropertyInfo<Dictionary<ulong, List<MegaCrit.Sts2.Core.Saves.Runs.SerializableReward>>>(options, propertyInfoValues6);
    jsonPropertyInfoArray[5].IsGetNullable = false;
    jsonPropertyInfoArray[5].IsSetNullable = false;
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId> propertyInfoValues7 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Models.ModelId>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Models.ModelId>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom) obj).ParentEventId),
      Setter = (Action<object, MegaCrit.Sts2.Core.Models.ModelId>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom) obj).ParentEventId = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "ParentEventId",
      JsonPropertyName = "parent_event_id",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom).GetProperty("ParentEventId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Models.ModelId), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[6] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Models.ModelId>(options, propertyInfoValues7);
    JsonPropertyInfoValues<bool> propertyInfoValues8 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom) obj).ShouldResumeParentEvent),
      Setter = (Action<object, bool>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom) obj).ShouldResumeParentEvent = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "ShouldResumeParentEvent",
      JsonPropertyName = "should_resume_parent_event",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom).GetProperty("ShouldResumeParentEvent", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (bool), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[7] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues8);
    JsonPropertyInfoValues<Dictionary<string, string>> propertyInfoValues9 = new JsonPropertyInfoValues<Dictionary<string, string>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom),
      Converter = (JsonConverter<Dictionary<string, string>>) null,
      Getter = (Func<object, Dictionary<string, string>>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom) obj).EncounterState),
      Setter = (Action<object, Dictionary<string, string>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom) obj).EncounterState = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "EncounterState",
      JsonPropertyName = "encounter_state",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom).GetProperty("EncounterState", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (Dictionary<string, string>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[8] = JsonMetadataServices.CreatePropertyInfo<Dictionary<string, string>>(options, propertyInfoValues9);
    jsonPropertyInfoArray[8].IsGetNullable = false;
    jsonPropertyInfoArray[8].IsSetNullable = false;
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet> SerializableRoomSet
  {
    get
    {
      return this._SerializableRoomSet ?? (this._SerializableRoomSet = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet> Create_SerializableRoomSet(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet>) (() => new MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.SerializableRoomSetPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] SerializableRoomSetPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[10];
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>> propertyInfoValues1 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Models.ModelId>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet) obj).EventIds),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet) obj).EventIds = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "EventIds",
      JsonPropertyName = "event_ids",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet).GetProperty("EventIds", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Models.ModelId>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Models.ModelId>>(options, propertyInfoValues1);
    jsonPropertyInfoArray[0].IsGetNullable = false;
    jsonPropertyInfoArray[0].IsSetNullable = false;
    JsonPropertyInfoValues<int> propertyInfoValues2 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet) obj).EventsVisited),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet) obj).EventsVisited = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "EventsVisited",
      JsonPropertyName = "events_visited",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet).GetProperty("EventsVisited", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues2);
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>> propertyInfoValues3 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Models.ModelId>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet) obj).NormalEncounterIds),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet) obj).NormalEncounterIds = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "NormalEncounterIds",
      JsonPropertyName = "normal_encounter_ids",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet).GetProperty("NormalEncounterIds", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Models.ModelId>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Models.ModelId>>(options, propertyInfoValues3);
    jsonPropertyInfoArray[2].IsGetNullable = false;
    jsonPropertyInfoArray[2].IsSetNullable = false;
    JsonPropertyInfoValues<int> propertyInfoValues4 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet) obj).NormalEncountersVisited),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet) obj).NormalEncountersVisited = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "NormalEncountersVisited",
      JsonPropertyName = "normal_encounters_visited",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet).GetProperty("NormalEncountersVisited", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[3] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues4);
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>> propertyInfoValues5 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Models.ModelId>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet) obj).EliteEncounterIds),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet) obj).EliteEncounterIds = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "EliteEncounterIds",
      JsonPropertyName = "elite_encounter_ids",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet).GetProperty("EliteEncounterIds", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Models.ModelId>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[4] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Models.ModelId>>(options, propertyInfoValues5);
    jsonPropertyInfoArray[4].IsGetNullable = false;
    jsonPropertyInfoArray[4].IsSetNullable = false;
    JsonPropertyInfoValues<int> propertyInfoValues6 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet) obj).EliteEncountersVisited),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet) obj).EliteEncountersVisited = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "EliteEncountersVisited",
      JsonPropertyName = "elite_encounters_visited",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet).GetProperty("EliteEncountersVisited", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[5] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues6);
    JsonPropertyInfoValues<int> propertyInfoValues7 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet) obj).BossEncountersVisited),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet) obj).BossEncountersVisited = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "BossEncountersVisited",
      JsonPropertyName = "boss_encounters_visited",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet).GetProperty("BossEncountersVisited", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[6] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues7);
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId> propertyInfoValues8 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Models.ModelId>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Models.ModelId>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet) obj).BossId),
      Setter = (Action<object, MegaCrit.Sts2.Core.Models.ModelId>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet) obj).BossId = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "BossId",
      JsonPropertyName = "boss_id",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet).GetProperty("BossId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Models.ModelId), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[7] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Models.ModelId>(options, propertyInfoValues8);
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId> propertyInfoValues9 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Models.ModelId>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Models.ModelId>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet) obj).SecondBossId),
      Setter = (Action<object, MegaCrit.Sts2.Core.Models.ModelId>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet) obj).SecondBossId = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "SecondBossId",
      JsonPropertyName = "second_boss_id",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet).GetProperty("SecondBossId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Models.ModelId), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[8] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Models.ModelId>(options, propertyInfoValues9);
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId> propertyInfoValues10 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Models.ModelId>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Models.ModelId>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet) obj).AncientId),
      Setter = (Action<object, MegaCrit.Sts2.Core.Models.ModelId>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet) obj).AncientId = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "AncientId",
      JsonPropertyName = "ancient_id",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet).GetProperty("AncientId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Models.ModelId), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[9] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Models.ModelId>(options, propertyInfoValues10);
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableRunOddsSet> SerializableRunOddsSet
  {
    get
    {
      return this._SerializableRunOddsSet ?? (this._SerializableRunOddsSet = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableRunOddsSet>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRunOddsSet)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableRunOddsSet> Create_SerializableRunOddsSet(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableRunOddsSet> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.Runs.SerializableRunOddsSet>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableRunOddsSet> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableRunOddsSet>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Saves.Runs.SerializableRunOddsSet>) (() => new MegaCrit.Sts2.Core.Saves.Runs.SerializableRunOddsSet()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Saves.Runs.SerializableRunOddsSet>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.SerializableRunOddsSetPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRunOddsSet).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Saves.Runs.SerializableRunOddsSet>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableRunOddsSet>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] SerializableRunOddsSetPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[4];
    JsonPropertyInfoValues<float> propertyInfoValues1 = new JsonPropertyInfoValues<float>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRunOddsSet),
      Converter = (JsonConverter<float>) null,
      Getter = (Func<object, float>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRunOddsSet) obj).UnknownMapPointMonsterOddsValue),
      Setter = (Action<object, float>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRunOddsSet) obj).UnknownMapPointMonsterOddsValue = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "UnknownMapPointMonsterOddsValue",
      JsonPropertyName = "unknown_map_point_monster_odds_value",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRunOddsSet).GetProperty("UnknownMapPointMonsterOddsValue", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (float), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<float>(options, propertyInfoValues1);
    JsonPropertyInfoValues<float> propertyInfoValues2 = new JsonPropertyInfoValues<float>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRunOddsSet),
      Converter = (JsonConverter<float>) null,
      Getter = (Func<object, float>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRunOddsSet) obj).UnknownMapPointEliteOddsValue),
      Setter = (Action<object, float>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRunOddsSet) obj).UnknownMapPointEliteOddsValue = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "UnknownMapPointEliteOddsValue",
      JsonPropertyName = "unknown_map_point_elite_odds_value",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRunOddsSet).GetProperty("UnknownMapPointEliteOddsValue", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (float), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<float>(options, propertyInfoValues2);
    JsonPropertyInfoValues<float> propertyInfoValues3 = new JsonPropertyInfoValues<float>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRunOddsSet),
      Converter = (JsonConverter<float>) null,
      Getter = (Func<object, float>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRunOddsSet) obj).UnknownMapPointTreasureOddsValue),
      Setter = (Action<object, float>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRunOddsSet) obj).UnknownMapPointTreasureOddsValue = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "UnknownMapPointTreasureOddsValue",
      JsonPropertyName = "unknown_map_point_treasure_odds_value",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRunOddsSet).GetProperty("UnknownMapPointTreasureOddsValue", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (float), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<float>(options, propertyInfoValues3);
    JsonPropertyInfoValues<float> propertyInfoValues4 = new JsonPropertyInfoValues<float>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRunOddsSet),
      Converter = (JsonConverter<float>) null,
      Getter = (Func<object, float>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRunOddsSet) obj).UnknownMapPointShopOddsValue),
      Setter = (Action<object, float>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRunOddsSet) obj).UnknownMapPointShopOddsValue = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "UnknownMapPointShopOddsValue",
      JsonPropertyName = "unknown_map_point_shop_odds_value",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRunOddsSet).GetProperty("UnknownMapPointShopOddsValue", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (float), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[3] = JsonMetadataServices.CreatePropertyInfo<float>(options, propertyInfoValues4);
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableRunRngSet> SerializableRunRngSet
  {
    get
    {
      return this._SerializableRunRngSet ?? (this._SerializableRunRngSet = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableRunRngSet>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRunRngSet)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableRunRngSet> Create_SerializableRunRngSet(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableRunRngSet> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.Runs.SerializableRunRngSet>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableRunRngSet> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableRunRngSet>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Saves.Runs.SerializableRunRngSet>) (() => new MegaCrit.Sts2.Core.Saves.Runs.SerializableRunRngSet()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Saves.Runs.SerializableRunRngSet>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.SerializableRunRngSetPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRunRngSet).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Saves.Runs.SerializableRunRngSet>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableRunRngSet>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] SerializableRunRngSetPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[2];
    JsonPropertyInfoValues<string> propertyInfoValues1 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRunRngSet),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRunRngSet) obj).Seed),
      Setter = (Action<object, string>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRunRngSet) obj).Seed = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Seed",
      JsonPropertyName = "seed",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRunRngSet).GetProperty("Seed", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues1);
    JsonPropertyInfoValues<Dictionary<MegaCrit.Sts2.Core.Entities.Rngs.RunRngType, MegaCrit.Sts2.Core.Saves.SerializableRng>> propertyInfoValues2 = new JsonPropertyInfoValues<Dictionary<MegaCrit.Sts2.Core.Entities.Rngs.RunRngType, MegaCrit.Sts2.Core.Saves.SerializableRng>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRunRngSet),
      Converter = (JsonConverter<Dictionary<MegaCrit.Sts2.Core.Entities.Rngs.RunRngType, MegaCrit.Sts2.Core.Saves.SerializableRng>>) null,
      Getter = (Func<object, Dictionary<MegaCrit.Sts2.Core.Entities.Rngs.RunRngType, MegaCrit.Sts2.Core.Saves.SerializableRng>>) (obj => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRunRngSet) obj).Rngs),
      Setter = (Action<object, Dictionary<MegaCrit.Sts2.Core.Entities.Rngs.RunRngType, MegaCrit.Sts2.Core.Saves.SerializableRng>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.Runs.SerializableRunRngSet) obj).Rngs = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Rngs",
      JsonPropertyName = "rngs",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRunRngSet).GetProperty("Rngs", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (Dictionary<MegaCrit.Sts2.Core.Entities.Rngs.RunRngType, MegaCrit.Sts2.Core.Saves.SerializableRng>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<Dictionary<MegaCrit.Sts2.Core.Entities.Rngs.RunRngType, MegaCrit.Sts2.Core.Saves.SerializableRng>>(options, propertyInfoValues2);
    jsonPropertyInfoArray[1].IsGetNullable = false;
    jsonPropertyInfoArray[1].IsSetNullable = false;
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.SerializableEpoch> SerializableEpoch
  {
    get
    {
      return this._SerializableEpoch ?? (this._SerializableEpoch = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.SerializableEpoch>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.SerializableEpoch)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.SerializableEpoch> Create_SerializableEpoch(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.SerializableEpoch> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.SerializableEpoch>(options, out jsonTypeInfo))
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.SerializableEpoch> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.SerializableEpoch>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Saves.SerializableEpoch>) null,
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Saves.SerializableEpoch>) (args => new MegaCrit.Sts2.Core.Saves.SerializableEpoch((string) args[0], (MegaCrit.Sts2.Core.Saves.EpochState) args[1])),
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.SerializableEpochPropInit(options)),
        ConstructorParameterMetadataInitializer = MegaCritSerializerContext.\u003C\u003EO.\u003C15\u003E__SerializableEpochCtorParamInit ?? (MegaCritSerializerContext.\u003C\u003EO.\u003C15\u003E__SerializableEpochCtorParamInit = new Func<JsonParameterInfoValues[]>(MegaCritSerializerContext.SerializableEpochCtorParamInit)),
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableEpoch).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, new Type[2]
        {
          typeof (string),
          typeof (MegaCrit.Sts2.Core.Saves.EpochState)
        }, (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Saves.SerializableEpoch>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Saves.SerializableEpoch>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] SerializableEpochPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[3];
    JsonPropertyInfoValues<string> propertyInfoValues1 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableEpoch),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableEpoch) obj).Id),
      Setter = (Action<object, string>) null,
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Id",
      JsonPropertyName = "id",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableEpoch).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues1);
    jsonPropertyInfoArray[0].IsGetNullable = false;
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.EpochState> propertyInfoValues2 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.EpochState>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableEpoch),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Saves.EpochState>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Saves.EpochState>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableEpoch) obj).State),
      Setter = (Action<object, MegaCrit.Sts2.Core.Saves.EpochState>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableEpoch) obj).State = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "State",
      JsonPropertyName = "state",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableEpoch).GetProperty("State", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Saves.EpochState), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Saves.EpochState>(options, propertyInfoValues2);
    JsonPropertyInfoValues<long> propertyInfoValues3 = new JsonPropertyInfoValues<long>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableEpoch),
      Converter = (JsonConverter<long>) null,
      Getter = (Func<object, long>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableEpoch) obj).ObtainDate),
      Setter = (Action<object, long>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableEpoch) obj).ObtainDate = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "ObtainDate",
      JsonPropertyName = "obtain_date",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableEpoch).GetProperty("ObtainDate", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (long), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<long>(options, propertyInfoValues3);
    return jsonPropertyInfoArray;
  }

  private static JsonParameterInfoValues[] SerializableEpochCtorParamInit()
  {
    return new JsonParameterInfoValues[2]
    {
      new JsonParameterInfoValues()
      {
        Name = "id",
        ParameterType = typeof (string),
        Position = 0,
        HasDefaultValue = false,
        DefaultValue = (object) null,
        IsNullable = false
      },
      new JsonParameterInfoValues()
      {
        Name = "state",
        ParameterType = typeof (MegaCrit.Sts2.Core.Saves.EpochState),
        Position = 1,
        HasDefaultValue = false,
        DefaultValue = (object) null,
        IsNullable = false
      }
    };
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.SerializableExtraPlayerFields> SerializableExtraPlayerFields
  {
    get
    {
      return this._SerializableExtraPlayerFields ?? (this._SerializableExtraPlayerFields = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.SerializableExtraPlayerFields>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.SerializableExtraPlayerFields)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.SerializableExtraPlayerFields> Create_SerializableExtraPlayerFields(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.SerializableExtraPlayerFields> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.SerializableExtraPlayerFields>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.SerializableExtraPlayerFields> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.SerializableExtraPlayerFields>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Saves.SerializableExtraPlayerFields>) (() => new MegaCrit.Sts2.Core.Saves.SerializableExtraPlayerFields()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Saves.SerializableExtraPlayerFields>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.SerializableExtraPlayerFieldsPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableExtraPlayerFields).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Saves.SerializableExtraPlayerFields>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Saves.SerializableExtraPlayerFields>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] SerializableExtraPlayerFieldsPropInit(
    JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[5];
    JsonPropertyInfoValues<int> propertyInfoValues1 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableExtraPlayerFields),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableExtraPlayerFields) obj).CardShopRemovalsUsed),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableExtraPlayerFields) obj).CardShopRemovalsUsed = value),
      IgnoreCondition = new JsonIgnoreCondition?((JsonIgnoreCondition) 2),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "CardShopRemovalsUsed",
      JsonPropertyName = "card_shop_removals_used",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableExtraPlayerFields).GetProperty("CardShopRemovalsUsed", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues1);
    JsonPropertyInfoValues<int> propertyInfoValues2 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableExtraPlayerFields),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableExtraPlayerFields) obj).WongoPoints),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableExtraPlayerFields) obj).WongoPoints = value),
      IgnoreCondition = new JsonIgnoreCondition?((JsonIgnoreCondition) 2),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "WongoPoints",
      JsonPropertyName = "wongo_points",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableExtraPlayerFields).GetProperty("WongoPoints", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues2);
    JsonPropertyInfoValues<bool> propertyInfoValues3 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableExtraPlayerFields),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableExtraPlayerFields) obj).CccomboBadgeUnlocked),
      Setter = (Action<object, bool>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableExtraPlayerFields) obj).CccomboBadgeUnlocked = value),
      IgnoreCondition = new JsonIgnoreCondition?((JsonIgnoreCondition) 2),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "CccomboBadgeUnlocked",
      JsonPropertyName = "ccccombo_badge_unlocked",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableExtraPlayerFields).GetProperty("CccomboBadgeUnlocked", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (bool), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues3);
    JsonPropertyInfoValues<int> propertyInfoValues4 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableExtraPlayerFields),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableExtraPlayerFields) obj).DamageDealt),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableExtraPlayerFields) obj).DamageDealt = value),
      IgnoreCondition = new JsonIgnoreCondition?((JsonIgnoreCondition) 2),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "DamageDealt",
      JsonPropertyName = "damage_dealt",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableExtraPlayerFields).GetProperty("DamageDealt", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[3] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues4);
    JsonPropertyInfoValues<int> propertyInfoValues5 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableExtraPlayerFields),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableExtraPlayerFields) obj).DebuffsApplied),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableExtraPlayerFields) obj).DebuffsApplied = value),
      IgnoreCondition = new JsonIgnoreCondition?((JsonIgnoreCondition) 2),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "DebuffsApplied",
      JsonPropertyName = "debuffs_applied",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableExtraPlayerFields).GetProperty("DebuffsApplied", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[4] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues5);
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.SerializablePlayerRngSet> SerializablePlayerRngSet
  {
    get
    {
      return this._SerializablePlayerRngSet ?? (this._SerializablePlayerRngSet = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.SerializablePlayerRngSet>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.SerializablePlayerRngSet)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.SerializablePlayerRngSet> Create_SerializablePlayerRngSet(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.SerializablePlayerRngSet> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.SerializablePlayerRngSet>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.SerializablePlayerRngSet> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.SerializablePlayerRngSet>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Saves.SerializablePlayerRngSet>) (() => new MegaCrit.Sts2.Core.Saves.SerializablePlayerRngSet()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Saves.SerializablePlayerRngSet>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.SerializablePlayerRngSetPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializablePlayerRngSet).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Saves.SerializablePlayerRngSet>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Saves.SerializablePlayerRngSet>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] SerializablePlayerRngSetPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[2];
    JsonPropertyInfoValues<ulong> propertyInfoValues1 = new JsonPropertyInfoValues<ulong>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializablePlayerRngSet),
      Converter = (JsonConverter<ulong>) null,
      Getter = (Func<object, ulong>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializablePlayerRngSet) obj).Seed),
      Setter = (Action<object, ulong>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializablePlayerRngSet) obj).Seed = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Seed",
      JsonPropertyName = "seed",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializablePlayerRngSet).GetProperty("Seed", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (ulong), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<ulong>(options, propertyInfoValues1);
    JsonPropertyInfoValues<Dictionary<MegaCrit.Sts2.Core.Entities.Rngs.PlayerRngType, MegaCrit.Sts2.Core.Saves.SerializableRng>> propertyInfoValues2 = new JsonPropertyInfoValues<Dictionary<MegaCrit.Sts2.Core.Entities.Rngs.PlayerRngType, MegaCrit.Sts2.Core.Saves.SerializableRng>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializablePlayerRngSet),
      Converter = (JsonConverter<Dictionary<MegaCrit.Sts2.Core.Entities.Rngs.PlayerRngType, MegaCrit.Sts2.Core.Saves.SerializableRng>>) null,
      Getter = (Func<object, Dictionary<MegaCrit.Sts2.Core.Entities.Rngs.PlayerRngType, MegaCrit.Sts2.Core.Saves.SerializableRng>>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializablePlayerRngSet) obj).Rngs),
      Setter = (Action<object, Dictionary<MegaCrit.Sts2.Core.Entities.Rngs.PlayerRngType, MegaCrit.Sts2.Core.Saves.SerializableRng>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializablePlayerRngSet) obj).Rngs = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Rngs",
      JsonPropertyName = "rngs",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializablePlayerRngSet).GetProperty("Rngs", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (Dictionary<MegaCrit.Sts2.Core.Entities.Rngs.PlayerRngType, MegaCrit.Sts2.Core.Saves.SerializableRng>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<Dictionary<MegaCrit.Sts2.Core.Entities.Rngs.PlayerRngType, MegaCrit.Sts2.Core.Saves.SerializableRng>>(options, propertyInfoValues2);
    jsonPropertyInfoArray[1].IsGetNullable = false;
    jsonPropertyInfoArray[1].IsSetNullable = false;
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.SerializableProgress> SerializableProgress
  {
    get
    {
      return this._SerializableProgress ?? (this._SerializableProgress = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.SerializableProgress>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.SerializableProgress> Create_SerializableProgress(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.SerializableProgress> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.SerializableProgress>(options, out jsonTypeInfo))
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.SerializableProgress> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.SerializableProgress>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Saves.SerializableProgress>) null,
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Saves.SerializableProgress>) (args => new MegaCrit.Sts2.Core.Saves.SerializableProgress()
        {
          UniqueId = (string) args[0]
        }),
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.SerializableProgressPropInit(options)),
        ConstructorParameterMetadataInitializer = MegaCritSerializerContext.\u003C\u003EO.\u003C16\u003E__SerializableProgressCtorParamInit ?? (MegaCritSerializerContext.\u003C\u003EO.\u003C16\u003E__SerializableProgressCtorParamInit = new Func<JsonParameterInfoValues[]>(MegaCritSerializerContext.SerializableProgressCtorParamInit)),
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Saves.SerializableProgress>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Saves.SerializableProgress>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] SerializableProgressPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[31 /*0x1F*/];
    JsonPropertyInfoValues<int> propertyInfoValues1 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).SchemaVersion),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).SchemaVersion = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "SchemaVersion",
      JsonPropertyName = "schema_version",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress).GetProperty("SchemaVersion", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues1);
    JsonPropertyInfoValues<string> propertyInfoValues2 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).UniqueId),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "UniqueId",
      JsonPropertyName = "unique_id",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress).GetProperty("UniqueId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues2);
    jsonPropertyInfoArray[1].IsGetNullable = false;
    jsonPropertyInfoArray[1].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.CharacterStats>> propertyInfoValues3 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.CharacterStats>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Saves.CharacterStats>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Saves.CharacterStats>>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).CharStats),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Saves.CharacterStats>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).CharStats = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "CharStats",
      JsonPropertyName = "character_stats",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress).GetProperty("CharStats", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Saves.CharacterStats>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Saves.CharacterStats>>(options, propertyInfoValues3);
    jsonPropertyInfoArray[2].IsGetNullable = false;
    jsonPropertyInfoArray[2].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.CardStats>> propertyInfoValues4 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.CardStats>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Saves.CardStats>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Saves.CardStats>>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).CardStats),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Saves.CardStats>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).CardStats = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "CardStats",
      JsonPropertyName = "card_stats",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress).GetProperty("CardStats", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Saves.CardStats>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[3] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Saves.CardStats>>(options, propertyInfoValues4);
    jsonPropertyInfoArray[3].IsGetNullable = false;
    jsonPropertyInfoArray[3].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.EncounterStats>> propertyInfoValues5 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.EncounterStats>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Saves.EncounterStats>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Saves.EncounterStats>>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).EncounterStats),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Saves.EncounterStats>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).EncounterStats = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "EncounterStats",
      JsonPropertyName = "encounter_stats",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress).GetProperty("EncounterStats", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Saves.EncounterStats>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[4] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Saves.EncounterStats>>(options, propertyInfoValues5);
    jsonPropertyInfoArray[4].IsGetNullable = false;
    jsonPropertyInfoArray[4].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.EnemyStats>> propertyInfoValues6 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.EnemyStats>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Saves.EnemyStats>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Saves.EnemyStats>>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).EnemyStats),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Saves.EnemyStats>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).EnemyStats = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "EnemyStats",
      JsonPropertyName = "enemy_stats",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress).GetProperty("EnemyStats", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Saves.EnemyStats>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[5] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Saves.EnemyStats>>(options, propertyInfoValues6);
    jsonPropertyInfoArray[5].IsGetNullable = false;
    jsonPropertyInfoArray[5].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.AncientStats>> propertyInfoValues7 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.AncientStats>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Saves.AncientStats>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Saves.AncientStats>>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).AncientStats),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Saves.AncientStats>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).AncientStats = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "AncientStats",
      JsonPropertyName = "ancient_stats",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress).GetProperty("AncientStats", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Saves.AncientStats>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[6] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Saves.AncientStats>>(options, propertyInfoValues7);
    jsonPropertyInfoArray[6].IsGetNullable = false;
    jsonPropertyInfoArray[6].IsSetNullable = false;
    JsonPropertyInfoValues<bool> propertyInfoValues8 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).EnableFtues),
      Setter = (Action<object, bool>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).EnableFtues = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "EnableFtues",
      JsonPropertyName = "enable_ftues",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress).GetProperty("EnableFtues", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (bool), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[7] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues8);
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.SerializableEpoch>> propertyInfoValues9 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.SerializableEpoch>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Saves.SerializableEpoch>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Saves.SerializableEpoch>>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).Epochs),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Saves.SerializableEpoch>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).Epochs = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Epochs",
      JsonPropertyName = "epochs",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress).GetProperty("Epochs", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Saves.SerializableEpoch>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[8] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Saves.SerializableEpoch>>(options, propertyInfoValues9);
    jsonPropertyInfoArray[8].IsGetNullable = false;
    jsonPropertyInfoArray[8].IsSetNullable = false;
    JsonPropertyInfoValues<List<string>> propertyInfoValues10 = new JsonPropertyInfoValues<List<string>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress),
      Converter = (JsonConverter<List<string>>) null,
      Getter = (Func<object, List<string>>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).FtueCompleted),
      Setter = (Action<object, List<string>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).FtueCompleted = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "FtueCompleted",
      JsonPropertyName = "ftue_completed",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress).GetProperty("FtueCompleted", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<string>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[9] = JsonMetadataServices.CreatePropertyInfo<List<string>>(options, propertyInfoValues10);
    jsonPropertyInfoArray[9].IsGetNullable = false;
    jsonPropertyInfoArray[9].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.SerializableUnlockedAchievement>> propertyInfoValues11 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.SerializableUnlockedAchievement>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Saves.SerializableUnlockedAchievement>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Saves.SerializableUnlockedAchievement>>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).UnlockedAchievements),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Saves.SerializableUnlockedAchievement>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).UnlockedAchievements = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "UnlockedAchievements",
      JsonPropertyName = "unlocked_achievements",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress).GetProperty("UnlockedAchievements", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Saves.SerializableUnlockedAchievement>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[10] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Saves.SerializableUnlockedAchievement>>(options, propertyInfoValues11);
    jsonPropertyInfoArray[10].IsGetNullable = false;
    jsonPropertyInfoArray[10].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>> propertyInfoValues12 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Models.ModelId>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).DiscoveredCards),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).DiscoveredCards = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "DiscoveredCards",
      JsonPropertyName = "discovered_cards",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress).GetProperty("DiscoveredCards", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Models.ModelId>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[11] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Models.ModelId>>(options, propertyInfoValues12);
    jsonPropertyInfoArray[11].IsGetNullable = false;
    jsonPropertyInfoArray[11].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>> propertyInfoValues13 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Models.ModelId>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).DiscoveredRelics),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).DiscoveredRelics = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "DiscoveredRelics",
      JsonPropertyName = "discovered_relics",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress).GetProperty("DiscoveredRelics", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Models.ModelId>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[12] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Models.ModelId>>(options, propertyInfoValues13);
    jsonPropertyInfoArray[12].IsGetNullable = false;
    jsonPropertyInfoArray[12].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>> propertyInfoValues14 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Models.ModelId>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).DiscoveredEvents),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).DiscoveredEvents = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "DiscoveredEvents",
      JsonPropertyName = "discovered_events",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress).GetProperty("DiscoveredEvents", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Models.ModelId>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[13] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Models.ModelId>>(options, propertyInfoValues14);
    jsonPropertyInfoArray[13].IsGetNullable = false;
    jsonPropertyInfoArray[13].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>> propertyInfoValues15 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Models.ModelId>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).DiscoveredPotions),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).DiscoveredPotions = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "DiscoveredPotions",
      JsonPropertyName = "discovered_potions",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress).GetProperty("DiscoveredPotions", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Models.ModelId>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[14] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Models.ModelId>>(options, propertyInfoValues15);
    jsonPropertyInfoArray[14].IsGetNullable = false;
    jsonPropertyInfoArray[14].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>> propertyInfoValues16 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Models.ModelId>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).DiscoveredActs),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).DiscoveredActs = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "DiscoveredActs",
      JsonPropertyName = "discovered_acts",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress).GetProperty("DiscoveredActs", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Models.ModelId>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[15] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Models.ModelId>>(options, propertyInfoValues16);
    jsonPropertyInfoArray[15].IsGetNullable = false;
    jsonPropertyInfoArray[15].IsSetNullable = false;
    JsonPropertyInfoValues<long> propertyInfoValues17 = new JsonPropertyInfoValues<long>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress),
      Converter = (JsonConverter<long>) null,
      Getter = (Func<object, long>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).TotalPlaytime),
      Setter = (Action<object, long>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).TotalPlaytime = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "TotalPlaytime",
      JsonPropertyName = "total_playtime",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress).GetProperty("TotalPlaytime", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (long), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[16 /*0x10*/] = JsonMetadataServices.CreatePropertyInfo<long>(options, propertyInfoValues17);
    JsonPropertyInfoValues<int> propertyInfoValues18 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).TotalUnlocks),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).TotalUnlocks = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "TotalUnlocks",
      JsonPropertyName = "total_unlocks",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress).GetProperty("TotalUnlocks", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[17] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues18);
    JsonPropertyInfoValues<int> propertyInfoValues19 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).CurrentScore),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).CurrentScore = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "CurrentScore",
      JsonPropertyName = "current_score",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress).GetProperty("CurrentScore", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[18] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues19);
    JsonPropertyInfoValues<long> propertyInfoValues20 = new JsonPropertyInfoValues<long>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress),
      Converter = (JsonConverter<long>) null,
      Getter = (Func<object, long>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).FloorsClimbed),
      Setter = (Action<object, long>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).FloorsClimbed = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "FloorsClimbed",
      JsonPropertyName = "floors_climbed",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress).GetProperty("FloorsClimbed", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (long), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[19] = JsonMetadataServices.CreatePropertyInfo<long>(options, propertyInfoValues20);
    JsonPropertyInfoValues<long> propertyInfoValues21 = new JsonPropertyInfoValues<long>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress),
      Converter = (JsonConverter<long>) null,
      Getter = (Func<object, long>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).ArchitectDamage),
      Setter = (Action<object, long>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).ArchitectDamage = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "ArchitectDamage",
      JsonPropertyName = "architect_damage",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress).GetProperty("ArchitectDamage", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (long), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[20] = JsonMetadataServices.CreatePropertyInfo<long>(options, propertyInfoValues21);
    JsonPropertyInfoValues<int> propertyInfoValues22 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).WongoPoints),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).WongoPoints = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "WongoPoints",
      JsonPropertyName = "wongo_points",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress).GetProperty("WongoPoints", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[21] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues22);
    JsonPropertyInfoValues<int> propertyInfoValues23 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).PreferredMultiplayerAscension),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).PreferredMultiplayerAscension = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "PreferredMultiplayerAscension",
      JsonPropertyName = "preferred_multiplayer_ascension",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress).GetProperty("PreferredMultiplayerAscension", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[22] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues23);
    JsonPropertyInfoValues<int> propertyInfoValues24 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).MaxMultiplayerAscension),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).MaxMultiplayerAscension = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "MaxMultiplayerAscension",
      JsonPropertyName = "max_multiplayer_ascension",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress).GetProperty("MaxMultiplayerAscension", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[23] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues24);
    JsonPropertyInfoValues<int> propertyInfoValues25 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).TestSubjectKills),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).TestSubjectKills = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "TestSubjectKills",
      JsonPropertyName = "test_subject_kills",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress).GetProperty("TestSubjectKills", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[24] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues25);
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId> propertyInfoValues26 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Models.ModelId>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Models.ModelId>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).PendingCharacterUnlock),
      Setter = (Action<object, MegaCrit.Sts2.Core.Models.ModelId>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableProgress) obj).PendingCharacterUnlock = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "PendingCharacterUnlock",
      JsonPropertyName = "pending_character_unlock",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress).GetProperty("PendingCharacterUnlock", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Models.ModelId), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[25] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Models.ModelId>(options, propertyInfoValues26);
    jsonPropertyInfoArray[25].IsGetNullable = false;
    jsonPropertyInfoArray[25].IsSetNullable = false;
    JsonPropertyInfoValues<int> propertyInfoValues27 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) null,
      Setter = (Action<object, int>) null,
      IgnoreCondition = new JsonIgnoreCondition?((JsonIgnoreCondition) 1),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Wins",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress).GetProperty("Wins", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[26] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues27);
    JsonPropertyInfoValues<int> propertyInfoValues28 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) null,
      Setter = (Action<object, int>) null,
      IgnoreCondition = new JsonIgnoreCondition?((JsonIgnoreCondition) 1),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Losses",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress).GetProperty("Losses", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[27] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues28);
    JsonPropertyInfoValues<long> propertyInfoValues29 = new JsonPropertyInfoValues<long>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress),
      Converter = (JsonConverter<long>) null,
      Getter = (Func<object, long>) null,
      Setter = (Action<object, long>) null,
      IgnoreCondition = new JsonIgnoreCondition?((JsonIgnoreCondition) 1),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "FastestVictory",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress).GetProperty("FastestVictory", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (long), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[28] = JsonMetadataServices.CreatePropertyInfo<long>(options, propertyInfoValues29);
    JsonPropertyInfoValues<long> propertyInfoValues30 = new JsonPropertyInfoValues<long>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress),
      Converter = (JsonConverter<long>) null,
      Getter = (Func<object, long>) null,
      Setter = (Action<object, long>) null,
      IgnoreCondition = new JsonIgnoreCondition?((JsonIgnoreCondition) 1),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "BestWinStreak",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress).GetProperty("BestWinStreak", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (long), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[29] = JsonMetadataServices.CreatePropertyInfo<long>(options, propertyInfoValues30);
    JsonPropertyInfoValues<int> propertyInfoValues31 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) null,
      Setter = (Action<object, int>) null,
      IgnoreCondition = new JsonIgnoreCondition?((JsonIgnoreCondition) 1),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "NumberOfRuns",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress).GetProperty("NumberOfRuns", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[30] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues31);
    return jsonPropertyInfoArray;
  }

  private static JsonParameterInfoValues[] SerializableProgressCtorParamInit()
  {
    return new JsonParameterInfoValues[1]
    {
      new JsonParameterInfoValues()
      {
        Name = "UniqueId",
        ParameterType = typeof (string),
        Position = 0,
        IsNullable = false,
        IsMemberInitializer = true
      }
    };
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.SerializableRng> SerializableRng
  {
    get
    {
      return this._SerializableRng ?? (this._SerializableRng = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.SerializableRng>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.SerializableRng)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.SerializableRng> Create_SerializableRng(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.SerializableRng> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.SerializableRng>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.SerializableRng> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.SerializableRng>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Saves.SerializableRng>) (() => new MegaCrit.Sts2.Core.Saves.SerializableRng()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Saves.SerializableRng>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.SerializableRngPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableRng).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Saves.SerializableRng>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Saves.SerializableRng>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] SerializableRngPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[5];
    JsonPropertyInfoValues<int> propertyInfoValues1 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableRng),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableRng) obj).counter),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableRng) obj).counter = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "counter",
      JsonPropertyName = "counter",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableRng).GetField("counter", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues1);
    JsonPropertyInfoValues<ulong> propertyInfoValues2 = new JsonPropertyInfoValues<ulong>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableRng),
      Converter = (JsonConverter<ulong>) null,
      Getter = (Func<object, ulong>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableRng) obj).state0),
      Setter = (Action<object, ulong>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableRng) obj).state0 = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "state0",
      JsonPropertyName = "s0",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableRng).GetField("state0", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<ulong>(options, propertyInfoValues2);
    JsonPropertyInfoValues<ulong> propertyInfoValues3 = new JsonPropertyInfoValues<ulong>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableRng),
      Converter = (JsonConverter<ulong>) null,
      Getter = (Func<object, ulong>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableRng) obj).state1),
      Setter = (Action<object, ulong>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableRng) obj).state1 = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "state1",
      JsonPropertyName = "s1",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableRng).GetField("state1", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<ulong>(options, propertyInfoValues3);
    JsonPropertyInfoValues<ulong> propertyInfoValues4 = new JsonPropertyInfoValues<ulong>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableRng),
      Converter = (JsonConverter<ulong>) null,
      Getter = (Func<object, ulong>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableRng) obj).state2),
      Setter = (Action<object, ulong>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableRng) obj).state2 = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "state2",
      JsonPropertyName = "s2",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableRng).GetField("state2", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[3] = JsonMetadataServices.CreatePropertyInfo<ulong>(options, propertyInfoValues4);
    JsonPropertyInfoValues<ulong> propertyInfoValues5 = new JsonPropertyInfoValues<ulong>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableRng),
      Converter = (JsonConverter<ulong>) null,
      Getter = (Func<object, ulong>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableRng) obj).state3),
      Setter = (Action<object, ulong>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableRng) obj).state3 = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "state3",
      JsonPropertyName = "s3",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableRng).GetField("state3", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[4] = JsonMetadataServices.CreatePropertyInfo<ulong>(options, propertyInfoValues5);
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.SerializableRun> SerializableRun
  {
    get
    {
      return this._SerializableRun ?? (this._SerializableRun = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.SerializableRun>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.SerializableRun)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.SerializableRun> Create_SerializableRun(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.SerializableRun> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.SerializableRun>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.SerializableRun> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.SerializableRun>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Saves.SerializableRun>) (() => new MegaCrit.Sts2.Core.Saves.SerializableRun()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Saves.SerializableRun>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.SerializableRunPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableRun).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Saves.SerializableRun>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Saves.SerializableRun>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] SerializableRunPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[24];
    JsonPropertyInfoValues<int> propertyInfoValues1 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableRun),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableRun) obj).SchemaVersion),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableRun) obj).SchemaVersion = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "SchemaVersion",
      JsonPropertyName = "schema_version",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableRun).GetProperty("SchemaVersion", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues1);
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel>> propertyInfoValues2 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableRun),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel>>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableRun) obj).Acts),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableRun) obj).Acts = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Acts",
      JsonPropertyName = "acts",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableRun).GetProperty("Acts", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel>>(options, propertyInfoValues2);
    jsonPropertyInfoArray[1].IsGetNullable = false;
    jsonPropertyInfoArray[1].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier>> propertyInfoValues3 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableRun),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier>>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableRun) obj).Modifiers),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableRun) obj).Modifiers = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Modifiers",
      JsonPropertyName = "modifiers",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableRun).GetProperty("Modifiers", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier>>(options, propertyInfoValues3);
    jsonPropertyInfoArray[2].IsGetNullable = false;
    jsonPropertyInfoArray[2].IsSetNullable = false;
    JsonPropertyInfoValues<System.DateTimeOffset?> propertyInfoValues4 = new JsonPropertyInfoValues<System.DateTimeOffset?>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableRun),
      Converter = (JsonConverter<System.DateTimeOffset?>) null,
      Getter = (Func<object, System.DateTimeOffset?>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableRun) obj).DailyTime),
      Setter = (Action<object, System.DateTimeOffset?>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableRun) obj).DailyTime = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "DailyTime",
      JsonPropertyName = "dailyTime",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableRun).GetProperty("DailyTime", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (System.DateTimeOffset?), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[3] = JsonMetadataServices.CreatePropertyInfo<System.DateTimeOffset?>(options, propertyInfoValues4);
    JsonPropertyInfoValues<int> propertyInfoValues5 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableRun),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableRun) obj).CurrentActIndex),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableRun) obj).CurrentActIndex = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "CurrentActIndex",
      JsonPropertyName = "current_act_index",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableRun).GetProperty("CurrentActIndex", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[4] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues5);
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>> propertyInfoValues6 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableRun),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Models.ModelId>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableRun) obj).EventsSeen),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableRun) obj).EventsSeen = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "EventsSeen",
      JsonPropertyName = "events_seen",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableRun).GetProperty("EventsSeen", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Models.ModelId>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[5] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Models.ModelId>>(options, propertyInfoValues6);
    jsonPropertyInfoArray[5].IsGetNullable = false;
    jsonPropertyInfoArray[5].IsSetNullable = false;
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom> propertyInfoValues7 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableRun),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableRun) obj).PreFinishedRoom),
      Setter = (Action<object, MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableRun) obj).PreFinishedRoom = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "PreFinishedRoom",
      JsonPropertyName = "pre_finished_room",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableRun).GetProperty("PreFinishedRoom", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[6] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom>(options, propertyInfoValues7);
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableRunOddsSet> propertyInfoValues8 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableRunOddsSet>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableRun),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Saves.Runs.SerializableRunOddsSet>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Saves.Runs.SerializableRunOddsSet>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableRun) obj).SerializableOdds),
      Setter = (Action<object, MegaCrit.Sts2.Core.Saves.Runs.SerializableRunOddsSet>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableRun) obj).SerializableOdds = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "SerializableOdds",
      JsonPropertyName = "odds",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableRun).GetProperty("SerializableOdds", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRunOddsSet), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[7] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableRunOddsSet>(options, propertyInfoValues8);
    jsonPropertyInfoArray[7].IsGetNullable = false;
    jsonPropertyInfoArray[7].IsSetNullable = false;
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelicGrabBag> propertyInfoValues9 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelicGrabBag>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableRun),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelicGrabBag>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Saves.Runs.SerializableRelicGrabBag>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableRun) obj).SerializableSharedRelicGrabBag),
      Setter = (Action<object, MegaCrit.Sts2.Core.Saves.Runs.SerializableRelicGrabBag>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableRun) obj).SerializableSharedRelicGrabBag = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "SerializableSharedRelicGrabBag",
      JsonPropertyName = "shared_relic_grab_bag",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableRun).GetProperty("SerializableSharedRelicGrabBag", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRelicGrabBag), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[8] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelicGrabBag>(options, propertyInfoValues9);
    jsonPropertyInfoArray[8].IsGetNullable = false;
    jsonPropertyInfoArray[8].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer>> propertyInfoValues10 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableRun),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer>>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableRun) obj).Players),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableRun) obj).Players = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Players",
      JsonPropertyName = "players",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableRun).GetProperty("Players", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[9] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer>>(options, propertyInfoValues10);
    jsonPropertyInfoArray[9].IsGetNullable = false;
    jsonPropertyInfoArray[9].IsSetNullable = false;
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableRunRngSet> propertyInfoValues11 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableRunRngSet>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableRun),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Saves.Runs.SerializableRunRngSet>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Saves.Runs.SerializableRunRngSet>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableRun) obj).SerializableRng),
      Setter = (Action<object, MegaCrit.Sts2.Core.Saves.Runs.SerializableRunRngSet>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableRun) obj).SerializableRng = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "SerializableRng",
      JsonPropertyName = "rng",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableRun).GetProperty("SerializableRng", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRunRngSet), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[10] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableRunRngSet>(options, propertyInfoValues11);
    jsonPropertyInfoArray[10].IsGetNullable = false;
    jsonPropertyInfoArray[10].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Map.MapCoord>> propertyInfoValues12 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Map.MapCoord>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableRun),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Map.MapCoord>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Map.MapCoord>>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableRun) obj).VisitedMapCoords),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Map.MapCoord>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableRun) obj).VisitedMapCoords = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "VisitedMapCoords",
      JsonPropertyName = "visited_map_coords",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableRun).GetProperty("VisitedMapCoords", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Map.MapCoord>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[11] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Map.MapCoord>>(options, propertyInfoValues12);
    jsonPropertyInfoArray[11].IsGetNullable = false;
    jsonPropertyInfoArray[11].IsSetNullable = false;
    JsonPropertyInfoValues<List<List<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>>> propertyInfoValues13 = new JsonPropertyInfoValues<List<List<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableRun),
      Converter = (JsonConverter<List<List<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>>>) null,
      Getter = (Func<object, List<List<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>>>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableRun) obj).MapPointHistory),
      Setter = (Action<object, List<List<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableRun) obj).MapPointHistory = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "MapPointHistory",
      JsonPropertyName = "map_point_history",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableRun).GetProperty("MapPointHistory", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<List<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[12] = JsonMetadataServices.CreatePropertyInfo<List<List<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>>>(options, propertyInfoValues13);
    jsonPropertyInfoArray[12].IsGetNullable = false;
    jsonPropertyInfoArray[12].IsSetNullable = false;
    JsonPropertyInfoValues<long> propertyInfoValues14 = new JsonPropertyInfoValues<long>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableRun),
      Converter = (JsonConverter<long>) null,
      Getter = (Func<object, long>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableRun) obj).SaveTime),
      Setter = (Action<object, long>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableRun) obj).SaveTime = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "SaveTime",
      JsonPropertyName = "save_time",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableRun).GetProperty("SaveTime", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (long), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[13] = JsonMetadataServices.CreatePropertyInfo<long>(options, propertyInfoValues14);
    JsonPropertyInfoValues<long> propertyInfoValues15 = new JsonPropertyInfoValues<long>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableRun),
      Converter = (JsonConverter<long>) null,
      Getter = (Func<object, long>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableRun) obj).StartTime),
      Setter = (Action<object, long>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableRun) obj).StartTime = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "StartTime",
      JsonPropertyName = "start_time",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableRun).GetProperty("StartTime", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (long), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[14] = JsonMetadataServices.CreatePropertyInfo<long>(options, propertyInfoValues15);
    JsonPropertyInfoValues<long> propertyInfoValues16 = new JsonPropertyInfoValues<long>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableRun),
      Converter = (JsonConverter<long>) null,
      Getter = (Func<object, long>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableRun) obj).RunTime),
      Setter = (Action<object, long>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableRun) obj).RunTime = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "RunTime",
      JsonPropertyName = "run_time",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableRun).GetProperty("RunTime", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (long), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[15] = JsonMetadataServices.CreatePropertyInfo<long>(options, propertyInfoValues16);
    JsonPropertyInfoValues<long> propertyInfoValues17 = new JsonPropertyInfoValues<long>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableRun),
      Converter = (JsonConverter<long>) null,
      Getter = (Func<object, long>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableRun) obj).WinTime),
      Setter = (Action<object, long>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableRun) obj).WinTime = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "WinTime",
      JsonPropertyName = "win_time",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableRun).GetProperty("WinTime", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (long), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[16 /*0x10*/] = JsonMetadataServices.CreatePropertyInfo<long>(options, propertyInfoValues17);
    JsonPropertyInfoValues<int> propertyInfoValues18 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableRun),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableRun) obj).Ascension),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableRun) obj).Ascension = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Ascension",
      JsonPropertyName = "ascension",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableRun).GetProperty("Ascension", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[17] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues18);
    JsonPropertyInfoValues<int> propertyInfoValues19 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableRun),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableRun) obj).NumReloads),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableRun) obj).NumReloads = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "NumReloads",
      JsonPropertyName = "num_reloads",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableRun).GetProperty("NumReloads", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[18] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues19);
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Platform.PlatformType> propertyInfoValues20 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Platform.PlatformType>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableRun),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Platform.PlatformType>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Platform.PlatformType>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableRun) obj).PlatformType),
      Setter = (Action<object, MegaCrit.Sts2.Core.Platform.PlatformType>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableRun) obj).PlatformType = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "PlatformType",
      JsonPropertyName = "platform_type",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableRun).GetProperty("PlatformType", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Platform.PlatformType), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[19] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Platform.PlatformType>(options, propertyInfoValues20);
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawings> propertyInfoValues21 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawings>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableRun),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawings>) MegaCritSerializerContext.ExpandConverter(typeof (MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawings), (JsonConverter) new SerializableMapDrawingsJsonConverter(), options),
      Getter = (Func<object, MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawings>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableRun) obj).MapDrawings),
      Setter = (Action<object, MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawings>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableRun) obj).MapDrawings = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "MapDrawings",
      JsonPropertyName = "map_drawings",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableRun).GetProperty("MapDrawings", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawings), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[20] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawings>(options, propertyInfoValues21);
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableExtraRunFields> propertyInfoValues22 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Saves.Runs.SerializableExtraRunFields>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableRun),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Saves.Runs.SerializableExtraRunFields>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Saves.Runs.SerializableExtraRunFields>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableRun) obj).ExtraFields),
      Setter = (Action<object, MegaCrit.Sts2.Core.Saves.Runs.SerializableExtraRunFields>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableRun) obj).ExtraFields = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "ExtraFields",
      JsonPropertyName = "extra_fields",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableRun).GetProperty("ExtraFields", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableExtraRunFields), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[21] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Saves.Runs.SerializableExtraRunFields>(options, propertyInfoValues22);
    jsonPropertyInfoArray[21].IsGetNullable = false;
    jsonPropertyInfoArray[21].IsSetNullable = false;
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Runs.GameMode> propertyInfoValues23 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Runs.GameMode>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableRun),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Runs.GameMode>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Runs.GameMode>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableRun) obj).GameMode),
      Setter = (Action<object, MegaCrit.Sts2.Core.Runs.GameMode>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SerializableRun) obj).GameMode = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "GameMode",
      JsonPropertyName = "game_mode",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableRun).GetProperty("GameMode", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Runs.GameMode), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[22] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Runs.GameMode>(options, propertyInfoValues23);
    JsonPropertyInfoValues<int> propertyInfoValues24 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableRun),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) null,
      Setter = (Action<object, int>) null,
      IgnoreCondition = new JsonIgnoreCondition?((JsonIgnoreCondition) 1),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "FloorReached",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableRun).GetProperty("FloorReached", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[23] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues24);
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.SerializableUnlockedAchievement> SerializableUnlockedAchievement
  {
    get
    {
      return this._SerializableUnlockedAchievement ?? (this._SerializableUnlockedAchievement = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.SerializableUnlockedAchievement>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.SerializableUnlockedAchievement)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.SerializableUnlockedAchievement> Create_SerializableUnlockedAchievement(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.SerializableUnlockedAchievement> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.SerializableUnlockedAchievement>(options, out jsonTypeInfo))
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.SerializableUnlockedAchievement> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.SerializableUnlockedAchievement>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Saves.SerializableUnlockedAchievement>) null,
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Saves.SerializableUnlockedAchievement>) (args => new MegaCrit.Sts2.Core.Saves.SerializableUnlockedAchievement()
        {
          Achievement = (string) args[0],
          UnlockTime = (long) args[1]
        }),
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.SerializableUnlockedAchievementPropInit(options)),
        ConstructorParameterMetadataInitializer = MegaCritSerializerContext.\u003C\u003EO.\u003C17\u003E__SerializableUnlockedAchievementCtorParamInit ?? (MegaCritSerializerContext.\u003C\u003EO.\u003C17\u003E__SerializableUnlockedAchievementCtorParamInit = new Func<JsonParameterInfoValues[]>(MegaCritSerializerContext.SerializableUnlockedAchievementCtorParamInit)),
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableUnlockedAchievement).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Saves.SerializableUnlockedAchievement>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Saves.SerializableUnlockedAchievement>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] SerializableUnlockedAchievementPropInit(
    JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[2];
    JsonPropertyInfoValues<string> propertyInfoValues1 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableUnlockedAchievement),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableUnlockedAchievement) obj).Achievement),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Achievement",
      JsonPropertyName = "achievement",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableUnlockedAchievement).GetProperty("Achievement", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues1);
    jsonPropertyInfoArray[0].IsGetNullable = false;
    jsonPropertyInfoArray[0].IsSetNullable = false;
    JsonPropertyInfoValues<long> propertyInfoValues2 = new JsonPropertyInfoValues<long>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SerializableUnlockedAchievement),
      Converter = (JsonConverter<long>) null,
      Getter = (Func<object, long>) (obj => ((MegaCrit.Sts2.Core.Saves.SerializableUnlockedAchievement) obj).UnlockTime),
      Setter = (Action<object, long>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "UnlockTime",
      JsonPropertyName = "unlock_time",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SerializableUnlockedAchievement).GetProperty("UnlockTime", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (long), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<long>(options, propertyInfoValues2);
    return jsonPropertyInfoArray;
  }

  private static JsonParameterInfoValues[] SerializableUnlockedAchievementCtorParamInit()
  {
    return new JsonParameterInfoValues[2]
    {
      new JsonParameterInfoValues()
      {
        Name = "Achievement",
        ParameterType = typeof (string),
        Position = 0,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "UnlockTime",
        ParameterType = typeof (long),
        Position = 1,
        IsNullable = false,
        IsMemberInitializer = true
      }
    };
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.SettingsSave> SettingsSave
  {
    get
    {
      return this._SettingsSave ?? (this._SettingsSave = (JsonTypeInfo<MegaCrit.Sts2.Core.Saves.SettingsSave>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Saves.SettingsSave)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Saves.SettingsSave> Create_SettingsSave(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Saves.SettingsSave> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Saves.SettingsSave>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.SettingsSave> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Saves.SettingsSave>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Saves.SettingsSave>) (() => new MegaCrit.Sts2.Core.Saves.SettingsSave()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Saves.SettingsSave>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.SettingsSavePropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SettingsSave).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Saves.SettingsSave>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Saves.SettingsSave>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] SettingsSavePropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[23];
    JsonPropertyInfoValues<int> propertyInfoValues1 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SettingsSave),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.SettingsSave) obj).SchemaVersion),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SettingsSave) obj).SchemaVersion = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "SchemaVersion",
      JsonPropertyName = "schema_version",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SettingsSave).GetProperty("SchemaVersion", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues1);
    JsonPropertyInfoValues<int> propertyInfoValues2 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SettingsSave),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.SettingsSave) obj).FpsLimit),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SettingsSave) obj).FpsLimit = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "FpsLimit",
      JsonPropertyName = "fps_limit",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SettingsSave).GetProperty("FpsLimit", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues2);
    JsonPropertyInfoValues<string> propertyInfoValues3 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SettingsSave),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Saves.SettingsSave) obj).Language),
      Setter = (Action<object, string>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SettingsSave) obj).Language = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Language",
      JsonPropertyName = "language",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SettingsSave).GetProperty("Language", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues3);
    JsonPropertyInfoValues<Godot.Vector2I> propertyInfoValues4 = new JsonPropertyInfoValues<Godot.Vector2I>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SettingsSave),
      Converter = (JsonConverter<Godot.Vector2I>) null,
      Getter = (Func<object, Godot.Vector2I>) (obj => ((MegaCrit.Sts2.Core.Saves.SettingsSave) obj).WindowPosition),
      Setter = (Action<object, Godot.Vector2I>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SettingsSave) obj).WindowPosition = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "WindowPosition",
      JsonPropertyName = "window_position",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SettingsSave).GetProperty("WindowPosition", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (Godot.Vector2I), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[3] = JsonMetadataServices.CreatePropertyInfo<Godot.Vector2I>(options, propertyInfoValues4);
    JsonPropertyInfoValues<Godot.Vector2I> propertyInfoValues5 = new JsonPropertyInfoValues<Godot.Vector2I>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SettingsSave),
      Converter = (JsonConverter<Godot.Vector2I>) null,
      Getter = (Func<object, Godot.Vector2I>) (obj => ((MegaCrit.Sts2.Core.Saves.SettingsSave) obj).WindowSize),
      Setter = (Action<object, Godot.Vector2I>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SettingsSave) obj).WindowSize = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "WindowSize",
      JsonPropertyName = "window_size",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SettingsSave).GetProperty("WindowSize", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (Godot.Vector2I), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[4] = JsonMetadataServices.CreatePropertyInfo<Godot.Vector2I>(options, propertyInfoValues5);
    JsonPropertyInfoValues<bool> propertyInfoValues6 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SettingsSave),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) (obj => ((MegaCrit.Sts2.Core.Saves.SettingsSave) obj).Fullscreen),
      Setter = (Action<object, bool>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SettingsSave) obj).Fullscreen = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Fullscreen",
      JsonPropertyName = "fullscreen",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SettingsSave).GetProperty("Fullscreen", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (bool), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[5] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues6);
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Settings.AspectRatioSetting> propertyInfoValues7 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Settings.AspectRatioSetting>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SettingsSave),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Settings.AspectRatioSetting>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Settings.AspectRatioSetting>) (obj => ((MegaCrit.Sts2.Core.Saves.SettingsSave) obj).AspectRatioSetting),
      Setter = (Action<object, MegaCrit.Sts2.Core.Settings.AspectRatioSetting>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SettingsSave) obj).AspectRatioSetting = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "AspectRatioSetting",
      JsonPropertyName = "aspect_ratio",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SettingsSave).GetProperty("AspectRatioSetting", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Settings.AspectRatioSetting), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[6] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Settings.AspectRatioSetting>(options, propertyInfoValues7);
    JsonPropertyInfoValues<int> propertyInfoValues8 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SettingsSave),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.SettingsSave) obj).TargetDisplay),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SettingsSave) obj).TargetDisplay = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "TargetDisplay",
      JsonPropertyName = "target_display",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SettingsSave).GetProperty("TargetDisplay", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[7] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues8);
    JsonPropertyInfoValues<bool> propertyInfoValues9 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SettingsSave),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) (obj => ((MegaCrit.Sts2.Core.Saves.SettingsSave) obj).ResizeWindows),
      Setter = (Action<object, bool>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SettingsSave) obj).ResizeWindows = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "ResizeWindows",
      JsonPropertyName = "resize_windows",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SettingsSave).GetProperty("ResizeWindows", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (bool), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[8] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues9);
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Settings.VSyncType> propertyInfoValues10 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Settings.VSyncType>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SettingsSave),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Settings.VSyncType>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Settings.VSyncType>) (obj => ((MegaCrit.Sts2.Core.Saves.SettingsSave) obj).VSync),
      Setter = (Action<object, MegaCrit.Sts2.Core.Settings.VSyncType>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SettingsSave) obj).VSync = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "VSync",
      JsonPropertyName = "vsync",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SettingsSave).GetProperty("VSync", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Settings.VSyncType), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[9] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Settings.VSyncType>(options, propertyInfoValues10);
    JsonPropertyInfoValues<int> propertyInfoValues11 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SettingsSave),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Saves.SettingsSave) obj).Msaa),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SettingsSave) obj).Msaa = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Msaa",
      JsonPropertyName = "msaa",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SettingsSave).GetProperty("Msaa", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[10] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues11);
    JsonPropertyInfoValues<float> propertyInfoValues12 = new JsonPropertyInfoValues<float>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SettingsSave),
      Converter = (JsonConverter<float>) null,
      Getter = (Func<object, float>) (obj => ((MegaCrit.Sts2.Core.Saves.SettingsSave) obj).VolumeBgm),
      Setter = (Action<object, float>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SettingsSave) obj).VolumeBgm = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "VolumeBgm",
      JsonPropertyName = "volume_bgm",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SettingsSave).GetProperty("VolumeBgm", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (float), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[11] = JsonMetadataServices.CreatePropertyInfo<float>(options, propertyInfoValues12);
    JsonPropertyInfoValues<float> propertyInfoValues13 = new JsonPropertyInfoValues<float>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SettingsSave),
      Converter = (JsonConverter<float>) null,
      Getter = (Func<object, float>) (obj => ((MegaCrit.Sts2.Core.Saves.SettingsSave) obj).VolumeMaster),
      Setter = (Action<object, float>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SettingsSave) obj).VolumeMaster = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "VolumeMaster",
      JsonPropertyName = "volume_master",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SettingsSave).GetProperty("VolumeMaster", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (float), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[12] = JsonMetadataServices.CreatePropertyInfo<float>(options, propertyInfoValues13);
    JsonPropertyInfoValues<float> propertyInfoValues14 = new JsonPropertyInfoValues<float>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SettingsSave),
      Converter = (JsonConverter<float>) null,
      Getter = (Func<object, float>) (obj => ((MegaCrit.Sts2.Core.Saves.SettingsSave) obj).VolumeSfx),
      Setter = (Action<object, float>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SettingsSave) obj).VolumeSfx = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "VolumeSfx",
      JsonPropertyName = "volume_sfx",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SettingsSave).GetProperty("VolumeSfx", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (float), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[13] = JsonMetadataServices.CreatePropertyInfo<float>(options, propertyInfoValues14);
    JsonPropertyInfoValues<float> propertyInfoValues15 = new JsonPropertyInfoValues<float>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SettingsSave),
      Converter = (JsonConverter<float>) null,
      Getter = (Func<object, float>) (obj => ((MegaCrit.Sts2.Core.Saves.SettingsSave) obj).VolumeAmbience),
      Setter = (Action<object, float>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SettingsSave) obj).VolumeAmbience = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "VolumeAmbience",
      JsonPropertyName = "volume_ambience",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SettingsSave).GetProperty("VolumeAmbience", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (float), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[14] = JsonMetadataServices.CreatePropertyInfo<float>(options, propertyInfoValues15);
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Modding.ModSettings> propertyInfoValues16 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Modding.ModSettings>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SettingsSave),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Modding.ModSettings>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Modding.ModSettings>) (obj => ((MegaCrit.Sts2.Core.Saves.SettingsSave) obj).ModSettings),
      Setter = (Action<object, MegaCrit.Sts2.Core.Modding.ModSettings>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SettingsSave) obj).ModSettings = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "ModSettings",
      JsonPropertyName = "mod_settings",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SettingsSave).GetProperty("ModSettings", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Modding.ModSettings), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[15] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Modding.ModSettings>(options, propertyInfoValues16);
    JsonPropertyInfoValues<bool> propertyInfoValues17 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SettingsSave),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) (obj => ((MegaCrit.Sts2.Core.Saves.SettingsSave) obj).SkipIntroLogo),
      Setter = (Action<object, bool>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SettingsSave) obj).SkipIntroLogo = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "SkipIntroLogo",
      JsonPropertyName = "skip_intro_logo",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SettingsSave).GetProperty("SkipIntroLogo", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (bool), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[16 /*0x10*/] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues17);
    JsonPropertyInfoValues<Dictionary<string, string>> propertyInfoValues18 = new JsonPropertyInfoValues<Dictionary<string, string>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SettingsSave),
      Converter = (JsonConverter<Dictionary<string, string>>) null,
      Getter = (Func<object, Dictionary<string, string>>) (obj => ((MegaCrit.Sts2.Core.Saves.SettingsSave) obj).KeyboardMapping),
      Setter = (Action<object, Dictionary<string, string>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SettingsSave) obj).KeyboardMapping = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "KeyboardMapping",
      JsonPropertyName = "keyboard_mapping",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SettingsSave).GetProperty("KeyboardMapping", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (Dictionary<string, string>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[17] = JsonMetadataServices.CreatePropertyInfo<Dictionary<string, string>>(options, propertyInfoValues18);
    jsonPropertyInfoArray[17].IsGetNullable = false;
    jsonPropertyInfoArray[17].IsSetNullable = false;
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.ControllerInput.ControllerMappingType> propertyInfoValues19 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.ControllerInput.ControllerMappingType>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SettingsSave),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.ControllerInput.ControllerMappingType>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.ControllerInput.ControllerMappingType>) (obj => ((MegaCrit.Sts2.Core.Saves.SettingsSave) obj).ControllerMappingType),
      Setter = (Action<object, MegaCrit.Sts2.Core.ControllerInput.ControllerMappingType>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SettingsSave) obj).ControllerMappingType = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "ControllerMappingType",
      JsonPropertyName = "controller_mapping_type",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SettingsSave).GetProperty("ControllerMappingType", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.ControllerInput.ControllerMappingType), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[18] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.ControllerInput.ControllerMappingType>(options, propertyInfoValues19);
    JsonPropertyInfoValues<Dictionary<string, string>> propertyInfoValues20 = new JsonPropertyInfoValues<Dictionary<string, string>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SettingsSave),
      Converter = (JsonConverter<Dictionary<string, string>>) null,
      Getter = (Func<object, Dictionary<string, string>>) (obj => ((MegaCrit.Sts2.Core.Saves.SettingsSave) obj).ControllerMapping),
      Setter = (Action<object, Dictionary<string, string>>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SettingsSave) obj).ControllerMapping = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "ControllerMapping",
      JsonPropertyName = "controller_mapping",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SettingsSave).GetProperty("ControllerMapping", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (Dictionary<string, string>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[19] = JsonMetadataServices.CreatePropertyInfo<Dictionary<string, string>>(options, propertyInfoValues20);
    jsonPropertyInfoArray[19].IsGetNullable = false;
    jsonPropertyInfoArray[19].IsSetNullable = false;
    JsonPropertyInfoValues<bool> propertyInfoValues21 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SettingsSave),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) (obj => ((MegaCrit.Sts2.Core.Saves.SettingsSave) obj).LimitFpsInBackground),
      Setter = (Action<object, bool>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SettingsSave) obj).LimitFpsInBackground = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "LimitFpsInBackground",
      JsonPropertyName = "limit_fps_in_background",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SettingsSave).GetProperty("LimitFpsInBackground", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (bool), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[20] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues21);
    JsonPropertyInfoValues<bool> propertyInfoValues22 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SettingsSave),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) (obj => ((MegaCrit.Sts2.Core.Saves.SettingsSave) obj).FullConsole),
      Setter = (Action<object, bool>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SettingsSave) obj).FullConsole = value),
      IgnoreCondition = new JsonIgnoreCondition?((JsonIgnoreCondition) 2),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "FullConsole",
      JsonPropertyName = "full_console",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SettingsSave).GetProperty("FullConsole", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (bool), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[21] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues22);
    JsonPropertyInfoValues<bool> propertyInfoValues23 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Saves.SettingsSave),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) (obj => ((MegaCrit.Sts2.Core.Saves.SettingsSave) obj).SeenEaDisclaimer),
      Setter = (Action<object, bool>) ((obj, value) => ((MegaCrit.Sts2.Core.Saves.SettingsSave) obj).SeenEaDisclaimer = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "SeenEaDisclaimer",
      JsonPropertyName = "seen_ea_disclaimer",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Saves.SettingsSave).GetProperty("SeenEaDisclaimer", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (bool), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[22] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues23);
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Settings.AspectRatioSetting> AspectRatioSetting
  {
    get
    {
      return this._AspectRatioSetting ?? (this._AspectRatioSetting = (JsonTypeInfo<MegaCrit.Sts2.Core.Settings.AspectRatioSetting>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Settings.AspectRatioSetting)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Settings.AspectRatioSetting> Create_AspectRatioSetting(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Settings.AspectRatioSetting> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Settings.AspectRatioSetting>(options, out jsonTypeInfo))
      jsonTypeInfo = JsonMetadataServices.CreateValueInfo<MegaCrit.Sts2.Core.Settings.AspectRatioSetting>(options, (JsonConverter) JsonMetadataServices.GetEnumConverter<MegaCrit.Sts2.Core.Settings.AspectRatioSetting>(options));
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Settings.FastModeType> FastModeType
  {
    get
    {
      return this._FastModeType ?? (this._FastModeType = (JsonTypeInfo<MegaCrit.Sts2.Core.Settings.FastModeType>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Settings.FastModeType)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Settings.FastModeType> Create_FastModeType(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Settings.FastModeType> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Settings.FastModeType>(options, out jsonTypeInfo))
      jsonTypeInfo = JsonMetadataServices.CreateValueInfo<MegaCrit.Sts2.Core.Settings.FastModeType>(options, (JsonConverter) JsonMetadataServices.GetEnumConverter<MegaCrit.Sts2.Core.Settings.FastModeType>(options));
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Settings.VSyncType> VSyncType
  {
    get
    {
      return this._VSyncType ?? (this._VSyncType = (JsonTypeInfo<MegaCrit.Sts2.Core.Settings.VSyncType>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Settings.VSyncType)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Settings.VSyncType> Create_VSyncType(JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Settings.VSyncType> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Settings.VSyncType>(options, out jsonTypeInfo))
      jsonTypeInfo = JsonMetadataServices.CreateValueInfo<MegaCrit.Sts2.Core.Settings.VSyncType>(options, (JsonConverter) JsonMetadataServices.GetEnumConverter<MegaCrit.Sts2.Core.Settings.VSyncType>(options));
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Unlocks.SerializableUnlockState> SerializableUnlockState
  {
    get
    {
      return this._SerializableUnlockState ?? (this._SerializableUnlockState = (JsonTypeInfo<MegaCrit.Sts2.Core.Unlocks.SerializableUnlockState>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Unlocks.SerializableUnlockState)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Unlocks.SerializableUnlockState> Create_SerializableUnlockState(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Unlocks.SerializableUnlockState> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Unlocks.SerializableUnlockState>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Unlocks.SerializableUnlockState> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Unlocks.SerializableUnlockState>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Unlocks.SerializableUnlockState>) (() => new MegaCrit.Sts2.Core.Unlocks.SerializableUnlockState()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Unlocks.SerializableUnlockState>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MegaCritSerializerContext.SerializableUnlockStatePropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Unlocks.SerializableUnlockState).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Unlocks.SerializableUnlockState>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Unlocks.SerializableUnlockState>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] SerializableUnlockStatePropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[3];
    JsonPropertyInfoValues<List<string>> propertyInfoValues1 = new JsonPropertyInfoValues<List<string>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Unlocks.SerializableUnlockState),
      Converter = (JsonConverter<List<string>>) null,
      Getter = (Func<object, List<string>>) (obj => ((MegaCrit.Sts2.Core.Unlocks.SerializableUnlockState) obj).UnlockedEpochs),
      Setter = (Action<object, List<string>>) ((obj, value) => ((MegaCrit.Sts2.Core.Unlocks.SerializableUnlockState) obj).UnlockedEpochs = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "UnlockedEpochs",
      JsonPropertyName = "unlocked_epochs",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Unlocks.SerializableUnlockState).GetProperty("UnlockedEpochs", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<string>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<List<string>>(options, propertyInfoValues1);
    jsonPropertyInfoArray[0].IsGetNullable = false;
    jsonPropertyInfoArray[0].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>> propertyInfoValues2 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Unlocks.SerializableUnlockState),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Models.ModelId>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) (obj => ((MegaCrit.Sts2.Core.Unlocks.SerializableUnlockState) obj).EncountersSeen),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) ((obj, value) => ((MegaCrit.Sts2.Core.Unlocks.SerializableUnlockState) obj).EncountersSeen = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "EncountersSeen",
      JsonPropertyName = "encounters_seen",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Unlocks.SerializableUnlockState).GetProperty("EncountersSeen", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Models.ModelId>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Models.ModelId>>(options, propertyInfoValues2);
    jsonPropertyInfoArray[1].IsGetNullable = false;
    jsonPropertyInfoArray[1].IsSetNullable = false;
    JsonPropertyInfoValues<int> propertyInfoValues3 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Unlocks.SerializableUnlockState),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Unlocks.SerializableUnlockState) obj).NumberOfRuns),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Unlocks.SerializableUnlockState) obj).NumberOfRuns = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "NumberOfRuns",
      JsonPropertyName = "number_of_runs",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Unlocks.SerializableUnlockState).GetProperty("NumberOfRuns", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues3);
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<Dictionary<MegaCrit.Sts2.Core.Entities.Relics.RelicRarity, List<MegaCrit.Sts2.Core.Models.ModelId>>> DictionaryRelicRarityListModelId
  {
    get
    {
      return this._DictionaryRelicRarityListModelId ?? (this._DictionaryRelicRarityListModelId = (JsonTypeInfo<Dictionary<MegaCrit.Sts2.Core.Entities.Relics.RelicRarity, List<MegaCrit.Sts2.Core.Models.ModelId>>>) this.Options.GetTypeInfo(typeof (Dictionary<MegaCrit.Sts2.Core.Entities.Relics.RelicRarity, List<MegaCrit.Sts2.Core.Models.ModelId>>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<Dictionary<MegaCrit.Sts2.Core.Entities.Relics.RelicRarity, List<MegaCrit.Sts2.Core.Models.ModelId>>> Create_DictionaryRelicRarityListModelId(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<Dictionary<MegaCrit.Sts2.Core.Entities.Relics.RelicRarity, List<MegaCrit.Sts2.Core.Models.ModelId>>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<Dictionary<MegaCrit.Sts2.Core.Entities.Relics.RelicRarity, List<MegaCrit.Sts2.Core.Models.ModelId>>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<Dictionary<MegaCrit.Sts2.Core.Entities.Relics.RelicRarity, List<MegaCrit.Sts2.Core.Models.ModelId>>> collectionInfoValues = new JsonCollectionInfoValues<Dictionary<MegaCrit.Sts2.Core.Entities.Relics.RelicRarity, List<MegaCrit.Sts2.Core.Models.ModelId>>>()
      {
        ObjectCreator = (Func<Dictionary<MegaCrit.Sts2.Core.Entities.Relics.RelicRarity, List<MegaCrit.Sts2.Core.Models.ModelId>>>) (() => new Dictionary<MegaCrit.Sts2.Core.Entities.Relics.RelicRarity, List<MegaCrit.Sts2.Core.Models.ModelId>>()),
        SerializeHandler = (Action<Utf8JsonWriter, Dictionary<MegaCrit.Sts2.Core.Entities.Relics.RelicRarity, List<MegaCrit.Sts2.Core.Models.ModelId>>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateDictionaryInfo<Dictionary<MegaCrit.Sts2.Core.Entities.Relics.RelicRarity, List<MegaCrit.Sts2.Core.Models.ModelId>>, MegaCrit.Sts2.Core.Entities.Relics.RelicRarity, List<MegaCrit.Sts2.Core.Models.ModelId>>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<Dictionary<MegaCrit.Sts2.Core.Entities.Rngs.PlayerRngType, MegaCrit.Sts2.Core.Saves.SerializableRng>> DictionaryPlayerRngTypeSerializableRng
  {
    get
    {
      return this._DictionaryPlayerRngTypeSerializableRng ?? (this._DictionaryPlayerRngTypeSerializableRng = (JsonTypeInfo<Dictionary<MegaCrit.Sts2.Core.Entities.Rngs.PlayerRngType, MegaCrit.Sts2.Core.Saves.SerializableRng>>) this.Options.GetTypeInfo(typeof (Dictionary<MegaCrit.Sts2.Core.Entities.Rngs.PlayerRngType, MegaCrit.Sts2.Core.Saves.SerializableRng>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<Dictionary<MegaCrit.Sts2.Core.Entities.Rngs.PlayerRngType, MegaCrit.Sts2.Core.Saves.SerializableRng>> Create_DictionaryPlayerRngTypeSerializableRng(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<Dictionary<MegaCrit.Sts2.Core.Entities.Rngs.PlayerRngType, MegaCrit.Sts2.Core.Saves.SerializableRng>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<Dictionary<MegaCrit.Sts2.Core.Entities.Rngs.PlayerRngType, MegaCrit.Sts2.Core.Saves.SerializableRng>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<Dictionary<MegaCrit.Sts2.Core.Entities.Rngs.PlayerRngType, MegaCrit.Sts2.Core.Saves.SerializableRng>> collectionInfoValues = new JsonCollectionInfoValues<Dictionary<MegaCrit.Sts2.Core.Entities.Rngs.PlayerRngType, MegaCrit.Sts2.Core.Saves.SerializableRng>>()
      {
        ObjectCreator = (Func<Dictionary<MegaCrit.Sts2.Core.Entities.Rngs.PlayerRngType, MegaCrit.Sts2.Core.Saves.SerializableRng>>) (() => new Dictionary<MegaCrit.Sts2.Core.Entities.Rngs.PlayerRngType, MegaCrit.Sts2.Core.Saves.SerializableRng>()),
        SerializeHandler = (Action<Utf8JsonWriter, Dictionary<MegaCrit.Sts2.Core.Entities.Rngs.PlayerRngType, MegaCrit.Sts2.Core.Saves.SerializableRng>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateDictionaryInfo<Dictionary<MegaCrit.Sts2.Core.Entities.Rngs.PlayerRngType, MegaCrit.Sts2.Core.Saves.SerializableRng>, MegaCrit.Sts2.Core.Entities.Rngs.PlayerRngType, MegaCrit.Sts2.Core.Saves.SerializableRng>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<Dictionary<MegaCrit.Sts2.Core.Entities.Rngs.RunRngType, MegaCrit.Sts2.Core.Saves.SerializableRng>> DictionaryRunRngTypeSerializableRng
  {
    get
    {
      return this._DictionaryRunRngTypeSerializableRng ?? (this._DictionaryRunRngTypeSerializableRng = (JsonTypeInfo<Dictionary<MegaCrit.Sts2.Core.Entities.Rngs.RunRngType, MegaCrit.Sts2.Core.Saves.SerializableRng>>) this.Options.GetTypeInfo(typeof (Dictionary<MegaCrit.Sts2.Core.Entities.Rngs.RunRngType, MegaCrit.Sts2.Core.Saves.SerializableRng>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<Dictionary<MegaCrit.Sts2.Core.Entities.Rngs.RunRngType, MegaCrit.Sts2.Core.Saves.SerializableRng>> Create_DictionaryRunRngTypeSerializableRng(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<Dictionary<MegaCrit.Sts2.Core.Entities.Rngs.RunRngType, MegaCrit.Sts2.Core.Saves.SerializableRng>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<Dictionary<MegaCrit.Sts2.Core.Entities.Rngs.RunRngType, MegaCrit.Sts2.Core.Saves.SerializableRng>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<Dictionary<MegaCrit.Sts2.Core.Entities.Rngs.RunRngType, MegaCrit.Sts2.Core.Saves.SerializableRng>> collectionInfoValues = new JsonCollectionInfoValues<Dictionary<MegaCrit.Sts2.Core.Entities.Rngs.RunRngType, MegaCrit.Sts2.Core.Saves.SerializableRng>>()
      {
        ObjectCreator = (Func<Dictionary<MegaCrit.Sts2.Core.Entities.Rngs.RunRngType, MegaCrit.Sts2.Core.Saves.SerializableRng>>) (() => new Dictionary<MegaCrit.Sts2.Core.Entities.Rngs.RunRngType, MegaCrit.Sts2.Core.Saves.SerializableRng>()),
        SerializeHandler = (Action<Utf8JsonWriter, Dictionary<MegaCrit.Sts2.Core.Entities.Rngs.RunRngType, MegaCrit.Sts2.Core.Saves.SerializableRng>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateDictionaryInfo<Dictionary<MegaCrit.Sts2.Core.Entities.Rngs.RunRngType, MegaCrit.Sts2.Core.Saves.SerializableRng>, MegaCrit.Sts2.Core.Entities.Rngs.RunRngType, MegaCrit.Sts2.Core.Saves.SerializableRng>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<Dictionary<string, object>> DictionaryStringObject
  {
    get
    {
      return this._DictionaryStringObject ?? (this._DictionaryStringObject = (JsonTypeInfo<Dictionary<string, object>>) this.Options.GetTypeInfo(typeof (Dictionary<string, object>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<Dictionary<string, object>> Create_DictionaryStringObject(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<Dictionary<string, object>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<Dictionary<string, object>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<Dictionary<string, object>> collectionInfoValues = new JsonCollectionInfoValues<Dictionary<string, object>>()
      {
        ObjectCreator = (Func<Dictionary<string, object>>) (() => new Dictionary<string, object>()),
        SerializeHandler = (Action<Utf8JsonWriter, Dictionary<string, object>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateDictionaryInfo<Dictionary<string, object>, string, object>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<Dictionary<string, string>> DictionaryStringString
  {
    get
    {
      return this._DictionaryStringString ?? (this._DictionaryStringString = (JsonTypeInfo<Dictionary<string, string>>) this.Options.GetTypeInfo(typeof (Dictionary<string, string>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<Dictionary<string, string>> Create_DictionaryStringString(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<Dictionary<string, string>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<Dictionary<string, string>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<Dictionary<string, string>> collectionInfoValues = new JsonCollectionInfoValues<Dictionary<string, string>>()
      {
        ObjectCreator = (Func<Dictionary<string, string>>) (() => new Dictionary<string, string>()),
        SerializeHandler = (Action<Utf8JsonWriter, Dictionary<string, string>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateDictionaryInfo<Dictionary<string, string>, string, string>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<Dictionary<ulong, List<MegaCrit.Sts2.Core.Saves.Runs.SerializableReward>>> DictionaryUInt64ListSerializableReward
  {
    get
    {
      return this._DictionaryUInt64ListSerializableReward ?? (this._DictionaryUInt64ListSerializableReward = (JsonTypeInfo<Dictionary<ulong, List<MegaCrit.Sts2.Core.Saves.Runs.SerializableReward>>>) this.Options.GetTypeInfo(typeof (Dictionary<ulong, List<MegaCrit.Sts2.Core.Saves.Runs.SerializableReward>>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<Dictionary<ulong, List<MegaCrit.Sts2.Core.Saves.Runs.SerializableReward>>> Create_DictionaryUInt64ListSerializableReward(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<Dictionary<ulong, List<MegaCrit.Sts2.Core.Saves.Runs.SerializableReward>>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<Dictionary<ulong, List<MegaCrit.Sts2.Core.Saves.Runs.SerializableReward>>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<Dictionary<ulong, List<MegaCrit.Sts2.Core.Saves.Runs.SerializableReward>>> collectionInfoValues = new JsonCollectionInfoValues<Dictionary<ulong, List<MegaCrit.Sts2.Core.Saves.Runs.SerializableReward>>>()
      {
        ObjectCreator = (Func<Dictionary<ulong, List<MegaCrit.Sts2.Core.Saves.Runs.SerializableReward>>>) (() => new Dictionary<ulong, List<MegaCrit.Sts2.Core.Saves.Runs.SerializableReward>>()),
        SerializeHandler = (Action<Utf8JsonWriter, Dictionary<ulong, List<MegaCrit.Sts2.Core.Saves.Runs.SerializableReward>>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateDictionaryInfo<Dictionary<ulong, List<MegaCrit.Sts2.Core.Saves.Runs.SerializableReward>>, ulong, List<MegaCrit.Sts2.Core.Saves.Runs.SerializableReward>>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableBadge>> IEnumerableSerializableBadge
  {
    get
    {
      return this._IEnumerableSerializableBadge ?? (this._IEnumerableSerializableBadge = (JsonTypeInfo<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableBadge>>) this.Options.GetTypeInfo(typeof (IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableBadge>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableBadge>> Create_IEnumerableSerializableBadge(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableBadge>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableBadge>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableBadge>> collectionInfoValues = new JsonCollectionInfoValues<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableBadge>>()
      {
        ObjectCreator = (Func<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableBadge>>) null,
        SerializeHandler = (Action<Utf8JsonWriter, IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableBadge>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateIEnumerableInfo<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableBadge>, MegaCrit.Sts2.Core.Saves.Runs.SerializableBadge>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>> IEnumerableSerializableCard
  {
    get
    {
      return this._IEnumerableSerializableCard ?? (this._IEnumerableSerializableCard = (JsonTypeInfo<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>) this.Options.GetTypeInfo(typeof (IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>> Create_IEnumerableSerializableCard(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>> collectionInfoValues = new JsonCollectionInfoValues<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>()
      {
        ObjectCreator = (Func<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>) null,
        SerializeHandler = (Action<Utf8JsonWriter, IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateIEnumerableInfo<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>, MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>> IEnumerableSerializablePotion
  {
    get
    {
      return this._IEnumerableSerializablePotion ?? (this._IEnumerableSerializablePotion = (JsonTypeInfo<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>>) this.Options.GetTypeInfo(typeof (IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>> Create_IEnumerableSerializablePotion(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>> collectionInfoValues = new JsonCollectionInfoValues<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>>()
      {
        ObjectCreator = (Func<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>>) null,
        SerializeHandler = (Action<Utf8JsonWriter, IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateIEnumerableInfo<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>, MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>> IEnumerableSerializableRelic
  {
    get
    {
      return this._IEnumerableSerializableRelic ?? (this._IEnumerableSerializableRelic = (JsonTypeInfo<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>>) this.Options.GetTypeInfo(typeof (IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>> Create_IEnumerableSerializableRelic(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>> collectionInfoValues = new JsonCollectionInfoValues<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>>()
      {
        ObjectCreator = (Func<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>>) null,
        SerializeHandler = (Action<Utf8JsonWriter, IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateIEnumerableInfo<IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>, MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<Godot.Vector2>> ListVector2
  {
    get
    {
      return this._ListVector2 ?? (this._ListVector2 = (JsonTypeInfo<List<Godot.Vector2>>) this.Options.GetTypeInfo(typeof (List<Godot.Vector2>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<Godot.Vector2>> Create_ListVector2(JsonSerializerOptions options)
  {
    JsonTypeInfo<List<Godot.Vector2>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<Godot.Vector2>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<Godot.Vector2>> collectionInfoValues = new JsonCollectionInfoValues<List<Godot.Vector2>>()
      {
        ObjectCreator = (Func<List<Godot.Vector2>>) (() => new List<Godot.Vector2>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<Godot.Vector2>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<Godot.Vector2>, Godot.Vector2>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Map.MapCoord>> ListMapCoord
  {
    get
    {
      return this._ListMapCoord ?? (this._ListMapCoord = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Map.MapCoord>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Map.MapCoord>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Map.MapCoord>> Create_ListMapCoord(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Map.MapCoord>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Map.MapCoord>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Map.MapCoord>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Map.MapCoord>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Map.MapCoord>>) (() => new List<MegaCrit.Sts2.Core.Map.MapCoord>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Map.MapCoord>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Map.MapCoord>, MegaCrit.Sts2.Core.Map.MapCoord>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Modding.ModDependency>> ListModDependency
  {
    get
    {
      return this._ListModDependency ?? (this._ListModDependency = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Modding.ModDependency>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Modding.ModDependency>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Modding.ModDependency>> Create_ListModDependency(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Modding.ModDependency>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Modding.ModDependency>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Modding.ModDependency>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Modding.ModDependency>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Modding.ModDependency>>) (() => new List<MegaCrit.Sts2.Core.Modding.ModDependency>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Modding.ModDependency>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Modding.ModDependency>, MegaCrit.Sts2.Core.Modding.ModDependency>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Modding.SettingsSaveMod>> ListSettingsSaveMod
  {
    get
    {
      return this._ListSettingsSaveMod ?? (this._ListSettingsSaveMod = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Modding.SettingsSaveMod>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Modding.SettingsSaveMod>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Modding.SettingsSaveMod>> Create_ListSettingsSaveMod(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Modding.SettingsSaveMod>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Modding.SettingsSaveMod>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Modding.SettingsSaveMod>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Modding.SettingsSaveMod>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Modding.SettingsSaveMod>>) (() => new List<MegaCrit.Sts2.Core.Modding.SettingsSaveMod>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Modding.SettingsSaveMod>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Modding.SettingsSaveMod>, MegaCrit.Sts2.Core.Modding.SettingsSaveMod>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Models.ModelId>> ListModelId
  {
    get
    {
      return this._ListModelId ?? (this._ListModelId = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Models.ModelId>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Models.ModelId>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Models.ModelId>> Create_ListModelId(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Models.ModelId>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Models.ModelId>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Models.ModelId>>) (() => new List<MegaCrit.Sts2.Core.Models.ModelId>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Models.ModelId>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Models.ModelId>, MegaCrit.Sts2.Core.Models.ModelId>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboard>> ListNullLeaderboard
  {
    get
    {
      return this._ListNullLeaderboard ?? (this._ListNullLeaderboard = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboard>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboard>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboard>> Create_ListNullLeaderboard(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboard>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboard>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboard>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboard>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboard>>) (() => new List<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboard>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboard>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboard>, MegaCrit.Sts2.Core.Platform.Null.NullLeaderboard>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry>> ListNullLeaderboardFileEntry
  {
    get
    {
      return this._ListNullLeaderboardFileEntry ?? (this._ListNullLeaderboardFileEntry = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry>> Create_ListNullLeaderboardFileEntry(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry>>) (() => new List<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry>, MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Platform.Null.NullMultiplayerName>> ListNullMultiplayerName
  {
    get
    {
      return this._ListNullMultiplayerName ?? (this._ListNullMultiplayerName = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Platform.Null.NullMultiplayerName>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Platform.Null.NullMultiplayerName>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Platform.Null.NullMultiplayerName>> Create_ListNullMultiplayerName(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Platform.Null.NullMultiplayerName>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Platform.Null.NullMultiplayerName>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Platform.Null.NullMultiplayerName>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Platform.Null.NullMultiplayerName>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Platform.Null.NullMultiplayerName>>) (() => new List<MegaCrit.Sts2.Core.Platform.Null.NullMultiplayerName>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Platform.Null.NullMultiplayerName>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Platform.Null.NullMultiplayerName>, MegaCrit.Sts2.Core.Platform.Null.NullMultiplayerName>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry>> ListAncientChoiceHistoryEntry
  {
    get
    {
      return this._ListAncientChoiceHistoryEntry ?? (this._ListAncientChoiceHistoryEntry = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry>> Create_ListAncientChoiceHistoryEntry(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry>>) (() => new List<MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry>, MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.History.CardChoiceHistoryEntry>> ListCardChoiceHistoryEntry
  {
    get
    {
      return this._ListCardChoiceHistoryEntry ?? (this._ListCardChoiceHistoryEntry = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.History.CardChoiceHistoryEntry>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Runs.History.CardChoiceHistoryEntry>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.History.CardChoiceHistoryEntry>> Create_ListCardChoiceHistoryEntry(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.History.CardChoiceHistoryEntry>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Runs.History.CardChoiceHistoryEntry>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Runs.History.CardChoiceHistoryEntry>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Runs.History.CardChoiceHistoryEntry>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Runs.History.CardChoiceHistoryEntry>>) (() => new List<MegaCrit.Sts2.Core.Runs.History.CardChoiceHistoryEntry>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Runs.History.CardChoiceHistoryEntry>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Runs.History.CardChoiceHistoryEntry>, MegaCrit.Sts2.Core.Runs.History.CardChoiceHistoryEntry>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.History.CardEnchantmentHistoryEntry>> ListCardEnchantmentHistoryEntry
  {
    get
    {
      return this._ListCardEnchantmentHistoryEntry ?? (this._ListCardEnchantmentHistoryEntry = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.History.CardEnchantmentHistoryEntry>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Runs.History.CardEnchantmentHistoryEntry>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.History.CardEnchantmentHistoryEntry>> Create_ListCardEnchantmentHistoryEntry(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.History.CardEnchantmentHistoryEntry>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Runs.History.CardEnchantmentHistoryEntry>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Runs.History.CardEnchantmentHistoryEntry>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Runs.History.CardEnchantmentHistoryEntry>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Runs.History.CardEnchantmentHistoryEntry>>) (() => new List<MegaCrit.Sts2.Core.Runs.History.CardEnchantmentHistoryEntry>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Runs.History.CardEnchantmentHistoryEntry>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Runs.History.CardEnchantmentHistoryEntry>, MegaCrit.Sts2.Core.Runs.History.CardEnchantmentHistoryEntry>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.History.CardTransformationHistoryEntry>> ListCardTransformationHistoryEntry
  {
    get
    {
      return this._ListCardTransformationHistoryEntry ?? (this._ListCardTransformationHistoryEntry = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.History.CardTransformationHistoryEntry>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Runs.History.CardTransformationHistoryEntry>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.History.CardTransformationHistoryEntry>> Create_ListCardTransformationHistoryEntry(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.History.CardTransformationHistoryEntry>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Runs.History.CardTransformationHistoryEntry>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Runs.History.CardTransformationHistoryEntry>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Runs.History.CardTransformationHistoryEntry>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Runs.History.CardTransformationHistoryEntry>>) (() => new List<MegaCrit.Sts2.Core.Runs.History.CardTransformationHistoryEntry>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Runs.History.CardTransformationHistoryEntry>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Runs.History.CardTransformationHistoryEntry>, MegaCrit.Sts2.Core.Runs.History.CardTransformationHistoryEntry>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.History.EventOptionHistoryEntry>> ListEventOptionHistoryEntry
  {
    get
    {
      return this._ListEventOptionHistoryEntry ?? (this._ListEventOptionHistoryEntry = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.History.EventOptionHistoryEntry>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Runs.History.EventOptionHistoryEntry>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.History.EventOptionHistoryEntry>> Create_ListEventOptionHistoryEntry(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.History.EventOptionHistoryEntry>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Runs.History.EventOptionHistoryEntry>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Runs.History.EventOptionHistoryEntry>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Runs.History.EventOptionHistoryEntry>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Runs.History.EventOptionHistoryEntry>>) (() => new List<MegaCrit.Sts2.Core.Runs.History.EventOptionHistoryEntry>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Runs.History.EventOptionHistoryEntry>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Runs.History.EventOptionHistoryEntry>, MegaCrit.Sts2.Core.Runs.History.EventOptionHistoryEntry>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>> ListMapPointHistoryEntry
  {
    get
    {
      return this._ListMapPointHistoryEntry ?? (this._ListMapPointHistoryEntry = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>> Create_ListMapPointHistoryEntry(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>>) (() => new List<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>, MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry>> ListMapPointRoomHistoryEntry
  {
    get
    {
      return this._ListMapPointRoomHistoryEntry ?? (this._ListMapPointRoomHistoryEntry = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry>> Create_ListMapPointRoomHistoryEntry(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry>>) (() => new List<MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry>, MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry>> ListModelChoiceHistoryEntry
  {
    get
    {
      return this._ListModelChoiceHistoryEntry ?? (this._ListModelChoiceHistoryEntry = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry>> Create_ListModelChoiceHistoryEntry(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry>>) (() => new List<MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry>, MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry>> ListPlayerMapPointHistoryEntry
  {
    get
    {
      return this._ListPlayerMapPointHistoryEntry ?? (this._ListPlayerMapPointHistoryEntry = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry>> Create_ListPlayerMapPointHistoryEntry(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry>>) (() => new List<MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry>, MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.RunHistoryPlayer>> ListRunHistoryPlayer
  {
    get
    {
      return this._ListRunHistoryPlayer ?? (this._ListRunHistoryPlayer = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.RunHistoryPlayer>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Runs.RunHistoryPlayer>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.RunHistoryPlayer>> Create_ListRunHistoryPlayer(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.RunHistoryPlayer>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Runs.RunHistoryPlayer>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Runs.RunHistoryPlayer>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Runs.RunHistoryPlayer>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Runs.RunHistoryPlayer>>) (() => new List<MegaCrit.Sts2.Core.Runs.RunHistoryPlayer>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Runs.RunHistoryPlayer>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Runs.RunHistoryPlayer>, MegaCrit.Sts2.Core.Runs.RunHistoryPlayer>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.AncientCharacterStats>> ListAncientCharacterStats
  {
    get
    {
      return this._ListAncientCharacterStats ?? (this._ListAncientCharacterStats = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.AncientCharacterStats>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Saves.AncientCharacterStats>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.AncientCharacterStats>> Create_ListAncientCharacterStats(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.AncientCharacterStats>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Saves.AncientCharacterStats>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.AncientCharacterStats>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.AncientCharacterStats>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Saves.AncientCharacterStats>>) (() => new List<MegaCrit.Sts2.Core.Saves.AncientCharacterStats>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Saves.AncientCharacterStats>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Saves.AncientCharacterStats>, MegaCrit.Sts2.Core.Saves.AncientCharacterStats>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.AncientStats>> ListAncientStats
  {
    get
    {
      return this._ListAncientStats ?? (this._ListAncientStats = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.AncientStats>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Saves.AncientStats>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.AncientStats>> Create_ListAncientStats(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.AncientStats>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Saves.AncientStats>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.AncientStats>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.AncientStats>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Saves.AncientStats>>) (() => new List<MegaCrit.Sts2.Core.Saves.AncientStats>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Saves.AncientStats>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Saves.AncientStats>, MegaCrit.Sts2.Core.Saves.AncientStats>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.BadgeStats>> ListBadgeStats
  {
    get
    {
      return this._ListBadgeStats ?? (this._ListBadgeStats = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.BadgeStats>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Saves.BadgeStats>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.BadgeStats>> Create_ListBadgeStats(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.BadgeStats>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Saves.BadgeStats>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.BadgeStats>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.BadgeStats>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Saves.BadgeStats>>) (() => new List<MegaCrit.Sts2.Core.Saves.BadgeStats>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Saves.BadgeStats>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Saves.BadgeStats>, MegaCrit.Sts2.Core.Saves.BadgeStats>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.CardStats>> ListCardStats
  {
    get
    {
      return this._ListCardStats ?? (this._ListCardStats = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.CardStats>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Saves.CardStats>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.CardStats>> Create_ListCardStats(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.CardStats>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Saves.CardStats>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.CardStats>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.CardStats>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Saves.CardStats>>) (() => new List<MegaCrit.Sts2.Core.Saves.CardStats>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Saves.CardStats>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Saves.CardStats>, MegaCrit.Sts2.Core.Saves.CardStats>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.CharacterStats>> ListCharacterStats
  {
    get
    {
      return this._ListCharacterStats ?? (this._ListCharacterStats = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.CharacterStats>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Saves.CharacterStats>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.CharacterStats>> Create_ListCharacterStats(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.CharacterStats>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Saves.CharacterStats>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.CharacterStats>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.CharacterStats>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Saves.CharacterStats>>) (() => new List<MegaCrit.Sts2.Core.Saves.CharacterStats>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Saves.CharacterStats>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Saves.CharacterStats>, MegaCrit.Sts2.Core.Saves.CharacterStats>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.EncounterStats>> ListEncounterStats
  {
    get
    {
      return this._ListEncounterStats ?? (this._ListEncounterStats = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.EncounterStats>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Saves.EncounterStats>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.EncounterStats>> Create_ListEncounterStats(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.EncounterStats>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Saves.EncounterStats>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.EncounterStats>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.EncounterStats>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Saves.EncounterStats>>) (() => new List<MegaCrit.Sts2.Core.Saves.EncounterStats>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Saves.EncounterStats>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Saves.EncounterStats>, MegaCrit.Sts2.Core.Saves.EncounterStats>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.EnemyStats>> ListEnemyStats
  {
    get
    {
      return this._ListEnemyStats ?? (this._ListEnemyStats = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.EnemyStats>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Saves.EnemyStats>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.EnemyStats>> Create_ListEnemyStats(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.EnemyStats>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Saves.EnemyStats>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.EnemyStats>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.EnemyStats>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Saves.EnemyStats>>) (() => new List<MegaCrit.Sts2.Core.Saves.EnemyStats>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Saves.EnemyStats>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Saves.EnemyStats>, MegaCrit.Sts2.Core.Saves.EnemyStats>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.FightStats>> ListFightStats
  {
    get
    {
      return this._ListFightStats ?? (this._ListFightStats = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.FightStats>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Saves.FightStats>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.FightStats>> Create_ListFightStats(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.FightStats>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Saves.FightStats>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.FightStats>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.FightStats>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Saves.FightStats>>) (() => new List<MegaCrit.Sts2.Core.Saves.FightStats>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Saves.FightStats>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Saves.FightStats>, MegaCrit.Sts2.Core.Saves.FightStats>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawingLine>> ListSerializableMapDrawingLine
  {
    get
    {
      return this._ListSerializableMapDrawingLine ?? (this._ListSerializableMapDrawingLine = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawingLine>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawingLine>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawingLine>> Create_ListSerializableMapDrawingLine(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawingLine>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawingLine>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawingLine>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawingLine>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawingLine>>) (() => new List<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawingLine>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawingLine>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawingLine>, MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawingLine>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializablePlayerMapDrawings>> ListSerializablePlayerMapDrawings
  {
    get
    {
      return this._ListSerializablePlayerMapDrawings ?? (this._ListSerializablePlayerMapDrawings = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializablePlayerMapDrawings>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializablePlayerMapDrawings>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializablePlayerMapDrawings>> Create_ListSerializablePlayerMapDrawings(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializablePlayerMapDrawings>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializablePlayerMapDrawings>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializablePlayerMapDrawings>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializablePlayerMapDrawings>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializablePlayerMapDrawings>>) (() => new List<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializablePlayerMapDrawings>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializablePlayerMapDrawings>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializablePlayerMapDrawings>, MegaCrit.Sts2.Core.Saves.MapDrawing.SerializablePlayerMapDrawings>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Migrations.MigratingData>> ListMigratingData
  {
    get
    {
      return this._ListMigratingData ?? (this._ListMigratingData = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Migrations.MigratingData>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Saves.Migrations.MigratingData>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Migrations.MigratingData>> Create_ListMigratingData(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Migrations.MigratingData>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Saves.Migrations.MigratingData>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.Migrations.MigratingData>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.Migrations.MigratingData>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Saves.Migrations.MigratingData>>) (() => new List<MegaCrit.Sts2.Core.Saves.Migrations.MigratingData>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Saves.Migrations.MigratingData>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Saves.Migrations.MigratingData>, MegaCrit.Sts2.Core.Saves.Migrations.MigratingData>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<bool>>> ListSavedPropertyBoolean
  {
    get
    {
      return this._ListSavedPropertyBoolean ?? (this._ListSavedPropertyBoolean = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<bool>>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<bool>>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<bool>>> Create_ListSavedPropertyBoolean(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<bool>>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<bool>>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<bool>>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<bool>>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<bool>>>) (() => new List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<bool>>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<bool>>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<bool>>, MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<bool>>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Models.ModelId>>> ListSavedPropertyModelId
  {
    get
    {
      return this._ListSavedPropertyModelId ?? (this._ListSavedPropertyModelId = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Models.ModelId>>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Models.ModelId>>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Models.ModelId>>> Create_ListSavedPropertyModelId(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Models.ModelId>>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Models.ModelId>>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Models.ModelId>>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Models.ModelId>>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Models.ModelId>>>) (() => new List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Models.ModelId>>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Models.ModelId>>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Models.ModelId>>, MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Models.ModelId>>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>>> ListSavedPropertySerializableCardArray
  {
    get
    {
      return this._ListSavedPropertySerializableCardArray ?? (this._ListSavedPropertySerializableCardArray = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>>> Create_ListSavedPropertySerializableCardArray(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>>>) (() => new List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>>, MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>> ListSavedPropertySerializableCard
  {
    get
    {
      return this._ListSavedPropertySerializableCard ?? (this._ListSavedPropertySerializableCard = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>> Create_ListSavedPropertySerializableCard(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>>) (() => new List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>, MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int[]>>> ListSavedPropertyInt32Array
  {
    get
    {
      return this._ListSavedPropertyInt32Array ?? (this._ListSavedPropertyInt32Array = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int[]>>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int[]>>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int[]>>> Create_ListSavedPropertyInt32Array(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int[]>>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int[]>>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int[]>>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int[]>>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int[]>>>) (() => new List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int[]>>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int[]>>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int[]>>, MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int[]>>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int>>> ListSavedPropertyInt32
  {
    get
    {
      return this._ListSavedPropertyInt32 ?? (this._ListSavedPropertyInt32 = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int>>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int>>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int>>> Create_ListSavedPropertyInt32(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int>>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int>>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int>>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int>>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int>>>) (() => new List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int>>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int>>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int>>, MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int>>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<string>>> ListSavedPropertyString
  {
    get
    {
      return this._ListSavedPropertyString ?? (this._ListSavedPropertyString = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<string>>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<string>>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<string>>> Create_ListSavedPropertyString(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<string>>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<string>>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<string>>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<string>>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<string>>>) (() => new List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<string>>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<string>>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<string>>, MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<string>>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel>> ListSerializableActModel
  {
    get
    {
      return this._ListSerializableActModel ?? (this._ListSerializableActModel = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel>> Create_ListSerializableActModel(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel>>) (() => new List<MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel>, MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>> ListSerializableCard
  {
    get
    {
      return this._ListSerializableCard ?? (this._ListSerializableCard = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>> Create_ListSerializableCard(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>) (() => new List<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>, MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint>> ListSerializableMapPoint
  {
    get
    {
      return this._ListSerializableMapPoint ?? (this._ListSerializableMapPoint = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint>> Create_ListSerializableMapPoint(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint>>) (() => new List<MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint>, MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier>> ListSerializableModifier
  {
    get
    {
      return this._ListSerializableModifier ?? (this._ListSerializableModifier = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier>> Create_ListSerializableModifier(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier>>) (() => new List<MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier>, MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer>> ListSerializablePlayer
  {
    get
    {
      return this._ListSerializablePlayer ?? (this._ListSerializablePlayer = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer>> Create_ListSerializablePlayer(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer>>) (() => new List<MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer>, MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>> ListSerializablePotion
  {
    get
    {
      return this._ListSerializablePotion ?? (this._ListSerializablePotion = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>> Create_ListSerializablePotion(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>>) (() => new List<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>, MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>> ListSerializableRelic
  {
    get
    {
      return this._ListSerializableRelic ?? (this._ListSerializableRelic = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>> Create_ListSerializableRelic(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>>) (() => new List<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>, MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableReward>> ListSerializableReward
  {
    get
    {
      return this._ListSerializableReward ?? (this._ListSerializableReward = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableReward>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Saves.Runs.SerializableReward>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableReward>> Create_ListSerializableReward(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableReward>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableReward>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableReward>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableReward>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableReward>>) (() => new List<MegaCrit.Sts2.Core.Saves.Runs.SerializableReward>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Saves.Runs.SerializableReward>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Saves.Runs.SerializableReward>, MegaCrit.Sts2.Core.Saves.Runs.SerializableReward>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.SerializableEpoch>> ListSerializableEpoch
  {
    get
    {
      return this._ListSerializableEpoch ?? (this._ListSerializableEpoch = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.SerializableEpoch>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Saves.SerializableEpoch>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.SerializableEpoch>> Create_ListSerializableEpoch(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.SerializableEpoch>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Saves.SerializableEpoch>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.SerializableEpoch>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.SerializableEpoch>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Saves.SerializableEpoch>>) (() => new List<MegaCrit.Sts2.Core.Saves.SerializableEpoch>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Saves.SerializableEpoch>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Saves.SerializableEpoch>, MegaCrit.Sts2.Core.Saves.SerializableEpoch>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.SerializableUnlockedAchievement>> ListSerializableUnlockedAchievement
  {
    get
    {
      return this._ListSerializableUnlockedAchievement ?? (this._ListSerializableUnlockedAchievement = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.SerializableUnlockedAchievement>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Saves.SerializableUnlockedAchievement>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.SerializableUnlockedAchievement>> Create_ListSerializableUnlockedAchievement(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Saves.SerializableUnlockedAchievement>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Saves.SerializableUnlockedAchievement>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.SerializableUnlockedAchievement>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Saves.SerializableUnlockedAchievement>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Saves.SerializableUnlockedAchievement>>) (() => new List<MegaCrit.Sts2.Core.Saves.SerializableUnlockedAchievement>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Saves.SerializableUnlockedAchievement>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Saves.SerializableUnlockedAchievement>, MegaCrit.Sts2.Core.Saves.SerializableUnlockedAchievement>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<Dictionary<string, object>>> ListDictionaryStringObject
  {
    get
    {
      return this._ListDictionaryStringObject ?? (this._ListDictionaryStringObject = (JsonTypeInfo<List<Dictionary<string, object>>>) this.Options.GetTypeInfo(typeof (List<Dictionary<string, object>>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<Dictionary<string, object>>> Create_ListDictionaryStringObject(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<Dictionary<string, object>>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<Dictionary<string, object>>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<Dictionary<string, object>>> collectionInfoValues = new JsonCollectionInfoValues<List<Dictionary<string, object>>>()
      {
        ObjectCreator = (Func<List<Dictionary<string, object>>>) (() => new List<Dictionary<string, object>>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<Dictionary<string, object>>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<Dictionary<string, object>>, Dictionary<string, object>>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<List<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>>> ListListMapPointHistoryEntry
  {
    get
    {
      return this._ListListMapPointHistoryEntry ?? (this._ListListMapPointHistoryEntry = (JsonTypeInfo<List<List<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>>>) this.Options.GetTypeInfo(typeof (List<List<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<List<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>>> Create_ListListMapPointHistoryEntry(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<List<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<List<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<List<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>>> collectionInfoValues = new JsonCollectionInfoValues<List<List<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>>>()
      {
        ObjectCreator = (Func<List<List<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>>>) (() => new List<List<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<List<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<List<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>>, List<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<List<MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry>>> ListListPlayerMapPointHistoryEntry
  {
    get
    {
      return this._ListListPlayerMapPointHistoryEntry ?? (this._ListListPlayerMapPointHistoryEntry = (JsonTypeInfo<List<List<MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry>>>) this.Options.GetTypeInfo(typeof (List<List<MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry>>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<List<MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry>>> Create_ListListPlayerMapPointHistoryEntry(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<List<MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry>>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<List<MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry>>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<List<MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry>>> collectionInfoValues = new JsonCollectionInfoValues<List<List<MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry>>>()
      {
        ObjectCreator = (Func<List<List<MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry>>>) (() => new List<List<MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry>>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<List<MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry>>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<List<MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry>>, List<MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry>>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<System.Text.Json.Nodes.JsonNode>> ListJsonNode
  {
    get
    {
      return this._ListJsonNode ?? (this._ListJsonNode = (JsonTypeInfo<List<System.Text.Json.Nodes.JsonNode>>) this.Options.GetTypeInfo(typeof (List<System.Text.Json.Nodes.JsonNode>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<System.Text.Json.Nodes.JsonNode>> Create_ListJsonNode(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<System.Text.Json.Nodes.JsonNode>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<System.Text.Json.Nodes.JsonNode>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<System.Text.Json.Nodes.JsonNode>> collectionInfoValues = new JsonCollectionInfoValues<List<System.Text.Json.Nodes.JsonNode>>()
      {
        ObjectCreator = (Func<List<System.Text.Json.Nodes.JsonNode>>) (() => new List<System.Text.Json.Nodes.JsonNode>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<System.Text.Json.Nodes.JsonNode>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<System.Text.Json.Nodes.JsonNode>, System.Text.Json.Nodes.JsonNode>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<string>> ListString
  {
    get
    {
      return this._ListString ?? (this._ListString = (JsonTypeInfo<List<string>>) this.Options.GetTypeInfo(typeof (List<string>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<string>> Create_ListString(JsonSerializerOptions options)
  {
    JsonTypeInfo<List<string>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<string>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<string>> collectionInfoValues = new JsonCollectionInfoValues<List<string>>()
      {
        ObjectCreator = (Func<List<string>>) (() => new List<string>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<string>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<string>, string>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<List<ulong>> ListUInt64
  {
    get
    {
      return this._ListUInt64 ?? (this._ListUInt64 = (JsonTypeInfo<List<ulong>>) this.Options.GetTypeInfo(typeof (List<ulong>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<ulong>> Create_ListUInt64(JsonSerializerOptions options)
  {
    JsonTypeInfo<List<ulong>> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<ulong>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<ulong>> collectionInfoValues = new JsonCollectionInfoValues<List<ulong>>()
      {
        ObjectCreator = (Func<List<ulong>>) (() => new List<ulong>()),
        SerializeHandler = (Action<Utf8JsonWriter, List<ulong>>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<ulong>, ulong>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<System.DateTimeOffset> DateTimeOffset
  {
    get
    {
      return this._DateTimeOffset ?? (this._DateTimeOffset = (JsonTypeInfo<System.DateTimeOffset>) this.Options.GetTypeInfo(typeof (System.DateTimeOffset)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<System.DateTimeOffset> Create_DateTimeOffset(JsonSerializerOptions options)
  {
    JsonTypeInfo<System.DateTimeOffset> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<System.DateTimeOffset>(options, out jsonTypeInfo))
      jsonTypeInfo = JsonMetadataServices.CreateValueInfo<System.DateTimeOffset>(options, (JsonConverter) JsonMetadataServices.DateTimeOffsetConverter);
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<System.DateTimeOffset?> NullableDateTimeOffset
  {
    get
    {
      return this._NullableDateTimeOffset ?? (this._NullableDateTimeOffset = (JsonTypeInfo<System.DateTimeOffset?>) this.Options.GetTypeInfo(typeof (System.DateTimeOffset?)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<System.DateTimeOffset?> Create_NullableDateTimeOffset(JsonSerializerOptions options)
  {
    JsonTypeInfo<System.DateTimeOffset?> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<System.DateTimeOffset?>(options, out jsonTypeInfo))
    {
      JsonConverter nullableConverter = (JsonConverter) JsonMetadataServices.GetNullableConverter<System.DateTimeOffset>(options);
      jsonTypeInfo = JsonMetadataServices.CreateValueInfo<System.DateTimeOffset?>(options, nullableConverter);
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<System.Text.Json.Nodes.JsonNode> JsonNode
  {
    get
    {
      return this._JsonNode ?? (this._JsonNode = (JsonTypeInfo<System.Text.Json.Nodes.JsonNode>) this.Options.GetTypeInfo(typeof (System.Text.Json.Nodes.JsonNode)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<System.Text.Json.Nodes.JsonNode> Create_JsonNode(JsonSerializerOptions options)
  {
    JsonTypeInfo<System.Text.Json.Nodes.JsonNode> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<System.Text.Json.Nodes.JsonNode>(options, out jsonTypeInfo))
      jsonTypeInfo = JsonMetadataServices.CreateValueInfo<System.Text.Json.Nodes.JsonNode>(options, (JsonConverter) JsonMetadataServices.JsonNodeConverter);
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<System.Text.Json.Nodes.JsonObject> JsonObject
  {
    get
    {
      return this._JsonObject ?? (this._JsonObject = (JsonTypeInfo<System.Text.Json.Nodes.JsonObject>) this.Options.GetTypeInfo(typeof (System.Text.Json.Nodes.JsonObject)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<System.Text.Json.Nodes.JsonObject> Create_JsonObject(JsonSerializerOptions options)
  {
    JsonTypeInfo<System.Text.Json.Nodes.JsonObject> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<System.Text.Json.Nodes.JsonObject>(options, out jsonTypeInfo))
      jsonTypeInfo = JsonMetadataServices.CreateValueInfo<System.Text.Json.Nodes.JsonObject>(options, (JsonConverter) JsonMetadataServices.JsonObjectConverter);
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<int> Int32
  {
    get
    {
      return this._Int32 ?? (this._Int32 = (JsonTypeInfo<int>) this.Options.GetTypeInfo(typeof (int)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<int> Create_Int32(JsonSerializerOptions options)
  {
    JsonTypeInfo<int> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<int>(options, out jsonTypeInfo))
      jsonTypeInfo = JsonMetadataServices.CreateValueInfo<int>(options, (JsonConverter) JsonMetadataServices.Int32Converter);
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<int?> NullableInt32
  {
    get
    {
      return this._NullableInt32 ?? (this._NullableInt32 = (JsonTypeInfo<int?>) this.Options.GetTypeInfo(typeof (int?)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<int?> Create_NullableInt32(JsonSerializerOptions options)
  {
    JsonTypeInfo<int?> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<int?>(options, out jsonTypeInfo))
    {
      JsonConverter nullableConverter = (JsonConverter) JsonMetadataServices.GetNullableConverter<int>(options);
      jsonTypeInfo = JsonMetadataServices.CreateValueInfo<int?>(options, nullableConverter);
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<int[]> Int32Array
  {
    get
    {
      return this._Int32Array ?? (this._Int32Array = (JsonTypeInfo<int[]>) this.Options.GetTypeInfo(typeof (int[])));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<int[]> Create_Int32Array(JsonSerializerOptions options)
  {
    JsonTypeInfo<int[]> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<int[]>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<int[]> collectionInfoValues = new JsonCollectionInfoValues<int[]>()
      {
        ObjectCreator = (Func<int[]>) null,
        SerializeHandler = (Action<Utf8JsonWriter, int[]>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateArrayInfo<int>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<long> Int64
  {
    get
    {
      return this._Int64 ?? (this._Int64 = (JsonTypeInfo<long>) this.Options.GetTypeInfo(typeof (long)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<long> Create_Int64(JsonSerializerOptions options)
  {
    JsonTypeInfo<long> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<long>(options, out jsonTypeInfo))
      jsonTypeInfo = JsonMetadataServices.CreateValueInfo<long>(options, (JsonConverter) JsonMetadataServices.Int64Converter);
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<object> Object
  {
    get
    {
      return this._Object ?? (this._Object = (JsonTypeInfo<object>) this.Options.GetTypeInfo(typeof (object)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<object> Create_Object(JsonSerializerOptions options)
  {
    JsonTypeInfo<object> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<object>(options, out jsonTypeInfo))
      jsonTypeInfo = JsonMetadataServices.CreateValueInfo<object>(options, (JsonConverter) JsonMetadataServices.ObjectConverter);
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<string> String
  {
    get
    {
      return this._String ?? (this._String = (JsonTypeInfo<string>) this.Options.GetTypeInfo(typeof (string)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<string> Create_String(JsonSerializerOptions options)
  {
    JsonTypeInfo<string> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<string>(options, out jsonTypeInfo))
      jsonTypeInfo = JsonMetadataServices.CreateValueInfo<string>(options, (JsonConverter) JsonMetadataServices.StringConverter);
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<ulong> UInt64
  {
    get
    {
      return this._UInt64 ?? (this._UInt64 = (JsonTypeInfo<ulong>) this.Options.GetTypeInfo(typeof (ulong)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<ulong> Create_UInt64(JsonSerializerOptions options)
  {
    JsonTypeInfo<ulong> jsonTypeInfo;
    if (!MegaCritSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<ulong>(options, out jsonTypeInfo))
      jsonTypeInfo = JsonMetadataServices.CreateValueInfo<ulong>(options, (JsonConverter) JsonMetadataServices.UInt64Converter);
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public static MegaCritSerializerContext Default { get; }

  protected override JsonSerializerOptions? GeneratedSerializerOptions { get; } = MegaCritSerializerContext.s_defaultOptions;

  public MegaCritSerializerContext()
    : base((JsonSerializerOptions) null)
  {
  }

  public MegaCritSerializerContext(JsonSerializerOptions options)
    : base(options)
  {
  }

  private static bool TryGetTypeInfoForRuntimeCustomConverter<TJsonMetadataType>(
    JsonSerializerOptions options,
    out JsonTypeInfo<TJsonMetadataType> jsonTypeInfo)
  {
    JsonConverter converterForType = MegaCritSerializerContext.GetRuntimeConverterForType(typeof (TJsonMetadataType), options);
    if (converterForType != null)
    {
      jsonTypeInfo = JsonMetadataServices.CreateValueInfo<TJsonMetadataType>(options, converterForType);
      return true;
    }
    jsonTypeInfo = (JsonTypeInfo<TJsonMetadataType>) null;
    return false;
  }

  private static JsonConverter? GetRuntimeConverterForType(Type type, JsonSerializerOptions options)
  {
    for (int index = 0; index < options.Converters.Count; ++index)
    {
      JsonConverter converter = options.Converters[index];
      if (converter != null && converter.CanConvert(type))
        return MegaCritSerializerContext.ExpandConverter(type, converter, options, false);
    }
    return (JsonConverter) null;
  }

  private static JsonConverter ExpandConverter(
    Type type,
    JsonConverter converter,
    JsonSerializerOptions options,
    bool validateCanConvert = true)
  {
    if (validateCanConvert && !converter.CanConvert(type))
      throw new InvalidOperationException($"The converter '{converter.GetType()}' is not compatible with the type '{type}'.");
    if (converter is JsonConverterFactory converterFactory)
    {
      converter = converterFactory.CreateConverter(type, options);
      if (converter == null || converter is JsonConverterFactory)
        throw new InvalidOperationException($"The converter '{converterFactory.GetType()}' cannot return null or a JsonConverterFactory instance.");
    }
    return converter;
  }

  public override JsonTypeInfo? GetTypeInfo(Type type)
  {
    JsonTypeInfo typeInfo;
    this.Options.TryGetTypeInfo(type, ref typeInfo);
    return typeInfo;
  }

  JsonTypeInfo? IJsonTypeInfoResolver.global\u003A\u003ASystem\u002EText\u002EJson\u002ESerialization\u002EMetadata\u002EIJsonTypeInfoResolver\u002EGetTypeInfo(
    Type type,
    JsonSerializerOptions options)
  {
    if (type == typeof (bool))
      return (JsonTypeInfo) this.Create_Boolean(options);
    if (type == typeof (double))
      return (JsonTypeInfo) this.Create_Double(options);
    if (type == typeof (float))
      return (JsonTypeInfo) this.Create_Single(options);
    if (type == typeof (Godot.Vector2))
      return (JsonTypeInfo) this.Create_Vector2(options);
    if (type == typeof (Godot.Vector2I))
      return (JsonTypeInfo) this.Create_Vector2I(options);
    if (type == typeof (MegaCrit.Sts2.Core.ControllerInput.ControllerMappingType))
      return (JsonTypeInfo) this.Create_ControllerMappingType(options);
    if (type == typeof (MegaCrit.Sts2.Core.Entities.Relics.RelicRarity))
      return (JsonTypeInfo) this.Create_RelicRarity(options);
    if (type == typeof (MegaCrit.Sts2.Core.Entities.Rngs.PlayerRngType))
      return (JsonTypeInfo) this.Create_PlayerRngType(options);
    if (type == typeof (MegaCrit.Sts2.Core.Entities.Rngs.RunRngType))
      return (JsonTypeInfo) this.Create_RunRngType(options);
    if (type == typeof (MegaCrit.Sts2.Core.Localization.LocString))
      return (JsonTypeInfo) this.Create_LocString(options);
    if (type == typeof (MegaCrit.Sts2.Core.Map.MapCoord))
      return (JsonTypeInfo) this.Create_MapCoord(options);
    if (type == typeof (MegaCrit.Sts2.Core.Map.MapPointType))
      return (JsonTypeInfo) this.Create_MapPointType(options);
    if (type == typeof (MegaCrit.Sts2.Core.Modding.ModDependency))
      return (JsonTypeInfo) this.Create_ModDependency(options);
    if (type == typeof (MegaCrit.Sts2.Core.Modding.ModManifest))
      return (JsonTypeInfo) this.Create_ModManifest(options);
    if (type == typeof (MegaCrit.Sts2.Core.Modding.ModSettings))
      return (JsonTypeInfo) this.Create_ModSettings(options);
    if (type == typeof (MegaCrit.Sts2.Core.Modding.ModSource))
      return (JsonTypeInfo) this.Create_ModSource(options);
    if (type == typeof (MegaCrit.Sts2.Core.Modding.SettingsSaveMod))
      return (JsonTypeInfo) this.Create_SettingsSaveMod(options);
    if (type == typeof (MegaCrit.Sts2.Core.Models.Badges.BadgeRarity))
      return (JsonTypeInfo) this.Create_BadgeRarity(options);
    if (type == typeof (MegaCrit.Sts2.Core.Models.ModelId))
      return (JsonTypeInfo) this.Create_ModelId(options);
    if (type == typeof (MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.FeedbackData))
      return (JsonTypeInfo) this.Create_FeedbackData(options);
    if (type == typeof (MegaCrit.Sts2.Core.Platform.Null.NullLeaderboard))
      return (JsonTypeInfo) this.Create_NullLeaderboard(options);
    if (type == typeof (MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFile))
      return (JsonTypeInfo) this.Create_NullLeaderboardFile(options);
    if (type == typeof (MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry))
      return (JsonTypeInfo) this.Create_NullLeaderboardFileEntry(options);
    if (type == typeof (MegaCrit.Sts2.Core.Platform.Null.NullMultiplayerName))
      return (JsonTypeInfo) this.Create_NullMultiplayerName(options);
    if (type == typeof (MegaCrit.Sts2.Core.Platform.PlatformType))
      return (JsonTypeInfo) this.Create_PlatformType(options);
    if (type == typeof (MegaCrit.Sts2.Core.Rewards.RewardType))
      return (JsonTypeInfo) this.Create_RewardType(options);
    if (type == typeof (MegaCrit.Sts2.Core.Rooms.RoomType))
      return (JsonTypeInfo) this.Create_RoomType(options);
    if (type == typeof (MegaCrit.Sts2.Core.Runs.CardCreationSource))
      return (JsonTypeInfo) this.Create_CardCreationSource(options);
    if (type == typeof (MegaCrit.Sts2.Core.Runs.CardRarityOddsType))
      return (JsonTypeInfo) this.Create_CardRarityOddsType(options);
    if (type == typeof (MegaCrit.Sts2.Core.Runs.GameMode))
      return (JsonTypeInfo) this.Create_GameMode(options);
    if (type == typeof (MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry))
      return (JsonTypeInfo) this.Create_AncientChoiceHistoryEntry(options);
    if (type == typeof (MegaCrit.Sts2.Core.Runs.History.CardChoiceHistoryEntry))
      return (JsonTypeInfo) this.Create_CardChoiceHistoryEntry(options);
    if (type == typeof (MegaCrit.Sts2.Core.Runs.History.CardEnchantmentHistoryEntry))
      return (JsonTypeInfo) this.Create_CardEnchantmentHistoryEntry(options);
    if (type == typeof (MegaCrit.Sts2.Core.Runs.History.CardTransformationHistoryEntry))
      return (JsonTypeInfo) this.Create_CardTransformationHistoryEntry(options);
    if (type == typeof (MegaCrit.Sts2.Core.Runs.History.EventOptionHistoryEntry))
      return (JsonTypeInfo) this.Create_EventOptionHistoryEntry(options);
    if (type == typeof (MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry))
      return (JsonTypeInfo) this.Create_MapPointHistoryEntry(options);
    if (type == typeof (MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry))
      return (JsonTypeInfo) this.Create_MapPointRoomHistoryEntry(options);
    if (type == typeof (MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry))
      return (JsonTypeInfo) this.Create_ModelChoiceHistoryEntry(options);
    if (type == typeof (MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry))
      return (JsonTypeInfo) this.Create_PlayerMapPointHistoryEntry(options);
    if (type == typeof (MegaCrit.Sts2.Core.Runs.RunHistory))
      return (JsonTypeInfo) this.Create_RunHistory(options);
    if (type == typeof (MegaCrit.Sts2.Core.Runs.RunHistoryPlayer))
      return (JsonTypeInfo) this.Create_RunHistoryPlayer(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.AncientCharacterStats))
      return (JsonTypeInfo) this.Create_AncientCharacterStats(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.AncientStats))
      return (JsonTypeInfo) this.Create_AncientStats(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.BadgeStats))
      return (JsonTypeInfo) this.Create_BadgeStats(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.CardStats))
      return (JsonTypeInfo) this.Create_CardStats(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.CharacterStats))
      return (JsonTypeInfo) this.Create_CharacterStats(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.EncounterStats))
      return (JsonTypeInfo) this.Create_EncounterStats(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.EnemyStats))
      return (JsonTypeInfo) this.Create_EnemyStats(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.EpochState))
      return (JsonTypeInfo) this.Create_EpochState(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.FightStats))
      return (JsonTypeInfo) this.Create_FightStats(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawingLine))
      return (JsonTypeInfo) this.Create_SerializableMapDrawingLine(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawings))
      return (JsonTypeInfo) this.Create_SerializableMapDrawings(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.MapDrawing.SerializablePlayerMapDrawings))
      return (JsonTypeInfo) this.Create_SerializablePlayerMapDrawings(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.Migrations.MigratingData))
      return (JsonTypeInfo) this.Create_MigratingData(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.PrefsSave))
      return (JsonTypeInfo) this.Create_PrefsSave(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.ProfileSave))
      return (JsonTypeInfo) this.Create_ProfileSave(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties))
      return (JsonTypeInfo) this.Create_SavedProperties(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<bool>))
      return (JsonTypeInfo) this.Create_SavedPropertyBoolean(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Models.ModelId>))
      return (JsonTypeInfo) this.Create_SavedPropertyModelId(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>))
      return (JsonTypeInfo) this.Create_SavedPropertySerializableCardArray(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>))
      return (JsonTypeInfo) this.Create_SavedPropertySerializableCard(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int[]>))
      return (JsonTypeInfo) this.Create_SavedPropertyInt32Array(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int>))
      return (JsonTypeInfo) this.Create_SavedPropertyInt32(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<string>))
      return (JsonTypeInfo) this.Create_SavedPropertyString(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableActMap))
      return (JsonTypeInfo) this.Create_SerializableActMap(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel))
      return (JsonTypeInfo) this.Create_SerializableActModel(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableBadge))
      return (JsonTypeInfo) this.Create_SerializableBadge(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableCard))
      return (JsonTypeInfo) this.Create_SerializableCard(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]))
      return (JsonTypeInfo) this.Create_SerializableCardArray(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableEnchantment))
      return (JsonTypeInfo) this.Create_SerializableEnchantment(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableExtraRunFields))
      return (JsonTypeInfo) this.Create_SerializableExtraRunFields(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint))
      return (JsonTypeInfo) this.Create_SerializableMapPoint(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier))
      return (JsonTypeInfo) this.Create_SerializableModifier(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer))
      return (JsonTypeInfo) this.Create_SerializablePlayer(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayerOddsSet))
      return (JsonTypeInfo) this.Create_SerializablePlayerOddsSet(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion))
      return (JsonTypeInfo) this.Create_SerializablePotion(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic))
      return (JsonTypeInfo) this.Create_SerializableRelic(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRelicGrabBag))
      return (JsonTypeInfo) this.Create_SerializableRelicGrabBag(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableReward))
      return (JsonTypeInfo) this.Create_SerializableReward(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRoom))
      return (JsonTypeInfo) this.Create_SerializableRoom(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRoomSet))
      return (JsonTypeInfo) this.Create_SerializableRoomSet(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRunOddsSet))
      return (JsonTypeInfo) this.Create_SerializableRunOddsSet(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.Runs.SerializableRunRngSet))
      return (JsonTypeInfo) this.Create_SerializableRunRngSet(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.SerializableEpoch))
      return (JsonTypeInfo) this.Create_SerializableEpoch(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.SerializableExtraPlayerFields))
      return (JsonTypeInfo) this.Create_SerializableExtraPlayerFields(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.SerializablePlayerRngSet))
      return (JsonTypeInfo) this.Create_SerializablePlayerRngSet(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.SerializableProgress))
      return (JsonTypeInfo) this.Create_SerializableProgress(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.SerializableRng))
      return (JsonTypeInfo) this.Create_SerializableRng(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.SerializableRun))
      return (JsonTypeInfo) this.Create_SerializableRun(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.SerializableUnlockedAchievement))
      return (JsonTypeInfo) this.Create_SerializableUnlockedAchievement(options);
    if (type == typeof (MegaCrit.Sts2.Core.Saves.SettingsSave))
      return (JsonTypeInfo) this.Create_SettingsSave(options);
    if (type == typeof (MegaCrit.Sts2.Core.Settings.AspectRatioSetting))
      return (JsonTypeInfo) this.Create_AspectRatioSetting(options);
    if (type == typeof (MegaCrit.Sts2.Core.Settings.FastModeType))
      return (JsonTypeInfo) this.Create_FastModeType(options);
    if (type == typeof (MegaCrit.Sts2.Core.Settings.VSyncType))
      return (JsonTypeInfo) this.Create_VSyncType(options);
    if (type == typeof (MegaCrit.Sts2.Core.Unlocks.SerializableUnlockState))
      return (JsonTypeInfo) this.Create_SerializableUnlockState(options);
    if (type == typeof (Dictionary<MegaCrit.Sts2.Core.Entities.Relics.RelicRarity, List<MegaCrit.Sts2.Core.Models.ModelId>>))
      return (JsonTypeInfo) this.Create_DictionaryRelicRarityListModelId(options);
    if (type == typeof (Dictionary<MegaCrit.Sts2.Core.Entities.Rngs.PlayerRngType, MegaCrit.Sts2.Core.Saves.SerializableRng>))
      return (JsonTypeInfo) this.Create_DictionaryPlayerRngTypeSerializableRng(options);
    if (type == typeof (Dictionary<MegaCrit.Sts2.Core.Entities.Rngs.RunRngType, MegaCrit.Sts2.Core.Saves.SerializableRng>))
      return (JsonTypeInfo) this.Create_DictionaryRunRngTypeSerializableRng(options);
    if (type == typeof (Dictionary<string, object>))
      return (JsonTypeInfo) this.Create_DictionaryStringObject(options);
    if (type == typeof (Dictionary<string, string>))
      return (JsonTypeInfo) this.Create_DictionaryStringString(options);
    if (type == typeof (Dictionary<ulong, List<MegaCrit.Sts2.Core.Saves.Runs.SerializableReward>>))
      return (JsonTypeInfo) this.Create_DictionaryUInt64ListSerializableReward(options);
    if (type == typeof (IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableBadge>))
      return (JsonTypeInfo) this.Create_IEnumerableSerializableBadge(options);
    if (type == typeof (IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>))
      return (JsonTypeInfo) this.Create_IEnumerableSerializableCard(options);
    if (type == typeof (IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>))
      return (JsonTypeInfo) this.Create_IEnumerableSerializablePotion(options);
    if (type == typeof (IEnumerable<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>))
      return (JsonTypeInfo) this.Create_IEnumerableSerializableRelic(options);
    if (type == typeof (List<Godot.Vector2>))
      return (JsonTypeInfo) this.Create_ListVector2(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Map.MapCoord>))
      return (JsonTypeInfo) this.Create_ListMapCoord(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Modding.ModDependency>))
      return (JsonTypeInfo) this.Create_ListModDependency(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Modding.SettingsSaveMod>))
      return (JsonTypeInfo) this.Create_ListSettingsSaveMod(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Models.ModelId>))
      return (JsonTypeInfo) this.Create_ListModelId(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboard>))
      return (JsonTypeInfo) this.Create_ListNullLeaderboard(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Platform.Null.NullLeaderboardFileEntry>))
      return (JsonTypeInfo) this.Create_ListNullLeaderboardFileEntry(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Platform.Null.NullMultiplayerName>))
      return (JsonTypeInfo) this.Create_ListNullMultiplayerName(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Runs.History.AncientChoiceHistoryEntry>))
      return (JsonTypeInfo) this.Create_ListAncientChoiceHistoryEntry(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Runs.History.CardChoiceHistoryEntry>))
      return (JsonTypeInfo) this.Create_ListCardChoiceHistoryEntry(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Runs.History.CardEnchantmentHistoryEntry>))
      return (JsonTypeInfo) this.Create_ListCardEnchantmentHistoryEntry(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Runs.History.CardTransformationHistoryEntry>))
      return (JsonTypeInfo) this.Create_ListCardTransformationHistoryEntry(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Runs.History.EventOptionHistoryEntry>))
      return (JsonTypeInfo) this.Create_ListEventOptionHistoryEntry(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>))
      return (JsonTypeInfo) this.Create_ListMapPointHistoryEntry(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Runs.History.MapPointRoomHistoryEntry>))
      return (JsonTypeInfo) this.Create_ListMapPointRoomHistoryEntry(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Runs.History.ModelChoiceHistoryEntry>))
      return (JsonTypeInfo) this.Create_ListModelChoiceHistoryEntry(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry>))
      return (JsonTypeInfo) this.Create_ListPlayerMapPointHistoryEntry(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Runs.RunHistoryPlayer>))
      return (JsonTypeInfo) this.Create_ListRunHistoryPlayer(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Saves.AncientCharacterStats>))
      return (JsonTypeInfo) this.Create_ListAncientCharacterStats(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Saves.AncientStats>))
      return (JsonTypeInfo) this.Create_ListAncientStats(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Saves.BadgeStats>))
      return (JsonTypeInfo) this.Create_ListBadgeStats(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Saves.CardStats>))
      return (JsonTypeInfo) this.Create_ListCardStats(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Saves.CharacterStats>))
      return (JsonTypeInfo) this.Create_ListCharacterStats(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Saves.EncounterStats>))
      return (JsonTypeInfo) this.Create_ListEncounterStats(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Saves.EnemyStats>))
      return (JsonTypeInfo) this.Create_ListEnemyStats(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Saves.FightStats>))
      return (JsonTypeInfo) this.Create_ListFightStats(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawingLine>))
      return (JsonTypeInfo) this.Create_ListSerializableMapDrawingLine(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Saves.MapDrawing.SerializablePlayerMapDrawings>))
      return (JsonTypeInfo) this.Create_ListSerializablePlayerMapDrawings(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Saves.Migrations.MigratingData>))
      return (JsonTypeInfo) this.Create_ListMigratingData(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<bool>>))
      return (JsonTypeInfo) this.Create_ListSavedPropertyBoolean(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Models.ModelId>>))
      return (JsonTypeInfo) this.Create_ListSavedPropertyModelId(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard[]>>))
      return (JsonTypeInfo) this.Create_ListSavedPropertySerializableCardArray(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>>))
      return (JsonTypeInfo) this.Create_ListSavedPropertySerializableCard(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int[]>>))
      return (JsonTypeInfo) this.Create_ListSavedPropertyInt32Array(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<int>>))
      return (JsonTypeInfo) this.Create_ListSavedPropertyInt32(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Saves.Runs.SavedProperties.SavedProperty<string>>))
      return (JsonTypeInfo) this.Create_ListSavedPropertyString(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel>))
      return (JsonTypeInfo) this.Create_ListSerializableActModel(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Saves.Runs.SerializableCard>))
      return (JsonTypeInfo) this.Create_ListSerializableCard(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Saves.Runs.SerializableMapPoint>))
      return (JsonTypeInfo) this.Create_ListSerializableMapPoint(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier>))
      return (JsonTypeInfo) this.Create_ListSerializableModifier(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer>))
      return (JsonTypeInfo) this.Create_ListSerializablePlayer(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Saves.Runs.SerializablePotion>))
      return (JsonTypeInfo) this.Create_ListSerializablePotion(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic>))
      return (JsonTypeInfo) this.Create_ListSerializableRelic(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Saves.Runs.SerializableReward>))
      return (JsonTypeInfo) this.Create_ListSerializableReward(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Saves.SerializableEpoch>))
      return (JsonTypeInfo) this.Create_ListSerializableEpoch(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Saves.SerializableUnlockedAchievement>))
      return (JsonTypeInfo) this.Create_ListSerializableUnlockedAchievement(options);
    if (type == typeof (List<Dictionary<string, object>>))
      return (JsonTypeInfo) this.Create_ListDictionaryStringObject(options);
    if (type == typeof (List<List<MegaCrit.Sts2.Core.Runs.History.MapPointHistoryEntry>>))
      return (JsonTypeInfo) this.Create_ListListMapPointHistoryEntry(options);
    if (type == typeof (List<List<MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry>>))
      return (JsonTypeInfo) this.Create_ListListPlayerMapPointHistoryEntry(options);
    if (type == typeof (List<System.Text.Json.Nodes.JsonNode>))
      return (JsonTypeInfo) this.Create_ListJsonNode(options);
    if (type == typeof (List<string>))
      return (JsonTypeInfo) this.Create_ListString(options);
    if (type == typeof (List<ulong>))
      return (JsonTypeInfo) this.Create_ListUInt64(options);
    if (type == typeof (System.DateTimeOffset))
      return (JsonTypeInfo) this.Create_DateTimeOffset(options);
    if (type == typeof (System.DateTimeOffset?))
      return (JsonTypeInfo) this.Create_NullableDateTimeOffset(options);
    if (type == typeof (System.Text.Json.Nodes.JsonNode))
      return (JsonTypeInfo) this.Create_JsonNode(options);
    if (type == typeof (System.Text.Json.Nodes.JsonObject))
      return (JsonTypeInfo) this.Create_JsonObject(options);
    if (type == typeof (int))
      return (JsonTypeInfo) this.Create_Int32(options);
    if (type == typeof (int?))
      return (JsonTypeInfo) this.Create_NullableInt32(options);
    if (type == typeof (int[]))
      return (JsonTypeInfo) this.Create_Int32Array(options);
    if (type == typeof (long))
      return (JsonTypeInfo) this.Create_Int64(options);
    if (type == typeof (object))
      return (JsonTypeInfo) this.Create_Object(options);
    if (type == typeof (string))
      return (JsonTypeInfo) this.Create_String(options);
    return type == typeof (ulong) ? (JsonTypeInfo) this.Create_UInt64(options) : (JsonTypeInfo) null;
  }

  static MegaCritSerializerContext()
  {
    JsonSerializerOptions serializerOptions = new JsonSerializerOptions();
    serializerOptions.Converters.Add((JsonConverter) new ModelIdRunSaveConverter());
    serializerOptions.Converters.Add((JsonConverter) new SnakeCaseJsonStringEnumConverter<Achievement>());
    serializerOptions.Converters.Add((JsonConverter) new SnakeCaseJsonStringEnumConverter<MegaCrit.Sts2.Core.Settings.AspectRatioSetting>());
    serializerOptions.Converters.Add((JsonConverter) new SnakeCaseJsonStringEnumConverter<MegaCrit.Sts2.Core.Settings.VSyncType>());
    serializerOptions.Converters.Add((JsonConverter) new SnakeCaseJsonStringEnumConverter<MegaCrit.Sts2.Core.Runs.GameMode>());
    serializerOptions.Converters.Add((JsonConverter) new SnakeCaseJsonStringEnumConverter<MegaCrit.Sts2.Core.Entities.Relics.RelicRarity>());
    serializerOptions.Converters.Add((JsonConverter) new SnakeCaseJsonStringEnumConverter<MegaCrit.Sts2.Core.Entities.Rngs.RunRngType>());
    serializerOptions.Converters.Add((JsonConverter) new SnakeCaseJsonStringEnumConverter<MegaCrit.Sts2.Core.Entities.Rngs.PlayerRngType>());
    serializerOptions.Converters.Add((JsonConverter) new SnakeCaseJsonStringEnumConverter<MegaCrit.Sts2.Core.Map.MapPointType>());
    serializerOptions.Converters.Add((JsonConverter) new SnakeCaseJsonStringEnumConverter<MegaCrit.Sts2.Core.Modding.ModSource>());
    serializerOptions.Converters.Add((JsonConverter) new SnakeCaseJsonStringEnumConverter<MegaCrit.Sts2.Core.Platform.PlatformType>());
    serializerOptions.Converters.Add((JsonConverter) new SnakeCaseJsonStringEnumConverter<MegaCrit.Sts2.Core.Rewards.RewardType>());
    serializerOptions.Converters.Add((JsonConverter) new SnakeCaseJsonStringEnumConverter<MegaCrit.Sts2.Core.Rooms.RoomType>());
    serializerOptions.Converters.Add((JsonConverter) new SnakeCaseJsonStringEnumConverter<MegaCrit.Sts2.Core.Saves.EpochState>());
    serializerOptions.Converters.Add((JsonConverter) new SnakeCaseJsonStringEnumConverter<MegaCrit.Sts2.Core.Settings.FastModeType>());
    serializerOptions.Converters.Add((JsonConverter) new SnakeCaseJsonStringEnumConverter<MegaCrit.Sts2.Core.Runs.CardCreationSource>());
    serializerOptions.Converters.Add((JsonConverter) new SnakeCaseJsonStringEnumConverter<MegaCrit.Sts2.Core.Runs.CardRarityOddsType>());
    serializerOptions.Converters.Add((JsonConverter) new SnakeCaseJsonStringEnumConverter<MegaCrit.Sts2.Core.ControllerInput.ControllerMappingType>());
    serializerOptions.Converters.Add((JsonConverter) new SnakeCaseJsonStringEnumConverter<MegaCrit.Sts2.Core.Models.Badges.BadgeRarity>());
    serializerOptions.IncludeFields = true;
    serializerOptions.ReadCommentHandling = (JsonCommentHandling) 1;
    serializerOptions.UnmappedMemberHandling = (JsonUnmappedMemberHandling) 0;
    serializerOptions.WriteIndented = true;
    MegaCritSerializerContext.s_defaultOptions = serializerOptions;
    MegaCritSerializerContext.Default = new MegaCritSerializerContext(new JsonSerializerOptions(MegaCritSerializerContext.s_defaultOptions));
  }
}
