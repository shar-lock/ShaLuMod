// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.PunchConstruct
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class PunchConstruct : MonsterModel
{
  private const string _attackDoubleTrigger = "DoubleAttack";
  private bool _startsWithFastPunch;
  private int _startingHpReduction;
  private const string _attackSingleSfx = "event:/sfx/enemy/enemy_attacks/punch_construct/punch_construct_attack_single";
  private const string _attackDoubleSfx = "event:/sfx/enemy/enemy_attacks/punch_construct/punch_construct_attack_double";
  private const string _buffSfx = "event:/sfx/enemy/enemy_attacks/punch_construct/punch_construct_buff";

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 60, 55);
  }

  public override int MaxInitialHp => this.MinInitialHp;

  private int StrongPunchDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 16 /*0x10*/, 14);
  }

  private int FastPunchDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 6, 5);
  }

  private int FastPunchRepeat => 2;

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Armor;

  public bool StartsWithFastPunch
  {
    get => this._startsWithFastPunch;
    set
    {
      this.AssertMutable();
      this._startsWithFastPunch = value;
    }
  }

  public int StartingHpReduction
  {
    get => this._startingHpReduction;
    set
    {
      this.AssertMutable();
      this._startingHpReduction = value;
    }
  }

  public override async Task AfterAddedToRoom()
  {
    await base.AfterAddedToRoom();
    ArtifactPower artifactPower = await PowerCmd.Apply<ArtifactPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 1M, this.Creature, (CardModel) null);
    if (this.StartingHpReduction <= 0)
      return;
    this.Creature.SetCurrentHpInternal((Decimal) Math.Max(1, this.Creature.CurrentHp - this.StartingHpReduction));
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState moveState1 = new MoveState("READY_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ReadyMove), new AbstractIntent[1]
    {
      (AbstractIntent) new DefendIntent()
    });
    MoveState moveState2 = new MoveState("STRONG_PUNCH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.StrongPunchMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.StrongPunchDamage)
    });
    MoveState moveState3 = new MoveState("FAST_PUNCH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.FastPunchMove), new AbstractIntent[2]
    {
      (AbstractIntent) new MultiAttackIntent(this.FastPunchDamage, this.FastPunchRepeat),
      (AbstractIntent) new DebuffIntent()
    });
    moveState1.FollowUpState = (MonsterState) moveState3;
    moveState3.FollowUpState = (MonsterState) moveState2;
    moveState2.FollowUpState = (MonsterState) moveState1;
    states.Add((MonsterState) moveState1);
    states.Add((MonsterState) moveState3);
    states.Add((MonsterState) moveState2);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, this.StartsWithFastPunch ? (MonsterState) moveState3 : (MonsterState) moveState1);
  }

  private async Task ReadyMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/punch_construct/punch_construct_buff");
    await CreatureCmd.TriggerAnim(this.Creature, "Cast", 0.8f);
    Decimal num = await CreatureCmd.GainBlock(this.Creature, 10M, ValueProp.Move, (CardPlay) null);
  }

  private async Task StrongPunchMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.StrongPunchDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.25f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/punch_construct/punch_construct_attack_single").WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
  }

  private async Task FastPunchMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.FastPunchDamage).WithHitCount(this.FastPunchRepeat).FromMonster((MonsterModel) this).WithAttackerAnim("DoubleAttack", 0.2f).OnlyPlayAnimOnce().WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/punch_construct/punch_construct_attack_double").WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
    IReadOnlyList<FrailPower> frailPowerList = await PowerCmd.Apply<FrailPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (IEnumerable<Creature>) targets, 1M, this.Creature, (CardModel) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("attack_double");
    AnimState state2 = new AnimState("block");
    AnimState state3 = new AnimState("attack");
    AnimState state4 = new AnimState("hurt");
    AnimState state5 = new AnimState("die");
    state2.NextState = initialState;
    state3.NextState = initialState;
    state4.NextState = initialState;
    state1.NextState = initialState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Cast", state2);
    animator.AddAnyState("Attack", state3);
    animator.AddAnyState("Dead", state5);
    animator.AddAnyState("Hit", state4);
    animator.AddAnyState("DoubleAttack", state1);
    return animator;
  }
}
