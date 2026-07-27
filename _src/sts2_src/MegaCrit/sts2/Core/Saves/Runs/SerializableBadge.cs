// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.Runs.SerializableBadge
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Models.Badges;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves.Runs;

public class SerializableBadge : IPacketSerializable
{
  [JsonPropertyName("id")]
  public required string Id { get; set; }

  [JsonPropertyName("rarity")]
  public required BadgeRarity Rarity { get; set; }

  public void Serialize(PacketWriter writer)
  {
    writer.WriteString(this.Id);
    writer.WriteEnum<BadgeRarity>(this.Rarity);
  }

  public void Deserialize(PacketReader reader)
  {
    this.Id = reader.ReadString();
    this.Rarity = reader.ReadEnum<BadgeRarity>();
  }
}
