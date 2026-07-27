// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Messages.Game.RequestResumeActionAfterPlayerChoiceMessage
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Runs;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Game;

public struct RequestResumeActionAfterPlayerChoiceMessage : 
  INetMessage,
  IPacketSerializable,
  IRunLocationTargetedMessage
{
  public uint actionId;
  public RunLocation location;

  public bool ShouldBroadcast => false;

  public NetTransferMode Mode => NetTransferMode.Reliable;

  public RunLocation Location => this.location;

  public LogLevel LogLevel => LogLevel.VeryDebug;

  public bool ShouldBuffer => true;

  public void Serialize(PacketWriter writer)
  {
    writer.WriteUInt(this.actionId);
    writer.Write<RunLocation>(this.location);
  }

  public void Deserialize(PacketReader reader)
  {
    this.actionId = reader.ReadUInt();
    this.location = reader.Read<RunLocation>();
  }
}
