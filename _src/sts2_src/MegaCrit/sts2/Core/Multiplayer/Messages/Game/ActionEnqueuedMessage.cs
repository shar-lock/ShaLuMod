// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Messages.Game.ActionEnqueuedMessage
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Runs;
using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Game;

public struct ActionEnqueuedMessage : INetMessage, IPacketSerializable, IRunLocationTargetedMessage
{
  public ulong playerId;
  public RunLocation location;
  public INetAction action;

  public bool ShouldBroadcast => false;

  public NetTransferMode Mode => NetTransferMode.Reliable;

  public LogLevel LogLevel => LogLevel.Debug;

  public bool ShouldBuffer => true;

  public RunLocation Location => this.location;

  public void Serialize(PacketWriter writer)
  {
    writer.WriteULong(this.playerId);
    writer.Write<RunLocation>(this.location);
    writer.WriteByte((byte) this.action.ToId());
    writer.Write<INetAction>(this.action);
  }

  public void Deserialize(PacketReader reader)
  {
    this.playerId = reader.ReadULong();
    this.location = reader.Read<RunLocation>();
    int id = (int) reader.ReadByte();
    Type type;
    if (!ActionTypes.TryGetActionType(id, out type))
      throw new InvalidOperationException($"Received net action of type {id} that does not map to any type!");
    this.action = (INetAction) Activator.CreateInstance(type);
    this.action.Deserialize(reader);
  }

  public override string ToString()
  {
    string str = "";
    if (this.action is NetPlayCardAction action)
    {
      CardModel cardModelOrNull = action.card.ToCardModelOrNull();
      if (cardModelOrNull != null)
        str = $"(card: {cardModelOrNull.Title})";
      else
        str = $"(Card ID {action.card.CombatCardIndex} not found in database!)";
    }
    return $"ActionEnqueuedMessage PlayerID: {this.playerId} Action: {this.action} Source Location: {this.location} {str}";
  }
}
