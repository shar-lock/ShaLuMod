// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Messages.HeartbeatRequestMessage
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Messages;

public record struct HeartbeatRequestMessage : INetMessage, IPacketSerializable
{
  public int counter;

  public bool ShouldBroadcast => false;

  public NetTransferMode Mode => NetTransferMode.Unreliable;

  public LogLevel LogLevel => LogLevel.VeryDebug;

  public bool ShouldBuffer => false;

  public void Serialize(PacketWriter writer) => writer.WriteInt(this.counter);

  public void Deserialize(PacketReader reader) => this.counter = reader.ReadInt();

  [CompilerGenerated]
  public override readonly int GetHashCode()
  {
    return EqualityComparer<int>.Default.GetHashCode(this.counter);
  }

  [CompilerGenerated]
  public readonly bool Equals(HeartbeatRequestMessage other)
  {
    return EqualityComparer<int>.Default.Equals(this.counter, other.counter);
  }
}
