// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.RuinedHelmet
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class RuinedHelmet : RelicModel
{
  private bool _usedThisCombat;

  public override RelicRarity Rarity => RelicRarity.Rare;

  private bool UsedThisCombat
  {
    get => this._usedThisCombat;
    set
    {
      this.AssertMutable();
      this._usedThisCombat = value;
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get
    {
      return (IEnumerable<IHoverTip>) new \u003C\u003Ez__ReadOnlySingleElementList<IHoverTip>(HoverTipFactory.FromPower<StrengthPower>());
    }
  }

  public override bool TryModifyPowerAmountReceived(
    PowerModel canonicalPower,
    Creature target,
    Decimal amount,
    Creature? applier,
    out Decimal modifiedAmount)
  {
    modifiedAmount = amount;
    if (!(canonicalPower is StrengthPower) || target != this.Owner.Creature || amount <= 0M || this.UsedThisCombat)
      return false;
    modifiedAmount *= 2M;
    return true;
  }

  public override Task AfterModifyingPowerAmountReceived(PowerModel power)
  {
    this.Flash();
    this.UsedThisCombat = true;
    return Task.CompletedTask;
  }

  public override Task AfterCombatEnd(CombatRoom _)
  {
    this.UsedThisCombat = false;
    return Task.CompletedTask;
  }
}
