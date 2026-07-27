// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby.LobbySeedChangedMessage
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby;

public struct LobbySeedChangedMessage : INetMessage, IPacketSerializable
{
  public string? seed;

  public bool ShouldBroadcast => true;

  public NetTransferMode Mode => NetTransferMode.Reliable;

  public LogLevel LogLevel => LogLevel.VeryDebug;

  public bool ShouldBuffer => true;

  public void Serialize(PacketWriter writer)
  {
    writer.WriteBool(this.seed != null);
    if (this.seed == null)
      return;
    writer.WriteString(this.seed);
  }

  public void Deserialize(PacketReader reader)
  {
    if (!reader.ReadBool())
      return;
    this.seed = reader.ReadString();
  }
}
