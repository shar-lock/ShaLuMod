// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby.LobbyPlayerChangedCharacterMessage
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Lobby;

public struct LobbyPlayerChangedCharacterMessage : INetMessage, IPacketSerializable
{
  public CharacterModel character;

  public bool ShouldBroadcast => true;

  public NetTransferMode Mode => NetTransferMode.Reliable;

  public LogLevel LogLevel => LogLevel.VeryDebug;

  public bool ShouldBuffer => true;

  public void Serialize(PacketWriter writer) => writer.WriteModel<CharacterModel>(this.character);

  public void Deserialize(PacketReader reader)
  {
    this.character = reader.ReadModel<CharacterModel>();
  }
}
