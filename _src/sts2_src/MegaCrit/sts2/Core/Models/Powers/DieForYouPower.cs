// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.DieForYouPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class DieForYouPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Single;

  public override bool ShouldPlayVfx => false;

  public override Creature ModifyUnblockedDamageTarget(
    Creature target,
    Decimal _,
    ValueProp props,
    Creature? __)
  {
    return target != this.Owner.PetOwner?.Creature || this.Owner.IsDead || !props.IsPoweredAttack() ? target : this.Owner;
  }

  public override bool ShouldAllowHitting(Creature creature) => creature.IsAlive;

  public override bool ShouldCreatureBeRemovedFromCombatAfterDeath(Creature creature)
  {
    return creature != this.Owner;
  }

  public override bool ShouldPowerBeRemovedAfterOwnerDeath() => false;
}
