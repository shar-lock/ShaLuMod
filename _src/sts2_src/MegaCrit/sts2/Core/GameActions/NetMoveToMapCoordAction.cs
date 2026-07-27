// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.GameActions.NetMoveToMapCoordAction
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.GameActions;

public struct NetMoveToMapCoordAction : INetAction, IPacketSerializable
{
  public MapCoord destination;

  public GameAction ToGameAction(Player player)
  {
    return (GameAction) new MoveToMapCoordAction(player, this.destination);
  }

  public void Serialize(PacketWriter writer) => writer.Write<MapCoord>(this.destination);

  public void Deserialize(PacketReader reader) => this.destination = reader.Read<MapCoord>();

  public override string ToString() => $"{"MoveToMapCoordAction"} to {this.destination}";
}
