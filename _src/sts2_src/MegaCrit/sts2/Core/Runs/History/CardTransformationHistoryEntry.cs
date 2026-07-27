// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Runs.History.CardTransformationHistoryEntry
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Saves.Runs;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Runs.History;

public struct CardTransformationHistoryEntry : IPacketSerializable
{
  public CardTransformationHistoryEntry(CardModel originalCard, CardModel finalCard)
  {
    // ISSUE: reference to a compiler-generated field
    this.\u003COriginalCard\u003Ek__BackingField = originalCard.ToSerializable();
    // ISSUE: reference to a compiler-generated field
    this.\u003CFinalCard\u003Ek__BackingField = finalCard.ToSerializable();
  }

  [JsonPropertyName("original_card")]
  public SerializableCard OriginalCard { get; set; }

  [JsonPropertyName("final_card")]
  public SerializableCard FinalCard { get; set; }

  public void Serialize(PacketWriter writer)
  {
    writer.Write<SerializableCard>(this.OriginalCard);
    writer.Write<SerializableCard>(this.FinalCard);
  }

  public void Deserialize(PacketReader reader)
  {
    this.OriginalCard = reader.Read<SerializableCard>();
    this.FinalCard = reader.Read<SerializableCard>();
  }
}
