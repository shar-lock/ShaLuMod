// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Sync.RewardSetSkippedMessage
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Runs;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Sync;

public class RewardSetSkippedMessage : INetMessage, IPacketSerializable, IRunLocationTargetedMessage
{
  public RunLocation location;
  public int setId;

  public bool ShouldBroadcast => true;

  public NetTransferMode Mode => NetTransferMode.Reliable;

  public LogLevel LogLevel => LogLevel.Debug;

  public bool ShouldBuffer => true;

  public RunLocation Location => this.location;

  public void Serialize(PacketWriter writer)
  {
    writer.Write<RunLocation>(this.location);
    writer.WriteInt(this.setId);
  }

  public void Deserialize(PacketReader reader)
  {
    this.location = reader.Read<RunLocation>();
    this.setId = reader.ReadInt();
  }
}
