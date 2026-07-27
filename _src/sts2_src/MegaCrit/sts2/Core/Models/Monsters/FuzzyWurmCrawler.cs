// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.FuzzyWurmCrawler
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
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class FuzzyWurmCrawler : MonsterModel
{
  private const string _firstAcidGoopMove = "FIRST_ACID_GOOP";
  private const string _inhaleTrigger = "Inhale";
  private bool _isPuffed;

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 58, 55);
  }

  public override int MaxInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 59, 57);
  }

  private int AcidGoopDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 6, 4);
  }

  private bool IsPuffed
  {
    get => this._isPuffed;
    set
    {
      this.AssertMutable();
      this._isPuffed = value;
    }
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("FIRST_ACID_GOOP", new Func<IReadOnlyList<Creature>, Task>(this.AcidGoop), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.AcidGoopDamage)
    });
    MoveState moveState1 = new MoveState("ACID_GOOP", new Func<IReadOnlyList<Creature>, Task>(this.AcidGoop), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.AcidGoopDamage)
    });
    MoveState moveState2 = new MoveState("INHALE", new Func<IReadOnlyList<Creature>, Task>(this.Inhale), new AbstractIntent[1]
    {
      (AbstractIntent) new BuffIntent()
    });
    initialState.FollowUpState = (MonsterState) moveState2;
    moveState2.FollowUpState = (MonsterState) moveState1;
    moveState1.FollowUpState = (MonsterState) initialState;
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) moveState1);
    states.Add((MonsterState) moveState2);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task AcidGoop(IReadOnlyList<Creature> targets)
  {
    if (TestMode.IsOff && targets.Any<Creature>())
    {
      NCreature creatureNode = this.Creature.GetCreatureNode();
      if (creatureNode != null)
      {
        Node2D specialNode = creatureNode.GetSpecialNode<Node2D>("Visuals/SpineBoneNode");
        if (specialNode != null)
          specialNode.Position = Vector2.op_Multiply(Vector2.Left, creatureNode.GlobalPosition.X - NCombatRoom.Instance.GetCreatureNode(targets.First<Creature>()).GlobalPosition.X);
      }
    }
    this.IsPuffed = false;
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.AcidGoopDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 1f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
  }

  private async Task Inhale(IReadOnlyList<Creature> targets)
  {
    this.IsPuffed = true;
    SfxCmd.Play(this.CastSfx);
    await CreatureCmd.TriggerAnim(this.Creature, nameof (Inhale), 0.6f);
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 7M, this.Creature, (CardModel) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("attack");
    AnimState state2 = new AnimState("hurt");
    AnimState state3 = new AnimState("die");
    AnimState state4 = new AnimState("inhale");
    AnimState animState = new AnimState("idle_loop_puffed", true);
    AnimState state5 = new AnimState("hurt_puffed");
    AnimState state6 = new AnimState("die_puffed");
    state1.NextState = initialState;
    state2.NextState = initialState;
    state4.NextState = animState;
    state5.NextState = animState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Inhale", state4);
    animator.AddAnyState("Attack", state1);
    animator.AddAnyState("Dead", state3, (Func<bool>) (() => !this.IsPuffed));
    animator.AddAnyState("Dead", state6, (Func<bool>) (() => this.IsPuffed));
    animator.AddAnyState("Hit", state2, (Func<bool>) (() => !this.IsPuffed));
    animator.AddAnyState("Hit", state5, (Func<bool>) (() => this.IsPuffed));
    return animator;
  }

  protected override bool ShouldShowMoveInBestiary(string moveStateId)
  {
    return moveStateId != "FIRST_ACID_GOOP";
  }
}
