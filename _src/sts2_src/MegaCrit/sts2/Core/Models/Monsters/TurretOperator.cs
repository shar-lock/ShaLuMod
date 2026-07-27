// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.TurretOperator
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

public sealed class TurretOperator : MonsterModel
{
  private const string _buffSfx = "event:/sfx/enemy/enemy_attacks/turret_operator/turret_operator_buff";
  private const int _fireRepeat = 5;
  private const string _crankTrigger = "Crank";

  public override string HurtSfx
  {
    get => "event:/sfx/enemy/enemy_attacks/turret_operator/turret_operator_hurt";
  }

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 51, 41);
  }

  public override int MaxInitialHp => this.MinInitialHp;

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Fur;

  private int FireDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 3);

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("UNLOAD_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.UnloadMove), new AbstractIntent[1]
    {
      (AbstractIntent) new MultiAttackIntent(this.FireDamage, 5)
    });
    MoveState moveState1 = new MoveState("UNLOAD_MOVE_2", new Func<IReadOnlyList<Creature>, Task>(this.UnloadMove), new AbstractIntent[1]
    {
      (AbstractIntent) new MultiAttackIntent(this.FireDamage, 5)
    });
    MoveState moveState2 = new MoveState("RELOAD_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ReloadMove), new AbstractIntent[1]
    {
      (AbstractIntent) new BuffIntent()
    });
    initialState.FollowUpState = (MonsterState) moveState1;
    moveState1.FollowUpState = (MonsterState) moveState2;
    moveState2.FollowUpState = (MonsterState) initialState;
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) moveState1);
    states.Add((MonsterState) moveState2);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task ReloadMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/turret_operator/turret_operator_buff");
    await CreatureCmd.TriggerAnim(this.Creature, "Crank", 0.4f);
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 1M, this.Creature, (CardModel) null);
  }

  private async Task UnloadMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.FireDamage).WithHitCount(5).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.4f).OnlyPlayAnimOnce().WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("crank");
    AnimState state2 = new AnimState("attack");
    AnimState state3 = new AnimState("hurt");
    AnimState state4 = new AnimState("die");
    state1.NextState = initialState;
    state2.NextState = initialState;
    state3.NextState = initialState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Crank", state1);
    animator.AddAnyState("Attack", state2);
    animator.AddAnyState("Dead", state4);
    animator.AddAnyState("Hit", state3);
    return animator;
  }
}
