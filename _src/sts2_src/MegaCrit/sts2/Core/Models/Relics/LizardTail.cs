// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.LizardTail
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class LizardTail : RelicModel
{
  private bool _wasUsed;

  public override RelicRarity Rarity => RelicRarity.Rare;

  public override bool IsUsedUp => this._wasUsed;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new HealVar(50M));
    }
  }

  [SavedProperty]
  public bool WasUsed
  {
    get => this._wasUsed;
    set
    {
      this.AssertMutable();
      this._wasUsed = value;
      if (!this.IsUsedUp)
        return;
      this.Status = RelicStatus.Disabled;
    }
  }

  public override bool ShouldDieLate(Creature creature)
  {
    return creature != this.Owner.Creature || this.WasUsed;
  }

  public override async Task AfterPreventingDeath(Creature creature)
  {
    this.Flash();
    this.WasUsed = true;
    await CreatureCmd.Heal(creature, Math.Max(1M, (Decimal) creature.MaxHp * (this.DynamicVars.Heal.BaseValue / 100M)));
  }
}
