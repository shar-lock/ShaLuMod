// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Localization.LocStringVariablesJsonConverter
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Localization;

public class LocStringVariablesJsonConverter : JsonConverter<Dictionary<string, object>>
{
  public override Dictionary<string, object> Read(
    ref Utf8JsonReader reader,
    Type typeToConvert,
    JsonSerializerOptions options)
  {
    Dictionary<string, SerializableDynamicVar> dictionary1 = JsonSerializer.Deserialize<Dictionary<string, SerializableDynamicVar>>(ref reader, SerializableDynamicVarDictionarySerializerContext.Default.DictionaryStringSerializableDynamicVar);
    if (dictionary1 == null)
      throw new InvalidOperationException("Could not read LocString variables");
    Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
    foreach (KeyValuePair<string, SerializableDynamicVar> keyValuePair in dictionary1)
      dictionary2[keyValuePair.Key] = keyValuePair.Value.ToDynamicVar(keyValuePair.Key);
    return dictionary2;
  }

  public override void Write(
    Utf8JsonWriter writer,
    Dictionary<string, object> varDict,
    JsonSerializerOptions options)
  {
    Dictionary<string, SerializableDynamicVar> dictionary = new Dictionary<string, SerializableDynamicVar>();
    foreach (KeyValuePair<string, object> keyValuePair in varDict)
    {
      SerializableDynamicVar? nullable = SerializableDynamicVar.FromDynamicVar(keyValuePair.Value);
      if (nullable.HasValue && (nullable.Value.type != DynamicVarType.BaseDynamic || !(keyValuePair.Value.GetType() != typeof (DynamicVar))))
        dictionary[keyValuePair.Key] = nullable.Value;
    }
    JsonSerializer.Serialize<Dictionary<string, SerializableDynamicVar>>(writer, dictionary, SerializableDynamicVarDictionarySerializerContext.Default.DictionaryStringSerializableDynamicVar);
  }
}
