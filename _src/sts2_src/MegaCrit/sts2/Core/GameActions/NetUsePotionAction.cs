// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.GameActions.NetUsePotionAction
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.GameActions;

public struct NetUsePotionAction : INetAction, IPacketSerializable
{
  public uint potionIndex;
  public uint? targetId;
  public ulong? targetPlayerId;
  public bool enqueuedInCombat;

  public GameAction ToGameAction(Player player)
  {
    return (GameAction) new UsePotionAction(player, this.potionIndex, this.targetId, this.targetPlayerId, this.enqueuedInCombat);
  }

  public void Serialize(PacketWriter writer)
  {
    writer.WriteUInt(this.potionIndex, 4);
    writer.WriteBool(this.enqueuedInCombat);
    writer.WriteBool(this.targetId.HasValue);
    if (this.targetId.HasValue)
      writer.WriteUInt(this.targetId.GetValueOrDefault(), 6);
    writer.WriteBool(this.targetPlayerId.HasValue);
    if (!this.targetPlayerId.HasValue)
      return;
    writer.WriteULong(this.targetPlayerId.Value);
  }

  public void Deserialize(PacketReader reader)
  {
    this.potionIndex = reader.ReadUInt(4);
    this.enqueuedInCombat = reader.ReadBool();
    this.targetId = !reader.ReadBool() ? new uint?() : new uint?(reader.ReadUInt(6));
    if (reader.ReadBool())
      this.targetPlayerId = new ulong?(reader.ReadULong());
    else
      this.targetPlayerId = new ulong?();
  }

  public override string ToString()
  {
    return $"NetUsePotionAction {this.potionIndex} target: {this.targetId} player: {this.targetPlayerId} combat: {this.enqueuedInCombat}";
  }
}
