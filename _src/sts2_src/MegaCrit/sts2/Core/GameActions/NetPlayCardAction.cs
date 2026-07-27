// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.GameActions.NetPlayCardAction
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using System.Runtime.CompilerServices;

#nullable enable
namespace MegaCrit.Sts2.Core.GameActions;

public struct NetPlayCardAction : INetAction, IPacketSerializable
{
  public NetCombatCard card;
  public ModelId modelId;
  public uint? targetId;

  public GameAction ToGameAction(Player player)
  {
    return (GameAction) new PlayCardAction(player, this.card, this.modelId, this.targetId);
  }

  public void Serialize(PacketWriter writer)
  {
    writer.Write<NetCombatCard>(this.card);
    writer.WriteModelEntry(this.modelId);
    writer.WriteBool(this.targetId.HasValue);
    if (!this.targetId.HasValue)
      return;
    writer.WriteUInt(this.targetId.GetValueOrDefault(), 6);
  }

  public void Deserialize(PacketReader reader)
  {
    this.card = reader.Read<NetCombatCard>();
    this.modelId = reader.ReadModelIdAssumingType<CardModel>();
    if (reader.ReadBool())
      this.targetId = new uint?(reader.ReadUInt(6));
    else
      this.targetId = new uint?();
  }

  public override string ToString()
  {
    DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 2);
    interpolatedStringHandler.AppendLiteral("NetPlayCardAction (");
    interpolatedStringHandler.AppendFormatted<NetCombatCard>(this.card);
    interpolatedStringHandler.AppendLiteral(") target: ");
    ref DefaultInterpolatedStringHandler local1 = ref interpolatedStringHandler;
    ref uint? local2 = ref this.targetId;
    string str = (local2.HasValue ? local2.GetValueOrDefault().ToString() : (string) null) ?? "null";
    local1.AppendFormatted(str);
    return interpolatedStringHandler.ToStringAndClear();
  }
}
