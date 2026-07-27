// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.Runs.SerializableExtraRunFields
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves.Runs;

public class SerializableExtraRunFields : IPacketSerializable
{
  [JsonPropertyName("started_with_neow")]
  [JsonIgnore]
  public bool StartedWithNeow { get; set; }

  [JsonPropertyName("test_subject_kills")]
  [JsonIgnore]
  public int TestSubjectKills { get; set; }

  [JsonPropertyName("freed_repy")]
  [JsonIgnore]
  public bool FreedRepy { get; set; }

  public void Serialize(PacketWriter writer)
  {
    writer.WriteBool(this.StartedWithNeow);
    writer.WriteInt(this.TestSubjectKills);
    writer.WriteBool(this.FreedRepy);
  }

  public void Deserialize(PacketReader reader)
  {
    this.StartedWithNeow = reader.ReadBool();
    this.TestSubjectKills = reader.ReadInt();
    this.FreedRepy = reader.ReadBool();
  }
}
