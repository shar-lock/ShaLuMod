// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Messages.Game.HookActionEnqueuedMessage
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Runs;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Game;

public struct HookActionEnqueuedMessage : 
  INetMessage,
  IPacketSerializable,
  IRunLocationTargetedMessage
{
  public RunLocation location;
  public ulong ownerId;
  public uint hookActionId;
  public GameActionType gameActionType;

  public bool ShouldBroadcast => false;

  public NetTransferMode Mode => NetTransferMode.Reliable;

  public LogLevel LogLevel => LogLevel.Debug;

  public bool ShouldBuffer => true;

  public RunLocation Location => this.location;

  public void Serialize(PacketWriter writer)
  {
    writer.Write<RunLocation>(this.location);
    writer.WriteULong(this.ownerId);
    writer.WriteUInt(this.hookActionId);
    writer.WriteEnum<GameActionType>(this.gameActionType);
  }

  public void Deserialize(PacketReader reader)
  {
    this.location = reader.Read<RunLocation>();
    this.ownerId = reader.ReadULong();
    this.hookActionId = reader.ReadUInt();
    this.gameActionType = reader.ReadEnum<GameActionType>();
  }

  public override string ToString()
  {
    return $"HookActionEnqueuedMessage id: {this.hookActionId} type: {this.gameActionType}";
  }
}
