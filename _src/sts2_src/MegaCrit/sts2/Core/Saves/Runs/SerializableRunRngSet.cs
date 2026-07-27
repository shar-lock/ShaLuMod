// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.Runs.SerializableRunRngSet
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Rngs;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves.Runs;

public class SerializableRunRngSet : IPacketSerializable
{
  [JsonPropertyName("seed")]
  public string? Seed { get; set; }

  [JsonPropertyName("rngs")]
  public Dictionary<RunRngType, SerializableRng> Rngs { get; set; } = new Dictionary<RunRngType, SerializableRng>();

  public void Serialize(PacketWriter writer)
  {
    writer.WriteString(this.Seed);
    writer.WriteInt(this.Rngs.Count, 8);
    foreach (RunRngType runRngType in Enum.GetValues<RunRngType>())
    {
      SerializableRng val;
      if (this.Rngs.TryGetValue(runRngType, out val))
      {
        writer.WriteEnum<RunRngType>(runRngType);
        writer.Write<SerializableRng>(val);
      }
    }
  }

  public void Deserialize(PacketReader reader)
  {
    this.Seed = reader.ReadString();
    int num = reader.ReadInt(8);
    for (int index = 0; index < num; ++index)
      this.Rngs[reader.ReadEnum<RunRngType>()] = reader.Read<SerializableRng>();
  }
}
