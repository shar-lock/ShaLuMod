// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Daily.TimeServerResult
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Daily;

public struct TimeServerResult : IPacketSerializable
{
  public DateTimeOffset serverTime;
  public DateTimeOffset localReceivedTime;

  public void Serialize(PacketWriter writer)
  {
    writer.WriteLong(this.serverTime.ToUnixTimeSeconds());
    writer.WriteLong(this.localReceivedTime.ToUnixTimeSeconds());
  }

  public void Deserialize(PacketReader reader)
  {
    this.serverTime = DateTimeOffset.FromUnixTimeSeconds(reader.ReadLong());
    this.localReceivedTime = DateTimeOffset.FromUnixTimeSeconds(reader.ReadLong());
  }
}
