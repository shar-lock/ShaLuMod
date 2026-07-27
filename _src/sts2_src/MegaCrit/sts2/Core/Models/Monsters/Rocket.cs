// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.Rocket
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.Bestiary;
using MegaCrit.Sts2.Core.Nodes.Vfx.Backgrounds;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class Rocket : MonsterModel
{
  private const string _attackSlamSfx = "event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_right_attack_slam";
  private const string _attackSnapSfx = "event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_right_attack_snap";
  private const string _attackRegrowSfx = "event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_right_regrow";
  private const string _buffSfxLoop = "event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_right_buff";
  private const string _rocketSfx = "event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_rocket";
  private NKaiserCrabBossBackground? _background;

  public override string DeathSfx
  {
    get => "event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_right_die";
  }

  public override bool ShouldFadeAfterDeath => false;

  public override bool ShouldDisappearFromDoom => false;

  public override float DeathAnimLengthOverride => 2.5f;

  private NKaiserCrabBossBackground? Background
  {
    get
    {
      this.AssertMutable();
      if (this._background == null)
      {
        NCombatRoom instance = NCombatRoom.Instance;
        this._background = ((Node) ((instance != null ? (Control) instance.Background : (Control) null) ?? NBestiary.Instance?.Layout))?.GetNode<NKaiserCrabBossBackground>(NodePath.op_Implicit("%KaiserCrab"));
      }
      return this._background;
    }
  }

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 209, 199);
  }

  public override int MaxInitialHp => this.MinInitialHp;

  private int TargetingReticleDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 3);
  }

  private int PrecisionBeamDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 20, 18);
  }

  private int LaserDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 35, 31 /*0x1F*/);
  }

  private int ChargeUpStrengthGain
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 3, 2);
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("TARGETING_RETICLE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.TargetingReticleMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.TargetingReticleDamage)
    });
    MoveState moveState1 = new MoveState("PRECISION_BEAM_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.PrecisionBeamMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.PrecisionBeamDamage)
    });
    MoveState moveState2 = new MoveState("CHARGE_UP_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ChargeUpMove), new AbstractIntent[1]
    {
      (AbstractIntent) new BuffIntent()
    });
    MoveState moveState3 = new MoveState("LASER_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.LaserMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.LaserDamage)
    });
    MoveState moveState4 = new MoveState("RECHARGE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.RechargeMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SleepIntent()
    });
    initialState.FollowUpState = (MonsterState) moveState1;
    moveState1.FollowUpState = (MonsterState) moveState2;
    moveState2.FollowUpState = (MonsterState) moveState3;
    moveState3.FollowUpState = (MonsterState) moveState4;
    moveState4.FollowUpState = (MonsterState) initialState;
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) moveState1);
    states.Add((MonsterState) moveState2);
    states.Add((MonsterState) moveState3);
    states.Add((MonsterState) moveState4);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  public override async Task AfterAddedToRoom()
  {
    await base.AfterAddedToRoom();
    IReadOnlyList<SurroundedPower> surroundedPowerList = await PowerCmd.Apply<SurroundedPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (IEnumerable<Creature>) this.CombatState.GetOpponentsOf(this.Creature), 1M, this.Creature, (CardModel) null);
    BackAttackRightPower attackRightPower = await PowerCmd.Apply<BackAttackRightPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 1M, this.Creature, (CardModel) null);
    CrabRagePower crabRagePower = await PowerCmd.Apply<CrabRagePower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 1M, this.Creature, (CardModel) null);
    ((CanvasItem) this.Background)?.SetVisible(true);
  }

  public override Task AfterCurrentHpChanged(Creature creature, Decimal delta)
  {
    if (creature != this.Creature || delta >= 0M)
      return Task.CompletedTask;
    this.Background?.PlayHurtAnim(NKaiserCrabBossBackground.ArmSide.Right);
    return Task.CompletedTask;
  }

  public override Task BeforeDeath(Creature creature)
  {
    if (creature != this.Creature)
      return Task.CompletedTask;
    NAudioManager.Instance.PlayOneShot(this.DeathSfx);
    this.Background?.PlayArmDeathAnim(NKaiserCrabBossBackground.ArmSide.Right);
    if (CombatManager.Instance.IsOverOrEnding)
    {
      this.Background?.PlayBodyDeathAnim();
      NRunMusicController.Instance?.UpdateMusicParameter("kaiser_crab_progress", 5f);
    }
    else
      NRunMusicController.Instance?.UpdateMusicParameter("kaiser_crab_progress", 1f);
    return Task.CompletedTask;
  }

  public override void BeforeRemovedFromRoom()
  {
    SfxCmd.StopLoop(this.Creature, "event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_right_buff");
  }

  private async Task TargetingReticleMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_right_attack_snap");
    await (this.Background?.PlayAttackAnim(NKaiserCrabBossBackground.ArmSide.Right, "attack", 0.35f) ?? Task.CompletedTask);
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.TargetingReticleDamage).FromMonster((MonsterModel) this).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
  }

  private async Task PrecisionBeamMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_right_attack_slam");
    await (this.Background?.PlayAttackAnim(NKaiserCrabBossBackground.ArmSide.Right, "attack_med", 0.5f) ?? Task.CompletedTask);
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.PrecisionBeamDamage).FromMonster((MonsterModel) this).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_heavy_blunt", tmpSfx: "heavy_attack.mp3").WithHitVfxSpawnedAtBase().Execute((PlayerChoiceContext) null);
  }

  private async Task ChargeUpMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.PlayLoop(this.Creature, "event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_right_buff");
    await (this.Background?.PlayRightSideChargeUpAnim(0.7f) ?? Task.CompletedTask);
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, (Decimal) this.ChargeUpStrengthGain, this.Creature, (CardModel) null);
  }

  private async Task LaserMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.StopLoop(this.Creature, "event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_right_buff");
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_rocket");
    await (this.Background?.PlayRightSideHeavy(0.5f) ?? Task.CompletedTask);
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.LaserDamage).FromMonster((MonsterModel) this).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
  }

  private async Task RechargeMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/kaiser_crab/kaiser_crab_right_regrow");
    await (this.Background?.PlayRightRecharge(0.5f) ?? Task.CompletedTask);
  }
}
