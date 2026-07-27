// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.SlumberingBeetle
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Animation;
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
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class SlumberingBeetle : MonsterModel
{
  public const string wakeUpTrigger = "WakeUp";
  private const string _rolloutTrigger = "Rollout";
  public const string rolloutMoveId = "ROLL_OUT_MOVE";
  private const string _rollSfx = "event:/sfx/enemy/enemy_attacks/slumbering_beetle/slumbering_beetle_roll";
  public const string wakeUp = "event:/sfx/enemy/enemy_attacks/slumbering_beetle/slumbering_beetle_wake_up";
  public const string sleepLoop = "event:/sfx/enemy/enemy_attacks/slumbering_beetle/slumbering_beetle_sleep_loop";
  private bool _isAwake;
  private NSleepingVfx? _sleepingVfx;

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 89, 86);
  }

  public override int MaxInitialHp => this.MinInitialHp;

  private int RolloutDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 18, 16 /*0x10*/);
  }

  private int PlatingAmount
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 18, 15);
  }

  public bool IsAwake
  {
    get => this._isAwake;
    set
    {
      this.AssertMutable();
      this._isAwake = value;
    }
  }

  private NSleepingVfx? SleepingVfx
  {
    get => this._sleepingVfx;
    set
    {
      this.AssertMutable();
      this._sleepingVfx = value;
    }
  }

  public override async Task AfterAddedToRoom()
  {
    await base.AfterAddedToRoom();
    PlatingPower platingPower = await PowerCmd.Apply<PlatingPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, (Decimal) this.PlatingAmount, this.Creature, (CardModel) null);
    SlumberPower slumberPower = await PowerCmd.Apply<SlumberPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 3M, this.Creature, (CardModel) null);
    SfxCmd.PlayLoop(this.Creature, "event:/sfx/enemy/enemy_attacks/slumbering_beetle/slumbering_beetle_sleep_loop");
    Marker2D specialNode = this.Creature.GetCreatureNode()?.GetSpecialNode<Marker2D>("%SleepVfxPos");
    if (specialNode != null)
    {
      this.SleepingVfx = NSleepingVfx.Create(((Node2D) specialNode).GlobalPosition);
      ((Node) specialNode).AddChildSafely((Node) this.SleepingVfx);
      this.SleepingVfx.Position = Vector2.Zero;
    }
    this.Creature.Died += new Action<Creature>(this.AfterDeath);
  }

  private void AfterDeath(Creature _)
  {
    this.Creature.Died -= new Action<Creature>(this.AfterDeath);
    this.SleepingVfx?.Stop();
    this.SleepingVfx = (NSleepingVfx) null;
  }

  public async Task WakeUpMove(IReadOnlyList<Creature> _)
  {
    SfxCmd.StopLoop(this.Creature, "event:/sfx/enemy/enemy_attacks/slumbering_beetle/slumbering_beetle_sleep_loop");
    this.SleepingVfx?.Stop();
    this.SleepingVfx = (NSleepingVfx) null;
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/slumbering_beetle/slumbering_beetle_wake_up");
    this.IsAwake = true;
    await CreatureCmd.TriggerAnim(this.Creature, "WakeUp", 0.6f);
    if (!this.Creature.HasPower<PlatingPower>())
      return;
    await PowerCmd.Remove((PowerModel) this.Creature.GetPower<PlatingPower>());
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState moveState = new MoveState("SNORE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SnoreMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SleepIntent()
    });
    MoveState move = new MoveState("ROLL_OUT_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.RolloutMove), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent(this.RolloutDamage),
      (AbstractIntent) new BuffIntent()
    });
    ConditionalBranchState conditionalBranchState = new ConditionalBranchState("SNORE_NEXT");
    moveState.FollowUpState = (MonsterState) conditionalBranchState;
    conditionalBranchState.AddState((MonsterState) moveState, (Func<bool>) (() => this.Creature.HasPower<SlumberPower>()));
    conditionalBranchState.AddState((MonsterState) move, (Func<bool>) (() => !this.Creature.HasPower<SlumberPower>()));
    move.FollowUpState = (MonsterState) move;
    states.Add((MonsterState) moveState);
    states.Add((MonsterState) conditionalBranchState);
    states.Add((MonsterState) move);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) moveState);
  }

  private Task SnoreMove(IReadOnlyList<Creature> targets) => Task.CompletedTask;

  private async Task RolloutMove(IReadOnlyList<Creature> targets)
  {
    if (TestMode.IsOff)
    {
      NCreature ncreature = (NCreature) null;
      foreach (Creature target in (IEnumerable<Creature>) targets)
      {
        NCreature creatureNode = target.GetCreatureNode();
        if (creatureNode != null && (ncreature == null || (double) ncreature.GlobalPosition.X > (double) creatureNode.GlobalPosition.X))
          ncreature = creatureNode;
      }
      NCreature creatureNode1 = this.Creature.GetCreatureNode();
      Node2D specialNode = creatureNode1?.GetSpecialNode<Node2D>("Visuals/SpineBoneNode");
      if (creatureNode1 != null && specialNode != null && ncreature != null)
      {
        float num = 750f * creatureNode1.Visuals.Scale.X;
        specialNode.GlobalPosition = new Vector2(ncreature.GlobalPosition.X + num, specialNode.GlobalPosition.Y);
      }
    }
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.RolloutDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Rollout", 0.5f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/slumbering_beetle/slumbering_beetle_roll").WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 2M, this.Creature, (CardModel) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("sleep_loop", true);
    AnimState state1 = new AnimState("wake_up");
    AnimState animState = new AnimState("idle_loop", true);
    AnimState state2 = new AnimState("cast");
    AnimState state3 = new AnimState("attack");
    AnimState state4 = new AnimState("roll");
    AnimState state5 = new AnimState("hurt");
    AnimState state6 = new AnimState("die");
    state1.NextState = animState;
    state3.NextState = animState;
    state2.NextState = animState;
    state4.NextState = animState;
    state5.NextState = animState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("WakeUp", state1);
    animator.AddAnyState("Dead", state6);
    animator.AddAnyState("Attack", state3);
    animator.AddAnyState("Rollout", state4);
    animator.AddAnyState("Cast", state2);
    animator.AddAnyState("Hit", state5, (Func<bool>) (() => this.IsAwake));
    return animator;
  }
}
