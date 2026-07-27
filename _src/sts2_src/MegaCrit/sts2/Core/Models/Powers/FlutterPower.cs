// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.FlutterPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class FlutterPower : PowerModel
{
  private const string _damageDecreaseKey = "DamageDecrease";

  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Counter;

  public override bool ShouldScaleInMultiplayer => true;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>(new DynamicVar("DamageDecrease", 50M));
    }
  }

  public override Decimal ModifyDamageMultiplicative(
    Creature? target,
    Decimal amount,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource,
    CardPlay? cardPlay)
  {
    return target != this.Owner || !props.IsPoweredAttack() ? 1M : this.DynamicVars["DamageDecrease"].BaseValue / 100M;
  }

  public override async Task AfterDamageReceived(
    PlayerChoiceContext choiceContext,
    Creature target,
    DamageResult result,
    ValueProp props,
    Creature? dealer,
    CardModel? cardSource)
  {
    if (target != this.Owner || result.UnblockedDamage == 0 || !props.IsPoweredAttack())
      return;
    await PowerCmd.Decrement((PowerModel) this);
    if (this.Amount > 0)
      return;
    await CreatureCmd.TriggerAnim(this.Owner, "StunTrigger", 0.6f);
    await CreatureCmd.Stun(this.Owner, new Func<IReadOnlyList<Creature>, Task>(this.StunnedMove), this.Owner.Monster.MoveStateMachine.StateLog.Last<MonsterState>().GetNextState(this.Owner, this.Owner.Monster.RunRng.MonsterAi));
    ((ThievingHopper) this.Owner.Monster).IsHovering = false;
    SfxCmd.StopLoop("event:/sfx/enemy/enemy_attacks/thieving_hopper/thieving_hopper_hover_loop");
    this.Flash();
    await Cmd.Wait(0.25f);
  }

  private Task StunnedMove(IReadOnlyList<Creature> targets) => Task.CompletedTask;
}
