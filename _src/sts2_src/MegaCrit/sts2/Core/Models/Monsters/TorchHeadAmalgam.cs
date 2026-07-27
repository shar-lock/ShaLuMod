// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.TorchHeadAmalgam
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
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
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class TorchHeadAmalgam : MonsterModel
{
  private const int _soulBeamRepeat = 3;
  private const string _debuffTrigger = "DebuffTrigger";
  private const string _beamSfx = "event:/sfx/enemy/enemy_attacks/torch_head_amalgam/torch_head_amalgam_beam";

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 211, 199);
  }

  public override int MaxInitialHp => this.MinInitialHp;

  private int StrongTackleDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 32 /*0x20*/, 26);
  }

  private int TackleDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 22, 18);
  }

  private int WeakTackleDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 16 /*0x10*/, 14);
  }

  private int SoulBeamDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 8, 8);
  }

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Armor;

  public override async Task AfterAddedToRoom()
  {
    await base.AfterAddedToRoom();
    MinionPower minionPower = await PowerCmd.Apply<MinionPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 1M, this.Creature, (CardModel) null);
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("STRONG_TACKLE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.StrongTackleMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.StrongTackleDamage)
    });
    MoveState moveState1 = new MoveState("TACKLE_2_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.TackleMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.TackleDamage)
    });
    MoveState moveState2 = new MoveState("BEAM_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SoulBeamMove), new AbstractIntent[1]
    {
      (AbstractIntent) new MultiAttackIntent(this.SoulBeamDamage, 3)
    });
    MoveState moveState3 = new MoveState("TACKLE_3_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.WeakTackleMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.WeakTackleDamage)
    });
    MoveState moveState4 = new MoveState("TACKLE_4_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.WeakTackleMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.WeakTackleDamage)
    });
    initialState.FollowUpState = (MonsterState) moveState1;
    moveState1.FollowUpState = (MonsterState) moveState2;
    moveState2.FollowUpState = (MonsterState) moveState3;
    moveState3.FollowUpState = (MonsterState) moveState4;
    moveState4.FollowUpState = (MonsterState) moveState2;
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) moveState1);
    states.Add((MonsterState) moveState2);
    states.Add((MonsterState) moveState3);
    states.Add((MonsterState) moveState4);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  public override void OnDieToDoom()
  {
    if (!TestMode.IsOff)
      return;
    NCreature creatureNode = this.Creature.GetCreatureNode();
    if (creatureNode == null)
      return;
    ((CanvasItem) creatureNode.GetSpecialNode<Node2D>("Visuals/torch1Slot/fire1_small_green/light_small"))?.SetVisible(false);
    ((CanvasItem) creatureNode.GetSpecialNode<Node2D>("Visuals/torch2Slot/fire2_small_green/light_small"))?.SetVisible(false);
    ((CanvasItem) creatureNode.GetSpecialNode<Node2D>("Visuals/torch3Slot/fire3_small_green/light_small"))?.SetVisible(false);
  }

  private async Task StrongTackleMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.StrongTackleDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.6f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
  }

  private async Task TackleMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.TackleDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.6f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
  }

  private async Task WeakTackleMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.WeakTackleDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.6f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
  }

  private async Task SoulBeamMove(IReadOnlyList<Creature> targets)
  {
    NCreature creatureNode1 = this.Creature.GetCreatureNode();
    Node2D specialNode = creatureNode1?.GetSpecialNode<Node2D>("Visuals/LaserControlBone");
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
        specialNode.Position = Vector2.op_Multiply(Vector2.Left, 3000f);
    }
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.SoulBeamDamage).WithHitCount(3).FromMonster((MonsterModel) this).WithAttackerAnim("DebuffTrigger", 0.8f).OnlyPlayAnimOnce().WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/torch_head_amalgam/torch_head_amalgam_beam").WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("debuff");
    AnimState state2 = new AnimState("attack");
    AnimState state3 = new AnimState("hurt");
    AnimState state4 = new AnimState("die");
    state1.NextState = initialState;
    state2.NextState = initialState;
    state3.NextState = initialState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("DebuffTrigger", state1);
    animator.AddAnyState("Attack", state2);
    animator.AddAnyState("Dead", state4);
    animator.AddAnyState("Hit", state3);
    return animator;
  }
}
