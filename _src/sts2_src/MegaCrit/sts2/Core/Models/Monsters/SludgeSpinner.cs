// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.SludgeSpinner
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

public sealed class SludgeSpinner : MonsterModel
{
  private const string _rageMove = "RAGE_MOVE";
  private const string _dashAttackSfx = "event:/sfx/enemy/enemy_attacks/sludge_spinner/sludge_spinner_attack_dash";
  private const string _spinAttackSfx = "event:/sfx/enemy/enemy_attacks/sludge_spinner/sludge_spinner_attack_spin";

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 41, 37);
  }

  public override int MaxInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 42, 39);
  }

  private int OilSprayDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 9, 8);
  }

  private int SlamDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 12, 11);
  }

  private int RageDamage => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 7, 6);

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Stone;

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState moveState = new MoveState("OIL_SPRAY_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.OilSprayMove), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent(this.OilSprayDamage),
      (AbstractIntent) new DebuffIntent()
    });
    MoveState state1 = new MoveState("SLAM_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SlamMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.SlamDamage)
    });
    MoveState state2 = new MoveState("RAGE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.RageMove), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent(this.RageDamage),
      (AbstractIntent) new BuffIntent()
    });
    RandomBranchState randomBranchState = new RandomBranchState("RAND");
    moveState.FollowUpState = (MonsterState) randomBranchState;
    state1.FollowUpState = (MonsterState) randomBranchState;
    state2.FollowUpState = (MonsterState) randomBranchState;
    randomBranchState.AddBranch((MonsterState) moveState, MoveRepeatType.CannotRepeat);
    randomBranchState.AddBranch((MonsterState) state1, MoveRepeatType.CannotRepeat);
    randomBranchState.AddBranch((MonsterState) state2, MoveRepeatType.CannotRepeat);
    states.Add((MonsterState) randomBranchState);
    states.Add((MonsterState) moveState);
    states.Add((MonsterState) state1);
    states.Add((MonsterState) state2);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) moveState);
  }

  private async Task OilSprayMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.OilSprayDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Cast", 0.5f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/sludge_spinner/sludge_spinner_attack_spin").WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
    IReadOnlyList<WeakPower> weakPowerList = await PowerCmd.Apply<WeakPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (IEnumerable<Creature>) targets, 1M, this.Creature, (CardModel) null);
  }

  private async Task SlamMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.SlamDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.15f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/sludge_spinner/sludge_spinner_attack_dash").WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
  }

  private async Task RageMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.RageDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.5f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/sludge_spinner/sludge_spinner_attack_dash").WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 3M, this.Creature, (CardModel) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("slam");
    AnimState state2 = new AnimState("spray");
    AnimState state3 = new AnimState("hurt");
    AnimState state4 = new AnimState("die");
    state1.NextState = initialState;
    state2.NextState = initialState;
    state3.NextState = initialState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Attack", state1);
    animator.AddAnyState("Cast", state2);
    animator.AddAnyState("Dead", state4);
    animator.AddAnyState("Hit", state3);
    return animator;
  }

  protected override bool ShouldShowMoveInBestiary(string moveStateId)
  {
    return moveStateId != "RAGE_MOVE";
  }
}
