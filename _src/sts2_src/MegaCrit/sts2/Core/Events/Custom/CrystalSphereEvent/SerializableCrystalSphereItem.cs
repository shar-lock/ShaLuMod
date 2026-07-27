// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Events.Custom.CrystalSphereEvent.SerializableCrystalSphereItem
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Events.Custom.CrystalSphereEvent;

public class SerializableCrystalSphereItem : IPacketSerializable
{
  public CrystalSphereItemType type;
  public CardRarity cardRarity;
  public PotionRarity potionRarity;
  public bool isBigGold;

  public void Serialize(PacketWriter writer)
  {
    writer.WriteEnum<CrystalSphereItemType>(this.type);
    if (this.type == CrystalSphereItemType.CardReward)
      writer.WriteEnum<CardRarity>(this.cardRarity);
    else if (this.type == CrystalSphereItemType.Potion)
    {
      writer.WriteEnum<PotionRarity>(this.potionRarity);
    }
    else
    {
      if (this.type != CrystalSphereItemType.Gold)
        return;
      writer.WriteBool(this.isBigGold);
    }
  }

  public void Deserialize(PacketReader reader)
  {
    this.type = reader.ReadEnum<CrystalSphereItemType>();
    if (this.type == CrystalSphereItemType.CardReward)
      this.cardRarity = reader.ReadEnum<CardRarity>();
    else if (this.type == CrystalSphereItemType.Potion)
    {
      this.potionRarity = reader.ReadEnum<PotionRarity>();
    }
    else
    {
      if (this.type != CrystalSphereItemType.Gold)
        return;
      this.isBigGold = reader.ReadBool();
    }
  }
}
