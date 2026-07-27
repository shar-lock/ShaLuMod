// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.SlitheringStrangler
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
using MegaCrit.Sts2.Core.MonsterMoves;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class SlitheringStrangler : MonsterModel
{
  private const string _attackDefendTrigger = "AttackDefendTrigger";
  private const string _attackHeadbuttSfx = "event:/sfx/enemy/enemy_attacks/slithering_strangler/slithering_strangler_attack_headbutt";
  private const string _attackTailSfx = "event:/sfx/enemy/enemy_attacks/slithering_strangler/slithering_strangler_tail";
  private const string _castSfx = "event:/sfx/enemy/enemy_attacks/slithering_strangler/slithering_strangler_cast";

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 54, 53);
  }

  public override int MaxInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 56, 55);
  }

  private int ThwackDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 8, 7);
  }

  private int LashDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 13, 12);
  }

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Plant;

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("CONSTRICT", new Func<IReadOnlyList<Creature>, Task>(this.ConstrictMove), new AbstractIntent[1]
    {
      (AbstractIntent) new DebuffIntent()
    });
    MoveState state1 = new MoveState("THWACK", new Func<IReadOnlyList<Creature>, Task>(this.ThwackMove), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent(this.ThwackDamage),
      (AbstractIntent) new DefendIntent()
    });
    MoveState state2 = new MoveState("LASH", new Func<IReadOnlyList<Creature>, Task>(this.LashMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.LashDamage)
    });
    RandomBranchState randomBranchState = new RandomBranchState("rand");
    initialState.FollowUpState = (MonsterState) randomBranchState;
    state1.FollowUpState = (MonsterState) initialState;
    state2.FollowUpState = (MonsterState) initialState;
    randomBranchState.AddBranch((MonsterState) state1, MoveRepeatType.CanRepeatForever);
    randomBranchState.AddBranch((MonsterState) state2, MoveRepeatType.CanRepeatForever);
    states.Add((MonsterState) randomBranchState);
    states.Add((MonsterState) state1);
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) state2);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task ConstrictMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/slithering_strangler/slithering_strangler_cast");
    await CreatureCmd.TriggerAnim(this.Creature, "Cast", 0.6f);
    IReadOnlyList<ConstrictPower> constrictPowerList = await PowerCmd.Apply<ConstrictPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (IEnumerable<Creature>) targets, 3M, this.Creature, (CardModel) null);
  }

  private async Task ThwackMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.ThwackDamage).FromMonster((MonsterModel) this).WithAttackerAnim("AttackDefendTrigger", 0.2f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/slithering_strangler/slithering_strangler_attack_headbutt").WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
    Decimal num = await CreatureCmd.GainBlock(this.Creature, 5M, ValueProp.Move, (CardPlay) null);
  }

  private async Task LashMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.LashDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.2f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/slithering_strangler/slithering_strangler_tail").WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("constrict");
    AnimState state2 = new AnimState("attack_defend");
    AnimState state3 = new AnimState("attack");
    AnimState state4 = new AnimState("hurt");
    AnimState state5 = new AnimState("die");
    state1.NextState = initialState;
    state3.NextState = initialState;
    state4.NextState = initialState;
    state2.NextState = initialState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Cast", state1);
    animator.AddAnyState("Attack", state3);
    animator.AddAnyState("Dead", state5);
    animator.AddAnyState("Hit", state4);
    animator.AddAnyState("AttackDefendTrigger", state2);
    return animator;
  }
}
