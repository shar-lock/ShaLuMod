// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.PaelsWing
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.CardRewardAlternatives;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Entities.Rewards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class PaelsWing : RelicModel
{
  public const string sacrificeAlternativeKey = "SACRIFICE";
  private const string _sacrificesKey = "Sacrifices";
  private bool _isActivating;
  private int _rewardsSacrificed;

  public override RelicRarity Rarity => RelicRarity.Ancient;

  public override bool ShowCounter => true;

  public override int DisplayAmount
  {
    get
    {
      return !this.IsActivating ? this.RewardsSacrificed % this.DynamicVars["Sacrifices"].IntValue : this.DynamicVars["Sacrifices"].IntValue;
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("Sacrifices", 2M));
    }
  }

  private bool IsActivating
  {
    get => this._isActivating;
    set
    {
      this.AssertMutable();
      this._isActivating = value;
      this.InvokeDisplayAmountChanged();
    }
  }

  [SavedProperty]
  public int RewardsSacrificed
  {
    get => this._rewardsSacrificed;
    set
    {
      this.AssertMutable();
      this._rewardsSacrificed = value;
      this.InvokeDisplayAmountChanged();
    }
  }

  public override bool TryModifyCardRewardAlternatives(
    Player player,
    CardReward cardReward,
    List<CardRewardAlternative> alternatives)
  {
    if (this.Owner != player)
      return false;
    alternatives.Add(new CardRewardAlternative("SACRIFICE", new Func<Task>(this.OnSacrifice), PostAlternateCardRewardAction.EndSelectionAndCompleteReward));
    return true;
  }

  public async Task OnSacrifice()
  {
    this.RewardsSacrificed++;
    this.Flash();
    if (this.RewardsSacrificed % this.DynamicVars["Sacrifices"].IntValue != 0)
      return;
    TaskHelper.RunSafely(this.DoActivateVisuals());
    RelicModel relicModel = await RelicCmd.Obtain(RelicFactory.PullNextRelicFromFront(this.Owner).ToMutable(), this.Owner);
  }

  private async Task DoActivateVisuals()
  {
    this.IsActivating = true;
    await Cmd.Wait(1f);
    this.IsActivating = false;
  }
}
