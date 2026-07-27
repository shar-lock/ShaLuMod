// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Flavor.MapDrawingModeChangedMessage
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Flavor;

public struct MapDrawingModeChangedMessage : INetMessage, IPacketSerializable
{
  public DrawingMode drawingMode;

  public bool ShouldBroadcast => true;

  public NetTransferMode Mode => NetTransferMode.Unreliable;

  public LogLevel LogLevel => LogLevel.VeryDebug;

  public bool ShouldBuffer => true;

  public void Serialize(PacketWriter writer) => writer.WriteEnum<DrawingMode>(this.drawingMode);

  public void Deserialize(PacketReader reader) => this.drawingMode = reader.ReadEnum<DrawingMode>();
}
