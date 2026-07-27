// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Odds.CardRarityOdds
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Odds;

public class CardRarityOdds : AbstractOdds
{
  public static float regularCommonOdds = AscensionHelper.GetValueIfAscension(AscensionLevel.Scarcity, 0.615f, 0.6f);
  public const float regularUncommonOdds = 0.37f;
  public const float eliteUncommonOdds = 0.4f;
  public const float bossCommonOdds = 0.0f;
  public const float bossUncommonOdds = 0.0f;
  public const float bossRareOdds = 1f;
  public const float shopUncommonOdds = 0.37f;
  private const float _baseRarityOffset = -0.05f;
  private const float _maxRarityOffset = 0.4f;

  public float RarityGrowth
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.Scarcity, 0.005f, 0.01f);
  }

  public static float RegularRareOdds
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.Scarcity, 0.0149f, 0.03f);
  }

  public static float EliteCommonOdds
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.Scarcity, 0.549f, 0.5f);
  }

  public static float EliteRareOdds
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.Scarcity, 0.05f, 0.1f);
  }

  public static float ShopCommonOdds
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.Scarcity, 0.585f, 0.54f);
  }

  public static float ShopRareOdds
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.Scarcity, 0.045f, 0.09f);
  }

  public CardRarityOdds(Rng rng)
    : base(-0.05f, rng)
  {
  }

  public CardRarityOdds(float initialValue, Rng rng)
    : base(initialValue, rng)
  {
  }

  public CardRarity Roll(CardRarityOddsType type)
  {
    CardRarity cardRarity = this.RollWithoutChangingFutureOdds(type, type == CardRarityOddsType.BossEncounter ? 0.0f : this.CurrentValue);
    if (cardRarity == CardRarity.Rare)
      this.CurrentValue = -0.05f;
    else
      this.CurrentValue = Math.Min(this.CurrentValue + this.RarityGrowth, 0.4f);
    return cardRarity;
  }

  public CardRarity RollWithoutChangingFutureOdds(CardRarityOddsType oddsType)
  {
    return this.RollWithoutChangingFutureOdds(oddsType, this.CurrentValue);
  }

  public CardRarity RollWithoutChangingFutureOdds(CardRarityOddsType type, float offset)
  {
    float num1 = this._rng.NextFloat();
    float num2 = CardRarityOdds.GetBaseOdds(type, CardRarity.Rare) + offset;
    Log.Info($"Card rarity: Rolled {num1}, need < {num2} for rare (offset = {offset})");
    if ((double) num1 < (double) num2)
      return CardRarity.Rare;
    return (double) num1 < (double) CardRarityOdds.GetBaseOdds(type, CardRarity.Uncommon) + (double) num2 ? CardRarity.Uncommon : CardRarity.Common;
  }

  public CardRarity RollWithBaseOdds(CardRarityOddsType type)
  {
    float num = this._rng.NextFloat();
    if ((double) num < (double) CardRarityOdds.GetBaseOdds(type, CardRarity.Rare))
      return CardRarity.Rare;
    return (double) num < (double) CardRarityOdds.GetBaseOdds(type, CardRarity.Uncommon) ? CardRarity.Uncommon : CardRarity.Common;
  }

  private static float GetBaseOdds(CardRarityOddsType type, CardRarity rarity)
  {
    switch (type)
    {
      case CardRarityOddsType.RegularEncounter:
        float baseOdds1;
        switch (rarity)
        {
          case CardRarity.Common:
            baseOdds1 = CardRarityOdds.regularCommonOdds;
            break;
          case CardRarity.Uncommon:
            baseOdds1 = 0.37f;
            break;
          case CardRarity.Rare:
            baseOdds1 = CardRarityOdds.RegularRareOdds;
            break;
          default:
            throw new ArgumentOutOfRangeException(nameof (rarity));
        }
        return baseOdds1;
      case CardRarityOddsType.EliteEncounter:
        float baseOdds2;
        switch (rarity)
        {
          case CardRarity.Common:
            baseOdds2 = CardRarityOdds.EliteCommonOdds;
            break;
          case CardRarity.Uncommon:
            baseOdds2 = 0.4f;
            break;
          case CardRarity.Rare:
            baseOdds2 = CardRarityOdds.EliteRareOdds;
            break;
          default:
            throw new ArgumentOutOfRangeException(nameof (rarity));
        }
        return baseOdds2;
      case CardRarityOddsType.BossEncounter:
        float baseOdds3;
        switch (rarity)
        {
          case CardRarity.Common:
            baseOdds3 = 0.0f;
            break;
          case CardRarity.Uncommon:
            baseOdds3 = 0.0f;
            break;
          case CardRarity.Rare:
            baseOdds3 = 1f;
            break;
          default:
            throw new ArgumentOutOfRangeException(nameof (rarity));
        }
        return baseOdds3;
      case CardRarityOddsType.Shop:
        float baseOdds4;
        switch (rarity)
        {
          case CardRarity.Common:
            baseOdds4 = CardRarityOdds.ShopCommonOdds;
            break;
          case CardRarity.Uncommon:
            baseOdds4 = 0.37f;
            break;
          case CardRarity.Rare:
            baseOdds4 = CardRarityOdds.ShopRareOdds;
            break;
          default:
            throw new ArgumentOutOfRangeException(nameof (rarity));
        }
        return baseOdds4;
      case CardRarityOddsType.Uniform:
        float baseOdds5;
        switch (rarity)
        {
          case CardRarity.Common:
            baseOdds5 = 0.33f;
            break;
          case CardRarity.Uncommon:
            baseOdds5 = 0.33f;
            break;
          case CardRarity.Rare:
            baseOdds5 = 0.33f;
            break;
          default:
            throw new ArgumentOutOfRangeException(nameof (rarity));
        }
        return baseOdds5;
      default:
        throw new ArgumentOutOfRangeException(nameof (type));
    }
  }
}
