// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Rewards.CardRemovalReward
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Runs;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Rewards;

public class CardRemovalReward(Player player) : Reward(player)
{
  private static string RewardIcon
  {
    get => ImageHelper.GetImagePath("ui/reward_screen/reward_icon_card_removal.png");
  }

  protected override RewardType RewardType => RewardType.RemoveCard;

  public override int RewardsSetIndex => 7;

  protected override string IconPath => CardRemovalReward.RewardIcon;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(CardRemovalReward.RewardIcon);
    }
  }

  public override bool IsPopulated => true;

  public override LocString Description
  {
    get => new LocString("gameplay_ui", "COMBAT_REWARD_CARD_REMOVAL");
  }

  public override void Populate()
  {
  }

  protected override async Task<bool> OnSelect()
  {
    Log.Info($"Player {this.Player.NetId} obtained card removal from reward");
    return await RunManager.Instance.RewardSynchronizer.DoUnsyncedCardRemoval(this.Player);
  }

  public override void MarkContentAsSeen()
  {
  }
}
