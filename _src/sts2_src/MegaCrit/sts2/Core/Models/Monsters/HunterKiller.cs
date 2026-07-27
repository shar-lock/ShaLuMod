// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.HunterKiller
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
using MegaCrit.Sts2.Core.MonsterMoves;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class HunterKiller : MonsterModel
{
  private const string _tripleAttackTrigger = "TripleAttack";
  private const int _punctureRepeat = 3;

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 126, 121);
  }

  public override int MaxInitialHp => this.MinInitialHp;

  private int BiteDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 19, 17);
  }

  private int PunctureDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 8, 7);
  }

  public override string TakeDamageSfx
  {
    get => "event:/sfx/enemy/enemy_attacks/hunter_killer/hunter_killer_hurt";
  }

  public override string DeathSfx
  {
    get => "event:/sfx/enemy/enemy_attacks/hunter_killer/hunter_killer_die";
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("TENDERIZING_GOOP_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.GoopMove), new AbstractIntent[1]
    {
      (AbstractIntent) new DebuffIntent()
    });
    MoveState state1 = new MoveState("BITE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BiteMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.BiteDamage)
    });
    MoveState state2 = new MoveState("PUNCTURE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.PunctureMove), new AbstractIntent[1]
    {
      (AbstractIntent) new MultiAttackIntent(this.PunctureDamage, 3)
    });
    RandomBranchState randomBranchState = new RandomBranchState("RAND");
    initialState.FollowUpState = (MonsterState) randomBranchState;
    state1.FollowUpState = (MonsterState) randomBranchState;
    state2.FollowUpState = (MonsterState) randomBranchState;
    randomBranchState.AddBranch((MonsterState) state1, MoveRepeatType.CannotRepeat);
    randomBranchState.AddBranch((MonsterState) state2, 2);
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) state1);
    states.Add((MonsterState) state2);
    states.Add((MonsterState) randomBranchState);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task GoopMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play(this.CastSfx);
    await CreatureCmd.TriggerAnim(this.Creature, "Cast", 0.4f);
    IReadOnlyList<TenderPower> tenderPowerList = await PowerCmd.Apply<TenderPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (IEnumerable<Creature>) targets, 1M, this.Creature, (CardModel) null);
  }

  private async Task BiteMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.BiteDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.3f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_bite").Execute((PlayerChoiceContext) null);
  }

  private async Task PunctureMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.PunctureDamage).WithHitCount(3).OnlyPlayAnimOnce().FromMonster((MonsterModel) this).WithAttackerAnim("TripleAttack", 0.3f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("cast");
    AnimState state2 = new AnimState("attack");
    AnimState state3 = new AnimState("attack_triple");
    AnimState state4 = new AnimState("hurt");
    AnimState state5 = new AnimState("die");
    state1.NextState = initialState;
    state2.NextState = initialState;
    state4.NextState = initialState;
    state3.NextState = initialState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Cast", state1);
    animator.AddAnyState("Attack", state2);
    animator.AddAnyState("Dead", state5);
    animator.AddAnyState("Hit", state4);
    animator.AddAnyState("TripleAttack", state3);
    return animator;
  }
}
