// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.BowlbugSilk
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
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class BowlbugSilk : MonsterModel
{
  private const int _thrashRepeat = 2;
  private const string _spitSfx = "event:/sfx/enemy/enemy_attacks/workbug_silk/workbug_silk_spit";
  private const string _spineSkin = "web";

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 41, 40);
  }

  public override int MaxInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 44, 43);
  }

  private int ThrashDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 5, 4);
  }

  public override string DeathSfx => "event:/sfx/enemy/enemy_attacks/workbug_silk/workbug_silk_die";

  public override void SetupSkins(MegaSprite spine, MegaSkeleton skeleton)
  {
    skeleton.SetSkin(skeleton.GetData().FindSkin("web"));
    skeleton.SetSlotsToSetupPose();
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState moveState = new MoveState("THRASH_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ThrashMove), new AbstractIntent[1]
    {
      (AbstractIntent) new MultiAttackIntent(this.ThrashDamage, 2)
    });
    MoveState initialState = new MoveState("TOXIC_SPIT_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SpitMove), new AbstractIntent[1]
    {
      (AbstractIntent) new DebuffIntent()
    });
    moveState.FollowUpState = (MonsterState) initialState;
    initialState.FollowUpState = (MonsterState) moveState;
    states.Add((MonsterState) moveState);
    states.Add((MonsterState) initialState);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task ThrashMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.ThrashDamage).WithHitCount(2).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.3f).OnlyPlayAnimOnce().WithAttackerFx(sfx: this.AttackSfx).WithHitFx("vfx/vfx_attack_blunt").Execute((PlayerChoiceContext) null);
  }

  private async Task SpitMove(IReadOnlyList<Creature> targets)
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
        float num = 0.0f * creatureNode1.Visuals.Scale.X;
        specialNode.GlobalPosition = new Vector2(ncreature.GlobalPosition.X + num, specialNode.GlobalPosition.Y);
      }
    }
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/workbug_silk/workbug_silk_spit");
    await CreatureCmd.TriggerAnim(this.Creature, "Cast", 0.8f);
    IReadOnlyList<WeakPower> weakPowerList = await PowerCmd.Apply<WeakPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (IEnumerable<Creature>) targets, 1M, this.Creature, (CardModel) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("spit");
    AnimState state2 = new AnimState("attack");
    AnimState state3 = new AnimState("hurt");
    AnimState state4 = new AnimState("die");
    state1.NextState = initialState;
    state2.NextState = initialState;
    state3.NextState = initialState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Dead", state4);
    animator.AddAnyState("Hit", state3);
    animator.AddAnyState("Cast", state1);
    animator.AddAnyState("Attack", state2);
    return animator;
  }
}
