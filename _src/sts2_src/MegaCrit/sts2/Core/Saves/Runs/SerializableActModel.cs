// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.Runs.SerializableActModel
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves.Runs;

public class SerializableActModel : IPacketSerializable
{
  [JsonPropertyName("id")]
  public ModelId? Id { get; set; }

  [JsonPropertyName("rooms")]
  public SerializableRoomSet SerializableRooms { get; set; }

  [JsonPropertyName("saved_map")]
  [JsonIgnore]
  public SerializableActMap? SavedMap { get; set; }

  public void Serialize(PacketWriter writer)
  {
    writer.WriteModelEntry(this.Id);
    writer.Write<SerializableRoomSet>(this.SerializableRooms);
    writer.WriteBool(this.SavedMap != null);
    if (this.SavedMap == null)
      return;
    writer.Write<SerializableActMap>(this.SavedMap);
  }

  public void Deserialize(PacketReader reader)
  {
    this.Id = reader.ReadModelIdAssumingType<ActModel>();
    this.SerializableRooms = reader.Read<SerializableRoomSet>();
    if (!reader.ReadBool())
      return;
    this.SavedMap = reader.Read<SerializableActMap>();
  }
}
