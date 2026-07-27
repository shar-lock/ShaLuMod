// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Sync.RewardObtainedMessage
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using MegaCrit.Sts2.Core.Multiplayer.Transport;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Text;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Messages.Game.Sync;

public struct RewardObtainedMessage : INetMessage, IPacketSerializable, IRunLocationTargetedMessage
{
  public required RewardType rewardType;
  public required RunLocation location;
  public CardModel? cardModel;
  public PotionModel? potionModel;
  public RelicModel? relicModel;
  public int? goldAmount;
  public required bool wasSkipped;

  public bool ShouldBroadcast => true;

  public NetTransferMode Mode => NetTransferMode.Reliable;

  public LogLevel LogLevel => LogLevel.VeryDebug;

  public bool ShouldBuffer => true;

  public RunLocation Location => this.location;

  public void Serialize(PacketWriter writer)
  {
    writer.WriteEnum<RewardType>(this.rewardType);
    writer.Write<RunLocation>(this.location);
    switch (this.rewardType)
    {
      case RewardType.Card:
        writer.Write<SerializableCard>(this.cardModel.ToSerializable());
        break;
      case RewardType.Gold:
        writer.WriteInt(this.goldAmount.Value);
        break;
      case RewardType.Potion:
        writer.WriteModelEntry(this.potionModel.Id);
        break;
      case RewardType.Relic:
        writer.Write<SerializableRelic>(this.relicModel.ToSerializable());
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
    writer.WriteBool(this.wasSkipped);
  }

  public void Deserialize(PacketReader reader)
  {
    this.rewardType = reader.ReadEnum<RewardType>();
    this.location = reader.Read<RunLocation>();
    switch (this.rewardType)
    {
      case RewardType.Card:
        this.cardModel = CardModel.FromSerializable(reader.Read<SerializableCard>());
        break;
      case RewardType.Gold:
        this.goldAmount = new int?(reader.ReadInt());
        break;
      case RewardType.Potion:
        this.potionModel = ModelDb.GetById<PotionModel>(reader.ReadModelIdAssumingType<PotionModel>());
        break;
      case RewardType.Relic:
        this.relicModel = RelicModel.FromSerializable(reader.Read<SerializableRelic>());
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
    this.wasSkipped = reader.ReadBool();
  }

  public override string ToString()
  {
    StringBuilder stringBuilder1 = new StringBuilder();
    StringBuilder stringBuilder2 = stringBuilder1;
    StringBuilder stringBuilder3 = stringBuilder2;
    StringBuilder.AppendInterpolatedStringHandler interpolatedStringHandler;
    // ISSUE: explicit constructor call
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(29, 4, stringBuilder2);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(nameof (RewardObtainedMessage));
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(" type: ");
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<RewardType>(this.rewardType);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(" location: ");
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<RunLocation>(this.location);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(" skipped: ");
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<bool>(this.wasSkipped);
    ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral(" ");
    ref StringBuilder.AppendInterpolatedStringHandler local1 = ref interpolatedStringHandler;
    stringBuilder3.Append(ref local1);
    switch (this.rewardType)
    {
      case RewardType.Card:
        StringBuilder stringBuilder4 = stringBuilder1;
        StringBuilder stringBuilder5 = stringBuilder4;
        // ISSUE: explicit constructor call
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(6, 1, stringBuilder4);
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("Card: ");
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<CardModel>(this.cardModel);
        ref StringBuilder.AppendInterpolatedStringHandler local2 = ref interpolatedStringHandler;
        stringBuilder5.Append(ref local2);
        break;
      case RewardType.Gold:
        StringBuilder stringBuilder6 = stringBuilder1;
        StringBuilder stringBuilder7 = stringBuilder6;
        // ISSUE: explicit constructor call
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(6, 1, stringBuilder6);
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("Gold: ");
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<int?>(this.goldAmount);
        ref StringBuilder.AppendInterpolatedStringHandler local3 = ref interpolatedStringHandler;
        stringBuilder7.Append(ref local3);
        break;
      case RewardType.Potion:
        StringBuilder stringBuilder8 = stringBuilder1;
        StringBuilder stringBuilder9 = stringBuilder8;
        // ISSUE: explicit constructor call
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(8, 1, stringBuilder8);
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("Potion: ");
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<PotionModel>(this.potionModel);
        ref StringBuilder.AppendInterpolatedStringHandler local4 = ref interpolatedStringHandler;
        stringBuilder9.Append(ref local4);
        break;
      case RewardType.Relic:
        StringBuilder stringBuilder10 = stringBuilder1;
        StringBuilder stringBuilder11 = stringBuilder10;
        // ISSUE: explicit constructor call
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).\u002Ector(7, 1, stringBuilder10);
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("Relic: ");
        ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted<RelicModel>(this.relicModel);
        ref StringBuilder.AppendInterpolatedStringHandler local5 = ref interpolatedStringHandler;
        stringBuilder11.Append(ref local5);
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
    return stringBuilder1.ToString();
  }
}
