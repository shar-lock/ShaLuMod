// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.TheBombPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class TheBombPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new DamageVar(40M, ValueProp.Unpowered));
    }
  }

  public void SetDamage(Decimal damage)
  {
    this.AssertMutable();
    this.DynamicVars.Damage.BaseValue = damage;
  }

  public override async Task BeforeSideTurnEnd(
    PlayerChoiceContext choiceContext,
    CombatSide side,
    IEnumerable<Creature> participants)
  {
    if (!participants.Contains<Creature>(this.Owner))
      return;
    if (this.Amount > 1)
    {
      await PowerCmd.Decrement((PowerModel) this);
    }
    else
    {
      this.Flash();
      await Cmd.CustomScaledWait(0.2f, 0.4f);
      foreach (Creature hittableEnemy in (IEnumerable<Creature>) this.CombatState.HittableEnemies)
      {
        NCombatRoom instance = NCombatRoom.Instance;
        if (instance != null)
          ((Node) instance.CombatVfxContainer).AddChildSafely((Node) NFireSmokePuffVfx.Create(hittableEnemy));
      }
      await Cmd.CustomScaledWait(0.2f, 0.4f);
      IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(choiceContext, (IEnumerable<Creature>) this.CombatState.HittableEnemies, this.DynamicVars.Damage, this.Owner);
      await PowerCmd.Remove((PowerModel) this);
    }
  }
}
