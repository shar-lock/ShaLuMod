// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Odds.PotionRewardOdds
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Rooms;

#nullable enable
namespace MegaCrit.Sts2.Core.Odds;

public class PotionRewardOdds : AbstractOdds
{
  public const float targetOdds = 0.5f;
  public const float eliteBonus = 0.25f;
  private const float _basePotionRewardOdds = 0.4f;

  public PotionRewardOdds(Rng rng)
    : base(0.4f, rng)
  {
  }

  public PotionRewardOdds(float initialValue, Rng rng)
    : base(initialValue, rng)
  {
  }

  public bool Roll(Player player, RoomType roomType)
  {
    float currentValue = this.CurrentValue;
    if (Hook.ShouldForcePotionReward(player.RunState, player, roomType))
      return true;
    float num1 = roomType != RoomType.Elite ? 0.0f : 0.25f;
    float num2 = currentValue + num1 * 0.5f;
    if ((double) this._rng.NextFloat() < (double) num2)
    {
      this.CurrentValue -= 0.1f;
      return true;
    }
    this.CurrentValue += 0.1f;
    return false;
  }
}
