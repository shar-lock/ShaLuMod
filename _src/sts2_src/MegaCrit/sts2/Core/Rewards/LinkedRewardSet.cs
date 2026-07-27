// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Rewards.LinkedRewardSet
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Rewards;

public class LinkedRewardSet : Reward
{
  private readonly List<Reward> _rewards;

  protected override RewardType RewardType => RewardType.None;

  public override int RewardsSetIndex
  {
    get => this.Rewards.Max<Reward>((Func<Reward, int>) (r => r.RewardsSetIndex));
  }

  private static LocString HoverTipTitle
  {
    get => new LocString("static_hover_tips", "LINKED_REWARDS.title");
  }

  private static LocString HoverTipDesc
  {
    get => new LocString("static_hover_tips", "LINKED_REWARDS.description");
  }

  public static HoverTip HoverTip
  {
    get => new HoverTip(LinkedRewardSet.HoverTipTitle, LinkedRewardSet.HoverTipDesc);
  }

  public IReadOnlyList<Reward> Rewards => (IReadOnlyList<Reward>) this._rewards.ToList<Reward>();

  public LinkedRewardSet(List<Reward> rewards, Player player)
    : base(player)
  {
    this._rewards = rewards;
    foreach (Reward reward in this._rewards)
      reward.ParentRewardSet = this;
  }

  public override bool IsPopulated
  {
    get => this._rewards.All<Reward>((Func<Reward, bool>) (r => r.IsPopulated));
  }

  public override void Populate()
  {
    foreach (Reward reward in this._rewards)
      reward.Populate();
  }

  public void RemoveReward(Reward reward) => this._rewards.Remove(reward);

  protected override Task<bool> OnSelect() => Task.FromResult<bool>(true);

  public override void OnSkipped()
  {
    foreach (Reward reward in this._rewards)
      reward.OnSkipped();
  }

  public override void MarkContentAsSeen()
  {
    foreach (Reward reward in this._rewards)
      reward.MarkContentAsSeen();
  }

  public override LocString Description => new LocString("gameplay_ui", "COMBAT_REWARD_LINKED");
}
