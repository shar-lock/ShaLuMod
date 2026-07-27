// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.BygoneEffigy
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
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class BygoneEffigy : MonsterModel
{
  public override int MinInitialHp
  {
    get
    {
      return AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 132, (int) sbyte.MaxValue);
    }
  }

  public override int MaxInitialHp => this.MinInitialHp;

  private int SlashDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 15, 13);
  }

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Stone;

  public override async Task AfterAddedToRoom()
  {
    await base.AfterAddedToRoom();
    SlowPower slowPower = await PowerCmd.Apply<SlowPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 1M, this.Creature, (CardModel) null);
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("SLEEP_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.InitialSleepMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SleepIntent()
    });
    MoveState moveState1 = new MoveState("WAKE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.WakeMove), new AbstractIntent[1]
    {
      (AbstractIntent) new BuffIntent()
    });
    MoveState moveState2 = new MoveState("SLEEP_MOVE_2", new Func<IReadOnlyList<Creature>, Task>(this.SleepMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SleepIntent()
    });
    MoveState moveState3 = new MoveState("SLASHES_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SlashMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.SlashDamage)
    });
    initialState.FollowUpState = (MonsterState) moveState1;
    moveState1.FollowUpState = (MonsterState) moveState3;
    moveState2.FollowUpState = (MonsterState) moveState3;
    moveState3.FollowUpState = (MonsterState) moveState3;
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) moveState1);
    states.Add((MonsterState) moveState2);
    states.Add((MonsterState) moveState3);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task InitialSleepMove(IReadOnlyList<Creature> targets)
  {
    ThinkCmd.Play(MonsterModel.L10NMonsterLookup("BYGONE_EFFIGY.moves.SLEEP.speakLine1"), this.Creature);
    await Cmd.Wait(0.5f);
  }

  private Task SleepMove(IReadOnlyList<Creature> targets) => Task.CompletedTask;

  private async Task WakeMove(IReadOnlyList<Creature> targets)
  {
    if (TestMode.IsOff)
      NRunMusicController.Instance?.TriggerEliteSecondPhase();
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 10M, this.Creature, (CardModel) null);
    TalkCmd.Play(MonsterModel.L10NMonsterLookup("BYGONE_EFFIGY.moves.SLEEP.speakLine2"), this.Creature, VfxColor.DarkGray, VfxDuration.Long);
    await Cmd.Wait(0.5f);
  }

  private async Task SlashMove(IReadOnlyList<Creature> targets)
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
        float num = 1500f * creatureNode1.Visuals.Scale.X;
        specialNode.GlobalPosition = new Vector2(ncreature.GlobalPosition.X + num, specialNode.GlobalPosition.Y);
      }
    }
    NCombatRoom.Instance?.RadialBlur(VfxPosition.Left);
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.SlashDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.1f).WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_slash").Execute((PlayerChoiceContext) null);
    await Cmd.Wait(0.25f);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("cast");
    AnimState state2 = new AnimState("attack");
    AnimState state3 = new AnimState("hurt");
    AnimState state4 = new AnimState("die");
    state1.NextState = initialState;
    state2.NextState = initialState;
    state3.NextState = initialState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Cast", state1);
    animator.AddAnyState("Attack", state2);
    animator.AddAnyState("Dead", state4);
    initialState.AddBranch("Hit", state3);
    state1.AddBranch("Hit", state3);
    state3.AddBranch("Hit", state3);
    return animator;
  }
}
