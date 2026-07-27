// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.FlailKnight
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

public class FlailKnight : MonsterModel
{
  private const string _flailAttackTrigger = "FlailAttack";
  private const string _ramAttackTrigger = "RamAttack";
  private const string _flailSfx = "event:/sfx/enemy/enemy_attacks/flail_knight/flail_knight_flail";
  private const string _chantSfx = "event:/sfx/enemy/enemy_attacks/flail_knight/flail_knight_war_chant";
  private const string _ramSfx = "event:/sfx/enemy/enemy_attacks/flail_knight/flail_knight_ram";
  private const int _flailRepeat = 2;

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 108, 101);
  }

  public override int MaxInitialHp => this.MinInitialHp;

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Armor;

  private int FlailDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 10, 9);
  }

  private int RamDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 17, 15);
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState state1 = new MoveState("WAR_CHANT", new Func<IReadOnlyList<Creature>, Task>(this.WarChantMove), new AbstractIntent[1]
    {
      (AbstractIntent) new BuffIntent()
    });
    MoveState state2 = new MoveState("FLAIL_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.FlailMove), new AbstractIntent[1]
    {
      (AbstractIntent) new MultiAttackIntent(this.FlailDamage, 2)
    });
    MoveState moveState = new MoveState("RAM_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.RamMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.RamDamage)
    });
    RandomBranchState randomBranchState = new RandomBranchState("RAND");
    state1.FollowUpState = (MonsterState) randomBranchState;
    state2.FollowUpState = (MonsterState) randomBranchState;
    moveState.FollowUpState = (MonsterState) randomBranchState;
    randomBranchState.AddBranch((MonsterState) state1, MoveRepeatType.CannotRepeat);
    randomBranchState.AddBranch((MonsterState) state2, 2);
    randomBranchState.AddBranch((MonsterState) moveState, 2);
    states.Add((MonsterState) state1);
    states.Add((MonsterState) state2);
    states.Add((MonsterState) moveState);
    states.Add((MonsterState) randomBranchState);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) moveState);
  }

  private async Task WarChantMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/flail_knight/flail_knight_war_chant");
    await CreatureCmd.TriggerAnim(this.Creature, "Cast", 0.5f);
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 3M, this.Creature, (CardModel) null);
  }

  public async Task FlailMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.FlailDamage).WithHitCount(2).FromMonster((MonsterModel) this).OnlyPlayAnimOnce().WithAttackerAnim("FlailAttack", 0.5f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/flail_knight/flail_knight_flail").WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
  }

  private async Task RamMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.RamDamage).FromMonster((MonsterModel) this).WithAttackerAnim("RamAttack", 0.5f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/flail_knight/flail_knight_ram").WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("buff");
    AnimState state2 = new AnimState("attack_flail");
    AnimState state3 = new AnimState("attack_ram");
    AnimState state4 = new AnimState("hurt");
    AnimState state5 = new AnimState("die");
    state1.NextState = initialState;
    state2.NextState = initialState;
    state3.NextState = initialState;
    state4.NextState = initialState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Dead", state5);
    animator.AddAnyState("Hit", state4);
    animator.AddAnyState("Cast", state1);
    animator.AddAnyState("FlailAttack", state2);
    animator.AddAnyState("RamAttack", state3);
    return animator;
  }
}
