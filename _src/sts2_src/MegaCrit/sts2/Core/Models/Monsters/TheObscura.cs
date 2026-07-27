// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.TheObscura
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class TheObscura : MonsterModel
{
  private const string _summonTrigger = "Summon";
  private const string _piercingGazeMove = "PIERCING_GAZE_MOVE";
  private const string _summonSfx = "event:/sfx/enemy/enemy_attacks/obscura/obscura_summon";
  private const string _buffSfx = "event:/sfx/enemy/enemy_attacks/obscura/obscura_buff";
  private bool _hasSummoned;

  protected override bool HasPhobiaSpineSkin => true;

  protected override string AttackSfx => "event:/sfx/enemy/enemy_attacks/obscura/obscura_attack";

  public override string DeathSfx => "event:/sfx/enemy/enemy_attacks/obscura/obscura_die";

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 129, 123);
  }

  public override int MaxInitialHp => this.MinInitialHp;

  private int PiercingGazeDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 11, 10);
  }

  private int HardeningStrikeDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 7, 6);
  }

  private int HardeningStrikeBlock
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 7, 6);
  }

  private bool HasSummoned
  {
    get => this._hasSummoned;
    set
    {
      this.AssertMutable();
      this._hasSummoned = value;
    }
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("ILLUSION_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.IllusionMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SummonIntent()
    });
    MoveState state1 = new MoveState("PIERCING_GAZE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.PiercingGazeMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.PiercingGazeDamage)
    });
    MoveState state2 = new MoveState("SAIL_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.WailMove), new AbstractIntent[1]
    {
      (AbstractIntent) new BuffIntent()
    });
    MoveState state3 = new MoveState("HARDENING_STRIKE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.HardeningStrikeMove), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent(this.HardeningStrikeDamage),
      (AbstractIntent) new DefendIntent()
    });
    RandomBranchState randomBranchState = new RandomBranchState("RAND");
    initialState.FollowUpState = (MonsterState) randomBranchState;
    state1.FollowUpState = (MonsterState) randomBranchState;
    state2.FollowUpState = (MonsterState) randomBranchState;
    state3.FollowUpState = (MonsterState) randomBranchState;
    randomBranchState.AddBranch((MonsterState) state1, MoveRepeatType.CannotRepeat);
    randomBranchState.AddBranch((MonsterState) state2, MoveRepeatType.CannotRepeat);
    randomBranchState.AddBranch((MonsterState) state3, MoveRepeatType.CannotRepeat);
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) state1);
    states.Add((MonsterState) state2);
    states.Add((MonsterState) state3);
    states.Add((MonsterState) randomBranchState);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task IllusionMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/obscura/obscura_summon");
    await CreatureCmd.TriggerAnim(this.Creature, "Summon", 0.15f);
    if (this.CombatState.IsLiveCombat())
    {
      Creature creature = await CreatureCmd.Add<Parafright>(this.CombatState, "illusion");
    }
    this.HasSummoned = true;
  }

  private async Task PiercingGazeMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.PiercingGazeDamage).FromMonster((MonsterModel) this).WithAttackerFx(sfx: this.AttackSfx).WithAttackerAnim("Attack", 0.3f).WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
  }

  private async Task WailMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/obscura/obscura_buff");
    await CreatureCmd.TriggerAnim(this.Creature, "Cast", 0.7f);
    IReadOnlyList<StrengthPower> strengthPowerList = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (IEnumerable<Creature>) this.CombatState.GetTeammatesOf(this.Creature), 3M, this.Creature, (CardModel) null);
  }

  private async Task HardeningStrikeMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.HardeningStrikeDamage).FromMonster((MonsterModel) this).WithAttackerFx(sfx: this.AttackSfx).WithAttackerAnim("Attack", 0.3f).WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
    Decimal num = await CreatureCmd.GainBlock(this.Creature, (Decimal) this.HardeningStrikeBlock, ValueProp.Move, (CardPlay) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState animState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("attack");
    AnimState state2 = new AnimState("die");
    AnimState state3 = new AnimState("hurt");
    AnimState state4 = new AnimState("cast");
    AnimState state5 = new AnimState("cast_intro");
    AnimState state6 = new AnimState("hurt_intro");
    AnimState state7 = new AnimState("die_intro");
    AnimState initialState = new AnimState("intro_loop", true);
    state1.NextState = animState;
    state3.NextState = animState;
    state4.NextState = animState;
    state5.NextState = animState;
    state6.NextState = initialState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Attack", state1);
    animator.AddAnyState("Hit", state3, (Func<bool>) (() => this.HasSummoned));
    animator.AddAnyState("Hit", state6, (Func<bool>) (() => !this.HasSummoned));
    animator.AddAnyState("Dead", state2, (Func<bool>) (() => this.HasSummoned));
    animator.AddAnyState("Dead", state7, (Func<bool>) (() => !this.HasSummoned));
    animator.AddAnyState("Cast", state4);
    animator.AddAnyState("Summon", state5);
    return animator;
  }

  protected override bool ShouldShowMoveInBestiary(string moveStateId)
  {
    return moveStateId != "PIERCING_GAZE_MOVE";
  }
}
