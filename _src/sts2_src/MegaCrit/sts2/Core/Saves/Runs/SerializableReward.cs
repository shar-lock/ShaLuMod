// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.Runs.SerializableReward
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves.Runs;

public class SerializableReward : IPacketSerializable
{
  [JsonPropertyName("reward_type")]
  public RewardType RewardType { get; set; }

  [JsonPropertyName("predetermined_model_id")]
  public ModelId PredeterminedModelId { get; set; } = ModelId.none;

  [JsonPropertyName("special_card")]
  public SerializableCard? SpecialCard { get; set; }

  [JsonPropertyName("gold_amount")]
  public int GoldAmount { get; set; }

  [JsonPropertyName("was_gold_stolen_back")]
  public bool WasGoldStolenBack { get; set; }

  [JsonPropertyName("source")]
  public CardCreationSource Source { get; set; }

  [JsonPropertyName("rarity_odds")]
  public CardRarityOddsType RarityOdds { get; set; }

  [JsonPropertyName("card_pools")]
  public List<ModelId> CardPoolIds { get; set; } = new List<ModelId>();

  [JsonPropertyName("option_count")]
  public int OptionCount { get; set; }

  [JsonPropertyName("custom_description_encounter_source_id")]
  public ModelId CustomDescriptionEncounterSourceId { get; set; } = ModelId.none;

  public void Serialize(PacketWriter writer)
  {
    writer.WriteInt((int) this.RewardType);
    writer.WriteFullModelId(this.PredeterminedModelId);
    if (this.RewardType == RewardType.SpecialCard)
      writer.Write<SerializableCard>(this.SpecialCard);
    writer.WriteInt(this.GoldAmount);
    writer.WriteBool(this.WasGoldStolenBack);
    writer.WriteEnum<CardCreationSource>(this.Source);
    writer.WriteEnum<CardRarityOddsType>(this.RarityOdds);
    writer.WriteModelEntriesInList((IReadOnlyCollection<ModelId>) this.CardPoolIds);
    writer.WriteInt(this.OptionCount);
    writer.WriteModelEntry(this.CustomDescriptionEncounterSourceId);
  }

  public void Deserialize(PacketReader reader)
  {
    this.RewardType = (RewardType) reader.ReadInt();
    this.PredeterminedModelId = reader.ReadFullModelId();
    if (this.RewardType == RewardType.SpecialCard)
      this.SpecialCard = reader.Read<SerializableCard>();
    this.GoldAmount = reader.ReadInt();
    this.WasGoldStolenBack = reader.ReadBool();
    this.Source = reader.ReadEnum<CardCreationSource>();
    this.RarityOdds = reader.ReadEnum<CardRarityOddsType>();
    this.CardPoolIds = reader.ReadModelIdListAssumingType<CardPoolModel>();
    this.OptionCount = reader.ReadInt();
    this.CustomDescriptionEncounterSourceId = reader.ReadModelIdAssumingType<EncounterModel>();
  }
}
