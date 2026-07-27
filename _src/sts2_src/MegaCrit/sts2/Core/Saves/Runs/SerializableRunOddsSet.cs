// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.Runs.SerializableRunOddsSet
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves.Runs;

public class SerializableRunOddsSet : IPacketSerializable
{
  [JsonPropertyName("unknown_map_point_monster_odds_value")]
  public float UnknownMapPointMonsterOddsValue { get; set; }

  [JsonPropertyName("unknown_map_point_elite_odds_value")]
  public float UnknownMapPointEliteOddsValue { get; set; }

  [JsonPropertyName("unknown_map_point_treasure_odds_value")]
  public float UnknownMapPointTreasureOddsValue { get; set; }

  [JsonPropertyName("unknown_map_point_shop_odds_value")]
  public float UnknownMapPointShopOddsValue { get; set; }

  public void Serialize(PacketWriter writer)
  {
    writer.WriteFloat(this.UnknownMapPointMonsterOddsValue);
    writer.WriteFloat(this.UnknownMapPointEliteOddsValue);
    writer.WriteFloat(this.UnknownMapPointTreasureOddsValue);
    writer.WriteFloat(this.UnknownMapPointShopOddsValue);
  }

  public void Deserialize(PacketReader reader)
  {
    this.UnknownMapPointMonsterOddsValue = reader.ReadFloat();
    this.UnknownMapPointEliteOddsValue = reader.ReadFloat();
    this.UnknownMapPointTreasureOddsValue = reader.ReadFloat();
    this.UnknownMapPointShopOddsValue = reader.ReadFloat();
  }
}
