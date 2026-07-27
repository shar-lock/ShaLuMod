// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Flavor.ReactionMessage
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Flavor;

public struct ReactionMessage : INetMessage, IPacketSerializable
{
  private static readonly QuantizeParams _quantizeParams = new QuantizeParams(-3f, 3f, 16 /*0x10*/);
  public ReactionType type;
  public Vector2 normalizedPosition;

  public bool ShouldBroadcast => true;

  public NetTransferMode Mode => NetTransferMode.Unreliable;

  public LogLevel LogLevel => LogLevel.VeryDebug;

  public bool ShouldBuffer => true;

  public void Serialize(PacketWriter writer)
  {
    writer.WriteEnum<ReactionType>(this.type);
    writer.WriteVector2(this.normalizedPosition, new QuantizeParams?(ReactionMessage._quantizeParams), new QuantizeParams?(ReactionMessage._quantizeParams));
  }

  public void Deserialize(PacketReader reader)
  {
    this.type = reader.ReadEnum<ReactionType>();
    this.normalizedPosition = reader.ReadVector2(new QuantizeParams?(ReactionMessage._quantizeParams), new QuantizeParams?(ReactionMessage._quantizeParams));
  }
}
