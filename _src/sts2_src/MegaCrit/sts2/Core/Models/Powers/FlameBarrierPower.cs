// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.FlameBarrierPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
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

public sealed class FlameBarrierPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override async Task AfterDamageReceived(
    PlayerChoiceContext choiceContext,
    Creature target,
    DamageResult _,
    ValueProp props,
    Creature? dealer,
    CardModel? __)
  {
    if (target != this.Owner || dealer == null || !props.IsPoweredAttack())
      return;
    IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(choiceContext, dealer, (Decimal) this.Amount, ValueProp.Unpowered, this.Owner);
  }

  public override async Task AfterSideTurnEnd(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IEnumerable<Creature> participants)
  {
    if (this.Owner.Side == side)
      return;
    await PowerCmd.Remove((PowerModel) this);
  }
}
