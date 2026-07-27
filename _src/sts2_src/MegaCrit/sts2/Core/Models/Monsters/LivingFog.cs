// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.LivingFog
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

public sealed class LivingFog : MonsterModel
{
  private int _bloatAmount = 1;
  private const string _spawnBombTrigger = "SpawnBomb";
  private const string _attackBlowSfx = "event:/sfx/enemy/enemy_attacks/living_fog/living_fog_attack_blow";
  private const string _summonSfx = "event:/sfx/enemy/enemy_attacks/living_fog/living_fog_summon";
  private const string _appearsSfx = "event:/sfx/enemy/enemy_attacks/living_fog/living_fog_minion_appear";

  public override int MinInitialHp
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.ToughEnemies, 82, 80 /*0x50*/);
  }

  public override int MaxInitialHp => this.MinInitialHp;

  private int AdvancedGasDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 9, 8);
  }

  private int BloatDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 6, 5);
  }

  private int SuperGasBlastDamage
  {
    get => AscensionHelper.GetValueIfAscension(AscensionLevel.DeadlyEnemies, 9, 8);
  }

  public override DamageSfxType TakeDamageSfxType => DamageSfxType.Magic;

  private int BloatAmount
  {
    get => this._bloatAmount;
    set
    {
      this.AssertMutable();
      this._bloatAmount = value;
    }
  }

  public override bool ShouldFadeAfterDeath => false;

  public override string DeathSfx => "event:/sfx/enemy/enemy_attacks/living_fog/living_fog_die";

  protected override MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine GenerateMoveStateMachine()
  {
    List<MonsterState> states = new List<MonsterState>();
    MoveState initialState = new MoveState("ADVANCED_GAS_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.AdvancedGasMove), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent(this.AdvancedGasDamage),
      (AbstractIntent) new CardDebuffIntent()
    });
    MoveState moveState1 = new MoveState("BLOAT_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.BloatMove), new AbstractIntent[2]
    {
      (AbstractIntent) new SingleAttackIntent(this.BloatDamage),
      (AbstractIntent) new SummonIntent()
    });
    MoveState moveState2 = new MoveState("SUPER_GAS_BLAST_MOVE", new Func<IReadOnlyList<Creature>, Task>(this.SuperGasBlastMove), new AbstractIntent[1]
    {
      (AbstractIntent) new SingleAttackIntent(this.SuperGasBlastDamage)
    });
    initialState.FollowUpState = (MonsterState) moveState1;
    moveState1.FollowUpState = (MonsterState) moveState2;
    moveState2.FollowUpState = (MonsterState) moveState1;
    states.Add((MonsterState) initialState);
    states.Add((MonsterState) moveState2);
    states.Add((MonsterState) moveState1);
    return new MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine.MonsterMoveStateMachine((IEnumerable<MonsterState>) states, (MonsterState) initialState);
  }

  private async Task AdvancedGasMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.AdvancedGasDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Cast", 1.25f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/living_fog/living_fog_attack_blow").WithHitVfxNode((Func<Creature, Node2D>) (_ => (Node2D) NGaseousImpactVfx.Create(CombatSide.Player, this.CombatState, new Color("#402f45")))).Execute((PlayerChoiceContext) null);
    IReadOnlyList<SmoggyPower> smoggyPowerList = await PowerCmd.Apply<SmoggyPower>((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), (IEnumerable<Creature>) targets, 1M, this.Creature, (CardModel) null);
  }

  private async Task BloatMove(IReadOnlyList<Creature> targets)
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_attacks/living_fog/living_fog_summon");
    await CreatureCmd.TriggerAnim(this.Creature, "SpawnBomb", 0.35f);
    for (int i = 0; i < this.BloatAmount; ++i)
    {
      string nextSlot = this.CombatState.Encounter?.GetNextSlot(this.CombatState);
      if (!string.IsNullOrEmpty(nextSlot))
      {
        SfxCmd.Play("event:/sfx/enemy/enemy_attacks/living_fog/living_fog_minion_appear");
        Creature creature = await CreatureCmd.Add<GasBomb>(this.CombatState, nextSlot);
      }
    }
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.BloatDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.1f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/living_fog/living_fog_attack_blow").WithHitVfxNode((Func<Creature, Node2D>) (_ => (Node2D) NGaseousImpactVfx.Create(CombatSide.Player, this.CombatState, new Color("#402f45")))).Execute((PlayerChoiceContext) null);
  }

  private async Task SuperGasBlastMove(IReadOnlyList<Creature> targets)
  {
    AttackCommand attackCommand = await DamageCmd.Attack((Decimal) this.SuperGasBlastDamage).FromMonster((MonsterModel) this).WithAttackerAnim("Attack", 0.1f).WithAttackerFx(sfx: "event:/sfx/enemy/enemy_attacks/living_fog/living_fog_attack_blow").WithHitVfxNode((Func<Creature, Node2D>) (_ => (Node2D) NGaseousImpactVfx.Create(CombatSide.Player, this.CombatState, new Color("#402f45")))).Execute((PlayerChoiceContext) null);
  }

  public override CreatureAnimator GenerateAnimator(MegaSprite controller)
  {
    AnimState animState = new AnimState("idle_loop", true);
    AnimState state1 = new AnimState("debuff");
    AnimState state2 = new AnimState("spawn_bomb");
    AnimState state3 = new AnimState("attack");
    AnimState state4 = new AnimState("hurt");
    AnimState state5 = new AnimState("die");
    state1.NextState = animState;
    state3.NextState = animState;
    state4.NextState = animState;
    state2.NextState = animState;
    CreatureAnimator animator = new CreatureAnimator(animState, controller);
    animator.AddAnyState("Idle", animState);
    animator.AddAnyState("Cast", state1);
    animator.AddAnyState("Attack", state3);
    animator.AddAnyState("Dead", state5);
    animator.AddAnyState("Hit", state4);
    animator.AddAnyState("SpawnBomb", state2);
    return animator;
  }
}
