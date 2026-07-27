// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.GameActions.NetDiscardPotionGameAction
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.GameActions;

public struct NetDiscardPotionGameAction : INetAction, IPacketSerializable
{
  public uint potionSlotIndex;
  public bool wasEnqueuedInCombat;

  public GameAction ToGameAction(Player player)
  {
    return (GameAction) new DiscardPotionGameAction(player, this.potionSlotIndex, this.wasEnqueuedInCombat);
  }

  public void Serialize(PacketWriter writer)
  {
    writer.WriteUInt(this.potionSlotIndex, 4);
    writer.WriteBool(this.wasEnqueuedInCombat);
  }

  public void Deserialize(PacketReader reader)
  {
    this.potionSlotIndex = reader.ReadUInt(4);
    this.wasEnqueuedInCombat = reader.ReadBool();
  }

  public override string ToString()
  {
    return $"{nameof (NetDiscardPotionGameAction)} slot {this.potionSlotIndex} in combat: {this.wasEnqueuedInCombat}";
  }
}
