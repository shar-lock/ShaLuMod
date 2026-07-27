// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.Runs.SerializableModifier
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves.Runs;

public class SerializableModifier : IPacketSerializable
{
  [JsonPropertyName("id")]
  public ModelId? Id { get; set; }

  [JsonPropertyName("props")]
  [JsonIgnore]
  public SavedProperties? Props { get; set; }

  public void Serialize(PacketWriter writer)
  {
    writer.WriteModelEntry(this.Id);
    writer.WriteBool(this.Props != null);
    if (this.Props == null)
      return;
    writer.Write<SavedProperties>(this.Props);
  }

  public void Deserialize(PacketReader reader)
  {
    this.Id = reader.ReadModelIdAssumingType<ModifierModel>();
    if (!reader.ReadBool())
      return;
    this.Props = reader.Read<SavedProperties>();
  }
}
