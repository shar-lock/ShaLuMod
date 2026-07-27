// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.Tunneler
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.TestSupport;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class Tunneler : MonsterModel
{
  private const string _dizzyMoveId = "DIZZY_MOVE";
  public const string biteMoveId = "BITE_MOVE";
  private const string _burrowedAttackTrigger = "BurrowAttack";
  private const string _burrowTrigger = "Burrow";
  private const string _stunTrigger = "Stun";
  private const string _wakeUpTrigger = "WakeUp";
  private const string _burrowSfx = "event:/sfx/enemy/enemy_attacks/burrowing_bug/burrowing_bug_burrow";
  private const string _hiddenBurrowAttackSfx = "event:/sfx/enemy/enemy_attacks/burrowing_bug/burrowing_bug_hidden_attack";
  private bool _isStunned;

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 92, 87);
  }

  public override int MaxInitialHp => this.MinInitialHp;

  protected override string AttackSfx
  {
    get => "event:/sfx/enemy/enemy_attacks/burrowing_bug/burrowing_bug_attack";
  }

  public override string HurtSfx
  {
    get => "event:/sfx/enemy/enemy_attacks/burrowing_bug/burrowing_bug_hurt";
  }

  public override string DeathSfx
  {
    get => "event:/sfx/enemy/enemy_attacks/burrowing_bug/burrowing_bug_die";
  }

  private int BiteDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 15, 13);
  }

  private int BlockGain
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 37, 32 /*0x20*/);
  }

  private int BelowDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 26, 23);
  }

  public bool IsStunned
  {
    get => this._isStunned;
    set
    {
      this.AssertMutable();
      this._isStunned = value;
    }
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("BITE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BiteMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.BiteDamage)
    });
    MoveState moveState1 = new MoveState("BURROW_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BurrowMove), new AbstractIntent[2]
    {
      (AbstractIntent) new BuffIntent(),
      (AbstractIntent) new DefendIntent()
    });
    MoveState moveState2 = new MoveState("BELOW_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BelowMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.BelowDamage)
    });
    MoveState moveState3 = new MoveState("DIZZY_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.StillDizzyMove), new AbstractIntent[1]
    {
      (AbstractIntent) new StunIntent()
    });
    initialState.FollowUpState = (MonsterState) moveState1;
    moveState1.FollowUpState = (MonsterState) moveState2;
    moveState2.FollowUpState = (MonsterState) moveState2;
    moveState3.FollowUpState = (MonsterState) initialState;
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) moveState1);
    states.Add((MonsterState) moveState2);
    states.Add((MonsterState) moveState3);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task BiteMove(IReadOnlyList<Creature> targets)
  {
    this.IsStunned = false;
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.BiteDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.25f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
  }

  private async Task BurrowMove(IReadOnlyList<Creature> targets)
  {
    this.IsStunned = false;
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/burrowing_bug/burrowing_bug_burrow");
    await CreatureCmd.TriggerAnim(this.Creature, "Burrow", 0.25f);
    BurrowedPower burrowedPower = await PowerCmd.Apply<BurrowedPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 1M, this.Creature, (CardModel) null);
    Decimal num = await CreatureCmd.GainBlock(this.Creature, (Decimal) this.BlockGain, ValueProp.Move, (CardPlay) null);
  }

  private async Task BelowMove(IReadOnlyList<Creature> targets)
  {
    this.IsStunned = false;
    if (TestMode.IsOff)
    {
      NCreature creatureNode1 = this.Creature.GetCreatureNode();
      Node2D specialNode = creatureNode1?.GetSpecialNode<Node2D>("Visuals/SpineBoneNode");
      if (specialNode != null && creatureNode1 != null)
      {
        if (targets.Count > 0)
        {
          NCreature creatureNode2 = targets[0].GetCreatureNode();
          if (creatureNode2 != null)
          {
            float num = 400f * creatureNode1.Visuals.Scale.X;
            specialNode.GlobalPosition = new Vector2(creatureNode2.GlobalPosition.X + num, specialNode.GlobalPosition.Y);
          }
        }
        else
          specialNode.Position = Vector2.op_Multiply(Vector2.Left, 400f);
      }
      SfxCmd.Play("event:/sfx/enemy/enemy_attacks/burrowing_bug/burrowing_bug_hidden_attack");
      await CreatureCmd.TriggerAnim(this.Creature, "BurrowAttack", 0.25f);
      await Cmd.Wait(1f);
    }
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.BelowDamage).FromMonster((MonsterModel) this).WithNoAttackerAnim().WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
  }

  public async Task GetStunned()
  {
    this.IsStunned = true;
    await CreatureCmd.TriggerAnim(this.Creature, "Stun", 0.25f);
  }

  public async Task StillDizzyMove(IReadOnlyList<Creature> targets)
  {
    this.IsStunned = false;
    await CreatureCmd.TriggerAnim(this.Creature, "WakeUp", 0.25f);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("die");
    AnimState state2 = new AnimState("hurt");
    AnimState state3 = new AnimState("attack");
    AnimState state4 = new AnimState("stun");
    AnimState animState1 = new AnimState("stunned_loop", true);
    AnimState state5 = new AnimState("stunned_hurt");
    AnimState state6 = new AnimState("wake_up");
    AnimState state7 = new AnimState("burrow");
    AnimState animState2 = new AnimState("hidden_loop", true);
    AnimState state8 = new AnimState("hidden_attack");
    AnimState state9 = new AnimState("hidden_die");
    state7.NextState = animState2;
    state8.NextState = animState2;
    state3.NextState = initialState;
    state2.NextState = initialState;
    state4.NextState = animState1;
    state5.NextState = animState1;
    state6.NextState = initialState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Hit", state2, (Func<bool>) (() => !this.Creature.HasPower<BurrowedPower>() && !this.IsStunned));
    animator.AddAnyState("Hit", state5, (Func<bool>) (() => !this.Creature.HasPower<BurrowedPower>() && this.IsStunned));
    animator.AddAnyState("Dead", state1, (Func<bool>) (() => !this.Creature.HasPower<BurrowedPower>()));
    animator.AddAnyState("Dead", state9, (Func<bool>) (() => this.Creature.HasPower<BurrowedPower>()));
    animator.AddAnyState("Attack", state3);
    animator.AddAnyState("BurrowAttack", state8);
    animator.AddAnyState("Burrow", state7);
    animator.AddAnyState("Stun", state4);
    animator.AddAnyState("WakeUp", state6);
    return animator;
  }

  public override List<BestiaryMonsterMove> GenerateBestiaryMoveList(
    NCreatureVisuals? creatureVisuals)
  {
    List<BestiaryMonsterMove> bestiaryMoveList = base.GenerateBestiaryMoveList(creatureVisuals);
    bestiaryMoveList.Insert(3, BestiaryMonsterMove.FromStun(new Func<Task>(this.GetStunned)));
    return bestiaryMoveList;
  }
}
