// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.Inklet
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

public sealed class Inklet : MonsterModel
{
  private const string _piercingGazeMove = "PIERCING_GAZE_MOVE";
  private const string _attackTripleTrigger = "TRIPLE_ATTACK";
  private const int _whirlwindRepeat = 3;
  private bool _middleInklet;
  private const string _attackTripleSfx = "event:/sfx/enemy/enemy_attacks/inklet/inklet_attack_triple";

  public override float HurtAnimationTrackOffsetForDoom => 0.04f;

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 12, 11);
  }

  public override int MaxInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 18, 17);
  }

  private int JabDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 3);

  private int WhirlwindDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 3, 2);
  }

  private int PiercingGazeDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 11, 10);
  }

  public bool MiddleInklet
  {
    get => this._middleInklet;
    set
    {
      this.AssertMutable();
      this._middleInklet = value;
    }
  }

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Magic;

  public override string HurtSfx => "event:/sfx/enemy/enemy_attacks/inklet/inklet_hurt";

  public override async Task AfterAddedToRoom()
  {
    await base.AfterAddedToRoom();
    SlipperyPower slipperyPower = await PowerCmd.Apply<SlipperyPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 1M, this.Creature, (CardModel) null);
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState state1 = new MoveState("JAB_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.JabMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.JabDamage)
    });
    MoveState state2 = new MoveState("WHIRLWIND_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.WhirlwindMove), new AbstractIntent[1]
    {
      (AbstractIntent) new MultiAttackIntent(this.WhirlwindDamage, 3)
    });
    MoveState state3 = new MoveState("PIERCING_GAZE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.PiercingGazeMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.PiercingGazeDamage)
    });
    RandomBranchState randomBranchState1 = new RandomBranchState("INIT_RAND");
    RandomBranchState randomBranchState2 = new RandomBranchState("RAND");
    state1.FollowUpState = (MonsterState) randomBranchState2;
    state3.FollowUpState = (MonsterState) randomBranchState2;
    state2.FollowUpState = (MonsterState) randomBranchState2;
    randomBranchState1.AddBranch((MonsterState) state1, 2, 1f);
    randomBranchState1.AddBranch((MonsterState) state2, MoveRepeatType.CannotRepeat, 1f);
    randomBranchState2.AddBranch((MonsterState) state3, MoveRepeatType.CannotRepeat, 1f);
    randomBranchState2.AddBranch((MonsterState) state2, MoveRepeatType.CannotRepeat, 1f);
    state1.FollowUpState = (MonsterState) randomBranchState2;
    state2.FollowUpState = (MonsterState) state1;
    state3.FollowUpState = (MonsterState) state1;
    states.Add((MonsterState) state1);
    states.Add((MonsterState) state3);
    states.Add((MonsterState) state2);
    states.Add((MonsterState) randomBranchState2);
    MoveState initialState = this._middleInklet ? state2 : state1;
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task JabMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.JabDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.75f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
  }

  private async Task WhirlwindMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.WhirlwindDamage).WithHitCount(3).FromMonster((MonsterModel) this).WithAttackerAnim("TRIPLE_ATTACK", 0.3f).OnlyPlayAnimOnce().WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/inklet/inklet_attack_triple").WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
  }

  private async Task PiercingGazeMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.PiercingGazeDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.75f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("cast");
    AnimState state2 = new AnimState("attack");
    AnimState state3 = new AnimState("attack_fast");
    AnimState state4 = new AnimState("hurt");
    AnimState state5 = new AnimState("die");
    state1.NextState = initialState;
    state2.NextState = initialState;
    state4.NextState = initialState;
    state3.NextState = initialState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Cast", state1);
    animator.AddAnyState("Attack", state2);
    animator.AddAnyState("TRIPLE_ATTACK", state3);
    animator.AddAnyState("Dead", state5);
    animator.AddAnyState("Hit", state4);
    return animator;
  }

  protected override bool ShouldShowMoveInBestiary(string moveStateId)
  {
    return moveStateId != "PIERCING_GAZE_MOVE";
  }
}
