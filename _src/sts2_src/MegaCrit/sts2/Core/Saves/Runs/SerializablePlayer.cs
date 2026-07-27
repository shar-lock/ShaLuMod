// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayer
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Unlocks;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves.Runs;

public class SerializablePlayer : IPacketSerializable
{
  [JsonPropertyName("character_id")]
  public ModelId? CharacterId { get; set; }

  [JsonPropertyName("current_hp")]
  public int CurrentHp { get; set; }

  [JsonPropertyName("max_hp")]
  public int MaxHp { get; set; }

  [JsonPropertyName("max_energy")]
  public int MaxEnergy { get; set; }

  [JsonPropertyName("max_potion_slot_count")]
  public int MaxPotionSlotCount { get; set; } = 3;

  [JsonPropertyName("gold")]
  public int Gold { get; set; }

  [JsonPropertyName("base_orb_slot_count")]
  public int BaseOrbSlotCount { get; set; }

  [JsonPropertyName("net_id")]
  public ulong NetId { get; set; }

  [JsonPropertyName("deck")]
  [JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
  public List<SerializableCard> Deck { get; set; } = new List<SerializableCard>();

  [JsonPropertyName("relics")]
  [JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
  public List<SerializableRelic> Relics { get; set; } = new List<SerializableRelic>();

  [JsonPropertyName("potions")]
  [JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
  public List<SerializablePotion> Potions { get; set; } = new List<SerializablePotion>();

  [JsonPropertyName("rng")]
  public SerializablePlayerRngSet Rng { get; set; }

  [JsonPropertyName("odds")]
  public SerializablePlayerOddsSet Odds { get; set; }

  [JsonPropertyName("relic_grab_bag")]
  public SerializableRelicGrabBag RelicGrabBag { get; set; }

  [JsonPropertyName("extra_fields")]
  public SerializableExtraPlayerFields ExtraFields { get; set; }

  [JsonPropertyName("unlock_state")]
  public SerializableUnlockState UnlockState { get; set; }

  [JsonPropertyName("discovered_cards")]
  [JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
  public List<ModelId> DiscoveredCards { get; set; } = new List<ModelId>();

  [JsonPropertyName("discovered_enemies")]
  [JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
  public List<ModelId> DiscoveredEnemies { get; set; } = new List<ModelId>();

  [JsonPropertyName("discovered_epochs")]
  [JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
  [JsonConverter(typeof (EpochIdListConverter))]
  public List<string> DiscoveredEpochs { get; set; } = new List<string>();

  [JsonPropertyName("discovered_potions")]
  [JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
  public List<ModelId> DiscoveredPotions { get; set; } = new List<ModelId>();

  [JsonPropertyName("discovered_relics")]
  [JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
  public List<ModelId> DiscoveredRelics { get; set; } = new List<ModelId>();

  public void Serialize(PacketWriter writer)
  {
    writer.WriteULong(this.NetId);
    writer.WriteModelEntry(this.CharacterId);
    writer.WriteInt(this.CurrentHp);
    writer.WriteInt(this.MaxHp);
    writer.WriteInt(this.MaxEnergy, 16 /*0x10*/);
    writer.WriteInt(this.MaxPotionSlotCount, 8);
    writer.WriteInt(this.Gold);
    writer.WriteInt(this.BaseOrbSlotCount, 16 /*0x10*/);
    writer.WriteList<SerializableCard>((IReadOnlyList<SerializableCard>) this.Deck);
    writer.WriteList<SerializableRelic>((IReadOnlyList<SerializableRelic>) this.Relics);
    writer.WriteList<SerializablePotion>((IReadOnlyList<SerializablePotion>) this.Potions);
    writer.Write<SerializablePlayerRngSet>(this.Rng);
    writer.Write<SerializablePlayerOddsSet>(this.Odds);
    writer.Write<SerializableRelicGrabBag>(this.RelicGrabBag);
    writer.Write<SerializableExtraPlayerFields>(this.ExtraFields);
    writer.Write<SerializableUnlockState>(this.UnlockState);
    writer.WriteFullModelIdList((IReadOnlyCollection<ModelId>) this.DiscoveredCards);
    writer.WriteFullModelIdList((IReadOnlyCollection<ModelId>) this.DiscoveredEnemies);
    writer.WriteInt(this.DiscoveredEpochs.Count);
    foreach (string discoveredEpoch in this.DiscoveredEpochs)
      writer.WriteEpochId(discoveredEpoch);
    writer.WriteFullModelIdList((IReadOnlyCollection<ModelId>) this.DiscoveredPotions);
    writer.WriteFullModelIdList((IReadOnlyCollection<ModelId>) this.DiscoveredRelics);
  }

  public void Deserialize(PacketReader reader)
  {
    this.NetId = reader.ReadULong();
    this.CharacterId = reader.ReadModelIdAssumingType<CharacterModel>();
    this.CurrentHp = reader.ReadInt();
    this.MaxHp = reader.ReadInt();
    this.MaxEnergy = reader.ReadInt(16 /*0x10*/);
    this.MaxPotionSlotCount = reader.ReadInt(8);
    this.Gold = reader.ReadInt();
    this.BaseOrbSlotCount = reader.ReadInt(16 /*0x10*/);
    this.Deck = reader.ReadList<SerializableCard>();
    this.Relics = reader.ReadList<SerializableRelic>();
    this.Potions = reader.ReadList<SerializablePotion>();
    this.Rng = reader.Read<SerializablePlayerRngSet>();
    this.Odds = reader.Read<SerializablePlayerOddsSet>();
    this.RelicGrabBag = reader.Read<SerializableRelicGrabBag>();
    this.ExtraFields = reader.Read<SerializableExtraPlayerFields>();
    this.UnlockState = reader.Read<SerializableUnlockState>();
    this.DiscoveredCards = reader.ReadFullModelIdList();
    this.DiscoveredEnemies = reader.ReadFullModelIdList();
    int num = reader.ReadInt();
    for (int index = 0; index < num; ++index)
      this.DiscoveredEpochs.Add(reader.ReadEpochId());
    this.DiscoveredPotions = reader.ReadFullModelIdList();
    this.DiscoveredRelics = reader.ReadFullModelIdList();
  }

  public SerializablePlayer Anonymized()
  {
    return new SerializablePlayer()
    {
      CharacterId = this.CharacterId,
      CurrentHp = this.CurrentHp,
      MaxHp = this.MaxHp,
      MaxEnergy = this.MaxEnergy,
      MaxPotionSlotCount = this.MaxPotionSlotCount,
      Gold = this.Gold,
      BaseOrbSlotCount = this.BaseOrbSlotCount,
      NetId = IdAnonymizer.Anonymize(this.NetId),
      Deck = this.Deck,
      Relics = this.Relics,
      Potions = this.Potions,
      Rng = this.Rng,
      Odds = this.Odds,
      RelicGrabBag = this.RelicGrabBag,
      ExtraFields = this.ExtraFields,
      UnlockState = this.UnlockState,
      DiscoveredCards = this.DiscoveredCards,
      DiscoveredEnemies = this.DiscoveredEnemies,
      DiscoveredEpochs = this.DiscoveredEpochs,
      DiscoveredPotions = this.DiscoveredPotions,
      DiscoveredRelics = this.DiscoveredRelics
    };
  }
}
