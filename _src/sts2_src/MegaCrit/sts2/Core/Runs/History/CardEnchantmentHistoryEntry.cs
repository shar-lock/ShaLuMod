// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Runs.History.CardEnchantmentHistoryEntry
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Saves.Runs;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Runs.History;

public struct CardEnchantmentHistoryEntry : IPacketSerializable
{
  public CardEnchantmentHistoryEntry(CardModel card, ModelId enchantment)
  {
    // ISSUE: reference to a compiler-generated field
    this.\u003CCard\u003Ek__BackingField = card.ToSerializable();
    // ISSUE: reference to a compiler-generated field
    this.\u003CEnchantment\u003Ek__BackingField = enchantment;
  }

  [JsonPropertyName("card")]
  public SerializableCard Card { get; set; }

  [JsonPropertyName("enchantment")]
  public ModelId Enchantment { get; set; }

  public void Serialize(PacketWriter writer)
  {
    writer.Write<SerializableCard>(this.Card);
    writer.WriteModelEntry(this.Enchantment);
  }

  public void Deserialize(PacketReader reader)
  {
    this.Card = reader.Read<SerializableCard>();
    this.Enchantment = reader.ReadModelIdAssumingType<EnchantmentModel>();
  }
}
