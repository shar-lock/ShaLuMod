// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.Mawler
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class Mawler : MonsterModel
{
  private const int _clawRepeat = 2;

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 76, 72);
  }

  public override int MaxInitialHp => this.MinInitialHp;

  private int RipAndTearDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 16 /*0x10*/, 14);
  }

  private int ClawDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 5, 4);

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState state1 = new MoveState("RIP_AND_TEAR_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.RipAndTearMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.RipAndTearDamage)
    });
    MoveState state2 = new MoveState("ROAR_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.RoarMove), new AbstractIntent[1]
    {
      (AbstractIntent) new DebuffIntent()
    });
    MoveState moveState = new MoveState("CLAW_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ClawMove), new AbstractIntent[1]
    {
      (AbstractIntent) new MultiAttackIntent(this.ClawDamage, 2)
    });
    RandomBranchState randomBranchState = new RandomBranchState("RAND");
    state1.FollowUpState = (MonsterState) randomBranchState;
    state2.FollowUpState = (MonsterState) randomBranchState;
    moveState.FollowUpState = (MonsterState) randomBranchState;
    randomBranchState.AddBranch((MonsterState) state1, MoveRepeatType.CannotRepeat, 1f);
    randomBranchState.AddBranch((MonsterState) state2, MoveRepeatType.UseOnlyOnce, 1f);
    randomBranchState.AddBranch((MonsterState) moveState, MoveRepeatType.CannotRepeat, 1f);
    states.Add((MonsterState) state1);
    states.Add((MonsterState) state2);
    states.Add((MonsterState) moveState);
    states.Add((MonsterState) randomBranchState);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) moveState);
  }

  private async Task RipAndTearMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.RipAndTearDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.35f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
  }

  private async Task RoarMove(IReadOnlyList<Creature> targets)
  {
    await CreatureCmd.TriggerAnim(this.Creature, "Cast", 0.5f);
    IReadOnlyList<VulnerablePower> vulnerablePowerList = await PowerCmd.Apply<VulnerablePower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (IEnumerable<Creature>) targets, 3M, this.Creature, (CardModel) null);
  }

  private async Task ClawMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.ClawDamage).WithHitCount(2).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.35f).OnlyPlayAnimOnce().WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState animState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("roar");
    AnimState state2 = new AnimState("attack");
    AnimState state3 = new AnimState("hurt");
    AnimState state4 = new AnimState("die");
    state1.NextState = animState;
    state2.NextState = animState;
    state3.NextState = animState;
    CreatureAnimator animator = new CreatureAnimator(animState, controller);
    animator.AddAnyState("Idle", animState);
    animator.AddAnyState("Cast", state1);
    animator.AddAnyState("Attack", state2);
    animator.AddAnyState("Dead", state4);
    animator.AddAnyState("Hit", state3);
    return animator;
  }
}
