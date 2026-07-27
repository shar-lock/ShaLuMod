// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Odds.PlayerOddsSet
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Saves.Runs;

#nullable enable
namespace MegaCrit.Sts2.Core.Odds;

public class PlayerOddsSet
{
  public CardRarityOdds CardRarity { get; private init; }

  public PotionRewardOdds PotionReward { get; private init; }

  private PlayerOddsSet()
  {
  }

  public PlayerOddsSet(PlayerRngSet rng)
  {
    this.CardRarity = new CardRarityOdds(rng.Rewards);
    this.PotionReward = new PotionRewardOdds(rng.Rewards);
  }

  public SerializablePlayerOddsSet ToSerializable()
  {
    return new SerializablePlayerOddsSet()
    {
      CardRarityOddsValue = this.CardRarity.CurrentValue,
      PotionRewardOddsValue = this.PotionReward.CurrentValue
    };
  }

  public static PlayerOddsSet FromSerializable(SerializablePlayerOddsSet save, PlayerRngSet rng)
  {
    return new PlayerOddsSet()
    {
      CardRarity = new CardRarityOdds(save.CardRarityOddsValue, rng.Rewards),
      PotionReward = new PotionRewardOdds(save.PotionRewardOddsValue, rng.Rewards)
    };
  }

  public void LoadFromSerializable(SerializablePlayerOddsSet save)
  {
    this.CardRarity.OverrideCurrentValue(save.CardRarityOddsValue);
    this.PotionReward.OverrideCurrentValue(save.PotionRewardOddsValue);
  }
}
