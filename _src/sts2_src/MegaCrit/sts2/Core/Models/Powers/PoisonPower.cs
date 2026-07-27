// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.PoisonPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class PoisonPower : PowerModel
{
  public override PowerType Type => PowerType.Debuff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override Color AmountLabelColor => PowerModel._normalAmountLabelColor;

  private int TriggerCount
  {
    get
    {
      return Math.Min(this.Amount, 1 + this.Owner.CombatState.GetOpponentsOf(this.Owner).Where<Creature>((Func<Creature, bool>) (c => c.IsAlive)).Sum<Creature>((Func<Creature, int>) (a => a.GetPowerAmount<AccelerantPower>())));
    }
  }

  public int CalculateTotalDamageNextTurn()
  {
    Decimal totalDamageNextTurn = 0M;
    int num1 = Math.Min(this.Amount, this.TriggerCount);
    for (int index = 0; index < num1; ++index)
    {
      Decimal num2 = Hook.ModifyDamage(this.Owner.CombatState.RunState, this.Owner.CombatState, this.Owner, (Creature) null, (Decimal) (this.Amount - index), ValueProp.Unblockable | ValueProp.Unpowered, (CardModel) null, (CardPlay) null, ModifyDamageHookType.All, CardPreviewMode.None, out IEnumerable<AbstractModel> _);
      totalDamageNextTurn += num2;
    }
    return (int) totalDamageNextTurn;
  }

  public override async Task AfterSideTurnStart(
    CombatSide side,
    IReadOnlyList<Creature> participants,
    ICombatState combatState)
  {
    if (!participants.Contains<Creature>(this.Owner))
      return;
    int iterations = this.TriggerCount;
    for (int i = 0; i < iterations; ++i)
    {
      IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Owner, (Decimal) this.Amount, ValueProp.Unblockable | ValueProp.Unpowered, (CardModel) null, (CardPlay) null);
      if (this.Owner.IsAlive)
        await PowerCmd.Decrement((PowerModel) this);
      else
        await Cmd.CustomScaledWait(0.1f, 0.25f);
    }
  }
}
