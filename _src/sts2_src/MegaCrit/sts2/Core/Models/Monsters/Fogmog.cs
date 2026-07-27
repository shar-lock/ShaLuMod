// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.Fogmog
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
using MegaCrit.Sts2.Core.MonsterMoves;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class Fogmog : MonsterModel
{
  private const string _swipeRandomMove = "SWIPE_RANDOM_MOVE";
  private const string _headbuttMove = "HEADBUTT_MOVE";
  private const string _summonTrigger = "Summon";
  private const string _sporesSfx = "event:/sfx/enemy/enemy_attacks/fogmog/fogmog_summon";

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 78, 74);
  }

  public override int MaxInitialHp => this.MinInitialHp;

  private int SwipeDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 9, 8);
  }

  private int HeadbuttDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 16 /*0x10*/, 14);
  }

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Plant;

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("ILLUSION_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.IllusionMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SummonIntent()
    });
    MoveState moveState = new MoveState("SWIPE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SwipeMove), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent(this.SwipeDamage),
      (AbstractIntent) new BuffIntent()
    });
    MoveState state1 = new MoveState("SWIPE_RANDOM_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SwipeMove), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent(this.SwipeDamage),
      (AbstractIntent) new BuffIntent()
    });
    MoveState state2 = new MoveState("HEADBUTT_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.HeadbuttMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.HeadbuttDamage)
    });
    RandomBranchState randomBranchState = new RandomBranchState("BRANCH");
    randomBranchState.AddBranch((MonsterState) state1, MoveRepeatType.CannotRepeat, (Func<float>) (() => 0.4f));
    randomBranchState.AddBranch((MonsterState) state2, MoveRepeatType.CannotRepeat, (Func<float>) (() => 0.6f));
    initialState.FollowUpState = (MonsterState) moveState;
    moveState.FollowUpState = (MonsterState) randomBranchState;
    state1.FollowUpState = (MonsterState) state2;
    state2.FollowUpState = (MonsterState) moveState;
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) moveState);
    states.Add((MonsterState) state1);
    states.Add((MonsterState) randomBranchState);
    states.Add((MonsterState) state2);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task IllusionMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/fogmog/fogmog_summon");
    await CreatureCmd.TriggerAnim(this.Creature, "Summon", 0.75f);
    if (!this.CombatState.IsLiveCombat())
      return;
    Creature creature = await CreatureCmd.Add<EyeWithTeeth>(this.CombatState, "illusion");
  }

  private async Task SwipeMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.SwipeDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.5f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 1M, this.Creature, (CardModel) null);
  }

  private async Task HeadbuttMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.HeadbuttDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.5f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("summon");
    AnimState state2 = new AnimState("attack");
    AnimState state3 = new AnimState("hurt");
    AnimState state4 = new AnimState("die");
    state1.NextState = initialState;
    state2.NextState = initialState;
    state3.NextState = initialState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Dead", state4);
    animator.AddAnyState("Hit", state3);
    animator.AddAnyState("Summon", state1);
    animator.AddAnyState("Attack", state2);
    return animator;
  }

  protected override bool ShouldShowMoveInBestiary(string moveStateId)
  {
    return moveStateId != "SWIPE_RANDOM_MOVE" && moveStateId != "HEADBUTT_MOVE";
  }
}
