// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.GasBomb
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Audio;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
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
using MegaCrit.Sts2.Core.Nodes.Vfx;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class GasBomb : MonsterModel
{
  private const string _explodeTrigger = "ExplodeTrigger";
  private bool _hasExploded;
  private const string _explodeSfx = "event:/sfx/enemy/enemy_attacks/living_fog/living_fog_explode";

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 8, 7);
  }

  public override int MaxInitialHp => this.MinInitialHp;

  private int ExplodeDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 9, 8);
  }

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Magic;

  public override async Task AfterAddedToRoom()
  {
    await base.AfterAddedToRoom();
    MinionPower minionPower = await PowerCmd.Apply<MinionPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), this.Creature, 1M, this.Creature, (CardModel) null);
  }

  private bool HasExploded
  {
    get => this._hasExploded;
    set
    {
      this.AssertMutable();
      this._hasExploded = value;
    }
  }

  public override bool ShouldFadeAfterDeath => false;

  public override string DeathSfx
  {
    get => "event:/sfx/enemy/enemy_attacks/living_fog/living_fog_minion_die";
  }

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("EXPLODE_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.ExplodeMove), new AbstractIntent[1]
    {
      (AbstractIntent) new DeathBlowIntent((Func<Decimal>) (() => (Decimal) this.ExplodeDamage))
    });
    states.Add((MonsterState) initialState);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task ExplodeMove(IReadOnlyList<Creature> targets)
  {
    this.HasExploded = true;
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.ExplodeDamage).FromMonster((MonsterModel) this).WithAttackerAnim("ExplodeTrigger", 0.1f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/living_fog/living_fog_explode").WithHitVfxNode((Func<Creature, Node2D>) (_ => (Node2D) NGaseousImpactVfx.Create(CombatSide.Player, this.CombatState, new Color("#402f45")))).Execute((PlayerChoiceContext) null);
    await CreatureCmd.Kill(this.Creature);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState state1 = new AnimState("idle_loop", true);
    AnimState initialState = new AnimState("spawn");
    AnimState state2 = new AnimState("explode");
    AnimState state3 = new AnimState("attack");
    AnimState state4 = new AnimState("hurt");
    AnimState state5 = new AnimState("die");
    state3.NextState = state1;
    state4.NextState = state1;
    initialState.NextState = state1;
    CreatureAnimator animator = new CreatureAnimator(initialState, controller);
    animator.AddAnyState("Idle", state1);
    animator.AddAnyState("Attack", state3);
    animator.AddAnyState("Dead", state5, (Func<bool>) (() => !this.HasExploded));
    animator.AddAnyState("Hit", state4);
    animator.AddAnyState("ExplodeTrigger", state2);
    return animator;
  }
}
