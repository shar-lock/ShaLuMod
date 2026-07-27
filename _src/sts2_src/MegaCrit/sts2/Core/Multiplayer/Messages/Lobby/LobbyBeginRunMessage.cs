// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby.LobbyBeginRunMessage
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby;

public struct LobbyBeginRunMessage : INetMessage, IPacketSerializable
{
  public List<LobbyPlayer>? playersInLobby;
  public string seed;
  public List<SerializableModifier> modifiers;
  public string act1;

  public bool ShouldBroadcast => true;

  public NetTransferMode Mode => NetTransferMode.Reliable;

  public LogLevel LogLevel => LogLevel.VeryDebug;

  public bool ShouldBuffer => true;

  public void Serialize(PacketWriter writer)
  {
    if (this.playersInLobby == null)
      throw new InvalidOperationException("Tried to serialize ClientSlotGrantedMessage with null list!");
    writer.WriteList<LobbyPlayer>((IReadOnlyList<LobbyPlayer>) this.playersInLobby, 3);
    writer.WriteString(this.seed);
    writer.WriteList<SerializableModifier>((IReadOnlyList<SerializableModifier>) this.modifiers);
    writer.WriteString(this.act1);
  }

  public void Deserialize(PacketReader reader)
  {
    this.playersInLobby = reader.ReadList<LobbyPlayer>(3);
    this.seed = reader.ReadString();
    this.modifiers = reader.ReadList<SerializableModifier>();
    this.act1 = reader.ReadString();
  }
}
