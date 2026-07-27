// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Multiplayer.NetPlayerChoiceResult
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Models;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.Multiplayer;

public struct NetPlayerChoiceResult : IPacketSerializable
{
  public PlayerChoiceType type;
  public List<CardModel>? canonicalCards;
  public List<NetCombatCard>? combatCards;
  public List<NetDeckCard>? deckCards;
  public List<SerializableCard>? mutableCards;
  public ulong? mutableCardOwner;
  public List<int>? indexes;
  public ulong? playerId;

  public void Serialize(PacketWriter writer)
  {
    writer.WriteEnum<PlayerChoiceType>(this.type);
    switch (this.type)
    {
      case PlayerChoiceType.CanonicalCard:
        writer.WriteInt(this.canonicalCards.Count);
        using (List<CardModel>.Enumerator enumerator = this.canonicalCards.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            CardModel current = enumerator.Current;
            writer.WriteModel<CardModel>(current);
          }
          break;
        }
      case PlayerChoiceType.CombatCard:
        writer.WriteList<NetCombatCard>((IReadOnlyList<NetCombatCard>) this.combatCards);
        break;
      case PlayerChoiceType.DeckCard:
        writer.WriteList<NetDeckCard>((IReadOnlyList<NetDeckCard>) this.deckCards);
        break;
      case PlayerChoiceType.MutableCard:
        writer.WriteList<SerializableCard>((IReadOnlyList<SerializableCard>) this.mutableCards);
        writer.WriteBool(this.mutableCardOwner.HasValue);
        if (!this.mutableCardOwner.HasValue)
          break;
        writer.WriteULong(this.mutableCardOwner.Value);
        break;
      case PlayerChoiceType.Player:
        writer.WriteBool(this.playerId.HasValue);
        if (!this.playerId.HasValue)
          break;
        writer.WriteULong(this.playerId.Value);
        break;
      case PlayerChoiceType.Index:
        writer.WriteInt(this.indexes.Count);
        using (List<int>.Enumerator enumerator = this.indexes.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            int current = enumerator.Current;
            writer.WriteInt(current);
          }
          break;
        }
      default:
        throw new ArgumentOutOfRangeException();
    }
  }

  public void Deserialize(PacketReader reader)
  {
    this.type = reader.ReadEnum<PlayerChoiceType>();
    switch (this.type)
    {
      case PlayerChoiceType.CanonicalCard:
        this.canonicalCards = new List<CardModel>();
        int num1 = reader.ReadInt();
        for (int index = 0; index < num1; ++index)
          this.canonicalCards.Add(reader.ReadModel<CardModel>());
        break;
      case PlayerChoiceType.CombatCard:
        this.combatCards = reader.ReadList<NetCombatCard>();
        break;
      case PlayerChoiceType.DeckCard:
        this.deckCards = reader.ReadList<NetDeckCard>();
        break;
      case PlayerChoiceType.MutableCard:
        this.mutableCards = reader.ReadList<SerializableCard>();
        if (!reader.ReadBool())
          break;
        this.mutableCardOwner = new ulong?(reader.ReadULong());
        break;
      case PlayerChoiceType.Player:
        if (!reader.ReadBool())
          break;
        this.playerId = new ulong?(reader.ReadULong());
        break;
      case PlayerChoiceType.Index:
        this.indexes = new List<int>();
        int num2 = reader.ReadInt();
        for (int index = 0; index < num2; ++index)
          this.indexes.Add(reader.ReadInt());
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
  }

  public override string ToString()
  {
    switch (this.type)
    {
      case PlayerChoiceType.CanonicalCard:
        return "NetPlayerChoiceResult canonical " + string.Join<CardModel>(",", (IEnumerable<CardModel>) (this.canonicalCards ?? new List<CardModel>()));
      case PlayerChoiceType.CombatCard:
        return "NetPlayerChoiceResult combat " + string.Join<NetCombatCard>(",", (IEnumerable<NetCombatCard>) (this.combatCards ?? new List<NetCombatCard>()));
      case PlayerChoiceType.DeckCard:
        return "NetPlayerChoiceResult deck " + string.Join<NetDeckCard>(",", (IEnumerable<NetDeckCard>) (this.deckCards ?? new List<NetDeckCard>()));
      case PlayerChoiceType.MutableCard:
        return $"{nameof (NetPlayerChoiceResult)} mutable cards {string.Join<SerializableCard>(",", (IEnumerable<SerializableCard>) (this.mutableCards ?? new List<SerializableCard>()))}, owner: {this.mutableCardOwner}";
      case PlayerChoiceType.Player:
        return $"{nameof (NetPlayerChoiceResult)} player ID {this.playerId}";
      case PlayerChoiceType.Index:
        return "NetPlayerChoiceResult indexes " + string.Join<int>(",", (IEnumerable<int>) (this.indexes ?? new List<int>()));
      default:
        throw new ArgumentOutOfRangeException();
    }
  }
}
