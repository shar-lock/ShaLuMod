// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.ToyBox
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class ToyBox : RelicModel
{
  private const string _relicsKey = "Relics";
  private const string _combatsKey = "Combats";
  private bool _isActivating;
  private int _combatsSeen;

  public static LocString WaxRelicPrefix => new LocString("relics", "TOY_BOX.waxRelicPrefix");

  public override RelicRarity Rarity => RelicRarity.Ancient;

  public override bool HasUponPickupEffect => true;

  public override bool IsUsedUp
  {
    get
    {
      return this.CombatsSeen >= this.DynamicVars["Combats"].IntValue * this.DynamicVars["Relics"].IntValue;
    }
  }

  public override bool ShowCounter => !this.IsUsedUp;

  public override int DisplayAmount
  {
    get
    {
      return !this.IsActivating ? this.CombatsSeen % this.DynamicVars["Combats"].IntValue : this.DynamicVars["Combats"].IntValue;
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
  public int CombatsSeen
  {
    get => this._combatsSeen;
    set
    {
      this.AssertMutable();
      this._combatsSeen = value;
      this.InvokeDisplayAmountChanged();
      if (!this.IsUsedUp)
        return;
      this.Status = RelicStatus.Disabled;
    }
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        new DynamicVar("Relics", 5M),
        new DynamicVar("Combats", 3M)
      });
    }
  }

  public override async Task AfterObtained()
  {
    List<Reward> rewards = new List<Reward>();
    for (int index = 0; index < this.DynamicVars["Relics"].IntValue; ++index)
    {
      RelicModel mutable = RelicFactory.PullNextRelicFromFront(this.Owner).ToMutable();
      mutable.IsWax = true;
      rewards.Add((Reward) new RelicReward(mutable, this.Owner));
    }
    await RewardsCmd.OfferCustom(this.Owner, rewards);
  }

  public override async Task AfterCombatEnd(CombatRoom __)
  {
    if (this.IsUsedUp)
      return;
    this.CombatsSeen++;
    if (this.CombatsSeen % this.DynamicVars["Combats"].IntValue != 0)
      return;
    TaskHelper.RunSafely(this.DoActivateVisuals());
    RelicModel relic = this.Owner.Relics.FirstOrDefault<RelicModel>((Func<RelicModel, bool>) (r => r != null && r.IsWax && !r.IsMelted));
    if (relic == null)
      return;
    await RelicCmd.Melt(relic);
    await Cmd.CustomScaledWait(0.5f, 0.75f);
  }

  private async Task DoActivateVisuals()
  {
    this.IsActivating = true;
    this.Flash();
    await Cmd.Wait(1f);
    this.IsActivating = false;
  }
}
