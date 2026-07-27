// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.GameActions.NetPickRelicAction
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.GameActions;

public struct NetPickRelicAction : INetAction, IPacketSerializable
{
  public int? relicIndex;

  public GameAction ToGameAction(Player player)
  {
    return (GameAction) new PickRelicAction(player, this.relicIndex);
  }

  public void Serialize(PacketWriter writer)
  {
    writer.WriteBool(this.relicIndex.HasValue);
    if (!this.relicIndex.HasValue)
      return;
    writer.WriteInt(this.relicIndex.Value, 8);
  }

  public void Deserialize(PacketReader reader)
  {
    if (!reader.ReadBool())
      return;
    this.relicIndex = new int?(reader.ReadInt(8));
  }

  public override string ToString() => $"{nameof (NetPickRelicAction)} index: {this.relicIndex}";
}
