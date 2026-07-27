// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Multiplayer.NetChecksumData
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Multiplayer.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.Multiplayer;

public struct NetChecksumData : IPacketSerializable
{
  public uint id;
  public uint checksum;

  public void Serialize(PacketWriter writer)
  {
    writer.WriteUInt(this.id);
    writer.WriteUInt(this.checksum);
  }

  public void Deserialize(PacketReader reader)
  {
    this.id = reader.ReadUInt();
    this.checksum = reader.ReadUInt();
  }
}
