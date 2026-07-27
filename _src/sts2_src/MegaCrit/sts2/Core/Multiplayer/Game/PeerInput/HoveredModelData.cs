// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput.HoveredModelData
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput;

public struct HoveredModelData : IPacketSerializable, IEquatable<HoveredModelData>
{
  public HoveredModelType type;
  public NetCombatCard? hoveredCombatCard;
  public int? hoveredRelicIndex;
  public int? hoveredPotionIndex;
  public ModelId? hoveredModelId;

  public bool Equals(HoveredModelData other)
  {
    if (this.type == other.type)
    {
      NetCombatCard? hoveredCombatCard1 = this.hoveredCombatCard;
      NetCombatCard? hoveredCombatCard2 = other.hoveredCombatCard;
      if ((hoveredCombatCard1.HasValue == hoveredCombatCard2.HasValue ? (hoveredCombatCard1.HasValue ? (hoveredCombatCard1.GetValueOrDefault() == hoveredCombatCard2.GetValueOrDefault() ? 1 : 0) : 1) : 0) != 0)
      {
        int? hoveredRelicIndex1 = this.hoveredRelicIndex;
        int? hoveredRelicIndex2 = other.hoveredRelicIndex;
        if (hoveredRelicIndex1.GetValueOrDefault() == hoveredRelicIndex2.GetValueOrDefault() & hoveredRelicIndex1.HasValue == hoveredRelicIndex2.HasValue)
        {
          int? hoveredPotionIndex1 = this.hoveredPotionIndex;
          int? hoveredPotionIndex2 = other.hoveredPotionIndex;
          if (hoveredPotionIndex1.GetValueOrDefault() == hoveredPotionIndex2.GetValueOrDefault() & hoveredPotionIndex1.HasValue == hoveredPotionIndex2.HasValue)
            return this.hoveredModelId == other.hoveredModelId;
        }
      }
    }
    return false;
  }

  public static HoveredModelData FromModel(AbstractModel? model)
  {
    if (model == null)
      return new HoveredModelData();
    HoveredModelData hoveredModelData = new HoveredModelData();
    if (!(model is CardModel card))
    {
      if (!(model is RelicModel relicModel))
      {
        if (model is PotionModel model1)
        {
          hoveredModelData.type = HoveredModelType.Potion;
          hoveredModelData.hoveredPotionIndex = new int?(model1.Owner.GetPotionSlotIndex(model1));
        }
        else
          throw new InvalidOperationException($"Model {model} has unsupported type for hovering");
      }
      else
      {
        hoveredModelData.type = HoveredModelType.Relic;
        hoveredModelData.hoveredRelicIndex = new int?(relicModel.Owner.Relics.IndexOf<RelicModel>(relicModel));
      }
    }
    else
    {
      hoveredModelData.type = HoveredModelType.Card;
      hoveredModelData.hoveredCombatCard = new NetCombatCard?(NetCombatCard.FromModel(card));
    }
    hoveredModelData.hoveredModelId = model.Id;
    return hoveredModelData;
  }

  public void Serialize(PacketWriter writer)
  {
    writer.WriteEnum<HoveredModelType>(this.type);
    switch (this.type)
    {
      case HoveredModelType.Card:
        writer.Write<NetCombatCard>(this.hoveredCombatCard.Value);
        break;
      case HoveredModelType.Relic:
        writer.WriteInt(this.hoveredRelicIndex.Value, 8);
        break;
      case HoveredModelType.Potion:
        writer.WriteInt(this.hoveredPotionIndex.Value, 4);
        break;
    }
    if (this.type == HoveredModelType.None)
      return;
    writer.WriteModelEntry(this.hoveredModelId);
  }

  public void Deserialize(PacketReader reader)
  {
    this.type = reader.ReadEnum<HoveredModelType>();
    switch (this.type)
    {
      case HoveredModelType.Card:
        this.hoveredCombatCard = new NetCombatCard?(reader.Read<NetCombatCard>());
        this.hoveredModelId = reader.ReadModelIdAssumingType<CardModel>();
        break;
      case HoveredModelType.Relic:
        this.hoveredRelicIndex = new int?(reader.ReadInt(8));
        this.hoveredModelId = reader.ReadModelIdAssumingType<RelicModel>();
        break;
      case HoveredModelType.Potion:
        this.hoveredPotionIndex = new int?(reader.ReadInt(4));
        this.hoveredModelId = reader.ReadModelIdAssumingType<PotionModel>();
        break;
    }
  }

  public override string ToString()
  {
    switch (this.type)
    {
      case HoveredModelType.None:
        return "HoveredModelData none";
      case HoveredModelType.Card:
        return $"HoveredModelData {this.hoveredCombatCard}";
      case HoveredModelType.Relic:
        return $"HoveredModelData relic index {this.hoveredRelicIndex}";
      case HoveredModelType.Potion:
        return $"HoveredModelData potion index {this.hoveredPotionIndex}";
      default:
        throw new ArgumentOutOfRangeException();
    }
  }
}
