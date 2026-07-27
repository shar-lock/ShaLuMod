// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.WongosMysteryTicket
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class WongosMysteryTicket : RelicModel
{
  private const string _remainingCombatsKey = "RemainingCombats";
  public const int combatsToActivate = 5;
  public const int relicCount = 3;
  private int _combatsFinished;
  private bool _gaveRelic;

  public override RelicRarity Rarity => RelicRarity.Event;

  public override bool IsUsedUp => this.GaveRelic;

  public override bool ShowCounter => this.DisplayAmount > 0;

  public override int DisplayAmount => 5 - this.CombatsFinished;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new RepeatVar(3),
        new DynamicVar("RemainingCombats", 5M)
      });
    }
  }

  [SavedProperty]
  public int CombatsFinished
  {
    get => this._combatsFinished;
    set
    {
      this.AssertMutable();
      this._combatsFinished = value;
      this.InvokeDisplayAmountChanged();
    }
  }

  [SavedProperty]
  public bool GaveRelic
  {
    get => this._gaveRelic;
    set
    {
      this.AssertMutable();
      this._gaveRelic = value;
      this.InvokeDisplayAmountChanged();
      if (!this._gaveRelic)
        return;
      this.Status = RelicStatus.Disabled;
    }
  }

  public override Task AfterCombatEnd(CombatRoom _)
  {
    ++this.CombatsFinished;
    this.DynamicVars["RemainingCombats"].BaseValue = Decimal.Max((Decimal) (5 - this.CombatsFinished), 0M);
    return Task.CompletedTask;
  }

  public override bool TryModifyRewards(Player player, List<Reward> rewards, AbstractRoom? room)
  {
    if (player != this.Owner || !(room is CombatRoom) || this.GaveRelic || 5 - this.CombatsFinished > 0)
      return false;
    for (int index = 0; index < this.DynamicVars.Repeat.IntValue; ++index)
      rewards.Add((Reward) new RelicReward(player));
    return true;
  }

  public override Task AfterModifyingRewards()
  {
    this.Flash();
    this.GaveRelic = true;
    return Task.CompletedTask;
  }
}
