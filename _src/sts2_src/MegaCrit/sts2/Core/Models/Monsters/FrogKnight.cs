// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.FrogKnight
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
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class FrogKnight : MonsterModel
{
  private const string _buffTrigger = "Buff";
  private const string _lashTrigger = "Lash";
  private const string _chargeTrigger = "charge";
  private const string _buffSfx = "event:/sfx/enemy/enemy_attacks/frog_knight/frog_knight_buff";
  private const string _chargeSfx = "event:/sfx/enemy/enemy_attacks/frog_knight/frog_knight_charge";
  private const string _tongueLashSfx = "event:/sfx/enemy/enemy_attacks/frog_knight/frog_knight_tongue_lash";
  private bool _hasBeetleCharged;

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 199, 191);
  }

  public override int MaxInitialHp => this.MinInitialHp;

  private int StrikeDownEvilDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 23, 21);
  }

  private int TongueLashDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 14, 13);
  }

  private int BeetleChargeDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 40, 35);
  }

  private int PlatingAmount
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 19, 15);
  }

  private bool HasBeetleCharged
  {
    get => this._hasBeetleCharged;
    set
    {
      this.AssertMutable();
      this._hasBeetleCharged = value;
    }
  }

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Armor;

  public override async Task AfterAddedToRoom()
  {
    await base.AfterAddedToRoom();
    PlatingPower platingPower = await PowerCmd.Apply<PlatingPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, (Decimal) this.PlatingAmount, this.Creature, (CardModel) null);
    this.HasBeetleCharged = false;
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState moveState1 = new MoveState("FOR_THE_QUEEN", new Func<IReadOnlyList<Creature>, Task>(this.ForTheQueenMove), new AbstractIntent[1]
    {
      (AbstractIntent) new BuffIntent()
    });
    MoveState moveState2 = new MoveState("STRIKE_DOWN_EVIL", new Func<IReadOnlyList<Creature>, Task>(this.StrikeDownEvilMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.StrikeDownEvilDamage)
    });
    MoveState moveState3 = new MoveState("TONGUE_LASH", new Func<IReadOnlyList<Creature>, Task>(this.TongueLashMove), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent(this.TongueLashDamage),
      (AbstractIntent) new DebuffIntent()
    });
    MoveState move = new MoveState("BEETLE_CHARGE", new Func<IReadOnlyList<Creature>, Task>(this.BeetleChargeMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.BeetleChargeDamage)
    });
    ConditionalBranchState conditionalBranchState = new ConditionalBranchState("HALF_HEALTH");
    conditionalBranchState.AddState((MonsterState) moveState3, (Func<bool>) (() => this.HasBeetleCharged || this.Creature.CurrentHp >= this.Creature.MaxHp / 2));
    conditionalBranchState.AddState((MonsterState) move, (Func<bool>) (() => !this.HasBeetleCharged && this.Creature.CurrentHp < this.Creature.MaxHp / 2));
    moveState1.FollowUpState = (MonsterState) conditionalBranchState;
    moveState2.FollowUpState = (MonsterState) moveState1;
    moveState3.FollowUpState = (MonsterState) moveState2;
    move.FollowUpState = (MonsterState) moveState3;
    states.Add((MonsterState) conditionalBranchState);
    states.Add((MonsterState) moveState1);
    states.Add((MonsterState) moveState2);
    states.Add((MonsterState) moveState3);
    states.Add((MonsterState) move);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) moveState3);
  }

  private async Task ForTheQueenMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/frog_knight/frog_knight_buff");
    await CreatureCmd.TriggerAnim(this.Creature, "Buff", 0.4f);
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 5M, this.Creature, (CardModel) null);
  }

  private async Task StrikeDownEvilMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.StrikeDownEvilDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.5f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
  }

  private async Task TongueLashMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.TongueLashDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Lash", 0.3f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/frog_knight/frog_knight_tongue_lash").WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
    IReadOnlyList<FrailPower> frailPowerList = await PowerCmd.Apply<FrailPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (IEnumerable<Creature>) targets, 2M, this.Creature, (CardModel) null);
  }

  private async Task BeetleChargeMove(IReadOnlyList<Creature> targets)
  {
    this.HasBeetleCharged = true;
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.BeetleChargeDamage).FromMonster((MonsterModel) this).WithAttackerAnim("charge", 0.6f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/frog_knight/frog_knight_charge").WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
    await Cmd.Wait(1f);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("buff");
    AnimState state2 = new AnimState("attack");
    AnimState state3 = new AnimState("attack_tongue");
    AnimState state4 = new AnimState("charge");
    AnimState state5 = new AnimState("hurt");
    AnimState state6 = new AnimState("die");
    state1.NextState = initialState;
    state2.NextState = initialState;
    state3.NextState = initialState;
    state4.NextState = initialState;
    state5.NextState = initialState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Buff", state1);
    animator.AddAnyState("Attack", state2);
    animator.AddAnyState("Lash", state3);
    animator.AddAnyState("charge", state4);
    animator.AddAnyState("Dead", state6);
    initialState.AddBranch("Hit", state5);
    state1.AddBranch("Hit", state5);
    state2.AddBranch("Hit", state5);
    state3.AddBranch("Hit", state5);
    state5.AddBranch("Hit", state5);
    return animator;
  }
}
