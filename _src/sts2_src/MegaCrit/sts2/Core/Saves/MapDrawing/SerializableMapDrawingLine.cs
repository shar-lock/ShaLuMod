// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.MapDrawing.SerializableMapDrawingLine
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves.MapDrawing;

public class SerializableMapDrawingLine : IPacketSerializable
{
  private static readonly QuantizeParams _quantizeParamsX = new QuantizeParams(-3f, 3f, 13);
  private static readonly QuantizeParams _quantizeParamsY = new QuantizeParams(-2f, 2f, 16 /*0x10*/);
  public bool isEraser;
  public List<Vector2> mapPoints = new List<Vector2>();

  public void Serialize(PacketWriter writer)
  {
    writer.WriteBool(this.isEraser);
    writer.WriteInt(this.mapPoints.Count, 16 /*0x10*/);
    foreach (Vector2 mapPoint in this.mapPoints)
      writer.WriteVector2(mapPoint, new QuantizeParams?(SerializableMapDrawingLine._quantizeParamsX), new QuantizeParams?(SerializableMapDrawingLine._quantizeParamsY));
  }

  public void Deserialize(PacketReader reader)
  {
    this.isEraser = reader.ReadBool();
    int num = reader.ReadInt(16 /*0x10*/);
    for (int index = 0; index < num; ++index)
      this.mapPoints.Add(reader.ReadVector2(new QuantizeParams?(SerializableMapDrawingLine._quantizeParamsX), new QuantizeParams?(SerializableMapDrawingLine._quantizeParamsY)));
  }
}
