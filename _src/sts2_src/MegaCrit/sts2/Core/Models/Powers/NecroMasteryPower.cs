// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.NecroMasteryPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class NecroMasteryPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override async Task AfterCurrentHpChanged(Creature creature, Decimal delta)
  {
    if (delta >= 0M || !(creature.Monster is Osty) || creature.PetOwner != this.Owner.Player)
      return;
    IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (IEnumerable<Creature>) creature.CombatState.HittableEnemies, -delta * (Decimal) this.Amount, ValueProp.Unblockable | ValueProp.Unpowered, this.Owner);
  }
}
