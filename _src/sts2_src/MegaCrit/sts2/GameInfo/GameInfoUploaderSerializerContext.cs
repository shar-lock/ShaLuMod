// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.GameInfo.GameInfoUploaderSerializerContext
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

#nullable enable
namespace MegaCrit.Sts2.GameInfo;

[JsonSourceGenerationOptions]
[JsonSerializable(typeof (List<MegaCrit.Sts2.GameInfo.Objects.IGameInfo>))]
[GeneratedCode("System.Text.Json.SourceGeneration", "9.0.12.31616")]
internal class GameInfoUploaderSerializerContext : JsonSerializerContext, IJsonTypeInfoResolver
{
  private JsonTypeInfo<bool>? _Boolean;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Models.ModelId>? _ModelId;
  private JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.AncientChoiceInfo>? _AncientChoiceInfo;
  private JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.CardInfo>? _CardInfo;
  private JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.DailyMods>? _DailyMods;
  private JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.EnchantmentInfo>? _EnchantmentInfo;
  private JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.EncounterInfo>? _EncounterInfo;
  private JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.EventInfo>? _EventInfo;
  private JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.IGameInfo>? _IGameInfo;
  private JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.Keywords>? _Keywords;
  private JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.NeowBonusInfo>? _NeowBonusInfo;
  private JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.PotionInfo>? _PotionInfo;
  private JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.RelicInfo>? _RelicInfo;
  private JsonTypeInfo<List<MegaCrit.Sts2.GameInfo.Objects.IGameInfo>>? _ListIGameInfo;
  private JsonTypeInfo<List<string>>? _ListString;
  private JsonTypeInfo<int>? _Int32;
  private JsonTypeInfo<string>? _String;
  private static readonly JsonSerializerOptions s_defaultOptions;
  private const BindingFlags InstanceMemberBindingFlags = (BindingFlags) 52;
  private static readonly JsonEncodedText PropName_category;
  private static readonly JsonEncodedText PropName_entry;
  private static readonly JsonEncodedText PropName_name;
  private static readonly JsonEncodedText PropName_bot_keyword;
  private static readonly JsonEncodedText PropName_bot_text;
  private static readonly JsonEncodedText PropName_id;
  private static readonly JsonEncodedText PropName_text;
  private static readonly JsonEncodedText PropName_ancient;
  private static readonly JsonEncodedText PropName_upgraded;
  private static readonly JsonEncodedText PropName_color;
  private static readonly JsonEncodedText PropName_rarity;
  private static readonly JsonEncodedText PropName_type;
  private static readonly JsonEncodedText PropName_base_damage;
  private static readonly JsonEncodedText PropName_energy;
  private static readonly JsonEncodedText PropName_star_cost;
  private static readonly JsonEncodedText PropName_has_art;
  private static readonly JsonEncodedText PropName_has_joke_art;
  private static readonly JsonEncodedText PropName_act;
  private static readonly JsonEncodedText PropName_tier;
  private static readonly JsonEncodedText PropName_options;

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
    if (!GameInfoUploaderSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<bool>(options, out jsonTypeInfo))
      jsonTypeInfo = JsonMetadataServices.CreateValueInfo<bool>(options, (JsonConverter) JsonMetadataServices.BooleanConverter);
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
    if (!GameInfoUploaderSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Models.ModelId>(options, out jsonTypeInfo))
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Models.ModelId> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Models.ModelId>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Models.ModelId>) null,
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Models.ModelId>) (args => new MegaCrit.Sts2.Core.Models.ModelId((string) args[0], (string) args[1])),
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => GameInfoUploaderSerializerContext.ModelIdPropInit(options)),
        ConstructorParameterMetadataInitializer = GameInfoUploaderSerializerContext.\u003C\u003EO.\u003C0\u003E__ModelIdCtorParamInit ?? (GameInfoUploaderSerializerContext.\u003C\u003EO.\u003C0\u003E__ModelIdCtorParamInit = new Func<JsonParameterInfoValues[]>(GameInfoUploaderSerializerContext.ModelIdCtorParamInit)),
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
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_category, value.Category);
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_entry, value.Entry);
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
  JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.AncientChoiceInfo> AncientChoiceInfo
  {
    get
    {
      return this._AncientChoiceInfo ?? (this._AncientChoiceInfo = (JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.AncientChoiceInfo>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.GameInfo.Objects.AncientChoiceInfo)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.AncientChoiceInfo> Create_AncientChoiceInfo(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.AncientChoiceInfo> jsonTypeInfo;
    if (!GameInfoUploaderSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.GameInfo.Objects.AncientChoiceInfo>(options, out jsonTypeInfo))
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      JsonObjectInfoValues<MegaCrit.Sts2.GameInfo.Objects.AncientChoiceInfo> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.GameInfo.Objects.AncientChoiceInfo>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.GameInfo.Objects.AncientChoiceInfo>) null,
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.GameInfo.Objects.AncientChoiceInfo>) (args => new MegaCrit.Sts2.GameInfo.Objects.AncientChoiceInfo()
        {
          Name = (string) args[0],
          BotKeyword = (string) args[1],
          BotText = (string) args[2],
          Id = (string) args[3],
          Text = (string) args[4],
          Ancient = (string) args[5]
        }),
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => GameInfoUploaderSerializerContext.AncientChoiceInfoPropInit(options)),
        ConstructorParameterMetadataInitializer = GameInfoUploaderSerializerContext.\u003C\u003EO.\u003C1\u003E__AncientChoiceInfoCtorParamInit ?? (GameInfoUploaderSerializerContext.\u003C\u003EO.\u003C1\u003E__AncientChoiceInfoCtorParamInit = new Func<JsonParameterInfoValues[]>(GameInfoUploaderSerializerContext.AncientChoiceInfoCtorParamInit)),
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.AncientChoiceInfo).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = new Action<Utf8JsonWriter, MegaCrit.Sts2.GameInfo.Objects.AncientChoiceInfo>(this.AncientChoiceInfoSerializeHandler)
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.GameInfo.Objects.AncientChoiceInfo>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] AncientChoiceInfoPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[6];
    JsonPropertyInfoValues<string> propertyInfoValues1 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.AncientChoiceInfo),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.AncientChoiceInfo) obj).Name),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Name",
      JsonPropertyName = "name",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.AncientChoiceInfo).GetProperty("Name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
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
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.AncientChoiceInfo),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.AncientChoiceInfo) obj).BotKeyword),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "BotKeyword",
      JsonPropertyName = "bot_keyword",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.AncientChoiceInfo).GetProperty("BotKeyword", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues2);
    jsonPropertyInfoArray[1].IsRequired = true;
    jsonPropertyInfoArray[1].IsGetNullable = false;
    jsonPropertyInfoArray[1].IsSetNullable = false;
    JsonPropertyInfoValues<string> propertyInfoValues3 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.AncientChoiceInfo),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.AncientChoiceInfo) obj).BotText),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "BotText",
      JsonPropertyName = "bot_text",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.AncientChoiceInfo).GetProperty("BotText", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues3);
    jsonPropertyInfoArray[2].IsRequired = true;
    jsonPropertyInfoArray[2].IsGetNullable = false;
    jsonPropertyInfoArray[2].IsSetNullable = false;
    JsonPropertyInfoValues<string> propertyInfoValues4 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.AncientChoiceInfo),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.AncientChoiceInfo) obj).Id),
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
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.AncientChoiceInfo).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[3] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues4);
    jsonPropertyInfoArray[3].IsRequired = true;
    jsonPropertyInfoArray[3].IsGetNullable = false;
    jsonPropertyInfoArray[3].IsSetNullable = false;
    JsonPropertyInfoValues<string> propertyInfoValues5 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.AncientChoiceInfo),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.AncientChoiceInfo) obj).Text),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Text",
      JsonPropertyName = "text",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.AncientChoiceInfo).GetProperty("Text", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[4] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues5);
    jsonPropertyInfoArray[4].IsRequired = true;
    jsonPropertyInfoArray[4].IsGetNullable = false;
    jsonPropertyInfoArray[4].IsSetNullable = false;
    JsonPropertyInfoValues<string> propertyInfoValues6 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.AncientChoiceInfo),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.AncientChoiceInfo) obj).Ancient),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Ancient",
      JsonPropertyName = "ancient",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.AncientChoiceInfo).GetProperty("Ancient", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[5] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues6);
    jsonPropertyInfoArray[5].IsRequired = true;
    jsonPropertyInfoArray[5].IsGetNullable = false;
    jsonPropertyInfoArray[5].IsSetNullable = false;
    return jsonPropertyInfoArray;
  }

  private void AncientChoiceInfoSerializeHandler(Utf8JsonWriter writer, MegaCrit.Sts2.GameInfo.Objects.AncientChoiceInfo? value)
  {
    if (value == null)
    {
      writer.WriteNullValue();
    }
    else
    {
      writer.WriteStartObject();
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_name, value.Name);
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_bot_keyword, value.BotKeyword);
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_bot_text, value.BotText);
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_id, value.Id);
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_text, value.Text);
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_ancient, value.Ancient);
      writer.WriteEndObject();
    }
  }

  private static JsonParameterInfoValues[] AncientChoiceInfoCtorParamInit()
  {
    return new JsonParameterInfoValues[6]
    {
      new JsonParameterInfoValues()
      {
        Name = "Name",
        ParameterType = typeof (string),
        Position = 0,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "BotKeyword",
        ParameterType = typeof (string),
        Position = 1,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "BotText",
        ParameterType = typeof (string),
        Position = 2,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Id",
        ParameterType = typeof (string),
        Position = 3,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Text",
        ParameterType = typeof (string),
        Position = 4,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Ancient",
        ParameterType = typeof (string),
        Position = 5,
        IsNullable = false,
        IsMemberInitializer = true
      }
    };
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.CardInfo> CardInfo
  {
    get
    {
      return this._CardInfo ?? (this._CardInfo = (JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.CardInfo>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.GameInfo.Objects.CardInfo)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.CardInfo> Create_CardInfo(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.CardInfo> jsonTypeInfo;
    if (!GameInfoUploaderSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.GameInfo.Objects.CardInfo>(options, out jsonTypeInfo))
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      JsonObjectInfoValues<MegaCrit.Sts2.GameInfo.Objects.CardInfo> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.GameInfo.Objects.CardInfo>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.GameInfo.Objects.CardInfo>) null,
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.GameInfo.Objects.CardInfo>) (args => new MegaCrit.Sts2.GameInfo.Objects.CardInfo()
        {
          Name = (string) args[0],
          BotKeyword = (string) args[1],
          BotText = (string) args[2],
          Id = (MegaCrit.Sts2.Core.Models.ModelId) args[3],
          Upgraded = (bool) args[4],
          Color = (string) args[5],
          Rarity = (string) args[6],
          Type = (string) args[7],
          BaseDamage = (int) args[8],
          Energy = (int) args[9],
          StarCost = (int) args[10],
          Text = (string) args[11],
          HasArt = (bool) args[12],
          HasJokeArt = (bool) args[13]
        }),
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => GameInfoUploaderSerializerContext.CardInfoPropInit(options)),
        ConstructorParameterMetadataInitializer = GameInfoUploaderSerializerContext.\u003C\u003EO.\u003C2\u003E__CardInfoCtorParamInit ?? (GameInfoUploaderSerializerContext.\u003C\u003EO.\u003C2\u003E__CardInfoCtorParamInit = new Func<JsonParameterInfoValues[]>(GameInfoUploaderSerializerContext.CardInfoCtorParamInit)),
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.CardInfo).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = new Action<Utf8JsonWriter, MegaCrit.Sts2.GameInfo.Objects.CardInfo>(this.CardInfoSerializeHandler)
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.GameInfo.Objects.CardInfo>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] CardInfoPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[14];
    JsonPropertyInfoValues<string> propertyInfoValues1 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.CardInfo),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.CardInfo) obj).Name),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Name",
      JsonPropertyName = "name",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.CardInfo).GetProperty("Name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
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
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.CardInfo),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.CardInfo) obj).BotKeyword),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "BotKeyword",
      JsonPropertyName = "bot_keyword",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.CardInfo).GetProperty("BotKeyword", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues2);
    jsonPropertyInfoArray[1].IsRequired = true;
    jsonPropertyInfoArray[1].IsGetNullable = false;
    jsonPropertyInfoArray[1].IsSetNullable = false;
    JsonPropertyInfoValues<string> propertyInfoValues3 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.CardInfo),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.CardInfo) obj).BotText),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "BotText",
      JsonPropertyName = "bot_text",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.CardInfo).GetProperty("BotText", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues3);
    jsonPropertyInfoArray[2].IsRequired = true;
    jsonPropertyInfoArray[2].IsGetNullable = false;
    jsonPropertyInfoArray[2].IsSetNullable = false;
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId> propertyInfoValues4 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.CardInfo),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Models.ModelId>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Models.ModelId>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.CardInfo) obj).Id),
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
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.CardInfo).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Models.ModelId), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[3] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Models.ModelId>(options, propertyInfoValues4);
    jsonPropertyInfoArray[3].IsRequired = true;
    jsonPropertyInfoArray[3].IsGetNullable = false;
    jsonPropertyInfoArray[3].IsSetNullable = false;
    JsonPropertyInfoValues<bool> propertyInfoValues5 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.CardInfo),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.CardInfo) obj).Upgraded),
      Setter = (Action<object, bool>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Upgraded",
      JsonPropertyName = "upgraded",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.CardInfo).GetProperty("Upgraded", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (bool), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[4] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues5);
    jsonPropertyInfoArray[4].IsRequired = true;
    JsonPropertyInfoValues<string> propertyInfoValues6 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.CardInfo),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.CardInfo) obj).Color),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Color",
      JsonPropertyName = "color",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.CardInfo).GetProperty("Color", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[5] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues6);
    jsonPropertyInfoArray[5].IsRequired = true;
    jsonPropertyInfoArray[5].IsGetNullable = false;
    jsonPropertyInfoArray[5].IsSetNullable = false;
    JsonPropertyInfoValues<string> propertyInfoValues7 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.CardInfo),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.CardInfo) obj).Rarity),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Rarity",
      JsonPropertyName = "rarity",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.CardInfo).GetProperty("Rarity", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[6] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues7);
    jsonPropertyInfoArray[6].IsRequired = true;
    jsonPropertyInfoArray[6].IsGetNullable = false;
    jsonPropertyInfoArray[6].IsSetNullable = false;
    JsonPropertyInfoValues<string> propertyInfoValues8 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.CardInfo),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.CardInfo) obj).Type),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Type",
      JsonPropertyName = "type",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.CardInfo).GetProperty("Type", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[7] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues8);
    jsonPropertyInfoArray[7].IsRequired = true;
    jsonPropertyInfoArray[7].IsGetNullable = false;
    jsonPropertyInfoArray[7].IsSetNullable = false;
    JsonPropertyInfoValues<int> propertyInfoValues9 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.CardInfo),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.CardInfo) obj).BaseDamage),
      Setter = (Action<object, int>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "BaseDamage",
      JsonPropertyName = "base_damage",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.CardInfo).GetProperty("BaseDamage", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[8] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues9);
    jsonPropertyInfoArray[8].IsRequired = true;
    JsonPropertyInfoValues<int> propertyInfoValues10 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.CardInfo),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.CardInfo) obj).Energy),
      Setter = (Action<object, int>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Energy",
      JsonPropertyName = "energy",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.CardInfo).GetProperty("Energy", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[9] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues10);
    jsonPropertyInfoArray[9].IsRequired = true;
    JsonPropertyInfoValues<int> propertyInfoValues11 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.CardInfo),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.CardInfo) obj).StarCost),
      Setter = (Action<object, int>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "StarCost",
      JsonPropertyName = "star_cost",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.CardInfo).GetProperty("StarCost", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[10] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues11);
    jsonPropertyInfoArray[10].IsRequired = true;
    JsonPropertyInfoValues<string> propertyInfoValues12 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.CardInfo),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.CardInfo) obj).Text),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Text",
      JsonPropertyName = "text",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.CardInfo).GetProperty("Text", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[11] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues12);
    jsonPropertyInfoArray[11].IsRequired = true;
    jsonPropertyInfoArray[11].IsGetNullable = false;
    jsonPropertyInfoArray[11].IsSetNullable = false;
    JsonPropertyInfoValues<bool> propertyInfoValues13 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.CardInfo),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.CardInfo) obj).HasArt),
      Setter = (Action<object, bool>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "HasArt",
      JsonPropertyName = "has_art",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.CardInfo).GetProperty("HasArt", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (bool), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[12] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues13);
    jsonPropertyInfoArray[12].IsRequired = true;
    JsonPropertyInfoValues<bool> propertyInfoValues14 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.CardInfo),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.CardInfo) obj).HasJokeArt),
      Setter = (Action<object, bool>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "HasJokeArt",
      JsonPropertyName = "has_joke_art",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.CardInfo).GetProperty("HasJokeArt", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (bool), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[13] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues14);
    jsonPropertyInfoArray[13].IsRequired = true;
    return jsonPropertyInfoArray;
  }

  private void CardInfoSerializeHandler(Utf8JsonWriter writer, MegaCrit.Sts2.GameInfo.Objects.CardInfo? value)
  {
    if (value == null)
    {
      writer.WriteNullValue();
    }
    else
    {
      writer.WriteStartObject();
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_name, value.Name);
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_bot_keyword, value.BotKeyword);
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_bot_text, value.BotText);
      writer.WritePropertyName(GameInfoUploaderSerializerContext.PropName_id);
      this.ModelIdSerializeHandler(writer, value.Id);
      writer.WriteBoolean(GameInfoUploaderSerializerContext.PropName_upgraded, value.Upgraded);
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_color, value.Color);
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_rarity, value.Rarity);
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_type, value.Type);
      writer.WriteNumber(GameInfoUploaderSerializerContext.PropName_base_damage, value.BaseDamage);
      writer.WriteNumber(GameInfoUploaderSerializerContext.PropName_energy, value.Energy);
      writer.WriteNumber(GameInfoUploaderSerializerContext.PropName_star_cost, value.StarCost);
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_text, value.Text);
      writer.WriteBoolean(GameInfoUploaderSerializerContext.PropName_has_art, value.HasArt);
      writer.WriteBoolean(GameInfoUploaderSerializerContext.PropName_has_joke_art, value.HasJokeArt);
      writer.WriteEndObject();
    }
  }

  private static JsonParameterInfoValues[] CardInfoCtorParamInit()
  {
    return new JsonParameterInfoValues[14]
    {
      new JsonParameterInfoValues()
      {
        Name = "Name",
        ParameterType = typeof (string),
        Position = 0,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "BotKeyword",
        ParameterType = typeof (string),
        Position = 1,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "BotText",
        ParameterType = typeof (string),
        Position = 2,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Id",
        ParameterType = typeof (MegaCrit.Sts2.Core.Models.ModelId),
        Position = 3,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Upgraded",
        ParameterType = typeof (bool),
        Position = 4,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Color",
        ParameterType = typeof (string),
        Position = 5,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Rarity",
        ParameterType = typeof (string),
        Position = 6,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Type",
        ParameterType = typeof (string),
        Position = 7,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "BaseDamage",
        ParameterType = typeof (int),
        Position = 8,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Energy",
        ParameterType = typeof (int),
        Position = 9,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "StarCost",
        ParameterType = typeof (int),
        Position = 10,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Text",
        ParameterType = typeof (string),
        Position = 11,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "HasArt",
        ParameterType = typeof (bool),
        Position = 12,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "HasJokeArt",
        ParameterType = typeof (bool),
        Position = 13,
        IsNullable = false,
        IsMemberInitializer = true
      }
    };
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.DailyMods> DailyMods
  {
    get
    {
      return this._DailyMods ?? (this._DailyMods = (JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.DailyMods>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.GameInfo.Objects.DailyMods)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.DailyMods> Create_DailyMods(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.DailyMods> jsonTypeInfo;
    if (!GameInfoUploaderSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.GameInfo.Objects.DailyMods>(options, out jsonTypeInfo))
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      JsonObjectInfoValues<MegaCrit.Sts2.GameInfo.Objects.DailyMods> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.GameInfo.Objects.DailyMods>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.GameInfo.Objects.DailyMods>) null,
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.GameInfo.Objects.DailyMods>) (args => new MegaCrit.Sts2.GameInfo.Objects.DailyMods()
        {
          Name = (string) args[0],
          BotKeyword = (string) args[1],
          BotText = (string) args[2],
          Text = (string) args[3]
        }),
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => GameInfoUploaderSerializerContext.DailyModsPropInit(options)),
        ConstructorParameterMetadataInitializer = GameInfoUploaderSerializerContext.\u003C\u003EO.\u003C3\u003E__DailyModsCtorParamInit ?? (GameInfoUploaderSerializerContext.\u003C\u003EO.\u003C3\u003E__DailyModsCtorParamInit = new Func<JsonParameterInfoValues[]>(GameInfoUploaderSerializerContext.DailyModsCtorParamInit)),
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.DailyMods).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = new Action<Utf8JsonWriter, MegaCrit.Sts2.GameInfo.Objects.DailyMods>(this.DailyModsSerializeHandler)
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.GameInfo.Objects.DailyMods>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] DailyModsPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[4];
    JsonPropertyInfoValues<string> propertyInfoValues1 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.DailyMods),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.DailyMods) obj).Name),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Name",
      JsonPropertyName = "name",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.DailyMods).GetProperty("Name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
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
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.DailyMods),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.DailyMods) obj).BotKeyword),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "BotKeyword",
      JsonPropertyName = "bot_keyword",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.DailyMods).GetProperty("BotKeyword", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues2);
    jsonPropertyInfoArray[1].IsRequired = true;
    jsonPropertyInfoArray[1].IsGetNullable = false;
    jsonPropertyInfoArray[1].IsSetNullable = false;
    JsonPropertyInfoValues<string> propertyInfoValues3 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.DailyMods),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.DailyMods) obj).BotText),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "BotText",
      JsonPropertyName = "bot_text",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.DailyMods).GetProperty("BotText", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues3);
    jsonPropertyInfoArray[2].IsRequired = true;
    jsonPropertyInfoArray[2].IsGetNullable = false;
    jsonPropertyInfoArray[2].IsSetNullable = false;
    JsonPropertyInfoValues<string> propertyInfoValues4 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.DailyMods),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.DailyMods) obj).Text),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Text",
      JsonPropertyName = "text",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.DailyMods).GetProperty("Text", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[3] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues4);
    jsonPropertyInfoArray[3].IsRequired = true;
    jsonPropertyInfoArray[3].IsGetNullable = false;
    jsonPropertyInfoArray[3].IsSetNullable = false;
    return jsonPropertyInfoArray;
  }

  private void DailyModsSerializeHandler(Utf8JsonWriter writer, MegaCrit.Sts2.GameInfo.Objects.DailyMods? value)
  {
    if (value == null)
    {
      writer.WriteNullValue();
    }
    else
    {
      writer.WriteStartObject();
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_name, value.Name);
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_bot_keyword, value.BotKeyword);
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_bot_text, value.BotText);
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_text, value.Text);
      writer.WriteEndObject();
    }
  }

  private static JsonParameterInfoValues[] DailyModsCtorParamInit()
  {
    return new JsonParameterInfoValues[4]
    {
      new JsonParameterInfoValues()
      {
        Name = "Name",
        ParameterType = typeof (string),
        Position = 0,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "BotKeyword",
        ParameterType = typeof (string),
        Position = 1,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "BotText",
        ParameterType = typeof (string),
        Position = 2,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Text",
        ParameterType = typeof (string),
        Position = 3,
        IsNullable = false,
        IsMemberInitializer = true
      }
    };
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.EnchantmentInfo> EnchantmentInfo
  {
    get
    {
      return this._EnchantmentInfo ?? (this._EnchantmentInfo = (JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.EnchantmentInfo>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.GameInfo.Objects.EnchantmentInfo)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.EnchantmentInfo> Create_EnchantmentInfo(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.EnchantmentInfo> jsonTypeInfo;
    if (!GameInfoUploaderSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.GameInfo.Objects.EnchantmentInfo>(options, out jsonTypeInfo))
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      JsonObjectInfoValues<MegaCrit.Sts2.GameInfo.Objects.EnchantmentInfo> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.GameInfo.Objects.EnchantmentInfo>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.GameInfo.Objects.EnchantmentInfo>) null,
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.GameInfo.Objects.EnchantmentInfo>) (args => new MegaCrit.Sts2.GameInfo.Objects.EnchantmentInfo()
        {
          Name = (string) args[0],
          BotKeyword = (string) args[1],
          BotText = (string) args[2],
          Id = (MegaCrit.Sts2.Core.Models.ModelId) args[3],
          Text = (string) args[4]
        }),
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => GameInfoUploaderSerializerContext.EnchantmentInfoPropInit(options)),
        ConstructorParameterMetadataInitializer = GameInfoUploaderSerializerContext.\u003C\u003EO.\u003C4\u003E__EnchantmentInfoCtorParamInit ?? (GameInfoUploaderSerializerContext.\u003C\u003EO.\u003C4\u003E__EnchantmentInfoCtorParamInit = new Func<JsonParameterInfoValues[]>(GameInfoUploaderSerializerContext.EnchantmentInfoCtorParamInit)),
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.EnchantmentInfo).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = new Action<Utf8JsonWriter, MegaCrit.Sts2.GameInfo.Objects.EnchantmentInfo>(this.EnchantmentInfoSerializeHandler)
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.GameInfo.Objects.EnchantmentInfo>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] EnchantmentInfoPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[5];
    JsonPropertyInfoValues<string> propertyInfoValues1 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.EnchantmentInfo),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.EnchantmentInfo) obj).Name),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Name",
      JsonPropertyName = "name",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.EnchantmentInfo).GetProperty("Name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
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
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.EnchantmentInfo),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.EnchantmentInfo) obj).BotKeyword),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "BotKeyword",
      JsonPropertyName = "bot_keyword",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.EnchantmentInfo).GetProperty("BotKeyword", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues2);
    jsonPropertyInfoArray[1].IsRequired = true;
    jsonPropertyInfoArray[1].IsGetNullable = false;
    jsonPropertyInfoArray[1].IsSetNullable = false;
    JsonPropertyInfoValues<string> propertyInfoValues3 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.EnchantmentInfo),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.EnchantmentInfo) obj).BotText),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "BotText",
      JsonPropertyName = "bot_text",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.EnchantmentInfo).GetProperty("BotText", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues3);
    jsonPropertyInfoArray[2].IsRequired = true;
    jsonPropertyInfoArray[2].IsGetNullable = false;
    jsonPropertyInfoArray[2].IsSetNullable = false;
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId> propertyInfoValues4 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.EnchantmentInfo),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Models.ModelId>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Models.ModelId>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.EnchantmentInfo) obj).Id),
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
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.EnchantmentInfo).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Models.ModelId), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[3] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Models.ModelId>(options, propertyInfoValues4);
    jsonPropertyInfoArray[3].IsRequired = true;
    jsonPropertyInfoArray[3].IsGetNullable = false;
    jsonPropertyInfoArray[3].IsSetNullable = false;
    JsonPropertyInfoValues<string> propertyInfoValues5 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.EnchantmentInfo),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.EnchantmentInfo) obj).Text),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Text",
      JsonPropertyName = "text",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.EnchantmentInfo).GetProperty("Text", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[4] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues5);
    jsonPropertyInfoArray[4].IsRequired = true;
    jsonPropertyInfoArray[4].IsGetNullable = false;
    jsonPropertyInfoArray[4].IsSetNullable = false;
    return jsonPropertyInfoArray;
  }

  private void EnchantmentInfoSerializeHandler(Utf8JsonWriter writer, MegaCrit.Sts2.GameInfo.Objects.EnchantmentInfo? value)
  {
    if (value == null)
    {
      writer.WriteNullValue();
    }
    else
    {
      writer.WriteStartObject();
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_name, value.Name);
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_bot_keyword, value.BotKeyword);
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_bot_text, value.BotText);
      writer.WritePropertyName(GameInfoUploaderSerializerContext.PropName_id);
      this.ModelIdSerializeHandler(writer, value.Id);
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_text, value.Text);
      writer.WriteEndObject();
    }
  }

  private static JsonParameterInfoValues[] EnchantmentInfoCtorParamInit()
  {
    return new JsonParameterInfoValues[5]
    {
      new JsonParameterInfoValues()
      {
        Name = "Name",
        ParameterType = typeof (string),
        Position = 0,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "BotKeyword",
        ParameterType = typeof (string),
        Position = 1,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "BotText",
        ParameterType = typeof (string),
        Position = 2,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Id",
        ParameterType = typeof (MegaCrit.Sts2.Core.Models.ModelId),
        Position = 3,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Text",
        ParameterType = typeof (string),
        Position = 4,
        IsNullable = false,
        IsMemberInitializer = true
      }
    };
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.EncounterInfo> EncounterInfo
  {
    get
    {
      return this._EncounterInfo ?? (this._EncounterInfo = (JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.EncounterInfo>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.GameInfo.Objects.EncounterInfo)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.EncounterInfo> Create_EncounterInfo(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.EncounterInfo> jsonTypeInfo;
    if (!GameInfoUploaderSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.GameInfo.Objects.EncounterInfo>(options, out jsonTypeInfo))
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      JsonObjectInfoValues<MegaCrit.Sts2.GameInfo.Objects.EncounterInfo> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.GameInfo.Objects.EncounterInfo>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.GameInfo.Objects.EncounterInfo>) null,
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.GameInfo.Objects.EncounterInfo>) (args => new MegaCrit.Sts2.GameInfo.Objects.EncounterInfo()
        {
          Name = (string) args[0],
          BotKeyword = (string) args[1],
          BotText = (string) args[2],
          Id = (MegaCrit.Sts2.Core.Models.ModelId) args[3],
          Act = (string) args[4],
          Tier = (string) args[5]
        }),
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => GameInfoUploaderSerializerContext.EncounterInfoPropInit(options)),
        ConstructorParameterMetadataInitializer = GameInfoUploaderSerializerContext.\u003C\u003EO.\u003C5\u003E__EncounterInfoCtorParamInit ?? (GameInfoUploaderSerializerContext.\u003C\u003EO.\u003C5\u003E__EncounterInfoCtorParamInit = new Func<JsonParameterInfoValues[]>(GameInfoUploaderSerializerContext.EncounterInfoCtorParamInit)),
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.EncounterInfo).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = new Action<Utf8JsonWriter, MegaCrit.Sts2.GameInfo.Objects.EncounterInfo>(this.EncounterInfoSerializeHandler)
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.GameInfo.Objects.EncounterInfo>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] EncounterInfoPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[6];
    JsonPropertyInfoValues<string> propertyInfoValues1 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.EncounterInfo),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.EncounterInfo) obj).Name),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Name",
      JsonPropertyName = "name",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.EncounterInfo).GetProperty("Name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
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
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.EncounterInfo),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.EncounterInfo) obj).BotKeyword),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "BotKeyword",
      JsonPropertyName = "bot_keyword",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.EncounterInfo).GetProperty("BotKeyword", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues2);
    jsonPropertyInfoArray[1].IsRequired = true;
    jsonPropertyInfoArray[1].IsGetNullable = false;
    jsonPropertyInfoArray[1].IsSetNullable = false;
    JsonPropertyInfoValues<string> propertyInfoValues3 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.EncounterInfo),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.EncounterInfo) obj).BotText),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "BotText",
      JsonPropertyName = "bot_text",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.EncounterInfo).GetProperty("BotText", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues3);
    jsonPropertyInfoArray[2].IsRequired = true;
    jsonPropertyInfoArray[2].IsGetNullable = false;
    jsonPropertyInfoArray[2].IsSetNullable = false;
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId> propertyInfoValues4 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.EncounterInfo),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Models.ModelId>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Models.ModelId>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.EncounterInfo) obj).Id),
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
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.EncounterInfo).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Models.ModelId), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[3] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Models.ModelId>(options, propertyInfoValues4);
    jsonPropertyInfoArray[3].IsRequired = true;
    jsonPropertyInfoArray[3].IsGetNullable = false;
    jsonPropertyInfoArray[3].IsSetNullable = false;
    JsonPropertyInfoValues<string> propertyInfoValues5 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.EncounterInfo),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.EncounterInfo) obj).Act),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Act",
      JsonPropertyName = "act",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.EncounterInfo).GetProperty("Act", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[4] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues5);
    jsonPropertyInfoArray[4].IsRequired = true;
    jsonPropertyInfoArray[4].IsGetNullable = false;
    jsonPropertyInfoArray[4].IsSetNullable = false;
    JsonPropertyInfoValues<string> propertyInfoValues6 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.EncounterInfo),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.EncounterInfo) obj).Tier),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Tier",
      JsonPropertyName = "tier",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.EncounterInfo).GetProperty("Tier", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[5] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues6);
    jsonPropertyInfoArray[5].IsRequired = true;
    jsonPropertyInfoArray[5].IsGetNullable = false;
    jsonPropertyInfoArray[5].IsSetNullable = false;
    return jsonPropertyInfoArray;
  }

  private void EncounterInfoSerializeHandler(Utf8JsonWriter writer, MegaCrit.Sts2.GameInfo.Objects.EncounterInfo? value)
  {
    if (value == null)
    {
      writer.WriteNullValue();
    }
    else
    {
      writer.WriteStartObject();
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_name, value.Name);
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_bot_keyword, value.BotKeyword);
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_bot_text, value.BotText);
      writer.WritePropertyName(GameInfoUploaderSerializerContext.PropName_id);
      this.ModelIdSerializeHandler(writer, value.Id);
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_act, value.Act);
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_tier, value.Tier);
      writer.WriteEndObject();
    }
  }

  private static JsonParameterInfoValues[] EncounterInfoCtorParamInit()
  {
    return new JsonParameterInfoValues[6]
    {
      new JsonParameterInfoValues()
      {
        Name = "Name",
        ParameterType = typeof (string),
        Position = 0,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "BotKeyword",
        ParameterType = typeof (string),
        Position = 1,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "BotText",
        ParameterType = typeof (string),
        Position = 2,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Id",
        ParameterType = typeof (MegaCrit.Sts2.Core.Models.ModelId),
        Position = 3,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Act",
        ParameterType = typeof (string),
        Position = 4,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Tier",
        ParameterType = typeof (string),
        Position = 5,
        IsNullable = false,
        IsMemberInitializer = true
      }
    };
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.EventInfo> EventInfo
  {
    get
    {
      return this._EventInfo ?? (this._EventInfo = (JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.EventInfo>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.GameInfo.Objects.EventInfo)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.EventInfo> Create_EventInfo(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.EventInfo> jsonTypeInfo;
    if (!GameInfoUploaderSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.GameInfo.Objects.EventInfo>(options, out jsonTypeInfo))
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      JsonObjectInfoValues<MegaCrit.Sts2.GameInfo.Objects.EventInfo> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.GameInfo.Objects.EventInfo>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.GameInfo.Objects.EventInfo>) null,
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.GameInfo.Objects.EventInfo>) (args => new MegaCrit.Sts2.GameInfo.Objects.EventInfo()
        {
          Name = (string) args[0],
          BotKeyword = (string) args[1],
          BotText = (string) args[2],
          Id = (MegaCrit.Sts2.Core.Models.ModelId) args[3],
          Act = (string) args[4],
          Options = (List<string>) args[5]
        }),
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => GameInfoUploaderSerializerContext.EventInfoPropInit(options)),
        ConstructorParameterMetadataInitializer = GameInfoUploaderSerializerContext.\u003C\u003EO.\u003C6\u003E__EventInfoCtorParamInit ?? (GameInfoUploaderSerializerContext.\u003C\u003EO.\u003C6\u003E__EventInfoCtorParamInit = new Func<JsonParameterInfoValues[]>(GameInfoUploaderSerializerContext.EventInfoCtorParamInit)),
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.EventInfo).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = new Action<Utf8JsonWriter, MegaCrit.Sts2.GameInfo.Objects.EventInfo>(this.EventInfoSerializeHandler)
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.GameInfo.Objects.EventInfo>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] EventInfoPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[6];
    JsonPropertyInfoValues<string> propertyInfoValues1 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.EventInfo),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.EventInfo) obj).Name),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Name",
      JsonPropertyName = "name",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.EventInfo).GetProperty("Name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
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
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.EventInfo),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.EventInfo) obj).BotKeyword),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "BotKeyword",
      JsonPropertyName = "bot_keyword",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.EventInfo).GetProperty("BotKeyword", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues2);
    jsonPropertyInfoArray[1].IsRequired = true;
    jsonPropertyInfoArray[1].IsGetNullable = false;
    jsonPropertyInfoArray[1].IsSetNullable = false;
    JsonPropertyInfoValues<string> propertyInfoValues3 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.EventInfo),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.EventInfo) obj).BotText),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "BotText",
      JsonPropertyName = "bot_text",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.EventInfo).GetProperty("BotText", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues3);
    jsonPropertyInfoArray[2].IsRequired = true;
    jsonPropertyInfoArray[2].IsGetNullable = false;
    jsonPropertyInfoArray[2].IsSetNullable = false;
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId> propertyInfoValues4 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.EventInfo),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Models.ModelId>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Models.ModelId>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.EventInfo) obj).Id),
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
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.EventInfo).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Models.ModelId), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[3] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Models.ModelId>(options, propertyInfoValues4);
    jsonPropertyInfoArray[3].IsRequired = true;
    jsonPropertyInfoArray[3].IsGetNullable = false;
    jsonPropertyInfoArray[3].IsSetNullable = false;
    JsonPropertyInfoValues<string> propertyInfoValues5 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.EventInfo),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.EventInfo) obj).Act),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Act",
      JsonPropertyName = "act",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.EventInfo).GetProperty("Act", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[4] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues5);
    jsonPropertyInfoArray[4].IsRequired = true;
    jsonPropertyInfoArray[4].IsGetNullable = false;
    jsonPropertyInfoArray[4].IsSetNullable = false;
    JsonPropertyInfoValues<List<string>> propertyInfoValues6 = new JsonPropertyInfoValues<List<string>>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.EventInfo),
      Converter = (JsonConverter<List<string>>) null,
      Getter = (Func<object, List<string>>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.EventInfo) obj).Options),
      Setter = (Action<object, List<string>>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Options",
      JsonPropertyName = nameof (options),
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.EventInfo).GetProperty("Options", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (List<string>), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[5] = JsonMetadataServices.CreatePropertyInfo<List<string>>(options, propertyInfoValues6);
    jsonPropertyInfoArray[5].IsRequired = true;
    jsonPropertyInfoArray[5].IsGetNullable = false;
    jsonPropertyInfoArray[5].IsSetNullable = false;
    return jsonPropertyInfoArray;
  }

  private void EventInfoSerializeHandler(Utf8JsonWriter writer, MegaCrit.Sts2.GameInfo.Objects.EventInfo? value)
  {
    if (value == null)
    {
      writer.WriteNullValue();
    }
    else
    {
      writer.WriteStartObject();
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_name, value.Name);
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_bot_keyword, value.BotKeyword);
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_bot_text, value.BotText);
      writer.WritePropertyName(GameInfoUploaderSerializerContext.PropName_id);
      this.ModelIdSerializeHandler(writer, value.Id);
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_act, value.Act);
      writer.WritePropertyName(GameInfoUploaderSerializerContext.PropName_options);
      this.ListStringSerializeHandler(writer, value.Options);
      writer.WriteEndObject();
    }
  }

  private static JsonParameterInfoValues[] EventInfoCtorParamInit()
  {
    return new JsonParameterInfoValues[6]
    {
      new JsonParameterInfoValues()
      {
        Name = "Name",
        ParameterType = typeof (string),
        Position = 0,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "BotKeyword",
        ParameterType = typeof (string),
        Position = 1,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "BotText",
        ParameterType = typeof (string),
        Position = 2,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Id",
        ParameterType = typeof (MegaCrit.Sts2.Core.Models.ModelId),
        Position = 3,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Act",
        ParameterType = typeof (string),
        Position = 4,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Options",
        ParameterType = typeof (List<string>),
        Position = 5,
        IsNullable = false,
        IsMemberInitializer = true
      }
    };
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.IGameInfo> IGameInfo
  {
    get
    {
      return this._IGameInfo ?? (this._IGameInfo = (JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.IGameInfo>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.GameInfo.Objects.IGameInfo)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.IGameInfo> Create_IGameInfo(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.IGameInfo> jsonTypeInfo;
    if (!GameInfoUploaderSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.GameInfo.Objects.IGameInfo>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.GameInfo.Objects.IGameInfo> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.GameInfo.Objects.IGameInfo>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.GameInfo.Objects.IGameInfo>) null,
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.GameInfo.Objects.IGameInfo>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => GameInfoUploaderSerializerContext.IGameInfoPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) null,
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.GameInfo.Objects.IGameInfo>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.GameInfo.Objects.IGameInfo>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] IGameInfoPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[3];
    JsonPropertyInfoValues<string> propertyInfoValues1 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = true,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.IGameInfo),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.IGameInfo) obj).Name),
      Setter = (Action<object, string>) null,
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Name",
      JsonPropertyName = "name",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.IGameInfo).GetProperty("Name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues1);
    jsonPropertyInfoArray[0].IsGetNullable = false;
    JsonPropertyInfoValues<string> propertyInfoValues2 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = true,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.IGameInfo),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.IGameInfo) obj).BotKeyword),
      Setter = (Action<object, string>) null,
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "BotKeyword",
      JsonPropertyName = "bot_keyword",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.IGameInfo).GetProperty("BotKeyword", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues2);
    jsonPropertyInfoArray[1].IsGetNullable = false;
    JsonPropertyInfoValues<string> propertyInfoValues3 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = true,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.IGameInfo),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.IGameInfo) obj).BotText),
      Setter = (Action<object, string>) null,
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "BotText",
      JsonPropertyName = "bot_text",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.IGameInfo).GetProperty("BotText", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues3);
    jsonPropertyInfoArray[2].IsGetNullable = false;
    return jsonPropertyInfoArray;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.Keywords> Keywords
  {
    get
    {
      return this._Keywords ?? (this._Keywords = (JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.Keywords>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.GameInfo.Objects.Keywords)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.Keywords> Create_Keywords(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.Keywords> jsonTypeInfo;
    if (!GameInfoUploaderSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.GameInfo.Objects.Keywords>(options, out jsonTypeInfo))
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      JsonObjectInfoValues<MegaCrit.Sts2.GameInfo.Objects.Keywords> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.GameInfo.Objects.Keywords>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.GameInfo.Objects.Keywords>) null,
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.GameInfo.Objects.Keywords>) (args => new MegaCrit.Sts2.GameInfo.Objects.Keywords()
        {
          Name = (string) args[0],
          BotKeyword = (string) args[1],
          BotText = (string) args[2]
        }),
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => GameInfoUploaderSerializerContext.KeywordsPropInit(options)),
        ConstructorParameterMetadataInitializer = GameInfoUploaderSerializerContext.\u003C\u003EO.\u003C7\u003E__KeywordsCtorParamInit ?? (GameInfoUploaderSerializerContext.\u003C\u003EO.\u003C7\u003E__KeywordsCtorParamInit = new Func<JsonParameterInfoValues[]>(GameInfoUploaderSerializerContext.KeywordsCtorParamInit)),
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.Keywords).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = new Action<Utf8JsonWriter, MegaCrit.Sts2.GameInfo.Objects.Keywords>(this.KeywordsSerializeHandler)
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.GameInfo.Objects.Keywords>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] KeywordsPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[3];
    JsonPropertyInfoValues<string> propertyInfoValues1 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.Keywords),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.Keywords) obj).Name),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Name",
      JsonPropertyName = "name",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.Keywords).GetProperty("Name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
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
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.Keywords),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.Keywords) obj).BotKeyword),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "BotKeyword",
      JsonPropertyName = "bot_keyword",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.Keywords).GetProperty("BotKeyword", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues2);
    jsonPropertyInfoArray[1].IsRequired = true;
    jsonPropertyInfoArray[1].IsGetNullable = false;
    jsonPropertyInfoArray[1].IsSetNullable = false;
    JsonPropertyInfoValues<string> propertyInfoValues3 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.Keywords),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.Keywords) obj).BotText),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "BotText",
      JsonPropertyName = "bot_text",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.Keywords).GetProperty("BotText", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues3);
    jsonPropertyInfoArray[2].IsRequired = true;
    jsonPropertyInfoArray[2].IsGetNullable = false;
    jsonPropertyInfoArray[2].IsSetNullable = false;
    return jsonPropertyInfoArray;
  }

  private void KeywordsSerializeHandler(Utf8JsonWriter writer, MegaCrit.Sts2.GameInfo.Objects.Keywords? value)
  {
    if (value == null)
    {
      writer.WriteNullValue();
    }
    else
    {
      writer.WriteStartObject();
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_name, value.Name);
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_bot_keyword, value.BotKeyword);
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_bot_text, value.BotText);
      writer.WriteEndObject();
    }
  }

  private static JsonParameterInfoValues[] KeywordsCtorParamInit()
  {
    return new JsonParameterInfoValues[3]
    {
      new JsonParameterInfoValues()
      {
        Name = "Name",
        ParameterType = typeof (string),
        Position = 0,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "BotKeyword",
        ParameterType = typeof (string),
        Position = 1,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "BotText",
        ParameterType = typeof (string),
        Position = 2,
        IsNullable = false,
        IsMemberInitializer = true
      }
    };
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.NeowBonusInfo> NeowBonusInfo
  {
    get
    {
      return this._NeowBonusInfo ?? (this._NeowBonusInfo = (JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.NeowBonusInfo>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.GameInfo.Objects.NeowBonusInfo)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.NeowBonusInfo> Create_NeowBonusInfo(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.NeowBonusInfo> jsonTypeInfo;
    if (!GameInfoUploaderSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.GameInfo.Objects.NeowBonusInfo>(options, out jsonTypeInfo))
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      JsonObjectInfoValues<MegaCrit.Sts2.GameInfo.Objects.NeowBonusInfo> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.GameInfo.Objects.NeowBonusInfo>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.GameInfo.Objects.NeowBonusInfo>) null,
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.GameInfo.Objects.NeowBonusInfo>) (args => new MegaCrit.Sts2.GameInfo.Objects.NeowBonusInfo()
        {
          Name = (string) args[0],
          BotKeyword = (string) args[1],
          BotText = (string) args[2],
          Id = (string) args[3],
          Text = (string) args[4]
        }),
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => GameInfoUploaderSerializerContext.NeowBonusInfoPropInit(options)),
        ConstructorParameterMetadataInitializer = GameInfoUploaderSerializerContext.\u003C\u003EO.\u003C8\u003E__NeowBonusInfoCtorParamInit ?? (GameInfoUploaderSerializerContext.\u003C\u003EO.\u003C8\u003E__NeowBonusInfoCtorParamInit = new Func<JsonParameterInfoValues[]>(GameInfoUploaderSerializerContext.NeowBonusInfoCtorParamInit)),
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.NeowBonusInfo).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = new Action<Utf8JsonWriter, MegaCrit.Sts2.GameInfo.Objects.NeowBonusInfo>(this.NeowBonusInfoSerializeHandler)
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.GameInfo.Objects.NeowBonusInfo>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] NeowBonusInfoPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[5];
    JsonPropertyInfoValues<string> propertyInfoValues1 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.NeowBonusInfo),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.NeowBonusInfo) obj).Name),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Name",
      JsonPropertyName = "name",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.NeowBonusInfo).GetProperty("Name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
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
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.NeowBonusInfo),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.NeowBonusInfo) obj).BotKeyword),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "BotKeyword",
      JsonPropertyName = "bot_keyword",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.NeowBonusInfo).GetProperty("BotKeyword", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues2);
    jsonPropertyInfoArray[1].IsRequired = true;
    jsonPropertyInfoArray[1].IsGetNullable = false;
    jsonPropertyInfoArray[1].IsSetNullable = false;
    JsonPropertyInfoValues<string> propertyInfoValues3 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.NeowBonusInfo),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.NeowBonusInfo) obj).BotText),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "BotText",
      JsonPropertyName = "bot_text",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.NeowBonusInfo).GetProperty("BotText", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues3);
    jsonPropertyInfoArray[2].IsRequired = true;
    jsonPropertyInfoArray[2].IsGetNullable = false;
    jsonPropertyInfoArray[2].IsSetNullable = false;
    JsonPropertyInfoValues<string> propertyInfoValues4 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.NeowBonusInfo),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.NeowBonusInfo) obj).Id),
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
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.NeowBonusInfo).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[3] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues4);
    jsonPropertyInfoArray[3].IsRequired = true;
    jsonPropertyInfoArray[3].IsGetNullable = false;
    jsonPropertyInfoArray[3].IsSetNullable = false;
    JsonPropertyInfoValues<string> propertyInfoValues5 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.NeowBonusInfo),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.NeowBonusInfo) obj).Text),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Text",
      JsonPropertyName = "text",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.NeowBonusInfo).GetProperty("Text", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[4] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues5);
    jsonPropertyInfoArray[4].IsRequired = true;
    jsonPropertyInfoArray[4].IsGetNullable = false;
    jsonPropertyInfoArray[4].IsSetNullable = false;
    return jsonPropertyInfoArray;
  }

  private void NeowBonusInfoSerializeHandler(Utf8JsonWriter writer, MegaCrit.Sts2.GameInfo.Objects.NeowBonusInfo? value)
  {
    if (value == null)
    {
      writer.WriteNullValue();
    }
    else
    {
      writer.WriteStartObject();
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_name, value.Name);
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_bot_keyword, value.BotKeyword);
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_bot_text, value.BotText);
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_id, value.Id);
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_text, value.Text);
      writer.WriteEndObject();
    }
  }

  private static JsonParameterInfoValues[] NeowBonusInfoCtorParamInit()
  {
    return new JsonParameterInfoValues[5]
    {
      new JsonParameterInfoValues()
      {
        Name = "Name",
        ParameterType = typeof (string),
        Position = 0,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "BotKeyword",
        ParameterType = typeof (string),
        Position = 1,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "BotText",
        ParameterType = typeof (string),
        Position = 2,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Id",
        ParameterType = typeof (string),
        Position = 3,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Text",
        ParameterType = typeof (string),
        Position = 4,
        IsNullable = false,
        IsMemberInitializer = true
      }
    };
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.PotionInfo> PotionInfo
  {
    get
    {
      return this._PotionInfo ?? (this._PotionInfo = (JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.PotionInfo>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.GameInfo.Objects.PotionInfo)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.PotionInfo> Create_PotionInfo(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.PotionInfo> jsonTypeInfo;
    if (!GameInfoUploaderSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.GameInfo.Objects.PotionInfo>(options, out jsonTypeInfo))
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      JsonObjectInfoValues<MegaCrit.Sts2.GameInfo.Objects.PotionInfo> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.GameInfo.Objects.PotionInfo>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.GameInfo.Objects.PotionInfo>) null,
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.GameInfo.Objects.PotionInfo>) (args => new MegaCrit.Sts2.GameInfo.Objects.PotionInfo()
        {
          Name = (string) args[0],
          BotKeyword = (string) args[1],
          BotText = (string) args[2],
          Id = (MegaCrit.Sts2.Core.Models.ModelId) args[3],
          Rarity = (string) args[4],
          Text = (string) args[5],
          Color = (string) args[6]
        }),
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => GameInfoUploaderSerializerContext.PotionInfoPropInit(options)),
        ConstructorParameterMetadataInitializer = GameInfoUploaderSerializerContext.\u003C\u003EO.\u003C9\u003E__PotionInfoCtorParamInit ?? (GameInfoUploaderSerializerContext.\u003C\u003EO.\u003C9\u003E__PotionInfoCtorParamInit = new Func<JsonParameterInfoValues[]>(GameInfoUploaderSerializerContext.PotionInfoCtorParamInit)),
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.PotionInfo).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = new Action<Utf8JsonWriter, MegaCrit.Sts2.GameInfo.Objects.PotionInfo>(this.PotionInfoSerializeHandler)
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.GameInfo.Objects.PotionInfo>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] PotionInfoPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[7];
    JsonPropertyInfoValues<string> propertyInfoValues1 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.PotionInfo),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.PotionInfo) obj).Name),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Name",
      JsonPropertyName = "name",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.PotionInfo).GetProperty("Name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
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
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.PotionInfo),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.PotionInfo) obj).BotKeyword),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "BotKeyword",
      JsonPropertyName = "bot_keyword",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.PotionInfo).GetProperty("BotKeyword", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues2);
    jsonPropertyInfoArray[1].IsRequired = true;
    jsonPropertyInfoArray[1].IsGetNullable = false;
    jsonPropertyInfoArray[1].IsSetNullable = false;
    JsonPropertyInfoValues<string> propertyInfoValues3 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.PotionInfo),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.PotionInfo) obj).BotText),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "BotText",
      JsonPropertyName = "bot_text",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.PotionInfo).GetProperty("BotText", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues3);
    jsonPropertyInfoArray[2].IsRequired = true;
    jsonPropertyInfoArray[2].IsGetNullable = false;
    jsonPropertyInfoArray[2].IsSetNullable = false;
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId> propertyInfoValues4 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.PotionInfo),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Models.ModelId>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Models.ModelId>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.PotionInfo) obj).Id),
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
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.PotionInfo).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Models.ModelId), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[3] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Models.ModelId>(options, propertyInfoValues4);
    jsonPropertyInfoArray[3].IsRequired = true;
    jsonPropertyInfoArray[3].IsGetNullable = false;
    jsonPropertyInfoArray[3].IsSetNullable = false;
    JsonPropertyInfoValues<string> propertyInfoValues5 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.PotionInfo),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.PotionInfo) obj).Rarity),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Rarity",
      JsonPropertyName = "rarity",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.PotionInfo).GetProperty("Rarity", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[4] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues5);
    jsonPropertyInfoArray[4].IsRequired = true;
    jsonPropertyInfoArray[4].IsGetNullable = false;
    jsonPropertyInfoArray[4].IsSetNullable = false;
    JsonPropertyInfoValues<string> propertyInfoValues6 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.PotionInfo),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.PotionInfo) obj).Text),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Text",
      JsonPropertyName = "text",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.PotionInfo).GetProperty("Text", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[5] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues6);
    jsonPropertyInfoArray[5].IsRequired = true;
    jsonPropertyInfoArray[5].IsGetNullable = false;
    jsonPropertyInfoArray[5].IsSetNullable = false;
    JsonPropertyInfoValues<string> propertyInfoValues7 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.PotionInfo),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.PotionInfo) obj).Color),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Color",
      JsonPropertyName = "color",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.PotionInfo).GetProperty("Color", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[6] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues7);
    jsonPropertyInfoArray[6].IsRequired = true;
    jsonPropertyInfoArray[6].IsGetNullable = false;
    jsonPropertyInfoArray[6].IsSetNullable = false;
    return jsonPropertyInfoArray;
  }

  private void PotionInfoSerializeHandler(Utf8JsonWriter writer, MegaCrit.Sts2.GameInfo.Objects.PotionInfo? value)
  {
    if (value == null)
    {
      writer.WriteNullValue();
    }
    else
    {
      writer.WriteStartObject();
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_name, value.Name);
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_bot_keyword, value.BotKeyword);
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_bot_text, value.BotText);
      writer.WritePropertyName(GameInfoUploaderSerializerContext.PropName_id);
      this.ModelIdSerializeHandler(writer, value.Id);
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_rarity, value.Rarity);
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_text, value.Text);
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_color, value.Color);
      writer.WriteEndObject();
    }
  }

  private static JsonParameterInfoValues[] PotionInfoCtorParamInit()
  {
    return new JsonParameterInfoValues[7]
    {
      new JsonParameterInfoValues()
      {
        Name = "Name",
        ParameterType = typeof (string),
        Position = 0,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "BotKeyword",
        ParameterType = typeof (string),
        Position = 1,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "BotText",
        ParameterType = typeof (string),
        Position = 2,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Id",
        ParameterType = typeof (MegaCrit.Sts2.Core.Models.ModelId),
        Position = 3,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Rarity",
        ParameterType = typeof (string),
        Position = 4,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Text",
        ParameterType = typeof (string),
        Position = 5,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Color",
        ParameterType = typeof (string),
        Position = 6,
        IsNullable = false,
        IsMemberInitializer = true
      }
    };
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.RelicInfo> RelicInfo
  {
    get
    {
      return this._RelicInfo ?? (this._RelicInfo = (JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.RelicInfo>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.GameInfo.Objects.RelicInfo)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.RelicInfo> Create_RelicInfo(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.GameInfo.Objects.RelicInfo> jsonTypeInfo;
    if (!GameInfoUploaderSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.GameInfo.Objects.RelicInfo>(options, out jsonTypeInfo))
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      JsonObjectInfoValues<MegaCrit.Sts2.GameInfo.Objects.RelicInfo> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.GameInfo.Objects.RelicInfo>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.GameInfo.Objects.RelicInfo>) null,
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.GameInfo.Objects.RelicInfo>) (args => new MegaCrit.Sts2.GameInfo.Objects.RelicInfo()
        {
          Name = (string) args[0],
          BotKeyword = (string) args[1],
          BotText = (string) args[2],
          Id = (MegaCrit.Sts2.Core.Models.ModelId) args[3],
          Rarity = (string) args[4],
          Text = (string) args[5],
          Color = (string) args[6]
        }),
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => GameInfoUploaderSerializerContext.RelicInfoPropInit(options)),
        ConstructorParameterMetadataInitializer = GameInfoUploaderSerializerContext.\u003C\u003EO.\u003C10\u003E__RelicInfoCtorParamInit ?? (GameInfoUploaderSerializerContext.\u003C\u003EO.\u003C10\u003E__RelicInfoCtorParamInit = new Func<JsonParameterInfoValues[]>(GameInfoUploaderSerializerContext.RelicInfoCtorParamInit)),
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.RelicInfo).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = new Action<Utf8JsonWriter, MegaCrit.Sts2.GameInfo.Objects.RelicInfo>(this.RelicInfoSerializeHandler)
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.GameInfo.Objects.RelicInfo>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] RelicInfoPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[7];
    JsonPropertyInfoValues<string> propertyInfoValues1 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.RelicInfo),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.RelicInfo) obj).Name),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Name",
      JsonPropertyName = "name",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.RelicInfo).GetProperty("Name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
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
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.RelicInfo),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.RelicInfo) obj).BotKeyword),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "BotKeyword",
      JsonPropertyName = "bot_keyword",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.RelicInfo).GetProperty("BotKeyword", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues2);
    jsonPropertyInfoArray[1].IsRequired = true;
    jsonPropertyInfoArray[1].IsGetNullable = false;
    jsonPropertyInfoArray[1].IsSetNullable = false;
    JsonPropertyInfoValues<string> propertyInfoValues3 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.RelicInfo),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.RelicInfo) obj).BotText),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "BotText",
      JsonPropertyName = "bot_text",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.RelicInfo).GetProperty("BotText", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues3);
    jsonPropertyInfoArray[2].IsRequired = true;
    jsonPropertyInfoArray[2].IsGetNullable = false;
    jsonPropertyInfoArray[2].IsSetNullable = false;
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId> propertyInfoValues4 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Models.ModelId>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.RelicInfo),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Models.ModelId>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Models.ModelId>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.RelicInfo) obj).Id),
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
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.RelicInfo).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (MegaCrit.Sts2.Core.Models.ModelId), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[3] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Models.ModelId>(options, propertyInfoValues4);
    jsonPropertyInfoArray[3].IsRequired = true;
    jsonPropertyInfoArray[3].IsGetNullable = false;
    jsonPropertyInfoArray[3].IsSetNullable = false;
    JsonPropertyInfoValues<string> propertyInfoValues5 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.RelicInfo),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.RelicInfo) obj).Rarity),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Rarity",
      JsonPropertyName = "rarity",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.RelicInfo).GetProperty("Rarity", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[4] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues5);
    jsonPropertyInfoArray[4].IsRequired = true;
    jsonPropertyInfoArray[4].IsGetNullable = false;
    jsonPropertyInfoArray[4].IsSetNullable = false;
    JsonPropertyInfoValues<string> propertyInfoValues6 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.RelicInfo),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.RelicInfo) obj).Text),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Text",
      JsonPropertyName = "text",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.RelicInfo).GetProperty("Text", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[5] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues6);
    jsonPropertyInfoArray[5].IsRequired = true;
    jsonPropertyInfoArray[5].IsGetNullable = false;
    jsonPropertyInfoArray[5].IsSetNullable = false;
    JsonPropertyInfoValues<string> propertyInfoValues7 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.GameInfo.Objects.RelicInfo),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.GameInfo.Objects.RelicInfo) obj).Color),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Color",
      JsonPropertyName = "color",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.GameInfo.Objects.RelicInfo).GetProperty("Color", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[6] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues7);
    jsonPropertyInfoArray[6].IsRequired = true;
    jsonPropertyInfoArray[6].IsGetNullable = false;
    jsonPropertyInfoArray[6].IsSetNullable = false;
    return jsonPropertyInfoArray;
  }

  private void RelicInfoSerializeHandler(Utf8JsonWriter writer, MegaCrit.Sts2.GameInfo.Objects.RelicInfo? value)
  {
    if (value == null)
    {
      writer.WriteNullValue();
    }
    else
    {
      writer.WriteStartObject();
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_name, value.Name);
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_bot_keyword, value.BotKeyword);
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_bot_text, value.BotText);
      writer.WritePropertyName(GameInfoUploaderSerializerContext.PropName_id);
      this.ModelIdSerializeHandler(writer, value.Id);
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_rarity, value.Rarity);
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_text, value.Text);
      writer.WriteString(GameInfoUploaderSerializerContext.PropName_color, value.Color);
      writer.WriteEndObject();
    }
  }

  private static JsonParameterInfoValues[] RelicInfoCtorParamInit()
  {
    return new JsonParameterInfoValues[7]
    {
      new JsonParameterInfoValues()
      {
        Name = "Name",
        ParameterType = typeof (string),
        Position = 0,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "BotKeyword",
        ParameterType = typeof (string),
        Position = 1,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "BotText",
        ParameterType = typeof (string),
        Position = 2,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Id",
        ParameterType = typeof (MegaCrit.Sts2.Core.Models.ModelId),
        Position = 3,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Rarity",
        ParameterType = typeof (string),
        Position = 4,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Text",
        ParameterType = typeof (string),
        Position = 5,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Color",
        ParameterType = typeof (string),
        Position = 6,
        IsNullable = false,
        IsMemberInitializer = true
      }
    };
  }

  public 
  #nullable disable
  JsonTypeInfo<List<MegaCrit.Sts2.GameInfo.Objects.IGameInfo>> ListIGameInfo
  {
    get
    {
      return this._ListIGameInfo ?? (this._ListIGameInfo = (JsonTypeInfo<List<MegaCrit.Sts2.GameInfo.Objects.IGameInfo>>) this.Options.GetTypeInfo(typeof (List<MegaCrit.Sts2.GameInfo.Objects.IGameInfo>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<List<MegaCrit.Sts2.GameInfo.Objects.IGameInfo>> Create_ListIGameInfo(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<List<MegaCrit.Sts2.GameInfo.Objects.IGameInfo>> jsonTypeInfo;
    if (!GameInfoUploaderSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<MegaCrit.Sts2.GameInfo.Objects.IGameInfo>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<List<MegaCrit.Sts2.GameInfo.Objects.IGameInfo>> collectionInfoValues = new JsonCollectionInfoValues<List<MegaCrit.Sts2.GameInfo.Objects.IGameInfo>>()
      {
        ObjectCreator = (Func<List<MegaCrit.Sts2.GameInfo.Objects.IGameInfo>>) (() => new List<MegaCrit.Sts2.GameInfo.Objects.IGameInfo>()),
        SerializeHandler = new Action<Utf8JsonWriter, List<MegaCrit.Sts2.GameInfo.Objects.IGameInfo>>(this.ListIGameInfoSerializeHandler)
      };
      jsonTypeInfo = JsonMetadataServices.CreateListInfo<List<MegaCrit.Sts2.GameInfo.Objects.IGameInfo>, MegaCrit.Sts2.GameInfo.Objects.IGameInfo>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private void ListIGameInfoSerializeHandler(Utf8JsonWriter writer, List<MegaCrit.Sts2.GameInfo.Objects.IGameInfo>? value)
  {
    if (value == null)
    {
      writer.WriteNullValue();
    }
    else
    {
      writer.WriteStartArray();
      for (int index = 0; index < value.Count; ++index)
        JsonSerializer.Serialize<MegaCrit.Sts2.GameInfo.Objects.IGameInfo>(writer, value[index], this.IGameInfo);
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
    if (!GameInfoUploaderSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<List<string>>(options, out jsonTypeInfo))
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
    if (!GameInfoUploaderSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<int>(options, out jsonTypeInfo))
      jsonTypeInfo = JsonMetadataServices.CreateValueInfo<int>(options, (JsonConverter) JsonMetadataServices.Int32Converter);
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
    if (!GameInfoUploaderSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<string>(options, out jsonTypeInfo))
      jsonTypeInfo = JsonMetadataServices.CreateValueInfo<string>(options, (JsonConverter) JsonMetadataServices.StringConverter);
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public static GameInfoUploaderSerializerContext Default { get; }

  protected override JsonSerializerOptions? GeneratedSerializerOptions { get; } = GameInfoUploaderSerializerContext.s_defaultOptions;

  public GameInfoUploaderSerializerContext()
    : base((JsonSerializerOptions) null)
  {
  }

  public GameInfoUploaderSerializerContext(JsonSerializerOptions options)
    : base(options)
  {
  }

  private static bool TryGetTypeInfoForRuntimeCustomConverter<TJsonMetadataType>(
    JsonSerializerOptions options,
    out JsonTypeInfo<TJsonMetadataType> jsonTypeInfo)
  {
    JsonConverter converterForType = GameInfoUploaderSerializerContext.GetRuntimeConverterForType(typeof (TJsonMetadataType), options);
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
        return GameInfoUploaderSerializerContext.ExpandConverter(type, converter, options, false);
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
    if (type == typeof (MegaCrit.Sts2.Core.Models.ModelId))
      return (JsonTypeInfo) this.Create_ModelId(options);
    if (type == typeof (MegaCrit.Sts2.GameInfo.Objects.AncientChoiceInfo))
      return (JsonTypeInfo) this.Create_AncientChoiceInfo(options);
    if (type == typeof (MegaCrit.Sts2.GameInfo.Objects.CardInfo))
      return (JsonTypeInfo) this.Create_CardInfo(options);
    if (type == typeof (MegaCrit.Sts2.GameInfo.Objects.DailyMods))
      return (JsonTypeInfo) this.Create_DailyMods(options);
    if (type == typeof (MegaCrit.Sts2.GameInfo.Objects.EnchantmentInfo))
      return (JsonTypeInfo) this.Create_EnchantmentInfo(options);
    if (type == typeof (MegaCrit.Sts2.GameInfo.Objects.EncounterInfo))
      return (JsonTypeInfo) this.Create_EncounterInfo(options);
    if (type == typeof (MegaCrit.Sts2.GameInfo.Objects.EventInfo))
      return (JsonTypeInfo) this.Create_EventInfo(options);
    if (type == typeof (MegaCrit.Sts2.GameInfo.Objects.IGameInfo))
      return (JsonTypeInfo) this.Create_IGameInfo(options);
    if (type == typeof (MegaCrit.Sts2.GameInfo.Objects.Keywords))
      return (JsonTypeInfo) this.Create_Keywords(options);
    if (type == typeof (MegaCrit.Sts2.GameInfo.Objects.NeowBonusInfo))
      return (JsonTypeInfo) this.Create_NeowBonusInfo(options);
    if (type == typeof (MegaCrit.Sts2.GameInfo.Objects.PotionInfo))
      return (JsonTypeInfo) this.Create_PotionInfo(options);
    if (type == typeof (MegaCrit.Sts2.GameInfo.Objects.RelicInfo))
      return (JsonTypeInfo) this.Create_RelicInfo(options);
    if (type == typeof (List<MegaCrit.Sts2.GameInfo.Objects.IGameInfo>))
      return (JsonTypeInfo) this.Create_ListIGameInfo(options);
    if (type == typeof (List<string>))
      return (JsonTypeInfo) this.Create_ListString(options);
    if (type == typeof (int))
      return (JsonTypeInfo) this.Create_Int32(options);
    return type == typeof (string) ? (JsonTypeInfo) this.Create_String(options) : (JsonTypeInfo) null;
  }

  static GameInfoUploaderSerializerContext()
  {
    JsonSerializerOptions serializerOptions = new JsonSerializerOptions();
    serializerOptions.Converters.Add((JsonConverter) new ModelIdMetricsConverter());
    serializerOptions.IncludeFields = true;
    serializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    serializerOptions.WriteIndented = true;
    GameInfoUploaderSerializerContext.s_defaultOptions = serializerOptions;
    GameInfoUploaderSerializerContext.Default = new GameInfoUploaderSerializerContext(new JsonSerializerOptions(GameInfoUploaderSerializerContext.s_defaultOptions));
    GameInfoUploaderSerializerContext.PropName_category = JsonEncodedText.Encode("category", (JavaScriptEncoder) null);
    GameInfoUploaderSerializerContext.PropName_entry = JsonEncodedText.Encode("entry", (JavaScriptEncoder) null);
    GameInfoUploaderSerializerContext.PropName_name = JsonEncodedText.Encode("name", (JavaScriptEncoder) null);
    GameInfoUploaderSerializerContext.PropName_bot_keyword = JsonEncodedText.Encode("bot_keyword", (JavaScriptEncoder) null);
    GameInfoUploaderSerializerContext.PropName_bot_text = JsonEncodedText.Encode("bot_text", (JavaScriptEncoder) null);
    GameInfoUploaderSerializerContext.PropName_id = JsonEncodedText.Encode("id", (JavaScriptEncoder) null);
    GameInfoUploaderSerializerContext.PropName_text = JsonEncodedText.Encode("text", (JavaScriptEncoder) null);
    GameInfoUploaderSerializerContext.PropName_ancient = JsonEncodedText.Encode("ancient", (JavaScriptEncoder) null);
    GameInfoUploaderSerializerContext.PropName_upgraded = JsonEncodedText.Encode("upgraded", (JavaScriptEncoder) null);
    GameInfoUploaderSerializerContext.PropName_color = JsonEncodedText.Encode("color", (JavaScriptEncoder) null);
    GameInfoUploaderSerializerContext.PropName_rarity = JsonEncodedText.Encode("rarity", (JavaScriptEncoder) null);
    GameInfoUploaderSerializerContext.PropName_type = JsonEncodedText.Encode("type", (JavaScriptEncoder) null);
    GameInfoUploaderSerializerContext.PropName_base_damage = JsonEncodedText.Encode("base_damage", (JavaScriptEncoder) null);
    GameInfoUploaderSerializerContext.PropName_energy = JsonEncodedText.Encode("energy", (JavaScriptEncoder) null);
    GameInfoUploaderSerializerContext.PropName_star_cost = JsonEncodedText.Encode("star_cost", (JavaScriptEncoder) null);
    GameInfoUploaderSerializerContext.PropName_has_art = JsonEncodedText.Encode("has_art", (JavaScriptEncoder) null);
    GameInfoUploaderSerializerContext.PropName_has_joke_art = JsonEncodedText.Encode("has_joke_art", (JavaScriptEncoder) null);
    GameInfoUploaderSerializerContext.PropName_act = JsonEncodedText.Encode("act", (JavaScriptEncoder) null);
    GameInfoUploaderSerializerContext.PropName_tier = JsonEncodedText.Encode("tier", (JavaScriptEncoder) null);
    GameInfoUploaderSerializerContext.PropName_options = JsonEncodedText.Encode("options", (JavaScriptEncoder) null);
  }
}
