// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Debug.ReleaseInfoJsonSerializerContext
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

#nullable enable
namespace MegaCrit.Sts2.Core.Debug;

[JsonSourceGenerationOptions(WriteIndented = true, UseStringEnumConverter = true, Converters = new Type[] {typeof (CustomDateTimeConverter)})]
[JsonSerializable(typeof (MegaCrit.Sts2.Core.Debug.ReleaseInfo))]
[GeneratedCode("System.Text.Json.SourceGeneration", "9.0.12.31616")]
internal class ReleaseInfoJsonSerializerContext : JsonSerializerContext, IJsonTypeInfoResolver
{
  private JsonTypeInfo<MegaCrit.Sts2.Core.Debug.ReleaseInfo>? _ReleaseInfo;
  private JsonTypeInfo<System.DateTime>? _DateTime;
  private JsonTypeInfo<int>? _Int32;
  private JsonTypeInfo<string>? _String;
  private static readonly JsonSerializerOptions s_defaultOptions;
  private const BindingFlags InstanceMemberBindingFlags = (BindingFlags) 52;

  public 
  #nullable disable
  JsonTypeInfo<MegaCrit.Sts2.Core.Debug.ReleaseInfo> ReleaseInfo
  {
    get
    {
      return this._ReleaseInfo ?? (this._ReleaseInfo = (JsonTypeInfo<MegaCrit.Sts2.Core.Debug.ReleaseInfo>) this.Options.GetTypeInfo(typeof (MegaCrit.Sts2.Core.Debug.ReleaseInfo)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<MegaCrit.Sts2.Core.Debug.ReleaseInfo> Create_ReleaseInfo(
    JsonSerializerOptions options)
  {
    JsonTypeInfo<MegaCrit.Sts2.Core.Debug.ReleaseInfo> jsonTypeInfo;
    if (!ReleaseInfoJsonSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<MegaCrit.Sts2.Core.Debug.ReleaseInfo>(options, out jsonTypeInfo))
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      JsonObjectInfoValues<MegaCrit.Sts2.Core.Debug.ReleaseInfo> objectInfoValues = new JsonObjectInfoValues<MegaCrit.Sts2.Core.Debug.ReleaseInfo>()
      {
        ObjectCreator = (Func<MegaCrit.Sts2.Core.Debug.ReleaseInfo>) null,
        ObjectWithParameterizedConstructorCreator = (Func<object[], MegaCrit.Sts2.Core.Debug.ReleaseInfo>) (args => new MegaCrit.Sts2.Core.Debug.ReleaseInfo()
        {
          Commit = (string) args[0],
          Version = (string) args[1],
          Date = (System.DateTime) args[2],
          Branch = (string) args[3],
          MainAssemblyHash = (int) args[4]
        }),
        PropertyMetadataInitializer = (Func<JsonSerializerContext, JsonPropertyInfo[]>) (_ => ReleaseInfoJsonSerializerContext.ReleaseInfoPropInit(options)),
        ConstructorParameterMetadataInitializer = ReleaseInfoJsonSerializerContext.\u003C\u003EO.\u003C0\u003E__ReleaseInfoCtorParamInit ?? (ReleaseInfoJsonSerializerContext.\u003C\u003EO.\u003C0\u003E__ReleaseInfoCtorParamInit = new Func<JsonParameterInfoValues[]>(ReleaseInfoJsonSerializerContext.ReleaseInfoCtorParamInit)),
        ConstructorAttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Debug.ReleaseInfo).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, Array.Empty<Type>(), (ParameterModifier[]) null)),
        SerializeHandler = (Action<Utf8JsonWriter, MegaCrit.Sts2.Core.Debug.ReleaseInfo>) null
      };
      jsonTypeInfo = JsonMetadataServices.CreateObjectInfo<MegaCrit.Sts2.Core.Debug.ReleaseInfo>(options, objectInfoValues);
      ((JsonTypeInfo) jsonTypeInfo).NumberHandling = new JsonNumberHandling?();
    }
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  private static JsonPropertyInfo[] ReleaseInfoPropInit(JsonSerializerOptions options)
  {
    JsonPropertyInfo[] jsonPropertyInfoArray = new JsonPropertyInfo[5];
    JsonPropertyInfoValues<string> propertyInfoValues1 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Debug.ReleaseInfo),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Debug.ReleaseInfo) obj).Commit),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Commit",
      JsonPropertyName = "commit",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Debug.ReleaseInfo).GetProperty("Commit", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
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
      DeclaringType = typeof (MegaCrit.Sts2.Core.Debug.ReleaseInfo),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Debug.ReleaseInfo) obj).Version),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Version",
      JsonPropertyName = "version",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Debug.ReleaseInfo).GetProperty("Version", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[1] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues2);
    jsonPropertyInfoArray[1].IsRequired = true;
    jsonPropertyInfoArray[1].IsGetNullable = false;
    jsonPropertyInfoArray[1].IsSetNullable = false;
    JsonPropertyInfoValues<System.DateTime> propertyInfoValues3 = new JsonPropertyInfoValues<System.DateTime>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Debug.ReleaseInfo),
      Converter = (JsonConverter<System.DateTime>) ReleaseInfoJsonSerializerContext.ExpandConverter(typeof (System.DateTime), (JsonConverter) new CustomDateTimeConverter(), options),
      Getter = (Func<object, System.DateTime>) (obj => ((MegaCrit.Sts2.Core.Debug.ReleaseInfo) obj).Date),
      Setter = (Action<object, System.DateTime>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Date",
      JsonPropertyName = "date",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Debug.ReleaseInfo).GetProperty("Date", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (System.DateTime), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[2] = JsonMetadataServices.CreatePropertyInfo<System.DateTime>(options, propertyInfoValues3);
    jsonPropertyInfoArray[2].IsRequired = true;
    JsonPropertyInfoValues<string> propertyInfoValues4 = new JsonPropertyInfoValues<string>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Debug.ReleaseInfo),
      Converter = (JsonConverter<string>) null,
      Getter = (Func<object, string>) (obj => ((MegaCrit.Sts2.Core.Debug.ReleaseInfo) obj).Branch),
      Setter = (Action<object, string>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "Branch",
      JsonPropertyName = "branch",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Debug.ReleaseInfo).GetProperty("Branch", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (string), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[3] = JsonMetadataServices.CreatePropertyInfo<string>(options, propertyInfoValues4);
    jsonPropertyInfoArray[3].IsRequired = true;
    jsonPropertyInfoArray[3].IsGetNullable = false;
    jsonPropertyInfoArray[3].IsSetNullable = false;
    JsonPropertyInfoValues<int> propertyInfoValues5 = new JsonPropertyInfoValues<int>()
    {
      IsProperty = true,
      IsPublic = true,
      IsVirtual = false,
      DeclaringType = typeof (MegaCrit.Sts2.Core.Debug.ReleaseInfo),
      Converter = (JsonConverter<int>) null,
      Getter = (Func<object, int>) (obj => ((MegaCrit.Sts2.Core.Debug.ReleaseInfo) obj).MainAssemblyHash),
      Setter = (Action<object, int>) ((obj, value) =>
      {
        throw new InvalidOperationException("Setting init-only properties is not supported in source generation mode.");
      }),
      IgnoreCondition = new JsonIgnoreCondition?(),
      HasJsonInclude = false,
      IsExtensionData = false,
      NumberHandling = new JsonNumberHandling?(),
      PropertyName = "MainAssemblyHash",
      JsonPropertyName = "main_assembly_hash",
      AttributeProviderFactory = (Func<ICustomAttributeProvider>) (() => (ICustomAttributeProvider) typeof (MegaCrit.Sts2.Core.Debug.ReleaseInfo).GetProperty("MainAssemblyHash", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (Binder) null, typeof (int), Array.Empty<Type>(), (ParameterModifier[]) null))
    };
    jsonPropertyInfoArray[4] = JsonMetadataServices.CreatePropertyInfo<int>(options, propertyInfoValues5);
    jsonPropertyInfoArray[4].IsRequired = true;
    return jsonPropertyInfoArray;
  }

  private static JsonParameterInfoValues[] ReleaseInfoCtorParamInit()
  {
    return new JsonParameterInfoValues[5]
    {
      new JsonParameterInfoValues()
      {
        Name = "Commit",
        ParameterType = typeof (string),
        Position = 0,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Version",
        ParameterType = typeof (string),
        Position = 1,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Date",
        ParameterType = typeof (System.DateTime),
        Position = 2,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "Branch",
        ParameterType = typeof (string),
        Position = 3,
        IsNullable = false,
        IsMemberInitializer = true
      },
      new JsonParameterInfoValues()
      {
        Name = "MainAssemblyHash",
        ParameterType = typeof (int),
        Position = 4,
        IsNullable = false,
        IsMemberInitializer = true
      }
    };
  }

  public 
  #nullable disable
  JsonTypeInfo<System.DateTime> DateTime
  {
    get
    {
      return this._DateTime ?? (this._DateTime = (JsonTypeInfo<System.DateTime>) this.Options.GetTypeInfo(typeof (System.DateTime)));
    }
  }

  private 
  #nullable enable
  JsonTypeInfo<System.DateTime> Create_DateTime(JsonSerializerOptions options)
  {
    JsonTypeInfo<System.DateTime> jsonTypeInfo;
    if (!ReleaseInfoJsonSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<System.DateTime>(options, out jsonTypeInfo))
      jsonTypeInfo = JsonMetadataServices.CreateValueInfo<System.DateTime>(options, (JsonConverter) JsonMetadataServices.DateTimeConverter);
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
    if (!ReleaseInfoJsonSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<int>(options, out jsonTypeInfo))
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
    if (!ReleaseInfoJsonSerializerContext.TryGetTypeInfoForRuntimeCustomConverter<string>(options, out jsonTypeInfo))
      jsonTypeInfo = JsonMetadataServices.CreateValueInfo<string>(options, (JsonConverter) JsonMetadataServices.StringConverter);
    ((JsonTypeInfo) jsonTypeInfo).OriginatingResolver = (IJsonTypeInfoResolver) this;
    return jsonTypeInfo;
  }

  public static ReleaseInfoJsonSerializerContext Default { get; }

  protected override JsonSerializerOptions? GeneratedSerializerOptions { get; } = ReleaseInfoJsonSerializerContext.s_defaultOptions;

  public ReleaseInfoJsonSerializerContext()
    : base((JsonSerializerOptions) null)
  {
  }

  public ReleaseInfoJsonSerializerContext(JsonSerializerOptions options)
    : base(options)
  {
  }

  private static bool TryGetTypeInfoForRuntimeCustomConverter<TJsonMetadataType>(
    JsonSerializerOptions options,
    out JsonTypeInfo<TJsonMetadataType> jsonTypeInfo)
  {
    JsonConverter converterForType = ReleaseInfoJsonSerializerContext.GetRuntimeConverterForType(typeof (TJsonMetadataType), options);
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
        return ReleaseInfoJsonSerializerContext.ExpandConverter(type, converter, options, false);
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
    if (type == typeof (MegaCrit.Sts2.Core.Debug.ReleaseInfo))
      return (JsonTypeInfo) this.Create_ReleaseInfo(options);
    if (type == typeof (System.DateTime))
      return (JsonTypeInfo) this.Create_DateTime(options);
    if (type == typeof (int))
      return (JsonTypeInfo) this.Create_Int32(options);
    return type == typeof (string) ? (JsonTypeInfo) this.Create_String(options) : (JsonTypeInfo) null;
  }

  static ReleaseInfoJsonSerializerContext()
  {
    JsonSerializerOptions serializerOptions = new JsonSerializerOptions();
    serializerOptions.Converters.Add((JsonConverter) new CustomDateTimeConverter());
    serializerOptions.WriteIndented = true;
    ReleaseInfoJsonSerializerContext.s_defaultOptions = serializerOptions;
    ReleaseInfoJsonSerializerContext.Default = new ReleaseInfoJsonSerializerContext(new JsonSerializerOptions(ReleaseInfoJsonSerializerContext.s_defaultOptions));
  }
}
