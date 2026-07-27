// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Localization.SerializableDynamicVarDictionarySerializerContext
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

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
namespace MegaCrit.Sts2.Core.Localization;

[JsonSourceGenerationOptions(WriteIndented = true, IncludeFields = true, UseStringEnumConverter = true)]
[JsonSerializable(typeof (Dictionary<string, MegaCrit.Sts2.Core.Localization.SerializableDynamicVar>))]
[GeneratedCode("System.Text.Json.SourceGeneration", "9.0.12.31616")]
internal class SerializableDynamicVarDictionarySerializerContext : 
  JsonSerializerContext,
  IJsonTypeInfoResolver
{
  private JsonTypeInfo<bool>? _Boolean;
  private JsonTypeInfo<System.Decimal>? _Decimal;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Localization.DynamicVarType>? _DynamicVarType;
  private JsonTypeInfo<MegaCrit.Sts2.Core.Localization.SerializableDynamicVar>? _SerializableDynamicVar;
  private JsonTypeInfo<Dictionary<string, MegaCrit.Sts2.Core.Localization.SerializableDynamicVar>>? _DictionaryStringSerializableDynamicVar;
  private JsonTypeInfo<string>? _String;
  private static readonly JsonSerializerOptions s_defaultOptions = new JsonSerializerOptions()
  {
    IncludeFields = true,
    WriteIndented = true
  };
  private const BindingFlags InstanceMemberBindingFlags = (BindingFlags) 52;
  private static readonly JsonEncodedText PropName_type = JsonEncodedText.Encode("type", (JavaScriptEncoder) null);
  private static readonly JsonEncodedText PropName_decimal_value = JsonEncodedText.Encode("decimal_value", (JavaScriptEncoder) null);
  private static readonly JsonEncodedText PropName_bool_value = JsonEncodedText.Encode("bool_value", (JavaScriptEncoder) null);
  private static readonly JsonEncodedText PropName_string_value = JsonEncodedText.Encode("string_value", (JavaScriptEncoder) null);

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
    if (!SerializableDynamicVarDictionarySerializerContext.TryGetTypeInfoForRuntimeCustomConverter<bool>(options, out jsonTypeInfo))
      jsonTypeInfo = JsonMetadataServices.CreateValueInfo<bool>(options, (JsonConverter) JsonMetadataServices.BooleanConverter);
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<System.Decimal> Decimal
  {
    get
    {
      return this._Decimal ?? (this._Decimal = (JsonTypeInfo<System.Decimal>) this.Options.GetTypeInfo(typeof (System.Decimal)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<System.Decimal> Create_Decimal(JsonSerializerOptions options)
  {
    JsonTypeInfo<System.Decimal> jsonTypeInfo;
    if (!SerializableDynamicVarDictionarySerializerContext.TryGetTypeInfoForRuntimeCustomConverter<System.Decimal>(options, out jsonTypeInfo))
      jsonTypeInfo = JsonMetadataServices.CreateValueInfo<System.Decimal>(options, (JsonConverter) JsonMetadataServices.DecimalConverter);
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Localization.DynamicVarType> DynamicVarType
  {
    get
    {
      return this._DynamicVarType ?? (this._DynamicVarType = (JsonTypeInfo<MegaCrit.Sts2.Core.Localization.DynamicVarType>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Localization.DynamicVarType)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Localization.DynamicVarType> Create_DynamicVarType(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Localization.DynamicVarType> jsonTypeInfo;
    if (!SerializableDynamicVarDictionarySerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Localization.DynamicVarType>(options, out jsonTypeInfo))
    {
      JsonConverter jsonConverter = SerializableDynamicVarDictionarySerializerContext.ExpandConverter(typeof (MegaCrit.Sts2.Core.Localization.DynamicVarType), (JsonConverter) new JsonStringEnumConverter<MegaCrit.Sts2.Core.Localization.DynamicVarType>(), options);
      jsonTypeInfo = JsonMetadataServices.CreateValueInfo<MegaCrit.Sts2.Core.Localization.DynamicVarType>(options, jsonConverter);
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Localization.SerializableDynamicVar> SerializableDynamicVar
  {
    get
    {
      return this._SerializableDynamicVar ?? (this._SerializableDynamicVar = (JsonTypeInfo<MegaCrit.Sts2.Core.Localization.SerializableDynamicVar>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Localization.SerializableDynamicVar)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Localization.SerializableDynamicVar> Create_SerializableDynamicVar(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Localization.SerializableDynamicVar> jsonTypeInfo;
    if (!SerializableDynamicVarDictionarySerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Localization.SerializableDynamicVar>(options, out jsonTypeInfo))
    {
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Localization.SerializableDynamicVar> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Localization.SerializableDynamicVar>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Localization.SerializableDynamicVar>) (() => new MegaCrit.Sts2.Core.Localization.SerializableDynamicVar()),
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Localization.SerializableDynamicVar>) null,
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => SerializableDynamicVarDictionarySerializerContext.SerializableDynamicVarPropInit(options)),
        ConstructorParameterMetadataInitializer = (Func<JsonParameterInfoValues[]>) null,
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Localization.SerializableDynamicVar).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = new Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Localization.SerializableDynamicVar>(this.SerializableDynamicVarSerializeHandler)
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Localization.SerializableDynamicVar>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] SerializableDynamicVarPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[4];
    JsonPropertyInfoValues<MegaCrit.Sts2.Core.Localization.DynamicVarType> propertyInfoValues1 = new JsonPropertyInfoValues<MegaCrit.Sts2.Core.Localization.DynamicVarType>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Localization.SerializableDynamicVar),
      Converter = (JsonConverter<MegaCrit.Sts2.Core.Localization.DynamicVarType>) null,
      Getter = (Func<object, MegaCrit.Sts2.Core.Localization.DynamicVarType>) (obj => ((MegaCrit.Sts2.Core.Localization.SerializableDynamicVar) obj).type),
      Setter = (Action<object, MegaCrit.Sts2.Core.Localization.DynamicVarType>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Localization.SerializableDynamicVar>(obj).type = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "type",
      JsonPropertyName = "type",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Localization.SerializableDynamicVar).GetField("type", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[0] = JsonMetadataServices.CreatePropertyInfo<MegaCrit.Sts2.Core.Localization.DynamicVarType>(options, propertyInfoValues1);
    JsonPropertyInfoValues<System.Decimal> propertyInfoValues2 = new JsonPropertyInfoValues<System.Decimal>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Localization.SerializableDynamicVar),
      Converter = (JsonConverter<System.Decimal>) null,
      Getter = (Func<object, System.Decimal>) (obj => ((MegaCrit.Sts2.Core.Localization.SerializableDynamicVar) obj).decimalValue),
      Setter = (Action<object, System.Decimal>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Localization.SerializableDynamicVar>(obj).decimalValue = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "decimalValue",
      JsonPropertyName = "decimal_value",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Localization.SerializableDynamicVar).GetField("decimalValue", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<System.Decimal>(options, propertyInfoValues2);
    JsonPropertyInfoValues<bool> propertyInfoValues3 = new JsonPropertyInfoValues<bool>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Localization.SerializableDynamicVar),
      Converter = (JsonConverter<bool>) null,
      Getter = (Func<object, bool>) (obj => ((MegaCrit.Sts2.Core.Localization.SerializableDynamicVar) obj).boolValue),
      Setter = (Action<object, bool>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Localization.SerializableDynamicVar>(obj).boolValue = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "boolValue",
      JsonPropertyName = "bool_value",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Localization.SerializableDynamicVar).GetField("boolValue", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<bool>(options, propertyInfoValues3);
    JsonPropertyInfoValues<string> propertyInfoValues4 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = false,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Localization.SerializableDynamicVar),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Localization.SerializableDynamicVar) obj).stringValue),
      Setter = (Action<object, string>) ((obj, value) => Unsafe.Unbox<MegaCrit.Sts2.Core.Localization.SerializableDynamicVar>(obj).stringValue = value),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "stringValue",
      JsonPropertyName = "string_value",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Localization.SerializableDynamicVar).GetField("stringValue", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
    };
    jsonPropertyInfoArray[3] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues4);
    return jsonPropertyInfoArray;
  }

  private void SerializableDynamicVarSerializeHandler(
    Utf8JsonWriter writer,
    MegaCrit.Sts2.Core.Localization.SerializableDynamicVar value)
  {
    writer.WriteStartObject();
    writer.WritePropertyName(SerializableDynamicVarDictionarySerializerContext.PropName_type);
    JsonSerializer.Serialize<MegaCrit.Sts2.Core.Localization.DynamicVarType>(writer, value.type, this.DynamicVarType);
    writer.WriteNumber(SerializableDynamicVarDictionarySerializerContext.PropName_decimal_value, value.decimalValue);
    writer.WriteBoolean(SerializableDynamicVarDictionarySerializerContext.PropName_bool_value, value.boolValue);
    writer.WriteString(SerializableDynamicVarDictionarySerializerContext.PropName_string_value, value.stringValue);
    writer.WriteEndObject();
  }

  public 
  #nullable disable
  JsonTypeInfo<Dictionary<string, MegaCrit.Sts2.Core.Localization.SerializableDynamicVar>> DictionaryStringSerializableDynamicVar
  {
    get
    {
      return this._DictionaryStringSerializableDynamicVar ?? (this._DictionaryStringSerializableDynamicVar = (JsonTypeInfo<Dictionary<string, MegaCrit.Sts2.Core.Localization.SerializableDynamicVar>>) this.Options.GetTypeInfo(typeof (Dictionary<string, MegaCrit.Sts2.Core.Localization.SerializableDynamicVar>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<Dictionary<string, MegaCrit.Sts2.Core.Localization.SerializableDynamicVar>> Create_DictionaryStringSerializableDynamicVar(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<Dictionary<string, MegaCrit.Sts2.Core.Localization.SerializableDynamicVar>> jsonTypeInfo;
    if (!SerializableDynamicVarDictionarySerializerContext.TryGetTypeInfoForRuntimeCustomConverter<Dictionary<string, MegaCrit.Sts2.Core.Localization.SerializableDynamicVar>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<Dictionary<string, MegaCrit.Sts2.Core.Localization.SerializableDynamicVar>> collectionInfoValues = new JsonCollectionInfoValues<Dictionary<string, MegaCrit.Sts2.Core.Localization.SerializableDynamicVar>>()
      {
        ObjectCreator = (Func<Dictionary<string, MegaCrit.Sts2.Core.Localization.SerializableDynamicVar>>) (() => new Dictionary<string, MegaCrit.Sts2.Core.Localization.SerializableDynamicVar>()),
        SerializeHandler = new Action<Utf8JsonWriter, Dictionary<string, MegaCrit.Sts2.Core.Localization.SerializableDynamicVar>>(this.DictionaryStringSerializableDynamicVarSerializeHandler)
      };
      jsonTypeInfo = JsonMetadataServices.CreateDictionaryInfo<Dictionary<string, MegaCrit.Sts2.Core.Localization.SerializableDynamicVar>, string, MegaCrit.Sts2.Core.Localization.SerializableDynamicVar>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private void DictionaryStringSerializableDynamicVarSerializeHandler(
    Utf8JsonWriter writer,
    Dictionary<string, MegaCrit.Sts2.Core.Localization.SerializableDynamicVar>? value)
  {
    if (value == null)
    {
      writer.WriteNullValue();
    }
    else
    {
      writer.WriteStartObject();
      foreach (KeyValuePair<string, MegaCrit.Sts2.Core.Localization.SerializableDynamicVar> keyValuePair in value)
      {
        writer.WritePropertyName(keyValuePair.Key);
        this.SerializableDynamicVarSerializeHandler(writer, keyValuePair.Value);
      }
      writer.WriteEndObject();
    }
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
    if (!SerializableDynamicVarDictionarySerializerContext.TryGetTypeInfoForRuntimeCustomConverter<string>(options, out jsonTypeInfo))
      jsonTypeInfo = JsonMetadataServices.CreateValueInfo<string>(options, (JsonConverter) JsonMetadataServices.StringConverter);
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public static SerializableDynamicVarDictionarySerializerContext Default { get; } = new SerializableDynamicVarDictionarySerializerContext(new JsonSerializerOptions(SerializableDynamicVarDictionarySerializerContext.s_defaultOptions));

  protected override JsonSerializerOptions? GeneratedSerializerOptions { get; } = SerializableDynamicVarDictionarySerializerContext.s_defaultOptions;

  public SerializableDynamicVarDictionarySerializerContext()
    : base((JsonSerializerOptions) null)
  {
  }

  public SerializableDynamicVarDictionarySerializerContext(JsonSerializerOptions options)
    : base(options)
  {
  }

  private static bool TryGetTypeInfoForRuntimeCustomConverter<TJsonMetadataType>(
    JsonSerializerOptions options,
    out JsonTypeInfo<TJsonMetadataType> jsonTypeInfo)
  {
    JsonConverter converterForType = SerializableDynamicVarDictionarySerializerContext.GetRuntimeConverterForType(typeof (TJsonMetadataType), options);
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
        return SerializableDynamicVarDictionarySerializerContext.ExpandConverter(type, converter, options, false);
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
    if (type == typeof (System.Decimal))
      return (JsonTypeInfo) this.Create_Decimal(options);
    if (type == typeof (MegaCrit.Sts2.Core.Localization.DynamicVarType))
      return (JsonTypeInfo) this.Create_DynamicVarType(options);
    if (type == typeof (MegaCrit.Sts2.Core.Localization.SerializableDynamicVar))
      return (JsonTypeInfo) this.Create_SerializableDynamicVar(options);
    if (type == typeof (Dictionary<string, MegaCrit.Sts2.Core.Localization.SerializableDynamicVar>))
      return (JsonTypeInfo) this.Create_DictionaryStringSerializableDynamicVar(options);
    return type == typeof (string) ? (JsonTypeInfo) this.Create_String(options) : (JsonTypeInfo) null;
  }
}
