// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.Entomancer
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

public sealed class Entomancer : MonsterModel
{
  private const string _rangedAttackMove = "attack_ranged";
  private const string _attackRangedSfx = "event:/sfx/enemy/enemy_attacks/entomancer/entomancer_attack_ranged";

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 155, 145);
  }

  public override int MaxInitialHp => this.MinInitialHp;

  private int SpearMoveDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 20, 18);
  }

  private int BeesRepeat => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 8, 7);

  private int BeesDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 3, 3);

  public override string DeathSfx => "event:/sfx/enemy/enemy_attacks/entomancer/entomancer_die";

  public override async Task AfterAddedToRoom()
  {
    await base.AfterAddedToRoom();
    PersonalHivePower personalHivePower = await PowerCmd.Apply<PersonalHivePower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 1M, this.Creature, (CardModel) null);
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState moveState1 = new MoveState("PHEROMONE_SPIT_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SpitMove), new AbstractIntent[1]
    {
      (AbstractIntent) new BuffIntent()
    });
    MoveState initialState = new MoveState("BEES_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BeesMove), new AbstractIntent[1]
    {
      (AbstractIntent) new MultiAttackIntent(this.BeesDamage, this.BeesRepeat)
    });
    MoveState moveState2 = new MoveState("SPEAR_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SpearMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.SpearMoveDamage)
    });
    initialState.FollowUpState = (MonsterState) moveState2;
    moveState2.FollowUpState = (MonsterState) moveState1;
    moveState1.FollowUpState = (MonsterState) initialState;
    states.Add((MonsterState) moveState1);
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) moveState2);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task SpitMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play(this.CastSfx);
    await CreatureCmd.TriggerAnim(this.Creature, "Cast", 0.5f);
    PersonalHivePower personalHivePower1 = this.Creature.Powers.OfType<PersonalHivePower>().FirstOrDefault<PersonalHivePower>();
    if (personalHivePower1 != null && personalHivePower1.Amount < 3)
    {
      PersonalHivePower personalHivePower2 = await PowerCmd.Apply<PersonalHivePower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 1M, this.Creature, (CardModel) null);
      StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 1M, this.Creature, (CardModel) null);
    }
    else
    {
      StrengthPower strengthPower1 = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 2M, this.Creature, (CardModel) null);
    }
  }

  private async Task BeesMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.BeesDamage).WithHitCount(this.BeesRepeat).FromMonster((MonsterModel) this).WithAttackerAnim("attack_ranged", 0.3f).OnlyPlayAnimOnce().WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/entomancer/entomancer_attack_ranged").WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
  }

  private async Task SpearMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.SpearMoveDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.25f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/entomancer/entomancer_attack_ranged").WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("cast");
    AnimState state2 = new AnimState("attack");
    AnimState state3 = new AnimState("hurt");
    AnimState state4 = new AnimState("die");
    AnimState state5 = new AnimState("attack_ranged");
    state1.NextState = initialState;
    state2.NextState = initialState;
    state5.NextState = initialState;
    state3.NextState = initialState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Cast", state1);
    animator.AddAnyState("Attack", state2);
    animator.AddAnyState("attack_ranged", state5);
    animator.AddAnyState("Dead", state4);
    animator.AddAnyState("Hit", state3);
    return animator;
  }
}
