// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Multiplayer.NetDeckCard
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.Multiplayer;

public struct NetDeckCard : IPacketSerializable
{
  public uint DeckIndex { get; private set; }

  public static NetDeckCard FromModel(CardModel card)
  {
    if (!card.IsMutable)
      throw new InvalidOperationException("Immutable instances of CardModel should not be serialized through NetDeckCard! Send the ModelId instead.");
    if (card.Pile == null)
      throw new InvalidOperationException("Cannot use NetDeckCard to serialize a card without a pile!");
    if (card.Pile.Type != PileType.Deck)
      throw new InvalidOperationException($"Cannot use {nameof (NetDeckCard)} to serialize card {card} that is in a non-deck pile!");
    return new NetDeckCard()
    {
      DeckIndex = (uint) card.Pile.Cards.IndexOf<CardModel>(card)
    };
  }

  public readonly CardModel ToCardModel(Player player) => player.Deck.Cards[(int) this.DeckIndex];

  public void Serialize(PacketWriter writer) => writer.WriteUInt(this.DeckIndex, 16 /*0x10*/);

  public void Deserialize(PacketReader reader) => this.DeckIndex = reader.ReadUInt(16 /*0x10*/);

  public override string ToString() => $"{nameof (NetDeckCard)} {this.DeckIndex}";
}
