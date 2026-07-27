// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.JsonSerializationUtility
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves;

public static class JsonSerializationUtility
{
  public static IJsonTypeInfoResolver DefaultResolver { get; } = (IJsonTypeInfoResolver) MegaCritSerializerContext.Default;

  public static JsonSerializerOptions Options { get; } = new JsonSerializerOptions(MegaCritSerializerContext.DefaultGeneratedSerializerOptions)
  {
    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    TypeInfoResolver = JsonTypeInfoResolver.WithAddedModifier(JsonTypeInfoResolver.WithAddedModifier((IJsonTypeInfoResolver) MegaCritSerializerContext.Default, new Action<JsonTypeInfo>(JsonSerializationUtility.AlphabetizeProperties)), new Action<JsonTypeInfo>(JsonSerializeConditionAttribute.CheckJsonSerializeConditionsModifier))
  };

  public static void AddTypeInfoResolver(IJsonTypeInfoResolver resolver)
  {
    JsonSerializationUtility.Options.TypeInfoResolverChain.Add(resolver);
  }

  public static async Task<string> SerializeAsync<T>(T data) where T : ISaveSchema
  {
    StreamReader reader;
    string endAsync;
    using (MemoryStream stream = new MemoryStream())
    {
      await JsonSerializer.SerializeAsync<T>((Stream) stream, data, JsonSerializationUtility.GetTypeInfo<T>(), new CancellationToken());
      ((Stream) stream).Position = 0L;
      reader = new StreamReader((Stream) stream);
      try
      {
        endAsync = await ((TextReader) reader).ReadToEndAsync();
      }
      finally
      {
        ((IDisposable) reader)?.Dispose();
      }
    }
    reader = (StreamReader) null;
    return endAsync;
  }

  public static JsonTypeInfo<T> GetTypeInfo<T>(T value)
  {
    return (JsonTypeInfo<T>) JsonSerializationUtility.Options.GetTypeInfo(typeof (T));
  }

  public static JsonTypeInfo<T> GetTypeInfo<T>()
  {
    return (JsonTypeInfo<T>) JsonSerializationUtility.Options.GetTypeInfo(typeof (T));
  }

  public static void AlphabetizeProperties(JsonTypeInfo info)
  {
    if (info.Kind != 1)
      return;
    List<JsonPropertyInfo> jsonPropertyInfoList = new List<JsonPropertyInfo>();
    jsonPropertyInfoList.AddRange((IEnumerable<JsonPropertyInfo>) info.Properties);
    jsonPropertyInfoList.Sort((Comparison<JsonPropertyInfo>) ((p1, p2) => string.CompareOrdinal(p1.Name, p2.Name)));
    info.Properties.Clear();
    for (int index = 0; index < jsonPropertyInfoList.Count; ++index)
    {
      jsonPropertyInfoList[index].Order = index;
      info.Properties.Add(jsonPropertyInfoList[index]);
    }
  }

  public static string ToJson<T>(T obj) where T : ISaveSchema
  {
    return JsonSerializer.Serialize<T>(obj, JsonSerializationUtility.GetTypeInfo<T>());
  }

  public static ReadSaveResult<T> FromJson<T>(string json) where T : ISaveSchema, new()
  {
    if (string.IsNullOrWhiteSpace(json))
    {
      Log.Error($"The json for type={typeof (T)} was empty!");
      return new ReadSaveResult<T>(ReadSaveStatus.FileEmpty);
    }
    try
    {
      T data = JsonSerializer.Deserialize<T>(json, JsonSerializationUtility.GetTypeInfo<T>());
      if ((object) data != null)
        return new ReadSaveResult<T>(data);
      Log.Error($"Json parsed as null! type={typeof (T)}");
      return new ReadSaveResult<T>(ReadSaveStatus.JsonParseError);
    }
    catch (JsonException ex)
    {
      string str = ex.Path ?? "unknown";
      Log.Error($"Failed to deserialize type={typeof (T)} at path={str}, line={ex.LineNumber}, position={ex.BytePositionInLine}: {((Exception) ex).Message}");
      return new ReadSaveResult<T>(ReadSaveStatus.JsonParseError, $"JSON error at {str} (line {ex.LineNumber}): {((Exception) ex).Message}");
    }
  }
}
