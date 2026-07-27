// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.MechaKnight
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Cards;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class MechaKnight : MonsterModel
{
  private const int _flamethrowerCardCount = 4;
  private const int _windupBlock = 15;
  private const string _windUpTrigger = "windUp";
  private const string _flameAttackTrigger = "flamethrower";
  private const string _chargeTrigger = "charge";
  private bool _isWoundUp;
  private const string _buffSfx = "event:/sfx/enemy/enemy_attacks/mechaknight/mechaknight_buff";
  private const string _dashSfx = "event:/sfx/enemy/enemy_attacks/mechaknight/mechaknight_dash";
  private const string _flamethrowerSfx = "event:/sfx/enemy/enemy_attacks/mechaknight/mechaknight_flamethrower";
  private const string _heavyAttackSfx = "event:/sfx/enemy/enemy_attacks/mechaknight/mechaknight_heavy_attack";

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 320, 300);
  }

  public override int MaxInitialHp => this.MinInitialHp;

  private static int ChargeDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 30, 25);
  }

  private static int HeavyCleaveDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 40, 35);
  }

  public override string DeathSfx => "event:/sfx/enemy/enemy_attacks/mechaknight/mechaknight_die";

  public override string HurtSfx => "event:/sfx/enemy/enemy_attacks/mechaknight/mechaknight_hurt";

  private bool IsWoundUp
  {
    get => this._isWoundUp;
    set
    {
      this.AssertMutable();
      this._isWoundUp = value;
    }
  }

  public override async Task AfterAddedToRoom()
  {
    await base.AfterAddedToRoom();
    ArtifactPower artifactPower = await PowerCmd.Apply<ArtifactPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 3M, this.Creature, (CardModel) null);
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("CHARGE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ChargeMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(MechaKnight.ChargeDamage)
    });
    MoveState moveState1 = new MoveState("FLAMETHROWER_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.FlamethrowerMove), new AbstractIntent[1]
    {
      (AbstractIntent) new StatusIntent(4)
    });
    MoveState moveState2 = new MoveState("WINDUP_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.WindupMove), new AbstractIntent[2]
    {
      (AbstractIntent) new DefendIntent(),
      (AbstractIntent) new BuffIntent()
    });
    MoveState moveState3 = new MoveState("HEAVY_CLEAVE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.HeavyCleaveMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(MechaKnight.HeavyCleaveDamage)
    });
    initialState.FollowUpState = (MonsterState) moveState1;
    moveState1.FollowUpState = (MonsterState) moveState2;
    moveState2.FollowUpState = (MonsterState) moveState3;
    moveState3.FollowUpState = (MonsterState) moveState1;
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) moveState3);
    states.Add((MonsterState) moveState2);
    states.Add((MonsterState) moveState1);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task ChargeMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) MechaKnight.ChargeDamage).FromMonster((MonsterModel) this).WithAttackerAnim("charge", 0.25f).WithWaitBeforeHit(0.5f, 1f).WithHitVfxNode((Func<Creature, Node2D>) (_ => (Node2D) NSpikeSplashVfx.Create(this.Creature, VfxColor.Gold))).WithHitFx(sfx: "event:/sfx/enemy/enemy_attacks/mechaknight/mechaknight_dash").WithHitVfxNode((Func<Creature, Node2D>) (_ => (Node2D) NBigSlashImpactVfx.Create(VfxCmd.GetSideCenter(CombatSide.Player, this.CombatState).Value, 180f, new Color("#80dbff")))).Execute((PlayerChoiceContext) null);
  }

  private async Task HeavyCleaveMove(IReadOnlyList<Creature> targets)
  {
    this.IsWoundUp = false;
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) MechaKnight.HeavyCleaveDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.4f).WithHitFx(sfx: "event:/sfx/enemy/enemy_attacks/mechaknight/mechaknight_heavy_attack").AfterAttackerAnim((Func<Task>) (() =>
    {
      NCombatRoom.Instance?.RadialBlur(VfxPosition.Left);
      NGame.Instance?.DoHitStop(ShakeStrength.Strong, ShakeDuration.Normal);
      return Task.CompletedTask;
    })).WithHitVfxNode((Func<Creature, Node2D>) (_ => (Node2D) NBigSlashVfx.Create(VfxCmd.GetSideCenter(CombatSide.Player, this.CombatState).Value, false))).WithHitVfxNode((Func<Creature, Node2D>) (_ => (Node2D) NBigSlashImpactVfx.Create(VfxCmd.GetSideCenter(CombatSide.Player, this.CombatState).Value, 180f, new Color("#80dbff")))).Execute((PlayerChoiceContext) null);
  }

  private async Task WindupMove(IReadOnlyList<Creature> targets)
  {
    this.IsWoundUp = true;
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/mechaknight/mechaknight_buff");
    await CreatureCmd.TriggerAnim(this.Creature, "windUp", 0.5f);
    Decimal num = await CreatureCmd.GainBlock(this.Creature, 15M, ValueProp.Move, (CardPlay) null);
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 5M, this.Creature, (CardModel) null);
  }

  private async Task FlamethrowerMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/mechaknight/mechaknight_flamethrower");
    await CreatureCmd.TriggerAnim(this.Creature, "flamethrower", 1.5f);
    await CardPileCmd.AddToCombatAndPreview<Burn>((IEnumerable<Creature>) targets, PileType.Hand, 4, (Player) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("hurt");
    AnimState state2 = new AnimState("die");
    AnimState state3 = new AnimState("attack_flame");
    AnimState state4 = new AnimState("attack_cleave");
    AnimState state5 = new AnimState("charge");
    AnimState state6 = new AnimState("wind_up");
    AnimState animState = new AnimState("idle_loop_wound", true);
    AnimState state7 = new AnimState("hurt_wound");
    state4.NextState = initialState;
    state5.NextState = initialState;
    state3.NextState = initialState;
    state1.NextState = initialState;
    state6.NextState = animState;
    state7.NextState = animState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Dead", state2);
    animator.AddAnyState("Attack", state4);
    animator.AddAnyState("flamethrower", state3);
    animator.AddAnyState("charge", state5);
    animator.AddAnyState("windUp", state6);
    initialState.AddBranch("Hit", state1, (Func<bool>) (() => !this.IsWoundUp));
    state3.AddBranch("Hit", state1, (Func<bool>) (() => !this.IsWoundUp));
    state1.AddBranch("Hit", state1, (Func<bool>) (() => !this.IsWoundUp));
    state4.AddBranch("Hit", state1, (Func<bool>) (() => !this.IsWoundUp));
    state6.AddBranch("Hit", state1, (Func<bool>) (() => !this.IsWoundUp));
    animState.AddBranch("Hit", state1, (Func<bool>) (() => !this.IsWoundUp));
    state7.AddBranch("Hit", state1, (Func<bool>) (() => !this.IsWoundUp));
    initialState.AddBranch("Hit", state7, (Func<bool>) (() => this.IsWoundUp));
    state3.AddBranch("Hit", state7, (Func<bool>) (() => this.IsWoundUp));
    state1.AddBranch("Hit", state7, (Func<bool>) (() => this.IsWoundUp));
    state4.AddBranch("Hit", state7, (Func<bool>) (() => this.IsWoundUp));
    state6.AddBranch("Hit", state7, (Func<bool>) (() => this.IsWoundUp));
    animState.AddBranch("Hit", state7, (Func<bool>) (() => this.IsWoundUp));
    state7.AddBranch("Hit", state7, (Func<bool>) (() => this.IsWoundUp));
    return animator;
  }
}
