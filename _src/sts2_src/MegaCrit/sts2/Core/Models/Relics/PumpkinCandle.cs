// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.PumpkinCandle
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Entities.RestSite;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class PumpkinCandle : RelicModel
{
  private const string _defaultCombatCountKey = "CombatCount";
  public const int kindleAmount = 5;
  private int _kindleCount;

  public override RelicRarity Rarity => RelicRarity.Ancient;

  public override bool ShowCounter => true;

  public override int DisplayAmount => this.KindleCount;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        new DynamicVar("CombatCount", 5M),
        (DynamicVar) new EnergyVar(1)
      });
    }
  }

  [SavedProperty]
  public int KindleCount
  {
    get => this._kindleCount;
    set
    {
      this.AssertMutable();
      this._kindleCount = value;
      this.Status = this.KindleCount > 0 ? RelicStatus.Normal : RelicStatus.Disabled;
      this.InvokeDisplayAmountChanged();
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.ForEnergy((RelicModel) this));
    }
  }

  public override Task AfterObtained()
  {
    this.Rekindle();
    return Task.CompletedTask;
  }

  public override Decimal ModifyMaxEnergy(Player player, Decimal amount)
  {
    return player != this.Owner || this.KindleCount <= 0 ? amount : amount + (Decimal) this.DynamicVars.Energy.IntValue;
  }

  public override Task AfterCombatEnd(CombatRoom room)
  {
    this.KindleCount = Math.Max(this.KindleCount - 1, 0);
    return Task.CompletedTask;
  }

  public override bool TryModifyRestSiteOptions(Player player, ICollection<RestSiteOption> options)
  {
    if (player != this.Owner)
      return false;
    options.Add((RestSiteOption) new KindleRestSiteOption(player));
    return true;
  }

  public void Rekindle()
  {
    this.KindleCount += 5;
    this.Flash();
  }
}
