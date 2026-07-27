// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.SneakyGremlin
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
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class SneakyGremlin : MonsterModel
{
  private const string _wakeUpTrigger = "WakeUpTrigger";
  private bool _isAwake;

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 11, 10);
  }

  public override int MaxInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 15, 14);
  }

  private int TackleDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 10, 9);
  }

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Insect;

  protected override string AttackSfx
  {
    get => "event:/sfx/enemy/enemy_attacks/gremlin_merc/sneaky_gremlin_attack";
  }

  public override string DeathSfx
  {
    get => "event:/sfx/enemy/enemy_attacks/gremlin_merc/sneaky_gremlin_die";
  }

  private bool IsAwake
  {
    get => this._isAwake;
    set
    {
      this.AssertMutable();
      this._isAwake = value;
    }
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("SPAWNED_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SpawnedMove), new AbstractIntent[1]
    {
      (AbstractIntent) new StunIntent()
    });
    MoveState moveState = new MoveState("TACKLE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.TackleMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.TackleDamage)
    });
    initialState.FollowUpState = (MonsterState) moveState;
    moveState.FollowUpState = (MonsterState) moveState;
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) moveState);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task SpawnedMove(IReadOnlyList<Creature> targets)
  {
    await CreatureCmd.TriggerAnim(this.Creature, "WakeUpTrigger", 0.8f);
    this.IsAwake = true;
  }

  private async Task TackleMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.TackleDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.1f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState state1 = new AnimState("awake_loop", true);
    AnimState initialState = new AnimState("spawn");
    AnimState state2 = new AnimState("attack");
    AnimState animState = new AnimState("stunned_loop", true);
    AnimState state3 = new AnimState("wake_up");
    AnimState state4 = new AnimState("hurt_stunned");
    AnimState state5 = new AnimState("hurt_awake");
    AnimState state6 = new AnimState("die");
    initialState.NextState = animState;
    state4.NextState = animState;
    state5.NextState = state1;
    state3.NextState = state1;
    state2.NextState = state1;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Idle", state1);
    animator.AddAnyState("WakeUpTrigger", state3);
    animator.AddAnyState("Attack", state2);
    animator.AddAnyState("Dead", state6);
    animator.AddAnyState("Hit", state5, (Func<bool>) (() => this.IsAwake));
    animator.AddAnyState("Hit", state4, (Func<bool>) (() => !this.IsAwake));
    return animator;
  }
}
