// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.SerializablePlayerRngSet
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Rngs;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves;

public class SerializablePlayerRngSet : IPacketSerializable
{
  [JsonPropertyName("seed")]
  public ulong Seed { get; set; }

  [JsonPropertyName("rngs")]
  public Dictionary<PlayerRngType, SerializableRng> Rngs { get; set; } = new Dictionary<PlayerRngType, SerializableRng>();

  public void Serialize(PacketWriter writer)
  {
    writer.WriteULong(this.Seed);
    writer.WriteInt(this.Rngs.Count, 8);
    foreach (PlayerRngType playerRngType in Enum.GetValues<PlayerRngType>())
    {
      SerializableRng val;
      if (this.Rngs.TryGetValue(playerRngType, out val))
      {
        writer.WriteEnum<PlayerRngType>(playerRngType);
        writer.Write<SerializableRng>(val);
      }
    }
  }

  public void Deserialize(PacketReader reader)
  {
    this.Seed = reader.ReadULong();
    int num = reader.ReadInt(8);
    for (int index = 0; index < num; ++index)
      this.Rngs[reader.ReadEnum<PlayerRngType>()] = reader.Read<SerializableRng>();
  }
}
