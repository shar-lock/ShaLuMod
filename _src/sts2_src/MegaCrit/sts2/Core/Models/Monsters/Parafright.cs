// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.Parafright
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
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class Parafright : MonsterModel
{
  public const string healSfx = "event:/sfx/enemy/enemy_attacks/obscura/obscura_hologram_heal";

  protected override string AttackSfx
  {
    get => "event:/sfx/enemy/enemy_attacks/obscura/obscura_hologram_attack";
  }

  public override string DeathSfx => "event:/sfx/enemy/enemy_attacks/obscura/obscura_hologram_die";

  public override int MinInitialHp => 21;

  public override int MaxInitialHp => this.MinInitialHp;

  private int SlamDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 17, 16 /*0x10*/);
  }

  public override bool HasDeathSfx => false;

  public override bool ShouldDisappearFromDoom => false;

  public override async Task AfterAddedToRoom()
  {
    await base.AfterAddedToRoom();
    IllusionPower illusionPower = await PowerCmd.Apply<IllusionPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 1M, this.Creature, (CardModel) null);
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("SLAM_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SlamMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.SlamDamage)
    });
    initialState.FollowUpState = (MonsterState) initialState;
    states.Add((MonsterState) initialState);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task SlamMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.SlamDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.15f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState animState1 = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("attack");
    AnimState state2 = new AnimState("die");
    AnimState state3 = new AnimState("hurt");
    AnimState state4 = new AnimState("hurt_stunned");
    AnimState animState2 = new AnimState("stunned_loop", true);
    AnimState initialState = new AnimState("spawn");
    AnimState state5 = new AnimState("wake_up");
    AnimState state6 = new AnimState("stun");
    initialState.NextState = animState1;
    state1.NextState = animState1;
    state3.NextState = animState1;
    state5.NextState = animState1;
    state6.NextState = animState2;
    state4.NextState = animState2;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Attack", state1);
    animator.AddAnyState("Hit", state3, (Func<bool>) (() => !this.Creature.GetPower<IllusionPower>().IsReviving));
    animator.AddAnyState("Hit", state4, (Func<bool>) (() => this.Creature.GetPower<IllusionPower>().IsReviving));
    animator.AddAnyState("StunTrigger", state6);
    animator.AddAnyState("WakeUpTrigger", state5);
    animator.AddAnyState("Dead", state2, (Func<bool>) (() => !this.CombatState.GetTeammatesOf(this.Creature).Any<Creature>((Func<Creature, bool>) (t => t != null && t.IsPrimaryEnemy && t.IsAlive))));
    return animator;
  }
}
