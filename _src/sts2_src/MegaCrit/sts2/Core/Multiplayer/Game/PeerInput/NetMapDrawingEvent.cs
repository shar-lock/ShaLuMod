// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput.NetMapDrawingEvent
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput;

public struct NetMapDrawingEvent : IPacketSerializable
{
  private static readonly QuantizeParams _quantizeParamsX = new QuantizeParams(-3f, 3f, 16 /*0x10*/);
  private static readonly QuantizeParams _quantizeParamsY = new QuantizeParams(-2f, 2f, 24);
  public MapDrawingEventType type;
  public DrawingMode? overrideDrawingMode;
  public Vector2 position;

  public void Serialize(PacketWriter writer)
  {
    writer.WriteEnum<MapDrawingEventType>(this.type);
    writer.WriteBool(this.overrideDrawingMode.HasValue);
    if (this.overrideDrawingMode.HasValue)
      writer.WriteEnum<DrawingMode>(this.overrideDrawingMode.Value);
    if (this.type == MapDrawingEventType.EndLine)
      return;
    writer.WriteVector2(this.position, new QuantizeParams?(NetMapDrawingEvent._quantizeParamsX), new QuantizeParams?(NetMapDrawingEvent._quantizeParamsY));
  }

  public void Deserialize(PacketReader reader)
  {
    this.type = reader.ReadEnum<MapDrawingEventType>();
    if (reader.ReadBool())
      this.overrideDrawingMode = new DrawingMode?(reader.ReadEnum<DrawingMode>());
    if (this.type == MapDrawingEventType.EndLine)
      return;
    this.position = reader.ReadVector2(new QuantizeParams?(NetMapDrawingEvent._quantizeParamsX), new QuantizeParams?(NetMapDrawingEvent._quantizeParamsY));
  }
}
