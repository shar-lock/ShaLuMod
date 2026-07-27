// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.GameInfo.ModelIdMetricsConverter
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.GameInfo;

public class ModelIdMetricsConverter : JsonConverter<ModelId>
{
  public override ModelId Read(
    ref Utf8JsonReader reader,
    Type typeToConvert,
    JsonSerializerOptions options)
  {
    throw new NotImplementedException();
  }

  public override void Write(Utf8JsonWriter writer, ModelId value, JsonSerializerOptions options)
  {
    if (value == ModelId.none)
      writer.WriteNullValue();
    else
      writer.WriteStringValue(value.Entry);
  }
}
