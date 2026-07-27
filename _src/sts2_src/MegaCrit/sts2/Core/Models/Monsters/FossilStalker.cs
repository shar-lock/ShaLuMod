// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.FossilStalker
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

public sealed class FossilStalker : MonsterModel
{
  private const string _attackDoubleTrigger = "AttackDouble";
  private const string _attackBuff = "event:/sfx/enemy/enemy_attacks/fossil_stalker/fossil_stalker_attack_buff";
  private const string _attackDouble = "event:/sfx/enemy/enemy_attacks/fossil_stalker/fossil_stalker_attack_double";
  private const string _attackSingle = "event:/sfx/enemy/enemy_attacks/fossil_stalker/fossil_stalker_attack_single";

  protected override bool HasPhobiaSpineSkin => true;

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 54, 51);
  }

  public override int MaxInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 56, 53);
  }

  private int TackleDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 11, 9);
  }

  private int LatchDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 14, 12);
  }

  private int LashDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 3);

  private int LashRepeat => 2;

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Stone;

  public override string HurtSfx
  {
    get => "event:/sfx/enemy/enemy_attacks/fossil_stalker/fossil_stalker_hurt";
  }

  public override async Task AfterAddedToRoom()
  {
    await base.AfterAddedToRoom();
    SuckPower suckPower = await PowerCmd.Apply<SuckPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 3M, this.Creature, (CardModel) null);
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState state1 = new MoveState("TACKLE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.TackleMove), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent(this.TackleDamage),
      (AbstractIntent) new DebuffIntent()
    });
    MoveState moveState = new MoveState("LATCH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.LatchMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.LatchDamage)
    });
    MoveState state2 = new MoveState("LASH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.LashAttack), new AbstractIntent[1]
    {
      (AbstractIntent) new MultiAttackIntent(this.LashDamage, this.LashRepeat)
    });
    RandomBranchState randomBranchState = new RandomBranchState("RAND");
    moveState.FollowUpState = (MonsterState) randomBranchState;
    state1.FollowUpState = (MonsterState) randomBranchState;
    state2.FollowUpState = (MonsterState) randomBranchState;
    randomBranchState.AddBranch((MonsterState) moveState, 2);
    randomBranchState.AddBranch((MonsterState) state1, 2);
    randomBranchState.AddBranch((MonsterState) state2, 2);
    states.Add((MonsterState) randomBranchState);
    states.Add((MonsterState) state1);
    states.Add((MonsterState) moveState);
    states.Add((MonsterState) state2);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) moveState);
  }

  private async Task TackleMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.TackleDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Cast", 0.35f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/fossil_stalker/fossil_stalker_attack_buff").WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
    IReadOnlyList<FrailPower> frailPowerList = await PowerCmd.Apply<FrailPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (IEnumerable<Creature>) targets, 1M, this.Creature, (CardModel) null);
  }

  private async Task LatchMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.LatchDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.2f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/fossil_stalker/fossil_stalker_attack_single").WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
  }

  private async Task LashAttack(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.LashDamage).WithHitCount(this.LashRepeat).FromMonster((MonsterModel) this).WithAttackerAnim("AttackDouble", 0.2f).OnlyPlayAnimOnce().WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/fossil_stalker/fossil_stalker_attack_double").WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState animState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("debuff");
    AnimState state2 = new AnimState("attack");
    AnimState state3 = new AnimState("hurt");
    AnimState state4 = new AnimState("die");
    AnimState state5 = new AnimState("attack_double");
    state1.NextState = animState;
    state2.NextState = animState;
    state3.NextState = animState;
    state5.NextState = animState;
    CreatureAnimator animator = new CreatureAnimator(animState, controller);
    animator.AddAnyState("Idle", animState);
    animator.AddAnyState("Cast", state1);
    animator.AddAnyState("Attack", state2);
    animator.AddAnyState("Dead", state4);
    animator.AddAnyState("Hit", state3);
    animator.AddAnyState("AttackDouble", state5);
    return animator;
  }
}
