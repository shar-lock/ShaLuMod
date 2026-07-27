// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.SnappingJaxfruit
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Context;
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

public sealed class SnappingJaxfruit : MonsterModel
{
  private const string _idleLoopSfx = "event:/sfx/enemy/enemy_attacks/orb_plant/orb_plant_idle_loop";

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 34, 31 /*0x1F*/);
  }

  public override int MaxInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 36, 33);
  }

  private int EnergyDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 4, 3);
  }

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Plant;

  public override async Task AfterAddedToRoom()
  {
    await base.AfterAddedToRoom();
    SfxCmd.PlayLoop(this.Creature, "event:/sfx/enemy/enemy_attacks/orb_plant/orb_plant_idle_loop");
  }

  public override void BeforeRemovedFromRoom()
  {
    SfxCmd.StopLoop(this.Creature, "event:/sfx/enemy/enemy_attacks/orb_plant/orb_plant_idle_loop");
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("ENERGY_ORB_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.EnergyOrb), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent(this.EnergyDamage),
      (AbstractIntent) new BuffIntent()
    });
    initialState.FollowUpState = (MonsterState) initialState;
    states.Add((MonsterState) initialState);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  public async Task EnergyOrb(IReadOnlyList<Creature> targets)
  {
    if (TestMode.IsOff)
    {
      NCreature creatureNode = this.Creature.GetCreatureNode();
      if (creatureNode != null)
      {
        Creature creature = LocalContext.GetMe(this.CombatState)?.Creature;
        creatureNode.GetSpecialNode<NSnappingJaxfruitVfx>("Visuals/NSnappingJaxfruitVfx")?.SetTarget(creature);
      }
    }
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.EnergyDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Cast", 0.25f).Execute((PlayerChoiceContext) null);
    StrengthPower strengthPower = await PowerCmd.Apply<StrengthPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 2M, this.Creature, (CardModel) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState initialState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("hurt");
    AnimState state2 = new AnimState("die");
    AnimState state3 = new AnimState("cast");
    state1.NextState = initialState;
    state3.NextState = initialState;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Dead", state2);
    animator.AddAnyState("Cast", state3);
    animator.AddAnyState("Hit", state1);
    return animator;
  }
}
