// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.VineShambler
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
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class VineShambler : MonsterModel
{
  private const string _vineShamblerVfxPath = "vfx/monsters/vine_shambler_vines/vine_shambler_vines_vfx";
  private const int _swipeRepeat = 2;
  private const string _swipeTrigger = "SwipePower";
  private const string _vinesTrigger = "Vines";
  private const string _chompTrigger = "Chomp";
  private const string _chomp = "event:/sfx/enemy/enemy_attacks/vine_shambler/vine_shambler_chomp";
  private const string _defensiveSwipe = "event:/sfx/enemy/enemy_attacks/vine_shambler/vine_shambler_defensive_swipe";
  private const string _graspingVines = "event:/sfx/enemy/enemy_attacks/vine_shambler/vine_shambler_cast";

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 64 /*0x40*/, 61);
  }

  public override int MaxInitialHp => this.MinInitialHp;

  private int GraspingVinesDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 9, 8);
  }

  private int SwipeDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 7, 6);
  }

  private int ChompDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 18, 16 /*0x10*/);
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState moveState1 = new MoveState("GRASPING_VINES_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.GraspingVinesMove), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent(this.GraspingVinesDamage),
      (AbstractIntent) new CardDebuffIntent()
    });
    MoveState initialState = new MoveState("SWIPE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SwipeMove), new AbstractIntent[1]
    {
      (AbstractIntent) new MultiAttackIntent(this.SwipeDamage, 2)
    });
    MoveState moveState2 = new MoveState("CHOMP_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ChompMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.ChompDamage)
    });
    initialState.FollowUpState = (MonsterState) moveState1;
    moveState1.FollowUpState = (MonsterState) moveState2;
    moveState2.FollowUpState = (MonsterState) initialState;
    states.Add((MonsterState) moveState1);
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) moveState2);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task GraspingVinesMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.GraspingVinesDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Vines", 0.5f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/vine_shambler/vine_shambler_cast").WithHitFx("vfx/monsters/vine_shambler_vines/vine_shambler_vines_vfx").SpawningHitVfxOnEachCreature().WithHitVfxSpawnedAtBase().Execute((PlayerChoiceContext) null);
    IReadOnlyList<TangledPower> tangledPowerList = await PowerCmd.Apply<TangledPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (IEnumerable<Creature>) targets, 1M, this.Creature, (CardModel) null);
  }

  private async Task SwipeMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.SwipeDamage).WithHitCount(2).FromMonster((MonsterModel) this).OnlyPlayAnimOnce().WithAttackerAnim("SwipePower", 0.4f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/vine_shambler/vine_shambler_defensive_swipe").WithHitFx("vfx/vfx_scratch").Execute((PlayerChoiceContext) null);
  }

  private async Task ChompMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.ChompDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Chomp", 0.4f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/vine_shambler/vine_shambler_chomp").WithHitFx("vfx/vfx_bite").Execute((PlayerChoiceContext) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("cast");
    AnimState state2 = new AnimState("attack_chomp");
    AnimState state3 = new AnimState("attack_swipe");
    AnimState state4 = new AnimState("attack_vines");
    AnimState state5 = new AnimState("hurt");
    AnimState state6 = new AnimState("die");
    state1.NextState = initialState;
    state2.NextState = initialState;
    state3.NextState = initialState;
    state4.NextState = initialState;
    state5.NextState = initialState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Dead", state6);
    animator.AddAnyState("Hit", state5);
    animator.AddAnyState("Cast", state1);
    animator.AddAnyState("Chomp", state2);
    animator.AddAnyState("SwipePower", state3);
    animator.AddAnyState("Vines", state4);
    return animator;
  }
}
