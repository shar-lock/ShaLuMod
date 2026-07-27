// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawingsJsonConverter
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using System;
using System.IO;
using System.IO.Compression;
using System.Text.Json;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves.MapDrawing;

public class SerializableMapDrawingsJsonConverter : JsonConverter<SerializableMapDrawings>
{
  private static readonly PacketWriter _packetWriter = new PacketWriter();

  public override SerializableMapDrawings Read(
    ref Utf8JsonReader reader,
    Type typeToConvert,
    JsonSerializerOptions options)
  {
    using (MemoryStream memoryStream1 = new MemoryStream())
    {
      using (MemoryStream memoryStream2 = new MemoryStream(Convert.FromBase64String(((Utf8JsonReader) ref reader).GetString())))
      {
        using (GZipStream gzipStream = new GZipStream((Stream) memoryStream2, CompressionMode.Decompress))
          gzipStream.CopyTo((Stream) memoryStream1);
      }
      PacketReader packetReader = new PacketReader();
      packetReader.Reset(memoryStream1.ToArray());
      return packetReader.Read<SerializableMapDrawings>();
    }
  }

  public override void Write(
    Utf8JsonWriter writer,
    SerializableMapDrawings mapDrawings,
    JsonSerializerOptions options)
  {
    string base64String;
    lock (SerializableMapDrawingsJsonConverter._packetWriter)
    {
      SerializableMapDrawingsJsonConverter._packetWriter.Reset();
      SerializableMapDrawingsJsonConverter._packetWriter.Write<SerializableMapDrawings>(mapDrawings);
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (GZipStream gzipStream = new GZipStream((Stream) memoryStream, CompressionLevel.Fastest, true))
          ((Stream) gzipStream).Write(SerializableMapDrawingsJsonConverter._packetWriter.Buffer, 0, SerializableMapDrawingsJsonConverter._packetWriter.BytePosition);
        base64String = Convert.ToBase64String(memoryStream.ToArray());
      }
    }
    writer.WriteStringValue(base64String);
  }
}
