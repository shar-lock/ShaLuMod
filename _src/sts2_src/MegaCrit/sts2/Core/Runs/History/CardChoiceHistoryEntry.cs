// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Runs.History.CardChoiceHistoryEntry
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Runs.History;

[Serializable]
public struct CardChoiceHistoryEntry
{
  [JsonPropertyName("was_picked")]
  public bool wasPicked;

  [JsonPropertyName("card")]
  public SerializableCard Card { get; set; }

  public CardChoiceHistoryEntry(CardModel card, bool wasPicked)
  {
    this.Card = card.ToSerializable();
    this.wasPicked = wasPicked;
  }

  public void Serialize<T>(PacketWriter writer) where T : AbstractModel
  {
    writer.Write<SerializableCard>(this.Card);
    writer.WriteBool(this.wasPicked);
  }

  public void Deserialize<T>(PacketReader reader) where T : AbstractModel
  {
    this.Card = reader.Read<SerializableCard>();
    this.wasPicked = reader.ReadBool();
  }
}
