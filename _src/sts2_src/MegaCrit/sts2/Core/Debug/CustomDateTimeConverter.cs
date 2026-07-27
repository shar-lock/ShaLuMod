// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Debug.CustomDateTimeConverter
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Debug;

public class CustomDateTimeConverter : JsonConverter<DateTime>
{
  public override DateTime Read(
    ref Utf8JsonReader reader,
    Type typeToConvert,
    JsonSerializerOptions options)
  {
    return DateTime.Parse(((Utf8JsonReader) ref reader).GetString() ?? string.Empty, (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
  }

  public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
  {
    writer.WriteStringValue(value.ToString("yyyy-MM-dd HH:mm:ss zzz", (IFormatProvider) CultureInfo.InvariantCulture));
  }
}
