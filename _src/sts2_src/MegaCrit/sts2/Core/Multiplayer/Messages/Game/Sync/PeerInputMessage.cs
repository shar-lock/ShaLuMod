// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Sync.PeerInputMessage
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Sync;

public class PeerInputMessage : INetMessage, IPacketSerializable
{
  private static readonly QuantizeParams _quantizeParams = new QuantizeParams(-3f, 3f, 16 /*0x10*/);
  public bool mouseDown;
  public bool isTargeting;
  public Vector2? netMousePos;
  public NetScreenType screenType;
  public HoveredModelData hoveredModelData;
  public bool isUsingController;
  public Vector2? controllerFocusPosition;

  public bool ShouldBroadcast => true;

  public NetTransferMode Mode => NetTransferMode.Unreliable;

  public LogLevel LogLevel => LogLevel.VeryDebug;

  public bool ShouldBuffer => false;

  public void Serialize(PacketWriter writer)
  {
    writer.WriteBool(this.mouseDown);
    writer.WriteBool(this.isTargeting);
    writer.WriteBool(this.netMousePos.HasValue);
    if (this.netMousePos.HasValue)
      writer.WriteVector2(this.netMousePos.Value, new QuantizeParams?(PeerInputMessage._quantizeParams), new QuantizeParams?(PeerInputMessage._quantizeParams));
    writer.WriteEnum<NetScreenType>(this.screenType);
    writer.Write<HoveredModelData>(this.hoveredModelData);
    writer.WriteBool(this.isUsingController);
    writer.WriteBool(this.controllerFocusPosition.HasValue);
    if (!this.controllerFocusPosition.HasValue)
      return;
    writer.WriteVector2(this.controllerFocusPosition.Value, new QuantizeParams?(PeerInputMessage._quantizeParams), new QuantizeParams?(PeerInputMessage._quantizeParams));
  }

  public void Deserialize(PacketReader reader)
  {
    this.mouseDown = reader.ReadBool();
    this.isTargeting = reader.ReadBool();
    if (reader.ReadBool())
      this.netMousePos = new Vector2?(reader.ReadVector2(new QuantizeParams?(PeerInputMessage._quantizeParams), new QuantizeParams?(PeerInputMessage._quantizeParams)));
    this.screenType = reader.ReadEnum<NetScreenType>();
    this.hoveredModelData = reader.Read<HoveredModelData>();
    this.isUsingController = reader.ReadBool();
    if (!reader.ReadBool())
      return;
    this.controllerFocusPosition = new Vector2?(reader.ReadVector2(new QuantizeParams?(PeerInputMessage._quantizeParams), new QuantizeParams?(PeerInputMessage._quantizeParams)));
  }
}
