// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby.ClientRejoinResponseMessage
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Saves;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby;

public struct ClientRejoinResponseMessage : INetMessage, IPacketSerializable
{
  public SerializableRun serializableRun;
  public NetFullCombatState? combatState;

  public bool ShouldBroadcast => false;

  public NetTransferMode Mode => NetTransferMode.Reliable;

  public LogLevel LogLevel => LogLevel.Info;

  public bool ShouldBuffer => true;

  public void Serialize(PacketWriter writer)
  {
    writer.Write<SerializableRun>(this.serializableRun);
    writer.WriteBool(this.combatState != null);
    if (this.combatState == null)
      return;
    writer.Write<NetFullCombatState>(this.combatState);
  }

  public void Deserialize(PacketReader reader)
  {
    this.serializableRun = reader.Read<SerializableRun>();
    if (!reader.ReadBool())
      return;
    this.combatState = reader.Read<NetFullCombatState>();
  }
}
