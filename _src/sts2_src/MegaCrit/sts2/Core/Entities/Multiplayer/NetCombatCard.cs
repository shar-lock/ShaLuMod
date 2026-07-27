// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Multiplayer.NetCombatCard
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.Multiplayer;

public struct NetCombatCard : IPacketSerializable, IEquatable<NetCombatCard>
{
  public uint CombatCardIndex { get; private set; }

  public static NetCombatCard ForTesting(uint index)
  {
    return new NetCombatCard() { CombatCardIndex = index };
  }

  public static NetCombatCard FromModel(CardModel card)
  {
    CardPile cardPile = card.IsMutable ? card.Pile : throw new InvalidOperationException("Immutable instances of CardModel should not be serialized through NetCombatCard! Send the ModelId instead.");
    if (cardPile != null && !cardPile.IsCombatPile)
      throw new InvalidOperationException("Cannot use NetCombatCard to serialize a card in a non-combat pile!");
    return new NetCombatCard()
    {
      CombatCardIndex = NetCombatCardDb.Instance.GetCardId(card)
    };
  }

  public readonly CardModel ToCardModel() => NetCombatCardDb.Instance.GetCard(this.CombatCardIndex);

  public readonly CardModel? ToCardModelOrNull()
  {
    CardModel card;
    NetCombatCardDb.Instance.TryGetCard(this.CombatCardIndex, out card);
    return card;
  }

  public void Serialize(PacketWriter writer) => writer.WriteUInt(this.CombatCardIndex, 16 /*0x10*/);

  public void Deserialize(PacketReader reader)
  {
    this.CombatCardIndex = reader.ReadUInt(16 /*0x10*/);
  }

  public bool Equals(NetCombatCard other)
  {
    return (int) this.CombatCardIndex == (int) other.CombatCardIndex;
  }

  public static bool operator ==(NetCombatCard c1, NetCombatCard c2) => c1.Equals(c2);

  public static bool operator !=(NetCombatCard c1, NetCombatCard c2) => !c1.Equals(c2);

  public override bool Equals(object? obj) => obj is NetCombatCard other && this.Equals(other);

  public override int GetHashCode() => this.CombatCardIndex.GetHashCode();

  public override string ToString() => $"{nameof (NetCombatCard)} {this.CombatCardIndex}";
}
