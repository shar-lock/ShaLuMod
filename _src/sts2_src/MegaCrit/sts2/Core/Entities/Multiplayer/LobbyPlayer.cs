// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Multiplayer.LobbyPlayer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Unlocks;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.Multiplayer;

public struct LobbyPlayer : IPacketSerializable
{
  public ulong id;
  public int slotId;
  public CharacterModel character;
  public SerializableUnlockState unlockState;
  public int maxMultiplayerAscensionUnlocked;
  public bool isReady;

  public void Serialize(PacketWriter writer)
  {
    writer.WriteULong(this.id);
    writer.WriteInt(this.slotId, 2);
    writer.WriteModel<CharacterModel>(this.character);
    writer.Write<SerializableUnlockState>(this.unlockState);
    writer.WriteInt(this.maxMultiplayerAscensionUnlocked);
    writer.WriteBool(this.isReady);
  }

  public void Deserialize(PacketReader reader)
  {
    this.id = reader.ReadULong();
    this.slotId = reader.ReadInt(2);
    this.character = reader.ReadModel<CharacterModel>();
    this.unlockState = reader.Read<SerializableUnlockState>();
    this.maxMultiplayerAscensionUnlocked = reader.ReadInt();
    this.isReady = reader.ReadBool();
  }

  public override string ToString() => $"Player {this.id}, {this.character.Id.Entry}";
}
