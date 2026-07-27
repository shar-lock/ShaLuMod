// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.JuggernautPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class JuggernautPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override async Task AfterBlockGained(
    Creature creature,
    Decimal amount,
    ValueProp props,
    CardModel? cardSource)
  {
    if (amount <= 0M || creature != this.Owner)
      return;
    IReadOnlyList<Creature> hittableEnemies = this.CombatState.HittableEnemies;
    if (hittableEnemies.Count == 0)
      return;
    Creature target = this.Owner.Player.RunState.Rng.CombatTargets.NextItem<Creature>((IEnumerable<Creature>) hittableEnemies);
    this.Flash();
    IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), target, (Decimal) this.Amount, ValueProp.Unpowered, this.Owner);
  }
}
