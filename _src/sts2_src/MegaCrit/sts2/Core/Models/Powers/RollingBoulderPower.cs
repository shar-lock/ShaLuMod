// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.RollingBoulderPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class RollingBoulderPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new DamageVar(5M, ValueProp.Unpowered));
    }
  }

  public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
  {
    if (player != this.Owner.Player)
      ;
    else
    {
      this.Flash();
      if (TestMode.IsOn)
      {
        IEnumerable<DamageResult> damageResults = await this.DoDamage(choiceContext, (IEnumerable<Creature>) this.CombatState.HittableEnemies);
      }
      else
      {
        List<Task> damageTasks = new List<Task>();
        NRollingBoulderVfx nrollingBoulderVfx = NRollingBoulderVfx.Create((IEnumerable<Creature>) this.CombatState.HittableEnemies, (Decimal) this.Amount);
        // ISSUE: object of a compiler-generated type is created
        ((GodotObject) nrollingBoulderVfx).Connect(NRollingBoulderVfx.SignalName.HitCreature, Callable.From<NCreature>((Action<NCreature>) (c => damageTasks.Add((Task) this.DoDamage(choiceContext, (IEnumerable<Creature>) new \u003C\u003Ez__ReadOnlySingleElementList<Creature>(c.Entity))))), 0U);
        SignalAwaiter signal = ((GodotObject) nrollingBoulderVfx).ToSignal((GodotObject) nrollingBoulderVfx, NRollingBoulderVfx.SignalName.Finished);
        ((GodotObject) NCombatRoom.Instance?.CombatVfxContainer).CallDeferred(Node.MethodName.AddChild, new Variant[1]
        {
          Variant.op_Implicit((GodotObject) nrollingBoulderVfx)
        });
        Variant[] variantArray = await signal;
        await Task.WhenAll((IEnumerable<Task>) damageTasks);
      }
      this.SetAmount(this.Amount + this.DynamicVars.Damage.IntValue);
    }
  }

  private Task<IEnumerable<DamageResult>> DoDamage(
    PlayerChoiceContext choiceContext,
    IEnumerable<Creature> targets)
  {
    return CreatureCmd.Damage(choiceContext, targets, (Decimal) this.Amount, ValueProp.Unpowered, this.Owner);
  }
}
