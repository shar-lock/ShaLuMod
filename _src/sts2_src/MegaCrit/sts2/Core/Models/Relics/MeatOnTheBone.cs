// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.MeatOnTheBone
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class MeatOnTheBone : RelicModel
{
  private const string _hpThresholdKey = "HpThreshold";

  public override RelicRarity Rarity => RelicRarity.Rare;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        new DynamicVar("HpThreshold", 50M),
        (DynamicVar) new HealVar(12M)
      });
    }
  }

  public override Task BeforeCombatStart()
  {
    if (this.WillHealOnCombatFinished())
      this.Status = RelicStatus.Active;
    return Task.CompletedTask;
  }

  public override Task AfterCurrentHpChanged(Creature creature, Decimal delta)
  {
    if (creature != this.Owner.Creature || !CombatManager.Instance.IsInProgress)
      return Task.CompletedTask;
    this.Status = this.WillHealOnCombatFinished() ? RelicStatus.Active : RelicStatus.Normal;
    return Task.CompletedTask;
  }

  public override async Task AfterCombatVictoryEarly(CombatRoom _)
  {
    if (this.Owner.Creature.IsDead)
      return;
    Creature creature = this.Owner.Creature;
    if (!this.WillHealOnCombatFinished())
      return;
    this.Status = RelicStatus.Normal;
    await CreatureCmd.Heal(creature, this.DynamicVars.Heal.BaseValue);
  }

  private bool WillHealOnCombatFinished()
  {
    Creature creature = this.Owner.Creature;
    int num = (int) ((Decimal) creature.MaxHp * (this.DynamicVars["HpThreshold"].BaseValue / 100M));
    return creature.CurrentHp <= num;
  }
}
