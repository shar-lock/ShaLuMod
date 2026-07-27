// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.MapDrawing.SerializablePlayerMapDrawings
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves.MapDrawing;

public class SerializablePlayerMapDrawings : IPacketSerializable
{
  public ulong playerId;
  public List<SerializableMapDrawingLine> lines = new List<SerializableMapDrawingLine>();

  public void Serialize(PacketWriter writer)
  {
    writer.WriteULong(this.playerId);
    writer.WriteList<SerializableMapDrawingLine>((IReadOnlyList<SerializableMapDrawingLine>) this.lines);
  }

  public void Deserialize(PacketReader reader)
  {
    this.playerId = reader.ReadULong();
    this.lines = reader.ReadList<SerializableMapDrawingLine>();
  }

  public SerializablePlayerMapDrawings Anonymized()
  {
    return new SerializablePlayerMapDrawings()
    {
      playerId = IdAnonymizer.Anonymize(this.playerId),
      lines = this.lines
    };
  }
}
