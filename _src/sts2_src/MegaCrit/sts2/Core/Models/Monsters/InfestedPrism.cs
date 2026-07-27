// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.InfestedPrism
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

public sealed class InfestedPrism : MonsterModel
{
  private const string _buffSfx = "event:/sfx/enemy/enemy_attacks/infested_prisms/infested_prisms_buff";
  private const string _attackDefendSfx = "event:/sfx/enemy/enemy_attacks/infested_prisms/infested_prisms_attack_defend";
  private const string _attackSpinSfx = "event:/sfx/enemy/enemy_attacks/infested_prisms/infested_prisms_attack_spin";
  private const string _attackBlockTrigger = "AttackBlock";
  private const string _attackDoubleTrigger = "AttackDouble";

  public override string DeathSfx
  {
    get => "event:/sfx/enemy/enemy_attacks/infested_prisms/infested_prisms_die";
  }

  protected override string AttackSfx
  {
    get => "event:/sfx/enemy/enemy_attacks/infested_prisms/infested_prisms_attack";
  }

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 171, 161);
  }

  public override int MaxInitialHp => this.MinInitialHp;

  private int JabDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 17, 15);
  }

  private int VitalSparkAmount
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 3, 2);
  }

  private int PulsateDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 10, 8);
  }

  private int PulsateBlock
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 22, 20);
  }

  private int RadiateDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 13, 11);
  }

  private int RadiateBlock
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 13, 11);
  }

  private int WhirlwindDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 6, 5);
  }

  private int WhirlwindRepeat => 3;

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Stone;

  public override async Task AfterAddedToRoom()
  {
    await base.AfterAddedToRoom();
    VitalSparkPower vitalSparkPower = await PowerCmd.Apply<VitalSparkPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, (Decimal) this.VitalSparkAmount, this.Creature, (CardModel) null);
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("JAB_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.JabMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.JabDamage)
    });
    MoveState moveState1 = new MoveState("RADIATE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.RadiateMove), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent(this.RadiateDamage),
      (AbstractIntent) new DefendIntent()
    });
    MoveState moveState2 = new MoveState("WHIRLWIND_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.WhirlwindMove), new AbstractIntent[1]
    {
      (AbstractIntent) new MultiAttackIntent(this.WhirlwindDamage, this.WhirlwindRepeat)
    });
    MoveState moveState3 = new MoveState("PULSATE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.PulsateMove), new AbstractIntent[3]
    {
      (AbstractIntent) new SingleAttackIntent(this.PulsateDamage),
      (AbstractIntent) new BuffIntent(),
      (AbstractIntent) new DefendIntent()
    });
    initialState.FollowUpState = (MonsterState) moveState1;
    moveState1.FollowUpState = (MonsterState) moveState2;
    moveState2.FollowUpState = (MonsterState) moveState3;
    moveState3.FollowUpState = (MonsterState) initialState;
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) moveState1);
    states.Add((MonsterState) moveState2);
    states.Add((MonsterState) moveState3);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task JabMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.JabDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.1f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
  }

  private async Task RadiateMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.RadiateDamage).FromMonster((MonsterModel) this).WithAttackerAnim("AttackBlock", 0.25f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/infested_prisms/infested_prisms_attack_defend").WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
    Decimal num = await CreatureCmd.GainBlock(this.Creature, (Decimal) this.RadiateBlock, ValueProp.Move, (CardPlay) null);
  }

  private async Task WhirlwindMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.WhirlwindDamage).WithHitCount(this.WhirlwindRepeat).FromMonster((MonsterModel) this).WithAttackerAnim("AttackDouble", 0.2f).OnlyPlayAnimOnce().WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/infested_prisms/infested_prisms_attack_spin").WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
  }

  private async Task PulsateMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.PulsateDamage).FromMonster((MonsterModel) this).WithAttackerAnim("AttackBlock", 0.25f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/infested_prisms/infested_prisms_attack_defend").WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
    Decimal num = await CreatureCmd.GainBlock(this.Creature, (Decimal) this.PulsateBlock, ValueProp.Move, (CardPlay) null);
    VitalSparkPower vitalSparkPower = await PowerCmd.Apply<VitalSparkPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, (Decimal) this.VitalSparkAmount, this.Creature, (CardModel) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("buff");
    AnimState state2 = new AnimState("attack");
    AnimState state3 = new AnimState("attack_block");
    AnimState state4 = new AnimState("attack_double");
    AnimState state5 = new AnimState("hurt");
    AnimState state6 = new AnimState("die");
    state1.NextState = initialState;
    state2.NextState = initialState;
    state5.NextState = initialState;
    state3.NextState = initialState;
    state4.NextState = initialState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Cast", state1);
    animator.AddAnyState("Attack", state2);
    animator.AddAnyState("Dead", state6);
    animator.AddAnyState("Hit", state5);
    animator.AddAnyState("AttackBlock", state3);
    animator.AddAnyState("AttackDouble", state4);
    return animator;
  }
}
