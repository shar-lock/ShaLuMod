// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.Runs.EpochIdListConverter
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves.Runs;

public class EpochIdListConverter : JsonConverter<List<string>>
{
  public override List<string> Read(
    ref Utf8JsonReader reader,
    Type typeToConvert,
    JsonSerializerOptions options)
  {
    List<string> stringList = new List<string>();
    if (((Utf8JsonReader) ref reader).TokenType != 3)
      throw new JsonException($"Expected start of array for epoch ID list, but got {((Utf8JsonReader) ref reader).TokenType}");
    while (((Utf8JsonReader) ref reader).Read() && ((Utf8JsonReader) ref reader).TokenType != 4)
    {
      string str1 = ((Utf8JsonReader) ref reader).GetString();
      if (str1 != null)
      {
        int num = str1.IndexOf('.');
        if (num >= 0)
        {
          string str2 = str1;
          int startIndex = num + 1;
          str1 = str2.Substring(startIndex, str2.Length - startIndex);
        }
        stringList.Add(str1);
      }
    }
    return stringList;
  }

  public override void Write(
    Utf8JsonWriter writer,
    List<string> value,
    JsonSerializerOptions options)
  {
    writer.WriteStartArray();
    foreach (string str in value)
      writer.WriteStringValue(str);
    writer.WriteEndArray();
  }
}
