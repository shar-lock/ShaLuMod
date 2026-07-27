// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.Runs.SerializableCard
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using System;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves.Runs;

public class SerializableCard : IPacketSerializable
{
  [JsonPropertyName("id")]
  public ModelId? Id { get; set; }

  [JsonPropertyName("current_upgrade_level")]
  [JsonIgnore]
  public int CurrentUpgradeLevel { get; set; }

  [JsonPropertyName("enchantment")]
  [JsonIgnore]
  public SerializableEnchantment? Enchantment { get; set; }

  [JsonPropertyName("props")]
  [JsonIgnore]
  public SavedProperties? Props { get; set; }

  [JsonPropertyName("floor_added_to_deck")]
  [JsonIgnore]
  public int? FloorAddedToDeck { get; set; }

  public void Serialize(PacketWriter writer)
  {
    writer.WriteModelEntry(this.Id);
    writer.WriteInt(this.CurrentUpgradeLevel, 8);
    writer.WriteBool(this.Enchantment != null);
    if (this.Enchantment != null)
      writer.Write<SerializableEnchantment>(this.Enchantment);
    writer.WriteBool(this.Props != null);
    if (this.Props != null)
      writer.Write<SavedProperties>(this.Props);
    writer.WriteBool(this.FloorAddedToDeck.HasValue);
    int? floorAddedToDeck = this.FloorAddedToDeck;
    if (!floorAddedToDeck.HasValue)
      return;
    PacketWriter packetWriter = writer;
    floorAddedToDeck = this.FloorAddedToDeck;
    int val = floorAddedToDeck.Value;
    packetWriter.WriteInt(val, 8);
  }

  public void Deserialize(PacketReader reader)
  {
    this.Id = reader.ReadModelIdAssumingType<CardModel>();
    this.CurrentUpgradeLevel = reader.ReadInt(8);
    if (reader.ReadBool())
      this.Enchantment = reader.Read<SerializableEnchantment>();
    if (reader.ReadBool())
      this.Props = reader.Read<SavedProperties>();
    if (!reader.ReadBool())
      return;
    this.FloorAddedToDeck = new int?(reader.ReadInt(8));
  }

  public override bool Equals(object? obj)
  {
    if (obj == null || obj.GetType() != this.GetType())
      return false;
    SerializableCard serializableCard = (SerializableCard) obj;
    return this.Id.Equals(serializableCard.Id) && this.CurrentUpgradeLevel == serializableCard.CurrentUpgradeLevel && object.Equals((object) this.Enchantment, (object) serializableCard.Enchantment);
  }

  public override int GetHashCode()
  {
    return HashCode.Combine<ModelId, int, SerializableEnchantment>(this.Id, this.CurrentUpgradeLevel, this.Enchantment);
  }

  public override string ToString()
  {
    return $"SerializableCard {this.Id}. Upgrades: {this.CurrentUpgradeLevel} Enchantment: {this.Enchantment} Props: {this.Props} Floor: {this.FloorAddedToDeck}";
  }
}
