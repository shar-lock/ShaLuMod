// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Localization.LocManagerSerializerContext
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

#nullable enable
namespace MegaCrit.Sts2.Core.Localization;

[JsonSourceGenerationOptions]
[JsonSerializable(typeof (Dictionary<string, string>))]
[JsonSerializable(typeof (Dictionary<string, int>))]
[GeneratedCode("System.Text.Json.SourceGeneration", "9.0.12.31616")]
internal class LocManagerSerializerContext : JsonSerializerContext, IJsonTypeInfoResolver
{
  private JsonTypeInfo<Dictionary<string, int>>? _DictionaryStringInt32;
  private JsonTypeInfo<Dictionary<string, string>>? _DictionaryStringString;
  private JsonTypeInfo<int>? _Int32;
  private JsonTypeInfo<string>? _String;
  private static readonly JsonSerializerOptions s_defaultOptions = new JsonSerializerOptions()
  {
    ReadCommentHandling = (JsonCommentHandling) 1
  };
  private const BindingFlags InstanceMemberBindingFlags = (BindingFlags) 52;

  public 
  #nullable disable
  JsonTypeInfo<Dictionary<string, int>> DictionaryStringInt32
  {
    get
    {
      return this._DictionaryStringInt32 ?? (this._DictionaryStringInt32 = (JsonTypeInfo<Dictionary<string, int>>) this.Options.GetTypeInfo(typeof (Dictionary<string, int>)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<Dictionary<string, int>> Create_DictionaryStringInt32(JsonSerializerOptions options)
  {
    JsonTypeInfo<Dictionary<string, int>> jsonTypeInfo;
    if (!LocManagerSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<Dictionary<string, int>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<Dictionary<string, int>> collectionInfoValues = new JsonCollectionInfoValues<Dictionary<string, int>>()
      {
        ObjectCreator = (Func<Dictionary<string, int>>) (() => new Dictionary<string, int>()),
        SerializeHandler = new Action<Utf8JsonWriter, Dictionary<string, int>>(this.DictionaryStringInt32SerializeHandler)
      };
      jsonTypeInfo = JsonMetadataServices.CreateDictionaryInfo<Dictionary<string, int>, string, int>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private void DictionaryStringInt32SerializeHandler(
    Utf8JsonWriter writer,
    Dictionary<string, int>? value)
  {
    if (value == null)
    {
      writer.WriteNullValue();
    }
    else
    {
      writer.WriteStartObject();
      foreach (KeyValuePair<string, int> keyValuePair in value)
        writer.WriteNumber(keyValuePair.Key, keyValuePair.Value);
      writer.WriteEndObject();
    }
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
    if (!LocManagerSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<Dictionary<string, string>>(options, out jsonTypeInfo))
    {
      JsonCollectionInfoValues<Dictionary<string, string>> collectionInfoValues = new JsonCollectionInfoValues<Dictionary<string, string>>()
      {
        ObjectCreator = (Func<Dictionary<string, string>>) (() => new Dictionary<string, string>()),
        SerializeHandler = new Action<Utf8JsonWriter, Dictionary<string, string>>(this.DictionaryStringStringSerializeHandler)
      };
      jsonTypeInfo = JsonMetadataServices.CreateDictionaryInfo<Dictionary<string, string>, string, string>(options, collectionInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private void DictionaryStringStringSerializeHandler(
    Utf8JsonWriter writer,
    Dictionary<string, string>? value)
  {
    if (value == null)
    {
      writer.WriteNullValue();
    }
    else
    {
      writer.WriteStartObject();
      foreach (KeyValuePair<string, string> keyValuePair in value)
        writer.WriteString(keyValuePair.Key, keyValuePair.Value);
      writer.WriteEndObject();
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
    if (!LocManagerSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<int>(options, out jsonTypeInfo))
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
    if (!LocManagerSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<string>(options, out jsonTypeInfo))
      jsonTypeInfo = JsonMetadataServices.CreateValueInfo<string>(options, (JsonConverter) JsonMetadataServices.StringConverter);
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public static LocManagerSerializerContext Default { get; } = new LocManagerSerializerContext(new JsonSerializerOptions(LocManagerSerializerContext.s_defaultOptions));

  protected override JsonSerializerOptions? GeneratedSerializerOptions { get; } = LocManagerSerializerContext.s_defaultOptions;

  public LocManagerSerializerContext()
    : base((JsonSerializerOptions) null)
  {
  }

  public LocManagerSerializerContext(JsonSerializerOptions options)
    : base(options)
  {
  }

  private static bool TryGetTypeInfoForRuntimeCustomConverter<TJsonMetadataType>(
    JsonSerializerOptions options,
    out JsonTypeInfo<TJsonMetadataType> jsonTypeInfo)
  {
    JsonConverter converterForType = LocManagerSerializerContext.GetRuntimeConverterForType(typeof (TJsonMetadataType), options);
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
        return LocManagerSerializerContext.ExpandConverter(type, converter, options, false);
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
    if (type == typeof (Dictionary<string, int>))
      return (JsonTypeInfo) this.Create_DictionaryStringInt32(options);
    if (type == typeof (Dictionary<string, string>))
      return (JsonTypeInfo) this.Create_DictionaryStringString(options);
    if (type == typeof (int))
      return (JsonTypeInfo) this.Create_Int32(options);
    return type == typeof (string) ? (JsonTypeInfo) this.Create_String(options) : (JsonTypeInfo) null;
  }
}
