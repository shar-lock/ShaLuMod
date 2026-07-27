// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.GameActions.NetUndoEndPlayerTurnAction
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.GameActions;

public struct NetUndoEndPlayerTurnAction : INetAction, IPacketSerializable
{
  public int turnNumber;

  public GameAction ToGameAction(Player player)
  {
    return (GameAction) new UndoEndPlayerTurnAction(player, this.turnNumber);
  }

  public void Serialize(PacketWriter writer) => writer.WriteInt(this.turnNumber, 16 /*0x10*/);

  public void Deserialize(PacketReader reader) => this.turnNumber = reader.ReadInt(16 /*0x10*/);

  public override string ToString()
  {
    return $"{nameof (NetUndoEndPlayerTurnAction)} turn: {this.turnNumber}";
  }
}
