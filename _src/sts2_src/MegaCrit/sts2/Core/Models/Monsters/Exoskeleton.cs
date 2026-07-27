// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.Exoskeleton
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

public sealed class Exoskeleton : MonsterModel
{
  private const string _buffTrigger = "Buff";
  private const int _buffAmount = 2;
  private const string _heavyAttackTrigger = "HeavyAttack";
  private const string _attackHeavySfx = "event:/sfx/enemy/enemy_attacks/roaches/roaches_attack_heavy";
  private const string _buffSfx = "event:/sfx/enemy/enemy_attacks/roaches/roaches_buff";

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 25, 24);
  }

  public override int MaxInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 29, 28);
  }

  private int SkitterDamage => 1;

  private int SkitterRepeats
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 3);
  }

  private int MandiblesDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 9, 8);
  }

  protected override string AttackSfx => "event:/sfx/enemy/enemy_attacks/roaches/roaches_attack";

  public override string DeathSfx => "event:/sfx/enemy/enemy_attacks/roaches/roaches_die";

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Insect;

  public override async Task AfterAddedToRoom()
  {
    await base.AfterAddedToRoom();
    HardToKillPower hardToKillPower = await PowerCmd.Apply<HardToKillPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 9M, this.Creature, (CardModel) null);
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState moveState1 = new MoveState("SKITTER_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SkitterMove), new AbstractIntent[1]
    {
      (AbstractIntent) new MultiAttackIntent(this.SkitterDamage, this.SkitterRepeats)
    });
    MoveState moveState2 = new MoveState("MANDIBLES_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.MandiblesMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.MandiblesDamage)
    });
    MoveState move1 = new MoveState("ENRAGE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.EnrageMove), new AbstractIntent[1]
    {
      (AbstractIntent) new BuffIntent()
    });
    RandomBranchState move2 = new RandomBranchState("RAND");
    move2.AddBranch((MonsterState) moveState1, MoveRepeatType.CannotRepeat, 1f);
    move2.AddBranch((MonsterState) moveState2, MoveRepeatType.CannotRepeat, 1f);
    ConditionalBranchState initialState = new ConditionalBranchState("INIT_MOVE");
    initialState.AddState((MonsterState) moveState1, (Func<bool>) (() => this.Creature.SlotName == "first"));
    initialState.AddState((MonsterState) moveState2, (Func<bool>) (() => this.Creature.SlotName == "second"));
    initialState.AddState((MonsterState) move1, (Func<bool>) (() => this.Creature.SlotName == "third"));
    initialState.AddState((MonsterState) move2, (Func<bool>) (() => this.Creature.SlotName == "fourth"));
    moveState1.FollowUpState = (MonsterState) move2;
    moveState2.FollowUpState = (MonsterState) move1;
    move1.FollowUpState = (MonsterState) move2;
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) move2);
    states.Add((MonsterState) moveState1);
    states.Add((MonsterState) moveState2);
    states.Add((MonsterState) move1);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task SkitterMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.SkitterDamage).WithHitCount(this.SkitterRepeats).FromMonster((MonsterModel) this).OnlyPlayAnimOnce().WithAttackerAnim("Attack", 0.15f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
  }

  private async Task MandiblesMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.MandiblesDamage).FromMonster((MonsterModel) this).WithAttackerAnim("HeavyAttack", 0.3f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/roaches/roaches_attack_heavy").WithHitFx("vfx/vfx_bite").Execute((PlayerChoiceContext) null);
  }

  private async Task EnrageMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/roaches/roaches_buff");
    await CreatureCmd.TriggerAnim(this.Creature, "Buff", 0.3f);
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 2M, this.Creature, (CardModel) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("die");
    AnimState state2 = new AnimState("hurt");
    AnimState state3 = new AnimState("cast");
    AnimState state4 = new AnimState("buff");
    AnimState state5 = new AnimState("attack");
    AnimState state6 = new AnimState("attack_heavy");
    state5.NextState = initialState;
    state3.NextState = initialState;
    state4.NextState = initialState;
    state2.NextState = initialState;
    state6.NextState = initialState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Hit", state2);
    animator.AddAnyState("Dead", state1);
    animator.AddAnyState("Cast", state3);
    animator.AddAnyState("Attack", state5);
    animator.AddAnyState("HeavyAttack", state6);
    animator.AddAnyState("Buff", state4);
    return animator;
  }
}
