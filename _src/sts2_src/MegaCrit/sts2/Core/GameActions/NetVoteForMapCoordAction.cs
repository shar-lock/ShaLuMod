// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.GameActions.NetVoteForMapCoordAction
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Runs;

#nullable enable
namespace MegaCrit.Sts2.Core.GameActions;

public struct NetVoteForMapCoordAction : INetAction, IPacketSerializable
{
  public MapLocation source;
  public MapVote? destination;

  public GameAction ToGameAction(Player player)
  {
    return (GameAction) new VoteForMapCoordAction(player, this.source, this.destination);
  }

  public void Serialize(PacketWriter writer)
  {
    writer.Write<MapLocation>(this.source);
    writer.WriteBool(this.destination.HasValue);
    if (!this.destination.HasValue)
      return;
    writer.Write<MapVote>(this.destination.Value);
  }

  public void Deserialize(PacketReader reader)
  {
    this.source = reader.Read<MapLocation>();
    if (!reader.ReadBool())
      return;
    this.destination = new MapVote?(reader.Read<MapVote>());
  }

  public override string ToString()
  {
    return $"{nameof (NetVoteForMapCoordAction)} {this.source}->{this.destination}";
  }
}
