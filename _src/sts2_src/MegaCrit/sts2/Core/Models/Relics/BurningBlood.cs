// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.BurningBlood
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class BurningBlood : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Starter;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new HealVar(6M));
    }
  }

  public override async Task AfterCombatVictory(CombatRoom _)
  {
    if (this.Owner.Creature.IsDead)
      return;
    this.Flash();
    await CreatureCmd.Heal(this.Owner.Creature, this.DynamicVars.Heal.BaseValue);
  }
}
