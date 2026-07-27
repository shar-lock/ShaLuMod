// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Messages.Game.RequestEnqueueActionMessage
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Runs;
using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Game;

public struct RequestEnqueueActionMessage : 
  INetMessage,
  IPacketSerializable,
  IRunLocationTargetedMessage
{
  public RunLocation location;
  public INetAction action;

  public bool ShouldBroadcast => false;

  public NetTransferMode Mode => NetTransferMode.Reliable;

  public RunLocation Location => this.location;

  public LogLevel LogLevel => LogLevel.VeryDebug;

  public bool ShouldBuffer => true;

  public void Serialize(PacketWriter writer)
  {
    writer.Write<RunLocation>(this.location);
    writer.WriteByte((byte) this.action.ToId());
    writer.Write<INetAction>(this.action);
  }

  public void Deserialize(PacketReader reader)
  {
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
    return $"{nameof (RequestEnqueueActionMessage)} location: {this.location} action: {this.action}";
  }
}
