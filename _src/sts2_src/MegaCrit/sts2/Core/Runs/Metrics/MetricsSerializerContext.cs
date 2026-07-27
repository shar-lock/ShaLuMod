// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Runs.Metrics.MetricsSerializerContext
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.GameInfo;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

#nullable enable
namespace MegaCrit.Sts2.Core.Runs.Metrics;

[JsonSourceGenerationOptions]
[JsonSerializable(typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics))]
[JsonSerializable(typeof (MegaCrit.Sts2.Core.Achievements.AchievementMetric))]
[JsonSerializable(typeof (MegaCrit.Sts2.Core.Timeline.EpochMetric))]
[JsonSerializable(typeof (MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric))]
[GeneratedCode("System.Text.Json.SourceGeneration", "9.0.12.31616")]
internal class MetricsSerializerContext : JsonSerializerContext, IJsonTypeInfoResolver
{
  private JsonTypeInfo<bool>? _Boolean;
  private JsonTypeInfo<float>? _Single;
  private JsonTypeInfo<Godot.Vector2I>? _Vector2I;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Achievements.AchievementMetric>? _AchievementMetric;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Models.ModelId>? _ModelId;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Runs.Metrics.ActWinMetric>? _ActWinMetric;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Runs.Metrics.AncientMetric>? _AncientMetric;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Runs.Metrics.CardChoiceMetric>? _CardChoiceMetric;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric>? _EncounterMetric;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric>? _EventChoiceMetric;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics>? _RunMetrics;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric>? _SettingsDataMetric;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Settings.AspectRatioSetting>? _AspectRatioSetting;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Settings.FastModeType>? _FastModeType;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Settings.VSyncType>? _VSyncType;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Timeline.EpochMetric>? _EpochMetric;
  private JsonTypeInfo<IEnumerable<MegaCrit.Sts2.Core.Models.ModelId>>? _IEnumerableModelId;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Models.ModelId>>? _ListModelId;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.Metrics.ActWinMetric>>? _ListActWinMetric;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.Metrics.AncientMetric>>? _ListAncientMetric;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.Metrics.CardChoiceMetric>>? _ListCardChoiceMetric;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric>>? _ListEncounterMetric;
  private JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric>>? _ListEventChoiceMetric;
  private JsonTypeInfo<List<string>>? _ListString;
  private JsonTypeInfo<int>? _Int32;
  private JsonTypeInfo<long>? _Int64;
  private JsonTypeInfo<string>? _String;
  private static readonly JsonSerializerOptions s_defaultOptions;
  private const BindingFlags InstanceMemberBindingFlags = (BindingFlags) 52;
  private static readonly JsonEncodedText PropName_x;
  private static readonly JsonEncodedText PropName_y;
  private static readonly JsonEncodedText PropName_buildId;
  private static readonly JsonEncodedText PropName_achievement;
  private static readonly JsonEncodedText PropName_totalAchievements;
  private static readonly JsonEncodedText PropName_totalPlaytime;
  private static readonly JsonEncodedText PropName_totalRuns;
  private static readonly JsonEncodedText PropName_category;
  private static readonly JsonEncodedText PropName_entry;
  private static readonly JsonEncodedText PropName_act;
  private static readonly JsonEncodedText PropName_win;
  private static readonly JsonEncodedText PropName_picked;
  private static readonly JsonEncodedText PropName_skipped;
  private static readonly JsonEncodedText PropName_id;
  private static readonly JsonEncodedText PropName_damage;
  private static readonly JsonEncodedText PropName_turns;
  private static readonly JsonEncodedText PropName_playerId;
  private static readonly JsonEncodedText PropName_character;
  private static readonly JsonEncodedText PropName_numPlayers;
  private static readonly JsonEncodedText PropName_team;
  private static readonly JsonEncodedText PropName_buildType;
  private static readonly JsonEncodedText PropName_ascension;
  private static readonly JsonEncodedText PropName_totalWinRate;
  private static readonly JsonEncodedText PropName_numReloads;
  private static readonly JsonEncodedText PropName_runPlaytime;
  private static readonly JsonEncodedText PropName_floorReached;
  private static readonly JsonEncodedText PropName_killedByEncounter;
  private static readonly JsonEncodedText PropName_cardChoices;
  private static readonly JsonEncodedText PropName_campfireUpgrades;
  private static readonly JsonEncodedText PropName_eventChoices;
  private static readonly JsonEncodedText PropName_ancientChoices;
  private static readonly JsonEncodedText PropName_relicBuys;
  private static readonly JsonEncodedText PropName_potionBuys;
  private static readonly JsonEncodedText PropName_colorlessBuys;
  private static readonly JsonEncodedText PropName_potionDiscards;
  private static readonly JsonEncodedText PropName_encounters;
  private static readonly JsonEncodedText PropName_actWins;
  private static readonly JsonEncodedText PropName_deck;
  private static readonly JsonEncodedText PropName_relics;
  private static readonly JsonEncodedText PropName_os;
  private static readonly JsonEncodedText PropName_platform;
  private static readonly JsonEncodedText PropName_systemRam;
  private static readonly JsonEncodedText PropName_language;
  private static readonly JsonEncodedText PropName_combatSpeed;
  private static readonly JsonEncodedText PropName_screenshake;
  private static readonly JsonEncodedText PropName_runTimer;
  private static readonly JsonEncodedText PropName_phobiaMode;
  private static readonly JsonEncodedText PropName_cardIndices;
  private static readonly JsonEncodedText PropName_displayCount;
  private static readonly JsonEncodedText PropName_displayResolution;
  private static readonly JsonEncodedText PropName_fullscreen;
  private static readonly JsonEncodedText PropName_aspectRatio;
  private static readonly JsonEncodedText PropName_resizeWindows;
  private static readonly JsonEncodedText PropName_vSync;
  private static readonly JsonEncodedText PropName_fpsLimit;
  private static readonly JsonEncodedText PropName_msaa;
  private static readonly JsonEncodedText PropName_epoch;
  private static readonly JsonEncodedText PropName_totalEpochs;

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
    if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<bool>(options, out jsonTypeInfo))
      jsonTypeInfo = JsonMetadataServices.CreateValueInfo<bool>(options, (JsonConverter) JsonMetadataServices.BooleanConverter);
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
    if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<float>(options, out jsonTypeInfo))
      jsonTypeInfo = JsonMetadataServices.CreateValueInfo<float>(options, (JsonConverter) JsonMetadataServices.SingleConverter);
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
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
    if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<Godot.Vector2I>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<Godot.Vector2I> objectInfoValues = new JsonObjectInfoValues<Godot.Vector2I>()
      {
        ObjectCreator = (Func<Godot.Vector2I>) (() => new Godot.Vector2I()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], Godot.Vector2I>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MetricsSerializerContext.Vector2IPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (Godot.Vector2I).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = new Action<Utf8JsonWriter, Godot.Vector2I>(this.Vector2ISerializeHandler)
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

  private void Vector2ISerializeHandler(Utf8JsonWriter writer, Godot.Vector2I value)
  {
    writer.WriteStartObject();
    writer.WriteNumber(MetricsSerializerContext.PropName_x, value.X);
    writer.WriteNumber(MetricsSerializerContext.PropName_y, value.Y);
    writer.WriteEndObject();
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Achievements.AchievementMetric> AchievementMetric
  {
    get
    {
      return this._AchievementMetric ?? (this._AchievementMetric = (JsonTypeInfo<MegaCrit.Sts2.Core.Achievements.AchievementMetric>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Achievements.AchievementMetric)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Achievements.AchievementMetric> Create_AchievementMetric(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Achievements.AchievementMetric> jsonTypeInfo;
    if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Achievements.AchievementMetric>(options, out jsonTypeInfo))
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Achievements.AchievementMetric> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Achievements.AchievementMetric>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Achievements.AchievementMetric>) null,
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Achievements.AchievementMetric>) (args => new MegaCrit.Sts2.Core.Achievements.AchievementMetric()
        {
          BuildId = (string) args[0],
          Achievement = (string) args[1]
        }),
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MetricsSerializerContext.AchievementMetricPropInit(options)),
        ConstructorParameterMetadataInitializer = MetricsSerializerContext.\u003C\u003EO.\u003C0\u003E__AchievementMetricCtorParamInit ?? (MetricsSerializerContext.\u003C\u003EO.\u003C0\u003E__AchievementMetricCtorParamInit = new Func<JsonParameterInfoValues[]>(MetricsSerializerContext.AchievementMetricCtorParamInit)),
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Achievements.AchievementMetric).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = new Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Achievements.AchievementMetric>(this.AchievementMetricSerializeHandler)
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Achievements.AchievementMetric>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] AchievementMetricPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[5];
    JsonPropertyInfoValues<string> propertyInfoValues1 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Achievements.AchievementMetric),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Achievements.AchievementMetric) obj).BuildId),
      Setter = (Action<object, string>) ((obj, value) => ((MegaCrit.Sts2.Core.Achievements.AchievementMetric) obj).BuildId = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "BuildId",
      JsonPropertyName = "buildId",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Achievements.AchievementMetric).GetProperty("BuildId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues1);
    jsonPropertyInfoArray[0].IsRequired = true;
    jsonPropertyInfoArray[0].IsGetNullable = false;
    jsonPropertyInfoArray[0].IsSetNullable = false;
    JsonPropertyInfoValues<string> propertyInfoValues2 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Achievements.AchievementMetric),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Achievements.AchievementMetric) obj).Achievement),
      Setter = (Action<object, string>) ((obj, value) => ((MegaCrit.Sts2.Core.Achievements.AchievementMetric) obj).Achievement = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Achievement",
      JsonPropertyName = "achievement",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Achievements.AchievementMetric).GetProperty("Achievement", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues2);
    jsonPropertyInfoArray[1].IsRequired = true;
    jsonPropertyInfoArray[1].IsGetNullable = false;
    jsonPropertyInfoArray[1].IsSetNullable = false;
    JsonPropertyInfoValues<int> propertyInfoValues3 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Achievements.AchievementMetric),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Achievements.AchievementMetric) obj).TotalAchievements),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Achievements.AchievementMetric) obj).TotalAchievements = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "TotalAchievements",
      JsonPropertyName = "totalAchievements",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Achievements.AchievementMetric).GetProperty("TotalAchievements", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues3);
    JsonPropertyInfoValues<long> propertyInfoValues4 = new JsonPropertyInfoValues<long>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Achievements.AchievementMetric),
      Converter = (JsonConverter<long>) null,
      Getter = (Func<object, long>) (obj => ((MegaCrit.Sts2.Core.Achievements.AchievementMetric) obj).TotalPlaytime),
      Setter = (Action<object, long>) ((obj, value) => ((MegaCrit.Sts2.Core.Achievements.AchievementMetric) obj).TotalPlaytime = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "TotalPlaytime",
      JsonPropertyName = "totalPlaytime",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Achievements.AchievementMetric).GetProperty("TotalPlaytime", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (long), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[3] = JsonMetadataServices.CreatePropertyInfo<long>(options, propertyInfoValues4);
    JsonPropertyInfoValues<int> propertyInfoValues5 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Achievements.AchievementMetric),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Achievements.AchievementMetric) obj).TotalRuns),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Achievements.AchievementMetric) obj).TotalRuns = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "TotalRuns",
      JsonPropertyName = "totalRuns",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Achievements.AchievementMetric).GetProperty("TotalRuns", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[4] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues5);
    return jsonPropertyInfoArray;
  }

  private void AchievementMetricSerializeHandler(Utf8JsonWriter writer, MegaCrit.Sts2.Core.Achievements.AchievementMetric? value)
  {
    if (value == null)
    {
      writer.WriteNullValue();
    }
    else
    {
      writer.WriteStartObject();
      writer.WriteString(MetricsSerializerContext.PropName_buildId, value.BuildId);
      writer.WriteString(MetricsSerializerContext.PropName_achievement, value.Achievement);
      writer.WriteNumber(MetricsSerializerContext.PropName_totalAchievements, value.TotalAchievements);
      writer.WriteNumber(MetricsSerializerContext.PropName_totalPlaytime, value.TotalPlaytime);
      writer.WriteNumber(MetricsSerializerContext.PropName_totalRuns, value.TotalRuns);
      writer.WriteEndObject();
    }
  }

  private static JsonParameterInfoValues[] AchievementMetricCtorParamInit()
  {
    return new JsonParameterInfoValues[2]
    {
      new JsonParameterInfoValues()
      {
        Name = "BuildId",
        ParameterType = typeof (string),
        Position = 0,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Achievement",
        ParameterType = typeof (string),
        Position = 1,
        IsNullable = false,
        IsMemberInitializer = true
      }
    };
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
    if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Models.ModelId>(options, out jsonTypeInfo))
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Models.ModelId> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Models.ModelId>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Models.ModelId>) null,
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Models.ModelId>) (args => new MegaCrit.Sts2.Core.Models.ModelId((string) args[0], (string) args[1])),
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MetricsSerializerContext.ModelIdPropInit(options)),
        ConstructorParameterMetadataInitializer = MetricsSerializerContext.\u003C\u003EO.\u003C1\u003E__ModelIdCtorParamInit ?? (MetricsSerializerContext.\u003C\u003EO.\u003C1\u003E__ModelIdCtorParamInit = new Func<JsonParameterInfoValues[]>(MetricsSerializerContext.ModelIdCtorParamInit)),
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Models.ModelId).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, new Type[2]
        {
          typeof (string),
          typeof (string)
        }, (ParameterModifier[]) null)),
        SerializeHandler = new Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Models.ModelId>(this.ModelIdSerializeHandler)
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

  private void ModelIdSerializeHandler(Utf8JsonWriter writer, MegaCrit.Sts2.Core.Models.ModelId? value)
  {
    if ((object) value == null)
    {
      writer.WriteNullValue();
    }
    else
    {
      writer.WriteStartObject();
      writer.WriteString(MetricsSerializerContext.PropName_category, value.Category);
      writer.WriteString(MetricsSerializerContext.PropName_entry, value.Entry);
      writer.WriteEndObject();
    }
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
  JsonTypeInfo<MegaCrit.Sts2.Core.Runs.Metrics.ActWinMetric> ActWinMetric
  {
    get
    {
      return this._ActWinMetric ?? (this._ActWinMetric = (JsonTypeInfo<MegaCrit.Sts2.Core.Runs.Metrics.ActWinMetric>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Runs.Metrics.ActWinMetric)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Runs.Metrics.ActWinMetric> Create_ActWinMetric(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Runs.Metrics.ActWinMetric> jsonTypeInfo;
    if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Runs.Metrics.ActWinMetric>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Runs.Metrics.ActWinMetric> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Runs.Metrics.ActWinMetric>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Runs.Metrics.ActWinMetric>) (() => new MegaCrit.Sts2.Core.Runs.Metrics.ActWinMetric()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Runs.Metrics.ActWinMetric>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MetricsSerializerContext.ActWinMetricPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.ActWinMetric).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = new Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Runs.Metrics.ActWinMetric>(this.ActWinMetricSerializeHandler)
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Runs.Metrics.ActWinMetric>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] ActWinMetricPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[2];
    JsonPropertyInfoValues<string> propertyInfoValues1 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.ActWinMetric),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.ActWinMetric) obj).act),
      Setter = (Action<object, string>) null,
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "act",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.ActWinMetric).GetField("act", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues1);
    jsonPropertyInfoArray[0].IsGetNullable = false;
    jsonPropertyInfoArray[0].IsSetNullable = false;
    JsonPropertyInfoValues<bool> propertyInfoValues2 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.ActWinMetric),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.ActWinMetric) obj).win),
      Setter = (Action<object, bool>) null,
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "win",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.ActWinMetric).GetField("win", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues2);
    return jsonPropertyInfoArray;
  }

  private void ActWinMetricSerializeHandler(Utf8JsonWriter writer, MegaCrit.Sts2.Core.Runs.Metrics.ActWinMetric value)
  {
    writer.WriteStartObject();
    writer.WriteString(MetricsSerializerContext.PropName_act, value.act);
    writer.WriteBoolean(MetricsSerializerContext.PropName_win, value.win);
    writer.WriteEndObject();
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Runs.Metrics.AncientMetric> AncientMetric
  {
    get
    {
      return this._AncientMetric ?? (this._AncientMetric = (JsonTypeInfo<MegaCrit.Sts2.Core.Runs.Metrics.AncientMetric>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Runs.Metrics.AncientMetric)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Runs.Metrics.AncientMetric> Create_AncientMetric(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Runs.Metrics.AncientMetric> jsonTypeInfo;
    if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Runs.Metrics.AncientMetric>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Runs.Metrics.AncientMetric> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Runs.Metrics.AncientMetric>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Runs.Metrics.AncientMetric>) (() => new MegaCrit.Sts2.Core.Runs.Metrics.AncientMetric()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Runs.Metrics.AncientMetric>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MetricsSerializerContext.AncientMetricPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.AncientMetric).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = new Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Runs.Metrics.AncientMetric>(this.AncientMetricSerializeHandler)
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Runs.Metrics.AncientMetric>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] AncientMetricPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[2];
    JsonPropertyInfoValues<string> propertyInfoValues1 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.AncientMetric),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.AncientMetric) obj).picked),
      Setter = (Action<object, string>) null,
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "picked",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.AncientMetric).GetField("picked", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues1);
    jsonPropertyInfoArray[0].IsGetNullable = false;
    jsonPropertyInfoArray[0].IsSetNullable = false;
    JsonPropertyInfoValues<List<string>> propertyInfoValues2 = new JsonPropertyInfoValues<List<string>>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.AncientMetric),
      Converter = (JsonConverter<List<string>>) null,
      Getter = (Func<object, List<string>>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.AncientMetric) obj).skipped),
      Setter = (Action<object, List<string>>) null,
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "skipped",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.AncientMetric).GetField("skipped", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<List<string>>(options, propertyInfoValues2);
    jsonPropertyInfoArray[1].IsGetNullable = false;
    jsonPropertyInfoArray[1].IsSetNullable = false;
    return jsonPropertyInfoArray;
  }

  private void AncientMetricSerializeHandler(Utf8JsonWriter writer, MegaCrit.Sts2.Core.Runs.Metrics.AncientMetric value)
  {
    writer.WriteStartObject();
    writer.WriteString(MetricsSerializerContext.PropName_picked, value.picked);
    writer.WritePropertyName(MetricsSerializerContext.PropName_skipped);
    this.ListStringSerializeHandler(writer, value.skipped);
    writer.WriteEndObject();
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Runs.Metrics.CardChoiceMetric> CardChoiceMetric
  {
    get
    {
      return this._CardChoiceMetric ?? (this._CardChoiceMetric = (JsonTypeInfo<MegaCrit.Sts2.Core.Runs.Metrics.CardChoiceMetric>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Runs.Metrics.CardChoiceMetric)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Runs.Metrics.CardChoiceMetric> Create_CardChoiceMetric(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Runs.Metrics.CardChoiceMetric> jsonTypeInfo;
    if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Runs.Metrics.CardChoiceMetric>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Runs.Metrics.CardChoiceMetric> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Runs.Metrics.CardChoiceMetric>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Runs.Metrics.CardChoiceMetric>) (() => new MegaCrit.Sts2.Core.Runs.Metrics.CardChoiceMetric()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Runs.Metrics.CardChoiceMetric>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MetricsSerializerContext.CardChoiceMetricPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.CardChoiceMetric).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = new Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Runs.Metrics.CardChoiceMetric>(this.CardChoiceMetricSerializeHandler)
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Runs.Metrics.CardChoiceMetric>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] CardChoiceMetricPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[2];
    JsonPropertyInfoValues<List<string>> propertyInfoValues1 = new JsonPropertyInfoValues<List<string>>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.CardChoiceMetric),
      Converter = (JsonConverter<List<string>>) null,
      Getter = (Func<object, List<string>>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.CardChoiceMetric) obj).picked),
      Setter = (Action<object, List<string>>) null,
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "picked",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.CardChoiceMetric).GetField("picked", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<List<string>>(options, propertyInfoValues1);
    jsonPropertyInfoArray[0].IsGetNullable = false;
    jsonPropertyInfoArray[0].IsSetNullable = false;
    JsonPropertyInfoValues<List<string>> propertyInfoValues2 = new JsonPropertyInfoValues<List<string>>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.CardChoiceMetric),
      Converter = (JsonConverter<List<string>>) null,
      Getter = (Func<object, List<string>>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.CardChoiceMetric) obj).skipped),
      Setter = (Action<object, List<string>>) null,
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "skipped",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.CardChoiceMetric).GetField("skipped", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<List<string>>(options, propertyInfoValues2);
    jsonPropertyInfoArray[1].IsGetNullable = false;
    jsonPropertyInfoArray[1].IsSetNullable = false;
    return jsonPropertyInfoArray;
  }

  private void CardChoiceMetricSerializeHandler(Utf8JsonWriter writer, MegaCrit.Sts2.Core.Runs.Metrics.CardChoiceMetric value)
  {
    writer.WriteStartObject();
    writer.WritePropertyName(MetricsSerializerContext.PropName_picked);
    this.ListStringSerializeHandler(writer, value.picked);
    writer.WritePropertyName(MetricsSerializerContext.PropName_skipped);
    this.ListStringSerializeHandler(writer, value.skipped);
    writer.WriteEndObject();
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric> EncounterMetric
  {
    get
    {
      return this._EncounterMetric ?? (this._EncounterMetric = (JsonTypeInfo<MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric> Create_EncounterMetric(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric> jsonTypeInfo;
    if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric>) (() => new MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MetricsSerializerContext.EncounterMetricPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = new Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric>(this.EncounterMetricSerializeHandler)
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] EncounterMetricPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[3];
    JsonPropertyInfoValues<string> propertyInfoValues1 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric) obj).id),
      Setter = (Action<object, string>) null,
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "id",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric).GetField("id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues1);
    jsonPropertyInfoArray[0].IsGetNullable = false;
    jsonPropertyInfoArray[0].IsSetNullable = false;
    JsonPropertyInfoValues<int> propertyInfoValues2 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric) obj).damage),
      Setter = (Action<object, int>) null,
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "damage",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric).GetField("damage", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues2);
    JsonPropertyInfoValues<int> propertyInfoValues3 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric) obj).turns),
      Setter = (Action<object, int>) null,
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "turns",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric).GetField("turns", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues3);
    return jsonPropertyInfoArray;
  }

  private void EncounterMetricSerializeHandler(Utf8JsonWriter writer, MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric value)
  {
    writer.WriteStartObject();
    writer.WriteString(MetricsSerializerContext.PropName_id, value.id);
    writer.WriteNumber(MetricsSerializerContext.PropName_damage, value.damage);
    writer.WriteNumber(MetricsSerializerContext.PropName_turns, value.turns);
    writer.WriteEndObject();
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric> EventChoiceMetric
  {
    get
    {
      return this._EventChoiceMetric ?? (this._EventChoiceMetric = (JsonTypeInfo<MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric> Create_EventChoiceMetric(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric> jsonTypeInfo;
    if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric>) (() => new MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MetricsSerializerContext.EventChoiceMetricPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = new Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric>(this.EventChoiceMetricSerializeHandler)
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] EventChoiceMetricPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[3];
    JsonPropertyInfoValues<string> propertyInfoValues1 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric) obj).id),
      Setter = (Action<object, string>) null,
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "id",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric).GetField("id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues1);
    jsonPropertyInfoArray[0].IsGetNullable = false;
    jsonPropertyInfoArray[0].IsSetNullable = false;
    JsonPropertyInfoValues<string> propertyInfoValues2 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric) obj).act),
      Setter = (Action<object, string>) null,
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "act",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric).GetField("act", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues2);
    jsonPropertyInfoArray[1].IsGetNullable = false;
    jsonPropertyInfoArray[1].IsSetNullable = false;
    JsonPropertyInfoValues<string> propertyInfoValues3 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric) obj).picked),
      Setter = (Action<object, string>) null,
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "picked",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric).GetField("picked", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues3);
    jsonPropertyInfoArray[2].IsGetNullable = false;
    jsonPropertyInfoArray[2].IsSetNullable = false;
    return jsonPropertyInfoArray;
  }

  private void EventChoiceMetricSerializeHandler(Utf8JsonWriter writer, MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric value)
  {
    writer.WriteStartObject();
    writer.WriteString(MetricsSerializerContext.PropName_id, value.id);
    writer.WriteString(MetricsSerializerContext.PropName_act, value.act);
    writer.WriteString(MetricsSerializerContext.PropName_picked, value.picked);
    writer.WriteEndObject();
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics> RunMetrics
  {
    get
    {
      return this._RunMetrics ?? (this._RunMetrics = (JsonTypeInfo<MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics> Create_RunMetrics(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics> jsonTypeInfo;
    if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics>(options, out jsonTypeInfo))
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics>) null,
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics>) (args => new MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics()
        {
          BuildId = (string) args[0],
          PlayerId = (string) args[1],
          Character = (MegaCrit.Sts2.Core.Models.ModelId) args[2],
          Win = (bool) args[3],
          NumPlayers = (int) args[4],
          Team = (List<MegaCrit.Sts2.Core.Models.ModelId>) args[5],
          BuildType = (string) args[6],
          Ascension = (int) args[7],
          TotalPlaytime = (float) args[8],
          TotalWinRate = (float) args[9],
          NumReloads = (int) args[10],
          RunPlaytime = (float) args[11],
          FloorReached = (int) args[12],
          KilledByEncounter = (MegaCrit.Sts2.Core.Models.ModelId) args[13],
          CardChoices = (List<MegaCrit.Sts2.Core.Runs.Metrics.CardChoiceMetric>) args[14],
          CampfireUpgrades = (List<string>) args[15],
          EventChoices = (List<MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric>) args[16 /*0x10*/],
          AncientChoices = (List<MegaCrit.Sts2.Core.Runs.Metrics.AncientMetric>) args[17],
          RelicBuys = (List<string>) args[18],
          PotionBuys = (List<string>) args[19],
          ColorlessBuys = (List<string>) args[20],
          PotionDiscards = (List<string>) args[21],
          Encounters = (List<MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric>) args[22],
          ActWins = (List<MegaCrit.Sts2.Core.Runs.Metrics.ActWinMetric>) args[23],
          Deck = (IEnumerable<MegaCrit.Sts2.Core.Models.ModelId>) args[24],
          Relics = (IEnumerable<MegaCrit.Sts2.Core.Models.ModelId>) args[25]
        }),
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MetricsSerializerContext.RunMetricsPropInit(options)),
        ConstructorParameterMetadataInitializer = MetricsSerializerContext.\u003C\u003EO.\u003C2\u003E__RunMetricsCtorParamInit ?? (MetricsSerializerContext.\u003C\u003EO.\u003C2\u003E__RunMetricsCtorParamInit = new Func<JsonParameterInfoValues[]>(MetricsSerializerContext.RunMetricsCtorParamInit)),
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = new Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics>(this.RunMetricsSerializeHandler)
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] RunMetricsPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[26];
    JsonPropertyInfoValues<string> propertyInfoValues1 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics) obj).BuildId),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "BuildId",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics).GetProperty("BuildId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues1);
    jsonPropertyInfoArray[0].IsRequired = true;
    jsonPropertyInfoArray[0].IsGetNullable = false;
    jsonPropertyInfoArray[0].IsSetNullable = false;
    JsonPropertyInfoValues<string> propertyInfoValues2 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics) obj).PlayerId),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "PlayerId",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics).GetProperty("PlayerId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues2);
    jsonPropertyInfoArray[1].IsRequired = true;
    jsonPropertyInfoArray[1].IsGetNullable = false;
    jsonPropertyInfoArray[1].IsSetNullable = false;
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId> propertyInfoValues3 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Models.ModelId>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Models.ModelId>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics) obj).Character),
      Setter = (Action<object, MegaCrit.Sts2.Core.Models.ModelId>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Character",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics).GetProperty("Character", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Models.ModelId), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Models.ModelId>(options, propertyInfoValues3);
    jsonPropertyInfoArray[2].IsRequired = true;
    jsonPropertyInfoArray[2].IsGetNullable = false;
    jsonPropertyInfoArray[2].IsSetNullable = false;
    JsonPropertyInfoValues<bool> propertyInfoValues4 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics) obj).Win),
      Setter = (Action<object, bool>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Win",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics).GetProperty("Win", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (bool), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[3] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues4);
    jsonPropertyInfoArray[3].IsRequired = true;
    JsonPropertyInfoValues<int> propertyInfoValues5 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics) obj).NumPlayers),
      Setter = (Action<object, int>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "NumPlayers",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics).GetProperty("NumPlayers", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[4] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues5);
    jsonPropertyInfoArray[4].IsRequired = true;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>> propertyInfoValues6 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Models.ModelId>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics) obj).Team),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Models.ModelId>>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Team",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics).GetProperty("Team", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Models.ModelId>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[5] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Models.ModelId>>(options, propertyInfoValues6);
    jsonPropertyInfoArray[5].IsRequired = true;
    jsonPropertyInfoArray[5].IsGetNullable = false;
    jsonPropertyInfoArray[5].IsSetNullable = false;
    JsonPropertyInfoValues<string> propertyInfoValues7 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics) obj).BuildType),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "BuildType",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics).GetProperty("BuildType", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[6] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues7);
    jsonPropertyInfoArray[6].IsRequired = true;
    jsonPropertyInfoArray[6].IsGetNullable = false;
    jsonPropertyInfoArray[6].IsSetNullable = false;
    JsonPropertyInfoValues<int> propertyInfoValues8 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics) obj).Ascension),
      Setter = (Action<object, int>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Ascension",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics).GetProperty("Ascension", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[7] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues8);
    JsonPropertyInfoValues<float> propertyInfoValues9 = new JsonPropertyInfoValues<float>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics),
      Converter = (JsonConverter<float>) null,
      Getter = (Func<object, float>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics) obj).TotalPlaytime),
      Setter = (Action<object, float>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "TotalPlaytime",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics).GetProperty("TotalPlaytime", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (float), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[8] = JsonMetadataServices.CreatePropertyInfo<float>(options, propertyInfoValues9);
    jsonPropertyInfoArray[8].IsRequired = true;
    JsonPropertyInfoValues<float> propertyInfoValues10 = new JsonPropertyInfoValues<float>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics),
      Converter = (JsonConverter<float>) null,
      Getter = (Func<object, float>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics) obj).TotalWinRate),
      Setter = (Action<object, float>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "TotalWinRate",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics).GetProperty("TotalWinRate", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (float), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[9] = JsonMetadataServices.CreatePropertyInfo<float>(options, propertyInfoValues10);
    jsonPropertyInfoArray[9].IsRequired = true;
    JsonPropertyInfoValues<int> propertyInfoValues11 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics) obj).NumReloads),
      Setter = (Action<object, int>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "NumReloads",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics).GetProperty("NumReloads", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[10] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues11);
    jsonPropertyInfoArray[10].IsRequired = true;
    JsonPropertyInfoValues<float> propertyInfoValues12 = new JsonPropertyInfoValues<float>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics),
      Converter = (JsonConverter<float>) null,
      Getter = (Func<object, float>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics) obj).RunPlaytime),
      Setter = (Action<object, float>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "RunPlaytime",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics).GetProperty("RunPlaytime", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (float), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[11] = JsonMetadataServices.CreatePropertyInfo<float>(options, propertyInfoValues12);
    jsonPropertyInfoArray[11].IsRequired = true;
    JsonPropertyInfoValues<int> propertyInfoValues13 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics) obj).FloorReached),
      Setter = (Action<object, int>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "FloorReached",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics).GetProperty("FloorReached", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[12] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues13);
    jsonPropertyInfoArray[12].IsRequired = true;
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId> propertyInfoValues14 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Models.ModelId>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Models.ModelId>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics) obj).KilledByEncounter),
      Setter = (Action<object, MegaCrit.Sts2.Core.Models.ModelId>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "KilledByEncounter",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics).GetProperty("KilledByEncounter", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Models.ModelId), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[13] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Models.ModelId>(options, propertyInfoValues14);
    jsonPropertyInfoArray[13].IsRequired = true;
    jsonPropertyInfoArray[13].IsGetNullable = false;
    jsonPropertyInfoArray[13].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Runs.Metrics.CardChoiceMetric>> propertyInfoValues15 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Runs.Metrics.CardChoiceMetric>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Runs.Metrics.CardChoiceMetric>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Runs.Metrics.CardChoiceMetric>>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics) obj).CardChoices),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Runs.Metrics.CardChoiceMetric>>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "CardChoices",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics).GetProperty("CardChoices", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Runs.Metrics.CardChoiceMetric>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[14] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Runs.Metrics.CardChoiceMetric>>(options, propertyInfoValues15);
    jsonPropertyInfoArray[14].IsRequired = true;
    jsonPropertyInfoArray[14].IsGetNullable = false;
    jsonPropertyInfoArray[14].IsSetNullable = false;
    JsonPropertyInfoValues<List<string>> propertyInfoValues16 = new JsonPropertyInfoValues<List<string>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics),
      Converter = (JsonConverter<List<string>>) null,
      Getter = (Func<object, List<string>>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics) obj).CampfireUpgrades),
      Setter = (Action<object, List<string>>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "CampfireUpgrades",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics).GetProperty("CampfireUpgrades", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<string>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[15] = JsonMetadataServices.CreatePropertyInfo<List<string>>(options, propertyInfoValues16);
    jsonPropertyInfoArray[15].IsRequired = true;
    jsonPropertyInfoArray[15].IsGetNullable = false;
    jsonPropertyInfoArray[15].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric>> propertyInfoValues17 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric>>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics) obj).EventChoices),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric>>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "EventChoices",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics).GetProperty("EventChoices", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[16 /*0x10*/] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric>>(options, propertyInfoValues17);
    jsonPropertyInfoArray[16 /*0x10*/].IsRequired = true;
    jsonPropertyInfoArray[16 /*0x10*/].IsGetNullable = false;
    jsonPropertyInfoArray[16 /*0x10*/].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Runs.Metrics.AncientMetric>> propertyInfoValues18 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Runs.Metrics.AncientMetric>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Runs.Metrics.AncientMetric>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Runs.Metrics.AncientMetric>>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics) obj).AncientChoices),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Runs.Metrics.AncientMetric>>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "AncientChoices",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics).GetProperty("AncientChoices", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Runs.Metrics.AncientMetric>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[17] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Runs.Metrics.AncientMetric>>(options, propertyInfoValues18);
    jsonPropertyInfoArray[17].IsRequired = true;
    jsonPropertyInfoArray[17].IsGetNullable = false;
    jsonPropertyInfoArray[17].IsSetNullable = false;
    JsonPropertyInfoValues<List<string>> propertyInfoValues19 = new JsonPropertyInfoValues<List<string>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics),
      Converter = (JsonConverter<List<string>>) null,
      Getter = (Func<object, List<string>>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics) obj).RelicBuys),
      Setter = (Action<object, List<string>>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "RelicBuys",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics).GetProperty("RelicBuys", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<string>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[18] = JsonMetadataServices.CreatePropertyInfo<List<string>>(options, propertyInfoValues19);
    jsonPropertyInfoArray[18].IsRequired = true;
    jsonPropertyInfoArray[18].IsGetNullable = false;
    jsonPropertyInfoArray[18].IsSetNullable = false;
    JsonPropertyInfoValues<List<string>> propertyInfoValues20 = new JsonPropertyInfoValues<List<string>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics),
      Converter = (JsonConverter<List<string>>) null,
      Getter = (Func<object, List<string>>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics) obj).PotionBuys),
      Setter = (Action<object, List<string>>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "PotionBuys",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics).GetProperty("PotionBuys", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<string>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[19] = JsonMetadataServices.CreatePropertyInfo<List<string>>(options, propertyInfoValues20);
    jsonPropertyInfoArray[19].IsRequired = true;
    jsonPropertyInfoArray[19].IsGetNullable = false;
    jsonPropertyInfoArray[19].IsSetNullable = false;
    JsonPropertyInfoValues<List<string>> propertyInfoValues21 = new JsonPropertyInfoValues<List<string>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics),
      Converter = (JsonConverter<List<string>>) null,
      Getter = (Func<object, List<string>>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics) obj).ColorlessBuys),
      Setter = (Action<object, List<string>>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "ColorlessBuys",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics).GetProperty("ColorlessBuys", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<string>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[20] = JsonMetadataServices.CreatePropertyInfo<List<string>>(options, propertyInfoValues21);
    jsonPropertyInfoArray[20].IsRequired = true;
    jsonPropertyInfoArray[20].IsGetNullable = false;
    jsonPropertyInfoArray[20].IsSetNullable = false;
    JsonPropertyInfoValues<List<string>> propertyInfoValues22 = new JsonPropertyInfoValues<List<string>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics),
      Converter = (JsonConverter<List<string>>) null,
      Getter = (Func<object, List<string>>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics) obj).PotionDiscards),
      Setter = (Action<object, List<string>>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "PotionDiscards",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics).GetProperty("PotionDiscards", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<string>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[21] = JsonMetadataServices.CreatePropertyInfo<List<string>>(options, propertyInfoValues22);
    jsonPropertyInfoArray[21].IsRequired = true;
    jsonPropertyInfoArray[21].IsGetNullable = false;
    jsonPropertyInfoArray[21].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric>> propertyInfoValues23 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric>>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics) obj).Encounters),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric>>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Encounters",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics).GetProperty("Encounters", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[22] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric>>(options, propertyInfoValues23);
    jsonPropertyInfoArray[22].IsRequired = true;
    jsonPropertyInfoArray[22].IsGetNullable = false;
    jsonPropertyInfoArray[22].IsSetNullable = false;
    JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Runs.Metrics.ActWinMetric>> propertyInfoValues24 = new JsonPropertyInfoValues<List<MegaCrit.Sts2.Core.Runs.Metrics.ActWinMetric>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics),
      Converter = (JsonConverter<List<MegaCrit.Sts2.Core.Runs.Metrics.ActWinMetric>>) null,
      Getter = (Func<object, List<MegaCrit.Sts2.Core.Runs.Metrics.ActWinMetric>>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics) obj).ActWins),
      Setter = (Action<object, List<MegaCrit.Sts2.Core.Runs.Metrics.ActWinMetric>>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "ActWins",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics).GetProperty("ActWins", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<MegaCrit.Sts2.Core.Runs.Metrics.ActWinMetric>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[23] = JsonMetadataServices.CreatePropertyInfo<List<MegaCrit.Sts2.Core.Runs.Metrics.ActWinMetric>>(options, propertyInfoValues24);
    jsonPropertyInfoArray[23].IsRequired = true;
    jsonPropertyInfoArray[23].IsGetNullable = false;
    jsonPropertyInfoArray[23].IsSetNullable = false;
    JsonPropertyInfoValues<IEnumerable<MegaCrit.Sts2.Core.Models.ModelId>> propertyInfoValues25 = new JsonPropertyInfoValues<IEnumerable<MegaCrit.Sts2.Core.Models.ModelId>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics),
      Converter = (JsonConverter<IEnumerable<MegaCrit.Sts2.Core.Models.ModelId>>) null,
      Getter = (Func<object, IEnumerable<MegaCrit.Sts2.Core.Models.ModelId>>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics) obj).Deck),
      Setter = (Action<object, IEnumerable<MegaCrit.Sts2.Core.Models.ModelId>>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Deck",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics).GetProperty("Deck", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (IEnumerable<MegaCrit.Sts2.Core.Models.ModelId>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[24] = JsonMetadataServices.CreatePropertyInfo<IEnumerable<MegaCrit.Sts2.Core.Models.ModelId>>(options, propertyInfoValues25);
    jsonPropertyInfoArray[24].IsRequired = true;
    jsonPropertyInfoArray[24].IsGetNullable = false;
    jsonPropertyInfoArray[24].IsSetNullable = false;
    JsonPropertyInfoValues<IEnumerable<MegaCrit.Sts2.Core.Models.ModelId>> propertyInfoValues26 = new JsonPropertyInfoValues<IEnumerable<MegaCrit.Sts2.Core.Models.ModelId>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics),
      Converter = (JsonConverter<IEnumerable<MegaCrit.Sts2.Core.Models.ModelId>>) null,
      Getter = (Func<object, IEnumerable<MegaCrit.Sts2.Core.Models.ModelId>>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics) obj).Relics),
      Setter = (Action<object, IEnumerable<MegaCrit.Sts2.Core.Models.ModelId>>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Relics",
      JsonPropertyName = (string) null,
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics).GetProperty("Relics", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (IEnumerable<MegaCrit.Sts2.Core.Models.ModelId>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[25] = JsonMetadataServices.CreatePropertyInfo<IEnumerable<MegaCrit.Sts2.Core.Models.ModelId>>(options, propertyInfoValues26);
    jsonPropertyInfoArray[25].IsRequired = true;
    jsonPropertyInfoArray[25].IsGetNullable = false;
    jsonPropertyInfoArray[25].IsSetNullable = false;
    return jsonPropertyInfoArray;
  }

  private void RunMetricsSerializeHandler(Utf8JsonWriter writer, MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics? value)
  {
    if (value == null)
    {
      writer.WriteNullValue();
    }
    else
    {
      writer.WriteStartObject();
      writer.WriteString(MetricsSerializerContext.PropName_buildId, value.BuildId);
      writer.WriteString(MetricsSerializerContext.PropName_playerId, value.PlayerId);
      writer.WritePropertyName(MetricsSerializerContext.PropName_character);
      this.ModelIdSerializeHandler(writer, value.Character);
      writer.WriteBoolean(MetricsSerializerContext.PropName_win, value.Win);
      writer.WriteNumber(MetricsSerializerContext.PropName_numPlayers, value.NumPlayers);
      writer.WritePropertyName(MetricsSerializerContext.PropName_team);
      this.ListModelIdSerializeHandler(writer, value.Team);
      writer.WriteString(MetricsSerializerContext.PropName_buildType, value.BuildType);
      writer.WriteNumber(MetricsSerializerContext.PropName_ascension, value.Ascension);
      writer.WriteNumber(MetricsSerializerContext.PropName_totalPlaytime, value.TotalPlaytime);
      writer.WriteNumber(MetricsSerializerContext.PropName_totalWinRate, value.TotalWinRate);
      writer.WriteNumber(MetricsSerializerContext.PropName_numReloads, value.NumReloads);
      writer.WriteNumber(MetricsSerializerContext.PropName_runPlaytime, value.RunPlaytime);
      writer.WriteNumber(MetricsSerializerContext.PropName_floorReached, value.FloorReached);
      writer.WritePropertyName(MetricsSerializerContext.PropName_killedByEncounter);
      this.ModelIdSerializeHandler(writer, value.KilledByEncounter);
      writer.WritePropertyName(MetricsSerializerContext.PropName_cardChoices);
      this.ListCardChoiceMetricSerializeHandler(writer, value.CardChoices);
      writer.WritePropertyName(MetricsSerializerContext.PropName_campfireUpgrades);
      this.ListStringSerializeHandler(writer, value.CampfireUpgrades);
      writer.WritePropertyName(MetricsSerializerContext.PropName_eventChoices);
      this.ListEventChoiceMetricSerializeHandler(writer, value.EventChoices);
      writer.WritePropertyName(MetricsSerializerContext.PropName_ancientChoices);
      this.ListAncientMetricSerializeHandler(writer, value.AncientChoices);
      writer.WritePropertyName(MetricsSerializerContext.PropName_relicBuys);
      this.ListStringSerializeHandler(writer, value.RelicBuys);
      writer.WritePropertyName(MetricsSerializerContext.PropName_potionBuys);
      this.ListStringSerializeHandler(writer, value.PotionBuys);
      writer.WritePropertyName(MetricsSerializerContext.PropName_colorlessBuys);
      this.ListStringSerializeHandler(writer, value.ColorlessBuys);
      writer.WritePropertyName(MetricsSerializerContext.PropName_potionDiscards);
      this.ListStringSerializeHandler(writer, value.PotionDiscards);
      writer.WritePropertyName(MetricsSerializerContext.PropName_encounters);
      this.ListEncounterMetricSerializeHandler(writer, value.Encounters);
      writer.WritePropertyName(MetricsSerializerContext.PropName_actWins);
      this.ListActWinMetricSerializeHandler(writer, value.ActWins);
      writer.WritePropertyName(MetricsSerializerContext.PropName_deck);
      this.IEnumerableModelIdSerializeHandler(writer, value.Deck);
      writer.WritePropertyName(MetricsSerializerContext.PropName_relics);
      this.IEnumerableModelIdSerializeHandler(writer, value.Relics);
      writer.WriteEndObject();
    }
  }

  private static JsonParameterInfoValues[] RunMetricsCtorParamInit()
  {
    return new JsonParameterInfoValues[26]
    {
      new JsonParameterInfoValues()
      {
        Name = "BuildId",
        ParameterType = typeof (string),
        Position = 0,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "PlayerId",
        ParameterType = typeof (string),
        Position = 1,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Character",
        ParameterType = typeof (MegaCrit.Sts2.Core.Models.ModelId),
        Position = 2,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Win",
        ParameterType = typeof (bool),
        Position = 3,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "NumPlayers",
        ParameterType = typeof (int),
        Position = 4,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Team",
        ParameterType = typeof (List<MegaCrit.Sts2.Core.Models.ModelId>),
        Position = 5,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "BuildType",
        ParameterType = typeof (string),
        Position = 6,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Ascension",
        ParameterType = typeof (int),
        Position = 7,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "TotalPlaytime",
        ParameterType = typeof (float),
        Position = 8,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "TotalWinRate",
        ParameterType = typeof (float),
        Position = 9,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "NumReloads",
        ParameterType = typeof (int),
        Position = 10,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "RunPlaytime",
        ParameterType = typeof (float),
        Position = 11,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "FloorReached",
        ParameterType = typeof (int),
        Position = 12,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "KilledByEncounter",
        ParameterType = typeof (MegaCrit.Sts2.Core.Models.ModelId),
        Position = 13,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "CardChoices",
        ParameterType = typeof (List<MegaCrit.Sts2.Core.Runs.Metrics.CardChoiceMetric>),
        Position = 14,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "CampfireUpgrades",
        ParameterType = typeof (List<string>),
        Position = 15,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "EventChoices",
        ParameterType = typeof (List<MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric>),
        Position = 16 /*0x10*/,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "AncientChoices",
        ParameterType = typeof (List<MegaCrit.Sts2.Core.Runs.Metrics.AncientMetric>),
        Position = 17,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "RelicBuys",
        ParameterType = typeof (List<string>),
        Position = 18,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "PotionBuys",
        ParameterType = typeof (List<string>),
        Position = 19,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "ColorlessBuys",
        ParameterType = typeof (List<string>),
        Position = 20,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "PotionDiscards",
        ParameterType = typeof (List<string>),
        Position = 21,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Encounters",
        ParameterType = typeof (List<MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric>),
        Position = 22,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "ActWins",
        ParameterType = typeof (List<MegaCrit.Sts2.Core.Runs.Metrics.ActWinMetric>),
        Position = 23,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Deck",
        ParameterType = typeof (IEnumerable<MegaCrit.Sts2.Core.Models.ModelId>),
        Position = 24,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Relics",
        ParameterType = typeof (IEnumerable<MegaCrit.Sts2.Core.Models.ModelId>),
        Position = 25,
        IsNullable = false,
        IsMemberInitializer = true
      }
    };
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric> SettingsDataMetric
  {
    get
    {
      return this._SettingsDataMetric ?? (this._SettingsDataMetric = (JsonTypeInfo<MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric> Create_SettingsDataMetric(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric> jsonTypeInfo;
    if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric>(options, out jsonTypeInfo))
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric>) null,
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric>) (args => new MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric()
        {
          BuildId = (string) args[0],
          FastModeType = (MegaCrit.Sts2.Core.Settings.FastModeType) args[1]
        }),
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MetricsSerializerContext.SettingsDataMetricPropInit(options)),
        ConstructorParameterMetadataInitializer = MetricsSerializerContext.\u003C\u003EO.\u003C3\u003E__SettingsDataMetricCtorParamInit ?? (MetricsSerializerContext.\u003C\u003EO.\u003C3\u003E__SettingsDataMetricCtorParamInit = new Func<JsonParameterInfoValues[]>(MetricsSerializerContext.SettingsDataMetricCtorParamInit)),
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = new Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric>(this.SettingsDataMetricSerializeHandler)
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] SettingsDataMetricPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[18];
    JsonPropertyInfoValues<string> propertyInfoValues1 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric) obj).BuildId),
      Setter = (Action<object, string>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric>(obj).BuildId = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "BuildId",
      JsonPropertyName = "buildId",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric).GetProperty("BuildId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues1);
    jsonPropertyInfoArray[0].IsRequired = true;
    jsonPropertyInfoArray[0].IsGetNullable = false;
    jsonPropertyInfoArray[0].IsSetNullable = false;
    JsonPropertyInfoValues<string> propertyInfoValues2 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric) obj).Os),
      Setter = (Action<object, string>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric>(obj).Os = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Os",
      JsonPropertyName = "os",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric).GetProperty("Os", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues2);
    jsonPropertyInfoArray[1].IsGetNullable = false;
    jsonPropertyInfoArray[1].IsSetNullable = false;
    JsonPropertyInfoValues<string> propertyInfoValues3 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric) obj).Platform),
      Setter = (Action<object, string>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric>(obj).Platform = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Platform",
      JsonPropertyName = "platform",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric).GetProperty("Platform", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues3);
    jsonPropertyInfoArray[2].IsGetNullable = false;
    jsonPropertyInfoArray[2].IsSetNullable = false;
    JsonPropertyInfoValues<int> propertyInfoValues4 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric) obj).SystemRam),
      Setter = (Action<object, int>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric>(obj).SystemRam = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "SystemRam",
      JsonPropertyName = "systemRam",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric).GetProperty("SystemRam", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[3] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues4);
    JsonPropertyInfoValues<string> propertyInfoValues5 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric) obj).LanguageCode),
      Setter = (Action<object, string>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric>(obj).LanguageCode = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "LanguageCode",
      JsonPropertyName = "language",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric).GetProperty("LanguageCode", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[4] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues5);
    jsonPropertyInfoArray[4].IsGetNullable = false;
    jsonPropertyInfoArray[4].IsSetNullable = false;
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Settings.FastModeType> propertyInfoValues6 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Settings.FastModeType>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Settings.FastModeType>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Settings.FastModeType>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric) obj).FastModeType),
      Setter = (Action<object, MegaCrit.Sts2.Core.Settings.FastModeType>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric>(obj).FastModeType = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "FastModeType",
      JsonPropertyName = "combatSpeed",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric).GetProperty("FastModeType", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Settings.FastModeType), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[5] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Settings.FastModeType>(options, propertyInfoValues6);
    jsonPropertyInfoArray[5].IsRequired = true;
    JsonPropertyInfoValues<int> propertyInfoValues7 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric) obj).Screenshake),
      Setter = (Action<object, int>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric>(obj).Screenshake = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Screenshake",
      JsonPropertyName = "screenshake",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric).GetProperty("Screenshake", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[6] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues7);
    JsonPropertyInfoValues<bool> propertyInfoValues8 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric) obj).ShowRunTimer),
      Setter = (Action<object, bool>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric>(obj).ShowRunTimer = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "ShowRunTimer",
      JsonPropertyName = "runTimer",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric).GetProperty("ShowRunTimer", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (bool), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[7] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues8);
    JsonPropertyInfoValues<bool> propertyInfoValues9 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric) obj).PhobiaMode),
      Setter = (Action<object, bool>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric>(obj).PhobiaMode = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "PhobiaMode",
      JsonPropertyName = "phobiaMode",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric).GetProperty("PhobiaMode", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (bool), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[8] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues9);
    JsonPropertyInfoValues<bool> propertyInfoValues10 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric) obj).ShowCardIndices),
      Setter = (Action<object, bool>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric>(obj).ShowCardIndices = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "ShowCardIndices",
      JsonPropertyName = "cardIndices",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric).GetProperty("ShowCardIndices", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (bool), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[9] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues10);
    JsonPropertyInfoValues<int> propertyInfoValues11 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric) obj).DisplayCount),
      Setter = (Action<object, int>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric>(obj).DisplayCount = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "DisplayCount",
      JsonPropertyName = "displayCount",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric).GetProperty("DisplayCount", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[10] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues11);
    JsonPropertyInfoValues<Godot.Vector2I> propertyInfoValues12 = new JsonPropertyInfoValues<Godot.Vector2I>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric),
      Converter = (JsonConverter<Godot.Vector2I>) null,
      Getter = (Func<object, Godot.Vector2I>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric) obj).DisplayResolution),
      Setter = (Action<object, Godot.Vector2I>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric>(obj).DisplayResolution = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "DisplayResolution",
      JsonPropertyName = "displayResolution",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric).GetProperty("DisplayResolution", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (Godot.Vector2I), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[11] = JsonMetadataServices.CreatePropertyInfo<Godot.Vector2I>(options, propertyInfoValues12);
    JsonPropertyInfoValues<bool> propertyInfoValues13 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric) obj).Fullscreen),
      Setter = (Action<object, bool>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric>(obj).Fullscreen = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Fullscreen",
      JsonPropertyName = "fullscreen",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric).GetProperty("Fullscreen", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (bool), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[12] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues13);
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Settings.AspectRatioSetting> propertyInfoValues14 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Settings.AspectRatioSetting>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Settings.AspectRatioSetting>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Settings.AspectRatioSetting>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric) obj).AspectRatio),
      Setter = (Action<object, MegaCrit.Sts2.Core.Settings.AspectRatioSetting>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric>(obj).AspectRatio = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "AspectRatio",
      JsonPropertyName = "aspectRatio",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric).GetProperty("AspectRatio", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Settings.AspectRatioSetting), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[13] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Settings.AspectRatioSetting>(options, propertyInfoValues14);
    JsonPropertyInfoValues<bool> propertyInfoValues15 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric) obj).ResizeWindows),
      Setter = (Action<object, bool>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric>(obj).ResizeWindows = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "ResizeWindows",
      JsonPropertyName = "resizeWindows",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric).GetProperty("ResizeWindows", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (bool), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[14] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues15);
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Settings.VSyncType> propertyInfoValues16 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Settings.VSyncType>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Settings.VSyncType>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Settings.VSyncType>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric) obj).VSync),
      Setter = (Action<object, MegaCrit.Sts2.Core.Settings.VSyncType>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric>(obj).VSync = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "VSync",
      JsonPropertyName = "vSync",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric).GetProperty("VSync", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Settings.VSyncType), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[15] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Settings.VSyncType>(options, propertyInfoValues16);
    JsonPropertyInfoValues<int> propertyInfoValues17 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric) obj).FpsLimit),
      Setter = (Action<object, int>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric>(obj).FpsLimit = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "FpsLimit",
      JsonPropertyName = "fpsLimit",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric).GetProperty("FpsLimit", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[16 /*0x10*/] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues17);
    JsonPropertyInfoValues<int> propertyInfoValues18 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric) obj).Msaa),
      Setter = (Action<object, int>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric>(obj).Msaa = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Msaa",
      JsonPropertyName = "msaa",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric).GetProperty("Msaa", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[17] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues18);
    return jsonPropertyInfoArray;
  }

  private void SettingsDataMetricSerializeHandler(Utf8JsonWriter writer, MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric value)
  {
    writer.WriteStartObject();
    writer.WriteString(MetricsSerializerContext.PropName_buildId, value.BuildId);
    writer.WriteString(MetricsSerializerContext.PropName_os, value.Os);
    writer.WriteString(MetricsSerializerContext.PropName_platform, value.Platform);
    writer.WriteNumber(MetricsSerializerContext.PropName_systemRam, value.SystemRam);
    writer.WriteString(MetricsSerializerContext.PropName_language, value.LanguageCode);
    writer.WritePropertyName(MetricsSerializerContext.PropName_combatSpeed);
    JsonSerializer.Serialize<MegaCrit.Sts2.Core.Settings.FastModeType>(writer, value.FastModeType, this.FastModeType);
    writer.WriteNumber(MetricsSerializerContext.PropName_screenshake, value.Screenshake);
    writer.WriteBoolean(MetricsSerializerContext.PropName_runTimer, value.ShowRunTimer);
    writer.WriteBoolean(MetricsSerializerContext.PropName_phobiaMode, value.PhobiaMode);
    writer.WriteBoolean(MetricsSerializerContext.PropName_cardIndices, value.ShowCardIndices);
    writer.WriteNumber(MetricsSerializerContext.PropName_displayCount, value.DisplayCount);
    writer.WritePropertyName(MetricsSerializerContext.PropName_displayResolution);
    this.Vector2ISerializeHandler(writer, value.DisplayResolution);
    writer.WriteBoolean(MetricsSerializerContext.PropName_fullscreen, value.Fullscreen);
    writer.WritePropertyName(MetricsSerializerContext.PropName_aspectRatio);
    JsonSerializer.Serialize<MegaCrit.Sts2.Core.Settings.AspectRatioSetting>(writer, value.AspectRatio, this.AspectRatioSetting);
    writer.WriteBoolean(MetricsSerializerContext.PropName_resizeWindows, value.ResizeWindows);
    writer.WritePropertyName(MetricsSerializerContext.PropName_vSync);
    JsonSerializer.Serialize<MegaCrit.Sts2.Core.Settings.VSyncType>(writer, value.VSync, this.VSyncType);
    writer.WriteNumber(MetricsSerializerContext.PropName_fpsLimit, value.FpsLimit);
    writer.WriteNumber(MetricsSerializerContext.PropName_msaa, value.Msaa);
    writer.WriteEndObject();
  }

  private static JsonParameterInfoValues[] SettingsDataMetricCtorParamInit()
  {
    return new JsonParameterInfoValues[2]
    {
      new JsonParameterInfoValues()
      {
        Name = "BuildId",
        ParameterType = typeof (string),
        Position = 0,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "FastModeType",
        ParameterType = typeof (MegaCrit.Sts2.Core.Settings.FastModeType),
        Position = 1,
        IsNullable = false,
        IsMemberInitializer = true
      }
    };
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
    if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Settings.AspectRatioSetting>(options, out jsonTypeInfo))
    {
      JsonConverter jsonConverter = MetricsSerializerContext.ExpandConverter(typeof (MegaCrit.Sts2.Core.Settings.AspectRatioSetting), (JsonConverter) new JsonStringEnumConverter<MegaCrit.Sts2.Core.Settings.AspectRatioSetting>(), options);
      jsonTypeInfo = JsonMetadataServices.CreateValueInfo<MegaCrit.Sts2.Core.Settings.AspectRatioSetting>(options, jsonConverter);
    }
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
    if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Settings.FastModeType>(options, out jsonTypeInfo))
    {
      JsonConverter jsonConverter = MetricsSerializerContext.ExpandConverter(typeof (MegaCrit.Sts2.Core.Settings.FastModeType), (JsonConverter) new JsonStringEnumConverter<MegaCrit.Sts2.Core.Settings.FastModeType>(), options);
      jsonTypeInfo = JsonMetadataServices.CreateValueInfo<MegaCrit.Sts2.Core.Settings.FastModeType>(options, jsonConverter);
    }
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
    if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Settings.VSyncType>(options, out jsonTypeInfo))
    {
      JsonConverter jsonConverter = MetricsSerializerContext.ExpandConverter(typeof (MegaCrit.Sts2.Core.Settings.VSyncType), (JsonConverter) new JsonStringEnumConverter<MegaCrit.Sts2.Core.Settings.VSyncType>(), options);
      jsonTypeInfo = JsonMetadataServices.CreateValueInfo<MegaCrit.Sts2.Core.Settings.VSyncType>(options, jsonConverter);
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Timeline.EpochMetric> EpochMetric
  {
    get
    {
      return this._EpochMetric ?? (this._EpochMetric = (JsonTypeInfo<MegaCrit.Sts2.Core.Timeline.EpochMetric>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Timeline.EpochMetric)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Timeline.EpochMetric> Create_EpochMetric(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Timeline.EpochMetric> jsonTypeInfo;
    if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Timeline.EpochMetric>(options, out jsonTypeInfo))
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Timeline.EpochMetric> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Timeline.EpochMetric>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Timeline.EpochMetric>) null,
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Timeline.EpochMetric>) (args => new MegaCrit.Sts2.Core.Timeline.EpochMetric()
        {
          BuildId = (string) args[0],
          Epoch = (string) args[1]
        }),
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => MetricsSerializerContext.EpochMetricPropInit(options)),
        ConstructorParameterMetadataInitializer = MetricsSerializerContext.\u003C\u003EO.\u003C4\u003E__EpochMetricCtorParamInit ?? (MetricsSerializerContext.\u003C\u003EO.\u003C4\u003E__EpochMetricCtorParamInit = new Func<JsonParameterInfoValues[]>(MetricsSerializerContext.EpochMetricCtorParamInit)),
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Timeline.EpochMetric).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = new Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Timeline.EpochMetric>(this.EpochMetricSerializeHandler)
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Timeline.EpochMetric>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] EpochMetricPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[5];
    JsonPropertyInfoValues<string> propertyInfoValues1 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Timeline.EpochMetric),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Timeline.EpochMetric) obj).BuildId),
      Setter = (Action<object, string>) ((obj, value) => ((MegaCrit.Sts2.Core.Timeline.EpochMetric) obj).BuildId = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "BuildId",
      JsonPropertyName = "buildId",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Timeline.EpochMetric).GetProperty("BuildId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues1);
    jsonPropertyInfoArray[0].IsRequired = true;
    jsonPropertyInfoArray[0].IsGetNullable = false;
    jsonPropertyInfoArray[0].IsSetNullable = false;
    JsonPropertyInfoValues<string> propertyInfoValues2 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Timeline.EpochMetric),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Timeline.EpochMetric) obj).Epoch),
      Setter = (Action<object, string>) ((obj, value) => ((MegaCrit.Sts2.Core.Timeline.EpochMetric) obj).Epoch = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Epoch",
      JsonPropertyName = "epoch",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Timeline.EpochMetric).GetProperty("Epoch", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues2);
    jsonPropertyInfoArray[1].IsRequired = true;
    jsonPropertyInfoArray[1].IsGetNullable = false;
    jsonPropertyInfoArray[1].IsSetNullable = false;
    JsonPropertyInfoValues<int> propertyInfoValues3 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Timeline.EpochMetric),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Timeline.EpochMetric) obj).TotalEpochs),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Timeline.EpochMetric) obj).TotalEpochs = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "TotalEpochs",
      JsonPropertyName = "totalEpochs",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Timeline.EpochMetric).GetProperty("TotalEpochs", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues3);
    JsonPropertyInfoValues<long> propertyInfoValues4 = new JsonPropertyInfoValues<long>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Timeline.EpochMetric),
      Converter = (JsonConverter<long>) null,
      Getter = (Func<object, long>) (obj => ((MegaCrit.Sts2.Core.Timeline.EpochMetric) obj).TotalPlaytime),
      Setter = (Action<object, long>) ((obj, value) => ((MegaCrit.Sts2.Core.Timeline.EpochMetric) obj).TotalPlaytime = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "TotalPlaytime",
      JsonPropertyName = "totalPlaytime",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Timeline.EpochMetric).GetProperty("TotalPlaytime", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (long), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[3] = JsonMetadataServices.CreatePropertyInfo<long>(options, propertyInfoValues4);
    JsonPropertyInfoValues<int> propertyInfoValues5 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Timeline.EpochMetric),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Timeline.EpochMetric) obj).TotalRuns),
      Setter = (Action<object, int>) ((obj, value) => ((MegaCrit.Sts2.Core.Timeline.EpochMetric) obj).TotalRuns = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "TotalRuns",
      JsonPropertyName = "totalRuns",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Timeline.EpochMetric).GetProperty("TotalRuns", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[4] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues5);
    return jsonPropertyInfoArray;
  }

  private void EpochMetricSerializeHandler(Utf8JsonWriter writer, MegaCrit.Sts2.Core.Timeline.EpochMetric? value)
  {
    if (value == null)
    {
      writer.WriteNullValue();
    }
    else
    {
      writer.WriteStartObject();
      writer.WriteString(MetricsSerializerContext.PropName_buildId, value.BuildId);
      writer.WriteString(MetricsSerializerContext.PropName_epoch, value.Epoch);
      writer.WriteNumber(MetricsSerializerContext.PropName_totalEpochs, value.TotalEpochs);
      writer.WriteNumber(MetricsSerializerContext.PropName_totalPlaytime, value.TotalPlaytime);
      writer.WriteNumber(MetricsSerializerContext.PropName_totalRuns, value.TotalRuns);
      writer.WriteEndObject();
    }
  }

  private static JsonParameterInfoValues[] EpochMetricCtorParamInit()
  {
    return new JsonParameterInfoValues[2]
    {
      new JsonParameterInfoValues()
      {
        Name = "BuildId",
        ParameterType = typeof (string),
        Position = 0,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Epoch",
        ParameterType = typeof (string),
        Position = 1,
        IsNullable = false,
        IsMemberInitializer = true
      }
    };
  }

  public 
  #nullable disable
  JsonTypeInfo<IEnumerable<MegaCrit.Sts2.Core.Models.ModelId>> IEnumerableModelId
  {
    get
    {
      return this._IEnumerableModelId ?? (this._IEnumerableModelId = (JsonTypeInfo<IEnumerable<MegaCrit.Sts2.Core.Models.ModelId>>) this.Options.GetTypeInfo(typeof (IEnumerable<MegaCrit.Sts2.Core.Models.ModelId>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<IEnumerable<MegaCrit.Sts2.Core.Models.ModelId>> Create_IEnumerableModelId(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<IEnumerable<MegaCrit.Sts2.Core.Models.ModelId>> jsonTypeInfo;
    if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<IEnumerable<MegaCrit.Sts2.Core.Models.ModelId>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<IEnumerable<MegaCrit.Sts2.Core.Models.ModelId>> collectionInfoValues = new JsonCollectionInfoValues<IEnumerable<MegaCrit.Sts2.Core.Models.ModelId>>()
      {
        ObjectCreator = (Func<IEnumerable<MegaCrit.Sts2.Core.Models.ModelId>>) null,
        SerializeHandler = new Action<Utf8JsonWriter, IEnumerable<MegaCrit.Sts2.Core.Models.ModelId>>(this.IEnumerableModelIdSerializeHandler)
      };
      jsonTypeInfo = JsonMetadataServices.CreateIEnumerableInfo<IEnumerable<MegaCrit.Sts2.Core.Models.ModelId>, MegaCrit.Sts2.Core.Models.ModelId>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private void IEnumerableModelIdSerializeHandler(Utf8JsonWriter writer, IEnumerable<MegaCrit.Sts2.Core.Models.ModelId>? value)
  {
    if (value == null)
    {
      writer.WriteNullValue();
    }
    else
    {
      writer.WriteStartArray();
      foreach (MegaCrit.Sts2.Core.Models.ModelId modelId in value)
        this.ModelIdSerializeHandler(writer, modelId);
      writer.WriteEndArray();
    }
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
    if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Models.ModelId>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Models.ModelId>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Models.ModelId>>) (() => new List<MegaCrit.Sts2.Core.Models.ModelId>()),
        SerializeHandler = new Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Models.ModelId>>(this.ListModelIdSerializeHandler)
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Models.ModelId>, MegaCrit.Sts2.Core.Models.ModelId>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private void ListModelIdSerializeHandler(Utf8JsonWriter writer, List<MegaCrit.Sts2.Core.Models.ModelId>? value)
  {
    if (value == null)
    {
      writer.WriteNullValue();
    }
    else
    {
      writer.WriteStartArray();
      for (int index = 0; index < value.Count; ++index)
        this.ModelIdSerializeHandler(writer, value[index]);
      writer.WriteEndArray();
    }
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.Metrics.ActWinMetric>> ListActWinMetric
  {
    get
    {
      return this._ListActWinMetric ?? (this._ListActWinMetric = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.Metrics.ActWinMetric>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Runs.Metrics.ActWinMetric>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.Metrics.ActWinMetric>> Create_ListActWinMetric(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.Metrics.ActWinMetric>> jsonTypeInfo;
    if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Runs.Metrics.ActWinMetric>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Runs.Metrics.ActWinMetric>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Runs.Metrics.ActWinMetric>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Runs.Metrics.ActWinMetric>>) (() => new List<MegaCrit.Sts2.Core.Runs.Metrics.ActWinMetric>()),
        SerializeHandler = new Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Runs.Metrics.ActWinMetric>>(this.ListActWinMetricSerializeHandler)
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Runs.Metrics.ActWinMetric>, MegaCrit.Sts2.Core.Runs.Metrics.ActWinMetric>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private void ListActWinMetricSerializeHandler(Utf8JsonWriter writer, List<MegaCrit.Sts2.Core.Runs.Metrics.ActWinMetric>? value)
  {
    if (value == null)
    {
      writer.WriteNullValue();
    }
    else
    {
      writer.WriteStartArray();
      for (int index = 0; index < value.Count; ++index)
        this.ActWinMetricSerializeHandler(writer, value[index]);
      writer.WriteEndArray();
    }
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.Metrics.AncientMetric>> ListAncientMetric
  {
    get
    {
      return this._ListAncientMetric ?? (this._ListAncientMetric = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.Metrics.AncientMetric>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Runs.Metrics.AncientMetric>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.Metrics.AncientMetric>> Create_ListAncientMetric(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.Metrics.AncientMetric>> jsonTypeInfo;
    if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Runs.Metrics.AncientMetric>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Runs.Metrics.AncientMetric>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Runs.Metrics.AncientMetric>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Runs.Metrics.AncientMetric>>) (() => new List<MegaCrit.Sts2.Core.Runs.Metrics.AncientMetric>()),
        SerializeHandler = new Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Runs.Metrics.AncientMetric>>(this.ListAncientMetricSerializeHandler)
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Runs.Metrics.AncientMetric>, MegaCrit.Sts2.Core.Runs.Metrics.AncientMetric>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private void ListAncientMetricSerializeHandler(Utf8JsonWriter writer, List<MegaCrit.Sts2.Core.Runs.Metrics.AncientMetric>? value)
  {
    if (value == null)
    {
      writer.WriteNullValue();
    }
    else
    {
      writer.WriteStartArray();
      for (int index = 0; index < value.Count; ++index)
        this.AncientMetricSerializeHandler(writer, value[index]);
      writer.WriteEndArray();
    }
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.Metrics.CardChoiceMetric>> ListCardChoiceMetric
  {
    get
    {
      return this._ListCardChoiceMetric ?? (this._ListCardChoiceMetric = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.Metrics.CardChoiceMetric>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Runs.Metrics.CardChoiceMetric>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.Metrics.CardChoiceMetric>> Create_ListCardChoiceMetric(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.Metrics.CardChoiceMetric>> jsonTypeInfo;
    if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Runs.Metrics.CardChoiceMetric>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Runs.Metrics.CardChoiceMetric>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Runs.Metrics.CardChoiceMetric>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Runs.Metrics.CardChoiceMetric>>) (() => new List<MegaCrit.Sts2.Core.Runs.Metrics.CardChoiceMetric>()),
        SerializeHandler = new Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Runs.Metrics.CardChoiceMetric>>(this.ListCardChoiceMetricSerializeHandler)
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Runs.Metrics.CardChoiceMetric>, MegaCrit.Sts2.Core.Runs.Metrics.CardChoiceMetric>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private void ListCardChoiceMetricSerializeHandler(
    Utf8JsonWriter writer,
    List<MegaCrit.Sts2.Core.Runs.Metrics.CardChoiceMetric>? value)
  {
    if (value == null)
    {
      writer.WriteNullValue();
    }
    else
    {
      writer.WriteStartArray();
      for (int index = 0; index < value.Count; ++index)
        this.CardChoiceMetricSerializeHandler(writer, value[index]);
      writer.WriteEndArray();
    }
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric>> ListEncounterMetric
  {
    get
    {
      return this._ListEncounterMetric ?? (this._ListEncounterMetric = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric>> Create_ListEncounterMetric(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric>> jsonTypeInfo;
    if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric>>) (() => new List<MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric>()),
        SerializeHandler = new Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric>>(this.ListEncounterMetricSerializeHandler)
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric>, MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private void ListEncounterMetricSerializeHandler(
    Utf8JsonWriter writer,
    List<MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric>? value)
  {
    if (value == null)
    {
      writer.WriteNullValue();
    }
    else
    {
      writer.WriteStartArray();
      for (int index = 0; index < value.Count; ++index)
        this.EncounterMetricSerializeHandler(writer, value[index]);
      writer.WriteEndArray();
    }
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric>> ListEventChoiceMetric
  {
    get
    {
      return this._ListEventChoiceMetric ?? (this._ListEventChoiceMetric = (JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric>> Create_ListEventChoiceMetric(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric>> jsonTypeInfo;
    if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric>>) (() => new List<MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric>()),
        SerializeHandler = new Action<Utf8JsonWriter, List<MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric>>(this.ListEventChoiceMetricSerializeHandler)
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric>, MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private void ListEventChoiceMetricSerializeHandler(
    Utf8JsonWriter writer,
    List<MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric>? value)
  {
    if (value == null)
    {
      writer.WriteNullValue();
    }
    else
    {
      writer.WriteStartArray();
      for (int index = 0; index < value.Count; ++index)
        this.EventChoiceMetricSerializeHandler(writer, value[index]);
      writer.WriteEndArray();
    }
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
    if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<string>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<string>> collectionInfoValues = new JsonCollectionInfoValues<List<string>>()
      {
        ObjectCreator = (Func<List<string>>) (() => new List<string>()),
        SerializeHandler = new Action<Utf8JsonWriter, List<string>>(this.ListStringSerializeHandler)
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<string>, string>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private void ListStringSerializeHandler(Utf8JsonWriter writer, List<string>? value)
  {
    if (value == null)
    {
      writer.WriteNullValue();
    }
    else
    {
      writer.WriteStartArray();
      for (int index = 0; index < value.Count; ++index)
        writer.WriteStringValue(value[index]);
      writer.WriteEndArray();
    }
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
    if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<int>(options, out jsonTypeInfo))
      jsonTypeInfo = JsonMetadataServices.CreateValueInfo<int>(options, (JsonConverter) JsonMetadataServices.Int32Converter);
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
    if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<long>(options, out jsonTypeInfo))
      jsonTypeInfo = JsonMetadataServices.CreateValueInfo<long>(options, (JsonConverter) JsonMetadataServices.Int64Converter);
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
    if (!MetricsSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<string>(options, out jsonTypeInfo))
      jsonTypeInfo = JsonMetadataServices.CreateValueInfo<string>(options, (JsonConverter) JsonMetadataServices.StringConverter);
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public static MetricsSerializerContext Default { get; }

  protected override JsonSerializerOptions? GeneratedSerializerOptions { get; } = MetricsSerializerContext.s_defaultOptions;

  public MetricsSerializerContext()
    : base((JsonSerializerOptions) null)
  {
  }

  public MetricsSerializerContext(JsonSerializerOptions options)
    : base(options)
  {
  }

  private static bool TryGetTypeInfoForRuntimeCustomConverter<TJsonMetadataType>(
    JsonSerializerOptions options,
    out JsonTypeInfo<TJsonMetadataType> jsonTypeInfo)
  {
    JsonConverter converterForType = MetricsSerializerContext.GetRuntimeConverterForType(typeof (TJsonMetadataType), options);
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
        return MetricsSerializerContext.ExpandConverter(type, converter, options, false);
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
    if (type == typeof (float))
      return (JsonTypeInfo) this.Create_Single(options);
    if (type == typeof (Godot.Vector2I))
      return (JsonTypeInfo) this.Create_Vector2I(options);
    if (type == typeof (MegaCrit.Sts2.Core.Achievements.AchievementMetric))
      return (JsonTypeInfo) this.Create_AchievementMetric(options);
    if (type == typeof (MegaCrit.Sts2.Core.Models.ModelId))
      return (JsonTypeInfo) this.Create_ModelId(options);
    if (type == typeof (MegaCrit.Sts2.Core.Runs.Metrics.ActWinMetric))
      return (JsonTypeInfo) this.Create_ActWinMetric(options);
    if (type == typeof (MegaCrit.Sts2.Core.Runs.Metrics.AncientMetric))
      return (JsonTypeInfo) this.Create_AncientMetric(options);
    if (type == typeof (MegaCrit.Sts2.Core.Runs.Metrics.CardChoiceMetric))
      return (JsonTypeInfo) this.Create_CardChoiceMetric(options);
    if (type == typeof (MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric))
      return (JsonTypeInfo) this.Create_EncounterMetric(options);
    if (type == typeof (MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric))
      return (JsonTypeInfo) this.Create_EventChoiceMetric(options);
    if (type == typeof (MegaCrit.Sts2.Core.Runs.Metrics.RunMetrics))
      return (JsonTypeInfo) this.Create_RunMetrics(options);
    if (type == typeof (MegaCrit.Sts2.Core.Runs.Metrics.SettingsDataMetric))
      return (JsonTypeInfo) this.Create_SettingsDataMetric(options);
    if (type == typeof (MegaCrit.Sts2.Core.Settings.AspectRatioSetting))
      return (JsonTypeInfo) this.Create_AspectRatioSetting(options);
    if (type == typeof (MegaCrit.Sts2.Core.Settings.FastModeType))
      return (JsonTypeInfo) this.Create_FastModeType(options);
    if (type == typeof (MegaCrit.Sts2.Core.Settings.VSyncType))
      return (JsonTypeInfo) this.Create_VSyncType(options);
    if (type == typeof (MegaCrit.Sts2.Core.Timeline.EpochMetric))
      return (JsonTypeInfo) this.Create_EpochMetric(options);
    if (type == typeof (IEnumerable<MegaCrit.Sts2.Core.Models.ModelId>))
      return (JsonTypeInfo) this.Create_IEnumerableModelId(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Models.ModelId>))
      return (JsonTypeInfo) this.Create_ListModelId(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Runs.Metrics.ActWinMetric>))
      return (JsonTypeInfo) this.Create_ListActWinMetric(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Runs.Metrics.AncientMetric>))
      return (JsonTypeInfo) this.Create_ListAncientMetric(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Runs.Metrics.CardChoiceMetric>))
      return (JsonTypeInfo) this.Create_ListCardChoiceMetric(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Runs.Metrics.EncounterMetric>))
      return (JsonTypeInfo) this.Create_ListEncounterMetric(options);
    if (type == typeof (List<MegaCrit.Sts2.Core.Runs.Metrics.EventChoiceMetric>))
      return (JsonTypeInfo) this.Create_ListEventChoiceMetric(options);
    if (type == typeof (List<string>))
      return (JsonTypeInfo) this.Create_ListString(options);
    if (type == typeof (int))
      return (JsonTypeInfo) this.Create_Int32(options);
    if (type == typeof (long))
      return (JsonTypeInfo) this.Create_Int64(options);
    return type == typeof (string) ? (JsonTypeInfo) this.Create_String(options) : (JsonTypeInfo) null;
  }

  static MetricsSerializerContext()
  {
    JsonSerializerOptions serializerOptions = new JsonSerializerOptions();
    serializerOptions.Converters.Add((JsonConverter) new ModelIdMetricsConverter());
    serializerOptions.IncludeFields = true;
    serializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    serializerOptions.WriteIndented = true;
    MetricsSerializerContext.s_defaultOptions = serializerOptions;
    MetricsSerializerContext.Default = new MetricsSerializerContext(new JsonSerializerOptions(MetricsSerializerContext.s_defaultOptions));
    MetricsSerializerContext.PropName_x = JsonEncodedText.Encode("x", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_y = JsonEncodedText.Encode("y", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_buildId = JsonEncodedText.Encode("buildId", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_achievement = JsonEncodedText.Encode("achievement", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_totalAchievements = JsonEncodedText.Encode("totalAchievements", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_totalPlaytime = JsonEncodedText.Encode("totalPlaytime", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_totalRuns = JsonEncodedText.Encode("totalRuns", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_category = JsonEncodedText.Encode("category", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_entry = JsonEncodedText.Encode("entry", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_act = JsonEncodedText.Encode("act", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_win = JsonEncodedText.Encode("win", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_picked = JsonEncodedText.Encode("picked", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_skipped = JsonEncodedText.Encode("skipped", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_id = JsonEncodedText.Encode("id", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_damage = JsonEncodedText.Encode("damage", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_turns = JsonEncodedText.Encode("turns", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_playerId = JsonEncodedText.Encode("playerId", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_character = JsonEncodedText.Encode("character", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_numPlayers = JsonEncodedText.Encode("numPlayers", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_team = JsonEncodedText.Encode("team", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_buildType = JsonEncodedText.Encode("buildType", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_ascension = JsonEncodedText.Encode("ascension", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_totalWinRate = JsonEncodedText.Encode("totalWinRate", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_numReloads = JsonEncodedText.Encode("numReloads", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_runPlaytime = JsonEncodedText.Encode("runPlaytime", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_floorReached = JsonEncodedText.Encode("floorReached", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_killedByEncounter = JsonEncodedText.Encode("killedByEncounter", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_cardChoices = JsonEncodedText.Encode("cardChoices", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_campfireUpgrades = JsonEncodedText.Encode("campfireUpgrades", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_eventChoices = JsonEncodedText.Encode("eventChoices", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_ancientChoices = JsonEncodedText.Encode("ancientChoices", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_relicBuys = JsonEncodedText.Encode("relicBuys", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_potionBuys = JsonEncodedText.Encode("potionBuys", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_colorlessBuys = JsonEncodedText.Encode("colorlessBuys", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_potionDiscards = JsonEncodedText.Encode("potionDiscards", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_encounters = JsonEncodedText.Encode("encounters", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_actWins = JsonEncodedText.Encode("actWins", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_deck = JsonEncodedText.Encode("deck", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_relics = JsonEncodedText.Encode("relics", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_os = JsonEncodedText.Encode("os", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_platform = JsonEncodedText.Encode("platform", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_systemRam = JsonEncodedText.Encode("systemRam", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_language = JsonEncodedText.Encode("language", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_combatSpeed = JsonEncodedText.Encode("combatSpeed", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_screenshake = JsonEncodedText.Encode("screenshake", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_runTimer = JsonEncodedText.Encode("runTimer", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_phobiaMode = JsonEncodedText.Encode("phobiaMode", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_cardIndices = JsonEncodedText.Encode("cardIndices", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_displayCount = JsonEncodedText.Encode("displayCount", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_displayResolution = JsonEncodedText.Encode("displayResolution", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_fullscreen = JsonEncodedText.Encode("fullscreen", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_aspectRatio = JsonEncodedText.Encode("aspectRatio", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_resizeWindows = JsonEncodedText.Encode("resizeWindows", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_vSync = JsonEncodedText.Encode("vSync", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_fpsLimit = JsonEncodedText.Encode("fpsLimit", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_msaa = JsonEncodedText.Encode("msaa", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_epoch = JsonEncodedText.Encode("epoch", (JavaScriptEncoder) null);
    MetricsSerializerContext.PropName_totalEpochs = JsonEncodedText.Encode("totalEpochs", (JavaScriptEncoder) null);
  }
}
