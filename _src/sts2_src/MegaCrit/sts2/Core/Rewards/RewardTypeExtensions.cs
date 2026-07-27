// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Rewards.RewardTypeExtensions
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Rewards;

public static class RewardTypeExtensions
{
  public static IEnumerable<string> GetAssetPaths(this RewardType rewardType)
  {
    IEnumerable<string> assetPaths;
    switch (rewardType)
    {
      case RewardType.Card:
        assetPaths = CardReward.AssetPaths;
        break;
      case RewardType.Gold:
        assetPaths = GoldReward.AssetPaths;
        break;
      case RewardType.RemoveCard:
        assetPaths = CardRemovalReward.AssetPaths;
        break;
      case RewardType.SpecialCard:
        assetPaths = SpecialCardReward.AssetPaths;
        break;
      default:
        assetPaths = (IEnumerable<string>) Array.Empty<string>();
        break;
    }
    return assetPaths;
  }
}
