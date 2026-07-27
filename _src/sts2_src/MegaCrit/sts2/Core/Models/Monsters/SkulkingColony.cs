// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.SkulkingColony
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class SkulkingColony : MonsterModel
{
  private const string _attackDoubleTrigger = "AttackDouble";
  private const string _attackBuffTrigger = "AttackBuff";
  private const string _attackHeavyTrigger = "AttackHeavy";
  private const string _kickSfx = "event:/sfx/enemy/enemy_attacks/skulking_colony/skulking_colony_kick";
  private const string _spinSfx = "event:/sfx/enemy/enemy_attacks/skulking_colony/skulking_colony_spin";
  private const string _slapSfx = "event:/sfx/enemy/enemy_attacks/skulking_colony/skulking_colony_slap";
  private const string _thrustSfx = "event:/sfx/enemy/enemy_attacks/skulking_colony/skulking_colony_thrust";

  protected override bool HasPhobiaSpineSkin => true;

  public override string HurtSfx
  {
    get => "event:/sfx/enemy/enemy_attacks/skulking_colony/skulking_colony_hurt";
  }

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 80 /*0x50*/, 75);
  }

  public override int MaxInitialHp => this.MinInitialHp;

  private int InertiaDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 11, 9);
  }

  private int ZoomDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 16 /*0x10*/, 14);
  }

  private int PiercingStabsDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 8, 7);
  }

  private int PiercingStabsRepeat => 2;

  private int InertiaStrengthGain
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 2);
  }

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Stone;

  public override async Task AfterAddedToRoom()
  {
    await base.AfterAddedToRoom();
    HardenedShellPower hardenedShellPower = await PowerCmd.Apply<HardenedShellPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 20M, this.Creature, (CardModel) null);
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("ZOOM_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ZoomMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.ZoomDamage)
    });
    MoveState moveState1 = new MoveState("ZOOM_MOVE_2", new Func<IReadOnlyList<Creature>, Task>(this.ZoomMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.ZoomDamage)
    });
    MoveState moveState2 = new MoveState("INERTIA_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.InertiaMove), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent(this.InertiaDamage),
      (AbstractIntent) new BuffIntent()
    });
    MoveState moveState3 = new MoveState("PIERCING_STABS_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.PiercingStabsMove), new AbstractIntent[1]
    {
      (AbstractIntent) new MultiAttackIntent(this.PiercingStabsDamage, this.PiercingStabsRepeat)
    });
    initialState.FollowUpState = (MonsterState) moveState1;
    moveState1.FollowUpState = (MonsterState) moveState2;
    moveState2.FollowUpState = (MonsterState) moveState3;
    moveState3.FollowUpState = (MonsterState) initialState;
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) moveState1);
    states.Add((MonsterState) moveState2);
    states.Add((MonsterState) moveState3);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task InertiaMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.InertiaDamage).FromMonster((MonsterModel) this).WithAttackerAnim("AttackBuff", 0.5f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/skulking_colony/skulking_colony_thrust").WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, (Decimal) this.InertiaStrengthGain, this.Creature, (CardModel) null);
  }

  private async Task PiercingStabsMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.PiercingStabsDamage).WithHitCount(this.PiercingStabsRepeat).OnlyPlayAnimOnce().FromMonster((MonsterModel) this).WithAttackerAnim("AttackDouble", 0.45f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/skulking_colony/skulking_colony_spin").WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
  }

  private async Task ZoomMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.ZoomDamage).FromMonster((MonsterModel) this).WithAttackerAnim("AttackHeavy", 0.25f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/skulking_colony/skulking_colony_kick").WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState animState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("cast");
    AnimState state2 = new AnimState("attack_buff");
    AnimState state3 = new AnimState("attack_heavy");
    AnimState state4 = new AnimState("attack_double");
    AnimState state5 = new AnimState("hurt");
    AnimState state6 = new AnimState("die");
    state1.NextState = animState;
    state5.NextState = animState;
    state2.NextState = animState;
    state3.NextState = animState;
    state4.NextState = animState;
    CreatureAnimator animator = new CreatureAnimator(animState, controller);
    animator.AddAnyState("Idle", animState);
    animator.AddAnyState("Cast", state1);
    animator.AddAnyState("AttackBuff", state2);
    animator.AddAnyState("AttackHeavy", state3);
    animator.AddAnyState("AttackDouble", state4);
    animator.AddAnyState("Dead", state6);
    animator.AddAnyState("Hit", state5);
    return animator;
  }
}
