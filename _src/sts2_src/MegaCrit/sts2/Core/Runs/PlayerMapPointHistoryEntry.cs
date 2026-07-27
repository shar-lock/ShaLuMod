// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Runs.PlayerMapPointHistoryEntry
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Runs.History;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Runs;

public class PlayerMapPointHistoryEntry : IPacketSerializable
{
  [JsonPropertyName("player_id")]
  public ulong PlayerId { get; set; }

  [JsonPropertyName("gold_gained")]
  public int GoldGained { get; set; }

  [JsonPropertyName("gold_spent")]
  public int GoldSpent { get; set; }

  [JsonPropertyName("gold_lost")]
  public int GoldLost { get; set; }

  [JsonPropertyName("gold_stolen")]
  public int GoldStolen { get; set; }

  [JsonPropertyName("stolen_loot")]
  public int StolenLoot { get; set; }

  [JsonPropertyName("current_gold")]
  public int CurrentGold { get; set; }

  [JsonPropertyName("current_hp")]
  public int CurrentHp { get; set; }

  [JsonPropertyName("max_hp")]
  public int MaxHp { get; set; }

  [JsonPropertyName("damage_taken")]
  public int DamageTaken { get; set; }

  [JsonPropertyName("hp_healed")]
  public int HpHealed { get; set; }

  [JsonPropertyName("max_hp_lost")]
  public int MaxHpLost { get; set; }

  [JsonPropertyName("max_hp_gained")]
  public int MaxHpGained { get; set; }

  [JsonPropertyName("ancient_choice")]
  [JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
  public List<AncientChoiceHistoryEntry> AncientChoices { get; set; } = new List<AncientChoiceHistoryEntry>();

  [JsonPropertyName("cards_gained")]
  [JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
  public List<SerializableCard> CardsGained { get; set; } = new List<SerializableCard>();

  [JsonPropertyName("card_choices")]
  [JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
  public List<CardChoiceHistoryEntry> CardChoices { get; set; } = new List<CardChoiceHistoryEntry>();

  [JsonPropertyName("relic_choices")]
  [JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
  public List<ModelChoiceHistoryEntry> RelicChoices { get; set; } = new List<ModelChoiceHistoryEntry>();

  [JsonPropertyName("potion_choices")]
  [JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
  public List<ModelChoiceHistoryEntry> PotionChoices { get; set; } = new List<ModelChoiceHistoryEntry>();

  [JsonPropertyName("potion_discarded")]
  [JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
  public List<ModelId> PotionDiscarded { get; set; } = new List<ModelId>();

  [JsonPropertyName("potion_used")]
  [JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
  public List<ModelId> PotionUsed { get; set; } = new List<ModelId>();

  [JsonPropertyName("cards_removed")]
  [JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
  public List<SerializableCard> CardsRemoved { get; set; } = new List<SerializableCard>();

  [JsonPropertyName("relics_removed")]
  [JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
  public List<ModelId> RelicsRemoved { get; set; } = new List<ModelId>();

  [JsonPropertyName("cards_enchanted")]
  [JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
  public List<CardEnchantmentHistoryEntry> CardsEnchanted { get; set; } = new List<CardEnchantmentHistoryEntry>();

  [JsonPropertyName("cards_transformed")]
  [JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
  public List<CardTransformationHistoryEntry> CardsTransformed { get; set; } = new List<CardTransformationHistoryEntry>();

  [JsonPropertyName("upgraded_cards")]
  [JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
  public List<ModelId> UpgradedCards { get; set; } = new List<ModelId>();

  [JsonPropertyName("downgraded_cards")]
  [JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
  public List<ModelId> DowngradedCards { get; set; } = new List<ModelId>();

  [JsonPropertyName("event_choices")]
  [JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
  public List<EventOptionHistoryEntry> EventChoices { get; set; } = new List<EventOptionHistoryEntry>();

  [JsonPropertyName("rest_site_choices")]
  [JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
  public List<string> RestSiteChoices { get; set; } = new List<string>();

  [JsonPropertyName("bought_relics")]
  [JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
  public List<ModelId> BoughtRelics { get; set; } = new List<ModelId>();

  [JsonPropertyName("bought_potions")]
  [JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
  public List<ModelId> BoughtPotions { get; set; } = new List<ModelId>();

  [JsonPropertyName("bought_colorless")]
  [JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
  public List<ModelId> BoughtColorless { get; set; } = new List<ModelId>();

  [JsonPropertyName("completed_quests")]
  [JsonSerializeCondition(SerializationCondition.SaveIfNotCollectionEmptyOrNull)]
  public List<ModelId> CompletedQuests { get; set; } = new List<ModelId>();

  [JsonPropertyName("is_affected_by_fur_coat")]
  public bool IsAffectedByFurCoat { get; set; }

  public LocString? GetAncientPickedChoiceLoc()
  {
    return this.AncientChoices.FirstOrDefault<AncientChoiceHistoryEntry>((Func<AncientChoiceHistoryEntry, bool>) (o => o.WasChosen))?.Title;
  }

  public List<LocString> GetAncientSkippedChoiceLoc()
  {
    return this.AncientChoices.Where<AncientChoiceHistoryEntry>((Func<AncientChoiceHistoryEntry, bool>) (o => !o.WasChosen)).Select<AncientChoiceHistoryEntry, LocString>((Func<AncientChoiceHistoryEntry, LocString>) (o => o.Title)).ToList<LocString>();
  }

  [JsonIgnore]
  public bool WasMugged => this.StolenLoot > 0;

  public void MarkLootStolen(int amount = 1) => this.StolenLoot += amount;

  public void MarkLootReturned(int amount = 1)
  {
    this.StolenLoot = Math.Max(0, this.StolenLoot - amount);
  }

  public void Serialize(PacketWriter writer)
  {
    writer.WriteULong(this.PlayerId);
    writer.WriteInt(this.GoldGained);
    writer.WriteInt(this.GoldSpent);
    writer.WriteInt(this.GoldLost);
    writer.WriteInt(this.GoldStolen);
    writer.WriteInt(this.StolenLoot);
    writer.WriteInt(this.CurrentGold);
    writer.WriteInt(this.CurrentHp);
    writer.WriteInt(this.MaxHp);
    writer.WriteInt(this.DamageTaken);
    writer.WriteInt(this.HpHealed);
    writer.WriteInt(this.MaxHpGained);
    writer.WriteInt(this.MaxHpLost);
    writer.WriteList<EventOptionHistoryEntry>((IReadOnlyList<EventOptionHistoryEntry>) this.EventChoices);
    writer.WriteList<AncientChoiceHistoryEntry>((IReadOnlyList<AncientChoiceHistoryEntry>) this.AncientChoices);
    writer.WriteList<SerializableCard>((IReadOnlyList<SerializableCard>) this.CardsGained);
    writer.WriteInt(this.CardChoices.Count);
    foreach (CardChoiceHistoryEntry cardChoice in this.CardChoices)
      cardChoice.Serialize<CardModel>(writer);
    writer.WriteInt(this.RelicChoices.Count);
    foreach (ModelChoiceHistoryEntry relicChoice in this.RelicChoices)
      relicChoice.Serialize<RelicModel>(writer);
    writer.WriteInt(this.PotionChoices.Count);
    foreach (ModelChoiceHistoryEntry potionChoice in this.PotionChoices)
      potionChoice.Serialize<PotionModel>(writer);
    writer.WriteModelEntriesInList((IReadOnlyCollection<ModelId>) this.PotionDiscarded);
    writer.WriteModelEntriesInList((IReadOnlyCollection<ModelId>) this.PotionUsed);
    writer.WriteList<SerializableCard>((IReadOnlyList<SerializableCard>) this.CardsRemoved);
    writer.WriteModelEntriesInList((IReadOnlyCollection<ModelId>) this.RelicsRemoved);
    writer.WriteList<CardEnchantmentHistoryEntry>((IReadOnlyList<CardEnchantmentHistoryEntry>) this.CardsEnchanted);
    writer.WriteList<CardTransformationHistoryEntry>((IReadOnlyList<CardTransformationHistoryEntry>) this.CardsTransformed);
    writer.WriteModelEntriesInList((IReadOnlyCollection<ModelId>) this.UpgradedCards);
    writer.WriteModelEntriesInList((IReadOnlyCollection<ModelId>) this.DowngradedCards);
    writer.WriteList<EventOptionHistoryEntry>((IReadOnlyList<EventOptionHistoryEntry>) this.EventChoices);
    writer.WriteInt(this.RestSiteChoices.Count);
    foreach (string restSiteChoice in this.RestSiteChoices)
      writer.WriteString(restSiteChoice);
    writer.WriteModelEntriesInList((IReadOnlyCollection<ModelId>) this.BoughtRelics);
    writer.WriteModelEntriesInList((IReadOnlyCollection<ModelId>) this.BoughtPotions);
    writer.WriteModelEntriesInList((IReadOnlyCollection<ModelId>) this.BoughtColorless);
    writer.WriteModelEntriesInList((IReadOnlyCollection<ModelId>) this.CompletedQuests);
    writer.WriteBool(this.IsAffectedByFurCoat);
  }

  public void Deserialize(PacketReader reader)
  {
    this.PlayerId = reader.ReadULong();
    this.GoldGained = reader.ReadInt();
    this.GoldSpent = reader.ReadInt();
    this.GoldLost = reader.ReadInt();
    this.GoldStolen = reader.ReadInt();
    this.StolenLoot = reader.ReadInt();
    this.CurrentGold = reader.ReadInt();
    this.CurrentHp = reader.ReadInt();
    this.MaxHp = reader.ReadInt();
    this.DamageTaken = reader.ReadInt();
    this.HpHealed = reader.ReadInt();
    this.MaxHpGained = reader.ReadInt();
    this.MaxHpLost = reader.ReadInt();
    this.EventChoices = reader.ReadList<EventOptionHistoryEntry>();
    this.AncientChoices = reader.ReadList<AncientChoiceHistoryEntry>();
    this.CardsGained = reader.ReadList<SerializableCard>();
    int num1 = reader.ReadInt();
    for (int index = 0; index < num1; ++index)
    {
      CardChoiceHistoryEntry choiceHistoryEntry = new CardChoiceHistoryEntry();
      choiceHistoryEntry.Deserialize<CardModel>(reader);
      this.CardChoices.Add(choiceHistoryEntry);
    }
    int num2 = reader.ReadInt();
    for (int index = 0; index < num2; ++index)
    {
      ModelChoiceHistoryEntry choiceHistoryEntry = new ModelChoiceHistoryEntry();
      choiceHistoryEntry.Deserialize<RelicModel>(reader);
      this.RelicChoices.Add(choiceHistoryEntry);
    }
    int num3 = reader.ReadInt();
    for (int index = 0; index < num3; ++index)
    {
      ModelChoiceHistoryEntry choiceHistoryEntry = new ModelChoiceHistoryEntry();
      choiceHistoryEntry.Deserialize<PotionModel>(reader);
      this.PotionChoices.Add(choiceHistoryEntry);
    }
    this.PotionDiscarded = reader.ReadModelIdListAssumingType<PotionModel>();
    this.PotionUsed = reader.ReadModelIdListAssumingType<PotionModel>();
    this.CardsRemoved = reader.ReadList<SerializableCard>();
    this.RelicsRemoved = reader.ReadModelIdListAssumingType<RelicModel>();
    this.CardsEnchanted = reader.ReadList<CardEnchantmentHistoryEntry>();
    this.CardsTransformed = reader.ReadList<CardTransformationHistoryEntry>();
    this.UpgradedCards = reader.ReadModelIdListAssumingType<CardModel>();
    this.DowngradedCards = reader.ReadModelIdListAssumingType<CardModel>();
    this.EventChoices = reader.ReadList<EventOptionHistoryEntry>();
    int num4 = reader.ReadInt();
    for (int index = 0; index < num4; ++index)
      this.RestSiteChoices.Add(reader.ReadString());
    this.BoughtRelics = reader.ReadModelIdListAssumingType<RelicModel>();
    this.BoughtPotions = reader.ReadModelIdListAssumingType<PotionModel>();
    this.BoughtColorless = reader.ReadModelIdListAssumingType<CardModel>();
    this.CompletedQuests = reader.ReadModelIdListAssumingType<CardModel>();
    this.IsAffectedByFurCoat = reader.ReadBool();
  }

  public PlayerMapPointHistoryEntry Anonymized()
  {
    return new PlayerMapPointHistoryEntry()
    {
      PlayerId = IdAnonymizer.Anonymize(this.PlayerId),
      GoldGained = this.GoldGained,
      GoldSpent = this.GoldSpent,
      GoldLost = this.GoldLost,
      GoldStolen = this.GoldStolen,
      StolenLoot = this.StolenLoot,
      CurrentGold = this.CurrentGold,
      CurrentHp = this.CurrentHp,
      MaxHp = this.MaxHp,
      DamageTaken = this.DamageTaken,
      HpHealed = this.HpHealed,
      MaxHpLost = this.MaxHpLost,
      MaxHpGained = this.MaxHpGained,
      AncientChoices = this.AncientChoices,
      CardsGained = this.CardsGained,
      CardChoices = this.CardChoices,
      RelicChoices = this.RelicChoices,
      PotionChoices = this.PotionChoices,
      PotionDiscarded = this.PotionDiscarded,
      PotionUsed = this.PotionUsed,
      CardsRemoved = this.CardsRemoved,
      RelicsRemoved = this.RelicsRemoved,
      CardsEnchanted = this.CardsEnchanted,
      CardsTransformed = this.CardsTransformed,
      UpgradedCards = this.UpgradedCards,
      DowngradedCards = this.DowngradedCards,
      EventChoices = this.EventChoices,
      RestSiteChoices = this.RestSiteChoices,
      BoughtRelics = this.BoughtRelics,
      BoughtPotions = this.BoughtPotions,
      BoughtColorless = this.BoughtColorless,
      CompletedQuests = this.CompletedQuests
    };
  }
}
