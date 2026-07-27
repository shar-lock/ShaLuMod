// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Rewards.GoldReward
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Rewards;

public class GoldReward : Reward
{
  public const int defaultMinGoldAmount = 10;
  public const int defaultMaxGoldAmount = 20;
  private readonly bool _wasGoldStolenBack;
  private readonly int _min;
  private readonly int _max;

  private static string RewardIcon
  {
    get => ImageHelper.GetImagePath("ui/reward_screen/reward_icon_money.png");
  }

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(GoldReward.RewardIcon);
    }
  }

  protected override RewardType RewardType => RewardType.Gold;

  public override int RewardsSetIndex => 1;

  protected override string IconPath => GoldReward.RewardIcon;

  public override Vector2 IconPosition => new Vector2(0.0f, -5f);

  public int Amount { get; private set; } = -1;

  public override LocString Description
  {
    get
    {
      LocString description = new LocString("gameplay_ui", this._wasGoldStolenBack ? "COMBAT_REWARD_GOLD_STOLEN" : "COMBAT_REWARD_GOLD");
      description.Add("gold", (Decimal) this.Amount);
      return description;
    }
  }

  public GoldReward(int amount, Player player, bool wasGoldStolenBack = false)
    : base(player)
  {
    this._min = amount;
    this._max = amount;
    this.Amount = amount;
    this._wasGoldStolenBack = wasGoldStolenBack;
  }

  public GoldReward(int min, int max, Player player, bool wasGoldStolenBack = false)
    : base(player)
  {
    this._min = min;
    this._max = max;
    this._wasGoldStolenBack = wasGoldStolenBack;
  }

  public override bool IsPopulated => this.Amount >= 0;

  public override void Populate()
  {
    this.Amount = (this._rngOverride ?? this.Player.PlayerRng.Rewards).NextInt(this._min, this._max + 1);
  }

  protected override async Task<bool> OnSelect()
  {
    await PlayerCmd.GainGold((Decimal) this.Amount, this.Player, this._wasGoldStolenBack);
    Log.Info($"Player {this.Player.NetId} obtained {this.Amount} gold from reward");
    return true;
  }

  public override SerializableReward ToSerializable()
  {
    return new SerializableReward()
    {
      RewardType = RewardType.Gold,
      GoldAmount = this.Amount,
      WasGoldStolenBack = this._wasGoldStolenBack
    };
  }

  public override void MarkContentAsSeen()
  {
  }
}
