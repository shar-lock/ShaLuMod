// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.Runs.SerializablePlayerOddsSet
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves.Runs;

public class SerializablePlayerOddsSet : IPacketSerializable
{
  [JsonPropertyName("card_rarity_odds_value")]
  public float CardRarityOddsValue { get; set; }

  [JsonPropertyName("potion_reward_odds_value")]
  public float PotionRewardOddsValue { get; set; }

  public void Serialize(PacketWriter writer)
  {
    writer.WriteFloat(this.CardRarityOddsValue);
    writer.WriteFloat(this.PotionRewardOddsValue);
  }

  public void Deserialize(PacketReader reader)
  {
    this.CardRarityOddsValue = reader.ReadFloat();
    this.PotionRewardOddsValue = reader.ReadFloat();
  }
}
