// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.SerializableExtraPlayerFields
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves;

public class SerializableExtraPlayerFields : IPacketSerializable
{
  [JsonPropertyName("card_shop_removals_used")]
  [JsonIgnore]
  public int CardShopRemovalsUsed { get; set; }

  [JsonPropertyName("wongo_points")]
  [JsonIgnore]
  public int WongoPoints { get; set; }

  [JsonPropertyName("ccccombo_badge_unlocked")]
  [JsonIgnore]
  public bool CccomboBadgeUnlocked { get; set; }

  [JsonPropertyName("damage_dealt")]
  [JsonIgnore]
  public int DamageDealt { get; set; }

  [JsonPropertyName("debuffs_applied")]
  [JsonIgnore]
  public int DebuffsApplied { get; set; }

  public void Serialize(PacketWriter writer)
  {
    writer.WriteInt(this.CardShopRemovalsUsed);
    writer.WriteInt(this.WongoPoints);
    writer.WriteBool(this.CccomboBadgeUnlocked);
    writer.WriteInt(this.DamageDealt);
    writer.WriteInt(this.DebuffsApplied);
  }

  public void Deserialize(PacketReader reader)
  {
    this.CardShopRemovalsUsed = reader.ReadInt();
    this.WongoPoints = reader.ReadInt();
    this.CccomboBadgeUnlocked = reader.ReadBool();
    this.DamageDealt = reader.ReadInt();
    this.DebuffsApplied = reader.ReadInt();
  }
}
