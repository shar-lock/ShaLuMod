// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawings
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves.MapDrawing;

public class SerializableMapDrawings : IPacketSerializable
{
  public List<SerializablePlayerMapDrawings> drawings = new List<SerializablePlayerMapDrawings>();

  public void Serialize(PacketWriter writer)
  {
    writer.WriteList<SerializablePlayerMapDrawings>((IReadOnlyList<SerializablePlayerMapDrawings>) this.drawings);
  }

  public void Deserialize(PacketReader reader)
  {
    this.drawings = reader.ReadList<SerializablePlayerMapDrawings>();
  }

  public SerializableMapDrawings Anonymized()
  {
    return new SerializableMapDrawings()
    {
      drawings = this.drawings.Select<SerializablePlayerMapDrawings, SerializablePlayerMapDrawings>((Func<SerializablePlayerMapDrawings, SerializablePlayerMapDrawings>) (d => d.Anonymized())).ToList<SerializablePlayerMapDrawings>()
    };
  }
}
