// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Flavor.MapDrawingMessage
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Flavor;

public class MapDrawingMessage : INetMessage, IPacketSerializable
{
  public static readonly int maxEventCount = (int) Math.Pow(2.0, 4.0) - 1;
  private const int _listBits = 4;
  private List<NetMapDrawingEvent> _events = new List<NetMapDrawingEvent>();
  public DrawingMode? drawingMode;

  public bool ShouldBroadcast => true;

  public NetTransferMode Mode => NetTransferMode.Unreliable;

  public LogLevel LogLevel => LogLevel.VeryDebug;

  public bool ShouldBuffer => true;

  public IReadOnlyList<NetMapDrawingEvent> Events
  {
    get => (IReadOnlyList<NetMapDrawingEvent>) this._events;
  }

  public bool TryAddEvent(NetMapDrawingEvent ev)
  {
    if (this._events.Count >= MapDrawingMessage.maxEventCount)
      return false;
    this._events.Add(ev);
    return true;
  }

  public void Serialize(PacketWriter writer)
  {
    writer.WriteList<NetMapDrawingEvent>((IReadOnlyList<NetMapDrawingEvent>) this._events, 4);
    writer.WriteBool(this.drawingMode.HasValue);
    if (!this.drawingMode.HasValue)
      return;
    writer.WriteEnum<DrawingMode>(this.drawingMode.Value);
  }

  public void Deserialize(PacketReader reader)
  {
    this._events = reader.ReadList<NetMapDrawingEvent>(4);
    if (!reader.ReadBool())
      return;
    this.drawingMode = new DrawingMode?(reader.ReadEnum<DrawingMode>());
  }
}
