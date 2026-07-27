// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.Runs.SerializableEnchantment
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using System;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves.Runs;

public class SerializableEnchantment : IPacketSerializable
{
  [JsonPropertyName("id")]
  public ModelId? Id { get; set; }

  [JsonPropertyName("amount")]
  public int Amount { get; set; }

  [JsonPropertyName("props")]
  [JsonIgnore]
  public SavedProperties? Props { get; set; }

  public void Serialize(PacketWriter writer)
  {
    writer.WriteModelEntry(this.Id);
    writer.WriteInt(this.Amount, 8);
    writer.WriteBool(this.Props != null);
    if (this.Props == null)
      return;
    writer.Write<SavedProperties>(this.Props);
  }

  public void Deserialize(PacketReader reader)
  {
    this.Id = reader.ReadModelIdAssumingType<EnchantmentModel>();
    this.Amount = reader.ReadInt(8);
    if (!reader.ReadBool())
      return;
    this.Props = reader.Read<SavedProperties>();
  }

  public override bool Equals(object? obj)
  {
    if (obj == null || obj.GetType() != this.GetType())
      return false;
    SerializableEnchantment serializableEnchantment = (SerializableEnchantment) obj;
    return this.Id.Equals(serializableEnchantment.Id) && this.Amount.Equals(serializableEnchantment.Amount);
  }

  public override int GetHashCode() => HashCode.Combine<ModelId, int>(this.Id, this.Amount);

  public override string ToString()
  {
    return $"SerializableEnchantment {this.Id} with amount {this.Amount} Props: {this.Props}";
  }
}
