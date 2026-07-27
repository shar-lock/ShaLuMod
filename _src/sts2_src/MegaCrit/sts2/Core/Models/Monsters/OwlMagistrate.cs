// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.OwlMagistrate
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

public sealed class OwlMagistrate : MonsterModel
{
  private const int _peckAssaultRepeat = 6;
  private const string _attackPeckAnimId = "attack_peck";
  private const string _takeOffTrigger = "TakeOff";
  private const string _attackPeckSfx = "event:/sfx/enemy/enemy_attacks/owl_magistrate/owl_magistrate_attack_peck";
  private const string _attackDiveSfx = "event:/sfx/enemy/enemy_attacks/owl_magistrate/owl_magistrate_attack_dive";
  private const string _takeOffSfx = "event:/sfx/enemy/enemy_attacks/owl_magistrate/owl_magistrate_take_off";
  private const string _flyLoopSfx = "event:/sfx/enemy/enemy_attacks/owl_magistrate/owl_magistrate_fly_loop";
  private bool _isFlying;

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 247, 231);
  }

  public override int MaxInitialHp => this.MinInitialHp;

  private int VerdictDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 36, 33);
  }

  private int ScrutinyDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 17, 16 /*0x10*/);
  }

  private int PeckAssaultDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 4);
  }

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Armor;

  public override string HurtSfx
  {
    get
    {
      return !this.IsFlying ? "event:/sfx/enemy/enemy_attacks/owl_magistrate/owl_magistrate_hurt" : "event:/sfx/enemy/enemy_attacks/owl_magistrate/owl_magistrate_hurt_flying";
    }
  }

  public override string DeathSfx
  {
    get
    {
      return !this.IsFlying ? "event:/sfx/enemy/enemy_attacks/owl_magistrate/owl_magistrate_die" : "event:/sfx/enemy/enemy_attacks/owl_magistrate/owl_magistrate_die_flying";
    }
  }

  private bool IsFlying
  {
    get => this._isFlying;
    set
    {
      this.AssertMutable();
      this._isFlying = value;
    }
  }

  public override void BeforeRemovedFromRoom()
  {
    if (!this.IsFlying)
      return;
    SfxCmd.StopLoop(this.Creature, "event:/sfx/enemy/enemy_attacks/owl_magistrate/owl_magistrate_fly_loop");
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("MAGISTRATE_SCRUTINY", new Func<IReadOnlyList<Creature>, Task>(this.MagistrateScrutinyMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.ScrutinyDamage)
    });
    MoveState moveState1 = new MoveState("PECK_ASSAULT", new Func<IReadOnlyList<Creature>, Task>(this.PeckAssaultMove), new AbstractIntent[1]
    {
      (AbstractIntent) new MultiAttackIntent(this.PeckAssaultDamage, 6)
    });
    MoveState moveState2 = new MoveState("JUDICIAL_FLIGHT", new Func<IReadOnlyList<Creature>, Task>(this.JudicialFlightMove), new AbstractIntent[1]
    {
      (AbstractIntent) new BuffIntent()
    });
    MoveState moveState3 = new MoveState("VERDICT", new Func<IReadOnlyList<Creature>, Task>(this.VerdictMove), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent(this.VerdictDamage),
      (AbstractIntent) new DebuffIntent()
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

  private async Task MagistrateScrutinyMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.ScrutinyDamage).FromMonster((MonsterModel) this).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/owl_magistrate/owl_magistrate_attack_peck").WithAttackerAnim("Attack", 0.5f).WithHitFx("vfx/vfx_gaze").Execute((PlayerChoiceContext) null);
  }

  private async Task PeckAssaultMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.PeckAssaultDamage).WithHitCount(6).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.5f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/owl_magistrate/owl_magistrate_attack_peck").OnlyPlayAnimOnce().WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
  }

  private async Task JudicialFlightMove(IReadOnlyList<Creature> targets)
  {
    this.IsFlying = true;
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/owl_magistrate/owl_magistrate_take_off");
    await CreatureCmd.TriggerAnim(this.Creature, "TakeOff", 0.0f);
    await Cmd.Wait(1.25f);
    SoarPower soarPower = await PowerCmd.Apply<SoarPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 1M, this.Creature, (CardModel) null);
    SfxCmd.PlayLoop(this.Creature, "event:/sfx/enemy/enemy_attacks/owl_magistrate/owl_magistrate_fly_loop");
  }

  private async Task VerdictMove(IReadOnlyList<Creature> targets)
  {
    this.IsFlying = false;
    SfxCmd.StopLoop(this.Creature, "event:/sfx/enemy/enemy_attacks/owl_magistrate/owl_magistrate_fly_loop");
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.VerdictDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.5f).WithHitFx("vfx/vfx_attack_slash", "event:/sfx/enemy/enemy_attacks/owl_magistrate/owl_magistrate_attack_dive").Execute((PlayerChoiceContext) null);
    IReadOnlyList<VulnerablePower> vulnerablePowerList = await PowerCmd.Apply<VulnerablePower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (IEnumerable<Creature>) targets, 4M, this.Creature, (CardModel) null);
    await PowerCmd.Remove<SoarPower>(this.Creature);
    await Cmd.Wait(1f);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("attack_peck");
    AnimState state2 = new AnimState("hurt");
    AnimState state3 = new AnimState("die");
    AnimState state4 = new AnimState("take_off");
    AnimState animState = new AnimState("fly_loop", true)
    {
      BoundsContainer = "FlyingBounds"
    };
    AnimState state5 = new AnimState("attack_dive")
    {
      BoundsContainer = "IdleBounds"
    };
    AnimState state6 = new AnimState("hurt_flying");
    AnimState state7 = new AnimState("die_flying");
    initialState.AddBranch("Attack", state1);
    initialState.AddBranch("Hit", state2);
    initialState.AddBranch("Dead", state3);
    state1.NextState = initialState;
    state1.AddBranch("Hit", state2);
    state1.AddBranch("Dead", state3);
    state2.NextState = initialState;
    state2.AddBranch("Attack", state1);
    state2.AddBranch("Dead", state3);
    state2.AddBranch("Hit", state2);
    state4.NextState = animState;
    state4.AddBranch("Hit", state6);
    state4.AddBranch("Dead", state7);
    animState.AddBranch("Attack", state5);
    animState.AddBranch("Hit", state6);
    animState.AddBranch("Dead", state7);
    state6.NextState = animState;
    state6.AddBranch("Attack", state5);
    state6.AddBranch("Dead", state7);
    state5.NextState = initialState;
    state5.AddBranch("Attack", state1);
    state5.AddBranch("Dead", state3);
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("TakeOff", state4);
    return animator;
  }
}
