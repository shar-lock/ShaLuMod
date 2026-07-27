// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby.ClientLoadJoinResponseMessage
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Saves;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby;

public struct ClientLoadJoinResponseMessage : INetMessage, IPacketSerializable
{
  public SerializableRun serializableRun;
  public List<ulong> playersAlreadyConnected;

  public bool ShouldBroadcast => false;

  public NetTransferMode Mode => NetTransferMode.Reliable;

  public LogLevel LogLevel => LogLevel.Info;

  public bool ShouldBuffer => true;

  public void Serialize(PacketWriter writer)
  {
    writer.Write<SerializableRun>(this.serializableRun);
    writer.WriteInt(this.playersAlreadyConnected.Count, 6);
    foreach (ulong val in this.playersAlreadyConnected)
      writer.WriteULong(val);
  }

  public void Deserialize(PacketReader reader)
  {
    this.serializableRun = reader.Read<SerializableRun>();
    this.playersAlreadyConnected = new List<ulong>();
    int num = reader.ReadInt(6);
    for (int index = 0; index < num; ++index)
      this.playersAlreadyConnected.Add(reader.ReadULong());
  }
}
