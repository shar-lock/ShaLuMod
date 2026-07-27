// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby.ClientLobbyJoinResponseMessage
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Daily;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby;

public struct ClientLobbyJoinResponseMessage : INetMessage, IPacketSerializable
{
  public List<LobbyPlayer>? playersInLobby;
  public TimeServerResult? dailyTime;
  public int ascension;
  public string? seed;
  public List<SerializableModifier> modifiers;

  public bool ShouldBroadcast => false;

  public NetTransferMode Mode => NetTransferMode.Reliable;

  public LogLevel LogLevel => LogLevel.Info;

  public bool ShouldBuffer => true;

  public void Serialize(PacketWriter writer)
  {
    if (this.playersInLobby == null)
      throw new InvalidOperationException("Tried to serialize ClientSlotGrantedMessage with null list!");
    writer.WriteList<LobbyPlayer>((IReadOnlyList<LobbyPlayer>) this.playersInLobby, 3);
    writer.WriteBool(this.dailyTime.HasValue);
    if (this.dailyTime.HasValue)
      writer.Write<TimeServerResult>(this.dailyTime.Value);
    writer.WriteBool(this.seed != null);
    if (this.seed != null)
      writer.WriteString(this.seed);
    writer.WriteInt(this.ascension, 5);
    writer.WriteList<SerializableModifier>((IReadOnlyList<SerializableModifier>) this.modifiers);
  }

  public void Deserialize(PacketReader reader)
  {
    this.playersInLobby = reader.ReadList<LobbyPlayer>(3);
    if (reader.ReadBool())
      this.dailyTime = new TimeServerResult?(reader.Read<TimeServerResult>());
    if (reader.ReadBool())
      this.seed = reader.ReadString();
    this.ascension = reader.ReadInt(5);
    this.modifiers = reader.ReadList<SerializableModifier>();
  }

  public override string ToString()
  {
    return $"{nameof (ClientLobbyJoinResponseMessage)} Players: {this.playersInLobby?.Count} Ascension: {this.ascension}";
  }
}
