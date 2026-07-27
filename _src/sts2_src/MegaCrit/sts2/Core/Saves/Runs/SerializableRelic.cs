// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.Runs.SerializableRelic
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves.Runs;

public class SerializableRelic : IPacketSerializable
{
  [JsonPropertyName("id")]
  public ModelId? Id { get; set; }

  [JsonPropertyName("props")]
  [JsonIgnore]
  public SavedProperties? Props { get; set; }

  [JsonPropertyName("floor_added_to_deck")]
  [JsonIgnore]
  public int? FloorAddedToDeck { get; set; }

  public void Serialize(PacketWriter writer)
  {
    writer.WriteModelEntry(this.Id);
    writer.WriteBool(this.Props != null);
    if (this.Props != null)
      writer.Write<SavedProperties>(this.Props);
    PacketWriter packetWriter = writer;
    int? floorAddedToDeck = this.FloorAddedToDeck;
    int num = floorAddedToDeck.HasValue ? 1 : 0;
    packetWriter.WriteBool(num != 0);
    floorAddedToDeck = this.FloorAddedToDeck;
    if (!floorAddedToDeck.HasValue)
      return;
    writer.WriteInt(this.FloorAddedToDeck.Value, 8);
  }

  public void Deserialize(PacketReader reader)
  {
    this.Id = reader.ReadModelIdAssumingType<RelicModel>();
    if (reader.ReadBool())
      this.Props = reader.Read<SavedProperties>();
    if (!reader.ReadBool())
      return;
    this.FloorAddedToDeck = new int?(reader.ReadInt(8));
  }
}
